using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Net.NetworkInformation;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Web.Http.Results;
using static OfficeOpenXml.ExcelErrorValue;
using System.Resources;
using System.Runtime.Remoting.Messaging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;


namespace ems.sales.DataAccess
{
    public class DaSmrTrnCustomerEnquiry
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        private OdbcDataReader objOdbcDataReader;

        DataTable dt_datatable;
        string lscompany_code,  lsrefno;

        string msEmployeeGID, lsemployee_gid, lsuser_gid, msGetEnquiryGid, msGetGid3, msGetGid2,msGetGID11,closure_date, lspercentage1, lsproductuomgid, lsproduct_type, lsproductgid1, QuoatationGID, EnquiryGID, TempQuoatationGID, lsenquiry_type, lsentity_code, lsleadstagegid, lscustomer_gid, lsleadbank_gid, lscampaign_gid, lspotential_value, lstype1, lsdesignation_code, lslead_status, lsleadstage, lspurchaseenquiry_flag, lsCode, msGetGid, msGetGid01, msGetGid1, msgetlead2campaign_gid, msGetPrivilege_gid, msGetModule2employee_gid, status, E;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, i;
        string tmpquotationdtl_gid, quotation_gid, product_gid, productgroup_gid, productgroup_name, customerproduct_code, product_name, display_field, product_price, qty_quoted, discountpercentage, discountamount;
        string uom_gid, uom_name, selling_price, price, tax_name, tax_name2, tax_name3, tax1_gid, tax2_gid, tax3_gid, slno, product_requireddate, productrequireddate_remarks, tax_percentage, tax_percentage2, tax_percentage3;
        string vendor_gid, tax_amount, tax_amount2, tax_amount3, salesperson, lsQuotationMode;
        string quotation_type, lsQOStatus;
        //Summary
        public void DaGetCustomerEnquirySummary(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {

                msSQL = "  Select distinct concat(a.enquiry_gid,' / ',a.enquiry_type) as enquiry_refno,m.leadbank_name,n.leadbankcontact_name," +
                    " n.leadbankcontact_gid,format(a.potorder_value, 2) as potorder_value, m.customer_gid," +
                    " concat(s.source_name, ' / ', m.referred_by) as source_name,z.branch_prefix," +
                    " concat(f.user_firstname, ' ', f.user_lastname) as campaign," +
                    " a.enquiry_gid,DATE_FORMAT(a.enquiry_date, '%d-%m-%Y') as enquiry_date," +
                    " a.leadbank_gid,m.leadbank_gid as lead_gid,a.customer_name,a.branch_gid, a.lead_status,z.branch_name," +
                    " concat(o.region_name, ' / ', m.leadbank_city, ' / ', m.leadbank_state) as region_name," +
                    " concat(f.user_firstname, ' - ', f.user_lastname) as assign_to,a.enquiry_referencenumber," +
                    " a.enquiry_status,a.enquiry_type,a.enquiry_remarks,a.potorder_value ,a.created_date ," +
                    " a.contact_person,a.contact_email,a.contact_address, T.customer_rating," +
                    " case when a.contact_person is null then concat(n.leadbankcontact_name,' / ',n.mobile,' / ',n.email) when a.contact_person is not null then" +
                    " concat(a.customerbranch_gid,' | ',a.contact_person,' | ',a.contact_number,' | ',a.contact_email) end as contact_details," +
                    " a.enquiry_referencenumber, a.enquiry_type from  crm_trn_tleadbank m "+
                    " left join smr_trn_tsalesenquiry a  on a.leadbank_gid = m.leadbank_gid" +
                    " left join crm_trn_tleadbankcontact n on n.leadbank_gid = m.leadbank_gid" +
                    " left join crm_mst_tregion o on m.leadbank_region = o.region_gid" +
                    " left join crm_trn_tenquiry2campaign p on p.customer_gid = a.customer_gid " +
                    " left join crm_mst_tleadstage r on r.leadstage_gid = p.leadstage_gid" +
                    " left join smr_trn_tcampaign q on q.campaign_gid = p.campaign_gid"+
                    " left join hrm_mst_temployee d on d.employee_gid = p.assign_to" +
                    " left join adm_mst_tuser b on b.user_gid = d.user_gid" +
                    " left join hrm_mst_temployee k on k.employee_gid = a.enquiry_receivedby" +
                    " left join adm_mst_tuser f on f.user_gid = k.user_gid" +
                    " left join hrm_mst_tbranch z on a.branch_gid = z.branch_gid" +
                    " left join crm_mst_tsource s on s.source_gid = m.source_gid" +
                    " left join crm_trn_tenquiry2campaign t on a.enquiry_gid = t.enquiry_gid" +
                    " group by a.enquiry_gid order by a.enquiry_gid desc ";

                 dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCusEnquiry>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCusEnquiry
                        {
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            customer_gid = dt["leadbank_gid"].ToString(),
                            customer = dt["customer_gid"].ToString(),
                            leadbank_gid = dt["lead_gid"].ToString(),
                            customercontact_gid = dt["leadbankcontact_gid"].ToString(),
                            enquiry_date = dt["enquiry_date"].ToString(),
                            enquiry_refno = dt["enquiry_refno"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            enquiry_referencenumber = dt["enquiry_referencenumber"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            created_by = dt["campaign"].ToString(),
                            potorder_value = dt["potorder_value"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            enquiry_status = dt["enquiry_status"].ToString(),
                            customer_rating = dt["customer_rating"].ToString(),
                            assign_to= dt["assign_to"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString()
                        });
                        values.cusenquiry_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Customer Enquiry !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // Lead Dropdown
        public void DaGetLeadDtl(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
                
                msSQL = " SELECT a.leadstage_gid,CASE WHEN a.leadstage_gid = '5' THEN 'Close' ELSE a.leadstage_name END AS leadstage_name " +
                    " FROM crm_mst_tleadstage a" +
                    " WHERE leadstage_gid='5' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetLeadDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetLeadDropdown
                        {
                            leadstage_gid = dt["leadstage_gid"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),

                        });
                        values.GetLeadDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Stage Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        // Update close
        public void DaGetUpdatedCloseEnquiry(string employee_gid, GetCusEnquiry values)
        {
            try
            {
                string stat = "Drop";

               

                msSQL = " select enquiry_status from smr_trn_tsalesenquiry where enquiry_gid='" + values.enquiry_gid + "'";
                string lsstatus = objdbconn.GetExecuteScalar(msSQL);

                if (lsstatus != "Quotation Raised")
                {

                    msSQL = "  update crm_trn_tenquiry2campaign set leadstage_gid='5' where enquiry_gid='" + values.enquiry_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " update smr_trn_tsalesenquiry set " +
                            " lead_status = '" + stat + "'," +
                            " enquiry_status='Drop' " +
                            " where enquiry_gid = '" + values.enquiry_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = " Enquiry Dropped Successfully";
                        return;

                    }

                    else
                    {
                        values.status = false;
                        values.message = "Error While Dropping Sales Enquiry";
                        return;
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Can't Close Enquiry, Because Quotation has been Already Raised";
                    return;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Closing Sales Enquiry !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           


        }

        // Team Dropdown
        public void DaGetTeamDtl(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
               
                msSQL = "select campaign_title,campaign_gid " +
                    " from smr_trn_tcampaign ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTeamDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTeamDropdown
                        {

                            campaign_gid = dt["campaign_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),

                        });
                        values.GetTeamDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Campain Tittle !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        //Employee Dropdown
        public void DaGetEmployeeDtl(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {

                msSQL = " select distinct a.employee_gid," +
                    " concat(c.user_firstname,' ',c.user_lastname)as employee_name" +
                    " from smr_trn_tcampaign2employee a" +
                    " inner join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " inner join adm_mst_tuser c on b.user_gid=c.user_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetEmployeeDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEmployeeDropdown
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),

                        });
                        values.GetEmployeeDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Employee Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        // Update Reassign
        public void DaGetUpdatedReAssignEnquiry(string user_gid, GetCusEnquiry values)
        {
            try
            {
                msSQL = " update crm_trn_tenquiry2campaign set " +
                        " assign_to = '" + values.employee_name + "'," +
                        " campaign_gid = '" + values.campaign_title + "', " +
                        " updated_by = '" + user_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                        " where enquiry_gid = '" + values.enquiry_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    msSQL = " update smr_trn_tsalesenquiry set " +
                            " enquiry_receivedby = '" + values.employee_name + "'" +
                            " where enquiry_gid = '" + values.enquiry_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = " Successfully Re-Assigned";
                    return;

                }

                else
                {
                    values.status = false;
                    values.message = "Error While Re-Assigning";
                    return;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating  Re-Assigning!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }
        public void DaGetProductGrp(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
               
                msSQL = " select distinct(a.productgroup_gid),b.productgroup_name " +
                    " from pmr_mst_tproduct a," +
                    " pmr_mst_tproductgroup b where a.productgroup_gid=b.productgroup_gid  and b.delete_flag='N' " +
                    " order by b.productgroup_name ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductGrp>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductGrp
                        {
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                        });
                        values.GetProductGrp = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Group Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }
        public void DaGetProducts(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
               
                msSQL = "select product_gid,product_name from pmr_mst_tproduct" +
                    " where status='1'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProducts>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProducts
                        {
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                        });
                        values.GetProducts = getModuleList;
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
        public void DaGetProductunit(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
               
                msSQL = " Select a.productuom_gid as uom_gid, a.productuom_name " +
                   " from pmr_mst_tproductuom a " +
                   " where a.delete_flag='N' and a.productuomclass_gid in (select productuomclass_gid from pmr_mst_tproduct where delete_flag='N' ) ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductUnits>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductUnits
                        {
                            productuom_name = dt["productuom_name"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                        });
                        values.GetProductUnits = getModuleList;
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

        public void DaGetCustomer(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
                

                msSQL = "Select a.customer_gid, a.customer_name " +
                     " from crm_mst_tcustomer a " +
                     "where a.status='Active'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCustomername>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomername
                        {
                            customer_name = dt["customer_name"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                        });
                        values.GetCustomername = getModuleList;
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

        public void DaGetLead(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {


                msSQL = "Select a.leadbank_gid, a.leadbank_name " +
                     " from crm_trn_tleadbank a where a.customer_gid is null";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetLeadname>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetLeadname
                        {
                            leadbank_name = dt["leadbank_name"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                        });
                        values.GetLeadname = getModuleList;
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
        public void DaGetOnChangeCustomerName(string customercontact_gid, MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
               

                if (customercontact_gid != null)
                {
                    msSQL = " select a.customercontact_gid,concat(a.address1, '   ', a.city, '   ', a.state, '   ', a.zip_code) as address1,ifnull(a.address2, '') as address2,ifnull(a.city, '') as city, " +
                     " ifnull(a.state, '') as state,ifnull(a.country_gid, '') as country_gid,ifnull(a.zip_code, '') as zip_code, c.customer_name," +
                     " ifnull(a.mobile, '') as mobile,ifnull(a.email, '') as email,ifnull(b.country_name, '') as country_name,a.customerbranch_name,concat(a.customercontact_name) as " +
                     " customercontact_names, c.customer_gid " +
                     " from crm_mst_tcustomercontact a " +
                     " left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid " +
                     " left join adm_mst_tcountry b on a.country_gid = b.country_gid " +
                     " where c.customer_gid='" + customercontact_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetCustomer>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetCustomer
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
                                customercontact_gid = dt["customercontact_gid"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),
                                customer_name = dt["customer_name"].ToString()
                            });
                            values.GetCustomer = getModuleList;
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

        public void DaGetOnChangeLead(string leadbank_gid, MdlSmrTrnCustomerEnquiry values)
        {
            try
            {


                if (leadbank_gid != null)
                {
                    msSQL = " select a.leadbankcontact_gid," +
                        " concat(a.address1, '   ', a.city, '   ', a.state, '   ', a.pincode) as address1" +
                        ",ifnull(a.address2, '') as address2,ifnull(a.city, '') as city" +
                        ",ifnull(a.state, '') as state,ifnull(a.country_gid, '') as country_gid" +
                        ",ifnull(a.pincode, '') as pincode, c.leadbank_name, ifnull(a.mobile, '') as mobile" +
                        ",ifnull(a.email, '') as email,ifnull(b.country_name, '') as country_name" +
                        ",a.leadbankcontact_name,concat(a.leadbankcontact_name) as leadbankcontact_name" +
                        ",c.leadbank_gid,a.leadbankbranch_name  " +
                        " from crm_trn_tleadbankcontact a" +
                        " left join crm_trn_tleadbank c on a.leadbank_gid = c.leadbank_gid" +
                        " left join adm_mst_tcountry b on a.country_gid = b.country_gid  " +
                        " where c.leadbank_gid='" + leadbank_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetLead>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetLead
                            {
                                leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                                leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                                country_name = dt["country_name"].ToString(),
                                contact_email = dt["email"].ToString(),
                                contact_number = dt["mobile"].ToString(),
                                pincode = dt["pincode"].ToString(),
                                country_gid = dt["country_gid"].ToString(),
                                state = dt["state"].ToString(),
                                city = dt["city"].ToString(),
                                address2 = dt["address2"].ToString(),
                                contact_address = dt["address1"].ToString(),
                                leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                leadbank_name = dt["leadbank_name"].ToString()
                            });
                            values.GetLead = getModuleList;
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

        public void DaGetOnChangeProductsName(string product_gid, MdlSmrTrnCustomerEnquiry values)
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

                    var getModuleList = new List<GetProductsName>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetProductsName
                            {
                                product_name = dt["product_name"].ToString(),
                                product_gid = dt["product_gid"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                productuom_name = dt["productuom_name"].ToString(),
                                productgroup_name = dt["productgroup_name"].ToString(),
                                productuom_gid = dt["productuom_gid"].ToString(),
                                productgroup_gid = dt["productgroup_gid"].ToString(),

                            });
                            values.GetProductsName = getModuleList;
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
        public void DaGetProductsSummary(string user_gid, string employee_gid, MdlSmrTrnCustomerEnquiry values)
        {
            try
            {

                msSQL = " select a.tmpsalesenquiry_gid,a.customerproduct_code,a.qty_requested,a.display_field, " +
                    "  CASE WHEN a.product_requireddate = '0000-00-00' THEN '' ELSE DATE_FORMAT(a.product_requireddate, '%d-%m-%Y') " +
                    " END AS product_requireddate, " +
                    " d.productgroup_name,b.product_code,b.product_name,c.productuom_name,a.product_gid, " +
                    " format(a.potential_value,2)as potential_value,a.product_requireddateremarks " +
                    " from smr_tmp_tsalesenquiry a left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                    " left join pmr_mst_tproductuom c on a.uom_gid=c.productuom_gid " +
                    " left join pmr_mst_tproductgroup d on a.productgroup_gid= d.productgroup_gid" +
                    " where a.created_by='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productsummarys_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productsummarys_list
                        {
                            tmpsalesenquiry_gid = dt["tmpsalesenquiry_gid"].ToString(),
                            customerproduct_code = dt["customerproduct_code"].ToString(),
                            qty_requested = dt["qty_requested"].ToString(),
                            product_requireddate = dt["product_requireddate"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            product_requireddateremarks = dt["product_requireddateremarks"].ToString(),


                        });
                        values.productsummarys_list = getModuleList;
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

        public void DaPostOnAdds(string user_gid, string employee_gid, productsummarys_list values)
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

                msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + values.product_name.Replace("'","\\\'") + "'";
                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name.Replace("'", "\\\'") + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "Select productgroup_gid from pmr_mst_tproductgroup where productgroup_name='" + values.productgroup_name.Replace("'", "\\\'") + "'";
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
                        objOdbcDataReader.Close();
                    }
                    else
                    {
                        lsenquiry_type = "Services";
                    }
                    
                }
                if (values.qty_requested == "undefined" || values.qty_requested == null || values.qty_requested == "" || Convert.ToInt32(values.qty_requested) < 1)
                {

                    values.status = false;
                    values.message = "Product quantity cannot be zero or empty";
                    return;

                }
                else
                {
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
                values.message = "Exception occured while Adding Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

    

        // Currency
        public void DaGetCurrencyDets(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
           


                msSQL = "currencyexchange_gid,currency_code,default_currency,exchange_rate from crm_trn_tcurrencyexchange order by currency_code asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCurrencyDetsDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCurrencyDetsDropdown

                        {
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            default_currency = dt["default_currency"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),

                        });
                        values.GetCurrencyDets = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Currency Code !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }


        // Branch
        public void DaGetBranchDet(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
              

                msSQL = "select branch_gid, branch_name from hrm_mst_tbranch ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranchDetsDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchDetsDropdown

                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),

                        });
                        values.GetBranchDet = getModuleList;
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

        // Tax 2
        public void DaGetSecondTax(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
                

                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetSecondtaxDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetSecondtaxDropdown

                        {
                            tax_gid2 = dt["tax_gid"].ToString(),
                            tax_name2 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()


                        });
                        values.GetSecondTax = getModuleList;
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

        // Tax 3
        public void DaGetThirdTax(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
               


                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetThirdtaxDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetThirdtaxDropdown

                        {
                            tax_gid3 = dt["tax_gid"].ToString(),
                            tax_name3 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()


                        });
                        values.GetThirdTax = getModuleList;
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

        // Tax 4
        public void DaGetEmployeePerson(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {             


                msSQL = " select a.employee_gid,c.user_gid,e.campaign_gid,concat(e.campaign_title, ' | ', c.user_code, ' | ', c.user_firstname, ' ', c.user_lastname)AS employee_name, e.campaign_title " +
                        " from adm_mst_tmodule2employee a " +
                        " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid " +
                        " left join adm_mst_tuser c on b.user_gid=c.user_gid " +
                        " left join smr_trn_tcampaign2employee d on a.employee_gid=d.employee_gid " +
                        " left join smr_trn_tcampaign e on e.campaign_gid = d.campaign_gid " +
                        " where a.module_gid = 'SMR' and a.hierarchy_level<>'-1' and e.delete_flag='N' and a.employee_gid in  " +
                        " (select employee_gid from smr_trn_tcampaign2employee where 1=1) group by employee_name asc; ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<GetAssignDropdown>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetAssignDropdown

                                {
                                    employee_gid = dt["employee_gid"].ToString() + '.' + dt["campaign_title"].ToString() + '.' + dt["campaign_gid"].ToString(),
                                    user_firstname = dt["employee_name"].ToString() ,
                                    campaign_gid = dt["campaign_gid"].ToString(),


                                });
                                values.GetEmployeePerson = getModuleList;
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

        // delete Product
        public void DaGetDeleteEnquiryProductSummary(string tmpsalesenquiry_gid, productsummarys_list values)
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

        public void DaPostCustomerEnquiry(string employee_gid, string user_gid, PostAll values)

        {
            try
            {

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

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

                            if (msGetGid == "E") // Assuming "E" is a string constant
                            {
                                values.status = true;
                                values.message = "Create Sequence Code PPDC for Sales Enquiry Details";
                            }

                            string lsnewproduct_flag = "Y";

                            msSQL = " Insert into smr_trn_tsalesenquirydtl " +
                                   " (enquirydtl_gid," +
                                   " enquiry_gid , " +
                                   //" customerproduct_code," +
                                   " product_gid," +
                                   " potential_value," +
                                   " uom_gid," +
                                   " productgroup_gid," +
                                   " qty_enquired, " +
                                   " created_by, " +
                                   " newproduct_flag, " +
                                   " product_requireddate) " +
                                   //" product_requireddateremarks," +
                                   //" display_field 
                                   " values (" +
                                   "'" + msGetGid + "'," +
                                   "'" + msGetGid1 + "'," +
                                   //"'" + dt["customerproduct_code"].ToString() + "'," +
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
                            //msSQL += "'" + dt["product_requireddateremarks"].ToString() + "',";
                            //msSQL += "'" + dt["display_field"].ToString() + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                            if (mnResult != 0)
                            {
                                msSQL = " Insert into acp_trn_tenquirydtl " +
                                    " (enquirydtl_gid," +
                                    " enquiry_gid , " +
                                    //" customerproduct_code," +
                                    " product_gid," +
                                    " potential_value," +
                                    " uom_gid," +
                                    " productgroup_gid," +
                                    " qty_enquired, " +
                                    " created_by, " +
                                    " product_requireddate) " +
                                    //" product_requireddateremarks," +
                                    //" display_field ) " +
                                    " values (" +
                                    "'" + msGetGid + "'," +
                                    "'" + msGetGid1 + "'," +
                                   //"'" + dt["customerproduct_code"].ToString() + "'," +
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
                                //    msSQL += "'" + dt["product_requireddateremarks"].ToString() + "',";
                                //    msSQL += "'" + dt["display_field"].ToString() + "')";
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

                        lsrefno = objcmnfunctions.GetMasterGID("ENQ", "", user_gid);
               




                        string lsenquiry_status = "Enquiry Raised";
                        string lspurchaseenquiry_flag = "Enquiry Raised";
                        string lslead_status = "Assigned";
                        //string lsenquiry_status = "Enquiry Raised";

                        msSQL = "select sum(potential_value) as potential_value from smr_trn_tsalesenquirydtl where enquiry_gid='" + msGetGid1 + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                           
                            lspotential_value = objOdbcDataReader["potential_value"].ToString();
                           
                        }

                        //if (values.customer_gid == null)
                        //{
                        //    msSQL = " select customer_gid from crm_trn_tleadbank where leadbank_gid='" + values.leadbank_gid + "' ";
                        //    values.customer_gid = objdbconn.GetExecuteScalar(msSQL);
                        //}
                        
                        
                            //msSQL = " select leadbank_gid from crm_trn_tleadbank where customer_gid='" + values.customer_gid + "' ";
                            //lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);

                            string uiDateStr = values.enquiry_date;
                            DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            string enquiry_date = uiDate.ToString("yyyy-MM-dd");

                            if (values.closure_date == "" || values.closure_date == null)
                            {
                                closure_date = "0000-00-00";
                            }
                        
                            else
                            {
                                string uiDateStr2 = values.closure_date;
                                DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                closure_date = uiDate2.ToString("yyyy-MM-dd");
                            }
                            
                           

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
                                        "'" + values.leadbank_gid + "'," +
                                        "'" + values.customer_gid + "'," +
                                        "'" + values.leadbank_name.Replace("'", "\\\'") + "', " +
                                        "'" + values.contact_number + "'," +
                                        "'" + values.leadbankcontact_name + "'," +
                                        "'" + values.contact_email + "'," +
                                        "'" + values.customerbranch_name + "'," +
                                        "'" + values.contact_address.Replace("'", "\\\'") + "'," +
                                         "'" + lsenquiry_type + "'," +
                                        "'" + enquiry_date + "', " +
                                        "'" + values.enquiry_remarks.Replace("'", "\\\'") + "', " +
                                        "' " + lsenquiry_status + "'," +
                                        "'" + lsrefno + "', " +
                                        "'" + closure_date + "', ";
                            msSQL += "'" + employee_gid + "', " +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + "', " +
                                     "'" + lspurchaseenquiry_flag + "'," +
                                     "'" + lspotential_value + "'," +
                                     "'" + values.customer_requirement.Replace("'", "\\\'") + "'," +
                                     "'" + values.landmark.Replace("'", "\\\'") + "'," +
                                     "'" + lslead_status + "'," +
                                     "'" + employee_gid + "', " +
                                     "'" + employeeGid + "', " +
                                     "'" + dt_datatable.Rows.Count + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        
                       
                        if(mnResult == 0)
                        {
                            values.status = false;
                            values.message = "Kindly Enter Valid Values";
                            return;
                        }
                       
                        else
                        {
                            //string uiDateStr = values.enquiry_date;
                            //DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //string enquiry_date = uiDate.ToString("yyyy-MM-dd");

                            string lsenquiry_flag = "PR Pending Approval";

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
                                " enquiry_receivedby, " +
                                " created_date, " +
                                " purchaseenquiry_flag, " +
                                " enquiry_assignedby, " +
                                " product_count)" +
                                " values (" +
                                "'" + msGetGid1 + "', " +
                                "'" + values.branch_name + "'," +
                                "'" + values.leadbank_gid + "'," +
                                "'" + values.customer_gid + "'," +
                                "'" + values.leadbank_name.Replace("'", "\\\'") + "'," +
                                "'" + values.contact_number + "'," +
                                "'" + values.contact_person + "'," +
                                "'" + values.contact_email + "'," +
                                "'" + values.customerbranch_name + "'," +
                                "'" + values.contact_address.Replace("'", "\\\'") + "'," +
                                "'" + enquiry_date + "', " +
                                "'" + values.enquiry_remarks.Replace("'", "\\\'") + "', " +
                                "' " + lsenquiry_status + "'," +
                                "'" + lsrefno + "', " +
                                "'" + values.customer_requirement.Replace("'", "\\\'") + "'," +
                                "'" + values.landmark.Replace("'", "\\\'") + "'," +
                                "'" + employee_gid + "', " +
                                "'" + employeeGid + "', " +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + "', " +
                                "' " + lsenquiry_flag + "', " +
                                "'" + employee_gid + "', " +
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
                            msSQL = "select distinct enquiry_type from smr_tmp_tsalesenquiry where created_by='" + employee_gid + "' ";
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
                           
                        }
                    }

                    string lslead = "Open";

                    msSQL = " update smr_trn_tsalesenquiry set enquiry_type='" + lsenquiry_type + "' where enquiry_gid='" + msGetGid1 + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update acp_trn_tenquiry set enquiry_type='" + lsenquiry_type + "' where enquiry_gid='" + msGetGid1 + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    msgetlead2campaign_gid = objcmnfunctions.GetMasterGID("BLCC");

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
                                                      " so_status, " +
                                                      " created_by, " +
                                                      " customer_gid, " +
                                                      " created_date, " +
                                                      " lead_status, " +
                                                      " customer_rating, " +                                                      
                                                      " campaign_gid," +
                                                      " assign_to ) " +
                                                      " Values ( " +
                                                      "'" + msgetlead2campaign_gid + "'," +
                                                      "'" + msGetGid1 + "'," +
                                                      "'" + lsso + "'," +
                                                      "'" + employee_gid + "'," +
                                                      "'" + values.leadbank_gid  + "'," +
                                                      "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                      "' " + lslead + " '," +
                                                      "'" + values.customer_rating + "'," +                                                      
                                                      "'" + campaignGid + "'," +
                                                      "'" + employeeGid + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    lslead_status = "Enquiry Assigned";
                        msSQL = " update smr_trn_tsalesenquiry Set " +
                                   " lead_status = '" + lslead_status + "' " +
                                   " where enquiry_gid = '" + msGetGid1 + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        //msSQL = " select customer_gid from crm_trn_tleadbank " +
                        //   " where leadbank_gid='" + values.leadbank_gid + "'";
                        //dt_datatable = objdbconn.GetDataTable(msSQL);
                        //if (dt_datatable.Rows.Count != 0)
                        //{
                        //    foreach (DataRow dt in dt_datatable.Rows)
                        //    {
                        //        if (DBNull.Value.Equals(dt["customer_gid"]))
                        //        {
                        //            lsleadstage = "1";
                        //        }
                        //        else
                        //        {
                        //            msSQL = " select enquiry_gid from smr_trn_tsalesenquiry where customer_gid='" + values.customer_gid + "'";
                        //            dt_datatable = objdbconn.GetDataTable(msSQL);
                        //            if (dt_datatable.Rows.Count != 0)
                        //            {
                        //                lsleadstage = "3";
                        //            }

                        //        }

                        //    msSQL = " update crm_trn_tenquiry2campaign  set " +
                        //             " leadstage_gid='" + lsleadstage + "'" +
                        //             " where lead2campaign_gid='" + msgetlead2campaign_gid + "'";
                        //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        //    if (mnResult != 0)
                        //    {
                        //        msSQL = " update crm_trn_tlead2campaign  set " +
                        //                      " leadstage_gid='6'" +
                        //                      " where leadbank_gid='" + lsleadbank_gid + "'";
                        //        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        //    }
                        //    }
                        //}


                   
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


        public void DaGetSmrTrnRaiseProposal(string enquiry_gid, MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
              
                msSQL = "select customer_name,customer_gid, enquiry_gid from smr_trn_tsalesenquiry where enquiry_gid='" + enquiry_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<proposal_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new proposal_list
                        {
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_gid = dt["customer_gid"].ToString()

                        });
                        values.proposal_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetDocumentType(MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
                
                msSQL = " select a.template_gid, a.template_name, a.template_content from adm_mst_ttemplate a " +
                " left join adm_mst_ttemplatetype b on b.templatetype_gid = a.templatetype_gid " +
                " where a.templatetype_gid='1' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<document_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new document_list
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                        });
                        values.document_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Template Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaUploaddocument(HttpRequest httpRequest, string user_gid, MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
             
                msSQL = " select doc_gid,file_name,file_path,temp_gid" +
                        " FROM crm_mst_ttemplatedoc where doc_gid='";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<uploaddocument>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new uploaddocument
                        {

                            file_name = dt["file_name"].ToString(),
                            doc_gid = dt["doc_gid"].ToString(),
                            file_path = dt["document_path"].ToString(),
                        });
                        values.uploaddocument = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting File Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetProposalSummary(string enquiry_gid, MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
                
                msSQL = " SELECT a.proposal_gid,b.leadbank_gid,date_format(a.created_date,'%d-%m-%Y') as created_date, a.enquiry_gid, a.template_name, concat(c.user_firstname,' ',c.user_lastname) as created_by,b.customer_name,b.customer_gid,a.proposal_from,a.document_path " +
                 " FROM crm_mst_tproposaltemplate a" +
                 " LEFT JOIN smr_trn_tsalesenquiry b ON b.enquiry_gid = a.enquiry_gid " +                 
                 " left join adm_mst_tuser c on c.user_gid= a.created_by " +
                 " where a.enquiry_gid='" + enquiry_gid + "' order by a.proposal_gid";



                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<proposalsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new proposalsummary_list
                        {
                            enquiry_gid = dt["enquiry_gid"].ToString(),                           
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            proposal_gid = dt["proposal_gid"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            proposal_from = dt["proposal_from"].ToString(),
                            document_path = dt["document_path"].ToString(),


                        });
                        values.proposalsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Proposal Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }


        public void DaPostproposal(HttpRequest httpRequest, result objResult, string user_gid)
        {
            try
            {
              
                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gid = string.Empty;
                MemoryStream ms_stream = new MemoryStream();
                string document_gid = string.Empty;
                string lscompany_code = string.Empty;
                HttpPostedFile httpPostedFile;

                string lspath;
                string msGetGid;
                string enquiry_gid = httpRequest.Form["enquiry_gid"];
              
                string proposal_name = httpRequest.Form["proposal_name"];
                string template_content = httpRequest.Form["template_content"];
               
                msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);


                MemoryStream ms = new MemoryStream();
                lspath = ConfigurationManager.AppSettings["upload_file"] + "erpdocument" + "/" + lscompany_code + "/" + "RaiseProposal/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                {
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                }
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        string lsfirstdocument_filepath = string.Empty;
                        httpFileCollection = httpRequest.Files;
                        for (int i = 0; i < httpFileCollection.Count; i++)
                        {
                            string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                            httpPostedFile = httpFileCollection[i];
                            string FileExtension = httpPostedFile.FileName;
                            string lsfile_gid = msdocument_gid;
                            string lscompany_document_flag = string.Empty;
                            FileExtension = Path.GetExtension(FileExtension).ToLower();
                            lsfile_gid = lsfile_gid + FileExtension;
                            Stream ls_readStream;
                            ls_readStream = httpPostedFile.InputStream;
                            ls_readStream.CopyTo(ms);

                            lspath = ConfigurationManager.AppSettings["upload_file"] + "/erpdocument" + "/" + lscompany_code + "/" + "RaiseProposal/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                            string status;
                            status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);

                            ms.Close();
                            lspath = "assets/media/images/erpdocument" + "/" + lscompany_code + "/" + "RaiseProposal/upload/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension; ; ;

                            string final_path = lspath + msdocument_gid + FileExtension;



                            msGetGid = objcmnfunctions.GetMasterGID("BPPL");


                            msSQL = " Insert into crm_mst_tproposaltemplate ( " +
                                                           " proposal_gid, " +
                                                           " enquiry_gid, " +
                                                           " template_name, " +
                                                           " template_content, " +
                                                           " document_path, " +
                                                           " proposal_from," +
                                                           " created_date, " +
                                                           " created_by ) " +
                                                           " Values ( " +
                                                           "'" + msGetGid + "'," +
                                                           "'" + enquiry_gid + "'," +
                                                           "'" + proposal_name + "'," +
                                                           "'" + template_content + "'," +
                                                           "' " + lspath + " ',";
                                                           if(enquiry_gid != null || enquiry_gid != "")
                                                           {
                                                             msSQL += "'Enquiry',";
                                                            }
                                                           else
                                                                {
                                                                msSQL += "'Quotation',";
                                                                }
                                                         msSQL +=  "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                           "'" + user_gid + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                msSQL = " Insert into smr_trn_tproposal ( " +
                                                          " proposal_gid, " +                                                         
                                                          " enquiry_gid, " +                                                         
                                                          " proposal_title, " +
                                                          " content, " +
                                                          " document_path, " +
                                                          " proposal_from," +
                                                          " created_date, " +
                                                          " created_by ) " +
                                                          " Values ( " +
                                                          "'" + msGetGid + "'," +                                                          
                                                          "'" + enquiry_gid + "'," +                                                         
                                                          "'" + proposal_name + "'," +
                                                          "'" + template_content + "'," +
                                                          "' " + lspath + " ',";
                                                         if (enquiry_gid != null || enquiry_gid != "")
                                                            {
                                                                    msSQL += "'Enquiry',";
                                                                }
                                                            else
                                                            {
                                                                msSQL += "'Quotation',";
                                                            }

                               msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                          "'" + user_gid + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                                if (mnResult != 0)
                            {
                                objResult.status = true;
                                objResult.message = "Proposal Raised  Successfully";
                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error While Raising Proposal";
                            }

                        }

                    }

                }
                catch (Exception ex)
                {
                    objResult.message = ex.Message.ToString();
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Proposal Raised!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           


        }


        public void DaGetViewEnquirySummary(string enquiry_gid, MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
              
                msSQL = "select distinct a.enquiry_gid,d.customercontact_gid,a.enquiry_date,b.branch_gid,b.branch_name,a.leadbank_gid," +
              " a.customer_name,a.customer_gid,  a.contact_number, a.contact_email, a.contact_address,l.user_firstname," +
              " x.customer_address, x.customer_address2,x.customer_city,x.customer_state, e.country_name, x.customer_pin,  d.mobile, d.email,a.contact_person," +
              " a.customerbranch_gid, a.enquiry_remarks, a.enquiry_referencenumber, a.customer_requirement,a.landmark,a.closure_date," +
              " format(g.potential_value, 2) as potential_value,g.qty_enquired,g.product_requireddate," +
              " i.product_code,i.product_name,c.productuom_name,k.productgroup_name " +
              " from smr_trn_tsalesenquiry a " +
              " left join hrm_mst_tbranch b on a.branch_gid = b.branch_gid " +
              " left join crm_trn_tleadbank h on a.customer_gid = h.customer_gid " +
              " left join crm_mst_tcustomer x on h.customer_gid = x.customer_gid " +
              " left join crm_mst_tcustomercontact d on d.customer_gid = x.customer_gid " +
              " left join adm_mst_tcountry e on x.customer_country = e.country_gid " +
              " left join crm_trn_tenquiry2campaign f on a.enquiry_gid = f.enquiry_gid " +
              " left join smr_trn_tsalesenquirydtl g  on a.enquiry_gid = g.enquiry_gid " +
              " left join pmr_mst_tproduct i on g.product_gid = i.product_gid " +
              " left join pmr_mst_tproductuom c on g.uom_gid = c.productuom_gid " +
              " left join pmr_mst_tproductgroup k on g.productgroup_gid = k.productgroup_gid " +
              " left join hrm_mst_temployee j on f.assign_to = j.employee_gid " +
              " left join adm_mst_tuser l on j.user_gid = l.user_gid " +
              " where a.enquiry_gid = '" + enquiry_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<enquirylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new enquirylist
                        {
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            enquiry_date = dt["enquiry_date"].ToString().Replace("00:00:00", ""),
                            branch_name = dt["branch_name"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            enquiry_referencenumber = dt["enquiry_referencenumber"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_number = dt["contact_number"].ToString(),
                            contact_person = dt["contact_person"].ToString(),
                            assign_to = dt["user_firstname"].ToString(),
                            contact_email = dt["contact_email"].ToString(),
                            enquiry_remarks = dt["enquiry_remarks"].ToString(),
                            contact_address = dt["contact_address"].ToString(),
                            customer_requirement = dt["customer_requirement"].ToString(),
                            landmark = dt["landmark"].ToString(),
                            closure_date = dt["closure_date"].ToString().Replace("00:00:00", ""),
                            productgroup_name = dt["productgroup_name"].ToString(),                           
                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),                            
                            productuom_name = dt["productuom_name"].ToString(),
                            qty_requested = dt["qty_enquired"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            product_requireddate = dt["product_requireddate"].ToString().Replace("00:00:00", ""),                            
                            customer_gid = dt["customer_gid"].ToString(),

                        });

                        values.enquiry_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while ViewEnquirySummary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");


            }
           
        }

        public void DaGetOnEditCustomerName(string customercontact_gid, MdlSmrTrnCustomerEnquiry values)
        {
            try
            {
               
                if (customercontact_gid != null)
                {
                    msSQL = "select a.customer_gid,a.customercontact_gid from crm_mst_tcustomercontact a left join crm_mst_tcustomer " +
                        "b on a.customer_gid=b.customer_gid where customer_name='" + customercontact_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                       
                        lscustomer_gid = objOdbcDataReader["customer_gid"].ToString();

                        objOdbcDataReader.Close();


                    }

                    msSQL = " select a.customercontact_gid,concat(a.address1, '   ', a.city, '   ', a.state, '   ', a.zip_code) as address1,ifnull(a.address2, '') as address2," +
                           " ifnull(a.city, '') as city,  ifnull(a.state, '') as state,ifnull(a.country_gid, '') as country_gid,ifnull(a.zip_code, '') as zip_code, " +
                           " ifnull(a.mobile, '') as mobile,ifnull(a.email, '') as email,ifnull(b.country_name, '') as country_name,a.customerbranch_name, " +
                           " concat(a.customercontact_name) as customercontact_names  from crm_mst_tcustomercontact a left join adm_mst_tcountry b on a.country_gid = b.country_gid " +
                           " where a.customer_gid = '" + lscustomer_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetCustomer>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetCustomer
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
                                customercontact_gid = dt["customercontact_gid"].ToString(),

                            });
                            values.GetCustomer = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Edit CustomerName !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }


  

        // DIRECT ENQUIRY PRODUCT EDIT

        public void DaGetDirectEnquiryEditProductSummary(string tmpsalesenquiry_gid, MdlSmrTrnCustomerEnquiry values)
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
                var getModuleList = new List<DirecteditenquiryList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new DirecteditenquiryList
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
                        values.directeditenquiry_list = getModuleList;
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

        // PRODUCT UPDATE DIRECT ENQUIRY 

        public void DaPostUpdateEnquiryProduct(string employee_gid, productsummarys_list values)
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

        public void DaGetCustomer360(string leadbank_gid,MdlSmrTrnCustomerEnquiry values)
        {

            try
            {
                if (leadbank_gid.Contains("BCRM"))
                {

                    msSQL = "Select customer_gid, customer_name " +
                         " from crm_mst_tcustomer  " +
                         "where customer_gid='" + leadbank_gid + "'";


                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetCustomername>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetCustomername
                            {
                                customer_name = dt["customer_name"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),
                            });
                            values.GetCustomername = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
                else
                {
                    msSQL = "Select customer_gid " +
                         " from crm_trn_tleadbank  " +
                         "where leadbank_gid='" + leadbank_gid + "'";
                    string lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);



                    msSQL = "Select customer_gid, customer_name " +
                         " from crm_mst_tcustomer  " +
                         "where customer_gid='" + lscustomer_gid + "'";


                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetCustomername>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetCustomername
                            {
                                customer_name = dt["customer_name"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),
                            });
                            values.GetCustomername = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetOnChangeCustomerName360(string customer_gid, MdlSmrTrnCustomerEnquiry values)
        {
            try
            {

                msSQL = " select c.customer_name,a.customercontact_gid,concat(a.address1,'   ',a.city,'   ',a.state,'   ',a.zip_code) as address1,ifnull(a.address2,'') as address2,ifnull(a.city,'') as city, " +
                    " ifnull(a.state,'') as state,ifnull(a.country_gid,'') as country_gid,ifnull(a.zip_code,'') as zip_code, " +
                    " ifnull(a.mobile,'') as mobile,ifnull(a.email,'') as email,ifnull(b.country_name,'') as country_name,a.customerbranch_name,concat(a.customercontact_name) as " +
                    " customercontact_names, c.customer_gid " +
                    " from crm_mst_tcustomercontact a " +
                    " left join crm_mst_tcustomer c on a.customer_gid=c.customer_gid " +
                    " left join adm_mst_tcountry b on a.country_gid=b.country_gid " +
                    " where c.customer_gid='" + customer_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetCustomer>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetCustomer
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
                                customercontact_gid = dt["customercontact_gid"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),
                                customer_name = dt["customer_name"].ToString(),

                            });
                            values.GetCustomer = getModuleList;
                        }
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

        // PROPOSAL DELETE

        public void DaDeleteProposal(string proposal_gid,productsummarys_list values)
        {
            
            msSQL = " delete from crm_mst_tproposaltemplate where proposal_gid='" + proposal_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {

                msSQL = " delete from smr_trn_tproposal where proposal_gid='" + proposal_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }

            if(mnResult !=0)
            {
                values.status = true;
                values.message = " Proposal Deleted Successfully";
                return;
            }
            else
            {
                values.status = false;
                values.message = " Error While Deleting Proposal";
                return;
            }


        }


        //Lead To Customer page

        public void DaGetLeadSummary(string leadbank_gid,MdlSmrTrnCustomerEnquiry values)
        {
            try
            {

                msSQL = "  Select distinct concat(a.enquiry_gid,' / ',a.enquiry_type) as enquiry_refno,m.leadbank_name,n.leadbankcontact_name," +
                    " n.leadbankcontact_gid,format(a.potorder_value, 2) as potorder_value, m.customer_gid," +
                    " concat(s.source_name, ' / ', m.referred_by) as source_name,z.branch_prefix," +
                    " concat(f.user_firstname, ' ', f.user_lastname) as campaign," +
                    " a.enquiry_gid,DATE_FORMAT(a.enquiry_date, '%d-%m-%Y') as enquiry_date," +
                    " a.leadbank_gid,a.customer_name,a.branch_gid, a.lead_status,z.branch_name," +
                    " concat(o.region_name, ' / ', m.leadbank_city, ' / ', m.leadbank_state) as region_name," +
                    " n.city as leadbank_city, n.state as leadbank_state," +
                    " concat(f.user_firstname, ' - ', f.user_lastname) as assign_to,a.enquiry_referencenumber," +
                    " a.enquiry_status,a.enquiry_type,a.enquiry_remarks,a.potorder_value ,a.created_date ," +
                    " a.contact_person,a.contact_email,a.contact_address, T.customer_rating," +
                    " case when a.contact_person is null then concat(n.leadbankcontact_name,' / ',n.mobile,' / ',n.email) when a.contact_person is not null then" +
                    " concat(a.customerbranch_gid,' | ',a.contact_person,' | ',a.contact_number,' | ',a.contact_email) end as contact_details," +
                    " a.contact_number,a.contact_email,n.pincode as leadbank_pin,n.address1,n.address2," +
                    " a.enquiry_referencenumber, a.enquiry_type from  crm_trn_tleadbank m " +
                    " left join smr_trn_tsalesenquiry a  on a.leadbank_gid = m.leadbank_gid" +
                    " left join crm_trn_tleadbankcontact n on n.leadbank_gid = m.leadbank_gid" +
                    " left join crm_mst_tregion o on m.leadbank_region = o.region_gid" +
                    " left join crm_trn_tenquiry2campaign p on p.customer_gid = a.customer_gid " +
                    " left join crm_mst_tleadstage r on r.leadstage_gid = p.leadstage_gid" +
                    " left join smr_trn_tcampaign q on q.campaign_gid = p.campaign_gid" +
                    " left join hrm_mst_temployee d on d.employee_gid = p.assign_to" +
                    " left join adm_mst_tuser b on b.user_gid = d.user_gid" +
                    " left join hrm_mst_temployee k on k.employee_gid = a.enquiry_receivedby" +
                    " left join adm_mst_tuser f on f.user_gid = k.user_gid" +
                    " left join hrm_mst_tbranch z on a.branch_gid = z.branch_gid" +
                    " left join crm_mst_tsource s on s.source_gid = m.source_gid" +
                    " left join crm_trn_tenquiry2campaign t on a.enquiry_gid = t.enquiry_gid" +
                    " where m.leadbank_gid = '" + leadbank_gid  + "'" +
                    " group by a.enquiry_gid order by a.enquiry_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCusEnquiry>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCusEnquiry
                        {
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            customer_gid = dt["leadbank_gid"].ToString(),
                            customer = dt["customer_gid"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            customercontact_gid = dt["leadbankcontact_gid"].ToString(),
                            enquiry_date = dt["enquiry_date"].ToString(),
                            enquiry_refno = dt["enquiry_refno"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            enquiry_referencenumber = dt["enquiry_referencenumber"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            created_by = dt["campaign"].ToString(),
                            potorder_value = dt["potorder_value"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            enquiry_status = dt["enquiry_status"].ToString(),
                            customer_rating = dt["customer_rating"].ToString(),
                            assign_to = dt["assign_to"].ToString(),
                            branch_prefix = dt["branch_prefix"].ToString(),
                            contact_address = dt["contact_address"].ToString(),
                            contact_email = dt["contact_email"].ToString(),
                            contact_number = dt["contact_number"].ToString(),
                            leadbank_state = dt["leadbank_state"].ToString(),
                            leadbank_city = dt["leadbank_city"].ToString(),
                            leadbank_pin = dt["leadbank_pin"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                        });
                        values.cusenquiry_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Customer Enquiry !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        // Lead to Customer Submit

        public void DaPostlead(string user_gid, postlead_list values)

        {
            try
            {
                    msGetGid01 = objcmnfunctions.GetMasterGID("CC");
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='CC'";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string lscustomer_code = "CC-" + "00" + lsCode;
                    string lscustomercode = "H.Q";
                    string lscustomer_branch = "H.Q";


                    //msSQL = " Select customer_type from crm_mst_tcustomertype where customertype_gid='" + values.customer_type + "'";
                    //string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);

                    
                        msGetGid = objcmnfunctions.GetMasterGID("BCRM");
                        msGetGid1 = objcmnfunctions.GetMasterGID("BCCM");
                        msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                        msGetGid3 = objcmnfunctions.GetMasterGID("BLCC");
                        msSQL = " insert into crm_mst_tcustomer (" +
                           " customer_gid," +
                           " customer_id, " +
                            " customer_code, " +
                           " customer_name, " +
                           " company_website, " +
                           " customer_address, " +
                           " customer_address2," +
                           " customer_city," +

                           " customer_country," +
                           " customer_region," +
                           " customer_state," +
                           " gst_number ," +
                           " customer_pin ," +
                           " customer_type ," +
                           " status ," +
                           " taxsegment_gid ," +
                           " pricesegment_gid ," +
                              " salesperson_gid ," +
                                " credit_days ," +
                                      " currency_gid ," +
                           " created_by," +
                           "created_date" +
                            ") values (" +
                           "'" + msGetGid + "', " +
                           "'" + lscustomer_code + "'," + 
                           "'" + lscustomer_code + "'," +
                            //"'H.Q'," +
                           "'" + values.leadbank_name.Replace("'", "\\\'") + "'," +
                           "'" + values.company_website + "'," +
                           "'" + values.address1.Replace("'", "\\\'") + "',";
                        if (values.address2 != null)
                        {

                            msSQL += "'" + values.address2.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.address2 + "',";
                        }
                        msSQL += "'" + values.customer_city + "'," +

                          "'" + values.countryname + "'," +
                          "'" + values.region_name + "'," +
                          "'" + values.customer_state + "'," +
                          "'" + values.tax_no + "'," +
                          "'" + values.postal_code + "'," +
                           "'" + values.customer_type + "'," +
                            "'Active'," +
                             "'" + values.taxsegment_name + "'," +
                             "'" + values.pricesegment_name + "'," +
                                "'" + values.sales_person + "',";
                        if (values.credit_days == null || values.credit_days == "")
                        {
                            msSQL += "'" + 0 + "',";
                        }
                        else
                        {

                            msSQL += "'" + values.credit_days + "',";
                        }
                        msSQL += "'" + values.currency + "'," +
                        "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        if (mnResult != 0)
                        {
                            //objfinance.finance_vendor_debitor("Sales", exclcustomer_code, values.customer_name, msGetGid, user_gid);
                            //string trace_comment = " Added a customer on " + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //objcmnfunctions.Tracelog(msGetGid, user_gid, trace_comment, "added_customer");

                            if (values.pricesegment_name != null || values.pricesegment_name != "undefined" || values.pricesegment_name != "")
                            {

                                msGetGID11 = objcmnfunctions.GetMasterGID("VPDC");
                                msSQL = "insert into smr_trn_tpricesegment2customer (" +
                                    "pricesegment2customer_gid," +
                                    "pricesegment_gid," +
                                    "customer_gid," +
                                    "customer_name," +
                                    "created_by," +
                                    "created_date " +
                                    ") values (" +
                                    "'" + msGetGID11 + "'," +
                                    "'" + values.pricesegment_name + "'," +
                                    "'" + msGetGid + "'," +
                                    "'" + values.leadbank_name.Replace("'", "\\\'") + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                            msSQL = " insert into crm_mst_tcustomercontact  (" +
                            " customercontact_gid," +
                            " customer_gid," +
                            " customercontact_name, " +
                            " customerbranch_name, " +
                            " email, " +
                              " billing_email, " +
                            " mobile, " +
                             " main_contact, " +
                            " designation," +
                            " address1," +
                            " address2," +
                            " state," +
                            " city," +
                            " country_gid," +
                            " region," +
                            " fax, " +
                            " zip_code, " +

                            " fax_country_code," +
                            " gst_number, " +
                             " created_by," +
                           "created_date" +

                            ") values (" +
                            "'" + msGetGid1 + "', " +
                            "'" + msGetGid + "', " +
                            "'" + values.leadbankcontact_name + "'," +
                            "'" + lscustomer_branch + "'," +
                            "'" + values.email + "'," +
                              "'" + values.billemail + "',";
                            if (values.mobiles != null)
                            {

                                msSQL += "'" + values.mobiles.e164Number + "',";
                            }
                            else
                            {
                                msSQL += "'',";
                            }
                            msSQL += "'Y'," +
                            "'" + values.designation + "'," +
                            "'" + values.address1.Replace("'", "\\\'") + "',";

                            if (values.address2 != null)
                            {

                                msSQL += "'" + values.address2.Replace("'", "\\\'") + "',";
                            }
                            else
                            {
                                msSQL += "'" + values.address2 + "',";
                            }
                            msSQL += "'" + values.customer_state + "'," +
                            "'" + values.customer_city + "'," +
                             "'" + values.countryname + "'," +
                            "'" + values.region_name + "'," +
                             "'" + values.fax + "'," +
                             "'" + values.postal_code + "'," +

                             "'" + values.country_code + "'," +
                           "'" + values.gst_number + "'," +
                           "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        if(mnResult == 1)
                        {
                            msSQL = "update crm_trn_tleadbank set customer_gid='"+ msGetGid + "' " +
                                    " where leadbank_gid = '"+values.customer_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = "update smr_trn_tsalesenquiry set customer_gid='" + msGetGid + "' " +
                                           " where leadbank_gid = '" + values.customer_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Customer Added Successfully";
                            return;
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Customer";
                            return;
                        }
                    
                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Customer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
    }
}  