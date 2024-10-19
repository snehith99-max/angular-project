using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace ems.sales.DataAccess
{
    public class DaSmrMstSalesteamSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lscampaign_location;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public string team_gid { get; private set; }

        public void DaGetSmrMstSalesteamSummary(MdlSmrMstSalesteamSummary values)
        {
            try
            {
               
                msSQL = " SELECT a.campaign_gid, a.campaign_title,b.branch_prefix,a.team_prefix, a.campaign_location,a.campaign_manager, concat(d.user_firstname,' ',d.user_lastname) as manager," +
                    " CASE  WHEN a.delete_flag = 'N' THEN 'Active'  ELSE 'InActive' END as statuses, " +
                    " b.branch_name, a.campaign_description, " +
                    " (SELECT count(lead2campaign_gid) FROM " +
                    " crm_trn_tenquiry2campaign k where k.campaign_gid = a.campaign_gid) as assigned_total, " +
                    " (SELECT count(employee_gid) FROM " +
                    " cmn_trn_tmanagerprivilege  where  team_gid = a.campaign_gid ) as manager_total, " +
                    " (SELECT count(employee_gid) FROM " +
                    " smr_trn_tcampaign2employee where campaign_gid = a.campaign_gid) as employee_total " +
                    " FROM smr_trn_tcampaign a " +
                    " left join hrm_mst_tbranch b on a.campaign_location = b.branch_gid " +
                    " left join hrm_mst_temployee c on a.campaign_manager = c.employee_gid " +
                    " left join adm_mst_tuser d on c.user_gid = d.user_gid " +
                    " order by campaign_gid desc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<salesteam_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new salesteam_list
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        branch_prefix = dt["branch_prefix"].ToString(),
                        campaign_title = dt["campaign_title"].ToString(),
                        team_prefix = dt["team_prefix"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        campaign_description = dt["campaign_description"].ToString(),
                        assigned_total = dt["assigned_total"].ToString(),
                        campaign_location = dt["campaign_location"].ToString(),
                        employee_gid = dt["campaign_manager"].ToString(),
                        statuses = dt["statuses"].ToString(),
                        employee_total = dt["employee_total"].ToString(),
                        manager_total = dt["manager_total"].ToString(),
                    });
                    values.salesteam_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Team Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetSmrMstSalesteamgrid(string compaign_gid, MdlSmrMstSalesteamSummary values)
        {
            try
            {
                msSQL = "select campaign_title from smr_trn_tcampaign " +
                   " where campaign_gid= '" + compaign_gid + "' ";


                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                   
                    values.campaign_name = objOdbcDataReader["campaign_title"].ToString();
                    
                }
                objOdbcDataReader.Close();

                msSQL = " select distinct a.campaign_gid,a.employee_gid,concat(c.user_firstname, '-', c.user_code) as user, " +
                    " ( SELECT count(x.lead2campaign_gid) FROM crm_trn_tenquiry2campaign x " +
                    " where x.assign_to = a.employee_gid and x.campaign_gid = a.campaign_gid) as total, " +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_tenquiry2campaign x " +
                    " where x.assign_to = a.employee_gid and (x.leadstage_gid ='1' or x.leadstage_gid is null) and x.campaign_gid = a.campaign_gid) as prospect, " +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_tenquiry2campaign x " +
                    " where x.assign_to = a.employee_gid and x.leadstage_gid ='5' and x.campaign_gid = a.campaign_gid) as potential, " +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_tenquiry2campaign x " +
                    " where x.assign_to = a.employee_gid and x.leadstage_gid ='3' and x.campaign_gid = a.campaign_gid) as completed, " +
                    " (SELECT count(x.lead2campaign_gid) FROM crm_trn_tenquiry2campaign x " +
                    " where x.assign_to = a.employee_gid and x.leadstage_gid ='4' and x.campaign_gid = a.campaign_gid) as drop_status " +
                    " from smr_trn_tcampaign2employee a inner join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                    " inner join adm_mst_tuser c on c.user_gid=b.user_gid " +
                    " where a.campaign_gid= '" + compaign_gid + "' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<salesteamgrid_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new salesteamgrid_list
                    {
                        employee_gid = dt["employee_gid"].ToString(),
                        campaign_gid = dt["campaign_gid"].ToString(),
                        user = dt["user"].ToString(),
                        total = dt["total"].ToString(),
                        prospect = dt["prospect"].ToString(),
                        potential = dt["potential"].ToString(),
                        completed = dt["completed"].ToString(),
                        drop_status = dt["drop_status"].ToString()
                    });
                    values.salesteamgrid_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
           
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Team Grid !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetemployee(MdlSmrMstSalesteamSummary values)
        {
            try
            {
               
                msSQL = " select a.employee_gid,concat(c.user_firstname,' ',c.user_lastname)As employee_name" +
                    " from adm_mst_tmodule2employee a" +
                    " inner join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " inner join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " where a.module_gid='SMR'  and c.user_status='Y' " +
                    " order by employee_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getemployee>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getemployee
                    {
                        employee_gid = dt["employee_gid"].ToString(),
                        employee_name = dt["employee_name"].ToString(),
                    });
                    values.Getemployee = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Employee !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void PostSalesTeam(string user_gid, salesteamgrid_list values)
        {
            try
            {
               
                msSQL = " select campaign_title from smr_trn_tcampaign where campaign_title = '" + values.team_name.Replace("'","\\\'") + "'";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                
            if (objOdbcDataReader.HasRows == true)
            {
                values.status = false;
                values.message = "Team Name Already Exists !!";
            }
            else
            {
                msGetGid = objcmnfunctions.GetMasterGID("BCNP");

                    msSQL = " insert into smr_trn_tcampaign(" +
                             " campaign_gid," +
                             " campaign_title," +
                             " team_prefix," +
                             " campaign_description," +
                             " campaign_location," +
                             " campaign_manager," +
                             " campaign_mailid," +
                             " created_by, " +
                             " created_date)" +
                             " values(" +
                             " '" + msGetGid + "'," +
                             " '" + values.team_name.Replace("'", "\\\'") + "'," +
                             " '" + values.team_prefix.Replace("'", "\\\'") + "',";
                             if( values.description != null)
                             {
                                  msSQL += " '" + values.description.Replace("'","\\\'") + "',";
                             }
                             else
                             {
                                msSQL += " '" + values.description + "',";
                             }
                       
                             msSQL+= "'" + values.branch_name + "'," +
                             "'" + values.employee_name + "'," +
                             "'" + values.mail_id + "'," +
                             "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    msSQL = " insert into cmn_trn_tmanagerprivilege( " +
                            " team_gid, " +
                            " employee_gid, " +
                            " module_gid, " +
                            " created_by, " +
                            " created_date) " +
                            " values( " +
                            "'" + msGetGid + "', " +
                            "'" + values.employee_name + "', " +
                            "'MKTCMPCPM', " +
                            "'" + user_gid + "', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        msSQL = " insert into smr_trn_tcampaign2employee( " +
                                " campaign_gid, " +
                                " employee_gid, " +
                                " created_by, " +
                                " created_date) " +
                                " values( " +
                                "'" + msGetGid + "'," +
                                "'" + values.employee_name + "'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                           values.status = true;
                           values.message = "Sales Team Added Successfully";
                        }
                    }                    
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Sales Team";
                }

            }
                objOdbcDataReader.Close();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Sales Team !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetEditSalesTeamSummary(string campaign_gid, MdlSmrMstSalesteamSummary values)
        {
            try
            {
                
                msSQL = " select a.campaign_title, a.campaign_location,a.team_prefix,a.campaign_gid, a.campaign_description, a.campaign_mailid," +
                    " concat(c.user_firstname,' - ',c.user_lastname) as campaign,a.campaign_manager " +
                    " from smr_trn_tcampaign a" +
                    " left join hrm_mst_temployee b on a.campaign_manager=b.employee_gid " +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " where campaign_gid='" + campaign_gid + "'";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<editsalesteam_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new editsalesteam_list
                    {
                        campaign_title = dt["campaign_title"].ToString(),
                        campaign_location = dt["campaign_location"].ToString(),
                        campaign_description = dt["campaign_description"].ToString(),
                        campaign_mailid = dt["campaign_mailid"].ToString(),
                        campaign_gid = dt["campaign_gid"].ToString(),
                        campaign_manager = dt["campaign_manager"].ToString(),
                        campaign = dt["campaign"].ToString(),
                        team_prefix = dt["team_prefix"].ToString(),
                    });
                    values.editsalesteam_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Editing Sales Team !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void PostUpdateSalesTeam(string user_gid, editsalesteam_list values)
        {
            try
            {
                // Retrieve the existing team name
                string msSQLGetExistingTeamName = "SELECT campaign_title FROM smr_trn_tcampaign WHERE campaign_gid = '" + values.campaign_gid + "'";
                string existingTeamName = objdbconn.GetExecuteScalar(msSQLGetExistingTeamName);

                // If the edited team name is the same as the existing one, skip the check
                if (existingTeamName != values.campaign_title)
                {
                    // Check if the new team name already exists
                    string msSQLCheckExistence = "SELECT COUNT(*) FROM smr_trn_tcampaign WHERE campaign_title = '" + values.campaign_title.Replace("'", "\\\'") + "'";
                    int existingTeamCount = Convert.ToInt32(objdbconn.GetExecuteScalar(msSQLCheckExistence));

                    if (existingTeamCount > 0)
                    {
                        values.status = false;
                        values.message = "Team Name Already Exists";
                        return;
                    }
                }

                // Update the sales team
                msSQL = "UPDATE smr_trn_tcampaign SET" +
                        " campaign_title = '" + values.campaign_title.Replace("'", "\\\'") + "'," +
                        " campaign_description = '" + values.campaign_description.Replace("'", "\\\'") + "'," +
                        " campaign_location = '" + values.campaign_location.Replace("'", "\\\'") + "'," +
                        " campaign_mailid = '" + values.campaign_mailid + "'," +
                        " team_prefix = '" + values.team_prefix + "'" +
                        " WHERE campaign_gid = '" + values.campaign_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Sales Team Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While updating Sales Team";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Sales Team !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetUnassignedEmplist(string campaign_gid, string campaign_location, MdlSmrMstSalesteamSummary values)
        {
            try
            {
               
                msSQL = " select a.employee_gid, concat(c.user_code,'/',c.user_firstname,' ',c.user_lastname) AS employee_name,'" + campaign_gid + "' as campaign_gid" +
                    " from adm_mst_tmodule2employee a" +
                    " inner join hrm_mst_temployee b on a.employee_gid = b.employee_gid" +
                    " inner join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " where a.module_gid = 'SMR'" +
                    " and b.branch_gid='" + campaign_location + "'" +
                    " and a.hierarchy_level<>'-1'" +
                    " and a.employee_gid not in" +
                    " (select employee_gid from" +
                    " smr_trn_tcampaign2employee" +
                    " where campaign_gid = '" + campaign_gid + "')" +
                    " group by employee_name asc";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetUnassignedEmplist>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetUnassignedEmplist
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        employee_gid = dt["employee_gid"].ToString(),
                        employee_name = dt["employee_name"].ToString(),
                    });
                    values.GetUnassignedEmplist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Unassigned Employee List !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetUnassignedlist(string campaign_gid,  MdlSmrMstSalesteamSummary values)
        {
            try
            {
               
                msSQL = " select campaign_location from smr_trn_tcampaign where campaign_gid='" + campaign_gid + " ' ";
            
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                   
                lscampaign_location = objOdbcDataReader["campaign_location"].ToString();
                    
            }
                objOdbcDataReader.Close();
                msSQL = " select a.employee_gid, concat(c.user_code,'','/','',c.user_firstname,' ',c.user_lastname)AS employee_name,'" + campaign_gid + "' as campaign_gid" +
                    " from adm_mst_tmodule2employee a" +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " where a.module_gid = 'SMR'" +                    
                    " and a.hierarchy_level<>'-1'" +
                    " and a.employee_gid not in" +
                    " (select employee_gid from" +
                    " smr_trn_tcampaign2employee" +
                    " where campaign_gid= '" + campaign_gid + "')" +
                    " group by employee_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetUnassignedlist>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetUnassignedlist
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        employee_gid = dt["employee_gid"].ToString(),
                        employee_name = dt["employee_name"].ToString(),
                    });
                    values.GetUnassignedlist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Unassigned List !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }        
        public void DaGetAssignedEmplist(string campaign_gid, string campaign_location, MdlSmrMstSalesteamSummary values)
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
                    " smr_trn_tcampaign2employee" +
                    " where campaign_gid= '" + campaign_gid + "')" +
                    " group by employee_name asc";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetAssignedEmplist>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetAssignedEmplist
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        employee_gid = dt["employee_gid"].ToString(),
                        employee_name = dt["employee_name"].ToString(),
                    });
                    values.GetAssignedEmplist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Assigned Employee List !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaPostAssignedEmplist(string user_gid, campaignassignemp_list values)
        {
            try
            {
                
                for (int i = 0; i < values.campaignunassignemp.ToArray().Length; i++)
            {
                msSQL = " insert into smr_trn_tcampaign2employee ( " +
                        " campaign_gid, " +
                        " employee_gid, " +
                        " created_by," +
                        " created_date ) " +
                        " values (  " +
                        " '" + values.campaignunassignemp[i]._key1 + "', " +
                        " '" + values.campaignunassignemp[i]._id + "', " +
                        " '" + user_gid + "', " +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Employee assigned successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Assigning Employee";
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Posting Employee list !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaPostUnassignedEmplist(string user_gid, campaignassignemp_list values)
        {
            try
            {
                if (values.campaignunassignemp != null || values.campaign_gid != null || values.campaign_gid != "")
                {
                    msSQL = "DELETE FROM smr_trn_tcampaign2employee WHERE campaign_gid = '" + values.campaign_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        for (int i = 0; i < values.campaignunassignemp.ToArray().Length; i++)
                        {

                            msSQL = " insert into smr_trn_tcampaign2employee ( " +
                                    " campaign_gid, " +
                                    " employee_gid, " +
                                    " created_by," +
                                    " created_date ) " +
                                    " values (  " +
                                    " '" + values.campaignunassignemp[i]._key1 + "', " +
                                    " '" + values.campaignunassignemp[i]._id + "', " +
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
                        values.message = "No records to process.";
                    }
                }
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while Posting Employee List !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        
        public void DaGetUnassignedManagerlist(string campaign_gid, MdlSmrMstSalesteamSummary values)
        {
            try
            {
              
                msSQL = " select a.employee_gid,concat(c.user_code,'','/','',c.user_firstname,' ',c.user_lastname)AS employee_name,'" + campaign_gid + "' as campaign_gid" +
                    " from adm_mst_tmodule2employee a" +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " where a.module_gid = 'SMR'" +                    
                    " and a.hierarchy_level<>'-1'" +
                    " and a.employee_gid not in" +
                    " (select employee_gid from" +
                    " cmn_trn_tmanagerprivilege" +
                    " where team_gid = '" + campaign_gid + "')" +
                    " group by employee_name asc";
            
            dt_datatable = objdbconn.GetDataTable(msSQL);
            
            var getModuleList = new List<GetUnassignedManagerlist>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetUnassignedManagerlist
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        employee_gid = dt["employee_gid"].ToString(),
                        employee_name = dt["employee_name"].ToString(),
                    });
                    values.GetUnassignedManagerlist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Unassigned Manager List !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetAssignedManagerlist(string campaign_gid, MdlSmrMstSalesteamSummary values)
        {
            try
            {
              
                msSQL = " select a.employee_gid,concat(c.user_code,'','/','',c.user_firstname,' ',c.user_lastname)AS employee_name,'" + campaign_gid + "' as campaign_gid" +
                    " from adm_mst_tmodule2employee a" +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " where a.module_gid = 'SMR'" +                    
                    " and a.hierarchy_level<>'-1'" +
                    " and a.employee_gid in " +
                    " (select employee_gid from" +
                    " cmn_trn_tmanagerprivilege" +
                    " where team_gid = '" + campaign_gid + "')" +
                    " group by employee_name asc";
            
            dt_datatable = objdbconn.GetDataTable(msSQL);
            
            var getModuleList = new List<GetAssignedManagerlist>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetAssignedManagerlist
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        employee_gid = dt["employee_gid"].ToString(),
                        employee_name = dt["employee_name"].ToString(),
                    });
                    values.GetAssignedManagerlist = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Assigned Manager List !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGetUnassignedManager(string campaign_gid, MdlSmrMstSalesteamSummary values)
        {
            try
            {
               
                msSQL = " select campaign_location from smr_trn_tcampaign where campaign_gid = '" + campaign_gid + "' ";
            
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

            if (objOdbcDataReader.HasRows)
            {
                    
                lscampaign_location = objOdbcDataReader["campaign_location"].ToString();
                   
            }
                objOdbcDataReader.Close();

                msSQL = " select a.employee_gid, concat(c.user_code,'','/','',c.user_firstname,' ', c.user_lastname) AS employee_name,'" + campaign_gid + "' as campaign_gid " +
                    " from adm_mst_tmodule2employee a " +
                    " left join hrm_mst_temployee b on a.employee_gid = b.employee_gid " +
                    " left join adm_mst_tuser c on b.user_gid = c.user_gid " +
                    " where a.module_gid = 'SMR' " +
                    " and b.branch_gid ='" + lscampaign_location + "'" +
                    " and a.hierarchy_level <> '-1'" +
                    " and a.employee_gid not in" +
                    " (select employee_gid from" +
                    " cmn_trn_tmanagerprivilege" +
                    " where team_gid= '" + campaign_gid + "')" +
                    " group by employee_name asc";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetUnassignedManager>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetUnassignedManager
                    {
                        campaign_gid = dt["campaign_gid"].ToString(),
                        employee_gid = dt["employee_gid"].ToString(),
                        employee_name = dt["employee_name"].ToString(),
                    });
                    values.GetUnassignedManager = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Unassigned Manager List !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        public void DaPostAssignedManagerlist(string user_gid, campaignassignmanager_list values)
        {
            try
            {
                if (values.campaignassignmanager != null || values.campaign_gid != null || values.campaign_gid != "")
                {

                    msSQL = "DELETE FROM cmn_trn_tmanagerprivilege WHERE team_gid = '" + values.campaign_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {

                        for (int i = 0; i < values.campaignassignmanager.ToArray().Length; i++)
                        {
                            msSQL = " SELECT employee_gid FROM smr_trn_tcampaign2employee " +
                                    " where campaign_gid = '" + values.campaignassignmanager[i]._key3 + "' " +
                                    " and employee_gid = '" + values.campaignassignmanager[i]._id + "' ";
                            dt_datatable = objdbconn.GetDataTable(msSQL);

                            if (dt_datatable.Rows.Count == 0)
                            {
                                msSQL = " insert into smr_trn_tcampaign2employee( " +
                                         " campaign_gid, " +
                                         " employee_gid, " +
                                         " created_by, " +
                                         " created_date) " +
                                         " values( " +
                                         "'" + values.campaignassignmanager[i]._key3 + "'," +
                                         "'" + values.campaignassignmanager[i]._id + "'," +
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
                                    "'" + values.campaignassignmanager[i]._key3 + "'," +
                                    "'" + values.campaignassignmanager[i]._id + "'," +
                                    " 'B2BCMPCPM', " +
                                    " '" + user_gid + "', " +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                values.status = true;
                                values.message = "Manager Assigned Successfully";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Assigning Manager";
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
                values.message = "Exception occured while Adding Assigned Manager List !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        
        public void DaPostUnassignedManagerlist(string user_gid, campaignassignmanager_list values)
        {
            try
            {
               
                for (int i = 0; i < values.campaignassignmanager.ToArray().Length; i++)
            {
                msSQL = " delete from cmn_trn_tmanagerprivilege " +
                        " where team_gid = '" + values.campaignassignmanager[i]._key3 + "' and employee_gid = '" + values.campaignassignmanager[i]._id + "' ";
                
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Manager Unassigned Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Unassigning Manager";
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Unassigned Manager List !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        // COUNT

        public void DaGetSmrTrnTeamCount(string employee_gid, string user_gid, MdlSmrMstSalesteamSummary values)
        {
            try
            {

                msSQL = " select(select count(campaign_gid) from smr_trn_tcampaign where campaign_title is not null) as teamcount," +
                        " (select count(customer_gid) from crm_mst_tcustomer where customer_gid is not null) as customercount," +
                        " (select count(k.lead2campaign_gid) FROM smr_trn_tcampaign a JOIN crm_trn_tenquiry2campaign k ON k.campaign_gid = a.campaign_gid) AS assigned_total ";



                dt_datatable = objdbconn.GetDataTable(msSQL);
                var customercount_list = new List<teamcount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        customercount_list.Add(new teamcount_list
                        {
                            employeecount = dt["teamcount"].ToString(),
                            customercount = dt["customercount"].ToString(),
                            assigned_count = dt["assigned_total"].ToString(),
                            //completed = dt["completed"].ToString(),
                            //drop_status = dt["drop_status"].ToString(),
                        });
                        values.teamcountlist = customercount_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Manager Count !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetTeamInactive(string campaign_gid, MdlSmrMstSalesteamSummary values)
        {
            try
            {

                msSQL = " update smr_trn_tcampaign set" +
                        " delete_flag='Y'" +
                        " where campaign_gid = '" + campaign_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Team Inactivated Successfully";
                    return;
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Team Inactivated";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Updating Customer Inactivated !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        public void DaGetTeamActive(string campaign_gid, MdlSmrMstSalesteamSummary values)
        {
            try
            {

                msSQL = " update smr_trn_tcampaign set" +
                        " delete_flag='N'" +
                        " where campaign_gid = '" + campaign_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Team Activated Successfully";
                    return;
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Activating Team";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Updating Customer Inactivated !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }


        public void DaGetManagers(string campaign_gid, MdlSmrMstSalesteamSummary values)
        {
            try
            {

                msSQL = "select distinct c.employee_gid,concat(d.user_firstname, ' ', d.user_lastname) as assign_manager, '" + campaign_gid + "' as campaign_gid " +
                        " from adm_mst_tmodule2employee a " +
                        " left join hrm_mst_temployee c on a.employee_gid = c.employee_gid " +
                        " left join adm_mst_tuser d on d.user_gid = c.user_gid " +
                        " where a.employee_gid in  (select employee_gid from cmn_trn_tmanagerprivilege where team_gid = '" + campaign_gid + "' ) and a.hierarchy_level<>'-1' and a.module_gid = 'SMR' group by c.employee_gid ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<countpopup_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new countpopup_list
                        {

                            assign_manager = dt["assign_manager"].ToString(),

                        });
                        values.countpopuplist = getModuleList;
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

        public void DaGetEmployee(string campaign_gid, MdlSmrMstSalesteamSummary values)
        {
            try
            {

                msSQL = "select a.campaign_gid ,a.employee_gid, concat(c.user_firstname, ' ',c.user_code) as assign_employee from smr_trn_tcampaign2employee a " +
                        " left join hrm_mst_temployee b on a.employee_gid = b.employee_gid left join adm_mst_tuser c on c.user_gid=b.user_gid  " +
                        " where a.campaign_gid= '" + campaign_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<countemp_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new countemp_list
                        {

                            assign_employee = dt["assign_employee"].ToString(),

                        });
                        values.countemplist = getModuleList;
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
    }
}