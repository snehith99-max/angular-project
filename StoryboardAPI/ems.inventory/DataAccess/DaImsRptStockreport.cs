using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.inventory.Models;


namespace ems.inventory.DataAccess
{
    public class DaImsRptStockreport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        DataSet ds_dataset;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsuom_gid, reference_gid,
            lsbranch_gid, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid,
            total_amount, opening_stock, opening_qty, created_date;
        double calculatedOpeningStock, productQtyDelivered, qtyReceived;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetImsRptStockreport(string branch_gid, MdlImsRptStockreport values)
        {
            try
            {
                msSQL = " SELECT distinct a.product_gid,b.productgroup_name,a.branch_gid,j.branch_name, d.product_code,j.branch_prefix," +
                        " d.product_name,c.productuom_name, round(sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty),2) as Stock_Balance," +
                        " Format(a.unit_price,2) as Product_Price ,i.bin_number,a.display_field, " +
                        " format(((sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty)) * a.unit_price),2) as Stock_Value," +
                        " h.location_name FROM  ims_trn_tstock a " +
                        " left join pmr_mst_tproduct d on a.product_gid = d.product_gid " +
                        " left join pmr_mst_tproductgroup b on d.productgroup_gid=b.productgroup_gid" +
                        " left join pmr_mst_tproductuom c on a.uom_gid = c.productuom_gid " +
                        " left join ims_mst_tstocktype f on f.stocktype_gid = a.stocktype_gid " +
                        " left join ims_mst_tlocation h on h.location_gid=a.location_gid " +
                        " left join ims_mst_tbin i on a.bin_gid=i.bin_gid " +
                        " left join hrm_mst_tbranch j on j.branch_gid = a.branch_gid " +
                        " WHERE 0 = 0 and a.branch_gid = '" + branch_gid + "' and a.stock_flag = 'Y' " +
                        " group by d.product_gid,d.productuom_gid,a.branch_gid Order by d.product_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stockreport_list
                        {
                            bin_number = dt["bin_number"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            stock_value = dt["stock_value"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.stockreport_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetBranch(MdlImsRptStockreport values)
        {
            try
            {
                objdbconn.OpenConn();
                msSQL = " Select branch_name,branch_gid " +
                    " from hrm_mst_tbranch ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<branch_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branch_list
                        {
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.branch_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetImsRptStockstatement(MdlImsRptStockreport values)
        {
            try
            {
                msSQL = " SELECT distinct a.product_gid,b.productgroup_name,a.branch_gid,j.branch_name, d.product_code, d.product_name,j.branch_prefix," +
                        " c.productuom_name, round(sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty),2)" +
                        " as Stock_Balance,Format(a.unit_price,2) as Product_Price ,i.bin_number,a.display_field, " +
                        " format(((sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty)) * a.unit_price),2) as Stock_Value," +
                        " h.location_name FROM  ims_trn_tstock a left join " +
                        " pmr_mst_tproduct d on a.product_gid = d.product_gid left join pmr_mst_tproductgroup b on d.productgroup_gid=b.productgroup_gid" +
                        " left join pmr_mst_tproductuom c on a.uom_gid = c.productuom_gid " +
                        " left join ims_mst_tstocktype f on f.stocktype_gid = a.stocktype_gid " +
                        " left join ims_mst_tlocation h on h.location_gid=a.location_gid " +
                        " left join ims_mst_tbin i on a.bin_gid=i.bin_gid " +
                        " left join hrm_mst_tbranch j on j.branch_gid = a.branch_gid " +
                        " WHERE 0 = 0 and a.stock_flag = 'Y' " +
                        " group by d.product_gid,d.productuom_gid,a.branch_gid Order by d.product_name asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stockreport_list
                        {

                            bin_number = dt["bin_number"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            stock_value = dt["stock_value"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.stockreport_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetStockStatementProduct(string product_gid, string branch_gid, MdlImsRptStockreport values)
        {
            try
            {
                msSQL = " SELECT distinct a.product_gid,b.productgroup_name,a.branch_gid,j.branch_name, d.product_code," +
                    " d.product_name,c.productuom_name, round(sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty),2) as Stock_Balance, " +
                    " Format(a.unit_price,2) as Product_Price ,i.bin_number,a.display_field, " +
                    " format(((sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty)) * a.unit_price),2) as Stock_Value, " +
                    " h.location_name ,(ROUND(SUM(a.stock_qty + a.amend_qty - a.damaged_qty - a.issued_qty - a.transfer_qty), 2) - SUM(COALESCE(o.qty_received, 0)) + COALESCE(m.product_qtydelivered, 0)) AS opening_stock " +
                    " FROM  ims_trn_tstock a  left join pmr_mst_tproduct d on a.product_gid = d.product_gid " +
                    " left join pmr_mst_tproductgroup b on d.productgroup_gid=b.productgroup_gid " +
                    " left join pmr_mst_tproductuom c on a.uom_gid = c.productuom_gid  " +
                    " left join ims_mst_tstocktype f on f.stocktype_gid = a.stocktype_gid  " +
                    " left join ims_mst_tlocation h on h.location_gid=a.location_gid " +
                    " left join ims_mst_tbin i on a.bin_gid=i.bin_gid  " +
                    " left join hrm_mst_tbranch j on j.branch_gid = a.branch_gid " +
                    " LEFT JOIN pmr_trn_tgrndtl k ON k.product_gid = a.product_gid " +
                    " LEFT JOIN pmr_trn_tpurchaseorderdtl o ON o.purchaseorderdtl_gid = k.purchaseorderdtl_gid " +
                    " LEFT JOIN smr_trn_tdeliveryorderdtl m ON m.product_gid = a.product_gid " +
                    " WHERE 0 = 0 and a.branch_gid = '" + branch_gid + "' and d.product_gid = '" + product_gid + "' and a.stock_flag = 'Y'  group by " +
                    " d.product_gid,d.productuom_gid,a.branch_gid Order by d.product_name asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new stockreport_list
                        {
                            bin_number = dt["bin_number"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            stock_value = dt["stock_value"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            opening_stock = dt["opening_stock"].ToString(),
                        });
                        values.stockreport_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetStockStatementPurchase(string product_gid, MdlImsRptStockreport values)
        {
            try
            {
                msSQL = "select a.vendor_gid,a.branch_gid,a.purchaseorder_gid,d.vendor_code,format(sum(c.product_price*qty_ordered),2) as stock_value," +
                        "d.vendor_companyname,sum(c.qty_ordered)as qty_ordered ,c.product_gid, " +
                        "d.contactperson_name,d.contact_telephonenumber,d.email_id " +
                        "from pmr_trn_tgrn a " +
                        "left join pmr_trn_tpurchaseorder b on b.purchaseorder_gid = a.purchaseorder_gid " +
                        "left join pmr_trn_tpurchaseorderdtl c on b.purchaseorder_gid = c.purchaseorder_gid " +
                        "left join acp_mst_tvendor d on d.vendor_gid = b.vendor_gid where c.product_gid = '" + product_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetStockpurchaseState_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetStockpurchaseState_list
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            vendor_code = dt["vendor_code"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            qty_ordered = dt["qty_ordered"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            stock_value = dt["stock_value"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                        });
                        values.GetStockpurchaseState_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetPurchasevendor(string vendor_gid, MdlImsRptStockreport values)
        {
            try
            {
                msSQL = "select a.vendor_gid,a.vendor_companyname,a.contactperson_name,a.contact_telephonenumber,a.email_id,a.gst_no,  " +
                        "concat(c.address1, ',', city, ',', c.state, ',', c.postal_code, ',', d.country_name) as address from acp_mst_tvendor a " +
                        "left join adm_mst_taddress c on c.address_gid = a.address_gid " +
                        "left join adm_mst_tcountry d on d.country_gid = c.country_gid " +
                        "where a.vendor_gid = '" + vendor_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetStockvendor_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetStockvendor_list
                        {
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            gst_no = dt["gst_no"].ToString(),
                            address = dt["address"].ToString(),
                        });
                        values.GetStockvendor_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Vendor Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetPurchaseorder_history(string vendor_gid, string product_gid, MdlImsRptStockreport values)
        {
            try
            {
                msSQL = " select /*+ MAX_EXECUTION_TIME(600000) */ distinct h.costcenter_name,a.purchaseorder_gid,  " +
                        " b.vendor_code,concat(b.contactperson_name,' / ',b.email_id,' / ',b.contact_telephonenumber) as Contact, " +
                        " concat(b.vendor_code,'/',b.vendor_companyname) as Vendor, " +
                        " CASE WHEN a.poref_no IS NULL OR a.poref_no = '' THEN a.purchaseorder_gid ELSE a.poref_no END AS porefno,  " +
                        " a.poref_no, a.purchaseorder_remarks,a.purchaseorder_status, format(a.total_amount,2) as total_amount," +
                        " Date_add(a.purchaseorder_date,Interval delivery_days day) as ExpectedPODeliveryDate , " +
                        " format(a.total_amount,2) as paymentamount,  DATE_FORMAT(a.purchaseorder_date , '%d-%m-%Y') as purchaseorder_date ," +
                        " a.vendor_status, CASE when a.invoice_flag <> 'IV Pending' then a.invoice_flag when a.grn_flag <> 'GRN Pending' " +
                        " then a.grn_flag else a.purchaseorder_flag end as 'overall_status', a.purchaseorder_flag, a.grn_flag, a.invoice_flag, " +
                        " b.vendor_companyname, c.branch_name,  case when group_concat(distinct purchaserequisition_referencenumber)=',' then ''  " +
                        " when group_concat(distinct purchaserequisition_referencenumber) <> ',' then group_concat(distinct purchaserequisition_referencenumber) end  as refrence_no,  " +
                        " bscc_flag,po_type,DATE_FORMAT(a.created_date , '%d-%m-%Y') as created_date,concat(d.user_firstname,' ',d.user_lastname) as created_by,l.grn_gid  from pmr_trn_tpurchaseorder a" +
                        " left join  acp_mst_tvendor b on b.vendor_gid = a.vendor_gid " +
                        " left join hrm_mst_tbranch c on c.branch_gid = a.branch_gid " +
                        " left join adm_mst_tuser d on d.user_gid = a.created_by " +
                        " left join hrm_mst_temployee e on e.user_gid = d.user_gid " +
                        " left join adm_mst_tmodule2employee  f on f.employee_gid = e.employee_gid " +
                        " left join pmr_mst_tcostcenter h on h.costcenter_gid=a.costcenter_gid " +
                        " left join pmr_Trn_tpr2po i on i.purchaseorder_gid=a.purchaseorder_gid " +
                        " left join pmr_Trn_tpurchaserequisition j on j.purchaserequisition_gid=i.purchaserequisition_gid  " +
                        " left join crm_trn_tcurrencyexchange k on k.currencyexchange_gid =a.currency_code " +
                        " left join pmr_trn_tpurchaseorderdtl g on g.purchaseorder_gid=a.purchaseorder_gid " +
                        " left join pmr_trn_tgrn l on l.purchaseorder_gid=a.purchaseorder_gid " +
                        " where 1=1 and a.workorderpo_flag='N' " +
                        " and a.purchaseorder_status <>'PO Amended' and a.purchaseorder_flag <> 'PO Canceled'" +
                        " and b.vendor_gid='" + vendor_gid + "' and g.product_gid='" + product_gid + "' group by a.purchaseorder_gid  " +
                        " order by  date(a.purchaseorder_date) desc,a.purchaseorder_date asc, a.purchaseorder_gid desc, b.vendor_companyname desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getpurchaseorder_history>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getpurchaseorder_history
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
                            grn_gid = dt["grn_gid"].ToString(),
                            grn_flag = dt["grn_flag"].ToString(),
                        });
                        values.Getpurchaseorder_history = getModuleList;
                    }
                }
                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading purchase History!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        public void DaGetStockStatementSales(string product_gid, MdlImsRptStockreport values)
        {
            try
            {
                msSQL = " select sum(a.qty_quoted) as qty_quoted, format(sum(a.qty_quoted *a.price),2 )as price,a.product_gid," +
                        " c.customer_id,c.customer_name, b.customer_gid, concat(b.customer_contact_person,' / ',b.customer_mobile,' / ',b.customer_email)  as contact " +
                        " from smr_trn_tsalesorderdtl a  left join smr_trn_tsalesorder b on b.salesorder_gid=a.salesorder_gid " +
                        " left join crm_mst_tcustomer c on b.customer_gid=c.customer_gid " +
                        " left join pmr_mst_tproduct d on a.product_gid=d.product_gid  where a.product_gid='" + product_gid + "'" +
                        " group by b.customer_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetStockStatement_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetStockStatement_list
                        {
                            qty_quoted = dt["qty_quoted"].ToString(),
                            price = dt["price"].ToString(),
                            customer_id = dt["customer_id"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            contact = dt["contact"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                        });
                        values.GetStockStatement_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetSalescustomer(string customer_gid, MdlImsRptStockreport values)
        {
            try
            {
                msSQL = "  select c.customer_address,c.customer_id,c.customer_name, b.customer_gid, concat(b.customer_contact_person,' / ',b.customer_mobile,' / ',b.customer_email)  as contact from smr_trn_tsalesorderdtl a  " +
                        "  left join smr_trn_tsalesorder b on b.salesorder_gid=a.salesorder_gid   " +
                        "  left join crm_mst_tcustomer c on b.customer_gid=c.customer_gid  " +
                        "  left join pmr_mst_tproduct d on a.product_gid=d.product_gid  where c.customer_gid='" + customer_gid + "'" +
                        "  group by b.customer_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetStockcustomer_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetStockcustomer_list
                        {
                            customer_address = dt["customer_address"].ToString(),
                            customer_id = dt["customer_id"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            contact = dt["contact"].ToString(),
                        });
                        values.GetStockcustomer_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading customer Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetStockStatementSummary(string product_gid, MdlImsRptStockreport values)
        {
            try
            {

                msSQL = "select a.customer_gid, a.customer_name from crm_mst_tcustomer a" +
                    " left join smr_trn_tsalesorder b on a.customer_gid=b.customer_gid " +
                    "left join smr_trn_tsalesorderdtl c on b.salesorder_gid = c.salesorder_gid " +
                    "where c.product_gid='" + product_gid + "' group by customer_gid";
                var GetCustomer_list = new List<StockcustomerList>();
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select sum(a.qty_quoted *a.price) as total_amount from smr_trn_tsalesorderdtl a " +
                                " left join smr_trn_tsalesorder b on a.salesorder_gid = b.salesorder_gid " +
                                " where  a.product_gid = '" + product_gid + "' and b.customer_gid = '" + dt["customer_gid"] + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            total_amount = objOdbcDataReader["total_amount"].ToString();
                            objOdbcDataReader.Close();
                        }
                        GetCustomer_list.Add(new StockcustomerList
                        {
                            customer_name = dt["customer_name"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            total_amount = total_amount,
                        });
                    }
                }

                msSQL = " select d.product_name,d.product_code,sum(a.qty_quoted) as qty_quoted, sum(a.qty_quoted *a.price) as price, " +
                        " c.customer_name, b.customer_gid, concat(year(b.salesorder_date),'-' ,year(b.salesorder_date) +1) as salesorder_date" +
                        " from smr_trn_tsalesorderdtl a" +
                        " left join smr_trn_tsalesorder b on b.salesorder_gid=a.salesorder_gid " +
                        " left join crm_mst_tcustomer c on b.customer_gid=c.customer_gid " +
                        " left join pmr_mst_tproduct d on a.product_gid=d.product_gid " +
                        " where a.product_gid='" + product_gid + "' group by d.product_name," +
                        " c.customer_name,b.customer_gid, concat(year(b.salesorder_date),'-' ,year(b.salesorder_date) +1) " +
                        " order by  c.customer_name, d.product_name";
                ds_dataset = objdbconn.GetDataSet(msSQL, "smr_trn_tsalesorder");
                var GetStockState = new List<GetStockState_list>();
                if (ds_dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                    {
                        GetStockState.Add(new GetStockState_list
                        {
                            qty_quoted = ds["qty_quoted"].ToString(),
                            price = ds["price"].ToString(),
                            customer_name = ds["customer_name"].ToString(),
                            salesorder_date = ds["salesorder_date"].ToString(),
                            customer_gid = ds["customer_gid"].ToString(),
                            product_code = ds["product_code"].ToString(),
                            product_name = ds["product_name"].ToString(),
                        });
                    }
                }

                values.StockcustomerList = GetCustomer_list;
                values.GetStockState_list = GetStockState;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Statement!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********"
                + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL +
                "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetSalesorder_history(string customer_gid, string product_gid, MdlImsRptStockreport values)
        {
            try
            {
                msSQL = " select distinct a.invoice_gid,x.company_code, s.salesorder_status, case when a.invoice_reference like '%AREF%' then j.agreement_referencenumber" +
                        " else  cast(concat(s.so_referenceno1,if (s.so_referencenumber<>'',concat(' ', ' | ', ' ', s.so_referencenumber),'') ) as char)  end as so_referencenumber, " +
                        " a.invoice_refno,CASE WHEN a.irncancel_date IS NOT NULL THEN 'IRN Cancelled' WHEN a.creditnote_status = 'Y' THEN 'Credit Noted'  ELSE a.invoice_status " +
                        " END AS status, a.mail_status,a.customer_gid,a.invoice_date,a.invoice_reference,a.additionalcharges_amount,a.discount_amount,   CASE when a.payment_flag<> 'PY Pending'" +
                        " then a.payment_flag else a.invoice_flag end as 'overall_status', a.mail_status,  a.payment_flag,  format(a.initialinvoice_amount, 2) as initialinvoice_amount," +
                        " s.salesorder_gid,a.invoice_status,a.invoice_flag,  case when a.irn is not null then 'IRN Generated' else 'IRN Pending' end as 'irn_status'," +
                        " case when a.irncancel_date is not null then 'IRN Cancelled' else '' end as 'irncancel_status', format(a.invoice_amount, 2) as invoice_amount," +
                        " c.customer_code,  concat(c.customer_name,' / ',h.country)  as customer_name,a.currency_code,   a.customer_contactnumber as mobile," +
                        " a.invoice_from,i.directorder_gid,a.progressive_invoice  from rbl_trn_tinvoice a  " +
                        " left join rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                        " left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid " +
                        " left join crm_mst_tcustomercontact m  on a.customer_gid = m.customer_gid  " +
                        " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code  " +
                        " left join adm_mst_tcompany x on x.country_gid = h.country_gid  " +
                        " left join smr_trn_tsalesorder s on a.invoice_reference = s.salesorder_gid  " +
                        " left join crm_trn_tagreement j on j.agreement_gid = a.invoice_reference  " +
                        " left join smr_trn_tdeliveryorder i on s.salesorder_gid = i.salesorder_gid  " +
                        " where a.invoice_type<>'Opening Invoice' and a.invoice_status<>'Invoice Cancelled' and c.customer_gid = '" + customer_gid + "' and b.product_gid = '" + product_gid + "' " +
                        " group by a.invoice_gid order by date(a.invoice_date) desc,a.invoice_date asc, a.invoice_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getsaleshistory_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getsaleshistory_list
                        {
                            invoice_date = Convert.ToDateTime(dt["invoice_date"].ToString()),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
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
                            company_code = dt["company_code"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString()
                        });
                        values.Getsaleshistory_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading purchase History!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        public void DaGetStockStatementPurchaseSummary(string product_gid, MdlImsRptStockreport values)
        {
            try
            {
                msSQL = "select a.vendor_gid, a.vendor_companyname from acp_mst_tvendor a" +
                    " left join pmr_trn_tpurchaseorder b on a.vendor_gid=b.vendor_gid " +
                    "left join pmr_trn_tpurchaseorderdtl c on b.purchaseorder_gid = c.purchaseorder_gid " +
                    "where c.product_gid='" + product_gid + "' group by vendor_gid";
                var GetCustomer_list = new List<StockpurchasecustomerList>();
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select sum(a.qty_ordered *a.product_price) as total_amount from pmr_trn_tpurchaseorderdtl a " +
                                " left join pmr_trn_tpurchaseorder b on a.purchaseorder_gid = b.purchaseorder_gid " +
                                " where  a.product_gid = '" + product_gid + "' and b.vendor_gid = '" + dt["vendor_gid"] + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            total_amount = objOdbcDataReader["total_amount"].ToString();
                        }
                        GetCustomer_list.Add(new StockpurchasecustomerList
                        {
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            total_amount = total_amount,
                        });
                    }
                }

                msSQL = " select d.product_name,sum(a.qty_ordered) as qty_ordered, sum(a.qty_ordered *a.product_price) as product_price, " +
                         " c.vendor_companyname, b.vendor_gid, concat(year(b.purchaseorder_date), '-', year(b.purchaseorder_date) + 1) as purchaseorder_date " +
                         " from pmr_trn_tpurchaseorderdtl a " +
                         " left join pmr_trn_tpurchaseorder b on b.purchaseorder_gid = a.purchaseorder_gid " +
                         " left join acp_mst_tvendor c on b.vendor_gid = c.vendor_gid " +
                         " left  join pmr_mst_tproduct d on a.product_gid = d.product_gid" +
                         " where a.product_gid = '" + product_gid + "' group by d.product_name," +
                         " c.vendor_companyname,b.vendor_gid, concat(year(b.purchaseorder_date), '-', year(b.purchaseorder_date) + 1) " +
                         " order by  c.vendor_companyname, d.product_name";
                ds_dataset = objdbconn.GetDataSet(msSQL, "pmr_trn_tpurchaseorderdtl");
                var GetStockState = new List<GetStockpurchaseState_list>();
                if (ds_dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow ds in ds_dataset.Tables[0].Rows)
                    {
                        GetStockState.Add(new GetStockpurchaseState_list
                        {
                            qty_ordered = ds["qty_ordered"].ToString(),
                            product_price = ds["product_price"].ToString(),
                            vendor_companyname = ds["vendor_companyname"].ToString(),
                            purchaseorder_date = ds["purchaseorder_date"].ToString(),
                            vendor_gid = ds["vendor_gid"].ToString(),
                        });
                    }
                }

                values.GetStockpurchaseState_list = GetStockState;
                values.StockpurchasecustomerList = GetCustomer_list;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Statement!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********"
                + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL +
                "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


        public void DaGetImsRptStockmovement(MdlImsRptStockreport values)
        {
            try
            {
                msSQL = "  SELECT distinct a.product_gid,b.productgroup_name,a.branch_gid,j.branch_name, d.product_code, d.product_name,j.branch_prefix, " +
                        " c.productuom_name,a.remarks, round(sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty),2) as Stock_Balance," +
                        " Format(a.unit_price,2) as Product_Price ,i.bin_number,a.display_field,  " +
                        " format(((sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty)) * a.unit_price),2) as Stock_Value," +
                        " h.location_name FROM  ims_trn_tstock a " +
                        " left join  pmr_mst_tproduct d on a.product_gid = d.product_gid " +
                        " left join pmr_mst_tproductgroup b on d.productgroup_gid=b.productgroup_gid " +
                        " left join pmr_mst_tproductuom c on a.uom_gid = c.productuom_gid  " +
                        " left join ims_mst_tstocktype f on f.stocktype_gid = a.stocktype_gid  " +
                        " left join ims_mst_tlocation h on h.location_gid=a.location_gid  " +
                        " left join ims_mst_tbin i on a.bin_gid=i.bin_gid  " +
                        " left join hrm_mst_tbranch j on j.branch_gid = a.branch_gid  WHERE 0 = 0 and a.stock_flag = 'Y'  " +
                        " group by d.product_gid,d.productuom_gid,a.branch_gid Order by d.product_name asc  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " SELECT CASE WHEN a.qty_delivered IS NULL OR a.qty_delivered = '' THEN 0  ELSE COALESCE(sum(a.qty_delivered), 0)  END AS qty_delivered " +
                                " FROM pmr_trn_tgrndtl a " +
                                " LEFT JOIN pmr_trn_tgrn b ON b.grn_gid = a.grn_gid " +
                                " where a.product_gid = '" + dt["product_gid"].ToString() + "' and " +
                                " b.branch_gid = '" + dt["branch_gid"].ToString() + "'";
                        string qty_receive = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " SELECT CASE WHEN a.issued_qty IS NULL OR a.issued_qty = '' THEN 0  " +
                               " ELSE COALESCE(sum(a.issued_qty), 0)  END AS product_qtydelivered " +
                               " FROM ims_trn_tstockdtl a where stock_type='Delivery' " +
                               " and product_gid='" + dt["product_gid"].ToString() + "' " +
                               " and branch_gid='" + dt["branch_gid"].ToString() + "'";
                        string product_qtydelivered = objdbconn.GetExecuteScalar(msSQL);
                        double stockBalance = Convert.ToDouble(dt["stock_balance"]);
                        qtyReceived = Convert.ToDouble(qty_receive);
                        productQtyDelivered = Convert.ToDouble(product_qtydelivered);
                        if (dt["remarks"].ToString() == "From Opening Stock")
                        {
                            calculatedOpeningStock = stockBalance - qtyReceived + productQtyDelivered;
                            opening_stock = calculatedOpeningStock.ToString();
                        }
                        else
                        {
                            opening_stock = "0";
                        }
                        getModuleList.Add(new stockreport_list
                        {

                            bin_number = dt["bin_number"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            stock_value = dt["stock_value"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            opening_stock = opening_stock,
                            qty_received = qtyReceived,
                            product_qtydelivered = productQtyDelivered,
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.stockreport_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetstockStatus(MdlImsRptStockreport values)
        {
            try
            {
                msSQL = " select substring(date_format(a.grn_date,'%M'),1,3)as month,sum(b.qty_delivered) as grn_count " +
                        " from pmr_trn_tgrn a left join pmr_trn_tgrndtl b on a.grn_gid =b.grn_gid where a.grn_date > date_add(now(), interval - 6 month)  " +
                        " and a.grn_date <= date(now()) group by date_format(a.grn_date, '%M') order by a.grn_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getstock_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select sum(b.product_qtydelivered)as product_qtydelivered  from smr_trn_tdeliveryorder a  " +
                                "  left join smr_trn_tdeliveryorderdtl b on b.directorder_gid=a.directorder_gid  " +
                                " where a.directorder_date > date_add(now(), interval - 6 month)    " +
                                " and a.directorder_date <= date(now()) and substring(date_format(a.directorder_date,'%M'),1,3)='" + dt["month"].ToString() + "'" +
                                " group by date_format(a.directorder_date, '%M') order by a.directorder_date desc ";
                        string delivery_count = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new Getstock_list
                        {
                            month = dt["month"].ToString(),
                            grn_count = dt["grn_count"].ToString(),
                            delivery_count = delivery_count,
                        });
                        values.Getstock_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetlastsixmonthstock(MdlImsRptStockreport values)
        {
            try
            {
                msSQL = " select DATE_FORMAT(grn_date, '%b-%Y')  as grn_date,substring(date_format(a.grn_date,'%M'),1,3)as month,sum(b.qty_delivered) as grn_count  " +
                        " from pmr_trn_tgrn a left join pmr_trn_tgrndtl b on a.grn_gid =b.grn_gid where a.grn_date > date_add(now(), interval - 6 month)  " +
                        " and a.grn_date <= date(now()) group by date_format(a.grn_date, '%M') order by a.grn_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getlastsixmonthstock_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select sum(b.product_qtydelivered)as product_qtydelivered  from smr_trn_tdeliveryorder a " +
                                " left join smr_trn_tdeliveryorderdtl b on b.directorder_gid=a.directorder_gid  where a.directorder_date > date_add(now(), interval - 6 month) " +
                                " and a.directorder_date <= date(now())  and substring(date_format(a.directorder_date,'%M'),1,3)='" + dt["month"].ToString() + "' group by date_format(a.directorder_date, '%M') order by a.directorder_date desc";
                        string delivery_count = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new Getlastsixmonthstock_list
                        {

                            month = dt["month"].ToString(),
                            grn_count = dt["grn_count"].ToString(),
                            delivery_count = delivery_count,
                            grn_date = dt["grn_date"].ToString(),
                        });
                        values.Getlastsixmonthstock_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetImsRptMovementreportsearch(string branch_gid, MdlImsRptStockreport values)
        {
            try
            {


                msSQL = "  SELECT distinct a.product_gid,b.productgroup_name,a.branch_gid,j.branch_name, d.product_code, d.product_name,j.branch_prefix, " +
                        " c.productuom_name,a.remarks, round(sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty),2) as Stock_Balance," +
                        " Format(a.unit_price,2) as Product_Price ,i.bin_number,a.display_field,  " +
                        " format(((sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty)) * a.unit_price),2) as Stock_Value," +
                        " h.location_name FROM  ims_trn_tstock a " +
                        " left join  pmr_mst_tproduct d on a.product_gid = d.product_gid " +
                        " left join pmr_mst_tproductgroup b on d.productgroup_gid=b.productgroup_gid " +
                        " left join pmr_mst_tproductuom c on a.uom_gid = c.productuom_gid  " +
                        " left join ims_mst_tstocktype f on f.stocktype_gid = a.stocktype_gid  " +
                        " left join ims_mst_tlocation h on h.location_gid=a.location_gid  " +
                        " left join ims_mst_tbin i on a.bin_gid=i.bin_gid  " +
                        " left join hrm_mst_tbranch j on j.branch_gid = a.branch_gid  WHERE 0 = 0 and a.stock_flag = 'Y' ";
                if (branch_gid == "" || branch_gid == "null" || branch_gid == "undefined")
                {

                }
                else
                {
                    msSQL += "AND a.branch_gid = '" + branch_gid + "'";
                }

                msSQL += " GROUP BY d.product_gid, d.productuom_gid, a.branch_gid ORDER BY  d.product_name ASC ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        msSQL = " SELECT CASE WHEN a.qty_delivered IS NULL OR a.qty_delivered = '' THEN 0  ELSE COALESCE(sum(a.qty_delivered), 0)  END AS qty_delivered " +
                                    " FROM pmr_trn_tgrndtl a " +
                                    " LEFT JOIN pmr_trn_tgrn b ON b.grn_gid = a.grn_gid " +
                                    " where a.product_gid = '" + dt["product_gid"].ToString() + "' and " +
                                    " b.branch_gid = '" + dt["branch_gid"].ToString() + "'";
                        string qty_receive = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " SELECT CASE WHEN a.issued_qty IS NULL OR a.issued_qty = '' THEN 0  " +
                               " ELSE COALESCE(sum(a.issued_qty), 0)  END AS product_qtydelivered " +
                               " FROM ims_trn_tstockdtl a where stock_type='Delivery' " +
                               " and product_gid='" + dt["product_gid"].ToString() + "' " +
                               " and branch_gid='" + dt["branch_gid"].ToString() + "'";
                        string product_qtydelivered = objdbconn.GetExecuteScalar(msSQL);
                        double stockBalance = Convert.ToDouble(dt["stock_balance"]);
                        qtyReceived = Convert.ToDouble(qty_receive);
                        productQtyDelivered = Convert.ToDouble(product_qtydelivered);
                        if (dt["remarks"].ToString() == "From Opening Stock")
                        {
                            calculatedOpeningStock = stockBalance - qtyReceived + productQtyDelivered;
                            opening_stock = calculatedOpeningStock.ToString();
                        }
                        else
                        {
                            opening_stock = "0";
                        }
                        getModuleList.Add(new stockreport_list
                        {

                            bin_number = dt["bin_number"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            stock_value = dt["stock_value"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            opening_stock = opening_stock,
                            qty_received = qtyReceived,
                            product_qtydelivered = productQtyDelivered,
                            branch_prefix = dt["branch_prefix"].ToString(),
                        });
                        values.stockreport_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetStockmovementview(string product_gid,string branch_gid, MdlImsRptStockreport values)
        {
            try
            {
                msSQL = "  SELECT distinct a.product_gid,b.productgroup_name,a.branch_gid,j.branch_name, d.product_code, d.product_name,j.branch_prefix, " +
                       " c.productuom_name,a.remarks, round(sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty),2) as Stock_Balance," +
                       " Format(a.unit_price,2) as Product_Price ,i.bin_number,a.display_field,  " +
                       " format(((sum(a.stock_qty+a.amend_qty-a.damaged_qty-a.issued_qty-a.transfer_qty)) * a.unit_price),2) as Stock_Value," +
                       " h.location_name,DATE_FORMAT(a.created_date , '%d-%m-%Y') as created_date,a.reference_gid,a.remarks FROM  ims_trn_tstock a " +
                       " left join  pmr_mst_tproduct d on a.product_gid = d.product_gid " +
                       " left join pmr_mst_tproductgroup b on d.productgroup_gid=b.productgroup_gid " +
                       " left join pmr_mst_tproductuom c on a.uom_gid = c.productuom_gid  " +
                       " left join ims_mst_tstocktype f on f.stocktype_gid = a.stocktype_gid  " +
                       " left join ims_mst_tlocation h on h.location_gid=a.location_gid  " +
                       " left join ims_mst_tbin i on a.bin_gid=i.bin_gid  " +
                       " left join hrm_mst_tbranch j on j.branch_gid = a.branch_gid  WHERE 0 = 0 and a.stock_flag = 'Y' and a.branch_gid='" + branch_gid + "' " +
                       " and a.product_gid='" + product_gid + "'" +
                       " group by d.product_gid,d.productuom_gid,a.branch_gid Order by d.product_name asc  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<stockreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " SELECT CASE WHEN a.qty_delivered IS NULL OR a.qty_delivered = '' THEN 0  ELSE COALESCE(sum(a.qty_delivered), 0)  END AS qty_delivered " +
                                " FROM pmr_trn_tgrndtl a " +
                                " LEFT JOIN pmr_trn_tgrn b ON b.grn_gid = a.grn_gid " +
                                " where a.product_gid = '" + dt["product_gid"].ToString() + "' and " +
                                " b.branch_gid = '" + dt["branch_gid"].ToString() + "'";
                        string qty_receive = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " SELECT CASE WHEN a.issued_qty IS NULL OR a.issued_qty = '' THEN 0  " +
                               " ELSE COALESCE(sum(a.issued_qty), 0)  END AS product_qtydelivered " +
                               " FROM ims_trn_tstockdtl a where stock_type='Delivery' " +
                               " and product_gid='" + dt["product_gid"].ToString() + "' " +
                               " and branch_gid='" + dt["branch_gid"].ToString() + "'";
                        string product_qtydelivered = objdbconn.GetExecuteScalar(msSQL);
                        double stockBalance = Convert.ToDouble(dt["stock_balance"]);
                        qtyReceived = Convert.ToDouble(qty_receive);
                        productQtyDelivered = Convert.ToDouble(product_qtydelivered);
                        if (dt["remarks"].ToString() == "From Opening Stock")
                        {
                            calculatedOpeningStock = stockBalance - qtyReceived + productQtyDelivered;
                            opening_stock = calculatedOpeningStock.ToString();
                        }
                        else
                        {
                            opening_stock = "0";
                        }
                        getModuleList.Add(new stockreport_list
                        {

                            bin_number = dt["bin_number"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            stock_balance = dt["stock_balance"].ToString(),
                            stock_value = dt["stock_value"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            opening_stock = opening_stock,
                            qty_received = qtyReceived,
                            product_qtydelivered = productQtyDelivered,
                            branch_prefix = dt["branch_prefix"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            reference_gid = dt["reference_gid"].ToString(),
                            remarks = dt["remarks"].ToString(),
                        });
                        values.stockreport_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Stock Report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
} }