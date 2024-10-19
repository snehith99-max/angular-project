using ems.outlet.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Globalization;

namespace ems.outlet.Dataaccess
{
    public class DaOtlWhatsAppOrder
    {
        string msSQL = string.Empty;
        DataTable dt_datatable;
        int mnResult,mnResult1;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        OdbcDataReader objOdbcDataReader;
        public void DaGetwhatsappordersummary(MdlOtlWhatsAppOrder values)
        {
            try
            {
                msSQL = "select a.kot_gid,a.order_id,format(a.kot_tot_price,2) as kot_tot_price ,a.customer_phone,format(a.kot_delivery_charges,2)as kot_delivery_charges,a.order_type,c.branch_name,a.created_date,a.address,a.payment_method,a.branch_gid,a.order_status" +
                        " ,a.contact_id,a.payment_status,a.message_id,sum(b.product_quantity) as total_quantity ,a.reject_reason," +
                        "count(b.kot_product_gid) as total_product,kitchen_status   from otl_trn_tkot a " +
                        "left join otl_trn_tkotdtl b on b.kot_gid=a.kot_gid " +
                        "left join hrm_mst_tbranch c on c.branch_gid=a.branch_gid " +
                        "WHERE a.order_status IN ('CONFIRMED', 'REJECTED') group by kot_gid order by a.created_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<whatsappordersummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new whatsappordersummary_list
                        {
                            kot_gid = dt["kot_gid"].ToString(),
                            order_id = dt["order_id"].ToString(),
                            kot_tot_price = dt["kot_tot_price"].ToString(),
                            customer_phone = dt["customer_phone"].ToString(),
                            kot_delivery_charges = dt["kot_delivery_charges"].ToString(),
                            order_type = dt["order_type"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            payment_status = dt["payment_status"].ToString(),
                            total_quantity = dt["total_quantity"].ToString(),
                            total_product = dt["total_product"].ToString(),
                            message_id = dt["message_id"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            address = dt["address"].ToString(),
                            payment_method = dt["payment_method"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            contact_id = dt["contact_id"].ToString(),
                            reject_reason = dt["reject_reason"].ToString(),
                            order_status = dt["order_status"].ToString(),
                            kitchen_status = dt["kitchen_status"].ToString()

                        });
                        values.whatsappordersummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Whatsapp summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Getting summary details" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********","/WhatsAppOrders/ErrorLog/Summary/" + "DaGetwhatsappordersummary " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetViewwhatsapporderSummary(string kot_gid, whatsappordersummary_list values)
        {
            try
            {
                msSQL = "select a.order_id,FORMAT(a.kot_tot_price,2) as kot_tot_price ,FORMAT(a.line_items_total,2) as line_items_total ,a.customer_phone,a.address,FORMAT(a.kot_delivery_charges,2) as kot_delivery_charges ,a.source,a.message_id,a.order_type, date_format(a.created_date,'%d-%m-%Y')  as created_date ,a.payment_status,b.branch_name,a.order_instructions," +
                    " a.reject_reason from otl_trn_tkot a left join hrm_mst_tbranch b on b.branch_gid=a.branch_gid  where a.kot_gid='" + kot_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.order_id = objOdbcDataReader["order_id"].ToString();
                    values.kot_tot_price = objOdbcDataReader["kot_tot_price"].ToString();
                    values.customer_phone = objOdbcDataReader["customer_phone"].ToString();
                    values.kot_delivery_charges = objOdbcDataReader["kot_delivery_charges"].ToString();
                    values.source = objOdbcDataReader["source"].ToString();
                    values.message_id = objOdbcDataReader["message_id"].ToString();
                    values.order_type = objOdbcDataReader["order_type"].ToString();
                    values.created_date = objOdbcDataReader["created_date"].ToString();
                    values.payment_status = objOdbcDataReader["payment_status"].ToString();
                    values.branch_name = objOdbcDataReader["branch_name"].ToString();
                    values.address = objOdbcDataReader["address"].ToString();
                    values.reject_reason = objOdbcDataReader["reject_reason"].ToString();
                    values.order_instructions = objOdbcDataReader["order_instructions"].ToString();
                    values.line_items_total = objOdbcDataReader["line_items_total"].ToString();

                }

                msSQL = "select a.kotdtl_gid,FORMAT(a.kot_product_price, 2) as kot_product_price,a.product_quantity,b.product_gid,a.kot_product_gid, " +
                        "FORMAT(a.product_amount, 2) as product_amount,a.currency,case when b.product_gid is not null then b.product_name else (select product_name from pmr_mst_tproduct a " +
                        "left join otl_trn_branch2product b on a.product_gid = b.product_gid " +
                        "where b.branch2product_gid = a.kot_product_gid) end as product_name, " + 
                        "case when b.product_gid is not null then b.product_desc else (select product_desc from pmr_mst_tproduct a " +
                        "left join otl_trn_branch2product b on a.product_gid = b.product_gid " +
                        "where b.branch2product_gid = a.kot_product_gid) end as product_desc,case when b.product_gid is not null then c.productgroup_name else (select c.productgroup_name from pmr_mst_tproduct a " +
                        "left join otl_trn_branch2product b on a.product_gid = b.product_gid " +
                        "left join pmr_mst_tproductgroup c on c.productgroup_gid = a.productgroup_gid " +
                        "where b.branch2product_gid = a.kot_product_gid) end as productgroup_name, " +
                        "case when b.product_gid is not null then b.product_code else (select product_code from pmr_mst_tproduct a " +
                        "left join otl_trn_branch2product b on a.product_gid = b.product_gid " +
                        "where b.branch2product_gid = a.kot_product_gid) end as product_code " +
                        "from otl_trn_tkotdtl a " +
                        "left join pmr_mst_tproduct b on b.product_gid = a.kot_product_gid " +
                        "left join pmr_mst_tproductgroup c on c.productgroup_gid = b.productgroup_gid " +
                        "left join otl_trn_branch2product d on d.product_gid = b.product_gid " +
                        "left join otl_trn_tkotdtl e on e.kot_product_gid = d.branch2product_gid " +
                        "left join otl_trn_branch2product f on f.product_gid = b.product_gid " +
                        "where a.kot_gid = '" + kot_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Viewwhatsappsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Viewwhatsappsummary_list
                        {
                            kotdtl_gid = dt["kotdtl_gid"].ToString(),
                            kot_product_price = dt["kot_product_price"].ToString(),
                            product_quantity = dt["product_quantity"].ToString(),
                            product_amount = dt["product_amount"].ToString(),
                            currency = dt["currency"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                        });
                    }
                    values.Viewwhatsappsummary_list = getModuleList;

                }
                dt_datatable.Dispose();

                objOdbcDataReader.Close();

            }
            catch(Exception ex)
            {
                values.message = "Exception occured while getting Whatsapp view summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "*************Query****" + "Exception Occurred While Getting summary details" + ex.Message.ToString() + " ******* " + " *******" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/View/" + "DaGetViewwhatsapporderSummary " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void Daonapprovewhatapporder(string kot_gid , MdlOtlWhatsAppOrder values)
        {
            try
            {
                string customer_name = "", customer_address = "", customer_gid = "" ,customer_phone = "", customer_country="", currencyexchange_gid="", exchange_rate="", currency_code="";
                msSQL = "select customer_phone from otl_trn_tkot where kot_gid = '" + kot_gid + "' ";
               customer_phone = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select customer_gid from crm_mst_tcustomercontact where mobile like '%" + Regex.Replace(customer_phone, @"^\+?(44|0|91|1)", "") + "%'";
                customer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select customer_name,customer_address,customer_country from crm_mst_tcustomer where customer_gid = '" + customer_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                foreach (DataRow dt in dt_datatable.Rows)
                {
                     customer_name = dt["customer_name"].ToString();
                    customer_address = dt["customer_address"].ToString();
                    customer_country = dt["customer_country"].ToString();
                }
                msSQL = "select currency_code, exchange_rate,currencyexchange_gid from crm_trn_tcurrencyexchange where country_gid = '" + customer_country + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    currency_code = dt["currency_code"].ToString();
                    exchange_rate = dt["exchange_rate"].ToString();
                    currencyexchange_gid = dt["currencyexchange_gid"].ToString();
                }
                string mssalesorderGID = objcmnfunctions.GetMasterGID("VSOP");

                msSQL = "select order_id,kot_tot_price,address,created_date,created_by,kot_delivery_charges,branch_gid,payment_status from otl_trn_tkot where kot_gid = '" + kot_gid + "'";
                objOdbcDataReader=objdbconn.GetDataReader(msSQL);
                if(objOdbcDataReader.HasRows==true)
                {
                    //string inputDate = objOdbcDataReader["created_date"].ToString();
                    //DateTime uiDate = DateTime.ParseExact(objOdbcDataReader["created_date"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    string salesorder_date = DateTime.Parse(objOdbcDataReader["created_date"].ToString()).ToString("yyyy-MM-dd HH:mm:");
                    msSQL = " insert  into smr_trn_tsalesorder (" +
                            " salesorder_gid ," +
                            " branch_gid ," +
                            " salesorder_date," +
                            " customer_gid," +
                            " customer_name," +
                            " customer_address ," +
                            " bill_to ," +
                            " shipping_to ," +
                            " created_by," +
                            " so_referenceno1 ," +
                            " Grandtotal, " +
                            " salesorder_status, " +
                            " grandtotal_l, " +
                            " currency_code, " +
                            " currency_gid, " +
                            //" exchange_rate, " +
                            " total_price," +
                            " total_amount," +
                            " tax_amount," +
                            " source_flag, " +
                            "created_date" +
                            " )values(" +
                            "'" + mssalesorderGID + "'," +
                            " '" + objOdbcDataReader["branch_gid"].ToString() + "'," +
                            " '" + salesorder_date + "'," +
                            " '" + customer_gid + "'," +
                            " '" + customer_name + "'," +
                            " '" + customer_address + "'," +
                            " '" + objOdbcDataReader["address"].ToString() + "'," +
                            " '" + objOdbcDataReader["address"].ToString() + "'," +
                            " '" + objOdbcDataReader["created_by"].ToString() + "'," +
                            " '" + objOdbcDataReader["order_id"].ToString() + "'," +
                            " '" + objOdbcDataReader["kot_tot_price"].ToString() + "'," +
                            " 'Approved'," +
                            "'" + objOdbcDataReader["kot_tot_price"].ToString() + "'," +
                            " '" + currency_code + "'," +
                            " '" + currencyexchange_gid + "'," +
                           // " '" + exchange_rate + "'," +
                            "'" + objOdbcDataReader["kot_tot_price"].ToString() + "'," +
                            " '" + objOdbcDataReader["kot_tot_price"].ToString() + "'," +
                            "'0'," +
                            "'W'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }


                //main table insert
               
                //product table insert 

                msSQL = "select  a.kot_product_gid,a.kot_product_price,a.product_quantity,a.product_amount, b.product_name, b.productgroup_gid,b.product_desc,b.productuom_gid, " +
                        " b.customerproduct_code from otl_trn_tkotdtl a " +
                        "left join pmr_mst_tproduct b on b.product_gid = a.kot_product_gid where a.kot_gid = '" + kot_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                foreach (DataRow dt in dt_datatable.Rows)
                {

                   string msGetGid = objcmnfunctions.GetMasterGID("VSDT");

                    msSQL = "insert into smr_trn_tsalesorderdtl (" +
                            " salesorderdtl_gid ," +
                            " salesorder_gid," +
                            " product_gid ," +
                            " product_name," +
                            " product_code," +
                            " product_price," +
                            " productgroup_gid," +
                            " product_remarks," +
                            " qty_quoted," +
                            " uom_gid," +
                            " price " +
                            ")values(" +
                            "'" + msGetGid + "'," +
                            "'" + mssalesorderGID + "'," +
                            "'" + dt["kot_product_gid"].ToString() + "'," +
                            "'" + dt["product_name"].ToString() + "'," +
                            "'" + dt["customerproduct_code"].ToString() + "'," +
                            "'" + dt["kot_product_price"].ToString() + "'," +
                            "'" + dt["productgroup_gid"].ToString() + "'," +
                            "'" + dt["product_desc"].ToString() + "'," +
                            "'" + dt["product_quantity"].ToString() + "'," +
                            "'" + dt["productuom_gid"].ToString() + "'," +
                            " '" + dt["kot_product_price"].ToString() + "')";

                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if(mnResult == 1 && mnResult1 == 1)
                {
                    msSQL = "update otl_trn_tkot set " +
                            "salesorder_gid = '" + mssalesorderGID + "' " +
                            "where kot_gid='" + kot_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Order approved successfully";
                   
                }
                else
                {
                    values.status = false;
                    values.message = "Error While approving order";
                }


                }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Whatsapp view summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss")
                + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }
        }
        public void DaUpdatewtsorderpayment(string kot_gid, result values)
        {
            msSQL = "update otl_trn_tkot set payment_status = 'PAID' where kot_gid = '" + kot_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                values.message = "Payment Status Updated Sucessfully";
            }
            else
            {
                values.status = false;
                values.message = "Error occured while updating Payment Status.";
            }
        }

        public void Dacompleteorder(string kot_gid, result values)
        {
            try
            {

                msSQL = "update otl_trn_tkot set kitchen_status = 'D' where kot_gid = '" + kot_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "The order is completed!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While Updating!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating!";

            }
        }


    }
}