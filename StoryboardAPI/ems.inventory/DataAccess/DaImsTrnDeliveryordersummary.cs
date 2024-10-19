using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.inventory.Models;
using OfficeOpenXml.FormulaParsing.Excel.Operators;
using System.Windows.Media.Media3D;
using System.Runtime.Remoting;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Drawing.Imaging;
using static System.Drawing.ImageConverter;
using System.Drawing;
using Newtonsoft.Json;
using System.Net;
using RestSharp;
using ParameterType = RestSharp.ParameterType;
using Newtonsoft.Json.Linq;
using System.Windows.Media;


namespace ems.inventory.DataAccess
{

    public class DaImsTrnDeliveryordersummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdelivery_quantity, lsdesignation_code, lsuom_gid, lsbranch_gid, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, msstockdtlGid, base_url, api_key;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        string mssalesorderGID;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string company_logo_path;
        Image company_logo;
        DataTable dt1 = new DataTable();
        DataTable DataTable4 = new DataTable();

        public void DaGetImsTrnDeliveryorderSummary(MdlImsTrnDeliveryordersummary values)
        {
            try
            {
                msSQL = " select distinct a.directorder_gid,s.branch_name,a.mode_of_despatch,c.so_referenceno1" +
                " as so_referenceno1," +
                " CONCAT_ws( '/', h.customer_id, h.customer_name) as customerdetails," +
                " directorder_refno, date_format(directorder_date, '%d-%m-%Y') as directorder_date, n.user_firstname, a.dc_no,a.salesorder_gid, " +
                " a.customer_name, customer_branchname,a.customer_contactperson, directorder_status,d.salesorder_status, " +
                " concat(CAST(date_format(delivered_date,'%d-%m-%Y') as CHAR),'/',delivered_to) as delivery_details, " +
                " concat_ws( '/',e.customercontact_name,e.mobile,e.email) as contact" +
                " from smr_trn_tdeliveryorder a " +
                " left join crm_mst_tcustomercontact e on e.customer_gid = a.customer_gid " +
                " left join crm_mst_tcustomer h on a.customer_gid=h.customer_gid" +
                " left join hrm_mst_temployee m on m.employee_gid=a.created_name " +
                " left join smr_trn_tsalesorder d on d.salesorder_gid = a.salesorder_gid " +
                " left join hrm_mst_tbranch s on s.branch_gid=a.customerbranch_gid " +
                " left join adm_mst_tuser n on n.user_gid= m.user_gid " +
                " left join smr_trn_tdeliveryorderdtl b on a.directorder_gid =b.directorder_gid " +
                " left join smr_trn_tsalesorder c on a.salesorder_gid=c.salesorder_gid " +
                " where dc_type<>'Direct DC' " +
                " order by a.directorder_date DESC,a.directorder_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<deliveryorder_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new deliveryorder_list
                        {

                            directorder_date = dt["directorder_date"].ToString(),
                            directorder_refno = dt["directorder_refno"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            branch_name = dt["customer_branchname"].ToString(),
                            delivery_status = dt["salesorder_status"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            dc_no = dt["dc_no"].ToString(),
                            directorder_gid = dt["directorder_gid"].ToString(),
                            mode_of_despatch = dt["mode_of_despatch"].ToString(),
                            customerdetails = dt["customerdetails"].ToString()



                        });
                        values.deliveryorder_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Delivery Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetImsTrnAddDeliveryorderSummary(MdlImsTrnDeliveryordersummary values)
        {
            try
            {
                msSQL = " select distinct a.salesorder_gid,y.branch_name,y.branch_prefix, cast(concat(a.so_referenceno1, " +
                        " if (a.so_referencenumber<>'',concat(' | ', a.so_referencenumber),'') ) as char)as so_referenceno1, date_format(a.salesorder_date, '%d-%m-%Y') as salesorder_date,  " +
                        " sum(b.qty_quoted) as qty_quoted,sum(b.product_delivered) as product_delivered,  " +
                        " a.customer_name,  a.customer_contact_person, a.salesorder_status,c.mobile,  " +
                        " a.despatch_status, case when a.customer_email is null then concat(c.customercontact_name,'/',c.email,'/',c.mobile)  " +
                        " when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_email,' / ',a.customer_mobile) end as contact  ,e.customer_id as customer_code " +
                        " from smr_trn_tsalesorder a " +
                        " left join smr_trn_tsalesorderdtl b on b.salesorder_gid = a.salesorder_gid " +
                        " left join crm_mst_tcustomercontact c on c.customer_gid = a.customer_gid " +
                        " left join crm_mst_tcustomer e on e.customer_gid = c.customer_gid " +
                        " left join hrm_mst_tbranch y on y.branch_gid = a.branch_gid where a.so_type != 'Services' " +
                        " group by salesorder_gid " +
                        " having(qty_quoted<> product_delivered)  order by a.salesorder_date desc, a.customer_name desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<adddeliveryorder_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new adddeliveryorder_list
                        {
                            salesorder_date = dt["salesorder_date"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
                            customer_code = dt["customer_code"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),

                        });
                        values.adddeliveryorder_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Delivery Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetRaiseDeliveryorderSummary(string salesorder_gid, MdlImsTrnDeliveryordersummary values)
        {
            try
            {

                msSQL = "  select a.salesorder_gid,a.so_referenceno1, " +
                        "  DATE_FORMAT(a.salesorder_date, '%d-%m-%Y') as salesorder_date,h.invoice_refno,a.termsandconditions,b.customer_gid,b.customer_code,format(a.grandtotal, 2) as grandtotal, a.customer_name, c.customerbranch_name ,  " +
                        "  concat(b.customer_address, b.customer_address2, b.customer_city, b.customer_state, b.customer_pin) as customer_address, " +
                        "  concat (c.email,c.mobile) as customer_details, " +
                        "  c.designation,c.customercontact_name,c.email as customer_email,c.mobile as customer_mobile,a.currency_code,a.shipping_to,  " +
                        "  a.customer_mobile,a.customer_email,a.customer_address as customer_address_so,  " +
                        "  c.customercontact_name as customer_contact_person,a.shipping_to,d.branch_name,c.gst_number,e.salesorderdtl_gid from smr_trn_tsalesorder a " +
                        "  left join crm_mst_tcustomer b on b.customer_gid = a.customer_gid " +
                        " left join rbl_trn_tinvoice h on h.invoice_reference=a.salesorder_gid " +
                        "  left join crm_mst_tcustomercontact c on c.customer_gid = a.customer_gid " +
                        "  left join hrm_mst_tbranch d on d.branch_gid = a.branch_gid " +
                        " left join smr_trn_tsalesorderdtl e on e.salesorder_gid = a.salesorder_gid " +
                        "  where a.salesorder_gid = '" + salesorder_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<raisedelivery_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new raisedelivery_list
                        {

                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            customer_mobile = dt["customer_mobile"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            customer_branchname = dt["customer_name"].ToString(),
                            customer_branch = dt["customerbranch_name"].ToString(),
                            customercontact_names = dt["customercontact_name"].ToString(),
                            customer_details = dt["customer_details"].ToString(),
                            customer_email = dt["customer_email"].ToString(),
                            customer_address_so = dt["customer_address_so"].ToString(),
                            customer_address = dt["customer_address_so"].ToString(),
                            so_referencenumber = dt["so_referenceno1"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            salesorderdtl_gid = dt["salesorderdtl_gid"].ToString(),
                        });
                        values.raisedelivery_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Raising Deilvery Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void Damodeofdispatchdropdown(MdlImsTrnDeliveryordersummary values)
        {
            msSQL = "select id , courierservice_id,name from crm_smm_tmintsoftcourierservice";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<modeofdispatch_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new modeofdispatch_list
                    {
                        id = dt["id"].ToString(),
                        courierservice_id = dt["courierservice_id"].ToString(),
                        name = dt["name"].ToString(),
                       
                    });
                    values.modeofdispatch_list = getModuleList;
                }
            }

            dt_datatable.Dispose();
        }
    

    
        public void DaGetProductdelivery(string salesorder_gid, string employee_gid, MdlImsTrnDeliveryordersummary values)
        {
            try
            {
                //msSQL = " delete from ims_tmp_tstock " +
                //    " where created_by = '" + employee_gid + "'";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "select distinct y.branch_name from smr_trn_tsalesorder a " +
                  "left join crm_mst_tcustomercontact c on c.customer_gid = a.customer_gid " +
                  "left join hrm_mst_tbranch y on y.branch_gid = a.branch_gid " +
                    " where salesorder_gid = '" + salesorder_gid + "' ";
                string branch_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select branch_gid from hrm_mst_tbranch where branch_name = '" + branch_name + "' ";
                string branch_gid = objdbconn.GetExecuteScalar(msSQL);




                msSQL = " SELECT   e.producttype_name, a.salesorderdtl_gid, k.stock_quantity, c.design_no,c.color_name, a.salesorder_gid,a.product_gid,k.tmpstock_gid, " +
                    " DATE_FORMAT(a.product_requireddate, '%d-%m-%Y') AS product_requireddate, z.productgroup_gid,z.productgroup_name,a.product_name, " +
                    " a.uom_gid,a.uom_name, a.qty_quoted,a.display_field,a.product_delivered,FORMAT(a.product_price, 2) AS product_price, " +
                    " a.discount_percentage, FORMAT(a.discount_amount, 2) AS discount_amount, FORMAT(a.tax_amount, 2) AS tax_amount, " +
                    " FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3, a.tax_name, a.tax_name2, a.tax_name3, FORMAT(a.price, 2) AS price, " +
                    " b.stockable,a.product_remarks,a.customerproduct_code,(SELECT   IFNULL(SUM(m.stock_qty) + SUM(m.amend_qty) - SUM(m.damaged_qty) - SUM(m.issued_qty) - SUM(m.transfer_qty), " +
                    " 0) AS available_quantity   FROM ims_trn_tstock m WHERE  m.stock_flag = 'Y'  AND m.product_gid = b.product_gid " +
                    " AND  m.branch_gid = '" + branch_gid + "') AS available_quantity, b.serial_flag, b.producttype_gid, " +
                    " a.tax1_gid, a.tax2_gid, a.tax3_gid, b.product_code FROM smr_trn_tsalesorderdtl a LEFT JOIN(SELECT salesorderdtl_gid, " +
                    " tmpstock_gid, stock_quantity FROM ims_tmp_tstock WHERE (product_gid, tmpstock_gid) IN (SELECT product_gid, " +
                    " MAX(tmpstock_gid) AS max_tmpstock_gid FROM ims_tmp_tstock GROUP BY product_gid) " +
                    " ) k ON k.salesorderdtl_gid = a.salesorderdtl_gid " +
                    " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                    " LEFT JOIN pmr_mst_tproductgroup z ON b.productgroup_gid = z.productgroup_gid " +
                    " LEFT JOIN acp_trn_torderdtl c ON c.salesorderdtl_gid = a.salesorderdtl_gid " +
                    " left join pmr_mst_tproducttype e on e.producttype_gid = b.producttype_gid " +
                    " WHERE a.salesorder_gid = '" + salesorder_gid + "' GROUP BY a.salesorderdtl_gid,   a.product_gid " +
                    " ORDER BY a.salesorderdtl_gid ASC, k.tmpstock_gid DESC ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<raisedelivery_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new raisedelivery_list
                        {

                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            salesorderdtl_gid = dt["salesorderdtl_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            customerproduct_code = dt["customerproduct_code"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            //product_desc = dt["product_desc"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            qty_issued = dt["stock_quantity"].ToString(),
                            available_quantity = dt["available_quantity"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            product_delivered = dt["product_delivered"].ToString(),
                            product_requireddate = dt["product_requireddate"].ToString(),
                            producttype_name = dt["producttype_name"].ToString(),


                        });
                        values.raisedelivery_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Delivery !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetOutstandingQty(string salesorder_gid, string salesorderdtl_gid, MdlImsTrnDeliveryordersummary values)
        {
            try
            {

                //    msSQL = "select salesorderdtl_gid from smr_trn_tsalesorderdtl where salesorder_gid = '" + salesorderdtl_gid + "' ";
                //string lssalesorderdtl_gid = objdbconn.GetExecuteScalar(msSQL);


                msSQL = "select (a.qty_quoted-a.product_delivered) as outstanding_qty,a.salesorderdtl_gid,a.product_gid,a.uom_gid," +
                    " a.display_field,b.product_name,a.product_remarks,c.productuom_name,b.branch_gid from smr_trn_tsalesorderdtl a" +
                    " left join pmr_mst_tproduct b on a.product_gid=b.product_gid" +
                    " left join pmr_mst_tproductuom c on a.uom_gid=c.productuom_gid" +
                    " where  a.salesorderdtl_gid = '" + salesorderdtl_gid + "' group by a.salesorderdtl_gid ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<OutstandingQty_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new OutstandingQty_list
                        {

                            outstanding_qty = dt["outstanding_qty"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            uom_name = dt["productuom_name"].ToString(),
                            //product_desc = dt["product_desc"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            salesorderdtl_gid = dt["salesorderdtl_gid"].ToString()
                        });
                        values.OutstandingQty_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Outstanding Quantity !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaIssueFromStock(string product_gid, string salesorder_gid, string salesorderdtl_gid, MdlImsTrnDeliveryordersummary values)
        {
            try
            {

                //    msSQL = "select salesorderdtl_gid from smr_trn_tsalesorderdtl where salesorder_gid = '" + salesorder_gid + "' ";
                //string lssalesorderdtl_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select y.branch_gid  from smr_trn_tsalesorderdtl a " +
                        "left join smr_trn_tsalesorder b on a.salesorder_gid = b.salesorder_gid " +
                        "left join crm_mst_tcustomercontact c on c.customer_gid = b.customer_gid " +
                        "left join hrm_mst_tbranch y on y.branch_gid = b.branch_gid " +
                        "where salesorderdtl_gid = '" + salesorderdtl_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    lsbranch_gid = objOdbcDataReader["branch_gid"].ToString();
                    objOdbcDataReader.Close();
                }

                msSQL = " select DATE_FORMAT(a.created_date, '%d-%m-%Y') as created_date, a.branch_gid,a.stock_gid,a.product_gid,a.display_field,a.uom_gid,a.reference_gid," +
                          " sum(a.stock_qty+amend_qty-a.issued_qty-damaged_qty-transfer_qty)as stock_qty,b.product_name,c.productuom_name" +
                          " from ims_trn_tstock a" +
                          " left join pmr_mst_tproduct b on a.product_gid=b.product_gid" +
                          " left join pmr_mst_tproductuom c on a.uom_gid=c.productuom_gid" +
                          " where a.product_gid='" + product_gid + "'" +
                          " and a.stock_flag='Y'" +
                          " and a.branch_gid='" + lsbranch_gid + "' " +
                          " order by date(a.created_date) asc,a.created_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<IssuedQty_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new IssuedQty_list
                        {

                            created_date = dt["created_date"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            display = dt["display_field"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            reference_gid = dt["reference_gid"].ToString(),
                            stock_qty = dt["stock_qty"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            uom_name = dt["productuom_name"].ToString(),
                        });
                        values.IssuedQty_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Issue From Stock !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostSelectIssueQtySubmit(string employee_gid, IssuedQty_list values)
        {
            try
            {
                if (Convert.ToDouble(values.total_amount) <= Convert.ToDouble(values.outstanding_qty))
                {
                    if (Convert.ToDouble(values.total_amount) <= Convert.ToDouble(values.stock_qty))
                    {
                        msSQL = " select * from ims_tmp_tstock where stock_gid='" + values.stock_gid + "' and product_gid='" + values.product_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count == 0)
                        {
                            msSQL = " insert into ims_tmp_tstock(" +
                                " stock_gid," +
                                " salesorderdtl_gid, " +
                                " product_gid," +
                                " stock_quantity," +
                                " created_by," +
                                " created_date," +
                                " branch_gid," +
                                " productuom_gid," +
                                " mrdtl_gid," +
                                " display_field" + ") " +
                                " values (" +
                                "'" + values.stock_gid + "'," +
                                "'" + values.salesorderdtl_gid + "'," +
                                "'" + values.product_gid + "'," +
                                "'" + values.total_amount + "'," +
                                "'" + employee_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'" + values.branch_gid + "'," +
                                "'" + values.uom_gid + "'," +
                                "'" + values.mrdtl_gid + "'," +
                                "'" + values.display_field + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Despatch Qty Updated";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Facing issue in Despatch Qty Update";
                            }

                        }
                        else
                        {
                            msSQL = " update ims_tmp_tstock set stock_quantity = '" + values.total_amount + "'" +
                                    " where stock_gid = '" + values.stock_gid + "'" +
                                    " and product_gid = '" + values.product_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Despatch Qty Updated";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Facing issue in Despatch Qty Update";
                            }
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Issue Quantity must be Less than or equal to Actual  Quantity";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Issue Quantity must be Less than or equal to Outstanding Quantity";

                }

                //values.txtstocktotal = "0.00";

                //msSQL = "select distinct y.branch_name from smr_trn_tsalesorder a " +
                //       "left join crm_mst_tcustomercontact c on c.customer_gid = a.customer_gid " +
                //       "left join hrm_mst_tbranch y on y.branch_gid = a.branch_gid " +
                //        " where salesorder_gid = '" + values.salesorder_gid + "' ";
                //string branch_name = objdbconn.GetExecuteScalar(msSQL);

                //msSQL = "select branch_gid from hrm_mst_tbranch where branch_name = '" + branch_name + "' ";
                //string branch_gid = objdbconn.GetExecuteScalar(msSQL);
                //msSQL = " SELECT    a.salesorderdtl_gid, k.stock_quantity, c.design_no,c.color_name, a.salesorder_gid,a.product_gid,k.tmpstock_gid, " +
                //           " DATE_FORMAT(a.product_requireddate, '%d-%m-%Y') AS product_requireddate, z.productgroup_gid,z.productgroup_name,a.product_name, " +
                //           " a.uom_gid,a.uom_name, a.qty_quoted,a.display_field,a.product_delivered,FORMAT(a.product_price, 2) AS product_price, " +
                //           " a.discount_percentage, FORMAT(a.discount_amount, 2) AS discount_amount, FORMAT(a.tax_amount, 2) AS tax_amount, " +
                //           " FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3, a.tax_name, a.tax_name2, a.tax_name3, FORMAT(a.price, 2) AS price, " +
                //           " b.stockable, a.customerproduct_code,(SELECT   IFNULL(SUM(m.stock_qty) + SUM(m.amend_qty) - SUM(m.damaged_qty) - SUM(m.issued_qty) - SUM(m.transfer_qty), " +
                //           " 0) AS available_quantity   FROM ims_trn_tstock m WHERE  m.stock_flag = 'Y'  AND m.product_gid = b.product_gid " +
                //           " AND  m.branch_gid = '" + branch_gid + "'   AND m.uom_gid = a.uom_gid  ) AS available_quantity, b.serial_flag, b.producttype_gid, " +
                //           " a.tax1_gid, a.tax2_gid, a.tax3_gid, b.product_code FROM smr_trn_tsalesorderdtl a LEFT JOIN(SELECT salesorderdtl_gid, " +
                //           " tmpstock_gid, stock_quantity FROM ims_tmp_tstock WHERE (product_gid, tmpstock_gid) IN (SELECT product_gid, " +
                //           " MAX(tmpstock_gid) AS max_tmpstock_gid FROM ims_tmp_tstock GROUP BY product_gid) " +
                //           " ) k ON k.salesorderdtl_gid = a.salesorderdtl_gid " +
                //           " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                //           " LEFT JOIN pmr_mst_tproductgroup z ON b.productgroup_gid = z.productgroup_gid " +
                //           " LEFT JOIN acp_trn_torderdtl c ON c.salesorderdtl_gid = a.salesorderdtl_gid " +
                //           " WHERE  a.salesorder_gid = '" + values.salesorder_gid + "' GROUP BY a.salesorderdtl_gid,   a.product_gid " +
                //           " ORDER BY a.salesorderdtl_gid ASC, k.tmpstock_gid DESC ";
                //dt_datatable = objdbconn.GetDataTable(msSQL);
                //var getModuleList = new List<raisedelivery_list>();
                //if (dt_datatable.Rows.Count != 0)
                //{
                //    foreach (DataRow dt in dt_datatable.Rows)
                //    {
                //        getModuleList.Add(new raisedelivery_list
                //        {
                //            salesorder_gid = dt["salesorder_gid"].ToString(),
                //            salesorderdtl_gid = dt["salesorderdtl_gid"].ToString(),
                //            productgroup_name = dt["productgroup_name"].ToString(),
                //            product_gid = dt["product_gid"].ToString(),
                //            customerproduct_code = dt["customerproduct_code"].ToString(),
                //            product_code = dt["product_code"].ToString(),
                //            product_name = dt["product_name"].ToString(),
                //            display_field = dt["display_field"].ToString(),
                //            uom_name = dt["uom_name"].ToString(),
                //            uom_gid = dt["uom_gid"].ToString(),
                //            qty_issued = dt["stock_quantity"].ToString(),
                //            available_quantity = dt["available_quantity"].ToString(),
                //            qty_quoted = dt["qty_quoted"].ToString(),
                //            product_delivered = dt["product_delivered"].ToString(),
                //            product_requireddate = dt["product_requireddate"].ToString(),

                //        });
                //        values.raisedelivery_list = getModuleList;
                //    }
                //}
                //if (mnResult != 1)
                //{
                //    values.status = false;
                //    values.message = " Error While Delivery Order Quantity";
                //}
                //dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Issue Quantity !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // Overall Submit 

        string xDate, createDate, lssendername, lssenderdesignation, lssender_contactnumber, lsbranch, lsproductcode, lsproduct, lsuomname, lsstockgid, lsproductgid, lsproductuomgid, lsstockquantity, lstrnstockquantity;
        public void DaPostDeliveryOrderSubmit(MdlImsTrnDeliveryorder values, string employee_gid)
        {
            try
            {
                //msSQL = " select * from ims_tmp_tstock where salesorderdtl_gid='" + values.salesorderdtl_gid1 + "'";
                //dt_datatable = objdbconn.GetDataTable(msSQL);
                //if (dt_datatable.Rows.Count != 0)
                //{
                msSQL = " select * from hrm_mst_temployee where employee_gid='" + employee_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    lssendername = objOdbcDataReader["employee_gid"].ToString();
                    lssenderdesignation = objOdbcDataReader["designation_gid"].ToString();
                    lssender_contactnumber = objOdbcDataReader["employee_mobileno"].ToString();
                    lsbranch = objOdbcDataReader["branch_gid"].ToString();
                    objOdbcDataReader.Close();
                }

                string uiDateStr2 = values.salesorder_date;
                DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string salesorder_date = uiDate2.ToString("yyyy-MM-dd");

                //msSQL = "select name from crm_smm_tmintsoftcourierservice where id='"+values.customer_mode + "'";
                //string mode =objdbconn.GetExecuteScalar(msSQL);
                mssalesorderGID = objcmnfunctions.GetMasterGID("VDOP");
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
                    " ) values (" +
                    "'" + mssalesorderGID + "'," +
                    "'" + salesorder_date + "'," +
                    "'" + mssalesorderGID + "'," +
                    "'" + values.salesorder_gid + "'," +
                    "'" + values.invoice_gid + "'," +
                    "'" + values.customer_gid + "'," +
                    "'" + values.customer_name.Replace("'", "\\\'") + "'," +
                    "'" + lsbranch + "'," +
                    "'" + values.customer_name.Replace("'", "\\\'") + "'," +
                    "'" + values.customercontact_names.Replace("'", "\\\'") + "'," +
                    "'" + values.customer_mobile + "'," +
                    " '" + values.customer_address.Replace("'", "\\\'") + "'," +
                    "'Despatch Done'," +
                    "'" + values.template_content.Replace("'", "\\\'") + "', " +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                    "'" + employee_gid + "'," +
                    "'" + lssendername + "'," +
                    "'" + employee_gid + "'," +
                    "'" + values.dc_no + "'," +
                    "'" + values.customer_mode + "'," +
                    "'" + values.tracker_id + "'," +
                    "'" + lssenderdesignation + "'," +
                    "'" + lssender_contactnumber + "',";
                if (values.grandtotalamount == null || values.grandtotalamount == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.grandtotalamount + "',";
                }
                msSQL += "'" + salesorder_date + "'," +
                "'" + values.customer_address_so.Replace("'", "\\\'") + "'," +
                "'" + values.no_of_boxs + "'," +
                "'" + values.dc_note.Replace("'", "\\\'") + "'," +
                "'" + values.customer_email + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.message = "Error occurred while inserting Records!";
                }
                msSQL = " select a.salesorderdtl_gid as salesorderdtl_gid,a.salesorder_gid,a.product_gid,date_format(a.product_requireddate, '%d-%m-%Y') as product_requireddate," +
                        " z.productgroup_gid,z.productgroup_name,a.product_name,a.uom_gid,a.uom_name,a.qty_quoted," +
                        " a.display_field,a.product_delivered,a.product_price, a.discount_percentage,format(a.discount_amount,2) as discount_amount," +
                        " format(a.tax_amount,2) as tax_amount,format(a.tax_amount2,2) as tax_amount2,format(a.tax_amount3,2) as tax_amount3, " +
                        " a.tax_name,a.tax_name2,a.tax_name3, a.price,b.stockable,a.customerproduct_code,b.product_desc, " +
                        " (select ifnull(sum(m.stock_qty)+sum(m.amend_qty)-sum(m.damaged_qty)-sum(m.issued_qty)-sum(m.transfer_qty),0) as available_quantity from " +
                        " ims_trn_tstock m " +
                        " where m.product_gid=a.product_gid) as available_quantity," +
                        " a.tax1_gid,a.tax2_gid,a.tax3_gid,b.product_code ,c.stock_quantity as stock_quantity " +
                        " from smr_trn_tsalesorderdtl a " +
                        " left join ims_tmp_tstock c on a.product_gid=c.product_gid " +
                        " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                        " left join pmr_mst_tproductgroup z on b.productgroup_gid=z.productgroup_gid" +
                        " WHERE  a.salesorder_gid = '" + values.salesorder_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        if (!string.IsNullOrEmpty(dt["stock_quantity"]?.ToString()) && Convert.ToDecimal(dt["stock_quantity"]) != 0)
                        {
                            if (Convert.ToDecimal(dt["qty_quoted"]) != Convert.ToDecimal(dt["product_delivered"]))
                             {
                            
                                // Update product_delivered in smr_trn_tsalesorderdtl table
                                msSQL = "UPDATE smr_trn_tsalesorderdtl set product_delivered = product_delivered + '" + dt["stock_quantity"] + "'," +
                                    "delivery_quantity = '" + dt["stock_quantity"] + "'," +
                                    "product_status = 'Updated From DO'" +
                                        "WHERE salesorderdtl_gid = '" + dt["salesorderdtl_gid"] + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                // Check if the update was successful
                                if (mnResult != 1)
                                {
                                    continue;
                                }
                                int deliveryQuantity = Math.Min(Convert.ToInt32(dt["stock_quantity"]), Convert.ToInt32(dt["qty_quoted"]));
                                // Insert into smr_trn_tdeliveryorderdtl table
                                msGetGid = objcmnfunctions.GetMasterGID("VDDC");
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
                                        "'" + dt["productgroup_gid"] + "', " +
                                        "'" + dt["productgroup_name"] + "', " +
                                        "'" + dt["product_gid"] + "', " +
                                        "'" + dt["product_name"] + "', " +
                                        "'" + dt["product_code"] + "', " +
                                        "'" + dt["uom_gid"] + "', " +
                                        "'" + dt["uom_name"] + "', " +
                                        "'" + dt["qty_quoted"] + "', ";

                                if (dt["price"] == null || DBNull.Value.Equals(dt["price"]))
                                {
                                    msSQL += "null,";
                                }
                                else
                                {
                                    msSQL += "'" + dt["price"] + "',";
                                }

                                if (dt["product_price"] == null || DBNull.Value.Equals(dt["product_price"]))
                                {
                                    msSQL += "null,";
                                }
                                else
                                {
                                    msSQL += "'" + dt["product_price"] + "',";
                                }

                                msSQL += "'" + dt["salesorderdtl_gid"] + "'," +
                                         "'" + deliveryQuantity + "')"; // Use the calculated delivery quantity

                                // Execute the insert query
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                    }


                }

                // Check if the process was successful
                if (mnResult == 1)
                {
                    values.status = true;
                }
                msSQL = " select * from ims_tmp_tstock where created_by='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                //if (objOdbcDataReader.HasRows)
                //{
                //    objOdbcDataReader.Read();
                //    lsstockgid = objOdbcDataReader["stock_gid"].ToString();
                //    lsproductuomgid = objOdbcDataReader["productuom_gid"].ToString();
                //    objOdbcDataReader.Close();
                //}
                if (dt_datatable.Rows.Count != 0)
                {

                    foreach (DataRow dt in dt_datatable.Rows)
                    {


                        msstockdtlGid = objcmnfunctions.GetMasterGID("ISTP");

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
                                   "'" + dt["display_field"].ToString().Replace("'", "\\\'") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " update ims_trn_tstock set " +
                                " stock_qty = stock_qty - '" + dt["stock_quantity"].ToString() + "' " +
                                " where stock_gid='" + dt["stock_gid"].ToString() + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " select distinct  " +
                               " sum(qty_quoted) as qty_quoted,sum(product_delivered) as product_delivered " +
                               " from smr_trn_tsalesorderdtl where salesorder_gid='" + values.salesorder_gid + "' group by salesorder_gid " +
                               " having(qty_quoted <> product_delivered) ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                        if (objOdbcDataReader.HasRows == true)
                        {
                            msSQL = " update smr_trn_tsalesorder " +
                                    " set salesorder_status= 'Delivery Done Partial' where " +
                                    " salesorder_gid = '" + values.salesorder_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        else
                        {
                            msSQL = " update smr_trn_tsalesorder " +
                                    " set salesorder_status= 'Delivery Completed' where " +
                                    " salesorder_gid = '" + values.salesorder_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = " update smr_trn_tdeliveryorder " +
                                    " set delivery_status ='Delivery Completed' where " +
                                    " salesorder_gid='" + values.salesorder_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }


                        msSQL = "select mintsoft_flag from adm_mst_tcompany";
                        string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                        if (mintsoft_flag == "Y")
                        {
                            StockToMintSoft OBJMintsoftStock = new StockToMintSoft();
                            msSQL = " select * from smr_trn_tminsoftconfig;";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.HasRows == true)
                            {
                                objOdbcDataReader.Read();
                                base_url = objOdbcDataReader["base_url"].ToString();
                                api_key = objOdbcDataReader["api_key"].ToString();
                            }
                            objOdbcDataReader.Close();
                            msSQL = " select customerproduct_code, mintsoftproduct_id from pmr_mst_tproduct where product_gid='" + values.product_gid + "'";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.HasRows == true)
                            {
                                OBJMintsoftStock.SKU = objOdbcDataReader["customerproduct_code"].ToString();
                                OBJMintsoftStock.ProductId = int.Parse(objOdbcDataReader["mintsoftproduct_id"].ToString());
                            }
                            msSQL= "select stock_qty from ims_trn_tstock where stock_gid='" + dt["stock_gid"].ToString()+"'";
                            string qty =objdbconn.GetExecuteScalar(msSQL);
                            OBJMintsoftStock.Quantity = int.Parse(qty);
                            OBJMintsoftStock.WarehouseId = 3;;
                            string json = JsonConvert.SerializeObject(OBJMintsoftStock);
                            JObject jsonObject = JObject.Parse(json);
                            JArray jsonArray = new JArray();
                            jsonArray.Add(jsonObject);
                            string jsonArrayString = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(base_url);
                            var request = new RestRequest("/api/Product/BulkOnHandStockUpdate", Method.POST);
                            request.AddHeader("ms-apikey", api_key);
                            request.AddParameter("application/json", jsonArrayString, ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                List<Class1> objResult = JsonConvert.DeserializeObject<List<Class1>>(response.Content);
                                if (objResult[0].Success)
                                {
                                    var result = objResult[0].ID;
                                }
                            }
                        }
                    }
                }
                        msSQL = " delete from ims_tmp_tstock " +
                                            " where created_by = '" + employee_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 0)
                        {
                            values.status = false;
                            values.message = "Error While Raising Delivery Order";

                        }
                        else
                        {
                            values.status = true;
                            values.message = "Delivery Order Raised Successfully.";
                        }
                        //}
                        //else
                        //{
                        //    values.status = false;
                        //    values.message = "Please fill the Despatch Quantity for atleast one product to Submit";
                        //}
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Delivery Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetImsTrnDeliveryorderSummaryView(string directorder_gid, MdlImsTrnDeliveryordersummary values)
        {
            try
            {

                msSQL = "select a.directorder_refno," +
                    " date_format(a.directorder_date,'%d/%m/%Y') as directorder_date, " +
                    "a.customer_name," +
                    "a.customer_branchname ," +
                    "a.customer_contactperson," +
                    " a.customer_address,  " +
                    " a.dc_note,  " +
                    " a.customer_emailid,  " +
                    " d.gst_number,  " +
                    " a.delivered_remarks," +
                    " a.terms_condition, " +
                    " a.Landline_no, " +
                    " a.customer_department,  " +
                    " a.grandtotal_amount,  " +
                    " a.addon_amount,  " +
                    " a.shipping_to, " +
                    " a.dc_no, " +
                    " e.branch_name, " +
                    " a.mode_of_despatch, " +
                    " a.no_of_boxs, " +
                    " a.tracker_id, " +
                    " case when a.customer_contactnumber is null then b.mobile when a.customer_contactnumber is not null then a.customer_contactnumber end as mobile," +
                    " a.customer_emailid,b.designation " +
                    " from smr_trn_tdeliveryorder a " +
                    " left join smr_trn_tdeliveryorderdtl c on c.directorder_gid=a.directorder_gid " +
                    " left join crm_mst_tcustomercontact b on b.customer_gid=a.customer_gid " +
                     " left join crm_mst_tcustomer d on d.customer_gid=a.customer_gid " +
                     " left join hrm_mst_tbranch e on a.customerbranch_gid=e.branch_gid " +
                    " where a.directorder_gid = '" + directorder_gid + "' group by a.directorder_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<deliveryorderview_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new deliveryorderview_list
                        {

                            directorder_date = dt["directorder_date"].ToString(),
                            directorder_refno = dt["directorder_refno"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            entity = dt["customer_branchname"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            shipping_to = dt["shipping_to"].ToString(),
                            customer_emailid = dt["customer_emailid"].ToString(),
                            dc_no = dt["dc_no"].ToString(),
                            tracker_id = dt["tracker_id"].ToString(),
                            directorder_remarks = dt["delivered_remarks"].ToString(),
                            customer_contactperson = dt["customer_contactperson"].ToString(),
                            mode_of_despatch = dt["mode_of_despatch"].ToString(),
                            terms_condition = dt["terms_condition"].ToString(),
                            dc_note = dt["dc_note"].ToString(),
                            no_of_boxs = dt["no_of_boxs"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            email_id = dt["customer_emailid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                        });
                        values.deliveryorderview_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Delivery Order View !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetImsTrnDeliveryorderProductView(string directorder_gid, MdlImsTrnDeliveryordersummary values)
        {
            try
            {
                msSQL = " select a.productgroup_name,d.design_no,d.color_name,a.product_code, " +
                        " a.product_name,a.product_description,a.productuom_name,a.product_qty as product_qty," +
                        " (a.product_qtydelivered - a.qty_returned) as product_qtydelivered,(a.qty_returned) as qty_return," +
                        " (a.product_qtydelivered) as total_product_qtydelivered,format(a.discount_amount, 2) as discount_amount," +
                        " format(a.tax_amount, 2) as tax_amount,format(a.product_total, 2) as product_total," +
                        " a.product_code as customerproduct_code from smr_trn_tdeliveryorderdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                        " left join smr_trn_tdeliveryorder c on a.directorder_gid = c.directorder_gid " +
                        " left join acp_trn_torderdtl d on d.salesorder_gid = c.salesorder_gid " +
                        " where a.directorder_gid = '" + directorder_gid + "'group by " +
                        " directorderdtl_gid order by directorderdtl_gid asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<deliveryorderview_list1>();
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new deliveryorderview_list1
                        {

                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_description = dt["product_description"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            total_qty_delivered = dt["total_product_qtydelivered"].ToString(),
                            product_qty = dt["product_qty"].ToString(),
                            product_qtydelivered = dt["product_qtydelivered"].ToString(),
                            qty_return = dt["qty_return"].ToString(),
                        });
                        values.deliveryorderview_list1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Delivery Order Product View!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public Dictionary<string, object> DaGetDeliveryOrderRpt(string directorder_gid, MdlImsTrnDeliveryordersummary values)
        {

            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();

            msSQL = " select a.salesorder_gid,h.invoice_refno as DataColumn21, date_format(a.directorder_date,'%d/%m/%Y')  as DataColumn24,c.email as DataColumn25,a.customer_name,a.directorder_gid,date_format(a.directorder_date,'%d-%m-%Y') as directorder_date,a.directorder_refno," +
                           " a.directorder_remarks,a.terms_condition,format(a.product_grandtotal,2) as product_grandtotal, " +
                           " format(a.addon_amount,2) as addon_amount,format(a.addon_discount,2) as addon_discount, " +
                           " format(a.grandtotal_amount,2) as grandtotal_amount,k.so_referenceno1 , " +
                           " k.so_referenceno1 as DataColumn23, a.shipping_to as customer_address2," +
                           " b.customer_name,a.dc_type, a.dc_no, a.mode_of_despatch, a.tracker_id," +
                           " a.customer_address,a.customer_contactperson as DataColumn22, a.customer_emailid as email," +
                           " a.customer_contactnumber as  mobile " +
                           " from smr_trn_tdeliveryorder a " +
                           " left join rbl_trn_tinvoice h on h.invoice_gid=a.invoice_gid " +
                           " left join smr_trn_tsalesorder k on k.salesorder_gid=a.salesorder_gid " +
                           " left join crm_mst_tcustomer b on b.customer_gid=a.customer_gid " +
                           " left join crm_mst_tcustomercontact c on c.customer_gid=b.customer_gid " +
                           " where a.directorder_gid='" + directorder_gid + "' group by a.directorder_gid ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");

            msSQL = " select a.directorderdtl_gid,a.directorder_gid,a.productuom_name,a.product_price, if(f.customerproduct_code='&nbsp;',' ',f.customerproduct_code) as customerproduct_code, " +
                    " a.product_name as product_description, format(a.product_qtydelivered,2) as product_qty, a.tax_name,a.tax_name2,a.tax_name3, sum(product_qtydelivered) as sumqtytotal," +
                    " d.dc_no,d.mode_of_despatch,a.tracker_id,c.productgroup_name,a.product_code, " +
                    " a.directorderdtl_gid as tax_amount,a.directorderdtl_gid as tax_amount2,e.design_no,e.color_name, " +
                    " a.directorderdtl_gid as tax_amount3, " +
                    " a.directorderdtl_gid as total_tax_amount " +
                    " from smr_trn_tdeliveryorderdtl a " +
                    " left join pmr_mst_tproduct b on b.product_gid=a.product_gid " +
                    " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid " +
                    " left join smr_trn_tdeliveryorder d on d.directorder_gid=a.directorder_gid " +
                    " left join acp_trn_torderdtl e on e.salesorder_gid=d.salesorder_gid and a.product_gid=e.product_gid " +
                    " left join smr_trn_tsalesorderdtl f on f.salesorderdtl_gid = a.salesorderdtl_gid " +
                    " where a.directorder_gid='" + directorder_gid + "' group by directorderdtl_gid order by directorderdtl_gid asc ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");

            msSQL = "select a.branch_name,a.address1,a.city,a.state,a.postal_code,concat(n.user_firstname,'',n.user_lastname) as DataColumn11,a.contact_number,a.email as email_address,a.gst_no, " +
                                  "a.branch_gid,a.branch_logo from hrm_mst_tbranch a " +
                                  "left join hrm_mst_temployee b on a.branch_gid=b.branch_gid " +
                                  "left join adm_mst_tuser n on b.user_gid=n.user_gid " +
                                  "left join smr_trn_tdeliveryorder c on c.created_name=b.employee_gid " +
                                  "where c.directorder_gid='" + directorder_gid + "'";


            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable3");

            msSQL = " select a.branch_logo_path as company_logo,a.authorized_sign_path as auth_sign from hrm_mst_tbranch a " +
              " left join hrm_mst_temployee b on a.branch_gid=b.branch_gid " +
              " left join smr_trn_tdeliveryorder c on c.created_name=b.employee_gid where c.directorder_gid ='" + directorder_gid + "'";

            dt1 = objdbconn.GetDataTable(msSQL);
            DataTable4.Columns.Add("company_logo", typeof(byte[]));
            if (dt1.Rows.Count != 0)
            {
                foreach (DataRow dr_datarow in dt1.Rows)
                {
                    company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["company_logo"].ToString().Replace("../../", ""));

                    if (System.IO.File.Exists(company_logo_path) == true)
                    {
                        //Convert  Image Path to Byte
                        company_logo = System.Drawing.Image.FromFile(company_logo_path);
                        byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(company_logo, typeof(byte[]));

                        DataTable4.Rows.Add(bytes);
                    }
                }
            }
            dt1.Dispose();
            DataTable4.TableName = "DataTable4";
            myDS.Tables.Add(DataTable4);
            msSQL = "SELECT company_code from adm_mst_Tcompany";
            string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            if (lscompany_code == "BOBA")
            {
                ReportDocument oRpt = new ReportDocument();
                oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_inventory"].ToString(), "DeleiveryNote.rpt"));
                oRpt.SetDataSource(myDS);
                string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Delivery Order_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
                oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
                myConnection.Close();

                var ls_response = objFnazurestorage.reportStreamDownload(path);
                File.Delete(path);
                return ls_response;
            }
            else
            {
                ReportDocument oRpt = new ReportDocument();
                oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_inventory"].ToString(), "DeleiveryNoteIndia.rpt"));
                oRpt.SetDataSource(myDS);
                string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Delivery Order_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
                oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
                myConnection.Close();

                var ls_response = objFnazurestorage.reportStreamDownload(path);
                File.Delete(path);
                return ls_response;
            }
        }
        public void DaGetDOsixmonthschart(MdlImsTrnDeliveryordersummary values)
        {


            msSQL = "  select  DATE_FORMAT(directorder_date, '%b-%Y')  as directorder_date,substring(date_format(a.directorder_date,'%M'),1,3)as month, "+
                    "  a.directorder_gid,year(a.directorder_gid) as year,count(a.directorder_gid) as ordercount,date_format(directorder_date, '%M/%Y') as month_wise " +
                    "  from smr_trn_tdeliveryorder a where a.directorder_date > date_add(now(), interval - 6 month) " +
                    "  and a.directorder_date <= date(now()) group by date_format(a.directorder_date, '%M') and a.dc_type<>'Direct DC' order by a.directorder_date desc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var Dolastsixmonths_list = new List<Dolastsixmonths_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    Dolastsixmonths_list.Add(new Dolastsixmonths_list
                    {
                        directorder_date = (dt["directorder_date"].ToString()),
                        months = (dt["month"].ToString()),
                        ordercount = (dt["ordercount"].ToString()),

                    });
                    values.Dolastsixmonths_list = Dolastsixmonths_list;
                }

            }

            msSQL = " select COUNT(a.salesorder_gid) AS approved_count,(select COUNT(a.directorder_gid)  FROM smr_trn_tdeliveryorder a "+
                    " WHERE a.directorder_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH)  AND a.directorder_date <= DATE(NOW()) AND a.dc_type<>'Direct DC' ) as order_count FROM smr_trn_tsalesorder a " +
                    " WHERE  a.salesorder_date > DATE_ADD(NOW(), INTERVAL - 6 MONTH)  AND a.salesorder_date <= DATE(NOW())";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                values.ordertodocount = objOdbcDataReader["order_count"].ToString();
                values.ordercount = objOdbcDataReader["approved_count"].ToString();
            }

        }

        public void DaGetDetialViewProduct(string directorder_refno, MdlImsTrnDeliveryordersummary values)
        {
            try
            {

                msSQL = "select a.product_qty,a.product_code,a.product_name,a.product_qtydelivered,a.qty_returned,(a.product_qtydelivered-a.qty_returned) as actual, c.productuom_name " +
                "from smr_trn_tdeliveryorderdtl a " +
                "left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                "left join pmr_mst_tproductuom c on a.product_uom_gid = c.productuom_gid " +
                "where a.directorder_gid='" + directorder_refno + "' order by a.directorderdtl_gid asc";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<product_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_list
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_qty = dt["product_qty"].ToString(),
                            product_qtydelivered = dt["product_qtydelivered"].ToString(),
                            qty_returned = dt["qty_returned"].ToString(),
                            actual = dt["actual"].ToString(),
                        });
                        values.product_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Issue Request Data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }
}