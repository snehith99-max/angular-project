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
    public class DaCustomerEnquiryEdit
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        private OdbcDataReader objOdbcDataReader;

        DataTable dt_datatable;

        string msEmployeeGID, lsemployee_gid, lsuser_gid, msGetEnquiryGid, closure_date, lspercentage1, lsproductuomgid, lsproduct_type, lsproductgid1, QuoatationGID, EnquiryGID, TempQuoatationGID, lsenquiry_type, lsentity_code, lsleadstagegid, lscustomer_gid, lsleadbank_gid, lscampaign_gid, lspotential_value, lstype1, lsdesignation_code, lslead_status, lsleadstage, lspurchaseenquiry_flag, lsCode, msGetGid, msGetGid1, msgetlead2campaign_gid, msGetPrivilege_gid, msGetModule2employee_gid, status, E;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, i;
        string tmpquotationdtl_gid, quotation_gid, product_gid, productgroup_gid, productgroup_name, customerproduct_code, product_name, display_field, product_price, qty_quoted, discountpercentage, discountamount;
        string uom_gid, uom_name, selling_price, price, tax_name, tax_name2, tax_name3, tax1_gid, tax2_gid, tax3_gid, slno, product_requireddate, productrequireddate_remarks, tax_percentage, tax_percentage2, tax_percentage3;
        string vendor_gid, tax_amount, tax_amount2, tax_amount3, salesperson, lsQuotationMode;
        string quotation_type, lsQOStatus;

        public void DaGetEditCustomerEnquirySummary(string enquiry_gid, MdlCustomerEnquiryEdit values)
        {
            try
            {
                msSQL = " delete from smr_tmp_tsalesenquiry where enquiry_gid='" + enquiry_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " Select a.enquiry_gid,h.campaign_gid,b.branch_gid,a.customerbranch_gid, h.campaign_title,a.customer_gid, date_format(a.enquiry_date,'%d-%m-%Y') as enquiry_date,a.customer_name,b.branch_name,a.contact_number, a.enquiry_referencenumber," +
                    " a.enquiry_remarks,a.contact_email,i.customer_rating,a.contact_address,a.contact_person,g.customer_rating,CASE WHEN a.closure_date = '0000-00-00' THEN '' ELSE DATE_FORMAT(a.closure_date, '%d-%m-%Y') END AS closure_date,a.landmark,a.customer_requirement, a.enquiry_receivedby," +
                    " concat(f.user_code, ' | ', f.user_firstname, ' ', f.user_lastname) as user, d.employee_gid" +
                    " from smr_trn_tsalesenquiry a" +
                    " left join hrm_mst_tbranch b on a.branch_gid = b.branch_gid" +
                    " left join hrm_mst_temployee d on a.enquiry_receivedby = d.employee_gid" +
                    " left join adm_mst_tuser f on d.user_gid = f.user_gid" +
                    " left join crm_trn_tenquiry2campaign g on a.customer_gid = g.customer_gid" +
                    " left join smr_trn_tcampaign h on g.campaign_gid = h.campaign_gid" +
                    " left join crm_trn_tenquiry2campaign i on a.enquiry_gid = i.enquiry_gid" +
                    " where a.enquiry_gid='" + enquiry_gid + "' group by a.enquiry_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<editenquirylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new editenquirylist
                        {
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            enquiry_date = dt["enquiry_date"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),                            
                            enquiry_referencenumber = dt["enquiry_referencenumber"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_number = dt["contact_number"].ToString(),
                            contact_person = dt["contact_person"].ToString(),
                            assign_to = dt["user"].ToString(),
                            customerbranch_name = dt["customerbranch_gid"].ToString(),
                            contact_email = dt["contact_email"].ToString(),
                            enquiry_remarks = dt["enquiry_remarks"].ToString(),
                            customer_rating = dt["customer_rating"].ToString(),
                            contact_address = dt["contact_address"].ToString(),
                            customer_requirement = dt["customer_requirement"].ToString(),
                            landmark = dt["landmark"].ToString(),
                            closure_date = dt["closure_date"].ToString(),                           
                            customer_gid = dt["customer_gid"].ToString(),

                        });
                        values.editenquiry_list = getModuleList;

                    }

                    dt_datatable.Dispose();
                }
                msSQL = " select a.enquiry_gid,a.enquirydtl_gid,CASE WHEN a.product_requireddate = '0000-00-00' THEN '0000-00-00' ELSE DATE_FORMAT(a.product_requireddate, '%Y-%m-%d') END AS product_requireddate, a.product_gid, a.productgroup_gid, a.uom_gid," +
                            " a.qty_enquired,a.potential_value, a.created_by from smr_trn_tsalesenquirydtl a" +
                            " where a.enquiry_gid='" + enquiry_gid + "' order by a.enquirydtl_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        string TmpEnquiryGID = objcmnfunctions.GetMasterGID("PPDC");

                        msSQL = " insert into smr_tmp_tsalesenquiry (" +
                                               " tmpsalesenquiry_gid, " +
                                               " enquiry_gid," +
                                               " product_gid," +
                                               " productgroup_gid," +
                                               " uom_gid," +
                                               " created_by, " +
                                               " qty_requested," +
                                               " product_requireddate," +
                                               " potential_value" +
                                               " )values(" +
                                               "'" + TmpEnquiryGID + "'," +
                                               "'" + enquiry_gid + "'," +
                                               "'" + dt["product_gid"] + "', " +
                                               "'" + dt["productgroup_gid"] + "', " +
                                               "'" + dt["uom_gid"] + "', " +
                                               "'" + dt["created_by"] + "', " +
                                               "'" + dt["qty_enquired"] + "'," +
                                               "'" + dt["product_requireddate"] + "'," +
                                               "'" + dt["potential_value"].ToString().Replace(", ", "") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }


                }
               

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while ViewEnquirySummary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


            }

        }

        // POST PRODUCT FOR EDIT CUSTOMER ENQUIRY
        public void DaPostProductEnquiryEdit(string employee_gid, editenquiryproductsummary_list values)
        {
            try
            {

                msGetGid = objcmnfunctions.GetMasterGID("PPDC");
                msGetGid1 = objcmnfunctions.GetMasterGID("PPTM");

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
                        " user_gid, " +
                        " product_requireddate," +
                        " enquiry_type) " +
                        " values( " +
                         "'" + msGetGid + "'," +
                         "'" + values.enquiry_gid + "'," +
                        "'" + lsproductgroupgid + "'," +
                        "'" + lsproductgid + "'," +
                        "'" + values.potential_value.Replace(", ", "") + "'," +
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
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Product";
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

        // CUSTOMER EDIT ENQUIRY - PRODUCT SUMMARY

        public void DaEditCustomerProductSummary(string enquiry_gid, MdlCustomerEnquiryEdit values)
        {
            try
            {


                msSQL = " select a.tmpsalesenquiry_gid,a.enquiry_gid,a.customerproduct_code,a.qty_requested,a.display_field, " +
                    " CASE WHEN a.product_requireddate = '0000-00-00' THEN '' ELSE DATE_FORMAT(a.product_requireddate, '%d-%m-%Y') END AS product_requireddate, " +
                    " d.productgroup_name,b.product_code,b.product_name,c.productuom_name,a.product_gid, " +
                    " format(a.potential_value,2)as potential_value,a.product_requireddateremarks " +
                    " from smr_tmp_tsalesenquiry a left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                    " left join pmr_mst_tproductuom c on a.uom_gid=c.productuom_gid " +
                    " left join pmr_mst_tproductgroup d on a.productgroup_gid= d.productgroup_gid" +
                    " where a.enquiry_gid='" + enquiry_gid + "' order by a.tmpsalesenquiry_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<editproductsummarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new editproductsummarylist
                        {
                            tmpsalesenquiry_gid = dt["tmpsalesenquiry_gid"].ToString(),
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            product_requireddate = dt["product_requireddate"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                        });
                        values.editproductsummary_list = getModuleList;
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


         //PRODUCT EDIT SUMMARY
        public void DaEditProductSummary(string tmpsalesenquiry_gid, MdlCustomerEnquiryEdit values)
        {
            try
            {


                msSQL = " select a.tmpsalesenquiry_gid,a.enquiry_gid,a.customerproduct_code,a.qty_requested,a.display_field, " +
                    " CASE WHEN a.product_requireddate = '0000-00-00' THEN '' ELSE DATE_FORMAT(a.product_requireddate, '%d-%m-%Y') END AS product_requireddate, " +
                    " d.productgroup_name,b.product_code,b.product_name,c.productuom_name,a.product_gid, " +
                    " format(a.potential_value,2)as potential_value,a.product_requireddateremarks " +
                    " from smr_tmp_tsalesenquiry a left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                    " left join pmr_mst_tproductuom c on a.uom_gid=c.productuom_gid " +
                    " left join pmr_mst_tproductgroup d on a.productgroup_gid= d.productgroup_gid" +
                    " where a.tmpsalesenquiry_gid='" + tmpsalesenquiry_gid + "' order by a.tmpsalesenquiry_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<editproductsummarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new editproductsummarylist
                        {
                            tmpsalesenquiry_gid = dt["tmpsalesenquiry_gid"].ToString(),
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            product_requireddate = dt["product_requireddate"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                        });
                        values.editproductsummary_list = getModuleList;
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

        // PRODUCT UPPDATE -- ENQUIRY TO QUOTATION 
        public void DaPostEnquiryUpdateProduct(string employee_gid, editproductsummarylist values)
        {
            try
            {

                if (values.product_requireddate == null || values.product_requireddate == "" || values.product_requireddate == "undefined")
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
                        " potential_value ='" + values.potential_value.Replace(",", "").Replace(" ", "").Trim() + "', " +
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
                }
                else
                {
                    values.status = false;
                    values.message = " Error While Updating Product Details ";
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

        // DELETE EVENT FOR EDIT CUSTOMER ENQUIRY PRODUCT SUMMARY

        public void DaDeleteProductSummary(string tmpsalesenquiry_gid, editproductsummarylist values)
        {
            try
            {

                msSQL = "delete from smr_tmp_tsalesenquiry where tmpsalesenquiry_gid='" + tmpsalesenquiry_gid + "'";
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


        public void DaPostCustomerEnquiryEdit(string employee_gid, string user_gid, Postedit values)

        {
            try
            {
                string totalvalue = values.assign_to;

                string[] separatedValues = totalvalue.Split('|').Concat(totalvalue.Split(' ')).ToArray();


                // Access individual components
                string campaign_title = separatedValues[0];
                string user_code = separatedValues[1];
                string user_firstname = separatedValues[2];
                string user_lastname = separatedValues.Length == 3 ? separatedValues[3] : null;

                msSQL = "SELECT * FROM smr_tmp_tsalesenquiry WHERE enquiry_gid='" + values.enquiry_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count != 0)

                {
                   
                msGetGid1 = objcmnfunctions.GetMasterGID("PPTP");

                if (msGetGid1 == "E") // Assuming "E" is a string constant
                {
                    values.status = true;
                    values.message = "Create Sequence Code PPTP for Sales Enquiry Details";
                }
                    msSQL = "delete from smr_trn_tsalesenquirydtl where enquiry_gid='" + values.enquiry_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "delete from acp_trn_tenquirydtl where enquiry_gid='" + values.enquiry_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "SELECT DISTINCT " +
                    "a.product_gid, a.product_remarks, a.customerproduct_code, a.potential_value," +
                    "a.qty_requested, a.uom_gid, a.display_field, a.product_requireddate, a.product_requireddateremarks, " +
                    "a.productgroup_gid" +
                    " FROM smr_tmp_tsalesenquiry a WHERE" +
                    " a.enquiry_gid = '" + values.enquiry_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("PPDC");

                        if (msGetGid == "E") // Assuming "E" is a string constant
                        {
                            values.status = true;
                            values.message = "Create Sequence Code PPDC for Sales Enquiry Details";
                        }

                           

                            string lsnewproduct_flag = "Y";

                        msSQL = " Insert into smr_trn_tsalesenquirydtl " +
                               " (enquirydtl_gid," +
                               " enquiry_gid , " +
                               " customerproduct_code," +
                               " product_gid," +
                               " potential_value," +
                               " uom_gid," +
                               " productgroup_gid," +
                               " qty_enquired, " +
                               " newproduct_flag, " +
                               " product_requireddate, " +
                               " product_requireddateremarks," +
                               " display_field ) " +
                               " values (" +
                               "'" + msGetGid + "'," +
                               "'" + values.enquiry_gid + "'," +
                               "'" + dt["customerproduct_code"].ToString() + "'," +
                               "'" + dt["product_gid"].ToString() + "'," +
                               "'" + dt["potential_value"].ToString() + "'," +
                               "'" + dt["uom_gid"].ToString() + "'," +
                               "'" + dt["productgroup_gid"].ToString() + "'," +
                               "'" + dt["qty_requested"].ToString() + "', " +
                               "'" + lsnewproduct_flag + "', ";

                        if (dt["product_requireddate"].ToString() == null || dt["product_requireddate"].ToString() == "" || dt["product_requireddate"].ToString() == "undefined")
                        {
                            msSQL += "0000-00-00,";
                        }
                        else
                        {
                            string formattedDate = ((DateTime)dt["product_requireddate"]).ToString("yyyy-MM-dd");
                            msSQL += "'" + formattedDate + "',";
                        }
                        msSQL += "'" + dt["product_requireddateremarks"].ToString() + "',";
                        msSQL += "'" + dt["display_field"].ToString() + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        if (mnResult != 0)
                        {
                            msSQL = " Insert into acp_trn_tenquirydtl " +
                                " (enquirydtl_gid," +
                                " enquiry_gid , " +
                                " customerproduct_code," +
                                " product_gid," +
                                " potential_value," +
                                " uom_gid," +
                                " productgroup_gid," +
                                " qty_enquired, " +
                                " product_requireddate, " +
                                " product_requireddateremarks," +
                                " display_field ) " +
                                " values (" +
                                "'" + msGetGid + "'," +
                                "'" + values.enquiry_gid + "'," +
                                  "'" + dt["customerproduct_code"].ToString() + "'," +
                               "'" + dt["product_gid"].ToString() + "'," +
                               "'" + dt["potential_value"].ToString() + "'," +
                               "'" + dt["uom_gid"].ToString() + "'," +
                               "'" + dt["productgroup_gid"].ToString() + "'," +
                               "'" + dt["qty_requested"].ToString() + "', ";

                                if (dt["product_requireddate"].ToString() == null || dt["product_requireddate"].ToString() == "" || dt["product_requireddate"].ToString() == "undefined")
                                {
                                    msSQL += "0000-00-00,";
                                }
                                else
                            {
                                string formattedDate = ((DateTime)dt["product_requireddate"]).ToString("yyyy-MM-dd");
                                msSQL += "'" + formattedDate + "',";
                            }
                            msSQL += "'" + dt["product_requireddateremarks"].ToString() + "',";
                            msSQL += "'" + dt["display_field"].ToString() + "')";
                        }

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    }
                }




                    if (mnResult != 0)
                    {

                        msSQL = "select sum(potential_value) as potential_value from smr_tmp_tsalesenquiry where enquiry_gid='" + values.enquiry_gid + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        
                        if (objOdbcDataReader.HasRows == true)
                        {
                            lspotential_value = objOdbcDataReader["potential_value"].ToString();

                        }
                       
                        string enquirystatus = "Enquiry Raised";
                        msSQL = "select branch_gid, branch_name from hrm_mst_tbranch where branch_name='" + values.branch_name + "' ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {

                                string uiDateStr1 = values.closure_date;
                                DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                closure_date = uiDate1.ToString("yyyy-MM-dd");

                                msSQL = " Select customer_gid from crm_mst_tcustomer where customer_name='" + values.customer_name + "'";
                                string lscustomergid = objdbconn.GetExecuteScalar(msSQL);

                                string trimmedName = user_code.Trim();


                                msSQL = "SELECT user_gid FROM adm_mst_tuser WHERE user_code = '" + trimmedName + "'";
                                string user = objdbconn.GetExecuteScalar(msSQL);

                                msSQL = "SELECT employee_gid FROM hrm_mst_temployee WHERE user_gid = '" + user + "'";
                                string employee = objdbconn.GetExecuteScalar(msSQL);

                                msSQL = "UPDATE smr_trn_tsalesenquiry SET " +
                                "branch_gid = '" + values.branch_gid + "', " +
                                "customer_gid = '" + lscustomergid + "', " +
                                "customer_name = '" + values.customer_name + "', " +
                                "contact_number = '" + values.contact_number + "', " +
                                "contact_person = '" + values.contact_person + "', " +
                                "contact_email = '" + values.contact_email + "', " +
                                "customerbranch_gid = '" + values.customerbranch_name + "', " +
                                "contact_address = '" + values.contact_address + "', " +
                                "enquiry_remarks = '" + values.enquiry_remarks + "', " +
                                "enquiry_referencenumber = '" + values.enquiry_referencenumber + "', " +
                                "closure_date = '" + closure_date + "', " +
                                "potorder_value = '" + lspotential_value + "', " +
                                "customer_requirement = '" + values.customer_requirement + "', " +
                                "landmark = '" + values.landmark + "', " +
                                "enquiry_status = '" + enquirystatus + "', " +
                                "enquiry_receivedby = '" + employee + "', " +
                                "product_count = '" + dt_datatable.Rows.Count + "' " +
                                "WHERE enquiry_gid = '" + values.enquiry_gid + "'";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                                if (mnResult != 0)
                                {


                                    msSQL = "UPDATE acp_trn_tenquiry SET " +
                                            "branch_gid = '" + dt["branch_gid"].ToString() + "', " +
                                            "customer_gid = '" + lscustomergid + "', " +
                                            "customer_name = '" + values.customer_name + "', " +
                                            "contact_number = '" + values.contact_number + "', " +
                                            "contact_person = '" + values.contact_person + "', " +
                                            "contact_email = '" + values.contact_email + "', " +
                                            "customerbranch_gid = '" + values.customerbranch_name + "', " +
                                            "contact_address = '" + values.contact_address + "', " +
                                            "enquiry_remarks = '" + values.enquiry_remarks + "', " +
                                            "enquiry_referencenumber = '" + values.enquiry_referencenumber + "', " +
                                            "customer_requirement = '" + values.customer_requirement + "', " +
                                            "landmark = '" + values.landmark + "', " +
                                            "enquiry_receivedby = '" + employee + "', " +
                                            "product_count = '" + dt_datatable.Rows.Count + "' " +
                                            "WHERE enquiry_gid = '" + values.enquiry_gid + "'";


                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }

                                if (mnResult != 0)
                                {
                                    msSQL = "Select campaign_gid from smr_trn_tcampaign where campaign_title='" + campaign_title + "'";
                                    string campaigngid = objdbconn.GetExecuteScalar(msSQL);

                                    msSQL = "Select lead2campaign_gid from crm_trn_tenquiry2campaign where enquiry_gid='" + values.enquiry_gid + "'";
                                    string lead2campaigngid = objdbconn.GetExecuteScalar(msSQL);

                                    string trimmedFirstName = user_code.Trim();


                                    msSQL = "SELECT user_gid FROM adm_mst_tuser WHERE user_code = '" + trimmedFirstName + "'";
                                    string usergid = objdbconn.GetExecuteScalar(msSQL);

                                    msSQL = "Select employee_gid from hrm_mst_temployee where user_gid='"+ usergid +"'";
                                    string employeegid = objdbconn.GetExecuteScalar(msSQL);

                                    string leadstatus = "Open";

                                    msSQL = "UPDATE crm_trn_tenquiry2campaign SET " +                                          
                                           "customer_gid = '" + lscustomergid + "', " +                                          
                                           "assign_to = '" + employeegid + "', " +                                          
                                           "campaign_gid = '" + campaigngid + "', " +                                          
                                           "lead_status = '" + leadstatus + "', " +                                          
                                           "customer_rating = '" + values.customer_rating + "', " +                                          
                                           "updated_by = '" + user_gid + "', " +                                          
                                           "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +                                          
                                           "leadstage_gid = '3'" +                                          
                                           "WHERE enquiry_gid = '" + values.enquiry_gid + "' and lead2campaign_gid='" + lead2campaigngid + "'";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                }

                                }
                            }

                        if (mnResult == 1 || mnResult == 0)
                        {
                            msSQL = "delete FROM smr_tmp_tsalesenquiry WHERE created_by='" + employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Enquiry Updated Successfully";
                            return;
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Enquiry";
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
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  Updating Enquiry !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }


        }
    }
}