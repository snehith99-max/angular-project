using ems.sales.Models;
using ems.utilities.Functions;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;
using System;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
namespace ems.sales.DataAccess
{
    public class DaSmrQuotation360CRM
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
        int lsamendcount;
        DataTable mail_datatable, dt_datatable;
        double taxAmount, reCalTotalAmount, reCalTaxAmount, subtotal, reCaldiscountAmount;

        string msEmployeeGID, lsemployee_gid, lsenquiry_type, start_date, end_date, lsentity_code, lsquotationgid, lsproductgid1, lstaxname1, TempSOGID, SalesOrderGID, msGetSalesOrderGID, lstaxname, lstaxamount, lspercentage1, lscustomer_gid, lsproduct_price, msGetTempGID, lsquotation_type, lsdesignation_code, lstaxname2, lstaxname3, lsamount2, lsamount3, lspercentage2, lspercentage3, lscustomer_code, pricingsheet_refno, roundoff, mssalesorderGID, lsCode, msGetGid, msGetGid1, msgetGid2, msgetGid4, lstype1, lshierarchy_flag, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, lspop_port;
        string msGetCustomergid, msconGetGID;
        string lscustomer_name;
        string lscontact_person, lscustomercontact_gid, lscustomerbranch_name, lscustomercontact_names,lstaxgid;
        string lstmpquotationgid;
        string lsproductgroup_gid;
        string lsproductgroup;
        string lsproductname_gid;
        string lsproductname;
        string lsuom_gid;
        string lsvendor_gid;
        string lsuom;
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
        MailMessage message = new MailMessage();

        //

