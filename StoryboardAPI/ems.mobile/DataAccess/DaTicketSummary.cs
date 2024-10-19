using ems.mobile.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace ems.mobile.DataAccess
{
    public class DaTicketSummary
    {

        dbconn objdbconn = new dbconn();
        string msSQL = string.Empty;
        DataTable dt_datatable;
        cmnfunctions objcmnfunctions = new cmnfunctions();

        public void DaGetTicketSummary(string user_code,MdlTicketSummary values)
        {
            try
            {
                msSQL = " SELECT date_format(a.complaint_date, '%d-%m-%Y') as complaint_date ,a.user_code as raised_by,x.image_path,z.video_path,a.location_name as location_remarks, " +
                        " a.complaint_gid, a.complaint_refno, a.complaint_title, " +
                        " CONCAT(a.customer_contactno) as customer_contactno, " +
                        " a.complaint_remarks, " +
                        " CASE WHEN e.leadstage_name<> '' THEN e.leadstage_name ELSE a.assign_status END AS 'assign_status', " +
                        " d.campaign_gid, d.complaint2campaign_gid " +
                        " FROM smr_trn_tcomplaint a " + 
                        " LEFT JOIN hrm_mst_temployee c ON a.created_by = c.employee_gid " +
                        " LEFT JOIN adm_mst_tuser b ON c.user_gid = b.user_gid " +
                        " LEFT JOIN smr_trn_tcomplaint2campaign d ON a.complaint_gid = d.complaint_gid " +
                        " LEFT JOIN smr_trn_tcomplaint_images x ON a.complaint_gid = a.complaint_gid " +
                        " LEFT JOIN smr_trn_tcomplaint_videos z ON a.complaint_gid = a.complaint_gid " +
                        " LEFT JOIN smr_mst_tleadstage e ON d.leadstage_gid = e.leadstage_gid " +
                        " WHERE a.user_code = '"+user_code+"' " +
                        " GROUP BY a.complaint_gid " +
                        " ORDER BY a.complaint_date DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ticket_summary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ticket_summary
                        {
                            complaint_date = dt["complaint_date"].ToString(),
                            raised_by = dt["raised_by"].ToString(),
                            image_path = dt["image_path"].ToString(),
                            video_path = dt["video_path"].ToString(),
                            location_remarks = dt["location_remarks"].ToString(),
                            complaint_gid = dt["complaint_gid"].ToString(),
                            complaint_refno = dt["complaint_refno"].ToString(),
                            complaint_title = dt["complaint_title"].ToString(),
                            customer_contactno = dt["customer_contactno"].ToString(),
                            complaint_remarks = dt["complaint_remarks"].ToString(),
                            assign_status = dt["assign_status"].ToString(),
                            campaign_gid = dt["campaign_gid"].ToString(),
                            complaint2campaign_gid = dt["complaint2campaign_gid"].ToString()
                        });
                        values.ticket_summary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error fetching ticketsummary !";
            }
        }

        public void DaGetTicketCount(string user_code, MdlTicketSummary values)
        {
            try
            {
                msSQL = " select count(complaint_gid) as tickets_count, " +
                        " (select count(complaint2campaign_gid)  from smr_trn_tcomplaint2campaign a " +
                        " left join smr_trn_tcomplaint b on a.complaint_gid = b.complaint_gid " +
                        " WHERE leadstage_gid = '2' AND user_code = '"+user_code+"') as workbin, " +
                        " (select count(complaint2campaign_gid) from smr_trn_tcomplaint2campaign a " +
                        " left join smr_trn_tcomplaint b on a.complaint_gid = b.complaint_gid " +
                        " WHERE leadstage_gid = '3' AND user_code = '"+user_code+"') as completed, " +
                        " (select count(complaint2campaign_gid) from smr_trn_tcomplaint2campaign a " +
                        " left join smr_trn_tcomplaint b on a.complaint_gid = b.complaint_gid " +
                        " WHERE leadstage_gid = '4' AND user_code ='"+user_code+"') as droped " +
                        " from smr_trn_tcomplaint WHERE user_code = '"+user_code+"' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ticket_count>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ticket_count
                        {
                            tickets_count = dt["tickets_count"].ToString(),
                            workbin = dt["workbin"].ToString(),
                            completed = dt["completed"].ToString(),
                            droped = dt["droped"].ToString(),
 
                        });
                        values.ticket_count = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error fetching ticketcount !";
            }
        }

        public void DaGetTicketSummaryDetailView(string complaint_gid, MdlTicketSummary values)
        {
            try
            {
                msSQL = "  SELECT date_format(a.complaint_date,'%d-%m-%Y') as complaint_date ,a.user_code as raised_by,a.imagePath,a.videopath,concat(a.unit,'/' ,a.block, '/',a.section) as location_remarks,a.complaint_gid , a.complaint_refno, a.complaint_title, " +
                        " CONCAT(a.customer_contactno) as customer_contactno,a.request_type as category_type, " +
                        " a.complaint_remarks, " +
                        " CASE WHEN e.leadstage_name<> '' THEN e.leadstage_name ELSE a.assign_status END AS 'assign_status', " +
                        " d.campaign_gid, d.complaint2campaign_gid " +
                        " FROM smr_trn_tcomplaint a " +
                        " LEFT JOIN hrm_mst_temployee c ON a.created_by = c.employee_gid " +
                        " LEFT JOIN adm_mst_tuser b ON c.user_gid = b.user_gid " +
                        " LEFT JOIN smr_trn_tcomplaint2campaign d ON a.complaint_gid = d.complaint_gid " +
                        " LEFT JOIN smr_mst_tleadstage e ON d.leadstage_gid = e.leadstage_gid " +
                        " WHERE a.complaint_gid = '"+ complaint_gid + "' GROUP BY a.complaint_gid " +
                        " ORDER BY a.complaint_date DESC, a.complaint_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ticketsummary_detailview>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ticketsummary_detailview
                        {
                            complaint_date = dt["complaint_date"].ToString(),
                            raised_by = dt["raised_by"].ToString(),
                            image_path = dt["imagePath"].ToString(),
                            video_path = dt["videopath"].ToString(),
                            location_remarks = dt["location_remarks"].ToString(),
                            complaint_gid = dt["complaint_gid"].ToString(),
                            complaint_refno = dt["complaint_refno"].ToString(),
                            complaint_title = dt["complaint_title"].ToString(),
                            customer_contactno = dt["customer_contactno"].ToString(),
                            category_type = dt["category_type"].ToString(),
                            complaint_remarks = dt["complaint_remarks"].ToString(),
                            assign_status = dt["assign_status"].ToString(),
                            campaign_gid = dt["campaign_gid"].ToString(),
                            complaint2campaign_gid = dt["complaint2campaign_gid"].ToString()

                        });
                        values.ticketsummary_detailview = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error occurs ticket Summary detail View !";
            }
        }

        public void DaGetTicketSummaryManagerAssign(string complaint_gid, MdlTicketSummary values)
        {
            try
            {
                msSQL = " select a.campaign_gid,c.complaint_gid,a.employee_gid,b.request_gid,c.request_gid,c.request_type as category_type, " +
                        " CONCAT(c.customer_contactperson, ' / ', c.customer_contactno, ' / ', c.customer_email) as customer_contactno, " +
                        " c.customer_email as email,c.complaint_remarks,concat(c.unit, '/', c.block, '/', c.section) as location_name,c.user_code as raised_by,c.assign_status,c.complaint_refno,date_format(c.complaint_date, '%d-%m-%Y') as complaint_date, " +
                        " CASE WHEN e.leadstage_name<> '' THEN e.leadstage_name ELSE c.assign_status END AS 'assign_status',c.imagePath,c.videopath, " +
                        " c.customer_name, c.customer_gid, c.customer_contactperson as customercontact_name,c.complaint_title, " +
                        " (SELECT CONCAT(b.user_firstname, '', b.user_lastname) " +
                        " FROM hrm_mst_temployee a " +
                        " LEFT JOIN adm_mst_tuser b ON a.user_gid = b.user_gid " +
                        " LEFT JOIN smr_trn_tcomplaint2campaign c ON c.assign_to = a.employee_gid " +
                        " WHERE c.complaint_gid = '"+ complaint_gid + "'" +
                        " LIMIT 1 ) AS contact_name, c.customer_contactno as mobile " +
                        " from smr_trn_tcomplaint2manager a " +
                        " LEFT JOIN smr_trn_tcomplaintcampaign b on b.campaign_gid = a.campaign_gid " +
                        " LEFT JOIN smr_trn_tcomplaint c on c.request_gid = b.request_gid " +
                        " LEFT JOIN smr_trn_tcomplaint2campaign d ON c.complaint_gid = d.complaint_gid " +
                        " LEFT JOIN smr_mst_tleadstage e ON d.leadstage_gid = e.leadstage_gid WHERE c.complaint_gid = '"+ complaint_gid + "' GROUP BY c.complaint_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ticketsummary_managerassign>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ticketsummary_managerassign
                        {
                            complaint_date = dt["complaint_date"].ToString(),
                            complaint_refno = dt["complaint_refno"].ToString(),
                            complaint_title = dt["complaint_title"].ToString(),
                            customer_contactno = dt["customer_contactno"].ToString(),
                            assign_status = dt["assign_status"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            email = dt["email"].ToString(),
                            location_name = dt["location_name"].ToString(),
                            raised_by = dt["raised_by"].ToString(),
                            campaign_gid = dt["campaign_gid"].ToString(),
                            complaint_remarks = dt["complaint_remarks"].ToString(),
                            complaint_gid = dt["complaint_gid"].ToString(),
                            category_type = dt["category_type"].ToString(),
                            imagePath = dt["imagePath"].ToString(),
                            videopath = dt["videopath"].ToString(),
                            contact_name = dt["contact_name"].ToString()

                        });
                        values.ticketsummary_managerassign = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Error Occurs Ticket Summary Manager Assign !";
            }
        }
    }
}