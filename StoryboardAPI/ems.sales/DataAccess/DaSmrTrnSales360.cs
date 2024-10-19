using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Diagnostics.Contracts;
using System.Drawing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
//using System.Windows.Media.TextFormatting;
//using OfficeOpenXml.Drawing.Slicer.Style;


namespace ems.sales.DataAccess
{
    public class DaSmrTrnSales360
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lscampaign_location, lscustomergid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        // CUSTOMER DETAILS

        public void DaGetCustomerDetails(string customer_gid, MdlSmrTrnSales360 values)
        {

            try
            {
                if (customer_gid.Contains("BCRM"))
                {
                    msSQL = " select c.leadbank_gid from crm_mst_tcustomer a" +
                            " left join crm_trn_tleadbank c on a.customer_gid = c.customer_gid " +
                            " where a.customer_gid='" + customer_gid + "'";
                    string lscustomergid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " SELECT a.customer_gid,c.leadbank_gid,d.source_name,f.leadbankcontact_gid,b.designation,a.customer_name,e.region_name,a.customer_id,g.customer_type,date_format(a.created_date, '%d-%m-%Y') as created_date," +
                            " a.customer_name,b.address1, b.email,b.customercontact_name, b.mobile from crm_mst_tcustomer a" +
                            " left join crm_mst_tcustomercontact b on a.customer_gid = b.customer_gid " +
                            " left join crm_trn_tleadbank c on a.customer_gid = c.customer_gid " +
                            " left join crm_mst_tsource d on c.source_gid = d.source_gid " +
                            " left join crm_mst_tcustomertype g on a.customer_type = g.customertype_gid " +
                            " left join crm_mst_tregion e on c.leadbank_region = e.region_gid " +
                            " left join crm_trn_tleadbankcontact f on c.leadbank_gid = f.leadbank_gid " +
                            " where a.customer_gid='" + customer_gid + "' group by a.customer_gid";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<overall_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new overall_list
                            {
                                customer_gid = dt["customer_gid"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                customer_name = dt["customer_name"].ToString(),
                                customer_id = dt["customer_id"].ToString(),
                                customercontact_name = dt["customercontact_name"].ToString(),
                                customercontact_gid = dt["leadbankcontact_gid"].ToString(),
                                emailid = dt["email"].ToString(),
                                mobile = dt["mobile"].ToString(),
                                customer_type = dt["customer_type"].ToString(),
                                source = dt["source_name"].ToString(),
                                region = dt["region_name"].ToString(),
                                created_date = dt["created_date"].ToString(),
                                designation = dt["designation"].ToString(),
                            });
                            values.overalllist = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();


                }

                else
                {

                    msSQL = " SELECT a.customer_gid,c.leadbank_gid,d.source_name,f.leadbankcontact_gid,b.designation,a.customer_name,e.region_name,a.customer_id,g.customer_type,date_format(a.created_date, '%d-%m-%Y') as created_date," +
                            " a.customer_name,b.address1, b.email,b.customercontact_name, b.mobile from crm_mst_tcustomer a" +
                            " left join crm_mst_tcustomercontact b on a.customer_gid = b.customer_gid " +
                            " left join crm_trn_tleadbank c on a.customer_gid = c.customer_gid " +
                            " left join crm_mst_tsource d on c.source_gid = d.source_gid " +
                             " left join crm_mst_tcustomertype g on a.customer_type = g.customertype_gid " +
                            " left join crm_mst_tregion e on c.leadbank_region = e.region_gid " +
                            " left join crm_trn_tleadbankcontact f on c.leadbank_gid = f.leadbank_gid " +
                            " where c.leadbank_gid='" + lscustomergid + "' group by a.customer_gid";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<overall_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new overall_list
                            {
                                customer_gid = dt["customer_gid"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                customer_name = dt["customer_name"].ToString(),
                                customer_id = dt["customer_id"].ToString(),
                                customercontact_name = dt["customercontact_name"].ToString(),
                                customercontact_gid = dt["leadbankcontact_gid"].ToString(),
                                emailid = dt["email"].ToString(),
                                mobile = dt["mobile"].ToString(),
                                customer_type = dt["customer_type"].ToString(),
                                source = dt["source_name"].ToString(),
                                region = dt["region_name"].ToString(),
                                created_date = dt["created_date"].ToString(),
                                designation = dt["designation"].ToString(),
                            });
                            values.overalllist = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Team Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // ENQUIRY DETAILS

        public void DaGet360EnquiryDetails(string customer_gid, MdlSmrTrnSales360 values)
        {
            try
            {
                if (customer_gid.Contains("BCRM"))
                {
                    msSQL = " select c.leadbank_gid from crm_mst_tcustomer a" +
                            " left join crm_trn_tleadbank c on a.customer_gid = c.customer_gid " +
                            " where a.customer_gid='" + customer_gid + "'";
                    string lscustomergid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " Select distinct concat(a.enquiry_gid,' / ',a.enquiry_type) as enquiry_refno,m.leadbank_gid,m.customer_gid,format(a.potorder_value, 2) as potorder_value, " +
                        " concat(s.source_name, ' / ', m.referred_by) as source_name, a.enquiry_gid,DATE_FORMAT(a.enquiry_date, '%d-%m-%Y') as enquiry_date, " +
                        " concat(h.user_firstname, ' ', h.user_lastname) as campaign, a.customer_name,a.branch_gid, a.customer_gid,a.lead_status," +
                        " z.branch_name, concat(o.region_name, ' / ', m.leadbank_city, ' / ', m.leadbank_state) as region_name,f.user_firstname as assign_to, " +
                        " a.enquiry_referencenumber,a.enquiry_status,a.enquiry_type,a.enquiry_remarks,a.potorder_value ,a.created_date , " +
                        " a.contact_person,a.contact_email,a.contact_address, p.customer_rating,case when a.contact_person is null then concat(n.leadbankcontact_name,' / ',n.mobile,' / ',n.email) " +
                        " when a.contact_person is not null then concat(a.customerbranch_gid,' | ',a.contact_person,' | ',a.contact_number,' | ',a.contact_email) end as contact_details, " +
                        " a.enquiry_referencenumber, a.enquiry_type from smr_trn_tsalesenquiry a " +
                        " left join crm_trn_tleadbank m on m.customer_gid = a.customer_gid " +
                        " left join crm_trn_tleadbankcontact n on n.leadbank_gid = m.leadbank_gid " +
                        " left join crm_mst_tregion o on m.leadbank_region = o.region_gid " +
                        " left join crm_trn_tenquiry2campaign p on p.enquiry_gid = a.enquiry_gid " +
                        " left join crm_mst_tleadstage r on r.leadstage_gid = p.leadstage_gid " +
                        " left join smr_trn_tcampaign q on q.campaign_gid = p.campaign_gid " +
                        " left join hrm_mst_temployee d on d.employee_gid = p.assign_to " +
                        " left join adm_mst_tuser b on b.user_gid = d.user_gid " +
                        " left join hrm_mst_temployee k on k.employee_gid = a.created_by " +
                        " left join adm_mst_tuser f on f.user_gid = d.user_gid " +
                        " left join hrm_mst_temployee g on a.enquiry_assignedby = g.employee_gid" +
                        " left join adm_mst_tuser h on g.user_gid = h.user_gid " +
                        " left join hrm_mst_tbranch z on a.branch_gid = z.branch_gid " +
                        " left join crm_mst_tsource s on s.source_gid = m.source_gid " +
                        " where a.customer_gid='" + customer_gid + "'" +
                        " group by a.enquiry_gid order by a.enquiry_gid desc";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<Getenquirydetails_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new Getenquirydetails_list
                            {
                                enquiry_gid = dt["enquiry_gid"].ToString(),
                                customer_gid = dt["leadbank_gid"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                enquiry_date = dt["enquiry_date"].ToString(),
                                branch_name = dt["branch_name"].ToString(),
                                customer_name = dt["customer_name"].ToString(),
                                created_date = dt["created_date"].ToString(),
                                enquiry_referencenumber = dt["enquiry_referencenumber"].ToString(),
                                //contact_details = dt["contact_details"].ToString(),
                                created_by = dt["campaign"].ToString(),
                                potorder_value = dt["potorder_value"].ToString(),
                                lead_status = dt["lead_status"].ToString(),
                                enquiry_status = dt["enquiry_status"].ToString(),
                                customer_rating = dt["customer_rating"].ToString(),
                                assign_to = dt["assign_to"].ToString()

                            });
                            values.Getenquirydetailslist = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();

                }
                else
                { 
                

                    msSQL = " Select distinct concat(a.enquiry_gid,' / ',a.enquiry_type) as enquiry_refno,m.leadbank_gid,m.customer_gid,format(a.potorder_value, 2) as potorder_value, " +
                        " concat(s.source_name, ' / ', m.referred_by) as source_name, a.enquiry_gid,DATE_FORMAT(a.enquiry_date, '%d-%m-%Y') as enquiry_date, " +
                        " concat(h.user_firstname, ' ', h.user_lastname) as campaign, a.customer_name,a.branch_gid, a.customer_gid,a.lead_status," +
                        " z.branch_name, concat(o.region_name, ' / ', m.leadbank_city, ' / ', m.leadbank_state) as region_name,f.user_firstname as assign_to, " +
                        " a.enquiry_referencenumber,a.enquiry_status,a.enquiry_type,a.enquiry_remarks,a.potorder_value ,a.created_date , " +
                        " a.contact_person,a.contact_email,a.contact_address, p.customer_rating,case when a.contact_person is null then concat(n.leadbankcontact_name,' / ',n.mobile,' / ',n.email) " +
                        " when a.contact_person is not null then concat(a.customerbranch_gid,' | ',a.contact_person,' | ',a.contact_number,' | ',a.contact_email) end as contact_details, " +
                        " a.enquiry_referencenumber, a.enquiry_type from smr_trn_tsalesenquiry a " +
                        " left join crm_trn_tleadbank m on m.customer_gid = a.customer_gid " +
                        " left join crm_trn_tleadbankcontact n on n.leadbank_gid = m.leadbank_gid " +
                        " left join crm_mst_tregion o on m.leadbank_region = o.region_gid " +
                        " left join crm_trn_tenquiry2campaign p on p.enquiry_gid = a.enquiry_gid " +
                        " left join crm_mst_tleadstage r on r.leadstage_gid = p.leadstage_gid " +
                        " left join smr_trn_tcampaign q on q.campaign_gid = p.campaign_gid " +
                        " left join hrm_mst_temployee d on d.employee_gid = p.assign_to " +
                        " left join adm_mst_tuser b on b.user_gid = d.user_gid " +
                        " left join hrm_mst_temployee k on k.employee_gid = a.created_by " +
                        " left join adm_mst_tuser f on f.user_gid = d.user_gid " +
                        " left join hrm_mst_temployee g on a.enquiry_assignedby = g.employee_gid" +
                        " left join adm_mst_tuser h on g.user_gid = h.user_gid " +
                        " left join hrm_mst_tbranch z on a.branch_gid = z.branch_gid " +
                        " left join crm_mst_tsource s on s.source_gid = m.source_gid " +
                        " where m.leadbank_gid='" + lscustomergid + "'" +
                        " group by a.enquiry_gid order by a.enquiry_gid desc";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<Getenquirydetails_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getenquirydetails_list
                        {
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            customer_gid = dt["leadbank_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            enquiry_date = dt["enquiry_date"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            enquiry_referencenumber = dt["enquiry_referencenumber"].ToString(),
                            //contact_details = dt["contact_details"].ToString(),
                            created_by = dt["campaign"].ToString(),
                            potorder_value = dt["potorder_value"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            enquiry_status = dt["enquiry_status"].ToString(),
                            customer_rating = dt["customer_rating"].ToString(),
                            assign_to = dt["assign_to"].ToString()

                        });
                        values.Getenquirydetailslist = getModuleList;
                    }
                }
                     dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Customer Enquiry !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        // QUOTTAION DETAILS

        public void DaGetQuotationDetails(string customer_gid, MdlSmrTrnSales360 values)
        {
            try
            {

                if (customer_gid.Contains("BCRM"))
                {
                    msSQL = " select c.leadbank_gid from crm_mst_tcustomer a" +
                            " left join crm_trn_tleadbank c on a.customer_gid = c.customer_gid " +
                            " where a.customer_gid='" + customer_gid + "'";
                    string lscustomergid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency = 'Y'";


                    string currency = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select distinct d.leadbank_gid,a.quotation_gid, a.customer_gid,a.quotation_referenceno1,a.quotation_status," +
                                    " ifnull(concat(f.enquiry_gid , '|' , a.quotation_type ),'Direct Quotation') as enquiry_gid,f.enquiry_referencenumber,s.source_name,DATE_FORMAT(a.quotation_date, '%d-%m-%Y') as quotation_date,a.quotation_type,a.currency_code, " +
                                  "  format(a.Grandtotal, 2) as Grandtotal,g.user_firstname as salesperson, concat(c.user_firstname,' - ',c.user_lastname) as created_by, " +
                                  " case when a.currency_code = '" + currency + "' then a.customer_name " +
                                  " when a.currency_code is null then a.customer_name " +
                                  " when a.currency_code is not null and a.currency_code <> '" + currency + "' then (a.customer_name) end as customer_name, " +
                                  " concat(e.leadbankbranch_name,' / ',e.leadbankcontact_name,' / ',e.email,' / ',e.mobile,' / ',e.address1) as contact, " +
                                  " a.customer_address " +
                                  " from smr_trn_treceivequotation a " +
                                  " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                                  " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                                  " left join crm_trn_tleadbank d on d.customer_gid=a.customer_gid " +
                                  " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                                  " left join crm_trn_tleadbankcontact e on e.leadbank_gid=d.leadbank_gid " +
                                  " left join adm_mst_tuser g on a.salesperson_gid= g.user_gid   " +
                                  " left join crm_mst_tsource s on s.source_gid=d.source_gid" +
                                  " left join acp_trn_tenquiry f on a.enquiry_gid=f.enquiry_gid " +
                                  " where 1=1 and  a.customer_gid='" + customer_gid + "' order by a.quotation_gid desc";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<getquote_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new getquote_list
                            {
                                quotation_gid = dt["quotation_gid"].ToString(),
                                customer_gid = dt["leadbank_gid"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                quotation_date = dt["quotation_date"].ToString(),
                                quotation_referenceno1 = dt["quotation_gid"].ToString(),
                                //contact = dt["contact"].ToString(),
                                quotation_type = dt["quotation_type"].ToString(),
                                created_by = dt["created_by"].ToString(),
                                quotation_status = dt["quotation_status"].ToString(),
                                Grandtotal = dt["Grandtotal"].ToString(),
                                assign_to = dt["salesperson"].ToString()


                            });
                            values.getquotelist = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
                else
                {

                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency = 'Y'";
                    string currency = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select distinct d.leadbank_gid,a.quotation_gid, a.customer_gid,a.quotation_referenceno1,a.quotation_status," +
                                    " ifnull(concat(f.enquiry_gid , '|' , a.quotation_type ),'Direct Quotation') as enquiry_gid,f.enquiry_referencenumber,s.source_name,DATE_FORMAT(a.quotation_date, '%d-%m-%Y') as quotation_date,a.quotation_type,a.currency_code, " +
                                  "  format(a.Grandtotal, 2) as Grandtotal,g.user_firstname as salesperson, concat(c.user_firstname,' - ',c.user_lastname) as created_by, " +
                                  " case when a.currency_code = '" + currency + "' then a.customer_name " +
                                  " when a.currency_code is null then a.customer_name " +
                                  " when a.currency_code is not null and a.currency_code <> '" + currency + "' then (a.customer_name) end as customer_name, " +
                                  " concat(e.leadbankbranch_name,' / ',e.leadbankcontact_name,' / ',e.email,' / ',e.mobile,' / ',e.address1) as contact, " +
                                  " a.customer_address " +
                                  " from smr_trn_treceivequotation a " +
                                  " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                                  " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                                  " left join crm_trn_tleadbank d on d.customer_gid=a.customer_gid " +
                                  " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                                  " left join crm_trn_tleadbankcontact e on e.leadbank_gid=d.leadbank_gid " +
                                  " left join adm_mst_tuser g on a.salesperson_gid= g.user_gid   " +
                                  " left join crm_mst_tsource s on s.source_gid=d.source_gid" +
                                  " left join acp_trn_tenquiry f on a.enquiry_gid=f.enquiry_gid " +
                                  " where 1=1 and  d.leadbank_gid='" + lscustomergid + "' order by a.quotation_gid desc";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<getquote_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new getquote_list
                            {
                                quotation_gid = dt["quotation_gid"].ToString(),
                                customer_gid = dt["leadbank_gid"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                quotation_date = dt["quotation_date"].ToString(),
                                quotation_referenceno1 = dt["quotation_gid"].ToString(),
                                //contact = dt["contact"].ToString(),
                                quotation_type = dt["quotation_type"].ToString(),
                                created_by = dt["created_by"].ToString(),
                                quotation_status = dt["quotation_status"].ToString(),
                                Grandtotal = dt["Grandtotal"].ToString(),
                                assign_to = dt["salesperson"].ToString()


                            });
                            values.getquotelist = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Quotation Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        // ORDER DETAILS

        public void DaGetSalesOrderDetails(string customer_gid, MdlSmrTrnSales360 values)
        {
            try
            {

                if (customer_gid.Contains("BCRM"))
                {
                    msSQL = " select c.leadbank_gid from crm_mst_tcustomer a" +
                            " left join crm_trn_tleadbank c on a.customer_gid = c.customer_gid " +
                            " where a.customer_gid='" + customer_gid + "'";
                    string lscustomergid = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency = 'Y'";
                    string currency = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select distinct a.salesorder_gid, a.so_referenceno1, l.customer_gid,l.leadbank_gid,c.user_firstname," +
                            " DATE_FORMAT(a.salesorder_date, '%d-%m-%Y') as salesorder_date,c.user_firstname as created_by,a.so_type,a.currency_code," +
                            " a.customer_contact_person, a.salesorder_status,a.currency_code,s.source_name,d.customer_code,i.branch_name, " +
                            " format(a.Grandtotal, 2) as Grandtotal,CONCAT(k.user_firstname, ' ', k.user_lastname) AS salesperson_name,  " +
                            " case when a.currency_code = '" + currency + "' then a.customer_name " +
                            " when a.currency_code is null then a.customer_name " +
                            " when a.currency_code is not null and a.currency_code <> '" + currency + "' then (a.customer_name) end as customer_name, " +
                            " concat(e.customerbranch_name,' / ',e.customercontact_name,' / ',e.email,' / ',e.mobile,' / ',e.address1) as contact, a.invoice_flag " +
                            " from smr_trn_tsalesorder a " +
                            " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid " +
                            " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid " +
                            " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                            " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                            " left join adm_mst_tuser c on b.user_gid= c.user_gid" +
                            " left join hrm_mst_tbranch i on a.branch_gid= i.branch_gid" +
                            " LEFT JOIN adm_mst_tuser k ON a.salesperson_gid = k.user_gid " +
                            " left join crm_trn_tleadbank l on l.customer_gid=a.customer_gid" +
                            " left join crm_mst_tsource s on s.source_gid=l.source_gid" +
                            " where 1=1 and a.customer_gid='" + customer_gid + "' order by a.salesorder_gid desc";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<orderdetail_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new orderdetail_list
                            {
                                salesorder_gid = dt["salesorder_gid"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                customer_gid = dt["leadbank_gid"].ToString(),
                                salesorder_date = dt["salesorder_date"].ToString(),
                                so_referenceno1 = dt["so_referenceno1"].ToString(),
                                branch_name = dt["branch_name"].ToString(),
                                so_type = dt["so_type"].ToString(),
                                Grandtotal = dt["Grandtotal"].ToString(),
                                user_firstname = dt["user_firstname"].ToString(),
                                salesorder_status = dt["salesorder_status"].ToString(),
                                salesperson_name = dt["salesperson_name"].ToString()
                            });
                            values.orderdetaillist = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
                else
                {

                    msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency = 'Y'";
                    string currency = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select distinct a.salesorder_gid, a.so_referenceno1, l.customer_gid,l.leadbank_gid,c.user_firstname," +
                            " DATE_FORMAT(a.salesorder_date, '%d-%m-%Y') as salesorder_date,c.user_firstname as created_by,a.so_type,a.currency_code," +
                            " a.customer_contact_person, a.salesorder_status,a.currency_code,s.source_name,d.customer_code,i.branch_name, " +
                            " format(a.Grandtotal, 2) as Grandtotal,CONCAT(k.user_firstname, ' ', k.user_lastname) AS salesperson_name,  " +
                            " case when a.currency_code = '" + currency + "' then a.customer_name " +
                            " when a.currency_code is null then a.customer_name " +
                            " when a.currency_code is not null and a.currency_code <> '" + currency + "' then (a.customer_name) end as customer_name, " +
                            " concat(e.customerbranch_name,' / ',e.customercontact_name,' / ',e.email,' / ',e.mobile,' / ',e.address1) as contact, a.invoice_flag " +
                            " from smr_trn_tsalesorder a " +
                            " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid " +
                            " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid " +
                            " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                            " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                            " left join adm_mst_tuser c on b.user_gid= c.user_gid" +
                            " left join hrm_mst_tbranch i on a.branch_gid= i.branch_gid" +
                            " LEFT JOIN adm_mst_tuser k ON a.salesperson_gid = k.user_gid " +
                            " left join crm_trn_tleadbank l on l.customer_gid=a.customer_gid" +
                            " left join crm_mst_tsource s on s.source_gid=l.source_gid" +
                            " where 1=1 and l.leadbank_gid='" + lscustomergid + "' order by a.salesorder_gid desc";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<orderdetail_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new orderdetail_list
                            {
                                salesorder_gid = dt["salesorder_gid"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                customer_gid = dt["leadbank_gid"].ToString(),
                                salesorder_date = dt["salesorder_date"].ToString(),
                                so_referenceno1 = dt["so_referenceno1"].ToString(),
                                branch_name = dt["branch_name"].ToString(),
                                so_type = dt["so_type"].ToString(),
                                Grandtotal = dt["Grandtotal"].ToString(),
                                user_firstname = dt["user_firstname"].ToString(),
                                salesorder_status = dt["salesorder_status"].ToString(),
                                salesperson_name = dt["salesperson_name"].ToString()
                            });
                            values.orderdetaillist = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // TILE COUNT

        public void DaGetCountandAmount(string employee_gid, string customer_gid, MdlSmrTrnSales360 values)
        {
            try
            {
                if (customer_gid.Contains("BCRM"))
                {
                    msSQL = " select c.leadbank_gid from crm_mst_tcustomer a" +
                            " left join crm_trn_tleadbank c on a.customer_gid = c.customer_gid " +
                            " where a.customer_gid='" + customer_gid + "'";
                    string lscustomergid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select(select count(a.quotation_gid) from smr_trn_treceivequotation a" +
                       " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid" +
                       " left join crm_trn_tleadbank c on b.customer_gid = c.customer_gid" +
                       " where a.customer_gid ='" + customer_gid + "') as quotataioncount ,  " +
                       " (select count(a.salesorder_gid) from smr_trn_tsalesorder a " +
                       "  left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                       " left join crm_trn_tleadbank c on b.customer_gid = c.customer_gid " +
                       "  where a.customer_gid='" + customer_gid + "') as salesordercount, " +
                       " (select count(a.invoice_gid) from rbl_trn_tinvoice a " +
                       "  left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                       " left join crm_trn_tleadbank c on b.customer_gid = c.customer_gid " +
                       "  where a.customer_gid='" + customer_gid + "') as invoicecount, " +
                       " (SELECT FORMAT(SUM(a.Grandtotal), '2') FROM smr_trn_treceivequotation a " +
                       " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid" +
                       " left join crm_trn_tleadbank c on b.customer_gid = c.customer_gid" +
                       " where a.customer_gid ='" + customer_gid + "') AS quoteamount," +
                       " (SELECT FORMAT(SUM(a.Grandtotal), '2') FROM smr_trn_tsalesorder a " +
                       " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                       " left join crm_trn_tleadbank c on b.customer_gid = c.customer_gid" +
                       " where a.customer_gid='" + customer_gid + "') AS salesorderamount," +
                       " (SELECT FORMAT(SUM(a.total_amount_L), '2') FROM rbl_trn_tinvoice a " +
                       " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                       " left join crm_trn_tleadbank c on b.customer_gid = c.customer_gid" +
                       " where a.customer_gid='" + customer_gid + "') AS invoiceamount," +
                       " (select count(a.proposal_gid) FROM crm_mst_tproposaltemplate a " +
                       " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid " +
                       " left join crm_mst_tcustomer c on b.customer_gid = c.customer_gid " +
                       " left join crm_trn_tleadbank d on c.customer_gid = d.customer_gid" +
                       " where b.customer_gid='" + customer_gid + "') AS proposalcount";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var customercount_list = new List<tilescount_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            customercount_list.Add(new tilescount_list
                            {
                                quotataioncount = dt["quotataioncount"].ToString(),
                                salesordercount = dt["salesordercount"].ToString(),
                                quoteamount = dt["quoteamount"].ToString(),
                                salesorderamount = dt["salesorderamount"].ToString(),
                                invoicecount = dt["invoicecount"].ToString(),
                                invoiceamount = dt["invoiceamount"].ToString(),
                                proposalcount = dt["proposalcount"].ToString(),
                            });
                            values.tilescountlist = customercount_list;
                        }
                    }
                    dt_datatable.Dispose();

                }
                else
                {

                    msSQL = " select(select count(a.quotation_gid) from smr_trn_treceivequotation a" +
                        " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid" +
                        " left join crm_trn_tleadbank c on b.customer_gid = c.customer_gid" +
                        " where c.leadbank_gid ='" + lscustomergid + "') as quotataioncount ,  " +
                        " (select count(a.salesorder_gid) from smr_trn_tsalesorder a " +
                        "  left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                        " left join crm_trn_tleadbank c on b.customer_gid = c.customer_gid " +
                        "  where c.leadbank_gid='" + lscustomergid + "') as salesordercount, " +
                        " (select count(a.invoice_gid) from rbl_trn_tinvoice a " +
                        "  left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                        " left join crm_trn_tleadbank c on b.customer_gid = c.customer_gid " +
                        "  where c.leadbank_gid='" + lscustomergid + "') as invoicecount, " +
                        " (SELECT FORMAT(SUM(a.Grandtotal), '2') FROM smr_trn_treceivequotation a " +
                        " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid" +
                        " left join crm_trn_tleadbank c on b.customer_gid = c.customer_gid" +
                        " where c.leadbank_gid ='" + lscustomergid + "') AS quoteamount," +
                        " (SELECT FORMAT(SUM(a.Grandtotal), '2') FROM smr_trn_tsalesorder a " +
                        " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                        " left join crm_trn_tleadbank c on b.customer_gid = c.customer_gid" +
                        " where c.leadbank_gid='" + lscustomergid + "') AS salesorderamount," +
                        " (SELECT FORMAT(SUM(a.total_amount_L), '2') FROM rbl_trn_tinvoice a " +
                        " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                        " left join crm_trn_tleadbank c on b.customer_gid = c.customer_gid" +
                        " where c.leadbank_gid='" + lscustomergid + "') AS invoiceamount," +
                        " (select count(a.proposal_gid) FROM crm_mst_tproposaltemplate a " +
                        " left join smr_trn_tsalesenquiry b on a.enquiry_gid = b.enquiry_gid " +
                        " left join crm_mst_tcustomer c on b.customer_gid = c.customer_gid " +
                        " left join crm_trn_tleadbank d on c.customer_gid = d.customer_gid" +
                        " where d.leadbank_gid='" + lscustomergid + "') AS proposalcount";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var customercount_list = new List<tilescount_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            customercount_list.Add(new tilescount_list
                            {
                                quotataioncount = dt["quotataioncount"].ToString(),
                                salesordercount = dt["salesordercount"].ToString(),
                                quoteamount = dt["quoteamount"].ToString(),
                                salesorderamount = dt["salesorderamount"].ToString(),
                                invoicecount = dt["invoicecount"].ToString(),
                                invoiceamount = dt["invoiceamount"].ToString(),
                                proposalcount = dt["proposalcount"].ToString(),
                            });
                            values.tilescountlist = customercount_list;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Manager Count !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // CUSTOMER UPDATE

        public void DaUpdateCustomer(string employee_gid,overall_list values) 
        {
            try
            { 
            msGetGid = objcmnfunctions.GetMasterGID("AUCV");

            msSQL = " Insert into crm_trn_tapprovecontactview(" +
                    " approvecontact_gid, " +
                    " contact_person," +
                    " source," +
                    " region," +
                    " company_code," +
                    " company_name, " +
                    " designation," +
                    " email," +
                    " mobile, " +
                    " approve_flag, " +
                    " leadbank_gid, " +
                    " employee_gid, " +
                    " date," +
                    " status," +
                    " request_from," +
                    " updated_flag " +
                    " )" +
                    " Values(" +
                    "'" + msGetGid + "'," +
                    " '" + values.customercontact_name + "', " +
                    " '" + values.source + "'," +
                    " '" + values.region + "'," +
                    " '" + values.customer_id + "'," +
                    " '" + values.customer_name + "'," +
                    " '" + values.designation + "'," +
                    " '" + values.emailid + "'," +
                    " '" + values.mobile + "'," +
                    "'N'," +
                    " '" + values.leadbank_gid + "', " +
                    "'" + employee_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    " 'Pending'," +
                    " 'My Enquiry'," +
                    "'N'" +
                    ")";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if(mnResult !=0)
                {
                    values.status = true;
                    values.message = "Contact Details Sent to Approval Successfully ";
                    return;
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Contact ";
                    return;
                }

        }

            catch (Exception ex)
            {
                values.message = "Exception occured while loading Manager Count !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // INVOICE DETAILS


        public void DaGetInvoiceDetails(string customer_gid, MdlSmrTrnSales360 values)
        {
            try
            {

                if (customer_gid.Contains("BCRM"))
                {
                    msSQL = " select c.leadbank_gid from crm_mst_tcustomer a" +
                            " left join crm_trn_tleadbank c on a.customer_gid = c.customer_gid " +
                            " where a.customer_gid='" + customer_gid + "'";
                    string lscustomergid = objdbconn.GetExecuteScalar(msSQL);


                

                    msSQL = " select a.invoice_gid, a.invoice_refno,date_format(a.invoice_date, '%d-%m-%Y') as invoice_date," +
                            " a.customer_gid, b.leadbank_gid, format(a.invoice_amount, 2) as invoice_amount, a.invoice_status" +
                            " from rbl_trn_tinvoice a  " +
                            " left join crm_trn_tleadbank b on a.customer_gid = b.customer_gid " +
                            " where b.leadbank_gid='" + customer_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<invoicedetail_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new invoicedetail_list
                            {
                                directorder_gid = dt["invoice_gid"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),
                                directorder_date = dt["invoice_date"].ToString(),                                                           
                                Grandtotal = dt["invoice_amount"].ToString(),
                                so_referenceno1 = dt["invoice_refno"].ToString(), 
                                invoice_type = dt["invoice_status"].ToString(), 
                                
                              
                            });
                            values.invoicedetaillist = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
                else
                {



                    msSQL = " select a.invoice_gid, a.invoice_refno,date_format(a.invoice_date, '%d-%m-%Y') as invoice_date," +
                            " a.customer_gid, b.leadbank_gid, format(a.invoice_amount, 2) as invoice_amount, a.invoice_status" +
                            " from rbl_trn_tinvoice a  " +
                            " left join crm_trn_tleadbank b on a.customer_gid = b.customer_gid " +
                            " where b.leadbank_gid='" + customer_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<invoicedetail_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new invoicedetail_list
                            {
                                directorder_gid = dt["invoice_gid"].ToString(),
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),
                                directorder_date = dt["invoice_date"].ToString(),
                                Grandtotal = dt["invoice_amount"].ToString(),
                                so_referenceno1 = dt["invoice_refno"].ToString(),
                                invoice_type = dt["invoice_status"].ToString(),
                            });
                            values.invoicedetaillist = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        //INACTIVE CUSTOMER

        public void DaInactiveCustomer(string user_gid, MdlSmrTrnSales360 values)
        {
            try
            {

                msSQL = " update crm_trn_tenquiry2campaign set" +
                        " leadstage_gid='5'" +
                        " where customer_gid = '" + values.customer_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = " update smr_trn_tsalesenquiry set" +
                       " enquiry_status='Drop'" +
                       " where customer_gid = '" + values.customer_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }
                
                if(mnResult != 0) { 
                    values.status = true;
                    values.message = "Enquiry Dropped Successfully";
                    return;
                }
            
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Dropping Enquiry";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Updating Customer Inactivated !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetLeadCountDetails(MdlSmrTrnSales360 values, string customer_gid)
        {
            try
            {

                msSQL = " select customer_gid from crm_mst_tcustomer where customer_gid  = '" + customer_gid + "'";
                string lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);


                msSQL = "SELECT Months, SUM(quotation_count) AS quotation_count, SUM(enquiry_count) AS enquiry_count, SUM(order_count) AS order_count," +
                    "sum(invoice_count) as invoice_count FROM ( SELECT DATE_FORMAT(a.created_date,'%b-%y') AS Months, COUNT(a.quotation_gid) AS " +
                    "quotation_count, 0 AS enquiry_count, 0 AS order_count,0 as invoice_count,created_date AS ordermonth FROM smr_trn_treceivequotation a" +
                    " WHERE   customer_gid='" + lscustomer_gid + "' and customer_gid!=''  GROUP BY Months UNION ALL SELECT DATE_FORMAT(a.created_date,'%b-%y') AS Months," +
                    " 0 AS quotation_count, COUNT(a.enquiry_gid) AS enquiry_count, 0 AS order_count,0 as invoice_count,created_date AS ordermonth " +
                    "FROM smr_trn_tsalesenquiry a WHERE  customer_gid='" + lscustomer_gid + "' and customer_gid!='' GROUP  BY Months UNION ALL SELECT DATE_FORMAT(a.created_date,'%b-%y')" +
                    " AS Months,  0 AS quotation_count, 0 AS enquiry_count, COUNT(a.salesorder_gid) AS order_count,0 as invoice_count,created_date AS ordermonth " +
                    "FROM smr_trn_tsalesorder a WHERE  customer_gid='" + lscustomer_gid + "'and customer_gid!='' GROUP BY Months UNION ALL SELECT DATE_FORMAT(a.created_date,'%b-%y') AS Months," +
                    " 0 AS quotation_count, 0 AS enquiry_count,0 AS order_count,COUNT(a.invoice_gid) AS invoice_count,created_date AS ordermonth FROM rbl_trn_tinvoice a" +
                    " WHERE customer_gid='" + lscustomer_gid + "' and customer_gid!='' GROUP BY Months) AS combined_data GROUP BY Months order by ordermonth;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<customerchart>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new customerchart
                        {
                            quotation_count = (dt["quotation_count"].ToString()),
                            enquiry_count = (dt["enquiry_count"].ToString()),
                            order_count = (dt["order_count"].ToString()),
                            invoice_count = (dt["invoice_count"].ToString()),
                            Months = (dt["Months"].ToString()),
                        });
                        values.customerchart = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Count Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaSmrNotesAdd(SmrNotes values, string user_gid, result objResult)
        {
            try
            {
                msSQL = " select leadbank_gid from crm_trn_tleadbank where customer_gid='" + values.customer_gid + "'";
                string leadbank_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "insert into crm_trn_tleadnotes " +
                    "(internal_notes," +
                    "customer_gid," +
                    "leadbank_gid," +
                    "created_date," +
                    "created_by) " +
                    "values(" +
                       "'" + values.internalnotestext_area + "', " +
                       "'" + values.customer_gid + "', " +
                       "'" + leadbank_gid + "', " +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                       "'" + user_gid + "') ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Notes Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Notes!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Uploading Telecaller Lead Notes!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaSmrGetNotesSummary(MdlSmrTrnSales360 values, string customer_gid)
        {
            try
            {

                msSQL = "select internal_notes, customer_gid, s_no from crm_trn_tleadnotes where customer_gid='" + customer_gid +"'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<SmrNotes>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new SmrNotes
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            s_no = dt["s_no"].ToString(),
                        });
                        values.SmrNotes = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Notes Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaNoteEditupdate(SmrNotes values, string user_gid, result objResult)
        {
            msSQL = "Update crm_trn_tleadnotes set internal_notes='" + values.internal_notes + "'," +
                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                " updated_by='" + user_gid + "' where s_no='" + values.s_no + "'; ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                values.message = "Note Updated Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Updating Note!!";
            }
        }
        public void DaSmrNotedelete(SmrNotes values, string user_gid, result objResult)
        {
            msSQL = "Delete from crm_trn_tleadnotes  where s_no='" + values.s_no + "'; ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                values.message = "Note Deleted Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Deleting Note!!";
            }
        }
        public void DaGetEnquiryDetails(MdlSmrTrnSales360 values, string customer_gid)
        {
            try
            {            
                msSQL = " Select distinct concat(a.enquiry_gid,' / ',a.enquiry_type) as enquiry_refno, a.enquiry_gid,a.customer_gid,n.customercontact_gid," +
                        " date_format(a.enquiry_date, '%d-%m-%Y') as enquiry_date,b.user_firstname,a.customer_name,a.branch_gid," +
                        " a.customer_gid,a.lead_status," +
                        " concat(o.region_name, ' / ', m.customer_city, ' / ', m.customer_state) as region_name," +
                        " a.enquiry_referencenumber,a.enquiry_status,a.enquiry_type," +
                        " concat(b.user_firstname, ' ', b.user_lastname) as campaign,a.enquiry_remarks," +
                        " a.contact_person,a.contact_email,a.contact_address," +
                        " case when a.contact_person is null then concat(n.customercontact_name,' / ',n.mobile,' / ',n.email)" +
                        " when a.contact_person is not null then concat(a.contact_person,' / ',a.contact_number,' / ',a.contact_email) end as contact_details," +
                        " a.enquiry_type from smr_trn_tsalesenquiry a" +
                        " left join crm_mst_tcustomer m on m.customer_gid = a.customer_gid" +
                        " left join crm_mst_tcustomercontact n on n.customer_gid = a.customer_gid" +
                        " left join crm_mst_tregion o on m.customer_region = o.region_gid" +
                        " left join crm_trn_tenquiry2campaign p on p.enquiry_gid = a.enquiry_gid" +
                        " left join hrm_mst_temployee d on d.employee_gid = p.assign_to" +
                        " left join adm_mst_tuser b on b.user_gid = d.user_gid" +
                        " where m.customer_gid = '" + customer_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getEnquiryList = new List<SmrEnquiry_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getEnquiryList.Add(new SmrEnquiry_list
                        {
                            enquiry_refno = dt["enquiry_refno"].ToString(),
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            customercontact_gid = dt["customercontact_gid"].ToString(),                            
                            enquiry_date = dt["enquiry_date"].ToString(),
                            enquiry_status = dt["enquiry_status"].ToString(),
                            enquiry_type = dt["enquiry_type"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            user_firstname = dt["user_firstname"].ToString()
                        });
                        values.SmrEnquiry_list = getEnquiryList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Enquiry Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaSmrGetDocumentDetails(MdlSmrTrnSales360 values,string customer_gid)
        {
            try
            {

                msSQL = "Select document_gid,document_title,document_upload,leadbank_gid ,document_type, remarks,date_format(created_date,'%d-%m-%Y') as created_date" +
              " from crm_trn_tdocument where customer_gid='" + customer_gid + "' and document_type='mylead'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<SmrDocumentList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new SmrDocumentList
                        {
                            document_gid = dt["document_gid"].ToString(),
                            document_title = dt["document_title"].ToString(),
                            document_upload = dt["document_upload"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            document_type = dt["document_type"].ToString(),
                            created_date = dt["created_date"].ToString(),


                        });
                        values.SmrDocumentList = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Document Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetSmrQuotationDetails(MdlSmrTrnSales360 values,string customer_gid)
        {
            try
            {

                msSQL = " select distinct a.customer_gid,a.quotation_gid, a.quotation_referenceno1, date_format(a.quotation_date,'%d-%m-%Y') as quotation_date,c.user_firstname,a.quotation_type,a.currency_code, " +
                     " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                     " a.customer_name," +
                     " a.customer_contact_person, a.quotation_status,a.enquiry_gid, " +
                     " case when a.contact_mail is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email) " +
                     " when a.contact_mail is not null then concat(a.customer_contact_person,' / ',a.contact_no,' / ',a.contact_mail) end as contact " +
                     " from smr_trn_treceivequotation a " +
                     " left join hrm_mst_temployee b on b.employee_gid = a.created_by "+
                    " left join adm_mst_tuser c on b.user_gid = c.user_gid "+
                     " left join crm_mst_tcustomer d on d.customer_gid = a.customer_gid " +
                     " left join crm_mst_tcustomercontact j on j.customer_gid = d.customer_gid"+
                    " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code"+
                    " left join crm_mst_tcustomercontact e on e.customer_gid = d.customer_gid"+
                     " where d.customer_gid='" + customer_gid + "' order by a.quotation_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<SmrQuotationList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new SmrQuotationList
                        {
                            customer_gid = dt["customer_gid"].ToString(),                                                        
                            quotation_gid = dt["quotation_gid"].ToString(),
                            quotation_referenceno1 = dt["quotation_gid"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            quotation_type = dt["quotation_type"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            quotation_status = dt["quotation_status"].ToString(),
                            enquiry_gid = dt["enquiry_gid"].ToString(),
                            contact = dt["contact"].ToString(),
                        });
                        values.SmrQuotationList = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Quotation Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetSmrOrderDetails(MdlSmrTrnSales360 values,string customer_gid)
        {
            try
            {
                msSQL = " select distinct a.customer_gid, a.salesorder_gid, a.so_referenceno1, date_format(a.salesorder_date,'%d-%m-%Y') as salesorder_date,c.user_firstname,a.so_type,a.currency_code, " +
                    "  a.customer_contact_person, a.salesorder_status,a.currency_code, " +
                    " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                    " a.customer_name, " +
                    " case when a.customer_email is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email) " +
                    " when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact,invoice_flag " +
                    "  from smr_trn_tsalesorder a " +
                    " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid " +
                    " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid " +
                    " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                    " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                    " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                    " where 1=1  and a.customer_gid ='" + customer_gid + "' order by  a.salesorder_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<SmrOrderList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new SmrOrderList
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            invoice_flag = dt["invoice_flag"].ToString(),

                        });
                        values.SmrOrderList = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Order Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetSmrInvoiceDetails(MdlSmrTrnSales360 values,string customer_gid)
        {
            try
            {               
                msSQL = " select distinct a.invoice_gid, case when a.invoice_reference like '%AREF%' then j.agreement_referencenumber else  " +
                        " cast(concat(s.so_referenceno1,if(s.so_referencenumber<>'',concat(' ',' | ',' ',s.so_referencenumber),'') ) as char)  end as so_referencenumber, " +
                        " a.invoice_refno, " +
                        " a.mail_Status,a.customer_gid,date_format(a.invoice_date, '%d-%m-%Y') as 'invoice_date'," +
                        " a.invoice_reference,a.additionalcharges_amount,a.discount_amount,  " +
                        " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', " +
                        " a.payment_flag,  format(a.initialinvoice_amount,2) as initialinvoice_amount,a.invoice_status,a.invoice_flag,  " +
                        " format(a.invoice_amount,2) as invoice_amount, " +
                        " c.customer_name,a.currency_code,  " +
                        " a.customer_contactnumber  as mobile,a.invoice_from,i.directorder_gid,a.progressive_invoice " +
                        " from rbl_trn_tinvoice a  " +
                        " left join rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid  " +
                        " left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid  " +
                        " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code  " +
                        " left join smr_trn_tsalesorder s on a.invoice_reference = s. salesorder_gid  " +
                        " left join crm_trn_tagreement j on j.agreement_gid = a.invoice_reference  " +
                        " left join smr_trn_tdeliveryorder i on s.salesorder_gid=i.salesorder_gid " +
                        " where a. customer_gid='" + customer_gid + "' and a.customer_gid!='' order by a.invoice_gid desc limit 1";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<SmrInvoiceList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new SmrInvoiceList
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            so_referencenumber = dt["so_referencenumber"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            mail_status = dt["mail_Status"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            invoice_reference = dt["invoice_reference"].ToString(),
                            additionalcharges_amount = dt["additionalcharges_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            overall_status = dt["overall_status"].ToString(),
                            payment_flag = dt["payment_flag"].ToString(),
                            initialinvoice_amount = dt["initialinvoice_amount"].ToString(),
                            invoice_status = dt["invoice_status"].ToString(),
                            invoice_flag = dt["invoice_flag"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            invoice_from = dt["invoice_from"].ToString(),
                            directorder_gid = dt["directorder_gid"].ToString(),
                            progressive_invoice = dt["progressive_invoice"].ToString(),

                        });
                        values.SmrInvoiceList = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Invoice Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaGetSalestatus(string customer_gid, MdlSmrTrnSales360 values)
        {
            try
            {
                msSQL = " SELECT Months, SUM(so_count) AS so_count,  SUM(invoice_count) AS invoice_count, " +
                    " SUM(quotation_count) AS quotation_count" +
                    " FROM (SELECT DATE_FORMAT(a.created_date, '%b') AS Months," +
                    " COUNT(a.salesorder_gid) AS so_count, 0 AS quotation_count," +
                    " 0 AS invoice_count FROM smr_trn_tsalesorder a" +
                    " WHERE  a.customer_gid='" + customer_gid + "' and a.created_date >= CURDATE() - INTERVAL 6 MONTH" +
                    " UNION ALL" +
                    " SELECT DATE_FORMAT(a.created_date, '%b') AS Months, 0 AS so_count," +
                    " COUNT(a.quotation_gid) AS quotation_count,0 AS invoice_count" +
                    " FROM smr_trn_treceivequotation a" +
                    " WHERE  a.customer_gid='" + customer_gid + "' and a.created_date >= CURDATE() - INTERVAL 6 MONTH" +
                    " UNION ALL" +
                    " SELECT DATE_FORMAT(a.created_date, '%b') AS Months," +
                    " 0 AS so_count, 0 AS quotation_count,COUNT(a.invoice_gid) AS invoice_count" +
                    " FROM rbl_trn_tinvoice a" +
                    " WHERE  a.customer_gid='" + customer_gid + "' and a.created_date >= CURDATE() - INTERVAL 6 MONTH) AS combined_data" +
                    " GROUP BY Months ORDER BY STR_TO_DATE(CONCAT('01-', Months), '%d-%b');";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetSalesStatus_list = new List<salescount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetSalesStatus_list.Add(new salescount
                        {
                            so_count = (dt["so_count"].ToString()),
                            quotation_count = (dt["quotation_count"].ToString()),
                            invoice_count = (dt["invoice_count"].ToString()),
                            Months = (dt["Months"].ToString()),
                        });
                        values.salescount = GetSalesStatus_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting MTD Counts !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }


        public void DaGetPaymentCount(string customer_gid, MdlSmrTrnSales360 values)
        {
            try
            {
                msSQL = " SELECT COUNT(CASE WHEN b.invoice_status IN ('Payment Pending') THEN 1 END) AS cancelled_payment," +
                    " COUNT(CASE WHEN b.invoice_status in ('Partially Paid','Payment Done Partial') THEN 1 END) AS approved_payment," +
                    " COUNT(CASE WHEN b.invoice_status = 'Payment Done' THEN 1 END) AS completed_payment" +
                    " FROM rbl_trn_tinvoice b LEFT JOIN crm_mst_tcustomer c ON c.customer_gid = b.customer_gid" +
                    " WHERE b.created_date >= CURDATE() - INTERVAL 6 MONTH and c.customer_gid='"+ customer_gid + "';";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdata = new List<paymentcounts_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow row in dt_datatable.Rows)
                    {
                        getdata.Add(new paymentcounts_list
                        {
                            cancelled_payment = row["cancelled_payment"].ToString(),
                            approved_payment = row["approved_payment"].ToString(),
                            completed_payment = row["completed_payment"].ToString()
                        });
                        values.paymentcounts_list = getdata;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting MTD Counts !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}