using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.crm.Models;

namespace ems.crm.DataAccess
{
    public class DaTeleCallerTeamSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lscampaign_title, lscampaign_location, lscampaign_gid, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, lsRowcount;


        public void DaGetTeamSummary(MdlTeleCallerTeamSummary values)
        {
            try
            {

                msSQL = "SELECT a.campaign_gid,b.branch_gid,a.campaign_title,a.campaign_prefix,a.campaign_mailid,a.campaign_description, a.campaign_location,b.branch_name,b.branch_prefix,date_format(a.campaign_startdate,'%d-%m-%Y') as campaign_startdate," +
             "  case when a.campaign_enddate = '0000-00-00' then 'Infinite' else date_format(a.campaign_enddate,'%d-%m-%Y') end as campaign_enddate, " +
             "  (SELECT count(lead2campaign_gid) FROM crm_trn_ttelelead2campaign k left join crm_trn_tleadbank b on k.leadbank_gid = b.leadbank_gid " +
             "  where k.campaign_gid=a.campaign_gid and b.leadbank_name is " +
             "  not null) as assigned_total, (SELECT  COUNT(employee_gid) FROM crm_trn_tteleteam2employee WHERE campaign_gid = a.campaign_gid) AS total_employees ," +
             "  (select count(employee_gid) from cmn_trn_tmanagerprivilege where team_gid = a.campaign_gid ) as total_managers " +
             "  FROM crm_trn_tteleteam a left join hrm_mst_tbranch b on a.campaign_location = b.branch_gid order by a.campaign_gid desc " ;
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<Telecallerlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Telecallerlist
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            branch = dt["branch_prefix"].ToString(),
                            mail_id = dt["campaign_mailid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            assigned_total = dt["assigned_total"].ToString(),
                            campaign_description = dt["campaign_description"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            campaign_location = dt["campaign_location"].ToString(),
                            total_employees = dt["total_employees"].ToString(),
                            total_managers = dt["total_managers"].ToString(),
                            campaign_prefix = dt["campaign_prefix"].ToString(),

                        });
                        values.Telecaller_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostTelecallerteam(string user_gid, Telecallerteamlist values)
        {
            try
            {

                msSQL = " select campaign_title from crm_trn_tteleteam where campaign_title = '" + values.team_name.Replace("'", "\\\'") + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Team Name Already Exist !!";
                }
                else
                {

                    msGetGid = objcmnfunctions.GetMasterGID("BTTP");

                    msSQL = " insert into crm_trn_tteleteam(" +
                              " campaign_gid," +
                              " campaign_title," +
                              " campaign_prefix," +
                              " campaign_description," +
                              " campaign_location," +
                              " campaign_manager," +
                              " campaign_mailid," +
                             " created_by, " +
                              " created_date)" +
                              " values(" +
                              " '" + msGetGid + "'," +
                              " '" + values.team_name.Replace("'", "\\\'") + "'," +
                              " '" + values.team_prefix + "',";
                    if (values.description != null
                        )
                    {
                        msSQL += " '" + values.description.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += " '" + values.description + "',";
                    }
                    msSQL += "'" + values.branch + "'," +
                     "'" + values.team_manager + "'," +
                     "'" + values.mail_id + "'," +
                     "'" + user_gid + "'," +
                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        msSQL = " insert into cmn_trn_tmanagerprivilege( " +
                                " team_gid, " +
                                " employee_gid, " +
                                " module_gid, " +
                                " created_by, " +
                                " created_date) " +
                                " values( " +
                                " '" + msGetGid + "', " +
                                "'" + values.team_manager + "'," +
                                " 'B2BTLCTCT', " +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                     mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                     if (mnResult == 1)
                       {
                            msSQL = " insert into crm_trn_tteleteam2employee( " +
                             " campaign_gid, " +
                             " employee_gid, " +
                             " created_by, " +
                             " created_date) " +
                             " values( " +
                             "'" + msGetGid + "'," +
                             "'" + values.team_manager + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                    }

                     if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Campaign Team Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Campaign Team";
                    }

                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Campaign Team!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaDeleteTeam(string campaign_gid, Telecallerteamlist values)
        {

            try
            {

                // Check if the team is assigned for lead process
                msSQL = "select campaign_gid from crm_trn_ttelelead2campaign where campaign_gid = '" + campaign_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count > 0)
                {
                    //dt_datatable.Close();
                    objdbconn.CloseConn();
                    values.status = false;
                    values.message = "This Team is assigned for Lead Process";
                }

                else
                {
                    // If all checks pass, proceed with deleting the team
                    msSQL = "delete from crm_trn_tteleteam where campaign_gid='" + campaign_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Campaign Team Deleted Successfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Campaign Team !!";
                    }

                 
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Marketing Team!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaUpdatedTelecallerteam(string user_gid, Telecallerteamlist values)
        {
            try
            {

                msSQL = " select campaign_title,campaign_gid from crm_trn_tteleteam where campaign_title = '" + values.team_name_edit.Replace("'", "\\\'") + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    lscampaign_title = objOdbcDataReader["campaign_title"].ToString();
                    lscampaign_gid = objOdbcDataReader["campaign_gid"].ToString();
                }

                if (lscampaign_gid == values.campaign_gid)
                {

                    msSQL = " update  crm_trn_tteleteam set " +
                    " campaign_title  = '" + values.team_name_edit.Replace("'", "\\\'") + "'," +
                    " campaign_prefix  = '" + values.team_prefix_edit + "',";
                    if (values.description_edit != null)
                    {
                        msSQL += " campaign_description  = '" + values.description_edit.Replace("'", "\\'") + "',";
                    }
                    else
                    {
                        msSQL += " campaign_description  = '" + values.description_edit + "',";
                    }
                   
         msSQL += " campaign_location  = '" + values.branch_edit + "'," +
                  " campaign_mailid  = '" + values.mail_id_edit + "'," +
                  " updated_by = '" + user_gid + "'," +
                  " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                  "' where campaign_gid='" + values.campaign_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Campaign Team Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Campaign Team";
                    }
                }
                else
                {
                    if (lscampaign_title != values.team_name_edit)
                    {

                        msSQL = " update  crm_trn_tteleteam set " +
                       " campaign_title  = '" + values.team_name_edit.Replace("'", "\\\'") + "',";
                      if (values.description_edit != null)
                        {
                            msSQL += " campaign_description  = '" + values.description_edit.Replace("'", "\\'") + "',";
                        }
                        else
                        {
                            msSQL += " campaign_description  = '" + values.description_edit + "',";
                        }
                       
                       msSQL +=   " campaign_location  = '" + values.branch_edit + "'," +
                      " campaign_mailid  = '" + values.mail_id_edit + "'," +
                      " updated_by = '" + user_gid + "'," +
                      " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                      "' where campaign_gid='" + values.campaign_gid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Campaign Team Updated Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Campaign Team !!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Same Campaign Team Already Exist !!";
                    }
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Campaign Team!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetTelecallerTeamCount(string employee_gid, string user_gid, MdlTeleCallerTeamSummary values)
        {
            try
            {

                msSQL = "SELECT (SELECT COUNT(a.lead2campaign_gid) FROM crm_trn_ttelelead2campaign a LEFT JOIN crm_trn_tleadbank b" +
                    " ON a.leadbank_gid = b.leadbank_gid) AS assigned_count, (SELECT COUNT(leadbank_gid) FROM crm_trn_tleadbank) AS total_count," +
                    " (SELECT COUNT(campaign_title) FROM crm_trn_tteleteam) AS team_count, (SELECT COUNT(DISTINCT employee_gid) " +
                    "FROM crm_trn_tcampaign2employee) AS total_employee, (SELECT COUNT(leadbank_gid) FROM crm_trn_tleadbank b" +
                    " WHERE b.leadbank_gid NOT IN (SELECT leadbank_gid FROM crm_trn_ttelelead2campaign)) AS unassigned_count;";

                //msSQL = " select (select count(leadbank_gid) from crm_trn_ttelelead2campaign where lead_status='Not Assigned') as unassigned_count, " +
                //        " (select count(leadbank_gid) from crm_trn_ttelelead2campaign where lead_status='Open') as assigned_count," +
                //        " (select count(leadbank_gid) from crm_trn_ttelelead2campaign where lead_status!= 'Pending') as total_count ," +
                //        " (select count(campaign_title) from crm_trn_tteleteam) as team_count," +
                //        " (SELECT count(DISTINCT  employee_gid) FROM crm_trn_tcampaign2employee) as total_employee";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getLeadBankCountList = new List<TelecallerTeamCount_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getLeadBankCountList.Add(new TelecallerTeamCount_List
                        {
                            unassigned_count = (dt["unassigned_count"].ToString()),
                            assigned_count = (dt["assigned_count"].ToString()),
                            team_count = (dt["team_count"].ToString()),
                            total_count = (dt["total_count"].ToString()),
                            total_employee = (dt["total_employee"].ToString()),
                        });
                        values.TelecallerTeamCount_List = getLeadBankCountList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Team Count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //assign employee
        public void DaGetUnassignedlist(string campaign_gid, string campaign_location, MdlTeleCallerTeamSummary values)
        {
            try
            {

                msSQL = " select distinct c.employee_gid,concat(d.user_code,'','/','',d.user_firstname,' ',d.user_lastname) AS employee_name,'" + campaign_gid + "' as campaign_gid  from adm_mst_tmodule2employee a " +
              " left join hrm_mst_temployee c on a.employee_gid = c.employee_gid" +
              " left join adm_mst_tuser d on d.user_gid = c.user_gid where c.employee_gid not in " +
              " (select employee_gid from crm_trn_tteleteam2employee where campaign_gid= '" + campaign_gid +"')" +
              " and a.hierarchy_level<>'-1' and  a.module_gid = 'MKT' group by c.employee_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetUnassignedlist1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetUnassignedlist1
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetUnassignedlist1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Unassigned List Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetAssignedlist(string campaign_gid, string campaign_location, MdlTeleCallerTeamSummary values)
        {
            try
            {

                msSQL = " select distinct c.employee_gid,concat(d.user_code,'','/','',d.user_firstname,' ',d.user_lastname) AS employee_name,'" + campaign_gid + "' as campaign_gid  from adm_mst_tmodule2employee a " +
            " left join hrm_mst_temployee c on a.employee_gid = c.employee_gid" +
            " left join adm_mst_tuser d on d.user_gid = c.user_gid where c.employee_gid in " +
            " (select employee_gid from crm_trn_tteleteam2employee where campaign_gid= '" + campaign_gid + "')" +
            " and a.hierarchy_level<>'-1' and  a.module_gid = 'MKT' group by c.employee_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetAssignedlist1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAssignedlist1
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetAssignedlist1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Assigned List Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaPostAssignedlist(string user_gid, campaignassign_list values)
        {
            try
            {

                for (int i = 0; i < values.campaignassign.ToArray().Length; i++)
                {
                    msSQL = " insert into crm_trn_tteleteam2employee ( " +
                            " campaign_gid, " +
                            " employee_gid, " +
                            " created_by," +
                            " created_date ) " +
                            " values (  " +
                            " '" + values.campaignassign[i]._key1 + "', " +
                            " '" + values.campaignassign[i]._id + "', " +
                            " '" + user_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Employee assigned to team successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Employee Assign";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Assigning Employee!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostUnassignedlist(string user_gid, campaignassign_list values)
        {
            try
            {
                if (values.campaignassign != null || values.campaign_gid != null || values.campaign_gid != "")
                {
                    msSQL = "DELETE FROM crm_trn_tteleteam2employee WHERE campaign_gid = '" + values.campaign_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        for (int i = 0; i < values.campaignassign.ToArray().Length; i++)
                        {

                            msSQL = " insert into crm_trn_tteleteam2employee ( " +
                                    " campaign_gid, " +
                                    " employee_gid, " +
                                    " created_by," +
                                    " created_date ) " +
                                    " values (  " +
                                    " '" + values.campaignassign[i]._key1 + "', " +
                                    " '" + values.campaignassign[i]._id + "', " +
                                    " '" + user_gid + "', " +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Records are Submitted Successfully ";

                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error Occured While submitting";
                            }
                        }

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occured";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "No records to process.";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while Unassigning Manager!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        //code by snehith
        public void DaGetUnassigned(string campaign_gid, MdlTeleCallerTeamSummary values)
        {
            try
            {

                msSQL = " select campaign_location from crm_trn_tcampaign where campaign_gid='" + campaign_gid + " '  ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lscampaign_location = objOdbcDataReader["campaign_location"].ToString();
                }
                msSQL = " select a.employee_gid,concat(c.user_code,'','/','',c.user_firstname,' ',c.user_lastname)AS employee_name,'" + campaign_gid + "' as campaign_gid" +
                  " from adm_mst_tmodule2employee a" +
                  " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                  " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                  " where a.module_gid = 'MKT'" +
                  " and b.branch_gid='" + lscampaign_location + "'" +
                  " and a.hierarchy_level<>'-1'" +
                  " and a.employee_gid not in" +
                  " (select employee_gid from" +
                  " crm_trn_tteleteam2employee" +
                  " where campaign_gid= '" + campaign_gid + "')" +
                  " group by employee_name";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetUnassigned>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetUnassigned
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetUnassigned = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Purchase Liability Report Chart!";
            }
        }

        public void DaGetTelecallerTeamDetailTable(string campaign_gid, MdlTeleCallerTeamSummary values)
        {
            try
            {

                msSQL = "select campaign_title from crm_trn_tteleteam " +
                   " where campaign_gid= '" + campaign_gid + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.campaign_name = objOdbcDataReader["campaign_title"].ToString();
                }



                msSQL = "  select distinct a.campaign_gid,a.employee_gid,concat(c.user_firstname, '/', c.user_code) as user," +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid " +
                    " where x.assign_to = a.employee_gid and b.leadbank_name is not null and(x.leadstage_gid='1' or x.leadstage_gid='2' or x.leadstage_gid='3'or " +
                    "  x.leadstage_gid='5'or x.leadstage_gid='6')and x.campaign_gid = a.campaign_gid) as total, " +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid  " +
                    " where x.assign_to = a.employee_gid and x.leadbank_gid  not in ( select leadbank_gid from crm_trn_tcalllog) and (x.leadstage_gid ='1') and " +
                    " b.leadbank_name is not null and x.campaign_gid = a.campaign_gid) as newleads, " +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid " +
                    " where x.assign_to = a.employee_gid   and x.leadstage_gid ='2' and b.leadbank_name is not null and x.campaign_gid = a.campaign_gid) as followup," +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid where x.assign_to = a.employee_gid " +
                    " and x.leadstage_gid ='5' and b.leadbank_name is not null and x.campaign_gid = a.campaign_gid) as drop_status," +
                    "(SELECT count(x.lead2campaign_gid) FROM crm_trn_ttelelead2campaign x left join crm_trn_tleadbank b on x.leadbank_gid = b.leadbank_gid where x.assign_to = a.employee_gid " +
                    "and (x.leadstage_gid ='4'or x.leadstage_gid='3'or x.leadstage_gid='6') and b.leadbank_name is not null and x.campaign_gid = a.campaign_gid) as prospect " +
                    " from crm_trn_tteleteam2employee a left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                    " left join adm_mst_tuser c on c.user_gid=b.user_gid  " +
                    " where a.campaign_gid= '" + campaign_gid + "' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<detailtelacalllerteam_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new detailtelacalllerteam_list1
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            campaign_gid = dt["campaign_gid"].ToString(),
                            user = dt["user"].ToString(),
                            total = dt["total"].ToString(),
                            newleads = dt["newleads"].ToString(),
                            followup = dt["followup"].ToString(),
                            //potential = dt["potential"].ToString(),
                            prospect = dt["prospect"].ToString(),
                            drop_status = dt["drop_status"].ToString(),
                            //customer = dt["customer"].ToString(),
                            //visit = dt["visit"].ToString(),

                        });
                        values.detailtelacalllerteam_list1 = getModuleList;
                    }
                }

                dt_datatable.Dispose();
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Marketing Team count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

           public MdlTeleCallerTeamSummary DaGetUserSummary(detailtelacalllerteam_list1 values)
        {
            MdlTeleCallerTeamSummary objMdlCampaignSummary1 = new MdlTeleCallerTeamSummary();
            try
            {
                 
                msSQL = " select concat(b.user_firstname, ' ', b.user_lastname) as user, " +
                    " (select campaign_title from crm_trn_tteleteam where campaign_gid='" + values.campaign_gid + "') as campaign_name " +
                    " from hrm_mst_temployee a " +
                    " left join adm_mst_tuser b on a.user_gid = b.user_gid " +
                    " where employee_gid = '" + values.employee_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<detailtelacalllerteam_list1>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new detailtelacalllerteam_list1
                        {
                            campaign_name = dt["campaign_name"].ToString(),
                            user_name = dt["user"].ToString(),
                        });
                        objMdlCampaignSummary1.detailtelacalllerteam_list1 = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Purchase Liability Report Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            return objMdlCampaignSummary1;
        }

        public void DaGetAssignSummary(MdlTeleCallerTeamSummary values)
        {

            try
            {   
                    msSQL = "Select a.customer_type, DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,concat_ws( ' ',n.user_firstname, n.user_lastname,'/',n.user_code) as created_by, a.leadbank_gid,a.leadbank_name,a.remarks, " +
                        "a.customer_gid, concat_ws( ' / ',c.leadbankcontact_name, c.mobile, c.email) as contact_details,f.source_name, " +
                        "concat_ws( ' / ',d.region_name,d.city) as region_name " +
                        "From crm_trn_tleadbank a " +
                        "left join crm_trn_tleadbankcontact c on a.leadbank_gid = c.leadbank_gid " +
                        "left join crm_mst_tregion d on a.leadbank_region = d.region_gid " +
                        "left join crm_mst_tsource f on a.source_gid = f.source_gid " +
                        "left join hrm_mst_temployee m on a.created_by = m.employee_gid " +
                        "left join adm_mst_tuser n on m.user_gid = n.user_gid " +
                        "where  a.leadbank_gid not in (select leadbank_gid from crm_trn_ttelelead2campaign) and c.main_contact ='Y' and a.leadbank_gid not in (select leadbank_gid from crm_trn_tappointment)";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<detailtelacalllerteam_list1>();
                lsRowcount = dt_datatable.Rows.Count;

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new detailtelacalllerteam_list1
                        {
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            Rowcount = lsRowcount,
                        });
                        values.detailtelacalllerteam_list1 = getModuleList;
                    }
                }

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Purchase Liability Report Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetAssignLead(string employee_gid, assignteam_list2 values)
        {

            try
            {

                for (int i = 0; i < values.summary_list2.ToArray().Length; i++)
                {

                    //msGetGid = objcmnfunctions.GetMasterGID("BLCC");
                    msSQL = " update crm_trn_tleadbank Set " +
                            " lead_status = 'Assigned' " +
                            " where leadbank_gid = '" + values.summary_list2[i].leadbank_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("BLCC");
                        msSQL1 = " Insert into crm_trn_ttelelead2campaign ( " +
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
                                "'" + values.summary_list2[i].leadbank_gid + "'," +
                                "'" + values.campaign_gid + "'," +
                                "'" + employee_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'Open'," +
                                "'" + values.summary_list2[i].schedule_remarks + "'," +
                                "'1'," +
                                "'" + values.employee_gid + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);

                        if (mnResult == 1)
                        {
                            msGetGid1 = objcmnfunctions.GetMasterGID("APMT");
                            msSQL1 = " Insert into crm_trn_tappointment ( " +
                               " appointment_gid, " +
                               " leadbank_gid, " +
                               " created_by, " +
                               " created_date, " +
                               " internal_notes, " +
                               " leadstage_gid, " +
                               " assign_to ) " +
                               " Values ( " +
                               "'" + msGetGid1 + "'," +
                               "'" + values.summary_list2[i].leadbank_gid + "'," +
                               "'" + employee_gid + "'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                               "'" + values.summary_list2[i].schedule_remarks + "'," +
                               "'0'," +
                               "'" + values.employee_gid + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);

                        }

                    }
                }

                if (mnResult == 1)
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
            catch (Exception ex)
            {
                values.message = "Exception occured while Assigning Lead to Employee!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //assign manager

        public void DaGetManagerUnassignedlist(string campaign_gid, string campaign_location, MdlTeleCallerTeamSummary values)
        {
            try
            {

               
                msSQL = " select distinct c.employee_gid,concat(d.user_code,'','/','',d.user_firstname,'-',d.user_lastname) as employee_name,'" + campaign_gid + "' as campaign_gid from adm_mst_tmodule2employee a " +
                        " left join hrm_mst_temployee c on a.employee_gid = c.employee_gid  left join adm_mst_tuser d on d.user_gid = c.user_gid " +
                        " left join crm_trn_tteleteam2employee e on e.employee_gid=a.employee_gid  where a.employee_gid not in " +
                        " (select employee_gid from cmn_trn_tmanagerprivilege where team_gid= '" + campaign_gid + "' ) and " +
                        " a.hierarchy_level<>'-1' and  a.module_gid = 'MKT' group by c.employee_gid ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetManagerUnassignedlist1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetManagerUnassignedlist1
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetManagerUnassignedlist1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Unassigned Manager List!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetManagerAssignedlist(string campaign_gid, string campaign_location, MdlTeleCallerTeamSummary values)
        {
            try
            {

                msSQL = " select distinct c.employee_gid,concat(d.user_code,'','/','',d.user_firstname,'-',d.user_lastname) as employee_name,'" + campaign_gid + "' as campaign_gid from adm_mst_tmodule2employee a " +
                         " left join hrm_mst_temployee c on a.employee_gid = c.employee_gid  left join adm_mst_tuser d on d.user_gid = c.user_gid " +
                         " left join crm_trn_tteleteam2employee e on e.employee_gid=a.employee_gid  where a.employee_gid in " +
                         " (select employee_gid from cmn_trn_tmanagerprivilege where team_gid= '" + campaign_gid + "' ) and " +
                         " a.hierarchy_level<>'-1' and  a.module_gid = 'MKT' group by c.employee_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetManagerAssignedlist1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetManagerAssignedlist1
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),

                        });
                        values.GetManagerAssignedlist1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Assigned Manager List!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaPostManagerAssignedlist(string user_gid, campaignassign_list values)
        {
            try
            {

                for (int i = 0; i < values.campaignassign.ToArray().Length; i++)
                {
                    msSQL = " SELECT employee_gid  FROM crm_trn_tteleteam2employee " +
                       " where campaign_gid = '" + values.campaignassign[i]._key3 + "'" +
                       " and employee_gid='" + values.campaignassign[i]._id + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count == 0)
                    {
                        msSQL = " insert into crm_trn_tteleteam2employee( " +
                             " campaign_gid, " +
                             " employee_gid, " +
                             " created_by, " +
                             " created_date) " +
                             " values( " +
                             "'" + values.campaignassign[i]._key3 + "'," +
                             "'" + values.campaignassign[i]._id + "'," +
                             " '" + user_gid + "', " +
                             " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    msSQL = " insert into cmn_trn_tmanagerprivilege( " +
                        " team_gid, " +
                        " employee_gid, " +
                        " module_gid, " +
                        " created_by, " +
                        " created_date) " +
                        " values( " +
                        "'" + values.campaignassign[i]._key3 + "'," +
                        "'" + values.campaignassign[i]._id + "'," +
                        " 'B2BTLCTCT', " +
                        " '" + user_gid + "', " +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = false;
                        values.message = "Manager Assigned Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Manager Assign";
                    }

                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Assigning Manager!";
            }

        }

        public void DaPostManagerUnassignedlist(string user_gid, campaignassign_list values)
        {
            try
            {
                if (values.campaignassign != null || values.campaign_gid != null || values.campaign_gid != "")
                {
                    msSQL = "DELETE FROM cmn_trn_tmanagerprivilege WHERE team_gid = '" + values.campaign_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        for (int i = 0; i < values.campaignassign.ToArray().Length; i++)
                        {
                            msSQL = " SELECT employee_gid  FROM crm_trn_tteleteam2employee " +
                                    " where campaign_gid = '" + values.campaignassign[i]._key3 + "'" +
                                    " and employee_gid='" + values.campaignassign[i]._id + "'";
                            dt_datatable = objdbconn.GetDataTable(msSQL);
                            if (dt_datatable.Rows.Count == 0)
                            {
                                msSQL = " insert into crm_trn_tteleteam2employee( " +
                                        " campaign_gid, " +
                                        " employee_gid, " +
                                        " created_by, " +
                                        " created_date) " +
                                        " values( " +
                                        "'" + values.campaignassign[i]._key3 + "'," +
                                        "'" + values.campaignassign[i]._id + "'," +
                                        " '" + user_gid + "', " +
                                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                                mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            msSQL = " insert into cmn_trn_tmanagerprivilege( " +
                                    " team_gid, " +
                                    " employee_gid, " +
                                    " module_gid, " +
                                    " created_by, " +
                                    " created_date) " +
                                    " values( " +
                                    "'" + values.campaignassign[i]._key3 + "'," +
                                    "'" + values.campaignassign[i]._id + "'," +
                                    " 'B2BTLCTCT', " +
                                    " '" + user_gid + "', " +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Records are Submitted Successfully ";

                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error Occured While submitting";
                            }
                        }

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occured";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "No records to process.";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while Unassigning Manager!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetManagerUnassigned(string campaign_gid, MdlTeleCallerTeamSummary values)
        {

            try
            {

                msSQL = " select campaign_location from crm_trn_tcampaign where campaign_gid='" + campaign_gid + " '  ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    lscampaign_location = objOdbcDataReader["campaign_location"].ToString();
                }
                msSQL = " select a.employee_gid,concat(c.user_code,'','/','',c.user_firstname,' ',c.user_lastname)AS employee_name,'" + campaign_gid + "' as campaign_gid" +
                    " from adm_mst_tmodule2employee a" +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " where a.module_gid = 'MKT'" +
                    " and b.branch_gid='" + lscampaign_location + "'" +
                    " and a.hierarchy_level<>'-1'" +
                    " and a.employee_gid not in" +
                    " (select employee_gid from" +
                    " cmn_trn_tmanagerprivilege" +
                    " where team_gid= '" + campaign_gid + "')" +
                    " group by employee_name";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetManagerUnassigned>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetManagerUnassigned
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetManagerUnassigned = getModuleList;
                    }
                }
                dt_datatable.Dispose();
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Unassigned Manager List!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetManager(string campaign_gid, MdlTeleCallerTeamSummary values)
        {
            try
            {

                msSQL = " select distinct c.employee_gid,concat(d.user_firstname,'/',d.user_lastname) as assign_manager,'" + campaign_gid + "' as campaign_gid from adm_mst_tmodule2employee a " +
                " left join hrm_mst_temployee c on a.employee_gid = c.employee_gid  left join adm_mst_tuser d on d.user_gid = c.user_gid " +
                " left join crm_trn_tteleteam2employee e on e.employee_gid=a.employee_gid  where a.employee_gid in " +
                " (select employee_gid from cmn_trn_tmanagerprivilege where team_gid= '" + campaign_gid + "' ) and " +
                " a.hierarchy_level<>'-1' and  a.module_gid = 'MKT' group by c.employee_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<managerlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new managerlist
                        {

                            assign_manager = dt["assign_manager"].ToString(),

                        });
                        values.manager_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaGetemployee(string campaign_gid, MdlTeleCallerTeamSummary values)
        {
            try
            {

                msSQL = "select a.campaign_gid ,a.employee_gid, concat(c.user_firstname, '/',c.user_code) as assign_employee from crm_trn_tteleteam2employee a " +
                    "left join hrm_mst_temployee b on a.employee_gid = b.employee_gid left join adm_mst_tuser c on c.user_gid=b.user_gid  " +
                    " where a.campaign_gid= '" + campaign_gid + "' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<employeelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employeelist
                        {

                            assign_employee = dt["assign_employee"].ToString(),

                        });
                        values.employee_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetassignlead(string campaign_gid, MdlTeleCallerTeamSummary values)
        {
            try
            {

                msSQL = "select lead2campaign_gid, concat(b.leadbank_name,' / ',c.leadbankcontact_name) as assign_lead from crm_trn_ttelelead2campaign a "+
                        " left join crm_trn_tleadbank b on a.leadbank_gid = b.leadbank_gid "+
                        " left join crm_trn_tleadbankcontact c on a.leadbank_gid = c.leadbank_gid "+
                        " where a.campaign_gid = '" + campaign_gid + "' and b.leadbank_name is not null ";


                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<employeelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employeelist
                        {

                            assign_lead = dt["assign_lead"].ToString(),

                        });
                        values.employee_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetSearchedLeads(string region_name, string source, string customer_type, MdlTeleCallerTeamSummary values)
        {

            try
            {
                msSQL = "Select a.customer_type, DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date,concat(n.user_firstname, ' ', n.user_lastname,'/',n.user_code) as created_by,  a.leadbank_gid,a.leadbank_name,a.remarks, " +
                        "a.customer_gid, concat(c.leadbankcontact_name, ' / ', c.mobile, ' / ', c.email) as contact_details,f.source_name, " +
                        "concat(d.region_name, ' / ', a.leadbank_city, ' / ', a.leadbank_state) as region_name " +
                        "From crm_trn_tleadbank a " +
                        "left join crm_trn_tleadbankcontact c on a.leadbank_gid = c.leadbank_gid " +
                        "left join crm_mst_tregion d on a.leadbank_region = d.region_gid " +
                        "left join crm_mst_tsource f on a.source_gid = f.source_gid " +
                        "left join hrm_mst_temployee m on a.created_by = m.employee_gid " +
                        "left join adm_mst_tuser n on m.user_gid = n.user_gid ";
                if (region_name != "null" || source != "null" || customer_type != "null")
                {
                    msSQL += "  where a.leadbank_gid not in (select leadbank_gid from crm_trn_tlead2campaign) and a.leadbank_gid not in (select leadbank_gid from crm_trn_ttelelead2campaign) ";
                    if (region_name != "null")
                    {
                        msSQL += "and a.leadbank_region = '" + region_name + "' ";
                    }
                    if (source != "null")
                    {
                        msSQL += "and a.source_gid = '" + source + "' ";
                    }
                    if (customer_type != "null")
                    {
                        msSQL += "and a.customertype_gid = '" + customer_type + "' ";
                    }
              
                }
                else
                {
                    msSQL += " where a.leadbank_gid not in (select leadbank_gid from crm_trn_tlead2campaign) and a.leadbank_gid not in (select leadbank_gid from crm_trn_ttelelead2campaign)";
                }

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<detailtelacalllerteam_list1>();
                lsRowcount = dt_datatable.Rows.Count;

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new detailtelacalllerteam_list1
                        {
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            Rowcount = lsRowcount,
                        });
                        values.detailtelacalllerteam_list1 = getModuleList;
                    }
                }

                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Purchase Liability Report Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

    }
}