using ems.crm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Net.Http;

namespace ems.crm.DataAccess
{
    public class DaAppointmentManagement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid;


        public void DaGetLeaddropdown(MdlAppointmentManagement values)
        {
            msSQL = "select a.leadbank_gid,a.leadbank_name,b.leadbankbranch_name,b.leadbankcontact_name,b.address1,b.address2,b.city,b.state,b.pincode,b.mobile,b.email,c.region_name,d.source_name " +
                " from crm_trn_tleadbank a left join  crm_trn_tleadbankcontact  b on a.leadbank_gid=b.leadbank_gid " +
                "left join crm_mst_tregion c on a.leadbank_region=c.region_gid" +
                " left join crm_mst_tsource d on a.source_gid=d.source_gid where b.main_contact ='Y';";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetLeaddropdown_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetLeaddropdown_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                        leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                        address1 = dt["address1"].ToString(),
                        address2 = dt["address2"].ToString(),
                        city = dt["city"].ToString(),
                        state = dt["state"].ToString(),
                        pincode = dt["pincode"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        email = dt["email"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        source_name = dt["source_name"].ToString(),
          

                    });
                    values.GetLeaddropdown_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetbussinessverticledropdown(MdlAppointmentManagement values)
        {
            msSQL = "select businessvertical_gid,business_vertical from crm_mst_tbusinessvertical where status_flag='Y';";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getbussinessverticledropdown_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getbussinessverticledropdown_list
                    {
                        businessvertical_gid = dt["businessvertical_gid"].ToString(),
                        business_vertical = dt["business_vertical"].ToString(),
                       

                    });
                    values.Getbussinessverticledropdown_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaPostAppointment(string user_gid, Postappointment_list values)
        {
            try
            {
                msSQL = "select campaign_gid,campaign_title from crm_trn_tcampaign where campaign_gid='" + values.campaign_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("APMT");
                    msSQL = " insert into crm_trn_tappointment (" +
                                     " appointment_gid," +
                                     " lead_title, " +
                                     " leadbank_gid, " +
                                     " business_vertical, " +
                                     " appointment_date, " +
                                     " Leadstage_gid," +
                                     " campaign_gid," +
                                     " assign_to," +
                                     " created_by," +
                                     "created_date" +
                                      ") values (" +
                                     "'" + msGetGid + "', " +
                                     "'" + values.lead_title.Replace("'", "\\\'")+ "'," +
                                     "'" + values.leadname_gid + "'," +
                                     "'" + values.bussiness_verticle + "'," +
                                     "'" + values.appointment_timing + "'," +
                                     "'" + "1" + "'," +
                                     "'" + values.campaign_gid + "'," +
                                     "'" + values.Employee_gid + "'," +
                                      "'" + user_gid + "'," +
                                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
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
                                "'" + values.lead_title.Replace("'", "\\\'") + "'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Occured Submitting Appointment ";
                    }
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("APMT");
                    msSQL = " insert into crm_trn_tappointment (" +
                                     " appointment_gid," +
                                     " lead_title, " +
                                     " leadbank_gid, " +
                                     " business_vertical, " +
                                     " appointment_date, " +
                                     " Leadstage_gid," +
                                     " created_by," +
                                     "created_date" +
                                      ") values (" +
                                     "'" + msGetGid + "', " +
                                     "'" + values.lead_title.Replace("'", "\\\'") + "'," +
                                     "'" + values.leadname_gid + "'," +
                                     "'" + values.bussiness_verticle + "'," +
                                     "'" + values.appointment_timing + "'," +
                                     "'" + "1" + "'," +
                                      "'" + user_gid + "'," +
                                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
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
                                "'" + values.lead_title.Replace("'", "\\\'") + "'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Occured Submitting Appointment ";
                    }
                }



                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Appointment Submitted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Occured Submitting Appointment  ";
                }
            }
            catch(Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }



        }

        public void DaGetAppointmentsummary(MdlAppointmentManagement values)
        {
            msSQL = "SELECT a.leadbank_gid,a.assign_to, a.appointment_gid, a.internal_notes,a.lead_title, a.potential_value," +
                " a.business_vertical, b.leadbank_name,c.leadbankbranch_name,c.leadbankcontact_name,c.email,c.mobile," +
                "CONCAT_WS('/',d.region_name,d.city, e.source_name) AS region_source, date_format(a.appointment_date,'%d-%b-%y  %h:%i %p')as appointment_date," +
                "f.business_vertical  as bussiness_name ,concat(h.user_firstname,' ',h.user_lastname) as assigned_employee,a.appointment_date as fullformat_date " +
                "FROM crm_trn_tappointment a LEFT JOIN crm_trn_tleadbank b ON a.leadbank_gid = b.leadbank_gid LEFT JOIN " +
                "crm_trn_tleadbankcontact c ON a.leadbank_gid = c.leadbank_gid LEFT JOIN crm_mst_tregion d ON b.leadbank_region = d.region_gid" +
                " LEFT JOIN crm_mst_tsource e ON b.source_gid = e.source_gid left join crm_mst_tbusinessvertical f" +
                " on a.business_vertical=f.businessvertical_gid left join hrm_mst_temployee g on a.assign_to=g.employee_gid  " +
                "left join adm_mst_tuser h on g.user_gid=h.user_gid where Leadstage_gid!='0' and c.main_contact ='Y' order by a.created_date desc;";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetAppointmentsummary_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetAppointmentsummary_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        assign_to = dt["assign_to"].ToString(),
                        appointment_gid = dt["appointment_gid"].ToString(),
                        lead_title = dt["lead_title"].ToString(),
                        internal_notes = dt["internal_notes"].ToString(),
                        potential_value = dt["potential_value"].ToString(),
                        business_vertical = dt["business_vertical"].ToString(),
                        leadbank_name = dt["leadbank_name"].ToString(),
                        leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                        leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                        email = dt["email"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        region_source = dt["region_source"].ToString(),
                        appointment_date = dt["appointment_date"].ToString(),
                        bussiness_name = dt["bussiness_name"].ToString(),
                        assigned_employee = dt["assigned_employee"].ToString(),
                        fullformat_date = dt["fullformat_date"].ToString(),


                    });
                    values.GetAppointmentsummary_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
         public void DaGetAppointmentTiles(MdlAppointmentManagement values)
         {
            msSQL = "select(select Count(appointment_gid)from crm_trn_tappointment where Leadstage_gid!='0') as total_appointment," +
                "(select (Count(distinct campaign_gid))from crm_trn_tappointment) as total_team," +
                "(select distinct(Count(assign_to))from crm_trn_tappointment where (assign_to is not null or assign_to!='') and Leadstage_gid!='0') as total_assigned," +
                "(select distinct(Count(appointment_gid))from crm_trn_tappointment where (assign_to is null or assign_to='') and Leadstage_gid!='0') as total_unassigned;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetAppointmentTiles_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetAppointmentTiles_list
                    {
                        total_appointment = dt["total_appointment"].ToString(),
                        total_team = dt["total_team"].ToString(),
                        total_assigned = dt["total_assigned"].ToString(),
                        total_unassigned = dt["total_unassigned"].ToString(),
                       

                    });
                    values.GetAppointmentTiles_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
         }
         public void DaGetTeamdropdown(MdlAppointmentManagement values)
            {
                msSQL = "Select a.campaign_gid,concat(a.campaign_prefix,' - ',a.campaign_title)as team_name from crm_trn_tcampaign a;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetTeamdropdown_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetTeamdropdown_list
                            {
                                campaign_gid = dt["campaign_gid"].ToString(),
                                campaign_prefix = dt["campaign_prefix"].ToString(),
                            });
                            values.GetTeamdropdown_list = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
            }

        public void DaPostAssignedEmployee(string employee_gid, PostAssignedEmployee_list values)
        {
            msSQL = " update crm_trn_tappointment set campaign_gid = '" + values.teamname_gid + "' , assign_to = '" + values.employee_gid + "' " +
                 " where appointment_gid = '" + values.appointment_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                values.message = " Updated Successfully !!";
            }
            else
            {
                values.status = false;
                values.message = "Error While Updating  !!";
            }
        }

        public void DaPosteditappointment(string employee_gid, Posteditappointment_list values)
        {
            msSQL = " update crm_trn_tappointment set lead_title = '" + values.editlead_title.Replace("'", "\\\'") + "' , leadbank_gid = '" + values.editleadname_gid + "',business_vertical='"+ values.editbussiness_verticle + "',appointment_date='"+values.editappointment_timing + "',updated_by='"+ employee_gid + "', updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                 " where appointment_gid = '" + values.appointment_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 1)
            {
                values.status = true;
                values.message = " Updated Successfully !!";
            }
            else
            {
                values.status = false;
                values.message = "Error While Updating  !!";
            }
        }
        public void Dashopifyenquiry(MdlGmailCampaign values)
        {


            try
            {
                msSQL = "SELECT s_no, inbox_id, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date, DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time, cc, subject, body, attachement_flag FROM crm_trn_tgmailinbox where inbox_status is Null ORDER BY s_no DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                values.gmailapiinboxsummary_list = dt_datatable.AsEnumerable().AsParallel()
                    .Select(row =>
                    {
                        var decodedSubject = DecodeFromBase64(row["subject"].ToString());
                        var attachementFlag = row["attachement_flag"].ToString();
                        if (decodedSubject.IndexOf("New customer message on", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            msSQL = "update crm_trn_tgmailinbox set shopify_enquiry = 'Y' where s_no='" + row["s_no"].ToString() + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                        return new gmailapiinboxsummary_list
                        {
                            s_no = row["s_no"].ToString(),
                            subject = decodedSubject,
                        };
                    })
                    .ToList();

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void Dashopifyenquirysummary(MdlGmailCampaign values)
        {
            try
            {
                msSQL = " SELECT s_no, inbox_id,leadbank_gid, from_id, DATE_FORMAT(sent_date, '%b %e, %Y %h:%i %p') AS sent_date," +
                        " DATE_FORMAT(sent_date, '%h:%i %p') AS sent_time, cc, subject," +
                        " body, attachement_flag FROM crm_trn_tgmailinbox where shopify_enquiry = 'Y' and inbox_status is Null ORDER BY s_no DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getmodulelist = new List<gmailapiinboxsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailapiinboxsummary_list
                        {
                            s_no = dt["s_no"].ToString(),
                            inbox_id = dt["inbox_id"].ToString(),
                            from_id = dt["from_id"].ToString(),
                            sent_date = dt["sent_date"].ToString(),
                            sent_time = dt["sent_time"].ToString(),
                            cc = dt["cc"].ToString(),
                            subject = DecodeFromBase64(dt["subject"].ToString()),
                            body = (dt["body"].ToString()),
                            attachement_flag = dt["attachement_flag"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),


                        });
                        values.gmailapiinboxsummary_list = getmodulelist;
                    }
                }

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
       private static string DecodeFromBase64(string base64Value)
        {
            if (string.IsNullOrEmpty(base64Value))
            {
                return string.Empty;
            }

            // Check if the string is a valid Base64 encoded string
            if (IsBase64String(base64Value))
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(base64Value);
                    return Encoding.UTF8.GetString(bytes);
                }
                catch (FormatException)
                {
                    // Handle the case where the string is not a valid Base64 encoded string
                    return base64Value; // Or handle as needed, e.g., return an error message
                }
            }

            return base64Value;
        }

        private static bool IsBase64String(string value)
        {
            // Regular expression to check if a string is Base64 encoded
            string base64Pattern = @"^[a-zA-Z0-9\+/]*={0,2}$";
            return Regex.IsMatch(value, base64Pattern) && (value.Length % 4 == 0);
        }
        private static string DecodesFromBase64(string base64Value)
        {
            if (string.IsNullOrEmpty(base64Value))
            {
                return string.Empty;
            }

            // Check if the string is a valid Base64 encoded string
            if (IsBase64Strings(base64Value))
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(base64Value);
                    return Encoding.UTF8.GetString(bytes);
                }
                catch (FormatException)
                {
                    // Handle the case where the string is not a valid Base64 encoded string
                    return base64Value; // Or handle as needed, e.g., return an error message
                }
            }

            return base64Value;
        }

        // Helper method to check if a string is a valid base64 encoded string
        private static bool IsBase64Strings(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            value = value.Trim();
            return (value.Length % 4 == 0) && Regex.IsMatch(value, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }
}