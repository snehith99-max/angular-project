﻿using ems.crm.Models;
using ems.system.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Net.NetworkInformation;



namespace ems.crm.DataAccess
{
    public class DaMyLead
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        finance_cmnfunction objfinance = new finance_cmnfunction();
        string msSQL = string.Empty;
        OdbcDataReader  objOdbcDataReader ;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetPrivilege_gid, msGetModule2employee_gi,
         lssource_name, lsleadbank_name, lscategoryindustry_name, lscountry_name, lsregion_name,
            lsbankcontact, msGetGid, msGetGid1, msGetGid2, msGetGid3, msGetGid4, msGetGid5, msGetGid6,
            msGetGid7, msGetGid8, msGetGid9, msGetGid10, msGetGid11, msGetGid12, lscount;

        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, mnResult6, mnResult7, mnResult8, mnResult9, mnResult10, mnResult11, mnResult12, mnResult13, mnResult14;
        char lsstatus, lsaddtocustomer;
        public void DaGetMyleadsSummary(MdlMyLead values, string employee_gid)

        {

            try
            {
                 
                msSQL = " Select b.leadbank_name,b.potential_value, b.customer_type, k.campaign_title,i.schedule_status, i.schedule_type,i.schedule_remarks,i.schedulelog_gid," +
                " concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details," +
                " concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as region_name," +
                    "(a.internal_notes) as internal_notes,cast(concat(i.schedule_date,' ', i.schedule_time) as datetime) as schedule," +
                    " z.leadstage_name, a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid" +
                    " From crm_trn_tlead2campaign a" +
                    " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid        " +
                    " left join crm_mst_tregion d on b.leadbank_region=d.region_gid           " +
                    " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                    " left join crm_mst_tsource h on b.source_gid=h.source_gid                " +
                    " left join crm_trn_tcampaign k on a.campaign_gid=k.campaign_gid          " +
                    " left join crm_trn_tschedulelog i on a.leadbank_gid = i.leadbank_gid " +
                    " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where a.assign_to = '" + employee_gid + "' " +
                    " and i.schedule_date = DATE_FORMAT(CURDATE(), '%Y-%m-%d')" +
                    " and g.status='Y' and g.main_contact='Y' and i.assign_to=  '" + employee_gid + "' " +
                    " order by b.leadbank_name asc";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<myleads_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new myleads_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            schedule_type = dt["schedule_type"].ToString(),
                            schedule = dt["schedule"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            schedule_remarks = dt["schedule_remarks"].ToString(),
                            schedule_status = dt["schedule_status"].ToString(),
                            schedulelog_gid = dt["schedulelog_gid"].ToString(),
                            potential_value = dt["potential_value"].ToString(),

                        });

                        values.myleadslist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Today Schedule Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            
        }
        public void DaGetTodaySummary(MdlMyLead values, string employee_gid)
        {
            try
            {
                 
                msSQL = " select f.assign_to,b.potential_value,b.customer_type,a.log_gid,a.schedulelog_gid,b.leadbank_gid,f.lead2campaign_gid,a.schedule_remarks,a.schedule_status, " +
                " cast(concat(a.schedule_date,' ', a.schedule_time) as datetime) as schedule," +
               " concat(b.leadbank_address1,'/',b.leadbank_address2,'/',b.leadbank_city,'/',b.leadbank_state,'-',b.leadbank_pin)as customer_address," +
               " concat(c.leadbankcontact_name,' / ',c.mobile,' / ',c.email) as contact_details," +
               " b.leadbank_name,d.region_name,a.schedule_type,f.lead2campaign_gid,c.leadbankcontact_gid  from crm_trn_tschedulelog a " +
                " inner join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                " inner join crm_trn_tleadbankcontact c on b.leadbank_gid = c.leadbank_gid " +
                " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                " left join crm_trn_tlead2campaign f on b.leadbank_gid=f.leadbank_gid " +
                " where (a.schedule_type='Meeting') and a.schedule_date > curdate() " +
                " and a.assign_to ='" + employee_gid + "'" +
                " and c.status='Y' and c.main_contact='Y' order by b.leadbank_name asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Todayvisit_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Todayvisit_list1
                        {
                            leadbank_gid1 = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid1 = dt["lead2campaign_gid"].ToString(),
                            leadbank_name1 = dt["leadbank_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            contact_details1 = dt["contact_details"].ToString(),
                            customer_address1 = dt["customer_address"].ToString(),
                            region_name1 = dt["region_name"].ToString(),
                            schedule_type1 = dt["schedule_type"].ToString(),
                            schedule1 = dt["schedule"].ToString(),
                            schedulelog_gid1 = dt["schedulelog_gid"].ToString(),

                            schedule_remarks = dt["schedule_remarks"].ToString(),
                            schedule_status = dt["schedule_status"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                        });
                        values.Todayvisit_list1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Upcoming Schedule Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetNewSummary(string employee_gid, MdlMyLead values)
        {

            try
            {
                 
                msSQL = " select b.leadbank_name,b.potential_value, b.customer_type,k.campaign_title,b.customer_type, g.mobile  ," +
              " concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details," +
              " concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as regionname," +
              " (a.internal_notes) as internal_notes,z.leadstage_name," +
              " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid" +
              " from crm_trn_tlead2campaign a" +
              " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid        " +
              " left join crm_mst_tregion d on b.leadbank_region=d.region_gid           " +
              " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
              " left join crm_mst_tsource h on b.source_gid=h.source_gid                " +
              " left join crm_trn_tcampaign k on a.campaign_gid=k.campaign_gid          " +
              " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
              " where a.assign_to = '" + employee_gid + "' and a.pending_call is null" +
              " and (a.leadstage_gid = '1' or a.leadstage_gid is null)" +
              " and g.status = 'Y' and g.main_contact = 'Y' " +
              " order by b.leadbank_name asc";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<newSummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new newSummary_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            regionname = dt["regionname"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            dialed_number = dt["mobile"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                        });
                        values.newSummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting New Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }


        public void DaGetInprogressSummary(MdlMyLead values, string employee_gid)
        {
            try
            {
                 
                msSQL = "Select  b.leadbank_name,b.potential_value,b.customer_type," +
              "concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details,b.leadbank_gid,a.call_count, g.mobile  ," +
              "concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as region_name, " +
              "(a.internal_notes) as internal_notes,z.leadstage_name,k.campaign_title, " +
              "a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid " +
              "From crm_trn_tlead2campaign a " +
              "left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid  " +
              "left join crm_mst_tregion d on b.leadbank_region=d.region_gid  " +
              "left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
              "left join crm_mst_tsource h on b.source_gid=h.source_gid " +
              "left join crm_trn_tcampaign k on a.campaign_gid=k.campaign_gid  " +
              "left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
              "where a.assign_to= '" + employee_gid + "' " +
              " and a.leadstage_gid='3' and  g.status='Y' and g.main_contact='Y' order by b.leadbank_name asc";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<inprogress_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new inprogress_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            call_count = dt["call_count"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            dialed_number = dt["mobile"].ToString(),
                        });

                        values.inprogresslist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Prospect Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetPotentialSummary(MdlMyLead values, string employee_gid)
        {

            try
            {
                msSQL = " SELECT FORMAT(SUM(a.potential_value),2) as potential_value from crm_trn_tleadbank a" +
                        " left join crm_trn_tlead2campaign b on a.leadbank_gid = b.leadbank_gid " +
                        "  left join crm_trn_tcampaign c on c.campaign_gid = b.campaign_gid " +
                        " left join crm_trn_tleadbankcontact g on a.leadbank_gid = g.leadbank_gid " +
                        " where g.status='Y' and g.main_contact='Y' and  b.assign_to = '" + employee_gid + "' and b.leadstage_gid = '4'";

                string potential_value_count = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "Select  b.leadbank_name,format(b.potential_value,2)as potential_value,b.customer_type," +
                   "concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details,b.leadbank_gid,a.call_count, g.mobile  ," +
                   "concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as region_name, " +
                   "(a.internal_notes) as internal_notes,z.leadstage_name,k.campaign_title, " +
                   "a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid " +
                   "From crm_trn_tlead2campaign a " +
                   "left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid  " +
                   "left join crm_mst_tregion d on b.leadbank_region=d.region_gid  " +
                   "left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                   "left join crm_mst_tsource h on b.source_gid=h.source_gid " +
                   "left join crm_trn_tcampaign k on a.campaign_gid=k.campaign_gid  " +
                   "left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
                   "where a.assign_to= '" + employee_gid + "' " +
                   " and a.leadstage_gid='4' and  g.status='Y' and g.main_contact='Y' order by b.leadbank_name asc";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<potential_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new potential_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead2campaign_gid = dt["lead2campaign_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            call_count = dt["call_count"].ToString(),
                            dialed_number = dt["mobile"].ToString(),
                            potential_value_count = potential_value_count
                        });

                        values.potential_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Potential Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }


        public void DaGetCustomerSummary(MdlMyLead values, string employee_gid)
        {

            try
            {
                msSQL = " SELECT FORMAT(SUM(a.potential_value),2) as potential_value from crm_trn_tleadbank a" +
                       " left join crm_trn_tlead2campaign b on a.leadbank_gid = b.leadbank_gid " +
                       "  left join crm_trn_tcampaign c on c.campaign_gid = b.campaign_gid " +
                       " left join crm_trn_tleadbankcontact g on a.leadbank_gid = g.leadbank_gid " +
                       " where g.status='Y' and g.main_contact='Y' and  b.assign_to = '" + employee_gid + "' and b.leadstage_gid = '6'";
                string potential_value_count = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " Select b.leadbank_name,format(b.potential_value,2)as potential_value, k.campaign_title, i.schedule_type,b.customer_type,b.leadbank_gid,a.call_count, g.mobile  ," +
               " concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details," +
               " concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as region_name," +
               " (a.internal_notes) as internal_notes,cast(concat(i.schedule_date,' ', i.schedule_time) as datetime) as schedule," +
               " z.leadstage_name,a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid" +
                   " From crm_trn_tlead2campaign a" +
                   " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid        " +
                   " left join crm_mst_tregion d on b.leadbank_region=d.region_gid           " +
                   " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                   " left join crm_mst_tsource h on b.source_gid=h.source_gid                " +
                   " left join crm_trn_tcampaign k on a.campaign_gid=k.campaign_gid          " +
                   " left join crm_trn_tschedulelog i on a.leadbank_gid = i.leadbank_gid " +
                   " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where a.assign_to =  '" + employee_gid + "' " +
             " and (a.leadstage_gid ='6')" +
             " and g.status='Y' and g.main_contact='Y' " +
                   " order by b.leadbank_name asc";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<customer_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new customer_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            call_count = dt["call_count"].ToString(),
                            dialed_number = dt["mobile"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            potential_value_count = potential_value_count

                        });

                        values.customerlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
           
        }

        public void DaGetDropSummary(MdlMyLead values, string employee_gid)
        {

            try
            {
                 
                msSQL = "Select  b.leadbank_name,k.campaign_title,b.potential_value,a.drop_remarks,a.assign_to,b.customer_type,b.leadbank_gid,a.call_count," +
               "concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details, " +
               "concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as region_name, " +
               "(a.internal_notes) as internal_notes, z.leadstage_name, " +
               "a.lead2campaign_gid,a.lead_base, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid " +
               "From crm_trn_tlead2campaign a  " +
               "left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid  " +
               "left join crm_mst_tregion d on b.leadbank_region=d.region_gid  " +
               "left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid  " +
               "left join crm_mst_tsource h on b.source_gid=h.source_gid  " +
               "left join crm_trn_tcampaign k on a.campaign_gid=k.campaign_gid  " +
               "left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid " +
               "where a.assign_to= '" + employee_gid + "' and a.leadstage_gid='5' " +
               "and g.status='Y' and g.main_contact='Y' order by b.leadbank_name asc ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<drop_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new drop_list1
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            drop_stage = dt["lead_base"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            call_count = dt["call_count"].ToString(),
                            drop_remarks = dt["drop_remarks"].ToString(),
                            potential_value = dt["potential_value"].ToString(),

                        });

                        values.droplist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Drop Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

            
        }

        public void DaGetAllSummary(MdlMyLead values, string employee_gid)
        {

            try
            {
                 
                msSQL = " Select b.leadbank_name, k.campaign_title,b.customer_type,b.leadbank_gid,a.call_count," +
               " concat(g.leadbankcontact_name,' / ',g.mobile,' / ',g.email) as contact_details," +
               " concat(d.region_name,'/',b.leadbank_city,'/',b.leadbank_state,'/',h.source_name) as region_name," +
                   " (a.internal_notes) as internal_notes,z.leadstage_name," +
                   " a.lead2campaign_gid, a.leadbank_gid, a.campaign_gid, g.leadbankcontact_gid" +
                   " From crm_trn_tlead2campaign a" +
                   " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid        " +
                   " left join crm_mst_tregion d on b.leadbank_region=d.region_gid           " +
                   " left join crm_trn_tleadbankcontact g on b.leadbank_gid = g.leadbank_gid " +
                   " left join crm_mst_tsource h on b.source_gid=h.source_gid                " +
                   " left join crm_trn_tcampaign k on a.campaign_gid=k.campaign_gid          " +
                   " left join crm_mst_tleadstage z on a.leadstage_gid=z.leadstage_gid" +
                    " where a.assign_to =  '" + employee_gid + "' " +
                   " and g.status='Y' and g.main_contact='Y' and a.leadstage_gid !='0' and a.leadstage_gid !='5' and a.leadstage_gid !='6'" +
                   " order by b.leadbank_name asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<all_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new all_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            internal_notes = dt["internal_notes"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            leadstage_name = dt["leadstage_name"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            call_count = dt["call_count"].ToString(),


                        });

                        values.alllist = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting All Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


         }

       
        public void DaGetleadbankeditSummary(string leadbank_gid, MdlMyLead values)
        {

            try
            {
                 
                msSQL = "Select  a.leadbank_gid,a.leadbank_name,a.leadbank_city, a.leadbank_address1, a.leadbank_address2, a.leadbank_state,a.leadbank_country," +
                "a.leadbank_pin, a.leadbank_region, a.company_website, c.leadbankcontact_name, c.email, c.mobile, c.designation,c.fax, c.phone1 " +
                "From crm_trn_tleadbank a left Join crm_trn_tlead2campaign b on b.leadbank_gid = a.leadbank_gid " +
                "Left Join crm_trn_tleadbankcontact c on c.leadbank_gid = a.leadbank_gid " +
                "where a.leadbank_gid ='" + leadbank_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadbankedit_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadbankedit_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            designation = dt["designation"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            email = dt["email"].ToString(),
                            company_website = dt["company_website"].ToString(),
                            fax = dt["fax"].ToString(),
                            phone1 = dt["phone1"].ToString(),
                            leadbank_address1 = dt["leadbank_address1"].ToString(),
                            leadbank_address2 = dt["leadbank_address2"].ToString(),
                            leadbank_pin = dt["leadbank_pin"].ToString(),
                            country_gid = dt["leadbank_country"].ToString(),
                            region_gid = dt["leadbank_region"].ToString(),
                        });
                        values.leadbankedit_list = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Leadbankedit Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetregiondropdown(MdlMyLead values)
        {
            try
            {

                msSQL = "SELECT region_gid, region_name FROM crm_mst_tregion Order by region_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<regionname_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new regionname_list
                        {
                            region_gid = dt["region_gid"].ToString(),
                            region_name = dt["region_name"].ToString(),

                        });
                        values.regionname_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Region Dropdown!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetcallresponsedropdown(MdlMyLead values)
        {

            try
            {

                msSQL = "select call_response,callresponse_gid,moving_stage,callresponse_code from crm_mst_callresponse where moving_stage!= 2 and  moving_stage!= 7";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<callresponse_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new callresponse_list
                        {
                            call_response = dt["call_response"].ToString(),
                            moving_stage = dt["moving_stage"].ToString(),

                        });
                        values.callresponse_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Callresponse Dropdown!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaGetindustrydropdown(string user_gid, MdlMyLead values)
        {
            try
            {
                 
                msSQL = "select categoryindustry_gid,categoryindustry_name  from crm_mst_tcategoryindustry";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<industryname_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new industryname_list
                        {
                            categoryindustry_gid = dt["categoryindustry_gid"].ToString(),
                            categoryindustry_name = dt["categoryindustry_name"].ToString(),

                        });
                        values.industryname_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Industry Dropdown!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
         
        }

       
        public void DaGetcountrydropdown(MdlMyLead values)
        {

            try
            {
                 
                msSQL = "Select country_gid,country_name from adm_mst_tcountry";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<country_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new country_list1
                        {
                            country_gid = dt["country_gid"].ToString(),
                            country_name = dt["country_name"].ToString(),
                        });
                        values.country_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Country Dropdown!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetcurrencydropdown(MdlMyLead values)
        {
            try
            {
                 
                msSQL = "Select currencyexchange_gid,currency_code from crm_trn_tcurrencyexchange";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<currency_codelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new currency_codelist
                        {
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                        });
                        values.currencycodelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Currency Dropdown!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostleadbank(string user_gid, leadbank_list values)
        {

            try
            {
                 
                msSQL = "Select customer_gid from crm_mst_tcustomer where customer_name ='" + values.leadbank_name + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count == 0)
                {

                    msGetGid3 = objcmnfunctions.GetMasterGID("BCRM");
                    if (msGetGid3 == "E")
                    {
                        values.Status = false;
                        values.message = "Create sequence code BCRM for lead bank";
                    }
                    msGetGid = objcmnfunctions.GetMasterGID("CC");
                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='CC'";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);

                    string lscustomer_code = "CC-" + "00" + lsCode;

                    msSQL = " INSERT INTO crm_mst_tcustomer" +
                               " (customer_gid," +
                               " customer_code, " +
                               " customer_name," +
                               " company_website," +
                               " customer_code," +
                               " customer_address," +
                               " customer_address2," +
                               " customer_country," +
                               " customer_city," +
                               " customer_pin," +
                               " customer_region," +
                               " main_branch," +
                               " status," +
                               " created_by," +
                               " created_date)" +
                               " values( " +
                               " '" + msGetGid3 + "'," +
                                "'" + lscustomer_code + "'," +
                               " '" + values.leadbank_name + "'," +
                               " '" + values.company_website + "'," +
                               " '  H.Q  '," +
                               " '" + values.leadbank_address1 + "'," +
                               " '" + values.leadbank_address2 + "'," +
                               " '" + values.country_name + "'," +
                               " '" + values.leadbank_city + "'," +
                               " '" + values.leadbank_pin + "'," +
                               " '" + values.region_name + "'," +
                               " '  y  '," +
                               " '  Active  '," +
                               " '" + user_gid + "'," +
                               " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                    mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult3 == 1)
                    {
                        msSQL = " update crm_trn_tleadbank set" +
                       " customer_gid = '" + msGetGid3 + "'" +
                      " where leadbank_gid = '" + values.leadbank_gid + "'";
                        mnResult4 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (mnResult4 == 1)
                    {
                        msGetGid4 = objcmnfunctions.GetMasterGID("BCCM");
                        if (msGetGid4 == "E")
                        {
                            values.Status = false;
                            values.message = "Create sequence code BCCM for lead bank";
                        }

                        objfinance.finance_vendor_debitor("Sales", lscustomer_code, values.leadbank_name, msGetGid, user_gid);
                        string trace_comment = " Added a customer on " + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        objcmnfunctions.Tracelog(msGetGid, user_gid, trace_comment, "added_customer");

                        msSQL = " INSERT INTO crm_mst_tcustomercontact" +
                       " (customercontact_gid," +
                       " customer_gid," +
                       " customerbranch_name, " +
                       " customercontact_name," +
                       " email," +
                       " mobile," +
                       " designation," +
                       " created_date," +
                       " created_by," +
                       " address1, " +
                       " address2, " +
                       " country_gid, " +
                       " region, " +
                       " zip_code, " +
                       " main_contact," +
                       " phone," +
                       " fax)" +
                       " values( " +
                       "'" + msGetGid4 + "'," +
                       "'" + msGetGid3 + "'," +
                       "'H.Q', " +
                       "'" + values.leadbankcontact_name + "'," +
                       "'" + values.email + "'," +
                       "'" + values.mobile + "'," +
                       "'" + values.designation + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       "'" + user_gid + "', " +
                       "'" + values.leadbank_address1 + "'," +
                       "'" + values.leadbank_address2 + "'," +
                       "'" + values.country_gid + "'," +
                       "'" + values.region_name + "', " +
                       "'" + values.leadbank_pin + "'," +
                       "'Y'," +
                       "'" + values.phone1 + "'," +
                       "'" + values.fax + "')";

                        mnResult5 = objdbconn.ExecuteNonQuerySQL(msSQL);


                        msSQL = " update crm_trn_tlead2campaign set leadstage_gid = '6' " +
                                  " where leadbank_gid ='" + values.leadbank_gid + "' and leadstage_gid = '3'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        values.message = "Added to customer successfully";
                    }
                }
                else
                {
                    values.message = "customer already exisits";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Leadbank!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

      

        public void DaMovetodrop(string user_gid, leadbank_list values)
        {
            try
            {
                 

                msSQL = " update crm_trn_tlead2campaign set leadstage_gid='5',lead_base='In Progress'" +
                    " where leadbank_gid='" + values.leadbank_gid + "' and (leadstage_gid='3') ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    values.message = "lead moved to drop successfully";
                }
                else
                {
                    values.message = "error while drop lead ";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Moving Lead to Drop!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetleadbankcontactSummary(string leadbank_gid, MdlMyLead values)
        {

            try
            {
                 
                msSQL = " select a.leadbankbranch_name,concat (a.address1,a.address2) As Address,a.city,a.state, " +
                    " a.pincode,b.country_name from crm_trn_tleadbankcontact a " +
                    " left join adm_mst_tcountry b on a.country_gid=b.country_gid " +
                    " where a.leadbank_gid ='" + leadbank_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadbankcontact_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadbankcontact_list
                        {
                            leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                            Address = dt["Address"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            pincode = dt["pincode"].ToString(),
                            country_name = dt["country_name"].ToString(),
                        });
                        values.leadbankcontact_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Leadbank Contact Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }  
        }
        public void DaGetProductdropdown(string productgroup_gid, MdlMyLead values)
        {

            try
            {
                 
                msSQL = "select product_gid,product_name from pmr_mst_tproduct  where productgroup_gid='" + productgroup_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<product_list32>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_list32
                        {
                            product_gid1 = dt["product_gid"].ToString(),
                            product_name1 = dt["product_name"].ToString()
                        });
                        values.product_list32 = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Purchase Liability Report Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
         
        }
        public void DaGetProductGroupdropdown(MdlMyLead values)
        {

            try
            {
                 
                msSQL = " Select productgroup_gid,productgroup_name from pmr_mst_tproductgroup ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<product_group_list12>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new product_group_list12
                        {
                            productgroup_gid1 = dt["productgroup_gid"].ToString(),
                            productgroup_name1 = dt["productgroup_name"].ToString(),

                        });
                        values.product_group_list12 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Dropdown!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaGetCallResponse(string leadbank_gid, MdlMyLead values, string user_gid)
        {

            try
            {
                 
                msSQL = " select remarks, DATE_FORMAT(created_date, '%d-%m-%Y') AS created_date from crm_trn_tcalllog where created_by =  '" + user_gid + "' and leadbank_gid='" + leadbank_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<call_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new call_list
                        {
                            call_remarks = dt["remarks"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            //call_count = dt["count"].ToString(),

                        });
                        values.call_list = getModuleList;
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Call Response!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostNewschedulelog(followup_list values, string user_gid, string employee_gid)
        {

            try
            {
                msSQL = " insert into crm_trn_tOpportunitylog ( " +
                " appointment_gid, " +
                " log_type, " +
                " log_date, " +
                " log_remarks, " +
                " created_by, " +
                " created_date ) " +
                " values (  " +
                "'" + values.appointment_gid + "'," +
                "'Opportunity'," +
                "'" + values.schedule_date + ' ' + values.schedule_time + "'," +
                "'" + values.schedule_remarks + "'," +
                "'" + user_gid + "'," +
                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult1 == 1)
                {
                    msSQL = "update crm_trn_tappointment set appointment_date ='" + values.schedule_date + ' ' + values.schedule_time + "' where appointment_gid = '" + values.appointment_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = " Opportunity scheduled  Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = " Error Occurs while Scheduling ";
                }
                }
                else
                {
                    values.status = false;
                    values.message = " Error Occurs while Scheduling ";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting New Schedule Log!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }         
        }
        public void DaPostTeleschedulelog(followup_list values, string user_gid, string employee_gid)
        {

            try
            {

                msSQL = " SELECT schedulelog_gid,leadbank_gid " +
            " from crm_trn_tschedulelog " +
            " where schedule_date = '" + values.schedule_date + "' and" +
            " schedule_time = '" + values.schedule_time + "' " +
            " and leadbank_gid = '" + values.leadbank_gid + "' and " +
            " schedule_type = 'Call Log' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.status = false;
                    values.message = "Already schedule added";
                }
                else
                {

                    msGetGid11 = objcmnfunctions.GetMasterGID("BLGP");
                    msSQL = " insert into crm_trn_tlog ( " +
                    " log_gid, " +
                    " leadbank_gid, " +
                    " log_type, " +
                    " log_desc, " +
                    " log_by, " +
                    " log_date ) " +
                    " values (  " +
                    "'" + msGetGid11 + "'," +
                    "'" + values.leadbank_gid + "'," +
                    "'Schedule'," +
                    "'" + (String.IsNullOrEmpty(values.schedule_remarks)? values.schedule_remarks : values.schedule_remarks .Replace("'","\\\'"))+ "'," +
                    "'" + user_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult1 == 0)
                    {
                        values.status = false;
                        values.message = "Error Occured While Inserting Records ";
                    }

                    msGetGid = objcmnfunctions.GetMasterGID("BSLC");
                    msSQL = " insert into crm_trn_tschedulelog  (" +
                    " schedulelog_gid, " +
                    " leadbank_gid," +
                     " schedule_date, " +
                     " schedule_time, " +
                     " schedule_type, " +
                      " schedule_remarks, " +
                      " schedule_status, " +
                      " status_flag, " +
                      " reference_gid, " +
                      " log_gid, " +
                      " assign_to, " +
                      " created_by, " +
                      " created_date ) " +
                    " values (" +
                   " '" + msGetGid + "', " +
                   " '" + values.leadbank_gid + "', " +
                   " '" + values.schedule_date + "'," +
                   " '" + values.schedule_time + "'," +
                   " '" + values.schedule_type + "'," +
                   "'" + (String.IsNullOrEmpty(values.schedule_remarks) ? values.schedule_remarks : values.schedule_remarks.Replace("'", "\\\'"))+ "'," +
                   " 'Pending'," +
                   " 'N'," +
                   "'" + values.lead2campaign_gid + "'," +
                   "'" + msGetGid11 + "'," +
                   "'" + employee_gid + "'," +
                    "'" + user_gid + "'," +
                   "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " update crm_trn_ttelelead2campaign set leadstage_gid='7' where leadbank_gid='" + values.leadbank_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        values.status = true;
                        values.message = " Call scheduled  Successfully";

                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = " Call scheduled  Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = " Error Occurs ";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting New Schedule Log!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostNewlog(string user_gid, new_list values)
        {
            try
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
                  " created_date ) " +
                " values (" +
               " '" + msGetGid + "', " +
               " '" + values.leadbank_gid + "', " +
               " '" + values.dialed_number + "'," +
               " '" + values.call_response + "'," +
              " '" + values.prosperctive_percentage + "'," +
               " '" + values.call_feedback + "'," +
               "'" + user_gid + "'," +
               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "select moving_stage from crm_mst_callresponse where call_response = '" + values.call_response + "'";
                string lsstage = objdbconn.GetExecuteScalar(msSQL);

                if (mnResult != 0)
                {
                    msSQL = " update crm_trn_tlead2campaign set leadstage_gid='" + lsstage + "' where leadbank_gid='" + values.leadbank_gid + "' and (leadstage_gid='1' or leadstage_gid is null) ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult != 0)
                {

                    msSQL = "select call_count+1 as call_count from crm_trn_tlead2campaign where leadbank_gid='" + values.leadbank_gid + "'";
                    string lscount = objdbconn.GetExecuteScalar(msSQL);

                    // Convert lscount to an integer
                    //int callCount = int.Parse(lscount);
                    //callCount++;
                    //string callcount = callCount.ToString();

                    msSQL = "update crm_trn_tlead2campaign set call_count = '" + lscount + "' where leadbank_gid ='" + values.leadbank_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = " Call Recorded Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = " Error Occurs ";

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting New Call Log!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }          
        }
        public void DaPostProspectlog(new_pending_list values, string user_gid)
        {

            try
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
                  " created_date ) " +
                " values (" +
               " '" + msGetGid + "', " +
               " '" + values.leadbank_gid + "', " +
               " '" + values.dialed_number + "'," +
               " '" + values.call_response + "'," +
              " '" + values.prosperctive_percentage + "'," +
               " '" + values.call_feedback + "'," +
               "'" + user_gid + "'," +
               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                msSQL = "select moving_stage from crm_mst_callresponse where call_response = '" + values.call_response + "'";
                string lsstage = objdbconn.GetExecuteScalar(msSQL);

                if (mnResult != 0)
                {
                    if (lsstage == "4" || lsstage == "5" || lsstage == "6" || lsstage == "7")
                    {
                        msSQL = " update crm_trn_tlead2campaign set leadstage_gid='" + lsstage + "' where leadbank_gid='" + values.leadbank_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }

                if (mnResult != 0)
                {
                    msSQL = "select call_count+1 as call_count from crm_trn_tlead2campaign where leadbank_gid='" + values.leadbank_gid + "'";
                    string lscount = objdbconn.GetExecuteScalar(msSQL);

                    // Convert lscount to an integer
                    //int callCount = int.Parse(lscount);
                    //callCount++;
                    //string callcount = callCount.ToString();

                    msSQL = "update crm_trn_tlead2campaign set call_count = '" + lscount + "' where leadbank_gid ='" + values.leadbank_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    values.status = true;
                    values.message = " Call Recorded Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = " Error Occurs ";

                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Prospect Call Log!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostPotentiallog(string user_gid, followup_list values)
        {

            try
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
                " created_date ) " +
                " values (" +
                " '" + msGetGid + "', " +
                " '" + values.leadbank_gid + "', " +
                " '" + values.dialed_number + "'," +
                " '" + values.call_response + "'," +
                " '" + values.prosperctive_percentage + "'," +
                " '" + values.call_feedback + "'," +
                "'" + user_gid + "'," +
                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                msSQL = "select moving_stage from crm_mst_callresponse where call_response = '" + values.call_response + "'";
                string lsstage = objdbconn.GetExecuteScalar(msSQL);

                if (mnResult != 0)
                {
                    if (lsstage == "5" || lsstage == "6" || lsstage == "7")
                    {
                        msSQL = " update crm_trn_tlead2campaign set leadstage_gid='" + lsstage + "' where leadbank_gid='" + values.leadbank_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }

                if (mnResult != 0)
                {
                    msSQL = "select call_count+1 as call_count from crm_trn_tlead2campaign where leadbank_gid='" + values.leadbank_gid + "'";

                    string lscount = objdbconn.GetExecuteScalar(msSQL);

                    // Convert lscount to an integer
                    //int callCount = int.Parse(lscount);
                    //callCount++;
                    //string callcount = callCount.ToString();

                    msSQL = "update crm_trn_tlead2campaign set call_count = '" + lscount + "' where leadbank_gid ='" + values.leadbank_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = " Call Recorded Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = " Error Occurs ";

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Potential Call log!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }           
        }

        // My Leads Count
        public void DaGetMyLeadsCount(string employee_gid, string user_gid, MdlMyLead values)
        {

            try
            {

                msSQL = "SELECT FORMAT(SUM(potential_value),2) as potential_value from crm_trn_tleadbank";
                string potential_value = objdbconn.GetExecuteScalar(msSQL);

                string NowdateLabel = DateTime.Now.ToString("yyyy-MM-dd");

                msSQL = " select (select count(lead2campaign_gid) from crm_trn_tlead2campaign a " +
                        " left join crm_trn_tschedulelog i on a.leadbank_gid = i.leadbank_gid " +
                        " where i.assign_to=  '" + employee_gid + "' and i.schedule_date = '" + NowdateLabel + "' ) as todaytask_count, " +
                        " (select count(schedulelog_gid) from crm_trn_tschedulelog a " +
                        " where (a.schedule_type='Meeting') and a.schedule_date > curdate() " +
                        " and a.assign_to ='" + employee_gid + "') as upcoming_counts," +
                        " (select count(lead2campaign_gid) from crm_trn_tlead2campaign where assign_to ='" + employee_gid + "'" +
                        " and leadstage_gid='1') as newlead_count," +
                        " (select count(lead2campaign_gid) from crm_trn_tlead2campaign where assign_to ='" + employee_gid + "' " +
                        " and leadstage_gid='3') as prospects_count," +
                        " (select count(lead2campaign_gid) from crm_trn_tlead2campaign where assign_to ='" + employee_gid + "' " +
                        " and leadstage_gid='4') as potential_count," +
                        " (select count(lead2campaign_gid) from crm_trn_tlead2campaign where assign_to ='" + employee_gid + "'" +
                        " and leadstage_gid='5') as drop_count," +
                        " (select count(lead2campaign_gid) from crm_trn_tlead2campaign where assign_to ='" + employee_gid + "'" +
                        " and leadstage_gid='6') as completed_count," +
                        " (select count(lead2campaign_gid) from crm_trn_tlead2campaign where assign_to ='" + employee_gid + "'" +
                        "and leadstage_gid !='0' and leadstage_gid !='5' and leadstage_gid !='6') as allleads_count";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getMyLeadsCountList = new List<getMyLeadsCount_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getMyLeadsCountList.Add(new getMyLeadsCount_List
                        {
                            todaytask_count = (dt["todaytask_count"].ToString()),
                            upcoming_counts = (dt["upcoming_counts"].ToString()),
                            newlead_count = (dt["newlead_count"].ToString()),
                            prospects_count = (dt["prospects_count"].ToString()),
                            potential_count = (dt["potential_count"].ToString()),
                            drop_count = (dt["drop_count"].ToString()),
                            completed_count = (dt["completed_count"].ToString()),
                            allleads_count = (dt["allleads_count"].ToString()),
                            potential_value = potential_value,

                        });
                        values.getMyLeadsCount_List = getMyLeadsCountList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting My Leads Tiles count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostcloselog(string employee_gid, ExpiredVisit_list1 values)
        {

            try
            {
                 
                   String str = values.meeting_time;

                    msSQL = " update crm_trn_tschedulelog set " +
                     " schedule_status='Closed'," +
                     " updated_by= '" + employee_gid + "', " +
                     " closing_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                     " status_flag='Y'," +
                     " updated_remarks = '" + values.schedule_remarks + "'" +
                     " where schedulelog_gid='" + values.schedulelog_gid + "'";


                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                
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
            catch (Exception ex)
            {
                values.message = "Exception occured while Closing Schedule Log!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
          
        }
        //+ "' and (leadstage_gid='2' or leadstage_gid is null)"

        public void DaPostpostonedlog(Upcomingvisit_list1 values, string user_gid)
        {

            try
            {
                 
                msSQL = " update crm_trn_tschedulelog set " +
                   " schedule_date= '" + values.postponed_date + "', " +
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
            catch (Exception ex)
            {
                values.message = "Exception occured while Postponed Schedule Log!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostdroplog(ExpiredVisit_list1 values, string user_gid)
        {

            try
            {
                msSQL = " update crm_trn_tappointment set leadstage_gid='5'" +
                        " where leadbank_gid='" + values.leadbank_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = " DELETE FROM crm_trn_tschedulelog WHERE leadbank_gid ='" + values.leadbank_gid + "'";

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
            catch (Exception ex)
            {
                values.message = "Exception occured while Dropping Schedule!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //Schedulelog summary
        public void DaGetSchedulelogsummary(string leadbank_gid, MdlMyLead values)
        {

            try
            {
                 
                msSQL = "SELECT CASE WHEN a.log_type = 'Schedule' THEN REPLACE(" +
                " CONCAT('Schedule Remark: ', b.schedule_remarks, '<br />'),'<br />','') END AS log_details," +
                " CASE WHEN a.log_type = 'Schedule' THEN CASE " +
                " WHEN e.schedule_type = 'Call' THEN CONCAT('Call Scheduled On :', ' ', DATE_FORMAT(e.schedule_date, '%d-%m-%Y'),',',TIME_FORMAT(b.schedule_time, '%h:%i %p'))" +
                " END END AS log_legend, a.leadbank_gid FROM crm_trn_tlog a" +
                " LEFT JOIN crm_trn_tschedulelog b ON b.log_gid = a.log_gid" +
                " LEFT JOIN crm_trn_tschedulelog e ON e.log_gid = a.log_gid" +
                " LEFT JOIN crm_trn_tcalllog c ON c.log_gid = a.log_gid" +
                " LEFT JOIN crm_trn_tfieldlog d ON d.log_gid = a.log_gid" +
                " WHERE a.leadbank_gid = '" + leadbank_gid + "' AND c.log_gid IS NULL AND d.log_gid IS NULL ORDER BY a.log_gid DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<schedulesummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new schedulesummary_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            log_details = dt["log_details"].ToString(),
                            log_legend = dt["log_legend"].ToString(),

                        });
                        values.schedulesummary_list = getModuleList;
                    }
                    dt_datatable.Dispose();
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Schedule Log Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetOpportunitylogsummary(string appointment_gid, MdlMyLead values)
        {

            try
            {

                msSQL = " select schedule_status,postponed_date, CASE WHEN log_type = 'Opportunity' THEN REPLACE(CONCAT('Schedule Remark : ', log_remarks, '<br />'),'<br />','') END AS log_details," +
                        " CASE WHEN log_type = 'Opportunity' THEN CASE WHEN log_type = 'Opportunity' THEN CONCAT('Meeting Scheduled On :', ' ', log_date) END END AS log_legend from crm_trn_topportunitylog " +
                        " WHERE appointment_gid = '" + appointment_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Opportunityschedulesummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Opportunityschedulesummary_list
                        {
                            log_details = dt["log_details"].ToString(),
                            log_legend = dt["log_legend"].ToString(),
                            schedule_status = dt["schedule_status"].ToString(),
                            postponed_date = dt["postponed_date"].ToString(),

                        });
                        values.Opportunityschedulesummary_list = getModuleList;
                    }
                    dt_datatable.Dispose();
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Schedule Log Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetMarketingAssignedTeam(string employee_gid, MdlMyLead values)
        {

            try
            {
                msSQL = "select a.campaign_gid,Concat (b.campaign_prefix,' / ',b.campaign_title) as campaign_title from crm_trn_tcampaign2employee a \r\nleft join crm_trn_tcampaign b on a.campaign_gid = b.campaign_gid where a.employee_gid= '" + employee_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<myleadsassignedteamdropdown_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new myleadsassignedteamdropdown_list
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),

                        });
                        values.myleadsassignedteamdropdown_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting My Leads Assigned Team Name Dropdown!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostmyleadsleadbank(postmyleadsleadbank_list values, string employee_gid )
        {

            try
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
                        " '" + lscustomer_type + "'," +
                        " '" + values.customer_type + "'," +
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
                        " '" + values.leadbankcontact_name + "'," +
                        " '" + values.email + "'," +
                        " '" + values.phone.e164Number + "'," +
                        " '" + values.phone1 + "'," +
                        " '" + values.country_code1 + "'," +
                        " '" + values.area_code1 + "'," +
                        " '" + values.phone2 + "'," +
                        " '" + values.country_code2 + "'," +
                        " '" + values.area_code2 + "'," +
                        " '" + values.fax + "'," +
                        " '" + values.fax_country_code + "'," +
                        " '" + values.fax_area_code + "'," +
                        " '" + values.designation + "'," +
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
            catch (Exception ex)
            {
                values.message = "Exception occured while Post Leads In Leadbank From My Leads!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}
