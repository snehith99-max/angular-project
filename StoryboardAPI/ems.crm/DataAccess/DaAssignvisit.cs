using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.crm.Models;

using static ems.crm.Models.MdlAssignvisit;
namespace ems.crm.DataAccess
{
    public class DaAssignvisit
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader  objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetassignvisitSummary(MdlAssignvisit values)
        {
            msSQL = " select a.assign_to,a.schedulelog_gid,b.leadbank_region,b.leadbank_gid,a.schedule_remarks,concat(h.user_firstname,'-',h.user_lastname) as assignto, " +
                " cast(concat(a.schedule_date,' ', a.schedule_time) as datetime) as schedule," +
                " concat(c.leadbankcontact_name,' / ',c.mobile,' / ',c.email) as contact_details,concat(f.user_firstname,'  ',f.user_lastname)as updated_by," +
                " concat(b.leadbank_address1,'/',b.leadbank_address2,'/',b.leadbank_city,'/',b.leadbank_state,'-',b.leadbank_pin)as customer_address," +
                 "concat(a.schedule_date, '', a.schedule_time) as schedule_dateandtime," +
                " b.leadbank_name,concat(d.region_name,'/',e.source_name) as region_source,a.schedule_type,concat(a.schedule_date, '/', a.schedule_time) as schedule_date,a.schedule_remarks  from crm_trn_tschedulelog a " +
                " inner join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                " inner join crm_trn_tleadbankcontact c on b.leadbank_gid = c.leadbank_gid " +
                " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                " left join adm_mst_tuser f on f.user_gid=a.created_by " +
                " left join hrm_mst_temployee g on g.employee_gid = a.assign_to " +
                " left join adm_mst_tuser h on h.user_gid = g.user_gid " +
                " where a.schedule_type='Meeting' and c.main_contact ='Y' " +
                " and c.status='Y' and a.assign_to!='' group by a.leadbank_gid order by b.leadbank_name asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<assignvisit_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new assignvisit_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        schedulelog_gid = dt["schedulelog_gid"].ToString(),
                        customer_address = dt["customer_address"].ToString(),
                        //leadbank_region = dt["leadbank_region"].ToString(),
                        schedule_type = dt["schedule_type"].ToString(),
                        schedule_dateandtime = dt["schedule_dateandtime"].ToString(),
                        schedule_remarks = dt["schedule_remarks"].ToString(),
                        assign_to = dt["assignto"].ToString(),
                        updated_by = dt["updated_by"].ToString(),
                        region_source = dt["region_source"].ToString(),
                        schedule_date = dt["schedule_date"].ToString(),


                    });
                    values.assignvisitlist = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetassignvisitSummaryToday(MdlAssignvisit values)
        {
            msSQL = " select a.assign_to,a.schedulelog_gid,b.leadbank_region,b.leadbank_gid,a.schedule_remarks,concat(h.user_firstname,'-',h.user_lastname) as assignto, " +
                " cast(concat(a.schedule_date,' ', a.schedule_time) as datetime) as schedule," +
                " concat(c.leadbankcontact_name,' / ',c.mobile,' / ',c.email) as contact_details,concat(f.user_firstname,'  ',f.user_lastname)as updated_by," +
                " concat(b.leadbank_address1,'/',b.leadbank_address2,'/',b.leadbank_city,'/',b.leadbank_state,'-',b.leadbank_pin)as customer_address," +
                 "concat(a.schedule_date, '', a.schedule_time) as schedule_dateandtime," +
                " b.leadbank_name,concat(d.region_name,'/',e.source_name) as region_source,a.schedule_type,concat(a.schedule_date, '/', a.schedule_time) as schedule_date,a.schedule_remarks  from crm_trn_tschedulelog a " +
                " inner join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                " inner join crm_trn_tleadbankcontact c on b.leadbank_gid = c.leadbank_gid " +
                " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                " left join adm_mst_tuser f on f.user_gid=a.created_by " +
                " left join hrm_mst_temployee g on g.employee_gid = a.assign_to " +
                " left join adm_mst_tuser h on h.user_gid = g.user_gid " +
                " where a.schedule_type='Meeting' and a.schedule_date = DATE_FORMAT(CURDATE(), '%Y-%m-%d') " +
                " and c.status='Y' and c.main_contact ='Y'and a.assign_to is null group by a.leadbank_gid order by b.leadbank_name asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<assignvisit_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new assignvisit_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        schedulelog_gid = dt["schedulelog_gid"].ToString(),
                        region_source = dt["region_source"].ToString(),
                        customer_address = dt["customer_address"].ToString(),
                        schedule_date = dt["schedule_date"].ToString(),
                        //leadbank_region = dt["leadbank_region"].ToString(),
                        schedule_type = dt["schedule_type"].ToString(),
                        schedule_dateandtime = dt["schedule_dateandtime"].ToString(),
                        schedule_remarks = dt["schedule_remarks"].ToString(),
                        assign_to = dt["assignto"].ToString(),
                        updated_by = dt["updated_by"].ToString(),



                    });
                    values.assignvisitlist = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetassignvisitSummaryUpcoming(MdlAssignvisit values)
        {
            msSQL = " select a.assign_to,a.schedulelog_gid,b.leadbank_region,b.leadbank_gid,a.schedule_remarks,concat(h.user_firstname,'-',h.user_lastname) as assignto, " +
                " cast(concat(a.schedule_date,' ', a.schedule_time) as datetime) as schedule," +
                " concat(c.leadbankcontact_name,' / ',c.mobile,' / ',c.email) as contact_details,concat(f.user_firstname,'  ',f.user_lastname)as updated_by," +
                " concat(b.leadbank_address1,'/',b.leadbank_address2,'/',b.leadbank_city,'/',b.leadbank_state,'-',b.leadbank_pin)as customer_address," +
                 "concat(a.schedule_date, '', a.schedule_time) as schedule_dateandtime," +
                " b.leadbank_name,concat(d.region_name,'/',e.source_name) as region_source,a.schedule_type,concat(a.schedule_date, '/', a.schedule_time) as schedule_date,a.schedule_remarks  from crm_trn_tschedulelog a " +
                " inner join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                " inner join crm_trn_tleadbankcontact c on b.leadbank_gid = c.leadbank_gid " +
                " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                " left join adm_mst_tuser f on f.user_gid=a.created_by " +
                " left join hrm_mst_temployee g on g.employee_gid = a.assign_to " +
                " left join adm_mst_tuser h on h.user_gid = g.user_gid " +
                " where a.schedule_type='Meeting' and a.schedule_date > DATE_FORMAT(CURDATE(), '%Y-%m-%d') " +
                " and c.status='Y' and c.main_contact ='Y' and a.assign_to is null group by a.leadbank_gid order by b.leadbank_name asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<assignvisit_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new assignvisit_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        schedulelog_gid = dt["schedulelog_gid"].ToString(),

                        customer_address = dt["customer_address"].ToString(),

                        //leadbank_region = dt["leadbank_region"].ToString(),
                        schedule_type = dt["schedule_type"].ToString(),
                        schedule_dateandtime = dt["schedule_dateandtime"].ToString(),
                        schedule_remarks = dt["schedule_remarks"].ToString(),
                        assign_to = dt["assignto"].ToString(),
                        updated_by = dt["updated_by"].ToString(),
                        region_source = dt["region_source"].ToString(),
                        schedule_date = dt["schedule_date"].ToString(),



                    });
                    values.assignvisitlist = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }


        public void DaGetassignvisitSummaryExpired(MdlAssignvisit values)
        {
            msSQL = " select a.assign_to,a.schedulelog_gid,b.leadbank_region,b.leadbank_gid,a.schedule_remarks,concat(h.user_firstname,'-',h.user_lastname) as assignto, " +
                " cast(concat(a.schedule_date,' ', a.schedule_time) as datetime) as schedule," +
                " concat(c.leadbankcontact_name,' / ',c.mobile,' / ',c.email) as contact_details,concat(f.user_firstname,'  ',f.user_lastname)as updated_by," +
                " concat(b.leadbank_address1,'/',b.leadbank_address2,'/',b.leadbank_city,'/',b.leadbank_state,'-',b.leadbank_pin)as customer_address," +
                 "concat(a.schedule_date, '/', a.schedule_time) as schedule_dateandtime," +
                " b.leadbank_name,concat(d.region_name,'/',e.source_name) as region_source,a.schedule_type,concat(a.schedule_date, '/', a.schedule_time) as schedule_date,a.schedule_remarks  from crm_trn_tschedulelog a " +
                " inner join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                " inner join crm_trn_tleadbankcontact c on b.leadbank_gid = c.leadbank_gid " +
                " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                " left join adm_mst_tuser f on f.user_gid=a.created_by " +
                " left join hrm_mst_temployee g on g.employee_gid = a.assign_to " +
                " left join adm_mst_tuser h on h.user_gid = g.user_gid " +
                " where a.schedule_type='Meeting' and a.schedule_date < DATE_FORMAT(CURDATE(), '%Y-%m-%d') " +
                " and c.status='Y'and c.main_contact ='Y' and a.assign_to is null group by a.leadbank_gid order by b.leadbank_name asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<assignvisit_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new assignvisit_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        schedulelog_gid = dt["schedulelog_gid"].ToString(),

                        customer_address = dt["customer_address"].ToString(),

                        //leadbank_region = dt["leadbank_region"].ToString(),
                        schedule_type = dt["schedule_type"].ToString(),
                        schedule_dateandtime = dt["schedule_dateandtime"].ToString(),
                        schedule_remarks = dt["schedule_remarks"].ToString(),
                        assign_to = dt["assignto"].ToString(),
                        updated_by = dt["updated_by"].ToString(),
                        region_source = dt["region_source"].ToString(),
                        schedule_date = dt["schedule_type"].ToString(),



                    });
                    values.assignvisitlist = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetmarketingteamdropdown(MdlAssignvisit values)
        {
            msSQL = " select campaign_gid,campaign_title from crm_trn_tcampaign";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<marketingteamdropdown_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new marketingteamdropdown_list
                    {
                        campaign_title = dt["campaign_title"].ToString(),
                        campaign_gid = dt["campaign_gid"].ToString(),
                    });
                    values.marketingteamdropdown_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetleadbankdropdown(MdlAssignvisit values)
        {
            msSQL = " select leadbank_gid,leadbank_name from crm_trn_tleadbank";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<lead_dropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new lead_dropdown
                    {
                        leadbank_name = dt["leadbank_name"].ToString(),
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                    });
                    values.lead_dropdown = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetexecutedropdown(string user_gid, string campaign_gid, MdlAssignvisit values)
        {
            msSQL = "select a.employee_gid, concat(b.user_firstname,' ',b.user_lastname) as executive " +
                 "From crm_trn_tcampaign2employee a " +
                 "Left Join hrm_mst_temployee c on a.employee_gid=c.employee_gid " +
                 "Left Join adm_mst_tuser b on c.user_gid=b.user_gid " +
                 "where a.campaign_gid ='" + campaign_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getexecutedropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getexecutedropdown
                    {
                        employee_gid = dt["employee_gid"].ToString(),
                        executive = dt["executive"].ToString(),
                    });
                    values.Getexecutedropdown = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetmarketingteamdropdownonchange(string campaign_gid, MdlAssignvisit values)
        {
            msSQL = "select a.employee_gid, concat(b.user_firstname,' ',b.user_lastname) as executive " +
                "From crm_trn_tcampaign2employee a " +
                "Left Join hrm_mst_temployee c on a.employee_gid=c.employee_gid " +
                "Left Join adm_mst_tuser b on c.user_gid=b.user_gid " +
                "where a.campaign_gid ='" + campaign_gid + "' ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getexecutedropdown>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getexecutedropdown
                    {

                        employee_gid = dt["employee_gid"].ToString(),
                        executive = dt["executive"].ToString(),

                    });
                    values.Getexecutedropdown = getModuleList;
                }
            }



        }

       
        public void DaGetAssignassignvisit(string employee_gid, assignvisitsubmit_list values)
        {

                msSQL = " update crm_trn_tschedulelog set " +
                        " assign_to = '" + values.executive + "'," +
                        "schedule_status = 'Pending'," +
                        " schedule_remarks = '" + values.schedule_remarks + "'" +
                        " where schedulelog_gid='" + values.schedulelog_gid + "'  ";
                     mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

               msSQL = "select leadbank_gid from crm_trn_tlead2campaign where crm_trn_tlead2campaign='" + values.leadbank_gid + "'";
               string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);
               
               if (lsleadbank_gid != "")
               {

                msSQL = " update crm_trn_tlead2campaign set " +
                        "campaign_gid = '" + values.campaign_gid + "'," +
                        " assign_to = '" + values.executive + "'," +
                        "leadstage_gid = '3'" +
                        " where leadbank_gid = '" + values.leadbank_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Assigned Sucessfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Occured Assigning";
                }

               }
               else
               {
                msSQL = " update crm_trn_tleadbank Set " +
                           " lead_status = 'Assigned' " +
                           " where leadbank_gid = '" + values.leadbank_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("BLCC");

                    msSQL = " Insert into crm_trn_tlead2campaign ( " +
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
                            "'" + msGetGid + "'," +
                            "'" + values.leadbank_gid + "'," +
                            "'" + values.campaign_gid + "'," +
                            "'" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            "'Open'," +
                            "'" + values.schedule_remarks + "'," +
                            "'3'," +
                            "'" + values.executive + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Lead Assigned Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Assigning Lead";
                    }
                }
            }
                
        }
        public void DaGetAssignleadvisit(string user_gid, assignvisitsubmit_list values)
        {

            msSQL = " update crm_trn_tschedulelog set " +
             //" assign_to = '" + values.summary_list[i].executive + "' " +
             " assign_to = '" + values.executive + "'," +
             "schedule_status = 'Pending'," +
            " schedule_remarks = '" + values.schedule_remarks + "'" +
            " where schedulelog_gid='" + values.schedulelog_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            msSQL = " update crm_trn_tlead2campaign set " +
                "campaign_gid = '" + values.campaign_gid + "'," +
                " assign_to = '" + values.executive + "'," +
                "leadstage_gid = '3'" +
                " where leadbank_gid = '" + values.leadbank_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Assigned Sucessfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Occured Assigning";
            }
        }
        public void DaGetvisittilecount(MdlAssignvisit values)
        {
            msSQL = " select (select count(distinct b.leadbank_gid) from crm_trn_tschedulelog b where b.schedule_date = DATE_FORMAT(CURDATE(), '%Y-%m-%d') " +
                " and b.schedule_type='Meeting' and b.assign_to is null ) as todayvisit ,(select count(distinct b.leadbank_gid) from crm_trn_tschedulelog b where b.schedule_date > DATE_FORMAT(CURDATE(), '%Y-%m-%d') " +
                "  and b.schedule_type='Meeting' and b.assign_to is null) as upcoming,(select count(distinct b.leadbank_gid) from crm_trn_tschedulelog b where b.schedule_date < DATE_FORMAT(CURDATE(), '%Y-%m-%d') " +
                " and b.schedule_type='Meeting' and b.assign_to is null) as expired,(select count(distinct b.leadbank_gid) from crm_trn_tschedulelog b where b.schedule_type='Meeting' and b.assign_to!='') as totalvisit";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<visittilecount_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new visittilecount_list
                    {
                        todayvisit = dt["todayvisit"].ToString(),
                        upcoming = dt["upcoming"].ToString(),
                        expired = dt["expired"].ToString(),
                        totalvisit = dt["totalvisit"].ToString(),

                        



                    });
                    values.visittilecount_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
    }
}