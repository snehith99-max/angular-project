﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.system.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Web;
using OfficeOpenXml;
using RestSharp;
using System.Configuration;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
//using ems.storage.Functions;
//using ems.hbapiconn.Functions;
//using ems.hbapiconn.Models;
using Newtonsoft.Json;
using System.Web.Http;
using System.Threading;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

using System.Web.Http.Results;

namespace ems.system.DataAccess
{
    public class DaManageEmployee
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        Fnazurestorage objcmnstorage = new Fnazurestorage();
        //FnSamAgroHBAPIConn objFnSamAgroHBAPIConn = new FnSamAgroHBAPIConn();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, msGetGid, msGetPrivilege_gid, msGetModule2employee_gid, msGetAPICode;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;
        int mnResult6;
        string lsentity_flag = string.Empty;
        int  k, ls_port, hierarchy_level;
        public string ls_server, ls_username, ls_password, tomail_id, tomail_id1, tomail_id2, sub, body, employeename, cc_mailid, employee_reporting_to, erp_id;      
        string lsemployee_name, lsemployee_gid, lscreated_by, lsemployee_reportingto, lsmodule2employee_gid;
        string sToken = string.Empty;
        Random rand = new Random();
        string lsto_mail, ls_baselocation,lstask_gid, lshierarchy_level, lscc_mail, strBCC, lsbcc_mail;
        string lstask_name, lstask_remarks, lscreated_date, lsbase_location, lsrptngto_erpid, lstaskassignedby, lsnewemployee_name, lsBccmail_id, lsreporting_to, lsreporting_toname;
        public string[] lsBCCReceipients;
        public string[] lsCCReceipients;
        string lsentity_name, lsemployee_emailid, lsemployee_mobileno, lsuser_firstname, lsuser_lastname, lsuser_code, lsdepartment_gid;
        string loglspath = "", logFileName = "";
        string lspath,lsentity_gid,lsemployee_externalid;
        string lsmodulereportingto_gid = "";
        public bool DaEmployeeSummary(employee_list objemployee)
        {
            try
            {
                msSQL = " Select distinct a.user_gid,c.useraccess,case when c.entity_gid is null then c.entity_name else z.entity_name end as entity_name , a.api_codes, " +
                    " a.user_code,concat(a.user_firstname,' ',a.user_lastname) as user_name ,c.employee_joiningdate," +
                    " c.employee_gender,  " +
                    " concat(j.address1,' ',j.address2,'/', j.city,'/', j.state,'/',k.country_name,'/', j.postal_code) as emp_address, " +
                    " d.designation_name,c.designation_gid,c.employee_gid,c.employee_emailid,e.branch_name,s.baselocation_name,c.baselocation_gid,concat(v.user_firstname,' ',v.user_lastname) as employeereporting_to, " +
                    " CASE " +
                    " WHEN a.user_status = 'Y' THEN 'Active'  " +
                    " WHEN a.user_status = 'N' THEN 'Inactive' " +
                    " END as user_status,c.department_gid,c.branch_gid, e.branch_name, g.department_name " +
                    " FROM adm_mst_tuser a " +
                    " left join hrm_mst_temployee c on a.user_gid = c.user_gid " +
                    " left join hrm_mst_temployee p on c.employeereporting_to = p.employee_gid " +
                    " left join adm_mst_tuser v on p.user_gid = v.user_gid " +
                    " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " + 
                    " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                    " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                    " left join adm_mst_taddress j on c.employee_gid=j.parent_gid " +
                    " left join adm_mst_tcountry k on j.country_gid=k.country_gid " +
                    " left join adm_mst_tentity z on z.entity_gid=c.entity_gid" +
                    " left join sys_mst_tbaselocation s on s.baselocation_gid=c.baselocation_gid" +
                    " left join hrm_trn_temployeedtl m on m.permanentaddress_gid=j.address_gid " +
                    " group by c.employee_gid " +
                    " order by c.employee_gid desc  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_employeelist =new List<employee>();
                if(dt_datatable.Rows.Count !=0)
                {
                  foreach(DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_employeelist.Add(new employee
                        {
                            employee_gid = dr_datarow["employee_gid"].ToString(),
                            user_gid = dr_datarow["user_gid"].ToString(),
                            branch_name = dr_datarow["branch_name"].ToString(),
                            designation_name = dr_datarow["designation_name"].ToString(),
                            department_name = dr_datarow["department_name"].ToString(),
                            user_code = dr_datarow["user_code"].ToString(),
                            employee_name = dr_datarow["user_name"].ToString(),
                            user_status = dr_datarow["user_status"].ToString(),
                            company_name = dr_datarow["entity_name"].ToString(),
                            baselocation_gid = dr_datarow["baselocation_gid"].ToString(),
                            baselocation_name = dr_datarow["baselocation_name"].ToString(),
                             employeereporting_to = dr_datarow["employeereporting_to"].ToString(),
                            employee_emailid = dr_datarow["employee_emailid"].ToString(),
                            created_by = dr_datarow["created_by"].ToString(),
                            created_date = dr_datarow["created_date"].ToString(),
                            api_code = dr_datarow["api_codes"].ToString()
                            
                        });
                    }
                    objemployee.employee = get_employeelist;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch(Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
        
        public bool DaEmployeeActiveSummary(employee_list objemployee)
        {
            try
            {
                msSQL = " Select distinct a.user_gid,c.erp_id,c.useraccess,a.api_codes,case when c.entity_gid is null then c.entity_name else z.entity_name end as entity_name , " +
                    " a.user_code,concat(a.user_firstname,' ',a.user_lastname) as user_name ,c.employee_joiningdate," +
                    " c.employee_gender,  " +
                    " concat(j.address1,' ',j.address2,'/', j.city,'/', j.state,'/',k.country_name,'/', j.postal_code) as emp_address, " +
                    " d.designation_name,c.designation_gid,c.employee_gid,c.employee_emailid,e.branch_name,s.baselocation_name,c.baselocation_gid,concat(v.user_firstname,' ',v.user_lastname) as employeereporting_to, " +
                    " concat(r.user_firstname,' ',r.user_lastname) as created_by, date_format(c.created_date, '%d-%m-%Y') as created_date, c.employee_externalid, c.employee_entitychange_flag," +
                    " CASE " +
                    " WHEN a.user_status = 'Y' THEN 'Active'  " +
                    " WHEN a.user_status = 'N' THEN 'Inactive' " +
                    " END as user_status,c.department_gid,c.branch_gid, e.branch_name, g.department_name" +
                    " FROM adm_mst_tuser a " +
                    " left join hrm_mst_temployee c on a.user_gid = c.user_gid " +
                    " left join hrm_mst_temployee p on c.employeereporting_to = p.employee_gid " +
                    " left join hrm_mst_temployee q on c.created_by = q.employee_gid " +
                    " left join adm_mst_tuser r on q.user_gid = r.user_gid " +
                    " left join adm_mst_tuser v on p.user_gid = v.user_gid " +
                    " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                    " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                    " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                    " left join adm_mst_taddress j on c.employee_gid=j.parent_gid " +
                    " left join adm_mst_tcountry k on j.country_gid=k.country_gid " +
                    " left join adm_mst_tentity z on z.entity_gid=c.entity_gid" +
                    " left join sys_mst_tbaselocation s on s.baselocation_gid=c.baselocation_gid" +
                    " left join hrm_trn_temployeedtl m on m.permanentaddress_gid=j.address_gid " +
                    " left join sys_mst_temployeelog n on n.employee_gid = c.employee_gid " +
                    " where a.user_status = 'Y' and (c.employee_status = 'A' || c.employee_status = null)" +
                    " group by c.employee_gid " +
                    " order by c.created_date desc  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_employeelist = new List<employee>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_employeelist.Add(new employee
                        {
                            employee_gid = dr_datarow["employee_gid"].ToString(),
                            user_gid = dr_datarow["user_gid"].ToString(),
                            branch_name = dr_datarow["branch_name"].ToString(),
                            designation_name = dr_datarow["designation_name"].ToString(),
                            department_name = dr_datarow["department_name"].ToString(),
                            user_code = dr_datarow["user_code"].ToString(),
                            employee_name = dr_datarow["user_name"].ToString(),
                            user_status = dr_datarow["user_status"].ToString(),
                            company_name = dr_datarow["entity_name"].ToString(),
                            baselocation_gid = dr_datarow["baselocation_gid"].ToString(),
                            baselocation_name = dr_datarow["baselocation_name"].ToString(),
                            employeereporting_to = dr_datarow["employeereporting_to"].ToString(),
                            employee_emailid = dr_datarow["employee_emailid"].ToString(),
                            created_by = dr_datarow["created_by"].ToString(),
                            created_date= dr_datarow["created_date"].ToString(),
                            erp_id = dr_datarow["erp_id"].ToString(),
                            employee_externalid = dr_datarow["employee_externalid"].ToString(),
                            employee_entitychange_flag = dr_datarow["employee_entitychange_flag"].ToString(),
                            api_code = dr_datarow["api_codes"].ToString()
                        });
                    }
                    objemployee.employee = get_employeelist;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
        public bool DaEmployeeRelievedSummary(employee_list objemployee)
        {
            try
            {
                msSQL = "select b.employee_gid,case when c.entity_gid is null then c.entity_name else z.entity_name end as entity_name, " +
                    " a.user_code ,concat(a.user_firstname,' ',a.user_lastname) as user_name , b.remarks, date_format(b.created_date, '%d-%m-%Y') as created_date, " +
                    " concat(e.user_firstname,' ',e.user_lastname) as created_by,date_format( b.exit_date, '%d-%m-%Y') as exit_date,concat(g.user_firstname, ' ', g.user_lastname) as employeereporting_to," +
                    " s.baselocation_name,c.baselocation_gid,c.department_gid,h.department_name, " +
                    " CASE " +
                   " WHEN a.user_status = 'Y' THEN 'Active'  " +
                   " WHEN a.user_status = 'N' THEN 'Inactive' " +
                   " END as user_access " +
                    " from hrm_trn_texitemployee b" +
                    " left join hrm_mst_temployee c on c.employee_gid = b.employee_gid " +
                    " left join adm_mst_tuser a on c.user_gid = a.user_gid " +
                    " left join hrm_mst_temployee d on d.employee_gid = b.created_by " +
                    " left join adm_mst_tuser e on d.user_gid = e.user_gid " +
                    " left join hrm_mst_temployee f on f.employee_gid = c.employeereporting_to "+
                    " left join adm_mst_tuser g on g.user_gid = f.user_gid " +
                    " left join sys_mst_tbaselocation s on s.baselocation_gid = c.baselocation_gid " + 
                    " left join hrm_mst_tdepartment h on h.department_gid = c.department_gid" +
                    " left join adm_mst_tentity z on z.entity_gid = c.entity_gid " +
                    " left join sys_mst_temployeelog i on c.employee_gid = i.employee_gid " +
                   " where a.user_status = 'Y' and  c.employee_status = 'R'" +
                    " group by c.employee_gid " +
                    " order by b.exit_date desc  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_employeelist = new List<employee>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_employeelist.Add(new employee
                        {
                            employee_gid = dr_datarow["employee_gid"].ToString(),
                            department_name = dr_datarow["department_name"].ToString(),
                            user_code = dr_datarow["user_code"].ToString(),
                            employee_name = dr_datarow["user_name"].ToString(),
                            entity_name = dr_datarow["entity_name"].ToString(),
                            baselocation_gid = dr_datarow["baselocation_gid"].ToString(),
                            baselocation_name = dr_datarow["baselocation_name"].ToString(),
                            employeereporting_to = dr_datarow["employeereporting_to"].ToString(),
                            created_date = dr_datarow["created_date"].ToString(),
                            created_by = dr_datarow["created_by"].ToString(),
                            remarks = dr_datarow["remarks"].ToString(),
                            relive_date = dr_datarow["exit_date"].ToString(),
                            user_access = dr_datarow["user_access"].ToString()
                    });
                    }
                    objemployee.employee = get_employeelist;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
        public bool DaEmployeePendingSummary(employee_list objemployee)
        {
            try
            {
                msSQL = " Select distinct a.user_gid,c.useraccess,case when c.entity_gid is null then c.entity_name else z.entity_name end as entity_name , " +
                    " a.user_code,concat(a.user_firstname,' ',a.user_lastname) as user_name ,a.api_codes," +
                    " c.employee_gender,  " +
                    " concat(j.address1,' ',j.address2,'/', j.city,'/', j.state,'/',k.country_name,'/', j.postal_code) as emp_address, " +
                    " d.designation_name,c.designation_gid,c.employee_gid,c.employee_emailid,e.branch_name,s.baselocation_name,"+
                    " c.baselocation_gid,concat(v.user_firstname,' ',v.user_lastname) as employeereporting_to, " +
                    " CASE " +
                    " WHEN a.user_status = 'Y' THEN 'Active'  " +
                    " WHEN a.user_status = 'N' THEN 'Inactive' " +
                    " END as user_status,c.department_gid,c.branch_gid, date_format(c.employee_joiningdate,'%d-%m-%Y') as joining_date,"+
                    " e.branch_name, g.department_name, date_format(c.created_date,'%d-%m-%Y') as created_date,concat(r.user_firstname,' ',r.user_lastname) as created_by, b.employee_status" +
                    " FROM adm_mst_tuser a " +
                    " left join hrm_mst_temployee c on a.user_gid = c.user_gid " +
                    " left join hrm_mst_temployee p on c.employeereporting_to = p.employee_gid " +
                    " left join hrm_mst_temployee q on q.employee_gid = c.created_by " +
                    " left join adm_mst_tuser r on q.user_gid = r.user_gid " +
                    " left join adm_mst_tuser v on p.user_gid = v.user_gid " +
                    " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                    " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                    " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                    " left join adm_mst_taddress j on c.employee_gid=j.parent_gid " +
                    " left join adm_mst_tcountry k on j.country_gid=k.country_gid " +
                    " left join adm_mst_tentity z on z.entity_gid=c.entity_gid" +
                    " left join sys_mst_tbaselocation s on s.baselocation_gid=c.baselocation_gid" +
                    " left join hrm_trn_temployeedtl m on m.permanentaddress_gid=j.address_gid " +
                    " left join sys_mst_temployeelog b on b.employee_gid = c.employee_gid " +
                    " where a.user_status = 'N' and c.employee_status = 'P'" +
                    " group by c.employee_gid " +
                    " order by c.employee_gid desc  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_employeelist = new List<employee>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_employeelist.Add(new employee
                        {
                            employee_gid = dr_datarow["employee_gid"].ToString(),
                            user_gid = dr_datarow["user_gid"].ToString(),
                            branch_name = dr_datarow["branch_name"].ToString(),
                            designation_name = dr_datarow["designation_name"].ToString(),
                            department_name = dr_datarow["department_name"].ToString(),
                            user_code = dr_datarow["user_code"].ToString(),
                            employee_name = dr_datarow["user_name"].ToString(),
                            user_status = dr_datarow["user_status"].ToString(),
                            entity_name = dr_datarow["entity_name"].ToString(),
                            baselocation_gid = dr_datarow["baselocation_gid"].ToString(),
                            baselocation_name = dr_datarow["baselocation_name"].ToString(),
                            employeereporting_to = dr_datarow["employeereporting_to"].ToString(),
                            employee_emailid = dr_datarow["employee_emailid"].ToString(),
                            created_date = dr_datarow["created_date"].ToString(),
                            created_by = dr_datarow["created_by"].ToString(),
                            joining_date = dr_datarow["joining_date"].ToString(),
                            employee_status = dr_datarow["employee_status"].ToString(),
                            api_code = dr_datarow["api_codes"].ToString()
                        });
                    }
                    objemployee.employee = get_employeelist;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
        public bool DaEmployeeInactiveSummary(employee_list objemployee)
        {
            try
            {
                msSQL = " Select distinct a.user_gid,c.useraccess,case when c.entity_gid is null then c.entity_name else z.entity_name end as entity_name ,"+
                        " a.user_code,concat(a.user_firstname,' ',a.user_lastname) as user_name ,concat(l.user_firstname,' ',l.user_lastname) as employeereporting_to, " +
                        " date_format(c.employee_joiningdate, '%d-%m-%Y') as joining_date,c.employee_gid, date_format(c.updated_date, '%d-%m-%Y') as updated_date, " +
                        " concat(e.user_firstname,' ',e.user_lastname) as updated_by,c.remarks  " +
                        " FROM adm_mst_tuser a left join hrm_mst_temployee c on a.user_gid = c.user_gid " +
                        " left join hrm_mst_temployee d on d.employee_gid = c.updated_by " +
                        " left join adm_mst_tuser e on e.user_gid = d.user_gid " +
                        " left join adm_mst_tentity z on z.entity_gid=c.entity_gid " +
                        " left join sys_mst_temployeelog n on n.employee_gid=c.employee_gid " +
                        " left join hrm_mst_temployee m on m.employee_gid=c.employeereporting_to" +
                        " left join adm_mst_tuser l on m.user_gid=l.user_gid " +
                        " where a.user_status = 'N' and c.employee_status='I'" +
                        " group by employee_gid " +
                        " order by updated_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_employeelist = new List<employee>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_employeelist.Add(new employee
                        {
                            employee_gid = dr_datarow["employee_gid"].ToString(),
                            user_gid = dr_datarow["user_gid"].ToString(),
                            user_code = dr_datarow["user_code"].ToString(),
                            employee_name = dr_datarow["user_name"].ToString(),
                            employeereporting_to = dr_datarow["employeereporting_to"].ToString(),
                            remarks = dr_datarow["remarks"].ToString(),
                            entity_name = dr_datarow["entity_name"].ToString(),
                            updated_date = dr_datarow["updated_date"].ToString(),
                            updated_by = dr_datarow["updated_by"].ToString(),
                            joining_date = dr_datarow["joining_date"].ToString(),
                        });
                    }
                    objemployee.employee = get_employeelist;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        public bool DaPopEntity(entity_list objentity_list)
        {
            try
            {
                msSQL = "select entity_gid,entity_name from adm_mst_tentity where 1=1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_entity = new List<entity>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_row in dt_datatable.Rows)
                    {
                        get_entity.Add(new entity
                        {
                            entity_gid = dr_row["entity_gid"].ToString(),
                            entity_name = dr_row["entity_name"].ToString()
                        });
                    }
                    objentity_list.entity = get_entity;
                    objentity_list.status = true;
                    return true;
                }
                else
                {
                    objentity_list.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
                return false;
            }
        }

        public bool DaPopBranch(employee_list objemployee_list)
        {
            try
            {
                msSQL = "select branch_gid,branch_name from hrm_mst_tbranch order by branch_name";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_branch_list = new List<employee>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_row in dt_datatable.Rows)
                    {
                        get_branch_list.Add(new employee
                        {
                            branch_gid = dr_row["branch_gid"].ToString(),
                            branch_name = dr_row["branch_name"].ToString()
                        });
                    }
                    objemployee_list.employee = get_branch_list;
                    objemployee_list.status = true;
                    return true;
                }
                else
                {
                    objemployee_list.status = false;
                    return false;
                }
            }
            catch
            {
                objemployee_list.status = false;
                return false;
            }
        }

        public bool DaPopDepartment(employee_list objemployee_list)
        {
            try
            {
                msSQL = "select department_gid,department_name from hrm_mst_tdepartment order by department_name";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_department_list = new List<employee>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_row in dt_datatable.Rows)
                    {
                        get_department_list.Add(new employee
                        {
                            department_gid = dr_row["department_gid"].ToString(),
                            department_name = dr_row["department_name"].ToString()
                        });
                    }
                    objemployee_list.employee = get_department_list;
                    objemployee_list.status = true;
                    return true;
                }
                else
                {
                    objemployee_list.status = false;
                    return false;
                }
            }
            catch
            {
                objemployee_list.status = false;
                return false;
            }
        }

