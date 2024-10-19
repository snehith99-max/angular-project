
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
    public class DaSmrSalesOrderAmend
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msINGetGID, msGetinGid, taxgid, msSQL1, msSQL2 = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lspercentage, start_date, end_date, lspercentage1, lsdesignation_code, lstaxname2, lsorder_type, lsproduct_type, lsproductgid1, lstaxname1, lsdiscountpercentage, lsdiscountamount, lsprice, lstype1, lsproduct_price, mssalesorderGID, mssalesorderGID1, mscusconGetGID, lscustomer_name, msGetCustomergid, lscustomer_gid, msGetGid2, msGetGid3, lsCode, msPOGetGID, msGetGID, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetAmendSalesOrderDtl( string salesorder_gid, MdlSmrSalesOrderAmend values)
        {
            try
            {          

                msSQL = " select a.salesorder_gid,date_format(a.salesorder_date,'%d-%m-%Y') as salesorder_date," +
                        " CASE WHEN a.start_date = '0000-00-00' THEN '' ELSE DATE_FORMAT(a.start_date, '%d-%m-%Y') END AS start_date," +
                        " CASE WHEN a.end_date = '0000-00-00' THEN '' ELSE DATE_FORMAT(a.end_date, '%d-%m-%Y') END AS end_date, " +
                        " order_note,a.so_referenceno1,a.customer_gid, " +
                        " a.customer_contact_person,a.total_amount,e.campaign_gid, concat(e.campaign_title, ' | ', f.user_code, ' | ', f.user_firstname, ' ', f.user_lastname) as username," +
                        " a.customer_address,a.so_remarks,a.created_by,format(a.total_price,2) as total_price, " +
                        " format(total_amount,2) as total_amount,format(a.roundoff,2) as roundoff,a.payment_days, " +
                        " a.delivery_days ,a.salesorder_status,format(a.Grandtotal,2) as Grandtotal,format(addon_charge,2) as addon_charge, " +
                        " a.so_referenceno1,format(additional_discount,2) as additional_discount, " +
                        " a.currency_code,a.exchange_rate,a.customer_contact_gid, " +
                        " a.termsandconditions,a.renewal_date,a.renewal_description ,a.shipping_to ,a.freight_terms, " +
                        " a.payment_terms,a.currency_gid,a.tax_gid,a.campaign_gid, " +
                        " a.salesperson_gid,customerbranch_gid,a.customer_email, " +
                        " b.branch_name,format(a.freight_charges,2) as freight_charges,format(a.buyback_charges,2) as buyback_charges,format(a.packing_charges,2) as packing_charges,format(a.insurance_charges,2) as insurance_charges,a.customer_mobile,a.customer_name,so_referenceno1  ,a.tax_name ,a.tax_amount " +
                        " from smr_trn_tsalesorder a " +
                        " left join hrm_mst_tbranch b on a.branch_gid=b.branch_gid " +
                        " left join crm_trn_tenquiry2campaign d on a.customer_gid=d.customer_gid " +
                        " left join smr_trn_tcampaign e on d.campaign_gid=e.campaign_gid " +
                        " left join adm_mst_tuser f on a.salesperson_gid=f.user_gid " +
                        " left join hrm_mst_temployee g on f.user_gid=g.user_gid " +
                        "  left join smr_trn_tsalesorderdtl c on c.salesorder_gid=a.salesorder_gid "+
                        " where a.salesorder_gid ='" + salesorder_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<SOAmendsummaryList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {                   
                       

                        getModuleList.Add(new SOAmendsummaryList
                        {
                            customer_name = dt["customer_name"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            freight_terms = dt["freight_terms"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            customer_mobile = dt["customer_mobile"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),                            
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            total_price = dt["total_price"].ToString(),
                            shipping_to = dt["shipping_to"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            so_remarks = dt["so_remarks"].ToString(),
                            so_referencenumber = dt["so_referenceno1"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            delivery_days = dt["delivery_days"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            additional_discount = dt["additional_discount"].ToString(),
                            addon_charge = dt["addon_charge"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            campaign_gid = dt["campaign_gid"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            total_amount = dt["total_amount"].ToString(),                            
                            start_date = dt["start_date"].ToString(),
                            end_date = dt["end_date"].ToString(),
                            salesperson_gid = dt["username"].ToString(),
                            freight_charges = dt["freight_charges"].ToString(),
                            packing_charges = dt["packing_charges"].ToString(),
                            buyback_charges = dt["buyback_charges"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_gid = dt["currency_gid"].ToString(),
                            insurance_charges = dt["insurance_charges"].ToString(),
                            customer_email = dt["customer_email"].ToString(),
                            salesorder_refno = dt["so_referenceno1"].ToString(),
                            tax_name4= dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString()
                        });
                    }
                    values.SOAmendsummary_List = getModuleList;
                }
                dt_datatable.Dispose();

                msSQL = " select salesorderdtl_gid,salesorder_gid,slno," +
                           " product_gid,product_name,qty_quoted,product_price,discount_percentage, " +
                           " discount_amount,tax_percentage,tax_amount,uom_gid,price,product_code," +
                           " uom_name,tax_name from smr_trn_tsalesorderdtl " +
                           " where salesorder_gid='" + salesorder_gid + "' AND salesorderdtl_gid NOT IN " +
                           " (SELECT salesorderdtl_gid FROM smr_tmp_tsalesorderdtl WHERE salesorder_gid ='" + salesorder_gid + "') group by salesorderdtl_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string msgettempGid = objcmnfunctions.GetMasterGID("VSDT");

                        msSQL2 = " insert into smr_tmp_tsalesorderdtl(" +
                                   " tmpsalesorderdtl_gid," +
                                   " salesorder_gid," +
                                   " discount_percentage," +
                                   " discount_amount," +
                                   " tax_percentage," +
                                   " tax_amount," +
                                   " price," +
                                   " tax_name," +
                                   " product_gid," +
                                   " product_name," +
                                   " uom_gid," +
                                   " uom_name," +
                                   " qty_quoted," +
                                   " product_code," +
                                   " product_price, " +
                                   " slno" +
                                   " ) " +
                                   " values ( " +
                                   "'" + msgettempGid + "', " +
                                   "'" + dt["salesorder_gid"].ToString() + "', " +
                                   "'" + dt["discount_percentage"].ToString() + "', " +
                                   "'" + dt["discount_amount"].ToString() + "', " +
                                   "'" + dt["tax_percentage"].ToString() + "', " +
                                   "'" + dt["tax_amount"].ToString() + "', " +
                                   "'" + dt["price"].ToString() + "', " +
                                   "'" + dt["tax_name"].ToString() + "', " +
                                   "'" + dt["product_gid"].ToString() + "', " +
                                   "'" + dt["product_name"].ToString() + "', " +
                                   "'" + dt["uom_gid"].ToString() + "', " +
                                   "'" + dt["uom_name"].ToString() + "', " +
                                   "'" + dt["qty_quoted"].ToString() + "'," +
                                   "'" + dt["product_code"].ToString() + "'," +
                                   "'" + dt["product_price"].ToString() + "'," +
                                   "'" + dt_datatable.Rows.Count + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL2);
                    }
                }
             
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // PRODUCT ADD FOR SALES ORDER AMEND
        public void DaPostSOAmendProduct(string employee_gid, amendtemplist values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("VSDT");

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

                msSQL = "select tax_gid from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                string lstaxgid = objdbconn.GetExecuteScalar(msSQL);

               
                msSQL = "select percentage from acp_mst_ttax where tax_gid='" + lstaxgid + "'";
                string lspercentage1 = objdbconn.GetExecuteScalar(msSQL);


 
                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                 " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                 " WHERE product_gid='" + lsproductgid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                   
                    if (objOdbcDataReader["producttype_name"].ToString() != "Services")
                    {
                        lsorder_type = "Sales";
                       
                    }
                    else
                    {
                        lsorder_type = "Services";
                    }
                    objOdbcDataReader.Close();
                }

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
                   "tax1_gid," +
                   " tax_name," +
                   " tax_amount," +
                   " discount_amount, " +
                   " discount_percentage, " +
                   " tax_percentage" +
                   ")values(" +
                   "'" + msGetGid + "'," +
                   "'" + values.salesorder_gid + "'," +
                   "'" + employee_gid + "'," +
                   "'" + lsproductgid + "'," +
                   "'" + values.product_code + "'," +
                   "'" + values.product_name + "'," +
                   "'" + values.unitprice + "'," +
                   "'" + values.quantity + "'," +
                   "'" + lsproductuomgid + "'," +
                   "'" + values.productuom_name + "'," +
                   "'" + values.totalamount + "'," +
                   " '" + lsorder_type + "', " +
                   "'" + lstaxgid + "'," +
                " '" + values.tax_name + "', " +
                   "'" + values.tax_amount + "'," +
                   "'" + lsdiscountamount + "'," +
                 "'" + lsdiscountpercentage + "'," +
                 "'" + lspercentage1 + "')";
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
                values.message = "Exception occured while Adding Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // PRODUCT SUMMARY FOR SALES ORDER AMEND
        public void DaGetSOProductAmendSummary(string employee_gid, string salesorder_gid, MdlSmrSalesOrderAmend values)
        {
            try
            {

                msSQL = " select a.tmpsalesorderdtl_gid,a.margin_percentage,a.margin_amount,a.salesorder_gid, " +
             " a.selling_price,a.productgroup_gid,a.productgroup_name, " +
             " if(a.customerproduct_code='+nbsp;',' ',a.customerproduct_code) as customerproduct_code," +
             " a.discount_percentage, format(a.discount_amount,2) as discount_amount, " +
             " a.tax_percentage,format(a.tax_amount,2) as tax_amount,a.vendor_gid,a.payment_days,a.delivery_period,  " +
             " a.product_remarks,format(a.price,2) as price,a.display_field, " +
             " a.tax_name,a.tax_name2,a.tax_name3,a.tax_percentage2,a.tax_percentage3, " +
             " format(a.tax_amount2,2) as tax_amount2,format(a.tax_amount3,2) as tax_amount3, " +
             " a.product_gid,a.product_name,a.uom_gid,a.uom_name,a.qty_quoted, " +
             " a.product_delivered,a.employee_gid,a.vendor_gid,e.vendor_companyname, " +
             " b.product_code,a.slno,a.tax1_gid,a.tax1_gid,a.tax3_gid,a.tax2_gid, " +
             " a.tax3_gid,a.product_requireddateremarks, " +
             " format(a.product_price,2) as product_price,date_format(product_requireddate,'%d-%m-%Y') " +
             " as product_requireddate " +
             " from smr_tmp_tsalesorderdtl a" +
             " left join pmr_mst_tproduct b on a.product_gid= b.product_gid " +
             " left join acp_mst_tvendor e on a.vendor_gid=e.vendor_gid " +
             "  where salesorder_gid='" + salesorder_gid + "' order by tmpsalesorderdtl_gid asc ";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                double grand_total = 0.00;
                var getModuleList = new List<amendtemplist>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt1 in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt1["price"].ToString());

                        getModuleList.Add(new amendtemplist
                        {
                            tmpsalesorderdtl_gid = dt1["tmpsalesorderdtl_gid"].ToString(),                           
                            salesorder_gid = dt1["salesorder_gid"].ToString(),                           
                            discountpercentage = dt1["discount_percentage"].ToString(),
                            discount_amount = dt1["discount_amount"].ToString(),
                            tax_percentage = dt1["tax_percentage"].ToString(),
                            tax_amount = dt1["tax_amount"].ToString(),                                    
                            price = dt1["price"].ToString(),
                            totalamount = dt1["price"].ToString(),
                            tax_name = dt1["tax_name"].ToString(),
                            product_gid = dt1["product_gid"].ToString(),
                            product_name = dt1["product_name"].ToString(),
                            uom_gid = dt1["uom_gid"].ToString(),
                            uom_name = dt1["uom_name"].ToString(),
                            qty_quoted = dt1["qty_quoted"].ToString(),                           
                            product_code = dt1["product_code"].ToString(),                                                     
                            product_price = dt1["product_price"].ToString(),                            
                            grand_total = grand_total
                        });
                    }
                    values.amendtemp_list = getModuleList;
                    values.grand_total = grand_total;
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // OVERALL SUBMIT FOR SALES ORDER AMEND

        public void DaAmendSalesOrder(string employee_gid, PostAmendSO_List values)
        {
            try
            {

                string totalvalue = values.user_name;

                string[] separatedValues = totalvalue.Split('|').Concat(totalvalue.Split(' ')).ToArray();


                // Access individual components
                string campaign_title = separatedValues[0];
                string user_code = separatedValues[1];
                string user_firstname = separatedValues[2];
                string user_lastname = separatedValues.Length == 3 ? separatedValues[3] : null;

              
                string lsstatus = "SO Amended";
                msSQL = " select branch_gid from hrm_mst_tbranch where branch_name='" + values.customer_branch + "'";
               string lsbranch_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + values.currency_code + "'";
               string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

                if (values.tax_name4 == null || values.tax_name4 == "")
                {
                    values.tax_name4 = "0.00";
                    taxgid = "0.00";
                }
                else
                {
                    msSQL = " select tax_gid from acp_mst_ttax where tax_name ='" + values.tax_name4 + "'";
                    taxgid = objdbconn.GetExecuteScalar(msSQL);
                }

                string msGetGid = objcmnfunctions.GetMasterGID("SORN");
                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='SORN'";
                string lsCode = objdbconn.GetExecuteScalar(msSQL);




                //double lslocalgrandtotal, lslocaladdon, lslocaladditionaldiscount, Total_Price, Total_Amount = 0.00;
                double lslocalgrandtotal = double.Parse(values.total_amount) * double.Parse(values.exchange_rate);
                double lslocaladdon = double.Parse(values.addon_charge) * double.Parse(values.exchange_rate);
                double lslocaladditionaldiscount = double.Parse(values.additional_discount);
                double Total_Price = double.Parse(values.producttotalamount);
                double Total_Amount = double.Parse(values.Grandtotal);
               
                string uiDateStr = values.salesorder_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string salesorder_date = uiDate.ToString("yyyy-MM-dd");

                string uiDateStr1 = values.start_date;
                DateTime uiDate1 = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string start_date = uiDate1.ToString("yyyy-MM-dd");

                string uiDateStr2 = values.end_date;
                DateTime uiDate2 = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string end_date = uiDate2.ToString("yyyy-MM-dd");

                string trimmedName = user_code.Trim();


                msSQL = "SELECT user_gid FROM adm_mst_tuser WHERE user_code = '" + trimmedName + "'";
                string user = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "insert into smr_trn_tsalesorder  (" +
                                " salesorder_gid ," +
                                " branch_gid ," +
                                " salesorder_date ," +
                                " customer_gid ," +
                                " customer_name ," +
                                " customer_contact_person ," +
                                " customer_address ," +
                                " shipping_to ," +
                                " freight_terms ," +
                                " payment_terms ," +
                                " customer_email ," +
                                " customer_mobile ," +
                                " so_referenceno1 ," +
                                " created_by ," +
                                " so_remarks ," +
                                " payment_days ," +
                                " delivery_days ," +
                                " Grandtotal ," +
                                " termsandconditions ," +
                                " salesorder_status ," +
                                " addon_charge ," +
                                " additional_discount ," +
                                " addon_charge_l ," +
                                " additional_discount_l ," +
                                " grandtotal_l ," +
                                " currency_code ," +
                                " currency_gid ," +
                                " exchange_rate ," +
                                " total_price ," +
                                " tax_name ," +
                                " tax_gid ," +
                                " tax_amount ," +
                                " total_amount ," +
                                " salesperson_gid ," +
                                " start_date ," +
                                " end_date ," +
                                " roundoff ," +
                                " updated_additional_discount ," +
                                " freight_charges ," +
                                " buyback_charges ," +
                                " packing_charges ," +
                                " insurance_charges " +
                                " )values(" +
                                " '" + values.salesorder_gid + "NHA" + lsCode + "'," +
                                " '" + lsbranch_gid + "'," +
                                " '" + salesorder_date + "'," +
                                " '" + values.customer_gid + "'," +
                                " '" + values.customer_name + "'," +
                                " '" + values.customer_contact_person + "'," +
                                " '" + values.customer_address.Replace("'", "\\\'") + "'," +
                                " '" + values.shipping_to.Replace("'", "\\\'") + "'," +
                                " '" + values.freight_terms + "'," +
                                " '" + values.payment_terms + "'," +
                                " '" + values.customer_email + "'," +
                                " '" + values.customer_mobile + "'," +
                                " '" + values.so_referencenumber + "'," +
                                " '" + employee_gid + "'," +
                                " '" + values.so_remarks + "'," +
                                " '" + values.payment_days + "'," +
                                " '" + values.delivery_days + "'," +
                                " '" + Total_Amount + "'," +
                                " '" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "") + "'," +
                                " '" + lsstatus + "',";
                if (values.addon_charge == null || values.addon_charge == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.addon_charge + "',";
                }
                if (values.additional_discount == null || values.additional_discount == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.additional_discount + "',";
                }
                msSQL += "'" + lslocaladdon + "'," +
                         "'" + lslocaladditionaldiscount + "'," +
                         "'" + values.Grandtotal.Replace(",", "").Trim() + "'," +
                         "'" + lscurrency_code + "'," +
                         "'" + values.currency_code + "'," +
                         "'" + values.exchange_rate + "'," +
                         "'" + Total_Price + "'," +
                         "'" + values.tax_name4 + "'," +
                         "'" + taxgid + "',";
                if (values.tax_amount4 == null || values.tax_amount4 == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.tax_amount4 + "',";
                }
                if (values.total_amount == null || values.total_amount == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.total_amount.Replace(",", "").Trim() + "',";
                }
                 msSQL += "'" + user + "'," +
                            "'" + start_date + "'," +
                            "'" + end_date + "',";
                if (values.roundoff == null || values.roundoff == "")
                     {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.roundoff + "',";
                }
                if (values.additional_discount == null || values.additional_discount == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.additional_discount + "',";
                }
                if (values.freight_charges == null || values.freight_charges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.freight_charges + "',";
                }
                if (values.buyback_charges == null || values.buyback_charges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.buyback_charges + "',";
                }
                if (values.packing_charges == null || values.packing_charges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.packing_charges + "',";
                }
                if (values.insurance_charges == null || values.insurance_charges == "")
                {
                    msSQL += "'0.00')";
                }
                else
                {
                    msSQL += "'" + values.insurance_charges + "')";
                }
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error Occurred while adding Salesorder";
                    msSQL = " delete from smr_trn_tsalesorder where salesorder_gid ='" + values.salesorder_gid + "NHA" + lsCode + "' ";
                   

                    return;
                }
                else
                {
                    msSQL = "insert into acp_trn_torder  (" +
                                " salesorder_gid ," +
                                " branch_gid ," +
                                " salesorder_date ," +
                                " customer_gid ," +
                                " customer_name ," +
                                " customer_contact_person ," +
                                " customer_address ," +
                                " shipping_to ," +                               
                                " customer_email ," +
                                " customer_mobile ," +
                                " so_referenceno1 ," +
                                " created_by ," +
                                " so_remarks ," +
                                " payment_days ," +
                                " delivery_days ," +
                                " Grandtotal ," +
                                " termsandconditions ," +
                                " salesorder_status ," +
                                " addon_charge ," +
                                " additional_discount ," +
                                " addon_charge_l ," +
                                " additional_discount_l ," +
                                " grandtotal_l ," +
                                " currency_code ," +
                                " currency_gid ," +
                                " exchange_rate ," +                               
                                " salesperson_gid ," +
                                " start_date ," +
                                " end_date ," +
                                " roundoff ," +
                                " updated_additional_discount ," +
                                " freight_charges ," +
                                " buyback_charges ," +
                                " packing_charges ," +
                                " insurance_charges " +
                                " )values(" +
                                " '" + values.salesorder_gid + "NHA" + lsCode + "'," +
                                " '" + lsbranch_gid + "'," +
                                " '" + salesorder_date + "'," +
                                " '" + values.customer_gid + "'," +
                                " '" + values.customer_name + "'," +
                                " '" + values.customer_contact_person + "'," +
                                " '" + values.customer_address.Replace("'", "\\\'") + "'," +
                                " '" + values.shipping_to.Replace("'", "\\\'") + "'," +                               
                                " '" + values.customer_email + "'," +
                                " '" + values.customer_mobile + "'," +
                                " '" + values.so_referencenumber + "'," +
                                " '" + employee_gid + "'," +
                                " '" + values.so_remarks + "'," +
                                " '" + values.payment_days + "'," +
                                " '" + values.delivery_days + "'," +
                                " '" + Total_Amount + "'," +
                                " '" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "") + "'," +
                                " '" + lsstatus + "',";
                    if (values.addon_charge == null || values.addon_charge == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.addon_charge + "',";
                    }
                    if (values.additional_discount == null || values.additional_discount == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.additional_discount + "',";
                    }
                    msSQL += "'" + lslocaladdon + "'," +
                             "'" + lslocaladditionaldiscount + "'," +
                             "'" + values.Grandtotal.Replace(",", "").Trim() + "'," +
                             "'" + lscurrency_code + "'," +
                             "'" + values.currency_code + "'," +
                             "'" + values.exchange_rate + "'," +
                             "'" + user + "'," +
                             "'" + start_date + "'," +
                             "'" + end_date + "',";
                    if (values.roundoff == null || values.roundoff == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.roundoff + "',";
                    }
                    if (values.additional_discount == null || values.additional_discount == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.additional_discount + "',";
                    }
                    if (values.freight_charges == null || values.freight_charges == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.freight_charges + "',";
                    }
                    if (values.buyback_charges == null || values.buyback_charges == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.buyback_charges + "',";
                    }
                    if (values.packing_charges == null || values.packing_charges == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.packing_charges + "',";
                    }
                    if (values.insurance_charges == null || values.insurance_charges == "")
                    {
                        msSQL += "'0.00')";
                    }
                    else
                    {
                        msSQL += "'" + values.insurance_charges + "')";
                    }
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                   
                }
                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = " Error Occurs While Submitting";
                    msSQL = " delete from smr_trn_tsalesorder where salesorder_gid ='" + values.salesorder_gid + "NHA" + lsCode + "' ";
                    msSQL = " delete from acp_trn_torder where salesorder_gid ='" + values.salesorder_gid + "NHA" + lsCode + "' ";

                    return;
                }

                    
                //DataTable objtbl;
            else
                {


                    msSQL = " select  tmpsalesorderdtl_gid, salesorder_gid, product_gid, productgroup_gid,product_code," +
                               " productgroup_name,  product_name, product_price, qty_quoted, format(discount_percentage,2) as margin_percentage," +
                               " format(discount_amount,2) as margin_amount,  uom_gid, uom_name, format(price,2) as price, tax_name, " +
                               "  tax1_gid, tax_amount,  tax_percentage, " +
                               " order_type" +
                               " from smr_tmp_tsalesorderdtl where salesorder_gid='" + values.salesorder_gid + "'" +
                               " order by tmpsalesorderdtl_gid";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {                          
                            
                            int i = 0;

                            string msGID = objcmnfunctions.GetMasterGID("VSDC");

                            msSQL = "insert into smr_trn_tsalesorderdtl (" +
                                    " salesorderdtl_gid ," +
                                    " salesorder_gid," +
                                    " product_gid ," +
                                    " product_code ," +
                                    " productgroup_gid," +
                                    " productgroup_name," +
                                    " product_name," +
                                    " product_price," +
                                    " qty_quoted," +
                                    " discount_percentage," +
                                    " discount_amount," +
                                    " uom_gid," +
                                    " uom_name," +
                                    " price," +
                                    " slno," +
                                    " tax_name," +
                                    " tax1_gid," +
                                    " tax_amount ," +
                                    " tax_percentage," +
                                    " type " +
                                    ")values(" +
                                    " '" + msGID + "'," +
                                    " '" + values.salesorder_gid + "NHA" + lsCode + "'," +
                                    " '" + dt["product_gid"].ToString() + "'," +
                                    " '" + dt["product_code"].ToString() + "'," +
                                    " '" + dt["productgroup_gid"].ToString() + "'," +
                                    " '" + dt["productgroup_name"].ToString() + "'," +
                                    " '" + dt["product_name"].ToString() + "'," +
                                    " '" + dt["product_price"].ToString() + "'," +
                                    " '" + dt["qty_quoted"].ToString() + "'," +
                                    " '" + dt["margin_percentage"].ToString() + "'," +
                                    " '" + dt["margin_amount"].ToString().Replace(",", "").Trim() + "'," +
                                    " '" + dt["uom_gid"].ToString() + "'," +
                                    " '" + dt["uom_name"].ToString() + "'," +
                                    " '" + dt["price"].ToString().Replace(",", "").Trim() + "'," +
                                    " '" + i + 1 + "'," +
                                    " '" + dt["tax_name"].ToString() + "'," +
                                    " '" + dt["tax1_gid"].ToString() + "'," +
                                    " '" + dt["tax_amount"].ToString() + "'," +
                                    " '" + dt["tax_percentage"].ToString() + "'," +
                                    " '" + dt["order_type"].ToString() + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                    if(mnResult == 1)
                    {
                        string salestype;
                        msSQL = "select order_type from smr_trn_tsalesorderdtl where salesorder_gid='" + values.salesorder_gid + "NHA" + lsCode + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                       
                        if (objOdbcDataReader.HasRows == true)
                        {
                            salestype = "sales";


                        }

                        else
                        {
                            salestype = "Service";
                        }
                        objOdbcDataReader.Close();

                        msSQL = " Update smr_trn_tsalesorder Set " +
                                " so_type = '" + salestype + "'" +
                                " where salesorder_gid ='" + values.salesorder_gid + "NHA" + lsCode + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Sales order Amended Successfully";
                        msSQL = "delete from smr_tmp_tsalesorderdtl";
                        return;
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error Occured While Amend Sales order";
                        msSQL = " delete from smr_trn_tsalesorder where salesorder_gid ='" + values.salesorder_gid + "NHA" + lsCode + "' ";
                        msSQL = " delete from acp_trn_torder where salesorder_gid ='" + values.salesorder_gid + "NHA" + lsCode + "' ";
                        msSQL = "delete from smr_tmp_tsalesorderdtl where salesorder_gid ='" + values.salesorder_gid + "NHA" + lsCode + "' ";
                        return;
                        }

                    }

                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting  Amend Sales order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // TEMPORARY SUMMARY
        public void DaGetProductEditSummary(string tmpsalesorderdtl_gid, MdlSmrSalesOrderAmend values)
        {
            try
            {

                msSQL = " select a.tmpsalesorderdtl_gid,a.salesorder_gid,a.customerproduct_code,a.vendor_gid,c.vendor_companyname,b.product_code,a.selling_price," +
                " a.employee_gid,a.product_gid,a.slno,a.productgroup_gid,format(a.tax_percentage,2)as tax_percentage,format(a.tax_percentage2,2)as tax_percentage2,format(a.tax_percentage3,2)as tax_percentage3," +
                " a.productgroup_name,a.product_name,a.display_field,a.product_price,a.qty_quoted,a.discount_percentage,a.margin_percentage,a.margin_amount," +
                " a.discount_amount,a.uom_gid,a.uom_name,a.price,a.tax_name,a.tax_name2,a.tax_name3,a.tax1_gid,a.tax2_gid,a.tax3_gid,   " +
                " a.tax_amount,a.tax_amount2,a.tax_amount3,a.order_type,date_format(a.product_requireddate,'%d-%m-%Y') as product_requireddate,a.product_requireddateremarks " +
                " from smr_tmp_tsalesorderdtl a " +
                " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                " left join acp_mst_tvendor c on a.vendor_gid=c.vendor_gid " +
                " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<editorderList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new editorderList
                        {
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            customerproduct_code = dt["customerproduct_code"].ToString(),
                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            unitprice = dt["product_price"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            slno = dt["slno"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            productgroup_name  = dt["productgroup_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            discountpercentage = dt["discount_percentage"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            margin_amount = dt["margin_amount"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            margin_percentage = dt["margin_percentage"].ToString(),
                            unit = dt["uom_name"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            totalamount = dt["price"].ToString()
                        });
                        values.editorderList = getModuleList;
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


        /// TEMP SUMMARY PRODUCT UPDATE
        public void DaupdateSalesOrderedit(string employee_gid, Salesorders_list values)
        {
            try
            {
                string lstax_name;
                string lstax_amount;

                msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.unit + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productgroup_gid from pmr_mst_tproductgroup where productgroup_name='" + values.productgroup_name + "'";
                string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select tax_gid from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                string lstaxgid = objdbconn.GetExecuteScalar(msSQL);


                if (values.tax_name == "" || values.tax_name == null)
                {
                    lstax_name = "No tax";
                }
                else
                {
                    lstax_name = values.tax_name;
                    //msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + values.tax_name + "'";
                    //lstax_name = objdbconn.GetExecuteScalar(msSQL);
                }
                if (values.tax_amount == null)
                {
                    lstax_amount = "0";
                }
                else
                {

                    lstax_amount = values.tax_amount;
                }
                

                msSQL = "update smr_tmp_tsalesorderdtl set " +
                    " product_gid='" + lsproductgid + "', " +
                    " product_name='" + values.product_name + "', " +
                    " product_code='" + values.product_code + "', " +
                    " productgroup_gid='" + lsproductgroupgid + "', " +
                    " productgroup_name='" + values.productgroup_name + "', " +
                    " uom_gid='" + lsproductuomgid + "', " +
                    " uom_name='" + values.unit + "', " +
                    " discount_amount='" + values.discountamount + "', " +
                    " discount_percentage='" + values.discountpercentage + "', " +
                    " qty_quoted='" + values.quantity + "', " +
                    " tax1_gid='" + lstaxgid + "', " +
                    " tax_name='" + lstax_name + "'," +
                    " price='" + values.total_amount + "'," +
                    " tax_amount='" + lstax_amount + "'" +
                    " where tmpsalesorderdtl_gid='" + values.tmpsalesorderdtl_gid + "' ";
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

    }
}