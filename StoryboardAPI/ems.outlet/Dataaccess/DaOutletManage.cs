using ems.outlet.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using ems.system.Models;
namespace ems.outlet.Dataaccess
{
    public class DaOutletManage :ApiController
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult2;
        string msGetGid, msGetGid1, lsempoyeegid;
        public void DaGetoutletsummary(MdlOutletmanage values)
        {
            try
            {
                //msSQL = "select a.campaign_gid,a.campaign_title,a.branch,b.branch_gid,a.campaign_description," +
                //        " CASE  WHEN a.delete_flag = 'N' THEN 'Active'  ELSE 'InActive' END as outlet_status," +
                //        " from otl_trn_touletcampaign a " +
                //        "left join hrm_mst_tbranch b on a.branch_gid = b.branch_gid " +
                //        "order by a.campaign_gid asc ";

                msSQL = "SELECT a.campaign_gid,a.campaign_title,a.branch,b.branch_gid,a.campaign_description," +
                        "  CASE  WHEN a.delete_flag = 'N' THEN 'Active' ELSE 'InActive'  END as outlet_status," +
                        "   (SELECT DISTINCT COUNT(campaign2manager_gid) FROM otl_trn_tcampaign2manager k WHERE k.campaign_gid = a.campaign_gid) as managercount," +
                        "   (SELECT DISTINCT COUNT(campaign2employee_gid) FROM otl_trn_tcampaign2employee m WHERE m.campaign_gid = a.campaign_gid) AS employeecount " +
                        "FROM otl_trn_touletcampaign a " +
                        "LEFT JOIN hrm_mst_tbranch b ON a.branch_gid = b.branch_gid " +
                        "ORDER BY a.campaign_gid ASC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<campaign_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new campaign_list
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            campaign_title = dt["campaign_title"].ToString(),
                            branch = dt["branch"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            campaign_description = dt["campaign_description"].ToString(),
                            employeecount = dt["employeecount"].ToString(),
                            managercount = dt["managercount"].ToString(),
                            outlet_status = dt["outlet_status"].ToString(),


                        });
                        values.campaign_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Additional Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetOtlTrnOutletCount(string employee_gid, string user_gid, MdlOutletmanage values)
        {
            try
            {

                msSQL = " select(select count(campaign_gid) from otl_trn_touletcampaign where campaign_title is not null) as outletcount, "+
                        " (select DISTINCT count(campaign2manager_gid) from otl_trn_tcampaign2manager where campaign_gid is not null) as managercount, "+
                        " (select DISTINCT count(campaign2employee_gid) from otl_trn_tcampaign2employee where campaign_gid is not null) AS employeecount ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var customercount_list = new List<outletCountList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        customercount_list.Add(new outletCountList
                        {
                            outletcount = dt["outletcount"].ToString(),
                            managercount = dt["managercount"].ToString(),
                            employeecount = dt["employeecount"].ToString(),
                            //completed = dt["completed"].ToString(),
                            //drop_status = dt["drop_status"].ToString(),
                        });
                        values.outletCountList = customercount_list;
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
        public void DaGetoutletbranch(MdlOutletmanage values)
        {
            msSQL = " Select branch_name,branch_gid  " +
                    " from hrm_mst_tbranch ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<branch_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new branch_list
                    {
                        branch_name = dt["branch_name"].ToString(),
                        branch_gid = dt["branch_gid"].ToString(),
                    });
                    values.branch_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaPostOutlet(string user_gid, campaign_list values)
        {
            try
            {
                msSQL = " select * from otl_trn_touletcampaign where campaign_title = '" + values.campaign_title + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.status = false;
                    values.message = "Outlet Name Already Exist !!";
                }
                else
                {
                    msSQL = " Select branch_name   " +
                          " from hrm_mst_tbranch where branch_gid ='" + values.branch + "'";
                    string lsbranch =objdbconn.GetExecuteScalar(msSQL);
                    msGetGid = objcmnfunctions.GetMasterGID("OUCA");
                    msSQL = " insert into otl_trn_touletcampaign(" +
                          " campaign_gid," +
                          " campaign_title," +
                          " branch_gid," +
                          " branch," +
                          " campaign_description," +
                          " created_by, " +
                          " created_date)" +
                          " values(" +
                          " '" + msGetGid + "'," +
                          " '" + values.campaign_title + "'," +
                          " '" + values.branch + "'," +
                          " '" + lsbranch + "'," +
                          " '" + values.campaign_description + "',";
                    msSQL += "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Outlet Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Outlet";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Inserting Outlet Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
              ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaPostUpdateoutlet(string user_gid, campaign_list values)
        {
            try
            {
                msSQL = " Select branch_name   " +
                        " from hrm_mst_tbranch where branch_gid ='" + values.branch + "'";
                string lsbranch = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " update otl_trn_touletcampaign set" +
                        " campaign_title = '" + values.campaign_title + "'," +
                        " campaign_description = '" + values.campaign_description + "'," +
                        " branch = '" + lsbranch + "'," +
                        " branch_gid = '" + values.branch + "'" +
                        " where campaign_gid = '" + values.campaign_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Outlet Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While updating Outlet";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Outlet !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetotlUnassignedManagerlist(string campaign_gid, MdlOutletmanage values)
        {
            try
            {

                msSQL = " select a.employee_gid,c.user_code,concat(c.user_firstname,' ',c.user_lastname)AS employee_name,d.department_name,'" + campaign_gid + "' as campaign_gid" +
                    " from adm_mst_tmodule2employee a" +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " left join hrm_mst_tdepartment d on b.department_gid=d.department_gid" +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " where a.employee_gid  in" +
                    " (select employee_gid from" +
                    " otl_trn_tcampaign2manager" +
                    " where campaign_gid = '" + campaign_gid + "')" +
                    " group by user_code asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetOTLUnassignedManagerlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOTLUnassignedManagerlist
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            department_name = dt["department_name"].ToString(),
                        });
                        values.GetOTLUnassignedManagerlist = getModuleList;
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
        public void DaGetotlAssignedManagerlist(string campaign_gid, MdlOutletmanage values)
        {
            try
            {

                msSQL = " select a.employee_gid,c.user_code,concat(c.user_firstname,' ',c.user_lastname)AS employee_name,d.department_name,'" + campaign_gid + "' as campaign_gid" +
                    " from adm_mst_tmodule2employee a" +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " left join hrm_mst_tdepartment d on b.department_gid=d.department_gid" +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " where a.employee_gid not in " +
                    " (select employee_gid from" +
                    " otl_trn_tcampaign2manager" +
                    " where campaign_gid = '" + campaign_gid + "')" +
                    " group by user_code asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetOTLAssignedManagerlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOTLAssignedManagerlist
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            department_name = dt["department_name"].ToString(),
                        });
                        values.GetOTLAssignedManagerlist = getModuleList;
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
        public void DaPostoutAssignedManagerlist(string user_gid, outletassignmanager_list values)
        {
            try
            {
                if (values.GetOTLAssignedManagerlist != null || values.campaign_gid != null || values.campaign_gid != "")
                {
                    //msSQL = "DELETE FROM otl_trn_tcampaign2manager WHERE campaign_gid = '" + values.campaign_gid + "' ";
                    //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    //if (mnResult != 0)
                    //{
                    for(int i = 0; i < values.GetOTLAssignedManagerlist.Count; i++)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("OCMA");
                        msSQL = " insert into otl_trn_tcampaign2manager( " +
                                     " campaign2manager_gid, " +
                                     " campaign_gid, " +
                                     " employee_gid, " +
                                     " created_by, " +
                                     " created_date) " +
                                     " values( " +
                                     "'" + msGetGid + "'," +
                                     "'" + values.GetOTLAssignedManagerlist[i].campaign_gid + "'," +
                                     "'" + values.GetOTLAssignedManagerlist[i].employee_gid + "'," +
                                     " '" + user_gid + "', " +
                                     " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult2 != 0)
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
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while Posting Employee List !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostOutUnassignedManagerlist(string user_gid, outletassignmanager_list values)
        {
            try
            {
                for (int i = 0; i < values.GetOTLUnassignedManagerlist.Count; i++)
                {
                    msSQL = "delete from otl_trn_tcampaign2manager where campaign_gid ='" + values.GetOTLUnassignedManagerlist[i].campaign_gid + "'" +
                        " and employee_gid='"+ values.GetOTLUnassignedManagerlist[i].employee_gid + "'";
                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult2 != 0)
                    {
                        values.status = true;
                        values.message = "Manager UnAssigned Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While UnAssigning Manager";
                    }
                }
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while Adding UnAssigned Manager List !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetotlUnassignedEmplist(string campaign_gid, MdlOutletmanage values)
        {
            try
            {

                msSQL = " select a.employee_gid,c.user_code,concat(c.user_firstname,' ',c.user_lastname)AS employee_name,d.department_name,'" + campaign_gid + "' as campaign_gid" +
                    " from adm_mst_tmodule2employee a" +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " left join hrm_mst_tdepartment d on b.department_gid=d.department_gid" +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " where a.employee_gid  in" +
                    " (select employee_gid from" +
                    " otl_trn_tcampaign2employee" +
                    " where campaign_gid = '" + campaign_gid + "')" +
                    " group by employee_name asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetOTLUnassignedEmplist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOTLUnassignedEmplist
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetOTLUnassignedEmplist = getModuleList;
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
        public void DaGetotlAssignedEmplist(string campaign_gid, MdlOutletmanage values)
        {
            try
            {

                msSQL = " select a.employee_gid,c.user_code,concat(c.user_code,'','/','',c.user_firstname,' ',c.user_lastname)AS employee_name,d.department_name,'" + campaign_gid + "' as campaign_gid" +
                    " from adm_mst_tmodule2employee a" +
                    " left join hrm_mst_temployee b on a.employee_gid=b.employee_gid" +
                    " left join hrm_mst_tdepartment d on b.department_gid=d.department_gid" +
                    " left join adm_mst_tuser c on b.user_gid=c.user_gid" +
                    " where a.employee_gid not in " +
                    " (select employee_gid from" +
                    " otl_trn_tcampaign2employee" +
                    " where campaign_gid= '" + campaign_gid + "')" +
                    " group by employee_name asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetOTLAssignedEmplist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOTLAssignedEmplist
                        {
                            campaign_gid = dt["campaign_gid"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            employee_name = dt["employee_name"].ToString(),
                        });
                        values.GetOTLAssignedEmplist = getModuleList;
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
        public void DaPostOutAssignedEmplist(string user_gid, outletassignemployee_list values)
        {
            try
            {
                if (values.GetOTLUnassignedEmplist != null || values.campaign_gid != null || values.campaign_gid != "")
                {
                    //msSQL = "DELETE FROM otl_trn_tcampaign2employee WHERE campaign_gid = '" + values.campaign_gid + "' ";
                    //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    //if (mnResult != 0)
                    //{
                        for (int i = 0; i < values.GetOTLUnassignedEmplist.ToArray().Length; i++)
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("OCEA");
                            msSQL = " insert into otl_trn_tcampaign2employee( " +
                                         " campaign2employee_gid, " +
                                         " campaign_gid, " +
                                         " employee_gid, " +
                                         " created_by, " +
                                         " created_date) " +
                                         " values( " +
                                         "'" + msGetGid + "'," +
                                         "'" + values.GetOTLUnassignedEmplist[i].campaign_gid + "'," +
                                         "'" + values.GetOTLUnassignedEmplist[i].employee_gid + "'," +
                                         " '" + user_gid + "', " +
                                         " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' )";
                            mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult2 == 1)
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

                    //}
                    
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
        public void DaPostOutUnassignedEmplist(string user_gid, outletassignemployee_list values)
        {
            try
            {
                for (int i = 0; i < values.GetOTLAssignedEmplist.ToArray().Length; i++)
                {
                    msSQL = "delete from otl_trn_tcampaign2employee where campaign_gid ='" + values.GetOTLAssignedEmplist[i].campaign_gid + "'" +
                        " and employee_gid='" + values.GetOTLAssignedEmplist[i].employee_gid + "'";
                    mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult2 != 0)
                    {
                        values.status = true;
                        values.message = "Employee UnAssigned Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While UnAssigning Employee";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding UnAssigned Employee List !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetOutletInactive(string campaign_gid, MdlOutletmanage values)
        {
            try
            {

                msSQL = " update otl_trn_touletcampaign set" +
                        " delete_flag='Y'" +
                        " where campaign_gid = '" + campaign_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Outlet Inactivated Successfully";
                    return;
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Outlet Inactivated";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Updating Outlet Inactivated !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        public void DaGetOutletActive(string campaign_gid, MdlOutletmanage values)
        {
            try
            {
                msSQL = " update otl_trn_touletcampaign set" +
                        " delete_flag='N'" +
                        " where campaign_gid = '" + campaign_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Outlet Activated Successfully";
                    return;
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Activating Outlet";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Updating Outlet Inactivated !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void DaGetotlmanagergrid(string campaign_gid, MdlOutletmanage values)
        {
            try
            {
                msSQL = "select a.employee_gid,concat(c.user_code,'','/','',c.user_firstname,' ',c.user_lastname)AS manager from otl_trn_tcampaign2manager a "+
                        "left join hrm_mst_temployee b on a.employee_gid = b.employee_gid left join adm_mst_tuser c on b.user_gid = c.user_gid where " +
                        "a.campaign_gid = '"+ campaign_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ouletmanagergrid_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ouletmanagergrid_list
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            manager = dt["manager"].ToString(),
                        });
                        values.ouletmanagergrid_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Additional Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetotlemloyeegrid(string campaign_gid, MdlOutletmanage values)
        {
            try
            {
                msSQL = "select a.employee_gid,concat(c.user_code,'','/','',c.user_firstname,' ',c.user_lastname)AS employee from otl_trn_tcampaign2employee a " +
                        "left join hrm_mst_temployee b on a.employee_gid = b.employee_gid left join adm_mst_tuser c on b.user_gid = c.user_gid where " +
                        "a.campaign_gid = '" + campaign_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<outletemployeegrid_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new outletemployeegrid_list
                        {
                            employee_gid = dt["employee_gid"].ToString(),
                            employee = dt["employee"].ToString(),
                        });
                        values.outletemployeegrid_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Additional Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetoutletbranchuser(MdlOutletmanage values)
        {
            try
            {
                msSQL = " select campaign_title,campaign_gid, campaign_description, branch from  otl_trn_touletcampaign";
                var Getoutletbranchuser = new List<Getoutletbranchuser_list>();
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0) 
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        Getoutletbranchuser.Add(new Getoutletbranchuser_list
                        {
                            branch_name = dt["campaign_title"].ToString(),
                            branch_gid = dt["campaign_gid"].ToString(),
                        });
                        values.Getoutletbranchuser_list = Getoutletbranchuser;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Additional Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetUserSummary(string branch_gid,MdlOutletmanage values)
        {
            try
            {
                msSQL = " select distinct a.user_gid, c.useraccess, concat(a.user_code,' / ',a.user_firstname,' ',a.user_lastname) as user_name, c.employee_joiningdate, " +
                        " c.employee_gender, concat(j.address1,' ',j.address2,'/', j.city,'/', j.state,'/',k.country_name,'/', j.postal_code) as emp_address, " +
                        " d.designation_name, c.designation_gid, c.employee_gid, concat(n.entity_prefix) as entity_name, case when a.user_status = 'Y' " +
                        " then 'Active' when a.user_status = 'N' then 'Inactive' end as user_status, c.department_gid, c.branch_gid, e.campaign_title as branch_name, g.department_name  " +
                        " from adm_mst_tuser a left join hrm_mst_temployee c on a.user_gid = c.user_gid" +
                        " left join adm_mst_tentity n on n.entity_gid = c.entity_gid " +
                        " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid" +
                        " left join otl_trn_touletcampaign e on c.branch_gid = e.campaign_gid " +
                        " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                        "left join adm_mst_taddress j on c.employee_gid = j.parent_gid " +
                        " left join adm_mst_tcountry k on j.country_gid = k.country_gid" +
                        " left join hrm_trn_temployeedtl m on m.permanentaddress_gid = j.address_gid " +
                        " where c.branch_gid='" + branch_gid  +  "' group by c.employee_gid order by c.employee_gid desc ";                
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_employeelist = new List<Outletuser_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_employeelist.Add(new Outletuser_list
                        {
                            user_gid = dr_datarow["user_gid"].ToString(),
                            useraccess = dr_datarow["useraccess"].ToString(),
                            user_name = dr_datarow["user_name"].ToString(),
                            employee_joiningdate = dr_datarow["employee_joiningdate"].ToString(),
                            employee_gender = dr_datarow["employee_gender"].ToString(),
                            emp_address = dr_datarow["emp_address"].ToString(),
                            designation_name = dr_datarow["designation_name"].ToString(),
                            designation_gid = dr_datarow["designation_gid"].ToString(),
                            employee_gid =  dr_datarow["employee_gid"].ToString(),
                            entity_name = dr_datarow["entity_name"].ToString(),
                            user_status = dr_datarow["user_status"].ToString(),
                            department_gid = dr_datarow["department_gid"].ToString(),
                            department_name = dr_datarow["department_name"].ToString(),
                            branch_gid = dr_datarow["branch_gid"].ToString(),
                            branch_name = dr_datarow["branch_name"].ToString(),

                        });
                    }
                    values.Outletuser_list = get_employeelist;
                }
                dt_datatable.Dispose();               
            }
            catch (Exception ex)
            {
                ex.ToString();                
            }
        }
        public void DaGetUserEditSummary(string employee_gid, MdlOutletmanage values)
        {
            try
            {
                msSQL = " select a.employee_gid,a.employee_gender,z.entity_name,a.identity_no,date_format(a.employee_dob,'%d-%m-%Y') as employee_dob,a.employee_sign,a.bloodgroup, " +
                   " a.employee_image,a.employee_photo,b.user_password, " +
                   " a.employee_emailid,a.employee_mobileno,a.employee_qualification,a.employee_documents, " +
                   " (select address1 from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_address1, " +
                   " (select address2 from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_address2, " +
                   " (select city from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_city, " +
                   " (select state from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_state, " +
                   " (select postal_code from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_postalcode, " +
                   " (select address_gid from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_addressgid, " +
                   " (select n.country_name from adm_mst_taddress m LEFT JOIN adm_mst_tcountry n ON m.country_gid = n.country_gid   where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_country, " +
                   " (select r.country_gid from adm_mst_taddress q LEFT JOIN adm_mst_tcountry r ON q.country_gid = r.country_gid   where parent_gid = '" + employee_gid + "' and address_type = 'Permanent') as permanent_countrygid, " +
                   " (select address1 from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_address1, " +
                   " (select address2 from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_address2, " +
                   " (select city from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_city, " +
                   " (select state from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_state, " +
                   " (select postal_code from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_postalcode, " +
                   " (select address_gid from adm_mst_taddress where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_addressgid, " +
                   " (select p.country_name from adm_mst_taddress o LEFT JOIN adm_mst_tcountry p ON o.country_gid = p.country_gid   where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_country, " +
                   " (select t.country_gid from adm_mst_taddress s LEFT JOIN adm_mst_tcountry t ON s.country_gid = t.country_gid   where parent_gid = '" + employee_gid + "' and address_type = 'Temporary') as temporary_countrygid, " +
                   " a.employee_experience,a.employee_experiencedtl, a.employeereporting_to , a.employment_type , " +
                   " b.user_code,b.user_firstname,b.user_lastname, case when b.user_status = 'Y' then 'Active' when b.user_status = 'N' then 'In-Active' end as user_status,b.usergroup_gid,c.usergroup_code,a.entity_gid,a.designation_gid,a.department_gid, " +
                   " a.branch_gid,d.campaign_title,  e.department_name,f.designation_name,  " +
                   " (select i.user_firstname from adm_mst_tuser i ,  hrm_mst_temployee j where i.user_gid = j.user_gid " +
                   " and a.employeereporting_to = j.employee_gid)  as approveby_name,(date_format(a.employee_joiningdate,'%d/%m/%Y')) as employee_joiningdate, " +
                  " ( Select k.user_firstname from adm_mst_tuser k ,hrm_mst_temployee l " +
                  "  where k.user_gid = l.user_gid and l.employee_gid = '" + employee_gid + "')  as approver_name,a.nationality,a.nric_no " +
                   " FROM hrm_mst_temployee a  LEFT JOIN adm_mst_tuser b ON a.user_gid = b.user_gid " +
                   " LEFT JOIN adm_mst_tusergroup c ON b.usergroup_gid = c.usergroup_gid " +
                   " LEFT JOIN otl_trn_touletcampaign d ON a.branch_gid = d.campaign_gid " +
                   " LEFT JOIN hrm_mst_tdepartment e ON a.department_gid = e.department_gid  " +
                   " LEFT JOIN adm_mst_tdesignation f ON a.designation_gid = f.designation_gid " +
                   " LEFT JOIN hrm_mst_tjobtype g ON a.jobtype_gid = g.jobtype_gid " +
                   " left join adm_mst_tentity z on z.entity_gid=a.entity_gid " +
                   " WHERE a.employee_gid = '" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOtlEdituserSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOtlEdituserSummary
                        {


                            employee_gid = dt["employee_gid"].ToString(),
                            employee_gender = dt["employee_gender"].ToString(),
                            entity_name = dt["entity_name"].ToString(),
                            identity_no = dt["identity_no"].ToString(),
                            employee_dob = dt["employee_dob"].ToString(),
                            employee_sign = dt["employee_sign"].ToString(),
                            bloodgroup = dt["bloodgroup"].ToString(),
                            entity_gid = dt["entity_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            designation_gid = dt["designation_gid"].ToString(),
                            employee_photo = dt["employee_photo"].ToString(),
                            employee_image = dt["employee_image"].ToString(),
                            employee_emailid = dt["employee_emailid"].ToString(),
                            employee_mobileno = dt["employee_mobileno"].ToString(),
                            employee_documents = dt["employee_documents"].ToString(),
                            employee_experience = dt["employee_experience"].ToString(),
                            employee_experiencedtl = dt["employee_experiencedtl"].ToString(),
                            employeereporting_to = dt["employeereporting_to"].ToString(),
                            employment_type = dt["employment_type"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            user_lastname = dt["user_lastname"].ToString(),
                            user_status = dt["user_status"].ToString(),
                            usergroup_gid = dt["usergroup_gid"].ToString(),
                            usergroup_code = dt["usergroup_code"].ToString(),
                            branch_name = dt["campaign_title"].ToString(),
                            department_name = dt["department_name"].ToString(),
                            approveby_name = dt["approveby_name"].ToString(),
                            employee_joiningdate = dt["employee_joiningdate"].ToString(),
                            approver_name = dt["approver_name"].ToString(),
                            nationality = dt["nationality"].ToString(),
                            permanent_countrygid = dt["permanent_countrygid"].ToString(),
                            temporary_countrygid = dt["temporary_countrygid"].ToString(),
                            nric_no = dt["nric_no"].ToString(),
                            permanent_address1 = dt["permanent_address1"].ToString(),
                            permanent_address2 = dt["permanent_address2"].ToString(),
                            permanent_city = dt["permanent_city"].ToString(),
                            permanent_state = dt["permanent_state"].ToString(),
                            permanent_postalcode = dt["permanent_postalcode"].ToString(),
                            permanent_country = dt["permanent_country"].ToString(),
                            permanent_addressgid = dt["permanent_addressgid"].ToString(),
                            temporary_address1 = dt["temporary_address1"].ToString(),
                            temporary_address2 = dt["temporary_address2"].ToString(),
                            temporary_city = dt["temporary_city"].ToString(),
                            temporary_postalcode = dt["temporary_postalcode"].ToString(),
                            temporary_country = dt["temporary_country"].ToString(),
                            temporary_state = dt["temporary_state"].ToString(),
                            temporary_addressgid = dt["temporary_addressgid"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            user_password = dt["user_password"].ToString(),


                        });
                        values.GetOtlEdituserSummary = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}