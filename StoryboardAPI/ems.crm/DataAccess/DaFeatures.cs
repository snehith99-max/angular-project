using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;
using ems.crm.Models;
using RestSharp;
using System.Net;
using System.Security.Cryptography;
using System.Configuration;
using Newtonsoft.Json;
namespace ems.crm.DataAccess
{
    public class DaFeatures
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, dt_datatable1;
        int mnResult;
        public void DaGetnotesSummary(MdlFeatures values, string user_gid)
        {
            try
            {

                msSQL = " select  s_no, notes_detail, CONCAT(b.user_firstname,' ',b.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date " +
                    " from crm_mst_tstickynotes a " +
                    " left join adm_mst_tuser b on b.user_gid=a.created_by where user_gid ='"+ user_gid + "' order by a.s_no desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<notesupdate_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new notesupdate_list
                        {
                            s_no = dt["s_no"].ToString(),
                            notes_detail = dt["notes_detail"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.notesupdate_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Group Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/Notes" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostnotesupdate(string user_gid, notesupdate_list values)
        {
            try
            {
                msSQL = "insert into crm_mst_tstickynotes ( " +
                    "notes_detail," +
                  " created_by, " +
                  " created_date)" +
                    " values(" +
                     "'" + values.notes_detail + "'," +
                     "'" + user_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Notes Added Successfully";
                }
                else
                {

                    values.status = false;
                    values.message = "Error While Adding Notes";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Notes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/Notes " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdatednotes(string user_gid, notesupdate_list values)
        {
            try
            {
                msSQL = " update  crm_mst_tstickynotes  set " +
               " notes_detail = '" + values.notes_detail + "'," +
              " updated_by = '" + user_gid + "'," +
              " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.s_no + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Notes Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Notes !!";
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Product Group!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/Notes " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void Dadeletenotes(string s_no, notesupdate_list values)
        {

            try
            {
                   msSQL = "  delete from crm_mst_tstickynotes where s_no='" + s_no + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Notes Deleted Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Notes!!";
                    }

                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Notes!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/Notes " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void Danotesdeleteevent(string s_no, notesupdate_list values)
        {

            try
            {
                msSQL = "  delete from crm_mst_tstickynotes where s_no='" + s_no + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Notes Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Notes!!";
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Notes!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/Notes " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void Daaddmeetingschedule(string user_gid, meetingschedule_list values)
        {
            try
            {
                msSQL = "insert into crm_mst_tscheduledtask ( " +
                    "title," +
                    "meeting_link," +
                    "start_date," +
                    "start_time," +
                    "end_time," +
                  " created_by, " +
                  " created_date)" +
                    " values(" +
                    "'" + values.meeting_title + "'," +
                    "'" + values.meeting_link + "'," +
                    "'" + values.start_date + "'," +
                    "'" + values.start_time + "'," +
                    "'" + values.end_time + "'," +
                    "'" + user_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Schedule Added Successfully";
                }
                else
                {

                    values.status = false;
                    values.message = "Error While Adding Schedule";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Notes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/Notes " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DameetingSummary(MdlFeatures values, string user_gid)
        {
            try
            {

                msSQL = " select  scheduletask_gid, title,DATE_FORMAT(start_date, '%d-%m-%Y') AS start_date,start_time,end_time,meeting_link " +
                    " from crm_mst_tscheduledtask where created_by ='" + user_gid + "' AND start_date >= CURDATE() order by start_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<meetingschedule_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new meetingschedule_list
                        {
                            s_no = dt["scheduletask_gid"].ToString(), 
                            meeting_title = dt["title"].ToString(),
                            start_date = dt["start_date"].ToString(),
                            start_time = dt["start_time"].ToString(),
                            end_time = dt["end_time"].ToString(),
                            meeting_link = dt["meeting_link"].ToString(),

                        });
                        values.meetingschedule_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Group Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/Notes" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void Dadeletmeetingschedule(string s_no, meetingschedule_list values)
        {

            try
            {
                msSQL = "  delete from crm_mst_tscheduledtask where scheduletask_gid='" + s_no + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Meeting Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Meeting!!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Notes!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/Notes " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaCalendlyMeeting(MdlCalendly values,string user_gid)
        {
            try
            {
                var meeting_list = new List<MdlCalendlyMeetingDates>();
                msSQL = "select distinct date(a.meeting_start_time) as meeting_dates " +
                        "from crm_mst_tcalendlymeetings a " + 
                        "left join crm_trn_tcalendlymeetingattendees b on a.meeting_gid = b.meeting_gid " + 
                        "where b.user_gid = '" + user_gid + "' and meeting_start_time > now()";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if(dt_datatable != null)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        var meeting_sub_list = new List<MdlCalendlyMeetingDetails>();

                        msSQL = "select a.meeting_url,a.meeting_participants,a.meeting_title,time(meeting_start_time) as start_time,time(meeting_end_time) as end_time,a.meeting_organiser_name,a.meeting_type,a.location " +
                                "from crm_mst_tcalendlymeetings a " +
                                "left join crm_trn_tcalendlymeetingattendees b on a.meeting_gid = b.meeting_gid " +
                                "where b.user_gid = '" + user_gid + "' and meeting_start_time > now() and meeting_start_time like '" + DateTime.Parse(dt["meeting_dates"].ToString()).ToString("yyyy-MM-dd") + "%'";
                        dt_datatable1 = objdbconn.GetDataTable(msSQL);
                        if(dt_datatable1 != null)
                        {
                            foreach(DataRow dr in dt_datatable1.Rows)
                            {
                                meeting_sub_list.Add(new MdlCalendlyMeetingDetails
                                {
                                    meeting_organiser = dr["meeting_organiser_name"].ToString(),
                                    meeting_url = dr["meeting_url"].ToString(),
                                    meeting_title = dr["meeting_title"].ToString(),
                                    meeting_participants = dr["meeting_participants"].ToString().Replace(",","<br>"),
                                    start_time = dr["start_time"].ToString(),
                                    end_time = dr["end_time"].ToString(),
                                    meeting_type = dr["meeting_type"].ToString(),
                                    location = dr["location"].ToString(),
                                });
                            }                            
                        }

                        meeting_list.Add(new MdlCalendlyMeetingDates
                        {
                            meeting_date = DateTime.Parse(dt["meeting_dates"].ToString()).ToString("dd MMM yy"),
                            meetingList = meeting_sub_list
                        });
                    }
                    values.calendlyMeetingList = meeting_list;
                    values.status = true;
                }
            }
            catch(Exception ex)
            {
                objcmnfunctions.LogForAudit(ex.ToString());
            }
            finally
            {
                if(dt_datatable != null)
                {
                    dt_datatable.Dispose();
                }
                if (dt_datatable1 != null)
                {
                    dt_datatable1.Dispose();
                }
            }
        }

        public void DaCalendlyUserDetails(MdlcalendlyAccountDetails values)
        {
            string token;
            try
            {
                msSQL = "select token from crm_smm_tcalendlyservice";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    token = objOdbcDataReader["token"].ToString();
                    objOdbcDataReader.Close();

                    msSQL = "select scheduling_url from crm_trn_tcalendlyaccountdetails";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if(objOdbcDataReader.HasRows)
                    {
                        values.scheduling_url = objOdbcDataReader["scheduling_url"].ToString();
                        values.status = true;
                    }
                    else
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["calendlybaseURL"].ToString());
                        var request = new RestRequest("/users/me", Method.GET);
                        request.AddHeader("authorization", "Bearer " + token);
                        IRestResponse response = client.Execute(request);
                        MdlCalendlyUserDetails objMdlCalendlyUserDetails = new MdlCalendlyUserDetails();
                        objMdlCalendlyUserDetails = JsonConvert.DeserializeObject<MdlCalendlyUserDetails>(response.Content);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            msSQL = "insert into crm_trn_tcalendlyaccountdetails(" +
                                    "avatar_url," +
                                    "organization_url," +
                                    "user_url," +
                                    "user_name," +
                                    "user_email," +
                                    "resource_type," +
                                    "scheduling_url," +
                                    "timezone," +
                                    "created_at," +
                                    "updated_at," +
                                    "created_date)values(" +
                                    "'" + objMdlCalendlyUserDetails.resource.avatar_url + "'," +
                                    "'" + objMdlCalendlyUserDetails.resource.current_organization + "'," +
                                    "'" + objMdlCalendlyUserDetails.resource.uri + "'," +
                                    "'" + objMdlCalendlyUserDetails.resource.name + "'," +
                                    "'" + objMdlCalendlyUserDetails.resource.email + "'," +
                                    "'" + objMdlCalendlyUserDetails.resource.resource_type + "'," +
                                    "'" + objMdlCalendlyUserDetails.resource.scheduling_url + "'," +
                                    "'" + objMdlCalendlyUserDetails.resource.timezone + "'," +
                                    "'" + objMdlCalendlyUserDetails.resource.created_at.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    "'" + objMdlCalendlyUserDetails.resource.updated_at.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if(mnResult == 0)
                            {
                                objcmnfunctions.LogForAudit("Insert failed: " + msSQL);
                            }
                            else
                            {
                                values.scheduling_url = objMdlCalendlyUserDetails.resource.scheduling_url;
                                values.status = true;
                            }
                        }
                        else if(response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            msSQL = "update crm_smm_tcalendlyservice set active_flag = 'N', error_message = 'Provided token is an invalid token. Kindly update a valid token to continue!'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if(mnResult == 0)
                            {
                                objcmnfunctions.LogForAudit("Update failed; " + msSQL);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit(ex.ToString());
            }
        }

        public void DaCalendlyCheckIfActive(result values)
        {            
            try
            {
                msSQL = "select active_flag from crm_smm_tcalendlyservice";
                string active_flag = objdbconn.GetExecuteScalar(msSQL);
                if (!String.IsNullOrEmpty(active_flag))
                {
                    if(active_flag == "Y")
                        values.status = true;
                }
            }
            catch(Exception ex)
            {
                objcmnfunctions.LogForAudit(ex.ToString());
            }
            finally
            {
                if (objOdbcDataReader != null)
                {
                    objOdbcDataReader.Close();
                }
            }
        }
    }
}