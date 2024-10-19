﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Globalization;

using ems.pmr.Models;
//using DocumentFormat.OpenXml.Drawing.Charts;





namespace ems.pmr.DataAccess
{
    public class DaPmrTrnPurchaseQuotaion
    {


        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string lspop_server, lspop_mail, lspop_password, lscompany, lscompany_code;
        string msINGetGID, msGet_att_Gid, msenquiryloggid;
        string lspath, lspath1, lspath2, mail_path, mail_filepath, pdf_name = "";
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        OdbcDataReader ds_tsalesorderadd;
        DataTable mail_datatable, dt_datatable;

        string msEmployeeGID, lsemployee_gid, lsentity_code, lsquotationgid, msGetSalesOrderGID, mssalesorderGID1, msGetTempGID, lsquotation_type, lsdesignation_code, lstaxname2, lstaxname3, lsamount2, lsamount3, lspercentage2, lspercentage3, lscustomer_code, pricingsheet_refno, roundoff, mssalesorderGID, lsCode, msGetGid, msGetGid1, msgetGid2, msgetGid4, lstype1, lshierarchy_flag, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, lspop_port;
        string msGetCustomergid, msconGetGID;
        string lscustomer_name;
        string lscontact_person, lsvendor_companyname, lscustomercontact_gid, lsproduct_code, lscustomerbranch_name, lsproductcode, lscustomercontact_names;
        string lstmpquotationgid;
        string lsquotationdtl_gid;
        string lsquotation_gid;
        string lsproductgroup_gid;
        string lsproductgroup;
        string lsproduct_gid;
        string lsproduct_name;
        string lsproductname_gid;
        string lsproductname;
        string lsproduct_price;
        string lsuom_gid;
        string lsvendor_gid;
        string lsqty_quoted;
        string lsdiscount_percentage, lsproductgroup_name, lsdiscount_amount, lstax_name, lsslno, lstax_amount, lstax2_gid, lstax3_gid, lstax_amount2, lstax_amount3;
        string lsuom, lstax1_gid, lstax_amount1;
        string lsunitprice;
        string lsquantity;
        string lsdiscountpercentage;
        string lsdiscountamount;
        string lstax_name1;
        string lscustomerproduct_code;
        string lstax_name2;
        string lstax_name3;
        string lstaxamount_1;
        string lstaxamount_2;
        string lstaxamount_3;
        string lstotalamount;
        string lssono, lsprice;
        string lsdisplay_field, lslocalmarginpercentage, lslocalsellingprice, lsuom_name, lsreqdate_remarks, lsrequired_date;
        public void DaGetPmrTrnPurchaseQuotation(MdlPmrTrnPurchaseQuotation values)
        {
            try
            {
                

                msSQL = "SELECT a.quotation_gid, a.quotation_referenceno1, a.branch_gid, a.vendor_companyname,DATE_FORMAT(a.quotation_date,'%d-%m-%Y') as quotation_date,b.vendor_gid, " +
            " CONCAT(a.contactperson_name, '/ ', a.vendor_address, '/ ', a.email_id, '/ ', a.contact_telephonenumber) AS contact, " +
            " CONCAT(d.user_firstname) as created_by,a.quotation_status, a.total_amount, a.grandtotal_l, a.qo_type " +
           " FROM pmr_trn_tquotation a " +
            " left join hrm_mst_Temployee c on c.employee_gid = a.created_by " +
            " left join adm_mst_tuser d on c.user_gid = d.user_gid " +
            "LEFT JOIN acp_mst_tvendor b ON b.vendor_gid = a.vendor_gid " +
            "WHERE 1 = 1 GROUP BY a.quotation_gid ORDER BY a.quotation_gid desc ";



                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<quotation_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new quotation_list
                        {

                            quotation_gid = dt["quotation_gid"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            quotation_referenceno1 = dt["quotation_referenceno1"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contact = dt["contact"].ToString(),
                            qo_type = dt["qo_type"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            grandtotal_l = dt["grandtotal_l"].ToString(),
                            quotation_status = dt["quotation_status"].ToString(),

                        });
                        values.quotation_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting purchase quotation!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
             $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
             ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
             msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
             DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        public void DaGetVendor(MdlPmrTrnPurchaseQuotation values)
        {
            try
            {
                

                msSQL = " select a.vendorregister_gid, a.vendor_companyname from acp_mst_tvendorregister a" +
                    " left join adm_mst_taddress b on a.address_gid = b.address_gid " +
             " where a.active_flag = 'Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Vendor_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Vendor_list1
                        {
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            vendor_gid = dt["vendorregister_gid"].ToString(),
                        });
                        values.Vendor_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting vendor!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
             $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
             ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
             msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
             DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        public void DaGetOnChangeVendor(string vendorregister_gid, MdlPmrTrnPurchaseQuotation values)
        {

            try
            {
                  

                msSQL = "  select a.vendorregister_gid,a.contactperson_name,a.contact_telephonenumber,a.email_id,b.country_gid,b.address1,b.address2,c.country_name,b.city,b.state, b.postal_code,b.branch_name " +
             " from acp_mst_tvendorregister a " +
              " left join adm_mst_taddress b on a.address_gid=b.address_gid " +
             " left join adm_mst_tcountry c on c.country_gid=b.country_gid " +
             " where a.vendorregister_gid='" + vendorregister_gid + "' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetVendordtl>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetVendordtl
                        {

                            vendorregister_gid = dt["vendorregister_gid"].ToString(),

                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            email_id = dt["email_id"].ToString(),



                        });
                        values.GetVendordtl = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing vendor!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
             $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
             ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
             msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
             DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             

        }
        public void DaGetCurrencyDtl(MdlPmrTrnPurchaseQuotation values)
        {
            try
            {
                msSQL = " select distinct a.currencyexchange_gid,a.currency_code,a.default_currency,a.exchange_rate from  crm_trn_tcurrencyexchange a " +
  " left join acp_mst_tvendor b on a.currencyexchange_gid = b.currencyexchange_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL, "crm_trn_tcurrencyexchange");

                var getModuleList = new List<GetCurrencyCodeDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCurrencyCodeDropdown

                        {
                            currency_code = dt["currency_code"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            default_currency = dt["default_currency"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),

                        });
                        values.GetCurrencyCodeDropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting currency!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
             $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
             ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
             msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
             DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }

        public void DaGetOnchangeCurrency(string currencyexchange_gid, MdlPmrTrnPurchaseQuotation values)
        {
            try
            {
                 
                msSQL = " select currencyexchange_gid,currency_code,exchange_rate from crm_trn_tcurrencyexchange " +
            " where currencyexchange_gid='" + currencyexchange_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOnchangecurrency>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOnchangecurrency
                        {

                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                        });
                        values.GetOnchangecurrency = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on chaning currency!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
             $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
             ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
             msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
             DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }


        public void DaPostAddProduct(string employee_gid, summaryprod_list values)

        {
            try
            {                

                msGetGid = objcmnfunctions.GetMasterGID("PQDC");
                msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "Select productgroup_gid from pmr_mst_tproductgroup where productgroup_name='" + values.productgroup_name + "'";
                string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + values.tax_name + "'";
                string lstaxname = objdbconn.GetExecuteScalar(msSQL);             

                msSQL = "select percentage from acp_mst_ttax where tax_gid='" + values.tax_name + "'";
                string lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                
                int i = 0;

                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                  " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                  " WHERE product_gid='" + lsproductgid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                objOdbcDataReader.Read();
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

                msSQL = " insert into pmr_tmp_tquotationdtl( " +
                   " quotationdtl_gid," +
                   " quotation_gid," +
                   " employee_gid," +
                   " product_gid," +
                   " productgroup_gid," +
                   " productgroup_name," +
                   " product_name," +                   
                   " product_price," +
                   " qty_quoted," +
                   " uom_gid," +
                   " uom_name," +
                   " price," +
                   " tax_name," +                   
                   " tax_amount," +                 
                   " tax_percentage, " +
                   " tax1_gid, " +
                   " discount_percentage," +
                   " discount_amount," +                
                   " order_type, " +
                   " product_code " +
                   ")values(" +
                   "'" + msGetGid + "'," +
                   "'" + values.quotation_gid + "'," +
                   "'" + employee_gid + "'," +
                   "'" + lsproductgid + "'," +
                   "'" + lsproductgroupgid + "'," +
                   "'" + values.productgroup_name + "'," +
                   "'" + values.product_name + "'," +                  
                   "'" + values.selling_price + "'," +
                   "'" + values.qty_quoted + "'," +
                   "'" + lsproductuomgid + "'," +
                   "'" + values.productuom_name + "'," +
                   "'" + values.totalamount + "'," +
                   "'" + lstaxname + "'," +                  
                   "'" + values.tax_amount + "'," +               
                   "'" + lspercentage1 + "'," +                      
                   "'" + values.tax_name + "'," +                   
                   "'" + values.discount_percentage + "'," +
                   "'" + values.discount_amount + "'," +                        
                   "'" + lsquotation_type + "', " +
                   " '" + values.product_code + "')";
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
                values.message = "Exception occured while adding product in purchase quotation!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
             $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
             ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
             msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
             DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }
        public void DaGetTempProductsSummary(string employee_gid, MdlPmrTrnPurchaseQuotation values)
        {
            try
            {
                
                double total_amount = 0.00;
                double ltotalamount = 0.00;

                msSQL = " Select quotationdtl_gid,quotation_gid,product_gid,productgroup_gid,productgroup_name ,product_code ," +
                   " product_name,FORMAT(product_price,2)AS product_price ,qty_quoted,format(discount_percentage,2)as discount_percentage ," +
                   " format(discount_amount,2)as discount_amount ,format(tax_percentage,2)as tax_percentage ,format(tax_amount,2)as tax_amount ," +
                   " product_remarks,vendor_gid,uom_gid,uom_name,payment_days,delivery_period,format(price,2)as price,display_field," +
                   " product_status,tax_name,tax_name2,tax_name3,format(tax_percentage2,2)as tax_percentage2 ,format(tax_percentage3,2)as tax_percentage3 ," +
                   " format(tax_amount2,2)as tax_amount2 ,format(tax_amount3,2) as tax_amount3 ,salesorder_refno,salesorder_status,salesorder_flag " +
                   " from pmr_tmp_tquotationdtl   " +
                   " where employee_gid='" + employee_gid + "'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<tempsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        total_amount += double.Parse(dt["price"].ToString());
                        ltotalamount += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new tempsummary_list
                        {
                            quotationdtl_gid = dt["quotationdtl_gid"].ToString(),
                            quotation_gid = dt["quotation_gid"].ToString(),
                            //customerproduct_code = dt["customerproduct_code"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            //product_requireddate = dt["product_requireddate"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            //selling_price = dt["selling_price"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            //tax_gid = dt["tax1_gid"].ToString(),
                            //tax2_gid = dt["tax2_gid"].ToString(),
                            //tax3_gid = dt["tax3_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            //product_requireddateremarks = dt["productrequireddate_remarks"].ToString(),
                            //slno = dt["slno"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            price = dt["price"].ToString()


                        });
                        values.prodsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                values.total_amount = total_amount;
                values.ltotalamount = ltotalamount;
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
             $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
             ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
             msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
             DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             

        }


        //Overalsubmit

        public void DaPostDirectQuotation(string employee_gid,string user_gid, post_list values)
        {
            try
            {
                 

                msSQL = " select vendor_companyname from acp_mst_tvendorregister where vendorregister_gid='" + values.vendor_companyname + "'";
                string lsvendor_companyname = objdbconn.GetExecuteScalar(msSQL);

                string uiDateStr2 = values.quotation_date;
                DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string quotation_date = uiDate2.ToString("yyyy-MM-dd");

             


                msGetGid = objcmnfunctions.GetMasterGID("PQNP","",user_gid);
                if (msGetGid == "E")
                {
                    values.status = true;
                    values.message = "Cannot able to generate Unique ID";
                }
                else
                {

                    msSQL = " insert  into pmr_trn_tquotation (" +
                   " quotation_gid ," +
                   " branch_gid ," +
                   " quotation_date," +
                   " vendor_companyname," +
                   " contactperson_name," +
                   " vendor_address," +
                   " email_id," +
                   " contact_telephonenumber, " +
                   " created_by," +
                   " quotation_referencenumber," +
                   " quotation_remarks," +
                   " quotation_referenceno1, " +
                   " payment_days, " +
                   " delivery_days, " +
                   " total_amount, " +
                   " termsandconditions, " +
                   " quotation_status, " +
                   " addon_charge, " +
                   " additional_discount, " +
                   " grandtotal_l, " +
                   " currency_code, " +
                   " currency_gid, " +
                   " exchange_rate, " +
                   " shipping_to, " +
                   " qo_type " +
                   ")values(" +

                   " '" + msGetGid + "'," +
                    " '" + values.branch_name + "'," +
                    " '" + quotation_date + "'," +
                    " '" + lsvendor_companyname + "'," +
                    " '" + values.contactperson_name + "'," +
                    " '" + values.vendor_address + "'," +
                    " '" + values.email_id + "'," +
                    " '" + values.contact_telephonenumber + "'," +
                    " '" + employee_gid + "'," +
                    " '" + values.quotation_referencenumber + "'," +
                    " '" + values.quotation_remarks + "'," +
                    " '" + values.quotation_referenceno1 + "'," +
                    " '" + values.payment_days + "'," +
                    " '" + values.delivery_days + "'," +
                    " '" + values.producttotalamount + "'," +
                    " '" + values.termsandconditions + "'," +
                    " 'Approved'," +
                    " '" + values.addon_charge + "'," +
                    " '" + values.additional_discount + "'," +
                    " '" + values.grandtotal + "'," +
                    " '" + values.currency_code + "'," +
                    " '" + values.currency_gid + "'," +
                    " '" + values.exchange_rate + "'," +
                    " '" + values.shipping_to + "'," +
                    " 'Services')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error occurred while Insertion";
                    }

                  else

                    msSQL = " select " +
                    " quotationdtl_gid," +
                    " quotation_gid," +
                    " product_gid," +
                    " productgroup_gid," +
                    " productgroup_name," +
                    " product_code," +
                    " product_name," +
                    " display_field, " +
                    " product_price," +
                    " qty_quoted," +
                    " discount_percentage," +
                    " discount_amount," +
                    " uom_gid," +
                    " uom_name," +
                    " price," +
                    " tax_name," +
                    " tax_name2," +
                    " tax_name3," +
                    " tax1_gid, " +
                    " tax2_gid, " +
                    " tax3_gid, " +
                    " tax_amount," +
                    " tax_amount2," +
                    " tax_amount3 from pmr_tmp_tquotationdtl" +
                    " where employee_gid='" + employee_gid + "'";


                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<post_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {

                            lsquotationdtl_gid = dt["quotationdtl_gid"].ToString();
                            lsquotation_gid = dt["quotation_gid"].ToString();
                            lsproduct_gid = dt["product_gid"].ToString();
                            lsproductgroup_gid = dt["productgroup_gid"].ToString();
                            lsproductgroup_name = dt["productgroup_name"].ToString();
                            lsproduct_name = dt["product_name"].ToString();
                            lsproduct_code = dt["product_code"].ToString();
                            lsdisplay_field = dt["display_field"].ToString();
                            lsproduct_price = dt["product_price"].ToString();
                            lsqty_quoted = dt["qty_quoted"].ToString();
                            lsdiscount_percentage = dt["discount_percentage"].ToString();
                            lsdiscount_amount = dt["discount_amount"].ToString();
                            lsuom_gid = dt["uom_gid"].ToString();
                            lsuom_name = dt["uom_name"].ToString();
                            lsprice = dt["price"].ToString();
                            lstax_name = dt["tax_name"].ToString();
                            lstax_name2 = dt["tax_name2"].ToString();
                            lstax_name3 = dt["tax_name3"].ToString();
                            lstax_amount = dt["tax_amount"].ToString();
                            lstax2_gid = dt["tax2_gid"].ToString();
                            lstax3_gid = dt["tax2_gid"].ToString();
                            lstax_amount = dt["tax_amount"].ToString();
                            lstax_amount2 = dt["tax_amount2"].ToString();
                            lstax_amount3 = dt["tax_amount3"].ToString();


                        }

                        mssalesorderGID1 = objcmnfunctions.GetMasterGID("PQDC", "", user_gid);
                        if (mssalesorderGID1 == "E")
                        {
                            values.status = true;
                            values.message = "Cannot able to generate Unique ID";
                        }
                        else
                        {

                            msSQL = " insert into pmr_trn_tquotationdtl (" +
                            " quotationdtl_gid ," +
                            " quotation_gid," +
                            " product_gid ," +
                            " productgroup_gid," +
                            " productgroup_name," +
                            " product_code," +
                            " product_name," +
                            " display_field," +
                            " product_price," +
                            " qty_quoted," +
                            " discount_percentage," +
                            " discount_amount," +
                            " uom_gid," +
                            " uom_name," +
                            " price," +
                            " tax_name," +
                            " tax_name2," +
                            " tax_name3," +
                            " tax1_gid," +
                            " tax2_gid," +
                            " tax3_gid," +
                            " tax_amount," +
                            " tax_amount2," +
                            " tax_amount3 " +
                            ")values(" +
                            " '" + mssalesorderGID1 + "'," +
                            " '" + msGetGid + "'," +
                            " '" + lsproductname_gid + "'," +
                            " '" + lsproductgroup_gid + "'," +
                            " '" + lsproductgroup_name + "'," +
                            " '" + lsproduct_code + "'," +
                            " '" + lsproduct_name + "'," +
                            " '" + lsdisplay_field + "'," +
                            " '" + lsproduct_price + "'," +
                            " '" + lsqty_quoted + "'," +
                            " '" + lsdiscount_percentage + "'," +
                            " '" + lsdiscount_amount + "'," +
                            " '" + lsuom_gid + "'," +
                            " '" + lsuom_name + "'," +
                            " '" + lsprice + "'," +
                            " '" + lstax_name + "'," +
                            " '" + lstax_name2 + "'," +
                            " '" + lstax_name3 + "'," +
                            " '" + lstax1_gid + "'," +
                            " '" + lstax2_gid + "'," +
                             " '" + lstax3_gid + "'," +
                            " '" + lstax_amount + "'," +
                             " '" + lstax_amount2 + "'," +
                             " '" + lstax_amount3 + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0 || mnResult == 0)
                            {
                                msSQL = " delete from pmr_tmp_tquotationdtl ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            if (mnResult != 0)
                            {

                                values.status = true;

                                values.message = "Quotation raised successfully";


                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error occurred while submitting";
                            }

                        }




                    }


                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while submitting Quotation!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
             $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
             ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
             msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
             DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }



        public void GetDeleteDirectPOProductSummary(string quotationdtl_gid, quotationPO_list values)
        {
            try
            {
                

                msSQL = " delete from pmr_tmp_tquotationdtl " +
    " where quotationdtl_gid='" + quotationdtl_gid + "'";
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
                values.message = "Exception occured while Deleting product in purchase quotation!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
             $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
             ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
             msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
             DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            

        }



        public void DaGetTermsandConditions(MdlPmrTrnPurchaseQuotation values)
        {
            try
            {
                
                msSQL = "select a.termsconditions_gid,a.template_name, a.template_content from pmr_trn_ttermsconditions a "; 
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTandCDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTandCDropdown
                        {
                            template_gid = dt["termsconditions_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            termsandconditions = dt["template_content"].ToString(),
                            //payment_terms = dt["payment_terms"].ToString()
                        });
                        values.GetTermsandConditions = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting terma and condition in purchase quotation!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
             $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
             ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
             msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
             DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
             
        }
        public void DaGetOnChangeTerms(string termsconditions_gid, MdlPmrTrnPurchaseQuotation values)
        {
            try
            {
                 
                if (termsconditions_gid != null)
                {
                    msSQL = "  select termsconditions_gid, template_name,template_content,payment_terms  from pmr_trn_ttermsconditions  where termsconditions_gid='" + termsconditions_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetTermDropdown>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetTermDropdown
                            {
                                template_gid = dt["termsconditions_gid"].ToString(),
                                template_name = dt["template_name"].ToString(),
                                termsandconditions = dt["template_content"].ToString(),
                                payment_terms = dt["payment_terms"].ToString()
                            });
                            values.terms_list = getModuleList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while on changing terms and condition in purchase quotation!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
             $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
             ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
             msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
             DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
    }
}
