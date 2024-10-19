using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.inventory.Models;
using System.Windows.Media.Media3D;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Drawing;


namespace ems.inventory.DataAccess
{
    public class DaImsTrnOpenDcSummary
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader, objODBCDataReader;
        DataTable dt_datatable;
        DataTable dt1 = new DataTable();
        DataTable DataTable4 = new DataTable();        
        string company_logo_path, authorized_sign_path;
        Image company_logo, DataColumn14;
        string msGetGid, msGetGid1, lsempoyeegid, mnCtr, lsbranch, lscostcenter, msIssueGID, lsemployeegid, lslocation, msGetGID, msGetImRC, lsmainbranch,msGetPodc, mcGetGID, lsStockGid, lsStockQty, msstockdtlGid, msGetStockTrackerGID, lsmaterialissued_date;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsdirectorder_date, lsproduct_qty, msstockGID, lsproduct_desc, lsreference_gid, lssendername, lssender_contactnumber, lssenderdesignation, mssalesorderGID, lsuom_gid, lsbranch_gid, lsCode, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        double lsbudgetallocated, lsprovisional, lsamtused, lsavailable, lsreq, lstolrequest, lsreq1, lsrequested;
        string lsproduct_gid, lsproductuom_gid, lsproduct_remarks, lsavailable2, lsproduct_code, lsproduct_name, lsproductuom_name, lsproductgroup_gid, lsproductgroup_name,
            lsqty_requested, lsdisplay_field,lsquantity_delivered;
        List<int> quantities = new List<int>();
        public void DaGetImsTrnOpenDeliveryOrderSummary(MdlImsTrnOpenDCSummary values)
        {
            try
            {

                msSQL = " select directorder_gid, directorder_refno, date_format(directorder_date, '%d-%b-%Y') as directorder_date,n.user_firstname, " +
               " a.customer_name, a.customer_branchname, a.customer_contactperson,a.created_by,a.directorder_status,a.delivery_status, " +
               " concat(CAST(date_format(delivered_date,'%d-%m-%Y') as CHAR),'/',delivered_to) as delivery_details, " +
               " concat(a.customer_contactperson,' / ',a.customer_contactnumber,' / ',a.customer_emailid)  as contact " +
               " from smr_trn_tdeliveryorder a " +
               " left join hrm_mst_temployee m on m.employee_gid=a.created_name " +
               " left join adm_mst_tuser n on n.user_gid= m.user_gid " +
               " where dc_type='Direct DC' order by date(a.directorder_date) desc,a.directorder_date asc, a.directorder_gid desc ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<opndcsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new opndcsummary_list
                        {
                            directorder_gid = dt["directorder_gid"].ToString(),
                            directorder_refno = dt["directorder_refno"].ToString(),
                            directorder_date = dt["directorder_date"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_branchname = dt["customer_branchname"].ToString(),
                            customer_contactperson = dt["customer_contactperson"].ToString(),
                            directorder_status = dt["directorder_status"].ToString(),
                            delivery_status = dt["delivery_status"].ToString(),
                            delivery_details = dt["delivery_details"].ToString(),
                            contact = dt["contact"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            created_by = dt["created_by"].ToString()

                        });
                        values.opndcsummary_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Open Delivery Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetImsTrnOpenDcAddSummary(MdlImsTrnOpenDCSummary values)
        {
            try
            {
                msSQL = " select distinct a.salesorder_gid, a.so_referenceno1, date_format(a.salesorder_date,'%d-%m-%Y') as salesorder_date, " +
                        " sum(b.qty_quoted) as qty_quoted,sum(b.product_delivered) as product_delivered, d.customer_name, c.customercontact_name as customer_contact_person," +
                        " a.salesorder_status,c.mobile,  a.despatch_status,  case when a.customer_email is null then concat(c.customercontact_name,'/',c.mobile,'/',c.email)  when a.customer_email" +
                        " is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact,d.customer_id  from smr_trn_tsalesorder a  " +
                        " left join smr_trn_tsalesorderdtl b on b.salesorder_gid = a.salesorder_gid  " +
                        " left join crm_mst_tcustomercontact c on c.customer_gid=a.customer_gid " +
                        " left join crm_mst_tcustomer d on d.customer_gid=a.customer_gid where a.salesorder_status not in ('Approve Pending','SO Amended','Rejected','Canceled')  and so_type='Sales' " +
                        " group by salesorder_gid  having(qty_quoted <> product_delivered)  order by a.salesorder_date desc, a.customer_name desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<opendcadd_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new opendcadd_list
                        {
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            so_referenceno1 = dt["salesorder_gid"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            product_delivered = dt["product_delivered"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            despatch_status = dt["despatch_status"].ToString(),
                            contact = dt["contact"].ToString(),
                            customer_id = dt["customer_id"].ToString(),
                        });
                        values.opendcadd_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Open DC Add !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // Add page
        public void DaGetOpenDcUpdate(string salesorder_gid, MdlImsTrnOpenDCSummary values)
        {
            try
            {

                 msSQL = " select date_format(d.directorder_date, '%d-%m-%Y') as directorder_date,d.directorder_refno,a.salesorder_gid,a.salesorder_date,a.termsandconditions,b.customer_gid,b.customer_code,format(a.grandtotal,2) as grandtotal, b.customer_name," +
                    " concat(b.customer_address,b.customer_address2,b.customer_city,b.customer_state,b.customer_pin) as customer_address," +
                    " c.designation,c.customercontact_name,c.email,c.mobile,a.currency_code,a.shipping_to, " +
                    " c.mobile as customer_mobile,c.email as customer_email,a.customer_address as customer_address_so, " +
                    " c.customercontact_name as customer_contact_person,a.shipping_to from smr_trn_tsalesorder a" +
                    " left join crm_mst_tcustomer b on b.customer_gid=a.customer_gid " +
                    " left join crm_mst_tcustomercontact c on c.customer_gid=a.customer_gid " +
                    " left join smr_trn_tdeliveryorder d on a.salesorder_gid= d.salesorder_gid " +
                    " where a.salesorder_gid='" + salesorder_gid + "' group by salesorder_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<opendcaddsel_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new opendcaddsel_list
                        {

                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_code = dt["customer_code"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            grandtotal = dt["grandtotal"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            designation = dt["designation"].ToString(),
                            customercontact_name = dt["customercontact_name"].ToString(),
                            email = dt["email"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            shipping_to = dt["shipping_to"].ToString(),
                            customer_mobile = dt["customer_mobile"].ToString(),
                            customer_email = dt["customer_email"].ToString(),
                            customer_address_so = dt["customer_address_so"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            directorder_refno = dt["directorder_refno"].ToString(),
                            directorder_date = dt["directorder_date"].ToString(),


                        });
                        values.opendcaddsel_list = getModuleList;

                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Open Dc !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetOpenDcUpdateProd(string salesorder_gid, MdlImsTrnOpenDCSummary values)
        {
            try
            {
                msSQL = "select salesorderdtl_gid from smr_trn_tsalesorderdtl where salesorder_gid = '" + salesorder_gid + "' ";
                string lssalesorderdtl_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select y.branch_gid ,a.uom_gid from smr_trn_tsalesorderdtl a " +
                        "left join smr_trn_tsalesorder b on a.salesorder_gid = b.salesorder_gid " +
                        "left join crm_mst_tcustomercontact c on c.customer_gid = b.customer_gid " +
                        "left join hrm_mst_tbranch y on y.branch_gid = b.branch_gid " +
                        "where salesorderdtl_gid = '" + lssalesorderdtl_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    lsbranch_gid = objOdbcDataReader["branch_gid"].ToString();
                    objOdbcDataReader.Close();
                }

                msSQL = " select a.salesorderdtl_gid,a.salesorder_gid,b.product_gid,c.productgroup_gid,c.productgroup_name,b.product_code, "+
                        " b.product_desc,b.product_name,a.uom_gid,a.uom_name,a.qty_quoted, a.display_field,a.product_delivered, "+
                        " format(a.product_price,2) as product_price, a.discount_percentage,format(a.discount_amount,2) as discount_amount, "+
                        " format(a.tax_amount,2) as tax_amount,format(a.tax_amount2,2) as tax_amount2,format(a.tax_amount3,2) as tax_amount3, "+  
                        " a.tax_name,a.tax_name2,a.tax_name3,format(a.price,2) as price,b.stockable,  "+
                        " (select ifnull(sum(m.stock_qty)+sum(m.amend_qty)-sum(m.damaged_qty)-sum(m.issued_qty)-sum(m.transfer_qty),0) as available_quantity from  "+
                        " ims_trn_tstock m where m.stock_flag='Y' and m.product_gid=a.product_gid and m.branch_gid='"+lsbranch_gid+"' and  " +
                        " m.uom_gid=a.uom_gid) as available_quantity,b.serial_flag,b.branch_gid,a.product_remarks,  a.tax1_gid,a.tax2_gid,a.tax3_gid "+
                        " from smr_trn_tsalesorderdtl a  left join smr_trn_tdeliveryorderdtl z on z.product_gid=a.product_gid "+
                        " left join pmr_mst_tproduct b on a.product_gid=b.product_gid "+
                        " left join pmr_mst_tproductgroup c on c.productgroup_gid = a.productgroup_gid   "+
                        " where a.salesorder_gid = '"+salesorder_gid+"' group by salesorderdtl_gid order by salesorderdtl_gid asc  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<opendcaddselprod_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new opendcaddselprod_list
                        {
                            salesorderdtl_gid = dt["salesorderdtl_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            product_delivered = dt["product_delivered"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax1_gid = dt["tax_name"].ToString(),
                            tax2_gid = dt["tax_name2"].ToString(),
                            tax3_gid = dt["tax_name3"].ToString(),
                            price = dt["price"].ToString(),
                            stockable = dt["stockable"].ToString(),
                            available_quantity = dt["available_quantity"].ToString(),
                            serial_flag = dt["serial_flag"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                        });
                        values.opendcaddselprod_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Opening DC Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetOnChangeTerms(string template_gid, MdlImsTrnOpenDCSummary values)
        {
            try
            {

                if (template_gid != null)
                {
                    msSQL = " select template_gid, template_name, template_content from adm_mst_ttemplate where template_gid='" + template_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetTermDropdown>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetTermDropdown
                            {
                                template_gid = dt["template_gid"].ToString(),
                                template_name = dt["template_name"].ToString(),
                                termsandconditions = dt["template_content"].ToString(),
                            });
                            values.terms_list = getModuleList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Template Content!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        // Submit Event
        public void DaPostOpenDcSubmit(string employee_gid, opendc_list values)
        {
            try
            {

                if (Convert.ToDouble(values.despatch_quantity) <= Convert.ToDouble(values.available_quantity))
                {
                    values.message = "Sum of the despatch quantity and delivered quantity must be less than or equal to the ordered quantity";
                }

                msSQL = " select * from hrm_mst_temployee where employee_gid='" + employee_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lssendername = objOdbcDataReader["employee_gid"].ToString();
                    lssenderdesignation = objOdbcDataReader["designation_gid"].ToString();
                    lssender_contactnumber = objOdbcDataReader["employee_mobileno"].ToString();

                }



                //msSQL = "SELECT salesorderdtl_gid  FROM smr_trn_tsalesorderdtl WHERE product_name = '" + values.product_name + "'";
                //string lssalesorderdtlgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select salesorderdtl_gid from smr_trn_tsalesorderdtl where salesorder_gid = '" + values.salesorder_gid + "' ";
                string lssalesorderdtl_gid = objdbconn.GetExecuteScalar(msSQL);


                msSQL = "SELECT salesorder_gid  FROM smr_trn_tsalesorderdtl WHERE salesorderdtl_gid = '" + lssalesorderdtl_gid + "'";
                string lssalesordergid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "SELECT customer_gid  FROM smr_trn_tsalesorder WHERE salesorder_gid = '" + lssalesordergid + "'";
                string lscustomergid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "SELECT branch_gid  FROM smr_trn_tsalesorder WHERE salesorder_gid = '" + lssalesordergid + "'";
                string lsbranch = objdbconn.GetExecuteScalar(msSQL);
                //msSQL = "SELECT customer_name FROM crm_mst_tcustomer WHERE customerbranch_name = '" + lscustomergid + "'";
                //string lscustomername = objdbconn.GetExecuteScalar(msSQL);

                try { 
                mssalesorderGID = objcmnfunctions.GetMasterGID("VDOP");

                msSQL = " insert into smr_trn_tdeliveryorder (" +
                " directorder_gid, " +
                " directorder_date," +
                " directorder_refno, " +
                " salesorder_gid, " +
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
                " sender_designation," +
                " sender_contactnumber, " +
                " grandtotal_amount, " +
                 " shipping_to, " +
                   " dc_type, " +
                " customer_emailid " +
                " ) values (" +
                "'" + mssalesorderGID + "'," +
                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                "'" + mssalesorderGID + "'," +
                "'" + lssalesordergid + "'," +
                "'" + lscustomergid + "'," +
                "'" + values.customer_name + "'," +
                "'" + lsbranch + "'," +
                "'" + values.customer_code + "'," +
                "'" + values.customer_contact_person + "'," +
                "'" + values.customer_mobile + "'," +
                " '" + values.customer_address_so + "'," +
                "'Despatch Done'," +
                "'" + values.termsandconditions + "', " +
                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                "'" + employee_gid + "'," +
                "'" + lssendername + "'," +
                "'" + lssenderdesignation + "'," +
                "'" + lssender_contactnumber + "',";
                if (values.grandtotal == null || values.grandtotal == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.grandtotal + "',";
                }
                msSQL += "'" + values.shipping_to + "'," +
                     "'Direct DC'," +
                    "'" + values.customer_email + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error occurred while inserting records!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Error occurred while inserting records in Open DC !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
                try
                {
                    msGetGID = objcmnfunctions.GetMasterGID("VDDC");

                    //msSQL = "update smr_trn_tsalesorderdtl set product_delivered='" + values.despatch_quantity + "' where salesorderdtl_gid='" + lssalesorderdtlgid + "'";
                    //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " SELECT productgroup_gid FROM pmr_mst_tproduct WHERE  product_name='" + values.product_name + "' ";
                    string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);
                                                                                 
                    msSQL = " SELECT product_gid FROM pmr_mst_tproduct WHERE product_name='" + values.product_name + "' ";
                    string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " SELECT productuom_gid FROM pmr_mst_tproductuom WHERE productuom_name='" + values.uom_name + "' ";
                    string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " insert into smr_trn_tdeliveryorderdtl (" +
                                    " directorderdtl_gid, " +
                                    " directorder_gid, " +
                                    " productgroup_gid, " +
                                    " productgroup_name, " +
                                    " product_gid," +
                                    " product_name, " +
                                    " product_uom_gid, " +
                                    " productuom_name, " +
                                    " product_qty, " +
                                    " product_description, " +
                                    " product_price, " +
                                    " discount_percentage, " +
                                    " discount_amount, " +
                                    " tax_name, " +
                                    " tax_name2, " +
                                    " product_total, " +
                                    " tax_name3, " +
                                     " dc_no, " +
                                    " mode_of_despatch, " +
                                    " tracker_id, " +
                                    " tax_amount, " +
                                    " tax_amount2, " +
                                    " tax_amount3, " +
                                    " tax1_gid, " +
                                    " tax2_gid, " +
                                    " tax3_gid, " +
                                    " product_qtydelivered, " +
                                    " salesorderdtl_gid " +
                                    "  ) " +
                                    " values ( " +
                                    "'" + msGetGID + "', " +
                                    "'" + mssalesorderGID + "'," +
                                    "'" + lsproductgroupgid + "', " +
                                    "'" + values.productgroup_name + "', " +
                                    "'" + lsproductgid + "', " +
                                    "'" + values.product_name + "', " +
                                    "'" + lsproductuomgid + "', " +
                                    "'" + values.uom_name + "'," +
                                     "'" + values.qty_quoted + "', " +
                                    "'" + values.display_field + "',";
                    if (values.price == null || values.price == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.price + "',";
                    }
                    if (values.discount_percentage == null || values.discount_percentage == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.discount_percentage + "',";
                    }
                    if (values.discount_amount == null || values.discount_amount == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.discount_amount + "',";
                    }
                    msSQL += "'" + values.tax_name + "'," +
                                     "'" + values.tax_name2 + "',";
                    if (values.total_amount == null || values.total_amount == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.total_amount + "',";
                    }
                    msSQL += "'" + values.tax_name3 + "'," +
                                    "'" + values.dc_no + "'," +
                                    "'" + values.despatch_mode + "'," +
                                    "'" + values.tracker_id + "',";
                    if (values.tax_amount == null || values.tax_amount == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_amount + "',";
                    }

                    if (values.tax_amount2 == null || values.tax_amount2 == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_amount2 + "',";
                    }

                    if (values.tax_amount3 == null || values.tax_amount3 == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_amount3 + "',";
                    }
                    msSQL += "'" + values.tax1_gid + "'," +
                                       "'" + values.tax2_gid + "'," +
                                        "'" + values.tax3_gid + "'," +
                                    "'" + values.despatch_quantity + "'," +
                                    "'" + lssalesorderdtl_gid + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    //if (mnResult == 1)
                    //{
                    //    msSQL = " select directorderdtl_gid, directorder_gid, productgroup_gid, productgroup_name, " +
                    //                  " product_gid, product_name, product_uom_gid, productuom_name, product_qty, " +
                    //                  " product_description, product_price, discount_percentage, discount_amount, " +
                    //                  " tax_name, tax_name2, tax_name3, tax_percentage, tax_percentage2, tax_percentage3, " +
                    //                  " tax_amount, tax_amount2, tax_amount3, product_total, total_amount, product_qtydelivered, dc_no, " +
                    //                    " mode_of_despatch, tracker_id, qty_returned, tax1_gid, tax2_gid, tax3_gid, " +
                    //                    " product_remarks, salesorderdtl_gid, warranty_date, created_by, created_date," +
                    //                    " updated_by, updated_date from smr_trn_tdeliveryorderdtl " +
                    //                    " where directorder_gid='" + mssalesorderGID + "' ";

                    //    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    //    if (objOdbcDataReader.HasRows)
                    //    {
                    //        lsproduct_qty = objOdbcDataReader["product_qty"].ToString();
                    //        lsreference_gid = objOdbcDataReader["directorder_gid"].ToString();
                    //        lsproduct_desc = objOdbcDataReader["product_description"].ToString();
                    //    }

                    //    msstockGID = objcmnfunctions.GetMasterGID("ISKP");

                    //    msSQL = " insert into ims_trn_tstock(" +
                    //         " stock_gid," +
                    //         " branch_gid," +
                    //         " product_gid," +
                    //         " uom_gid," +
                    //         " created_by," +
                    //         " created_date," +
                    //         " unit_price," +
                    //         " stock_qty," +
                    //         " reference_gid, " +
                    //         " stock_flag, " +
                    //         " display_field " +
                    //         " ) values (" +
                    //         "'" + msstockGID + "'," +
                    //         "'" + lsbranch + "'," +
                    //         "'" + lsproductgid + "'," +
                    //         "'" + lsproductuomgid + "'," +
                    //         "'" + employee_gid + "'," +
                    //         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "', ";
                    //    if (values.price == null || values.price == "")
                    //    {
                    //        msSQL += "'0.00',";
                    //    }
                    //    else
                    //    {
                    //        msSQL += "'" + values.price + "',";
                    //    }
                    //    msSQL += "'" + lsproduct_qty + "'," +
                    //         "'" + lsreference_gid + "'," +
                    //         "'Y'," +
                    //         "'" + lsproduct_desc + "')";

                    //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    //}

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Stock Details Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Stock Details";

                    }
                
                 }
            catch (Exception ex)
            {
                values.message = "Exception occured While Adding Stock Details in Open DC !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Open DC !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           

        }
        public Dictionary<string, object> DaGetOpenDCpdf(string directorder_gid, MdlImsTrnOpenDCSummary values)
        {

            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();
            Fnazurestorage objFnazurestorage = new Fnazurestorage();
            msSQL = " select  directorder_gid,date_format(directorder_date,'%d-%m-%Y') as directorder_date,directorder_refno,directorder_remarks,directorder_status,delivery_status,delivered_date, " +
                    " shipping_to,dc_type,mode_of_despatch as DataColumn25 ,dc_no, dc_note, no_of_boxs, branch_name " +
                    " from ims_trn_tdeliveryorder " +
                    " where directorder_gid='" + directorder_gid + "' group by directorder_gid ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");

            msSQL = " select directorder_gid,productgroup_name,product_gid,product_name,product_code,product_qty,product_price,tracker_id,qty_returned, " +
                    " product_description,discount_percentage,discount_amount,tax_name,tax_name2,tax_name3,tax_percentage,tax_percentage2, " +
                    " tax_percentage3,tax_amount,tax_amount2,tax_amount3,product_total,total_amount, " +
                    " tracker_id from ims_trn_tdeliveryorderdtl " +
                    " where directorder_gid='" + directorder_gid + "' " +
                    " group by directorderdtl_gid order by directorderdtl_gid asc ";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");

            msSQL = " select a.branch_name,c.address1,c.city,c.state,c.postal_code,c.email as email_address,c.gst_no, c.branch_gid,c.contact_number " +
                    " from ims_trn_tdeliveryorder a " +
                    " left join hrm_mst_tbranch c on c.branch_gid=c.branch_gid  " +
                    " where a.directorder_gid='" + directorder_gid + "'";


            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable3");

            msSQL = " select a.branch_logo_path as company_logo from hrm_mst_tbranch a " +
                    " left join ims_trn_tdeliveryorder b on b.branch_gid=b.branch_gid  " +
                    " where directorder_gid='" + directorder_gid + "'";
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
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_inventory"].ToString(), "Ims_Rpt_openDeliveryChallan.rpt"));
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Delivery Challan_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();

            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);
            return ls_response;

        }


        //download Report files
        public Dictionary<string, object> DaGetOpenDCRpt(string directorder_gid, MdlImsTrnOpenDCSummary values)
        {
            msSQL = "select company_code from adm_mst_tcompany";
            string lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            if (lscompany_code == "MEDIA" || lscompany_code == "media")
            {
                OdbcConnection myConnection = new OdbcConnection();
                myConnection.ConnectionString = objdbconn.GetConnectionString();
                OdbcCommand MyCommand = new OdbcCommand();
                MyCommand.Connection = myConnection;
                DataSet myDS = new DataSet();
                OdbcDataAdapter MyDA = new OdbcDataAdapter();
                Fnazurestorage objFnazurestorage = new Fnazurestorage();
                msSQL = " select a.directorder_gid,date_format(a.directorder_date,'%d-%m-%Y') as directorder_date,a.directorder_refno," +
                                             " a.directorder_remarks,a.terms_condition,format(a.product_grandtotal,2) as product_grandtotal, " +
                                             " format(a.addon_amount,2) as addon_amount,format(a.addon_discount,2) as addon_discount, " +
                                             " format(a.grandtotal_amount,2) as grandtotal_amount, " +
                                             " k.so_referenceno1 as DataColumn23, a.shipping_to as customer_address2," +
                                             " b.customer_name,a.dc_type, a.dc_no, a.mode_of_despatch, a.tracker_id," +
                                             " a.customer_address,a.customer_contactperson as DataColumn22, a.customer_emailid as email," +
                                             " a.customer_contactnumber as  mobile " +
                                             " from smr_trn_tdeliveryorder a " +
                                             " left join smr_trn_tsalesorder k on k.salesorder_gid=a.salesorder_gid " +
                                             " left join crm_mst_tcustomer b on b.customer_gid=a.customer_gid " +
                                             " left join crm_mst_tcustomercontact c on c.customer_gid=b.customer_gid " +
                                             " where a.directorder_gid='" + directorder_gid + "' group by a.directorder_gid ";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable1");

                msSQL = " select a.directorderdtl_gid,a.directorder_gid,a.productuom_name,a.product_qty,a.product_price, if(f.customerproduct_code='&nbsp;',' ',f.customerproduct_code) as customerproduct_code, " +
                        " a.product_qtydelivered, a.tax_name,a.tax_name2,a.tax_name3, sum(product_qtydelivered) as sumqtytotal," +
                        " a.dc_no,a.mode_of_despatch,a.tracker_id,b.product_code,c.productgroup_name,a.product_name,f.product_remarks as product_description, " +
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

                msSQL = "select a.branch_name,a.address1,a.city,a.state,a.postal_code,a.contact_number,concat(d.user_firstname,'',d.user_lastname) as DataColumn11,a.email as email_address,a.gst_no, " +
                                      "a.branch_gid,a.branch_logo from hrm_mst_tbranch a " +
                                      "left join hrm_mst_temployee b on a.branch_gid=b.branch_gid " +
                                      "left join smr_trn_tdeliveryorder c on c.created_name=b.employee_gid " +
                                      "left join adm_mst_tuser d on b.user_gid=d.user_gid " +
                                      "where c.directorder_gid='" + directorder_gid + "'";


                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable3");

                msSQL = " select a.salesorder_gid,b.salesorder_gid,b.directorder_gid,a.design_no from acp_trn_torderdtl a " +
                           " left join smr_trn_tdeliveryorder b on b.salesorder_gid = a.salesorder_gid " +
                           " where b.directorder_gid='" + directorder_gid + "'";

                //mysql = "select authorized_sign_path as branch_logo ,branch_logo_path as auth_sign from hrm_mst_tbranch";


                //msSQL = "SELECT a.salesorder_gid,b.salesorder_gid,b.directorder_gid,a.design_no, c.branch_logo_path AS branch_logo,c.authorized_sign_path AS sign FROM acp_trn_torderdtl a " +
                //        "LEFT JOIN smr_trn_tdeliveryorder b ON b.salesorder_gid = a.salesorder_gid"+
                //        "LEFT JOIN hrm_mst_tbranch c ON c.branch_gid = b.customerbranch_gid" +
                //        "WHERE b.directorder_gid = '" + directorder_gid + "'";
                //dt1 = objdbconn.GetDataTable(msSQL);
                //DataTable4.Columns.Add("branch_logo", typeof(byte[]));
                //DataTable4.Columns.Add("auth_sign", typeof(byte[]));
                //if (dt1.Rows.Count != 0)
                //{
                //    foreach (DataRow dr_datarow in dt1.Rows)
                //    {
                //        company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["branch_logo"].ToString().Replace("../../", ""));
                //        authorized_sign_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["auth_sign"].ToString().Replace("../../", ""));

                //        if (System.IO.File.Exists(company_logo_path) && System.IO.File.Exists(authorized_sign_path))
                //        {
                //            //Convert  Image Path to Byte
                //            branch_logo = System.Drawing.Image.FromFile(company_logo_path);
                //            auth_sign = System.Drawing.Image.FromFile(authorized_sign_path);
                //            byte[] branch_logo_bytes = (byte[])(new ImageConverter()).ConvertTo(branch_logo, typeof(byte[]));
                //            byte[] auth_sign_bytes = (byte[])(new ImageConverter()).ConvertTo(auth_sign, typeof(byte[]));

                //            DataRow newRow = DataTable4.NewRow();
                //            newRow["branch_logo"] = branch_logo_bytes;
                //            newRow["auth_sign"] = auth_sign_bytes;
                //            DataTable4.Rows.Add(newRow);
                //        }
                //    }
                //}
                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable4");

                ReportDocument oRpt = new ReportDocument();
                oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_inventory"].ToString(), "medialink.rpt"));
                oRpt.SetDataSource(myDS);
                string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Delivery Order_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
                oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
                myConnection.Close();

                var ls_response = objFnazurestorage.reportStreamDownload(path);
                File.Delete(path);
                return ls_response;
            }

            else {
                OdbcConnection myConnection = new OdbcConnection();
                myConnection.ConnectionString = objdbconn.GetConnectionString();
                OdbcCommand MyCommand = new OdbcCommand();
                MyCommand.Connection = myConnection;
                DataSet myDS = new DataSet();
                OdbcDataAdapter MyDA = new OdbcDataAdapter();
                Fnazurestorage objFnazurestorage = new Fnazurestorage();
                msSQL = " select a.directorder_gid,date_format(a.directorder_date,'%d-%m-%Y') as directorder_date,a.directorder_refno," +
                                             " a.directorder_remarks,a.terms_condition,format(a.product_grandtotal,2) as product_grandtotal, " +
                                             " format(a.addon_amount,2) as addon_amount,format(a.addon_discount,2) as addon_discount, " +
                                             " format(a.grandtotal_amount,2) as grandtotal_amount, " +
                                             " k.so_referenceno1 as DataColumn23, a.shipping_to as customer_address2," +
                                             " b.customer_name,a.dc_type, a.dc_no, a.mode_of_despatch, a.tracker_id," +
                                             " a.customer_address,a.customer_contactperson as DataColumn22, a.customer_emailid as email," +
                                             " a.customer_contactnumber as  mobile " +
                                             " from smr_trn_tdeliveryorder a " +
                                             " left join smr_trn_tsalesorder k on k.salesorder_gid=a.salesorder_gid " +
                                             " left join crm_mst_tcustomer b on b.customer_gid=a.customer_gid " +
                                             " left join crm_mst_tcustomercontact c on c.customer_gid=b.customer_gid " +
                                             " where a.directorder_gid='" + directorder_gid + "' group by a.directorder_gid ";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable1");

                msSQL = " select a.directorderdtl_gid,a.directorder_gid,a.productuom_name,a.product_qty,a.product_price, if(f.customerproduct_code='&nbsp;',' ',f.customerproduct_code) as customerproduct_code, " +
                        " a.product_qtydelivered, a.tax_name,a.tax_name2,a.tax_name3, sum(product_qtydelivered) as sumqtytotal," +
                        " a.dc_no,a.mode_of_despatch,a.tracker_id,a.product_code,c.productgroup_name,a.product_name f.product_remarks as product_description, " +
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

                msSQL = "select a.branch_name,a.address1,a.city,a.state,a.postal_code,a.contact_number,concat(d.user_firstname,'',d.user_lastname) as DataColumn11,a.email as email_address,a.gst_no, " +
                                      "a.branch_gid,a.branch_logo from hrm_mst_tbranch a " +
                                      "left join hrm_mst_temployee b on a.branch_gid=b.branch_gid " +
                                      "left join smr_trn_tdeliveryorder c on c.created_name=b.employee_gid " +
                                      "left join adm_mst_tuser d on b.user_gid=d.user_gid " +
                                      "where c.directorder_gid='" + directorder_gid + "'";


                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable3");

                msSQL = " select a.salesorder_gid,b.salesorder_gid,b.directorder_gid,a.design_no from acp_trn_torderdtl a " +
                           " left join smr_trn_tdeliveryorder b on b.salesorder_gid = a.salesorder_gid " +
                           " where b.directorder_gid='" + directorder_gid + "'";

                MyCommand.CommandText = msSQL;
                MyCommand.CommandType = System.Data.CommandType.Text;
                MyDA.SelectCommand = MyCommand;
                myDS.EnforceConstraints = false;
                MyDA.Fill(myDS, "DataTable4");

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

            }
        public void DaGetImsTrnOpenDCSummary(MdlImsTrnOpenDCSummary values)
        {
            try
            {

                msSQL =" select directorder_gid, directorder_refno, date_format(directorder_date, '%d-%b-%Y') as directorder_date,n.user_firstname,a.created_by, directorder_status,a.delivery_status, " +
                       " concat(CAST(date_format(delivered_date, '%d-%m-%Y') as CHAR), '/', delivered_to) as delivery_details,a.branch_gid,a.branch_name "+
                       " from ims_trn_tdeliveryorder a " +
                       " left join hrm_mst_temployee m on m.employee_gid = a.created_name " +
                       " left join adm_mst_tuser n on n.user_gid = m.user_gid " +
                       " where dc_type = 'Direct DC' order by date(a.directorder_date) desc,a.directorder_date asc, a.directorder_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<opndcsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new opndcsummary_list
                        {
                            directorder_gid = dt["directorder_gid"].ToString(),
                            directorder_refno = dt["directorder_refno"].ToString(),
                            directorder_date = dt["directorder_date"].ToString(),
                            directorder_status = dt["directorder_status"].ToString(),
                            delivery_status = dt["delivery_status"].ToString(),
                            delivery_details = dt["delivery_details"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString()


                        });
                        values.opndcsummary_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Open Delivery Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetImsTrnDCBranch(MdlImsTrnOpenDCSummary values)
        {
            try
            {

                msSQL = "select branch_gid,branch_name from hrm_mst_tbranch ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<dcbranch_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new dcbranch_list
                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString()
                        });
                        values.dcbranch_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading branch drop down !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Inventory/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostDcproduct(string user_gid, imsproductissue_list values)
        {
            try
            {

                msSQL = "select product_name from pmr_mst_tproduct where product_gid='" + values.product_name + "'";
                lsproduct_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productgroup_name from pmr_mst_tproductgroup where productgroup_gid='" + values.productgroup_name + "'";
                lsproductgroup_name = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " select qty_requested,display_field from ims_tmp_topendc where " +
                        " product_gid = '" + values.product_name + "' and " +
                        " productuom_gid = '" + values.productuom_gid + "' and " +
                        " display_field = '" + values.display_field + "' and " +
                        " user_gid = '" + user_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        lsrequested = (double)dt["qty_requested"];
                        lsreq1 = values.qty_requested;
                        lstolrequest = lsreq1 + lsrequested;
                        lsreq = values.stock_quantity;
                        if (lsreq < lstolrequest)
                        {
                            values.status = false;
                            values.message = "Requested Quantity should not be higher than available stock!";
                        }
                        else
                        {
                            msSQL = " update ims_tmp_topendc " +
                                    " set qty_requested ='" + lstolrequest + "', " +
                                    " display_field ='" + values.display_field + "' " +
                                    " where  " +
                                    " product_gid = '" + values.product_name + "' and " +
                                    " user_gid = '" + user_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                values.status = true;
                                values.message = "Product Qty Updated Successfully!!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Product Qty!!";
                            }
                        }
                    }
                }
                else
                {
                    msSQL = " insert into ims_tmp_topendc( " +
                            " product_gid, " +
                            " productuom_gid,  " +
                            " productgroup_gid,  " +
                            " product_code,  " +
                            " product_name,  " +
                            " productuom_name,  " +
                            " productgroup_name,  " +
                            " qty_requested, " +
                            " user_gid, " +
                            " display_field ) " +
                            " values( " +
                            " '" + values.product_name + "'," +
                            " '" + values.productuom_name + "'," +
                            " '" + values.productgroup_name + "'," +
                            " '" + values.product_code + "'," +
                            " '" + lsproduct_name + "'," +
                            " '" + values.productuom_name + "'," +
                            " '" + lsproductgroup_name + "'," +
                            " '" + values.qty_requested + "'," +
                            " '" + user_gid + "'," +
                            " '" + values.display_field + "'" + ")";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product Added Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Product!!";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGettmpdcProduct(string user_gid, MdlImsTrnOpenDCSummary values)
        {
            try
            {
                msSQL = "select a.branch_gid  from hrm_mst_temployee a left join adm_mst_tuser  b on b.user_gid=a.user_gid where a.user_gid='" + user_gid + "'";
                string lsbranch = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " select a.tmpdc_gid,ROUND(SUM(g.stock_qty + g.amend_qty - g.issued_qty - g.damaged_qty - g.transfer_qty), 2) AS stock_quantity, a.product_remarks, Format(a.qty_requested,2) as qty_requested, " +
                        " Format(a.qty_issued,2) as qty_issued, a.product_gid," +
                        " b.product_name, b.product_code, c.productgroup_name, " +
                        " f.productuom_name,  a.display_field  from ims_tmp_topendc a " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid  " +
                        " left join pmr_mst_tproductgroup c on b.productgroup_gid = c.productgroup_gid " +
                        " left join pmr_mst_tproductuomclass d on d.productuomclass_gid = b.productuomclass_gid " +
                        " left join pmr_mst_tproductuom f on f.productuom_gid = a.productuom_gid" +
                        " LEFT JOIN ims_trn_tstock g ON b.product_gid = g.product_gid where a.user_gid = '" + user_gid + "' " +
                        " AND g.branch_gid = '" + lsbranch + "' AND(g.stock_qty - g.issued_qty) >= 0 AND g.stock_flag = 'Y' group by g.product_gid";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<tmpdcproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new tmpdcproduct_list
                        {
                            tmpdc_gid = dt["tmpdc_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product = dt["product_name"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            display_field = dt["display_field"].ToString(),
                            stock_quantity = Convert.ToDecimal(dt["stock_quantity"]).ToString("F2"),
                        });
                        values.tmpdcproduct_list = getModuleList;
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
        public void DaopenDCSubmit(string user_gid, opendcnew_list values)
        {
            try
            {
                msSQL = "select count(*) as mnCtr from ims_tmp_topendc where user_gid ='" + user_gid + "'";
                mnCtr = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select branch_name from hrm_mst_tbranch where branch_gid='" + values.branch_name + "'";
                lsbranch = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select branch_gid from hrm_mst_tbranch where mainbranch_flag='Y'";
                lsmainbranch=objdbconn.GetExecuteScalar(msSQL);
                DateTime uiDate = DateTime.ParseExact(values.directorder_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                lsdirectorder_date = uiDate.ToString("yyyy-MM-dd HH:mm:ss");
                mssalesorderGID = objcmnfunctions.GetMasterGID("VDOP");
                msSQL = " insert into ims_trn_tdeliveryorder (" +
                        " directorder_gid," +
                        " directorder_date," +
                        " directorder_refno," +
                        " delivery_status," +
                        " delivered_to," +
                        " delivered_date," +
                        " dc_type," +
                        " dc_no," +
                        " mode_of_despatch," +
                        " tracker_id," +
                        " dc_note," +
                        " no_of_boxs," +
                        " shipping_to," +
                        " Branch_gid," +
                        " branch_name," +
                        " created_by," +
                        " created_date"+
                        " ) values (" +
                        "'" + mssalesorderGID + "'," +
                        "'" + lsdirectorder_date + "'," +
                        "'" + mssalesorderGID + "'," +
                        "'Dc Generated'," +
                        "'" + values.branch_name + "'," +
                        "'" + lsdirectorder_date + "'," +
                        "'Direct DC'," +
                        "'" + values.tracker_id + "'," +
                        "'" + values.mode_of_despatch + "'," +
                        "'" + values.tracker_id + "'," +
                        "'" + values.dc_note + "'," +
                        "'" + values.no_of_boxs + "'," +
                        "'" + values.shipping_to + "'," +
                        "'" + values.branch_name + "'," +
                        "'" + lsbranch + "'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msIssueGID = objcmnfunctions.GetMasterGID("IMIT");
                    msSQL = " Insert into ims_trn_tmaterialissued (" +
                              " materialissued_gid, " +
                              " materialissued_date, " + 
                              " branch_gid, " +
                              " materialrequisition_gid, " +
                              " materialrequisition_type, " +
                              " materialissued_status, " +
                              " materialissued_reference, " +
                              " user_gid, " +
                              " priority, " +
                              " materialissued_remarks, " +
                              " location_gid, " +
                              " created_date) " +
                              " values ( " +
                              "'" + msIssueGID + "'," +
                              "'" + lsdirectorder_date + "', " +
                              "'" + values.branch_name + "'," +
                              "'" + msGetGid + "'," +
                              "'PT00010001204'," +
                              "'Issued Accept Pending'," +
                              "'" + values.dc_note + "', " +
                              "'" + user_gid + "'," +
                              "'N', " +
                              "'" + values.dc_note + "', " +
                              "'" + lslocation + "', " +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " select a.product_gid, productuom_gid, a.product_remarks,a.product_code,a.product_name," +
                                " a.productuom_name,a.productgroup_gid,a.productgroup_name," +
                                " a.qty_requested,a.qty_issued,a.display_field from ims_tmp_topendc a" +
                                " where a.user_gid ='" + user_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                lsproduct_gid = dt["product_gid"].ToString();
                                lsproductuom_gid = dt["productuom_gid"].ToString();
                                lsproduct_remarks = dt["product_remarks"].ToString();
                                lsproduct_code = dt["product_code"].ToString();
                                lsproduct_name = dt["product_name"].ToString();
                                lsproductuom_name = dt["productuom_name"].ToString();
                                lsproductgroup_gid = dt["productgroup_gid"].ToString();
                                lsproductgroup_name = dt["productgroup_name"].ToString();
                                lsqty_requested = dt["qty_requested"].ToString();
                                lsquantity_delivered = dt["qty_issued"].ToString();
                                lsdisplay_field = dt["display_field"].ToString();
                                msGetImRC = objcmnfunctions.GetMasterGID("VDDC");
                                msSQL = " insert into ims_trn_tdeliveryorderdtl (" +
                                        " directorderdtl_gid, " +
                                        " directorder_gid, " +
                                        " productgroup_gid, " +
                                        " productgroup_name, " +
                                        " product_gid," +
                                        " product_code," +
                                        " product_name, " +
                                        " product_uom_gid, " +
                                        " productuom_name, " +
                                        " product_qty, " +
                                        " product_desc, " +
                                        " product_price, " +
                                        " discount_percentage, " +
                                        " discount_amount, " +
                                        " tax_name, " +
                                        " tax_name2, " +
                                        " product_total, " +
                                        " tax_name3, " +
                                        " dc_no, " +
                                        " mode_of_despatch, " +
                                        " tracker_id, " +
                                        " tax_amount, " +
                                        " tax_amount2, " +
                                        " tax_amount3, " +
                                        " tax1_gid, " +
                                        " tax2_gid, " +
                                        " tax3_gid, " +
                                        " product_qtydelivered " +
                                        "  ) " +
                                        " values ( " +
                                        "'" + msGetImRC + "', " +
                                        "'" + mssalesorderGID + "'," +
                                        "'" + lsproductgroup_gid + "', " +
                                        "'" + lsproductgroup_name + "', " +
                                        "'" + lsproduct_gid + "', " + 
                                        "'" + lsproduct_code + "', " +
                                        "'" + lsproduct_name + "', " +
                                        "'" + lsproductuom_name + "', " +
                                        "'" + lsproductuom_gid + "', " +
                                        "'" + lsproduct_desc + "'," +
                                        "'" + lsqty_requested + "', " +
                                        "'" + values.display_field + "',";
                                        if (values.price == null || values.price == "")
                                        {
                                            msSQL += "'0.00',";
                                        }
                                        else
                                        {
                                            msSQL += "'" + values.price + "',";
                                        }
                                        if (values.discount_percentage == null || values.discount_percentage == "")
                                        {
                                            msSQL += "'0.00',";
                                        }
                                        else
                                        {
                                            msSQL += "'" + values.discount_percentage + "',";
                                        }
                                        if (values.discount_amount == null || values.discount_amount == "")
                                        {
                                            msSQL += "'0.00',";
                                        }
                                        else
                                        {
                                            msSQL += "'" + values.discount_amount + "',";
                                        }
                                        msSQL += "'" + values.tax_name + "'," +
                                                 "'" + values.tax_name2 + "',";
                                        if (values.total_amount == null || values.total_amount == "")
                                        {
                                            msSQL += "'0.00',";
                                        }
                                        else
                                        {
                                            msSQL += "'" + values.total_amount + "',";
                                        }
                                        msSQL += "'" + values.tax_name3 + "'," +
                                                 "'" + values.dc_no + "'," +
                                                 "'" + values.despatch_mode + "'," +
                                                 "'" + values.tracker_id + "',";
                                        if (values.tax_amount == null || values.tax_amount == "")
                                        {
                                            msSQL += "'0.00',";
                                        }
                                        else
                                        {
                                            msSQL += "'" + values.tax_amount + "',";
                                        }

                                        if (values.tax_amount2 == null || values.tax_amount2 == "")
                                        {
                                            msSQL += "'0.00',";
                                        }
                                        else
                                        {
                                            msSQL += "'" + values.tax_amount2 + "',";
                                        }

                                        if (values.tax_amount3 == null || values.tax_amount3 == "")
                                        {
                                            msSQL += "'0.00',";
                                        }
                                        else
                                        {
                                            msSQL += "'" + values.tax_amount3 + "',";
                                        }
                                        msSQL += "'" + values.tax1_gid + "'," +
                                                 "'" + values.tax2_gid + "'," +
                                                 "'" + values.tax3_gid + "'," +
                                                 "'" + lsquantity_delivered + "')";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 0)
                                {
                                    values.status = false;
                                    values.message = "Error While Inserting material details!";
                                }
                                    msSQL = " SELECT (stock_qty+amend_qty-issued_qty-damaged_qty-transfer_qty) as stock_qty, " +
                                            " issued_qty, stock_gid FROM ims_trn_tstock WHERE (stock_qty > issued_qty)" +
                                            " AND product_gid = '" + lsproduct_gid + "' and stock_flag='Y'" +
                                            " AND branch_gid = '" + lsmainbranch + "' ";
                                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                                if (objODBCDataReader.HasRows)
                                {
                                    int stockQty = Convert.ToInt32(objODBCDataReader["stock_qty"]);
                                    int issueQty = Convert.ToInt32(objODBCDataReader["issued_qty"]);
                                    string stockgid = objODBCDataReader["stock_gid"].ToString();
                                    int qtyrest = Convert.ToInt32(lsqty_requested);
                                    int quantityForThisRow = Math.Min(qtyrest, stockQty);

                                    quantities.Add(quantityForThisRow);
                                    while (objODBCDataReader.Read() && quantities.Count > 0)
                                    {
                                        
                                            msSQL = " UPDATE ims_trn_tstock " +
                                                    " SET issued_qty = " + (issueQty + quantityForThisRow) + " " +
                                                    " WHERE product_gid = '" + lsproduct_gid + "' " +
                                                    " AND branch_gid = '" + lsmainbranch + "' " +
                                                    " AND stock_gid = '" + stockgid + "'";
                                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }
                                    objODBCDataReader.Close();
                                }
                                if (mnResult1 != 0)
                                {
                                    mcGetGID = objcmnfunctions.GetMasterGID("IMNL");
                                    msSQL = "  Select  distinct  sum(f.stock_qty+f.amend_qty-f.issued_qty-f.damaged_qty-f.transfer_qty) As stock_quantity," +
                                            "  stock_gid from ims_trn_tstock f " +
                                            "  where f.product_gid ='" + lsproduct_gid + "'  and f.branch_gid='" + values.branch_name + "' group by f.product_gid ";
                                    objODBCDataReader = objdbconn.GetDataReader(msSQL);
                                    if (objODBCDataReader.HasRows)
                                    {
                                        if (objODBCDataReader.Read())
                                        {
                                            lsStockQty = objODBCDataReader["stock_quantity"].ToString();
                                            lsStockGid = objODBCDataReader["stock_gid"].ToString();
                                            objODBCDataReader.Close();
                                        }
                                    }
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
                                              "'" + lsStockGid + "'," +
                                              "'" + values.branch_name + "'," +
                                              "'" + lsproduct_gid + "'," +
                                              "'" + lsproductuom_gid + "'," +
                                              "'" + lsqty_requested + "'," +
                                              "'0.00'," +
                                              "'0.00'," +
                                              "'0.00'," +
                                              "'0.00'," +
                                              "'0.00'," +
                                              "'" + msIssueGID + "'," +
                                              "'Issued'," +
                                              "''," +
                                              "'" + user_gid + "'," +
                                              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                              "'')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 0)
                                    {
                                        values.status = false;
                                        values.message = "Error Occured While Inserting Stockdtl!";

                                    }
                                    msGetStockTrackerGID = objcmnfunctions.GetMasterGID("ISTK");

                                    msSQL = " Insert into ims_trn_tstocktracker (" +
                                           " stocktracker_gid," +
                                           " stock_gid, " +
                                           " branch_gid," +
                                           " product_gid," +
                                           " productuom_gid," +
                                           " display_field," +
                                           " qty_issued," +
                                           " stocktype_gid," +
                                           " created_by," +
                                           " created_date," +
                                           " remarks, " +
                                           " mrdtl_gid," +
                                           " mr_gid," +
                                           " reference_gid) " +
                                           " values ( " +
                                           " '" + msGetStockTrackerGID + "', " +
                                           " '" + lsStockGid + "'," +
                                           " '" + values.branch_name + "'," +
                                           " '" + lsproduct_gid + "', " +
                                           " '" + lsproductuom_gid + "', " +
                                           " '" + values.display_field + "'," +
                                           "'" + lsqty_requested + "'," +
                                           " 'SY0905270006'," +
                                           " '" + user_gid + "'," +
                                           " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                           " 'Material Issued', " +
                                           " '" + msGetImRC + "', " +
                                           " '" + msGetGid + "'," +
                                           " '" + msIssueGID + "')";
                                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);

                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Inserting Stock!";
                                }

                            }
                        }
                        if (mnResult2 != 0)
                        {
                            msSQL = " select b.employee_gid from adm_mst_tuser a " +
                                    " left join hrm_mst_temployee b on a.user_gid=b.user_gid " +
                                    " where a.user_gid ='" + user_gid + "'";
                            lsemployeegid = objdbconn.GetExecuteScalar(msSQL);

                            msGetPodc = objcmnfunctions.GetMasterGID("PODC");
                            msSQL = " insert into ims_trn_tmrapproval ( " +
                                    " approval_gid, " +
                                    " approved_by, " +
                                    " approved_date, " +
                                    " submodule_gid, " +
                                    " approval_flag, " +
                                    " mrapproval_gid " +
                                    " ) values ( " +
                                    "'" + msGetPodc + "'," +
                                    "'" + lsemployeegid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                    "'IMSTRNMRA'," +
                                    "'Y'," +
                                    "'" + msGetGid + "') ";
                            mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult3 != 0)
                            {
                               msSQL = " update ims_trn_tmrapproval set " +
                                       " approval_remarks = 'Self Approved', " +
                                       " approval_flag = 'Y', " +
                                       " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                       " where approved_by = '" + lsemployeegid + "'" +
                                       " and mrapproval_gid = '" + msGetGid + "' and submodule_gid='IMSTRNMRA'";
                                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                 
                                    if (mnResult2 != 0)
                                    {
                                        msSQL = " delete from ims_tmp_topendc " +
                                                " where user_gid = '" + user_gid + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult != 0)
                                        {
                                            values.status = true;
                                            values.message = "Direct DC Generated successfully";
                                        }
                                    }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error Occured While Inserting DC Status!";
                            }

                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occured While Inserting Stock Tracker!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Inserting Open DC !";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Inserting Material Requisition !";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting data!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetViewDCSummary(string directorder_gid, MdlImsTrnOpenDCSummary values)
        {
            try
            {

                msSQL = "SELECT  directorder_gid, date_format(directorder_date , '%Y-%m-%d') as directorder_date, directorder_refno," +
                    " delivery_status, delivered_to, delivered_to, date_format(delivered_date, '%Y-%m-%d') as delivered_date, dc_type," +
                    " dc_no, mode_of_despatch,tracker_id,dc_note, no_of_boxs, " +
                    "shipping_to, Branch_gid, branch_name, created_by," +
                    " created_date FROM ims_trn_tdeliveryorder WHERE directorder_gid = '"+directorder_gid+"';";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<viewdc_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new viewdc_list
                        {
                            directorder_gid = dt["directorder_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            directorder_date = dt["directorder_date"].ToString(),
                            directorder_refno = dt["directorder_refno"].ToString(),
                            delivery_status = dt["delivery_status"].ToString(),
                            delivered_to = dt["delivered_to"].ToString(),
                            delivered_date = dt["delivered_date"].ToString(),
                            dc_type = dt["dc_type"].ToString(),
                            dc_no = dt["dc_no"].ToString(),
                            mode_of_despatch = dt["mode_of_despatch"].ToString(),
                            tracker_id = dt["tracker_id"].ToString(),
                            dc_note = dt["dc_note"].ToString(),
                            no_of_boxs = dt["no_of_boxs"].ToString(),
                            shipping_to = dt["shipping_to"].ToString(),
                            Branch_gid = dt["Branch_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });

                        values.viewdc_list = getModuleList;

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

        public void DaGetdcProduct(string user_gid, string directorder_gid, MdlImsTrnOpenDCSummary values)
        {
            try
            {
              
                msSQL = "select  sum(f.stock_qty+f.amend_qty-f.issued_qty-f.damaged_qty-f.transfer_qty) as stock_quantity, a.directorderdtl_gid, " +
                    " a.directorder_gid,  a. productgroup_gid,   " +
                    " a.productgroup_name,   a.product_gid, b.product_code," +
                    " a.product_name,   a.product_uom_gid, a.  productuom_name," +
                    " a.  product_qty,  a. product_description, a.  product_price," +
                    " a. discount_percentage, a.discount_amount, a.   tax_name,  a. tax_name2," +
                    " a.  product_total,  a.tax_name3,   a.dc_no,   a.mode_of_despatch, " +
                    " a.tracker_id, a.tax_amount,  a.tax_amount2, a.tax_amount3,  a.tax1_gid," +
                    " a.tax2_gid,  a.tax3_gid,  a.product_qtydelivered" +
                    " from ims_trn_tdeliveryorderdtl a" +
                    " left join pmr_mst_tproduct b on b.product_gid = a.product_gid" +
                    "  left join ims_trn_tstock f on a.product_gid=f.product_gid " +
                    " where directorder_gid = '" + directorder_gid + "' group by a.product_gid";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<tmpdcproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new tmpdcproduct_list
                        {
                           
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product = dt["product_name"].ToString(),
                            qty_requested = dt["product_qty"].ToString(),
                            product_remarks = dt["product_description"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            stock_quantity = Convert.ToDecimal(dt["product_qtydelivered"]).ToString("F2"),
                            available_quantity = dt["stock_quantity"].ToString()
                        });
                        values.tmpdcproduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetOpendcViewProduct(string salesorder_gid, MdlImsTrnOpenDCSummary values)
        {
            try
            {

                msSQL = " select a.qty_quoted,b.product_name,a.product_delivered,e.productuom_name,b.product_code,   " +
                        " (select ifnull(sum(m.stock_qty) + sum(m.amend_qty) - sum(m.damaged_qty) - sum(m.issued_qty) - sum(m.transfer_qty), 0) as available_quantity "+
                        " from ims_trn_tstock m where m.stock_flag = 'Y' and m.product_gid = a.product_gid and m.branch_gid = c.branch_gid and a.uom_gid = m.uom_gid) as available_quantity"+
                        " from smr_trn_tsalesorderdtl a left join pmr_mst_tproduct b on a.product_gid = b.product_gid "+
                        " left join smr_trn_tsalesorder c on c.salesorder_gid = a.salesorder_gid " +
                        " left join pmr_mst_tproductuom e on e.productuom_gid = a.uom_gid " +
                        " where a.salesorder_gid = '"+ salesorder_gid + "'" +
                        " group by a.product_gid,a.uom_gid order by a.salesorderdtl_gid asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOpendcView_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOpendcView_list
                        {
                            qty_quoted = dt["qty_quoted"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_delivered = dt["product_delivered"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            available_quantity = dt["available_quantity"].ToString()
                        });
                        values.GetOpendcView_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
    }
}