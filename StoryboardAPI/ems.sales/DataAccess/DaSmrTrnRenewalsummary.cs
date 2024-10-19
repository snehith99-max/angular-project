using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using ems.sales.Models;
using System.Globalization;
using StoryboardAPI.Models;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Diagnostics.Eventing.Reader;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace ems.sales.DataAccess
{
    public class DaSmrTrnRenewalsummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msINGetGID = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader, objodbcDataReader, objodbcdatareader1, dt;
        DataTable dt_datatable;
        string msEmployeeGID,lscustomerproduct_code, renewal_gid, lscustomer_code, msAccGetGID, lscustomer_id, lsCustomername, lssalesorder_gid, lsprice, msagreeGID, msagreementdtlGID, msrenewalGID, SalesOrderGID, msGetGID, lsdiscountpercentage, lsdiscountamount, mssalesorderGID, lsproductgid, lsproductuom_gid, lsproduct_name, lsproductuom_name,msgetlead2campaign_gid, lsorder_type, mssalesorderGID1, end_date, lsrefno,lscompany_code, start_date, TempSOGID, lsemployee_gid, lsentity_code, lsdesignation_code, lscampaign_title, lscampaign_location, lscampaign_gid, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, lsRowcount;
        string lstax2 = "0.00", lstax1 = "0.00", lstax3 = "0.00", lstax_gid;
        double rreCalTotalAmount, lsbasic_amount = 0.00;
        finance_cmnfunction objfincmn = new finance_cmnfunction();


        public void DaGetRenewalSummary(MdlSmrTrnRenewalsummary values)
        {
            try
            {
                string lsadddate = DateTime.Now.ToString("yyyy-MM-dd");
                string lsbelow = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");

                string msSQL = "SELECT DISTINCT a.renewal_gid, a.customer_gid, a.renewal_type AS renewal,  DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date, " +
                    " a.renewal_to, a.renewal_status,  CONCAT(c.user_firstname, '-', c.user_lastname) AS user_name, d.customer_name, " +
                    "CONCAT(e.customercontact_name, ' / ', e.mobile, ' / ', e.email) AS contact_details, g.salesorder_gid, " +
                    "DATE_FORMAT(g.salesorder_date, '%d-%m-%Y') AS salesorder_date, FORMAT(g.Grandtotal, 2) AS Grandtotal, a.renewal_description, a.created_by, " +
                    "a.created_date, CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration " +
                    "FROM crm_trn_trenewal a LEFT JOIN smr_trn_tsalesorder g ON a.salesorder_gid = g.salesorder_gid " +
                    "LEFT JOIN hrm_mst_temployee b ON a.renewal_to = b.employee_gid  " +
                    "LEFT JOIN adm_mst_tuser c ON b.user_gid = c.user_gid " +
                    "LEFT JOIN crm_mst_tcustomer d ON a.customer_gid = d.customer_gid  " +
                    "LEFT JOIN crm_mst_tcustomercontact e ON a.customer_gid = e.customer_gid  WHERE a.renewal_status<> 'Closed' " +
                    "AND a.renewal_type<> 'Agreement' AND DATEDIFF(a.renewal_date, CURRENT_DATE) <> 0 AND a.renewal_date <= '2024-08-12' AND a.renewal_date >= '2024-07-13' " +
                    "AND a.renewal_status != 'Dropped' UNION " +
                    "SELECT DISTINCT a.renewal_gid, a.customer_gid, a.renewal_type AS renewal,  DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date, a.renewal_to, " +
                    "a.renewal_status,CONCAT(c.user_firstname, '-', c.user_lastname) AS user_name, d.customer_name, " +
                    "CONCAT(e.customercontact_name, ' / ', e.mobile, ' / ', e.email) AS contact_details, g.agreement_gid,  " +
                    "DATE_FORMAT(g.agreement_date, '%d-%m-%Y') AS salesorder_date, FORMAT(g.Grandtotal, 2) AS Grandtotal, a.renewal_description, a.created_by, a.created_date, " +
                    "CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration  FROM crm_trn_trenewal a " +
                    "LEFT JOIN crm_trn_tagreement g ON a.salesorder_gid = g.agreement_gid  LEFT JOIN hrm_mst_temployee b ON a.renewal_to = b.employee_gid  " +
                    "LEFT JOIN adm_mst_tuser c ON b.user_gid = c.user_gid  LEFT JOIN crm_mst_tcustomer d ON a.customer_gid = d.customer_gid  " +
                    "LEFT JOIN crm_mst_tcustomercontact e ON a.customer_gid = e.customer_gid  WHERE a.renewal_status != 'Dropped' AND a.renewal_status<> 'Closed' AND " +
                    "a.renewal_type = 'Agreement'  AND DATEDIFF(a.renewal_date, CURRENT_DATE) <> 0 AND a.renewal_date <= '2024-08-12' AND a.renewal_date >= '2024-07-13' " +
                    "ORDER BY renewal_date ASC";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<renewalsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new renewalsummary_list
                        {
                            renewal_gid = dt["renewal_gid"].ToString(),
                            duration = dt["duration"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(), // Adjust these fields according to your actual column names
                            renewal_description = dt["renewal_description"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            renewal = dt["renewal"].ToString(),
                            renewal_status = dt["renewal_status"].ToString(),
                            //assigned_total = dt["assigned_total"].ToString(),
                            renewal_to = dt["renewal_to"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                        });
                    }
                    values.renewalsummary_list = getModuleList;
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetSmrTrnRenewalall(MdlSmrTrnRenewalsummary values)
        {
            try
            {


            string msSQL = " SELECT  a.renewal_gid,  d.salesorder_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                           " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                           " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.salesorder_date, '%d-%m-%Y') AS order_agreement_date," +
                           " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a  " +
                           " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid " +
                           " LEFT JOIN smr_trn_tsalesorder d ON a.salesorder_gid = d.salesorder_gid  " +
                           " WHERE a.renewal_status <> 'Closed'  AND a.renewal_status != 'Dropped' AND a.renewal_type = 'sales' " +
                           " AND d.renewal_flag = 'Y'UNION " +
                           " SELECT a.renewal_gid,d.agreement_gid AS order_agreement_gid,a.customer_gid,a.renewal_type AS renewal," +
                           " DATE_FORMAT(a.renewal_date, '%d-%m-%Y') AS renewal_date,b.customer_name,CONCAT(c.customercontact_name, ' / ', c.mobile, ' / ', c.email) AS contact_details," +
                           " FORMAT(d.Grandtotal, 2) AS Grandtotal,DATE_FORMAT(d.agreement_date, '%d-%m-%Y') AS order_agreement_date," +
                           " CONCAT(DATEDIFF(a.renewal_date, CURRENT_DATE), ' days') AS duration FROM crm_trn_trenewal a" +
                           " LEFT JOIN crm_mst_tcustomer b ON a.customer_gid = b.customer_gid LEFT JOIN crm_mst_tcustomercontact c ON a.customer_gid = c.customer_gid "+                           ""+
                           " LEFT JOIN crm_trn_tagreement d ON a.agreement_gid = d.agreement_gid "+
                           " WHERE  a.renewal_type = 'Agreement' AND a.renewal_status != 'Dropped'" +
                           " ORDER BY renewal_gid DESC";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<renewalsummary_list2>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new renewalsummary_list2
                        {
                            renewal_gid = dt["renewal_gid"].ToString(),
                            duration = dt["duration"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(), 
                            renewal = dt["renewal"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            salesorder_date = dt["order_agreement_date"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                        });
                    }
                    values.renewalsummary_list2 = getModuleList;
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetEditrenewalSummary(string renewal_gid, MdlSmrTrnRenewalsummary values)
        {
            try
            {

                msSQL = "SELECT salesorder_gid FROM crm_trn_trenewal WHERE renewal_gid = '" + renewal_gid + "'";
                objodbcDataReader = objdbconn.GetDataReader(msSQL);
                string lssalesorder_gid = null;

                if (objodbcDataReader.HasRows)
                {
                    lssalesorder_gid = objodbcDataReader["salesorder_gid"].ToString();
                }

                // Construct the appropriate SQL query based on the presence of lssalesorder_gid
                if (!string.IsNullOrEmpty(lssalesorder_gid))
                {

                    msSQL = " SELECT distinct a.salesorder_gid,a.currency_gid, a.customer_gid,a.currency_code, a.customerbranch_gid, a.exchange_rate, " +

                   " DATE_FORMAT(a.salesorder_date, '%d-%m-%Y') AS salesorder_date, a.salesperson_gid, " +

                   " concat(g.user_code, ' / ', g.user_firstname, ' ', g.user_lastname) AS salesperson_name, " +

                   " FORMAT(a.Grandtotal, 2) AS Grandtotal, a.termsandconditions, a.addon_charge AS addon_charge, " +

                   "a.additional_discount_l AS additional_discount, a.payment_days, a.tax_amount AS tax_amount, " +

                   " a.delivery_days, a.so_referenceno1, a.payment_terms, a.freight_terms, " +

                   "a.roundoff AS roundoff, a.so_remarks, a.shipping_to, " +

                   " a.customer_address, a.customer_name, b.country_name, m.customercontact_name AS customer_contact_person,n.customer_city as city,n.customer_state as state , " +

                   " case when a.start_date = '0000-00-00' then '' else DATE_FORMAT(a.start_date, '%d-%m-%Y') end AS start_date,case when a.end_date = '0000-00-00' then '' else DATE_FORMAT(a.end_date, '%d-%m-%Y') end AS end_date, " +

                   " a.termsandconditions, m.mobile, m.email,n.gst_number ,e.branch_name, " +

                   " FORMAT(a.total_amount, 2) AS total_amount, a.freight_charges AS freight_charges, " +

                   " FORMAT(a.packing_charges, 2) AS packing_charges,format(a.total_price, 2) as total_price,a.tax_name, FORMAT(a.buyback_charges, 2) AS buyback_charges, " +

                   " FORMAT(a.insurance_charges, 2) AS insurance_charges,CASE  WHEN a.renewal_flag = 'Y' THEN 'Yes' WHEN a.renewal_flag = 'N' THEN 'No'  END AS renewal_status ," +
                   " CASE   WHEN a.renewal_flag = 'Y' THEN DATE_FORMAT(r.renewal_date, '%d-%m-%Y')  ELSE NULL END AS renewal_date,CASE WHEN a.renewal_flag = 'Y' THEN r.frequency_term ELSE NULL END AS frequency_term FROM smr_trn_tsalesorder a " +

                   " LEFT JOIN smr_trn_tsalesorderdtl d ON d.salesorder_gid = a.salesorder_gid " +

                   " LEFT JOIN hrm_mst_tbranch e ON e.branch_gid = a.branch_gid " +

                   " LEFT JOIN acp_mst_ttax f ON f.tax_gid = a.tax_gid " +

                   " LEFT JOIN adm_mst_tuser g ON g.user_gid = a.salesperson_gid " +

                   " LEFT JOIN crm_trn_tcurrencyexchange h ON a.currency_gid = h.currencyexchange_gid " +

                   " LEFT JOIN crm_mst_tcustomercontact m ON m.customer_gid = a.customer_gid " +

                   " LEFT JOIN crm_mst_tcustomer n ON n.customer_gid = a.customer_gid " +

                   " LEFT JOIN  crm_trn_trenewal r ON a.salesorder_gid = r.salesorder_gid " +
                    " LEFT JOIN  adm_mst_tcountry b ON n.customer_country = b.country_gid  " +
                    "WHERE r.renewal_gid = '" + renewal_gid + "' " +
                   " GROUP BY a.salesorder_gid, d.product_gid ORDER BY a.salesorder_gid ASC";


                }
                else
                {

                    msSQL = " SELECT distinct a.agreement_gid as salesorder_gid  ,a.customer_gid,a.currency_gid, a.currency_code, a.customerbranch_gid, a.exchange_rate,  " +

                    "  DATE_FORMAT(a.agreement_date, '%d-%m-%Y') AS salesorder_date, " +

                    " concat(g.user_code, ' / ', g.user_firstname, ' ', g.user_lastname) AS salesperson_name,  " +

                    " FORMAT(a.Grandtotal, 2) AS Grandtotal, a.termsandconditions, a.addon_charge AS addon_charge, " +

                    " a.additional_discount AS additional_discount, a.payment_days, a.tax_amount AS tax_amount, " +

                    " a.delivery_days, a.agreement_referencenumber as so_referenceno1, a.payment_days as payment_terms, a.delivery_days as freight_terms,   " +

                    " a.roundoff AS roundoff, a.ag_remarks as so_remarks, a.shipping_to, " +

                    " a.customer_address, a.customer_name, b.country_name,  m.customercontact_name AS customer_contact_person, n.customer_city as city,n.customer_state as state ," +

                    " case when a.start_date = '0000-00-00' then '' else DATE_FORMAT(a.start_date, '%d-%m-%Y') end AS start_date,case when a.end_date = '0000-00-00' then '' else DATE_FORMAT(a.end_date, '%d-%m-%Y') end AS end_date, " +

                    " a.termsandconditions, m.mobile, m.email,n.gst_number ,e.branch_name, " +

                    " FORMAT(a.total_amount, 2) AS total_amount,a.freight_charges AS freight_charges, " +

                    " FORMAT(a.packing_charges, 2) AS packing_charges,format(a.total_price, 2) as total_price,a.tax_name, FORMAT(a.buyback_charges, 2) AS buyback_charges, " +

                    " FORMAT(a.insurance_charges, 2) AS insurance_charges, Date_Format(r.renewal_date,'%d-%m-%Y') as renewal_date FROM crm_trn_tagreement a " +

                    " LEFT JOIN crm_trn_tagreementdtl d ON d.agreement_gid = a.agreement_gid " +

                    " LEFT JOIN hrm_mst_tbranch e ON e.branch_gid = a.branch_gid " +

                    " LEFT JOIN acp_mst_ttax f ON f.tax_gid = a.tax_gid " +

                    " LEFT JOIN adm_mst_tuser g ON g.user_gid = a.agreement_gid " +

                    " LEFT JOIN crm_trn_tcurrencyexchange h ON a.currency_gid = h.currencyexchange_gid " +

                    " LEFT JOIN crm_mst_tcustomercontact m ON m.customer_gid = a.customer_gid " +

                    " LEFT JOIN crm_mst_tcustomer n ON n.customer_gid = a.customer_gid " +

                    " LEFT JOIN  crm_trn_trenewal r ON a.agreement_gid = r.agreement_gid " +
                    " LEFT JOIN  adm_mst_tcountry b ON n.customer_country = b.country_gid  " +
                     "WHERE r.renewal_gid = '" + renewal_gid + "' " +
                    " GROUP BY a.agreement_gid, d.product_gid ORDER BY a.agreement_gid ASC";
                }

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<renewalview_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new renewalview_list
                        {

                            salesorder_gid = dt["salesorder_gid"].ToString(),

                            tax_name = dt["tax_name"].ToString(),
                            currency_gid = dt["currency_gid"].ToString(),

                            renewal_date = dt["renewal_date"].ToString(),

                            salesorder_date = dt["salesorder_date"].ToString(),

                            customer_name = dt["customer_name"].ToString(),

                            branch_name = dt["branch_name"].ToString(),

                            customer_contact_person = dt["customer_contact_person"].ToString(),

                            customer_email = dt["email"].ToString(),

                            customer_mobile = dt["mobile"].ToString(),

                            gst_number = dt["gst_number"].ToString(),

                            customer_address = dt["customer_address"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),

                            start_date = dt["start_date"].ToString(),

                            end_date = dt["end_date"].ToString(),

                            currency_code = dt["currency_code"].ToString(),

                            exchange_rate = dt["exchange_rate"].ToString(),

                            freight_terms = dt["freight_terms"].ToString(),

                            payment_terms = dt["payment_terms"].ToString(),

                            payment_days = dt["payment_days"].ToString(),

                            so_referencenumber = dt["so_referenceno1"].ToString(),

                            shipping_to = dt["shipping_to"].ToString(),

                            delivery_days = dt["delivery_days"].ToString(),

                            so_remarks = dt["so_remarks"].ToString(),

                            salesperson_name = dt["salesperson_name"].ToString(),

                            addon_charge = dt["addon_charge"].ToString(),

                            additional_discount = dt["additional_discount"].ToString(),

                            freight_charges = dt["freight_charges"].ToString(),

                            buyback_charges = dt["buyback_charges"].ToString(),

                            packing_charges = dt["packing_charges"].ToString(),

                            insurance_charges = dt["insurance_charges"].ToString(),

                            roundoff = dt["roundoff"].ToString(),

                            Grandtotal = dt["Grandtotal"].ToString(),

                            termsandconditions = dt["termsandconditions"].ToString(),

                            total_price = dt["total_price"].ToString(),

                            price = dt["total_price"].ToString(),

                            total_amount = dt["total_amount"].ToString(),

                            tax_amount = dt["tax_amount"].ToString(),



                        });
                        values.renewalview_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetViewrenewalSummary(string renewal_gid, MdlSmrTrnRenewalsummary values)
        {
            try
            {

                msSQL = "SELECT salesorder_gid FROM crm_trn_trenewal WHERE renewal_gid = '" + renewal_gid + "'";
                objodbcDataReader = objdbconn.GetDataReader(msSQL);
                string lssalesorder_gid = null;

                if (objodbcDataReader.HasRows)
                {
                    lssalesorder_gid = objodbcDataReader["salesorder_gid"].ToString();
                }

                // Construct the appropriate SQL query based on the presence of lssalesorder_gid
                if (!string.IsNullOrEmpty(lssalesorder_gid))
                {

                    msSQL = " SELECT distinct a.salesorder_gid,a.currency_gid, a.customer_gid,a.currency_code, a.customerbranch_gid, a.exchange_rate, " +

                   " DATE_FORMAT(a.salesorder_date, '%d-%m-%Y') AS salesorder_date, a.salesperson_gid, " +

                   " concat(g.user_code, ' / ', g.user_firstname, ' ', g.user_lastname) AS salesperson_name, " +

                   " FORMAT(a.Grandtotal, 2) AS Grandtotal, a.termsandconditions, FORMAT(a.addon_charge, 2) AS addon_charge, " +

                   " FORMAT(a.additional_discount_l, 2) AS additional_discount, a.payment_days, a.tax_amount AS tax_amount, " +

                   " a.delivery_days, a.so_referenceno1, a.payment_terms, a.freight_terms, " +

                   " FORMAT(a.roundoff, 2) AS roundoff, a.so_remarks, a.shipping_to, " +

                   " a.customer_address, a.customer_name, b.country_name, m.customercontact_name AS customer_contact_person,n.customer_city as city,n.customer_state as state , " +

                   " case when a.start_date = '0000-00-00' then '' else DATE_FORMAT(a.start_date, '%d-%m-%Y') end AS start_date,case when a.end_date = '0000-00-00' then '' else DATE_FORMAT(a.end_date, '%d-%m-%Y') end AS end_date, " +

                   " a.termsandconditions, m.mobile, m.email,n.gst_number ,e.branch_name, " +

                   " FORMAT(a.total_amount, 2) AS total_amount, FORMAT(a.freight_charges, 2) AS freight_charges, " +

                   " FORMAT(a.packing_charges, 2) AS packing_charges,format(a.total_price, 2) as total_price,a.tax_name, FORMAT(a.buyback_charges, 2) AS buyback_charges, " +

                   " FORMAT(a.insurance_charges, 2) AS insurance_charges,CASE  WHEN a.renewal_flag = 'Y' THEN 'Yes' WHEN a.renewal_flag = 'N' THEN 'No'  END AS renewal_status ," +
                   " CASE   WHEN a.renewal_flag = 'Y' THEN DATE_FORMAT(r.renewal_date, '%d-%m-%Y')  ELSE NULL END AS renewal_date,CASE WHEN a.renewal_flag = 'Y' THEN r.frequency_term ELSE NULL END AS frequency_term FROM smr_trn_tsalesorder a " +

                   " LEFT JOIN smr_trn_tsalesorderdtl d ON d.salesorder_gid = a.salesorder_gid " +

                   " LEFT JOIN hrm_mst_tbranch e ON e.branch_gid = a.branch_gid " +

                   " LEFT JOIN acp_mst_ttax f ON f.tax_gid = a.tax_gid " +

                   " LEFT JOIN adm_mst_tuser g ON g.user_gid = a.salesperson_gid " +

                   " LEFT JOIN crm_trn_tcurrencyexchange h ON a.currency_gid = h.currencyexchange_gid " +

                   " LEFT JOIN crm_mst_tcustomercontact m ON m.customer_gid = a.customer_gid " +

                   " LEFT JOIN crm_mst_tcustomer n ON n.customer_gid = a.customer_gid " +

                   " LEFT JOIN  crm_trn_trenewal r ON a.salesorder_gid = r.salesorder_gid " +
                    " LEFT JOIN  adm_mst_tcountry b ON n.customer_country = b.country_gid  " +
                    "WHERE r.renewal_gid = '" + renewal_gid + "' " +
                   " GROUP BY a.salesorder_gid, d.product_gid ORDER BY a.salesorder_gid ASC";


                }
                else
                {

                    msSQL = " SELECT distinct a.agreement_gid as salesorder_gid  ,a.customer_gid,a.currency_gid, a.currency_code, a.customerbranch_gid, a.exchange_rate,  " +

                    "  DATE_FORMAT(a.agreement_date, '%d-%m-%Y') AS salesorder_date, " +

                    " concat(g.user_code, ' / ', g.user_firstname, ' ', g.user_lastname) AS salesperson_name,  " +

                    " FORMAT(a.Grandtotal, 2) AS Grandtotal, a.termsandconditions, FORMAT(a.addon_charge, 2) AS addon_charge, " +

                    " FORMAT(a.additional_discount, 2) AS additional_discount, a.payment_days, a.tax_amount AS tax_amount, " +

                    " a.delivery_days, a.agreement_referencenumber as so_referenceno1, a.payment_days as payment_terms, a.delivery_days as freight_terms,   " +

                    " FORMAT(a.roundoff, 2) AS roundoff, a.ag_remarks as so_remarks, a.shipping_to, " +

                    " a.customer_address, a.customer_name, b.country_name,  m.customercontact_name AS customer_contact_person, n.customer_city as city,n.customer_state as state ," +

                    " case when a.start_date = '0000-00-00' then '' else DATE_FORMAT(a.start_date, '%d-%m-%Y') end AS start_date,case when a.end_date = '0000-00-00' then '' else DATE_FORMAT(a.end_date, '%d-%m-%Y') end AS end_date, " +

                    " a.termsandconditions, m.mobile, m.email,n.gst_number ,e.branch_name, " +

                    " FORMAT(a.total_amount, 2) AS total_amount, FORMAT(a.freight_charges, 2) AS freight_charges, " +

                    " FORMAT(a.packing_charges, 2) AS packing_charges,format(a.total_price, 2) as total_price,a.tax_name, FORMAT(a.buyback_charges, 2) AS buyback_charges, " +

                    " FORMAT(a.insurance_charges, 2) AS insurance_charges, Date_Format(r.renewal_date,'%d-%m-%Y') as renewal_date FROM crm_trn_tagreement a " +

                    " LEFT JOIN crm_trn_tagreementdtl d ON d.agreement_gid = a.agreement_gid " +

                    " LEFT JOIN hrm_mst_tbranch e ON e.branch_gid = a.branch_gid " +

                    " LEFT JOIN acp_mst_ttax f ON f.tax_gid = a.tax_gid " +

                    " LEFT JOIN adm_mst_tuser g ON g.user_gid = a.agreement_gid " +

                    " LEFT JOIN crm_trn_tcurrencyexchange h ON a.currency_gid = h.currencyexchange_gid " +

                    " LEFT JOIN crm_mst_tcustomercontact m ON m.customer_gid = a.customer_gid " +

                    " LEFT JOIN crm_mst_tcustomer n ON n.customer_gid = a.customer_gid " +

                    " LEFT JOIN  crm_trn_trenewal r ON a.agreement_gid = r.agreement_gid " +
                    " LEFT JOIN  adm_mst_tcountry b ON n.customer_country = b.country_gid  " +
                     "WHERE r.renewal_gid = '" + renewal_gid + "' " +
                    " GROUP BY a.agreement_gid, d.product_gid ORDER BY a.agreement_gid ASC";
                }

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<renewalview_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new renewalview_list
                        {

                            salesorder_gid = dt["salesorder_gid"].ToString(),

                            tax_name = dt["tax_name"].ToString(),
                            currency_gid = dt["currency_gid"].ToString(),
                       
                            renewal_date = dt["renewal_date"].ToString(),

                            salesorder_date = dt["salesorder_date"].ToString(),

                            customer_name = dt["customer_name"].ToString(),

                            branch_name = dt["branch_name"].ToString(),

                            customer_contact_person = dt["customer_contact_person"].ToString(),

                            customer_email = dt["email"].ToString(),

                            customer_mobile = dt["mobile"].ToString(),

                            gst_number = dt["gst_number"].ToString(),

                            customer_address = dt["customer_address"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),

                            start_date = dt["start_date"].ToString(),

                            end_date = dt["end_date"].ToString(),

                            currency_code = dt["currency_code"].ToString(),

                            exchange_rate = dt["exchange_rate"].ToString(),

                            freight_terms = dt["freight_terms"].ToString(),

                            payment_terms = dt["payment_terms"].ToString(),

                            payment_days = dt["payment_days"].ToString(),

                            so_referencenumber = dt["so_referenceno1"].ToString(),

                            shipping_to = dt["shipping_to"].ToString(),

                            delivery_days = dt["delivery_days"].ToString(),

                            so_remarks = dt["so_remarks"].ToString(),

                            salesperson_name = dt["salesperson_name"].ToString(),

                            addon_charge = dt["addon_charge"].ToString(),

                            additional_discount = dt["additional_discount"].ToString(),

                            freight_charges = dt["freight_charges"].ToString(),

                            buyback_charges = dt["buyback_charges"].ToString(),

                            packing_charges = dt["packing_charges"].ToString(),

                            insurance_charges = dt["insurance_charges"].ToString(),

                            roundoff = dt["roundoff"].ToString(),

                            Grandtotal = dt["Grandtotal"].ToString(),

                            termsandconditions = dt["termsandconditions"].ToString(),

                            total_price = dt["total_price"].ToString(),

                            price = dt["total_price"].ToString(),

                            total_amount = dt["total_amount"].ToString(),

                            tax_amount = dt["tax_amount"].ToString(),



                        });
                        values.renewalview_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
              $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
              DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        private DataTable GetTaxDetailsForProduct(string product_gid, string customer_gid)
        {
            // Query tax segment details based on product_gid
            msSQL = "SELECT f.taxsegment_gid, d.taxsegment_gid, e.taxsegment_name, d.tax_name, d.tax_gid, " +
                "case when d.tax_percentage = ROUND(d.tax_percentage) then ROUND(d.tax_percentage) else d.tax_percentage end AS tax_percentage, " +
                "d.tax_amount, a.mrp_price, a.cost_price, a.product_gid " +
                "FROM acp_mst_ttaxsegment2product d " +
                "LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                "LEFT JOIN crm_mst_tcustomer f ON f.taxsegment_gid = e.taxsegment_gid " +
                "LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid " +
                "WHERE a.product_gid = '" + product_gid + "' AND f.customer_gid = '" + customer_gid + "'";

            // Execute query to get tax segment details
            return objdbconn.GetDataTable(msSQL);
        }
        public void DaGetViewrenewaldetails(string renewal_gid, MdlSmrTrnRenewalsummary values)
        {
            try
            {
                // Fetch salesorder_gid based on renewal_gid
                msSQL = "SELECT salesorder_gid FROM crm_trn_trenewal WHERE renewal_gid = '" + renewal_gid + "'";
                objodbcDataReader = objdbconn.GetDataReader(msSQL);
                string lssalesorder_gid = null;

                if (objodbcDataReader.HasRows)
                {
                    lssalesorder_gid = objodbcDataReader["salesorder_gid"].ToString();
                }

                if (!string.IsNullOrEmpty(lssalesorder_gid))
                {
                    msSQL = "SELECT d.salesorderdtl_gid,d.product_gid, i.productgroup_name, d.product_remarks, d.product_name, d.salesorderdtl_gid, " +
                            "d.product_code, d.uom_name, d.qty_quoted, d.margin_amount, d.margin_percentage, d.discount_percentage, " +
                            "d.discount_amount, FORMAT(d.product_price, 2) AS product_price, d.tax_name, format(d.tax_amount, 2) AS tax_amount, " +
                            "FORMAT(d.price, 2) AS price, e.tax_prefix AS tax_prefix1, l.tax_prefix AS tax_prefix2 " +
                            " FROM smr_trn_tsalesorder a " +
                            "LEFT JOIN smr_trn_tsalesorderdtl d ON d.salesorder_gid = a.salesorder_gid " +
                            "LEFT JOIN acp_mst_ttax e ON e.tax_gid = d.tax1_gid " +
                            "LEFT JOIN acp_mst_ttax l ON l.tax_gid = d.tax2_gid " +
                            "LEFT JOIN pmr_mst_tproductgroup i ON i.productgroup_gid = d.productgroup_gid " +
                            "LEFT JOIN crm_trn_trenewal j ON j.salesorder_gid = a.salesorder_gid " +
                            "WHERE j.renewal_gid = '" + renewal_gid + "' " +
                            "GROUP BY d.product_gid";
                }
                else
                {
                    msSQL = "SELECT d.agreementdtl_gid as salesorderdtl_gid,d.product_gid, i.productgroup_name, d.product_remarks, d.product_name, " +
                            "d.product_code, d.uom_name, d.qty_quoted, d.discount_percentage, d.discount_amount, " +
                            "FORMAT(d.product_price, 2) AS product_price, d.tax_name, format(d.tax_amount, 2) AS tax_amount, " +
                            "FORMAT(d.price, 2) AS price, e.tax_prefix AS tax_prefix1, l.tax_prefix AS tax_prefix2 " +
                            " FROM crm_trn_tagreement a " +
                            "LEFT JOIN crm_trn_tagreementdtl d ON d.agreement_gid = a.agreement_gid " +
                            "LEFT JOIN acp_mst_ttax e ON e.tax_gid = d.tax1_gid " +
                            "LEFT JOIN acp_mst_ttax l ON l.tax_gid = d.tax2_gid " +
                            "LEFT JOIN pmr_mst_tproductgroup i ON i.productgroup_gid = d.productgroup_gid " +
                            "LEFT JOIN crm_trn_trenewal j ON j.agreement_gid = a.agreement_gid " +
                            "WHERE j.renewal_gid = '" + renewal_gid + "' " +
                            "GROUP BY d.product_gid";
                }

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Viewrenewaldetail_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Viewrenewaldetail_list
                        {
                            product_code = dt["product_code"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            tax_prefix = dt["tax_prefix1"].ToString(),
                            tax_prefix2 = dt["tax_prefix2"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            price = dt["price"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            salesorderdtl_gid = dt["salesorderdtl_gid"].ToString(),
                        });
                    }

                    values.Viewrenewaldetail_list = getModuleList;
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetViewsalesorderSummary(string salesorder_gid, MdlSmrTrnRenewalsummary values)

        {

            try

            {

                msSQL = " SELECT distinct a.salesorder_gid,a.currency_gid, a.currency_code, a.customerbranch_gid, a.exchange_rate, " +

                    " DATE_FORMAT(a.salesorder_date, '%d-%m-%Y') AS salesorder_date, a.salesperson_gid, " +

                    " concat(g.user_code, ' / ', g.user_firstname, ' ', g.user_lastname) AS salesperson_name, " +

                    " FORMAT(a.Grandtotal, 2) AS Grandtotal, a.termsandconditions, FORMAT(a.addon_charge, 2) AS addon_charge, " +

                    " FORMAT(a.additional_discount_l, 2) AS additional_discount, a.payment_days, FORMAT(a.tax_amount, 2) AS tax_amount, " +

                    " a.delivery_days, a.so_referenceno1, a.payment_terms, a.freight_terms, " +

                    " FORMAT(a.roundoff, 2) AS roundoff, a.so_remarks, a.shipping_to, " +

                    " a.customer_address, a.customer_name,  a.customer_contact_person AS customer_contact_person, " +

                    " case when a.start_date = '0000-00-00' then '' else DATE_FORMAT(a.start_date, '%d-%m-%Y') end AS start_date,case when a.end_date = '0000-00-00' then '' else DATE_FORMAT(a.end_date, '%d-%m-%Y') end AS end_date, " +

                    " a.termsandconditions, m.mobile, m.email,n.gst_number ,e.branch_name, " +

                    " FORMAT(a.total_amount, 2) AS total_amount, FORMAT(a.freight_charges, 2) AS freight_charges, " +

                    " FORMAT(a.packing_charges, 2) AS packing_charges,format(a.total_price, 2) as total_price,a.tax_name, FORMAT(a.buyback_charges, 2) AS buyback_charges, " +

                    " FORMAT(a.insurance_charges, 2) AS insurance_charges FROM smr_trn_tsalesorder a " +

                    " LEFT JOIN smr_trn_tsalesorderdtl d ON d.salesorder_gid = a.salesorder_gid " +

                    " LEFT JOIN hrm_mst_tbranch e ON e.branch_gid = a.branch_gid " +

                    " LEFT JOIN acp_mst_ttax f ON f.tax_gid = a.tax_gid " +

                    " LEFT JOIN adm_mst_tuser g ON g.user_gid = a.salesperson_gid " +

                    " LEFT JOIN crm_trn_tcurrencyexchange h ON a.currency_gid = h.currencyexchange_gid " +

                    " LEFT JOIN crm_mst_tcustomercontact m ON m.customer_gid = a.customer_gid " +

                    " LEFT JOIN crm_mst_tcustomer n ON n.customer_gid = a.customer_gid " +

                    " WHERE a.salesorder_gid = '" + salesorder_gid + "' GROUP BY a.salesorder_gid, d.product_gid ORDER BY a.salesorder_gid ASC";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<renewalsalesorder_list>();

                if (dt_datatable.Rows.Count != 0)

                {

                    foreach (DataRow dt in dt_datatable.Rows)

                    {

                        getModuleList.Add(new renewalsalesorder_list

                        {


                            salesorder_gid = dt["salesorder_gid"].ToString(),

                            tax_name = dt["tax_name"].ToString(),

                            salesorder_date = dt["salesorder_date"].ToString(),

                            customer_name = dt["customer_name"].ToString(),

                            branch_name = dt["branch_name"].ToString(),

                            customer_contact_person = dt["customer_contact_person"].ToString(),

                            customer_email = dt["email"].ToString(),

                            customer_mobile = dt["mobile"].ToString(),

                            gst_number = dt["gst_number"].ToString(),

                            customer_address = dt["customer_address"].ToString(),

                            start_date = dt["start_date"].ToString(),

                            end_date = dt["end_date"].ToString(),

                            currency_code = dt["currency_code"].ToString(),

                            exchange_rate = dt["exchange_rate"].ToString(),

                            freight_terms = dt["freight_terms"].ToString(),

                            payment_terms = dt["payment_terms"].ToString(),

                            payment_days = dt["payment_days"].ToString(),

                            so_referencenumber = dt["so_referenceno1"].ToString(),

                            shipping_to = dt["shipping_to"].ToString(),

                            delivery_days = dt["delivery_days"].ToString(),

                            so_remarks = dt["so_remarks"].ToString(),

                            salesperson_name = dt["salesperson_name"].ToString(),

                            addon_charge = dt["addon_charge"].ToString(),

                            additional_discount = dt["additional_discount"].ToString(),

                            freight_charges = dt["freight_charges"].ToString(),

                            buyback_charges = dt["buyback_charges"].ToString(),

                            packing_charges = dt["packing_charges"].ToString(),

                            insurance_charges = dt["insurance_charges"].ToString(),

                            roundoff = dt["roundoff"].ToString(),

                            Grandtotal = dt["Grandtotal"].ToString(),

                            termsandconditions = dt["termsandconditions"].ToString(),

                            total_price = dt["total_price"].ToString(),

                            price = dt["total_price"].ToString(),

                            total_amount = dt["total_amount"].ToString(),

                            tax_amount = dt["tax_amount"].ToString(),


                        });

                        values.renewalsalesorder_list = getModuleList;

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
        public void DaGetViewsalesorderdetails(string salesorder_gid, MdlSmrTrnRenewalsummary values)
        {
            try

            {

                msSQL = " select d.display_field,d.product_gid,i.productgroup_name,d.product_remarks,d.product_name,d.salesorderdtl_gid," +
                    " CASE WHEN d.product_code IS NULL THEN d.customerproduct_code ELSE d.product_code END AS product_code ," +
                    " d.uom_name,d.qty_quoted,d.display_field,d.margin_amount,d.margin_percentage,d.discount_percentage, d.discount_amount," +
                    " FORMAT(d.product_price, 2) AS product_price ,d.tax_name,format(d.tax_amount, 2) as tax_amount,format(d.tax_amount2, 2) as tax_amount2,FORMAT(d.price, 2) AS price," +
                    " e.tax_prefix as tax_prefix1 ,l.tax_prefix as tax_prefix2 " +
                    " FROM smr_trn_tsalesorder a LEFT JOIN smr_trn_tsalesorderdtl d ON d.salesorder_gid = a.salesorder_gid " +
                    " LEFT JOIN acp_mst_ttax e ON e.tax_gid = d.tax1_gid" +
                    " LEFT JOIN acp_mst_ttax l ON l.tax_gid = d.tax2_gid " +
                    " LEFT JOIN pmr_mst_tproductgroup i ON i.productgroup_gid = d.productgroup_gid " +
                    " WHERE a.salesorder_gid = '" + salesorder_gid + "' group by d.salesorderdtl_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<renewalsalesorderdetails_list>();



                if (dt_datatable.Rows.Count != 0)

                {

                    foreach (DataRow dt in dt_datatable.Rows)

                    {

                        getModuleList.Add(new renewalsalesorderdetails_list

                        {

                            product_code = dt["product_code"].ToString(),

                            product_gid = dt["product_gid"].ToString(),

                            product_name = dt["product_name"].ToString(),

                            productgroup_name = dt["productgroup_name"].ToString(),

                            product_remarks = dt["display_field"].ToString(),

                            uom_name = dt["uom_name"].ToString(),

                            tax_prefix = dt["tax_prefix1"].ToString(),
                            tax_prefix2 = dt["tax_prefix2"].ToString(),

                            qty_quoted = dt["qty_quoted"].ToString(),

                            price = dt["price"].ToString(),

                            product_price = dt["product_price"].ToString(),

                            margin_percentage = dt["margin_percentage"].ToString(),

                            tax_name = dt["tax_name"].ToString(),

                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),

                            margin_amount = dt["margin_amount"].ToString(),

                            discount_amount = dt["discount_amount"].ToString(),

                            discount_percentage = dt["discount_percentage"].ToString(),

                        });



                        values.renewalsalesorderdetails_list = getModuleList;



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
        public void DaGetassignrenewal(string campaign_gid, string employee_gid, string renewal_gid, string user_gid, MdlSmrTrnRenewalsummary values)
        {
            try
            {
                msSQL = " SELECT renewal_to FROM crm_trn_trenewal a " +
                        " WHERE renewal_gid = '" + renewal_gid + "'";
                string renewal_to = objdbconn.GetExecuteScalar(msSQL);
                if (renewal_to != "")
                {
                    values.message = "Already Assigned to the Renewal!";
                    return;
                }
                else
                { 
                string msGetGid1 = objcmnfunctions.GetMasterGID("BLDP");
                 msSQL = " update crm_trn_trenewal Set " +
                        " assigned = 'Y' ," +
                        " renewal_to = '" + employee_gid + "'" +
                        " where renewal_gid = '" + renewal_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msgetlead2campaign_gid = objcmnfunctions.GetMasterGID("BLDP");
                msSQL = " INSERT INTO crm_trn_trenewal2campaign (" +
                        " lead2campaign_gid, " +
                        " leadbank_gid, " +
                        " campaign_gid, " +
                        " renewal_gid, " +
                        " created_by, " +
                        " assign_to, " +
                        " lead_status, " +
                        " created_date) " +
                        " VALUES (" +
                        "'" + msgetlead2campaign_gid + "', " +
                        "'" + campaign_gid + "', " +
                        "'" + campaign_gid + "', " +
                        "'" + renewal_gid + "', " +
                        "'" + user_gid + "', " +
                        "'" + employee_gid + "', " +
                        "'Open', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Assign to Employee successfully !";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Adding Employee ";
                    }
                }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding renewal!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetdeleterenewal(string renewal_gid, MdlSmrTrnRenewalsummary values)
        {
            try
            {
                msSQL = "delete from crm_trn_trenewal " +
                    " where renewal_gid='" + renewal_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Renewal Deleted successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Renewal";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding renewal!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetProductsearchSummary(MdlSmrTrnRenewalsummary values)
        {
            try
            {
                string lsSqlType = "product";

                msSQL = " call pmr_mst_spproductsearch('" + lsSqlType + "','','')";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetProductsearchs1>();
                var allTaxSegmentsList = new List<GetTaxSegmentList1>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        var product = new GetProductsearchs1
                        {
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            unitprice = dt["mrp_price"].ToString(),
                            product_desc = dt["product_desc"].ToString(),
                            quantity = 0,
                            total_amount = 0,
                            discount_percentage = 0,
                            discount_amount = 0,
                        };
                        getModuleList.Add(product);

                    }
                    values.GetProductsearchs1 = getModuleList; // Assign GetProductsearch to values.GetProductsearch
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                  msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                  DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void GetDeleteDirectSOProductSummary(string invoicedtl_gid, salesorders_list values)
        {
            try
            {

                msSQL = " delete from rbl_tmp_tinvoicedtl " +
                        " where invoicedtl_gid='" + invoicedtl_gid + "'";
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
        public void DaPosttempProductAdd(string employee_gid, PosttempProduct_list values)
        {
            try
            {

                msGetGid = objcmnfunctions.GetMasterGID("VSDT");
                msSQL = " SELECT a.productuom_gid, a.product_gid, a.product_name, b.productuom_name FROM pmr_mst_tproduct a " +
                     " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                     " WHERE product_gid = '" + values.product_name.Replace("'","\\\'") + "'";
                objodbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objodbcDataReader.HasRows == true)
                {
                    lsproductgid = objodbcDataReader["product_gid"].ToString();
                    lsproductuom_gid = objodbcDataReader["productuom_gid"].ToString();
                    lsproduct_name = objodbcDataReader["product_name"].ToString();
                    lsproductuom_name = objodbcDataReader["productuom_name"].ToString();
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

                msSQL = " insert into smr_tmp_tsalesorderdtl( " +
                              " tmpsalesorderdtl_gid," +
                              " employee_gid," +
                              " product_gid," +
                              " product_code," +
                               " customerproduct_code," +
                              " product_name," +
                              " productgroup_gid," +
                              " product_price," +
                              " qty_quoted," +
                              " uom_gid," +
                              " uom_name," +
                              " price," +
                              " order_type," +
                              " tax_rate, " +
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
                              "'" + employee_gid + "'," +
                              "'" + lsproductgid + "'," +
                              "'" + values.product_code.Replace("'", "\\\'") + "'," +
                               "'" + values.product_code.Replace("'", "\\\'") + "'," +
                              "'" + lsproduct_name.Replace("'", "\\\'") + "'," +
                              "'" + values.productgroup_name + "'," +
                              "'" + values.unitprice + "'," +
                              "'" + values.productquantity + "'," +
                              "'" + lsproductuom_gid + "'," +
                              "'" + lsproductuom_name.Replace("'", "\\\'") + "'," +
                              "'" + values.producttotal_amount + "'," +
                              " 'Agreement', " +
                              " '" + values.tax_prefix + "', " +
                              " '" + values.taxsegment_gid + "', " +
                              " '" + values.taxsegment_gid + "', " +
                              " '" + values.taxgid1 + "', " +
                              " '" + values.taxgid2 + "', " +
                              " '" + values.taxgid3 + "', " +
                              " '" + values.tax_prefix + "', " +
                              " '" + values.tax_prefix2 + "', " +
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

                msSQL += "'" + values.discount_amount + "',";
                if(values.product_remarks != null)
                {
                    msSQL += "'" + values.product_remarks.Replace("'", "\\\'") + "',";
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
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void DaPostAgreementOrder(string employee_gid, postagree_list values)
        {
            try
            {

                string totalvalue = values.user_name;
                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select tax_prefix from acp_mst_ttax where tax_gid='" + values.tax_name4 + "'";
                string lstaxname1 = objdbconn.GetExecuteScalar(msSQL);
                string lscustomerbranch = "H.Q";
                string lscampaign_gid = "NO CAMPAIGN";

                msSQL = " select * from smr_tmp_tsalesorderdtl " +
                    " where employee_gid='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count != 0)
                {

                    string inputDate = values.agreement_date;
                    DateTime uiDate = DateTime.ParseExact(inputDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string agreement_date = uiDate.ToString("yyyy-MM-dd");

                    string inputDate1 = values.renewal_date;
                    DateTime uiDate1 = DateTime.ParseExact(inputDate1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string renewal_date = uiDate1.ToString("yyyy-MM-dd");

                    msSQL = " select customer_name from crm_mst_tcustomer where customer_gid='" + values.customer_gid + " '";
                    string lscustomername = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid='" + values.currency_code + " '";
                    string currency_code = objdbconn.GetExecuteScalar(msSQL);

                    string lslocaladdon = "0.00";
                    string lslocaladditionaldiscount = "0.00";
                    string lslocalgrandtotal = " 0.00";
                    string lsgst = "0.00";
                    string lsamount4 = "0.00";

                    double totalAmount = double.TryParse(values.tax_amount4, out double totalpriceValue) ? totalpriceValue : 0;
                    double addonCharges = double.TryParse(values.addon_charge, out double addonChargesValue) ? addonChargesValue : 0;
                    double freightCharges = double.TryParse(values.freight_charges, out double freightChargesValue) ? freightChargesValue : 0;
                    double packingCharges = double.TryParse(values.packing_charges, out double packingChargesValue) ? packingChargesValue : 0;
                    double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                    double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                    double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                    double buybackCharges = double.TryParse(values.buyback_charges, out double buybackChargesValue) ? buybackChargesValue : 0;

                    double grandTotal = (totalAmount + addonCharges + freightCharges + packingCharges + insuranceCharges + roundoff) - additionaldiscountAmount - buybackCharges;

                    msagreeGID = objcmnfunctions.GetMasterGID("AREF");

                    msrenewalGID = objcmnfunctions.GetMasterGID("BRLP");

                    msSQL = " insert  into crm_trn_tagreement (" +
                            " agreement_gid ," +
                            " branch_gid, " +
                            " agreement_date," +
                            " customer_gid, " +
                            " customer_name ," +
                            " customer_address ," +
                            " bill_to," +
                            " shipping_to ," +
                            " created_by," +
                            " agreement_referencenumber, " +
                            " payment_days, " +
                            " delivery_days, " +
                            " Grandtotal, " +
                            " termsandconditions, " +
                            " agreement_status, " +
                            " addon_charge, " +
                            " additional_discount, " +
                            " addon_charge_l, " +
                            " additional_discount_l, " +
                            " grandtotal_l, " +
                            " renewal_date, " +
                            " renewal_gid, " +
                            " renewal_description, " +
                            " currency_code, " +
                            " currency_gid, " +
                            " exchange_rate, " +
                            " updated_addon_charge, " +
                            " agreement_type, " +
                            " tax_gid," +
                            " tax_name, " +
                            " gst_amount," +
                            " total_price," +
                            " total_amount," +
                            " tax_amount," +
                            " updated_additional_discount, " +
                            " freight_charges," +
                            " buyback_charges," +
                            " packing_charges," +
                            " insurance_charges, " +
                            " roundoff ," +
                            " created_date " +
                            " ) values (" +
                            "'" + msagreeGID + "'," +
                            "'" + values.branch_name + "'," +
                            "'" + agreement_date + "'," +
                            "'" + values.customer_gid + "'," +
                            "'" + lscustomername.Replace("'", "\\\'") + "'," +
                            "'" + values.address1.Replace("'", "\\\'") + "'," +
                            "'" + values.address1.Replace("'", "\\\'") + "'," +
                            "'" + values.shipping_address.Replace("'", "\\\'") + "'," +
                            "'" + employee_gid + "'," +
                            "'" + msagreeGID + "'," +
                            "'" + values.payment_days + "'," +
                            "'" + values.delivery_days + "'," +
                            "'" + values.grandtotal.Replace(",", "").Trim() + "'," +
                            "'" + (String.IsNullOrEmpty(values.termsandconditions) ? values.termsandconditions : values.termsandconditions.Replace("'","\\\'")) + "'," +
                            "'Live Agreement'," ;
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
                             "'" + lslocaladditionaldiscount + "'," +
                             "'" + lslocalgrandtotal + "'," +
                             "'" + renewal_date + "'," +
                             "'" + msrenewalGID + "'," +
                             "'" + values.agreement_remarks.Replace("'", "\\\'") + "'," +
                             "'" + currency_code + "'," +
                             "'" + values.currency_code + "'," +
                             "'" + values.exchange_rate + "',";
                            if (values.addon_charge == "")
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + values.addon_charge + "',";
                            }
                    msSQL +="'Live Agreement',"+
                            "'" + values.tax_name4 + "'," +
                            "'" + lstaxname1 + "', " +
                            "'" + lsgst + "',";
                            msSQL += " '" + values.totalamount.Replace(",", "").Trim() + "',";
                            if (values.grandtotal == null && values.grandtotal == "")
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += " '" + values.grandtotal.Replace(",", "").Trim() + "',";
                            }

                            if (values.tax_amount4 != "" && values.tax_amount4 != null)
                            {
                                msSQL += "'" + values.tax_amount4 + "',";
                            }
                            else
                            {
                                msSQL += "'" + lsamount4 + "',";
                            }
                            if (values.additional_discount != "")
                            {
                                msSQL += "'" + values.additional_discount + "',";
                            }
                            else
                            {
                                msSQL += "'" + lslocaladditionaldiscount + "',";
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
                            if (values.roundoff == "")
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += "'" + values.roundoff + "',";
                            }
                     msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                     mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                   if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = " Some Error Occurred While Inserting Agreement order Details";
                        return;
                    }
                   else
                    {
                        msSQL = " Insert into crm_trn_trenewal ( " +
                                " renewal_gid, " +
                                " customer_gid," +
                                " renewal_date, " +
                                " agreement_gid, " +
                                " created_by, " +
                                " renewal_type, " +
                                " created_date) " +
                               " Values ( " +
                                 "'" + msrenewalGID + "'," +
                                 "'" + values.customer_gid + "'," +
                                 "'" + renewal_date + "'," +
                                 "'" + msagreeGID + "'," +
                                 "'" + employee_gid + "'," +
                                 "'Agreement'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    msSQL = " select " +
                            " tmpsalesorderdtl_gid," +
                            " salesorder_gid," +
                            " product_gid," +
                            " productgroup_gid," +
                            " product_remarks," +
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
                             " tax_name2," +
                            " tax2_gid, " +
                            " tax_amount2," +
                             " tax_percentage2," +
                            " slno," +
                            " tax_percentage," +
                            " order_type, " +
                            " taxsegment_gid, " +
                            " taxsegmenttax_gid " +
                            " from smr_tmp_tsalesorderdtl" +
                            " where employee_gid='" + employee_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<postagree_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new postagree_list
                            {
                                tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                                product_gid = dt["product_gid"].ToString(),
                                product_name = dt["product_name"].ToString(),
                                product_code = dt["product_code"].ToString(),
                                productuom_name = dt["uom_name"].ToString(),
                                productgroup_gid = dt["productgroup_gid"].ToString(),
                                product_remarks = dt["product_remarks"].ToString(),
                                unitprice = dt["product_price"].ToString(),
                                quantity = dt["qty_quoted"].ToString(),
                                discountpercentage = dt["discount_percentage"].ToString(),
                                discountamount = dt["discount_amount"].ToString(),
                                tax_name = dt["tax_name"].ToString(),
                                tax_amount = dt["tax_amount"].ToString(),
                                totalamount = dt["price"].ToString(),
                                order_type = dt["order_type"].ToString(),
                                slno = dt["slno"].ToString(),
                                taxsegment_gid = dt["taxsegment_gid"].ToString(),
                                taxsegmenttax_gid = dt["taxsegmenttax_gid"].ToString(),
                            });

                            int i = 0;

                            msagreementdtlGID = objcmnfunctions.GetMasterGID("VSDC");

                            msSQL = " insert into crm_trn_tagreementdtl (" +
                                " agreementdtl_gid ," +
                                " agreement_gid," +
                                " product_gid ," +
                                " productgroup_gid," +
                                " product_name," +
                                " product_code," +
                                " display_field," +
                                " product_remarks," +
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
                                " tax_name2," +
                                " tax2_gid," +
                                " tax_percentage2," +
                                " tax_amount2," +
                                " tax_percentage," +
                                " taxsegment_gid," +
                                " taxsegmenttax_gid," +
                                " created_by," +
                                " type " +
                                ")values(" +
                                " '" + msagreementdtlGID + "'," +
                                " '" + msagreeGID + "'," +
                                " '" + dt["product_gid"].ToString() + "'," +
                                " '" + dt["productgroup_gid"].ToString() + "'," +
                                " '" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                                " '" + dt["product_code"].ToString().Replace("'", "\\\'") + "'," +
                                " '" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +
                                " '" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +
                                " '" + dt["product_price"].ToString() + "'," +
                                " '" + dt["qty_quoted"].ToString() + "'," +
                                " '" + dt["discount_percentage"].ToString() + "'," +
                                " '" + dt["discount_amount"].ToString() + "'," +
                                " '" + dt["tax_amount"].ToString() + "'," +
                                " '" + dt["uom_gid"].ToString() + "'," +
                                " '" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                                " '" + dt["price"].ToString() + "'," +
                                " '" + dt["tax_name"].ToString() + "'," +
                                " '" + dt["tax1_gid"].ToString() + "'," +
                                " '" + dt["tax_name2"].ToString() + "'," +
                                " '" + dt["tax2_gid"].ToString() + "'," +
                                " '" + dt["tax_percentage2"].ToString() + "'," +
                                " '" + dt["tax_amount2"].ToString() + "'," +
                                " '" + dt["tax_percentage"].ToString() + "'," +
                                " '" + dt["taxsegment_gid"].ToString() + "'," +
                                " '" + dt["taxsegmenttax_gid"].ToString() + "'," +
                                " '" + employee_gid + "'," +
                                " '" + dt["order_type"].ToString().Replace("'", "\\\'") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 0)
                            {
                                values.status = false;
                                values.message = "Error occurred while Insertion";
                                return;
                            }

                        }
                    }
                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = " Some Error Occurred While Inserting Salesorder Details";
                        return;
                    }
                    else
                    {
                            msSQL = " delete from smr_tmp_tsalesorderdtl " +
                                    " where employee_gid='" + employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    
                           values.status = true;
                           values.message = "Agreement Order Raised Successfully";
                           return;

                    }
              }
               
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Agreement Order !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetassignTeamSummary(MdlSmrTrnRenewalsummary values)
        {
            try
            {

                msSQL = "SELECT a.campaign_gid, a.campaign_title,a.campaign_prefix, a.campaign_location," +
               " b.branch_name,b.branch_gid,b.branch_prefix,a.campaign_description,a.campaign_mailid, " +
               " (select count(employee_gid) from cmn_trn_tmanagerprivilege where team_gid = a.campaign_gid ) as total_managers," +
               " (SELECT  COUNT(employee_gid) FROM crm_trn_trenewal2employee WHERE campaign_gid = a.campaign_gid) AS total_employees " +
               "FROM crm_trn_trenewalteam a left join hrm_mst_tbranch b on a.campaign_location = b.branch_gid where 1 = 1 ORDER BY a.campaign_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<renewalassignteam_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new renewalassignteam_list
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            branch = dt["branch_name"].ToString(),
                            description = dt["campaign_description"].ToString(),
                            mail_id = dt["campaign_mailid"].ToString(),
                            team_name = dt["campaign_title"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            renewal_gid = dt["campaign_location"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            total_managers = dt["total_managers"].ToString(),
                            total_employees = dt["total_employees"].ToString(),
                        });
                        values.renewalassignteam_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetrenewalToinvoiceDetailsSummary(string renewal_gid, MdlSmrTrnRenewalsummary values)
        {
            try
            {
              msSQL = "SELECT salesorder_gid FROM crm_trn_trenewal WHERE renewal_gid = '" + renewal_gid + "'";
              objodbcDataReader = objdbconn.GetDataReader(msSQL);
              string lssalesorder_gid = null;
               if (objodbcDataReader.HasRows)
                {
                    lssalesorder_gid = objodbcDataReader["salesorder_gid"].ToString();
                }
                if (!string.IsNullOrEmpty(lssalesorder_gid))
                {

                    msSQL = " select a.salesorder_gid as serviceorder_gid,r.renewal_gid,d.currencyexchange_gid,b.customer_id," +
                    "a.branch_gid, f.branch_name,a.payment_days,a.delivery_days, concat(a.so_referenceno1,case when so_referencenumber='' then '' else concat(' ','-',' '," +
                    " case when so_referencenumber is not null then so_referenceno1 else '' end) end )as so_reference," +
                    " DATE_format(a.salesorder_date, '%d-%m-%Y') as serviceorder_date, " +
                    " concat(a.customer_name,'/',c.email) as customer_name,b.customer_gid,format(a.grandtotal, 2) as grand_total,a.shipping_to," +
                    " a.customer_contact_person as customercontact_name,c.email as email,b.customer_code," +
                    " a.termsandconditions,c.mobile,b.gst_number," +
                    " a.addon_charge as addon_amount ,a.additional_discount as discount_amount," +
                    " a.customer_address,format(total_amount,2) as order_total ," +
                    " a.freight_charges as freight_charges," +
                    " format(a.buyback_charges,2)as buyback_charges," +
                    " format(a.packing_charges,2)as packing_charges," +
                    " format(a.insurance_charges,2)as insurance_charges," +
                    "a.tax_name,a.tax_gid,a.roundoff,a.tax_amount,a.tax_name4," +
                    " a.currency_code, a.currency_gid, a.exchange_rate, c.address1,c.address2,c.city," +
                    " c.state,c.country_gid,c.zip_code,e.country_name " +
                    " from smr_trn_tsalesorder a" +
                    " left join crm_mst_tcustomer b on a.customer_gid=b.customer_gid " +
                    " left join crm_mst_tcustomercontact c on b.customer_gid = c.customer_gid " +
                    " left join crm_trn_tcurrencyexchange d on d.currency_code=a.currency_code" +
                    " left join adm_mst_tcountry e on c.country_gid=e.country_gid " +
                    " left join hrm_mst_tbranch f on f.branch_gid=a.branch_gid" +
                    " LEFT JOIN crm_trn_trenewal r ON a.salesorder_gid = r.salesorder_gid " +
                    " where r.renewal_gid='" + renewal_gid + "'";
                }
                else
                {
                    msSQL = " select a.agreement_gid as serviceorder_gid,r.renewal_gid,d.currencyexchange_gid,b.customer_id," +
                 "a.branch_gid, f.branch_name,a.payment_days,a.delivery_days, concat(a.agreement_referenceno1,case when agreement_referencenumber='' then '' else concat(' ','-',' '," +
                 " case when agreement_referencenumber is not null then agreement_referenceno1 else '' end) end )as so_reference," +
                 " DATE_format(a.agreement_date, '%d-%m-%Y') as serviceorder_date, " +
                 " concat(a.customer_name,'/',c.email) as customer_name,b.customer_gid,format(a.grandtotal, 2) as grand_total,a.shipping_to," +
                 " a.customer_contact_person as customercontact_name,c.email as email,b.customer_code," +
                 " a.termsandconditions,c.mobile,b.gst_number," +
                 " a.addon_charge as addon_amount ,a.additional_discount as discount_amount," +
                 " a.customer_address,format(total_amount,2) as order_total ," +
                 " a.freight_charges as freight_charges," +
                 " format(a.buyback_charges,2)as buyback_charges," +
                 " format(a.packing_charges,2)as packing_charges," +
                 " format(a.insurance_charges,2)as insurance_charges," +
                 "a.tax_name,a.tax_gid,a.roundoff,a.tax_amount,a.tax_name4," +
                 " a.currency_code, a.currency_gid, a.exchange_rate, c.address1,c.address2,c.city," +
                 " c.state,c.country_gid,c.zip_code,e.country_name " +
                 " from crm_trn_tagreement a" +
                 " left join crm_mst_tcustomer b on a.customer_gid=b.customer_gid " +
                 " left join crm_mst_tcustomercontact c on b.customer_gid = c.customer_gid " +
                 " left join crm_trn_tcurrencyexchange d on d.currency_code=a.currency_code" +
                 " left join adm_mst_tcountry e on c.country_gid=e.country_gid " +
                 " left join hrm_mst_tbranch f on f.branch_gid=a.branch_gid" +
                 " LEFT JOIN  crm_trn_trenewal r ON a.agreement_gid = r.agreement_gid " +
                 " where r.renewal_gid='" + renewal_gid + "'";

                }

                var Getordertoinvoice = new List<Getrenewaltoinvoice_list>();
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count > 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            Getordertoinvoice.Add(new Getrenewaltoinvoice_list
                            {
                                salesorder_gid = dt["serviceorder_gid"].ToString(),
                                so_reference = dt["so_reference"].ToString(),
                                renewal_gid = dt["renewal_gid"].ToString(),
                                serviceorder_date = dt["serviceorder_date"].ToString(),
                                customer_name = dt["customer_name"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),
                                grand_total = dt["grand_total"].ToString(),
                                customercontact_name = dt["customercontact_name"].ToString(),
                                email = dt["email"].ToString(),
                                customer_code = dt["customer_code"].ToString(),
                                termsandconditions = dt["termsandconditions"].ToString(),
                                customer_mobile = dt["mobile"].ToString(),
                                addon_amount = dt["addon_amount"].ToString(),
                                discount_amount = dt["discount_amount"].ToString(),
                                shipping_to = dt["shipping_to"].ToString(),
                                customer_address = dt["customer_address"].ToString(),
                                order_total = dt["order_total"].ToString(),
                                freight_charges = dt["freight_charges"].ToString(),
                                buyback_charges = dt["buyback_charges"].ToString(),
                                packing_charges = dt["packing_charges"].ToString(),
                                insurance_charges = dt["insurance_charges"].ToString(),
                                currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                                customer_id = dt["customer_id"].ToString(),
                                currency_code = dt["currency_code"].ToString(),
                                currency_gid = dt["currency_gid"].ToString(),
                                exchange_rate = dt["exchange_rate"].ToString(),
                                country_name = dt["country_name"].ToString(),
                                zip_code = dt["zip_code"].ToString(),
                                country_gid = dt["country_gid"].ToString(),
                                state = dt["state"].ToString(),
                                city = dt["city"].ToString(),
                                address1 = dt["address1"].ToString(),
                                address2 = dt["address2"].ToString(),
                                branch_gid = dt["branch_gid"].ToString(),
                                branch_name = dt["branch_name"].ToString(),
                                delivery_days = dt["delivery_days"].ToString(),
                                payment_days = dt["payment_days"].ToString(),
                                gst_number = dt["gst_number"].ToString(),
                                tax_name = dt["tax_name"].ToString(),
                                tax_gid = dt["tax_gid"].ToString(),
                                roundoff = dt["roundoff"].ToString(),
                                tax_amount = dt["tax_amount"].ToString(),
                                tax_name4 = dt["tax_name4"].ToString(),
                            });
                            values.Getrenewaltoinvoice_list = Getordertoinvoice;
                        }
                    }
                }
            catch (Exception ex)
            {
                values.message = "Exception occurred while Order to invoice summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                  msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                  DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetrenewalToInvoiceProductSummary(string employee_gid, string renewal_gid, MdlSmrTrnRenewalsummary values)
        {
            try
            {
                msSQL = " delete from rbl_tmp_tinvoicedtl where employee_gid='" + employee_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "SELECT salesorder_gid FROM crm_trn_trenewal WHERE renewal_gid = '" + renewal_gid + "'";
                objodbcDataReader = objdbconn.GetDataReader(msSQL);
                string lssalesorder_gid = null;
                if (objodbcDataReader.HasRows)
                {
                    lssalesorder_gid = objodbcDataReader["salesorder_gid"].ToString();
                }
                if (!string.IsNullOrEmpty(lssalesorder_gid))
                {
                    msSQL = " select a.salesorderdtl_gid as serviceorderdtl_gid,a.salesorder_gid," +
                " if(a.customerproduct_code='&nbsp;',' ',a.customerproduct_code) as customerproduct_code," +
                " a.product_gid,a.tax1_gid,a.tax2_gid,a.tax3_gid,format(a.qty_quoted,2) as qty_quoted,a.uom_gid,a.uom_name," +
                " format(a.vendor_price,2) as amount ,format(a.product_price,2) as productprice,a.price," +
                " format(a.tax_amount,2) as tax_amount1, " +
                " format(a.tax_amount2,2) as tax_amount2,format(a.tax_amount3,2) as tax_amount3," +
                " format(a.price,2) as total_amount,c.productgroup_gid, c.productgroup_name,a.product_code, " +
                " a.display_field as description,a.product_remarks as description1, a.tax_name as tax_name1,a.tax_name2,a.tax_name3, " +
                " format(a.discount_amount,2) as discount_amount,a.margin_percentage, " +
                " format(a.discount_percentage,2) as discount_percentage,a.product_name, a.taxsegment_gid, " +
                "a.tax_percentage,a.tax_percentage2,a.tax_percentage3 " +
                " from smr_trn_tsalesorderdtl a " +
                " left join pmr_mst_tproductgroup c on c.productgroup_gid=a.productgroup_gid " +
                " LEFT JOIN crm_trn_trenewal j ON j.salesorder_gid = a.salesorder_gid " +
               " WHERE j.renewal_gid = '" + renewal_gid + "' " +
               " group by a.product_gid,a.salesorderdtl_gid order by a.salesorderdtl_gid asc ";
                }
                else
                {
                    msSQL = " select a.agreementdtl_gid as serviceorderdtl_gid, a.agreement_gid  as salesorder_gid," +
                                    " if(a.customerproduct_code='&nbsp;',' ',a.customerproduct_code) as customerproduct_code," +
                                    " a.product_gid,a.tax1_gid,a.tax2_gid,a.tax3_gid,format(a.qty_quoted,2) as qty_quoted,a.uom_gid,a.uom_name," +
                                    " format(a.vendor_price,2) as amount ,format(a.product_price,2) as productprice,a.price," +
                                    " format(a.tax_amount,2) as tax_amount1, " +
                                    " format(a.tax_amount2,2) as tax_amount2,format(a.tax_amount3,2) as tax_amount3," +
                                    " format(a.price,2) as total_amount,c.productgroup_gid, c.productgroup_name,a.product_code, " +
                                    " a.display_field as description,a.product_remarks as description1, a.tax_name as tax_name1,a.tax_name2,a.tax_name3, " +
                                    " format(a.discount_amount,2) as discount_amount,a.margin_percentage, " +
                                    " format(a.discount_percentage,2) as discount_percentage,a.product_name, a.taxsegment_gid, " +
                                    "a.tax_percentage,a.tax_percentage2,a.tax_percentage3 " +
                                    " from crm_trn_tagreementdtl a " +
                                    " left join pmr_mst_tproductgroup c on c.productgroup_gid=a.productgroup_gid" +
                                    " LEFT JOIN crm_trn_trenewal j ON j.agreement_gid = a.agreement_gid " +
                                   " WHERE j.renewal_gid = '" + renewal_gid + "' " +
                                   " group by a.product_gid,serviceorderdtl_gid order by serviceorderdtl_gid asc ";
                }
                  
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string invoicedtl_gid = objcmnfunctions.GetMasterGID("SIVC");
                        msSQL = " insert into rbl_tmp_tinvoicedtl ( " +
                        " invoicedtl_gid, invoice_gid, product_gid," +
                        " qty_invoice,product_price, discount_percentage," +
                        " discount_amount, tax_amount, product_total," +
                        " uom_gid, uom_name, tax_amount2, tax_amount3," +
                        " tax_name, tax_name2,tax_name3,display_field," +
                        " tax1_gid, tax2_gid, tax3_gid, product_name," +
                        " productgroup_gid," +
                        " productgroup_name, " +
                        " employee_gid, " +
                        " selling_price, " +
                        " product_code," +
                        " customerproduct_code, " +
                        " tax_percentage," +
                        " tax_percentage2," +
                        " tax_percentage3 ," +
                        " vendor_price, " +
                        " product_remarks " +
                        ") values (" +
                        "'" + invoicedtl_gid + "'," +
                        "'" + dt["salesorder_gid"] + "'," +
                        "'" + dt["product_gid"] + "'," +
                        "'" + dt["qty_quoted"] + "'," +
                        "'" + dt["productprice"].ToString().Replace(",", "") + "'," +
                        "'" + dt["discount_percentage"] + "'," +
                        "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                        "'" + dt["tax_amount1"].ToString().Replace(",", "") + "'," +
                        "'" + dt["total_amount"].ToString().Replace(",", "") + "'," +
                        "'" + dt["uom_gid"] + "'," +
                        "'" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                        "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                        "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                        "'" + dt["tax_name1"].ToString() + "'," +
                        "'" + dt["tax_name2"].ToString() + "'," +
                        "'" + dt["tax_name3"].ToString() + "'," +
                        "'" + dt["description"].ToString().Replace("'", "\\\'") + "'," +
                        "'" + dt["tax1_gid"].ToString() + "'," +
                        "'" + dt["tax2_gid"].ToString() + "'," +
                        "'" + dt["tax3_gid"].ToString() + "'," +
                        "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                        "'" + dt["productgroup_gid"].ToString() + "'," +
                        "'" + dt["productgroup_name"].ToString().Replace("'", "\\\'") + "'," +
                        "'" + employee_gid + "'," +
                        "'" + dt["productprice"].ToString().Replace(",", "") + "'," +
                        "'" + dt["product_code"].ToString().Replace("'", "\\\'") + "'," +
                        "'" + dt["customerproduct_code"].ToString().Replace("'", "\\\'") + "'," +
                        "'" + dt["tax_percentage"] + "'," +
                        "'" + dt["tax_percentage2"] + "'," +
                        "'" + dt["tax_percentage3"] + "'," +
                        "'" + dt["productprice"].ToString().Replace(",", "") + "'," +
                        "'" + dt["description"].ToString().Replace("'", "\\\'") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        ;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while Order to invoice product summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                  msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                  DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetrenewalToInvoiceProductDetails(string employee_gid, string renewal_gid, MdlSmrTrnSalesorder values)
        {
            try
            {

                string lssalesorder_gid = null;
                double grand_total = 0.00;
                double grandtotal = 0.00;
           
                msSQL = "SELECT  a.invoicedtl_gid,a.invoice_gid, a.product_gid, a.product_name," +
                    "a.customerproduct_code, a.product_remarks,a.product_code, b.productgroup_gid," +
                    " FORMAT(a.qty_invoice, 2) AS qty_ordered, c.productgroup_name, a.uom_name, b.producttype_gid," +
                    " a.created_by, a.product_price, FORMAT(a.product_price, 2) AS producttotal_price," +
                    " a.discount_percentage,a.product_total, FORMAT(a.discount_amount, 2) AS discount_amount," +
                    " a.tax_percentage, FORMAT(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field," +
                    " a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2,a.tax_percentage3," +
                    " FORMAT(a.tax_amount2, 2) AS tax_amount2, FORMAT(a.tax_amount3, 2) AS tax_amount3, a.tax1_gid," +
                    " a.tax2_gid, a.tax3_gid ," +
                    " concat( case when a.tax_name is null then '' else a.tax_name end, ' '," +
                    " case when a.tax_percentage = '0' then '' else concat('',a.tax_percentage,'%') end ," +
                    " case when a.tax_amount = '0' then '' else concat(':',a.tax_amount) end) as tax," +
                    " concat(case when a.tax_name2 is null then '' else a.tax_name2 end, ' ', " +
                    " case when a.tax_percentage2 = '0' then '' else concat('', a.tax_percentage2, '%') end, " +
                    " case when a.tax_amount2 = '0' then '' else concat(':', a.tax_amount2) end) as tax2," +
                    " concat(case when a.tax_name3 is null then '' else a.tax_name3 end, ' '," +
                    " case when a.tax_percentage3 = '0' then '' else concat('', a.tax_percentage3, ' %   ')" +
                    " end, case when a.tax_amount3 = '0' then '  ' else concat(' : ', a.tax_amount3) end) as tax3," +
                    "format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.taxsegment_gid " +
                    " FROM rbl_tmp_tinvoicedtl a  LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                    " LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                    " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid  " +
                    " WHERE a.employee_gid = '" + employee_gid + "'";
                      

                var GetOrderToInvoiceProduct = new List<GetOrderToInvoiceProduct_list>();
                DataTable dt_datatable1 = objdbconn.GetDataTable(msSQL);
                if (dt_datatable1.Rows.Count > 0)
                {
                    foreach (DataRow dt in dt_datatable1.Rows)
                    {
                        grand_total += double.Parse(dt["product_total"].ToString());
                        grandtotal += double.Parse(dt["product_total"].ToString());
                        GetOrderToInvoiceProduct.Add(new GetOrderToInvoiceProduct_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoicedtl_gid = dt["invoicedtl_gid"].ToString(),
                            customerproduct_code = dt["customerproduct_code"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            tax1_gid = dt["tax1_gid"].ToString(),
                            tax2_gid = dt["tax2_gid"].ToString(),
                            tax3_gid = dt["tax3_gid"].ToString(),
                            qty_quoted =double.Parse( dt["qty_ordered"].ToString()),
                            uom_gid = dt["uom_gid"].ToString(),
                            product_total = double.Parse(dt["product_total"].ToString()),
                            productprice = dt["product_price"].ToString(),
                            tax_amount1 = double.Parse(dt["tax_amount"].ToString()),
                            tax_amount2 = double.Parse(dt["tax_amount2"].ToString()),
                            tax_amount3 = double.Parse(dt["tax_amount3"].ToString()),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            tax_name1 = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            discount_amount = double.Parse(dt["discount_amount"].ToString()),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            taxamount = double.Parse(dt["taxamount"].ToString()),
                        });
                        values.GetOrderToInvoiceProduct_list = GetOrderToInvoiceProduct;
                        values.grand_total = Math.Round(grand_total, 2);
                        values.grandtotal = Math.Round(grandtotal, 2);
                    }
                    dt_datatable1.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while Order to invoice summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                  msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" +
                  DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostrenewalToInvoiceProductAdd(string employee_gid, ordertoinvoiceproductsubmit_list1 values)
        {
            try
            {

                double discount_precentage = double.TryParse(values.discountprecentage, out double discountprecentage) ? discountprecentage : 0;
                double discount_amount = double.TryParse(values.discount_amount, out double discountamount) ? discountamount : 0;
                msSQL = " SELECT a.productuom_gid, a.product_gid,a.customerproduct_code, a.product_name, b.productuom_name" +
                    " ,a.productgroup_gid FROM pmr_mst_tproduct a " +
                 " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                 " WHERE a.product_gid = '" + values.product_name + "'";
                objodbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objodbcDataReader.HasRows == true)
                {
                    lsproductgid = objodbcDataReader["product_gid"].ToString();
                    lsproductuom_gid = objodbcDataReader["productuom_gid"].ToString();
                    lsproduct_name = objodbcDataReader["product_name"].ToString();
                    lsproductuom_name = objodbcDataReader["productuom_name"].ToString();
                    lscustomerproduct_code = objodbcDataReader["customerproduct_code"].ToString();
                }
                string invoicedtl_gid = objcmnfunctions.GetMasterGID("SIVC");
                string invoice_gid = objcmnfunctions.GetMasterGID("INV");
                msGetGid = objcmnfunctions.GetMasterGID("SIVC");
                msSQL = " insert into rbl_tmp_tinvoicedtl ( " +
                        " invoicedtl_gid, invoice_gid, product_gid," +
                        " qty_invoice,product_price, discount_percentage," +
                        " discount_amount, tax_amount, product_total," +
                        " uom_gid, uom_name, tax_amount2, tax_amount3," +
                        " tax_name, tax_name2,tax_name3,display_field," +
                        " tax1_gid, tax2_gid, tax3_gid, product_name," +
                        " productgroup_gid," +
                        " productgroup_name, " +
                        " employee_gid, " +
                        " selling_price, " +
                        " product_code," +
                        " customerproduct_code, " +
                        " tax_percentage," +
                        " tax_percentage2," +
                        " tax_percentage3 ," +
                        " vendor_price, " +
                        " product_remarks " +
                        " ) values ( " +
                        "'" + invoicedtl_gid + "'," +
                        "'" + values.salesorder_gid + "'," +
                        "'" + lsproductgid + "'," +
                        "'" + values.productquantity + "'," +
                        "'" + values.unitprice + "'," +
                        "'" + discount_precentage + "'," +
                        "'" + discount_amount + "'," +
                        "'" + values.taxamount1 + "'," +
                        "'" + values.producttotal_amount + "'," +
                        "'" + lsproductuom_gid + "'," +
                        "'" + lsproductuom_name + "'," +
                        "'" + values.taxamount2 + "'," +
                        "'" + values.taxamount3 + "'," +
                        "'" + values.tax_prefix + "'," +
                        "'" + values.tax_prefix2 + "'," +
                        "'" + values.tax_prefix3 + "'," +
                        "'" + lsproduct_name + "'," +
                        "'" + values.taxgid1 + "'," +
                        "'" + values.taxgid2 + "'," +
                        "'" + values.taxgid3 + "'," +
                        "'" + lsproduct_name + "'," +
                        "'" + values.productgroup_gid + "'," +
                        "'" + values.productgroup_name + "'," +
                        "'" + employee_gid + "'," +
                        "'" + values.unitprice + "'," +
                        "'" + values.product_code + "'," +
                        "'" + lscustomerproduct_code + "'," +
                        "'" + values.taxprecentage1 + "'," +
                        "'" + values.taxprecentage2 + "'," +
                        "'" + values.taxprecentage3 + "'," +
                        "'" + values.unitprice + "'," +
                        "'" + values.product_desc + "')";

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
                values.message = "Exception occurred while changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostOnsubmitrenewaltoInvoice(string employee_gid, string user_gid, renewaltoinvoicesubmit values)
        {
            try
            {
                
                string lscustomercontactname = "";
                string lscustomeremail = "";
                string lscustomermobile = "";
                string lscustomercontactgid = "";
                double addonCharges = double.TryParse(values.addon_charge, out double addonChargesValue) ? addonChargesValue : 0;
                double freightCharges = double.TryParse(values.freight_charges, out double freightChargesValue) ? freightChargesValue : 0;
                double forwardingCharges = double.TryParse(values.forwardingCharges, out double packingChargesValue) ? packingChargesValue : 0;
                double insuranceCharges = double.TryParse(values.insurance_charges, out double insuranceChargesValue) ? insuranceChargesValue : 0;
                double roundoff = double.TryParse(values.roundoff, out double roundoffValue) ? roundoffValue : 0;
                double additionaldiscountAmount = double.TryParse(values.additional_discount, out double discountAmountValue) ? discountAmountValue : 0;
                double buybackCharges = double.TryParse(values.buybackcharges, out double buybackChargesValue) ? buybackChargesValue : 0;
                double lsexchange = double.Parse(values.exchange_rate);
                double lstotalamount_l = Math.Round((double.Parse(values.producttotalamount) * lsexchange), 2);
                double lsgrandtotal_l = Math.Round((double.Parse(values.grandtotal) * lsexchange), 2);
                double lsaddoncharges_l = Math.Round((double.Parse(values.addon_charge) * lsexchange), 2);
                double lsadditionaldiscountAmount_l = Math.Round((additionaldiscountAmount * lsexchange), 2);
                double addonCharges_l = Math.Round(addonCharges * double.Parse(values.exchange_rate), 2);
                double freightCharges_l = Math.Round(freightCharges * double.Parse(values.exchange_rate), 2); ;
                double forwardingCharges_l = Math.Round(forwardingCharges * double.Parse(values.exchange_rate), 2);
                double insuranceCharges_l = Math.Round(insuranceCharges * double.Parse(values.exchange_rate), 2);
                double additionaldiscountAmount_l = Math.Round(additionaldiscountAmount * double.Parse(values.exchange_rate), 2);
                double buybackCharges_l = Math.Round(buybackCharges * double.Parse(values.exchange_rate), 2);

                string uiDateStr = values.invoice_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string mysqlinvoiceDate = uiDate.ToString("yyyy-MM-dd");

                string uiDateStr2 = values.due_date;
                DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string mysqlpaymentDate = uiDate2.ToString("yyyy-MM-dd");

                string uiDateStr3 = values.renewal_date;
                DateTime uiDate3 = DateTime.ParseExact(uiDateStr3, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string renewalDate = uiDate3.ToString("yyyy-MM-dd");

                msINGetGID = objcmnfunctions.GetMasterGID("SIVT");
                string ls_referenceno = "";
                ls_referenceno = objcmnfunctions.getSequencecustomizerGID("INV", "RBL", values.branch_gid);

                msSQL = "select customer_name, customer_code, customer_id from crm_mst_tcustomer where customer_gid='" + values.customer_gid + "'";
                dt = objdbconn.GetDataReader(msSQL);
                if (dt.HasRows == true)
                {
                    dt.Read();
                    lsCustomername = dt["customer_name"].ToString();
                    lscustomer_code = dt["customer_code"].ToString();
                    lscustomer_id = dt["customer_id"].ToString();
                }
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

                }

                msSQL = " select customercontact_name, email, mobile, concat(address1,' ', address2) as address from crm_mst_tcustomercontact" +
                    " where customer_gid='" + values.customer_gid + "'";
                objodbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objodbcDataReader.HasRows == true)
                {
                    values.customercontactperson = objodbcDataReader["customercontact_name"].ToString();
                    values.customercontactnumber = objodbcDataReader["mobile"].ToString();
                    values.customeraddress = objodbcDataReader["address"].ToString();
                    values.customeremailaddress = objodbcDataReader["email"].ToString();
                }
                msSQL = "SELECT salesorder_gid,agreement_gid FROM crm_trn_trenewal WHERE renewal_gid = '" + values.renewal_gid + "'";
                objodbcDataReader = objdbconn.GetDataReader(msSQL);
                string lssalesorder_gid = null;
                if (objodbcDataReader.HasRows)
                {
                    lssalesorder_gid = objodbcDataReader["salesorder_gid"].ToString();
                    if (lssalesorder_gid==null||lssalesorder_gid=="")
                    {
                        lssalesorder_gid = objodbcDataReader["agreement_gid"].ToString();

                    }
                }


                msSQL = "select currencyexchange_gid from crm_trn_tcurrencyexchange  where currency_code='" + values.currency_code + "'";
                string lscurrencycodegid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select tax_gid from acp_mst_ttax  where tax_name='" + values.tax_name4.Replace("'", "\\\'") + "'";
                string lstaxgid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = "select tax_percentage from acp_mst_ttax  where tax_gid='" + lstaxgid + "'";
                string lstaxpercentage = objdbconn.GetExecuteScalar(msSQL);

               

                msSQL = "SELECT  a.invoicedtl_gid, a.product_gid, a.customerproduct_code,a.product_name, a.product_remarks,a.product_code, b.productgroup_gid," +
                " format(a.qty_invoice, 2) AS qty_ordered_1, a.qty_invoice as qty_ordered, c.productgroup_name, a.uom_name, b.producttype_gid," +
                " a.created_by, a.product_price, format(a.product_price, 2) AS producttotal_price," +
                " a.discount_percentage,a.product_total,a.discount_amount, format(a.discount_amount, 2) AS discount_amount_1," +
                " a.tax_percentage, format(a.tax_amount, 2) AS tax_amount, a.uom_gid, a.display_field," +
                " a.tax_name, a.tax_name2, a.tax_name3, a.tax_percentage2,a.tax_percentage3," +
                " format(a.tax_amount2, 2) AS tax_amount2, format(a.tax_amount3, 2) AS tax_amount3, a.tax1_gid," +
                " a.tax2_gid, a.tax3_gid ," +
                " concat( case when a.tax_name is null then '' else a.tax_name end, ' '," +
                " case when a.tax_percentage = '0' then '' else concat('',a.tax_percentage,'%') end ," +
                " case when a.tax_amount = '0' then '' else concat(':',a.tax_amount) end) as tax," +
                " concat(case when a.tax_name2 is null then '' else a.tax_name2 end, ' ', " +
                " case when a.tax_percentage2 = '0' then '' else concat('', a.tax_percentage2, '%') end, " +
                " case when a.tax_amount2 = '0' then '' else concat(':', a.tax_amount2) end) as tax2," +
                " concat(case when a.tax_name3 is null then '' else a.tax_name3 end, ' '," +
                " case when a.tax_percentage3 = '0' then '' else concat('', a.tax_percentage3, ' %   ')" +
                " end, case when a.tax_amount3 = '0' then '  ' else concat(' : ', a.tax_amount3) end) as tax3," +
                " format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.taxsegment_gid,vendor_price " +
                " FROM rbl_tmp_tinvoicedtl a  " +
                "LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                " LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid" +
                " LEFT JOIN pmr_mst_tproductuom d ON a.uom_gid = d.productuom_gid  " +
                " WHERE a.employee_gid = '" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    double lsvendorprice = Math.Round(double.Parse(dt["vendor_price"].ToString().Replace(",", "")), 2);
                    double lsproduct_discount = Math.Round(double.Parse(dt["discount_amount"].ToString().Replace(",", "")), 2);

                    double lsproduct_price = Math.Round((double.Parse(dt["qty_ordered"].ToString()) * lsvendorprice) - lsproduct_discount, 2);
                    double lsexchangerate = double.Parse(values.exchange_rate);
                    double lsproductprice_l = Math.Round((lsproduct_price * lsexchangerate), 2);
                    double lstaxamount_l = Math.Round((double.Parse(dt["tax_amount"].ToString().Replace(",", "")) * lsexchangerate), 2);
                    double lstaxamount2_l = Math.Round((double.Parse(dt["tax_amount2"].ToString().Replace(",", "")) * lsexchangerate), 2);
                    double lstaxamount3_l = Math.Round((double.Parse(dt["tax_amount3"].ToString().Replace(",", "")) * lsexchangerate), 2);
                    double lsdiscountamount_l = Math.Round((double.Parse(dt["discount_amount"].ToString().Replace(",", "")) * lsexchangerate), 2);
                    double lsproducttotal_l = Math.Round((double.Parse(dt["product_total"].ToString().Replace(",", "")) * lsexchangerate), 2);
                    msGetGid = objcmnfunctions.GetMasterGID("SIVC");

                    msSQL = " insert into rbl_trn_tinvoicedtl (" +
                        " invoicedtl_gid, " +
                        " invoice_gid, " +
                        " invoice_reference, " +
                        " product_gid, " +
                        " product_code, " +
                        " productgroup_gid, " +
                        " product_name, " +
                        " uom_gid, " +
                        " productuom_name, " +
                        " product_price, " +
                        " discount_percentage, " +
                        " discount_amount, " +
                        " tax_name, " +
                        " tax_name2, " +
                        " tax_name3, " +
                        " tax1_gid," +
                        " tax2_gid," +
                        " tax3_gid," +
                        " tax_percentage, " +
                        " tax_percentage2, " +
                        " tax_percentage3, " +
                        " tax_amount, " +
                        " tax_amount2, " +
                        " tax_amount3, " +
                        " qty_invoice, " +
                        " product_remarks, " +
                        " product_total, " +
                        " product_price_L, " +
                        " discount_amount_L, " +
                        " tax_amount1_L, " +
                        " tax_amount2_L, " +
                        " tax_amount3_L, " +
                        " product_total_L, " +
                        " display_field, " +
                        " customerproduct_code," +
                        " taxsegment_gid," +
                        " vendor_price," +
                        " created_by," +
                        " created_date " +
                            " ) values ( " +
                            "'" + msGetGid + "'," +
                            "'" + msINGetGID + "'," +
                            "'" + values.salesorder_gid + "'," +
                            "'" + dt["product_gid"].ToString() + "'," +
                            "'" + dt["product_code"].ToString().Replace("'", "\\\'") + "'," +
                             "'" + dt["productgroup_gid"].ToString() + "'," +
                            "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                            "'" + dt["uom_gid"].ToString() + "'," +
                            "'" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                            "'" + dt["product_price"].ToString() + "'," +
                            "'" + dt["discount_percentage"].ToString() + "'," +
                            "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_name"].ToString() + "'," +
                            "'" + dt["tax_name2"].ToString() + "'," +
                            "'" + dt["tax_name3"].ToString() + "'," +
                            "'" + dt["tax1_gid"].ToString() + "'," +
                            "'" + dt["tax2_gid"].ToString() + "'," +
                            "'" + dt["tax3_gid"].ToString() + "'," +
                            "'" + dt["tax_percentage"].ToString() + "'," +
                            "'" + dt["tax_percentage2"].ToString() + "'," +
                            "'" + dt["tax_percentage3"].ToString() + "'," +
                            "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                            "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                            "'" + dt["qty_ordered"].ToString() + "'," +
                            "'" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +

                            "'" + dt["product_total"].ToString().Replace(",", "") + "'," +
                            "'" + lsproductprice_l + "'," +
                            "'" + lsdiscountamount_l + "'," +
                            "'" + lstaxamount_l + "'," +
                            "'" + lstaxamount2_l + "'," +
                            "'" +lstaxamount3_l + "'," +
                            "'" + lsproducttotal_l + "'," +
                            "'" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +
                            "'" + dt["product_code"].ToString() + "'," +
                            "'" + dt["taxsegment_gid"].ToString() + "'," +
                            "'" + dt["vendor_price"].ToString() + "'," +
                            "'" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')"
                            ;
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update smr_Trn_Tsalesorderdtl set qty_executed='" + dt["qty_ordered"].ToString() + "' where salesorder_gid='" + values.salesorder_gid + "' and product_gid='" + dt["product_gid"].ToString() + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (!string.IsNullOrEmpty(lssalesorder_gid))
                    {
                        mssalesorderGID1 = objcmnfunctions.GetMasterGID("VSDC");
                        if (mssalesorderGID1 == "E")
                        {
                            values.message = "Create Sequence code for VSDC ";
                            return;
                        }
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
                           "'" + dt["customerproduct_code"].ToString().Replace("'", "\\\'") + "'," +
                           "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                           "'" + dt["product_code"].ToString().Replace("'", "\\\'") + "'," +
                           "'" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +
                           "'" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +
                           "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
                           "'" + dt["qty_ordered"].ToString() + "'," +
                           "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                           "'" + dt["uom_gid"].ToString() + "'," +
                           "'" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                           "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
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
                           "'" + dt["tax_amount"].ToString().Replace(",", "") + "'," +
                           "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                           "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                           "'" + dt["discount_percentage"].ToString() + "'," +
                           "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                           "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
                           "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
                           "'" + dt["taxsegment_gid"].ToString().Replace(",", "") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {
                            values.status = false;
                            values.message = "Error occurred while Insertion";
                            return;
                        }
                    }
                    
                }
                string so_reference = "SOREF" + GetRandomString(5);
                string lstype = string.Empty;
                msSQL = "select distinct type from smr_trn_tsalesorderdtl where salesorder_gid='" + values.salesorder_gid + "' ";
                string lstype_1 = objdbconn.GetExecuteScalar(msSQL);
                if (lstype == "")
                {
                    lstype = lstype_1;
                }
                else
                {
                    lstype = "Both";
                }



                msSQL = "select so_type from smr_trn_tsalesorder a " +
               "where a.salesorder_gid='" + values.salesorder_gid + "'";
                string order_type = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " insert into rbl_trn_tinvoice(" +
                       " invoice_gid," +
                       " invoice_date," +
                       " payment_term, " +
                       " payment_date," +
                       " invoice_from," +
                       " invoice_type," +
                       " customer_gid," +
                       " customer_name," +
                       " customer_contactperson," +
                       " customer_contactnumber," +
                       " customer_address," +
                       " customer_email," +
                       " mode_of_despatch," +
                       " total_amount," +
                       " invoice_amount," +
                       " invoice_refno," +
                       " invoice_status," +
                       " invoice_flag," +
                       " user_gid," +
                       " discount_amount," +
                       " additionalcharges_amount," +
                       " total_amount_L," +
                       " invoice_amount_L," +
                       " discount_amount_L," +
                       " additionalcharges_amount_L," +
                       " invoice_remarks," +
                       " termsandconditions," +
                       " currency_code," +
                       " exchange_rate," +
                       " branch_gid, " +
                       " roundoff," +
                       " tax_gid," +
                       " tax_name," +
                       " tax_percentage, " +
                       " tax_amount," +
                       " taxsegment_gid," +
                       " created_date," +
                       " freight_charges," +
                       " packing_charges," +
                       " delivery_date," +
                       " payment_days," +
                       " delivery_days," +
                       " insurance_charges, " +
                       " invoice_reference, " +
                       " shipping_to, " +
                       " renewal_gid, " +
                       " sales_type, " +
                       " bill_email " +
                       " ) values (" +
                       " '" + msINGetGID + "'," +
                       "'" + mysqlinvoiceDate + "'," +
                       "'" + (String.IsNullOrEmpty(values.payment_days) ? values.payment_days : values.payment_days.Replace("'","\\\'")) + "'," +
                       "'" + mysqlpaymentDate + "'," +
                       "'" + order_type + "'," +
                       "'" + order_type + "'," +
                       " '" + values.customer_gid + "'," +
                       " '" + lsCustomername.Replace("'","\\\'") + "'," +
                       " '" + values.customercontactperson.Replace("'", "\\\'") + "'," +
                       " '" + values.customercontactnumber + "'," +
                       " '" + (String.IsNullOrEmpty(values.customer_address) ? values.customer_address : values.customer_address.Replace("'", "\\\'")) + "'," +
                       " '" + values.customeremailaddress + "'," +
                       " '" + (String.IsNullOrEmpty(values.dispatch_mode) ? values.dispatch_mode : values.dispatch_mode.Replace("'","\\\'")) + "'," +
                       " '" + values.producttotalamount + "'," +
                       " '" + values.grandtotal + "'," +
                       " '" + ls_referenceno + "'," +
                       " 'Payment Pending'," +
                       " 'Invoice Approved'," +
                       " '" + employee_gid + "'," +
                       " '" + additionaldiscountAmount + "'," +
                       " '" + addonCharges + "'," +
                       "'" +  lstotalamount_l+ "'," +
                       "'" + lsgrandtotal_l + "'," +
                       "'" + lsadditionaldiscountAmount_l + "'," +
                       "'" + lsaddoncharges_l + "'," +
                       "'" + values.remarks + "'," +
                       "'" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "\\\'") + "', " +
                       "'" + values.currency_code + "'," +
                       "'" + values.exchange_rate + "'," +
                       "'" + values.branch_gid + "', " +
                       "'" + roundoff + "'," +
                       "'" + lstaxgid + "'," +
                       "'" + values.tax_name4.Replace("'", "\\\'") + "', " +
                       "'" + lstaxpercentage + "'," +
                       "'" + values.tax_amount4 + "'," +
                       "'" + values.taxsegment_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                       "'" + freightCharges + "'," +
                       "'" + forwardingCharges + "'," +
                       "'" + (String.IsNullOrEmpty(values.delivery_days) ? values.delivery_days : values.delivery_days.Replace("'", "\\\'")) + "'," +
                       "'" + values.payment + "'," +
                       "'" + (String.IsNullOrEmpty(values.delivery_days) ? values.delivery_days : values.delivery_days.Replace("'", "\\\'")) + "'," +
                       "'" + insuranceCharges + "'," +
                       "'" + lssalesorder_gid + "'," +
                       "'" + (String.IsNullOrEmpty(values.shipping_to) ? values.shipping_to : values.shipping_to.Replace("'", "\\\'")) + "'," +
                       "'" + values.renewal_gid + "'," +
                        "'" + values.sales_type + "'," +
                       "'" + values.bill_email + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = "select finance_flag from adm_mst_tcompany ";
                string finance_flag = objdbconn.GetExecuteScalar(msSQL);
                if (finance_flag == "Y")
                {
                    msSQL = "SELECT account_gid FROM crm_mst_tcustomer WHERE customer_gid='" + values.customer_gid + "'";
                    objodbcDataReader = objdbconn.GetDataReader(msSQL);

                    if (objodbcDataReader.HasRows)
                    {
                        while (objodbcDataReader.Read())
                        {
                            string lsaccount_gid = objodbcDataReader["account_gid"]?.ToString(); // Safely get the value

                            // Check if lsaccount_gid is null or empty
                            if (string.IsNullOrEmpty(lsaccount_gid))
                            {
                                objfincmn.finance_vendor_debitor("Sales", lscustomer_id, lsCustomername.Replace("'", "\\\'"), values.customer_gid, user_gid);
                                string trace_comment = "Added a customer on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                objcmnfunctions.Tracelog(msGetGid, user_gid, trace_comment, "added_customer");
                            }
                        }
                    }
                    objodbcDataReader.Close();
                }
                if (!string.IsNullOrEmpty(lssalesorder_gid))
                {
                      mssalesorderGID = objcmnfunctions.GetMasterGID("VSOP");

                    msSQL = " insert  into smr_trn_tsalesorder (" +
                             " salesorder_gid ," +
                             " branch_gid ," +
                             " salesorder_date," +
                             " customer_gid," +
                             " customer_name," +
                             " customer_contact_gid," +
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
                             " total_price," +
                             " total_amount," +
                             " tax_amount," +
                             " roundoff, " +
                             " updated_addon_charge, " +
                             " updated_additional_discount, " +
                             " freight_charges," +
                             " buyback_charges," +
                             " packing_charges," +
                             " insurance_charges, " +
                             " renewal_flag ," +
                             "created_date" +
                             " )values(" +
                             " '" + mssalesorderGID + "'," +
                             " '" + values.branch_gid  + "'," +
                             " '" + mysqlinvoiceDate + "'," +
                             " '" + values.customer_gid + "'," +
                             " '" + (String.IsNullOrEmpty(lsCustomername) ? lsCustomername : lsCustomername.Replace("'","\\\'")) + "'," +
                             " '" + lscustomercontactgid + "'," +
                             " '" + (String.IsNullOrEmpty(lscustomercontactname) ? lscustomercontactname : lscustomercontactname.Replace("'", "\\\'")) + "'," +
                             " '" + (String.IsNullOrEmpty(values.customer_address) ? values.customer_address : values.customer_address.Replace("'", "\\\'")) + "'," +
                             " '" + lscustomeremail + "'," +
                             " '" + lscustomermobile + "'," +
                             " '" + employee_gid + "'," +
                             " '" + lsrefno + "'," +
                             " '" + (String.IsNullOrEmpty(values.remarks) ? values.remarks : values.remarks.Replace("'", "\\\'")) + "'," +
                             " '" + (String.IsNullOrEmpty(values.payment_days) ? values.payment_days : values.payment_days.Replace("'", "\\\'")) + "'," +
                             " '" + (String.IsNullOrEmpty(values.delivery_days) ? values.delivery_days : values.delivery_days.Replace("'", "\\\'")) + "'," +
                             " '" + values.grandtotal.Replace(",", "").Trim() + "'," +
                             " '" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "") + "'," +
                             " 'Approved'," +
                             "'"+ addonCharges + "'," +
                             "'"+ additionaldiscountAmount + "'," +
                             "'"+ addonCharges + "'," +
                             "'" + additionaldiscountAmount + "'," +
                             " '" + values.grandtotal.Replace(",", "").Trim() + "'," +
                             " '" + values.currency_code + "'," +
                             " '" + currencygid + "'," +
                             " '" + values.exchange_rate + "'," +
                             " '" + values.shipping_to.Replace("'","\\\'") + "'," +
                           "'" + (String.IsNullOrEmpty(values.delivery_days) ? values.delivery_days : values.delivery_days.Replace("'", "\\\'")) + "'," +
                           "'" + (String.IsNullOrEmpty(values.payment_days) ? values.payment_days : values.payment_days.Replace("'", "\\\'")) + "'," +
                            "'" + lstaxgid + "'," +
                             " '" + values.tax_name4 + "'," +
                              "'" + values.grandtotal + "'," +
                             "'" + values.grandtotal+"'," +
                          "'" + values.tax_amount4 + "'," +                   
                             " '" + roundoff + "'," +                   
                             " '" + addonCharges + "'," +
                            "'" + additionaldiscountAmount + "'," +                    
                            "'" + freightCharges + "'," +                                        
                            "'" + buybackCharges + "'," +                                                            
                            "'" + forwardingCharges + "'," +                                                                               
                            "'" + insuranceCharges + "'," +                                                                               
                           " 'Y'," +
                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }

                    if (mnResult == 1)
                {
                    msSQL = "SELECT salesorder_gid,agreement_gid FROM crm_trn_trenewal WHERE renewal_gid = '" + values.renewal_gid + "'";
                    objodbcDataReader = objdbconn.GetDataReader(msSQL);
                   
                    if (objodbcDataReader.HasRows)
                    {
                        lssalesorder_gid = objodbcDataReader["salesorder_gid"].ToString();
                       

                    }
                    if (!string.IsNullOrEmpty(lssalesorder_gid))
                    {
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
                               " 'Y'," +
                                "'" + values.frequency_terms + "'," +
                                "'" + values.customer_gid + "'," +
                                "'" + renewalDate + "'," +
                                "'" + mssalesorderGID + "'," +
                                "'" + employee_gid + "'," +
                                "'sales'," +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update crm_trn_trenewal set " +
                                  " renewal_status = 'closed' " +
                                  " where salesorder_gid = '" + lssalesorder_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        msSQL = "SELECT agreement_gid FROM crm_trn_tagreement WHERE renewal_gid = '" + values.renewal_gid + "'";
                        string lsagreement_gid = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " update crm_trn_trenewal set " +
                                "renewal_date = '" + renewalDate + "' " +
                                " where agreement_gid = '" + lsagreement_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                  }
                    
                   if (mnResult == 1)
                {
                    msSQL = "update cst_trn_tsalesorder set invoice_gid='" + msINGetGID + "' where salesorder_gid='" + lssalesorder_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "update smr_trn_tsalesorder set salesorder_status='Invoice Raised' where salesorder_gid='" + lssalesorder_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    msSQL = " update smr_trn_tsalesorder set so_type='" + order_type + "' where salesorder_gid='" + lssalesorder_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update acp_trn_torder set so_type='" + order_type + "' where salesorder_gid='" + lssalesorder_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    
                    if (finance_flag == "Y")
                    {

                        double roundoff_l = roundoff * Convert.ToDouble(values.exchange_rate);
                        string lsproduct_price_l = "", lstax1_gid = "", lstax2_gid = "";
                        msSQL = " select sum(product_price_L) as product_price_L,sum(tax_amount1_L) as tax1,sum(tax_amount2_L) as tax2,tax1_gid,tax2_gid from rbl_trn_tinvoicedtl " +
                             " where invoice_gid='" + msINGetGID + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            lsproduct_price_l = objOdbcDataReader["product_price_L"].ToString();
                            lstax1 = objOdbcDataReader["tax1"].ToString();
                            lstax2 = objOdbcDataReader["tax2"].ToString();
                            lstax1_gid = objOdbcDataReader["tax1_gid"].ToString();
                            lstax2_gid = objOdbcDataReader["tax2_gid"].ToString();
                        }
                        objOdbcDataReader.Close();
                        lsbasic_amount = Math.Round(double.Parse(lsproduct_price_l), 2);
                        objfincmn.jn_invoice(mysqlinvoiceDate, values.remarks, values.branch_gid, ls_referenceno, msINGetGID
                         , lsbasic_amount, addonCharges_l, additionaldiscountAmount_l, lsgrandtotal_l, values.customer_gid, "Invoice", "RBL",
                         values.sales_type, roundoff_l, freightCharges_l, buybackCharges_l, forwardingCharges_l, insuranceCharges_l, values.tax_amount4, values.tax_name4);






                        if (lstax1 != "0.00" && lstax1 != " "&& lstax1!=null)
                        {
                            decimal lstaxsum = decimal.Parse(lstax1);
                            string lstaxamount = lstaxsum.ToString("F2");
                            double tax_amount = Math.Round(double.Parse(lstaxamount), 2);

                            objfincmn.jn_sales_tax(msINGetGID, ls_referenceno, values.remarks, tax_amount, lstax1_gid);
                        }
                        if (lstax2 != "0.00" && lstax2 != "" && lstax1 != null&& lstax2 != "0")
                        {
                            decimal lstaxsum = decimal.Parse(lstax2);
                            string lstaxamount = lstaxsum.ToString("F2");
                            double tax_amount = Math.Round(double.Parse(lstaxamount), 2);

                            objfincmn.jn_sales_tax(msINGetGID, ls_referenceno, values.remarks, tax_amount, lstax2_gid);
                        }




                    }


                    msSQL = " delete from rbl_tmp_tinvoicedtl where employee_gid='" + employee_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    values.status = true;
                    values.message = "Invoice Raised Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while submitting the Invoice";
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while submitting Invoice!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public string GetRandomString(int length)
        {
            string guidResult = Guid.NewGuid().ToString();
            guidResult = guidResult.Replace("-", string.Empty);
            guidResult = guidResult.ToUpper();

            if (length <= 0 || length > guidResult.Length)
            {
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);
            }

            return guidResult.Substring(0, length);
        }
        public void DaProductAddRenewalEdit(string employee_gid,PosttempProduct_list values)
        {
            try
            {
                msSQL = "select salesorder_gid FROM crm_trn_trenewal WHERE renewal_gid = '" + values.renewal_gid + "'";
                objodbcDataReader = objdbconn.GetDataReader(msSQL);
                string lssalesorder_gid = null;
                if (objodbcDataReader.HasRows)
                {
                    lssalesorder_gid = objodbcDataReader["salesorder_gid"].ToString();
                }
               
                if (string.IsNullOrEmpty(lssalesorder_gid))
                {
                    msGetGid = objcmnfunctions.GetMasterGID("VSDT");
                    msSQL = " SELECT a.productuom_gid, a.product_gid, a.product_name, b.productuom_name FROM pmr_mst_tproduct a " +
                         " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                         " WHERE product_gid = '" + values.product_name + "'";
                    objodbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objodbcDataReader.HasRows == true)
                    {
                        lsproductgid = objodbcDataReader["product_gid"].ToString();
                        lsproductuom_gid = objodbcDataReader["productuom_gid"].ToString();
                        lsproduct_name = objodbcDataReader["product_name"].ToString();
                        lsproductuom_name = objodbcDataReader["productuom_name"].ToString();
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

                    msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                             " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                              " WHERE product_gid='" + values.product_name + "'";
                    objodbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objodbcDataReader.HasRows == true)
                    {
                        lsorder_type = "Sales";
                    }
                    else
                    {
                        lsorder_type = "Services";
                    }
                    msSQL = " insert into smr_tmp_tsalesorderdtl( " +
                                  " tmpsalesorderdtl_gid," +
                                  " employee_gid," +
                                  " product_gid," +
                                  " product_code," +
                                   " customerproduct_code," +
                                  " product_name," +
                                  " productgroup_gid," +
                                  " product_price," +
                                  " qty_quoted," +
                                  " uom_gid," +
                                  " uom_name," +
                                  " price," +
                                  " order_type," +
                                  " tax_rate, " +
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
                                  "'" + employee_gid + "'," +
                                  "'" + lsproductgid + "'," +
                                  "'" + values.product_code + "'," +
                                   "'" + values.product_code + "'," +
                                  "'" + lsproduct_name.Replace("'","\\\'") + "'," +
                                  "'" + values.productgroup_name + "'," +
                                  "'" + values.unitprice + "'," +
                                  "'" + values.productquantity + "'," +
                                  "'" + lsproductuom_gid + "'," +
                                  "'" + lsproductuom_name + "'," +
                                  "'" + values.producttotal_amount + "'," +
                                  " '" + lsorder_type + "', " +
                                  " '" + values.tax_prefix + "', " +
                                  " '" + values.taxsegment_gid + "', " +
                                  " '" + values.taxsegment_gid + "', " +
                                  " '" + values.taxgid1 + "', " +
                                  " '" + values.taxgid2 + "', " +
                                  " '" + values.taxgid3 + "', " +
                                  " '" + values.tax_prefix + "', " +
                                  " '" + values.tax_prefix2 + "', " +
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
                       "'" + values.discount_amount + "'," +
                       "'" + (String.IsNullOrEmpty(values.product_remarks) ? values.product_remarks : values.product_remarks.Replace("'","\\\'")) + "'," +
                       "'" + lsdiscountpercentage + "')";
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
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("VSDT");
                    msSQL = " SELECT a.productuom_gid, a.product_gid, a.product_name, b.productuom_name FROM pmr_mst_tproduct a " +
                         " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                         " WHERE product_gid = '" + values.product_name + "'";
                    objodbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objodbcDataReader.HasRows == true)
                    {
                        lsproductgid = objodbcDataReader["product_gid"].ToString();
                        lsproductuom_gid = objodbcDataReader["productuom_gid"].ToString();
                        lsproduct_name = objodbcDataReader["product_name"].ToString();
                        lsproductuom_name = objodbcDataReader["productuom_name"].ToString();
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

                    msSQL = " insert into smr_tmp_tsalesorderdtl( " +
                                  " tmpsalesorderdtl_gid," +
                                  " employee_gid," +
                                  " product_gid," +
                                  " product_code," +
                                   " customerproduct_code," +
                                  " product_name," +
                                  " productgroup_gid," +
                                  " product_price," +
                                  " qty_quoted," +
                                  " uom_gid," +
                                  " uom_name," +
                                  " price," +
                                  " order_type," +
                                  " tax_rate, " +
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
                                  "'" + employee_gid + "'," +
                                  "'" + lsproductgid + "'," +
                                  "'" + values.product_code + "'," +
                                   "'" + values.product_code + "'," +
                                  "'" + lsproduct_name + "'," +
                                  "'" + values.productgroup_name + "'," +
                                  "'" + values.unitprice + "'," +
                                  "'" + values.productquantity + "'," +
                                  "'" + lsproductuom_gid + "'," +
                                  "'" + lsproductuom_name + "'," +
                                  "'" + values.producttotal_amount + "'," +
                                  " 'Agreement', " +
                                  " '" + values.tax_prefix + "', " +
                                  " '" + values.taxsegment_gid + "', " +
                                  " '" + values.taxsegment_gid + "', " +
                                  " '" + values.taxgid1 + "', " +
                                  " '" + values.taxgid2 + "', " +
                                  " '" + values.taxgid3 + "', " +
                                  " '" + values.tax_prefix + "', " +
                                  " '" + values.tax_prefix2 + "', " +
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
                       "'" + values.discount_amount + "'," +
                       "'" + values.product_remarks + "'," +
                       "'" + lsdiscountpercentage + "')";
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
                values.message = "Exception occurred while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGettmpProductSummaryRenewalEdit(string employee_gid, MdlSmrTrnRenewalsummary values)
        {
            try
            {
                double grand_total = 0.00;
                double grandtotal = 0.00;
                msSQL = " select a.tmpsalesorderdtl_gid, a.product_gid, a.productgroup_gid,a.product_price, a.qty_quoted,a.discount_amount," +
                    "a.discount_percentage , b.product_name, c.productgroup_name, a.tax_percentage,a.tax_amount,a.uom_gid, " +
                    "a.product_remarks,a.uom_name,a.price,a.tax_name,a.tax_name2,a.tax_percentage2 ,a.tax_amount2,a.employee_gid," +
                    "a.tax1_gid,a.tax2_gid,a.product_code,a.tax_rate,a.taxsegment_gid,a.taxsegmenttax_gid," +
                    " format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount" +
                    " from smr_tmp_tsalesorderdtl a \r\n left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                    " left join pmr_mst_tproductgroup c on a.productgroup_gid=c.productgroup_gid\r\n " +
                    "where employee_gid='"+ employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ProductSummaryRenewal_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["price"].ToString());
                        grandtotal += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new ProductSummaryRenewal_list
                        {
                            product_code = dt["product_code"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            uom_name = dt["uom_name"].ToString(),
                            tax_prefix = dt["tax_name"].ToString(),
                            tax_prefix2 = dt["tax_name2"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            price = dt["price"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_amount = dt["taxamount"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                        });
                    }

                    values.ProductSummaryRenewal_list = getModuleList;
                    values.grand_total= grand_total;
                    values.grandtotal = grandtotal;
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetProductSummaryRenewalEdit(string renewal_gid, string employee_gid ,MdlSmrTrnRenewalsummary values )
        {
            try
            {
                msSQL = "delete from smr_tmp_tsalesorderdtl where employee_gid='" + employee_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                

                // Fetch salesorder_gid based on renewal_gid
                msSQL = "SELECT salesorder_gid FROM crm_trn_trenewal WHERE renewal_gid = '" + renewal_gid + "'";
                objodbcDataReader = objdbconn.GetDataReader(msSQL);
                string lssalesorder_gid = null;

                if (objodbcDataReader.HasRows)
                {
                    lssalesorder_gid = objodbcDataReader["salesorder_gid"].ToString();
                }
                objodbcDataReader.Close();


                msSQL = "Select agreement_gid from crm_trn_tagreement where renewal_gid = '" + renewal_gid + "'";
                string agreement_gid = objdbconn.GetExecuteScalar(msSQL);

                if (!string.IsNullOrEmpty(lssalesorder_gid))
                {
                    msSQL = "SELECT d.salesorderdtl_gid,d.product_gid, i.productgroup_name, d.product_remarks, d.product_name, d.salesorderdtl_gid, " +
                            "d.product_code, d.uom_name,d.uom_gid, d.qty_quoted, d.margin_amount, d.margin_percentage, d.discount_percentage, " +
                            "d.discount_amount, FORMAT(d.product_price, 2) AS product_price, d.tax_name, format(d.tax_amount, 2) AS tax_amount, " +
                            "FORMAT(d.price, 2) AS price,i.productgroup_gid,d.taxsegment_gid, d.taxsegmenttax_gid," +
                            " e.tax_prefix AS tax_prefix1, l.tax_prefix AS tax_prefix2 ,d.tax1_gid,d.tax2_gid,d.tax3_gid," +
                            "d.tax_percentage2,d.tax_percentage3,d.tax_percentage,d.tax_amount2,d.tax_amount3" +
                            " FROM smr_trn_tsalesorder a " +
                            " LEFT JOIN smr_trn_tsalesorderdtl d ON d.salesorder_gid = a.salesorder_gid " +
                            " LEFT JOIN acp_mst_ttax e ON e.tax_gid = d.tax1_gid " +
                            " LEFT JOIN acp_mst_ttax l ON l.tax_gid = d.tax2_gid " +
                            " LEFT JOIN pmr_mst_tproductgroup i ON i.productgroup_gid = d.productgroup_gid " +
                            " LEFT JOIN crm_trn_trenewal j ON j.salesorder_gid = a.salesorder_gid " +
                            " WHERE j.renewal_gid = '" + renewal_gid + "' group by d.salesorderdtl_gid";
                }
                else
                {
                    msSQL = "SELECT d.agreementdtl_gid as salesorderdtl_gid,d.product_gid, i.productgroup_name, d.product_remarks, d.product_name, " +
                            "d.product_code, d.uom_name, d.qty_quoted, d.discount_percentage, d.discount_amount, " +
                            "FORMAT(d.product_price, 2) AS product_price,d.uom_gid,d.uom_name, d.tax_name, format(d.tax_amount, 2) AS tax_amount, " +
                            "FORMAT(d.price, 2) AS price,i.productgroup_gid, e.tax_prefix AS tax_prefix1," +
                            "d.taxsegment_gid,d.taxsegmenttax_gid, l.tax_prefix AS tax_prefix2,d.tax1_gid,d.tax2_gid,d.tax3_gid," +
                            "d.tax_percentage2,d.tax_percentage3,d.tax_percentage,d.tax_amount2,d.tax_amount3 " +
                            " FROM crm_trn_tagreement a " +
                            "LEFT JOIN crm_trn_tagreementdtl d ON d.agreement_gid = a.agreement_gid " +
                            "LEFT JOIN acp_mst_ttax e ON e.tax_gid = d.tax1_gid " +
                            "LEFT JOIN acp_mst_ttax l ON l.tax_gid = d.tax2_gid " +
                            "LEFT JOIN pmr_mst_tproductgroup i ON i.productgroup_gid = d.productgroup_gid " +
                            "LEFT JOIN crm_trn_trenewal j ON j.agreement_gid = a.agreement_gid " +
                            "WHERE d.agreement_gid = '" + agreement_gid + "' group by d.agreementdtl_gid ";
                }
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("VSDT");

                        msSQL = " insert into smr_tmp_tsalesorderdtl( " +
                              " tmpsalesorderdtl_gid,employee_gid, product_gid, product_code," +
                              " product_name, productgroup_gid, product_price, qty_quoted,uom_gid,uom_name, price," +
                              " taxsegment_gid,taxsegmenttax_gid,tax1_gid,tax2_gid, tax3_gid, " +
                             " tax_name,  tax_name2,  tax_name3, tax_percentage,  tax_percentage2,  tax_percentage3, " +
                             " tax_amount, tax_amount2, tax_amount3, discount_amount,product_remarks, discount_percentage" +
                              ")values(" +
                              "'" + msGetGid + "'," +
                              "'" + employee_gid + "'," +
                              "'" + dt["product_gid"] + "'," +
                              "'" + dt["product_code"].ToString().Replace("'", "\\\'") + "'," +
                              "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                              "'" + dt["productgroup_gid"] + "'," +
                              "'" + dt["product_price"].ToString().Replace(",", "") + "'," +
                              "'" + dt["qty_quoted"].ToString().Replace(",", "") + "'," +
                              "'" + dt["uom_gid"].ToString() + "'," +
                              "'" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                              "'" + dt["price"].ToString().Replace(",", "") + "'," +
                              "'" + dt["taxsegment_gid"].ToString() + "'," +
                              "'" + dt["taxsegmenttax_gid"].ToString() + "'," +
                              "'" + dt["tax1_gid"].ToString() + "'," +
                              "'" + dt["tax2_gid"].ToString() + "'," +
                              "'" + dt["tax3_gid"].ToString() + "'," +
                              "'" + dt["tax_prefix1"].ToString() + "'," +
                              "'" + dt["tax_prefix2"].ToString() + "'," +
                              "'" + dt["tax_prefix1"].ToString() + "'," +
                              "'" + dt["tax_percentage"].ToString() + "'," +
                              "'" + dt["tax_percentage2"].ToString() + "'," +
                              "'" + dt["tax_percentage3"].ToString() + "'," +
                              "'" + dt["tax_amount"].ToString().Replace(",","") + "'," +
                              "'" + dt["tax_amount2"].ToString().Replace(",", "") + "'," +
                              "'" + dt["tax_amount3"].ToString().Replace(",", "") + "'," +
                              "'" + dt["discount_amount"].ToString().Replace(",", "") + "'," +
                              "'" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +
                              "'" + dt["discount_percentage"].ToString() + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while getting PO summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostRenewalEdit(string employee_gid, PostRenewalEdit_list values)
        {
            msSQL = "select salesorder_gid FROM crm_trn_trenewal WHERE renewal_gid = '" + values.renewal_gid + "'";
            objodbcDataReader = objdbconn.GetDataReader(msSQL);
            string lssalesorder_gid = null;
            if (objodbcDataReader.HasRows)
            {
                lssalesorder_gid = objodbcDataReader["salesorder_gid"].ToString();
            }

            string inputDate = values.agreement_date;
            DateTime uiDate = DateTime.ParseExact(inputDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string agreement_date = uiDate.ToString("yyyy-MM-dd");


            string inputdate2 = values.renewal_date;
            DateTime renedate = DateTime.ParseExact(inputdate2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string Renewal_Date = renedate.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(lssalesorder_gid))
            {

                msSQL = "update crm_trn_tagreement set" +
                    " branch_gid = '" + values.branch_gid + "',agreement_date='" + agreement_date + "', customer_gid='" + values.customer_gid + "', roundoff='" + values.roundoff + "', freight_charges='" + values.freight_charges + "'," +
                    " customer_address = '" + (String.IsNullOrEmpty(values.address1) ? values.address1 : values.address1.Replace("'","\\\'")) + "',agreement_referencenumber='" + values.agreement_referencenumber + "', total_amount='" + values.grandtotal + "'," +
                    " payment_days = '" + (String.IsNullOrEmpty(values.payment_days) ? values.payment_days : values.payment_days.Replace("'", "\\\'")) + "',delivery_days='" + (String.IsNullOrEmpty(values.delivery_days) ? values.delivery_days : values.delivery_days.Replace("'", "\\\'")) + "'," +
                    " termsandconditions='" + (String.IsNullOrEmpty(values.termsandconditions) ? values.termsandconditions : values.termsandconditions.Replace("'", "\\\'")) + "', Grandtotal='" + values.grandtotal + "'," +
                    " customer_name='" + (String.IsNullOrEmpty(values.customer_name) ? values.customer_name : values.customer_name.Replace("'", "\\\'")) + "',addon_charge='" + values.addon_charge + "',additional_discount='" + values.additional_discount + "'," +
                    " currency_code='" + values.currency_code + "', exchange_rate='" + values.exchange_rate + "'," +
                    " currency_gid='" + values.currencyexchange_gid + "',renewal_date='" + Renewal_Date + "',renewal_gid='" + values.renewal_gid + "'," +
                    " shipping_to='" + (String.IsNullOrEmpty(values.shipping_address) ? values.shipping_address : values.shipping_address.Replace("'", "\\\'")) + "', tax_amount='" + values.tax_amount4 + "'," +
                    " total_price='" + values.totalamount + "', tax_name='" + (String.IsNullOrEmpty(values.tax_name4) ? values.tax_name4 : values.tax_name4.Replace("'", "\\\'")) + "', tax_gid='" + values.tax_gid + "'," +
                    " bill_to='" + (String.IsNullOrEmpty(values.address1) ? values.address1 : values.address1.Replace("'", "\\\'")) + "' where agreement_gid='" + values.salesorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " update crm_trn_trenewal set renewal_date='" + Renewal_Date + "',customer_gid='" + values.customer_gid + "'," +
                    " agreement_gid='" + values.salesorder_gid + "' where  renewal_gid='" + values.renewal_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select a.tmpsalesorderdtl_gid, a.product_gid, a.productgroup_gid,a.product_price, a.qty_quoted,a.discount_amount," +
                  "a.discount_percentage , b.product_name, c.productgroup_name, a.tax_percentage,a.tax_amount,a.uom_gid, " +
                  "a.product_remarks,a.uom_name,a.price,a.tax_name,a.tax_name2,a.tax_percentage2 ,a.tax_amount2,a.employee_gid," +
                  "a.tax1_gid,a.tax2_gid,a.product_code,a.tax_rate,a.taxsegment_gid,a.taxsegmenttax_gid," +
                  " format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount" +
                  " from smr_tmp_tsalesorderdtl a \r\n left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                  " left join pmr_mst_tproductgroup c on a.productgroup_gid=c.productgroup_gid\r\n " +
                  "where employee_gid='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    msSQL = " delete from crm_trn_tagreementdtl where agreement_gid='" + values.salesorder_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        msagreementdtlGID = objcmnfunctions.GetMasterGID("VSDC");

                        msSQL = " insert into crm_trn_tagreementdtl (" +
                            " agreementdtl_gid , agreement_gid, product_gid , productgroup_gid, product_name," +
                            " product_code, display_field, product_remarks,product_price,qty_quoted, discount_percentage," +
                            " discount_amount, tax_amount ,uom_gid,uom_name,price, tax_name, tax1_gid, tax_name2, tax2_gid," +
                            " tax_percentage2, tax_amount2, tax_percentage, taxsegment_gid, taxsegmenttax_gid," +
                            " type " +
                            ")values(" +
                            " '" + msagreementdtlGID + "'," +
                            " '" + values.salesorder_gid + "'," +
                            " '" + dt["product_gid"].ToString() + "'," +
                            " '" + dt["productgroup_gid"].ToString() + "'," +
                            " '" + dt["product_name"].ToString().Replace("'","\\\'") + "'," +
                            " '" + dt["product_code"].ToString().Replace("'", "\\\'") + "'," +
                            " '" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +
                            " '" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +
                            " '" + dt["product_price"].ToString() + "'," +
                            " '" + dt["qty_quoted"].ToString() + "'," +
                            " '" + dt["discount_percentage"].ToString() + "'," +
                            " '" + dt["discount_amount"].ToString() + "'," +
                            " '" + dt["tax_amount"].ToString() + "'," +
                            " '" + dt["uom_gid"].ToString() + "'," +
                            " '" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                            " '" + dt["price"].ToString() + "'," +
                            " '" + dt["tax_name"].ToString() + "'," +
                            " '" + dt["tax1_gid"].ToString() + "'," +
                            " '" + dt["tax_name2"].ToString() + "'," +
                            " '" + dt["tax2_gid"].ToString() + "'," +
                            " '" + dt["tax_percentage2"].ToString() + "'," +
                            " '" + dt["tax_amount2"].ToString() + "'," +
                            " '" + dt["tax_percentage"].ToString() + "'," +
                            " '" + dt["taxsegment_gid"].ToString() + "'," +
                            " '" + dt["taxsegmenttax_gid"].ToString() + "'," +
                            " 'Agreement')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Renewal Updated successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error while updating renewal";
                        }
                    }
                }
            }
            else
            {
                string inputDate1 = values.agreement_date;
                DateTime uiDate1 = DateTime.ParseExact(inputDate1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string salesorder_date = uiDate1.ToString("yyyy-MM-dd");

                msSQL = " select customer_name from crm_mst_tcustomer where customer_gid='" + values.customer_gid + " '";
                string lscustomername = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select branch_gid from hrm_mst_Tbranch where branch_name='" + values.branch_name.Replace("'", "\\\'") + " '";
                string lsbranchgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select currencyexchange_gid from crm_trn_tcurrencyexchange where currency_code='" + values.currency_code + " '";
                string currency_code = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select tax_prefix from acp_mst_Ttax where tax_prefix='" + values.tax_gid + " '";
                string lstax_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select user_name from adm_mst_Tuser  where user_gid='" + values.salesperson_gid + " '";
                string lssalesperson_name = objdbconn.GetExecuteScalar(msSQL);

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



                msSQL = " update smr_trn_tsalesorder  set " +
                                "  salesorder_gid='" + lssalesorder_gid + "'," +
                                 " branch_gid='" + lsbranchgid + "'," +
                                " salesorder_date='" + salesorder_date + "'," +
                                " customer_gid='" + values.customer_gid + "'," +
                                " customer_name='" + lscustomername.Replace("'", "\\\'") + "'," +
                                " customer_address ='" + values.address1.Replace("'", "\\\'") + "'," +
                                " created_by='" + employee_gid + "'," +
                                " so_referenceno1='" + values.agreement_referencenumber + "' ," +
                                " so_remarks='" + (String.IsNullOrEmpty(values.agreement_remarks) ? values.agreement_remarks : values.agreement_remarks.Replace("'","\\\'")) + "'," +
                                " payment_days='" + (String.IsNullOrEmpty(values.payment_days) ? values.payment_days : values.payment_days.Replace("'", "\\\'")) + "', " +
                                " delivery_days='" + (String.IsNullOrEmpty(values.delivery_days) ? values.delivery_days : values.delivery_days.Replace("'", "\\\'")) + "', " +
                                " Grandtotal='" + values.grandtotal.Replace(",", "").Trim() + "', " +
                                " termsandconditions='" + (String.IsNullOrEmpty(values.termsandconditions) ? values.termsandconditions : values.termsandconditions.Replace("'", "\\\'")) + "', " +
                                " addon_charge='" + values.addon_charge.Trim() + "', " +
                                " additional_discount='" + values.additional_discount + "', " +
                                " addon_charge_l='" + values.addon_charge.Trim() + "', " +
                                " additional_discount_l='" + values.additional_discount + "', " +
                                " grandtotal_l='" + values.grandtotal.Replace(",", "").Trim() + "', " +
                                " currency_code='" + values.currency_code + "', " +
                                " currency_gid='" + currency_code + "', " +
                                " exchange_rate='" + values.exchange_rate + "', " +
                                " shipping_to='" + values.shipping_address.Replace("'", "\\\'") + "', " +
                                " tax_gid='" + values.tax_gid + "'," +
                                " tax_name ='" + (String.IsNullOrEmpty(lstax_name) ? lstax_name : lstax_name.Replace("'", "\\\'")) + "'," +
                                " gst_amount='" + lsgst + "'," +
                                " freight_charges='" + values.freight_charges + "'," +
                                " total_price='" + values.totalamount.Replace(",", "").Trim() + "'," +
                                " total_amount='" + values.grandtotal.Replace(",", "").Trim() + "',";
                if (values.tax_amount4 == null || values.tax_amount4 == "")
                {
                    msSQL += " tax_amount='0',";
                }
                else
                {
                    msSQL += " tax_amount='" + values.tax_amount4.Replace(",", "").Trim() + "',";
                }
                msSQL += " salesperson_gid='" + values.user_name + "'," +
                         " roundoff='" + values.roundoff + "', " +
                         " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                          " where salesorder_gid='" + values.salesorder_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1) {
                    msSQL = " update acp_trn_Torder  set " +
                                "  salesorder_gid='" + values.salesorder_gid + "'," +
                                " branch_gid='" + lsbranchgid + "'," +
                                " salesorder_date='" + salesorder_date + "'," +
                                " customer_gid='" + values.customer_gid + "'," +
                                " customer_name='" + lscustomername.Replace("'", "\\\'") + "'," +
                                " customer_address ='" + values.address1.Replace("'", "\\\'") + "'," +
                                " created_by='" + employee_gid + "'," +
                                " so_referenceno1='" + values.so_referencenumber + "' ," +
                                " so_remarks='" + (String.IsNullOrEmpty(values.so_remarks) ? values.so_remarks : values.so_remarks.Replace("'", "\\\'")) + "'," +
                                " payment_days='" + (String.IsNullOrEmpty(values.payment_days) ? values.payment_days : values.payment_days.Replace("'", "\\\'")) + "', " +
                                " delivery_days='" + (String.IsNullOrEmpty(values.delivery_days) ? values.delivery_days : values.delivery_days.Replace("'", "\\\'")) + "', " +
                                " Grandtotal='" + values.grandtotal.Replace(",", "").Trim() + "', " +
                                " termsandconditions='" + (String.IsNullOrEmpty(values.termsandconditions) ? values.termsandconditions : values.termsandconditions.Replace("'", "\\\'")) + "', " +
                                " addon_charge='" + values.addon_charge + "', " +
                                " additional_discount='" + values.additional_discount + "', " +
                                " addon_charge_l='" + values.addon_charge + "', " +
                                " additional_discount_l='" + values.additional_discount + "', " +
                                " grandtotal_l='" + values.grandtotal.Replace(",", "").Trim() + "', " +
                                " currency_code='" + values.currency_code + "', " +
                                " currency_gid='" + currency_code + "', " +
                                " exchange_rate='" + values.exchange_rate + "', " +
                                " shipping_to='" + values.shipping_address.Replace("'", "\\\'") + "', " +
                                " total_amount='" + values.grandtotal.Replace(",", "").Trim() + "'," +
                                " salesperson_gid='" + values.user_name + "'," +
                                " roundoff='" + values.roundoff + "', " +
                                " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                 " where salesorder_gid='" + values.salesorder_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1) {
                        msSQL = " update crm_trn_trenewal  set " +
                                 " customer_gid='" + values.customer_gid + "'," +
                                 " created_by='" + employee_gid + "'," +
                                 " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                 " where salesorder_gid='" + values.salesorder_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " select a.tmpsalesorderdtl_gid, a.product_gid, a.productgroup_gid,a.product_price, a.qty_quoted,a.discount_amount," +
                              "a.discount_percentage , b.product_name, c.productgroup_name, a.tax_percentage,a.tax_amount,a.uom_gid, " +
                              "a.product_remarks,a.uom_name,a.price,a.tax_name,a.tax_name2,a.tax_percentage2 ,a.tax_amount2,a.employee_gid," +
                              "a.tax1_gid,a.tax2_gid,a.product_code,a.tax_rate,a.taxsegment_gid,a.taxsegmenttax_gid," +
                              " format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount" +
                              " from smr_tmp_tsalesorderdtl a \r\n left join pmr_mst_tproduct b on a.product_gid=b.product_gid " +
                              " left join pmr_mst_tproductgroup c on a.productgroup_gid=c.productgroup_gid\r\n " +
                              "where employee_gid='" + employee_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count > 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows) 
                            { 
                                int i = 0;
                                mssalesorderGID1 = objcmnfunctions.GetMasterGID("VSDC");
                                msSQL = " insert into smr_trn_tsalesorderdtl (" +
                                     " salesorderdtl_gid , salesorder_gid, product_gid , product_name," +
                                     " product_code, productgroup_gid,product_price, qty_quoted," +
                                     " discount_percentage, discount_amount, tax_amount , tax_amount2 ," +
                                     " uom_gid, uom_name, price, tax_name, tax_name2, tax1_gid," +
                                     " tax2_gid, slno, product_remarks, display_field, tax_percentage, tax_percentage2," +
                                     " taxsegment_gid, taxsegmenttax_gid," +
                                     " type " +
                                     ")values(" +
                                     " '" + mssalesorderGID1 + "'," +
                                     " '" + values.salesorder_gid + "'," +
                                     " '" + dt["product_gid"].ToString() + "'," +
                                     " '" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                                     " '" + dt["product_code"].ToString().Replace("'", "\\\'") + "'," +
                                      " '" + dt["productgroup_gid"].ToString() + "'," +
                                     " '" + dt["product_price"].ToString() + "'," +
                                     " '" + dt["qty_quoted"].ToString() + "'," +
                                     " '" + dt["discount_percentage"].ToString() + "'," +
                                     " '" + dt["discount_amount"].ToString() + "'," +
                                     " '" + dt["tax_amount"].ToString() + "'," +
                                     " '" + dt["tax_amount2"].ToString() + "'," +
                                     " '" + dt["uom_gid"].ToString() + "'," +
                                     " '" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                                     " '" + dt["price"].ToString() + "'," +
                                     " '" + dt["tax_name"].ToString() + "'," +
                                     " '" + dt["tax_name2"].ToString() + "'," +
                                     " '" + dt["tax1_gid"].ToString() + "'," +
                                     " '" + dt["tax2_gid"].ToString() + "'," +
                                     " '" + i + 1 + "'," +
                                      " '" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +
                                       " '" + dt["product_remarks"].ToString().Replace("'", "\\\'") + "'," +
                                     " '" + dt["tax_percentage"].ToString() + "'," +
                                     " '" + dt["tax_percentage2"].ToString() + "'," +
                                     " '" + dt["taxsegment_gid"].ToString() + "'," +
                                     " '" + dt["taxsegmenttax_gid"].ToString() + "'," +
                                     " '')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

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
                                   " '" + values.salesorder_gid + "'," +
                                   " '" + dt["product_gid"].ToString() + "'," +
                                   " '" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                                   " '" + dt["product_price"].ToString() + "'," +
                                   " '" + dt["qty_quoted"].ToString() + "'," +
                                   " '" + dt["discount_percentage"].ToString() + "'," +
                                   " '" + dt["discount_amount"].ToString() + "'," +
                                   " '" + dt["tax_amount"].ToString() + "'," +
                                   " '" + dt["uom_gid"].ToString() + "'," +
                                   " '" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                                   " '" + dt["price"].ToString() + "'," +
                                   " '" + dt["tax_name"].ToString() + "'," +
                                    " '" + dt["tax1_gid"].ToString() + "'," +
                                   " '" + values.slno + "'," +
                                   " '" + dt["tax_percentage"].ToString() + "'," +
                                   " '" + dt["taxsegment_gid"].ToString() + "'," +
                                   " '', " +
                                   " '" + values.salesorder_refno + "')";
                             mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Renewal Updated Successfully.";
                            }
                            else 
                            {
                                values.status = false;
                                values.message = "Error while updating renewal.";
                            }
                        }
                    }

                }
            }
        }
        public void DaGetagreementtoinvoicetag(MdlSmrTrnRenewalsummary values, string renewal_gid)
        {
            try
            {
                msSQL = " select customer_gid from crm_trn_trenewal where renewal_gid ='" + renewal_gid + "'";
                string lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select distinct a.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag,a.mail_status,a.customer_gid,DATE_FORMAT(a.invoice_date, '%d-%m-%Y') AS invoice_date,a.invoice_reference,a.additionalcharges_amount,a.discount_amount," +
                   " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', a.mail_status, " +
                   " a.payment_flag,  format(a.initialinvoice_amount,2) as initialinvoice_amount, a.customer_name, " +
                   " format((a.invoice_amount + a.extraadditional_amount - a.extradiscount_amount),2) as invoice_amount,concat( a.customer_contactperson,' / ', a.customer_contactnumber) as customer_contactperson, " +
                   " a.customer_contactnumber  as mobile,a.invoice_from  from rbl_trn_tinvoice a  " +
                   " left join rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid  left join rbl_trn_tso2invoice f on f.invoice_gid=a.invoice_gid " +
                   " left join smr_trn_tdeliveryorder d on d.directorder_gid = f.directorder_gid  left join crm_mst_tcustomer c on a.customer_gid = c.customer_gid " +
                   " left join crm_mst_tcustomercontact e on e.customer_gid=c.customer_gid  left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                   " where  a.invoice_type<>'Opening Invoice' and a.customer_gid='" + lscustomer_gid + "' and a.renewal_gid is null ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<invoicetagsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new invoicetagsummary_list
                        {
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            contact = dt["customer_contactperson"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            invoice_date = dt["invoice_date"].ToString(),
                            renewal_gid= renewal_gid,


                        });
                        values.invoicetagsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPostMappedinvoicetag(string user_gid, mapinvoice_lists values)
        {
            try
            {
                for (int i = 0; i < values.invoicetagsummary_list.ToArray().Length; i++)
                {
                    msSQL = " select agreement_gid from crm_trn_tagreement where renewal_gid ='" + values.renewal_gid + "'";
                    string lsagreement_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " update rbl_trn_tinvoice Set " +
                            " invoice_from = 'Agreement' ," +
                            " renewal_gid = '" + values.renewal_gid + "'," +
                            " invoice_reference = '" + lsagreement_gid + "'" +
                            " where invoice_gid = '" + values.invoicetagsummary_list[i].invoice_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Invoice Tag Succesfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error while occured Invoice Tag ";
                    }
                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occurred while Mapping product !! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }



    }
}