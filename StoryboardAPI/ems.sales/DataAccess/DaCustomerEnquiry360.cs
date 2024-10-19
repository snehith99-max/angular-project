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

    public class DaCustomerEnquiry360
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        private OdbcDataReader objOdbcDataReader;

        DataTable dt_datatable;

        string  closure_date, product_requireddate, lsproductgid1, lsrefno, lscompany_code, EnquiryGID,  lsenquiry_type, lsleadbank_gid, lscampaign_gid, lspotential_value, lslead_status, lsleadstage,  msGetGid, msGetGid1, msgetlead2campaign_gid;
        int mnResult;
 

        // PRODUCT DROP DOWN FOR CUSTOMER ENQUIRY 360
        public void DaGetProductECRM(MdlCustomerEnquiry360 values)
        {
            try
            {
                msSQL = "select product_gid,product_name from pmr_mst_tproduct" +
                    " where status='1'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductDropDownECRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductDropDownECRM
                        {
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                        });
                        values.GetProductECRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product  Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // BRANCH DROP DOWN FOR CUSTOMER ENQUIRY FROM 360
        public void DaGetBranchECRM(MdlCustomerEnquiry360 values)
        {
            try
            {
                msSQL = "select branch_gid, branch_name from hrm_mst_tbranch ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranchDropDownECRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchDropDownECRM

                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),

                        });
                        values.GetBranchECRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Branch Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // CUSTOMER DROPDOWN FOR CUSTOMER ENQUIRY FROM 360
        public void DaGetCustomerECRM(MdlCustomerEnquiry360 values, string leadabank_gid)
        {
            try
            {
                msSQL = "Select customer_gid from crm_trn_tleadbank where leadbank_gid='" + leadabank_gid + "' ";
                string lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "Select a.customer_gid, a.customer_name " +
                " from crm_mst_tcustomer a where customer_gid='" + lscustomer_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCustomerDropDownECRM>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomerDropDownECRM

                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),

                        });
                        values.GetCustomerECRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // SALES PERSON DROP DOWN FOR CUSTOMER ENQUIRY FROM 360
        public void DaGetSalesPersonECRM(MdlCustomerEnquiry360 values)
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
                        var getModuleList = new List<GetSalesPersonDropDownECRM>();
                       if (dt_datatable.Rows.Count != 0)
                         {
                          foreach (DataRow dt in dt_datatable.Rows)
                            {
                              getModuleList.Add(new GetSalesPersonDropDownECRM
                              {
                                  employee_gid = dt["employee_gid"].ToString() + '.' + dt["campaign_title"].ToString() + '.' + dt["campaign_gid"].ToString(),
                                  user_firstname = dt["employee_name"].ToString(),
                                  campaign_gid = dt["campaign_gid"].ToString(),

                              });
                                values.GetSalesPersonECRM = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Employee Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // 

        public void DaGetOnChangeCustomerECRM(string customercontact_gid, MdlCustomerEnquiry360 values)
        {
            try
            {
                if (customercontact_gid != null)
                {
                    msSQL = " select a.customercontact_gid,concat(a.address1,'   ',a.city,'   ',a.state,'   ',a.zip_code) as address1,ifnull(a.address2,'') as address2,ifnull(a.city,'') as city, " +
                    " ifnull(a.state,'') as state,ifnull(a.country_gid,'') as country_gid,ifnull(a.zip_code,'') as zip_code, " +
                    " ifnull(a.mobile,'') as mobile,ifnull(a.email,'') as email,ifnull(b.country_name,'') as country_name,a.customerbranch_name,concat(a.customercontact_name) as " +
                    " customercontact_names, c.customer_gid " +
                    " from crm_mst_tcustomercontact a " +
                    " left join crm_mst_tcustomer c on a.customer_gid=c.customer_gid " +
                    " left join adm_mst_tcountry b on a.country_gid=b.country_gid " +
                    " where c.customer_gid='" + customercontact_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetOnchangeCustomerECRM>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetOnchangeCustomerECRM
                            {
                                customercontact_name = dt["customercontact_names"].ToString(),
                                customerbranch_name = dt["customerbranch_name"].ToString(),
                                country_name = dt["country_name"].ToString(),
                                contact_email = dt["email"].ToString(),
                                contact_number = dt["mobile"].ToString(),
                                zip_code = dt["zip_code"].ToString(),
                                country_gid = dt["country_gid"].ToString(),
                                state = dt["state"].ToString(),
                                city = dt["city"].ToString(),
                                address2 = dt["address2"].ToString(),
                                contact_address = dt["address1"].ToString(),                               
                                customer_gid = dt["customer_gid"].ToString(),

                            });
                            values.GetOnchangeCustomer_ECRM = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        //

        public void DaGetOnChangeProductECRM(string product_gid, MdlCustomerEnquiry360 values)
        {
            try
            {

                if (product_gid != null)
                {
                    msSQL = " Select a.product_name,a.product_gid, a.product_code, b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                         " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                        " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid  " +
                    " where a.product_gid='" + product_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetOnChangeProductECRM>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetOnChangeProductECRM
                            {
                                product_name = dt["product_name"].ToString(),
                                product_gid = dt["product_gid"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),
                                productuom_gid = dt["productuom_gid"].ToString(),
                                productgroup_gid = dt["productgroup_gid"].ToString(),

                            });
                            values.GetOnChangeProduct_ECRM = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // PRODUCT ADD FOR CUSTOMER ENQUIRY FROM CRM 360
        public void DaPostProductECRM(string user_gid, string employee_gid, ECRMProductSummary_list values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("PPDC");
                EnquiryGID = objcmnfunctions.GetMasterGID("PPTP");

                if (values.product_requireddate == null || values.product_requireddate == "")
                {
                    product_requireddate = "0000-00-00";
                }
                else
                {
                    string uiDateStr2 = values.product_requireddate;
                    DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    product_requireddate = uiDate2.ToString("yyyy-MM-dd");
                }

                msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name + "'";
                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "Select productgroup_gid from pmr_mst_tproductgroup where productgroup_name='" + values.productgroup_name + "'";
                string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                 " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                 " WHERE product_gid='" + lsproductgid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows == true)
                {
                    if (objOdbcDataReader["producttype_name"].ToString() != "Services")
                    {
                        lsenquiry_type = "Sales";
                    }
                    else
                    {
                        lsenquiry_type = "Services";
                    }

                }
               
                msSQL = " insert into smr_tmp_tsalesenquiry( " +
                        " tmpsalesenquiry_gid , " +
                        " enquiry_gid, " +
                        " productgroup_gid, " +
                        " product_gid, " +
                        " potential_value," +
                        " uom_gid," +
                        " qty_requested, " +
                        " created_by, " +
                        " product_requireddate," +
                        " enquiry_type) " +
                        " values( " +
                         "'" + msGetGid + "'," +
                         "'" + EnquiryGID + "'," +
                        "'" + lsproductgroupgid + "'," +
                        "'" + lsproductgid + "'," +
                        "'" + values.potential_value + "'," +
                        "'" + lsproductuomgid + "'," +
                        "'" + values.qty_requested + "', " +
                        "'" + employee_gid + "', " +
                        "'" + product_requireddate + "'," +
                        "'" + lsenquiry_type + "') ";
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
                values.message = "Exception occured while Adding Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // PRODUCT SUMMARY FOR CUSTOMER ENQUIRY FROM CRM 360
        public void DaGetProductSummaryECRM(string user_gid, string employee_gid, MdlCustomerEnquiry360 values)
        {
            try
            {

                msSQL = " select a.tmpsalesenquiry_gid,a.customerproduct_code,a.qty_requested,a.display_field, " +
                    " CASE WHEN a.product_requireddate = '0000-00-00' THEN '' ELSE DATE_FORMAT(a.product_requireddate, '%d-%m-%Y') " +
                    " END AS product_requireddate, " +
                    " d.productgroup_name,b.product_code,b.product_name,c.productuom_name,a.product_gid, " +
                    " format(a.potential_value,2)as potential_value,a.product_requireddateremarks " +
                    " from smr_tmp_tsalesenquiry a left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                    " left join pmr_mst_tproductuom c on a.uom_gid=c.productuom_gid " +
                    " left join pmr_mst_tproductgroup d on a.productgroup_gid= d.productgroup_gid" +
                    " where a.created_by='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ECRMProductSummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ECRMProductSummary_list
                        {
                            tmpsalesenquiry_gid = dt["tmpsalesenquiry_gid"].ToString(),                            
                            qty_requested = dt["qty_requested"].ToString(),
                            product_requireddate = dt["product_requireddate"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            potential_value = dt["potential_value"].ToString(),                         
                        });
                        values.ECRMProductSummarylist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // PRODUCT EDIT FOR CUSTOMER ENQUIRY FROM CRM 360
        public void DaEditProductSummaryECRM(string tmpsalesenquiry_gid, MdlCustomerEnquiry360 values)
        {
            try
            {
                msSQL = " Select a.tmpsalesenquiry_gid,a.enquiry_gid,a.product_gid,a.qty_requested,a.uom_gid,a.productgroup_gid,a.potential_value," +
                        " DATE_FORMAT(a.product_requireddate, '%d-%m-%Y') AS product_requireddate," +
                        " c.product_name, c.product_code,b.productgroup_name,d.productuom_name from smr_tmp_tsalesenquiry a" +
                        " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
                        " left join pmr_mst_tproduct c on a.product_gid = c.product_gid" +
                        " left join pmr_mst_tproductuom d on a.uom_gid = d.productuom_gid " +
                        " where a.tmpsalesenquiry_gid = '" + tmpsalesenquiry_gid + "' group by a.product_gid";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<ECRMProductSummary_list>();
                       if (dt_datatable.Rows.Count != 0)
                        {
                          foreach (DataRow dt in dt_datatable.Rows)
                           {
                             getModuleList.Add(new ECRMProductSummary_list
                              {
                                tmpsalesenquiry_gid = dt["tmpsalesenquiry_gid"].ToString(),
                                product_gid = dt["product_gid"].ToString(),
                                product_name = dt["product_name"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),
                                productgroup_gid = dt["productgroup_gid"].ToString(),
                                qty_requested = dt["qty_requested"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                potential_value = dt["potential_value"].ToString(),
                               product_requireddate = dt["product_requireddate"].ToString(),

                        });
                        values.ECRMProductSummarylist = getModuleList;
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

        // PRODUCT UPDATE FOR CUSTOMER ENQUIRY FROM CRM 360
        public void DaUpdateEnquiryProductECRM(string employee_gid, ECRMProductSummary_list values)
        {
            try
            {

                if (values.product_requireddate == null || values.product_requireddate == "")
                {
                    product_requireddate = "0000-00-00";
                }
                else
                {
                    string uiDateStr = values.product_requireddate;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    product_requireddate = uiDate.ToString("yyyy-MM-dd");
                }
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
                msSQL = "Select productgroup_gid from pmr_mst_tproductgroup where productgroup_name='" + values.productgroup_name + "'";
                string lsproductgroupgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "Select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " update smr_tmp_tsalesenquiry set " +
                        " productgroup_gid = '" + lsproductgroupgid + "', " +
                        " potential_value ='" + values.potential_value + "', " +
                        " qty_requested='" + values.qty_requested + "', " +
                        " uom_gid='" + lsproductuomgid + "'," +
                        " product_gid='" + lsproductgid1 + "'," +
                        " product_requireddate = '" + product_requireddate + "'," +
                        " created_by= '" + employee_gid + "'," +
                        " enquiry_type= '" + lsenquiry_type + "'" +
                        " where tmpsalesenquiry_gid='" + values.tmpsalesenquiry_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select qty_requested,display_field,customerproduct_code,potential_value,tmpsalesenquiry_gid " +
                       " from smr_tmp_tsalesenquiry where " +
                       " product_gid = '" + lsproductgid1 + "' and " +
                       " uom_gid='" + lsproductuomgid + "' and  " +
                       " created_by = '" + employee_gid + "' and" +
                       " enquiry_type='" + lsenquiry_type + "' and product_requireddate='" + product_requireddate + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string lsqtyrequested = dt["qty_requested"].ToString();
                        string lspotentialvalue = dt["potential_value"].ToString();

                        msSQL = " update smr_tmp_tsalesenquiry set " +
                                " qty_requested='" + lsqtyrequested + "', " +
                                " potential_value ='" + lspotentialvalue + "' " +
                                " where tmpsalesenquiry_gid='" + values.tmpsalesenquiry_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = " Product Updated Successfully ";
                    return;
                }
                else
                {
                    values.status = false;
                    values.message = " Error While Updating Product Details ";
                    return;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Product Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // DELETE PRODUCT SUMMARY FOR CUSTOMER ENQUIRY FROM CRM 360
        public void DaDeleteProductSummaryECRM(string tmpsalesenquiry_gid, ECRMProductSummary_list values)
        {
            try
            {

                msSQL = "delete from smr_tmp_tsalesenquiry where tmpsalesenquiry_gid='" + tmpsalesenquiry_gid + "'  ";
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
                values.message = "Exception occured while  Deleting Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // OVERALL SUBMIT FOR CUSTOMER ENQUIRY FROM CRM 360

        public void DaPostCustomerEnquiryECRM(string employee_gid, string user_gid, PostECRM_list values)
        {
            try
            {

                string totalvalue = values.user_firstname;

                string[] separatedValues = totalvalue.Split('.');

                // Access individual components
                string employeeGid = separatedValues[0];
                string campaignTitle = separatedValues[1];
                string campaignGid = separatedValues[2];

                msSQL = "SELECT * FROM smr_tmp_tsalesenquiry WHERE created_by='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count != 0)
                {


                    msGetGid1 = objcmnfunctions.GetMasterGID("PPTP");

                    if (msGetGid1 == "E") // Assuming "E" is a string constant
                    {
                        values.status = true;
                        values.message = "Create Sequence Code PPTP for Sales Enquiry Details";
                    }


                    msSQL = "SELECT DISTINCT " +
                        "a.product_gid, a.product_remarks, a.customerproduct_code, a.potential_value,a.created_by," +
                        "a.qty_requested, a.uom_gid, a.display_field,a.product_requireddate, a.product_requireddateremarks, " +
                        "a.productgroup_gid" +
                        " FROM smr_tmp_tsalesenquiry a WHERE" +
                        " a.created_by = '" + employee_gid + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("PPDC");

                            if (msGetGid == "E") 
                            {
                                values.status = true;
                                values.message = "Create Sequence Code PPDC for Sales Enquiry Details";
                            }

                            string lsnewproduct_flag = "Y";

                            msSQL = " Insert into smr_trn_tsalesenquirydtl " +
                                   " (enquirydtl_gid," +
                                   " enquiry_gid , " +                                   
                                   " product_gid," +
                                   " potential_value," +
                                   " uom_gid," +
                                   " productgroup_gid," +
                                   " qty_enquired, " +
                                   " created_by, " +
                                   " newproduct_flag, " +
                                   " product_requireddate) " +                                   
                                   " values (" +
                                   "'" + msGetGid + "'," +
                                   "'" + msGetGid1 + "'," +                                  
                                   "'" + dt["product_gid"].ToString() + "'," +
                                   "'" + dt["potential_value"].ToString() + "'," +
                                   "'" + dt["uom_gid"].ToString() + "'," +
                                   "'" + dt["productgroup_gid"].ToString() + "'," +
                                   "'" + dt["qty_requested"].ToString() + "', " +
                                   "'" + dt["created_by"].ToString() + "', " +
                                   "'" + lsnewproduct_flag + "', ";

                            if (dt["product_requireddate"].ToString() == null || dt["product_requireddate"].ToString() == "" || dt["product_requireddate"].ToString() == "undefined")
                            {
                                msSQL += "'0000-00-00')";
                            }
                            else
                            {
                                msSQL += "'" + Convert.ToDateTime(dt["product_requireddate"]).ToString("yyyy-MM-dd") + "') ";

                            }

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                            if (mnResult != 0)
                            {
                                msSQL = " Insert into acp_trn_tenquirydtl " +
                                    " (enquirydtl_gid," +
                                    " enquiry_gid , " +                                    
                                    " product_gid," +
                                    " potential_value," +
                                    " uom_gid," +
                                    " productgroup_gid," +
                                    " qty_enquired, " +
                                    " created_by, " +
                                    " product_requireddate) " +                                    
                                    " values (" +
                                    "'" + msGetGid + "'," +
                                    "'" + msGetGid1 + "'," +                                  
                                   "'" + dt["product_gid"].ToString() + "'," +
                                   "'" + dt["potential_value"].ToString() + "'," +
                                   "'" + dt["uom_gid"].ToString() + "'," +
                                   "'" + dt["productgroup_gid"].ToString() + "'," +
                                   "'" + dt["qty_requested"].ToString() + "'," +
                                   "'" + dt["created_by"].ToString() + "',";

                                if (dt["product_requireddate"].ToString() == null || dt["product_requireddate"].ToString() == "" || dt["product_requireddate"].ToString() == "undefined")
                                {
                                    msSQL += "'0000-00-00')";
                                }
                                else
                                {
                                    msSQL += "'" + Convert.ToDateTime(dt["product_requireddate"]).ToString("yyyy-MM-dd") + "') ";


                                }

                            }
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                    else
                    {
                        values.message = "Please add the product to raise enquiry";
                    }
                    if (mnResult == 1)
                    {

                        lsrefno = objcmnfunctions.GetMasterGID("ENQ");

                        string lsenquiry_status = "Enquiry Raised";
                        string lspurchaseenquiry_flag = "Enquiry Raised";
                        string lslead_status = "Assigned";
                        msSQL = "select sum(potential_value) as potential_value from smr_trn_tsalesenquirydtl where enquiry_gid='" + msGetGid1 + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        
                        if (objOdbcDataReader.HasRows == true)
                        {
                            lspotential_value = objOdbcDataReader["potential_value"].ToString();

                        }
                       


                            msSQL = " select leadbank_gid from crm_trn_tleadbank where customer_gid='" + values.customer_gid + "' ";
                            lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);

                            string uiDateStr = values.enquiry_date;
                            DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            string enquiry_date = uiDate.ToString("yyyy-MM-dd");

                            string uiDateStr2 = values.closure_date;
                            DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            closure_date = uiDate2.ToString("yyyy-MM-dd");

                            msSQL = " Insert into smr_trn_tsalesenquiry " +
                                        " (enquiry_gid, " +
                                        " branch_gid, " +
                                        " leadbank_gid, " +
                                        " customer_gid, " +
                                        " customer_name, " +
                                        " contact_number, " +
                                        " contact_person, " +
                                        " contact_email, " +
                                        " customerbranch_gid," +
                                        " contact_address, " +
                                        " enquiry_type, " +
                                        " enquiry_date, " +
                                        " enquiry_remarks, " +
                                        " enquiry_status, " +
                                        " enquiry_referencenumber, " +
                                        " closure_date," +
                                        " created_by, " +
                                        " created_date, " +
                                        " purchaseenquiry_flag, " +
                                        " potorder_value," +
                                        " customer_requirement," +
                                        " landmark," +
                                        " lead_status," +
                                        " enquiry_assignedby, " +
                                        " enquiry_receivedby, " +
                                        " product_count)" +
                                        " values (" +
                                        "'" + msGetGid1 + "', " +
                                        "'" + values.branch_name + "', " +
                                        "'" + lsleadbank_gid + "'," +
                                        "'" + values.customer_gid + "'," +
                                        "'" + values.customer_name + "', " +
                                        "'" + values.contact_number + "'," +
                                        "'" + values.customercontact_name + "'," +
                                        "'" + values.contact_email + "'," +
                                        "'" + values.customerbranch_name + "'," +
                                        "'" + values.contact_address + "'," +
                                         "'" + lsenquiry_type + "'," +
                                        "'" + enquiry_date + "', " +
                                        "'" + values.enquiry_remarks + "', " +
                                        "' " + lsenquiry_status + "'," +
                                        "'" + lsrefno + "', " +
                                        "'" + closure_date + "', ";
                            msSQL += "'" + employee_gid + "', " +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + "', " +
                                     "'" + lspurchaseenquiry_flag + "'," +
                                     "'" + lspotential_value + "'," +
                                     "'" + values.customer_requirement + "'," +
                                     "'" + values.landmark + "'," +
                                     "'" + lslead_status + "'," +
                                     "'" + employee_gid + "', " +
                                     "'" + employeeGid + "', " +
                                     "'" + dt_datatable.Rows.Count + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }

                        if (mnResult == 0)
                        {
                            values.status = false;
                            values.message = "Kindly Enter Valid Values";
                            return;
                        }

                        else
                        {
                            string uiDateStr = values.enquiry_date;
                            DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            string enquiry_date = uiDate.ToString("yyyy-MM-dd");

                            string lsenquiry_flag = "PR Pending Approval";
                            string lsenquiry_status = "Enquiry Raised";

                        msSQL = " Insert into acp_trn_tenquiry " +
                                " (enquiry_gid, " +
                                " branch_gid, " +
                                " leadbank_gid, " +
                                " customer_gid," +
                                " customer_name, " +
                                " contact_number, " +
                                " contact_person, " +
                                " contact_email, " +
                                " customerbranch_gid," +
                                " contact_address, " +
                                " enquiry_date, " +
                                " enquiry_remarks, " +
                                " enquiry_status, " +
                                " enquiry_referencenumber, " +
                                " customer_requirement," +
                                " landmark," +
                                " created_by, " +
                                " created_date, " +
                                " purchaseenquiry_flag, " +
                                " enquiry_assignedby, " +
                                " enquiry_receivedby, " +
                                " product_count)" +
                                " values (" +
                                "'" + msGetGid1 + "', " +
                                "'" + values.branch_name + "'," +
                                "'" + lsleadbank_gid + "'," +
                                "'" + values.customer_gid + "'," +
                                "'" + values.customer_name + "'," +
                                "'" + values.contact_number + "'," +
                                "'" + values.customercontact_name + "'," +
                                "'" + values.contact_email + "'," +
                                "'" + values.customerbranch_name + "'," +
                                "'" + values.contact_address + "'," +
                                "'" + enquiry_date + "', " +
                                "'" + values.enquiry_remarks + "', " +
                                "' " + lsenquiry_status + "'," +
                                "'" + lsrefno + "', " +
                                "'" + values.customer_requirement + "'," +
                                "'" + values.landmark + "'," +
                                "'" + employee_gid + "', " +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + "', " +
                                "' " + lsenquiry_flag + "', " +
                                "'" + employee_gid + "', " +
                                "'" + employeeGid + "', " +
                                "'" + dt_datatable.Rows.Count + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        
                        if (mnResult == 0)
                        {
                            values.status = false;
                            values.message = "Kindly Enter Valid Values";
                            return;
                        }

                        else
                        {
                            msSQL = "select distinct enquiry_type from smr_tmp_tsalesenquiry where created_by='" + employee_gid + "' ";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            
                            if (objOdbcDataReader.HasRows == true)
                            {
                                lsenquiry_type = "Sales";
                            }

                            else
                            {
                                lsenquiry_type = "Service";
                            }
                           
                        }
                    }

                    string lslead = "Open";

                    msSQL = " update smr_trn_tsalesenquiry set enquiry_type='" + lsenquiry_type + "' where enquiry_gid='" + msGetGid1 + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update acp_trn_tenquiry set enquiry_type='" + lsenquiry_type + "' where enquiry_gid='" + msGetGid1 + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    msgetlead2campaign_gid = objcmnfunctions.GetMasterGID("BLCC");
                    lscampaign_gid = objcmnfunctions.GetMasterGID("BCNP");

                    string lsso = "N";

                    if (msgetlead2campaign_gid == "E")
                    {
                        values.status = true;
                        values.message = "Create sequence code BLCC for Enquiry to campaign ";
                        return;
                    }                  
                            
                            msSQL = " Insert into crm_trn_tenquiry2campaign ( " +
                                                      " lead2campaign_gid, " +
                                                      " enquiry_gid, " +
                                                      " created_by, " +
                                                      " so_status," +
                                                      " customer_gid, " +
                                                      " created_date, " +
                                                      " lead_status, " +
                                                      " customer_rating, " +
                                                      " leadstage_gid, " +
                                                      " campaign_gid," +
                                                      " assign_to ) " +
                                                      " Values ( " +
                                                      "'" + msgetlead2campaign_gid + "'," +
                                                      "'" + msGetGid1 + "'," +
                                                      "'" + employee_gid + "'," +
                                                      "'" + lsso + "'," +
                                                   "'" + values.customer_gid + "'," +
                                                      "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                      "' " + lslead + " '," +
                                                      "'" + values.customer_rating + "'," +
                                                      "'" + lsleadstage + "', " +
                                                      "'" + campaignGid + "'," +
                                                      "'" + employeeGid + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    lslead_status = "Enquiry Assigned";

                    msSQL = " update smr_trn_tsalesenquiry Set " +
                                   " lead_status = '" + lslead_status + "' " +
                                   " where enquiry_gid = '" + msGetGid1 + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " select customer_gid from crm_trn_tleadbank " +
                           " where leadbank_gid='" + lsleadbank_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                if (DBNull.Value.Equals(dt["customer_gid"]))
                                {
                                    lsleadstage = "1";
                                }
                                else
                                {
                                    msSQL = " select enquiry_gid from smr_trn_tsalesenquiry where customer_gid='" + values.customer_gid + "'";
                                    dt_datatable = objdbconn.GetDataTable(msSQL);
                                    if (dt_datatable.Rows.Count != 0)
                                    {
                                        lsleadstage = "3";
                                    }
                                }

                            msSQL = " update crm_trn_tenquiry2campaign  set " +
                               " leadstage_gid='" + lsleadstage + "'" +
                               " where lead2campaign_gid='" + msgetlead2campaign_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {

                                msSQL = " update crm_trn_tlead2campaign  set " +
                                          " leadstage_gid='" + lsleadstage + "'" +
                                          " where leadbank_gid='" + lsleadbank_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            }
                        }
                    
                    if (mnResult != 0 || mnResult == 0)
                    {
                        msSQL = "delete FROM smr_tmp_tsalesenquiry WHERE created_by='" + employee_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Enquiry Raised Successfully";
                        return;
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Raising Enquiry";
                        return;
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Select one Product to Raise Enquiry";
                    return;
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Raising Enquiry !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
    }
}