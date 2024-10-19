using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Web;
using static ems.sales.Models.MdlSalesOrder360;

namespace ems.sales.DataAccess
{
    public class DaSalesOrder360
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msINGetGID, msGetinGid, msSQL1, msSQL2 = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, msgetlead2campaign_gid,lsstage,employee,lsso, lspercentage, lstaxgid, start_date, end_date, lspercentage1, lsdesignation_code, lstaxname2, lsorder_type, lsproduct_type, lsproductgid1, lstaxname1, lsdiscountpercentage, lsdiscountamount, lsprice, lstype1, lsproduct_price, mssalesorderGID, mssalesorderGID1, mscusconGetGID, lscustomer_name, msGetCustomergid, lscustomer_gid, msGetGid2, msGetGid3, lsCode, msPOGetGID, msGetGID, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;


        public void DaGetBranchDtl(MdlSalesOrder360 values)
        {
            try
            {



                msSQL = "select branch_gid,branch_name from hrm_mst_tbranch";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranchDropdownSOCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchDropdownSOCRM

                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),

                        });
                        values.GetBranchSOCRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Branch Dropdown !";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        public void DaGetCustomerDtlCRM(MdlSalesOrder360 values, string leadbank_gid)
        {
            try
            {



                msSQL = "Select customer_gid from crm_trn_tleadbank where leadbank_gid='" + leadbank_gid + "' ";
                string lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "Select a.customer_gid, a.customer_name " +
                " from crm_mst_tcustomer a where customer_gid='" + lscustomer_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCustomerDropdownSOCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomerDropdownSOCRM

                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),

                        });
                        values.GetCustomerSOCRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Customer dropdown CRM !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetPersonDtl(MdlSalesOrder360 values)
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
                var getModuleList = new List<GetPersonDropdownSOCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPersonDropdownSOCRM

                        {
                            user_gid = dt["user_gid"].ToString() + '.' + dt["campaign_title"].ToString() + '.' + dt["campaign_gid"].ToString(),
                            user_name = dt["employee_name"].ToString(),

                        });
                        values.GetPersonSOCRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Person Dropdown !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetTax1Dtl(MdlSalesOrder360 values)
        {
            try
            {



                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxoneDropdownSOCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxoneDropdownSOCRM

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax1_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()
                        });
                        values.GetTax1SOCRM = getModuleList;
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

        public void DaGetTax4DtlSOCRM(MdlSalesOrder360 values)
        {
            try
            {



                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxFourDropdownSOCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxFourDropdownSOCRM

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name4 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTax4SOCRM = getModuleList;
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

        // Product Drop down CRM
        public void DaGetProductNamDtlCRM(MdlSalesOrder360 values)
        {
            try
            {



                msSQL = "Select product_gid, product_name from pmr_mst_tproduct where status='1'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductNamDropdownSOCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductNamDropdownSOCRM

                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                        values.GetProductNameSOCRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product Name dropdown CRM !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetTermsandConditions(MdlSalesOrder360 values)
        {
            try
            {

                msSQL = "  select c.template_gid, c.template_name, c.template_content from adm_mst_ttemplate c ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTandCDropdownSOCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTandCDropdownSOCRM
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            termsandconditions = dt["template_content"].ToString()
                        });
                        values.GetTermsandConditionsSOCRM = getModuleList;
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

        public void DaGetCustomerOnchangeCRM(string customer_gid, MdlSalesOrder360 values)
        {
            try
            {


                if (customer_gid != null)
                {
                    msSQL = " select a.customercontact_gid,concat(a.address1,'   ',a.city,'   ',a.state) as address1,ifnull(a.address2,'') as address2,ifnull(a.city,'') as city, " +
                    " ifnull(a.state,'') as state,ifnull(a.country_gid,'') as country_gid,ifnull(a.zip_code,'') as zip_code, " +
                    " ifnull(a.mobile,'') as mobile,a.email,ifnull(b.country_name,'') as country_name,a.customerbranch_name,concat(customerbranch_name,' | ',a.customercontact_name) as " +
                    " customercontact_names, c.customer_gid " +
                    " from crm_mst_tcustomercontact a " +
                    " left join crm_mst_tcustomer c on a.customer_gid=c.customer_gid " +
                    " left join adm_mst_tcountry b on a.country_gid=b.country_gid " +
                    " where c.customer_gid='" + customer_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetCustomerDetSOCRM>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetCustomerDetSOCRM
                            {
                                customercontact_names = dt["customercontact_names"].ToString(),
                                branch_name = dt["customerbranch_name"].ToString(),
                                country_name = dt["country_name"].ToString(),
                                customer_email = dt["email"].ToString(),
                                customer_mobile = dt["mobile"].ToString(),
                                zip_code = dt["zip_code"].ToString(),
                                country_gid = dt["country_gid"].ToString(),
                                state = dt["state"].ToString(),
                                city = dt["city"].ToString(),
                                address2 = dt["address2"].ToString(),
                                customer_address = dt["address1"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),

                            });
                            values.GetCustomerdetSOCRM = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Customer Onchange CRM  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetOnchangeCurrency(string currencyexchange_gid, MdlSalesOrder360 values)
        {
            try
            {

                msSQL = " select currencyexchange_gid,currency_code,exchange_rate from crm_trn_tcurrencyexchange " +
                " where currencyexchange_gid='" + currencyexchange_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOnchangeCurrencySOCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOnchangeCurrencySOCRM
                        {

                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                        });
                        values.GetOnchangecurrencySOCRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Currency !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        //onchange CRM
        public void DaGetOnChangeProductsNameCRM(string customercontact_gid,string product_gid, MdlSalesOrder360 values)
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

                        var getModuleList = new List<getproductsCodeSOCRM>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new getproductsCodeSOCRM
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    unitprice = lsproduct_price,

                                });
                                values.ProductsCodesSOCRM = getModuleList;
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

                        var getModuleList = new List<getproductsCodeSOCRM>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new getproductsCodeSOCRM
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    unitprice = dt["cost_price"].ToString(),

                                });
                                values.ProductsCodesSOCRM = getModuleList;
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


        public void GetOnChangeProductsNamesCRM(string product_gid, MdlSalesOrder360 values)

        {
            try
            {
                msSQL = " Select a.product_name, a.product_code,a.cost_price,a.mrp_price," +
                    " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                     " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                    " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                " where a.product_gid='" + product_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<getproductsCodeSOCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new getproductsCodeSOCRM
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            unitprice = dt["cost_price"].ToString(),

                        });
                        values.ProductsCodesSOCRM = getModuleList;
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


        //product add//

        public void DaPostOnAdds(string employee_gid, salesorders_listSOCRM values)
        {
            try
            {


                msGetGid = objcmnfunctions.GetMasterGID("VSDT");

                //tmpsalesorderdtl_gid = msGetGid;

                msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);


                if (values.discount_percentage == null || values.discount_percentage == "")
                {
                    lsdiscountpercentage = "0.00";
                }
                else
                {
                    lsdiscountpercentage = values.discount_percentage;
                }

                if (values.discountamount == null || values.discountamount == "")
                {
                    lsdiscountamount = "0.00";
                }
                else
                {
                    lsdiscountamount = values.discountamount;
                }


                if (values.tax_gid == null || values.tax_gid == "")
                {
                    msSQL = "select tax_gid from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                   lstaxgid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select percentage from acp_mst_ttax where tax_gid='" + lstaxgid + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
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
                        lsorder_type = "Sales";
                    }
                    else
                    {
                        lsorder_type = "Services";
                    }

                }
                objOdbcDataReader.Close();

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

        //product summary//
        public void DaGetSalesOrdersummary(string employee_gid, MdlSalesOrder360 values)
        {
            try
            {


                double grand_total = 0.00;
                double grandtotal = 0.00;

                msSQL = "SELECT a.tmpsalesorderdtl_gid,a.quotation_gid, a.tax_name, a.tax_amount, a.salesorderdtl_gid, a.salesorder_gid, a.product_gid, a.productgroup_gid, " +
                   " a.productgroup_name, a.product_name, FORMAT(a.product_price, 2) AS product_price, b.product_code, a.qty_quoted, a.product_remarks, " +
                   " a.uom_gid, a.vendor_gid, a.slno, a.uom_name, FORMAT(a.price, 2) AS price, " +
                   " FORMAT(a.discount_percentage,2) AS discount_percentage, FORMAT(a.discount_amount,2) AS discount_amount, " +
                   " FORMAT(a.selling_price, '0.00') AS selling_price " +
                   " FROM smr_tmp_tsalesorderdtl a " +
                   " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                   " LEFT JOIN acp_mst_tvendor e ON a.vendor_gid = e.vendor_gid " +
                   " WHERE a.employee_gid = '" + employee_gid + "' group by a.product_gid, b.delete_flag = 'N' and a.quotation_gid is null" +
                   " ORDER BY a.slno ASC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<salesorders_listSOCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["price"].ToString());
                        grandtotal += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new salesorders_listSOCRM
                        {
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            slno = dt["slno"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            discountpercentage = double.Parse(dt["discount_percentage"].ToString()),
                            //productgroup_name = dt["productgroup_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            quantity = double.Parse(dt["qty_quoted"].ToString()),
                            uom_gid = dt["uom_gid"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            producttotalamount = dt["price"].ToString(),
                            totalamount = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            grand_total = dt["price"].ToString(),
                            grandtotal = dt["price"].ToString(),



                        });
                        values.salesorderslistSOCRM = getModuleList;
                        values.grand_total = grand_total;
                        values.grandtotal = grandtotal;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaGetOnChangeTerms(string template_gid, MdlSalesOrder360 values)
        {
            try
            {

                if (template_gid != null)
                {
                    msSQL = " select template_gid, template_name, template_content from adm_mst_ttemplate where template_gid='" + template_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetTermDropdownSOCRM>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetTermDropdownSOCRM
                            {
                                template_gid = dt["template_gid"].ToString(),
                                template_name = dt["template_name"].ToString(),
                                termsandconditions = dt["template_content"].ToString(),
                            });
                            values.terms_listSOCRM = getModuleList;
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

        // Overall Submit

        public void DaPostSalesOrder(string employee_gid, string user_gid, postsales_listSOCRM values)
        {
            try
            {
                string totalvalue = values.user_name;

                string[] separatedValues = totalvalue.Split('.');

                // Access individual components
                string USERGid = separatedValues[0];
                string campaignTitle = separatedValues[1];
                string campaignGid = separatedValues[2];
                msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + values.tax_name4 + "'";
                string lstaxname = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid='" + values.currency_code + "'";
                string currency_code = objdbconn.GetExecuteScalar(msSQL);

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

                    string lslocaladdon = values.addon_charge;
                string lslocaladditionaldiscount = values.additional_discount;
                string lslocalgrandtotal = values.grandtotal;
                string lsgst = "0.00";
                    //string lsproducttotalamount = "0.00";

                    double totalAmount = double.Parse(values.totalamount);
                    double addonCharges = double.TryParse(values.addon_charge, out double addonChargesValue) ? addonChargesValue : 0;
                    double freightCharges = double.TryParse(values.freight_charges, out double freightChargesValue) ? freightChargesValue : 0;
                    double packingCharges = double.TryParse(values.packing_charges, out double packingChargesValue) ? packingChargesValue : 0;
                    double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                    double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                    double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                    double buybackCharges = double.TryParse(values.buyback_charges, out double buybackChargesValue) ? buybackChargesValue : 0;

                    double grandTotal = (totalAmount + addonCharges + freightCharges + packingCharges + insuranceCharges + roundoff) - additionaldiscountAmount - buybackCharges;

                    string lsrefno = objcmnfunctions.GetMasterGID("SOR");
                    
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
                         " tax_amount, " +
                         " gst_amount," +
                         " total_price," +
                         " total_amount," +
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
                         "created_date" +
                         " )values(" +
                         " '" + mssalesorderGID + "'," +
                         " '" + values.branch_name + "'," +
                         " '" + salesorder_date + "'," +
                         " '" + values.customer_gid + "'," +
                         " '" + lscustomername + "'," +
                         " '" + values.customercontact_gid + "'," +
                         " '" + lscustomerbranch + "'," +
                         " '" + values.customercontact_names + "'," +
                         " '" + values.customer_address + "'," +
                         " '" + values.customer_email + "'," +
                         " '" + values.customer_mobile + "'," +
                         " '" + employee_gid + "'," +                        
                         " '" + lsrefno + "'," +
                         " '" + values.so_remarks + "'," +
                         " '" + values.payment_days + "'," +
                         " '" + values.delivery_days + "'," +
                         " '" + values.grandtotal.Replace(",", "") + "'," +
                         " '" + values.termsandconditions + "'," +
                         " 'Approved',";
                if (values.addon_charge != "")
                {
                    msSQL += "'" + values.addon_charge + "',";
                }
                else
                {
                    msSQL += "'" + lslocaladdon + "',";
                }
                if (values.additional_discount != "")
                {
                    msSQL += "'" + values.additional_discount + "',";
                }
                else
                {
                    msSQL += "'" + lslocaladditionaldiscount + "',";
                }
                if (values.addon_charge != "")
                {
                    msSQL += "'" + values.addon_charge + "',";
                }
                else
                {
                    msSQL += "'" + lslocaladditionaldiscount + "',";
                }
                if (values.additional_discount != "")
                {
                    msSQL += "'" + values.additional_discount + "',";
                }
                else
                {
                    msSQL += "'" + lslocaladditionaldiscount + "',";
                }
                msSQL += " '" + lslocalgrandtotal.Replace(",", "") + "'," +
                     " '" + currency_code + "'," +
                     " '" + values.currency_code + "'," +
                     " '" + values.exchange_rate + "'," +
                     " '" + values.shipping_to + "'," +
                     "'" + values.freight_terms + "'," +
                     "'" + values.payment_terms + "'," +
                     " '" + values.tax_name4 + "'," +
                     " '" + lstaxname + "', " +
                     " '" + values.tax_amount4 + "', " +
                     " '" + lsgst + "'," +
                     " '" + values.totalamount.Replace(",", "") + "'," +
                     " '" + values.totalamount.Replace(",", "") + "'," +
                     " '" + values.vessel_name + "'," +
                     " '" + USERGid + "'," +
                     " '" + start_date + "'," +
                     " '" + end_date + "',";
                if (values.roundoff == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.roundoff + "',";
                }
                if (values.addon_charge == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.addon_charge + "',";
                }
                if (values.additional_discount == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.additional_discount + "',";
                }
                if (values.freight_charges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.freight_charges + "',";
                }
                if (values.buyback_charges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.buyback_charges + "',";
                }
                if (values.packing_charges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.packing_charges + "',";
                }
                if (values.insurance_charges == "")
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += "'" + values.insurance_charges + "',";
                }
                msSQL += " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = " Some Error Occurred While Inserting Salesorder Details";
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
                           " so_referenceno1, " +
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
                           " '" + values.branch_name + "'," +
                           " '" + salesorder_date + "'," +
                           " '" + values.customer_gid + "'," +
                           " '" + lscustomername + "'," +
                           " '" + values.customercontact_gid + "'," +
                           " '" + lscustomerbranch + "'," +
                           " '" + values.customercontact_names + "'," +
                           " '" + values.customer_address + "'," +
                           " '" + values.customer_email + "'," +
                           " '" + values.customer_mobile + "'," +
                           " '" + employee_gid + "'," +                          
                           " '" + values.so_remarks + "'," +
                          " '" + lsrefno + "'," +
                           " '" + values.payment_days + "'," +
                           " '" + values.delivery_days + "'," +
                           " '" + values.grandtotal + "'," +
                           " '" + values.termsandcondition + "'," +
                           " 'Approved',";
                    if (values.addon_charge == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.addon_charge + "',";
                    }
                    if (values.additional_discount == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.additional_discount + "',";
                    }
                    msSQL += "'" + lslocaladdon + "'," +
                           " '" + lslocaladditionaldiscount + "'," +
                           " '" + lslocalgrandtotal + "'," +
                           " '" + values.currencyexchange_gid + "'," +
                           " '" + values.currency_code + "'," +
                           " '" + values.exchange_rate + "',";
                    if (values.addon_charge == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.addon_charge + "',";
                    }
                    if (values.additional_discount == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.additional_discount + "',";
                    }
                    msSQL += "'" + values.shipping_to + "'," +
                           " '" + lscampaign_gid + "'," +
                           " '" + values.vessel_name + "',";
                    if (values.roundoff == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.roundoff + "',";
                    }
                    msSQL += "'" + values.user_name + "',";
                    if (values.freight_charges == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.freight_charges + "',";
                    }
                    if (values.buyback_charges == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.buyback_charges + "',";
                    }
                    if (values.packing_charges == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.packing_charges + "',";
                    }
                    if (values.insurance_charges == "")
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.insurance_charges + "')";
                    }
                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult2 != 1)
                    {
                        values.status = false;
                        values.message = " Error Occurs While Inserting in Customer Details";
                        return;
                    }

                }

                msSQL = " select " +
                        " tmpsalesorderdtl_gid," +
                        " salesorder_gid," +
                        " product_gid," +
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
                        " tax1_gid, " +
                        " tax_amount," +
                        " slno," +
                        " tax_percentage," +
                        " order_type " +
                        " from smr_tmp_tsalesorderdtl" +
                        " where employee_gid='" + employee_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<postsales_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new postsales_list
                        {
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),                            
                            productuom_name = dt["uom_name"].ToString(),
                            unitprice = dt["product_price"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            discountpercentage = dt["discount_percentage"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),                           
                            totalamount = dt["price"].ToString(),                            
                            order_type = dt["order_type"].ToString(),
                            slno = dt["slno"].ToString()                          

                        });



                        int i = 0;

                        mssalesorderGID1 = objcmnfunctions.GetMasterGID("VSDC");

                        if (mssalesorderGID1 == "E")
                        {
                                values.status = false;
                                values.message = "Create Sequence code for VSDC ";
                                return;
                        }



                            msSQL = " insert into smr_trn_tsalesorderdtl (" +
                                 " salesorderdtl_gid ," +
                                 " salesorder_gid," +
                                 " product_gid ," +
                                 " product_name," +
                                 " product_code," +
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
                                 " type " +
                                 ")values(" +
                                 " '" + mssalesorderGID1 + "'," +
                                 " '" + mssalesorderGID + "'," +
                                 " '" + dt["product_gid"].ToString() + "'," +
                                 " '" + dt["product_name"].ToString() + "'," +
                                 " '" + dt["product_code"].ToString() + "'," +
                                 " '" + dt["product_price"].ToString() + "'," +
                                 " '" + dt["qty_quoted"].ToString() + "'," +
                                 " '" + dt["discount_percentage"].ToString() + "'," +
                                 " '" + dt["discount_amount"].ToString() + "'," +
                                 " '" + dt["tax_amount"].ToString() + "'," +
                                 " '" + dt["uom_gid"].ToString() + "'," +
                                 " '" + dt["uom_name"].ToString() + "'," +
                                 " '" + dt["price"].ToString() + "'," +
                                 " '" + dt["tax_name"].ToString() + "'," +
                                 " '" + dt["tax1_gid"].ToString() + "'," +
                                 " '" + i + 1 + "'," +
                                 " '" + dt["tax_percentage"].ToString() + "'," +
                                  " '" + dt["order_type"].ToString() + "')";
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
                         " type, " +
                         " salesorder_refno" +
                         ")values(" +
                         " '" + mssalesorderGID1 + "'," +
                         " '" + mssalesorderGID + "'," +
                         " '" + dt["product_gid"].ToString() + "'," +                        
                         " '" + dt["product_name"].ToString() + "'," +                       
                         " '" + dt["product_price"].ToString() + "'," +
                         " '" + dt["qty_quoted"].ToString() + "'," +
                         " '" + dt["discount_percentage"].ToString() + "'," +
                         " '" + dt["discount_amount"].ToString() + "'," +
                         " '" + dt["tax_amount"].ToString() + "'," +
                         " '" + dt["uom_gid"].ToString() + "'," +
                         " '" + dt["uom_name"].ToString() + "'," +
                         " '" + dt["price"].ToString() + "'," +
                         " '" + dt["tax_name"].ToString() + "'," +                       
                         " '" + dt["tax1_gid"].ToString() + "'," +                         
                         " '" + values.slno + "'," +                     
                         " '" + dt["tax_percentage"].ToString() + "'," +                  
                         " '" + dt["order_type"].ToString() + "', " +
                         " '" + values.salesorder_refno + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }




                msSQL = "select distinct type from smr_trn_tsalesorderdtl where salesorder_gid='" + mssalesorderGID + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                  
                if (objOdbcDataReader.HasRows == true)
                {

                    lsorder_type = "Sales";


                }

                else
                {
                    lsorder_type = "Service";
                }

                    objOdbcDataReader.Close();



                    msSQL = " update smr_trn_tsalesorder set so_type='" + lsorder_type + "' where salesorder_gid='" + mssalesorderGID + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = " update acp_trn_torder set so_type='" + lsorder_type + "' where salesorder_gid='" + mssalesorderGID + "' ";
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


                        if (mnResult == 0)
                        {
                            values.status = false;
                        }


                        msSQL = "select approval_flag from smr_trn_tapproval where submodule_gid='SMRSROSOA' and soapproval_gid='" + mssalesorderGID + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == false)
                        {
                            
                            msSQL = "update smr_trn_tsalesorder set salesorder_status='Approved',salesorder_remarks='" + values.so_remarks + "', " +
                                   " approved_by='" + employee_gid + "', approved_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where salesorder_gid='" + mssalesorderGID + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = "update acp_trn_torder set salesorder_status='Approved',salesorder_remarks='" + values.so_remarks + "', " +
                                  " approved_by='" + employee_gid + "', approved_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where salesorder_gid='" + mssalesorderGID + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                           
                        }
                        else
                        {
                            msSQL = "select approved_by from smr_trn_tapproval where submodule_gid='SMRSROSOA' and soapproval_gid='" + mssalesorderGID + "'";
                            objOdbcDataReader1 = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader1.RecordsAffected == 1)
                            {
                                
                                msSQL = " update smr_trn_tapproval set " +
                               " approval_flag = 'Y', " +
                               " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                               " where approved_by = '" + employee_gid + "'" +
                               " and soapproval_gid = '" + mssalesorderGID + "' and submodule_gid='SMRSROSOA'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                msSQL = "update smr_trn_tsalesorder set salesorder_status='Approved',salesorder_remarks='" + values.so_remarks + "', " +
                                   " approved_by='" + employee_gid + "', approved_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where salesorder_gid='" + mssalesorderGID + "' ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                msSQL = "update acp_trn_torder set salesorder_status='Approved',salesorder_remarks='" + values.so_remarks + "', " +
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

                        if (mnResult != 0)
                        {


                            string lsstage = "8";
                             msgetlead2campaign_gid = objcmnfunctions.GetMasterGID("BLCC");
                             lsso = "N";

                            msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + USERGid + "'";
                             employee = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = " Insert into crm_trn_tenquiry2campaign ( " +
                                                              " lead2campaign_gid, " +
                                                              " salesorder_gid, " +
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

                        if(mnResult !=0)
                        {
                            msSQL = " select leadbank_gid from crm_mst_tcustomer where customer_gid='" + values.customer_gid + "'";
                            string leadbank = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = " update crm_trn_tlead2campaign  set " +
                                                " leadstage_gid='8'" +
                                                " where leadbank_gid='" + leadbank + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                        if (mnResult != 0 || mnResult == 0)
                        {

                            msSQL = " delete from smr_tmp_tsalesorderdtl " +
                                    " where employee_gid='" + employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }


                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Sales Order Successfully Raised";
                            return;
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Raising Sales Order";
                            return;
                        }
                    
                    }

                    else
                    { 

                        values.status = true;
                        values.message = "Select one Product to Raise Enquiry";
                        return;
                    }

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

        public void GetDeleteDirectSOProductSummary(string tmpsalesorderdtl_gid, salesorders_listSOCRM values)
        {
            try
            {



                msSQL = "select price from smr_tmp_tsalesorderdtl " +
                    " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows == true)

                {
                    lsprice = objOdbcDataReader["price"].ToString();
                }
                objOdbcDataReader.Close();
                msSQL = " delete from smr_tmp_tsalesorderdtl " +
                        " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "'";
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
                values.message = "Exception occured while deleting DirectSO product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetSalesorderdetail(string product_gid, MdlSalesOrder360 values)
        {
            try
            {

                msSQL = " select a.salesorder_gid,a.salesorderdtl_gid,a.product_gid,b.currency_code,a.product_requireddate as product_requireddate, " +
                    " d.product_name,date_format(c.salesorder_date, '%d-%m-%Y') as salesorder_date, " +
                    " b.customer_gid,b.customer_name,a.qty_quoted,format(a.price, 2) as product_price" +
                    " from smr_trn_tsalesorderdtl a left join smr_trn_tsalesorder b on a.salesorder_gid = b.salesorder_gid" +
                    " left join pmr_mst_tproduct d on a.product_gid = d.product_gid" +
                    " left join acp_trn_torder c on a.salesorder_gid = c.salesorder_gid" +
                    " where a.product_gid = '" + product_gid + "' group by a.price  order by c.salesorder_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Directeddetailslist2SOCRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Directeddetailslist2SOCRM
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            currency_code = dt["currency_code"].ToString(),

                        });
                        values.DirecteddetailslistSOCRM = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Detail !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // DIRECT SALES ORDER PRODCUCT SUMMARY EDIT
        public void DaGetDirectSalesOrderEditProductSummary(string tmpsalesorderdtl_gid, MdlSalesOrder360 values)
        {
            try
            {
                msSQL = " Select tmpsalesorderdtl_gid, salesorder_gid, product_gid,product_name, product_price, qty_quoted,tax1_gid, discount_percentage," +
                    " discount_amount, tax_amount, tax_name, uom_gid, uom_name, price, product_code from smr_tmp_tsalesorderdtl" +
                    " where tmpsalesorderdtl_gid = '" + tmpsalesorderdtl_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<DirecteditSalesorderListsocrm>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new DirecteditSalesorderListsocrm
                        {
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            unitprice = dt["product_price"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            totalamount = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_gid = dt["tax1_gid"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),

                        });
                        values.directeditsalesorder_listsocrm = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Product Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // PRODUCT SUMMARY UPDATE FOR DIRECT SALES ORDER

        public void DaPostUpdateDirectSOProduct(string employee_gid, DirecteditSalesorderListsocrm values)
        {
            try
            {
                msSQL = "Select tax_gid from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                string TaxGID = objdbconn.GetExecuteScalar(msSQL);

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

                msSQL = "Select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);
                if (values.tax_gid == null)
                {
                    msSQL = "Select percentage from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    msSQL = "Select tax_name from acp_mst_ttax where tax_gid='" + values.tax_gid + "'";
                    values.tax_name = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "Select percentage from acp_mst_ttax where tax_name='" + values.tax_name + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }
                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                 " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                 " WHERE product_gid='" + lsproductgid1 + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows == true)
                {
                    lsproduct_type = "Sales";

                }
                else
                {
                    lsproduct_type = "Services";
                }
                objOdbcDataReader.Close();
                
                msSQL = " select * from smr_tmp_tsalesorderdtl where product_gid='" + lsproductgid1 + "' and uom_gid='" + lsproductuomgid + "' " +
                        " and product_price='" + values.unitprice + "' and tax1_gid='" + values.tax_name + "'  and employee_gid='" + employee_gid + "' " +
                        " and tmpsalesorderdtl_gid = '" + values.tmpsalesorderdtl_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
                if (objOdbcDataReader.HasRows == true)
                {
                    msSQL = " update smr_tmp_tsalesorderdtl set qty_quoted='" + Convert.ToDouble(values.quantity) + Convert.ToDouble(objOdbcDataReader["qty_quoted"].ToString()) + "' " +
                            " price='" + Convert.ToDouble(values.totalamount) + Convert.ToDouble(objOdbcDataReader["price"].ToString()) + "' " +
                            " where tmpsalesorderdtl_gid='" + values.tmpsalesorderdtl_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " delete from smr_tmp_tsalesorderdtl where tmpsalesorderdtl_gid='" + values.tmpsalesorderdtl_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                
                else
                {
                    msSQL = " update smr_tmp_tsalesorderdtl set " +
                           " product_gid = '" + lsproductgid1 + "'," +
                           " product_name= '" + values.product_name + "'," +
                           " product_price='" + values.unitprice + "'," +
                           " qty_quoted='" + values.quantity + "'," +
                           " margin_percentage='" + values.discount_percentage + "'," +
                           " margin_amount='" + values.discountamount + "'," +
                           " uom_gid = '" + lsproductuomgid + "', " +
                           " uom_name='" + values.productuom_name + "'," +
                           " price='" + values.totalamount + "'," +
                           " employee_gid='" + employee_gid + "'," +
                           " tax_name= '" + values.tax_name + "'," +
                           " tax1_gid= '" + TaxGID + "'," +
                           " tax_amount='" + values.tax_amount + "'," +
                           " order_type='" + lsproduct_type + "', " +
                           " tax_percentage='" + lspercentage1 + "'" +
                           " where tmpsalesorderdtl_gid='" + values.tmpsalesorderdtl_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                objOdbcDataReader.Close();
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = " Product Updated Successfully! ";

                }
                else
                {
                    values.status = false;
                    values.message = " Error While Updating Product! ";

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating DirectSO Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

    }
}