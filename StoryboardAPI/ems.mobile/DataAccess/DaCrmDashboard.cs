using ems.mobile.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace ems.mobile.DataAccess
{

    public class MblCrmDashboard
    {
        dbconn objdbconn = new dbconn();
        string msSQL = string.Empty;
        DataTable dt_datatable;
        cmnfunctions objcmnfunctions = new cmnfunctions();


        public void DaGetAppointmentHeader(string employee_gid, MdlCrmDashboard values)
        {
            {


                msSQL = "SELECT " +
                        "SUM(CASE WHEN DATE(appointment_date) = CURDATE() THEN 1 ELSE 0 END) AS today_count, " +
                        "SUM(CASE WHEN DATE(appointment_date) = CURDATE() AND appointment_date > NOW() AND Leadstage_gid='1' THEN 1 ELSE 0 END) AS upcoming_count, " +
                        "SUM(CASE WHEN DATE(appointment_date) = CURDATE() AND appointment_date < NOW() THEN 1 ELSE 0 END) AS expired_count, " +
                        "SUM(CASE WHEN DATE(appointment_date) = CURDATE() AND Leadstage_gid > '1' THEN 1 ELSE 0 END) AS completed_today_count " +
                        "FROM crm_trn_tappointment " +
                        "WHERE assign_to = '" + employee_gid + "' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<appointmentvalue_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new appointmentvalue_list
                        {
                            today_count = dt["today_count"].ToString(),
                            upcoming_count = dt["upcoming_count"].ToString(),
                            expired_count = dt["expired_count"].ToString(),
                            completed_today_count = dt["completed_today_count"].ToString()


                        });
                        values.Appointmentvalue_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

        }
        public void DaGetAppointmentSummary(string employee_gid, MdlCrmDashboard values)
        {
             msSQL = "select a.leadbank_gid, a.appointment_gid, a.lead_title, a.internal_notes, a.potential_value, f.business_vertical, b.leadbank_name, " +
                "concat(b.leadbank_name, ' / ', c.leadbankbranch_name, ' / ', c.leadbankcontact_name) as lead_contact, " +
                "concat(c.leadbankbranch_name, ' / ', c.leadbankcontact_name) as Details, concat(c.region_name, ' / ', e.source_name) as region_source, " +
                "date_format(a.appointment_date, '%W %e %M %Y') as appointment_date, " +
                "date_format(a.appointment_date, '%h:%i %p') as appointment_time " +
                "from crm_trn_tappointment a " +
                "left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid " +
                "left join crm_trn_tleadbankcontact c on a.leadbank_gid = c.leadbank_gid " +
                "left join crm_mst_tregion d on b.leadbank_region = d.region_gid " +
                "left join crm_mst_tsource e on b.source_gid = e.source_gid " +
                "left join crm_mst_tbusinessvertical f on a.business_vertical = f.businessvertical_gid " +
                "where curdate() = date(a.appointment_date) " +
                "and a.assign_to = '" + employee_gid + "' " +
                "and a.Leadstage_gid = '1' " +
                "and a.appointment_date > NOW() " +
                "and a.appointment_date = ( " +
                "    select min(appointment_date) " +
                "    from crm_trn_tappointment " +
                "    where curdate() = date(appointment_date) " +
                "    and assign_to = '" + employee_gid + "' " +
                "    and Leadstage_gid = '1' " +
                "    and appointment_date > NOW() " +
                ") " +
                "order by a.appointment_date";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<appointmetsummary_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new appointmetsummary_list
                    {
                        appointment_gid = dt["appointment_gid"].ToString(),
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead_title = dt["lead_title"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        lead_contact = dt["lead_contact"].ToString(),
                        appointment_date = dt["appointment_date"].ToString(),
                        appointment_time = dt["appointment_time"].ToString(),
                        region_source = dt["region_source"].ToString(),
                        business_vertical = dt["business_vertical"].ToString(),

                    });
                    values.Appointmetsummary_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetUpcomingAppointmentSummary(string employee_gid, MdlCrmDashboard values)
        {
            msSQL = "select a.leadbank_gid, a.appointment_gid, a.lead_title, a.internal_notes, a.potential_value, f.business_vertical, b.leadbank_name, " +
                  "concat(b.leadbank_name, ' / ', c.leadbankbranch_name, ' / ', c.leadbankcontact_name) as lead_contact, " +
                  "concat(c.leadbankbranch_name, ' / ', c.leadbankcontact_name) as Details, concat(c.region_name, ' / ', e.source_name) as region_source, " +
                  "date_format(a.appointment_date, '%W %e %M %Y') as appointment_date, " +
                  "date_format(a.appointment_date, '%h:%i %p') as appointment_time " +
                  "from crm_trn_tappointment a " +
                  "left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid " +
                  "left join crm_trn_tleadbankcontact c on a.leadbank_gid = c.leadbank_gid " +
                  "left join crm_mst_tregion d on b.leadbank_region = d.region_gid " +
                  "left join crm_mst_tsource e on b.source_gid = e.source_gid " +
                  "left join crm_mst_tbusinessvertical f on a.business_vertical = f.businessvertical_gid " +
                  "where curdate() = date(a.appointment_date) " +
                  "and a.assign_to = '" + employee_gid + "' " +
                  "and a.Leadstage_gid = '1' " +
                  "and a.appointment_date > NOW() " +
                  "order by a.appointment_date " +
                  "LIMIT 18446744073709551615 OFFSET 1";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<upcomingappointmetsummary_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new upcomingappointmetsummary_list
                    {
                        appointment_gid = dt["appointment_gid"].ToString(),
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead_title = dt["lead_title"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        lead_contact = dt["lead_contact"].ToString(),
                        appointment_date = dt["appointment_date"].ToString(),
                        appointment_time = dt["appointment_time"].ToString(),
                        region_source = dt["region_source"].ToString(),
                        business_vertical = dt["business_vertical"].ToString(),

                    });
                    values.UpcomingAppointmetsummary_list = getModuleList;
                }
            }
            dt_datatable.Dispose();

        }   

        public void DaGet360CardViewDetails(string employee_gid, MdlCrmDashboard values, string appointment_gid, string leadbank_gid)
        {

            msSQL = "select b.leadbank_gid,b.customer_gid,d.leadstage_name,concat(b.leadbank_name, ' / ', c.leadbankbranch_name, ' / ', c.leadbankcontact_name) as lead_contact,concat(h.user_firstname,' ',h.user_lastname,'/',h.user_code)as assign_to,format(a.potential_value,2)as potential_value,b.leadbank_name,a.lead_title, date_format(a.appointment_date, '%e %b %Y') as appointment_date, f.business_vertical, date_format(b.created_date, '%e %b %Y') as created_date, b.customer_type,c.leadbankcontact_name,c.email,c.mobile from crm_trn_tappointment a " +
                   " left join crm_trn_tleadbank b on b.leadbank_gid = a.leadbank_gid " +
                   " left join crm_trn_tleadbankcontact c on c.leadbank_gid = b.leadbank_gid " +
                   " left join crm_mst_tleadstage d on d.leadstage_gid = a.leadstage_gid " +
                   " left join crm_mst_tbusinessvertical f on a.business_vertical = f.business_vertical " +
                   " left join hrm_mst_temployee g on a.assign_to=g.employee_gid " +
                   " left join adm_mst_tuser h on g.user_gid=h.user_gid " +
                   " where a.appointment_gid = '" + appointment_gid + "' AND  a.leadbank_gid = '" + leadbank_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<threesixtycardviewdetails_list > ();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new threesixtycardviewdetails_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        leadstage_name = dt["leadstage_name"].ToString(),
                        potential_value = dt["potential_value"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        lead_title = dt["lead_title"].ToString(),
                        appointment_date = dt["appointment_date"].ToString(),
                        created_date = dt["created_date"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                        email = dt["email"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        business_vertical = dt["business_vertical"].ToString(),
                        assign_to = dt["assign_to"].ToString(),
                        lead_contact = dt["lead_contact"].ToString(),
                    });
                    values.ThreeSixtyData_list = getModuleList;
                }
            }
            dt_datatable.Dispose();

        }
        public void DaGetOverallCountDetails(string employee_gid, MdlCrmDashboard values, string leadbank_gid)
        {

            msSQL = " select customer_gid from crm_trn_tleadbank where leadbank_gid  = '" + leadbank_gid + "'";
            string lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " SELECT id FROM crm_smm_whatsapp WHERE  leadbank_gid =  '" + leadbank_gid + "' ";
            string lscontact_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = "SELECT(SELECT COUNT(*) FROM smr_trn_treceivequotation WHERE customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS totalquotation_count," +
    " (SELECT round(SUM(Grandtotal),2) FROM smr_trn_treceivequotation WHERE customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS totalquotation_amount," +
    " (SELECT COUNT(quotation_gid) FROM smr_trn_treceivequotation WHERE quotation_status = 'Quotation Amended' AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS quotationcancelled_count," +
    " (SELECT round(SUM(Grandtotal),2) FROM smr_trn_treceivequotation WHERE quotation_status = 'Quotation Amended' AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS quotationcancelled_amount," +
    " (SELECT COUNT(*) FROM smr_trn_tsalesenquiry WHERE enquiry_status = 'Quotation Accepted' AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS quotation_accepted," +
    " (SELECT round(SUM(potorder_value),2) FROM smr_trn_tsalesenquiry WHERE enquiry_status = 'Quotation Accepted' AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS quotationaccepted_amount," +
    " (SELECT COUNT(salesorder_gid) FROM smr_trn_tsalesorder WHERE salesorder_status NOT IN('SO Amended') AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS order_count," +
    " (SELECT round(SUM(Grandtotal),2) FROM smr_trn_tsalesorder WHERE salesorder_status NOT IN('SO Amended') AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS order_amount," +
    " (SELECT COUNT(*) FROM smr_trn_tsalesorder WHERE salesorder_status = 'Delivery Completed' AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS delivery_count," +
    " (SELECT round(SUM(Grandtotal),2) FROM smr_trn_tsalesorder WHERE salesorder_status = 'Delivery Completed' AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS delivery_amount," +
    " (SELECT COUNT(*) FROM smr_trn_tsalesorder WHERE salesorder_status NOT IN('SO Amended') AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') -" +
    " (SELECT COUNT(*) FROM smr_trn_tsalesorder WHERE salesorder_status = 'Delivery Completed' AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS deliverypending_count," +
    " (SELECT round(SUM(Grandtotal),2) FROM smr_trn_tsalesorder WHERE salesorder_status NOT IN('SO Amended') AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') - " +
    " (SELECT round(SUM(Grandtotal),2) FROM smr_trn_tsalesorder WHERE salesorder_status = 'Delivery Completed' AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS deliverypending_amount," +
    " (select  COUNT(*) from rbl_trn_tinvoice where customer_gid= '" + lscustomer_gid + "' and customer_gid != '') AS invoice_count," +
    " (select round(SUM(invoice_amount),2) from rbl_trn_tinvoice where customer_gid= '" + lscustomer_gid + "' and customer_gid != '') AS invoice_amount," +
    " (SELECT COUNT(*) FROM rbl_trn_tinvoice WHERE invoice_status = 'payment done' AND customer_gid ='" + lscustomer_gid + "' and customer_gid != '') AS paymentreceived_count," +
    " (SELECT round(SUM(invoice_amount),2) FROM rbl_trn_tinvoice WHERE invoice_status = 'payment done' AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS paymentreceived_amount," +
    " (SELECT COUNT(invoice_gid) FROM rbl_trn_tinvoice WHERE payment_amount = '0.00' AND invoice_status = 'Payment Pending' AND customer_gid = '" + lscustomer_gid + "' and customer_gid != '') AS paymentpending_count," +
    " (SELECT round(SUM(invoice_amount),2) FROM rbl_trn_tinvoice WHERE payment_amount = '0.00' AND invoice_status = 'Payment Pending' AND customer_gid ='" + lscustomer_gid + "' and customer_gid != '') AS paymentpending_amount";



            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<threesixtycountdetails_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new threesixtycountdetails_list
                    {
                        totalquotation_count = dt["totalquotation_count"].ToString(),
                        totalquotation_amount = dt["totalquotation_amount"].ToString(),
                        quotationaccepted_count = dt["quotation_accepted"].ToString(),
                        quotationaccepted_amount = dt["quotationaccepted_amount"].ToString(),
                        quotationdropped_count = dt["quotationcancelled_count"].ToString(),
                        quotationdropped_amount = dt["quotationcancelled_amount"].ToString(),
                        totalorder_count = dt["order_count"].ToString(),
                        totalorder_amount = dt["order_amount"].ToString(),
                        delevery_count = dt["delivery_count"].ToString(),
                        delevery_amount = dt["delivery_amount"].ToString(),
                        orderpending_count = dt["deliverypending_count"].ToString(),
                        orderpending_amount = dt["deliverypending_amount"].ToString(),
                        totalinvoice_count = dt["invoice_count"].ToString(),
                        totalinvoice_amount = dt["invoice_amount"].ToString(),
                        paymentreceived_count = dt["paymentreceived_count"].ToString(),
                        paymentreceived_amount = dt["paymentreceived_amount"].ToString(),
                        paymentpending_count = dt["paymentpending_count"].ToString(),
                        paymentpending_amount = dt["paymentpending_amount"].ToString()

                    });
                    values.Threesixtycountdetails_list = getModuleList;
                }
            }
            dt_datatable.Dispose();


        }

        public void DaGetleaddropdownDetails(string employee_gid, MdlCrmDashboard values)
        {
            

            msSQL = "select leadstage_gid,leadstage_name from crm_mst_tleadstage where leadstage_gid != '2' and leadstage_gid != '8'";



            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<leadstage_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new leadstage_list
                    {
                        leadstage_gid = dt["leadstage_gid"].ToString(),
                        leadstage_name = dt["leadstage_name"].ToString()
                       

                    });
                    values.LeadStage_list = getModuleList;
                }
            }
            dt_datatable.Dispose();



        }

        public void DaGetPostMblLeadStageDetails(string employee_gid, MdlCrmDashboard values)
        {
        
        
        }

        }


  }
