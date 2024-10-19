using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;


namespace ems.sales.DataAccess
{
    public class DaSmrTrnMyEnquiry
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsemployeegid, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        private string display_field;
        private string tax1_gid;
        private string tax2_gid;
        private string tax3_gid;

        public void DaGetSmrTrnMyEnquiry(string employee_gid, MdlSmrTrnMyEnquiry values)
        {
            try
            {
               
            msSQL = " Select b.leadbank_gid,a.customer_gid,mm.customer_type,a.enquiry_gid,b.leadbank_id,b.leadbank_name,b.leadbank_code,d.leadbankcontact_name,b.source_gid," +
                " concat(d.leadbankcontact_name,' / ',d.mobile,' / ',d.email) as contact_details,g.schedule_date,concat(h.user_firstname,'-',h.user_lastname) as name," +
                " date(g.schedule_date) as schedule_date, g.schedule_remarks, z.leadstage_name,g.schedule_type, " +
                " Case when a.internal_notes is not null then a.internal_notes  when a.internal_notes is null then b.remarks  end as internal_notes," +
                " e.region_name,a.lead2campaign_gid,f.campaign_title,d.leadbankcontact_gid " +
                " From crm_trn_tenquiry2campaign a " +
                " left join smr_trn_tsalesenquiry k on a.enquiry_gid=k.enquiry_gid " +
                " left join smr_trn_tsalesorder q on a.salesorder_gid=q.salesorder_gid " +
                " left join smr_trn_treceivequotation r on a.quotation_gid=r.quotation_gid " +
                " left join crm_trn_tleadbank b on k.customer_gid = b.customer_gid " +
                " left join crm_trn_tleadbankcontact d on b.leadbank_gid = d.leadbank_gid " +
                " left join crm_mst_tregion e on b.leadbank_region = e.region_gid " +
                " left join crm_mst_tsource l on b.source_gid=l.source_gid " +
                " left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid " +
                " left join crm_trn_tenquiryschedulelog g on g.enquiry_gid=a.enquiry_gid " +
                " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                " left join hrm_mst_temployee p on p.employee_gid=a.assign_to " +
                " left join adm_mst_tuser h on h.user_gid=p.user_gid " +
                " left join crm_mst_tcustomer j on k.customer_gid=j.customer_gid " +
                " left join crm_mst_tcustomertype m on j.customer_type=m.customertype_gid " +
             "   where a.assign_to='" + employee_gid + "' and (k.enquiry_date='"+ DateTime.Now.ToString("yyyy-MM-dd") + "')  and d.status = 'Y' and d.main_contact = 'Y' and a.leadstage_gid in ('3','4','5','8') ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<task_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new task_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        leadbank_code = dt["leadbank_code"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        leadstage_name = dt["leadstage_name"].ToString(),
                        schedule_type = dt["schedule_type"].ToString(),
                        schedule_date = dt["schedule_date"].ToString(),
                        name = dt["name"].ToString(),
                        schedule_remarks = dt["schedule_remarks"].ToString()



                    });
                    values.task_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading My Enquiry Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }
        public void DaGetSmrTrnMyEnquirynew(string employee_gid, MdlSmrTrnMyEnquiry values)
        {
            try
            {
                DateTime today = DateTime.Today;
                DayOfWeek dayOfWeek = today.DayOfWeek;

                int offset = 0;
                int offset_l = 0;

                switch (dayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        offset = 0;
                        break;
                    case DayOfWeek.Monday:
                        offset = 5;
                        break;
                }

                DateTime currentdate = today.AddDays(offset);
                DateTime fivedaysafter = today.AddDays(7 + offset);


            msSQL = " Select b.leadbank_gid,a.customer_gid,m.customer_type,a.enquiry_gid,date_format(a.created_date,'%d-%m-%y') as created_date,b.leadbank_id,b.leadbank_name,b.leadbank_code,d.leadbankcontact_name,b.source_gid," +
            " concat(d.leadbankcontact_name,' / ',d.mobile,' / ',d.email) as contact_details,concat(h.user_firstname,'-',h.user_lastname) as name ,z.leadstage_name, " +
            " Case when a.internal_notes is not null then a.internal_notes  when a.internal_notes is null then b.remarks  end as internal_notes, " +
            " e.region_name,a.lead2campaign_gid,f.campaign_title,d.leadbankcontact_gid " +
            " From crm_trn_tenquiry2campaign a " +
            " inner join smr_trn_tsalesenquiry k on a.enquiry_gid=k.enquiry_gid " +
            " inner join crm_trn_tleadbank b on k.customer_gid = b.customer_gid " +
            " left join crm_trn_tleadbankcontact d on b.leadbank_gid = d.leadbank_gid " +
            " left join crm_mst_tregion e on b.leadbank_region = e.region_gid " +
            " left join crm_mst_tsource l on b.source_gid=l.source_gid " +
            " left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid " +
            " left join crm_mst_tcustomer j on k.customer_gid=j.customer_gid " +
            " left join hrm_mst_temployee g on g.employee_gid=a.assign_to " +
            " left join adm_mst_tuser h on h.user_gid=g.user_gid " +
            " left join crm_mst_tcustomertype m on j.customer_type=m.customertype_gid " +
            " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
            " where a.assign_to  in ('" + employee_gid + "') and  k.enquiry_date='"+ DateTime.Now.ToString("yyyy-MM-dd")  +"' and  d.status = 'Y' and d.main_contact = 'Y' " +
            "  order by b.leadbank_gid desc";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<new_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new new_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        leadbank_code = dt["leadbank_code"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        created_date = dt["created_date"].ToString(),
                        name = dt["name"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        leadstage_name = dt["leadstage_name"].ToString(),



                    });
                    values.new_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading My Enqiry New !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        public void DaGetSmrTrnMyEnquiryProspect(string employee_gid, MdlSmrTrnMyEnquiry values)
        {
            try
            {

            msSQL = " Select b.leadbank_gid,a.customer_gid,m.customer_type,a.enquiry_gid,b.leadbank_id,b.leadbank_name,b.leadbank_code,d.leadbankcontact_name,b.source_gid," +
                " concat(d.leadbankcontact_name,' / ',d.mobile,' / ',d.email) as contact_details,g.schedule_date,concat(h.user_firstname,'-',h.user_lastname) as name," +
                " date(g.schedule_date) as schedule_date, g.schedule_remarks, z.leadstage_name,g.schedule_type, " +
                " Case when a.internal_notes is not null then a.internal_notes  when a.internal_notes is null then b.remarks  end as internal_notes," +
                " e.region_name,a.lead2campaign_gid,f.campaign_title,d.leadbankcontact_gid " +
                " From crm_trn_tenquiry2campaign a " +
                " left join smr_trn_tsalesenquiry k on a.enquiry_gid=k.enquiry_gid " +
                " left join crm_trn_tleadbank b on k.customer_gid = b.customer_gid " +
                " left join crm_trn_tleadbankcontact d on b.leadbank_gid = d.leadbank_gid " +
                " left join crm_mst_tregion e on b.leadbank_region = e.region_gid " +
                " left join crm_mst_tsource l on b.source_gid=l.source_gid " +
                " left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid " +
                " left join crm_trn_tenquiryschedulelog g on g.enquiry_gid=a.enquiry_gid " +
                " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                " left join hrm_mst_temployee p on p.employee_gid=a.assign_to " +
                " left join adm_mst_tuser h on h.user_gid=p.user_gid " +
                 " left join crm_mst_tcustomer j on k.customer_gid=j.customer_gid " +
                 " left join crm_mst_tcustomertype m on j.customer_type=m.customertype_gid " +
                " where a.assign_to  in ('" + employee_gid + "') and d.status='Y' and d.main_contact='Y' and " +
               " a.leadstage_gid ='3' and so_status <>'Y' order by b.leadbank_gid  desc ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<prospect_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new prospect_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        leadbank_code = dt["leadbank_code"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        schedule_type = dt["schedule_type"].ToString(),
                        schedule_date = dt["schedule_date"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        name = dt["name"].ToString()



                    });
                    values.prospect_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading My Enquiry Prospect !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }
        public void DaGetSmrTrnMyEnquiryPotential(string employee_gid, MdlSmrTrnMyEnquiry values)
        {
            try
            {

            msSQL = " Select b.leadbank_gid,a.customer_gid,m.customer_type,a.enquiry_gid,date_format(a.updated_date,'%d-%m-%y') as updated_date,b.leadbank_id,b.leadbank_name,b.leadbank_code,d.leadbankcontact_name," +
                  " Case when a.internal_notes is not null then a.internal_notes  when a.internal_notes is null then b.remarks  end as internal_notes,b.remarks, " +
                  " e.region_name,a.lead2campaign_gid,f.campaign_title,concat(d.leadbankcontact_name,' /',d.mobile,' /',d.email) as contact_details,d.leadbankcontact_gid,concat(h.user_firstname,'-',h.user_lastname) as name  " +
                  " From crm_trn_tenquiry2campaign a " +
                  " left join smr_trn_tsalesenquiry k on a.enquiry_gid=k.enquiry_gid " +
                  " left join crm_trn_tleadbank b on a.customer_gid = b.customer_gid " +
                  " left join crm_trn_tleadbankcontact d on b.leadbank_gid = d.leadbank_gid " +
                  " left join crm_mst_tregion e on b.leadbank_region = e.region_gid " +
                  " left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid " +
                  " left join hrm_mst_temployee g on g.employee_gid = a.assign_to " +
                  " left join adm_mst_tuser h on h.user_gid = g.user_gid " +
                  " left join crm_mst_tsource l on b.source_gid=l.source_gid " +
                   " left join crm_mst_tcustomer j on a.customer_gid=j.customer_gid " +
                    " left join crm_mst_tcustomertype m on j.customer_type=m.customertype_gid " +
                  " where a.assign_to  in ('" + employee_gid + "')  and " +
                  " a.leadstage_gid ='4'  and so_status <>'Y' order by b.leadbank_gid  desc ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Potential_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Potential_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        leadbank_code = dt["leadbank_code"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        updated_date = dt["updated_date"].ToString(),
                        leadbankcontact_name = dt["contact_details"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        name = dt["name"].ToString(),
                        remarks = dt["remarks"].ToString()



                    });
                    values.Potential_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading My Enquiry Potential !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           

        }
        public void DaGetSmrTrnMyEnquiryCompleted(string employee_gid, MdlSmrTrnMyEnquiry values)
        {
            try
            {

            msSQL = " Select b.leadbank_gid,a.customer_gid,m.customer_type,o.customerbranch_name,a.enquiry_gid,date_format(a.updated_date,'%dd-%mm-%yyyy') as updated_date,b.leadbank_name,b.leadbank_code, d.leadbankcontact_name,b.leadbank_id, " +
               " e.region_name,a.so_status,a.lead2campaign_gid,f.campaign_title,d.leadbankcontact_gid, concat(d.leadbankcontact_name,' /',d.mobile,' /',d.email) as contact_details, " +
               " Case when a.internal_notes is not null then a.internal_notes  when a.internal_notes is null then b.remarks  end as internal_notes, " +
               " concat(h.user_firstname,'-',h.user_lastname) as name  " +
               " From crm_trn_tenquiry2campaign a " +
               " left join smr_trn_tsalesenquiry k on a.enquiry_gid=k.enquiry_gid " +
               " left join crm_trn_tleadbank b on a.customer_gid = b.customer_gid " +
               " left join crm_trn_tleadbankcontact d on b.leadbank_gid = d.leadbank_gid " +
               " left join crm_mst_tregion e on b.leadbank_region = e.region_gid " +
               " left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid " +
               " left join crm_mst_tsource l on b.source_gid=l.source_gid " +
               " left join hrm_mst_temployee g on g.employee_gid=a.assign_to " +
               " left join adm_mst_tuser h on h.user_gid = g.user_gid " +
                 " left join crm_mst_tcustomer j on a.customer_gid=j.customer_gid " +
                 " left join crm_mst_tcustomercontact o on a.customer_gid=o.customer_gid " +
                 "  left join crm_mst_tcustomertype m on j.customer_type=m.customertype_gid  " +
              " where a.assign_to  in ('" + employee_gid + "') and d.status='Y' and d.main_contact='Y' and " +
              " a.leadstage_gid ='8' order by b.leadbank_gid  desc ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Completed_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Completed_list
                    {
                        //leadstage_gid = dt["leadstage_gid"].ToString(),
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        leadbank_code = dt["leadbank_code"].ToString(),
                        updated_date = dt["updated_date"].ToString(),
                        leadbankcontact_name = dt["contact_details"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        name = dt["name"].ToString(),
                        customerbranch_name = dt["customerbranch_name"].ToString()



                    });
                    values.Completed_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading My Enquiry Complete !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            



        }
        public void DaGetSmrTrnMyEnquiryDrop(string employee_gid, MdlSmrTrnMyEnquiry values)
        {
            try
            {

            msSQL = " Select b.leadbank_gid,a.customer_gid,m.customer_type,a.enquiry_gid,date_format(a.updated_date,'%d-%m-%y') as updated_date,b.leadbank_name,b.leadbank_code, d.leadbankcontact_name,b.leadbank_id, " +
               " e.region_name,a.so_status,a.lead2campaign_gid,f.campaign_title,concat(d.leadbankcontact_name,' /',d.mobile,' /',d.email) as contact_details,d.leadbankcontact_gid ,concat(h.user_firstname,'-',h.user_lastname) as name ," +
               " Case when a.internal_notes is not null then a.internal_notes " +
               " when a.internal_notes is null then b.remarks  end as internal_notes,i.leadstage_name " +
               " From crm_trn_tenquiry2campaign a " +
               " left join smr_trn_tsalesenquiry k on a.enquiry_gid=k.enquiry_gid " +
               " left join crm_trn_tleadbank b on k.customer_gid = b.customer_gid " +
               " left join crm_trn_tleadbankcontact d on b.leadbank_gid = d.leadbank_gid " +
               " left join crm_mst_tregion e on b.leadbank_region = e.region_gid " +
               " left join crm_mst_tsource l on b.source_gid=l.source_gid " +
               " left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid " +
               " left join hrm_mst_temployee g on g.employee_gid=a.assign_to " +
               " left join adm_mst_tuser h on h.user_gid=g.user_gid " +
               " left join crm_mst_tenquiry i on i.leadstage_gid=a.dropped_stage" +
                " left join crm_mst_tcustomer j on k.customer_gid=j.customer_gid " +
                " left join crm_mst_tcustomertype m on j.customer_type=m.customertype_gid " +
               " where a.assign_to  in ('" + employee_gid + "') and d.status='Y' and d.main_contact='Y' and " +
               " (a.leadstage_gid='5' ) order by b.leadbank_gid desc  ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Drop_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Drop_list
                    {
                        //leadstage_gid = dt["leadstage_gid"].ToString(),
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        leadbank_code = dt["leadbank_code"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        updated_date = dt["updated_date"].ToString(),
                        leadbankcontact_name = dt["contact_details"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        name = dt["name"].ToString(),
                         leadstage_name = dt["leadstage_name"].ToString()
                        



                    });
                    values.Drop_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting My Enquiry Drop !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           

        }

        public void DaGetSmrTrnMyEnquiryAll(string employee_gid, MdlSmrTrnMyEnquiry values)
        {
            try
            {
            msSQL = " Select b.leadbank_gid,a.customer_gid,m.customer_type,a.enquiry_gid,b.leadbank_name,b.leadbank_code,d.leadbankcontact_name,b.leadbank_id, " +
                " Case when a.internal_notes is not null then a.internal_notes  when a.internal_notes is null then b.remarks  end as internal_notes, " +
                "  e.region_name,a.lead2campaign_gid,f.campaign_title,concat(d.leadbankcontact_name,' /',d.mobile,' /',d.email) as contact_details,d.leadbankcontact_gid,concat(h.user_firstname,'-',h.user_lastname) as name " +
                " From crm_trn_tenquiry2campaign a " +
                " left join smr_trn_tsalesenquiry k on a.enquiry_gid=k.enquiry_gid  " +
                " left join crm_trn_tleadbank b on k.customer_gid = b.customer_gid  " +
                " left join crm_trn_tleadbankcontact d on b.leadbank_gid = d.leadbank_gid  " +
                " left join crm_mst_tregion e on b.leadbank_region = e.region_gid " +
                " left join crm_mst_tsource l on b.source_gid=l.source_gid   " +
                " left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid  " +
                " left join hrm_mst_temployee g on g.employee_gid=a.assign_to    " +
                " left join adm_mst_tuser h on h.user_gid=g.user_gid    " +
                " left join crm_mst_tcustomer j on k.customer_gid=j.customer_gid " +
                " left join crm_mst_tcustomertype m on j.customer_type=m.customertype_gid " +
                " where a.assign_to  in  ('" + employee_gid + "') and d.status='Y'   " +
                " and d.main_contact='Y'  order by b.leadbank_gid  desc";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<All_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new All_list
                    {
                       
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        leadbank_code = dt["leadbank_code"].ToString(), 
                        leadbankcontact_name = dt["contact_details"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        name = dt["name"].ToString(),
                     




                    });
                    values.All_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading My Enquiry All !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            


        }
        public void DaGetMyenquiryCount ( string employee_gid, MdlSmrTrnMyEnquiry values)
        {
            string NowdateLabel = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
               

                msSQL = " select(Select  count( a.enquiry_gid) From crm_trn_tenquiry2campaign a  inner join smr_trn_tsalesenquiry k on " +
                    "a.enquiry_gid=k.enquiry_gid   inner join crm_trn_tleadbank b on k.customer_gid = b.customer_gid  left join crm_trn_tleadbankcontact d" +
                    " on b.leadbank_gid = d.leadbank_gid   left join crm_mst_tregion e on b.leadbank_region = e.region_gid  left join crm_mst_tsource l on " +
                    "b.source_gid=l.source_gid   left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid  left join hrm_mst_temployee g on " +
                    "g.employee_gid=a.assign_to  left join adm_mst_tuser h on h.user_gid=g.user_gid  where a.assign_to  in ('" + employee_gid + "') and " +
                    " k.enquiry_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and d.status = 'Y' and d.main_contact = 'Y'  ) as todaytask_count,  " +
                    "( Select count( a.enquiry_gid) From crm_trn_tenquiry2campaign a  left join smr_trn_tsalesenquiry k on a.enquiry_gid=k.enquiry_gid " +
                    " left join crm_trn_tleadbank b on k.customer_gid = b.customer_gid  left join crm_trn_tleadbankcontact d on b.leadbank_gid = d.leadbank_gid" +
                    " left join crm_mst_tregion e on b.leadbank_region = e.region_gid   left join crm_mst_tsource l on b.source_gid=l.source_gid " +
                    " left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid   left join crm_trn_tenquiryschedulelog g on g.enquiry_gid=a.enquiry_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid  left join hrm_mst_temployee p on p.employee_gid=a.assign_to " +
                    " left join adm_mst_tuser h on h.user_gid=p.user_gid  left join crm_mst_tcustomer j on k.customer_gid=j.customer_gid" +
                    "  where a.assign_to  in ('" + employee_gid + "') and d.status='Y' and d.main_contact='Y' and a.leadstage_gid ='3' and so_status <>'Y') as prospects_count, " +
                    " ( Select count( CASE WHEN a.quotation_gid IS NULL THEN k.enquiry_gid ELSE a.quotation_gid END)  From crm_trn_tenquiry2campaign a " +
                    "  left join smr_trn_tsalesenquiry k on a.enquiry_gid=k.enquiry_gid   left join crm_trn_tleadbank b on a.customer_gid = b.customer_gid  " +
                    "left join crm_trn_tleadbankcontact d on b.leadbank_gid = d.leadbank_gid  left join crm_mst_tregion e on b.leadbank_region = e.region_gid " +
                    " left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid   left join hrm_mst_temployee g on g.employee_gid = a.assign_to   " +
                    "left join adm_mst_tuser h on h.user_gid = g.user_gid   left join crm_mst_tsource l on b.source_gid=l.source_gid     " +
                    "  left join crm_mst_tcustomer j on a.customer_gid=j.customer_gid  where a.assign_to  in ('" + employee_gid + "') " +
                    "and d.status='Y' and d.main_contact='Y' and  a.leadstage_gid ='4'  and so_status <>'Y' ) as potential_count, " +
                    " (Select count( a.enquiry_gid)From crm_trn_tenquiry2campaign a  left join smr_trn_tsalesenquiry k on a.enquiry_gid=k.enquiry_gid " +
                    " left join crm_trn_tleadbank b on k.customer_gid = b.customer_gid  left join crm_trn_tleadbankcontact d on b.leadbank_gid = d.leadbank_gid" +
                    "  left join crm_mst_tregion e on b.leadbank_region = e.region_gid  left join crm_mst_tsource l on b.source_gid=l.source_gid " +
                    " left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid  left join hrm_mst_temployee g on g.employee_gid=a.assign_to " +
                    " left join adm_mst_tuser h on h.user_gid=g.user_gid  left join crm_mst_tenquiry i on i.leadstage_gid=a.dropped_stage " +
                    "left join crm_mst_tcustomer j on k.customer_gid=j.customer_gid  where a.assign_to  in ('" + employee_gid + "') and d.status='Y' and d.main_contact='Y' and a.leadstage_gid='5') as drop_count,  " +
                    " (Select  count(CASE WHEN a.quotation_gid IS NULL THEN k.enquiry_gid ELSE a.quotation_gid END) From crm_trn_tenquiry2campaign a  left join smr_trn_tsalesenquiry k on a.enquiry_gid=k.enquiry_gid  " +
                    "left join crm_trn_tleadbank b on a.customer_gid = b.customer_gid left join crm_trn_tleadbankcontact d on b.leadbank_gid = d.leadbank_gid " +
                    "left join crm_mst_tregion e on b.leadbank_region = e.region_gid  left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid " +
                    "left join crm_mst_tsource l on b.source_gid=l.source_gid  left join hrm_mst_temployee g on g.employee_gid=a.assign_to  " +
                    "left join adm_mst_tuser h on h.user_gid = g.user_gid  left join crm_mst_tcustomer j on a.customer_gid=j.customer_gid  where a.assign_to  in ('" + employee_gid + "')" +
                    " and d.status='Y' and d.main_contact='Y' and a.leadstage_gid ='8' ) as completed_count, ( Select count(a.enquiry_gid)  From crm_trn_tenquiry2campaign a " +
                    " left join smr_trn_tsalesenquiry k on a.enquiry_gid=k.enquiry_gid   left join crm_trn_tleadbank b on k.customer_gid = b.customer_gid   " +
                    "left join crm_trn_tleadbankcontact d on b.leadbank_gid = d.leadbank_gid   left join crm_mst_tregion e on b.leadbank_region = e.region_gid " +
                    " left join crm_mst_tsource l on b.source_gid=l.source_gid    left join smr_trn_tcampaign f on f.campaign_gid = a.campaign_gid  " +
                    " left join hrm_mst_temployee g on g.employee_gid=a.assign_to     left join adm_mst_tuser h on h.user_gid=g.user_gid   " +
                    "  left join crm_mst_tcustomer j on k.customer_gid=j.customer_gid  where a.assign_to  in  ('" + employee_gid + "') and d.status='Y'    and d.main_contact='Y') as allleads_count;";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var MyenquiryCount_list = new List<MyenquiryCount_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    MyenquiryCount_list.Add(new MyenquiryCount_list
                    {
                        todaytask_count = (dt["todaytask_count"].ToString()),
                        //newlead_count = (dt["newlead_count"].ToString()),
                        prospects_count = (dt["prospects_count"].ToString()),
                        potential_count = (dt["potential_count"].ToString()),
                        drop_count = (dt["drop_count"].ToString()),
                        completed_count = (dt["completed_count"].ToString()),
                        allleads_count = (dt["allleads_count"].ToString()),
                    });
                    values.MyenquiryCount_list = MyenquiryCount_list;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting My Enquiry Count !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }            
        }
    }
}


    
