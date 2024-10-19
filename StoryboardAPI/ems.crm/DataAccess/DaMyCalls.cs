
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.crm.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using System.Web.Http.Results;



namespace ems.crm.DataAccess
{
    public class DaMyCalls
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader  objOdbcDataReader ;
        DataTable dt_datatable;
        string msEmployeeGID, lssource_gid, lsbusinessvertical_gid, lsemployee_gid, lsentity_code, lsdesignation_code, lsleadbank_gid, lsCode, msGetGids1, msGetGid, msGetGids, msGetGid1, msGetGid7, msGetGid2, msGetGid3, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, product_gid, mnResult5;
        char lsstatus, lsaddtocustomer;

        public void DaGetcallschedulesummary(string employee_gid, MdlMyCalls values)
        {


            msSQL = " Select g.mobile,b.leadbank_name,b.customer_type,K.campaign_title, e.schedulelog_gid,e.schedule_type,e.schedule_remarks,e.schedule_status," +
                " concat_ws(' / ',g.leadbankcontact_name,g.mobile,g.email) as contact_details,concat_ws(' / ',e.schedule_date,e.schedule_time) as schedule_time,h.source_name, " +
                    " concat_ws(' / ',d.region_name,d.city) as region_name, g.mobile as dialed_number, " +
                    " Case when a.internal_notes is not null then a.internal_notes  when a.internal_notes is null then b.remarks  end as internal_notes, " +
                    " z.leadstage_name,  a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid  From crm_trn_ttelelead2campaign a " +
                    " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid " +
                    " left join crm_trn_tschedulelog e on a.leadbank_gid = e.leadbank_gid " +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid  " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
                    " left join crm_mst_tcustomertype c on b.customertype_gid = c.customertype_gid " +
                    " where a.assign_to = '" + employee_gid + "'   and b.leadbank_name is not null and  a.leadstage_gid != '5'" +
                    " and e.schedule_date = DATE_FORMAT(CURDATE(), '%Y-%m-%d')and g.main_contact ='Y' order by b.leadbank_name asc;";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<schedule_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new schedule_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        schedulelog_gid = dt["schedulelog_gid"].ToString(),
                        schedule_type = dt["schedule_type"].ToString(),
                        schedule_time = dt["schedule_time"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        source_name = dt["source_name"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        internal_notes = dt["schedule_remarks"].ToString(),
                        schedule_status = dt["schedule_status"].ToString(),
                        mobile = dt["mobile"].ToString()


                    });
                    values.schedule_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetupcomingcallschedulesummary(string employee_gid, MdlMyCalls values)
        {


            msSQL = " Select g.mobile,b.leadbank_name,b.customer_type,K.campaign_title, e.schedulelog_gid,e.schedule_type,concat_ws(' / ',e.schedule_date,e.schedule_time) as schedule_time,e.schedule_remarks,e.schedule_status," +
                " concat_ws(' / ',g.leadbankcontact_name,g.mobile,g.email) as contact_details,e.schedule_date,h.source_name, " +
                    " concat_ws(' / ',d.region_name,d.city,h.source_name) as region_name, g.mobile as dialed_number, " +
                    " Case when a.internal_notes is not null then a.internal_notes  when a.internal_notes is null then b.remarks  end as internal_notes, " +
                    " z.leadstage_name,  a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid  From crm_trn_ttelelead2campaign a " +
                    " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid " +
                    " left join crm_trn_tschedulelog e on a.leadbank_gid = e.leadbank_gid " +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid  " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                    " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
                    " left join crm_mst_tcustomertype c on b.customertype_gid = c.customertype_gid " +
                    " where a.assign_to = '" + employee_gid + "'   and b.leadbank_name is not null and  a.leadstage_gid != '5'" +
                    " and e.schedule_date > DATE_FORMAT(CURDATE(), '%Y-%m-%d')and g.main_contact ='Y' order by b.leadbank_name asc;";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<schedule_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new schedule_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        schedulelog_gid = dt["schedulelog_gid"].ToString(),
                        schedule_type = dt["schedule_type"].ToString(),
                        schedule_time = dt["schedule_time"].ToString(),
                        schedule_date = dt["schedule_date"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        source_name = dt["source_name"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        internal_notes = dt["schedule_remarks"].ToString(),
                        schedule_status = dt["schedule_status"].ToString(),
                        mobile = dt["mobile"].ToString()


                    });
                    values.schedule_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetcallnewsummary(string employee_gid, MdlMyCalls values)
        {
            //msSQL = "select from crm_trn_tappointment where ";
            //string appointment_gid = objdbconn.GetExecuteScalar(msSQL);
            msSQL = "Select g.mobile,b.leadbank_name,b.customer_type,K.campaign_title, " +
                " concat_ws(' / ',g.leadbankcontact_name,g.mobile,g.email) as contact_details, " +
                " concat_ws(' / ',d.region_name,d.city,h.source_name) as region_name, g.mobile as dialed_number," +
                " a.internal_notes, " +
                " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid, CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid " +
                " ,b.remarks From crm_trn_ttelelead2campaign a " +
                " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid " + 
                " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid  " +
                " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid" +
                " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
                " left join crm_mst_tcustomertype c on b.customertype_gid = c.customertype_gid" +
                " where a.assign_to = '" + employee_gid + "' " +
                " and (a.leadstage_gid ='1') and b.leadbank_name is not null and  a.leadstage_gid != '5'" +
                " and a.leadbank_gid  not in ( select leadbank_gid from crm_trn_tcalllog) " +
                " and g.status='Y' and g.main_contact ='Y' order by b.leadbank_name asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<schedule_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = " select count(s_no) from crm_trn_tleadnotes where leadbank_gid = '" + dt["leadbank_gid"].ToString() + "'  ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);
                    getmodulelist.Add(new schedule_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        campaign_gid = dt["campaign_gid"].ToString(),
                        leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        dialed_number = dt["dialed_number"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        appointment_gid = dt["appointment_gid"].ToString(),
                        remarks = dt["remarks"].ToString(),
                        notes_count = lsCode,


                    });
                    values.schedule_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetPendingSummary(string employee_gid, MdlMyCalls values)
        {
            msSQL = " Select g.mobile,b.leadbank_name,k.campaign_title," +
              " concat_ws(' / ',g.leadbankcontact_name,g.mobile,g.email) as contact_details," +
              " concat_ws(' / ',d.region_name,d.city,h.source_name) as regionname," +
              " a.internal_notes," +
              " date(i.schedule_date) as schedule," +
              " i.schedule_type,i.schedule_remarks,z.leadstage_name," +
              " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid, CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid " +
              " ,b.remarks From crm_trn_ttelelead2campaign a" +
              " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid        " +
              " left join crm_mst_tregion d on b.leadbank_region=d.region_gid           " +
              " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
              " left join crm_mst_tsource h on b.source_gid=h.source_gid                " +
              " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid          " +
              " left join crm_trn_tschedulelog i on a.leadbank_gid=i.leadbank_gid " +
              " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
              " where a.assign_to = '" + employee_gid + "' " +
              " and (a.leadstage_gid = '1') and a.leadbank_gid in ( select leadbank_gid from crm_trn_tcalllog)and g.main_contact ='Y' " +
              "  group by b.leadbank_name  order by i.schedule_date,i.schedule_time ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<new_pending_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = " select count(s_no) from crm_trn_tleadnotes where leadbank_gid = '" + dt["leadbank_gid"].ToString() + "'  ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);
                    getModuleList.Add(new new_pending_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        regionname = dt["regionname"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        appointment_gid = dt["appointment_gid"].ToString(),
                        remarks = dt["remarks"].ToString(),
                        notes_count = lsCode,

                    });

                    values.new_pending_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetcallfollowupsummary(string employee_gid, MdlMyCalls values)
        {
            msSQL = "Select  g.mobile,b.leadbank_name,b.customer_type,K.campaign_title, " +
                " concat_ws(' / ',g.leadbankcontact_name,g.mobile,g.email) as contact_details, " +
                " concat_ws(' / ',d.region_name,d.city,h.source_name) as region_name, " +
                " a.internal_notes, z.leadstage_name, " +
                " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid , b.remarks, CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid" + 
                " From crm_trn_ttelelead2campaign a " +
                " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid " +
                " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid  " +
                " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid" +
                " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
                " left join crm_mst_tcustomertype c on b.customertype_gid = c.customertype_gid" +
                " where a.assign_to = '" + employee_gid + "' " +
                " and (a.leadstage_gid ='2') and b.leadbank_name is not null and  a.leadstage_gid != '5'" +
                " and g.status='Y'and g.main_contact ='Y'  order by b.leadbank_name asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<schedule_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new schedule_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        campaign_gid = dt["campaign_gid"].ToString(),
                        leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        remarks = dt["remarks"].ToString(),
                        appointment_gid = dt["appointment_gid"].ToString(),
                        notes_count = lsCode,

                    });
                    values.schedule_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetcallprospectsummary(string employee_gid, MdlMyCalls values)
        {
            msSQL = "Select  b.leadbank_name,b.customer_type,K.campaign_title, " +
                " concat_ws(' / ',g.leadbankcontact_name,g.mobile,g.email) as contact_details, " +
                " concat_ws(' / ',d.region_name,d.city,h.source_name) as region_name,b.remarks, " +
                " a.internal_notes, z.leadstage_name, " +
                " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid , CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid" +
                " From crm_trn_ttelelead2campaign a " +
                " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid " +
                " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid  " +
                " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid" +
                " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
                " left join crm_mst_tcustomertype c on b.customertype_gid = c.customertype_gid" +
                " where a.assign_to = '" + employee_gid + "' " +
                " and (a.leadstage_gid ='7') and b.leadbank_name is not null and  a.leadstage_gid != '5' and g.main_contact ='Y'" +
                " order by b.leadbank_name asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<schedule_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new schedule_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        campaign_gid = dt["campaign_gid"].ToString(),
                        leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        appointment_gid = dt["appointment_gid"].ToString(),
                        remarks = dt["remarks"].ToString(),
                    });
                    values.schedule_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetcalldropsummary(string employee_gid, MdlMyCalls values)
        {
            msSQL = "Select  b.leadbank_name,b.customer_type,K.campaign_title, " +
                "concat_ws(' / ',g.leadbankcontact_name,g.mobile,g.email) as contact_details, " +
                " concat_ws(' / ',d.region_name,d.city,h.source_name) as region_name, " +
                " a.internal_notes, z.leadstage_name, " +
                " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid ,b.remarks , CASE WHEN EXISTS (SELECT 1 FROM crm_trn_tappointment app WHERE app.Leadstage_gid!=0 and  app.leadbank_gid = a.leadbank_gid) THEN 'Yes' ELSE 'No' END AS appointment_gid " +
                " From crm_trn_ttelelead2campaign a " +
                " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid " +
                " left join crm_mst_tregion d on b.leadbank_region=d.region_gid" +
                " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid  " +
                " left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                " left join crm_trn_tteleteam k on a.campaign_gid=k.campaign_gid" +
                " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
                " left join crm_mst_tcustomertype c on b.customertype_gid = c.customertype_gid" +
                " where a.assign_to = '" + employee_gid + "' " +
                " and (a.leadstage_gid ='5') and b.leadbank_name is not null and g.main_contact ='Y' " +
                " order by b.leadbank_name asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<schedule_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = " select count(s_no) from crm_trn_tleadnotes where leadbank_gid = '" + dt["leadbank_gid"].ToString() + "'  ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);
                    getmodulelist.Add(new schedule_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        campaign_gid = dt["campaign_gid"].ToString(),
                        leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        remarks = dt["remarks"].ToString(),
                        appointment_gid = dt["appointment_gid"].ToString(),
                        notes_count = lsCode,
                    });
                    values.schedule_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetcallallummary(string employee_gid, MdlMyCalls values)
        {
            msSQL = "select a.leadbank_gid,a.lead2campaign_gid,b.leadbank_name,e.customer_type,concat_ws('/',c.region_name,c.city) as region_name,d.source_name," +
                "concat_ws(' / ',f.leadbankcontact_name,f.email,f.mobile) as contact_details,f.mobile as dialed_number ," +
                " a.internal_notes,b.remarks,a.leadstage_gid from crm_trn_ttelelead2campaign a left join" +
                " crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid left join crm_mst_tregion c on c.region_gid=b.leadbank_region left join" +
                " crm_mst_tsource d on d.source_gid=b.source_gid left join crm_mst_tcustomertype e  on  e.customertype_gid=b.customertype_gid" +
                " left join crm_trn_tleadbankcontact f on b.leadbank_gid=f.leadbank_gid   where a.assign_to='"+ employee_gid + "' and f.main_contact ='Y' ;";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<schedule_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = " select count(s_no) from crm_trn_tleadnotes where leadbank_gid = '" + dt["leadbank_gid"].ToString() + "'  ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);
                    getmodulelist.Add(new schedule_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        source_name = dt["source_name"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        dialed_number = dt["dialed_number"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        remarks = dt["remarks"].ToString(),
                        leadstage_gid = dt["leadstage_gid"].ToString(),
                        notes_count = lsCode,

                    });
                    values.schedule_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }

        public result DaPostcalls(callinginput values, string user_gid)
        {
            result objresult = new result();
            try
            {
                string msSQL = "select agent_gid,agent_name,agent_mailid,agent_number from crm_smm_tclicktocallagents where in_callstatus = 'N' and user_gid ='" + user_gid + "'";
                OdbcDataReader objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader != null)
                {
                    if (objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Read();
                        string contactjson = "{\"station\":\"" + objOdbcDataReader["agent_number"].ToString() + "\",\"phone_number\":\"" + values.phone_number + "\",\"cli_number\":\"8633537772\",\"agent\" : {\"identity\":\"user\",\"value\":\"" + objOdbcDataReader["agent_mailid"].ToString() + "\"},\"custdata\":{}}";

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["clicktocallbaseurl"].ToString());
                        var request = new RestRequest("/api/v2/call", Method.POST);
                        request.AddHeader("Authorization", ConfigurationManager.AppSettings["Click2CallToken"]);
                        request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            click2callresponse objclick2callresponse = new click2callresponse();
                            objclick2callresponse = JsonConvert.DeserializeObject<click2callresponse>(response.Content);

                            msSQL = "update crm_smm_tclicktocallagents set uniqueid ='" + objclick2callresponse.uid + "', in_callstatus = 'Y' where agent_gid='" + objOdbcDataReader["agent_gid"].ToString() + "'";
                            int mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            objresult.message = "Success";
                            objresult.status = true;
                        }
                        else
                        {
                            click2callerrorresponse objclick2callresponse = new click2callerrorresponse();
                            objclick2callresponse = JsonConvert.DeserializeObject<click2callerrorresponse>(response.Content);
                            objresult.message = objclick2callresponse.error + " - " + objclick2callresponse.message;
                        }
                    }
                    else
                    {
                        objresult.message = "Cannot Place call since you are in call already!";
                    }
                }
                else
                {
                    objresult.message = "Error Occured!";
                }

            }
            catch (Exception ex)
            {
                objresult.message = "Exception occured! Please try again after sometime!!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString() + "******APIREF******* websiteCustomer2Whatsapp");
            }
            return objresult;

        }
        public void DaGetProductdropdown(MdlMyCalls values)
        {
            msSQL = "select product_gid,product_name from pmr_mst_tproduct";


            dt_datatable = objdbconn.GetDataTable(msSQL);
           
            var getModuleList = new List<product_list3>();
           
            if (dt_datatable.Rows.Count != 0)
            {
                

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new product_list3
                    {
                        product_gid = dt["product_gid"].ToString(),
                        product_name = dt["product_name"].ToString()

                    });
                    values.product_list3 = getModuleList;
                }
            }

            dt_datatable.Dispose();
        }
        public void DaGetProductGroupdropdown(string product_gid, MdlMyCalls values)
        {

            msSQL = " Select a.productgroup_gid, a.productgroup_name " +
                  " from pmr_mst_tproductgroup a " +
                  " inner join pmr_mst_tproduct b on a.productgroup_gid=b.productgroup_gid " +
                  " where b.product_gid='" + product_gid + "' " +
                  " order by productgroup_name asc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<product_group_list1>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new product_group_list1
                    {
                        productgroup_gid = dt["productgroup_gid"].ToString(),
                        productgroup_name = dt["productgroup_name"].ToString(),

                    });
                    values.product_group_list1 = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostFollowschedulelog(followup_list values, string user_gid)//for schedule call & meeting in campaign manager & my campaign
        {
            try
            {
                msSQL = " SELECT schedulelog_gid,leadbank_gid " +
                 " from crm_trn_tschedulelog " +
                 " where schedule_date = '" + values.schedule_date + "' and" +
                 " schedule_time = '" + values.schedule_time + "' " +
                 " and leadbank_gid = '" + values.leadbank_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Already schedule added";
                }
                else
                {
                    msGetGids1 = objcmnfunctions.GetMasterGID("BLGP");
                    msSQL = " insert into crm_trn_tlog ( " +
                    " log_gid, " +
                    " leadbank_gid, " +
                    " log_type, " +
                    " log_desc, " +
                    " log_by, " +
                    " log_date ) " +
                    " values (  " +
                    "'" + msGetGids1 + "'," +
                    "'" + values.leadbank_gid + "'," +
                    "'Schedule',";
                    if (values.schedule_remarks != null
                         )
                    {
                        msSQL += " '" + values.schedule_remarks.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += " '" + values.schedule_remarks + "',";
                    }
                  msSQL +=  "'" + user_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult1 == 0)
                    {
                        values.status = false;
                        values.message = "Error Occured While Inserting Records ";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                    }
                    else
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("BSLC");
                        msSQL = " insert into crm_trn_tschedulelog  (" +
                        " schedulelog_gid, " +
                        " leadbank_gid," +
                         " schedule_date, " +
                         " schedule_time, " +
                         " schedule_type, " +
                          " schedule_remarks, " +
                          " status_flag, " +
                          " reference_gid, " +
                          " log_gid, " +
                          " schedule_status, " +
                          " created_by, " +
                          " created_date ) " +
                        " values (" +
                       " '" + msGetGid + "', " +
                       " '" + values.leadbank_gid + "', " +
                       " '" + values.schedule_date + "'," +
                       " '" + values.schedule_time + "'," +
                       " '" + values.schedule_type + "',";
                       if (values.schedule_remarks != null)
                         
                        {
                            msSQL += " '" + values.schedule_remarks.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += " '" + values.schedule_remarks + "',";
                        }
                       msSQL += " 'N'," +
                       "'" + values.lead2campaign_gid + "'," +
                       "'" + msGetGids1 + "'," +
                       "'Pending'," +
                       "'" + user_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            if (values.schedule_type == "Meeting")
                            {
                                msSQL = " update crm_trn_ttelelead2campaign set leadstage_gid='7', updated_by = '" + user_gid + "'," +
                                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                   "where leadbank_gid='" + values.leadbank_gid + "'and (leadstage_gid='1' or leadstage_gid='2' and leadstage_gid !='5') ";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if(mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = " Call scheduled  Successfully";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = " Error Occurs While Scheduling Call!! ";
                                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********Error Occured While Updating Telecaller Status*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                }
                            }
                            else
                            {
                                values.status = true;
                                values.message = " Call scheduled  Successfully";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = " Error Occurs While Scheduling Call!! ";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                        }
                    }

                   
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = " Error Occurs While Scheduling Call!! ";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }
       public void DaPostNewlog( string user_gid, new_list values)/////// LEAD STAGE TRANSFER BY CALL RECORD FOR ALL
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("BCLC");

                msSQL = "select moving_stage from crm_mst_callresponse where callresponse_gid='" + values.call_response + "' ;";
                string moving_stage = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " insert into crm_trn_tcalllog  (" +
                " calllog_gid, " +
                " leadbank_gid," +
                 " mobile_number, " +
                 " moving_stage, " +
                 " call_response, " +
                  " prospective_percentage, " +
                  " product_gid, " +
                  " remarks, " +
                  " created_by, " +
                  " created_date ) " +
                " values (" +
               " '" + msGetGid + "', " +
               " '" + values.leadbank_gid + "', " +
               " '" + values.dialed_number + "'," +
               " '" + moving_stage + "'," +
               " '" + values.call_response + "'," +
              " '" + values.prosperctive_percentage + "'," +
              " '" + values.product_name + "'," +
               " '" + values.call_feedback.Replace("'", "\\\'") + "'," +
               "'" + user_gid + "'," +
               "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    if (moving_stage == "7")
                    {
                        msSQL = " update crm_trn_ttelelead2campaign set leadstage_gid='" + moving_stage + "', updated_by = '" + user_gid + "'," + " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                           "where leadbank_gid='" + values.leadbank_gid + "'and (leadstage_gid='1' or leadstage_gid='2' and leadstage_gid !='5') ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msGetGid2 = objcmnfunctions.GetMasterGID("BSLC");
                        msGetGids1 = objcmnfunctions.GetMasterGID("BLGP");
                        msSQL = " insert into crm_trn_tschedulelog  (" +
                        " schedulelog_gid, " +
                        " leadbank_gid," +
                         " schedule_type, " +
                          " status_flag, " +
                          " log_gid, " +
                          " created_by, " +
                          " schedule_date, " +
                          " schedule_time, " +
                          " created_date ) " +
                        " values (" +
                       " '" + msGetGid2 + "', " +
                       " '" + values.leadbank_gid + "', " +
                       " 'Meeting'," +
                       " 'N'," +
                       "'" + msGetGids1 + "'," +
                       "'" + user_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       "'" + DateTime.Now.ToString("HH:mm") + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = " Call Recorded Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = " Error Occurs While Recording Call";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                        }
                    }
                    else
                    {
                        msSQL = " update crm_trn_ttelelead2campaign set leadstage_gid='" + moving_stage + "', updated_by = '" + user_gid + "'," +
                     " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                       "where leadbank_gid='" + values.leadbank_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = " Call Recorded Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = " Error Occurs While Recording Call ";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                        }
                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = " Call Recorded Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = " Error Occurs While Recording Call ";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                    }
                }
                else
                {
                    values.status = false;
                    values.message = " Error Occurs While Recording Call ";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                }
            }
            catch(Exception ex)
            {
                values.message = "Exception occured while Posting calls Record!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }
        public void DaPostFollowuplog(string user_gid, followup_list values)
        {

            msGetGid = objcmnfunctions.GetMasterGID("BCLC");
            msSQL = " insert into crm_trn_tcalllog  (" +
            " calllog_gid, " +
            " leadbank_gid," +
             " mobile_number, " +
             " call_response, " +
              " prospective_percentage, " +
              " remarks, " +
              " created_by, " +
              " schedule_date, " +
              " created_date ) " +
            " values (" +
           " '" + msGetGid + "', " +
           " '" + values.leadbank_gid + "', " +
           " '" + values.dialed_number + "'," +
           " '" + values.call_response + "'," +
          " '" + values.prosperctive_percentage + "'," +
           " '" + values.call_feedback + "'," +
           "'" + user_gid + "'," +
           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msSQL = "select moving_stage from crm_mst_callresponse where call_response='" + values.call_response + "' ;";
            string moving_stage = objdbconn.GetExecuteScalar(msSQL);
            if (moving_stage == "3")
            {
                msSQL = " update crm_trn_ttelelead2campaign set leadstage_gid='" + moving_stage + "'" +
                   "where leadbank_gid='" + values.leadbank_gid + "'and (leadstage_gid='1' or leadstage_gid='2' and leadstage_gid !='5') ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                //msSQL = " update crm_trn_tlead2campaign set leadstage_gid='3'" +
                //    "where leadbank_gid='" + values.leadbank_gid + "' and (leadstage_gid='0' or leadstage_gid is null) ";
                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msGetGid2 = objcmnfunctions.GetMasterGID("BSLC");
                msGetGids1 = objcmnfunctions.GetMasterGID("BLGP");
                msSQL = " insert into crm_trn_tschedulelog  (" +
                " schedulelog_gid, " +
                " leadbank_gid," +
                 " schedule_type, " +
                  " status_flag, " +
                  " log_gid, " +
                  " created_by, " +
                  " schedule_date, " +
                  " schedule_time, " +
                  " created_date ) " +
                " values (" +
               " '" + msGetGid2 + "', " +
               " '" + values.leadbank_gid + "', " +
               " 'Meeting'," +
               " 'N'," +
               "'" + msGetGids1 + "'," +
               "'" + user_gid + "'," +
               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                "'" + DateTime.Now.ToString("HH:mm") + "'," +
               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            }
            else if (moving_stage != "3")
            {
                msSQL = " update crm_trn_ttelelead2campaign set leadstage_gid='" + moving_stage + "'" +
               "where leadbank_gid='" + values.leadbank_gid + "'and  leadstage_gid !='5' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            else
            {
                msSQL = " update crm_trn_tlead2campaign set Pending_call='Y' where leadbank_gid='" + values.leadbank_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }


            if (mnResult != 0)
            {
                values.status = true;
                values.message = " Call Recorded Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = " Error Occurs ";

            }
        }
        public void DaGetMycallstilescount(string employee_gid, MdlMyCalls values)
        {
            msSQL = "select(select count(a.schedulelog_gid) from crm_trn_tschedulelog a left join" +
                    " crm_trn_ttelelead2campaign x on x.leadbank_gid = a.leadbank_gid where a.schedule_date = DATE_FORMAT(CURDATE(), '%Y-%m-%d') and" +
                    " x.leadstage_gid != '5' and x.assign_to= '" + employee_gid + "') as schedule_count ," +
                    "(select count(x.leadbank_gid)  from crm_trn_tschedulelog a left join" +
                    " crm_trn_ttelelead2campaign x on x.leadbank_gid = a.leadbank_gid where a.schedule_date > DATE_FORMAT(CURDATE(), '%Y-%m-%d') and" +
                    " x.leadstage_gid != '5' and x.assign_to= '" + employee_gid + "') as upcomingschedule_count ," +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid " +
                    " where x.so_status <>'Y' and (x.leadstage_gid ='1') and x.assign_to= '"+ employee_gid + "' and " +
                    " x.leadbank_gid  not in ( select leadbank_gid from crm_trn_tcalllog) and b.leadbank_name is not null and  x.leadstage_gid != '5' ) as newleads_count ," +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid " +
                    " where x.so_status <>'Y' and (x.leadstage_gid ='2') and x.assign_to= '" + employee_gid + "') as followup_count ," +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid " +
                    " where x.so_status <>'Y' and (x.leadstage_gid ='7') and x.assign_to= '" + employee_gid + "') as prospect_count ," +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid " +
                    " where x.so_status <>'Y' and (x.leadstage_gid ='5') and x.assign_to= '" + employee_gid + "') as drop_count ," +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid " +
                    " where x.so_status <>'Y' and (x.leadstage_gid ='1') and x.leadbank_gid in ( select leadbank_gid from crm_trn_tcalllog) and x.assign_to= '" + employee_gid + "') as pending_count ," +
                    " (select count(leadstage_gid) from crm_trn_ttelelead2campaign where so_status<>'Y' and (leadstage_gid ='1' or leadstage_gid ='2' or leadstage_gid ='5'  or leadstage_gid ='7') and leadstage_gid is not null and assign_to = '" + employee_gid + "') as alllead_count";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<mycallstilescount_list>();

            if (dt_datatable.Rows.Count != 0)
            {


                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new mycallstilescount_list
                    {
                        schedule_count = dt["schedule_count"].ToString(),
                        newleads_count = dt["newleads_count"].ToString(),
                        followup_count = dt["followup_count"].ToString(),
                        prospect_count = dt["prospect_count"].ToString(),
                        drop_count = dt["drop_count"].ToString(),
                        pending_count = dt["pending_count"].ToString(),
                        alllead_count = dt["alllead_count"].ToString(),
                        upcomingschedule_count = dt["upcomingschedule_count"].ToString()



                    });
                    values.mycallstilescount_list = getModuleList;
                }
            }

            dt_datatable.Dispose();
        }
        public void DaGetMycallsresponsedropdown(MdlMyCalls values)
        {

            msSQL = " select concat(call_response ,' - ', a.leadstage_name)as call_response,callresponse_gid,moving_stage,callresponse_code from crm_mst_callresponse" +
                    " left join crm_mst_tleadstage a on a.leadstage_gid = moving_stage" +
                    " where moving_stage!= 4 and  moving_stage!= 6 and moving_stage!= 8 and active_flag='Y' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<mycallsresponse_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new mycallsresponse_list
                    {
                        call_response = dt["call_response"].ToString(),
                        moving_stage = dt["moving_stage"].ToString(),
                        callresponse_gid = dt["callresponse_gid"].ToString(),

                    });
                    values.mycallsresponse_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetMycallsresponsefollowupdropdown(MdlMyCalls values)
        {

            msSQL = " select concat(call_response ,' - ', a.leadstage_name)as call_response,callresponse_gid,moving_stage,callresponse_code from crm_mst_callresponse" +
                    " left join crm_mst_tleadstage a on a.leadstage_gid = moving_stage" +
                    " where moving_stage!= 1 and  moving_stage!= 8 and moving_stage!= 4 and  moving_stage!= 6 and active_flag='Y' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<mycallsresponse_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new mycallsresponse_list
                    {
                        call_response = dt["call_response"].ToString(),
                        moving_stage = dt["moving_stage"].ToString(),
                        callresponse_gid = dt["callresponse_gid"].ToString(),

                    });
                    values.mycallsresponse_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetAssignedteamdropdown(string employee_gid, MdlMyCalls values)
        {

            msSQL = " select a.campaign_gid,Concat (b.campaign_prefix,' / ',b.campaign_title) as campaign_title from crm_trn_tteleteam2employee a left join crm_trn_tteleteam b on a.campaign_gid = b.campaign_gid where a.employee_gid= '" + employee_gid + "' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<assignedteamdropdown_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new assignedteamdropdown_list
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),

                    });
                    values.assignedteamdropdown_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostmycallleadbank(string employee_gid, postleadbank_list values)
        {
            msSQL = " Select  region_name from crm_mst_tregion where  region_gid = '" + values.region_name + "'";
            string lsregion_name = objdbconn.GetExecuteScalar(msSQL);
            msSQL = " select customer_type from crm_mst_tcustomertype Where customertype_gid='" + values.customer_type + "'";
            string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);
            if (values.status != "Y")
            {
                lsstatus = 'Y';
            }

            if (values.addtocustomer != false)
            {
                lsaddtocustomer = 'Y';
            }
            msGetGid = objcmnfunctions.GetMasterGID("BMCC");
            msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");
            msSQL = " INSERT INTO crm_trn_tleadbank(" +
                    " leadbank_gid," +
                    " source_gid," +
                    " leadbank_id," +
                    " leadbank_name," +
                    " status," +
                    " company_website," +
                    " approval_flag, " +
                    " lead_status," +
                    " leadbank_code," +
                    " leadbank_state," +
                    " leadbank_address1," +
                    " leadbank_address2," +
                    " leadbank_region," +
                    " leadbank_country," +
                    " leadbank_city," +
                    " leadbank_pin," +
                    " lead_type," +
                    " customer_type," +
                    " customertype_gid," +
                    " created_by," +
                    " remarks," +
                    " categoryindustry_gid," +
                    " referred_by," +
                    " main_branch," +
                    " created_date)" +
                    " values(" +
                    " '" + msGetGid1 + "'," +
                    " '" + values.source_name + "'," +
                    " '" + msGetGid + "'," +
                    " '" + values.leadbank_name + "'," +
                    " ' Y'," +
                    " '" + values.company_website + "'," +
                    " 'Approved'," +
                    " 'Assigned'," +
                    " 'H.Q'," +
                    " '" + values.leadbank_state + "'," +
                    " '" + values.leadbank_address1 + "'," +
                    " '" + values.leadbank_address2 + "'," +
                    " '" + values.region_name + "'," +
                    " '" + values.country_name + "'," +
                    " '" + values.leadbank_city + "'," +
                    " '" + values.leadbank_pin + "'," +
                    " '" + values.lead_type + "'," +
                    " '" + values.customer_type+ "'," +
                    " '" + lscustomer_type + "'," +
                    " '" + employee_gid + "'," +
                    " '" + values.remarks + "'," +
                    " '" + values.categoryindustry_name + "'," +
                    " '" + values.referred_by + "'," +
                    " 'Y'," +
                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
              mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
            if (msGetGid2 == "E")
            {
                values.Status = false;
                values.message = "Create sequence code BLCC for Lead Bank";
            }
            msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                    " (leadbankcontact_gid," +
                    " leadbank_gid," +
                    " leadbankcontact_name," +
                    " email, " +
                    " mobile," +
                    " phone1," +
                    " country_code1," +
                    " area_code1," +
                    " phone2," +
                    " country_code2," +
                    " area_code2," +
                    " fax," +
                    " fax_country_code," +
                    " fax_area_code," +
                    " designation," +
                    " created_date," +
                    " created_by," +
                    " leadbankbranch_name, " +
                    " address1, " +
                    " address2, " +
                    " city, " +
                    " state, " +
                    " pincode, " +
                    " country_gid, " +
                    " region_name, " +
                    " main_contact)" +
                    " values( " +
                    " '" + msGetGid2 + "'," +
                    " '" + msGetGid1 + "'," +
                    " '" + values.leadbankcontact_name + "',"+
                    " '" + values.email + "',"+
                    " '" + values.phone.e164Number + "',"+
                    " '" + values.phone1 + "'," +
                    " '" + values.country_code1 + "'," +
                    " '" + values.area_code1 + "'," +
                    " '" + values.phone2 + "'," +
                    " '" + values.country_code2 + "'," +
                    " '" + values.area_code2 + "'," +
                    " '" + values.fax + "'," +
                    " '" + values.fax_country_code + "'," +
                    " '" + values.fax_area_code + "',"+
                    " '" + values.designation + "',"+
                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    " '" + employee_gid + "'," +
                    " 'H.Q'," +
                    " '" + values.leadbank_address1 + "'," +
                    " '" + values.leadbank_address2 + "'," +
                    " '" + values.leadbank_city + "'," +
                    " '" + values.leadbank_state + "'," +
                    " '" + values.leadbank_pin + "'," +
                    " '" + values.country_gid + "'," +
                    " '" + lsregion_name + "'," +
                    " 'Y'" + ")";
            mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
            msGetGid3 = objcmnfunctions.GetMasterGID("BLCC");
            msSQL = " Insert into crm_trn_ttelelead2campaign ( " +
                    " lead2campaign_gid, " +
                    " leadbank_gid, " +
                    " campaign_gid, " +
                    " created_by, " +
                    " created_date, " +
                    " lead_status, " +
                    " internal_notes, " +
                    " leadstage_gid, " +
                    " assign_to ) " +
                    " Values ( " +
                    "'" + msGetGid3 + "'," +
                    "'" + msGetGid1 + "'," +
                    "'" + values.team_name + "'," +
                    "'" + employee_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    "'Open'," +
                    "'" + values.remarks + "'," +
                    "'1'," +
                    "'" + employee_gid + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            msGetGid7 = objcmnfunctions.GetMasterGID("APMT");
            msSQL = " Insert into crm_trn_tappointment ( " +
               " appointment_gid, " +
               " leadbank_gid, " +
               " created_by, " +
               " created_date, " +
               " internal_notes, " +
               " leadstage_gid, " +
               " assign_to ) " +
               " Values ( " +
               "'" + msGetGid7 + "'," +
               "'" + msGetGid1 + "'," +
               "'" + employee_gid + "'," +
               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
               "'" + values.schedule_remarks + "'," +
               "'0'," +
               "'" + employee_gid + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult != 0)
            {
                values.Status = true;
                values.message = "Lead Added Successfully";
            }
            else
            {
                values.Status = false;
                values.message = "Error Occurred While Inserting Records";
            }
        }
        public void DaPostAppointmentmycalls(string user_gid, postappointmentmycalls_list values)
        {
            try
            {
                msSQL = " update crm_trn_tappointment set lead_title='" + values.lead_title + "',business_vertical='" + values.bussiness_verticle + "'," +
                   " appointment_date='" + values.appointment_timing + "',Leadstage_gid='1',assign_to='',created_by='" + user_gid + "', " +
                   " created_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where leadbank_gid='" + values.leadbank_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = " insert into crm_trn_tOpportunitylog ( " +
                            " appointment_gid, " +
                            " log_type, " +
                            " log_date, " +
                            " log_remarks, " +
                            " schedule_status, " +
                            " created_by, " +
                            " created_date ) " +
                            " values (  " +
                            "'" + msGetGid + "'," +
                            "'Opportunity'," +
                            "'" + values.appointment_timing + "'," +
                            "'" + values.lead_title + "'," +
                            "'Pending'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        msSQL = "update crm_trn_ttelelead2campaign set leadstage_gid='7', updated_by = '" + user_gid + "'," +
                                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " where leadbank_gid='" + values.leadbank_gid + "'";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult1 == 1)
                        {
                            values.status = true;
                            values.message = "Opportunity created successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error occurred while moving lead to opportunity";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error occurred while moving lead to opportunity";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error occurred while moving lead to opportunity";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********"+ "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                }
            }
            catch(Exception ex)
            {
                values.status = false;
                values.message = "Error occurred while moving lead to opportunity";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }
        public void DaPostaddtooportunity(string user_gid, postappointmentmycalls_list values)
        {

            msSQL = "select source_gid from crm_mst_tsource where source_name = 'Shopify Enquiry'";
            string source_gid = objdbconn.GetExecuteScalar(msSQL);

            if (string.IsNullOrEmpty(source_gid))
            {
                msGetGid = objcmnfunctions.GetMasterGID("BSEM");
                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='BSEM' order by finyear desc limit 0,1 ";
                string lsCode = objdbconn.GetExecuteScalar(msSQL);
                string lssource_code = "SCM" + "000" + lsCode;

                msSQL = " insert into crm_mst_tsource(" +
                        " source_gid," +
                        " source_code," +
                        " source_name," +
                        " created_by, " +
                        " created_date)" +
                        " values(" +
                        " '" + msGetGid + "'," +
                        " '" + lssource_code + "'," +
                        "'Shopify Enquiry'," +
                        "'" + user_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    lssource_gid = msGetGid;
                }
            }
            else
            {
                lssource_gid = source_gid;
            }
            msSQL = "select businessvertical_gid from crm_mst_tbusinessvertical where business_vertical = 'Online Products'";
            string businessvertical_gid = objdbconn.GetExecuteScalar(msSQL);

            if (string.IsNullOrEmpty(businessvertical_gid))
            {
                msGetGid = objcmnfunctions.GetMasterGID("BV");

                msSQL = " insert into crm_mst_tbusinessvertical(" +
                            " businessvertical_gid," +
                            " business_vertical," +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            " '" + msGetGid + "'," +
                            "'Online Products',";
                msSQL += "'" + user_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    lsbusinessvertical_gid = msGetGid;
                }
            }
            else
            {
                lsbusinessvertical_gid = businessvertical_gid;
            }
            msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + user_gid + "'";
            string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

            msGetGid = objcmnfunctions.GetMasterGID("BMCC");
            msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

            msSQL = " INSERT INTO crm_trn_tleadbank(" +
                    " leadbank_gid," +
                    " source_gid," +
                    " leadbank_id," +
                    " leadbank_name," +
                    " status," +
                    " approval_flag, " +
                    " lead_status," +
                    " leadbank_code," +
                    " created_by," +
                    " main_branch," +
                    " assign_to," +
                    " remarks," +
                    " created_date)" +
                    " values(" +
                    " '" + msGetGid1 + "'," +
                    " '" + lssource_gid + "'," +
                    " '" + msGetGid + "'," +
                    " '" + values.post_list.name + "'," +
                    " 'Y'," +
                    " 'Approved'," +
                    " 'Not Assigned'," +
                    " 'H.Q'," +
                    " '" + lsemployee_gid + "'," +
                    " 'Y'," +
                    " '" + values.employee_gid + "'," +
                    " '" + values.post_list.comment + "'," +
                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
          
            msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                " (leadbankcontact_gid," +
                " leadbank_gid," +
                " email," +
                " mobile," +
                " created_date," +
                " created_by," +
                " leadbankbranch_name, " +
                " main_contact)" +
                " values( " +
                " '" + msGetGid2 + "'," +
                " '" + msGetGid1 + "'," +
                " '" + values.post_list.email + "'," +
                " '" + values.post_list.phoneNumber + "'," +
                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                " '" + lsemployee_gid + "'," +
                " 'H.Q'," +
                " 'y'" + ")";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
          


                msGetGid = objcmnfunctions.GetMasterGID("APMT");
            msSQL = " insert into crm_trn_tappointment (" +
                             " appointment_gid," +
                             " lead_title, " +
                             " leadbank_gid, " +
                             " business_vertical, " +
                             " appointment_date, " +
                             " assign_to, " +
                             " campaign_gid, " +
                             " Leadstage_gid," +
                             " created_by," +
                             "created_date" +
                              ") values (" +
                             "'" + msGetGid + "', " +
                             "'" + values.lead_title + "'," +
                             "'" + msGetGid1 + "'," +
                             "'" + lsbusinessvertical_gid + "'," +
                             "'" + values.appointment_timing + "'," +
                             "'" + values.employee_gid + "'," +
                             "'" + values.teamname_gid + "'," +
                             "'" + "1" + "'," +
                              "'" + user_gid + "'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
               
                    msSQL = " insert into crm_trn_tOpportunitylog ( " +
                            " appointment_gid, " +
                            " log_type, " +
                            " log_date, " +
                            " log_remarks, " +
                            " created_by, " +
                            " created_date ) " +
                            " values (  " +
                            "'" + msGetGid + "'," +
                            "'Opportunity'," +
                            "'" + values.appointment_timing + "'," +
                            "'" + values.lead_title + "'," +
                            "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "update crm_trn_tgmailinbox set leadbank_gid='" + msGetGid1 + "' where s_no='" + values.post_list.s_no + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Assigned Successfully";
                    }
                    }
                    else
                    {
                    values.status = false;
                    values.message = "Error Occurred While Assigning Record";
                    }
         }
         
        }
        public void DaPostscheduleclose(string user_gid, Postscheduleclose_list values)//SCHEDULE CLOSE
        {
            try
            {
                msSQL = " update crm_trn_tschedulelog set " +
                     " schedule_status='Closed'," +
                     " updated_by= '" + user_gid + "', " +
                     " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                     " closing_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                     " status_flag='Y'," +
                     " schedule_remarks = '" + values.schedule_remarks + "'" +
                     " where schedulelog_gid='" + values.schedulelog_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Schedule Closed Sucessfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred While Closing Schedule";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                }

            }
            catch(Exception ex)
            {
                values.status = false;
                values.message = "Error Occurred While Closing Schedule!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");



            }

        }
        public void DaPostschedulepostpone(string user_gid, Postschedulepostpone_list values)//SCHEDULE POSTPONE
        {
            try
            {
                msSQL = " update crm_trn_tschedulelog set " +
                  " schedule_date= '" + values.postponed_date + "', " +
                  " schedule_time='" + values.meeting_time_postponed + "', " +
                  " schedule_remarks = '" + values.schedule_remarks + "'," +
                  " updated_by = '" + user_gid + "'," +
                  " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where schedulelog_gid='" + values.schedulelog_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Schedule Postpone Sucessfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred While Postpone Schedule";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + "******" +
                        "*****" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                }
            }
           catch(Exception ex)
            {
                values.status = false;
                values.message = "Error Occurred While Postpone Schedule!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }
        public void DaPostscheduledrop(string user_gid, Postscheduledrop_list values)//SCHEDULE DROP
        {
            try
            {

                msSQL = " update crm_trn_ttelelead2campaign set leadstage_gid='5',drop_remarks='" + values.drop_reason + "', updated_by = '" + user_gid + "'," +
                  " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                            " where leadbank_gid='" + values.leadbank_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = " DELETE FROM crm_trn_tschedulelog WHERE schedulelog_gid ='" + values.schedulelog_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Schedule Drop Sucessfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Drop Schedule";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred While Drop Schedule";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                }

            }
           catch(Exception ex)
            {
                values.status = false;
                values.message = "Error Occurred While Drop Schedule!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
       public void DaGetCallLogLead(string leadbank_gid, MdlMyCalls values)
        {
            try
            {
                msSQL = "select calllog_gid,leadbank_gid,mobile_number,CONCAT_ws(' / ',d.leadstage_name,e.call_response) AS call_response,prospective_percentage,remarks,c.productgroup_name,b.product_name,DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date " +
                    "from crm_trn_tcalllog a  left join pmr_mst_tproduct b on b.product_gid=a.product_gid" +
                    " left join pmr_mst_tproductgroup c on c.productgroup_gid=b.productgroup_gid " +
                    "left join crm_mst_tleadstage d on d.leadstage_gid=a.moving_stage " +
                    "left join crm_mst_callresponse e on e.callresponse_gid= a.call_response where a.leadbank_gid ='" + leadbank_gid + "' order by d.leadstage_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCallLogLead_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCallLogLead_list
                        {

                            calllog_gid = dt["calllog_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            mobile_number = dt["mobile_number"].ToString(),
                            call_response = dt["call_response"].ToString(),
                            prospective_percentage = dt["prospective_percentage"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.GetCallLogLead_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Telecaller Manager Drop Remarks!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}