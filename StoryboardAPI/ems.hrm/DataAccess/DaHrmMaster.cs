﻿using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using ems.hrm.Models;
using ems.hrm.DataAccess;
using MySql.Data.MySqlClient;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.hrm.DataAccess
{
    public class DaHrmMaster
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        Fnazurestorage objcmnstorage = new Fnazurestorage();
        DataTable dt_datatable;
        OdbcDataReader objMySqlDataReader, objMySqlDataReader1;
        string msSQL, msGetGid, msGetcodeGid,msGetteam2member_gid,
        msGet_LocationGid, clusterGID, msGet_clusterGid, regionGID, msGet_regionGid, msGetTaskCode, msGetUserCode,
        msGetTask2AssignedToGid, msGetTask2EscalationMailToGid, msGetHRCode, msGetHR2NotifyToGid, msGetAPICode, msGetsystem_ownername_gid;
        int mnResult, mnResultSub1, mnResultSub2;
        string lslevelonemodule_gid, module_gid, lsleveltwomodule_gid, module_gid_parent, lsleveltwomodulestatus_gid, lslevelonemodulestatus_gid;
        string lsmaster_value, lslms_code, lsbureau_code, lsbase_value, lssalutation_value, lsproject_value, lsbloodgroup_value, lsdocumentgid;
        string lsleveloneparent_gid, lsleveltwoparent_gid, lslevelthreeparent_gid, lsleveltwo_name, lslevelone_name,
           lslevelthree_name1, lsleveltwo_name1, lslevelone_name1;
        string lscreated_date, lscreated_by, lsleveltwomodule1_gid, lslevelthreeparent1_gid, lsleveltwoparent1_gid, lsleveloneparent1_gid,
             lsleveltwomodulemenu_gid, lslevelonemodulemenu_gid, lsuser_gid;
        string lsemployee_gid, lsemployee_name, lsemployeegroup_gid, lsemployeegroup_name, lslevelfourparent_gid, lslevelthree_name, lslevelthreemodule_gid;
        string lsuser_code, lsexternalsystem_name;

        public string msGetSeqGid { get; private set; }
        public string lsentity_code { get; private set; }

        public void DaCreateSubFunction(master1 values, string employee_gid)
        {
            try
            {
                
                string subfunction_name;

                msSQL = " SELECT subfunction_name FROM sys_mst_tsubfunction where delete_flag='N'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                //var getSegment = new List<CalendarGroupComparison_List>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {

                        subfunction_name = (dr_datarow["subfunction_name"].ToString());

                        if (subfunction_name == values.subfunction_name)
                        {
                            values.message = "This Sub Function Already Exists";
                            values.status = false;
                            return;
                        }
                    }

                    dt_datatable.Dispose();
                }
                msGetGid = objcmnfunctions.GetMasterGID("SCRT");
                msGetAPICode = objcmnfunctions.GetApiMasterGID("SUBF");
                msSQL = " insert into sys_mst_tsubfunction(" +
                        " subfunction_gid  ," +
                         " api_code," +
                        " subfunction_name  ," +
                        " created_by," +
                        " created_date)" +
                        " values(" +
                        "'" + msGetGid + "'," +
                        "'" + msGetAPICode + "'," +
                        "'" + values.subfunction_name.Replace("'", "\\'") + "'," +
                        "'" + employee_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;

                    values.message = "Sub Function Added Successfully";

                    values.message = "Sub Function Added Successfully";

                    values.message = "Sub Function Added Successfully";

                }
                else
                {
                    values.message = "Error Occured while Adding";
                    values.status = false;
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetSubFunction(MdlHrmMaster objmaster)
        {
            try
            {
                
                msSQL = " SELECT a.subfunction_gid ,a.subfunction_name , date_format(a.created_date,'%d-%m-%Y %h:%i %p') as created_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as status" +
                        " FROM sys_mst_tsubfunction a" +
                        " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid where a.delete_flag='N' order by a.subfunction_gid   desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmaster_list = new List<master_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmaster_list.Add(new master_list
                        {
                            subfunction_gid = (dr_datarow["subfunction_gid"].ToString()),

                            subfunction_name = (dr_datarow["subfunction_name"].ToString()),
                            created_by = (dr_datarow["created_by"].ToString()),
                            created_date = (dr_datarow["created_date"].ToString()),
                            status = (dr_datarow["status"].ToString()),
                        });
                    }
                    objmaster.master_list = getmaster_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch (Exception ex)
            {
                objmaster.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaEditSubFunction(string subfunction_gid, master1 values)
        {
            try
            {
                
                msSQL = " SELECT subfunction_gid ,subfunction_name , status as Status FROM sys_mst_tsubfunction " +
                        " where subfunction_gid ='" + subfunction_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    values.subfunction_gid = objMySqlDataReader["subfunction_gid"].ToString();
                    values.subfunction_name = objMySqlDataReader["subfunction_name"].ToString();
                    values.Status = objMySqlDataReader["Status"].ToString();
                }
                objMySqlDataReader.Close();
                values.status = true;

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteSubFunction(string subfunction_gid, string employee_gid, master1 values)
        {
            try
            {
                
                msSQL = " update sys_mst_tsubfunction  set delete_flag='Y'," +
                    " deleted_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                   " deleted_by='" + employee_gid + "'" +
                   " where subfunction_gid='" + subfunction_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = "Sub Function Deleted Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaSubFunctionInactiveLogview(string subfunction_gid, MdlHrmMaster values)
        {
            try
            {
                
                msSQL = " SELECT a.subfunction_gid ,date_format(a.updated_date,'%d-%m-%Y %h:%i %p') as updated_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as updated_by," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as Status, a.remarks" +
                        " FROM sys_mst_tsubfunctioninactivelog a" +
                        " left join hrm_mst_temployee b on a.updated_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                        " where a.subfunction_gid  ='" + subfunction_gid + "' order by a.subfunctioninactivelog_gid    desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmaster_list = new List<master_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmaster_list.Add(new master_list
                        {
                            subfunction_gid = (dr_datarow["subfunction_gid"].ToString()),
                            updated_by = (dr_datarow["updated_by"].ToString()),
                            updated_date = (dr_datarow["updated_date"].ToString()),
                            status = (dr_datarow["Status"].ToString()),
                            remarks = (dr_datarow["remarks"].ToString()),
                        });
                    }
                    values.master_list = getmaster_list;
                }
                dt_datatable.Dispose();
                values.status = true;
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaInactiveSubFunction(master1 values, string employee_gid)
        {
            try
            {
                
                msSQL = " update sys_mst_tsubfunction set status ='" + values.rbo_status + "'," +
                    " remarks='" + values.remarks.Replace("'", "\\'") + "'" +
                    " where subfunction_gid ='" + values.subfunction_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("SCRI");

                    msSQL = " insert into sys_mst_tsubfunctioninactivelog (" +
                          " subfunctioninactivelog_gid   , " +
                          " subfunction_gid ," +
                          " subfunction_name  ," +
                          " status," +
                          " remarks," +
                          " updated_by," +
                          " updated_date) " +
                          " values (" +
                          " '" + msGetGid + "'," +
                          " '" + values.subfunction_gid + "'," +
                          " '" + values.subfunction_name.Replace("'", "\\'") + "'," +
                          " '" + values.rbo_status + "'," +
                          " '" + values.remarks.Replace("'", "\\'") + "'," +
                          " '" + employee_gid + "'," +
                          " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (values.rbo_status == 'N')
                    {
                        values.status = false;
                        values.message = "Sub Function Inactivated Successfully";
                    }
                    else
                    {
                        values.status = true;
                        values.message = "Sub Function Activated Successfully";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateSubFunction(string employee_gid, master1 values)
        {
            try
            {
                
                string subfunction_name;

                msSQL = " SELECT subfunction_name FROM sys_mst_tsubfunction where delete_flag='N' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                //var getSegment = new List<CalendarGroupComparison_List>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {

                        //getSegment.Add(new EncoreProductComparison_List
                        //{


                        subfunction_name = (dr_datarow["subfunction_name"].ToString());

                        if (subfunction_name == values.subfunction_name)
                        {
                            values.message = "This Sub Function Already Exists";
                            values.status = false;
                            return;
                        }
                    }

                    dt_datatable.Dispose();
                }

                msSQL = "select updated_by, updated_date,subfunction_gid from sys_mst_tsubfunction where subfunction_gid  ='" + values.subfunction_gid + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    string lsUpdatedBy = objMySqlDataReader["updated_by"].ToString();
                    string lsUpdatedDate = objMySqlDataReader["updated_date"].ToString();

                    if (!(String.IsNullOrEmpty(lsUpdatedBy)) && !(String.IsNullOrEmpty(lsUpdatedDate)))
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("SCRL");
                        msSQL = " insert into sys_mst_tsubfunctionlog(" +
                                  " subfunction_loggid   ," +
                                  " subfunction_gid ," +
                                  " subfunction_name , " +
                                  " created_by, " +
                                  " created_date) " +
                                  " values(" +
                                  "'" + msGetGid + "'," +
                                  "'" + values.subfunction_gid + "'," +
                                  "'" + values.subfunction_name.Replace("'", "\\'") + "'," +
                                  "'" + employee_gid + "'," +
                                  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                objMySqlDataReader.Close();
                msSQL = " update sys_mst_tsubfunction set " +
                        " subfunction_name ='" + values.subfunction_name.Replace("'", "\\'") + "'," +
                         " updated_by='" + employee_gid + "'," +
                         " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                         " where subfunction_gid ='" + values.subfunction_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;

                    values.message = "Sub Function Updated Successfully";

                    values.message = "SubFunction Updated Successfully";

                    values.message = "Sub Function Updated Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While Updating";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetBaseLocationlist(MdlHrmMaster objmaster)
        {
            try
            {
                
                msSQL = " SELECT a.baselocation_gid ,a.baselocation_name " +
                        " FROM sys_mst_tbaselocation a" +
                        " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid where a.delete_flag='N' order by a.baselocation_gid  desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getlocation_list = new List<location_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getlocation_list.Add(new location_list
                        {
                            baselocation_gid = (dr_datarow["baselocation_gid"].ToString()),
                            baselocation_name = (dr_datarow["baselocation_name"].ToString()),

                        });
                    }
                    objmaster.location_list = getlocation_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch (Exception ex)
            {
                objmaster.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public bool DaPopSubfunction(MdlEmployeeOnboard objemployee_list)
        {
            try
            {
                
                msSQL = "select subfunction_gid,subfunction_name from sys_mst_tsubfunction where status='Y' and delete_flag='N'; ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_subfunction_list = new List<employee>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_row in dt_datatable.Rows)
                    {
                        get_subfunction_list.Add(new employee
                        {
                            subfunction_gid = dr_row["subfunction_gid"].ToString(),
                            subfunction_name = dr_row["subfunction_name"].ToString()
                        });
                    }
                    objemployee_list.employee = get_subfunction_list;
                    objemployee_list.status = true;
                    return true;
                }
                else
                {
                    objemployee_list.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objemployee_list.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return false;
            }
        }
        public void DaGetBaseLocation(MdlHrmMaster objmaster)
        {
            try
            {
                
                msSQL = " SELECT a.baselocation_gid ,a.api_code,a.baselocation_name, date_format(a.created_date,'%d-%m-%Y %h:%i %p') as created_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as status" +
                        " FROM sys_mst_tbaselocation a" +
                        " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid where a.delete_flag='N' order by a.baselocation_gid  desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmaster_list = new List<master_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmaster_list.Add(new master_list
                        {
                            baselocation_gid = (dr_datarow["baselocation_gid"].ToString()),
                            api_code = (dr_datarow["api_code"].ToString()),
                            baselocation_name = (dr_datarow["baselocation_name"].ToString()),
                            created_by = (dr_datarow["created_by"].ToString()),
                            created_date = (dr_datarow["created_date"].ToString()),
                            status = (dr_datarow["status"].ToString()),
                        });
                    }
                    objmaster.master_list = getmaster_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch (Exception ex)
            {
                objmaster.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetBaseLocationlistActive(MdlHrmMaster objmaster)
        {
            try
            {
                
                msSQL = " SELECT a.baselocation_gid ,a.baselocation_name " +
                        " FROM sys_mst_tbaselocation a" +
                        " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid where a.delete_flag='N' and status='Y' order by a.baselocation_gid  desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getlocation_list = new List<location_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getlocation_list.Add(new location_list
                        {
                            baselocation_gid = (dr_datarow["baselocation_gid"].ToString()),
                            baselocation_name = (dr_datarow["baselocation_name"].ToString()),

                        });
                    }
                    objmaster.location_list = getlocation_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch (Exception ex)
            {
                objmaster.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaCreateBaseLocation(master1 values, string employee_gid)
        {
            try
            {
                
                msSQL = "select baselocation_name from sys_mst_tbaselocation where baselocation_name = '" + values.baselocation_name.Replace("'", "\\'") + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Base Location Already Exist";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("SBLT");
                    msGetAPICode = objcmnfunctions.GetApiMasterGID("BSLN");
                    msSQL = " insert into sys_mst_tbaselocation(" +
                            " baselocation_gid ," +
                            " api_code," +
                            " baselocation_name ," +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid + "'," +
                           "'" + msGetAPICode + "'," +
                          "'" + values.baselocation_name.Replace("'", "") + "'," +
                            "'" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Base Location Added Successfully";
                    }
                    else
                    {
                        values.message = "Error Occured While Adding";
                        values.status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaEditBaseLocation(string baselocation_gid, master1 values)
        {
            try
            {
                
                msSQL = " SELECT baselocation_gid,baselocation_name, status as Status FROM sys_mst_tbaselocation " +
                        " where baselocation_gid='" + baselocation_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    values.baselocation_gid = objMySqlDataReader["baselocation_gid"].ToString();
                    values.baselocation_name = objMySqlDataReader["baselocation_name"].ToString();

                    //values.status_baselocation = objMySqlDataReader["status_baselocation"].ToString();
                    values.Status = objMySqlDataReader["status"].ToString();
                }
                objMySqlDataReader.Close();
                values.status = true;

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateBaseLocation(string employee_gid, master1 values)
        {
            try
            {
                
                msSQL = "select baselocation_gid from sys_mst_tbaselocation where delete_flag='N' and baselocation_name = '" + values.baselocation_name.Replace("'", "\\'") + "'";
                lsdocumentgid = objdbconn.GetExecuteScalar(msSQL);
                if (lsdocumentgid != "")
                {
                    if (lsdocumentgid != values.baselocation_gid)
                    {
                        values.message = "Base Location Already Exist";
                        values.status = false;
                        return;
                    }
                }

                msSQL = " update sys_mst_tbaselocation set " +
                " baselocation_name='" + values.baselocation_name.Replace("'", "") + "'," +
                " updated_by='" + employee_gid + "'," +
                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                " where baselocation_gid='" + values.baselocation_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    //msGetGid = objcmnfunctions.GetMasterGID("MELG");
                    msGetGid = objcmnfunctions.GetMasterGID("SBLL");
                    msSQL = " insert into sys_mst_tbaselocationlog(" +
                              " baselocation_loggid," +
                              " baselocation_gid," +
                              " baselocation_name , " +
                              " created_by, " +
                              " created_date) " +
                              " values(" +
                              "'" + msGetGid + "'," +
                              "'" + values.baselocation_gid + "'," +
                              "'" + lsbase_value + "'," +
                              "'" + employee_gid + "'," +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Base Location Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred While Updating";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaInactiveBaseLocation(master1 values, string employee_gid)
        {
            try
            {
                
                msSQL = " update sys_mst_tbaselocation set status ='" + values.rbo_status + "'," +
                    " remarks='" + values.remarks.Replace("'", "") + "'" +
                    " where baselocation_gid='" + values.baselocation_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("SBLI");

                    msSQL = " insert into sys_mst_tbaselocationinactivelog (" +
                          " baselocationinactivelog_gid   , " +
                          " baselocation_gid," +
                          " baselocation_name ," +
                          " status," +
                          " remarks," +
                          " updated_by," +
                          " updated_date) " +
                          " values (" +
                          " '" + msGetGid + "'," +
                          " '" + values.baselocation_gid + "'," +
                          " '" + values.baselocation_name + "'," +
                          " '" + values.rbo_status + "'," +
                          " '" + values.remarks.Replace("'", "") + "'," +
                          " '" + employee_gid + "'," +
                          " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (values.rbo_status == 'N')
                    {
                        values.status = true;
                        values.message = "Base Location Inactivated Successfully";
                    }
                    else
                    {
                        values.status = true;
                        values.message = "Base Location Activated Successfully";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteBaseLocation(string baselocation_gid, string employee_gid, master1 values)
        {
            try
            {
                
                msSQL = " update sys_mst_tbaselocation  set delete_flag='Y'," +
                    " deleted_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                   " deleted_by='" + employee_gid + "'" +
                   " where baselocation_gid='" + baselocation_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = "Base Location Deleted Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaBaseLocationInactiveLogview(string baselocation_gid, MdlHrmMaster values)
        {
            try
            {
                
                msSQL = " SELECT a.baselocation_gid,date_format(a.updated_date,'%d-%m-%Y %h:%i %p') as updated_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as updated_by," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as Status, a.remarks" +
                        " FROM sys_mst_tbaselocationinactivelog a" +
                        " left join hrm_mst_temployee b on a.updated_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                        " where a.baselocation_gid ='" + baselocation_gid + "' order by a.baselocationinactivelog_gid   desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmaster_list = new List<master_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmaster_list.Add(new master_list
                        {
                            baselocation_gid = (dr_datarow["baselocation_gid"].ToString()),
                            updated_by = (dr_datarow["updated_by"].ToString()),
                            updated_date = (dr_datarow["updated_date"].ToString()),
                            status = (dr_datarow["status"].ToString()),
                            remarks = (dr_datarow["remarks"].ToString()),
                        });
                    }
                    values.master_list = getmaster_list;
                }
                dt_datatable.Dispose();
                values.status = true;
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Blood Group
        public void DaGetBloodGroup(MdlHrmMaster objmaster)
        {
            try
            {
                
                msSQL = " SELECT a.bloodgroup_gid ,a.api_code,a.bloodgroup_name, date_format(a.created_date,'%d-%m-%Y %h:%i %p') as created_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as status" +
                        " FROM sys_mst_tbloodgroup a" +
                        " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid where a.delete_flag='N' order by a.bloodgroup_gid  desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmaster_list = new List<master_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmaster_list.Add(new master_list
                        {
                            bloodgroup_gid = (dr_datarow["bloodgroup_gid"].ToString()),
                            api_code = (dr_datarow["api_code"].ToString()),
                            bloodgroup_name = (dr_datarow["bloodgroup_name"].ToString()),
                            created_by = (dr_datarow["created_by"].ToString()),
                            created_date = (dr_datarow["created_date"].ToString()),
                            status = (dr_datarow["status"].ToString()),
                        });
                    }
                    objmaster.master_list = getmaster_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch (Exception ex)
            {
                objmaster.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetBloodGroupActive(MdlHrmMaster objmaster)
        {
            try
            {
                
                msSQL = " SELECT a.bloodgroup_gid, a.bloodgroup_name, date_format(a.created_date,'%d-%m-%Y') as created_date, a.created_by, a.status FROM sys_mst_tbloodgroup a order by a.bloodgroup_gid desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmaster_list = new List<master_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmaster_list.Add(new master_list
                        {
                            bloodgroup_gid = (dr_datarow["bloodgroup_gid"].ToString()),
                            bloodgroup_name = (dr_datarow["bloodgroup_name"].ToString()),
                            created_date = (dr_datarow["created_date"].ToString()),
                            created_by = (dr_datarow["created_by"].ToString()),
                            status = (dr_datarow["status"].ToString()),
                        });
                    }
                    objmaster.master_list = getmaster_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch (Exception ex)
            {
                objmaster.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaCreateBloodGroup(master1 values, string employee_gid)
        {
            try
            {
                
                msSQL = "select bloodgroup_name from sys_mst_tbloodgroup where bloodgroup_name = '" + values.bloodgroup_name.Replace("'", "\\'") + "' and delete_flag='N' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Blood Group Already Exist";
                }
                else
                {
                    msGetGid = objcmnfunctions.GetMasterGID("SBGT");
                    msGetAPICode = objcmnfunctions.GetApiMasterGID("BLOD");
                    msSQL = " insert into sys_mst_tbloodgroup(" +
                            " bloodgroup_gid ," +
                            " api_code," +
                            " bloodgroup_name ," +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid + "'," +
                            "'" + msGetAPICode + "'," +
                            "'" + values.bloodgroup_name.Replace("'", "") + "'," +
                            "'" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Blood Group Added Successfully";
                    }
                    else
                    {
                        values.message = "Error Occured while Adding";
                        values.status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaEditBloodGroup(string bloodgroup_gid, master1 values)
        {
            try
            {
                
                msSQL = " SELECT bloodgroup_gid,bloodgroup_name, status as Status FROM sys_mst_tbloodgroup " +
                        " where bloodgroup_gid='" + bloodgroup_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    values.bloodgroup_gid = objMySqlDataReader["bloodgroup_gid"].ToString();
                    values.bloodgroup_name = objMySqlDataReader["bloodgroup_name"].ToString();
                    values.Status = objMySqlDataReader["Status"].ToString();
                }
                objMySqlDataReader.Close();
                values.status = true;

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateBloodGroup(string employee_gid, master1 values)
        {
            try
            {
                
                msSQL = "select bloodgroup_gid from sys_mst_tbloodgroup where delete_flag='N' and bloodgroup_name = '" + values.bloodgroup_name.Replace("'", "\\'") + "'";
                lsdocumentgid = objdbconn.GetExecuteScalar(msSQL);
                if (lsdocumentgid != "")
                {
                    if (lsdocumentgid != values.bloodgroup_gid)
                    {
                        values.message = "Blood group Name Already Exist";
                        values.status = false;
                        return;
                    }
                }


                msSQL = " update sys_mst_tbloodgroup set " +
                " bloodgroup_name='" + values.bloodgroup_name.Replace("'", "") + "'," +
                " updated_by='" + employee_gid + "'," +
                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                " where bloodgroup_gid='" + values.bloodgroup_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult != 0)
                {
                    //msGetGid = objcmnfunctions.GetMasterGID("MELG");
                    msGetGid = objcmnfunctions.GetMasterGID("SBGL");
                    msSQL = " insert into sys_mst_tbloodgrouplog(" +
                              " bloodgroup_loggid   ," +
                              " bloodgroup_gid," +
                              " bloodgroup_name , " +
                              " created_by, " +
                              " created_date) " +
                              " values(" +
                              "'" + msGetGid + "'," +
                              "'" + values.bloodgroup_gid + "'," +
                              "'" + lsbloodgroup_value + "'," +
                              "'" + employee_gid + "'," +
                              "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Blood Group Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred While Updating";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaInactiveBloodGroup(master1 values, string employee_gid)
        {
            try
            {
                
                msSQL = " update sys_mst_tbloodgroup set status ='" + values.rbo_status + "'," +
                    " remarks='" + values.remarks.Replace("'", "") + "'" +
                    " where bloodgroup_gid='" + values.bloodgroup_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("SBGI");

                    msSQL = " insert into sys_mst_tbloodgroupinactivelog (" +
                          " bloodgroupinactivelog_gid   , " +
                          " bloodgroup_gid," +
                          " bloodgroup_name ," +
                          " status," +
                          " remarks," +
                          " updated_by," +
                          " updated_date) " +
                          " values (" +
                          " '" + msGetGid + "'," +
                          " '" + values.bloodgroup_gid + "'," +
                          " '" + values.bloodgroup_name + "'," +
                          " '" + values.rbo_status + "'," +
                          " '" + values.remarks.Replace("'", "") + "'," +
                          " '" + employee_gid + "'," +
                          " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (values.rbo_status == 'N')
                    {
                        values.status = true;
                        values.message = "Blood Group Inactivated Successfully";
                    }
                    else
                    {
                        values.status = true;
                        values.message = "Blood Group Activated Successfully";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteBloodGroup(string bloodgroup_gid, string employee_gid, master1 values)
        {
            try
            {
                
                msSQL = " update sys_mst_tbloodgroup   set delete_flag='Y'," +
                    " deleted_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                   " deleted_by='" + employee_gid + "'" +
                   " where bloodgroup_gid='" + bloodgroup_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = "Blood Group Deleted Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaBloodGroupInactiveLogview(string bloodgroup_gid, MdlHrmMaster values)
        {
            try
            {
                
                msSQL = " SELECT a.bloodgroup_gid,date_format(a.updated_date,'%d-%m-%Y %h:%i %p') as updated_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as updated_by," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as Status, a.remarks" +
                        " FROM sys_mst_tbloodgroupinactivelog a" +
                        " left join hrm_mst_temployee b on a.updated_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                        " where a.bloodgroup_gid ='" + bloodgroup_gid + "' order by a.bloodgroupinactivelog_gid    desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmaster_list = new List<master_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmaster_list.Add(new master_list
                        {
                            bloodgroup_gid = (dr_datarow["bloodgroup_gid"].ToString()),
                            updated_by = (dr_datarow["updated_by"].ToString()),
                            updated_date = (dr_datarow["updated_date"].ToString()),
                            status = (dr_datarow["Status"].ToString()),
                            remarks = (dr_datarow["remarks"].ToString()),
                        });
                    }
                    values.master_list = getmaster_list;
                }
                dt_datatable.Dispose();
                values.status = true;
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Task
        public void DaPostTaskAdd(MdlTask values, string employee_gid)
        {
            try
            {
                
                string lstat, lstask_description, lstask_name, lsteam_name = "";



                if (values.tat == null || values.tat == "")
                    lstat = "";
                else
                    lstat = values.tat.Replace("'", "\\'");
                if (values.task_description == null || values.task_description == "")
                    lstask_description = "";
                else
                    lstask_description = values.task_description.Replace("'", "\\'");
                if (values.task_name == null || values.task_name == "")
                    lstask_name = "";
                else
                    lstask_name = values.task_name.Replace("'", "\\'");
                if (values.team_name == null || values.team_name == "")
                    lsteam_name = "";
                else
                    lsteam_name = values.team_name.Replace("'", "\\'");


                msSQL = " SELECT task_name FROM sys_mst_ttask where task_name ='" + lstask_name + "'";
                string GetTaskName = objdbconn.GetExecuteScalar(msSQL);
                if (GetTaskName == lstask_name)
                {
                    values.message = "Task Name Already Exists";
                    values.status = false;
                    return;
                }

                msGetAPICode = objcmnfunctions.GetApiMasterGID("TAAC");
                msGetGid = objcmnfunctions.GetMasterGID("STSK");
                msGetTaskCode = objcmnfunctions.GetMasterGID("TSKC");
                msSQL = " insert into sys_mst_ttask(" +
                        " task_gid ," +
                        " api_code ," +
                        " task_code ," +
                        " task_name," +
                        " team_name," +
                        " team_gid," +
                        " task_description," +
                        " tat," +
                        " created_by," +
                        " created_date)" +
                        " values(" +
                        "'" + msGetGid + "'," +
                        "'" + msGetAPICode + "'," +
                        "'" + msGetTaskCode + "'," +
                        "'" + lstask_name + "'," +
                        "'" + lsteam_name + "'," +
                        "'" + values.team_gid + "'," +
                        "'" + lstask_description + "'," +
                        "'" + lstat + "'," +
                        "'" + employee_gid + "'," +
                          "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                //for (var i = 0; i < values.assigned_to.Count; i++)
                //{
                //    msGetTask2AssignedToGid = objcmnfunctions.GetMasterGID("TAST");
                //    msSQL = "Insert into sys_mst_ttask2assignedto( " +
                //           " task2assignedto_gid, " +
                //           " task_gid," +
                //           " assignedto_gid," +
                //           " assignedto_name," +
                //           " created_by," +
                //           " created_date)" +
                //           " values(" +
                //           "'" + msGetTask2AssignedToGid + "'," +
                //           "'" + msGetGid + "'," +
                //           "'" + values.assigned_to[i].employee_gid + "'," +
                //           "'" + values.assigned_to[i].employee_name + "'," +
                //           "'" + employee_gid + "'," +
                //           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                //    mnResultSub1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                //} 
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Task Added Successfully";
                }
                else
                {
                    values.message = "Error Occured While Adding Task";
                    values.status = false;
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetTaskSummary(MdlHrmMaster objmaster)
        {
            try
            {
                
                msSQL = " SELECT a.task_gid ,a.task_name,a.team_name,a.team_gid, date_format(a.created_date,'%d-%m-%Y %h:%i %p') as created_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by,api_code," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as status" +
                        " FROM sys_mst_ttask a" +
                        " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid where a.delete_flag='N' order by a.task_gid  desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmaster_list = new List<master_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmaster_list.Add(new master_list
                        {
                            task_gid = (dr_datarow["task_gid"].ToString()),
                            task_name = (dr_datarow["task_name"].ToString()),
                            team_name = (dr_datarow["team_name"].ToString()),
                            team_gid = (dr_datarow["team_gid"].ToString()),
                            created_by = (dr_datarow["created_by"].ToString()),
                            created_date = (dr_datarow["created_date"].ToString()),
                            status = (dr_datarow["status"].ToString()),
                            api_code = (dr_datarow["api_code"].ToString()),
                        });
                    }
                    objmaster.master_list = getmaster_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch (Exception ex)
            {
                objmaster.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaEditTask(string task_gid, MdlTask objmaster)
        {
            try
            {
                
                msSQL = " SELECT task_gid,task_code,task_name,team_name,team_gid,task_description,tat, status as Status FROM sys_mst_ttask " +
                    " where task_gid='" + task_gid + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    objmaster.task_gid = objMySqlDataReader["task_gid"].ToString();
                    objmaster.task_code = objMySqlDataReader["task_code"].ToString();
                    objmaster.task_name = objMySqlDataReader["task_name"].ToString();
                    objmaster.team_name = objMySqlDataReader["team_name"].ToString();
                    objmaster.team_gid = objMySqlDataReader["team_gid"].ToString();
                    objmaster.task_description = objMySqlDataReader["task_description"].ToString();
                    objmaster.tat = objMySqlDataReader["tat"].ToString();
                    objmaster.Status = objMySqlDataReader["Status"].ToString();
                }
                objMySqlDataReader.Close();

                //msSQL = " select assignedto_gid,assignedto_name from sys_mst_ttask2assignedto " +
                //" where task_gid='" + task_gid + "'";
                //dt_datatable = objdbconn.GetDataTable(msSQL);
                //var getassignedtoList = new List<assignedto_list>();
                //if (dt_datatable.Rows.Count != 0)
                //{
                //    foreach (DataRow dt in dt_datatable.Rows)
                //    {
                //        getassignedtoList.Add(new assignedto_list
                //        {
                //            employee_gid = dt["assignedto_gid"].ToString(),
                //            employee_name = dt["assignedto_name"].ToString(),
                //        });
                //        objmaster.assigned_to = getassignedtoList;
                //    }
                //}

                //msSQL = " SELECT a.user_firstname,a.user_gid , " +
                //    " concat(a.user_firstname,' ',a.user_lastname,' / ',a.user_code) as employee_name,b.employee_gid from adm_mst_tuser a " +
                //  " LEFT JOIN hrm_mst_temployee b ON a.user_gid=b.user_gid " +
                //  " where user_status<>'N' order by a.user_firstname asc";

                //dt_datatable = objdbconn.GetDataTable(msSQL);
                //if (dt_datatable.Rows.Count != 0)
                //{
                //    objmaster.assignedto_general = dt_datatable.AsEnumerable().Select(row =>
                //      new assignedto_list
                //      {
                //          employee_gid = row["employee_gid"].ToString(),
                //          employee_name = row["employee_name"].ToString()
                //      }
                //    ).ToList();
                //}
                //dt_datatable.Dispose();  
            }
            catch (Exception ex)
            {
                objmaster.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public bool DaUpdateTask(string employee_gid, MdlTask values)
        {
            try
            {
                
                string lstat, lstask_description, lstask_name, lsteam_name = "";
                if (values.tat == null || values.tat == "")
                    lstat = "";
                else
                    lstat = values.tat.Replace("'", "\\'");
                if (values.task_description == null || values.task_description == "")
                    lstask_description = "";
                else
                    lstask_description = values.task_description.Replace("'", "\\'");
                if (values.task_name == null || values.task_name == "")
                    lstask_name = "";
                else
                    lstask_name = values.task_name.Replace("'", "\\'");
                if (values.team_name == null || values.team_name == "")
                    lsteam_name = "";
                else
                    lsteam_name = values.team_name.Replace("'", "\\'");



                msSQL = " SELECT task_name FROM sys_mst_ttask where lcase(task_name) ='" + lstask_name.ToLower() + "'" +
                        " and task_gid != '" + values.task_gid + "'";
                string GetTaskName = objdbconn.GetExecuteScalar(msSQL);
                if (GetTaskName != "" && GetTaskName != null)
                {
                    values.message = "Task Name Already Exists";
                    values.status = false;
                    return false;
                }

                msSQL = "select task_gid, task_name, updated_by, updated_date from sys_mst_ttask where task_gid='" + values.task_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    string lsUpdatedBy = objMySqlDataReader["updated_by"].ToString();
                    string lsUpdatedDate = objMySqlDataReader["updated_date"].ToString();

                    if (!(String.IsNullOrEmpty(lsUpdatedBy)) && !(String.IsNullOrEmpty(lsUpdatedDate)))
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("PMSL");
                        msSQL = " insert into sys_mst_ttasklog(" +
                                  " tasklog_gid  ," +
                                  " task_gid," +
                                  " task_name, " +
                                  "team_name," +
                                  "team_gid," +
                                  " updated_by, " +
                                  " updated_date) " +
                                  " values(" +
                                  "'" + msGetGid + "'," +
                                  "'" + values.task_gid + "'," +
                                  "'" + lstask_name + "'," +
                                   "'" + values.team_gid + "'," +
                                   "'" + lsteam_name + "'," +
                                  "'" + employee_gid + "'," +
                                  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                objMySqlDataReader.Close();

                msSQL = " update sys_mst_ttask set " +
                        " task_name='" + lstask_name + "'," +
                         " team_name='" + lsteam_name + "'," +
                        " tat='" + lstat + "'," +
                        " task_description='" + lstask_description + "'," +
                        " updated_by='" + employee_gid + "'," +
                         " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                         " where task_gid='" + values.task_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Task Updated Successfully";
                    return true;
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured While Updating Task";
                    return false;
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                return false;
            }
        }
        public void DaInactiveTask(master1 values, string employee_gid)
        {
            try
            {
                
                msSQL = " select taskinitiate_gid from sys_mst_ttaskinitiate where task_gid='" + values.task_gid + "' and (task_status= 'null' or task_status = 'Initiated')";
                objMySqlDataReader1 = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader1.HasRows == true)
                {
                    objMySqlDataReader1.Close();
                    values.message = "Can't able to inactive Task, Because it is tagged to Employee Onboarding";
                    values.status = false;
                }
                else
                {
                    msSQL = " update sys_mst_ttask set status ='" + values.rbo_status + "'," +
                        " remarks='" + values.remarks.Replace("'", "") + "'" +
                        " where task_gid='" + values.task_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("STKI");

                        msSQL = " insert into sys_mst_ttaskinactivelog (" +
                              " taskinactivelog_gid  , " +
                              " task_gid," +
                              " task_name," +
                              " status," +
                              " remarks," +
                              " updated_by," +
                              " updated_date) " +
                              " values (" +
                              " '" + msGetGid + "'," +
                              " '" + values.task_gid + "'," +
                              " '" + values.task_name + "'," +
                              " '" + values.rbo_status + "'," +
                              " '" + values.remarks.Replace("'", "") + "'," +
                              " '" + employee_gid + "'," +
                              " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (values.rbo_status == 'N')
                        {
                            values.status = true;
                            values.message = "Task Inactivated Successfully";
                        }
                        else
                        {
                            values.status = true;
                            values.message = "Task Activated Successfully";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred";
                    }

                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaTaskInactiveLogview(string task_gid, MdlHrmMaster values)
        {
            try
            {
                
                msSQL = " SELECT task_gid,date_format(a.updated_date,'%d-%m-%Y %h:%i %p') as updated_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as updated_by," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as Status, a.remarks" +
                        " FROM sys_mst_ttaskinactivelog a" +
                        " left join hrm_mst_temployee b on a.updated_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                        " where task_gid ='" + task_gid + "' order by a.taskinactivelog_gid    desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmaster_list = new List<master_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmaster_list.Add(new master_list
                        {
                            task_gid = (dr_datarow["task_gid"].ToString()),
                            updated_by = (dr_datarow["updated_by"].ToString()),
                            updated_date = (dr_datarow["updated_date"].ToString()),
                            status = (dr_datarow["Status"].ToString()),
                            remarks = (dr_datarow["remarks"].ToString()),
                        });
                    }
                    values.master_list = getmaster_list;
                }
                dt_datatable.Dispose();
                values.status = true;
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteTask(string task_gid, result values)
        {
            try
            {
                
                msSQL = " delete from sys_mst_ttask where task_gid='" + task_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Task Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured..!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetTaskMultiselectList(string task_gid, MdlTask objmaster)
        {
            try
            {
                
                msSQL = " SELECT GROUP_CONCAT(distinct(b.assignedto_name) SEPARATOR ', ') as assignedto_name, " +
                    " GROUP_CONCAT(distinct(c.escalationmailto_name) SEPARATOR ', ') as escalationmailto_name FROM sys_mst_ttask a " +
                    " left join sys_mst_ttask2assignedto b on a.task_gid = b.task_gid" +
                    " left join sys_mst_ttask2escalationmailto c on a.task_gid = c.task_gid" +
                    " where a.task_gid='" + task_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    objmaster.assignedto_name = objMySqlDataReader["assignedto_name"].ToString();
                    objmaster.escalationmailto_name = objMySqlDataReader["escalationmailto_name"].ToString();

                }
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {
                objmaster.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostTeammaster(Mdlteam values, string employee_gid)
        {
            try
            {
                
                msSQL = "select team_name from sys_mst_tteam where team_name = '" + values.team_name.Replace("'", "\\'") + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Team Name Already Exist";
                }
                else
                {

                    msGetGid = objcmnfunctions.GetMasterGID("SMTT");
                    msGetcodeGid = objcmnfunctions.GetMasterGID("TEON");
                    msSQL = " insert into sys_mst_tteam(" +
                            " team_gid ," +
                            " team_code ," +
                            " team_name," +
                            " teammanager_gid," +
                            " teammanager_name," +
                            " created_by," +
                            " created_date," +
                            " status)" +
                            " values(" +
                            "'" + msGetGid + "'," +
                             "'" + msGetcodeGid + "'," +
                            "'" + values.team_name.Replace("'", "") + "'," +
                            "'" + values.teammanager_gid + "'," +
                            "'" + values.teammanager_name + "'," +

                            "'" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                            "'Y')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    for (var i = 0; i < values.teammembers.Count; i++)
                    {
                        msGetteam2member_gid = objcmnfunctions.GetMasterGID("TEMM");


                        msSQL = "Insert into sys_mst_tteam2member( " +
                               " team2member_gid, " +
                               " team_gid," +
                               " member_gid," +
                               " member_name," +
                               " created_by," +
                               " created_date," +
                                " status)" +
                               " values(" +
                               "'" + msGetteam2member_gid + "'," +
                               "'" + msGetGid + "'," +
                               "'" + values.teammembers[i].employee_gid + "'," +
                               "'" + values.teammembers[i].employee_name + "'," +
                               "'" + employee_gid + "'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'Y')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Team Added Successfully";
                    }
                    else
                    {
                        values.message = "Error Occured While Adding";
                        values.status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetTeammembersEdit(string team_gid, Mdlteam objmaster)
        {
            try
            {
                
                msSQL = " select team_gid,team_name,teammanager_gid,teammanager_name,status as Status from sys_mst_tteam" +
                    " where team_gid='" + team_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    objmaster.team_gid = objMySqlDataReader["team_gid"].ToString();
                    objmaster.team_name = objMySqlDataReader["team_name"].ToString();
                    objmaster.teammanager_gid = objMySqlDataReader["teammanager_gid"].ToString();
                    objmaster.teammanager_name = objMySqlDataReader["teammanager_name"].ToString();
                    objmaster.Status = objMySqlDataReader["Status"].ToString();
                }
                objMySqlDataReader.Close();

                msSQL = " select team2member_gid ,member_gid,member_name from sys_mst_tteam2member " +
                    " where team_gid='" + team_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getteammembersList = new List<teammembersdtl>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getteammembersList.Add(new teammembersdtl
                        {
                            employee_gid = dt["member_gid"].ToString(),
                            employee_name = dt["member_name"].ToString(),
                        });
                        objmaster.teammembersdtl = getteammembersList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                objmaster.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetteammastermembers(string team_gid, teammemberslist values)
        {
            try
            {
                
                msSQL = " select group_concat(member_name) as member_name  from sys_mst_tteam2member " +
                 " where team_gid='" + team_gid + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    values.member_name = objMySqlDataReader["member_name"].ToString();
                    values.member_name = values.member_name.Replace(",", ", ");
                }
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetTeammaster(Mdlteam objmaster)
        {
            try
            {
                
                msSQL = " SELECT a.team_gid ,a.team_name,a.teammanager_name, date_format(a.created_date,'%d-%m-%Y %h:%i %p') as created_date, " +
                                   " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by, " +
                                  " case when a.status='N' then 'Inactive' else 'Active' end as status" +
                                   " FROM sys_mst_tteam a" +
                                   " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                                   " left join adm_mst_tuser c on c.user_gid = b.user_gid order by a.created_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getteamgroup_list = new List<teamgroup>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getteamgroup_list.Add(new teamgroup
                        {
                            team_gid = (dr_datarow["team_gid"].ToString()),
                            team_name = (dr_datarow["team_name"].ToString()),
                            teammanager_name = (dr_datarow["teammanager_name"].ToString()),
                            created_date = (dr_datarow["created_date"].ToString()),
                            created_by = (dr_datarow["created_by"].ToString()),
                            status = (dr_datarow["status"].ToString()),

                        });

                    }
                    objmaster.teamgroup = getteamgroup_list;
                }
                dt_datatable.Dispose();
                objmaster.status = true;
            }
            catch (Exception ex)
            {
                objmaster.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateTeamDtl(string employee_gid, Mdlteam values)
        {
            try
            {
                
                msSQL = " select team_name from sys_mst_tteam " +
                    " where lcase(team_name) = '" + values.team_name.Replace("'", "\\'").ToLower() + "'" +
                    " and team_gid !='" + values.team_gid + "'";
                string lsTeamname = objdbconn.GetExecuteScalar(msSQL);
                if (lsTeamname != "" && lsTeamname != null)
                {
                    values.status = false;
                    values.message = "Team Name Already Exist";
                }
                else
                {
                    msSQL = " update sys_mst_tteam set " +
                            " team_name ='" + values.team_name + "', " +
                            " teammanager_gid ='" + values.teammanager_gid + "', " +
                            " teammanager_name ='" + values.teammanager_name + "' " +
                            " where team_gid='" + values.team_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "delete from sys_mst_tteam2member where team_gid='" + values.team_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        for (var i = 0; i < values.teammembers.Count; i++)
                        {
                            msGetteam2member_gid = objcmnfunctions.GetMasterGID("TEMM");
                            msSQL = "Insert into sys_mst_tteam2member( " +
                                   " team2member_gid, " +
                                   " team_gid," +
                                   " member_gid," +
                                   " member_name," +
                                   " created_by," +
                                   " created_date," +
                                    " status)" +
                                   " values(" +
                                   "'" + msGetteam2member_gid + "'," +
                                   "'" + values.team_gid + "'," +
                                   "'" + values.teammembers[i].employee_gid + "'," +
                                   "'" + values.teammembers[i].employee_name + "'," +
                                   "'" + employee_gid + "'," +
                                   "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                   "'Y')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Team Updated Successfully";
                    }
                    else
                    {
                        values.message = "Error Occured !";
                        values.status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaInactiveTeamMaster(Mdlteam values, string employee_gid)
        {
            try
            {
                
                msSQL = " update sys_mst_tteam set status ='" + values.rbo_status + "'," +
                    " remarks='" + values.remarks.Replace("'", "") + "' " +
                    " where team_gid='" + values.team_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("TMSL");

                    msSQL = " insert into sys_mst_tteaminactivelog (" +
                          " teammasterinactivelog_gid   , " +
                          " team_gid," +
                          " team_name ," +
                          " status," +
                          " remarks," +
                          " updated_by," +
                          " updated_date) " +
                          " values (" +
                          " '" + msGetGid + "'," +
                          " '" + values.team_gid + "'," +
                          " '" + values.team_name + "'," +
                          " '" + values.rbo_status + "'," +
                          " '" + values.remarks.Replace("'", "") + "'," +
                          " '" + employee_gid + "'," +
                          " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    values.status = true;
                    if (values.rbo_status == 'N')
                        values.message = "Team Inactivated Successfully";
                    else
                        values.message = "Team Activated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void TeamMasterInactiveLogview(string team_gid, Mdlteam values)
        {
            try
            {
                
                msSQL = " SELECT a.team_gid,date_format(a.updated_date,'%d-%m-%Y %h:%i %p') as updated_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as updated_by," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as Status, a.remarks" +
                        " FROM sys_mst_tteaminactivelog a" +
                        " left join hrm_mst_temployee b on a.updated_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid " +
                        " where a.team_gid ='" + team_gid + "' order by a.teammasterinactivelog_gid   desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmaster_list = new List<master_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        getmaster_list.Add(new master_list
                        {
                            baselocation_gid = (dr_datarow["team_gid"].ToString()),
                            updated_by = (dr_datarow["updated_by"].ToString()),
                            updated_date = (dr_datarow["updated_date"].ToString()),
                            status = (dr_datarow["status"].ToString()),
                            remarks = (dr_datarow["remarks"].ToString()),
                        });
                    }
                    values.master_list = getmaster_list;
                }
                dt_datatable.Dispose();
                values.status = true;
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaDeleteTeammaster(string team_gid, string employee_gid, Mdlteam values)
        {
            try
            {
                
                msSQL = " delete from sys_mst_tteam where team_gid='" + team_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from sys_mst_tteam2member where team_gid='" + team_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = "Team Deleted Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetEmployee(MdlEmployee objemployee)
        {
            try
            {
                
                msSQL = " SELECT a.user_firstname,a.user_gid ,concat(a.user_firstname,' ',a.user_lastname,' / ',a.user_code) as employee_name,b.employee_gid from adm_mst_tuser a " +
                   " LEFT JOIN hrm_mst_temployee b ON a.user_gid=b.user_gid " +
                   " where user_status<>'N' order by a.user_firstname asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_employee = new List<taskemployee_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    objemployee.taskemployee_list = dt_datatable.AsEnumerable().Select(row => new taskemployee_list
                    {
                        employee_gid = row["employee_gid"].ToString(),
                        employee_name = row["employee_name"].ToString()
                    }
                    ).ToList();
                }
                dt_datatable.Dispose();
                objemployee.status = true;
            }
            catch (Exception ex)
            {
                objemployee.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetmemberEmployee(MdlEmployee objemployee)
        {
            try
            {
                
                msSQL = " SELECT a.user_firstname,a.user_gid ,concat(a.user_firstname,' ',a.user_lastname,' / ',a.user_code) as employee_name,b.employee_gid from adm_mst_tuser a " +
                   " LEFT JOIN hrm_mst_temployee b ON a.user_gid=b.user_gid " +
                   " where user_status<>'N' order by a.user_firstname asc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_employee = new List<taskmemberemployee_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    objemployee.taskmemberemployee_list = dt_datatable.AsEnumerable().Select(row => new taskmemberemployee_list
                    {
                        employee_gid = row["employee_gid"].ToString(),
                        employee_name = row["employee_name"].ToString()
                    }
                    ).ToList();
                }
                dt_datatable.Dispose();
                objemployee.status = true;
            }
            catch (Exception ex)
            {
                objemployee.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
    }
}