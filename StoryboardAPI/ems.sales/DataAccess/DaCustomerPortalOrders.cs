using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ems.sales.DataAccess
{
    public class DaCustomerPortalOrders
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msSalesGid, mssalesorderGID1, msGetGid = string.Empty;
        string start_date, end_date;
        DataTable dt_datatable;
        OdbcDataReader objOdbcDataReader;
        int mnResult;
        public void DaGetCustomerPortalSalesordersummary(MdlCustomerPortalOrders values)
        {
            try
            {
                msSQL = " select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                string currency = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select distinct a.salesorder_gid,a.source_flag, a.so_referenceno1,a.customer_gid,a.mintsoftid," +
                        " DATE_FORMAT(a.salesorder_date, '%d-%m-%Y') as salesorder_date,c.user_firstname as created_by,a.so_type,a.currency_code," +
                        " a.customer_contact_person, a.salesorder_status,a.currency_code,s.source_name,d.customer_code,i.branch_name, " +
                        " format(a.Grandtotal,2) as Grandtotal, concat(k.user_firstname, ' ', k.user_lastname) AS salesperson_name,  " +
                        " CONCAT(d.customer_id, ' / ', d.customer_name) AS customerdetails," +
                        " case when a.currency_code = '" + currency + "' then a.customer_name " +
                        " when a.currency_code is null then a.customer_name " +
                        " when a.currency_code is not null and a.currency_code <> '" + currency + "' then (a.customer_name) end as customer_name, " +
                        " case when a.customer_email is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email) " +
                        " when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact,a.invoice_flag " +
                        " from cst_trn_tsalesorder a " +
                        " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid" +
                        "  left join cst_trn_tsalesorderdtl  x on a.salesorder_gid=x.salesorder_gid" +
                        " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid" +
                        " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                        " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                        " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                        " left join hrm_mst_tbranch i on a.branch_gid= i.branch_gid " +
                        " LEFT JOIN adm_mst_tuser k ON a.salesperson_gid = k.user_gid " +
                        " left join crm_trn_tleadbank l on l.customer_gid=a.customer_gid " +
                        " left join crm_mst_tsource s on s.source_gid=l.source_gid " +
                        "  where 1=1 and a.salesorder_status ='Pending' order by a.salesorder_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<CustomerPortalSalesOrder_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new CustomerPortalSalesOrder_list
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            so_type = dt["so_type"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            user_firstname = dt["created_by"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
                            mintsoftid = dt["mintsoftid"].ToString(),
                            salesperson_name = dt["salesperson_name"].ToString(),
                            customer_code = dt["customer_code"].ToString(),
                            customerdetails = dt["customerdetails"].ToString(),
                            source_flag = dt["source_flag"].ToString(),

                        });
                        values.CustomerPortalSalesOrder_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetPortalCustomerViewsalesorderSummary(string salesorder_gid, MdlCustomerPortalOrders values)
        {
            try
            {
                msSQL = " select a.salesorder_gid as serviceorder_gid,d.currencyexchange_gid,b.customer_id," +
                "a.branch_gid, f.branch_name,a.payment_days,a.delivery_days, concat(a.so_referenceno1,case when so_referencenumber='' then '' else concat(' ','-',' '," +
                " case when so_referencenumber is not null then so_referenceno1 else '' end) end )as so_reference," +
                " DATE_format(a.salesorder_date, '%d-%m-%Y') as serviceorder_date, " +
                " concat(a.customer_name,'/',c.email) as customer_name,b.customer_gid,format(a.grandtotal, 2) as grand_total,a.shipping_to," +
                " a.customer_contact_person as customercontact_name,c.email as email,b.customer_code,a.order_instruction," +
                " a.billing_email,a.customer_email, a.customer_contact_person," +
                " a.termsandconditions,c.mobile,b.gst_number," +
                " a.addon_charge as addon_amount ,a.additional_discount as discount_amount," +
                " a.customer_address,format(total_amount,2) as order_total ," +
                " format(a.freight_charges,2)as freight_charges," +
                " format(a.buyback_charges,2)as buyback_charges," +
                " format(a.packing_charges,2)as packing_charges," +
                " format(a.insurance_charges,2)as insurance_charges," +
                "a.tax_name,a.tax_gid,a.roundoff,a.tax_amount,a.tax_name4," +
                " a.currency_code, a.currency_gid, a.exchange_rate, c.address1,c.address2,c.city," +
                " c.state,c.country_gid,c.zip_code,e.country_name " +
                " from cst_trn_tsalesorder a" +
                " left join crm_mst_tcustomer b on a.customer_gid=b.customer_gid " +
                " left join crm_mst_tcustomercontact c on b.customer_gid = c.customer_gid " +
                " left join crm_trn_tcurrencyexchange d on d.currency_code=a.currency_code" +
                " left join adm_mst_tcountry e on c.country_gid=e.country_gid " +
                " left join hrm_mst_tbranch f on f.branch_gid=a.branch_gid" +
                " where a.salesorder_gid='" + salesorder_gid + "'";
                var Getordertoinvoice = new List<PortalCustomerSalesorderView_list>();
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        Getordertoinvoice.Add(new PortalCustomerSalesorderView_list
                        {
                            salesorder_gid = dt["serviceorder_gid"].ToString(),
                            so_reference = dt["so_reference"].ToString(),
                            serviceorder_date = dt["serviceorder_date"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            grand_total = dt["grand_total"].ToString(),
                            customercontact_name = dt["customercontact_name"].ToString(),
                            customer_email = dt["customer_email"].ToString(),
                            customer_code = dt["customer_code"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            customer_mobile = dt["mobile"].ToString(),
                            addon_amount = dt["addon_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            shipping_to = dt["shipping_to"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            order_total = dt["order_total"].ToString(),
                            freight_charges = dt["freight_charges"].ToString(),
                            buyback_charges = dt["buyback_charges"].ToString(),
                            packing_charges = dt["packing_charges"].ToString(),
                            insurance_charges = dt["insurance_charges"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            customer_id = dt["customer_id"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            currency_gid = dt["currency_gid"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            zip_code = dt["zip_code"].ToString(),
                            country_gid = dt["country_gid"].ToString(),
                            state = dt["state"].ToString(),
                            city = dt["city"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            order_instruction = dt["order_instruction"].ToString(),
                            billing_email = dt["billing_email"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_name4 = dt["tax_name4"].ToString(),
                        });
                        values.PortalCustomerSalesorderView_list = Getordertoinvoice;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while Order to invoice summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                  msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                  DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetPortalCustomerViewsalesorderDetails(string salesorder_gid, string employee_gid, MdlCustomerPortalOrders values)
        {
            try
            {
               
                msSQL = " select a.salesorderdtl_gid as serviceorderdtl_gid,a.salesorder_gid," +
                " if(a.customerproduct_code='&nbsp;',' ',a.customerproduct_code) as customerproduct_code," +
                " a.product_gid,a.tax1_gid,a.tax2_gid,a.tax3_gid,format(a.qty_quoted,2) as qty_quoted1,a.qty_quoted,a.uom_gid,a.uom_name," +
                " format(a.vendor_price,2) as amount ,format(a.product_price,2) as productprice,a.product_price,a.price," +
                " format(a.tax_amount,2) as tax_amount1, " +
                " format(a.tax_amount2,2) as tax_amount2,format(a.tax_amount3,2) as tax_amount3," +
                " format(a.price,2) as total_amount,c.productgroup_gid, c.productgroup_name,a.product_code, " +
                " a.display_field as description,a.tax_name as tax_name1,a.tax_name2,a.tax_name3, " +
                " a.discount_amount as discount_amount,a.margin_percentage, " +
                " a.discount_percentage as discount_percentage,a.product_name, a.taxsegment_gid, " +
                "a.tax_percentage,a.tax_percentage2,a.tax_percentage3 , format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount" +
                " from cst_trn_tsalesorderdtl a " +
                " left join pmr_mst_tproduct b on b.product_gid=a.product_gid " +
                "left join pmr_mst_tproductgroup c on c.productgroup_gid=a.productgroup_gid" +
                " where a.salesorder_gid='" + salesorder_gid + "' and b.delete_flag='N' " +
                " group by b.product_gid,a.salesorderdtl_gid order by a.salesorderdtl_gid asc ";                
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<PortalCustomerSalesorderViewdetails_list>();              
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new PortalCustomerSalesorderViewdetails_list
                        {
                            serviceorderdtl_gid = dt["serviceorderdtl_gid"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            productprice = dt["productprice"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            price = dt["price"].ToString(),
                            tax_amount1 = dt["tax_amount1"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            tax_name1 = dt["tax_name1"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            taxamount = dt["taxamount"].ToString(),
                        });
                        values.PortalCustomerSalesorderViewdetails_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Summary !";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetTempProductSummary(string salesorder_gid, string employee_gid, MdlCustomerPortalOrders values)
        {
            try
            {
                double grand_total = 0.00;
                double grandtotal = 0.00;

                msSQL = "select a.tmpsalesorderdtl_gid,a.taxsegment_gid,a.salesorder_gid, a.tax_name,a.tax_amount,a.tax_percentage,a.tax_name2,a.tax_amount2,a.tax_percentage2,a.tax_name3,a.tax_amount3,a.tax_percentage3, p.customer_gid,a.tax_amount, a.salesorderdtl_gid, a.salesorder_gid, a.product_gid, a.productgroup_gid, " +
                   " v.productgroup_name, a.product_name, format(a.product_price, 2) as product_price, b.product_code, a.qty_quoted, a.product_remarks, " +
                   " a.uom_gid, a.vendor_gid, a.slno, a.uom_name, format(a.price, 2) as price, " +
                   " format(a.discount_percentage,2) as discount_percentage, format(a.discount_amount,2) as discount_amount, " +
                   " format(a.selling_price, '0.00') as selling_price,a.product_remarks, " +
                   " concat( case when a.tax_name is null then '' else a.tax_name end, ' '," +
                   " case when a.tax_percentage = '0' then '' else concat('',a.tax_percentage,'%') end , " +
                   " case when a.tax_amount = '0' then '' else concat(':',a.tax_amount) end) as tax, " +
                   " concat(case when a.tax_name2 is null then '' else a.tax_name2 end, ' ', " +
                   " case when a.tax_percentage2 = '0' then '' else concat('', a.tax_percentage2, '%') end, " +
                   " case when a.tax_amount2 = '0' then '' else concat(':', a.tax_amount2) end) as tax2, " +
                   " concat(  case when a.tax_name3 is null then '' else a.tax_name3 end, ' ', " +
                   " case when a.tax_percentage3 = '0' then '' else concat('', a.tax_percentage3, ' %   ')" +
                   " end, case when a.tax_amount3 = '0' then '  ' else concat(' : ', a.tax_amount3) end) as tax3" +
                   " , format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount " +
                   " FROM smr_tmp_tsalesorderdtl a " +                   
                   " left join crm_mst_tcustomer p on p.taxsegment_gid = a.taxsegment_gid " +
                   " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                   " left join pmr_mst_tproductgroup v on b.productgroup_gid=v.productgroup_gid " +
                   " left join acp_mst_tvendor e on a.vendor_gid = e.vendor_gid " +
                   " where a.salesorder_gid='" + salesorder_gid + "' and a.employee_gid='" + employee_gid + "' and b.delete_flag='N' order by (a.slno+0) asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<CustomerPortalGettemporarysummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["price"].ToString());
                        grandtotal += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new CustomerPortalGettemporarysummary
                        {
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            slno = dt["slno"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            discountpercentage = double.Parse(dt["discount_percentage"].ToString()),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            producttotalamount = dt["price"].ToString(),
                            totalamount = dt["price"].ToString(),
                            taxname1 = dt["tax_name"].ToString(),
                            taxname2 = dt["tax_name2"].ToString(),
                            taxname3 = dt["tax_name3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            grand_total = dt["price"].ToString(),
                            grandtotal = dt["price"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            tax = dt["tax"].ToString(),
                            tax2 = dt["tax2"].ToString(),
                            tax3 = dt["tax3"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            taxamount = dt["taxamount"].ToString(),
                        });
                    }
                    values.CustomerPortalGettemporarysummary = getModuleList;
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
        public void PostCustomerOrderTOSalesOrder(string employee_gid,  string user_gid, string salesorder_gid,string customer_gid, MdlCustomerPortalOrders  values)
        {

            string packingCharges = "0.00";
            string buybackCharges = "0.00";
            string insuranceCharges = "0.00";
            string lslocalgrandtotal = " 0.00";
            string lsgst = "0.00";
            string freightCharges = "0.00";

            msSQL = " select customer_name from crm_mst_tcustomer where customer_gid='" + customer_gid + "'";
            string lscustomername = objdbconn.GetExecuteScalar(msSQL);


            string lsinvoice_refno = "", lsorder_refno = "";
            string lsrefno, lscompany_code;
             lsrefno = objcmnfunctions.GetMasterGID("SO");
            msSalesGid = objcmnfunctions.GetMasterGID("VSOP");
            msSQL = "select company_code from adm_mst_Tcompany";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            if (lscompany_code == "BOBA")
            {
                msSQL = "select distinct branch_gid from hrm_mst_tbranch";
                string branch_name = objdbconn.GetExecuteScalar(msSQL);
                string ls_referenceno = objcmnfunctions.getSequencecustomizerGID("INV", "RBL", branch_name);
                msSQL = "SELECT SEQUENCE_CURVAL FROM adm_mst_tsequencecodecustomizer  WHERE sequence_code='INV' AND branch_gid='" + branch_name + "'";
                string lscode = objdbconn.GetExecuteScalar(msSQL);


                lsinvoice_refno = "SI" + " - " + lscode;
                lsorder_refno = "SO" + " - " + lscode;

            }
            else
            {
                lsinvoice_refno = msSalesGid;
                lsorder_refno = lsrefno;

            }
            msSQL = " select a.salesorder_gid as serviceorder_gid,a.customerbranch_gid,d.currencyexchange_gid,b.customer_id," +
                "a.branch_gid, f.branch_name,a.payment_days,a.delivery_days, concat(a.so_referenceno1,case when so_referencenumber='' then '' else concat(' ','-',' '," +
                " case when so_referencenumber is not null then so_referenceno1 else '' end) end )as so_reference," +
                " DATE_format(a.salesorder_date, '%Y-%m-%d') as serviceorder_date,a.customer_instruction, " +
                " concat(a.customer_name,'/',c.email) as customer_name,b.customer_gid,format(a.grandtotal, 2) as grand_total,a.shipping_to," +
                " a.customer_contact_person as customercontact_name,c.email as email,b.customer_code," +
                " a.termsandconditions,c.mobile,b.gst_number,a.Grandtotal,a.contact_email_address," +
                " format(a.addon_charge, 2) as addon_amount ,format(a.additional_discount, 2) as discount_amount," +
                " a.customer_address,format(total_amount,2) as order_total ," +
                " format(a.freight_charges,2)as freight_charges," +
                " format(a.buyback_charges,2)as buyback_charges," +
                " format(a.packing_charges,2)as packing_charges," +
                " format(a.insurance_charges,2)as insurance_charges," +
                "a.tax_name,a.tax_gid,a.roundoff,a.tax_amount,a.tax_name4," +
                " a.currency_code, a.currency_gid, a.exchange_rate, c.address1,c.address2,c.city," +
                " c.state,c.country_gid,c.zip_code,e.country_name " +
                " from cst_trn_tsalesorder a" +
                " left join crm_mst_tcustomer b on a.customer_gid=b.customer_gid " +
                " left join crm_mst_tcustomercontact c on b.customer_gid = c.customer_gid " +
                " left join crm_trn_tcurrencyexchange d on d.currency_code=a.currency_code" +
                " left join adm_mst_tcountry e on c.country_gid=e.country_gid " +
                " left join hrm_mst_tbranch f on f.branch_gid=a.branch_gid" +
                " where a.salesorder_gid='" + salesorder_gid + "'";          
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count> 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    string order_instruct = dt["customer_instruction"].ToString();


                    msSQL = " insert  into smr_trn_tsalesorder (" +
                            " salesorder_gid ," +
                            " branch_gid ," +
                            " salesorder_date," +
                            " customer_gid," +
                             " customerbranch_gid," +
                            " customer_name," +                                                   
                            " customer_contact_person," +
                            " customer_address," +
                             " shipping_to," +
                            " customer_email, " +
                            " customer_mobile, " +
                            " created_by," +
                            " so_referenceno1 ," +
                            " so_referencenumber ," +
                            " so_remarks," +
                            " Grandtotal, " +                            
                            " salesorder_status, " +
                            " addon_charge, " +
                            " additional_discount, " +
                            " addon_charge_l, " +
                            " additional_discount_l, " +
                            " grandtotal_l, " +
                            " currency_code, " +
                            " currency_gid, " +
                            " exchange_rate, " +                            
                            " freight_terms, " +
                            " payment_terms," +
                            
                            " gst_amount," +
                            " total_price," +
                            " total_amount," +
                            " tax_amount," +                                                        
                            " start_date, " +
                            " end_date, " +
                            " roundoff, " +
                            " updated_addon_charge, " +
                            " updated_additional_discount, " +
                            " freight_charges," +
                            " buyback_charges," +
                            " packing_charges," +
                            " insurance_charges, " +
                            " customer_instruction, " +
                            " contact_email_address, " +
                            "created_date" +
                            " )values(" +
                            " '" + dt["serviceorder_gid"] + "'," +
                            " '" + dt["branch_gid"] + "'," +
                            " '" + dt["serviceorder_date"] + "'," +
                            " '" + customer_gid + "'," +
                             " '" + dt["customerbranch_gid"] + "'," +
                            " '" + lscustomername + "'," +                                                       
                            " '" + dt["customercontact_name"] + "'," +
                            " '" + dt["customer_address"] + "'," +
                             " '" + dt["customer_address"] + "'," +
                            " '" + dt["email"] + "'," +
                            " '" + dt["mobile"] + "'," +
                            " '" + employee_gid + "'," +
                            " '" + lsorder_refno + "'," +
                              " '" + lsinvoice_refno + "'," +
                            " '" +(String.IsNullOrEmpty(order_instruct) ? order_instruct: order_instruct.Replace("'","\\\'"))+ "'," +
                            " '" + dt["Grandtotal"] + "'," +                            
                            " 'Approved'," +
                            "'" + dt["addon_amount"] + "'," +
                            "'" + dt["discount_amount"] + "'," +
                            "'" + dt["addon_amount"] + "'," +
                            "'" + dt["discount_amount"] + "'," +
                            " '" + lslocalgrandtotal + "'," +
                            " '" + dt["currency_code"] + "'," +
                            " '" + dt["currency_gid"] + "'," +
                            " '" + dt["exchange_rate"] + "'," +                            
                            "'0'," +
                            "'0'," +
                            
                            "'" + lsgst + "'," +                            
                            "'" + dt["Grandtotal"].ToString().Replace(",","") + "'," +
                            "'" + dt["order_total"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount"] + "'," +
                            " '0000-00-00'," +
                            " '0000-00-00'," +
                            " '" + dt["roundoff"] + "'," +
                            " '" + dt["addon_amount"] + "'," +
                           "'" + dt["discount_amount"] + "'," +
                           "'" + freightCharges + "'," +
                           "'" + buybackCharges + "'," +
                           "'" + packingCharges + "'," +
                           "'" + insuranceCharges + "'," +
                           "'" + (String.IsNullOrEmpty(order_instruct) ? order_instruct : order_instruct.Replace("'", "\\\'")) + "'," +
                           "'" + dt["contact_email_address"] + "'," +
                           " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
            }




            msSQL = " select a.salesorderdtl_gid as serviceorderdtl_gid,a.salesorder_gid," +
                   " if(a.customerproduct_code='&nbsp;',' ',a.customerproduct_code) as customerproduct_code," +
                   " a.product_gid,a.tax1_gid,a.tax2_gid,a.tax3_gid,format(a.qty_quoted,2) as qty_quoted1,a.qty_quoted,a.uom_gid,a.uom_name," +
                   " format(a.vendor_price,2) as amount ,format(a.product_price,2) as productprice,a.product_price,a.price," +
                   " format(a.tax_amount,2) as tax_amount1, " +
                   " format(a.tax_amount2,2) as tax_amount2,format(a.tax_amount3,2) as tax_amount3," +
                   " format(a.price,2) as total_amount,c.productgroup_gid, c.productgroup_name,a.product_code, " +
                   " a.display_field as description,a.tax_name as tax_name1,a.tax_name2,a.tax_name3, " +
                   " a.discount_amount as discount_amount,a.margin_percentage, " +
                   " a.discount_percentage as discount_percentage,a.product_name, a.taxsegment_gid, " +
                   "a.tax_percentage,a.tax_percentage2,a.tax_percentage3 , format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount" +
                   " from cst_trn_tsalesorderdtl a " +
                   " left join pmr_mst_tproduct b on b.product_gid=a.product_gid " +
                   "left join pmr_mst_tproductgroup c on c.productgroup_gid=a.productgroup_gid" +
                   " where a.salesorder_gid='" + salesorder_gid + "' and b.delete_flag='N' " +
                   " group by b.product_gid,a.salesorderdtl_gid order by a.salesorderdtl_gid asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<postsales_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    int i = 0;

                    mssalesorderGID1 = objcmnfunctions.GetMasterGID("VSDC");
                    
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
                      " price_l, " +
                      " taxsegment_gid " +
                      ") values (" +
                      "'" + mssalesorderGID1 + "'," +
                      "'" + dt["salesorder_gid"] + "'," +
                       "'" + dt["product_gid"].ToString() + "'," +
                       "'" + dt["productgroup_gid"].ToString() + "'," +
                       "'" + dt["productgroup_name"].ToString() + "'," +
                       "'" + dt["customerproduct_code"].ToString() + "'," +
                       "'" + dt["product_name"].ToString() + "'," +
                       "'" + dt["product_code"].ToString() + "'," +
                       "'" + dt["product_name"].ToString() + "'," +
                       "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
                       "'" + dt["qty_quoted"].ToString() + "'," +
                       "'" + dt["tax_amount1"].ToString().Replace(",", "") + "'," +
                       "'" + dt["uom_gid"].ToString() + "'," +
                       "'" + dt["uom_name"].ToString() + "'," +
                       "'" + dt["price"].ToString().Replace(",", "") + "'," +
                       "'" + dt["tax_name1"].ToString() + "'," +
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
                       "'" + dt["tax_amount1"].ToString().Replace(",", "") + "'," +
                       "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                       "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                       "'" + dt["discount_percentage"].ToString() + "'," +
                       "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                       "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
                       "'" + dt["price"].ToString().Replace(",", "") + "'," +
                       "'" + dt["taxsegment_gid"].ToString().Replace(",", "") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
            }
            if (mnResult == 1)
            {
                msSQL = "update cst_trn_tsalesorder set salesorder_status='Approved' where salesorder_gid='" + salesorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                values.status = true;
                values.message = "Order Approved Successfully.";
            }
            else
            {
                values.status = false;
                values.message = "Error while Approving Order.";
            }
        }
    }
}