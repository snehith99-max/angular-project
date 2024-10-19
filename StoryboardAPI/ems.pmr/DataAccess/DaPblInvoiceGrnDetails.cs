using ems.pmr.Models;
using ems.utilities.Functions;
using Microsoft.SqlServer.Server;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;


namespace ems.pmr.DataAccess
{
    public class DaPblInvoiceGrnDetails
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunction = new cmnfunctions();
        finance_cmnfunction objfincmn = new finance_cmnfunction();
        OdbcDataReader odbcdr;
        string mssql = string.Empty;
        DataTable dt_datatable;
        string msGetGID, msINGetGID, mspGetGID, msGetGID1, msGetGID2;
        int mnResult;
        string lsuom_gid, lsproduct_gid, lsproductgroup_name, lsproductgroup_code, lsDiscount_Percentage
             , lsTax_Amount3, lsTax_Percentage, lsTax_Percentage2, lsTax_Percentage3, lsTax_name, lsTax_name2, lsTax_name3
            , lsTax1_gid, lsTax2_gid, lsTax3_gid, lscurrency_taxamount2,
            lscurrency_taxamount3;
        string lsgrn_gid, lsgrndtl_gid, lspurchaseorder_gid, lspurchaseorderdtl_gid
            , lsproduct_code, lsproduct_name, lsproductuom_name, lsproductuom_code,
            lstPO_IV_flag;
        string lsbranchname, ls_referenceno, lsinvoice_status, lstGRN_IV_flag, lsbranch, lsinvoiceref_flag, lsinv_ref_no, msGetGid, lspurchaseorder_from, lsvendoraddress, lsvendor_gid, lsvendor_contact, lspayment_terms, lsmode_despatch;
        string parsed_Date; string invoice_Date;
        int lsproduct_price, lsInvoiceqty_billed 
            , lsqty_invoice;
        decimal lsTax_Amount, lsTax_Amount2, lscurrency_taxamount1, lsexchange_rate, lscurrency_unitprice, lscurrency_discountamount, lsDiscount_Amount;

        decimal delsqty_invoice, lsGRN_Sum;
        double lsproduct_total;

        public void DaGetGrnAmountDetails(string purchaseorder_gid, MdlPblInvoiceGrnDetails values)
        {
            mssql = "select distinct a.payment_days,d.payment_term,date_format(d.payment_date,'%d-%m-%Y') as payment_date , case when a.currency_code is null then 'INR' else a.currency_code end as currency_code,a.branch_gid,b.branch_name, " +
                " a.purchaseorder_gid,date_format(purchaseorder_date,'%d-%m-%y') as purchaseorder_date,case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate,a.buybackorscrap,format(a.addon_amount,2) as addon_amount,format(a.freightcharges,2) as freightcharges,format(a.discount_amount,2) as discount_amount, " +
                " a.total_amount,format(a.packing_charges,2)as packing_charges,format(a.insurance_charges,2)as insurance_charges, " +
                " format(a.roundoff,2)as roundoff, format(a.tax_amount,2) as tax_amount,f.tax_name,a.tax_gid from pmr_trn_tpurchaseorder a " +
                " left join hrm_mst_tbranch b on a.branch_gid = b.branch_gid " +
                " left join pmr_trn_tgrn c on a.purchaseorder_gid = c.purchaseorder_gid " +
                " left join acp_trn_tinvoice d on c.invoice_refno = d.invoice_refno " +
                " left join acp_mst_ttax f on a.tax_gid = f.tax_gid " +
                " where a.purchaseorder_gid='" + purchaseorder_gid + "' group by a.purchaseorder_gid ";
            dt_datatable = objdbconn.GetDataTable(mssql);
            var GetPurchaseOrderGrnDetails = new List<GetInvoicePurchaseOrderGrnDetails_list>();
            if (dt_datatable.Rows.Count > 0)
            {   
                foreach (DataRow dr in dt_datatable.Rows)
                {
                    GetPurchaseOrderGrnDetails.Add(new GetInvoicePurchaseOrderGrnDetails_list
                    {
                        currency_code = dr["currency_code"].ToString(),
                        branch_name = dr["branch_name"].ToString(),
                        purchaseorder_date = dr["purchaseorder_date"].ToString(),
                        purchaseorder_gid = dr["purchaseorder_gid"].ToString(),
                        branch_gid = dr["branch_gid"].ToString(),
                        exchange_rate = dr["exchange_rate"].ToString(),
                        buybackorscrap = dr["buybackorscrap"].ToString(),
                        addon_amount = dr["addon_amount"].ToString(),
                        freightcharges = dr["freightcharges"].ToString(),
                        discount_amount = dr["discount_amount"].ToString(),
                        Grand_total = dr["total_amount"].ToString(),
                        packing_charges = dr["packing_charges"].ToString(),
                        insurance_charges = dr["insurance_charges"].ToString(),
                        roundoff = dr["roundoff"].ToString(),
                        payment_date = dr["payment_date"].ToString(),
                        payment_term = dr["payment_term"].ToString(),
                        payment_days = dr["payment_days"].ToString(),
                        tax_name = dr["tax_name"].ToString(),
                        tax_amount = dr["tax_amount"].ToString(),
                        tax_gid = dr["tax_gid"].ToString(),
                    });
                }
                values.GetInvoicePurchaseOrderGrnDetails_list = GetPurchaseOrderGrnDetails;
            }
        }

