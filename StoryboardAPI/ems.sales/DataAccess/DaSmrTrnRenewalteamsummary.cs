using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using static ems.sales.Models.MdlSmrTrnRenewalteamsummary;
using ems.sales.Models;

namespace ems.sales.DataAccess
{
    public class DaSmrTrnRenewalteamsummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, msgetlead2campaign_gid, renewal_gid, lsentity_code, lsdesignation_code, lscampaign_title, lscampaign_location, lscampaign_gid, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, lsRowcount;


        public void DaGetTeamSummary(MdlSmrTrnRenewalteamsummary values)
        {
            try
            {

                msSQL = "SELECT a.campaign_gid, a.campaign_title,a.campaign_prefix, a.campaign_location," +
               " b.branch_name,b.branch_gid,b.branch_prefix,a.campaign_description,a.campaign_mailid, " +
               " (select count(employee_gid) from cmn_trn_tmanagerprivilege where team_gid = a.campaign_gid ) as total_managers," +
               " (SELECT  COUNT(employee_gid) FROM crm_trn_trenewal2employee WHERE campaign_gid = a.campaign_gid) AS total_employees " +
               "FROM crm_trn_trenewalteam a left join hrm_mst_tbranch b on a.campaign_location = b.branch_gid where 1 = 1 ORDER BY a.campaign_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<renewalteam_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new renewalteam_list
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            branch = dt["branch_name"].ToString(),
                            description = dt["campaign_description"].ToString(),
                            mail_id = dt["campaign_mailid"].ToString(),
                            team_name = dt["campaign_title"].ToString(),
                            team_prefix = dt["campaign_prefix"].ToString(),
                            //assigned_total = dt["assigned_total"].ToString(),
                            campaign_location = dt["campaign_location"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            total_managers = dt["total_managers"].ToString(),
                            total_employees = dt["total_employees"].ToString(),
                        });
                        values.renewalteam_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostRenewalteam(string user_gid, renewalteam_list values)
        {
            try
            {

                msSQL = " select campaign_title from crm_trn_trenewalteam where campaign_title = '" + values.team_name.Replace("'","\\\'") + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Team Name Already Exist !!";
                }
                else
                {

                    msGetGid = objcmnfunctions.GetMasterGID("BCNP");

                    msSQL = " insert into crm_trn_trenewalteam(" +
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
                              " '" + values.team_prefix.Replace("'", "\\\'") + "'," +
                             " '" + values.description.Replace("'", "\\\'") + "'," +
                             "'" + values.branch + "'," +
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
                                " 'RENTLRTRT', " +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = " insert into crm_trn_trenewal2employee( " +
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
                        values.message = "Renewal Team Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Renewal Team";
                    }

                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Renewal Team!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostAssignedlist(string user_gid, renewlassign_list values)
        {
            try
            {

                for (int i = 0; i < values.campaignassign.ToArray().Length; i++)
                {
                    msSQL = " insert into crm_trn_tcampaign2employee ( " +
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

        public void DaPostUnassignedlist(string user_gid, renewlassign_list values)
        {
            try
            {
                if (values.campaignassign != null || values.campaign_gid != null || values.campaign_gid != "")
                {
                    msSQL = "DELETE FROM crm_trn_trenewal2employee WHERE campaign_gid = '" + values.campaign_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        for (int i = 0; i < values.campaignassign.ToArray().Length; i++)
                        {

                            msSQL = " insert into crm_trn_trenewal2employee ( " +
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
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostManagerUnassignedlist(string user_gid, renewalunassign_list values)
        {
            try
            {
                if (values.campaignunassign != null || values.campaign_gid != null || values.campaign_gid != "")
                {
                    msSQL = "DELETE FROM cmn_trn_tmanagerprivilege WHERE team_gid = '" + values.campaign_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        for (int i = 0; i < values.campaignunassign.ToArray().Length; i++)
                        {
                            msSQL = " SELECT employee_gid  FROM crm_trn_trenewal2employee " +
                                  " where campaign_gid = '" + values.campaignunassign[i]._key3 + "'" +
                                  " and employee_gid='" + values.campaignunassign[i]._id + "'";
                            dt_datatable = objdbconn.GetDataTable(msSQL);
                            if (dt_datatable.Rows.Count == 0)
                            {
                                msSQL = " insert into crm_trn_trenewal2employee( " +
                                     " campaign_gid, " +
                                     " employee_gid, " +
                                     " created_by, " +
                                     " created_date) " +
                                     " values( " +
                                     "'" + values.campaignunassign[i]._key3 + "'," +
                                     "'" + values.campaignunassign[i]._id + "'," +
                                     " '" + user_gid + "', " +
                                     " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                                mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }
                            msSQL = "INSERT INTO cmn_trn_tmanagerprivilege (team_gid, employee_gid, module_gid, created_by, created_date) " +
                                     "VALUES ('" + values.campaignunassign[i]._key3 + "', '" + values.campaignunassign[i]._id + "', 'B2BCMPCPM', '" + user_gid + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
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
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetUnassignedlist(string campaign_gid, string campaign_location, MdlSmrTrnRenewalteamsummary values)
        {
            try
            {

                msSQL = " select a.employee_gid,concat(c.user_code,'','/','',c.user_firstname,' ',c.user_lastname)AS employee_name,'" + campaign_gid + "' as campaign_gid" +
              " from adm_mst_tmodule2employee a" +
              " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
              " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
              " where a.module_gid = 'SMR'" +
              " and b.branch_gid='" + campaign_location + "'" +
              " and a.hierarchy_level<>'-1'" +
              " and a.employee_gid not in" +
              " (select employee_gid from" +
              " crm_trn_trenewal2employee" +
              " where campaign_gid= '" + campaign_gid + "')";
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
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetAssignedlist(string campaign_gid, string campaign_location, MdlSmrTrnRenewalteamsummary values)
        {
            try
            {

                msSQL = " select a.employee_gid,concat(c.user_code,'','/','',c.user_firstname,' ',c.user_lastname)AS employee_name,'" + campaign_gid + "' as campaign_gid" +
                    " from adm_mst_tmodule2employee a" +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " where a.module_gid = 'SMR'" +
                    " and b.branch_gid='" + campaign_location + "'" +
                    " and a.hierarchy_level<>'-1'" +
                    " and a.employee_gid in " +
                    " (select employee_gid from" +
                    " crm_trn_trenewal2employee" +
                    " where campaign_gid= '" + campaign_gid + "')";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetAssignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAssignedlist
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetAssignedlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Assigned List Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetManagerUnassignedlist(string campaign_gid, string campaign_location, MdlSmrTrnRenewalteamsummary values)
        {
            try
            {

                msSQL = " select a.employee_gid,concat(c.user_code,'','/','',c.user_firstname,' ',c.user_lastname)AS employee_name,'" + campaign_gid + "' as campaign_gid" +
                " from adm_mst_tmodule2employee a" +
                " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                " where a.module_gid = 'SMR'" +
                " and b.branch_gid='" + campaign_location + "'" +
                " and a.hierarchy_level<>'-1'" +
                " and a.employee_gid not in" +
                " (select employee_gid from" +
                " cmn_trn_tmanagerprivilege" +
                " where team_gid= '" + campaign_gid + "')" +
                " group by  a.employee_gid, c.user_code, c.user_firstname," +
                " c.user_lastname, campaign_gid ORDER BY" +
                " employee_name ASC;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetManagerUnassignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetManagerUnassignedlist
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetManagerUnassignedlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Unassigned Manager List!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetManagerAssignedlist(string campaign_gid, string campaign_location, MdlSmrTrnRenewalteamsummary values)
        {
            try
            {

                msSQL = " select a.employee_gid,concat(c.user_code,'','/','',c.user_firstname,' ',c.user_lastname)AS employee_name,'" + campaign_gid + "' as campaign_gid" +
               " from adm_mst_tmodule2employee a" +
               " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
               " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
               " where a.module_gid = 'SMR'" +
               " and b.branch_gid='" + campaign_location + "'" +
               " and a.hierarchy_level<>'-1'" +
               " and a.employee_gid in " +
               " (select employee_gid from" +
               " cmn_trn_tmanagerprivilege" +
               " where team_gid= '" + campaign_gid + "')" +
               " group by a.employee_gid, c.user_code, c.user_firstname, c.user_lastname, " +
               "campaign_gid ORDER BY    employee_name ASC;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetManagerAssignedlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetManagerAssignedlist
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),

                        });
                        values.GetManagerAssignedlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Assigned Manager List!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaUpdatedrenewalteam(string user_gid, renewalteam_list values)
        {
            try
            {

                msSQL = " select campaign_title,campaign_gid from crm_trn_trenewalteam where campaign_title = '" + values.team_name_edit.Replace("'", "\\\'") + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    lscampaign_title = objOdbcDataReader["campaign_title"].ToString();
                    lscampaign_gid = objOdbcDataReader["campaign_gid"].ToString();
                    objOdbcDataReader.Close();
                }

                if (lscampaign_gid == values.campaign_gid)
                {

                    msSQL = " update  crm_trn_trenewalteam set " +
             " campaign_title  = '" + values.team_name_edit.Replace("'", "\\\'") + "'," +
             " campaign_prefix  = '" + values.team_prefix_edit.Replace("'", "\\\'") + "'," +
             " campaign_description  = '" + values.description_edit.Replace("'", "\\\'") + "'," +
              " campaign_location  = '" + values.branch_edit + "'," +
             " campaign_mailid  = '" + values.mail_id_edit + "'," +
             " updated_by = '" + user_gid + "'," +
             " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
             "' where campaign_gid='" + values.campaign_gid + "'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Renewal Team Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Renewal Team";
                    }
                }
                else
                {
                    if (lscampaign_title != values.team_name_edit)
                    {

                        msSQL = " update  crm_trn_trenewalteam set " +
                 " campaign_title  = '" + values.team_name_edit.Replace("'", "\\\'") + "'," +
                 " campaign_description  = '" + values.description_edit.Replace("'", "\\\'") + "'," +
                  " campaign_location  = '" + values.branch_edit + "'," +
                 " campaign_mailid  = '" + values.mail_id_edit + "'," +
                 " updated_by = '" + user_gid + "'," +
                 " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                 "' where campaign_gid='" + values.campaign_gid + "'  ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Renewal Team Updated Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Renewal Team !!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Same Renewal Team Already Exist !!";
                    }
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Renewal Team!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteTeam(string campaign_gid, renewalteam_list values)
        {

            try
            {

                // Check if the team is assigned for lead process
                msSQL = "select campaign_gid from crm_trn_trenewal2campaign where campaign_gid = '" + campaign_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                if (dt_datatable.Rows.Count > 0)
                {
                    //dt_datatable.Close();
                    objdbconn.CloseConn();
                    values.status = false;
                    values.message = "This team is assigned for lead process";
                }
                else
                {
                    // Check if the team has assigned employees
                    msSQL = "select employee_gid from crm_trn_trenewal2employee where campaign_gid='" + campaign_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable.Rows.Count > 0)
                    {
                        values.status = false;
                        values.message = "This Team cannot be deleted. Unassign all employees.";
                    }
                    else
                    {
                        // Check if there are assigned managers to the team
                        msSQL = "select employee_gid from cmn_trn_tmanagerprivilege where team_gid='" + campaign_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        if (dt_datatable.Rows.Count > 0)
                        {
                            values.status = false;
                            values.message = "Unassign all the manager in order to delete the team.";
                        }
                        else
                        {
                            // If all checks pass, proceed with deleting the team
                            msSQL = "delete from crm_trn_trenewalteam where campaign_gid='" + campaign_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                values.status = true;
                                values.message = "Renewal Team Deleted Successfully !!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Deleting Renewal Team !!";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Renewal Team!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetManagers(string campaign_gid, MdlSmrTrnRenewalteamsummary values)
        {
            try
            {

                msSQL = "select distinct c.employee_gid,concat(d.user_firstname,' ',d.user_lastname, '/',d.user_code ) as assign_manager, '" + campaign_gid + "' as campaign_gid " +
                        " from adm_mst_tmodule2employee a " +
                        " left join hrm_mst_temployee c on a.employee_gid = c.employee_gid " +
                        " left join adm_mst_tuser d on d.user_gid = c.user_gid " +
                        " where a.employee_gid in  (select employee_gid from cmn_trn_tmanagerprivilege where team_gid = '" + campaign_gid + "' ) and a.hierarchy_level<>'-1' and a.module_gid = 'SMR' group by c.employee_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<popup_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new popup_list
                        {

                            assign_manager = dt["assign_manager"].ToString(),

                        });
                        values.popup_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Managers list";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetEmployees(string campaign_gid, MdlSmrTrnRenewalteamsummary values)
        {
            try
            {

                msSQL = "select a.campaign_gid ,a.employee_gid, concat(c.user_firstname,' ',c.user_lastname, '/',c.user_code) as assign_employee from crm_trn_trenewal2employee a " +
                        " left join hrm_mst_temployee b on a.employee_gid = b.employee_gid left join adm_mst_tuser c on c.user_gid=b.user_gid  " +
                        " where a.campaign_gid= '" + campaign_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<popup_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new popup_list
                        {

                            assign_employee = dt["assign_employee"].ToString(),

                        });
                        values.popup_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Managers list";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetSmrRenewalassignlist( MdlSmrTrnRenewalteamsummary values)
        {
            try
            {
                msSQL = " SELECT distinct a.renewal_gid,a.customer_gid,a.renewal_type as renewal,(date_format(a.renewal_date,'%d-%m-%Y')) as renewal_date ,a.renewal_to,a.renewal_status, " +
                " concat(c.user_firstname, '-' ,c.user_lastname) as user_name ,d.customer_name, " +
                " concat(e.customercontact_name,' / ',e.mobile,' / ',e.email) as contact_details,  g.salesorder_gid," +
                " (date_format(g.salesorder_date,'%d-%m-%Y')) as salesorder_date, format(g.Grandtotal,2) as Grandtotal," +
                " a.renewal_description, a.created_by, a.created_date " +
                " from crm_trn_trenewal a " +
                " left join smr_trn_tsalesorder g on a.salesorder_gid = g.salesorder_gid " +
                "  left join hrm_mst_temployee b on a.renewal_to=b.employee_gid" +
                "  left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                "  left join crm_mst_tcustomer d on a.customer_gid =d.customer_gid" +
                 " left join crm_mst_tcustomercontact e on a.customer_gid = e.customer_gid" +
                 " where (a.renewal_to IS NULL OR TRIM(a.renewal_to) = '') and a.renewal_status<>'Closed' and a.renewal_type <> 'Agreement'" +
              "  Union " +
                " SELECT distinct a.renewal_gid,a.customer_gid,a.renewal_type as renewal,(date_format(a.renewal_date,'%d-%m-%Y')) as renewal_date ,a.renewal_to,a.renewal_status, " +
                 " concat(c.user_firstname, '-' ,c.user_lastname) as user_name ,d.customer_name, " +
                 " concat(e.customercontact_name,' / ',e.mobile,' / ',e.email) as contact_details,  g.agreement_gid," +
                 " (date_format(g.agreement_date,'%d-%m-%Y')) as salesorder_date, format(g.Grandtotal,2) as Grandtotal," +
                " a.renewal_description, a.created_by, a.created_date " +
                 " from crm_trn_trenewal a " +
                 " left join crm_trn_tagreement g on a.salesorder_gid = g.agreement_gid" +
                 " left join hrm_mst_temployee b on a.renewal_to=b.employee_gid " +
                 " left join adm_mst_tuser c on b.user_gid=c.user_gid " +
                 " left join crm_mst_tcustomer d on a.customer_gid =d.customer_gid " +
                 " left join crm_mst_tcustomercontact e on a.customer_gid = e.customer_gid" +
                 " left join crm_trn_trenewal2campaign co ON a.renewal_gid = co.renewal_gid " +
                 " where (a.renewal_to IS NULL OR TRIM(a.renewal_to) = '') and a.renewal_status<>'Closed' and a.renewal_type = 'Agreement'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<unmappedrenewal_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new unmappedrenewal_list
                        {

                            salesorder_date = dt["salesorder_date"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            renewal_gid = dt["renewal_gid"].ToString(),

                            customer_name = dt["customer_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            renewal_date = dt["renewal_date"].ToString(),
                            renewal = dt["renewal"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            renewal_description = dt["renewal_description"].ToString(),
                        });
                        values.unmappedrenewal_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
         
        }


        public void DaGetrenewalemployee(string campaign_gid, MdlSmrTrnRenewalteamsummary values)
        {
            try
            {

          msSQL = " SELECT a.campaign_gid,a.employee_gid,CONCAT(c.user_firstname, '-', c.user_code) AS user, " +
                  " (SELECT COUNT(lead2campaign_gid) FROM crm_trn_trenewal2campaign WHERE assign_to = a.employee_gid AND campaign_gid = a.campaign_gid) AS totalrenewal," +
                  " (SELECT COUNT(d.lead2campaign_gid) FROM crm_trn_trenewal2campaign d LEFT JOIN crm_trn_trenewal e ON e.renewal_gid = d.renewal_gid WHERE d.assign_to = a.employee_gid   " +
                  " AND e.renewal_status = 'closed' AND d.campaign_gid = a.campaign_gid) AS completed," +
                  " (SELECT COUNT(d.lead2campaign_gid) FROM crm_trn_trenewal2campaign d LEFT JOIN crm_trn_trenewal e ON e.renewal_gid = d.renewal_gid WHERE d.assign_to = a.employee_gid " +
                  " AND e.renewal_status = 'open' AND d.campaign_gid = a.campaign_gid) AS upcomming," +
                  " (SELECT COUNT(d.lead2campaign_gid) FROM crm_trn_trenewal2campaign d LEFT JOIN crm_trn_trenewal e ON e.renewal_gid = d.renewal_gid " +
                  " WHERE d.assign_to = a.employee_gid AND e.renewal_status = 'droped' AND d.campaign_gid = a.campaign_gid) AS dropped FROM crm_trn_trenewal2employee a  " +
                  " LEFT JOIN hrm_mst_temployee b ON a.employee_gid = b.employee_gid   LEFT JOIN adm_mst_tuser c ON c.user_gid = b.user_gid  " +
                  " where  a.campaign_gid = '" + campaign_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<renewalemployee_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new renewalemployee_list
                        {
                            user = dt["user"].ToString(),
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            totalrenewal = dt["totalrenewal"].ToString(),
                            upcomming = dt["upcomming"].ToString(),
                            completed = dt["completed"].ToString(),
                            dropped = dt["dropped"].ToString(),
                            
                        });
                        values.renewalemployee_list = getModuleList;
                    }
                }

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Team Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Renewal/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostMappedrenewalassign(string user_gid, mapproduct_lists1 values)
        {
            try
            {
                for (int i = 0; i < values.unmappedrenewal_list.ToArray().Length; i++)

                {

                    string msGetGid1 = objcmnfunctions.GetMasterGID("BLDP");
                    msSQL = " update crm_trn_trenewal Set " +
                            " assigned = 'Y' ," +
                            " renewal_to = '" + values.employee_gid + "'" +
                            " where renewal_gid = '" + values.unmappedrenewal_list[i].renewal_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msgetlead2campaign_gid = objcmnfunctions.GetMasterGID("BLDP");
                    msSQL = "INSERT INTO crm_trn_trenewal2campaign (" +
                                                    "lead2campaign_gid, " +
                                                    "leadbank_gid, " +
                                                    "campaign_gid, " +
                                                    "renewal_gid, " +
                                                    "created_by, " +
                                                    "assign_to, " +
                                                    "lead_status, " +
                                                    "created_date) " +
                                                    "VALUES (" +
                                                    "'" + msgetlead2campaign_gid + "', " +
                                                    "'" + values.campaign_gid + "', " +
                                                    "'" + values.campaign_gid + "', " +
                                                    "'" + values.unmappedrenewal_list[i].renewal_gid + "', " +
                                                    "'" + user_gid + "', " +                                                
                                                    "'" + values.employee_gid + "', " +
                                                    "'Open', " +
                                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Renewal Assign Succesfully !!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error while occured assigning Renewal ";
                    }
                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occurred while Mapping product !! " + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Shopify/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }


    }
}