using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.crm.Models;
using System.Threading;

namespace ems.crm.DataAccess
{
    public class DaMyAppointment
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, today_count_opportunity, upcoming_count_opportunity, expired_count_opportunity;
        public void DaGetTotalappointmentSummary(string employee_gid, MdlMyAppointment values)
        {
            try
            {

                msSQL = " select a.leadbank_gid,a.appointment_gid,a.lead_title,a.internal_notes,a.potential_value,f.business_vertical, b.leadbank_name, " +
                        "concat_ws(' / ',b.leadbank_name,c.leadbankbranch_name,c.leadbankcontact_name,c.mobile,c.email)as lead_contact, " +
                        " concat_ws(' / ',c.leadbankbranch_name,c.leadbankcontact_name) as Details,concat_ws(' / ',d.region_name,d.city,e.source_name) as region_source, " +
                        " date_format(a.appointment_date,'%d-%b-%y  %h:%i %p')as appointment_date " +
                        " from crm_trn_tappointment a " + 
                        " left join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                        " left join crm_trn_tleadbankcontact c on a.leadbank_gid=c.leadbank_gid " +
                        " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                        " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                        " left join crm_mst_tbusinessvertical f on a.business_vertical=f.businessvertical_gid "+
                        " where a.assign_to='" + employee_gid + "'and c.main_contact ='Y' and Leadstage_gid in ('1','3','4','5','6')";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<gettotalappointmentsummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gettotalappointmentsummary_lists
                        {
                            appointment_gid = dt["appointment_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead_title = dt["lead_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            lead_contact = dt["lead_contact"].ToString(),
                            appointment_date = dt["appointment_date"].ToString(),
                            region_source = dt["region_source"].ToString(),
                            business_vertical = dt["business_vertical"].ToString(),

                        });
                        values.gettotalappointmentsummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Total My Appointment Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetCompletedappointmentSummary(string employee_gid, MdlMyAppointment values)
        {
            try
            {

                msSQL = " select a.leadbank_gid,a.appointment_gid,a.lead_title,a.internal_notes,a.potential_value,f.business_vertical, b.leadbank_name, " +
                        " concat_ws(' / ',b.leadbank_name,c.leadbankbranch_name,c.leadbankcontact_name,c.mobile,c.email)as lead_contact, " +
                        " concat_ws(' / ',c.leadbankbranch_name,c.leadbankcontact_name) as Details,concat_ws(' / ',d.region_name,d.city,e.source_name) as region_source, " +
                        " date_format(a.appointment_date,'%d-%b-%y  %h:%i %p')as appointment_date " +
                        " from crm_trn_tappointment a " +
                        " left join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                        " left join crm_trn_tleadbankcontact c on a.leadbank_gid=c.leadbank_gid " +
                        " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                        " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                        " left join crm_mst_tbusinessvertical f on a.business_vertical=f.businessvertical_gid " +
                        " where a.Leadstage_gid > '1' and c.main_contact ='Y' and a.assign_to='" + employee_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<gettotalappointmentsummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gettotalappointmentsummary_lists
                        {
                            appointment_gid = dt["appointment_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead_title = dt["lead_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            lead_contact = dt["lead_contact"].ToString(),
                            appointment_date = dt["appointment_date"].ToString(),
                            region_source = dt["region_source"].ToString(),
                            business_vertical = dt["business_vertical"].ToString(),

                        });
                        values.gettotalappointmentsummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Completed My Appointment Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }     
        public void DaGetTodayappointmentSummary(string employee_gid, MdlMyAppointment values)
        {
            try
            {

                msSQL = " select a.Opportunitylog_gid,b.leadbank_gid,a.schedule_status,b.appointment_gid,b.lead_title,b.internal_notes,b.potential_value,g.business_vertical, c.leadbank_name, " +
                        " concat_ws(' / ',c.leadbank_name,d.leadbankbranch_name,d.leadbankcontact_name,d.mobile,d.email)as lead_contact, " +
                        " concat_ws(' / ',d.leadbankbranch_name,d.leadbankcontact_name) as Details,concat_ws(' / ',e.region_name,e.city,f.source_name) as region_source, " +
                        " date_format(a.log_date,'%d-%b-%y  %h:%i %p')as appointment_date " + 
                        " from crm_trn_topportunitylog a " +
                        " left join crm_trn_tappointment b on a.appointment_gid=b.appointment_gid " +
                        " left join crm_trn_tleadbank c on b.leadbank_gid=c.leadbank_gid " +
                        " left join crm_trn_tleadbankcontact d on b.leadbank_gid=d.leadbank_gid " +
                        " left join crm_mst_tregion e on c.leadbank_region=e.region_gid " +
                        " left join crm_mst_tsource f on c.source_gid=f.source_gid " +
                        " left join crm_mst_tbusinessvertical g on b.business_vertical=g.businessvertical_gid " +
                        " where curdate()=date(a.log_date) and d.main_contact ='Y' and a.schedule_status !='close' and a.schedule_status !='Postponed' and b.assign_to='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<gettotalappointmentsummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gettotalappointmentsummary_lists
                        {
                            appointment_gid = dt["appointment_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead_title = dt["lead_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            lead_contact = dt["lead_contact"].ToString(),
                            appointment_date = dt["appointment_date"].ToString(),
                            region_source = dt["region_source"].ToString(),
                            business_vertical = dt["business_vertical"].ToString(),
                            schedule_status = dt["schedule_status"].ToString(),
                            Opportunitylog_gid = dt["Opportunitylog_gid"].ToString(),

                        });
                        values.gettotalappointmentsummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Today My Appointment Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }     
        public void DaGetUpcomingappointmentSummary(string employee_gid, MdlMyAppointment values)
        {
            try
            {

                            msSQL = " select a.Opportunitylog_gid,b.leadbank_gid,a.schedule_status,b.appointment_gid,b.lead_title,b.internal_notes,b.potential_value,g.business_vertical, c.leadbank_name, " +
                                    " concat_ws(' / ',c.leadbank_name,d.leadbankbranch_name,d.leadbankcontact_name,d.mobile,d.email)as lead_contact, " +
                                    " concat_ws(' / ',d.leadbankbranch_name,d.leadbankcontact_name) as Details,concat_ws(' / ',e.region_name,e.city,f.source_name) as region_source, " +
                                    " date_format(a.log_date,'%d-%b-%y  %h:%i %p')as appointment_date " + 
                                    " from crm_trn_topportunitylog a " +
                                    " left join crm_trn_tappointment b on a.appointment_gid=b.appointment_gid " +
                                    " left join crm_trn_tleadbank c on b.leadbank_gid=c.leadbank_gid " +
                                    " left join crm_trn_tleadbankcontact d on b.leadbank_gid=d.leadbank_gid " +
                                    " left join crm_mst_tregion e on c.leadbank_region=e.region_gid " +
                                    " left join crm_mst_tsource f on c.source_gid=f.source_gid " +
                                    " left join crm_mst_tbusinessvertical g on b.business_vertical=g.businessvertical_gid " +
                                    " where date(a.log_date) > curdate() and d.main_contact ='Y' and a.schedule_status !='close' and a.schedule_status !='Postponed' and b.assign_to='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<gettotalappointmentsummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gettotalappointmentsummary_lists
                        {
                            appointment_gid = dt["appointment_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead_title = dt["lead_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            lead_contact = dt["lead_contact"].ToString(),
                            appointment_date = dt["appointment_date"].ToString(),
                            region_source = dt["region_source"].ToString(),
                            business_vertical = dt["business_vertical"].ToString(),
                            schedule_status = dt["schedule_status"].ToString(),
                            Opportunitylog_gid = dt["Opportunitylog_gid"].ToString(),

                        });
                        values.gettotalappointmentsummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Upcoming My Appointment Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }     
        public void DaGetExpiredappointmentSummary(string employee_gid, MdlMyAppointment values)
        {
            try
            {

                msSQL = " select a.Opportunitylog_gid,b.leadbank_gid,a.schedule_status,b.appointment_gid,b.lead_title,b.internal_notes,b.potential_value,g.business_vertical, c.leadbank_name, " +
                        " concat_ws(' / ',c.leadbank_name,d.leadbankbranch_name,d.leadbankcontact_name,d.mobile,d.email)as lead_contact, " +
                        " concat_ws(' / ',d.leadbankbranch_name,d.leadbankcontact_name) as Details,concat_ws(' / ',e.region_name,e.city,f.source_name) as region_source, " +
                        " date_format(a.log_date,'%d-%b-%y  %h:%i %p')as appointment_date " + 
                        " from crm_trn_topportunitylog a " +
                        " left join crm_trn_tappointment b on a.appointment_gid=b.appointment_gid " +
                        " left join crm_trn_tleadbank c on b.leadbank_gid=c.leadbank_gid " +
                        " left join crm_trn_tleadbankcontact d on b.leadbank_gid=d.leadbank_gid " +
                        " left join crm_mst_tregion e on c.leadbank_region=e.region_gid " +
                        " left join crm_mst_tsource f on c.source_gid=f.source_gid " +
                        " left join crm_mst_tbusinessvertical g on b.business_vertical=g.businessvertical_gid " +
                        " where date(a.log_date) < curdate() and d.main_contact ='Y' and a.schedule_status !='close' and a.schedule_status !='Postponed' and b.assign_to='" + employee_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<gettotalappointmentsummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gettotalappointmentsummary_lists
                        {
                            appointment_gid = dt["appointment_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead_title = dt["lead_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            lead_contact = dt["lead_contact"].ToString(),
                            appointment_date = dt["appointment_date"].ToString(),
                            region_source = dt["region_source"].ToString(),
                            business_vertical = dt["business_vertical"].ToString(),
                            schedule_status = dt["schedule_status"].ToString(),
                            Opportunitylog_gid = dt["Opportunitylog_gid"].ToString(),

                        });
                        values.gettotalappointmentsummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Expired My Appointment Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }     
        public void DaGetNewappointmentSummary(string employee_gid, MdlMyAppointment values)
        {
            try
            {

                msSQL = " select a.leadbank_gid,a.appointment_gid,a.lead_title,a.internal_notes,a.potential_value,f.business_vertical, b.leadbank_name, " +
                        " concat_ws(' / ',b.leadbank_name,c.leadbankbranch_name,c.leadbankcontact_name,c.mobile,c.email)as lead_contact, " +
                        " concat_ws(' / ',c.leadbankbranch_name,c.leadbankcontact_name) as Details,CONCAT_WS('/',d.region_name,d.city,e.source_name)as region_source, " +
                        " date_format(a.appointment_date,'%d-%b-%y  %h:%i %p')as appointment_date " +
                        " from crm_trn_tappointment a " +
                        " left join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                        " left join crm_trn_tleadbankcontact c on a.leadbank_gid=c.leadbank_gid " +
                        " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                        " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                        " left join crm_mst_tbusinessvertical f on a.business_vertical=f.businessvertical_gid " +
                        " where a.Leadstage_gid = '1'and c.main_contact ='Y' and a.assign_to='" + employee_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<gettotalappointmentsummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gettotalappointmentsummary_lists
                        {
                            appointment_gid = dt["appointment_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead_title = dt["lead_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            lead_contact = dt["lead_contact"].ToString(),
                            appointment_date = dt["appointment_date"].ToString(),
                            region_source = dt["region_source"].ToString(),
                            business_vertical = dt["business_vertical"].ToString(),

                        });
                        values.gettotalappointmentsummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting New My Appointment Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }     
        public void DaGetprospectappointmentSummary(string employee_gid, MdlMyAppointment values)
        {
            try
            {

                msSQL = " select a.leadbank_gid,a.appointment_gid,a.lead_title,a.internal_notes,a.potential_value,f.business_vertical, b.leadbank_name, " +
                        " concat_ws(' / ',b.leadbank_name,c.leadbankbranch_name,c.leadbankcontact_name,c.mobile,c.email)as lead_contact, " +
                        " concat_ws(' / ',c.leadbankbranch_name,c.leadbankcontact_name) as Details,concat_ws(' / ',d.region_name,d.city,e.source_name) as region_source, " +
                        " date_format(a.appointment_date,'%d-%b-%y  %h:%i %p')as appointment_date " +
                        " from crm_trn_tappointment a " +
                        " left join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                        " left join crm_trn_tleadbankcontact c on a.leadbank_gid=c.leadbank_gid " +
                        " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                        " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                        " left join crm_mst_tbusinessvertical f on a.business_vertical=f.businessvertical_gid " +
                        " where a.Leadstage_gid = '3'and c.main_contact ='Y' and a.assign_to='" + employee_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<gettotalappointmentsummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gettotalappointmentsummary_lists
                        {
                            appointment_gid = dt["appointment_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead_title = dt["lead_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            lead_contact = dt["lead_contact"].ToString(),
                            appointment_date = dt["appointment_date"].ToString(),
                            region_source = dt["region_source"].ToString(),
                            business_vertical = dt["business_vertical"].ToString(),

                        });
                        values.gettotalappointmentsummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting New My Appointment Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }     
        public void DaGetpotentialsappointmentSummary(string employee_gid, MdlMyAppointment values)
        {
            try
            {
                msSQL = " SELECT FORMAT(SUM(b.potential_value),2) as potential_value from crm_trn_tleadbank a" +
                      " left join crm_trn_tappointment b on a.leadbank_gid = b.leadbank_gid " +
                      "  left join crm_trn_tcampaign c on c.campaign_gid = b.campaign_gid " +
                      " left join crm_trn_tleadbankcontact g on a.leadbank_gid = g.leadbank_gid " +
                      " where g.status='Y' and g.main_contact='Y' and  b.assign_to = '" + employee_gid + "' and b.leadstage_gid = '4'";

                string potential_value_count = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select a.leadbank_gid,a.appointment_gid,a.lead_title,a.internal_notes,format(a.potential_value,2)as potential_value,f.business_vertical, b.leadbank_name, " +
                        " concat_ws(' / ',b.leadbank_name,c.leadbankbranch_name,c.leadbankcontact_name,c.mobile,c.email)as lead_contact, " +
                        " concat_ws(' / ',c.leadbankbranch_name,c.leadbankcontact_name) as Details,concat_ws(' / ',d.region_name,d.city,e.source_name) as region_source, " +
                        " date_format(a.appointment_date,'%d-%b-%y  %h:%i %p')as appointment_date " +
                        " from crm_trn_tappointment a " +
                        " left join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                        " left join crm_trn_tleadbankcontact c on a.leadbank_gid=c.leadbank_gid " +
                        " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                        " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                        " left join crm_mst_tbusinessvertical f on a.business_vertical=f.businessvertical_gid " +
                        " where a.Leadstage_gid = '4'and c.main_contact ='Y' and a.assign_to='" + employee_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<gettotalappointmentsummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gettotalappointmentsummary_lists
                        {
                            appointment_gid = dt["appointment_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead_title = dt["lead_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            lead_contact = dt["lead_contact"].ToString(),
                            appointment_date = dt["appointment_date"].ToString(),
                            region_source = dt["region_source"].ToString(),
                            business_vertical = dt["business_vertical"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            potential_value_count = potential_value_count

                        });
                        values.gettotalappointmentsummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting New My Appointment Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }     
        public void DaGetclosedappointmentSummary(string employee_gid, MdlMyAppointment values)
        {
            try
            {
                msSQL = " SELECT FORMAT(SUM(b.potential_value),2) as potential_value from crm_trn_tleadbank a" +
                     " left join crm_trn_tappointment b on a.leadbank_gid = b.leadbank_gid " +
                     "  left join crm_trn_tcampaign c on c.campaign_gid = b.campaign_gid " +
                     " left join crm_trn_tleadbankcontact g on a.leadbank_gid = g.leadbank_gid " +
                     " where g.status='Y' and g.main_contact='Y' and  b.assign_to = '" + employee_gid + "' and b.leadstage_gid = '6'";

                string potential_value_count = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select a.leadbank_gid,a.appointment_gid,a.lead_title,a.internal_notes,format(a.potential_value,2)as potential_value,f.business_vertical, b.leadbank_name, " +
                        " concat_ws(' / ',b.leadbank_name,c.leadbankbranch_name,c.leadbankcontact_name,c.mobile,c.email)as lead_contact, " +
                        " concat_ws(' / ',c.leadbankbranch_name,c.leadbankcontact_name) as Details,concat_ws(' / ',d.region_name,d.city,e.source_name) as region_source, " +
                        " date_format(a.appointment_date,'%d-%b-%y  %h:%i %p')as appointment_date " +
                        " from crm_trn_tappointment a " +
                        " left join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                        " left join crm_trn_tleadbankcontact c on a.leadbank_gid=c.leadbank_gid " +
                        " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                        " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                        " left join crm_mst_tbusinessvertical f on a.business_vertical=f.businessvertical_gid " +
                        " where a.Leadstage_gid = '6'and c.main_contact ='Y' and a.assign_to='" + employee_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<gettotalappointmentsummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gettotalappointmentsummary_lists
                        {
                            appointment_gid = dt["appointment_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead_title = dt["lead_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            lead_contact = dt["lead_contact"].ToString(),
                            appointment_date = dt["appointment_date"].ToString(),
                            region_source = dt["region_source"].ToString(),
                            business_vertical = dt["business_vertical"].ToString(),
                            potential_value = dt["potential_value"].ToString(),
                            potential_value_count = potential_value_count

                        });
                        values.gettotalappointmentsummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting New My Appointment Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }     
        public void DaGetdropappointmentSummary(string employee_gid, MdlMyAppointment values)
        {
            try
            {

                msSQL = " select a.leadbank_gid,a.appointment_gid,a.lead_title,a.internal_notes,a.potential_value,f.business_vertical, b.leadbank_name, " +
                        " concat_ws(' / ',b.leadbank_name,c.leadbankbranch_name,c.leadbankcontact_name,c.mobile,c.email)as lead_contact, " +
                        "concat_ws(' / ',c.leadbankbranch_name,c.leadbankcontact_name) as Details,concat_ws(' / ',d.region_name,d.city,e.source_name) as region_source, " +
                        " date_format(a.appointment_date,'%d-%b-%y  %h:%i %p')as appointment_date " +
                        " from crm_trn_tappointment a " +
                        " left join crm_trn_tleadbank b on a.leadbank_gid=b.leadbank_gid " +
                        " left join crm_trn_tleadbankcontact c on a.leadbank_gid=c.leadbank_gid " +
                        " left join crm_mst_tregion d on b.leadbank_region=d.region_gid " +
                        " left join crm_mst_tsource e on b.source_gid=e.source_gid " +
                        " left join crm_mst_tbusinessvertical f on a.business_vertical=f.businessvertical_gid " +
                        " where a.Leadstage_gid = '5'and c.main_contact ='Y' and a.assign_to='" + employee_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<gettotalappointmentsummary_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gettotalappointmentsummary_lists
                        {
                            appointment_gid = dt["appointment_gid"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            lead_title = dt["lead_title"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            lead_contact = dt["lead_contact"].ToString(),
                            appointment_date = dt["appointment_date"].ToString(),
                            region_source = dt["region_source"].ToString(),
                            business_vertical = dt["business_vertical"].ToString(),

                        });
                        values.gettotalappointmentsummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting New My Appointment Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetMyAppointmentTilesCount(string employee_gid, MdlMyAppointment values)
        {
            try
            {
                msSQL = " select sum(CASE WHEN CURDATE() = DATE(a.log_date) THEN 1 ELSE 0 END) AS today_count_opportunity, " +
                        " sum(CASE WHEN DATE(a.log_date) > CURDATE() THEN 1 ELSE 0 END) AS upcoming_count_opportunity, " +
                        " sum(CASE WHEN DATE(a.log_date) < CURDATE() THEN 1 ELSE 0 END) AS expired_count_opportunity from crm_trn_topportunitylog a " +
                        " left join crm_trn_tappointment b on b.appointment_gid = a.appointment_gid " +
                        " where a.schedule_status !='close' and a.schedule_status !='Postponed' and b.assign_to='" + employee_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                   today_count_opportunity = objOdbcDataReader["today_count_opportunity"].ToString();
                   upcoming_count_opportunity = objOdbcDataReader["upcoming_count_opportunity"].ToString();
                   expired_count_opportunity = objOdbcDataReader["expired_count_opportunity"].ToString();
                }
                objOdbcDataReader.Close();

                msSQL = " select sum(case when Leadstage_gid > '1' then 1 else 0 end ) as completed_count, " +
                        " sum(case when curdate()=date(appointment_date) then 1 else 0 end ) as today_count, " +
                        " sum(case when date(appointment_date) >  curdate() then 1 else 0 end ) as upcoming_count, " +
                        " sum(case when date(appointment_date) <  curdate() then 1 else 0 end ) as expired_count, " +
                         " sum(case when Leadstage_gid = '1' then 1 else 0 end ) as New_count, " +
                        " sum(case when Leadstage_gid = '3' then 1 else 0 end ) as prospect_count, " +
                        " sum(case when Leadstage_gid = '4' then 1 else 0 end ) as potentials_count, " +
                        " sum(case when Leadstage_gid = '5' then 1 else 0 end ) as drop_count,  " +
                        " sum(case when Leadstage_gid = '6' then 1 else 0 end ) as closed_count,  " +
                        " sum(case when appointment_gid is not null and Leadstage_gid in ('1','3','4','5','6') then 1 else 0 end ) as total_count " +
                        " from crm_trn_tappointment where assign_to='" + employee_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<getmyappointmenttilescount_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new getmyappointmenttilescount_lists
                        {
                            total_count = dt["total_count"].ToString(),
                            completed_count = dt["completed_count"].ToString(),
                            today_count = today_count_opportunity,
                            upcoming_count = upcoming_count_opportunity,
                            expired_count = expired_count_opportunity,
                            New_count = dt["New_count"].ToString(),
                            prospect_count = dt["prospect_count"].ToString(),
                            potentials_count = dt["potentials_count"].ToString(),
                            drop_count = dt["drop_count"].ToString(),
                            closed_count = dt["closed_count"].ToString(),

                        });
                        values.getmyappointmenttilescount_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting My Appointment Tiles Count Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetMyAppointmentlogsummary(string leadbank_gid, MdlMyAppointment values)
        {
            try
            {

                msSQL = " select lead_title,appointment_date from crm_trn_tappointment where leadbank_gid='" + leadbank_gid + "' order by appointment_date";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<getmyappointmentlog_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new getmyappointmentlog_lists
                        {
                            appointment_date = dt["appointment_date"].ToString(),
                            lead_title = dt["lead_title"].ToString(),

                        });
                        values.getmyappointmentlog_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting My Appointment log Summary Count Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostsstatusclose(string user_gid, Poststatusclose_list values)//SCHEDULE CLOSE
        {
            try
            {
                msSQL = " update crm_trn_topportunitylog set " +
                     " schedule_status ='Close'," +
                     " updated_by= '" + user_gid + "', " +
                     " updated_date= '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                     " log_remarks = '" + values.schedule_remarks + "'" +
                     " where Opportunitylog_gid ='" + values.Opportunitylog_gid + "'";
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
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error Occurred While Closing Schedule!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaPoststatuspostpone(string user_gid, Poststatuspostpone_list values)//SCHEDULE POSTPONE
        {
            try
            {
                msSQL = " update crm_trn_topportunitylog set " +
                  " log_remarks = '" + values.schedule_remarks + "'," +
                  " schedule_status = 'Postponed'," +
                  " postponed_date = '" + values.postponed_date + "'," +
                  " updated_by = '" + user_gid + "'," +
                  " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Opportunitylog_gid ='" + values.Opportunitylog_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " insert into crm_trn_tOpportunitylog ( " +
                      " appointment_gid, " +
                      " log_type, " +
                      " log_date, " +
                      " log_remarks, " +
                      " schedule_status, " +
                      " created_by, " +
                      " created_date ) " +
                      " values (  " +
                      "'" + values.appointment_gid + "'," +
                      "'Opportunity'," +
                      "'" + values.postponed_date + "'," +
                      "'" + values.schedule_remarks + "'," +
                      "'Pending'," +
                      "'" + user_gid + "'," +
                      "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
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
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error Occurred While Postpone Schedule!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }
    }
}