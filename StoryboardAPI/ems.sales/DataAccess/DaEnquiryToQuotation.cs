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
    public class DaEnquiryToQuotation
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        private OdbcDataReader objOdbcDataReader;
        double taxAmount, reCalTotalAmount, subtotal, reCaldiscountAmount, reCalTaxAmount;
        decimal lsmrp_price;
        DataTable dt_datatable;
        string lstaxname, lstaxamount, lsproduct_price, quotation_type,  lspercentage1, lsproductuomgid, lsQuotationMode, lsproduct_type, QuoatationGID,TempQuoatationGID,  lscustomer_gid, lsleadbank_gid,  msGetGid,lsorder_type,lsquotation_type;
        int mnResult;


        public void DaGetEnquiryQuotationSummary(string enquiry_gid, string employee_gid, MdlEnquiryToQuotation values)
        {
            try
            {
                msSQL = " delete from smr_tmp_tsalesorderdtl where enquiry_gid='" + enquiry_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                msSQL = " select a.branch_gid, a.enquiry_gid,a.customerbranch_gid,m.campaign_gid,concat(m.campaign_title, ' | ', l.user_code, ' | ', l.user_firstname, ' ', l.user_lastname)AS employee_name,l.user_gid,a.enquiry_referencenumber,DATE_FORMAT(a.enquiry_date,'%d-%m-%Y') as enquiry_date,a.enquiry_remarks,a.contact_number," +
                                " d.branch_name,a.contact_email,a.contact_person, contact_address as customer_address,a.leadbank_gid,a.customer_name,a.customer_gid, i.product_name," +
                                " i.qty_quoted, format(i.price, 2) as price,i.product_code, i.uom_name, l.user_firstname," +
                                "  c.customercontact_gid,concat(c.address1,' ',c.state) as address1,ifnull(c.address2,'') as address2," +
                                " ifnull(c.city,'') as city,h.pricesegment_name,h.pricesegment_gid ,e.taxsegment_name,e.taxsegment_gid,c.country_gid, " +
                                " b.credit_days,b.customer_pin,b.customer_region,b.gst_number" +
                                " from smr_trn_tsalesenquiry a " +
                                " left join crm_mst_tcustomer b on b.customer_gid = a.customer_gid  " +
                                " left join acp_mst_ttaxsegment e on e.taxsegment_gid = b.taxsegment_gid  " +
                                " left join crm_mst_tregion f on b.customer_region = f.region_gid " +
                                " left join hrm_mst_temployee j on a.enquiry_receivedby = j.employee_gid " +
                                " left join crm_mst_tcustomercontact c on c.customer_gid = b.customer_gid   " +
                                " left join smr_trn_tpricesegment h on h.pricesegment_gid=b.pricesegment_gid  " +
                                " left join crm_trn_tenquiry2campaign k on a.customer_gid = k.customer_gid " +
                                " left join smr_trn_tcampaign m on k.campaign_gid = m.campaign_gid " +
                                " left join adm_mst_tuser l on j.user_gid = l.user_gid " +
                                " left join hrm_mst_tbranch d on a.branch_gid = d.branch_gid " +
                                " left join smr_tmp_treceivequotationdtl i on a.enquiry_gid = i.enquiry_gid" +
                                " where a.enquiry_gid = '" + enquiry_gid + "' group by a.enquiry_gid, i.product_gid";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<GetEnqtoQuotation>();

                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                msSQL = " Select customer_gid from crm_mst_tcustomer where customer_name='" + dt["customer_name"].ToString() + "'";
                                string customergid = objdbconn.GetExecuteScalar(msSQL);

                                getModuleList.Add(new GetEnqtoQuotation
                                {
                                    enquiry_gid = dt["enquiry_gid"].ToString(),
                                    customer_gid = customergid,
                                    quotation_date = dt["enquiry_date"].ToString(),
                                    branch_name = dt["branch_name"].ToString(),
                                    branch_gid = dt["branch_gid"].ToString(),
                                    customer_name = dt["customer_name"].ToString(),
                                    customer_contact = dt["contact_person"].ToString(),
                                    customer_mobile = dt["contact_number"].ToString(),
                                    customer_email = dt["contact_email"].ToString(),
                                    so_remarks = dt["enquiry_remarks"].ToString(),
                                    user_name = dt["employee_name"].ToString(),
                                    customer_address = dt["customer_address"].ToString(),
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["uom_name"].ToString(),
                                    quantity = dt["qty_quoted"].ToString(),
                                    enquiry_referencenumber = dt["enquiry_referencenumber"].ToString(),
                                    totalamount = dt["price"].ToString(),
                                    producttotalamount = dt["price"].ToString(),
                                    grandtotal = dt["price"].ToString(),
                                    city = dt["city"].ToString(),
                                    pricesegment_name = dt["pricesegment_name"].ToString(),
                                    pricesegment_gid = dt["pricesegment_gid"].ToString(),
                                    taxsegment_name = dt["taxsegment_name"].ToString(),
                                    taxsegment_gid = dt["taxsegment_gid"].ToString(),
                                    country_gid = dt["country_gid"].ToString(),
                                    credit_days = dt["credit_days"].ToString(),
                                    customer_region = dt["customer_region"].ToString(),
                                    customer_pin = dt["customer_pin"].ToString(),
                                    gst_number = dt["gst_number"].ToString(),
                                });

                                values.EnquiryQuotationlist = getModuleList;
                            }

                            dt_datatable.Dispose();
                        }



                        msSQL = " select a.enquiry_gid, a.enquirydtl_gid, a.product_gid, a.productgroup_gid,b.product_name, a.uom_gid, b.product_code," +
                                        " a.qty_enquired,c.productgroup_name, d.productuom_name,a.potential_value,a.enquiryproduct_type, a.created_by from smr_trn_tsalesenquirydtl a" +
                                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid" +
                                        " left join pmr_mst_tproductgroup c on a.productgroup_gid = a.productgroup_gid" +
                                        " left join pmr_mst_tproductuom d on a.uom_gid = d.productuom_gid" +
                                        " where a.enquiry_gid='" + enquiry_gid + "'  AND enquirydtl_gid  NOT IN(SELECT enquirydtl_gid FROM smr_tmp_treceivequotationdtl WHERE enquiry_gid ='" + enquiry_gid + "') group by a.enquirydtl_gid";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                QuoatationGID = objcmnfunctions.GetMasterGID("VQDC");
                                TempQuoatationGID = objcmnfunctions.GetMasterGID("VQDT");

                                msSQL = " insert into smr_tmp_tsalesorderdtl (" +
                                        " tmpsalesorderdtl_gid, " +
                                        " salesorder_gid," +
                                        " enquiry_gid," +
                                        " enquirydtl_gid," +
                                        " product_gid," +
                                        " productgroup_gid," +
                                        " productgroup_name," +                                       
                                        " product_name," +
                                        " product_code," +
                                        " uom_name," +
                                        " uom_gid," +
                                        //" quotation_type," +
                                        " created_by, " +
                                        " qty_quoted," +
                                        " employee_gid," +
                                        " price" +
                                        " )values(" +
                                        "'" + TempQuoatationGID + "'," +
                                        "'" + QuoatationGID + "'," +
                                        "'" + enquiry_gid + "'," +
                                        "'" + dt["enquirydtl_gid"] + "', " +
                                        "'" + dt["product_gid"] + "', " +
                                        "'" + dt["productgroup_gid"] + "', " +
                                        "'" + dt["productgroup_name"].ToString().Replace("'","\\\'") + "', " +                                       
                                        "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "', " +
                                        "'" + dt["product_code"] + "', " +
                                        "'" + dt["productuom_name"].ToString().Replace("'", "\\\'") + "', " +
                                        "'" + dt["uom_gid"] + "', " +
                                        //"'" + dt["enquiryproduct_type"] + "', " +
                                        "'" + dt["created_by"] + "', " +
                                        "'" + dt["qty_enquired"] + "'," +
                                        "'" + employee_gid + "'," +
                                        "'" + dt["potential_value"].ToString().Replace(", ", "") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                        }
       

    }

            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        // 

        public void DaGetProductETQ(MdlEnquiryToQuotation values)
        {
            try
            {
                msSQL = "select product_gid,product_name from pmr_mst_tproduct" +
                    " where status='1'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductETQDropDown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductETQDropDown
                        {
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                        });
                        values.GetProductETQ = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // PRODUCT ON CHANGE EVENT FOR CUSTOMER ENQUIRY FROM CRM 360

        public void DaGetOnChangeProductETQ(string customercontact_gid, string product_gid, MdlEnquiryToQuotation values)
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

                        var getModuleList = new List<GetOnChangeProductDropdownETQ>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetOnChangeProductDropdownETQ
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    unitprice = lsproduct_price,

                                });
                                values.GetOnChangeProductETQ = getModuleList;
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

                            var getGetTaxSegmentList = new List<TaxSegmentLists>();
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    getGetTaxSegmentList.Add(new TaxSegmentLists
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
                                    values.TaxSegmentLists = getGetTaxSegmentList;
                                }
                            }

                            dt_datatable.Dispose();
                        }
                    }
                    else
                    {
                        msSQL = " Select a.product_name, a.product_code,a.cost_price," +
                            " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                             " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                            " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                        " where a.product_gid='" + product_gid + "' ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        var getModuleList = new List<GetOnChangeProductDropdownETQ>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetOnChangeProductDropdownETQ
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    unitprice = dt["cost_price"].ToString(),

                                });
                                values.GetOnChangeProductETQ = getModuleList;
                            }
                        }
                    }
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

        // PRODUCT SUMMARY TAX DROP DOWN FOR CUSTOMER ENQUIRY FROM CRM 360

        public void DaGetProdTaxETQ(MdlEnquiryToQuotation values)
        {
            try
            {
                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProdTaxDropdownETQ>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProdTaxDropdownETQ

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetProdTaxETQ = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        //

        // Tax 4
        public void DaGetOverallTaxETQ(MdlEnquiryToQuotation values)
        {
            try
            {
                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOverallTaxDropDownETQ>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOverallTaxDropDownETQ

                        {
                            tax_gid4 = dt["tax_gid"].ToString(),
                            tax_name4 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetOverallTaxETQ = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        //Terms And Condition Dropdown

        public void DaGetTermSForETQ(MdlEnquiryToQuotation values)

        {
            try
            {
                msSQL = " select a.template_gid, c.template_name, c.template_content from adm_trn_ttemplate2module a " +
                        " left join adm_mst_tmodule b on a.module_gid = b.module_gid " +
                        " left join adm_mst_ttemplate c on a.template_gid = c.template_gid " +
                        " left join adm_mst_ttemplatetype d on c.templatetype_gid = d.templatetype_gid " +
                        " where a.module_gid = 'SMR' ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<GetTermDropDownETQ>();
                       if (dt_datatable.Rows.Count != 0)
                           {
                            foreach (DataRow dt in dt_datatable.Rows)
                               {
                                getModuleList.Add(new GetTermDropDownETQ
                                {
                                   template_gid = dt["template_gid"].ToString(),
                                   template_name = dt["template_name"].ToString(),
                                   template_content = dt["template_content"].ToString()
                                });
                                values.GetTermETQ = getModuleList;
                            }
                       }
                       dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Template Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // 
        public void DaGetOnChangeTermsAndConditionsETQ(string template_gid, MdlEnquiryToQuotation values)
        {
            try
            {

                if (template_gid != null)
                {
                    msSQL = " select a.template_gid, c.template_name, c.template_content from adm_trn_ttemplate2module a " +
                   " left join adm_mst_tmodule b on a.module_gid = b.module_gid " +
                   " left join adm_mst_ttemplate c on a.template_gid = c.template_gid " +
                   " left join adm_mst_ttemplatetype d on c.templatetype_gid = d.templatetype_gid " +
                   " where a.module_gid = 'SMR' and c.template_gid = '" + template_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetTermsandConditionETQ>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetTermsandConditionETQ
                            {
                                template_gid = dt["template_gid"].ToString(),
                                template_name = dt["template_name"].ToString(),
                                template_content = dt["template_content"].ToString(),
                            });
                            values.GetTermsandCondition_ETQ = getModuleList;
                        }
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Template Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // ENQUIRY TO QUOTATION PRODUCT SUMMARY

        public void DaGetEnquirytoQuotationTempSummary(string enquiry_gid, MdlEnquiryToQuotation values)
        {
            try
            {

                 double grand_total = 0.00;
                double grandtotal = 0.00;

                msSQL = "SELECT a.tmpsalesorderdtl_gid,a.taxsegment_gid,a.salesorder_gid, a.tax_name,a.tax_amount,a.tax_percentage,a.tax_name2,a.tax_amount2,a.tax_percentage2,a.tax_name3,a.tax_amount3,a.tax_percentage3,a.tax_amount, a.salesorderdtl_gid, a.salesorder_gid, a.product_gid, a.productgroup_gid, " +
                   " v.productgroup_name, a.product_name, FORMAT(a.product_price, 2) AS product_price, b.product_code, a.qty_quoted, a.product_remarks, " +
                   " a.uom_gid, a.vendor_gid, a.slno, a.uom_name, FORMAT(a.price, 2) AS price, " +
                   " FORMAT(a.discount_percentage,2) AS discount_percentage, FORMAT(a.discount_amount,2) AS discount_amount, " +
                   " FORMAT(a.selling_price, '0.00') AS selling_price,a.product_remarks, " +
                   " CONCAT( CASE WHEN a.tax_name IS NULL THEN '' ELSE a.tax_name END, ' '," +
                   "CASE WHEN a.tax_percentage = '0' THEN '' ELSE concat('',a.tax_percentage,'%') END , " +
                   " CASE WHEN a.tax_amount = '0' THEN '' ELSE concat(':',a.tax_amount) END) AS tax, " +
                   " CONCAT(CASE WHEN a.tax_name2 IS NULL THEN '' ELSE a.tax_name2 END, ' ', " +
                   " CASE WHEN a.tax_percentage2 = '0' THEN '' ELSE concat('', a.tax_percentage2, '%') END, " +
                   " CASE WHEN a.tax_amount2 = '0' THEN '' ELSE concat(':', a.tax_amount2) END) AS tax2, " +
                   " CONCAT(  CASE WHEN a.tax_name3 IS NULL THEN '' ELSE a.tax_name3 END, ' ', " +
                   " CASE WHEN a.tax_percentage3 = '0' THEN '' ELSE concat('', a.tax_percentage3, ' %   ')" +
                   " END, CASE WHEN a.tax_amount3 = '0' THEN '  ' ELSE concat(' : ', a.tax_amount3) END) AS tax3" +
                   " , format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.tax_rate " +
                   " FROM smr_tmp_tsalesorderdtl a " +
                   " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                   " left join pmr_mst_tproductgroup v on b.productgroup_gid=v.productgroup_gid " +
                   " LEFT JOIN acp_mst_tvendor e ON a.vendor_gid = e.vendor_gid " +
                   " WHERE a.enquiry_gid='" + enquiry_gid + "' and b.delete_flag='N' order by (a.slno+0) asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Gettemporarysummary1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["price"].ToString());
                        grandtotal += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new Gettemporarysummary1
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
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            grand_total = dt["price"].ToString(),
                            grandtotal = dt["price"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            tax = dt["tax"].ToString(),
                            tax2 = dt["tax2"].ToString(),
                            tax3 = dt["tax3"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            taxamount = dt["taxamount"].ToString(),
                            tax_rate = dt["tax_rate"].ToString(),


                        });
                    }
                    values.temp_list1 = getModuleList;
                }

                dt_datatable.Dispose();
                values.grand_total = grand_total;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Temproary Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // PRODUCT ADD FOR ENQUIRY TO QUOTATION

        public void DaGetEnquirytoQuotationProductAdd(string employee_gid, EnquirytoQuotationsummary_list values)
        {
            try
            {

                msGetGid = objcmnfunctions.GetMasterGID("VQDT");
                QuoatationGID = objcmnfunctions.GetMasterGID("VQNP");

                msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "Select productgroup_gid from pmr_mst_tproductgroup where productgroup_name='" + values.productgroup_name + "'";
                string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);


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

                   
                    if (values.currency_code != "INR")
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


                msSQL = " insert into smr_tmp_treceivequotationdtl (" +
                                    " tmpquotationdtl_gid, " +
                                    " quotation_gid," +
                                    " product_price," +
                                    " product_gid," +
                                    " productgroup_gid," +
                                    " productgroup_name," +
                                    " product_name," +
                                    " product_code," +
                                    " uom_name," +
                                    " uom_gid," +
                                    " created_by, " +
                                    " quotation_type, " +
                                    " qty_quoted," +
                                    " discount_percentage," +
                                    " discount_amount," +
                                    " tax_percentage," +
                                    " taxsegment_gid, " +
                                    " taxsegmenttax_gid, " +
                                    " tax_amount," +
                                    " tax_name," +
                                    " tax1_gid," +
                                    " price," +
                                    "enquiry_gid" +
                                    " )values(" +
                                    "'" + msGetGid + "'," +
                                    "'" + QuoatationGID + "'," +
                                    "'" + values.unitprice + "'," +
                                    "'" + lsproductgid + "', " +
                                    "'" + lsproductgroupgid + "', " +
                                    "'" + values.productgroup_name + "', " +
                                    "'" + values.product_name + "', " +
                                    "'" + values.product_code + "', " +
                                    "'" + values.productuom_name + "', " +
                                    "'" + lsproductuomgid + "', " +
                                    "'" + employee_gid + "', " +
                                    "'" + values.quotation_type + "', " +
                                    "'" + values.quantity + "'," +
                                    "'" + values.discountpercentage + "'," +
                                    "'" + values.discountamount + "'," +
                                    "'" + lspercentage1 + "'," +
                                    "'" + values.taxsegment_gid + "'," +
                                    "'" + values.taxsegmenttax_gid + "'," +
                                    "'" + reCalTaxAmount + "'," +
                                    "'" + lstaxname + "'," +
                                    "'" + values.tax_name + "'," +
                                    "'" + reCalTotalAmount + "'," +
                                    "'" + values.enquiry_gid + "')";
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
            catch (Exception ex)
            {
                values.message = "Exception occured while  Adding Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // POST ENQUIRY TO QUOTATION

        public void DaPostEnquirytoQuotation(string employee_gid, string user_gid, PostEnquiryToQUotation_list values)
        {
            try
            {

                msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + user_gid + "'";
                string employee = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select * from smr_tmp_tsalesorderdtl " +
                     " where enquiry_gid='" + values.enquiry_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {

                   string lsrefno = objcmnfunctions.getSequencecustomizerGID("QU", "RBL", values.branch_name);

                    if (msGetGid == "E")
                    {
                        values.status = false;
                        values.message = "Create Sequence Code VQDC for Raise Enquiry";
                        return;
                    }

                    msSQL = " Select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + values.currency_code + "'";
                    string lscurrencycode = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = " select leadbank_gid from crm_trn_tleadbank where customer_gid='" + values.customer_gid + "'";
                    string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);

                    string lsquotation_status = "Approved";
                    string lsgst_percentage = "0.00";
                    string uiDateStr = values.quotation_date;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string quotation_date = uiDate.ToString("yyyy-MM-dd");
                    msGetGid = objcmnfunctions.GetMasterGID("VQDC");

                    msSQL = " insert  into smr_trn_treceivequotation (" +
                             " quotation_gid ," +
                             " branch_gid ," +
                             " quotation_date," +
                             " customer_gid," +
                             " customer_name," +
                             " created_by," +
                             " quotation_remarks," +
                             " quotation_referenceno1, " +
                             " payment_days," +
                             " delivery_days, " +
                             " Grandtotal, " +
                             " termsandconditions, " +
                             " quotation_status," +
                             " contact_no, " +
                             " address1," +
                             " address2," +
                             " contact_mail, " +
                             " addon_charge, " +
                             " additional_discount, " +
                             " addon_charge_l, " +
                             " additional_discount_l, " +
                             " grandtotal_l, " +
                             " currency_code, " +
                             " exchange_rate, " +
                             " currency_gid, " +
                             " total_amount," +
                             " gst_percentage," +
                             " salesperson_gid," +
                             " roundoff, " +
                             " total_price, " +
                             " freight_charges," +
                             " buyback_charges," +
                             " packing_charges," +
                             " insurance_charges," +
                             " tax_amount4," +
                             " tax_name4," +
                             " created_date, " +
                             " pincode " +
                             ") values ( " +
                             " '" + msGetGid + "'," +
                             " '" + values.branch_name + "'," +
                             " '" + quotation_date + "'," +
                             " '" + values.customer_gid + "'," +
                             " '" + values.customer_name.Replace("'", "\\\'") + "'," +
                             " '" + employee_gid + "'," +
                             " '" + values.quotation_remarks.Replace("'", "\\\'") + "'," +
                             " '" + lsrefno + "'," +
                             " '" + values.payment_days.Replace("'", "\\\'") + "'," +
                             " '" + values.delivery_days.Replace("'", "\\\'") + "'," +
                             "'" + values.grandtotal.Replace(",", "").Trim() + "', " +
                             " '" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "\\\'") + "'," +
                             " '" + lsquotation_status + "'," +
                             " '" + values.mobile + "'," +
                             " '" + values.address1.Replace("'", "\\\'") + "'," +
                             " '" + values.address2.Replace("'", "\\\'") + "'," +
                             " '" + values.email + "'," +
                             "' " + values.addoncharge + "'," +
                             "'" + values.additional_discount + "',";
                    if (values.addoncharge == "" || values.addoncharge == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.addoncharge.Replace(",", "").Trim() + "',";
                    }
                    if (values.additional_discount == "" || values.additional_discount == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.additional_discount.Replace(",", "").Trim() + "',";
                    }
                    msSQL += "'" + values.grandtotal.Replace(",", "").Trim() + "', " +
                             "'" + lscurrencycode + "'," +
                             "'" + values.exchange_rate + "'," +
                             "'" + values.currency_code + "',";
                    if (values.total_amount == "" || values.total_amount == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.total_amount.Replace(",", "").Trim() + "',";
                    }
                    msSQL += " '" + lsgst_percentage + "'," +
                             " '" + values.user_name + "',";
                    if (values.roundoff == "" || values.roundoff == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.roundoff + "',";
                    }
                    msSQL += "'" + values.producttotalamount + "',";
                    if (values.freightcharges == "" || values.freightcharges == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.freightcharges + "',";
                    }
                    if (values.buybackcharges == "" || values.buybackcharges == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.buybackcharges + "',";
                    }
                    if (values.packing_charges == "" || values.packing_charges == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.packing_charges + "',";
                    }
                    if (lstaxname == "" || lstaxname == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + lstaxname + "',";
                    }
                    if (values.tax_amount4 == "" || values.tax_amount4 == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_amount4 + "',";
                    }
                    if (values.tax_name4 == "" || values.tax_name4 == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_name4 + "',";
                    }
                    msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                              "'" + values.zip_code + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error Occured while inserting Quotation";
                        return;
                    }

                    else
                    {
                        msSQL = "SELECT a.tmpsalesorderdtl_gid,a.taxsegment_gid,a.salesorder_gid, a.tax_name,a.tax_amount," +
                            "a.tax_percentage,a.tax_name2,a.tax_amount2,a.tax_percentage2,a.tax_name3,a.tax_amount3," +
                            "a.tax_percentage3,a.tax_amount, a.salesorderdtl_gid, a.salesorder_gid, a.product_gid, a.productgroup_gid," +
                            "a.tax1_gid, a.tax2_gid,a.tax3_gid, " +
                      " v.productgroup_name, a.product_name, FORMAT(a.product_price, 2) AS product_price, b.product_code, a.qty_quoted, a.product_remarks, " +
                      " a.uom_gid, a.vendor_gid, a.slno, a.uom_name, FORMAT(a.price, 2) AS price, " +
                      " FORMAT(a.discount_percentage,2) AS discount_percentage, FORMAT(a.discount_amount,2) AS discount_amount, " +
                      " FORMAT(a.selling_price, '0.00') AS selling_price,a.product_remarks, " +
                      " CONCAT( CASE WHEN a.tax_name IS NULL THEN '' ELSE a.tax_name END, ' '," +
                      "CASE WHEN a.tax_percentage = '0' THEN '' ELSE concat('',a.tax_percentage,'%') END , " +
                      " CASE WHEN a.tax_amount = '0' THEN '' ELSE concat(':',a.tax_amount) END) AS tax, " +
                      " CONCAT(CASE WHEN a.tax_name2 IS NULL THEN '' ELSE a.tax_name2 END, ' ', " +
                      " CASE WHEN a.tax_percentage2 = '0' THEN '' ELSE concat('', a.tax_percentage2, '%') END, " +
                      " CASE WHEN a.tax_amount2 = '0' THEN '' ELSE concat(':', a.tax_amount2) END) AS tax2, " +
                      " CONCAT(  CASE WHEN a.tax_name3 IS NULL THEN '' ELSE a.tax_name3 END, ' ', " +
                      " CASE WHEN a.tax_percentage3 = '0' THEN '' ELSE concat('', a.tax_percentage3, ' %   ')" +
                      " END, CASE WHEN a.tax_amount3 = '0' THEN '  ' ELSE concat(' : ', a.tax_amount3) END) AS tax3" +
                      " , format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.tax_rate " +
                      " FROM smr_tmp_tsalesorderdtl a " +
                      " left join smr_trn_tsalesorder s on s.salesorder_gid=a.salesorder_gid " +
                      " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                      " left join pmr_mst_tproductgroup v on b.productgroup_gid=v.productgroup_gid " +
                      " LEFT JOIN acp_mst_tvendor e ON a.vendor_gid = e.vendor_gid " +
                      " where a.enquiry_gid='" + values.enquiry_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<Post_List>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {

                                string msgetGid2 = objcmnfunctions.GetMasterGID("VQDT");
                                if (msgetGid2 == "E")
                                {
                                    values.status = true;
                                    values.message = "Create Sequence Code PPDC for Sales Enquiry Details";
                                    return;
                                }
                                else
                                {
                                    msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                        " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                         " WHERE product_gid='" + dt["product_gid"].ToString() + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        lsorder_type = "Sales";
                                    }
                                    else
                                    {
                                        lsorder_type = "Services";
                                    }

                                    msSQL = "insert into smr_trn_treceivequotationdtl (" +
                                            " quotationdtl_gid ," +
                                            " quotation_gid," +
                                            " product_gid ," +
                                            " productgroup_gid," +
                                            " productgroup_name," +
                                            " product_name," +
                                            " product_code," +
                                            " display_field," +
                                            " product_price," +
                                            " qty_quoted," +
                                            " discount_percentage," +
                                            " discount_amount," +
                                            " uom_gid," +
                                            " uom_name," +
                                            " price," +
                                            " quote_type," +
                                            " tax_name," +
                                            " tax_name2," +
                                            " tax_name3," +
                                            " taxsegment_gid," +
                                            " product_remarks," +
                                            " tax_percentage," +
                                            " tax_percentage2," +
                                            " tax_percentage3," +
                                            " tax_amount," +
                                            " tax_amount2," +
                                            " tax_amount3," +
                                            " tax1_gid," +
                                            " tax2_gid," +
                                            " tax3_gid," +
                                            " slno " +
                                            ")values(" +
                                            " '" + msgetGid2 + "'," +
                                            " '" + msGetGid + "'," +
                                            " '" + dt["product_gid"].ToString() + "'," +
                                            " '" + dt["productgroup_gid"].ToString() + "'," +
                                            " '" + dt["productgroup_name"].ToString().ToString().Replace("'", "\\\'") + "'," +
                                            " '" + dt["product_name"].ToString().ToString().Replace("'", "\\\'") + "'," +
                                            " '" + dt["product_code"].ToString() + "'," +
                                            " '" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +
                                            " '" + dt["product_price"].ToString().Replace(",", "").Trim() + "'," +
                                            " '" + dt["qty_quoted"].ToString() + "'," +
                                            " '" + dt["discount_percentage"].ToString() + "'," +
                                            " '" + dt["discount_amount"].ToString().Replace(",", "").Trim() + "'," +
                                            " '" + dt["uom_gid"].ToString() + "'," +
                                            " '" + dt["uom_name"].ToString().ToString().Replace("'", "\\\'") + "'," +
                                            " '" + dt["price"].ToString().Replace(",", "").Trim() + "'," +
                                            " '" + lsorder_type + "'," +
                                            " '" + dt["tax_name"].ToString() + "'," +
                                            " '" + dt["tax_name2"].ToString() + "'," +
                                            " '" + dt["tax_name3"].ToString() + "'," +
                                            " '" + dt["taxsegment_gid"].ToString() + "'," +
                                            " '" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +
                                            " '" + dt["tax_percentage"].ToString() + "'," +
                                            " '" + dt["tax_percentage2"].ToString() + "'," +
                                            " '" + dt["tax_percentage3"].ToString() + "'," +
                                            " '" + dt["tax_amount"].ToString() + "'," +
                                            " '" + dt["tax_amount2"].ToString() + "'," +
                                            " '" + dt["tax_amount3"].ToString() + "'," +
                                            " '" + dt["tax1_gid"].ToString() + "'," +
                                            " '" + dt["tax2_gid"].ToString() + "'," +
                                            " '" + dt["tax3_gid"].ToString() + "'," +
                                            "'" + dt_datatable.Rows.Count + "')";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                }
                            }
                        }
                        if (mnResult == 0)
                        {
                            values.status = false;
                            values.message = "Error occured while Inserting into Quotationdtl";
                            return;
                        }
                        else
                        {
                            msSQL = " delete from smr_tmp_tsalesorderdtl " +
                           " where employee_gid='" + employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = "select distinct quotation_type from smr_tmp_treceivequotationdtl where created_by='" + employee_gid + "' ";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                            if (objOdbcDataReader.HasRows == true)
                            {
                                lsquotation_type = "sales";

                            }
                            else
                            {
                                lsquotation_type = "Service";
                            }

                        }

                        msSQL = " update smr_trn_treceivequotation set quotation_type='" + lsquotation_type + "' where quotation_gid='" + msGetGid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " update smr_trn_treceivequotation Set " +
                    " leadbank_gid = '" + lsleadbank_gid + "'" +
                    " where quotation_gid='" + msGetGid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }


                    string lsstage = "4";
                    string msgetlead2campaign_gid = objcmnfunctions.GetMasterGID("BLCC");
                    string lsso = "N";


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
                                   "'" + values.campaignGid + "'," +
                                   "'" + employee_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    string msgetGid4 = objcmnfunctions.GetMasterGID("PODC");
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
                                "'" + msGetGid + "') ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "select approval_flag from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + msGetGid + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == false)
                        {

                            msSQL = " Update smr_trn_treceivequotation Set " +
                                   " quotation_status = 'Approved', " +
                                   " approved_by = '" + employee_gid + "', " +
                                   " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                   " where quotation_gid = '" + msGetGid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                        else
                        {
                            msSQL = "select approved_by from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + msGetGid + "'";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        }
                        if (objOdbcDataReader.RecordsAffected == 1)
                        {

                            msSQL = " update smr_trn_tapproval set " +
                           " approval_flag = 'Y', " +
                           " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                           " where approved_by = '" + employee_gid + "'" +
                           " and qoapproval_gid = '" + msGetGid + "' and submodule_gid='SMRSMRQAP'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            msSQL = " Update smr_trn_treceivequotation Set " +
                                   " quotation_status = 'Approved', " +
                           " approved_by = '" + employee_gid + "', " +
                           " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                           " where quotation_gid = '" + msGetGid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                        else if (objOdbcDataReader.RecordsAffected > 1)
                        {

                            msSQL = " update smr_trn_tapproval set " +
                                   " approval_flag = 'Y', " +
                                   " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                   " where approved_by = '" + employee_gid + "'" +
                                   " and qoapproval_gid = '" + msGetGid + "' and submodule_gid='SMRSMRQAP'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                    }


                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Quotation Raised Successfully!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Raising Quotation!";
                        return;
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Select Atleast One Product to Raise Quotation";
                    return;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submiting  Quotation !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // ENQUIRY TO QUOTATION PRODUCT SUMMARY EDIT
        public void DaGetEnqtoQuoteEditProductSummary(string tmpquotationdtl_gid, MdlEnquiryToQuotation values)
        {
            try
            {


                msSQL = " Select a.enquiry_gid,a.tmpquotationdtl_gid, a.quotation_gid,a.product_gid, a.product_name,a.productgroup_gid, " +
                        " a.productgroup_name, a.uom_gid, a.uom_name, a.product_code, a.discount_percentage, " +
                        " a.discount_amount,a.price, a.tax_name, a.tax_amount, a.tax_percentage, a.qty_quoted,(CASE WHEN a.product_price = '0' THEN b.cost_price ELSE a.product_price END) as product_price " +
                        " from smr_tmp_treceivequotationdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                        " where tmpquotationdtl_gid = '" + tmpquotationdtl_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<EditQuoteProductList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new EditQuoteProductList
                        {
                            tmpquotationdtl_gid = dt["tmpquotationdtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            unitprice = dt["product_price"].ToString(),
                            discountpercentage = dt["discount_percentage"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            totalamount = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                        });
                        values.EditQuoteProduct_list = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while EnqtoQuote Edit ProductSummary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        // PRODUCT UPPDATE -- ENQUIRY TO QUOTATION 
        public void DaPostUpdateEnquirytoQuotationProduct(string employee_gid, EditQuoteProductList values)
        {
            try
            {
                if (values.tax_gid != null || values.tax_gid != "")
                {
                    msSQL = " select percentage from acp_mst_ttax  where tax_gid='" + values.tax_name + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select tax_name from acp_mst_ttax  where tax_gid='" + values.tax_name + "'";
                    lstaxname = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                 
                    msSQL = " select percentage from acp_mst_ttax  where tax_name='" + values.tax_name + "'";
                     lspercentage1 = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select tax_name from acp_mst_ttax  where tax_gid='" + values.tax_name + "'";
                    lstaxname = objdbconn.GetExecuteScalar(msSQL);
                }
               
                    msSQL = "Select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "' and delete_flag='N' and" +
                        " product_code='" + values.product_code + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                
                    if (objOdbcDataReader.HasRows == true)
                    {
                        values.product_gid = objOdbcDataReader["product_gid"].ToString();
                    }
               
                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                        " left join pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                        " WHERE product_gid='" + values.product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows == true)
                {
                    lsproduct_type = "Sales";

                }
                else
                {
                    lsproduct_type = "Services";
                }
               
                msSQL = " update smr_tmp_treceivequotationdtl set " +
                        " product_gid='" + values.product_gid + "'," +
                        " product_name='" + values.product_name + "'," +
                        " product_price='" + values.unitprice + "'," +
                        " qty_quoted='" + values.quantity + "'," +
                        " discount_percentage='" + values.discountpercentage + "'," +
                        " discount_amount='" + values.discountamount + "'," +
                        " uom_gid='" + lsproductuomgid + "'," +
                        " uom_name='" + values.productuom_name + "'," +
                        " price='" + values.totalamount.Replace(",", "") + "',";
                         if (lspercentage1 == "" || lspercentage1 == null || lspercentage1 == "undefined")
                         {
                               msSQL += "tax_percentage = '0.00', ";
                         }
                         else
                         {
                               msSQL += " tax_percentage='" + lspercentage1 + "',";
                         }
                msSQL += " tax_name='" + lstaxname + "'," +
                        " tax_amount='" + values.tax_amount + "'," +
                        " updated_by='" + employee_gid + "'," +
                        " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " product_code='" + values.product_code + "'," +
                        " quotation_type='" + lsproduct_type + "'" +
                        " where tmpquotationdtl_gid='" + values.tmpquotationdtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = " Product Updated Successfully ";
                }
                else
                {
                    values.status = false;
                    values.message = " Error While Updating Product Details ";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while UpdateEnquirytoQuotation Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        // ENQUIRY TO QUOTATION DELETE EVENT
        public void DaGetDeleteQuoteProductSummary(string tmpquotationdtl_gid, EditQuoteProductList values)
        {
            try
            {

                msSQL = "delete from smr_tmp_treceivequotationdtl where tmpquotationdtl_gid='" + tmpquotationdtl_gid + "'  ";
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
                values.message = "Exception occured while Deleting Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        // ENQUIRY TO QUOTATION PRODUCT SUMMARY DETAIL BUTTON
        public void DaGetRaiseQuotedetail(string product_gid, MdlEnquiryToQuotation values)
        {
            try
            {

                msSQL = " select a.quotation_gid,b.quotationdtl_gid,b.customerproduct_code,b.product_gid," +
                        " a.currency_code,b.product_requireddate as " + "product_requireddate, " +
                        " d.product_name,date_format(a.quotation_date,'%d-%m-%Y') as quotation_date, " +
                        " a.customer_gid,a.customer_name,b.qty_quoted,format(b.product_price,2) as product_price,c.leadbank_name  from smr_trn_treceivequotation a " +
                         " left join smr_trn_treceivequotationdtl b on a.quotation_gid=b.quotation_gid  " +
                         " left join pmr_mst_tproduct d on b.product_gid = d.product_gid " +
                         " left join crm_trn_tleadbank c on a.customer_gid=c.leadbank_gid " +
                         " where b.product_gid='" + product_gid + "' "+
                         " group by b.product_price " +
                         " order by a.quotation_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<EditQuoteProductList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new EditQuoteProductList
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                        });
                        values.EditQuoteProduct_list = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Raise Quotedetail !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetProductTaxdetails(string product_gid, string customercontact_gid, MdlEnquiryToQuotation values)
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

                        var getModuleList = new List<GetOnChangeProductDropdownETQ>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetOnChangeProductDropdownETQ
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    unitprice = lsproduct_price,

                                });
                                values.GetOnChangeProductETQ = getModuleList;
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

                            var getGetTaxSegmentList = new List<QuoteSegmentList>();
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    getGetTaxSegmentList.Add(new QuoteSegmentList
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
                                    values.QuoteSegment_List = getGetTaxSegmentList;
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

                        var getModuleList = new List<GetOnChangeProductDropdownETQ>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetOnChangeProductDropdownETQ
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    unitprice = dt["mrp_price"].ToString(),

                                });
                                values.GetOnChangeProductETQ = getModuleList;
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

                            var getGetTaxSegmentList = new List<QuoteSegmentList>();
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    getGetTaxSegmentList.Add(new QuoteSegmentList
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
                                    values.QuoteSegment_List = getGetTaxSegmentList;
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
    }
}