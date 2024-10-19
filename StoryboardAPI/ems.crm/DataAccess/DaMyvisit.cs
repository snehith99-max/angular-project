﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.crm.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
//using System.Web;
//using OfficeOpenXml;
using System.Configuration;
using System.IO;
//using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;



namespace ems.crm.DataAccess
{
    public class DaMyvisit
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader  objOdbcDataReader ;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetGid3, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;



        public void DaGetExpiredSummary(MdlMyvisit values, string employee_gid)
        {
            msSQL = " select f.assign_to,a.log_gid,a.schedulelog_gid,b.leadbank_gid,a.schedule_remarks,a.schedule_status, " +
                 " cast(concat(a.schedule_date,' ', a.schedule_time) as datetime) as schedule," +
                " concat(b.leadbank_address1,'/',b.leadbank_address2,'/',b.leadbank_city,'/',b.leadbank_state,'-',b.leadbank_pin)as customer_address," +
                " concat(c.leadbankcontact_name,' / ',c.mobile,' / ',c.email) as contact_details," +
                " b.leadbank_name,concat(d.region_name,'/',e.source_name) as region_source,a.schedule_type,a.schedule_remarks,f.lead2campaign_gid,c.leadbankcontact_gid  from crm_trn_tschedulelog a " +
                 " inner join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                 " inner join crm_trn_tleadbankcontact c on b.leadbank_gid = c.leadbank_gid " +
                 " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                 " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                 " left join crm_trn_tlead2campaign f on b.leadbank_gid=f.leadbank_gid "+
                 " where (a.schedule_type='Meeting') and a.schedule_date < curdate() " +
                 " and a.assign_to ='" + employee_gid + "' and a.schedule_status!='Closed'" +
                 " and c.status='Y'  order by b.leadbank_name asc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<ExpiredVisit_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new ExpiredVisit_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        customer_address = dt["customer_address"].ToString(),
                        region_source = dt["region_source"].ToString(),
                        schedule_type = dt["schedule_type"].ToString(),
                        schedule = dt["schedule"].ToString(),
                        ScheduleRemarks = dt["schedule_remarks"].ToString(),
                        schedule_status = dt["schedule_status"].ToString(),
                        log_gid = dt["log_gid"].ToString(),
                        schedulelog_gid = dt["schedulelog_gid"].ToString(),
                    });
                    values.ExpiredVisit_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetCompletedSummary(MdlMyvisit values, string employee_gid)
        {
            msSQL = " select f.assign_to,a.log_gid,a.schedulelog_gid,b.leadbank_gid,a.schedule_remarks,a.updated_remarks,a.schedule_status, " +
                 " cast(concat(a.schedule_date,' ', a.schedule_time) as datetime) as schedule," +
                " concat(b.leadbank_address1,'/',b.leadbank_address2,'/',b.leadbank_city,'/',b.leadbank_state,'-',b.leadbank_pin)as customer_address," +
                " concat(c.leadbankcontact_name,' / ',c.mobile,' / ',c.email) as contact_details," +
                " b.leadbank_name,concat(d.region_name,'/',e.source_name) as region_source,a.schedule_type,a.schedule_remarks,f.lead2campaign_gid,c.leadbankcontact_gid  from crm_trn_tschedulelog a " +
                 " inner join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                 " inner join crm_trn_tleadbankcontact c on b.leadbank_gid = c.leadbank_gid " +
                 " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                 " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                 " left join crm_trn_tlead2campaign f on b.leadbank_gid=f.leadbank_gid " +
                 " where (a.schedule_type='Meeting') and a.schedule_status='Closed' " +
                 " and a.assign_to ='" + employee_gid + "'" +
                 " and c.status='Y'  order by b.leadbank_name asc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<ExpiredVisit_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new ExpiredVisit_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        customer_address = dt["customer_address"].ToString(),
                        region_source = dt["region_source"].ToString(),
                        schedule_type = dt["schedule_type"].ToString(),
                        schedule = dt["schedule"].ToString(),
                        ScheduleRemarks = dt["updated_remarks"].ToString(),
                        schedule_status = dt["schedule_status"].ToString(),
                        log_gid = dt["log_gid"].ToString(),
                        schedulelog_gid = dt["schedulelog_gid"].ToString(),
                    });
                    values.ExpiredVisit_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetTodaySummary(MdlMyvisit values, string employee_gid)
        {
            msSQL = msSQL = " select f.assign_to,a.log_gid,a.schedulelog_gid,b.leadbank_gid,a.schedule_remarks,a.schedule_status, " +
                 " cast(concat(a.schedule_date,' ', a.schedule_time) as datetime) as schedule," +
                " concat(b.leadbank_address1,'/',b.leadbank_address2,'/',b.leadbank_city,'/',b.leadbank_state,'-',b.leadbank_pin)as customer_address," +
                " concat(c.leadbankcontact_name,' / ',c.mobile,' / ',c.email) as contact_details," +
                " b.leadbank_name,concat(d.region_name,'/',e.source_name) as region_source,a.schedule_type,a.schedule_remarks,f.lead2campaign_gid,c.leadbankcontact_gid  from crm_trn_tschedulelog a " +
                 " inner join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                 " inner join crm_trn_tleadbankcontact c on b.leadbank_gid = c.leadbank_gid " +
                 " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                 " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                 " left join crm_trn_tlead2campaign f on b.leadbank_gid=f.leadbank_gid " +
                 " where (a.schedule_type='Meeting') and a.schedule_date = curdate() " +
                 " and a.assign_to ='" + employee_gid + "' and a.schedule_status != 'Closed'" +
                 " and c.status='Y'  order by b.leadbank_name asc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Todayvisit_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Todayvisit_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        customer_address = dt["customer_address"].ToString(),
                        region_name = dt["region_source"].ToString(),
                        schedule_type = dt["schedule_type"].ToString(),
                        schedule = dt["schedule"].ToString(),
                        schedulelog_gid = dt["schedulelog_gid"].ToString(),
                        log_gid = dt["log_gid"].ToString(),
                        ScheduleRemarks = dt["schedule_remarks"].ToString(),
                        schedule_status = dt["schedule_status"].ToString(),
                        //created_date = dt["created_date"].ToString(),
                    });
                    values.Todayvisit_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetUpcomingSummary(MdlMyvisit values, string employee_gid)
        {
            msSQL = " select f.assign_to,a.log_gid,a.schedulelog_gid,b.leadbank_gid,a.schedule_remarks,a.schedule_status, " +
                 " cast(concat(a.schedule_date,' ', a.schedule_time) as datetime) as schedule," +
                " concat(b.leadbank_address1,'/',b.leadbank_address2,'/',b.leadbank_city,'/',b.leadbank_state,'-',b.leadbank_pin)as customer_address," +
                " concat(c.leadbankcontact_name,' / ',c.mobile,' / ',c.email) as contact_details," +
                " b.leadbank_name,concat(d.region_name,'/',e.source_name) as region_source,a.schedule_type,a.schedule_remarks,f.lead2campaign_gid,c.leadbankcontact_gid  from crm_trn_tschedulelog a " +
                 " inner join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                 " inner join crm_trn_tleadbankcontact c on b.leadbank_gid = c.leadbank_gid " +
                 " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                 " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                 " left join crm_trn_tlead2campaign f on b.leadbank_gid=f.leadbank_gid " +
                 " where (a.schedule_type='Meeting') and a.schedule_date > curdate() " +
                 " and a.assign_to ='" + employee_gid + "' and a.schedule_status!='Closed'" +
                 " and c.status='Y'  order by b.leadbank_name asc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Upcomingvisit_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Upcomingvisit_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        customer_address = dt["customer_address"].ToString(),
                        region_source = dt["region_source"].ToString(),
                        schedule_type = dt["schedule_type"].ToString(),
                        schedule = dt["schedule"].ToString(),
                        schedulelog_gid = dt["schedulelog_gid"].ToString(),
                        log_gid = dt["log_gid"].ToString(),

                        ScheduleRemarks = dt["schedule_remarks"].ToString(),
                        schedule_status = dt["schedule_status"].ToString(),





                        //created_date = dt["created_date"].ToString(),
                    });
                    values.Upcomingvisit_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostcloselog( string employee_gid, ExpiredVisit_list values)
        {
            String str = values.meeting_time;

            msGetGid3 = objcmnfunctions.GetMasterGID("BFLC");
            msSQL = "insert into crm_trn_tfieldlog(" +
                    " fieldlog_gid," +
                    " leadbank_gid," +
                    " log_gid, " +
                    " fieldvisit_date," +
                    " fieldvisit_hour," +
                    " fieldvisit_minute," +
                    " fieldvisit_location," +
                    " fieldvisit_contactperson," +
                    " fieldvisit_contactperson2," +
                    " prospective_percentage," +
                    " fieldvisit_remarks," +
                    " created_by, " +
                    " created_date)" +
                    " values ( " +
                    " '" + msGetGid3 + "', " +
                    " '" + values.leadbank_gid + "', " +
                    " '" + values.log_gid + "'," +
                    " '" + values.date_of_demo + "'," +
                    " '" + values.meeting_time[1]+ "'," +
                    " '" + values.meeting_time[3] + "'," +
                    " '" + values.location + "'," +
                     " '" + values.contact_person + "'," +
                      " '" + values.altrnate_person + "'," +
                    " '" + values.prosperctive_percentage + "'," +
                    " '" + values.schedule_remarks + "',"+
                    "'" + employee_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {

                msSQL = " update crm_trn_tlead2campaign set leadstage_gid='3',assign_to='" + employee_gid + "' where leadbank_gid='" + values.leadbank_gid + "' "; ;
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " update crm_trn_tschedulelog set " +
                 " schedule_status='Closed'," +
                 " updated_by= '" + employee_gid + "', " +
                 " closing_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                 " status_flag='Y'," +
                 " updated_remarks = '" + values.schedule_remarks + "'" +
                 " where log_gid = '" + values.log_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            if (mnResult != 0)
            {
                
                values.status = true;
                values.message = " Lead Closed Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error Occured While Closing Records";

            }

        }
        //+ "' and (leadstage_gid='2' or leadstage_gid is null)"

        public void DaPostpostonedlog(Upcomingvisit_list values, string user_gid)
        {

            msSQL = " update crm_trn_tschedulelog set " +
                     " schedule_date= '" + values.postponed_date+ "', " +
                     " schedule_time='" + values.meeting_time_postponed + "', " +
                     " updated_by = '" + user_gid + "'," +
                     " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where schedulelog_gid='" + values.schedulelog_gid + "' ";



            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = " Postponed Date Updated Successfully";
            }
            else
            {
                values.status = false;
                values.message = " Error Occured While Updating Records ";

            }

        }

        public void DaPostdroplog(ExpiredVisit_list values, string user_gid)
        {
            msSQL = " update crm_trn_tlead2campaign set leadstage_gid='5',lead_base='My Visit' where leadbank_gid='" + values.leadbank_gid + "' and (leadstage_gid='2') ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            msSQL = " DELETE FROM crm_trn_tschedulelog WHERE leadbank_gid ='" + values.leadbank_gid+"'";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Data Moved To Drop Successfully !!";
            }
            else
            {
                values.status = false;
                values.message = "Error While Moving To Drop!!";
            }

        }

        public void DaGetProductdropdown(MdlMyvisit values)
        {
            msSQL = "select product_gid,product_name from pmr_mst_tproduct";


            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<product_list1>();

            if (dt_datatable.Rows.Count != 0)
            {


                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new product_list1
                    {
                        product_gid = dt["product_gid"].ToString(),
                        product_name = dt["product_name"].ToString()

                    });
                    values.product_list1 = getModuleList;
                }
            }

            dt_datatable.Dispose();
        }
        public void DaGetProductGroupdropdown(MdlMyvisit values)
        {

            msSQL = " Select DISTINCT(a.productgroup_name) ,a.productgroup_gid " +
              " from pmr_mst_tproductgroup a " +
              " left join pmr_mst_tproduct b on a.productgroup_gid=b.productgroup_gid ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<product_group_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new product_group_list
                    {
                        productgroup_gid = dt["productgroup_gid"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),


                    });
                    values.product_group_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaPostonline(ExpiredVisit_list values, string user_gid)
        {

            msGetGid = objcmnfunctions.GetMasterGID("BOMC");
            msSQL = " insert into crm_trn_tonmeetlog(" +
                     " onmeetlog_gid," +
                     " leadbank_gid," +
                      " log_gid," +
                      " onmeet_date," +
                      " onmeet_hour," +
                      " onmeet_contactperson," +
                       " technical_aid," +
                       " demo_shown," +
                       " prospective_percentage," +
                       "onmeet_remarks," +
                       "created_by," +
                      " created_date)" +
                      " values(" +
                      " '" + msGetGid + "'," +
                     " '" + values.leadbank_gid + "'," +
                     " '" + values.log_gid + "'," +
                      "'" + values.date_of_demo_online + "'," +
                      "'" + values.meeting_time_online + "'," +
                      " '" + values.contact_person_online + "'," +
                      "'" + values.technical_assist + "'," +
                      "'" + values.schedule_type + "'," +
                      "'" + values.prosperctive_percentage_online + "'," +
                       "'" + values.demo_remarks + "'," +
                        "'" + user_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Online Meeting Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Adding Online Meeting";
            }

        }

        public void DaPostoffline(ExpiredVisit_list values, string user_gid)
        {

            msGetGid = objcmnfunctions.GetMasterGID("BFLC");
            msSQL = " insert into crm_trn_tfieldlog(" +
                     " fieldlog_gid," +
                     " leadbank_gid," +
                     "log_gid,"+
                      "demo_shown," +
                      " fieldvisit_date," +
                      " fieldvisit_hour," +
                       " fieldvisit_contactperson," +
                       " fieldvisit_contactperson2," +
                       " fieldvisit_location," +
                        "fieldvisit_remarks," +
                       "created_by," +
                      " created_date)" +
                      " values(" +
                      " '" + msGetGid + "'," +
                     "'" + values.leadbank_gid + "'," +
                     "'" + values.log_gid + "',"+
                     "'" + values.schedule_type_offline + "'," +
                      "'" + values.date_of_visit_offline + "'," +
                      "'" + values.meeting_time_offline + "'," +
                      "'" + values.visited_by + "'," +
                      "'" + values.contact_person_offline + "'," +
                      "'" + values.location_offline + "'," +
                       "'" + values.meeting_remarks_offline + "'," +
                        "'" + user_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Offline Meeting Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Adding Postoffline Meeting";
            }

        }


        public void DaGetMyVisitCount(MdlMyvisit values, string employee_gid)
        {
            msSQL = "  select (select count(distinct b.leadbank_gid) from crm_trn_tschedulelog b where b.schedule_date = DATE_FORMAT(CURDATE()," +
                " '%Y-%m-%d')  and b.schedule_type='Meeting' and b.assign_to = '"+ employee_gid + "' and b.schedule_status!='Closed' ) as todayvisit , (select count(distinct b.leadbank_gid)" +
                " from crm_trn_tschedulelog b where b.schedule_date > DATE_FORMAT(CURDATE(), '%Y-%m-%d') and b.schedule_type='Meeting'" +
                " and b.assign_to = '"+ employee_gid + "' and b.schedule_status!='Closed' ) as upcoming, (select count(distinct b.leadbank_gid) from crm_trn_tschedulelog b" +
                " where b.schedule_date < DATE_FORMAT(CURDATE(), '%Y-%m-%d') and b.schedule_type='Meeting' and b.assign_to = '"+ employee_gid + "' and b.schedule_status!='Closed' ) as expired," +
                "  (select count(distinct b.leadbank_gid) from crm_trn_tschedulelog b where b.schedule_status='Closed' and" +
                "  b.schedule_type='Meeting' and b.assign_to = '"+ employee_gid + "'  ) as completed";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<myvisitcount_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new myvisitcount_list
                    {
                        todayvisit = dt["todayvisit"].ToString(),
                        upcoming = dt["upcoming"].ToString(),
                        expired = dt["expired"].ToString(),
                        completed = dt["completed"].ToString(),
                       
                    });
                    values.myvisitcount_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetOnlineScheduleLogSummary(string log_gid,MdlMyvisit values)
        {
            msSQL = "select a.leadbank_name,b.onmeet_date,b.onmeet_hour,b.onmeet_contactperson,b.onmeet_remarks,b.technical_aid,b.demo_shown " +
                    "from crm_trn_tleadbank a " +
                    "left join crm_trn_tonmeetlog b on a.leadbank_gid=b.leadbank_gid " +
                    "where b.log_gid= '" + log_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<schedulelogsummary_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new schedulelogsummary_list
                    {
                        leadbank_name = dt["leadbank_name"].ToString(),
                        onmeet_date = dt["onmeet_date"].ToString(),
                        onmeet_hour = dt["onmeet_hour"].ToString(),
                        onmeet_contactperson = dt["onmeet_contactperson"].ToString(),
                        onmeet_remarks = dt["onmeet_remarks"].ToString(),
                        technical_aid = dt["technical_aid"].ToString(),
                        demo_shown = dt["demo_shown"].ToString(),

                    });
                    values.schedulelogsummary_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetOfflineScheduleLogSummary(string log_gid, MdlMyvisit values)
        {
            msSQL = "select a.leadbank_name,b.fieldvisit_date,b.fieldvisit_hour,b.fieldvisit_contactperson,b.fieldvisit_remarks,b.fieldvisit_contactperson2,b.demo_shown " +
                    "from crm_trn_tleadbank a " +
                    "left join crm_trn_tfieldlog b on a.leadbank_gid=b.leadbank_gid " +
                    "where b.log_gid='" + log_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<schedulelogsummary_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new schedulelogsummary_list
                    {
                        leadbank_name = dt["leadbank_name"].ToString(),
                        fieldvisit_date = dt["fieldvisit_date"].ToString(),
                        fieldvisit_hour = dt["fieldvisit_hour"].ToString(),
                        fieldvisit_contactperson = dt["fieldvisit_contactperson"].ToString(),
                        fieldvisit_remarks = dt["fieldvisit_remarks"].ToString(),
                        fieldvisit_contactperson2 = dt["fieldvisit_contactperson2"].ToString(),
                        demo_shown = dt["demo_shown"].ToString(),

                    });
                    values.schedulelogsummary_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
    }
}