        public bool DaPopSubfunction(employee_list objemployee_list)
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
                return false;
            }

        }
        public bool DaEmployeeProfileView(employee objemployee, string employee_gid)
        {
            try
            {
                msSQL =" Select distinct a.user_gid,c.useraccess," +
                       " case when c.entity_gid is null then c.entity_name else z.entity_name end as entity_name , " +
                       " user_firstname,user_lastname," +
                       " a.user_code,concat(a.user_firstname,' ',a.user_lastname) as user_name ,c.employee_joiningdate," +
                       " c.employee_gender,c.role_gid,employeereporting_to,  " +
                       " concat(j.address1,' ',j.address2,'/', j.city,'/', j.state,'/',k.country_name,'/', j.postal_code) as emp_address, " +
                       " d.designation_name,c.designation_gid,c.employee_gid,e.branch_name, employee_mobileno,c.entity_gid," +
                       " CASE " +
                       " WHEN a.user_status = 'Y' THEN 'Active'  " +
                       " WHEN a.user_status = 'N' THEN 'Inactive' " +
                       " END as user_access,a.user_status,c.department_gid,c.branch_gid,n.role_name, e.branch_name, g.department_name," +
                       " DATE_FORMAT(c.employee_joiningdate, '%d/%m/%Y') as joiningdate, " +
                       " c.employee_personalno as personal_phone_no,c.employee_emailid " +
                       " FROM hrm_mst_temployee c " +
                       " left join adm_mst_tuser a on a.user_gid = c.user_gid " +
                       " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                       " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                       " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                       " left join adm_mst_taddress j on c.employee_gid=j.parent_gid " +
                       " left join adm_mst_tcountry k on j.country_gid=k.country_gid " +
                       " left join adm_mst_tentity z on z.entity_gid=c.entity_gid" +
                       " left join hrm_trn_temployeedtl m on m.permanentaddress_gid=j.address_gid" +
                       " left join hrm_mst_trole n on n.role_gid=c.role_gid " +
                       " where c.employee_gid='" + employee_gid + "'" +
                       " group by c.employee_gid " +
                       " order by c.employee_gid desc ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objemployee.company_name = objOdbcDataReader["entity_name"].ToString();
                    objemployee.entity_gid = objOdbcDataReader["entity_gid"].ToString();
                    objemployee.branch_gid = objOdbcDataReader["branch_gid"].ToString();
                    objemployee.branch_name = objOdbcDataReader["branch_name"].ToString();
                    objemployee.department_gid = objOdbcDataReader["department_gid"].ToString();
                    objemployee.department_name = objOdbcDataReader["department_name"].ToString();
                    objemployee.designation_gid = objOdbcDataReader["designation_gid"].ToString();
                    objemployee.designation_name = objOdbcDataReader["designation_name"].ToString();
                    objemployee.useraccess = objOdbcDataReader["useraccess"].ToString();
                    objemployee.user_access = objOdbcDataReader["user_access"].ToString();
                    objemployee.user_status = objOdbcDataReader["user_status"].ToString();
                    objemployee.user_firstname = objOdbcDataReader["user_firstname"].ToString();
                    objemployee.user_lastname = objOdbcDataReader["user_lastname"].ToString();
                    objemployee.gender = objOdbcDataReader["employee_gender"].ToString();
                    objemployee.employee_emailid = objOdbcDataReader["employee_emailid"].ToString();
                    objemployee.employee_mobileno = objOdbcDataReader["employee_mobileno"].ToString();
                    objemployee.user_code = objOdbcDataReader["user_code"].ToString();
                    objemployee.role_gid = objOdbcDataReader["role_gid"].ToString();
                    objemployee.role_name = objOdbcDataReader["role_name"].ToString();
                    objemployee.employee_reportingto = objOdbcDataReader["employeereporting_to"].ToString();
                    objemployee.joining_date = objOdbcDataReader["joiningdate"].ToString();
                    objemployee.personal_phone_no = objOdbcDataReader["personal_phone_no"].ToString();
                    objemployee.personal_emailid = objOdbcDataReader["employee_emailid"].ToString();

                    msSQL = "select integrated_gmail from hrm_mst_temployee where user_gid='" + objOdbcDataReader["user_gid"].ToString() + "'";
                    string gmail_address = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select default_template from crm_smm_gmail_service where gmail_address = '" + gmail_address + "'";
                    //  string default_template = objdbconn.GetExecuteScalar(msSQL);
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows == true)
                    {
                        objemployee.default_template = objOdbcDataReader["default_template"].ToString();

                        objOdbcDataReader.Close();
                    }
                    else
                    {

                        objOdbcDataReader.Close();
                    }


                    if (objOdbcDataReader["joiningdate"].ToString() != "")
                    {
                        objemployee.joiningdate = Convert.ToDateTime(objOdbcDataReader["joiningdate"].ToString());
                    }
                }
             
                msSQL = "select concat(user_code, ' || ', user_firstname,' ',user_lastname) as employee_reportingto_name" +
                        " from hrm_mst_temployee a" +
                        " left join adm_mst_tuser b on a.user_gid = b.user_gid" +
                        " where employee_gid = '" + objemployee.employee_reportingto + "'";
                objemployee.employee_reportingto_name = objdbconn.GetExecuteScalar(msSQL);

                string lspermanentaddressGID = string.Empty;
                string lstemporaryaddressGID = string.Empty;

                msSQL = " Select permanentaddress_gid,temporaryaddress_gid from " +
                        " hrm_trn_temployeedtl where " +
                        " employee_gid = '" + employee_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    lspermanentaddressGID = objOdbcDataReader["permanentaddress_gid"].ToString();
                    lstemporaryaddressGID = objOdbcDataReader["temporaryaddress_gid"].ToString();

                }
                msSQL = " Select a.address1,a.address2,a.city, " +
                        " a.postal_code,a.state,b.country_name,b.country_gid" +
                        " from adm_mst_taddress a,adm_mst_tcountry b " +
                        " where  address_gid = '" + lspermanentaddressGID + "' and " +
                        " a.country_gid = b.country_gid and " +
                        " a.parent_gid = '" + employee_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objemployee.per_address1 = objOdbcDataReader["address1"].ToString();
                    objemployee.per_address2 = objOdbcDataReader["address2"].ToString();
                    objemployee.per_country_gid = objOdbcDataReader["country_gid"].ToString();
                    objemployee.per_country_name = objOdbcDataReader["country_name"].ToString();
                    objemployee.per_state = objOdbcDataReader["state"].ToString();
                    objemployee.per_city = objOdbcDataReader["city"].ToString();
                    objemployee.per_postal_code = objOdbcDataReader["postal_code"].ToString();
                }

                msSQL = " Select a.address1,a.address2,a.city, " +
                        " a.postal_code,a.state,b.country_name,b.country_gid" +
                        " from adm_mst_taddress a,adm_mst_tcountry b " +
                        " where  address_gid = '" + lstemporaryaddressGID + "' and " +
                        " a.country_gid = b.country_gid and " +
                        " a.parent_gid = '" + employee_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    objemployee.temp_address1 = objOdbcDataReader["address1"].ToString();
                    objemployee.temp_address2 = objOdbcDataReader["address2"].ToString();
                    objemployee.temp_country_gid = objOdbcDataReader["country_gid"].ToString();
                    objemployee.temp_country_name = objOdbcDataReader["country_name"].ToString();
                    objemployee.temp_state = objOdbcDataReader["state"].ToString();
                    objemployee.temp_city = objOdbcDataReader["city"].ToString();
                    objemployee.temp_postal_code = objOdbcDataReader["postal_code"].ToString();
                }
                objemployee.status = true;
                return true;
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
                objemployee.status = false;
                return false;
            }
        }

        public void DaGetEmployeename(string user_gid, employee_list values)
        {
            result objresult = new result();
            try
            {
                msSQL = " SELECT employee_gid FROM adm_mst_tuser a left join hrm_mst_temployee b on b.user_gid=a.user_gid WHERE a.user_gid='" + user_gid + "' ";
                lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select concat(user_firstname,' ',user_lastname,' / ',user_code) As Name , " +
                        " CASE WHEN '" + lsemployee_gid + "' = 'E1' THEN 'Y' ELSE 'N' END AS service_flag " +
                        " FROM adm_mst_tuser " +
                        " left join hrm_mst_temployee using(user_gid) where employee_gid='" + lsemployee_gid + "'";
                       dt_datatable = objdbconn.GetDataTable(msSQL);

                var get_holiday = new List<employeename_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        get_holiday.Add(new employeename_list
                        {
                            Name = dt["Name"].ToString(),
                            service_flag = dt["service_flag"].ToString(),

                        });
                    }
                    values.employeename_list = get_holiday;
                }
                dt_datatable.Dispose();
                objresult.status = true;
            }
            catch
            {
                objresult.status = false;
            }
        }




    }

}
