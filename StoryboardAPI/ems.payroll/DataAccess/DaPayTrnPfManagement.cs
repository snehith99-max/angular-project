using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.payroll.Models;
using static ems.payroll.Models.MdlPayTrnPfManagement;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Globalization;

namespace ems.payroll.DataAccess
{
    public class DaPayTrnPfManagement
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGID, msGetGid1, lsempoyeegid, msgetassign2employee_gid, lsaccountdtl_gid,pf_doj;

        public void DaGetPfManagementSummary(MdlPayTrnPfManagement values)
        {
            msSQL = "select employee_gid,user_code,employee_name,branch_name,department_name,designation_name from pay_trn_tpfmanagement";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetPfManagement>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetPfManagement
                    {
                        employee_gid = dt["employee_gid"].ToString(),
                        user_code = dt["user_code"].ToString(),
                        employee_name = dt["employee_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        designation_name = dt["designation_name"].ToString(),


                    });
                    values.GetPfManagement_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }


        public void DaGetPfEmployeeSummary(MdlPayTrnPfManagement values)
        {
            msSQL = " select a.user_code,concat(a.user_firstname,'',a.user_lastname) as employee_name,b.employee_gid,b.department_gid,b.branch_gid," +
                       " b.designation_gid,c.department_name,d.branch_name,e.designation_name from adm_mst_tuser a" +
                       " left join hrm_mst_temployee b on a.user_gid=b.user_gid" +
                       " left join hrm_mst_tdepartment c on b.department_gid=c.department_gid" +
                       " left join hrm_mst_tbranch d on b.branch_gid=d.branch_gid" +
                       " left join adm_mst_tdesignation e on b.designation_gid=e.designation_gid" +
                       " where user_status='Y' and b.employee_gid not in (select employee_gid from pay_trn_tpfmanagement) group by employee_gid";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetPfEmployee>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetPfEmployee
                    {
                        employee_gid = dt["employee_gid"].ToString(),
                       user_code = dt["user_code"].ToString(),
                        employee_name = dt["employee_name"].ToString(),
                        branch_name = dt["branch_name"].ToString(),
                        department_name = dt["department_name"].ToString(),
                        designation_name = dt["designation_name"].ToString(),
                        department_gid = dt["department_gid"].ToString(),
                        branch_gid = dt["branch_gid"].ToString(),
                        designation_gid = dt["designation_gid"].ToString(),


                    });
                    values.GetPfEmployee_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaEmployeeAssignSubmit(string user_gid, GetEmployeesubmit_list values)
        {
            try 
            {

                foreach (var data in values.GetPfEmployee_list)
                {

                    msSQL = " select a.user_code,concat(a.user_firstname,'',a.user_lastname) as employee_name,b.employee_gid,b.department_gid,b.branch_gid," +
                        " b.designation_gid,c.department_name,d.branch_name,e.designation_name from adm_mst_tuser a" +
                        " left join hrm_mst_temployee b on a.user_gid=b.user_gid" +
                        " left join hrm_mst_tdepartment c on b.department_gid=c.department_gid" +
                        " left join hrm_mst_tbranch d on b.branch_gid=d.branch_gid" +
                        " left join adm_mst_tdesignation e on b.designation_gid=e.designation_gid" +
                        " where user_status='Y'and b.employee_gid='" + data.employee_gid + "' group by b.employee_gid order by b.employee_gid desc";
                   
                    dt_datatable = objdbconn.GetDataTable(msSQL);


                    if (dt_datatable.Rows.Count != 0)
                    {
                        msgetassign2employee_gid = objcmnfunctions.GetMasterGID("PGMT");

                        msSQL = " insert into pay_trn_tpfmanagement ( " +
                            " pfmanage_gid," +
                            " employee_gid," +
                            " user_code," +
                            " employee_name," +
                            " branch_name," +
                            " department_name," +
                            " designation_name," +
                            " created_by," +
                            " created_date" +
                            " ) Values ( " +
                            " '" + msgetassign2employee_gid + "', " +
                            " '" + data.employee_gid + "', " +
                            " '" + data.user_code + "', " +
                            " '" + data.employee_name+ "', " +
                            " '" + data.branch_name + "', " +
                            " '" + data.department_name + "', " +
                            " '" + data.designation_name + "'," +
                            " '" + user_gid + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Please Select Atleast one employee for Assign";
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


        public void DaGetEmployeePfSummary(string employee_gid, MdlPayTrnPfManagement values)
        {
            try
            {

                msSQL = " select accountdtl_gid,pf_no,date_format(pf_doj, '%d-%m-%Y') as pf_doj,experience,remarks,totalperiod_preservice from hrm_trn_tformaccountdetails where employee_gid='"  + employee_gid +  "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetAddpfDetails>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetAddpfDetails
                        {


                            accountdtl_gid = dt["accountdtl_gid"].ToString(),
                            pf_no = dt["pf_no"].ToString(),
                            pf_doj = dt["pf_doj"].ToString(),
                            experience = dt["experience"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            totalperiod_preservice = dt["totalperiod_preservice"].ToString(),
                           



                        });
                        values.GetAddpfDetails_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
               //values.message = "Exception occured while Getting PF Details !";
               // objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               //$" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               //msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaPostemployeeAssignSubmit(string employee_gid, GetEmployeesubmit_list values)
        {
            try
            {

                    msSQL = msSQL = "Select employee_gid,accountdtl_gid from hrm_trn_tformaccountdetails where employee_gid = '" + values.employee_gid + "'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows) {

                    lsaccountdtl_gid = objOdbcDataReader["accountdtl_gid"].ToString();


                    string uiDateStr2 = values.pf_doj;
                    DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    pf_doj = uiDate2.ToString("yyyy-MM-dd");

                    msSQL = "update hrm_trn_tformaccountdetails set " +
                    "pf_no='" + values.pf_no + "'," +
                    "pf_doj='" + pf_doj + "'," +
                    "experience='" + values.experience + "'," +
                    "totalperiod_preservice='" + values.totalperoidofpreservice + "'," +
                    "remarks='" + values.remarks + "'" +
                    "where accountdtl_gid='" + objOdbcDataReader["accountdtl_gid"].ToString() + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "PF Details updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occred While Updating Record";
                    }

                }

                else
                {


                    string uiDateStr2 = values.pf_doj;
                    DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    pf_doj = uiDate2.ToString("yyyy-MM-dd");

                    msGetGID = objcmnfunctions.GetMasterGID("FACCN");

                    msSQL = " insert into hrm_trn_tformaccountdetails(" +
                    " accountdtl_gid, " +
                    " employee_gid," +
                    " pf_no, " +
                    " pf_doj," +
                    " experience," +
                    " totalperiod_preservice," +
                    " remarks ," +
                    " created_date," +
                    " created_by)" +
                    " values (" +
                    "'" + msGetGID + "'," +
                    "'" +values.employee_gid + "'," +
                    "'" + values.pf_no + "'," +
                    "'" + pf_doj + "'," +
                    "'" + values.experience + "'," +
                    "'" + values.totalperoidofpreservice + "'," +
                    "'" + values.remarks + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    "'" + employee_gid + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "PF Details Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occred While Adding Record";
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
    }
}