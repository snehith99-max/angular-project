using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Net.Mail;
using ems.sales.Models;
using System.Data;
using System.Globalization;


namespace ems.sales.DataAccess
{
    public class DaSmrQuotationAmend
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;  
        DataTable dt_datatable;
        decimal lsmrp_price;
        double taxAmount, costPrice, quantity, discountPercentage, subtotal, discountAmount, reCalTotalAmount, reCalTaxAmount, reCaldiscountAmount;
        string  lstaxname, lstaxamount, lspercentage1,  lsproduct_price, lscustomerbranch_name,  lsquotation_type,msGetGid1, msgetGid2, msgetGid4;
        int mnResult;
        string lsdiscountpercentage;
        string lsdiscountamount, lsamendcount;

        MailMessage message = new MailMessage();

        // AMEND DATA FETCHING FROM QUOTATION SUMARY 
        public void DaGetQuotationamend(string quotation_gid, MdlSmrQuotationAmend values)

        {
            try
            {
                msSQL = " delete from smr_tmp_treceivequotationdtl ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = "select a.quotation_gid, DATE_FORMAT(a.quotation_date, '%d-%m-%Y') as quotation_date, " +
                            "a.quotation_referencenumber," + "a.customer_gid," + "a.customer_address," +
                            "concat(customer_contact_person)as contact_person," +
                            "a.quotation_remarks," + "a.created_by," + "format(a.total_amount,2) as total_amount," +
                            "a.payment_days," + "a.delivery_days ," + "a.quotation_status," + "a.contact_no," + "a.contact_mail," + "a.customer_name," +
                            " Grandtotal, " +
                            "format(a.addon_charge,2) as addon_charge, " + "a.quotation_referenceno1," +
                            "format(a.additional_discount,2) as additional_discount," + "a.currency_code, " +
                            "a.exchange_rate, " + "a.currency_gid , " + "a.termsandconditions, " + "a.created_date,"  + 
                            "a.branch_gid, b.branch_name,a.tax_gid," + " a.tax_name," + " e.campaign_gid," + "concat(e.campaign_title, ' | ', f.user_code, ' | ', f.user_firstname, ' ', f.user_lastname) as username," + " a.vessel_name," + " a.enquiry_refno," +
                            " format(a.total_price,2)as total_price,a.pricingsheet_gid,a.pricingsheet_refno,a.freight_terms,a.payment_terms,a.customerenquiryref_number,format(a.roundoff,2) as roundoff, " +
                            " format(ifnull(a.freight_charges,0.00),2)as freight_charges," +
                            " format(ifnull(a.buyback_charges,0.00),2)as buyback_charges," + " format(ifnull(a.packing_charges,0.00),2)as packing_charges," +
                            " format(ifnull(a.insurance_charges,0.00),2)as insurance_charges,a.enquiry_gid , a.tax_amount" +
                            " from smr_trn_treceivequotation a " + " left join hrm_mst_tbranch b on b.branch_gid=a.branch_gid " +
                            " left join smr_tmp_treceivequotationdtl l on a.quotation_gid = l.quotation_gid " +
                            " left join crm_trn_tenquiry2campaign d on a.quotation_gid=d.quotation_gid " +
                            " left join smr_trn_tcampaign e on d.campaign_gid=e.campaign_gid " +
                            " left join adm_mst_tuser f on a.salesperson_gid=f.user_gid " +
                            " left join hrm_mst_temployee g on f.user_gid=g.user_gid " +
                            " where a.quotation_gid ='" + quotation_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<quotationamendlist>();
                if (dt_datatable.Rows.Count != 0)

                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " Select customer_gid from crm_mst_tcustomer where customer_name='" + dt["customer_name"] + "'";
                        string customergid = objdbconn.GetExecuteScalar(msSQL);
                        getModuleList.Add(new quotationamendlist
                        {
                            quotation_gid = dt["quotation_gid"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            quotation_referencenumber = dt["quotation_referencenumber"].ToString(),
                            customer_gid = customergid,
                            quotation_remarks = dt["quotation_remarks"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            contact_person = dt["contact_person"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            delivery_days = dt["delivery_days"].ToString(),
                            contact_no = dt["contact_no"].ToString(),
                            contact_mail = dt["contact_mail"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            addon_charge = dt["addon_charge"].ToString(),
                            quotation_referenceno1 = dt["quotation_referenceno1"].ToString(),
                            additional_discount = dt["additional_discount"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_gid = dt["currency_gid"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            salesperson_gid = dt["username"].ToString(),
                            total_price = dt["total_price"].ToString(),                            
                            freight_terms = dt["freight_terms"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            customerenquiryref_number = dt["customerenquiryref_number"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            freight_charges = dt["freight_charges"].ToString(),
                            buyback_charges = dt["buyback_charges"].ToString(),
                            packing_charges = dt["packing_charges"].ToString(),
                            insurance_charges = dt["insurance_charges"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            tax_amount4= dt["tax_amount"].ToString()
                        });
                        values.quotationamend_list = getModuleList;
                    }

                    dt_datatable.Dispose();

                    msSQL = " Select quotation_gid, quotationdtl_gid,quote_type, product_name, product_gid, product_code,productgroup_name, productgroup_gid, " +
                            " uom_name, uom_gid, price, qty_quoted, discount_percentage, discount_amount, product_price,created_by, " +
                            " tax_name, tax_percentage, tax_amount from smr_trn_treceivequotationdtl where quotation_gid = '" + quotation_gid + "' order by quotationdtl_gid";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    string TempQuotationDtl = objcmnfunctions.GetMasterGID("VQDT");
                    if (dt_datatable.Rows.Count != 0)

                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            

                            msSQL = " Insert into smr_tmp_treceivequotationdtl(" +
                                    " tmpquotationdtl_gid, " +
                                    " quotation_gid, " +
                                    " quotationdtl_gid, " +
                                    " quotation_type, " +
                                    " product_gid," +
                                    " product_code," +
                                    " product_name," +
                                    " productgroup_name," +
                                    " productgroup_gid," +
                                    " uom_gid," +
                                    " uom_name," +
                                    " qty_quoted," +
                                    " discount_percentage," +
                                    " discount_amount," +
                                    " tax_name," +
                                    " tax_amount," +
                                    " product_price," +
                                    " created_by," +
                                    " price" +
                                    ") values (" +
                                    "'" + dt["quotationdtl_gid"] + "'," +
                                    "'" + quotation_gid + "'," +
                                     "'" + dt["quotationdtl_gid"] + "', " +
                                     "'" + dt["quote_type"] + "', " +
                                    "'" + dt["product_gid"] + "', " +
                                    "'" + dt["product_code"] + "', " +
                                    "'" + dt["product_name"] + "', " +
                                    "'" + dt["productgroup_name"] + "', " +
                                    "'" + dt["productgroup_gid"] + "', " +
                                    "'" + dt["uom_gid"] + "', " +
                                    "'" + dt["uom_name"] + "', " +
                                    "'" + dt["qty_quoted"] + "', " +
                                    "'" + dt["discount_percentage"] + "', " +
                                    "'" + dt["discount_amount"] + "', " +
                                    "'" + dt["tax_name"] + "', " +
                                    "'" + dt["tax_amount"] + "', " +
                                    "'" + dt["product_price"] + "', " +
                                    "'" + dt["created_by"] + "', " +
                                    "'" + dt["price"] + "') ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                    }
                }



            }
            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // PRODUCT SUMMARY FOR QUOTATION AMEND
        public void DaGetQuotationproductSummary(MdlSmrQuotationAmend values, string quotation_gid)
        {
            try
            {
                double grand_total = 0.00;
                msSQL = " select a.tmpquotationdtl_gid,a.product_gid, a.quotationdtl_gid,a.quotation_gid,a.productgroup_gid, " +
                        " a.productgroup_name,a.product_gid,a.product_name,a.product_code,a.qty_quoted, " +
                        " a.slno, a.product_price,tax_percentage, tax_amount,a.discount_percentage,a.discount_amount, a.tax_name, " +
                        " a.uom_gid,a.uom_name,a.price from smr_tmp_treceivequotationdtl a " +
                        " where a.quotation_gid = '" + quotation_gid + "' group by a.tmpquotationdtl_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<quoteamendproductlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new quoteamendproductlist
                        {
                            quotation_gid = dt["quotation_gid"].ToString(),
                            tmpquotationdtl_gid = dt["tmpquotationdtl_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            discount = dt["discount_amount"].ToString(),
                            product_total = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            grandtotal = dt["price"].ToString()


                        });
                        values.quoteamend_productlist = getModuleList;
                        values.grandtotal = grand_total;
                    }
                }
                dt_datatable.Dispose();
                values.grandtotal = grand_total;
      
                msSQL = " select f.taxsegment_gid,d.taxsegment_gid,e.taxsegment_name,d.tax_name,d.tax_gid, " +
                               " CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage,d.tax_amount, " +
                               " a.mrp_price,a.cost_price from acp_mst_ttaxsegment2product d " +
                               " left join acp_mst_ttaxsegment e on e.taxsegment_gid=d.taxsegment_gid " +
                               " left join crm_mst_tcustomer f on f.taxsegment_gid = e.taxsegment_gid " +
                               " left  join pmr_mst_tproduct a on a.product_gid=d.product_gid " +
                               " where a.product_gid='" + values.product_gid + "'   and f.customer_gid='" + values.customer_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getGetTaxSegmentList = new List<SegmentList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getGetTaxSegmentList.Add(new SegmentList
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
                        values.SegmentList = getGetTaxSegmentList;
                    }
                }

                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Temproary Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        // QUOTATION AMEND - PRODUCT ADD
        public void DaPostAmendProduct(string employee_gid, quoteamendproductlist values)
        {
            try
            {
                msGetGid1 = objcmnfunctions.GetMasterGID("VQDT");

                msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                if (values.discountpercentage == null || values.discountpercentage == "")
                {
                    lsdiscountpercentage = "0.00";
                }
                else
                {
                    lsdiscountpercentage = values.discountpercentage;
                }

                if (values.discountamount == null || values.discountamount == "")
                {
                    lsdiscountamount = "0.00";
                }
                else
                {
                    lsdiscountamount = values.discountamount;
                }


                if (values.tax_name == null || values.tax_name == "")
                {
                    lstaxname = "0.00";
                }
                else
                {
                    msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + values.tax_name + "'";
                    lstaxname = objdbconn.GetExecuteScalar(msSQL);
                }

                if (values.tax_name == null || values.tax_name == "")
                {
                    lspercentage1 = "0.00";
                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax where tax_gid='" + values.tax_name + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }


                if (values.tax_amount == null || values.tax_amount == "")
                {
                    lstaxamount = "0.00";
                }
                else
                {
                    lstaxamount = values.tax_amount;
                }
                if ((lspercentage1 == "" || lspercentage1 == null) && (lstaxamount == null || lstaxamount == ""))
                {
                    lstaxamount = "0.00";
                    lspercentage1 = "0";
                }

                int i = 0;

                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                  " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                  " WHERE product_gid='" + lsproductgid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    
                    if (objOdbcDataReader["producttype_name"].ToString() != "Services")
                    {
                        lsquotation_type = "Sales";
                    }
                    else
                    {
                        lsquotation_type = "Services";
                    }
                   

                }
                objOdbcDataReader.Close();
                try
                {

                    double exchange, costPrice, quantity, discountPercentage;

                    if (!double.TryParse(values.exchange_rate, out exchange))
                    {
                        exchange = 0.0;
                    }

                    if (!double.TryParse(values.unitprice, out costPrice))
                    {
                        costPrice = 0.0;
                    }


                    if (!double.TryParse(values.quantity, out quantity))
                    {
                        quantity = 0.0;
                    }


                    if (!double.TryParse(values.discountpercentage, out discountPercentage))
                    {
                        discountPercentage = 0.0;
                    }
                    if (!double.TryParse(values.tax_amount, out taxAmount))
                    {
                        taxAmount = 0.0;
                    }

                    subtotal = exchange * costPrice * quantity;
                    reCaldiscountAmount = Math.Round((subtotal * discountPercentage) / 100, 2);
                    reCalTaxAmount = taxAmount;
                    reCalTotalAmount = subtotal - reCaldiscountAmount + reCalTaxAmount;
                    reCalTotalAmount = Math.Round(reCalTotalAmount);
                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid='" + values.currency_code + "'";
                    string currencycode = objdbconn.GetExecuteScalar(msSQL);
                    if (currencycode != "INR")
                    {
                        if (reCalTotalAmount % 1 != 0)
                        {
                            // Round to the nearest 0.5 if total amount has cents
                            reCalTotalAmount = Math.Round(reCalTotalAmount * 2, MidpointRounding.AwayFromZero) / 2;

                        }
                    }

                }
                catch (Exception ex)
                {
                    values.message = "Exception occured while calculating!";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                    values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }

                msSQL = " insert into smr_tmp_treceivequotationdtl( " +
                        " tmpquotationdtl_gid," +
                        " quotation_gid," +
                        " product_gid," +
                        " product_code," +
                        " product_name," +
                        " product_price," +
                        " qty_quoted," +
                        " discount_percentage," +
                        " discount_amount," +
                        " uom_gid," +
                        " uom_name," +
                        " price," +
                        " created_by," +
                        " tax_name, " +
                        " tax1_gid, " +
                        " slno, " +
                        " quotation_type, " +
                        " tax_percentage," +
                        " taxsegment_gid, " +
                        " taxsegmenttax_gid, " +
                        " tax_amount " +
                        " ) values( " +
                        "'" + msGetGid1 + "'," +
                        "'" + values.quotation_gid + "'," +
                        "'" + lsproductgid + "'," +
                        "'" + values.product_code + "'," +
                        "'" + values.product_name + "', " +
                        "'" + values.unitprice + "', " +
                        "'" + values.quantity + "', " +
                        "'" + lsdiscountpercentage + "', " +
                        "'" + lsdiscountamount + "', " +
                        "'" + lsproductuomgid + "', " +
                        "'" + values.productuom_name + "', " +
                        "'" + reCalTotalAmount + "', " +
                        "'" + employee_gid + "', " +
                        "'" + lstaxname + "', " +
                        "'" + values.tax_name + "', " +
                        "'" + i + 1 + "', " +
                        "'" + lsquotation_type + "'," +
                        "'" + lspercentage1 + "'," +
                         "'" + values.taxsegment_gid + "'," +
                        "'" + values.taxsegmenttax_gid + "'," +
                        "'" + reCalTaxAmount + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)

                {
                    values.status = true;
                    values.message = "Product Added Successfully";
                    return;
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Product";
                    return;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // PRODUCT ON CHANGE EVENT

        public void DaGetOnChangeProductNameQOAmend(string customercontact_gid,string product_gid, MdlSmrQuotationAmend values)
        {
            try
            {

                if (customercontact_gid != null)
                {

                    msSQL = "  select a.product_price from smr_trn_tpricesegment2product a    left join smr_trn_tpricesegment2customer b on a.pricesegment_gid= b.pricesegment_gid " +
                        "  left join pmr_mst_tproduct c on a.product_gid=c.product_gid where b.customer_gid='" + customercontact_gid + "'   and a.product_gid='" + product_gid + "'";
                    lsproduct_price = objdbconn.GetExecuteScalar(msSQL);
                    if (lsproduct_price != "")
                    {

                        msSQL = " Select a.product_name, a.product_code,case when f.customer_gid is not null then(select a.product_price from smr_trn_tpricesegment2product a " +
                        " left join smr_trn_tpricesegment2customer b on a.pricesegment_gid= b.pricesegment_gid where b.customer_gid='" + customercontact_gid + "'" +
                        " and a.product_gid='" + product_gid + "') else (a.mrp_price)end as cost_price,  b.productuom_gid,b.productuom_name,c.productgroup_name," +
                        "c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid " +
                        "  left join pmr_mst_tproductgroup c on  a.productgroup_gid = c.productgroup_gid  left join smr_trn_tpricesegment2product e" +
                        " on a.product_gid=e.product_gid left join smr_trn_tpricesegment2customer f on e.pricesegment_gid=f.pricesegment_gid " +
                        " where a.product_gid='" + product_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        var getModuleList = new List<GetProductChangeQOAmend>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetProductChangeQOAmend
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    unitprice = lsproduct_price,

                                });
                                values.GetProductChange_QOAmend = getModuleList;
                            }
                        }
                        if (dt_datatable.Rows.Count > 0)
                        {
                            lsmrp_price = Convert.ToDecimal(lsproduct_price);
                        }
                        if (lsmrp_price > 0)
                        {
                            msSQL = " select f.taxsegment_gid,d.taxsegment_gid,e.taxsegment_name,d.tax_name,d.tax_gid, " +
                                " CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage,d.tax_amount, " +
                                " a.mrp_price,a.cost_price from acp_mst_ttaxsegment2product d " +
                                " left join acp_mst_ttaxsegment e on e.taxsegment_gid=d.taxsegment_gid " +
                                " left join crm_mst_tcustomer f on f.taxsegment_gid = e.taxsegment_gid " +
                                " left  join pmr_mst_tproduct a on a.product_gid=d.product_gid " +
                                " where a.product_gid='" + product_gid + "'   and f.customer_gid='" + customercontact_gid + "' ";
                            dt_datatable = objdbconn.GetDataTable(msSQL);

                            var getGetTaxSegmentList = new List<SegmentList>();
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    getGetTaxSegmentList.Add(new SegmentList
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
                                    values.SegmentList = getGetTaxSegmentList;
                                }
                            }

                            dt_datatable.Dispose();
                        }
                    }
                    else
                    {
                        msSQL = " Select a.product_name, a.product_code,a.cost_price,a.mrp_price," +
                            " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                             " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                            " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                        " where a.product_gid='" + product_gid + "' ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        var getModuleList = new List<GetProductChangeQOAmend>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetProductChangeQOAmend
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    unitprice = dt["mrp_price"].ToString(),

                                });
                                values.GetProductChange_QOAmend = getModuleList;
                            }
                        }

                        if (dt_datatable.Rows.Count > 0)
                        {
                            lsmrp_price = Convert.ToDecimal(dt_datatable.Rows[0]["mrp_price"]);
                        }
                        if (lsmrp_price > 0)
                        {
                            msSQL = " select f.taxsegment_gid,d.taxsegment_gid,e.taxsegment_name,d.tax_name,d.tax_gid, " +
                                " CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage,d.tax_amount, " +
                                " a.mrp_price,a.cost_price from acp_mst_ttaxsegment2product d " +
                                " left join acp_mst_ttaxsegment e on e.taxsegment_gid=d.taxsegment_gid " +
                                " left join crm_mst_tcustomer f on f.taxsegment_gid = e.taxsegment_gid " +
                                " left  join pmr_mst_tproduct a on a.product_gid=d.product_gid " +
                                " where a.product_gid='" + product_gid + "'   and f.customer_gid='" + customercontact_gid + "' ";
                            dt_datatable = objdbconn.GetDataTable(msSQL);

                            var getGetTaxSegmentList = new List<SegmentList>();
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    getGetTaxSegmentList.Add(new SegmentList
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
                                    values.SegmentList = getGetTaxSegmentList;
                                }
                            }
                            dt_datatable.Dispose();

                        }
                    }
                    dt_datatable.Dispose();
                }
                else
                {
                    values.status = false;
                    values.message = "Kindly Select Customer Before Adding Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Adding Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        // QUOTATION AMEND SUBMIT EVENT
        public void DaPostQuotationAmend(string employee_gid, PostQuoteAmend_List values)
        {
            try
            {

                string msSQL = "select * from smr_tmp_treceivequotationdtl " +
                    "where quotation_gid ='" + values.quotation_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {

                    string totalvalue = values.user_name;

                    string[] separatedValues = totalvalue.Split('|').Concat(totalvalue.Split(' ')).ToArray();

                    // Access individual components

                    string campaign_title = separatedValues[0];
                    string user_code = separatedValues[1];
                    string user_firstname = separatedValues[2];
                    string user_lastname = separatedValues.Length == 3 ? separatedValues[3] : null;



                    msSQL = "select customercontact_gid from crm_mst_tcustomercontact where customercontact_name = '" + values.customercontact_names + "'";
                    string lscustomercontactgid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select customerbranch_name from crm_mst_tcustomercontact where customercontact_gid=  '" + lscustomercontactgid + "'";
                     string lscustomercontact_names = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select branch_gid from hrm_mst_tbranch where branch_name=  '" + values.branch_name + "'";
                    string branch_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid=  '" + values.currency_code + "'";
                    string currencycode = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select tax_gid from acp_mst_ttax where tax_name=  '" + values.tax_name4 + "'";
                    string taxgid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select customer_gid from crm_mst_tcustomer where customer_name=  '" + values.customer_name + "'";
                    string customer_gid = objdbconn.GetExecuteScalar(msSQL);

                    string lsquotation_status = "Quotation Amended";


                    string trimmedName = user_code.Trim();


                    msSQL = "SELECT user_gid FROM adm_mst_tuser WHERE user_code = '" + trimmedName + "'";
                    string user = objdbconn.GetExecuteScalar(msSQL);

                    string uiDateStr = values.quotation_date;
                    DateTime uidate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string quotation_date = uidate.ToString("yyyy-MM-dd");

                    string msGetGid = objcmnfunctions.GetMasterGID("QARN");
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='QARN'";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = "insert into smr_trn_treceivequotation (" +
                                      " quotation_gid," +
                                      " quotation_referencenumber ," +
                                      " branch_gid," +
                                      " quotation_date," +
                                      " customer_name," +
                                      " customer_gid," +
                                      " customer_contact_person," +
                                      " customerbranch_gid," +
                                      " customercontact_gid," +
                                      " created_by," +
                                      " quotation_remarks," +
                                      " quotation_referenceno1," +
                                      " payment_days," +
                                      " delivery_days," +
                                      " Grandtotal," +
                                      " tax_gid," +
                                      " tax_name," +
                                      " tax_amount," +
                                      " quotation_status," +
                                      " contact_no," +
                                      " customer_address," +
                                      " contact_mail," +
                                      " addon_charge," +
                                      " additional_discount," +
                                      " currency_gid," +
                                      " currency_code," +
                                      " exchange_rate," +
                                      " total_amount," +
                                      " salesperson_gid," +
                                      " freight_terms," +
                                      " payment_terms," +
                                      " roundoff," +
                                      " total_price," +
                                      " freight_charges," +
                                      " buyback_charges," +
                                      " packing_charges," +
                                      " created_date," +
                                      " insurance_charges," +
                                      " termsandconditions" +
                                      ")values(" +
                                      " '" + values.quotation_gid + "NHA" + lsCode + "'," +
                                      " '" + values.quotation_referencenumber + "'," +
                                      " '" + branch_gid + "'," +
                                      " '" + quotation_date + "'," +
                                      " '" + values.customer_name + "'," +
                                      " '" + customer_gid + "'," +
                                      " '" + values.customercontact_names + "'," +
                                      " '" + lscustomerbranch_name + "'," +
                                      " '" + lscustomercontactgid + "'," +
                                      " '" + employee_gid + "'," +
                                      " '" + values.quotation_remarks + "'," +
                                      " '" + values.quotation_referenceno1 + "'," +
                                      " '" + values.payment_days + "'," +
                                      " '" + values.delivery_days + "'," +
                                      " '" + values.Grandtotal.Replace(",","") + "'," +
                                      " '" + taxgid + "'," +
                                      " '" + values.tax_name4 + "'," +
                                      " '" + values.tax_amount4 + "'," +
                                      " '" + lsquotation_status + "'," +
                                      " '" + values.customer_mobile + "', " +
                                      " '" + values.customer_address + "', " +
                                      " '" + values.customer_email + "', ";
                    if (values.addon_charge == "" | values.addon_charge == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.addon_charge + "',";
                    }
                    if (values.additional_discount == "" | values.additional_discount == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.additional_discount + "',";
                    }
                    msSQL += " '" + values.currency_code + "'," +
                                       " '" + currencycode + "', " +
                                       " '" + values.exchange_rate + "', ";
                    if (values.total_amount == "" | values.total_amount == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.total_amount.Replace(",", "") + "',";
                    }

                    msSQL += " '" + campaign_title + "' ," +
                         " '" + values.freight_terms + "', " +
                         " '" + values.payment_terms + "' ,";
                    if (values.roundoff == "" | values.roundoff == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.roundoff + "',";
                    }
                    msSQL += " '" + values.producttotalamount.Replace(",", "") + "', ";
                    if (values.freight_charges == "" | values.freight_charges == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.freight_charges + "',";
                    }
                    if (values.buyback_charges == "" | values.buyback_charges == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.buyback_charges + "',";
                    }
                    if (values.packing_charges == "" | values.packing_charges == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.packing_charges + "',";
                    }
                    msSQL += " '" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
                    if (values.insurance_charges == "" | values.insurance_charges == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.insurance_charges + "',";
                    }
                  msSQL += " '" + values.termsandconditions + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error Occurred while updating Quotation";
                        return;
                    }

                    else
                    {
                        msSQL = " select a.tmpquotationdtl_gid,a.quotation_type, a.quotationdtl_gid,a.quotation_gid,a.productgroup_gid, " +
                            " a.productgroup_name,a.product_gid,a.product_name,a.product_code,a.qty_quoted, " +
                            " a.slno, a.product_price,tax_percentage, tax_amount,a.discount_percentage,a.discount_amount, a.tax_name, " +
                            " a.uom_gid,a.uom_name, taxsegment_gid, taxsegmenttax_gid,a.price,a.tax1_gid from smr_tmp_treceivequotationdtl a " +
                            " where a.quotation_gid = '" + values.quotation_gid + "' group by a.product_gid ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {

                            foreach (DataRow dt in dt_datatable.Rows)

                            {
                                msgetGid2 = objcmnfunctions.GetMasterGID("VQDC");

                                int i = 0;
                                msSQL = "insert into smr_trn_treceivequotationdtl (" +
                                       " quotationdtl_gid ," +
                                       " quotation_gid," +
                                       " product_gid ," +
                                       " productgroup_gid," +
                                       " productgroup_name," +
                                       " product_name," +
                                       " product_code," +
                                       " product_price," +
                                       " qty_quoted," +
                                       " discount_percentage," +
                                       " discount_amount," +
                                       " uom_gid," +
                                       " uom_name," +
                                       " price," +
                                       " tax_name," +
                                       " tax_percentage," +
                                       " taxsegment_gid," +
                                       " taxsegmenttax_gid," +
                                       " quote_type," +
                                       " slno," +
                                       " tax_amount" +
                                       ")values(" +
                                       " '" + msgetGid2 + "'," +
                                       " '" + values.quotation_gid + "NHA" + lsCode + "'," +
                                       " '" + dt["product_gid"].ToString() + "'," +
                                       " '" + dt["productgroup_gid"].ToString() + "'," +
                                       " '" + dt["productgroup_name"].ToString() + "'," +
                                       " '" + dt["product_name"].ToString() + "'," +
                                       " '" + dt["product_code"].ToString() + "'," +
                                       " '" + dt["product_price"].ToString() + "'," +
                                       " '" + dt["qty_quoted"].ToString() + "'," +
                                       " '" + dt["discount_percentage"].ToString() + "'," +
                                        " '" + dt["discount_amount"].ToString() + "'," +
                                       " '" + dt["uom_gid"].ToString() + "'," +
                                        " '" + dt["uom_name"].ToString() + "'," +
                                       " '" + dt["price"] + "'," +
                                       " '" + dt["tax_name"].ToString() + "'," +
                                       " '" + dt["tax_percentage"].ToString() + "'," +
                                       " '" + dt["taxsegment_gid"].ToString() + "'," +
                                       " '" + dt["taxsegmenttax_gid"].ToString() + "'," +
                                       " '" + dt["quotation_type"].ToString() + "'," +
                                       " '" + i + 1 + "', " +
                                       " '" + dt["tax_amount"].ToString() + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 0)
                                {
                                    values.status = false;
                                    values.message = "Error occured while Inserting into Quotationdtl";
                                    return;
                                }

                            }
                        }


                        msSQL = "select quote_type from smr_trn_treceivequotationdtl where quotation_gid='" + values.quotation_gid + "NHA" + lsCode + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                       
                        if (objOdbcDataReader.HasRows == true)
                        {
                            lsquotation_type = "sales";


                        }

                        else
                        {
                            lsquotation_type = "Service";
                        }
                        objOdbcDataReader.Close();


                        msSQL = " Update smr_trn_treceivequotation Set " +
                                  " quotation_type = '" + lsquotation_type + "'" +                               
                                  " where quotation_gid ='" + values.quotation_gid + "NHA" + lsCode + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " Select customer_gid from crm_mst_tcustomer where customer_name='" + values.customer_name + "'";
                        string lscustomergid = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " Select leadbank_gid from crm_trn_tleadbank where customer_gid='" + lscustomergid + "'";
                        string lsleadbankgid = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " Select leadbankcontact_gid from crm_trn_tleadbankcontact where leadbank_gid='" + lsleadbankgid + "'";
                        string lsleadbankcontact = objdbconn.GetExecuteScalar(msSQL);

                        string lsstage = "4";
                       string  msgetlead2campaign_gid = objcmnfunctions.GetMasterGID("BLCC");
                        string lsso = "N";

                        msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + user + "'";
                        string employee = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " select campaign_gid from smr_trn_tcampaign where campaign_title='" + campaign_title + "'";
                        string campaignGid = objdbconn.GetExecuteScalar(msSQL);



                        msSQL = " Insert into crm_trn_tenquiry2campaign ( " +
                                                          " lead2campaign_gid, " +
                                                          " quotation_gid, " +
                                                          " so_status, " +
                                                          " created_by, " +
                                                          " customer_gid, " +
                                                          " leadstage_gid," +
                                                          " created_date, " +
                                                          " campaign_gid," +
                                                          " assign_to ) " +
                                                          " Values ( " +
                                                          "'" + msgetlead2campaign_gid + "'," +
                                                          "'" + msGetGid + "'," +
                                                          "'" + lsso + "'," +
                                                          "'" + employee_gid + "'," +
                                                          "'" + customer_gid + "'," +
                                                          "'" + lsstage + "'," +
                                                          "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                          "'" + campaignGid + "'," +
                                                          "'" + employee + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        msSQL =   " Update smr_trn_treceivequotation Set " +
                                  " leadbank_gid = '" + lsleadbankgid + "'," +
                                  " leadbankcontact_gid = '" + lsleadbankcontact + "'" +
                                  " where quotation_gid ='" + values.quotation_gid + "NHA" + lsCode + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }


                    msSQL = " delete from smr_tmp_treceivequotationdtl " +
                            " where created_by='" + employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error occured while Inserting into Temp Data";
                        return;
                    }


                    msgetGid4 = objcmnfunctions.GetMasterGID("PODC");
                    {
                        msSQL = " insert into smr_trn_tapproval ( " +
                                " approval_gid, " +
                                " approved_by, " +
                                " approved_date, " +
                                " submodule_gid, " +
                                " qoapproval_gid " +
                                " ) values ( " +
                                "'" + msgetGid4 + "'," +
                                " '" + employee_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'SMRSMRQAP'," +
                                "'" + values.quotation_gid + "NHA" + lsCode + "') ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        msSQL = "select approval_flag from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + values.quotation_gid + "NHA" + lsamendcount +  "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == false)
                        {
                            
                            msSQL = " Update smr_trn_treceivequotation Set " +
                                   " quotation_status = 'QUotation Amended', " +
                                   " approved_by = '" + employee_gid + "', " +
                                   " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                   " where quotation_gid = '" + values.quotation_gid + "NHA" + lsCode + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                           
                        }
                        else
                        {
                            msSQL = "select approved_by from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + values.quotation_gid + "NHA" + lsamendcount +  "'";
                            objOdbcDataReader1 = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader1.RecordsAffected == 1)
                            {
                               
                                msSQL = " update smr_trn_tapproval set " +
                               " approval_flag = 'Y', " +
                               " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                               " where approved_by = '" + employee_gid + "'" +
                               " and qoapproval_gid = '" + values.quotation_gid + "NHA" + lsCode + "' and submodule_gid='SMRSMRQAP'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                msSQL = " Update smr_trn_treceivequotation Set " +
                                       " quotation_status = 'Quotation Amended', " +
                               " approved_by = '" + employee_gid + "', " +
                               " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                               " where quotation_gid = '" + values.quotation_gid + "NHA" + lsCode + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                
                            }
                            else if (objOdbcDataReader1.RecordsAffected > 1)
                            {
                               
                                msSQL = " update smr_trn_tapproval set " +
                                       " approval_flag = 'Y', " +
                                       " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                       " where approved_by = '" + employee_gid + "'" +
                                       " and qoapproval_gid = '" + values.quotation_gid + "' and submodule_gid='SMRSMRQAP'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                               
                            }
                        }
                        objOdbcDataReader.Close();
                        objOdbcDataReader1.Close();

                    }

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Quotation Amend Successfully!";
                        return;
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Amend Quotation!";
                        return;
                    }
                }
                else
                {
                    values.status = true;
                    values.message = "Select one Product to Raise Quotation";
                    return;
                
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        //

        public void DaDeleteAmendProductSummary(string tmpquotationdtl_gid, quoteamendproductlist values)
        {

            try
            {

                msSQL = " delete from smr_tmp_treceivequotationdtl " +
                        " where tmpquotationdtl_gid='" + tmpquotationdtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)

                {
                    values.status = true;
                    values.message = "Product Deleted Successfully!";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting The Product!";
                }


            }
            
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting The Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        public void DaPostUpdateQuotationAmendProduct(string employee_gid, quoteamendproductlist values)
        {
            try
            {
               
                string lstax_amount;

                msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.unit + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select tax_gid from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                string lstaxname = objdbconn.GetExecuteScalar(msSQL);

                if (values.tax_amount == null)
                {
                    lstax_amount = "0";
                }
                else
                {

                    lstax_amount = values.tax_amount;
                }


                msSQL = "update smr_tmp_treceivequotationdtl set " +
                         " product_gid='" + lsproductgid + "', " +
                         " product_name='" + values.product_name + "', " +
                         " product_code='" + values.product_code + "', " +
                         " uom_gid='" + lsproductuomgid + "', " +
                         " uom_name='" + values.unit + "', " +
                         " discount_amount='" + values.discountamount + "', " +
                         " discount_percentage='" + values.discountpercentage + "', " +
                         " qty_quoted='" + values.quantity + "', " +
                         " tax1_gid='" + lstaxname + "', " +
                         " tax_name='" + values.tax_name + "'," +
                         " price='" + values.total_amount + "'," +
                         " product_price='" + values.unitprice + "'," +
                         " tax_amount='" + lstax_amount + "'" +
                         " where tmpquotationdtl_gid='" + values.tmpquotationdtl_gid + "' ";


                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While Updating Product";
                }

            }

            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Sales Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }



        }



        // EDIT AMEND PRODUCT

        public void DaGetQuotationAmendEditProductSummary(string tmpquotationdtl_gid, MdlSmrQuotationAmend values)
        {
            try
            {

                msSQL = " Select a.tmpquotationdtl_gid, a.slno,a.tax_name,a.enquiry_gid,a.tax1_gid,a.tax_amount, " +
                    " d.productuom_name,a.quotationdtl_gid,a.quotation_gid,a.product_gid,a.product_name,  " +
                    " format(a.product_price,2) as product_price ,a.product_code,a.qty_quoted,a.product_remarks,a.uom_gid,  " +
                    " a.uom_name,format(a.price,2) as price , format(a.discount_percentage,2)as discount_percentage," +
                    " format(a.discount_amount,2)as discount_amount from smr_tmp_treceivequotationdtl a  " +
                    " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                    " left join pmr_mst_tproductgroup c on a.productgroup_gid=c.productgroup_gid  " +
                    " left join pmr_mst_tproductuom d on b.productuom_gid=d.productuom_gid " +
                    " where a.tmpquotationdtl_gid = '" + tmpquotationdtl_gid + "' group by a.product_gid";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<AmendproductList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new AmendproductList
                        {
                            tmpquotationdtl_gid = dt["tmpquotationdtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            //productgroup_name = dt["productgroup_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            price = dt["price"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            tax_gid = dt["tax1_gid"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_name = dt["tax_name"].ToString(),

                        });
                        values.Amendproduct_List = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Direct Enquiry Edit ProductSummary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
    }
}