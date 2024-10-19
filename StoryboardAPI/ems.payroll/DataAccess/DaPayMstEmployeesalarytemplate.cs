﻿using ems.payroll.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using MySql.Data.MySqlClient;


namespace ems.payroll.DataAccess
{
    public class DaPayMstEmployeesalarytemplate
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult, mnResult1;
        string msGetGid, msGetGid1, lsempoyeegid, lssalarycomponent_gid, lssalarycomponent_name;

        // Module Master Summary
        public void DaGetEmployeesalarytemplateSummary(MdlPayMstEmployeesalarytemplate values)
        {
            try
            {
                
                msSQL = " Select distinct format(a.basic_salary,2) as basic_salary,format(a.net_salary,2) as net_salary, " +
                 " concat(n.unit_name,'-',m.block_name,'-',l.section_name) as unit_name, " +
                 " format(a.gross_salary,2) as gross_salary,b.user_gid,h.salarygradetemplate_name, " +
                 " a.employee2salarygradetemplate_gid,concat( b.user_code,' / ',ifnull(b.user_firstname,''),' ',ifnull(b.user_lastname,'')) as user_name, " +
                 " d.designation_name, c.employee_gid, e.branch_name, " +
                 " c.department_gid, c.branch_gid, e.branch_name, g.department_name " +
                 " FROM pay_trn_temployee2salarygradetemplate a " +
                 " left join hrm_mst_temployee c on  a.employee_gid = c.employee_gid " +
                 " left join adm_mst_tuser b on c.user_gid=b.user_gid " +
                 " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                 " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                 " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                 " left join pay_mst_tsalarygradetemplate h on a.salarygradetemplate_gid=h.salarygradetemplate_gid " +
                 " left join hrm_trn_temployeetypedtl i on c.employee_gid=i.employee_gid " +
                 " left join hrm_mst_tsectionassign2employee k on k.employee_gid = c.employee_gid " +
                 "  left join hrm_mst_tsection l on k.section_gid=l.section_gid " +
                 " left join hrm_mst_tblock m on k.block_gid=m.block_gid " +
                 " left join hrm_mst_tunit n on n.unit_gid=k.unit_gid " +
                 " WHERE 0=0  group by c.employee_gid order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<salarygrade_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new salarygrade_list
                        {
                            basic_salary = dt["basic_salary"].ToString(),
                            net_salary = dt["net_salary"].ToString(),
                            unit_name = dt["unit_name"].ToString(),
                            gross_salary = dt["gross_salary"].ToString(),
                            user_gid = dt["user_gid"].ToString(),
                            salarygradetemplate_name = dt["salarygradetemplate_name"].ToString(),
                            employee2salarygradetemplate_gid = dt["employee2salarygradetemplate_gid"].ToString(),
                            user_name = dt["user_name"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            department_name = dt["department_name"].ToString(),
                        });
                        values.salarygrade_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Employee salary template!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }
        public void DaGettemplateName(MdlPayMstEmployeesalarytemplate values)
        {
            try
            {
                
                msSQL = " select salarygradetemplate_gid, salarygradetemplate_code, " +
                    " salarygradetemplate_name,basic_salary, gross_salary, net_salary " +
                    " from pay_mst_tsalarygradetemplate where salarystartfrom_flag='N' order by salarygradetemplate_name";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<template_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new template_list
                        {
                            salarygradetemplate_name = dt["salarygradetemplate_name"].ToString(),
                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            salarygradetemplate_code = dt["salarygradetemplate_code"].ToString(),
                            basic_salary = dt["basic_salary"].ToString(),
                            net_salary = dt["net_salary"].ToString(),
                            gross_salary = dt["gross_salary"].ToString(),
                        });
                        values.template_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Template name!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
         
        }

        public void DaDetailssummary(string salarygradetemplate_gid, MdlPayMstEmployeesalarytemplate values)
        {
            try
            {
                
                msSQL = " select salarygradetemplate_gid, salarygradetemplate_code, " +
                    " salarygradetemplate_name,basic_salary, gross_salary, net_salary " +
                    " from pay_mst_tsalarygradetemplate where salarygradetemplate_gid='" + salarygradetemplate_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<template_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new template_list
                        {
                            salarygradetemplate_name = dt["salarygradetemplate_name"].ToString(),
                            template_name = dt["salarygradetemplate_name"].ToString(),
                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            salarygradetemplate_code = dt["salarygradetemplate_code"].ToString(),
                            basic_salary = dt["basic_salary"].ToString(),
                            net_salary = dt["net_salary"].ToString(),
                            gross_salary = dt["gross_salary"].ToString(),
                        });
                        values.template_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Detail summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        
        }
        public void DaGetComponentpopup(string employee2salarygradetemplate_gid, string salarygradetype, MdlPayMstEmployeesalarytemplate values)
        {
            try
            {
                
                msSQL = " select a.salarycomponent_name,a.salarycomponent_percentage,a.salarycomponent_amount,a.salarycomponent_amount_employer,a.othercomponent_type as other_type from pay_trn_temployee2salarygradetemplatedtl a" +
                    " where a.employee2salarygradetemplate_gid = '" + employee2salarygradetemplate_gid + "' and a.salarygradetype= '" + salarygradetype + "' and a.primecomponent_flag <>'Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<AddtionalComponent>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new AddtionalComponent
                        {
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            salarycomponent_amount = dt["salarycomponent_amount"].ToString(),
                            salarycomponent_amount_employer = dt["salarycomponent_amount_employer"].ToString(),
                            other_type = dt["other_type"].ToString(),

                        });
                        values.AddtionalComponent = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component group!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaAddtictionsummary(string salarygradetemplate_gid, string salarygradetype, MdlPayMstEmployeesalarytemplate values)
        {
            try
            {
                
                msSQL = " select salarygradetemplatedtl_gid, salarygradetemplate_gid, salarygradetype,salarycomponent_gid, " +
                " salarycomponent_name,componentgroup_gid, componentgroup_name,salarycomponent_amount, affect_in,salarycomponent_percentage,salarycomponent_percentage_employer, " +
                " case when salarycomponent_amount_employer is null then 0.00 else salarycomponent_amount_employer end as salarycomponent_amount_employer " +
                " from pay_mst_tsalarygradetemplatedtl " +
                " where salarygradetemplate_gid='" + salarygradetemplate_gid + "' and salarygradetype ='" + salarygradetype + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<summaryaddition_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new summaryaddition_list
                        {
                            salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetype = dt["salarygradetype"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            employee_contribution = dt["salarycomponent_amount"].ToString(),
                            affect_in = dt["affect_in"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            salarycomponent_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                            employer_contribution = dt["salarycomponent_amount_employer"].ToString(),

                        });
                        values.summaryaddition_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding Addiction summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void Dadeductionsummary(string salarygradetemplate_gid, string salarygradetype, MdlPayMstEmployeesalarytemplate values)
        {
            try
            {
                
                msSQL = " select salarygradetemplatedtl_gid, salarygradetemplate_gid, salarygradetype,salarycomponent_gid, " +
                " salarycomponent_name,componentgroup_gid, componentgroup_name,salarycomponent_amount, affect_in,salarycomponent_percentage,salarycomponent_percentage_employer, " +
                " case when salarycomponent_amount_employer is null then 0.00 else salarycomponent_amount_employer end as salarycomponent_amount_employer " +
                " from pay_mst_tsalarygradetemplatedtl " +
                " where salarygradetemplate_gid='" + salarygradetemplate_gid + "' and salarygradetype = '" + salarygradetype + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<summarydeduction_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new summarydeduction_list
                        {
                            salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetype = dt["salarygradetype"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            demployee_contribution = dt["salarycomponent_amount"].ToString(),
                            affect_in = dt["affect_in"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            salarycomponent_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                            demployer_contribution = dt["salarycomponent_amount_employer"].ToString(),

                        });
                        values.summarydeduction_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding deduction summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetGetEmployeegradeassignsummary(MdlPayMstEmployeesalarytemplate values)
        {
            try
            {
                
                msSQL = " Select distinct a.user_gid, concat(n.unit_name,'-',m.block_name,'-',l.section_name) as unit_name," +
                      "a.user_firstname, a.user_code, " +
                      " d.designation_name ,c.employee_gid,e.branch_name, " +
                      " CASE " +
                      " WHEN a.user_status = 'Y' THEN 'Active' " +
                      " WHEN a.user_status = 'N' THEN 'Inactive' " +
                      " END as user_status,c.department_gid,c.branch_gid, e.branch_name, g.department_name " +
                      " FROM adm_mst_tuser a " +
                      " left join hrm_mst_temployee c on a.user_gid = c.user_gid " +
                      " left join adm_mst_tdesignation d on c.designation_gid = d.designation_gid " +
                      " left join hrm_mst_tbranch e on c.branch_gid = e.branch_gid " +
                      " left join hrm_mst_tdepartment g on g.department_gid = c.department_gid " +
                      " left join hrm_trn_temployeetypedtl h on c.employee_gid=h.employee_gid " +
                      " left join hrm_mst_temployeetype j on h.employeetype_name= j.employee_type " +
                      " left join hrm_mst_tsectionassign2employee k on k.employee_gid = c.employee_gid " +
                      " left join hrm_mst_tsection l on k.section_gid=l.section_gid " +
                      " left join hrm_mst_tblock m on k.block_gid=m.block_gid " +
                      " left join hrm_mst_tunit n on n.unit_gid=k.unit_gid " +
                      " WHERE  a.user_status='Y' " +
                      " and c.employee_gid not in " +
                      "(select employee_gid from pay_trn_temployee2salarygradetemplate ) order by c.created_date desc,a.user_code asc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getgradeassign_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getgradeassign_list
                        {
                            user_gid = dt["user_gid"].ToString(),
                            unit_name = dt["unit_name"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            user_code = dt["user_code"].ToString(),
                            designation_name = dt["designation_name"].ToString(),
                            employee_gid = dt["employee_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            user_status = dt["user_status"].ToString(),
                            department_gid = dt["department_gid"].ToString(),
                            branch_gid = dt["branch_gid"].ToString(),
                            department_name = dt["department_name"].ToString(),
                        });
                        values.Getgradeassign_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Employee grade assign!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }



        public void DaPostSalaryGradetoemployee(string user_gid, SalaryGradetoemployee values)
        {
            try
            {
                
                foreach (var data in values.employee_lists)
                {
                    string msgetsalarygradeGID = objcmnfunctions.GetMasterGID("PSAM");
                    if (msgetsalarygradeGID == "E")
                    {
                        values.status = false;
                        values.message = "Error While generating Gid";

                    }
                    else
                    {

                        foreach (var data1 in values.summaryaddition_list)
                        {
                            string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSDM");
                            if (msGetsalrygradedtlGID == "E")
                            {
                                values.status = false;
                                values.message = "Error While generating Gid";
                                return;

                            }
                            msSQL = "select salarycomponent_gid from pay_mst_tsalarycomponent where  component_name = '" + data1.salarycomponent_name + "'";
                            string lssalarycomponent_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = " insert into pay_trn_temployee2salarygradetemplatedtl (" +
                                            " employee2salarygradetemplatedtl_gid," +
                                            " employee2salarygradetemplate_gid," +
                                            " salarygradetemplate_giddtl," +
                                            " salarygradetype," +
                                            " componentgroup_gid," +
                                            " componentgroup_name," +
                                            " salarycomponent_name," +
                                            " created_by," +
                                            " created_date," +
                                            " salarycomponent_percentage, " +
                                            " salarycomponent_amount," +
                                            " salarycomponent_percentage_employer, " +
                                            " salarycomponent_amount_employer," +
                                            " salarycomponent_gid," +
                                            " affect_in" +
                                            ")values(" +
                               " '" + msGetsalrygradedtlGID + "'," +
                               " '" + msgetsalarygradeGID + "'," +
                               " '" + values.salarygradetemplate_gid + "'," +
                               " '" + data1.salarygradetype + "'," +
                               " '" + data1.componentgroup_gid + "'," +
                               " '" + data1.componentgroup_name + "'," +
                               " '" + data1.salarycomponent_name + "'," +
                               " '" + user_gid + "'," +
                               " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                               " '" + data1.salarycomponent_percentage + "'," +
                               " '" + data1.employee_contribution + "'," +
                               " '" + data1.salarycomponent_percentage_employer + "'," +
                               " '" + data1.employer_contribution + "'," +
                               " '" + lssalarycomponent_gid + "'," +
                               " '" + data1.affect_in + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                        foreach (var data2 in values.summarydeduction_list)
                        {
                            if (data2.salarycomponent_name == "" || data2.salarycomponent_name == null)
                            {
                                msSQL = "select component_name from pay_mst_tsalarycomponent where  salarycomponent_gid = '" + data2.salarycomponent_gid + "'";
                                lssalarycomponent_name = objdbconn.GetExecuteScalar(msSQL);
                            }
                            else
                            { lssalarycomponent_name = data2.salarycomponent_name; }
                            if (data2.salarycomponent_gid == "" || data2.salarycomponent_gid == null)
                            {
                                msSQL = "select salarycomponent_gid from pay_mst_tsalarycomponent where  component_name = '" + data2.salarycomponent_name + "'";
                                lssalarycomponent_gid = objdbconn.GetExecuteScalar(msSQL);
                            }
                            else
                            { lssalarycomponent_gid = data2.salarycomponent_gid; }



                            string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSDM");
                            msSQL = " insert into pay_trn_temployee2salarygradetemplatedtl (" +
                                        " employee2salarygradetemplatedtl_gid," +
                                        " employee2salarygradetemplate_gid," +
                                        " salarygradetemplate_giddtl," +
                                        " salarygradetype," +
                                        " componentgroup_gid," +
                                        " componentgroup_name," +
                                        " salarycomponent_name," +
                                        " created_by," +
                                        " created_date," +
                                        " salarycomponent_percentage, " +
                                        " salarycomponent_amount," +
                                        " salarycomponent_percentage_employer, " +
                                        " salarycomponent_amount_employer," +
                                        " salarycomponent_gid," +
                                        " affect_in" +
                                        ")values(" +
                           " '" + msGetsalrygradedtlGID + "'," +
                           " '" + msgetsalarygradeGID + "'," +
                           " '" + values.salarygradetemplate_gid + "'," +
                           " '" + data2.salarygradetype + "'," +
                           " '" + data2.componentgroup_gid + "'," +
                           " '" + data2.componentgroup_name + "'," +
                           " '" + lssalarycomponent_name + "'," +
                           " '" + user_gid + "'," +
                           " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                           " '" + data2.salarycomponent_percentage + "'," +
                           " '" + data2.demployee_contribution + "'," +
                           " '" + data2.salarycomponent_percentage_employer + "'," +
                           " '" + data2.demployer_contribution + "'," +
                           " '" + lssalarycomponent_gid + "'," +
                           " '" + data2.affect_in + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }

                        msSQL = "select user_gid from  hrm_mst_temployee where employee_gid= '" + data.employee_gid + "'";
                        string lsuser_gid = objdbconn.GetExecuteScalar(msSQL);


                        msSQL = " insert into pay_trn_temployee2salarygradetemplate(" +
                            " employee2salarygradetemplate_gid ," +
                            " salarygradetemplate_gid," +
                            " employee_gid ," +
                            " user_gid ," +
                            " created_by," +
                            " created_date," +
                            " basic_salary, " +
                            " gross_salary," +
                            " ctc, " +
                            " salary_mode," +
                            " net_salary" +
                            " )values(" +
                        " '" + msgetsalarygradeGID + "'," +
                        " '" + values.salarygradetemplate_gid + "'," +
                        " '" + data.employee_gid + "'," +
                        " '" + lsuser_gid + "'," +
                        " '" + user_gid + "'," +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " '" + values.NewBasicSalary + "'," +
                        " '" + values.gross_salary + "'," +
                        " '" + values.ctc + "'," +
                        " '" + "Basic" + "'," +
                        " '" + values.net_salary + "')";

                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                if (mnResult1 != 0)
                {
                    values.status = true;
                    values.message = "Salary Grade Assigned to employee  Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Assigning  Salary Grade to employee";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Employee salary grade assign!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaEditAddtictionsummary(string employee2salarygradetemplate_gid, string salarygradetype, MdlPayMstEmployeesalarytemplate values)
        {
            try
            {
                
                msSQL = " select distinct a.employee2salarygradetemplatedtl_gid,a.salarygradetemplate_giddtl,a.employee2salarygradetemplate_gid,a.salarygradetype, a.componentgroup_name, " +
                             " a.componentgroup_gid,a.salarycomponent_name,a.salarycomponent_percentage,a.salarycomponent_amount, a.affect_in, a.salarycomponent_percentage_employer, " +
                             " a.salarycomponent_amount_employer, a.othercomponent_type, a.salarycomponent_gid from pay_trn_temployee2salarygradetemplatedtl a " +
                            " where a.employee2salarygradetemplate_gid ='" + employee2salarygradetemplate_gid + "'" +
                            " and a.salarygradetype='Addition' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editsummaryaddition_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Editsummaryaddition_list
                        {
                            employee2salarygradetemplatedtl_gid = dt["employee2salarygradetemplatedtl_gid"].ToString(),
                            employee2salarygradetemplate_gid = dt["employee2salarygradetemplate_gid"].ToString(),
                            salarygradetemplate_giddtl = dt["salarygradetemplate_giddtl"].ToString(),
                            salarygradetype = dt["salarygradetype"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            employee_contribution = dt["salarycomponent_amount"].ToString(),
                            affect_in = dt["affect_in"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            salarycomponent_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                            employer_contribution = dt["salarycomponent_amount_employer"].ToString(),

                        });
                        values.Editsummaryaddition_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading edit Addiction Summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaEditdeductionsummary(string employee2salarygradetemplate_gid, string salarygradetype, MdlPayMstEmployeesalarytemplate values)
        {
            try
            {
                
                msSQL = " select a.employee2salarygradetemplatedtl_gid,a.employee2salarygradetemplate_gid,a.salarygradetemplate_giddtl,a.salarygradetype,a.salarycomponent_name, a.componentgroup_name, " +
                             " a.componentgroup_gid, b.component_name,a.salarycomponent_percentage,a.salarycomponent_amount, a.affect_in, a.salarycomponent_percentage_employer, " +
                             " a.salarycomponent_amount_employer, a.othercomponent_type, a.salarycomponent_gid from pay_trn_temployee2salarygradetemplatedtl a " +
                             " left join pay_mst_tsalarycomponent b on a.salarycomponent_name = b.component_name" +
                            " where a.employee2salarygradetemplate_gid ='" + employee2salarygradetemplate_gid + "'" +
                            " and a.salarygradetype='Deduction' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editsummarydeduction_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Editsummarydeduction_list
                        {
                            employee2salarygradetemplatedtl_gid = dt["employee2salarygradetemplatedtl_gid"].ToString(),
                            employee2salarygradetemplate_gid = dt["employee2salarygradetemplate_gid"].ToString(),
                            salarygradetemplate_giddtl = dt["salarygradetemplate_giddtl"].ToString(),
                            salarygradetype = dt["salarygradetype"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            demployee_contribution = dt["salarycomponent_amount"].ToString(),
                            affect_in = dt["affect_in"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            salarycomponent_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                            demployer_contribution = dt["salarycomponent_amount_employer"].ToString(),

                        });
                        values.Editsummarydeduction_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading edit Deduction Summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaEditDetailssummary(string employee2salarygradetemplate_gid, MdlPayMstEmployeesalarytemplate values)
        {
            try
            {
                
                msSQL = " select  salarygradetemplate_gid from pay_trn_temployee2salarygradetemplate  " +
                   " where employee2salarygradetemplate_gid ='" + employee2salarygradetemplate_gid + "' ";
                string lssalarygradetemplate_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select salarygradetemplate_name from pay_mst_tsalarygradetemplate   " +
                      " where salarygradetemplate_gid='" + lssalarygradetemplate_gid + "' ";
                string lssalarygradetemplate_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select  employee2salarygradetemplate_gid,salarygradetemplate_gid,employee_gid, " +
                        " basic_salary,gross_salary,net_salary,ctc from pay_trn_temployee2salarygradetemplate  " +
                        " where employee2salarygradetemplate_gid ='" + employee2salarygradetemplate_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<edittemplate_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new edittemplate_list
                        {
                            employee2salarygradetemplate_gid = dt["employee2salarygradetemplate_gid"].ToString(),
                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            template_name = lssalarygradetemplate_name,
                            employee_gid = dt["employee_gid"].ToString(),
                            basic_salary = dt["basic_salary"].ToString(),
                            net_salary = dt["net_salary"].ToString(),
                            gross_salary = dt["gross_salary"].ToString(),
                            ctc = dt["ctc"].ToString(),

                        });
                        values.edittemplate_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading edit Detail Summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaUpdateSalaryGradetoemployee(string user_gid, updateSalaryGradetoemployee values)
        {
            try
            {
                
                string lsemployeehistory = objcmnfunctions.GetMasterGID("EMPH");
                if (lsemployeehistory == "E")
                {
                    values.status = false;
                    values.message = "Error While generating Gid";
                    return;

                }

                msSQL = "select employee_gid from  pay_trn_temployee2salarygradetemplate WHERE employee2salarygradetemplate_gid = '" + values.employee2salarygradetemplate_gid + "'";
                string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " Insert into hrm_trn_temployeehistory ( " +
                    " employeehistory_gid , " +
                    " employee_gid ,  " +
                    " salarygradetemplate_gid ,  " +
                    " basic_salary , " +
                    " gross_salary ,  " +
                    " ctc , " +
                    " net_salary ,  " +
                    " created_by , " +
                    " created_date  " +
                       " )values ( " +
                    " '" + lsemployeehistory + "'," +
                    " '" + lsemployee_gid + "'," +
                    " '" + values.salarygradetemplate_gid + "'," +
                     " '" + values.NewBasicSalary + "'," +
                    " '" + values.gross_salary + "'," +
                    " '" + values.ctc + "'," +
                    " '" + values.net_salary + "'," +
                    " '" + user_gid + "'," +
                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "delete from pay_trn_temployee2salarygradetemplatedtl where employee2salarygradetemplate_gid = '" + values.employee2salarygradetemplate_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                foreach (var data in values.Editsummaryaddition_list)
                {
                    string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSDM");
                    if (msGetsalrygradedtlGID == "E")
                    {
                        values.status = false;
                        values.message = "Error While generating Gid";
                        return;

                    }
                    msSQL = "select salarycomponent_gid,primecomponent_flag from pay_mst_tsalarycomponent where  component_name = '" + data.salarycomponent_name + "'";
                    string lssalarycomponent_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select primecomponent_flag from pay_mst_tsalarycomponent where  component_name = '" + data.salarycomponent_name + "'";
                    string lsprimecomponent_flag = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " insert into pay_trn_temployee2salarygradetemplatedtl (" +
                              " employee2salarygradetemplatedtl_gid," +
                              " employee2salarygradetemplate_gid," +
                              " salarygradetemplate_giddtl," +
                              " salarygradetype," +
                              " componentgroup_gid," +
                              " componentgroup_name," +
                              " salarycomponent_name," +
                              " created_by," +
                              " created_date," +
                              " salarycomponent_percentage, " +
                              " salarycomponent_amount," +
                              " salarycomponent_percentage_employer, " +
                              " salarycomponent_amount_employer," +
                              " salarycomponent_gid," +
                              " primecomponent_flag," +
                              " affect_in" +
                              ")values(" +
                       " '" + msGetsalrygradedtlGID + "'," +
                       " '" + data.employee2salarygradetemplate_gid + "'," +
                       " '" + data.salarygradetemplate_giddtl + "'," +
                       " '" + data.salarygradetype + "'," +
                       " '" + data.componentgroup_gid + "'," +
                       " '" + data.componentgroup_name + "'," +
                       " '" + data.salarycomponent_name + "'," +
                       " '" + user_gid + "'," +
                       " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       " '" + data.salarycomponent_percentage + "'," +
                       " '" + data.employee_contribution + "'," +
                       " '" + data.salarycomponent_percentage_employer + "'," +
                       " '" + data.employer_contribution + "'," +
                       " '" + lssalarycomponent_gid + "'," +
                       " '" + lsprimecomponent_flag + "'," +
                       " '" + data.affect_in + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }
                foreach (var data in values.Editsummarydeduction_list)
                {
                    if (data.salarycomponent_name == "" || data.salarycomponent_name == null)
                    {
                        msSQL = "select component_name from pay_mst_tsalarycomponent where  salarycomponent_gid = '" + data.salarycomponent_gid + "'";
                        lssalarycomponent_name = objdbconn.GetExecuteScalar(msSQL);
                    }
                    else
                    { lssalarycomponent_name = data.salarycomponent_name; }
                    if (data.salarycomponent_gid == "" || data.salarycomponent_gid == null)
                    {
                        msSQL = "select salarycomponent_gid from pay_mst_tsalarycomponent where  component_name = '" + data.salarycomponent_name + "'";
                        lssalarycomponent_gid = objdbconn.GetExecuteScalar(msSQL);
                    }
                    else
                    { lssalarycomponent_gid = data.salarycomponent_gid; }



                    string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSDM");
                    if (msGetsalrygradedtlGID == "E")
                    {
                        values.status = false;
                        values.message = "Error While generating Gid";
                        return;

                    }
                    msSQL = " insert into pay_trn_temployee2salarygradetemplatedtl (" +
                                   " employee2salarygradetemplatedtl_gid," +
                                   " employee2salarygradetemplate_gid," +
                                   " salarygradetemplate_giddtl ," +
                                   " salarygradetype," +
                                   " componentgroup_gid," +
                                   " componentgroup_name," +
                                   " salarycomponent_name," +
                                   " created_by," +
                                   " created_date," +
                                   " salarycomponent_percentage, " +
                                   " salarycomponent_amount," +
                                   " salarycomponent_percentage_employer, " +
                                   " salarycomponent_amount_employer," +
                                   " salarycomponent_gid, " +
                                   " affect_in" +
                                   ")values(" +
                       " '" + msGetsalrygradedtlGID + "'," +
                       " '" + data.employee2salarygradetemplate_gid + "'," +
                       " '" + data.salarygradetemplate_giddtl + "'," +
                       " '" + data.salarygradetype + "'," +
                       " '" + data.componentgroup_gid + "'," +
                       " '" + data.componentgroup_name + "'," +
                       " '" + lssalarycomponent_name + "'," +
                       " '" + user_gid + "'," +
                       " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       " '" + data.salarycomponent_percentage + "'," +
                       " '" + data.demployee_contribution + "'," +
                       " '" + data.salarycomponent_percentage_employer + "'," +
                       " '" + data.demployer_contribution + "'," +
                       " '" + lssalarycomponent_gid + "'," +
                       " '" + data.affect_in + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }

          msSQL = "update pay_trn_temployee2salarygradetemplate set" +
                " salarygradetemplate_gid= '" + values.salarygradetemplate_gid + "'," +
                " basic_salary= '" + values.NewBasicSalary + "'," +
                " gross_salary='" + values.gross_salary + "'," +
                " ctc='" + values.ctc + "'," +
                " net_salary='" + values.net_salary + "'" +
                " where employee2salarygradetemplate_gid ='" + values.employee2salarygradetemplate_gid + "'";

                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult1 != 0)
                {
                    values.status = true;
                    values.message = "Update Salary Grade  to employee  Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating  Salary Grade to employee";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while update Salary grade to Employee!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
          
        }
    }
}