        public void DaGetBranchQCRM(MdlQuotation360CRM values)
        {
            try
            {
                msSQL = "select branch_gid,branch_name from hrm_mst_tbranch";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranchDropDownQuoteCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchDropDownQuoteCRM

                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),

                        });
                        values.GetBranchQCRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Branch !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // CURRENCY DROP DOWN FOR QUOTATION 360
        public void DaGetCurrencyQCRM(MdlQuotation360CRM values)
        { 
            try
            {
                msSQL = " select currencyexchange_gid,currency_code,default_currency,exchange_rate " +
                        " from crm_trn_tcurrencyexchange order by currency_code asc";                
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCurrencyDropdownQuoteCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCurrencyDropdownQuoteCRM

                        {
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            default_currency = dt["default_currency"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),

                        });
                        values.GetCurrencyQCRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Currency!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // PRODUCT DROPDOWM FOR 360 QUOTATION
        public void DaGetProductNameQCRM(MdlQuotation360CRM values)
        {
            try
            {
                msSQL = "Select product_gid, product_name from pmr_mst_tproduct where status='1'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductDropdownQuoteCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductDropdownQuoteCRM

                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                        values.GetProductDropdown_QCRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

            catch (Exception ex)
            {

                values.message = "Exception occured while loading Product  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // PRODUCT SUMMARY TAX DROPDOWN
        public void DaGetTax1QCRM(MdlQuotation360CRM values)
        {
            try
            {
                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxDropdownQuoteCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxDropdownQuoteCRM

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax1_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()
                        });
                        values.GetTaxQCRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // OVERALL SUMMARY TAX DROPDOWN
        public void DaGetTax2QCRM(MdlQuotation360CRM values)
        {
            try
            {
                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxDropdown2QuoteCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxDropdown2QuoteCRM

                        {
                            tax_gid = dt["tax_gid"].ToString(),                            
                            tax_name4 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()
                        });
                        values.GetTax2QCRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // TERMS AND CONDITIONS FOR QUOTATION CRM 360
        public void DaGetTermsandConditionsQCRM(MdlQuotation360CRM values)
        {
            try
            {
                msSQL = "  select c.template_gid, c.template_name, c.template_content from adm_mst_ttemplate c ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTermsDropdownQuoteCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTermsDropdownQuoteCRM
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            termsandconditions = dt["template_content"].ToString()
                        });
                        values.GetTermsQCRM = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Template Name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        //CUSTOMER DROPDOWN FOR QUOTATION 360
        public void DaGetCustomerQCRM(string leadbank_gid, MdlQuotation360CRM values)
        {
            try
            {

                msSQL = "Select customer_gid from crm_trn_tleadbank where leadbank_gid='" + leadbank_gid + "' ";
                string lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "Select a.customer_gid, a.customer_name " +
                " from crm_mst_tcustomer a where customer_gid='" + lscustomer_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCustomerDropDownQuoteCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomerDropDownQuoteCRM

                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),

                        });
                        values.GetCustomerQCRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Customer Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // SALES PERSON DROP DOWN FOR QUOTATION 360
        public void DaGetSalesPersonQCRM(MdlQuotation360CRM values)
        {
            try
            {
                msSQL = " select a.employee_gid,c.user_gid,e.campaign_gid,concat(e.campaign_title, ' | ', c.user_code, ' | ', c.user_firstname, ' ', c.user_lastname)AS employee_name, e.campaign_title " +
                        " from adm_mst_tmodule2employee a " +
                        " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                        " left join adm_mst_tuser c on b.user_gid=c.user_gid " +
                        " left join smr_trn_tcampaign2employee d on a.employee_gid=d.employee_gid " +
                        " left join smr_trn_tcampaign e on e.campaign_gid = d.campaign_gid " +
                        " where a.module_gid = 'SMR' and a.hierarchy_level<>'-1' and a.employee_gid in  " +
                        " (select employee_gid from smr_trn_tcampaign2employee where 1=1) group by employee_name asc; ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<GetSalesDropdownQuoteCRM>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                              getModuleList.Add(new GetSalesDropdownQuoteCRM

                              {
                                  user_gid = dt["user_gid"].ToString() + '.' + dt["campaign_title"].ToString() + '.' + dt["campaign_gid"].ToString(),
                                  user_name = dt["employee_name"].ToString(),
                               });
                              values.GetSalesPersonQCRM = getModuleList;
                        }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading User Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        // ON CHANGE CUSTOMER NAME FOR QUOTATION 360
        public void DaGetOnChangeCustomerQCRM(string customercontact_gid, MdlQuotation360CRM values)
        {
            try
            {
                if (customercontact_gid != null)
                {
                    msSQL = " select a.customercontact_gid,concat(a.address1,'   ',a.city,'   ',a.state) as address1,ifnull(a.address2,'') as address2,ifnull(a.city,'') as city, " +
                    " ifnull(a.state,'') as state,ifnull(a.country_gid,'') as country_gid,ifnull(a.zip_code,'') as zip_code, " +
                    " ifnull(a.mobile,'') as mobile,a.email,ifnull(b.country_name,'') as country_name,a.customerbranch_name,concat(a.customercontact_name) as " +
                    " customercontact_names, c.customer_gid " +
                    " from crm_mst_tcustomercontact a " +
                    " left join crm_mst_tcustomer c on a.customer_gid=c.customer_gid " +
                    " left join adm_mst_tcountry b on a.country_gid=b.country_gid " +
                    " where c.customer_gid='" + customercontact_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetCustomerOnChangeQuoteCRM>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetCustomerOnChangeQuoteCRM
                            {
                                customercontact_names = dt["customercontact_names"].ToString(),
                                branch_name = dt["customerbranch_name"].ToString(),
                                country_name = dt["country_name"].ToString(),
                                email = dt["email"].ToString(),
                                mobile = dt["mobile"].ToString(),
                                zip_code = dt["zip_code"].ToString(),
                                country_gid = dt["country_gid"].ToString(),
                                state = dt["state"].ToString(),
                                city = dt["city"].ToString(),
                                address2 = dt["address2"].ToString(),
                                address1 = dt["address1"].ToString(),
                                customercontact_gid = dt["customercontact_gid"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),

                            });
                            values.GetCustomerOnChange_QCRM = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Update Customer Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // ON CHANGE PRODUCT NAME FOR QUOTATION 360
        public void DaGetOnChangeProductNameQCRM(string product_gid, string customercontact_gid, MdlQuotation360CRM values)
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

                        var getModuleList = new List<GetProductOnChangeQuoteCRM>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetProductOnChangeQuoteCRM
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    unitprice = lsproduct_price,

                                });
                                values.GetProductOnChange_QCRM = getModuleList;
                            }
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

                        var getModuleList = new List<GetProductOnChangeQuoteCRM>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetProductOnChangeQuoteCRM
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    unitprice = dt["cost_price"].ToString(),

                                });
                                values.GetProductOnChange_QCRM = getModuleList;
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


        public void DaGetOnChangeProductNamesQCRM(string product_gid, MdlQuotation360CRM values)

        {
            try
            {
                msSQL = " Select a.product_name, a.product_code,a.cost_price,a.mrp_price," +
                    " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                     " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                    " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                " where a.product_gid='" + product_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetProductOnChangeQuoteCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductOnChangeQuoteCRM
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            unitprice = dt["cost_price"].ToString(),

                        });
                        values.GetProductOnChange_QCRM = getModuleList;
                    }
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


        // TERMS AND CONDITIONS ON CHANGE EVENT FOR QUOTATION 360
        public void DaGetOnChangeTermsQCRM(string template_gid, MdlQuotation360CRM values)
        {
            try
            {
                if (template_gid != null)
                {
                    msSQL = " select template_gid, template_name, template_content from adm_mst_ttemplate where template_gid='" + template_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetOnChangeTermsQuoteCRM>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetOnChangeTermsQuoteCRM
                            {
                                template_gid = dt["template_gid"].ToString(),
                                template_name = dt["template_name"].ToString(),
                                termsandconditions = dt["template_content"].ToString(),
                            });
                            values.GetOnChangeTerms_QCRM = getModuleList;
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

        // PRODUCT ADD FOR QUOTATION 360
        public void DaPostAddProductQCRM(string employee_gid,  Quote360Product values)
        {
            try
            {
                msGetGid1 = objcmnfunctions.GetMasterGID("VQDT");

                msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select tax_gid from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                string lstaxgid = objdbconn.GetExecuteScalar(msSQL);

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
                    msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + lstaxgid + "'";
                    lstaxname = objdbconn.GetExecuteScalar(msSQL);
                }

                if (values.tax_name == null || values.tax_name == "")
                {
                    lspercentage1 = "0.00";
                }
                else
                {
                    msSQL = "select percentage from acp_mst_ttax where tax_gid='" + lstaxgid + "'";
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
                         " taxsegment_gid, " +
                        " taxsegmenttax_gid, " +
                        " tax_name, " +
                        " tax1_gid, " +
                        " slno, " +
                        " quotation_type, " +
                        " tax_percentage," +
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
                        "'" + values.taxsegment_gid + "', " +
                        "'" + values.taxsegmenttax_gid + "', " +                    
                        "'" + lstaxname + "', " +
                        "'" + lstaxgid + "', " +
                         "'" + i + 1 + "', " +
                        "'" + lsquotation_type + "'," +
                        "'" + lspercentage1 + "'," +
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

        //
        public void DaGetQCRMTempProductSummary(string employee_gid, MdlQuotation360CRM values)
        {
            try
            {
                double total_amount = 0.00;
                double ltotalamount = 0.00;

                msSQL = " Select a.tmpquotationdtl_gid, a.slno,a.tax_name,a.enquiry_gid,a.tax1_gid,a.tax_amount, " +
                    " d.productuom_name,a.quotationdtl_gid,a.quotation_gid,a.product_gid,a.product_name,  " +
                    " format(a.product_price,2) as product_price ,a.product_code,a.qty_quoted,a.product_remarks,a.uom_gid,  " +
                    " a.uom_name,format(a.price,2) as price , format(a.discount_percentage,2)as discount_percentage," +
                    " format(a.discount_amount,2)as discount_amount from smr_tmp_treceivequotationdtl a  " +
                    " left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                    " left join pmr_mst_tproductgroup c on a.productgroup_gid=c.productgroup_gid  " +
                    " left join pmr_mst_tproductuom d on b.productuom_gid=d.productuom_gid " +
                    " left join acp_mst_tvendor e on a.vendor_gid=e.vendor_gid  " +
                    " where a.created_by='" + employee_gid + "' and b.delete_flag='N'and a.enquiry_gid is null order by a.slno asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Quote360Product>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        total_amount += double.Parse(dt["price"].ToString());
                        ltotalamount += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new Quote360Product
                        {
                            tmpquotationdtl_gid = dt["tmpquotationdtl_gid"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            unitprice = dt["product_price"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            tax_gid = dt["tax1_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            slno = dt["slno"].ToString(),
                            discountpercentage = dt["discount_percentage"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            price = dt["price"].ToString()
                        });
                        values.Quotation360Product_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                values.total_amount = total_amount;
                values.ltotalamount = ltotalamount;



            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Temproary Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // PRODUCT SUMMARY EDIT FOR QUOTATION 360
        public void DaGetQuotation360EditProductSummary(string tmpquotationdtl_gid, MdlQuotation360CRM values)
        {
            try
            {
                msSQL = " Select tmpquotationdtl_gid,quotation_gid,product_gid,productgroup_gid,tax1_gid," +
                    " productgroup_name,product_name,qty_quoted,discount_percentage,discount_amount,product_price," +
                    " tax_percentage,tax_amount,uom_gid,uom_name,price,tax_name,product_code from smr_tmp_treceivequotationdtl" +
                    " where tmpquotationdtl_gid = '" + tmpquotationdtl_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Quote360Product>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Quote360Product
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
                            tax_gid = dt["tax1_gid"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),

                        });
                        values.Quotation360Product_list = getModuleList;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  DirectQuotationEditProductSummary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // 
        public void DaUpdateQuotationProductCRM(string employee_gid, Quote360Product values)
        {
            try
            {

                if (values.product_gid != null)
                {
                    lsproductgid1 = values.product_gid;
                    msSQL = "Select product_name from pmr_mst_tproduct where product_gid='" + lsproductgid1 + "'";
                    values.product_name = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    msSQL = " Select product_gid from pmr_mst_tproduct where product_name = '" + values.product_name + "'";
                    lsproductgid1 = objdbconn.GetExecuteScalar(msSQL);
                }
                if (values.tax_gid == null || values.tax_gid == "")
                {
                    msSQL = "Select percentage from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    msSQL = "Select percentage from acp_mst_ttax where tax_gid='" + values.tax_gid + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }

                msSQL = "Select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                        " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                        " WHERE product_gid='" + lsproductgid1 + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows == true)
                {
                    lsenquiry_type = "Sales";
                }

                else
                {
                    lsenquiry_type = "Service";
                }
                objOdbcDataReader.Close();


                msSQL = " select * from smr_tmp_treceivequotationdtl where product_gid='" + lsproductgid1 + "' and uom_gid='" + lsproductuomgid + "'" +
                        " and product_price='" + values.unitprice + "'" +
                        "  and created_by='" + employee_gid + "' " +
                        " and discount_percentage='" + values.discountpercentage + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    
                    msSQL = " update smr_tmp_treceivequotationdtl set qty_quoted='" + Convert.ToDouble(values.quantity) + "', " +
                            " price='" + Convert.ToDouble(values.totalamount) + "'," +
                             " tax_name= '" + values.tax_name + "'," +
                           " tax_amount='" + values.tax_amount + "'" +
                            " where tmpquotationdtl_gid='" + values.tmpquotationdtl_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                   
                }
                else
                {
                    msSQL = " update smr_tmp_treceivequotationdtl set " +
                           " product_gid = '" + lsproductgid1 + "'," +
                           " product_code='" + values.product_code + "' ," +
                           " product_name= '" + values.product_name + "'," +
                           " product_price='" + values.unitprice + "'," +
                           " qty_quoted='" + values.quantity + "'," +
                           " discount_percentage='" + values.discountpercentage + "'," +
                           " discount_amount='" + values.discountamount + "'," +
                           " uom_gid = '" + lsproductuomgid + "', " +
                           " uom_name='" + values.productuom_name + "'," +
                           " price='" + values.totalamount + "'," +
                           " created_by='" + employee_gid + "'," +
                           " tax_name= '" + values.tax_name + "'," +
                           " tax_amount='" + values.tax_amount + "'," +
                           " tax_percentage='" + lspercentage1 + "'" +
                           " where tmpquotationdtl_gid='" + values.tmpquotationdtl_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                objOdbcDataReader.Close();
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = " Product Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = " Error While Updating Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // DELETE PRODUCT EVENT FOR QUOTATION 360
        public void DaDeleteQCRMProductSummary(string tmpquotationdtl_gid, Quote360Product values)
        {
            try
            {
                msSQL = "select price from smr_tmp_treceivequotationdtl " +
                    " where tmpquotationdtl_gid='" + tmpquotationdtl_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)

                {
                  
                    lsprice = objOdbcDataReader["price"].ToString();
                    
                }
                objOdbcDataReader.Close();

                msSQL = " delete from smr_tmp_treceivequotationdtl " +
                        " where tmpquotationdtl_gid='" + tmpquotationdtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)

                {
                    values.status = true;
                    values.message = "Product Deleted Successfully!";
                    return;

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting The Product!";
                    return;

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Deleting The Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // POST DIRECT QUOTATION FOR 360
        public void DaPostDirectQuotationCRM(string employee_gid, string user_gid, PostQuoteCRM_List values)
        {
            try
            {
                string totalvalue = values.user_name;

                string[] separatedValues = totalvalue.Split('.');

                // Access individual components
                string USERGid = separatedValues[0];
                string campaignTitle = separatedValues[1];
                string campaignGid = separatedValues[2];

                msSQL = " select * from smr_tmp_treceivequotationdtl " +
                     " where created_by='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("VQDC");
                    if (msGetGid == "E")
                    {
                        values.status = false;
                        values.message = "Create Sequence Code VQDC for Raise Enquiry";
                        return;
                    }

                    if (values.tax_amount == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_amount + "',";
                    }


                    if (msGetGid == "New Ref.No")
                    {
                        msGetGid = ("quotation_gid");
                    }
                    else
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("VQDC");

                    }
                    msSQL = " select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid=  '" + values.currency_code + "'";
                    string lscurrencycode = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select customercontact_gid from crm_mst_tcustomercontact where customer_gid=  '" + values.customer_gid + "'";
                    string lscustomercontactgid = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = " select tax_name from acp_mst_ttax where tax_gid=  '" + values.tax_name4 + "'";
                    string taxname = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select leadbank_gid from crm_trn_tleadbank where customer_gid='" + values.customer_gid + "'";
                    string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);

                    lscustomercontact_names = values.customercontact_names;
                    string lsquotation_status = "Approved";
                    string lsadditional_discount_l = values.additional_discount;
                    string lsaddon_charge_l = values.addoncharge;
                    string lsgrandtotal_l = values.grandtotal;
                    string lsgst_percentage = "0.00";

                    string uiDateStr = values.quotation_date;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string quotation_date = uiDate.ToString("yyyy-MM-dd");

                    double totalAmount = double.Parse(values.total_amount);
                    double addonCharges = double.TryParse(values.addoncharge, out double addonChargesValue) ? addonChargesValue : 0;
                    double freightCharges = double.TryParse(values.freightcharges, out double freightChargesValue) ? freightChargesValue : 0;
                    double packingCharges = double.TryParse(values.packing_charges, out double packingChargesValue) ? packingChargesValue : 0;
                    double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                    double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                    double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                    double buybackCharges = double.TryParse(values.buybackcharges, out double buybackChargesValue) ? buybackChargesValue : 0;

                    double grandTotal = (totalAmount + addonCharges + freightCharges + packingCharges + insuranceCharges + roundoff) - additionaldiscountAmount - buybackCharges;

                    msSQL = " insert  into smr_trn_treceivequotation (" +
                             " quotation_gid ," +
                             " branch_gid ," +
                             " quotation_date," +
                             " customer_gid," +
                             " leadbank_gid," +
                             " customer_name," +
                             " customercontact_gid," +
                             " customer_contact_person," +
                             " created_by," +
                             " quotation_remarks," +
                             " quotation_referenceno1, " +
                             " payment_days, " +
                             " delivery_days, " +
                             " Grandtotal, " +
                             " termsandconditions, " +
                             " quotation_status, " +
                             " contact_no, " +
                             " customer_address, " +
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
                             " tax_gid," +
                             " salesperson_gid," +
                             " freight_terms, " +
                             " payment_terms," +
                             " tax_name," +                             
                             " roundoff, " +
                             " tax_amount, " +
                             " total_price, " +
                             " freight_charges," +
                             " buyback_charges," +
                             " packing_charges," +
                             " created_date ," +
                             " insurance_charges " +
                             ") values ( " +
                             " '" + msGetGid + "'," +
                             " '" + values.branch_name + "'," +
                             " '" + quotation_date + "'," +
                             " '" + values.customer_gid + "'," +
                             " '" + lsleadbank_gid + "'," +
                             " '" + values.customer_name + "'," +
                             " '" + lscustomercontactgid + "'," +
                             " '" + lscustomercontact_names + "'," +
                             " '" + employee_gid + "'," +
                             " '" + values.quotation_remarks + "'," +
                             " '" + values.quotation_referenceno1 + "'," +
                             " '" + values.payment_days + "'," +
                             " '" + values.delivery_days + "'," +
                             "'" + grandTotal.ToString().Replace(".", "") + "', " +
                             " '" + values.termsandconditions + "'," +
                             " '" + lsquotation_status + "'," +
                             " '" + values.mobile + "'," +
                             " '" + values.address1 + "'," +
                             " '" + values.email + "',";
                             if (values.addoncharge == "" || values.addoncharge == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.addoncharge + "',";
                    }
                    if (values.additional_discount == "" || values.additional_discount == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.additional_discount + "',";
                    }
                    msSQL +="'" + lsaddon_charge_l + "'," +
                             "'" + lsadditional_discount_l + "'," +
                             "'" + lsgrandtotal_l + "', " +
                             "'" + lscurrencycode + "'," +
                             "'" + values.exchange_rate + "'," +
                             "'" + values.currency_code + "',";
                    if (values.total_amount == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.total_amount + "',";
                    }
                    msSQL += "'" + lsgst_percentage + "', ";
                    if (values.tax_name4 == "" || values.tax_name4 == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_name4 + "',";
                    }
                   msSQL += "'" + USERGid + "'," +
                    "'" + values.freight_terms + "'," +
                    "'" + values.payment_terms + "',";
                   if(taxname == null || taxname == "")
                {
                        msSQL += "'--No Tax--', ";
                    }
                else
                    {
                        msSQL += "'" + taxname + "',";

                    }
                    if (values.roundoff == "" || values.roundoff == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.roundoff + "',";
                    }
                    if (values.tax_amount4 == "" || values.tax_amount4 == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_amount4 + "',";
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
                    if (values.packing_charges == "" || values.packing_charges == null )
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.packing_charges + "',";
                    }
                    msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
                    if (values.insurance_charges == "")
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
                        values.message = "Error Occured while inserting Quotation";
                        return;
                    }

                    else
                    {
                        msSQL = " select " +
                              " tmpquotationdtl_gid," +
                              " quotation_gid," +
                              " product_gid," +
                              " productgroup_gid," +
                              " productgroup_name," +
                              " product_name," +
                              " product_code," +
                              " product_price," +
                              " qty_quoted," +
                              " format(discount_percentage,2) as discount_percentage," +
                              " format(discount_amount,2) as discount_amount, " +
                              " uom_gid," +
                              " uom_name," +
                              " format(price,2) as price," +
                              " tax_name, " +                           
                              " slno, " +                    
                              " tax_amount, " +
                              " taxsegment_gid, " +
                              " taxsegmenttax_gid " +
                               " from smr_tmp_treceivequotationdtl  where created_by='" + employee_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<Post_List>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new Post_List
                                {

                                    tmpquotationdtl_gid = dt["tmpquotationdtl_gid"].ToString(),
                                    quotation_gid = dt["quotation_gid"].ToString(),
                                    product_gid = dt["product_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    customerproduct_code = dt["uom_name"].ToString(),
                                    product_name = dt["product_name"].ToString(),
                                    product_price = dt["product_price"].ToString(),
                                    quantity = dt["qty_quoted"].ToString(),
                                    discountpercentage = dt["discount_amount"].ToString(),
                                    discountamount = dt["discount_amount"].ToString(),
                                    productuom_gid = dt["uom_gid"].ToString(),
                                    productuom_name = dt["uom_gid"].ToString(),
                                    tax_name = dt["tax_name"].ToString(),
                                    slno = dt["slno"].ToString(),
                                    tax_amount = dt["tax_amount"].ToString(),
                                    price = dt["price"].ToString(),
                                });

                                msgetGid2 = objcmnfunctions.GetMasterGID("VQDC");
                                if (msgetGid2 == "E")
                                {
                                    values.status = true;
                                    values.message = "Create Sequence Code PPDC for Sales Enquiry Details";
                                    return;
                                }
                                else
                                {
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
                                            " taxsegment_gid," +
                                            " taxsegmenttax_gid," +
                                            " tax_name," +
                                            " slno," +
                                            " tax_amount " +
                                            ")values(" +
                                            " '" + msgetGid2 + "'," +
                                            " '" + msGetGid + "'," +
                                            " '" + dt["product_gid"].ToString() + "'," +
                                            " '" + dt["productgroup_gid"].ToString() + "'," +
                                            " '" + dt["productgroup_name"].ToString() + "'," +
                                            " '" + dt["product_name"].ToString() + "'," +
                                            " '" + dt["product_code"].ToString() + "'," +
                                            " '" + dt["product_price"].ToString() + "'," +
                                            " '" + dt["qty_quoted"].ToString() + "'," +
                                            " '" + dt["discount_percentage"].ToString() + "'," +
                                            " '" + dt["discount_amount"].ToString().Replace(",", "").Trim() + "'," +
                                            " '" + dt["uom_gid"].ToString() + "'," +
                                            " '" + dt["uom_name"].ToString() + "'," +
                                            " '" + dt["price"].ToString().Replace(",", "").Trim() + "'," +
                                            " '" + dt["taxsegment_gid"].ToString() + "'," +
                                            " '" + dt["taxsegmenttax_gid"].ToString() + "'," +
                                            " '" + dt["tax_name"].ToString() + "'," +
                                            " '" + dt_datatable.Rows.Count + "', ";
                                    msSQL += " '" + dt["tax_amount"].ToString() + "')";
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
                            objOdbcDataReader.Close();


                        }

                        msSQL = " update smr_trn_treceivequotation set quotation_type='" + lsquotation_type + "' where quotation_gid='" + msGetGid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " update smr_trn_treceivequotation Set " +
                    " leadbank_gid = '" + lsleadbank_gid + "'" +                    
                    " where quotation_gid='" + msGetGid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
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
                            objOdbcDataReader1 = objdbconn.GetDataReader(msSQL);
                        }
                        if (objOdbcDataReader1.RecordsAffected == 1)
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
                        else if (objOdbcDataReader1.RecordsAffected > 1)
                        {
                           
                            msSQL = " update smr_trn_tapproval set " +
                                   " approval_flag = 'Y', " +
                                   " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                   " where approved_by = '" + employee_gid + "'" +
                                   " and qoapproval_gid = '" + msGetGid + "' and submodule_gid='SMRSMRQAP'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                           
                        }
                        objOdbcDataReader.Close();
                        objOdbcDataReader1.Close();
                    }
                    if (mnResult != 0)
                    {
                        string lsstage = "4";
                      string  msgetlead2campaign_gid = objcmnfunctions.GetMasterGID("BLCC");
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
                    }
                    if (mnResult != 0)
                    {
                        msSQL = " select leadbank_gid from crm_mst_tcustomer where customer_gid='" + values.customer_gid + "'";
                        string leadbank = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " update crm_trn_tlead2campaign  set " +
                                            " leadstage_gid='4'" +
                                            " where leadbank_gid='" + leadbank + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }

                    if (mnResult == 0 || mnResult != 0)
                    {
                        msSQL = " delete from smr_tmp_treceivequotationdtl " +
                             " where created_by='" + employee_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }


                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Quotation Raised Successfully!";
                        return;
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
    }
}