        public void DaGetVendorDetails(string vendor_gid, MdlPblInvoiceGrnDetails values)
        {
            mssql = " select concat(b.address1,'',case when b.address2 is not null then b.address2 else '' end,'    ', b.city,'    ', b.state,'    ', b.postal_code) as vendor_address, b.fax,a.vendor_gid, " +
                    " a.contactperson_name, a.vendor_companyname,a.email_id,a.payment_terms,a.currencyexchange_gid,d.currency_code, " +
                    " a.contact_telephonenumber, c.country_name from acp_mst_tvendor a " +
                    " left join adm_mst_taddress b on b.address_gid = a.address_gid " +
                    " left join adm_mst_tcountry c on c.country_gid = b.country_gid " +
                    " left join crm_trn_tcurrencyexchange d on a.currencyexchange_gid = d.currencyexchange_gid " +
                    " where a.vendor_gid = '" + vendor_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(mssql);
            var GetVendorDetails = new List<GetInvoiceVendorDetails_list>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow Dt in dt_datatable.Rows)
                {
                    GetVendorDetails.Add(new GetInvoiceVendorDetails_list
                    {
                        vendor_address = Dt["vendor_address"].ToString(),
                        vendor_gid = Dt["vendor_gid"].ToString(),
                        fax = Dt["fax"].ToString(),
                        payment_terms = Dt["payment_terms"].ToString(),
                        contactperson_name = Dt["contactperson_name"].ToString(),
                        vendor_companyname = Dt["vendor_companyname"].ToString(),
                        email_id = Dt["email_id"].ToString(),
                        currencyexchange_gid = Dt["currencyexchange_gid"].ToString(),
                        contact_telephonenumber = Dt["contact_telephonenumber"].ToString(),
                        country_name = Dt["country_name"].ToString(),
                        currency_code = Dt["currency_code"].ToString(),
                    });
                }
                values.GetInvoiceVendorDetails_list = GetVendorDetails;
            }
        }
        public void DaGetPurchaseOrderDetails(string grn_gid, MdlPblInvoiceGrnDetails values)
        {
  
            mssql = " select grndtl_gid,FORMAT(a.qty_delivered * g.product_price - ((a.qty_delivered * g.product_price) * g.discount_percentage / 100) " +
                    " * h.percentage / 100, 2) as product_price_L, a.grn_gid, a.product_gid, a.uom_gid, (a.qty_delivered - a.qty_rejected - a.qty_shortage) " +
                    " as qty_delivered,a.qtyreceivedas,  qty_rejected,qty_accepted, qty_billed, qty_excess,a.qty_invoice, qty_returned, a.qty_grnadjusted, " +
                    " a.purchaseorderdtl_gid, receiveduom_gid, a.split_flag, a.location_gid, a.display_field,f.productgroup_code,f.productgroup_name," +
                    " e.productuom_code,  d.product_name,d.product_code,e.productuom_name,format(g.discount_percentage,2) as discount_percentage, " +
                    " ((a.qty_delivered * g.product_price) * g.discount_percentage / 100) as discount_amount,g.product_price,  g.excise_percentage," +
                    " g.excise_amount,g.tax_name,g.tax_name2,g.tax_name3,g.product_price_L as total_amount, FORMAT((a.qty_delivered * g.product_price - ((a.qty_delivered * g.product_price) * " +
                    " g.discount_percentage / 100))  * h.percentage / 100, 2) as tax_amount,format(g.tax_amount2,2) as tax_amount2,format(g.tax_amount3,2) as " +
                    " tax_amount3,b.purchaseorder_gid, (case when (h.percentage is null) then '0.00'  when (h.percentage='')then '0.00' else " +
                    " cast(h.percentage as char) end )as tax_percentage1,(case when(i.percentage is null) then '0.00'  when (i.percentage='') then '0.00' " +
                    " else cast(i.percentage as char) end)as tax_percentage2,(case when(j.percentage is null) then '0.00'  when (j.percentage='') then '0.00' " +
                    " else cast(j.percentage as char) end )as tax_percentage3,c.buybackorscrap,c.taxsegment_gid, g.taxsegmenttax_gid,g.tax_amount,c.vendor_gid from pmr_trn_tgrndtl a  left join pmr_trn_tgrn b on a.grn_gid=b.grn_gid " +
                    " left join pmr_trn_tpurchaseorder  c on b.purchaseorder_gid=c.purchaseorder_gid  left join pmr_trn_tpurchaseorderdtl g on a.purchaseorderdtl_gid=g.purchaseorderdtl_gid " +
                    " left join pmr_mst_tproduct d on a.product_gid=d.product_gid   left join pmr_mst_tproductuom e on d.productuom_gid=e.productuom_gid  left join pmr_mst_tproductgroup f " +
                    " on d.productgroup_gid=f.productgroup_gid  left join acp_mst_ttax h on h.tax_gid=g.tax1_gid  left join acp_mst_ttax i on i.tax_gid=g.tax2_gid  left join acp_mst_ttax j on " +
                    " j.tax_gid=g.tax3_gid where b.grn_gid in  ('" + grn_gid + "')";
            dt_datatable = objdbconn.GetDataTable(mssql);
            var GetInvoiceGrnDetails = new List<GetInvoiceGrnDetails_list>();
            var allTaxSegmentsList = new List<GetInvTaxSegmentList>();

            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow DT in dt_datatable.Rows)
                {
                  
                    GetInvoiceGrnDetails.Add(new GetInvoiceGrnDetails_list
                    {
                        grndtl_gid = DT["grndtl_gid"].ToString(),
                        grn_gid = DT["grn_gid"].ToString(),
                        product_gid = DT["product_gid"].ToString(),
                        qty_delivered = DT["qty_delivered"].ToString(),
                        qtyreceivedas = DT["qtyreceivedas"].ToString(),
                        qty_rejected = DT["qty_rejected"].ToString(),
                        qty_accepted = DT["qty_accepted"].ToString(),
                        qty_billed = DT["qty_billed"].ToString(),
                        qty_excess = DT["qty_excess"].ToString(),
                        qty_invoice = DT["qty_invoice"].ToString(),
                        qty_returned = DT["qty_returned"].ToString(),
                        qty_grnadjusted = DT["qty_grnadjusted"].ToString(),
                        purchaseorderdtl_gid = DT["purchaseorderdtl_gid"].ToString(),
                        receiveduom_gid = DT["receiveduom_gid"].ToString(),
                        split_flag = DT["split_flag"].ToString(),
                        location_gid = DT["location_gid"].ToString(),
                        display_field = DT["display_field"].ToString(),
                        productgroup_code = DT["productgroup_code"].ToString(),
                        productgroup_name = DT["productgroup_name"].ToString(),
                        productuom_code = DT["productuom_code"].ToString(),
                        product_name = DT["product_name"].ToString(),
                        product_code = DT["product_code"].ToString(),
                        productuom_name = DT["productuom_name"].ToString(),
                        discount_percentage = DT["discount_percentage"].ToString(),
                        discount_amount = DT["discount_amount"].ToString(),
                        product_price = DT["product_price"].ToString(),
                        excise_percentage = DT["excise_percentage"].ToString(),
                        excise_amount = DT["excise_amount"].ToString(),
                        tax_name = DT["tax_name"].ToString(),
                        tax_name2 = DT["tax_name2"].ToString(),
                        tax_name3 = DT["tax_name3"].ToString(),
                        tax_amount = DT["tax_amount"].ToString(),
                        tax_amount2 = DT["tax_amount2"].ToString(),
                        tax_amount3 = DT["tax_amount3"].ToString(),
                        purchaseorder_gid = DT["purchaseorder_gid"].ToString(),
                        tax_percentage1 = DT["tax_percentage1"].ToString(),
                        tax_percentage2 = DT["tax_percentage2"].ToString(),
                        tax_percentage3 = DT["tax_percentage3"].ToString(),
                        buybackorscrap = DT["buybackorscrap"].ToString(),
                        total_amount = DT["total_amount"].ToString(),
                        taxsegment_gid = DT["taxsegment_gid"].ToString(),
                        taxsegmenttax_gid = DT["taxsegmenttax_gid"].ToString(),
                        vendor_gid = DT["vendor_gid"].ToString(),
                      
                    });

                    string taxsegment_gid = DT["taxsegment_gid"].ToString();
                    string productGid = DT["product_gid"].ToString();
                    string productName = DT["product_name"].ToString();
                    string vendor_gid= DT["vendor_gid"].ToString();

                    if (!string.IsNullOrEmpty(taxsegment_gid) && taxsegment_gid != "undefined")
                    {
                        StringBuilder taxSegmentQuery = new StringBuilder("SELECT f.taxsegment_gid, d.taxsegment_gid, " +
                           "e.taxsegment_name, d.tax_name, d.tax_gid, CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) " +
                           "THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, d.tax_amount, a.mrp_price, " +
                           "a.cost_price FROM acp_mst_ttaxsegment2product d " +
                           "LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                           "LEFT JOIN acp_mst_tvendor f ON f.taxsegment_gid = e.taxsegment_gid " +
                           "LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid WHERE a.product_gid = '");
                        taxSegmentQuery.Append(productGid).Append("' AND f.vendor_gid = '").Append(vendor_gid).Append("'");

                        dt_datatable = objdbconn.GetDataTable(taxSegmentQuery.ToString());

                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt1 in dt_datatable.Rows)
                            {
                                allTaxSegmentsList.Add(new GetInvTaxSegmentList
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

            values.GetInvoiceGrnDetails_list = GetInvoiceGrnDetails;
            values.GetInvTaxSegmentList = allTaxSegmentsList;
           
        }

        public void DaGetViewApprovalAddSummary(string purchaseorder_gid, MdlPblInvoiceGrnDetails values)
        {
            try
            {

                mssql = " select concat (a.purchaseorder_gid) as purchaseorder_gid,m.user_firstname as requested_by,a.mode_despatch,a.requested_details,a.po_covernote,b.gst_no, " +
                        " b.email_id,b.contact_telephonenumber,b.contactperson_name,a.tax_amount as overalltax ,format(i.product_price_L,2) as product_price_L, a.purchaserequisition_gid,a.purchaseorder_remarks, " +
                        " case when a.exchange_rate is null then '1' else a.exchange_rate end as exchange_rate,w.address2,i.tax_name,i.tax_name2,i.tax_name3, " +
                        " a.purchaserequisition_gid, a.quotation_gid, a.branch_gid,a.ship_via,a.payment_terms,a.delivery_location,a.freight_terms, " +
                        " date_format(a.purchaseorder_date, '%d-%m-%y') as purchaseorder_date , " +
                        " a.vendor_address,a.vendor_contact_person,a.created_by,a.priority,a.priority_remarks, " + " case when a.priority = 'Y' then 'High' else 'Low' end as priority_n, " +
                        " CASE when a.invoice_flag<> 'IV Pending' then a.invoice_flag " +
                        " when a.grn_flag<> 'GRN Pending' then a.grn_flag " +
                        " else a.purchaseorder_flag end as 'overall_status', a.approver_remarks, " +
                        " a.purchaseorder_reference,format(a.total_amount, 2) as total_amount , " +
                        " concat(f.address1,f.postal_code) as branch_add1, " +
                        " CONCAT(b.vendor_companyname, '\n', w.address1, '\n',w.city, '\n', w.state, '\n', w.postal_code, '\n',  a.vendor_emailid ,'\n', b.contactperson_name, '\n',b.contact_telephonenumber ) AS shipping_address, " +
                        " a.vendor_emailid, a.vendor_faxnumber, a.vendor_contactnumber, " +
                        " a.termsandconditions, a.payment_term, " +
                        " b.vendor_companyname, g.user_firstname as approved_by, concat(i.qty_ordered,' ',i.productuom_name) as qyt_unit ," +
                        " concat(c.user_firstname, ' - ', e.department_name) as user_firstname, " +
                        " d.employee_emailid, d.employee_mobileno, f.branch_name," +
                        " format(a.discount_amount, 2) as discount_amount, format(a.tax_percentage, 2) as tax_percentage, " +
                        " format(a.addon_amount, 2) as addon_amount,format(a.roundoff, 2) as roundoff, " +
                        " concat_ws('-', h.costcenter_name, h.costcenter_gid) as costcenter_name,format(h.budget_available, 2) as budget_available,h.costcenter_gid, " +
                        " a.payment_days,a.tax_gid,a.delivery_days,format(a.freightcharges,2) as freightcharges,a.buybackorscrap,a.manualporef_no, " +
                        " format(a.packing_charges, 2) as packing_charges, format(a.insurance_charges, 2) as insurance_charges ,format(a.discount_amount,2) as additional_discount, " +
                        " i.purchaseorderdtl_gid, i.product_gid, " +
                        " i.product_price, i.qty_ordered,i.needby_date," +
                        " format(i.discount_percentage, 2) as discount_percentage , " +
                        " format(i.discount_amount, 2) discount_amount1 , " +
                        " format(i.tax_percentage, 2) as tax_percentage, " +
                        " format(i.tax_percentage2, 2) as tax_percentage2, " +
                        " format(i.tax_percentage3, 2) as tax_percentage3, " +
                        " format(i.tax_amount, 2) as tax_amount, " +
                        " format(i.tax_amount2, 2) as tax_amount2, " +
                        " format(i.tax_amount3, 2) as tax_amount3,i.taxseg_taxname1,i.taxseg_taxpercent1,format(i.taxseg_taxamount1,2) AS taxseg_taxamount1,i.taxseg_taxname2,i.taxseg_taxpercent2,format(i.taxseg_taxamount2,2) AS taxseg_taxamount2, " +
                        " i.qty_Received, i.qty_grnadjusted, i.taxseg_taxname3,i.taxseg_taxpercent3,format(i.taxseg_taxamount3,2) as taxseg_taxamount3, " +
                        " i.product_remarks, format((qty_ordered * i.product_price), 2) as product_totalprice, " +
                        " format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2) " +
                        " as product_total, i.product_code, (i.product_name) as product_name,format(a.netamount, 2) as netamount," +
                        " k.productgroup_name, i.productuom_name,i.purchaseorder_gid,i.display_field_name, a.currency_code,a.poref_no,i.tax1_gid,y.tax_name as overalltaxname, " +
                        "(SELECT FORMAT(SUM(tax_amount), 2) FROM pmr_trn_tpurchaseorderdtl where purchaseorder_gid = '" + purchaseorder_gid + "') AS overall_tax,Z.invoice_amount AS invoice_amount, Z.total AS total" +
                        " from pmr_trn_tpurchaseorder a " +
                        " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid " +
                        " left join adm_mst_taddress w on b.address_gid = w.address_gid " +
                        " left join adm_mst_tuser c on c.user_gid = a.created_by " +
                        " left join hrm_mst_temployee d on d.user_gid = c.user_gid " +
                        " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid " +
                        " left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid " +
                        " left join adm_mst_tuser g on g.user_gid = a.approved_by " +
                        " left join pmr_mst_tcostcenter h on h.costcenter_gid = a.costcenter_gid " +
                        " left join pmr_trn_tpurchaseorderdtl i ON a.purchaseorder_gid = i.purchaseorder_gid " +
                        " left join acp_mst_ttax y on y.tax_gid = a.tax_gid " +
                        " left join pmr_mst_tproduct j on i.product_gid = j.product_gid " +
                        " left join pmr_trn_tinvoiceapprovaldtl z on z.product_gid = j.product_gid " +
                        " left join pmr_mst_tproductgroup k on j.productgroup_gid = k.productgroup_gid " +
                        " left join pmr_mst_tproductuom l on i.uom_gid = l.productuom_gid " +
                        " left join adm_mst_tuser m on m.user_gid = a.requested_by " +
                        " where a.purchaseorder_gid = '" + purchaseorder_gid + "' group by j.product_gid ";
                dt_datatable = objdbconn.GetDataTable(mssql);
                var getModuleList = new List<GetViewApproval>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetViewApproval
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
                            invoice_amount = dt["invoice_amount"].ToString(),
                            total = dt["total"].ToString()




                        });

                        values.GetViewApproval = getModuleList;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO summary!";
                objcmnfunction.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              mssql + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetVendorUserDetails ( string user_gid, MdlPblInvoiceGrnDetails values)
        {
           mssql = " select concat_ws(' - ', a.user_firstname, c.department_name) as employee_name, " +
                   " b.employee_emailid, b.employee_phoneno, c.department_name,b.employee_mobileno from  " +
                   " adm_mst_tuser a  " +
                   " left join hrm_mst_temployee b on a.user_gid = b.user_gid  " +
                   " left join hrm_mst_tdepartment c on b.department_gid = c.department_gid  " +
                   " where a.user_gid = '"+ user_gid  + "'";
            dt_datatable = objdbconn.GetDataTable(mssql);
            var GetVendorUserDetails = new List<GetVendorUserDetails_list>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach(DataRow VUD in dt_datatable.Rows)
                {
                    GetVendorUserDetails.Add(new GetVendorUserDetails_list()
                    {
                        employee_name = VUD["employee_name"].ToString(),
                        employee_emailid  = VUD["employee_emailid"].ToString(),
                        employee_phoneno = VUD["employee_phoneno"].ToString(),
                        employee_mobileno = VUD["employee_mobileno"].ToString(),
                    });                   
                }
                values.GetVendorUserDetails_list = GetVendorUserDetails;
            }
        }

        public void DaGetPurchaseTyepDetails(MdlPblInvoiceGrnDetails values)
        {
            mssql = " select a.account_gid, a.purchasetype_name from pmr_trn_tpurchasetype a " +
                " where a.account_gid <> 'null'";
            dt_datatable = objdbconn.GetDataTable (mssql);
            var GetPurchaseType = new List<GetPurchaseType_list>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach(DataRow Pr in dt_datatable.Rows)
                {
                    GetPurchaseType.Add(new GetPurchaseType_list() {

                        account_gid = Pr["account_gid"].ToString(),
                        purchasetype_name = Pr["purchasetype_name"].ToString(),

                    });
                }
                values.GetPurchaseType_list = GetPurchaseType;
            }
        }

        public void DaPostOverAllSubmit(string user_gid, string employee_gid, OverallSubmit_list values)
        {
            msINGetGID = objcmnfunction.GetMasterGID("SIVP");
            mssql = " select  i.tax1_gid as producttax, i.tax2_gid as producttax2, concat(i.qty_ordered,' ',i.productuom_name) as qyt_unit ,n.qty_delivered,n.qtyreceivedas, format(i.product_price_L,2) as product_price_L, format(a.addon_amount, 2) as addon_amount," +
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
                        " i.discount_amount,l.productuom_code, a.exchange_rate,i.uom_gid,i.taxsegment_gid,i.tax_name,i.tax_name2,a.taxsegmenttax_gid,a.mode_despatch,p.grndtl_gid,o.grn_gid,a.purchaseorder_from " +
                     "  ,n.qtyreceivedas,FORMAT((n.qtyreceivedas * i.product_price_L * i.discount_percentage / 100), 2) AS discountamount," +
                     " FORMAT(( (n.qtyreceivedas * i.product_price_L) - (n.qtyreceivedas * i.product_price_L * i.discount_percentage / 100) ) * (i.tax_percentage / 100), 2) AS tax1," +
                     " FORMAT(((n.qtyreceivedas * i.product_price_L) - (n.qtyreceivedas * i.product_price_L * i.discount_percentage / 100)) * (i.tax_percentage2 / 100), 2) AS tax2," +
                     " FORMAT(((n.qtyreceivedas * i.product_price_L) - (n.qtyreceivedas * i.product_price_L * i.discount_percentage / 100) + (((n.qtyreceivedas * i.product_price_L)" +
                     " - (n.qtyreceivedas * i.product_price_L * i.discount_percentage / 100)) * (i.tax_percentage / 100)) + (((n.qtyreceivedas * i.product_price_L) - " +
                     " (n.qtyreceivedas * i.product_price_L * i.discount_percentage / 100)) * (i.tax_percentage2 / 100) )), 2) AS total_amount " +                        
                        " from pmr_trn_tpurchaseorder a  " +
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
                        " where a.purchaseorder_gid = '" + values.purchaseorder_gid + "' " +
                        " and i.purchaseorderdtl_gid in (select a.purchaseorderdtl_gid from pmr_trn_tgrndtl a " +
                        " left join pmr_trn_tpurchaseorderdtl i ON a.purchaseorderdtl_gid = i.purchaseorderdtl_gid " +
                        " where i.purchaseorder_gid = '" + values.purchaseorder_gid + "') group by purchaseorderdtl_gid ";
            dt_datatable = objdbconn.GetDataTable(mssql);
            foreach(DataRow Pr in dt_datatable.Rows)
            {
                string taxsegment_gid= Pr["taxsegment_gid"].ToString();
                string taxsegmenttax_gid = Pr["taxsegmenttax_gid"].ToString();
                lsproduct_gid = Pr["product_gid"].ToString();
                lsuom_gid = Pr["uom_gid"].ToString();
                lsmode_despatch = Pr["mode_despatch"].ToString();
                lsproductgroup_name = Pr["productgroup_name"].ToString();
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
                if (decimal.TryParse(Pr["total_amount"]?.ToString()?.Replace(",", ""), out product_total))
                {
                    lsproduct_total= product_total;
                }
                else
                {
                    lsproduct_total = 0;
                }

                decimal lsInvoiceqty_billed, Invoiceqty_billed;
                if (decimal.TryParse(Pr["qtyreceivedas"]?.ToString(), out Invoiceqty_billed))
                {
                    lsInvoiceqty_billed = Invoiceqty_billed;
                }
                else
                {
                    lsInvoiceqty_billed = 0;
                }
                decimal lsproducttotal_amount, producttotal_amount;
                if (decimal.TryParse(Pr["total_amount"]?.ToString(), out producttotal_amount))
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
                lsTax_Percentage2 = Pr["tax_percentage2"].ToString();
                lsTax_Percentage3 = Pr["tax_percentage3"].ToString() ;
                if (decimal.TryParse(Pr["exchange_rate"].ToString(), out decimal exchangeRate))
                {
                    lsexchange_rate = exchangeRate;
                }
                else
                {
                    lsexchange_rate = 0;
                }
                if (decimal.TryParse(Pr["tax1"].ToString(), out decimal taxAmount))
                {
                    lsTax_Amount = taxAmount;
                }
               
                else
                {
                    
                    lsTax_Amount = 0;
                }
                if (decimal.TryParse(Pr["tax2"].ToString(), out decimal taxAmount2))
                {
                    lsTax_Amount2 = taxAmount2;
                }

                else
                {

                    lsTax_Amount2 = 0;
                }
                if (decimal.TryParse(Pr["discountamount"].ToString(), out decimal discountamount))
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


                msGetGID = objcmnfunction.GetMasterGID("SIVC");

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
                if(lsTax_Percentage != "0.00")
                {
                    mssql += "'" + lsTax_Percentage + "', ";
                }
                else
                {
                    mssql += "'0.00', ";
                }
                if (lsTax_Percentage2 != "0.00")
                {
                    mssql += "'" + lsTax_Percentage2 + "', ";
                }
                else
                {
                    mssql += "'0.00', ";
                }
                if (lsTax_Percentage3 != "0.00")
                {
                    mssql += "'" + lsTax_Percentage3 + "', ";

                }
                else
                {
                    mssql += "'0.00', ";
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
                           "'" + lsInvoiceqty_billed + "', " +
                           "'" + values.product_remarks + "', " +
                           "'" + lsproduct_name + "'," +
                           "'" + lsproduct_total + "'," +
                           "'" + lscurrency_discountamount + "'," +
                           "'" + lscurrency_taxamount1 + "',";
                if(lscurrency_taxamount2 != "")
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

                mspGetGID = objcmnfunction.GetMasterGID("SPIP");
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


                if(lspurchaseorder_from == "Purchase")
                {
                    decimal lsPO_Sum = lsInvoiceqty_billed;

                    decimal lspo_sumdec = lsInvoiceqty_billed;

                    mssql = " select qty_invoice from pmr_trn_tpurchaseorderdtl  where " +
                            " purchaseorderdtl_gid = '" + lspurchaseorderdtl_gid + "'";
                    odbcdr = objdbconn.GetDataReader(mssql);
                    if (odbcdr.HasRows)
                    {
                        odbcdr.Read();
                        lsqty_invoice = Convert.ToInt32(odbcdr["qty_invoice"].ToString());
                        lsPO_Sum = lsPO_Sum += lsqty_invoice;
                        odbcdr.Close();
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
                    odbcdr.Read();
                    if (odbcdr.HasRows)
                    {
                     
                        lstPO_IV_flag = "Invoice Raised";
                        
                    }
                    else
                    {
                        lstPO_IV_flag = "Invoice Raised";
                    }
                    odbcdr.Close();
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
                        odbcdr.Read();
                        if (decimal.TryParse(odbcdr["qty_invoice"].ToString(), out decimal qtyinvoice))
                        {
                            delsqty_invoice = qtyinvoice;
                        }
                        else
                        {
                            delsqty_invoice = 0;
                        }                                                
                        lsGRN_Sum = lsGRN_Sum += delsqty_invoice;
                        odbcdr.Close() ;
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
                        odbcdr.Read();
                        lsinvoice_status = "IV Work In Progress";
                        lstGRN_IV_flag = "Invoice Raised Partial";
                        odbcdr.Close();
                    }
                    else
                    {
                        odbcdr.Read() ;
                        lsinvoice_status = "IV Completed";
                        lstGRN_IV_flag = "Invoice Raised";
                        odbcdr.Close(); 
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
            if(odbcdr.HasRows)
            {
                odbcdr.Read();
                lsvendoraddress = odbcdr["vendor_address"].ToString();
                lsbranch = odbcdr["branch_gid"].ToString();
                lsvendor_gid = odbcdr["vendor_gid"].ToString();
                lsvendor_contact = odbcdr["vendor_contact_person"].ToString();
                odbcdr.Close() ;
            }
            //mssql = " select invoiceref_flag from pbl_mst_tconfiguration ";
            //odbcdr = objdbconn.GetDataReader(mssql);
            //if(odbcdr.HasRows)
            //{
            //    odbcdr.Read();
            //    lsinvoiceref_flag = odbcdr["invoiceref_flag"].ToString();
            //    if(lsinvoiceref_flag == "Y")
            //    {
            //        lsinv_ref_no = values.inv_ref_no;
            //    }
            //    else
            //    {
            //        lsinv_ref_no = objcmnfunction.GetMasterGID("PINV");
            //        odbcdr.Close();
            //    }
            //}

            //string ls_referenceno;

            //if (values.inv_ref_no == "")
            //{
            //    ls_referenceno = objcmnfunction.getSequencecustomizerGID("PINV", "PBL", values.branch_name);
            //}
            //else
            //{

            //    ls_referenceno = values.inv_ref_no;
            //}
            mssql = "select branch_gid from hrm_mst_tbranch where branch_name ='" + values.branch_name + "'";
            lsbranchname = objdbconn.GetExecuteScalar(mssql);
            

            if (values.inv_ref_no == "")
            {
                ls_referenceno = objcmnfunction.getSequencecustomizerGID("PINV", "PBL", lsbranchname);
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
                        objcmnfunction.Tracelog(msGetGid, user_gid, trace_comment, "added_vendor");

                    }
                }
                odbcdr.Close();
            }

            //string msAccGetGID = objcmnfunction.GetMasterGID("FCOA");

            //mssql = " insert into acc_mst_tchartofaccount( " +
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
            //mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
            //if (mnResult == 1)
            //{
            //    mssql = " update acp_mst_tvendor set " +
            //            " account_gid = '" + msAccGetGID + "'" +
            //            " where vendor_gid='" + vendor_gid + "'";
            //    mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
            //}


            //int lspacking_charges = values.packing_charges;
            //int lscurrency_packing =  * lsexchange_rate;
            string payment_date = values.payment_date;
            DateTime parsedDate;
            if (DateTime.TryParse(payment_date, out parsedDate))
            {
                parsed_Date = parsedDate.ToString("yyyy-MM-dd");
            }

            //string invoice_date = values.invoice_date;
            //DateTime Invoice_Date;
            //if (DateTime.TryParse(invoice_date, out Invoice_Date))
            //{
            //   invoice_Date = Invoice_Date.ToString("yyyy-MM-dd");
            //}
            string invoice_date = values.invoice_date;
            if (!string.IsNullOrEmpty(invoice_date))
            {
                DateTime uiDate = DateTime.ParseExact(invoice_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                invoice_Date = uiDate.ToString("yyyy-MM-dd");
            }
            else
            {
                invoice_Date = DateTime.Now.ToString("yyyy-MM-dd");
            }




            //string uiDateStr2 = values.invoice_date;
            //DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            //string Invoice_date = uiDate2.ToString("yyyy-MM-dd");

            mssql = "select tax_prefix from acp_mst_ttax where tax_gid ='" + values.tax_name4 + "' ";
            string lstax = objdbconn.GetExecuteScalar(mssql);


            mssql = " insert into acp_trn_tinvoice (" +
                    " invoice_gid, " +
                    " invoice_refno, " +
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
                    "'" + ls_referenceno + "'," +
                    "'" + values.inv_ref_no + "'," +
                    "'" + values.inv_ref_no + "'," +
                    "'" + values.shipping_address + "'," +
                    "'" + (String.IsNullOrEmpty(lsmode_despatch) ? lsmode_despatch : lsmode_despatch.Replace("'", "\\\'")) + "',"+
                    "'" + values.billing_email + "'," +
                    "'" + lsvendor_contact + "'," +
                    "'" + (String.IsNullOrEmpty(lsvendoraddress) ? lsvendoraddress : lsvendoraddress.Replace("'", "\\\'")) + "'," +
                    "'" + user_gid + "'," +
                    "'" + invoice_Date + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
                    if (string.IsNullOrEmpty(values.total_amount))
                    {
                        mssql += "'0.00',";
                    }
                    else
                    {
                        mssql += "'" + values.total_amount.Replace(",", "") +"',";
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
            "'" + "Payment Pending" + "'," +
            "'" + (String.IsNullOrEmpty(values.invoice_remarks) ? values.invoice_remarks : values.invoice_remarks.Replace("'", "\\\'"))  + "', " +
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
            mssql += "'" + lstax + "'," +
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

            mssql += "'" + values.tax_name4 + "'," +
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
            mssql += "'" + (String.IsNullOrEmpty(values.invoice_remarks) ? values.invoice_remarks : values.invoice_remarks.Replace("'", "\\\'")) + "'," +
                     "'" + (String.IsNullOrEmpty(values.template_content) ? values.template_content : values.template_content.Replace("'", "\\\'")) + "'," +
                     "'" + (String.IsNullOrEmpty(values.delivery_term) ? values.delivery_term : values.delivery_term.Replace("'", "\\\'")) + "'," +
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

                    if (string.IsNullOrEmpty(values.exchange_rate))
                    {
                        values.exchange_rate ="1" ;
                    }
                    else { 
                        
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
                     values.purchase_type, fin_roundoff, fin_freightcharges, fin_buybackCharges, lstax, fin_overalltax_amount, fin_forwardingCharges, fin_insuranceCharges);



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

                            // Tax 3 Calculation
                            msSQL = "SELECT SUM(tax_amount3_L) AS tax3 FROM acp_trn_tinvoicedtl " +
                                    "WHERE invoice_gid = '" + msINGetGID + "' AND tax3_gid = '" + objODBCDataReader["tax_gid"] + "'";
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

                                objfincmn.jn_purchase_tax(msINGetGID, ls_referenceno, values.invoice_remarks, lstaxamount, lstax_gid);
                            }
                        }
                    }

                    objdbconn.CloseConn();
                }

                         
                values.status = true;
                values.message = "Invoice Confirmed Succesfully !";
            }
            else
            {
                values.status = false;
                values.message = "Error occured while Invoice Confirmed";
            }
        }

        public void DaGetInvoiceNetAmount(string grn_gid , MdlPblInvoiceGrnDetails values)
        {
            mssql = " SELECT SUM( product_price_L) as sum_product_price_L FROM pmr_trn_tgrndtl a LEFT JOIN pmr_trn_tpurchaseorderdtl g " +
                    " ON a.purchaseorderdtl_gid=g.purchaseorderdtl_gid    LEFT JOIN acp_mst_ttax h ON h.tax_gid=g.tax1_gid    " +
                    " LEFT JOIN pmr_trn_tgrn b ON a.grn_gid=b.grn_gid " +
                    " where b.grn_gid = '" + grn_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(mssql);
            var GetNetamount_list = new List<GetNetamount_list>();
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow Dt in dt_datatable.Rows)
                {
                    GetNetamount_list.Add(new GetNetamount_list
                    {
                        netamount = Dt["sum_product_price_L"].ToString()
                      
                      
                    });
                }
                values.GetNetamount_list  = GetNetamount_list;
            }
        }

        public void DaGetTax4Dtl(MdlPblInvoiceGrnDetails values)
        {
            try
            {



                mssql = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(mssql);
                var getModuleList = new List<GetTax4Dropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTax4Dropdown

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name4 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTaxfourDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Dropdown !";
                objcmnfunction.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + mssql + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostpricerequest(string user_gid, string employee_gid, OverapprovalSubmit_list values)
        {
            try
            {
                for (int i = 0; i < values.GetInvoiceGrnDetails_list.Count; i++)
                {
                    if (msGetGID1 == null)
                    {
                        msGetGID1 = objcmnfunction.GetMasterGID("PIIV");
                        mssql = " insert into pmr_trn_tinvoiceapproval (" +
                                    " approval_gid, " +
                                    " purchaseorder_gid, " +
                                    " req_status, " +
                                    " requested_by, " +
                                    " requested_date " +
                                    " )values ( " +
                                    "'" + msGetGID1 + "', " +
                                    "'" + values.GetInvoiceGrnDetails_list[i].purchaseorder_gid + "'," +
                                    "'Pending', " +
                                    "'" + user_gid + "', " +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
                    }
                    msGetGID2 = objcmnfunction.GetMasterGID("PIVD");
                    mssql = " insert into pmr_trn_tinvoiceapprovaldtl (" +
                              " approvaldtl_gid, " +
                              " approval_gid, " +
                              " purchaseorder_gid, " +
                              " product_gid, " +
                              " product_name, " +
                              " product_price, " +
                              " total, " +
                              " invoice_amount, " +
                              " requested_by, " +
                              " requested_date " +
                              " )values ( " +
                              "'" + msGetGID2 + "', " +
                              "'" + msGetGID1 + "', " +
                              "'" + values.GetInvoiceGrnDetails_list[i].purchaseorder_gid + "'," +
                              "'" + values.GetInvoiceGrnDetails_list[i].product_gid + "'," +
                              "'" + values.GetInvoiceGrnDetails_list[i].product_name + "'," +
                              "'" + values.GetInvoiceGrnDetails_list[i].product_price_L + "'," +
                              "'" + values.GetInvoiceGrnDetails_list[i].product_total + "'," +     
                              "'" + values.GetInvoiceGrnDetails_list[i].invoice_amount + "'," +
                              "'" + user_gid + "', " +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Approval request Raised";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occured while Invoice Confirmed";
                    }
                }
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while inserting approval !";
                objcmnfunction.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + mssql + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }



        public void DaGetPurchaseInvoicesummary(string purchaseorder_gid, string grn_gid, MdlPblInvoiceGrnDetails values)
        {
            try
            {

                mssql = " SELECT x.tax_prefix,a.total_amount_L, CONCAT(a.purchaseorder_gid) AS purchaseorder_gid,m.user_firstname AS requested_by,a.mode_despatch,a.po_type," +
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
                        " ,a.total_amount as pototalamount ,a.discount_amount as podiscountamount ,a.tax_percentage as potaxpercentage,a.tax_amount as potaxamount,a.addon_amount as poaddonamount,a.freightcharges as pofreightcharges,a.tax_gid as potaxgid,a.roundoff as poroundoff,a.netamount as poproducttotal" +
                        " FROM pmr_trn_tpurchaseorder a LEFT JOIN acp_mst_tvendor b ON a.vendor_gid = b.vendor_gid    " +
                        " LEFT JOIN adm_mst_taddress w ON b.address_gid = w.address_gid LEFT JOIN adm_mst_tuser c ON c.user_gid = a.created_by   " +
                        " LEFT JOIN hrm_mst_temployee d ON d.user_gid = c.user_gid  LEFT JOIN hrm_mst_tdepartment e ON e.department_gid = d.department_gid " +
                        " LEFT JOIN hrm_mst_tbranch f ON a.branch_gid = f.branch_gid LEFT JOIN adm_mst_tuser g ON g.user_gid = a.approved_by    LEFT JOIN acp_mst_ttax x ON x.tax_gid = a.tax_gid " +
                        " LEFT JOIN pmr_mst_tcostcenter h ON h.costcenter_gid = a.costcenter_gid LEFT JOIN adm_mst_tuser m ON m.user_gid = a.requested_by LEFT JOIN crm_trn_tcurrencyexchange z ON z.currencyexchange_gid = a.currency_code" +
                        " where a.purchaseorder_gid = '" + purchaseorder_gid + "'";
                dt_datatable = objdbconn.GetDataTable(mssql);
                var getModuleList = new List<GetinviocePO>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetinviocePO
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
                            requested_by = dt["requested_by"].ToString(),
                            requested_details = dt["requested_details"].ToString(),
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
                            po_type = dt["po_type"].ToString(),
                            overalltax = dt["overalltax"].ToString(),
                            tax_prefix = dt["tax_prefix"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            pototalamount = dt["pototalamount"].ToString(),
                            podiscountamount = dt["podiscountamount"].ToString(),
                            potaxpercentage = dt["potaxpercentage"].ToString(),
                            potaxamount = dt["potaxamount"].ToString(),
                            poaddonamount = dt["poaddonamount"].ToString(),
                            pofreightcharges = dt["pofreightcharges"].ToString(),
                            potaxgid = dt["potaxgid"].ToString(),
                            poroundoff = dt["poroundoff"].ToString(),
                            poproducttotal = dt["poproducttotal"].ToString(),
                        });

                        values.GetinviocePO = getModuleList;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO summary!";
                objcmnfunction.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              mssql + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetPurchaseInvoiceproduct(string purchaseorder_gid, MdlPblInvoiceGrnDetails values)
        {
            try
            {
                double grand_total = 0.00;
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
                        " FORMAT(i.tax_amount + i.tax_amount2, 2) AS overall_tax ,i.tax_name, i.tax_percentage, i.tax_name2, i.tax_percentage2 , i.tax_amount,i.tax_amount2" +
                       " ,n.qtyreceivedas, FORMAT((n.qtyreceivedas * i.product_price_L * i.discount_percentage / 100), 2) AS discountamount," +
                       " FORMAT(((n.qtyreceivedas * i.product_price_L) - (n.qtyreceivedas * i.product_price_L * i.discount_percentage / 100)) * (i.tax_percentage / 100), 2)" +
                       " AS tax1, FORMAT(((n.qtyreceivedas * i.product_price_L) - (n.qtyreceivedas * i.product_price_L * i.discount_percentage / 100)) * (i.tax_percentage2 / 100),2)" +
                       " AS tax2, FORMAT(((n.qtyreceivedas * i.product_price_L) - (n.qtyreceivedas * i.product_price_L * i.discount_percentage / 100) + (((n.qtyreceivedas * i.product_price_L)" +
                       " - (n.qtyreceivedas * i.product_price_L * i.discount_percentage / 100)) * (i.tax_percentage / 100)) + (((n.qtyreceivedas * i.product_price_L) - " +
                       " (n.qtyreceivedas * i.product_price_L * i.discount_percentage / 100)) * (i.tax_percentage2 / 100))), 2) AS total_amount " +
                        " from pmr_trn_tpurchaseorder a  " +
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
                        " left join adm_mst_tuser m on m.user_gid = a.requested_by "+
                        "  left join pmr_trn_tgrndtl n on n.purchaseorderdtl_gid = i.purchaseorderdtl_gid " +
                        " left join pmr_trn_tgrn o on o.grn_gid = n.grn_gid "+
                        " where a.purchaseorder_gid = '" + purchaseorder_gid + "' " +
                        " and i.purchaseorderdtl_gid in (select a.purchaseorderdtl_gid from pmr_trn_tgrndtl a " +
                        " left join pmr_trn_tpurchaseorderdtl i ON a.purchaseorderdtl_gid = i.purchaseorderdtl_gid " +
                        " where i.purchaseorder_gid = '" + purchaseorder_gid + "')";
                dt_datatable = objdbconn.GetDataTable(mssql);
                var getModuleList = new List<GetinvioceProduct>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        //grand_total += double.Parse(dt["product_price_L"].ToString());
                        //grand_total += (double.Parse(dt["product_price_L"].ToString()) * double.Parse(dt["qtyreceivedas"].ToString())- double.Parse(dt["discountamount"].ToString()) + double.Parse(dt["tax1"].ToString()) + double.Parse(dt["tax2"].ToString()));
                        grand_total += Math.Round( (double.Parse(dt["product_price_L"].ToString()) * double.Parse(dt["qtyreceivedas"].ToString())
                        - double.Parse(dt["discountamount"].ToString()) + double.Parse(dt["tax1"].ToString())  + double.Parse(dt["tax2"].ToString())), 2);

                        getModuleList.Add(new GetinvioceProduct
                        {

                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            display_field_name = dt["display_field_name"].ToString(),
                            qyt_unit = dt["qtyreceivedas"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount1 = dt["discountamount"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            tax_amount = dt["tax1"].ToString(),
                            tax_amount2 = dt["tax2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            product_total = dt["total_amount"].ToString(),
                            addon_amount = dt["addon_amount"].ToString(),
                            overalltaxname = dt["overalltaxname"].ToString(),
                            overall_tax = dt["overall_tax"].ToString(),
                            product_price_L = dt["product_price_L"].ToString(),
                        });

                        values.GetinvioceProduct = getModuleList;
                        values.grand_total = grand_total;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO summary!";
                objcmnfunction.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              mssql + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaPostSubmit(string user_gid, string employee_gid, OverallSubmit_list values)
        {
            mssql = " select a.freightcharges,a.discount_amount,a.addon_amount,a.tax_percentage,a.currency_code,a.exchange_rate,a.shipping_address,a.total_amount,d.tax_name,a.tax_amount," +
                    " a.tax_gid,a.freight_terms,a.payment_terms,a.netamount,a.requested_details,a.po_covernote,a.requested_by,a.termsandconditions,a.vendor_gid,a.branch_gid , " +
                     " b.vendor_companyname,b.contactperson_name,b.contact_telephonenumber,b.email_id,b.billing_email,c.branch_name,a.insurance_charges,a.packing_charges,a.roundoff " +
                     "  from pmr_trn_tpurchaseorder a left join acp_mst_tvendor b on b.vendor_gid = a.vendor_gid " +
                     "   left join hrm_mst_tbranch c on c.branch_gid = a.branch_gid " +
                     "   left join acp_mst_ttax d on d.tax_gid = a.tax_gid " +
                      "   where a.purchaseorder_gid = '" + values.purchaseorder_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(mssql);
            foreach (DataRow Mr in dt_datatable.Rows)
            {
                string freightcharges = Mr["freightcharges"].ToString();
                string discount_amount = Mr["discount_amount"].ToString();
                string addon_amount = Mr["addon_amount"].ToString();
                string tax_percentage = Mr["tax_percentage"].ToString();
                string currency_code = Mr["currency_code"].ToString();
                string exchange_rate = Mr["exchange_rate"].ToString();
                string shipping_address = Mr["shipping_address"].ToString();
                string tax_gid = Mr["tax_gid"].ToString();
                string freight_terms = Mr["freight_terms"].ToString();
                string payment_terms = Mr["payment_terms"].ToString();
                string netamount = Mr["netamount"].ToString();
                string requested_details = Mr["requested_details"].ToString();
                string po_covernote = Mr["po_covernote"].ToString();
                string requested_by = Mr["requested_by"].ToString();
                string termsandconditions = Mr["termsandconditions"].ToString();
                string vendor_gid = Mr["vendor_gid"].ToString();
                string branch_gid = Mr["branch_gid"].ToString();
                string vendor_companyname = Mr["vendor_companyname"].ToString();
                string contactperson_name = Mr["contactperson_name"].ToString();
                string contact_telephonenumber = Mr["contact_telephonenumber"].ToString();
                string email_id = Mr["email_id"].ToString();
                string billing_email = Mr["billing_email"].ToString();
                string total_amount = Mr["total_amount"].ToString();
                string tax_name = Mr["tax_name"].ToString();
                string tax_amount = Mr["tax_amount"].ToString();
                string branch_name = Mr["branch_name"].ToString();
                string roundoff = Mr["roundoff"].ToString();
                string packing_charges = Mr["packing_charges"].ToString();
                string insurance_charges = Mr["insurance_charges"].ToString();


                msINGetGID = objcmnfunction.GetMasterGID("SIVP");

                mssql = " select c.grndtl_gid,b.grn_gid,FORMAT(c.qty_delivered * a.product_price - ((c.qty_delivered * a.product_price) * a.discount_percentage / 100) " +
                        " * h.percentage / 100, 2) as product_price_L,a.purchaseorderdtl_gid,a.tax_percentage,e.purchaseorder_from,a.purchaseorder_gid, a.product_gid," +
                        " a.uom_gid, a.product_price,d.productuom_code, c.qty_delivered, a.tax_amount, a.discount_percentage, a.tax_name,e.exchange_rate,  a.product_code, " +
                        " a.product_name,a.tax1_gid, a.productuom_name, a.discount_amount,a.taxsegment_gid,a.taxsegmenttax_gid from pmr_trn_tpurchaseorderdtl a  left join pmr_trn_tgrn b on " +
                        " a.purchaseorder_gid=b.purchaseorder_gid  left join pmr_trn_tgrndtl c on c.grn_gid=b.grn_gid  left join pmr_mst_tproductuom d on " +
                        " d.productuom_name = c.productuom_name  left join pmr_trn_tpurchaseorder e on e.purchaseorder_gid=a.purchaseorder_gid " +
                        " left join acp_mst_ttax h on h.tax_gid=a.tax1_gid " +
                        " where a.purchaseorder_gid ='" + values.purchaseorder_gid + "' group by purchaseorderdtl_gid ";
                dt_datatable = objdbconn.GetDataTable(mssql);
                foreach (DataRow Pr in dt_datatable.Rows)
                {
                    string taxsegment_gid = Pr["taxsegment_gid"].ToString();
                    string taxsegmenttax_gid = Pr["taxsegmenttax_gid"].ToString();
                    lsproduct_gid = Pr["product_gid"].ToString();
                    lsuom_gid = Pr["uom_gid"].ToString();
                    //decimal lsproduct_price = decimal.Parse(Pr["product_price"].ToString());
                    //decimal lsproduct_total = decimal.Parse(Pr["product_price_L"].ToString().Replace(",", ""));
                    //decimal lsInvoiceqty_billed = decimal.Parse(Pr["qty_delivered"].ToString());
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
                    if (decimal.TryParse(Pr["product_price_L"]?.ToString()?.Replace(",", ""), out product_total))
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


                    lsDiscount_Percentage = Pr["discount_percentage"].ToString();
                    lsTax_name = Pr["tax_name"].ToString();
                    lsproduct_code = Pr["product_code"].ToString();
                    lsproduct_name = Pr["product_name"].ToString();
                    lsTax1_gid = Pr["tax1_gid"].ToString();
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
                    if (decimal.TryParse(Pr["discount_amount"].ToString(), out decimal discountamount))
                    {
                        lsDiscount_Amount = discountamount;
                    }
                    else
                    {
                        lsDiscount_Amount = 0;
                    }

                    decimal lsnet_price = lsInvoiceqty_billed * lsproduct_price;
                    decimal lstotalproduct_amount = lsInvoiceqty_billed * lsproduct_price - lsDiscount_Amount + lsTax_Amount;
                    lscurrency_unitprice = lsproduct_price * lsexchange_rate;
                    lscurrency_discountamount = lsDiscount_Amount * lsexchange_rate;
                    lscurrency_taxamount1 = lsTax_Amount * lsexchange_rate;

                    mssql = " select a.purchaseorderdtl_gid,a.product_gid,a.uom_gid,a.product_price,a.qty_ordered,a.productuom_name," +
                            " a.discount_percentage,a.discount_amount,a.tax_percentage,a.tax_amount,a.tax_name,a.tax_amount2," +
                            " a.tax_percentage2,a.tax_name2,a.tax1_gid,a.tax2_gid,a.product_code,a.product_name,a.producttotal_amount," +
                            " i.invoice_amount,i.total from pmr_trn_tpurchaseorderdtl a " +
                            " left join pmr_trn_tinvoiceapprovaldtl i on a.purchaseorder_gid= i.purchaseorder_gid" +
                            "   where a.purchaseorder_gid = '" + values.purchaseorder_gid + "' group by purchaseorderdtl_gid ";
                    dt_datatable = objdbconn.GetDataTable(mssql);
                    foreach (DataRow Ri in dt_datatable.Rows)
                    {
                        string ipurchaseorderdtl_gid = Ri["purchaseorderdtl_gid"].ToString();
                        string iproduct_gid = Ri["product_gid"].ToString();
                        string iuom_gid = Ri["uom_gid"].ToString();
                        string iproduct_price = Ri["product_price"].ToString();
                        string iqty_ordered = Ri["qty_ordered"].ToString();
                        string idiscount_percentage = Ri["discount_percentage"].ToString();
                        string idiscount_amount = Ri["discount_amount"].ToString();
                        string itax_percentage = Ri["tax_percentage"].ToString();
                        string itax_amount = Ri["tax_amount"].ToString();
                        string itax_name = Ri["tax_name"].ToString();
                        string itax_amount2 = Ri["tax_amount2"].ToString();
                        string itax_percentage2 = Ri["tax_percentage2"].ToString();
                        string itax_name2 = Ri["tax_name2"].ToString();
                        string itax1_gid = Ri["tax1_gid"].ToString();
                        string itax2_gid = Ri["tax2_gid"].ToString();
                        string iproduct_code = Ri["product_code"].ToString();
                        string iproduct_name = Ri["product_name"].ToString();
                        string iproducttotal_amount = Ri["producttotal_amount"].ToString();
                        string iinvoice_amount = Ri["invoice_amount"].ToString();
                        string iproductuom_name = Ri["productuom_name"].ToString();
                        string itotal = Ri["total"].ToString();




                        msGetGID = objcmnfunction.GetMasterGID("SIVC");

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
                                   "'" + iproduct_gid + "', " +
                                   "'" + iuom_gid + "', " +
                                   "'" + iinvoice_amount + "', " +
                                   "'" + itotal + "', " +
                                   "'" + idiscount_percentage + "', " +
                                   "'" + idiscount_amount + "', " +
                                   "'" + itax_name + "', " +
                                   "'" + itax_name2 + "', " +
                                   "'" + lsTax_name3 + "', ";
                        if (itax_percentage != "")
                        {
                            mssql += "'0.00', ";
                        }
                        else
                        {
                            mssql += "'" + itax_percentage + "', ";
                        }
                        if (itax_percentage2 != "")
                        {
                            mssql += "'0.00', ";

                        }
                        else
                        {
                            mssql += "'" + itax_percentage2 + "', ";
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
                            mssql += "'" + itax_amount + "', ";
                        }
                        else
                        {
                            mssql += "'0.00', ";
                        }
                        if (itax_amount2 != "")
                        {
                            mssql += "'0.00', ";

                        }
                        else
                        {
                            mssql += "'" + itax_amount2 + "', ";
                        }
                        if (lsTax_Amount3 != "")
                        {
                            mssql += "'0.00', ";

                        }
                        else
                        {
                            mssql += "'" + lsTax_Amount3 + "', ";
                        }
                        mssql += "'" + itax1_gid + "'," +
                                   "'" + itax2_gid + "'," +
                                   "'" + lsTax3_gid + "'," +
                                   "'" + lsInvoiceqty_billed + "', " +
                                   "'" + values.product_remarks.Trim().Replace("'", "\\\'") + "'," +
                                   "'" + iproduct_name + "'," +
                                   "'" + lsproduct_total + "'," +
                                   "'" + idiscount_amount + "'," +
                                   "'" + itax_amount + "',";
                        if (itax_amount2 != "")
                        {
                            mssql += "'0.00', ";
                        }
                        else
                        {
                            mssql += "'" + itax_amount2 + "',";
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
                                    "'" + iproduct_code + "', " +
                                   "'" + iproduct_name + "', " +
                                    "'" + lsproductuom_code + "', " +
                                   "'" + iproductuom_name + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
                    }
                    mspGetGID = objcmnfunction.GetMasterGID("SPIP");
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
                                    " Set invoice_status = '" + lsinvoice_status + "'," +
                                    " invoice_flag = '" + lstGRN_IV_flag + "'" +
                                    " where grn_gid = '" + lsgrn_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
                    }
                }

                mssql = "select branch_gid from pmr_trn_tpurchaseorder where purchaseorder_gid='" + lspurchaseorder_gid + "'";
                odbcdr = objdbconn.GetDataReader(mssql);
                if (odbcdr.HasRows)
                {
                    lsbranch = odbcdr["branch_gid"].ToString();
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
                        lsinv_ref_no = objcmnfunction.GetMasterGID("PINV");
                    }
                }

                //int lspacking_charges = values.packing_charges;
                //int lscurrency_packing =  * lsexchange_rate;
                //string payment_date = values.payment_date;
                //DateTime parsedDate;
                //if (DateTime.TryParse(payment_date, out parsedDate))
                //{
                //    parsed_Date = parsedDate.ToString("yyyy-MM-dd");
                //}

                //string invoice_date = values.invoice_date;
                //DateTime Invoice_Date;
                //if (DateTime.TryParse(invoice_date, out Invoice_Date))
                //{
                //    invoice_Date = Invoice_Date.ToString("yyyy-MM-dd");
                //}
                //string uiDateStr2 = values.invoice_date;
                //DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //string Invoice_date = uiDate2.ToString("yyyy-MM-dd");
                mssql = " Update pmr_trn_tinvoiceapproval " +
                      " Set req_status = 'Invoice Approved'" +
                      " where purchaseorder_gid = '" + values.purchaseorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(mssql);


                mssql = " insert into acp_trn_tinvoice (" +
                        " invoice_gid, " +
                        " invoice_refno, " +
                        " invoice_reference, " +
                        " user_gid, " +
                        " invoice_date," +
                        " payment_date," +
                        " systemgenerated_amount, " +
                        " additionalcharges_amount, " +
                        " discount_amount, " +
                        " total_amount, " +
                        " invoice_amount, " +
                        " payment_term," +
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
                        " vendorinvoiceref_no," +
                        " round_off," +
                        " tax_gid," +
                         " taxsegment_gid," +
                         " taxsegmenttax_gid," +
                        " packing_charges," +
                        " insurance_charges " +
                        " ) values (" +
                        "'" + msINGetGID + "'," +
                        "'" + lsinv_ref_no + "'," +
                        "'" + lsinv_ref_no + "'," +
                        "'" + user_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'" + total_amount + "'," +
                        "'" + freightcharges + "'," +
                        "'" + discount_amount + "'," +
                        "'" + total_amount + "'," +
                        "'" + total_amount + "',";
                        if (string.IsNullOrEmpty(values.payment_days))
                        {
                            mssql += "'0.00',";
                        }
                        else
                        {
                            mssql += "'" + values.payment_days + "',";
                        }

                        mssql += "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                "'" + vendor_gid + "'," +
                                "'" + "IV Completed" + "'," +
                                "'" + "Payment Pending" + "'," +
                                "'" + values.invoice_remarks + "', " +
                                "'" + lspurchaseorder_from + "'," +
                                "'" + addon_amount + "',";
                        if (string.IsNullOrEmpty(discount_amount))
                        {
                            mssql += "'0.00',";
                        }
                        else
                        {
                            mssql += "'" + discount_amount + "',";
                        }
                mssql += "'" + total_amount + "'," +
                        "'" + currency_code + "'," +
                        "'" + exchange_rate + "'," +
                        "'" + freightcharges + "'," +
                        "'0.00'," +
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
                mssql += "'" + tax_name + "'," +
                        "'" + tax_percentage + "'," +
                        "'" + tax_amount + "'," +
                        "'" + branch_name + "'," +
                        "'" + values.Vendor_ref_no + "'," +
                        "'" + roundoff + "'," +
                        "'" + tax_gid + "'," +
                        "'" + values.taxsegment_gid + "'," +
                         "'" + values.taxsegmenttax_gid + "'," +
                        "'" + packing_charges + "'," +
                        "'" + insurance_charges + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(mssql);

            }
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Invoice Confirmed";
            }
            else
            {
                values.status = false;
                values.message = "Error occured while Invoice Confirmed";
            }
        }


        public void DaGetPurchaseServiceInvoicesummary(string purchaseorder_gid, MdlPblInvoiceGrnDetails values)
        {
            try
            {

                mssql = " select a.po_type,x.renewal_flag,x.frequency_term,date_format(x.renewal_date,'%d-%m-%Y') as renewal_date," +
                        " a.file_path,a.file_name,b.vendor_companyname,b.address_gid,a.shipping_address,c.branch_name,d.tax_name," +
                        " a.purchaseorder_gid,a.branch_gid,a.vendor_gid,  a.vendor_address,a.mode_despatch,a.poref_no," +
                        " a.termsandconditions,a.discount_amount,a.tax_percentage,a.tax_amount,a.addon_amount,g.currencyexchange_gid," +
                        " a.currency_code,a.exchange_rate,a.freightcharges,a.tax_gid,a.roundoff,a.freight_terms,a.netamount," +
                        " a.requested_by,e.user_firstname,b.email_id,b.contactperson_name, a.requested_details,a.po_covernote ,a.total_amount," +
                        " date_format(a.purchaseorder_date,'%d-%m-%Y') as purchaseorder_date,date_format(a.expected_date,'%d-%m-%Y') as expected_date," +
                        " a.payment_terms,f.address2,b.gst_no,b.contact_telephonenumber , CONCAT_WS(' ', f.address1,'\n', f.address2," +
                        " '\n', f.city,'\n',f.postal_code) AS bill_to, CONCAT_WS('\n', c.address1, c.city,c.state, c.postal_code) AS " +
                        " shipping_address from pmr_trn_tpurchaseorder a left join acp_mst_tvendor b on a.vendor_gid=b.vendor_gid  " +
                        " left join hrm_mst_tbranch c on a.branch_gid=c.branch_gid   left join acp_mst_ttax d on a.tax_gid= d.tax_gid " +
                        " left join adm_mst_taddress f on b.address_gid = f.address_gid  left join adm_mst_tuser e on " +
                        " a.requested_by = e.user_gid   left join pmr_trn_trenewal x on a.purchaseorder_gid = x.purchaseorder_gid  " +
                        " left join crm_trn_tcurrencyexchange g on a.currency_code = g.currency_code  " +
                        " where a.purchaseorder_gid = '" + purchaseorder_gid + "'";
                dt_datatable = objdbconn.GetDataTable(mssql);
                var getModuleList = new List<GetinviocePO>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetinviocePO
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
                            requested_by = dt["requested_by"].ToString(),
                            requested_details = dt["requested_details"].ToString(),
                            delivery_terms = dt["freight_terms"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            mode_despatch = dt["mode_despatch"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            po_covernote = dt["po_covernote"].ToString(),
                            netamount = dt["netamount"].ToString(),
                            //total_amount_L = dt["total_amount_L"].ToString(),
                            addon_amount = dt["addon_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            freightcharges = dt["freightcharges"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            additional_discount = dt["discount_amount"].ToString(),
                            shipping_address = dt["shipping_address"].ToString(),
                            bill_to = dt["bill_to"].ToString(),
                            po_type = dt["po_type"].ToString(),
                            overalltax = dt["tax_amount"].ToString(),
                            tax_prefix = dt["tax_name"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            
                        });

                        values.GetinviocePO = getModuleList;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO summary!";
                objcmnfunction.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              mssql + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

         public void DaGetPurchaseServiceInvoiceproduct(string purchaseorder_gid, MdlPblInvoiceGrnDetails values)
        {
            try
            {
                //double grand_total = 0.00;
                mssql = "select c.productgroup_name, a.purchaseorderdtl_gid, a.purchaseorder_gid, a.qty_ordered,a.product_gid,a.product_code," +
                        " a.product_name,a.productuom_name,a.product_price,a.display_field_name, " +
                        " a.discount_percentage,a.discount_amount,a.tax_name,a.tax_name2,a.tax_percentage, " +
                        " a.tax_percentage2,a.tax_amount2,a.tax_amount,a.tax_amount1_L,a.tax_amount2_L,  " +
                        " a.tax1_gid,a.tax2_gid,a.producttotal_amount from pmr_trn_tpurchaseorderdtl a " +
                        " left join  pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join  pmr_mst_tproductgroup c on c.productgroup_gid=b.productgroup_gid " +
                        " WHERE purchaseorder_gid = '" + purchaseorder_gid + "'";
                dt_datatable = objdbconn.GetDataTable(mssql);
                var getModuleList = new List<GetinvioceProduct>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        mssql = "select  SUM(a.producttotal_amount) as producttotal_amount from pmr_trn_tpurchaseorderdtl a " +
                            " left join pmr_mst_tproduct b on a.product_gid = b.product_gid  left join pmr_mst_tproductgroup c " +
                            " on c.productgroup_gid = b.productgroup_gid  WHERE  purchaseorder_gid = '" + purchaseorder_gid + "'";
                       string  lstotal = objdbconn.GetExecuteScalar(mssql);
                        //grand_total += double.Parse(dt["product_price"].ToString()) * double.Parse(dt["qty_ordered"].ToString())- double.Parse(dt["discount_percentage"].ToString());

                        getModuleList.Add(new GetinvioceProduct
                        {

                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field_name = dt["display_field_name"].ToString(),
                            qyt_unit = dt["qty_ordered"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount1 = dt["discount_amount"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            product_total = dt["producttotal_amount"].ToString(),
                            product_price_L = dt["product_price"].ToString(),
                            netamount= lstotal,
                        });

                        values.GetinvioceProduct = getModuleList;
                        //values.grand_total = grand_total;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting PO summary!";
                objcmnfunction.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              mssql + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPostOverAllServiceSubmit(string user_gid, string employee_gid, OverallSubmit_list values)
        {
            msINGetGID = objcmnfunction.GetMasterGID("SIVP");
            //mssql = " select  i.tax1_gid as producttax, i.tax2_gid as producttax2, concat(i.qty_ordered,' ',i.productuom_name) as qyt_unit ,n.qty_delivered, format(i.product_price_L,2) as product_price_L, format(a.addon_amount, 2) as addon_amount," +
            //            " format(a.roundoff, 2) as roundoff,  concat_ws('-', h.costcenter_name, h.costcenter_gid) as costcenter_name,format(h.budget_available, 2) as budget_available," +
            //            " h.costcenter_gid,  a.payment_days,a.tax_gid,a.delivery_days,format(a.freightcharges,2) as freightcharges,a.buybackorscrap,a.manualporef_no,  " +
            //            " format(a.packing_charges, 2) as packing_charges, format(a.insurance_charges, 2) as insurance_charges ,format(a.discount_amount,2) as additional_discount,  " +
            //            " i.purchaseorderdtl_gid, i.product_gid,  i.product_price, i.qty_ordered,i.needby_date, format(i.discount_percentage, 2) as discount_percentage ,  " +
            //            " format(i.discount_amount, 2) discount_amount1 ,  format(i.tax_percentage, 2) as tax_percentage,  format(i.tax_percentage2, 2) as tax_percentage2,  " +
            //            " format(i.tax_percentage3, 2) as tax_percentage3,  format(i.tax_amount, 2) as tax_amount,  format(i.tax_amount2, 2) as tax_amount2, " +
            //            " format(i.tax_amount3, 2) as tax_amount3,i.taxseg_taxname1,i.taxseg_taxpercent1,format(i.taxseg_taxamount1,2) AS taxseg_taxamount1," +
            //            " i.taxseg_taxname2,i.taxseg_taxpercent2,format(i.taxseg_taxamount2,2) AS taxseg_taxamount2,  i.qty_Received, i.qty_grnadjusted, " +
            //            " i.taxseg_taxname3,i.taxseg_taxpercent3,format(i.taxseg_taxamount3,2) as taxseg_taxamount3,  i.product_remarks, " +
            //            " format((qty_ordered * i.product_price), 2) as product_totalprice,  format((((qty_ordered * i.product_price) - i.discount_amount) + i.tax_amount + i.tax_amount2 + i.tax_amount3), 2)  as producttotal_amount," +
            //            " i.product_code, (i.product_name) as product_name,format(a.netamount, 2) as netamount, k.productgroup_name, i.productuom_name," +
            //            " i.purchaseorder_gid,i.display_field_name, a.currency_code,a.poref_no,i.tax1_gid,y.tax_name as overalltaxname, " +
            //            " FORMAT(i.tax_amount + i.tax_amount2, 2) AS overall_tax ,CONCAT(i.tax_name, ' ', i.tax_percentage, ' , ', i.tax_name2, ' ', i.tax_percentage2) AS taxesname ," +
            //            " concat(i.tax_amount,', ',i.tax_amount2) as taxesamt," +
            //            " i.discount_amount,l.productuom_code, a.exchange_rate,i.uom_gid,i.taxsegment_gid,i.tax_name,i.tax_name2,a.taxsegmenttax_gid,a.mode_despatch,p.grndtl_gid,o.grn_gid,a.purchaseorder_from from pmr_trn_tpurchaseorder a  " +
            //            " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid  " +
            //            " left join adm_mst_taddress w on b.address_gid = w.address_gid  " +
            //            " left join adm_mst_tuser c on c.user_gid = a.created_by  " +
            //            " left join hrm_mst_temployee d on d.user_gid = c.user_gid  " +
            //            " left join hrm_mst_tdepartment e on e.department_gid = d.department_gid  " +
            //            " left join hrm_mst_tbranch f on a.branch_gid = f.branch_gid  " +
            //            " left join adm_mst_tuser g on g.user_gid = a.approved_by  " +
            //            " left join pmr_mst_tcostcenter h on h.costcenter_gid = a.costcenter_gid  " +
            //            " left join pmr_trn_tpurchaseorderdtl i ON a.purchaseorder_gid = i.purchaseorder_gid  " +
            //            " left join acp_mst_ttax y on y.tax_gid = a.tax_gid  " +
            //            " left join pmr_mst_tproduct j on i.product_gid = j.product_gid  " +
            //            " left join pmr_mst_tproductgroup k on j.productgroup_gid = k.productgroup_gid  " +
            //            " left join pmr_mst_tproductuom l on i.uom_gid = l.productuom_gid  " +
            //            " left join adm_mst_tuser m on m.user_gid = a.requested_by " +
            //            "  left join pmr_trn_tgrndtl n on n.purchaseorderdtl_gid = i.purchaseorderdtl_gid " +
            //            " left join pmr_trn_tgrn o on o.grn_gid = n.grn_gid " +
            //            "  left join pmr_trn_tgrndtl p on o.grn_gid=p.grn_gid" +
            //            " where a.purchaseorder_gid = '" + values.purchaseorder_gid + "' " +
            //            " and i.purchaseorderdtl_gid in (select a.purchaseorderdtl_gid from pmr_trn_tgrndtl a " +
            //            " left join pmr_trn_tpurchaseorderdtl i ON a.purchaseorderdtl_gid = i.purchaseorderdtl_gid " +
            //            " where i.purchaseorder_gid = '" + values.purchaseorder_gid + "') group by purchaseorderdtl_gid ";
            //dt_datatable = objdbconn.GetDataTable(mssql);
            //foreach (DataRow Pr in dt_datatable.Rows)
            //{
            //    string taxsegment_gid = Pr["taxsegment_gid"].ToString();
            //    string taxsegmenttax_gid = Pr["taxsegmenttax_gid"].ToString();
            //    lsproduct_gid = Pr["product_gid"].ToString();
            //    lsuom_gid = Pr["uom_gid"].ToString();
            //    lsmode_despatch = Pr["mode_despatch"].ToString();
            //    lsproductgroup_name = Pr["productgroup_name"].ToString();
            //    decimal product_price, lsproduct_price;
            //    if (decimal.TryParse(Pr["product_price"]?.ToString(), out product_price))
            //    {
            //        lsproduct_price = product_price;
            //    }
            //    else
            //    {
            //        lsproduct_price = 0;
            //    }
            //    decimal lsproduct_total, product_total;
            //    if (decimal.TryParse(Pr["producttotal_amount"]?.ToString()?.Replace(",", ""), out product_total))
            //    {
            //        lsproduct_total = product_total;
            //    }
            //    else
            //    {
            //        lsproduct_total = 0;
            //    }

            //    decimal lsInvoiceqty_billed, Invoiceqty_billed;
            //    if (decimal.TryParse(Pr["qty_delivered"]?.ToString(), out Invoiceqty_billed))
            //    {
            //        lsInvoiceqty_billed = Invoiceqty_billed;
            //    }
            //    else
            //    {
            //        lsInvoiceqty_billed = 0;
            //    }
            //    decimal lsproducttotal_amount, producttotal_amount;
            //    if (decimal.TryParse(Pr["producttotal_amount"]?.ToString(), out producttotal_amount))
            //    {
            //        lsproducttotal_amount = producttotal_amount;
            //    }
            //    else
            //    {
            //        lsproducttotal_amount = 0;
            //    }

            //    lsDiscount_Percentage = Pr["discount_percentage"].ToString();
            //    lsTax_name = Pr["tax_name"].ToString();
            //    lsTax_name2 = Pr["tax_name2"].ToString();
            //    lsproduct_code = Pr["product_code"].ToString();
            //    lsproduct_name = Pr["product_name"].ToString();
            //    lsTax1_gid = Pr["producttax"].ToString();
            //    lsTax2_gid = Pr["producttax2"].ToString();
            //    lsproductuom_name = Pr["productuom_name"].ToString();

            //    lsproductuom_code = Pr["productuom_code"].ToString();
            //    lsgrndtl_gid = Pr["grndtl_gid"].ToString();
            //    lsgrn_gid = Pr["grn_gid"].ToString();
            //    lspurchaseorderdtl_gid = Pr["purchaseorderdtl_gid"].ToString();
            //    lspurchaseorder_gid = Pr["purchaseorder_gid"].ToString();
            //    lspurchaseorder_from = Pr["purchaseorder_from"].ToString();
            //    lsTax_Percentage = Pr["tax_percentage"].ToString();
            //    lsTax_Percentage2 = Pr["tax_percentage2"].ToString();
            //    lsTax_Percentage3 = Pr["tax_percentage3"].ToString();
            //    if (decimal.TryParse(Pr["exchange_rate"].ToString(), out decimal exchangeRate))
            //    {
            //        lsexchange_rate = exchangeRate;
            //    }
            //    else
            //    {
            //        lsexchange_rate = 0;
            //    }
            //    if (decimal.TryParse(Pr["tax_amount"].ToString(), out decimal taxAmount))
            //    {
            //        lsTax_Amount = taxAmount;
            //    }

            //    else
            //    {

            //        lsTax_Amount = 0;
            //    }
            //    if (decimal.TryParse(Pr["tax_amount2"].ToString(), out decimal taxAmount2))
            //    {
            //        lsTax_Amount2 = taxAmount2;
            //    }

            //    else
            //    {

            //        lsTax_Amount2 = 0;
            //    }
            //    if (decimal.TryParse(Pr["discount_amount"].ToString(), out decimal discountamount))
            //    {
            //        lsDiscount_Amount = discountamount;
            //    }
            //    else
            //    {
            //        lsDiscount_Amount = 0;
            //    }

            //    lscurrency_unitprice = lsproduct_price * lsexchange_rate;
            //    lscurrency_discountamount = lsDiscount_Amount * lsexchange_rate;
            //    lscurrency_taxamount1 = lsTax_Amount * lsexchange_rate;
           
            mssql = "select c.productgroup_name, a.purchaseorderdtl_gid, a.purchaseorder_gid, a.qty_ordered,a.product_gid,a.product_code," +
                        " a.product_name,a.productuom_name,a.product_price,a.display_field_name, " +
                        " a.discount_percentage,a.discount_amount,a.tax_name,a.tax_name2,a.tax_percentage, " +
                        " a.tax_percentage2,a.tax_amount2,a.tax_amount,a.tax_amount1_L,a.tax_amount2_L,  " +
                        " a.tax1_gid,a.tax2_gid,a.producttotal_amount from pmr_trn_tpurchaseorderdtl a " +
                        " left join  pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join  pmr_mst_tproductgroup c on c.productgroup_gid=b.productgroup_gid " +
                        " WHERE purchaseorder_gid = '" + values.purchaseorder_gid + "'";
            dt_datatable = objdbconn.GetDataTable(mssql);
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {

                    msGetGID = objcmnfunction.GetMasterGID("SIVC");

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
                               //" taxsegment_gid," +
                               //" taxsegmenttax_gid," +
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
                                "'" + dt["product_gid"].ToString() + "', " +
                               "'" + lsuom_gid + "', " +
                                  "'" + dt["product_price"].ToString() + "', " +
                               "'" + dt["producttotal_amount"].ToString() + "', " +
                                "'" + dt["discount_percentage"].ToString() + "', " +
                                "'" + dt["discount_amount"].ToString() + "', " +
                               "'" + dt["tax_name"].ToString() + "', " +
                               "'" + dt["tax_name2"].ToString() + "', " +
                               "'" + lsTax_name3 + "', ";
                    if (dt["tax_percentage"].ToString() != "0.00")
                    {
                        mssql += "'" + dt["tax_percentage"].ToString() + "', ";
                    }
                    else
                    {
                        mssql += "'0.00', ";
                    }
                    if (dt["tax_percentage2"].ToString() != "0.00")
                    {
                        mssql += "'" + dt["tax_percentage2"].ToString() + "', ";
                    }
                    else
                    {
                        mssql += "'0.00', ";
                    }
                    if (lsTax_Percentage3 != "0.00")
                    {
                        mssql += "'0.00', ";

                    }
                    else
                    {
                        mssql += "'0.00', ";
                    }
                    if (dt["tax_amount"].ToString() !="0")
                    {
                        mssql += "'" + dt["tax_amount"].ToString() + "', ";
                    }
                    else
                    {
                        mssql += "'0.00', ";
                    }
                    if (dt["tax_amount2"].ToString() != "0")
                    {
                        mssql += "'" + dt["tax_amount2"].ToString() + "', ";

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
                    mssql += "'" + dt["tax1_gid"].ToString() + "'," +
                               "'" + dt["tax2_gid"].ToString() + "'," +
                               "'" + lsTax3_gid + "'," +
                               "'" + dt["qty_ordered"].ToString() + "', " +
                               "'" + dt["display_field_name"].ToString().Replace("'", "\\\'") + "', " +
                               "'" + dt["product_price"].ToString() + "'," +
                               "'" + dt["producttotal_amount"].ToString() + "'," +
                               "'" + dt["discount_amount"].ToString() + "'," +
                               "'" + dt["tax_amount"].ToString() + "',";
                    if (dt["tax_amount2"].ToString() != "")
                    {
                        mssql += "'0.00', ";
                    }
                    else
                    {
                        mssql += "'" + dt["tax_amount2"].ToString() + "',";
                    }
                    if (lscurrency_taxamount3 != "")
                    {
                        mssql += "'0.00', ";
                    }
                    else
                    {
                        mssql += "'" + lscurrency_taxamount3 + "',";
                    }

                    //mssql += "'" + taxsegment_gid + "', " +
                    //           "'" + taxsegmenttax_gid + "', " +
                    mssql += "'" + lsproductgroup_code + "', " +
                                   "'" + dt["productgroup_name"].ToString().Replace("'", "\\\'") + "', " +
                                    "'" + lsproduct_code + "', " +
                                   "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "', " +
                                    "'" + lsproductuom_code + "', " +
                                   "'" + dt["productuom_name"].ToString().Replace("'", "\\\'") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
                }
            }

            //mspGetGID = objcmnfunction.GetMasterGID("SPIP");
            //mssql = " insert into acp_trn_tpo2invoice (" +
            //            " po2invoice_gid, " +
            //            " invoice_gid, " +
            //            " invoicedtl_gid, " +
            //            " grn_gid, " +
            //            " grndtl_gid, " +
            //            " purchaseorder_gid, " +
            //            " purchaseorderdtl_gid, " +
            //            " product_gid, " +
            //            " qty_invoice, " +
            //            " display_field_name)" +
            //            " values ( " +
            //            "'" + mspGetGID + "'," +
            //            "'" + msINGetGID + "'," +
            //            "'" + msGetGID + "'," +
            //            "'" + lsgrn_gid + "'," +
            //            "'" + lsgrndtl_gid + "', " +
            //            "'" + lspurchaseorder_gid + "'," +
            //            "'" + lspurchaseorderdtl_gid + "'," +
            //            "'" + lsproduct_gid + "'," +
            //            "'" + lsInvoiceqty_billed + "'," +
            //            "'" + lsproduct_name + "')";
            //mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
            lspurchaseorder_from = "Purchase";

                if (lspurchaseorder_from == "Purchase")
                {
                    decimal lsPO_Sum = lsInvoiceqty_billed;

                    decimal lspo_sumdec = lsInvoiceqty_billed;

                    mssql = " select qty_invoice from pmr_trn_tpurchaseorderdtl  where " +
                            " purchaseorderdtl_gid = '" + values.purchaseorder_gid + "'";
                    odbcdr = objdbconn.GetDataReader(mssql);
                    if (odbcdr.HasRows)
                    {
                        odbcdr.Read();
                        lsqty_invoice = Convert.ToInt32(odbcdr["qty_invoice"].ToString());
                        lsPO_Sum = lsPO_Sum += lsqty_invoice;
                        odbcdr.Close();
                    }

                    mssql = " Update pmr_trn_tpurchaseorderdtl " +
                                                " Set qty_invoice = '" + lsPO_Sum + "'" +
                                                " where purchaseorderdtl_gid = '" + values.purchaseorder_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(mssql);


                    mssql = " select qty_invoice, qty_ordered " +
                                " from pmr_trn_tpurchaseorderdtl  where " +
                                " purchaseorder_gid = '" + values.purchaseorder_gid + "'and" +
                                " qty_invoice < qty_ordered ";
                    odbcdr = objdbconn.GetDataReader(mssql);
                    odbcdr.Read();
                    if (odbcdr.HasRows)
                    {

                        lstPO_IV_flag = "Invoice Raised";

                    }
                    else
                    {
                        lstPO_IV_flag = "Invoice Raised";
                    }
                    odbcdr.Close();
                    mssql = " Update pmr_trn_tpurchaseorder " +
                               " Set invoice_flag = '" + lstPO_IV_flag + "'" +
                               " where purchaseorder_gid = '" + values.purchaseorder_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(mssql);

                    lsGRN_Sum = lsInvoiceqty_billed;


                    //mssql = " select qty_invoice from pmr_trn_tgrndtl  where " +
                    //           " grndtl_gid = '" + lsgrndtl_gid + "'";
                    //odbcdr = objdbconn.GetDataReader(mssql);
                    //if (odbcdr.HasRows)
                    //{
                    //    odbcdr.Read();
                    //    if (decimal.TryParse(odbcdr["qty_invoice"].ToString(), out decimal qtyinvoice))
                    //    {
                    //        delsqty_invoice = qtyinvoice;
                    //    }
                    //    else
                    //    {
                    //        delsqty_invoice = 0;
                    //    }
                    //    lsGRN_Sum = lsGRN_Sum += delsqty_invoice;
                    //    odbcdr.Close();
                    //}

                    //mssql = " Update pmr_trn_tgrndtl " +
                    //           " Set qty_invoice = '" + lsGRN_Sum + "'" +
                    //           " where grndtl_gid = '" + lsgrndtl_gid + "'";
                    //mnResult = objdbconn.ExecuteNonQuerySQL(mssql);


                    //mssql = " select qty_delivered, qty_rejected, qty_invoice " +
                    //            " from pmr_trn_tgrndtl  where " +
                    //            " grn_gid = '" + lsgrn_gid + "'and" +
                    //            " qty_invoice < (qty_delivered - qty_rejected) ";
                    //odbcdr = objdbconn.GetDataReader(mssql);
                    //if (odbcdr.HasRows)
                    //{
                    //    odbcdr.Read();
                    //    lsinvoice_status = "IV Work In Progress";
                    //    lstGRN_IV_flag = "Invoice Raised Partial";
                    //    odbcdr.Close();
                    //}
                    //else
                    //{
                    //    odbcdr.Read();
                    //    lsinvoice_status = "IV Completed";
                    //    lstGRN_IV_flag = "Invoice Raised";
                    //    odbcdr.Close();
                    //}

                    //mssql = " Update pmr_trn_tgrn " +
                    //            " Set invoice_status = 'IV Completed'," +
                    //            " invoice_flag = 'Invoice Raised'" +
                    //            " where grn_gid = '" + lsgrn_gid + "'";
                    //mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
                }
            //}

            mssql = "select vendor_address,branch_gid,vendor_gid,vendor_contact_person from pmr_trn_tpurchaseorder where purchaseorder_gid='" + values.purchaseorder_gid + "'";
            odbcdr = objdbconn.GetDataReader(mssql);
            if (odbcdr.HasRows)
            {
                odbcdr.Read();
                lsvendoraddress = odbcdr["vendor_address"].ToString();
                lsbranch = odbcdr["branch_gid"].ToString();
                lsvendor_gid = odbcdr["vendor_gid"].ToString();
                lsvendor_contact = odbcdr["vendor_contact_person"].ToString();
                odbcdr.Close();
            }
            //mssql = " select invoiceref_flag from pbl_mst_tconfiguration ";
            //odbcdr = objdbconn.GetDataReader(mssql);
            //if(odbcdr.HasRows)
            //{
            //    odbcdr.Read();
            //    lsinvoiceref_flag = odbcdr["invoiceref_flag"].ToString();
            //    if(lsinvoiceref_flag == "Y")
            //    {
            //        lsinv_ref_no = values.inv_ref_no;
            //    }
            //    else
            //    {
            //        lsinv_ref_no = objcmnfunction.GetMasterGID("PINV");
            //        odbcdr.Close();
            //    }
            //}

            //string ls_referenceno;

            //if (values.inv_ref_no == "")
            //{
            //    ls_referenceno = objcmnfunction.getSequencecustomizerGID("PINV", "PBL", values.branch_name);
            //}
            //else
            //{

            //    ls_referenceno = values.inv_ref_no;
            //}
            mssql = "select branch_gid from hrm_mst_tbranch where branch_name ='" + values.branch_name + "'";
            lsbranchname = objdbconn.GetExecuteScalar(mssql);


            if (values.inv_ref_no == "")
            {
                ls_referenceno = objcmnfunction.getSequencecustomizerGID("PINV", "PBL", lsbranchname);
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
                        objcmnfunction.Tracelog(msGetGid, user_gid, trace_comment, "added_vendor");

                    }
                }
                odbcdr.Close();
            }

            //string msAccGetGID = objcmnfunction.GetMasterGID("FCOA");

            //mssql = " insert into acc_mst_tchartofaccount( " +
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
            //mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
            //if (mnResult == 1)
            //{
            //    mssql = " update acp_mst_tvendor set " +
            //            " account_gid = '" + msAccGetGID + "'" +
            //            " where vendor_gid='" + vendor_gid + "'";
            //    mnResult = objdbconn.ExecuteNonQuerySQL(mssql);
            //}


            //int lspacking_charges = values.packing_charges;
            //int lscurrency_packing =  * lsexchange_rate;
            string payment_date = values.payment_date;
            DateTime parsedDate;
            if (DateTime.TryParse(payment_date, out parsedDate))
            {
                parsed_Date = parsedDate.ToString("yyyy-MM-dd");
            }

            //string invoice_date = values.invoice_date;
            //DateTime Invoice_Date;
            //if (DateTime.TryParse(invoice_date, out Invoice_Date))
            //{
            //   invoice_Date = Invoice_Date.ToString("yyyy-MM-dd");
            //}
            string invoice_date = values.invoice_date;
            if (!string.IsNullOrEmpty(invoice_date))
            {
                DateTime uiDate = DateTime.ParseExact(invoice_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                invoice_Date = uiDate.ToString("yyyy-MM-dd");
            }
            else
            {
                invoice_Date = DateTime.Now.ToString("yyyy-MM-dd");
            }




            //string uiDateStr2 = values.invoice_date;
            //DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            //string Invoice_date = uiDate2.ToString("yyyy-MM-dd");


            mssql = "select tax_prefix from acp_mst_ttax where tax_gid ='" + values.tax_name4 + "' ";
            string lstax = objdbconn.GetExecuteScalar(mssql);

            mssql = " insert into acp_trn_tinvoice (" +
                    " invoice_gid, " +
                    " invoice_refno, " +
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
                    "'" + ls_referenceno + "'," +
                    "'" + values.inv_ref_no + "'," +
                    "'" + values.inv_ref_no + "'," +
                    "'" + values.shipping_address + "'," +
                    "'" + values.dispatch_mode + "'," +
                    "'" + values.billing_email + "'," +
                    "'" + values.contactperson_name + "'," +
                    "'" + values.address1 + "'," +
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
            "'" + "Invoice Approved" + "'," +
            "'" + "Payment Pending" + "'," +
            "'" + values.invoice_remarks.Replace("'", "\\\'") + "', " +
            "'Purchase'," +
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
            mssql += "'" + lstax + "'," +
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

            mssql += "'" + values.tax_name4 + "'," +
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
                     "'" + values.template_content.Replace("'", "\\\'") + "'," +
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
                     values.purchase_type, fin_roundoff, fin_freightcharges, fin_buybackCharges, lstax, fin_overalltax_amount, fin_forwardingCharges, fin_insuranceCharges);



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

                            // Tax 3 Calculation
                            msSQL = "SELECT SUM(tax_amount3_L) AS tax3 FROM acp_trn_tinvoicedtl " +
                                    "WHERE invoice_gid = '" + msINGetGID + "' AND tax3_gid = '" + objODBCDataReader["tax_gid"] + "'";
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

                                objfincmn.jn_purchase_tax(msINGetGID, ls_referenceno, values.invoice_remarks, lstaxamount, lstax_gid);
                            }
                        }
                    }

                    objdbconn.CloseConn();
                }


                values.status = true;
                values.message = "Invoice Confirmed Succesfully !";
            }
            else
            {
                values.status = false;
                values.message = "Error occured while Invoice Confirmed";
            }
        }



    }
}