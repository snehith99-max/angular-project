using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Web.Http.Routing.Constraints;


namespace ems.sales.DataAccess
{
    public class DaSmrQuoteToOrder
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        DataTable dt_datatable;
        string msSQL = string.Empty;
        int mnResult, mnResult2;
        decimal lsmrp_price;
        double exchange, costPrice, quantity, discountPercentage, subtotal, discountAmount, reCalTotalAmount, reCaltax_amount, reCaldiscountAmount, reCalTaxAmount, taxAmount;
        string SalesOrderGID, TempSOGID, lstaxgid, lsproducttype, lsorder_type;
        OdbcDataReader objOdbcDataReader;
        string start_date, mssalesorderGID, lstaxamount, end_date, lstype1, lscustomerbranch_name, lsdiscountamount, lsdiscountpercentage, lsquotation_type, msGetGid, lsproductgid1, lsenquiry_type, lspercentage1;
        string lsproductgid, lsproductuom_gid, lsproduct_name, lsproductuom_name, lscompany_code, mssalesorderGID1
            , lsrefno, msgetlead2campaign_gid, msINGetGID, msGetGID;
        // Summary bind

        public void DaGetRaiseSOSummary(string quotation_gid, string employee_gid, MdlSmrQuoteToOrder values)
        {
            try
            {
                msSQL = " delete from smr_tmp_tsalesorderdtl where employee_gid='" + employee_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select a.quotation_gid,case when b.leadbank_gid is null then k.leadbank_gid else b.leadbank_gid end as leadbank_gid," +
                    " case when g.customer_gid is null then q.customer_gid  else g.customer_gid end as customer_gid, " +
                    " case when g.customer_name is null then q.customer_name else g.customer_name end as customer_name_1 ," +
                    " case when g.gst_number is null  then q.gst_number else g.gst_number end as gst_number," +
                    " a.currency_code,a.quotation_referencenumber,a.currency_gid, a.termsandconditions," +
                    " DATE_format(a.quotation_date, '%d-%m-%Y') as quotation_date,  a.quotation_referencenumber,a.quotation_remarks," +
                    " a.branch_gid, a.tax_name4, format(a.tax_amount4,2) as tax_amount, a.quotation_referenceno1,a.payment_days," +
                    " e.branch_name,a.customerbranch_gid,a.freight_terms, " +
                    " case when c.address1 is null then k.leadbank_address1 else c.address1 end as address1," +
                    "case when c.address2 is null then k.leadbank_address2 else c.address2 end as address2," +
                    " a.payment_terms,h.currency_code as code,a.exchange_rate,h.currencyexchange_gid, h.exchange_rate as code," +
                    " a.contact_mail,a.contact_no,concat(a.customer_contact_person) as contact_person,  a.customer_address," +
                    " concat(a.contact_mail,'',a.contact_no) as customer_details," +
                    " format(a.Grandtotal_l, 2) as Grandtotal_l,format(a.buyback_charges, 2) as buyback_charges,a.currency_gid,a.exchange_rate as rate, format(a.Grandtotal,2) as Grandtotal ," +
                    " a.addon_charge as addon_charge,a.additional_discount as additional_discount," +
                    " a.customer_name, format(a.gst_percentage, 2) as gst_percentage, a.tax_gid,format(a.total_amount, 2) as total_amount," +
                    " format(a.total_price, 2) as total_price, a.payment_days,a.delivery_days,a.enquiry_refno,m.campaign_gid," +
                    " concat(m.campaign_title, ' | ', j.user_code, ' | ', j.user_firstname, ' ', j.user_lastname) as salesperson_gid, " +
                    " concat(j.user_code, ' | ', j.user_firstname, ' ', j.user_lastname) as user_name," +
                    " c.customercontact_name,c.mobile,c.email,m.campaign_title,j.user_gid," +
                    " a.freight_charges as freight_charges,format(a.packing_charges, 2) as packing_charges, " +
                    " a.roundoff as roundoff ,  format(a.insurance_charges, 2) as insurance_charges,c.zip_code,c.city" +
                    " from smr_trn_treceivequotation a  " +
                    " left join crm_trn_tleadbank b on b.leadbank_gid=a.leadbank_gid " +
                    " left join crm_mst_tcustomer g on a.customer_gid = g.customer_gid  " +
                    " left join crm_trn_tleadbank k on k.leadbank_gid=a.customer_gid " +
                    " left join crm_mst_tcustomer q on k.customer_gid = q.customer_gid  " +
                    " left join crm_trn_tcurrencyexchange h on h.currencyexchange_gid = a.currency_gid  " +
                    " left join crm_mst_tcustomercontact c on g.customer_gid = c.customer_gid  " +
                    " left join hrm_mst_tbranch e on e.branch_gid = a.branch_gid  " +
                    " left join crm_trn_tenquiry2campaign f on a.quotation_gid = f.quotation_gid  " +
                    " left join smr_trn_tcampaign m on f.campaign_gid = m.campaign_gid " +
                    " left join hrm_mst_temployee i on f.assign_to = i.employee_gid  " +
                    " left join adm_mst_tuser j on a.salesperson_gid = j.user_gid  " +
                    " where a.quotation_gid = '" + quotation_gid + "' group by a.quotation_gid";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetsummaryList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetsummaryList
                        {
                            quotation_gid = dt["quotation_gid"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            salesorder_date = dt["quotation_date"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            quotation_referenceno1 = dt["quotation_referencenumber"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            so_referencenumber = dt["enquiry_refno"].ToString(),
                            customercontact_names = dt["contact_person"].ToString(),
                            customer_mobile = dt["contact_no"].ToString(),
                            customer_email = dt["contact_mail"].ToString(),
                            so_remarks = dt["quotation_remarks"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            customer_details = dt["customer_details"].ToString(),
                            freight_terms = dt["freight_terms"].ToString(),
                            exchange_rate = dt["rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            addon_charge = dt["addon_charge"].ToString(),
                            additional_discount = dt["additional_discount"].ToString(),
                            buyback_charges = dt["buyback_charges"].ToString(),
                            insurance_charges = dt["insurance_charges"].ToString(),
                            freight_charges = dt["freight_charges"].ToString(),
                            packing_charges = dt["packing_charges"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            delivery_days = dt["delivery_days"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            total_amount = dt["total_amount"].ToString(),
                            tax_amount4 = dt["tax_amount"].ToString(),
                            tax_name4 = dt["tax_name4"].ToString(),
                            user_gid = dt["user_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            campaign_gid = dt["campaign_gid"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            zip_code = dt["zip_code"].ToString(),
                            city = dt["city"].ToString(),

                        });
                        values.SO_list = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Raise Sales Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void GetOnChangeProductsNameQTO(string customercontact_gid, string product_gid, MdlSmrQuoteToOrder values)
        {
            try
            {
                if (customercontact_gid != null)
                {

                    msSQL = "  select a.product_price from smr_trn_tpricesegment2product a    left join smr_trn_tpricesegment2customer b on a.pricesegment_gid= b.pricesegment_gid " +
                        "  left join pmr_mst_tproduct c on a.product_gid=c.product_gid where b.customer_gid='" + customercontact_gid + "'   and a.product_gid='" + product_gid + "'";
                    string lsproduct_price = objdbconn.GetExecuteScalar(msSQL);
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

                        var getModuleList = new List<GetOnchangeProdQTO>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetOnchangeProdQTO
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    selling_price = lsproduct_price,

                                });
                                values.GetOnchangeProd_QTO = getModuleList;
                            }
                        }


                        if (dt_datatable.Rows.Count > 0)
                        {
                            lsmrp_price = Convert.ToDecimal(lsproduct_price);
                        }
                        if (lsmrp_price > 0)
                        {
                            msSQL = " select f.taxsegment_gid,d.taxsegment_gid,e.taxsegment_name,d.tax_name,d.tax_gid, " +
                                " case when d.tax_percentage = ROUND(d.tax_percentage) then ROUND(d.tax_percentage) else d.tax_percentage end as tax_percentage,d.tax_amount, " +
                                " a.mrp_price,a.cost_price from acp_mst_ttaxsegment2product d " +
                                " left join acp_mst_ttaxsegment e on e.taxsegment_gid=d.taxsegment_gid " +
                                " left join crm_mst_tcustomer f on f.taxsegment_gid = e.taxsegment_gid " +
                                " left  join pmr_mst_tproduct a on a.product_gid=d.product_gid " +
                                " where a.product_gid='" + product_gid + "'   and f.customer_gid='" + customercontact_gid + "' ";
                            dt_datatable = objdbconn.GetDataTable(msSQL);

                            var getGetTaxSegmentList = new List<GetQTOTaxSegmentList>();
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    getGetTaxSegmentList.Add(new GetQTOTaxSegmentList
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
                                    values.QTOTaxSegmentList = getGetTaxSegmentList;
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
                        var QTgetModuleList = new List<GetOnchangeProdQTO>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                QTgetModuleList.Add(new GetOnchangeProdQTO
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    selling_price = dt["mrp_price"].ToString(),

                                });
                                values.GetOnchangeProd_QTO = QTgetModuleList;
                            }
                        }

                        if (dt_datatable.Rows.Count > 0)
                        {
                            lsmrp_price = Convert.ToDecimal(dt_datatable.Rows[0]["mrp_price"]);
                        }
                        if (lsmrp_price > 0)
                        {
                            msSQL = " select f.taxsegment_gid,d.taxsegment_gid,e.taxsegment_name,d.tax_name,d.tax_gid, " +
                                " case when d.tax_percentage = ROUND(d.tax_percentage) then ROUND(d.tax_percentage) else d.tax_percentage end as tax_percentage,d.tax_amount, " +
                                " a.mrp_price,a.cost_price from acp_mst_ttaxsegment2product d " +
                                " left join acp_mst_ttaxsegment e on e.taxsegment_gid=d.taxsegment_gid " +
                                " left join crm_mst_tcustomer f on f.taxsegment_gid = e.taxsegment_gid " +
                                " left  join pmr_mst_tproduct a on a.product_gid=d.product_gid " +
                                " where a.product_gid='" + product_gid + "'   and f.customer_gid='" + customercontact_gid + "' ";
                            dt_datatable = objdbconn.GetDataTable(msSQL);

                            var getGetTaxSegmentList = new List<GetQTOTaxSegmentList>();
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    getGetTaxSegmentList.Add(new GetQTOTaxSegmentList
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
                                    values.QTOTaxSegmentList = getGetTaxSegmentList;
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
        public void DaGetDeleteQuotetoOrderProductSummary(string tmpsalesorderdtl_gid, GetsummaryList values)
        {
            try
            {

                msSQL = " delete from smr_tmp_tsalesorderdtl " +
                    " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product  Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaPostQuotationToOrder(string employee_gid, string user_gid, postsalesQuote_list values)
        {
            try
            {

                string USERGid = values.user_gid;
                string campaignTitle = values.campaign_title;
                string campaignGid = values.campaign_gid;
                string lscustomercontactname = "";
                string lscustomeremail = "";
                string lscustomermobile = "";
                string lscustomercontactgid = "";

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + values.tax_name4 + "'";
                string lstaxname1 = objdbconn.GetExecuteScalar(msSQL);


                string lscustomerbranch = "H.Q";
                string lscampaign_gid = "NO CAMPAIGN";

                msSQL = " select * from smr_tmp_tsalesorderdtl " +
                    " where employee_gid='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count != 0)
                {

                    string inputDate = values.salesorder_date;
                    DateTime uiDate = DateTime.ParseExact(inputDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string salesorder_date = uiDate.ToString("yyyy-MM-dd");

                    if (values.start_date == null || values.start_date == "")
                    {
                        start_date = "0000-00-00";
                    }
                    else
                    {
                        string uiDateStr2 = values.start_date;
                        DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        start_date = uiDate2.ToString("yyyy-MM-dd");
                    }

                    if (values.end_date == null || values.end_date == "")
                    {
                        end_date = "0000-00-00";
                    }
                    else
                    {
                        string uiDateStr2 = values.end_date;
                        DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        end_date = uiDate2.ToString("yyyy-MM-dd");
                    }
                    msSQL = " select customer_name from crm_mst_tcustomer where customer_gid='" + values.customer_gid + " '";
                    string lscustomername = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select currencyexchange_gid from crm_trn_tcurrencyexchange where currency_code='" + values.currency_code + " '";
                    string currencygid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " select customercontact_name,customerbranch_name,customercontact_gid, email, mobile, address1, address2 from crm_mst_tcustomercontact where customer_gid ='" + values.customer_gid + " '";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                  
                    if (objOdbcDataReader.HasRows == true)
                    {
                        lscustomercontactname = objOdbcDataReader["customercontact_name"].ToString();
                        lscustomeremail = objOdbcDataReader["email"].ToString();
                        lscustomermobile = objOdbcDataReader["mobile"].ToString();
                        lscustomercontactgid = objOdbcDataReader["customercontact_gid"].ToString();

                        objOdbcDataReader.Close();

                    }
                   
                    string lslocaladdon = "0.00";
                    string lslocaladditionaldiscount = "0.00";
                    string lslocalgrandtotal = " 0.00";
                    string lsgst = "0.00";
                    string lsamount4 = "0.00";
                    //string lsproducttotalamount = "0.00";

                    double totalAmount = double.TryParse(values.tax_amount4, out double totalpriceValue) ? totalpriceValue : 0;
                    double addonCharges = double.TryParse(values.addon_charge, out double addonChargesValue) ? addonChargesValue : 0;
                    double freightCharges = double.TryParse(values.freight_charges, out double freightChargesValue) ? freightChargesValue : 0;
                    double packingCharges = double.TryParse(values.packing_charges, out double packingChargesValue) ? packingChargesValue : 0;
                    double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                    double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                    double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                    double buybackCharges = double.TryParse(values.buyback_charges, out double buybackChargesValue) ? buybackChargesValue : 0;

                    double grandTotal = (totalAmount + addonCharges + freightCharges + packingCharges + insuranceCharges + roundoff) - additionaldiscountAmount - buybackCharges;


                    lsrefno = objcmnfunctions.GetMasterGID("SO");

                    mssalesorderGID = objcmnfunctions.GetMasterGID("VSOP");

                    msSQL = " insert  into smr_trn_tsalesorder (" +
                             " salesorder_gid ," +
                             " branch_gid ," +
                             " salesorder_date," +
                             " customer_gid," +
                             " customer_name," +
                             " customer_contact_gid," +
                             " customerbranch_gid," +
                             " customer_contact_person," +
                             " customer_address," +
                             " customer_email, " +
                             " customer_mobile, " +
                             " created_by," +
                             " so_referenceno1 ," +
                             " so_remarks," +
                             " payment_days, " +
                             " delivery_days, " +
                             " Grandtotal, " +
                             " termsandconditions, " +
                             " salesorder_status, " +
                             " addon_charge, " +
                             " additional_discount, " +
                             " addon_charge_l, " +
                             " additional_discount_l, " +
                             " grandtotal_l, " +
                             " currency_code, " +
                             " currency_gid, " +
                             " exchange_rate, " +
                             " shipping_to, " +
                             " freight_terms, " +
                             " payment_terms," +
                             " tax_gid," +
                             " tax_name, " +
                             " gst_amount," +
                             " total_price," +
                             " total_amount," +
                             " tax_amount," +
                             " vessel_name," +
                             " salesperson_gid," +
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
                             //" contact_email_address, " +
                             " renewal_flag ," +
                             "created_date" +
                             " )values(" +
                             " '" + mssalesorderGID + "'," +
                             " '" + values.branch_gid + "'," +
                             " '" + salesorder_date + "'," +
                             " '" + values.customer_gid + "'," +
                             " '" + lscustomername.Replace("'", "\\\'") + "'," +
                             " '" + lscustomercontactgid + "'," +
                             " '" + lscustomerbranch + "'," +
                             " '" + lscustomercontactname.Replace("'", "\\\'") + "'," +
                             " '" + values.customer_address.Replace("'", "\\\'") + "'," +
                             " '" + lscustomeremail + "'," +
                             " '" + lscustomermobile + "'," +
                             " '" + employee_gid + "'," +
                             " '" + lsrefno + "',";
                    if(values.so_remarks != null)
                    {
                        msSQL += " '" + values.so_remarks.Replace("'","\\\'") + "',";
                    }
                    else
                    {
                        msSQL += " '" + values.so_remarks + "',";
                    }
                            
                          msSQL +=   " '" + values.payment_days.Replace("'", "\\\'") + "'," +
                             " '" + values.delivery_days.Replace("'", "\\\'") + "'," +
                             " '" + values.grandtotal.Replace(",", "").Trim() + "'," +
                             " '" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "\\\'") + "'," +
                             " 'Approved'," +
                             "'" + addonCharges + "'," +
                             "'" + additionaldiscountAmount + "'," +
                             "'" + addonCharges + "'," +
                             "'" + additionaldiscountAmount + "'," +
                             " '" + lslocalgrandtotal + "'," +
                             " '" + values.currency_code + "'," +
                             " '" + currencygid + "'," +
                             " '" + values.exchange_rate + "'," +
                             " '" + (String.IsNullOrEmpty(values.shipping_to)? values.shipping_to : values.shipping_to.Replace("'","\\\'")) + "'," +
                             "'" + values.freight_terms.Replace("'", "\\\'") + "'," +
                             "'" + values.payment_terms.Replace("'", "\\\'") + "'," +
                             " '" + values.tax_name4 + "'," +
                             " '" + lstaxname1.Replace("'","\\\'") + "', " +
                             "'" + lsgst + "'," +
                             "'" + values.totalamount.Replace(",", "").Trim() + "'," +
                             "'" + values.grandtotal + "'," +
                             "'" + totalAmount + "'," +
                             " '" + values.vessel_name + "'," +
                             " '" + USERGid + "'," +
                             " '" + start_date + "'," +
                             " '" + end_date + "'," +
                             " '" + roundoff + "'," +
                             " '" + addonCharges + "'," +
                            "'" + additionaldiscountAmount + "'," +
                            "'" + freightCharges + "'," +
                            "'" + buybackCharges + "'," +
                            "'" + packingCharges + "'," +
                            "'" + insuranceCharges + "'," +
                            "'" + values.customerinstruction.Replace("'", "\\\'") + "'," +
                           // "'" + values.contactemailaddress.Replace("'", "\\\'") + "'," +
                           "'" + values.renewal_mode + "'," +
                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = " Some Error Occurred While Inserting Salesorder Details";
                        return;
                    }
                    else
                    {
                        msSQL = " insert  into acp_trn_torder (" +
                               " salesorder_gid ," +
                               " branch_gid ," +
                               " salesorder_date," +
                               " customer_gid," +
                               " customer_name," +
                               " customer_contact_gid," +
                               " customerbranch_gid," +
                               " customer_contact_person," +
                               " customer_address," +
                               " customer_email, " +
                               " customer_mobile, " +
                               " created_by," +
                               " so_remarks," +
                               " so_referencenumber," +
                               " payment_days, " +
                               " delivery_days, " +
                               " Grandtotal, " +
                               " termsandconditions, " +
                               " salesorder_status, " +
                               " addon_charge, " +
                               " additional_discount, " +
                               " addon_charge_l, " +
                               " additional_discount_l, " +
                               " grandtotal_l, " +
                               " currency_code, " +
                               " currency_gid, " +
                               " exchange_rate, " +
                               " updated_addon_charge, " +
                               " updated_additional_discount, " +
                               " shipping_to, " +
                               " campaign_gid, " +
                               " vessel_name," +
                               " roundoff," +
                               " salesperson_gid, " +
                               " freight_charges," +
                               " buyback_charges," +
                               " packing_charges," +
                               " insurance_charges " +
                               ") values(" +
                               " '" + mssalesorderGID + "'," +
                               " '" + values.branch_gid + "'," +
                               " '" + salesorder_date + "'," +
                               " '" + values.customer_gid + "'," +
                               " '" + lscustomername.Replace("'", "\\\'") + "'," +
                               " '" + values.customercontact_gid + "'," +
                               " '" + lscustomerbranch + "'," +
                               " '" + values.customercontact_names.Replace("'", "\\\'") + "'," +
                               " '" + values.customer_address.Replace("'", "\\\'") + "'," +
                               " '" + values.customer_email + "'," +
                               " '" + values.customer_mobile + "'," +
                               " '" + employee_gid + "',";
                               if (values.so_remarks != null)
                        {
                            msSQL += " '" + values.so_remarks.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += " '" + values.so_remarks + "',";
                        }

                       msSQL += " '" + lsrefno + "'," +
                               " '" + values.payment_days.Replace("'", "\\\'") + "'," +
                               " '" + values.delivery_days.Replace("'", "\\\'") + "'," +
                               " '" + values.grandtotal + "'," +
                               " '" + values.termsandconditions.Replace("'", "\\\'") + "'," +
                               " 'Approved'," +
                               "'" + addonCharges + "'," +
                               "'" + additionaldiscountAmount + "'," +
                               "'" + lslocaladdon + "'," +
                               "'" + lslocaladditionaldiscount + "'," +
                               "'" + lslocalgrandtotal + "'," +
                               "'" + values.currencyexchange_gid + "'," +
                               "'" + values.currency_code + "'," +
                               "'" + values.exchange_rate + "'," +
                               "'" + addonCharges + "'," +
                               "'" + additionaldiscountAmount + "'," +
                               "'" + (String.IsNullOrEmpty(values.shipping_to) ? values.shipping_to : values.shipping_to.Replace("'", "\\\'")) + "'," +
                               "'" + lscampaign_gid + "'," +
                               "'" + values.vessel_name + "'," +
                               "'" + roundoff + "'," +
                               "'" + USERGid + "'," +
                               "'" + freightCharges + "'," +
                               "'" + buybackCharges + "'," +
                               "'" + packingCharges + "'," +
                               "'" + insuranceCharges + "')";
                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult2 == 1)
                        {
                            values.status = true;
                        }
                    }
                    if (values.renewal_mode == "Y")
                    {
                        msINGetGID = objcmnfunctions.GetMasterGID("BRLP");

                        msSQL = " Insert into crm_trn_trenewal ( " +
                                " renewal_gid, " +
                                " renewal_flag, " +
                                " frequency_term, " +
                                " customer_gid," +
                                " renewal_date, " +
                                " salesorder_gid, " +
                                " created_by, " +
                                " renewal_type, " +
                                " created_date) " +
                               " Values ( " +
                                 "'" + msINGetGID + "'," +
                                 "'" + values.renewal_mode + "'," +
                                 "'" + values.frequency_terms + "'," +
                                 "'" + values.customer_gid + "'," +
                                 "'" + values.renewal_date + "'," +
                                 "'" + mssalesorderGID + "'," +
                                 "'" + employee_gid + "'," +
                                 "'sales'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    msSQL = "SELECT a.tmpsalesorderdtl_gid,a.taxsegment_gid,a.salesorder_gid,a.customerproduct_code," +
                        " a.tax_name,a.tax_amount,a.tax1_gid,a.tax2_gid,a.tax3_gid,a.tax_percentage,a.tax_name2,a.tax_amount2,a.tax_percentage2," +
                        "a.tax_name3,a.tax_amount3,a.tax_percentage3,a.order_type, p.customer_gid,a.tax_amount, a.salesorderdtl_gid," +
                        " a.salesorder_gid, a.product_gid, a.productgroup_gid, " +
                   " v.productgroup_name, a.product_name, format(a.product_price, 2) as product_price," +
                   " b.product_code, a.qty_quoted, a.product_remarks, " +
                   " a.uom_gid, a.vendor_gid, a.slno, a.uom_name, format(a.price, 2) as price, " +
                   " format(a.discount_percentage,2) as discount_percentage, " +
                   "format(a.discount_amount,2) as discount_amount, " +
                   " format(a.selling_price, '0.00') as selling_price,a.product_remarks, " +
                   " concat( case when a.tax_name is null then '' else a.tax_name end, ' '," +
                   "case when a.tax_percentage = '0' then '' else concat('',a.tax_percentage,'%') end , " +
                   " case when a.tax_amount = '0' then '' else concat(':',a.tax_amount) end) as tax, " +
                   " concat(case when a.tax_name2 is null then '' else a.tax_name2 end, ' ', " +
                   " case when a.tax_percentage2 = '0' then '' else concat('', a.tax_percentage2, '%') end, " +
                   " case when a.tax_amount2 = '0' then '' else concat(':', a.tax_amount2) end) as tax2, " +
                   " concat(  case when a.tax_name3 is null then '' else a.tax_name3 end, ' ', " +
                   " case when a.tax_percentage3 = '0' then '' else concat('', a.tax_percentage3, ' %   ')" +
                   " end, case when a.tax_amount3 = '0' then '  ' else concat(' : ', a.tax_amount3) end) as tax3" +
                   " , format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount, a.taxsegment_gid " +
                   " FROM smr_tmp_tsalesorderdtl a " +
                   " left join smr_trn_tsalesorder s on s.salesorder_gid=a.salesorder_gid " +
                   " left join crm_mst_tcustomer p on p.taxsegment_gid = a.taxsegment_gid " +
                   " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                   " left join pmr_mst_tproductgroup v on b.productgroup_gid=v.productgroup_gid " +
                   " LEFT JOIN acp_mst_tvendor e ON a.vendor_gid = e.vendor_gid " +
                   " WHERE a.employee_gid='" + employee_gid + "' and b.delete_flag='N' order by (a.slno+0) asc";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<postsales_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            int i = 0;

                            mssalesorderGID1 = objcmnfunctions.GetMasterGID("VSDC");
                            if (mssalesorderGID1 == "E")
                            {
                                values.message = "Create Sequence code for VSDC ";
                                return;
                            }
                            string display_field = dt["product_remarks"].ToString();

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
                              " product_remarks," +
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
                              "'" + mssalesorderGID + "'," +
                               "'" + dt["product_gid"].ToString() + "'," +
                               "'" + dt["productgroup_gid"].ToString() + "'," +
                               "'" + dt["productgroup_name"].ToString().Replace("'", "\\\'") + "'," +
                               "'" + dt["customerproduct_code"].ToString() + "'," +
                               "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                               "'" + dt["product_code"].ToString() + "',";
                            if(display_field != null)
                            {
                                msSQL += "'" + display_field.Replace("'","\\\'") + "'," +
                               "'" + display_field.Replace("'", "\\\'") + "',";
                            }
                            else
                            {
                                msSQL += "'" + display_field + "'," +
                              "'" + display_field + "',";
                            }
                              
                            msSQL +=   "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
                               "'" + dt["qty_quoted"].ToString() + "'," +
                               "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                               "'" + dt["uom_gid"].ToString() + "'," +
                               "'" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                               "'" + dt["price"].ToString().Replace(",", "") + "'," +
                               "'" + dt["tax_name"].ToString() + "'," +
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
                               "'" + dt["order_type"].ToString() + "'," +
                               "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                               "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                               "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                               "'" + dt["discount_percentage"].ToString() + "'," +
                               "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                               "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
                               "'" + dt["price"].ToString().Replace(",", "") + "'," +
                               "'" + dt["taxsegment_gid"].ToString().Replace(",", "") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                values.status = false;
                                values.message = "Error occurred while Insertion";
                                return;
                            }

                            msSQL = " insert into acp_trn_torderdtl (" +
                             " salesorderdtl_gid ," +
                             " salesorder_gid," +
                             " product_gid ," +
                             " product_name," +
                             " product_price," +
                             " qty_quoted," +
                             " discount_percentage," +
                             " discount_amount," +
                             " tax_amount ," +
                             " uom_gid," +
                             " uom_name," +
                             " price," +
                             " tax_name," +
                             " tax1_gid," +
                             " slno," +
                             " tax_percentage," +
                             " taxsegment_gid," +
                             " type, " +
                             " salesorder_refno" +
                             ")values(" +
                             " '" + mssalesorderGID1 + "'," +
                             " '" + mssalesorderGID + "'," +
                             " '" + dt["product_gid"].ToString() + "'," +
                             " '" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                             " '" + dt["product_price"].ToString().Replace(",", "") + "'," +
                             " '" + dt["qty_quoted"].ToString() + "'," +
                             " '" + dt["discount_percentage"].ToString() + "'," +
                             " '" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                             " '" + dt["tax_amount"].ToString() + "'," +
                             " '" + dt["uom_gid"].ToString() + "'," +
                             " '" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                             " '" + dt["price"].ToString().Replace(",", "") + "'," +
                             " '" + dt["tax_name"].ToString() + "'," +
                              " '" + dt["tax1_gid"].ToString() + "'," +
                             " '" + values.slno + "'," +
                             " '" + dt["tax_percentage"].ToString() + "'," +
                             " '" + dt["taxsegment_gid"].ToString() + "'," +
                             " '" + dt["order_type"].ToString() + "', " +
                             " '" + values.salesorder_refno + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }

                    msSQL = "select distinct type from smr_trn_tsalesorderdtl where salesorder_gid='" + mssalesorderGID + "' ";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objOdbcDataReader.HasRows == true)
                    {
                        if (objOdbcDataReader["type"].ToString() != "Services")
                        {
                            lsorder_type = "Sales";

                        }

                        else
                        {
                            lsorder_type = "Services";
                        }
                    }
                    objOdbcDataReader.Close();


                    msSQL = " update smr_trn_tsalesorder set so_type='" + lsorder_type + "' where salesorder_gid='" + mssalesorderGID + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update acp_trn_torder set so_type='" + lsorder_type + "' where salesorder_gid='" + mssalesorderGID + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    string lsstage = "8";
                    msgetlead2campaign_gid = objcmnfunctions.GetMasterGID("BLCC");
                    string lsso = "N";

                    msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + USERGid + "'";
                    string employee = objdbconn.GetExecuteScalar(msSQL);


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
                                                      "'" + values.customer_gid + "'," +
                                                      "'" + lsstage + "'," +
                                                      "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                      "'" + campaignGid + "'," +
                                                      "'" + employee + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "select hierarchy_flag from adm_mst_tcompany where hierarchy_flag ='Y'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                       
                        msGetGID = objcmnfunctions.GetMasterGID("PODC");
                        msSQL = " insert into smr_trn_tapproval ( " +
                        " approval_gid, " +
                        " approved_by, " +
                        " approved_date, " +
                        " submodule_gid, " +
                        " soapproval_gid " +
                        " ) values ( " +
                        "'" + msGetGID + "'," +
                        " '" + employee_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        "'SMRSROSOA'," +
                        "'" + mssalesorderGID + "') ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                       
                        msSQL = "select approval_flag from smr_trn_tapproval where submodule_gid='SMRSROSOA' and soapproval_gid='" + mssalesorderGID + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == false)
                        {
                            
                            msSQL = "update smr_trn_tsalesorder set salesorder_status='Approved',salesorder_remarks='" + values.so_remarks.Replace("'", "\\\'") + "', " +
                                   " approved_by='" + employee_gid + "', approved_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where salesorder_gid='" + mssalesorderGID + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = "update acp_trn_torder set salesorder_status='Approved',salesorder_remarks='" + values.so_remarks.Replace("'", "\\\'") + "', " +
                                  " approved_by='" + employee_gid + "', approved_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where salesorder_gid='" + mssalesorderGID + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                           
                        }
                        else
                        {
                            msSQL = "select approved_by from smr_trn_tapproval where submodule_gid='SMRSROSOA' and soapproval_gid='" + mssalesorderGID + "'";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.RecordsAffected == 1)
                            {
                                
                                msSQL = " update smr_trn_tapproval set " +
                               " approval_flag = 'Y', " +
                               " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                               " where approved_by = '" + employee_gid + "'" +
                               " and soapproval_gid = '" + mssalesorderGID + "' and submodule_gid='SMRSROSOA'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                msSQL = "update smr_trn_tsalesorder set salesorder_status='Approved',salesorder_remarks='" + values.so_remarks.Replace("'", "\\\'") + "', " +
                                   " approved_by='" + employee_gid + "', approved_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where salesorder_gid='" + mssalesorderGID + "' ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = "update acp_trn_torder set salesorder_status='Approved',salesorder_remarks='" + values.so_remarks.Replace("'", "\\\'") + "', " +
                                      " approved_by='" + employee_gid + "', approved_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where salesorder_gid='" + mssalesorderGID + "' ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                
                            }
                            else
                            {
                                msSQL = " update smr_trn_tapproval set " +
                                           " approval_flag = 'Y', " +
                                           " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                           " where approved_by = '" + employee_gid + "'" +
                                           " and soapproval_gid = '" + mssalesorderGID + "' and submodule_gid='SMRSROSOA'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
                        objOdbcDataReader.Close();
                        if (mnResult2 != 0 || mnResult2 == 0)
                        {
                            msSQL = " delete from smr_tmp_tsalesorderdtl " +
                                    " where employee_gid='" + employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        if (mnResult2 != 0)
                        {
                            values.status = true;
                            values.message = "Sales Order  Raised Successfully";
                            return;
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Raising Sales Order";
                            return;
                        }
                    }
                }
                else
                {
                    values.status = true;
                    values.message = "Select one Product to Raise Enquiry";
                    return;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Sales Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetProductAdd(string employee_gid, PostProductQuote values)

        {
            try
            {
                string lsproducttype_name = "";
                msGetGid = objcmnfunctions.GetMasterGID("VSDT");


                msSQL = " SELECT a.productuom_gid, a.product_gid, a.product_name,a.producttype_name, b.productuom_name FROM pmr_mst_tproduct a " +
                     " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                     " WHERE product_gid = '" + values.product_name + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                   
                    lsproductgid = objOdbcDataReader["product_gid"].ToString();
                    lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();
                    lsproduct_name = objOdbcDataReader["product_name"].ToString();
                    lsproductuom_name = objOdbcDataReader["productuom_name"].ToString();
                    lsproducttype_name = objOdbcDataReader["producttype_name"].ToString();

                    objOdbcDataReader.Close();


                }

                if (values.productdiscount == null || values.productdiscount == "")
                {
                    lsdiscountpercentage = "0.00";
                }
                else
                {
                    lsdiscountpercentage = values.productdiscount;
                }

                if (values.discount_amount == null || values.discount_amount == "")
                {
                    lsdiscountamount = "0.00";
                }
                else
                {
                    lsdiscountamount = values.discount_amount;
                }

              
                
                if (lsproducttype_name == "Services")
                {
                    lsorder_type = "Sales";

               }
                else
                {
                    lsorder_type = "Services";
                }
                
              
                 if (values.unitprice == null || values.unitprice == "" || values.unitprice == "undefined")
                {
                    values.status = false;
                    values.message = "Price cannot be left empty";
                    return;
                }
                else
                {


                    msSQL = " insert into smr_tmp_tsalesorderdtl( " +
                          " tmpsalesorderdtl_gid," +
                          " salesorder_gid," +
                          " employee_gid," +
                          " product_gid," +
                          " product_code," +
                          " product_name," +
                          " product_price," +
                          " qty_quoted," +
                          " uom_gid," +
                          " uom_name," +
                          " price," +
                          " order_type," +
                          " taxsegment_gid, " +
                         " taxsegmenttax_gid, " +
                         " tax1_gid, " +
                         " tax2_gid, " +
                         " tax3_gid, " +
                         " tax_name, " +
                         " tax_name2, " +
                         " tax_name3, " +
                         " tax_percentage, " +
                         " tax_percentage2, " +
                         " tax_percentage3, " +
                         " tax_amount, " +
                         " tax_amount2, " +
                         " tax_amount3, " +
                          " discount_amount, " +
                          " product_remarks, " +
                          " discount_percentage" +
                          ")values(" +
                          "'" + msGetGid + "'," +
                          "'" + values.quotation_gid + "'," +
                          "'" + employee_gid + "'," +
                          "'" + values.product_name + "'," +
                          "'" + values.product_code + "'," +
                          "'" + lsproduct_name.Replace("'","\\\'") + "'," +
                          "'" + values.unitprice + "'," +
                          "'" + values.productquantity + "'," +
                          "'" + lsproductuom_gid + "'," +
                          "'" + (String.IsNullOrEmpty(lsproductuom_name) ? lsproductuom_name : lsproductuom_name.Replace("'","\\\'")) + "'," +
                          "'" + values.producttotal_amount + "'," +
                          " '" + lsorder_type + "', " +
                          " '" + values.taxsegment_gid + "', " +
                          " '" + values.taxsegment_gid + "', " +
                          " '" + values.taxgid1 + "', " +
                          " '" + values.taxgid2 + "', " +
                          " '" + values.taxgid3 + "', " +
                          " '" + values.taxname1 + "', " +
                          " '" + values.taxname2 + "', " +
                          " '" + values.taxname3 + "', ";
                    if (values.taxprecentage1 == "" || values.taxprecentage1 == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += " '" + values.taxprecentage1 + "', ";
                    }
                    if (values.taxprecentage2 == "" || values.taxprecentage2 == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += " '" + values.taxprecentage2 + "', ";
                    }
                    if (values.taxprecentage3 == "" || values.taxprecentage3 == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += " '" + values.taxprecentage3 + "', ";
                    }
                    if (values.taxamount1 == "" || values.taxamount1 == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += " '" + values.taxamount1 + "', ";
                    }
                    if (values.taxamount2 == "" || values.taxamount2 == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += " '" + values.taxamount2 + "', ";
                    }
                    if (values.taxamount3 == "" || values.taxamount3 == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += " '" + values.taxamount3 + "', ";
                    }

                    msSQL +=
                       "'" + values.discount_amount + "',";
                    if(values.product_remarks != null)
                    {
                        msSQL += "'" + values.product_remarks.Replace("'","\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.product_remarks + "',";
                    }
                      
                      msSQL += "'" + lsdiscountpercentage + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

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

            
                
        }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }        
        public void DaGetTemporarySummary(string employee_gid,string quotation_gid, MdlSmrQuoteToOrder values)
        {
            try
            {
                double grand_total = 0.00;
                double grandtotal = 0.00;
                
                msSQL = "select q.tax_prefix as tax_prefix1,u.tax_prefix as tax_prefix2,w.tax_prefix as tax_prefix3,a.tmpsalesorderdtl_gid,a.taxsegment_gid,a.salesorder_gid, a.tax_name,format(a.tax_amount,2) as tax_amount,a.tax_percentage,a.tax_name2,format(a.tax_amount2,2) as tax_amount2,a.tax_percentage2,a.tax_name3,format(a.tax_amount3,2) as tax_amount3,a.tax_percentage3, p.customer_gid,format(a.tax_amount,2) as tax_amount, a.salesorderdtl_gid, a.salesorder_gid, a.product_gid, a.productgroup_gid, " +
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
                   " left join smr_trn_tsalesorder s on s.salesorder_gid=a.salesorder_gid " +
                   " left join crm_mst_tcustomer p on p.taxsegment_gid = a.taxsegment_gid " +
                   " left join acp_mst_ttax q on q.tax_gid = a.tax1_gid " +
                   " left join acp_mst_ttax u on u.tax_gid = a.tax2_gid " +
                   " left join acp_mst_ttax w on w.tax_gid = a.tax3_gid " +
                   " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                   " left join pmr_mst_tproductgroup v on b.productgroup_gid=v.productgroup_gid " +
                   " left join acp_mst_tvendor e on a.vendor_gid = e.vendor_gid " +
                   " where a.salesorder_gid='" + quotation_gid + "' and a.employee_gid='" + employee_gid  + "' and b.delete_flag='N' order by (a.slno+0) asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Gettemporarysummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["price"].ToString());
                        grandtotal += double.Parse(dt["price"].ToString());                                                
                        getModuleList.Add(new Gettemporarysummary
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
                            tax_prefix1 = dt["tax_prefix1"].ToString(),
                            tax_prefix2 = dt["tax_prefix2"].ToString(),
                            tax_prefix3 = dt["tax_prefix3"].ToString(),
                        });
                    }
                    values.temp_list = getModuleList;
                }

                dt_datatable.Dispose();
                values.grand_total = Math.Round(grand_total,2);
                values.grandtotal = Math.Round(grandtotal,2);                
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
        public void DaGetQuotetoOrderProductEditSummary(string tmpsalesorderdtl_gid, MdlSmrQuoteToOrder values)
        {
            try
            {

                msSQL = " select a.tmpsalesorderdtl_gid,a.salesorder_gid, a.selling_price, a.employee_gid, a.product_gid, b.product_code," +
                " a.slno, a.productgroup_gid,a.productgroup_name, a.product_name,a.product_price,a.qty_quoted,a.discount_percentage," +
                    " a.discount_amount,a.uom_gid,a.uom_name,a.price,a.tax_name,a.tax1_gid, a.tax_amount, " +
                    " format(a.tax_percentage,2)as tax_percentage " +
                    " from smr_tmp_tsalesorderdtl a " +
                    " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                    " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "'  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<directeditQuotationList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new directeditQuotationList
                        {
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            selling_price = dt["product_price"].ToString(),
                            discountpercentage = dt["discount_percentage"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            totalamount = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_gid = dt["tax1_gid"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),

                        });
                        values.directeditquotation_list = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while QuotetoOrderProductEditSummary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }      
        public void DaPostUpdateQuotationtoOrderProductSummary(string employee_gid, directeditQuotationList values)
        {
            try
            {

                if (values.product_gid != null)
                {

                    msSQL = "Select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
                    lsproductgid1 = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    msSQL = " Select product_name from pmr_mst_tproduct where product_gid = '" + values.product_gid + "'";
                    lsproductgid1 = objdbconn.GetExecuteScalar(msSQL);
                }
                if (values.tax_gid == null)
                {
                    msSQL = "Select tax_gid from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                    string lstaxgid = objdbconn.GetExecuteScalar(msSQL);
                
                msSQL = "Select percentage from acp_mst_ttax where tax_gid='" + lstaxgid + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    msSQL = "Select tax_gid from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                    string lstaxgid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "Select percentage from acp_mst_ttax where tax_gid='" + lstaxgid + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }
                msSQL = " select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                      " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                      " WHERE product_gid='" + lsproductgid1 + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows == true)
                {
                    lsenquiry_type = "Sales";

                    objOdbcDataReader.Close();
                }

                else
                {
                    lsenquiry_type = "Service";
                }
                
                msSQL = " select * from smr_tmp_tsalesorderdtl where product_gid='" + lsproductgid1 + "' and uom_gid='" + lsproductuomgid + "' " +
              "  and employee_gid='" + employee_gid + "' " +
                " and tmpsalesorderdtl_gid = '" + values.tmpsalesorderdtl_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                   
                    msSQL = " update smr_tmp_tsalesorderdtl set qty_quoted='" + Convert.ToDouble(values.quantity) + "'," +
                           " discount_percentage='" + values.discountpercentage + "'," +
                           " discount_amount='" + values.discountamount + "'," +
                            " tax_amount='" + values.tax_amount + "'," +
                             " tax_name= '" + values.tax_name + "'," +
                          " price='" + Convert.ToDouble(values.price) + "' " +
                          " where tmpsalesorderdtl_gid='" + objOdbcDataReader["tmpsalesorderdtl_gid"] + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                   
                }
                else
                {
                    msSQL = " update smr_tmp_tsalesorderdtl set " +
                  " product_gid = '" + lsproductgid1 + "'," +
                  " product_name= '" + values.product_name + "'," +
                  " product_price='" + values.product_price + "'," +
                  " qty_quoted='" + values.quantity + "'," +
                  " discount_percentage='" + values.discountpercentage + "'," +
                  " discount_amount='" + values.discountamount + "'," +
                  " uom_gid = '" + lsproductuomgid + "', " +
                  " uom_name='" + values.productuom_name + "'," +
                  " price='" + values.price + "'," +
                  " employee_gid='" + employee_gid + "'," +
                  " tax_name= '" + values.tax_name + "'," +
                  " tax_amount='" + values.tax_amount + "',";
                    if (lspercentage1 == "" || lspercentage1 == null || lspercentage1 == "undefined") {

                        msSQL += " tax_percentage='0.00',";
                    }
                    else
                    {
                       msSQL+= " tax_percentage='" + lspercentage1 + "',";

                    }
                   msSQL+= " order_type='" + lsenquiry_type + "'" +
                 " where tmpsalesorderdtl_gid='" + values.tmpsalesorderdtl_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                objOdbcDataReader.Close();

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Product Updated Successfully";
                }

                else
                {
                    values.status = false;
                    values.message = "Error While Updating Product";
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Update QuotationtoOrderProduct!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetQuotationtoSOProductSummary(string employee_gid, string quotation_gid, MdlSmrQuoteToOrder values)
        {
            msSQL = " select a.quotationdtl_gid,a.quotation_gid,a.customerproduct_code,a.product_gid,b.product_code,a.productgroup_gid,a.productgroup_name," +
                      "a.product_name,format(a.product_price,2)as product_price , a.created_by," +
                      " a.qty_quoted,format(a.discount_percentage, 2) as margin_percentage ,format(a.discount_amount, 2) as margin_amount ,b.product_name, " +
                      " format(a.tax_amount, 2) as tax_amount , a.product_remarks,  a.uom_gid, a.uom_name,a.tax_name,a.tax_name2,a.tax_name3," +
                      " a.payment_days, a.delivery_period,a.price,a.product_price,a.display_field,a.product_status,a.quotation_refno, " +
                      " format(a.tax_amount2, 2) as tax_amount2,format(a.tax_amount3, 2) as tax_amount3,format(a.tax_percentage, 2) as tax_percentage,format(a.tax_percentage2, 2) as tax_percentage2, " +
                      " format(a.tax_percentage3, 2) as tax_percentage3,a.vendor_gid,a.slno,a.tax1_gid,a.tax2_gid,a.tax3_gid, " +
                      " a.quotation_refno,format(a.selling_price, 2) as selling_price, " +
                      " date_format(a.product_requireddate, '%Y-%m-%d') as product_requireddate ,a.productrequireddate_remarks," +
                      " concat(if (a.productrequireddate_remarks = '',' ',concat(productrequireddate_remarks, ' ', '/', ' ')),'',cast(date_format(a.product_requireddate, '%d-%m-%Y') as char)) as requireddateremarks " +
                      " from smr_trn_treceivequotationdtl a " +
                      " left join pmr_mst_tproduct b on a.product_gid = b.product_gid" +
                      " where a.quotation_gid ='" + quotation_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    SalesOrderGID = objcmnfunctions.GetMasterGID("VSOP");
                    TempSOGID = objcmnfunctions.GetMasterGID("VSDT");

                    msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                         " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                          " WHERE product_gid='" + dt["product_gid"] + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                   
                    if (objOdbcDataReader.HasRows == true)
                    {
                        lsproducttype = "Sales";
                    }
                    else
                    {
                        lsproducttype = "Services";
                    }
                    objOdbcDataReader.Close();

                    string display_field = dt["product_remarks"].ToString();

                    msSQL = " insert into smr_tmp_tsalesorderdtl(" +
                            " tmpsalesorderdtl_gid, " +
                            " salesorder_gid, " +
                            " quotation_gid, " +
                            " quotationdtl_gid, " +
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
                            " tax_name2," +
                            " tax_name3," +
                            " tax_amount," +
                            " tax_amount2," +
                            " tax_amount3," +
                            " tax_percentage," +
                            " tax_percentage2," +
                            " tax_percentage3," +
                            " tax1_gid," +
                            " tax2_gid," +
                            " tax3_gid," +
                            " margin_percentage," +
                            " margin_amount," +
                            " product_price," +
                            " selling_price," +
                            " employee_gid," +
                            " salesorder_refno," +
                            " order_type," +
                            " product_remarks," +
                            " price" +
                            ") values (" +
                            "'" + TempSOGID + "'," +
                            "'" + quotation_gid + "'," +
                            "'" + quotation_gid + "'," +
                             "'" + dt["quotationdtl_gid"] + "', " +
                            "'" + dt["product_gid"] + "', " +
                            "'" + dt["product_code"] + "', " +
                            "'" + dt["product_name"].ToString().Replace("'","\\\'") + "', " +
                            "'" + dt["productgroup_name"].ToString().Replace("'", "\\\'") + "', " +
                            "'" + dt["productgroup_gid"] + "', " +
                            "'" + dt["uom_gid"] + "', " +
                            "'" + dt["uom_name"].ToString().Replace("'", "\\\'") + "', " +
                            "'" + dt["qty_quoted"] + "', " +
                            "'" + dt["margin_percentage"] + "', " +
                            "'" + dt["margin_amount"].ToString().Replace(",", "") + "', " +
                            "'" + dt["tax_name"] + "', " +
                            "'" + dt["tax_name2"] + "', " +
                            "'" + dt["tax_name3"] + "', " +
                            "'" + dt["tax_amount"].ToString().Replace(",", "") + "', " +
                            "'" + dt["tax_amount2"].ToString().Replace(",", "") + "', " +
                            "'" + dt["tax_amount3"].ToString().Replace(",", "") + "', " +
                            "'" + dt["tax_percentage"] + "', " +
                            "'" + dt["tax_percentage2"] + "', " +
                            "'" + dt["tax_percentage3"] + "', " +
                            "'" + dt["tax1_gid"] + "', " +
                            "'" + dt["tax2_gid"] + "', " +
                            "'" + dt["tax3_gid"] + "', " +
                            "'" + dt["margin_percentage"] + "', " +
                            "'" + dt["margin_amount"].ToString().Replace(",", "") + "', " +
                            "'" + dt["product_price"].ToString().Replace(",", "") + "', " +
                            "'" + dt["selling_price"].ToString().Replace(",", "") + "', " +
                            "'" + employee_gid + "', " +
                            "'" + dt["quotation_refno"] + "', " +
                            "'" + lsproducttype + "', ";
                    if(display_field != null)
                    {
                       msSQL += "'" + display_field.Replace("'", "\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "'" + display_field + "', ";
                    }
                          msSQL +=
                            "'" + dt["price"].ToString().Replace(",", "") + "') ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
            }
        }
    }
}