using ems.payroll.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;

namespace ems.payroll.DataAccess
{
    public class DaPayMstEditEmp2Grade
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        OdbcDataReader objOdbcDataReader, objOdbcDataReader1, objOdbcDataReader2, objOdbcDataReader3;
        //OdbcDataReader objMySqlDataReader;

        DataTable dt_datatable;
        int mnResult, lssalarycomponent_percentage;
        string msGetGid, msGetGid1, lsempoyeegid, msTemporaryAddressGetGID, lsemployee2salarygradetemplate_gid, msGetemployeetype, msPermanentAddressGetGID, msgetshift, lssalarycomponent_name, lssalarycomponent_gid, msUserGid, msEmployeeGID;
        double lsbasicsalary;
        Double lsother_addition = 0.00;
        Double lsotheraddition_employer = 0.00;
        Double lsother_deduction = 0.00;
        Double lsotherdeduction_employer = 0.00;
        Double lsdeductamount = 0.00;
        Double lsbasic_salary = 0.00;
        Double lsnetsalary = 0.00;
        Double lsctc = 0.00;
        Double lsbasic_salary_employeer = 0.00;



        public void DaGetsalarygradetemplatedropdown(MdlPayMstGradeConfirm values)
        {
            try
            {

                msSQL = " select salarygradetemplate_gid,salarygradetemplate_name  " +
                    " from pay_mst_tsalarygradetemplate ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<gradetemplatedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new gradetemplatedropdown
                        {
                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            salarygradetemplate_name = dt["salarygradetemplate_name"].ToString(),
                        });
                        values.gradetemplatedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public Emp2Gradeedit_list Daeditgrade2employeedetails(string employee2salarygradetemplate_gid)
        {
            try
            {
                Emp2Gradeedit_list objEmp2Gradeedit_list = new Emp2Gradeedit_list();
                {
                    msSQL = "select salarygradetemplate_gid, gross_salary, salary_mode,net_salary,ctc " +
                        " from pay_trn_temployee2salarygradetemplate " +
                        " where employee2salarygradetemplate_gid = '" + employee2salarygradetemplate_gid + "'";
                }

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objEmp2Gradeedit_list.salarygradetemplate_gid = objOdbcDataReader["salarygradetemplate_gid"].ToString();
                    objEmp2Gradeedit_list.gross_salary = objOdbcDataReader["gross_salary"].ToString();
                    objEmp2Gradeedit_list.salary_mode = objOdbcDataReader["salary_mode"].ToString();
                    objEmp2Gradeedit_list.net_salary = objOdbcDataReader["net_salary"].ToString();
                    objEmp2Gradeedit_list.ctc = objOdbcDataReader["ctc"].ToString();



                    objOdbcDataReader.Close();
                }
                return objEmp2Gradeedit_list;
            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return null;
            }
        }

        public void DaEditadditionsummary(string employee2salarygradetemplate_gid, MdlPayMstEditEmp2Grade values)
        {
            try
            {
                msSQL = " select a.salarygradetemplatedtl_gid, a.salarygradetemplate_gid, a.componentgroup_name, a.componentgroup_gid, a.salarygradetype,a.salarycomponent_name, " +
                        " a.salarycomponent_percentage, a.salarycomponent_amount, a.affect_in, a.othercomponent_type,a.salarycomponent_percentage_employer, " +
                        " a.salarycomponent_amount_employer, a.salarycomponent_gid from pay_mst_tsalarygradetemplatedtl a " +
                        " left join pay_trn_temployee2salarygradetemplate b on a.salarygradetemplate_gid = b.salarygradetemplate_gid " +
                        " where employee2salarygradetemplate_gid ='" + employee2salarygradetemplate_gid + "' " + 
                        " and salarygradetype='Addition' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editaditional_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Editaditional_list
                        {
                            salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarygradetype = dt["salarygradetype"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            salarycomponent_amount = dt["salarycomponent_amount"].ToString(),
                            affecting_in = dt["affect_in"].ToString(),
                            othercomponent_type = dt["othercomponent_type"].ToString(),
                            salarycomponent_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                            salarycomponent_amount_employer = dt["salarycomponent_amount_employer"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            component_type = dt["salarygradetype"].ToString(),
                            component_percentage = dt["salarycomponent_percentage"].ToString(),
                            component_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                        });
                        values.Editaditional_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading edit Salary Grade!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaEditdeductionsummary(string employee2salarygradetemplate_gid, MdlPayMstEditEmp2Grade values)
        {
            try
            {
                msSQL = " select a.salarygradetemplatedtl_gid, a.salarygradetemplate_gid, a.componentgroup_name, a.componentgroup_gid, a.salarygradetype,a.salarycomponent_name, " +
                        " a.salarycomponent_percentage, a.salarycomponent_amount, a.affect_in, a.othercomponent_type,a.salarycomponent_percentage_employer, " +
                        " a.salarycomponent_amount_employer, a.salarycomponent_gid from pay_mst_tsalarygradetemplatedtl a " +
                        " left join pay_trn_temployee2salarygradetemplate b on a.salarygradetemplate_gid = b.salarygradetemplate_gid " +
                        " where employee2salarygradetemplate_gid ='" + employee2salarygradetemplate_gid + "' " +
                        " and salarygradetype='Deduction' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editdeductional_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Editdeductional_list
                        {
                            salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarygradetype = dt["salarygradetype"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            salarycomponent_amount = dt["salarycomponent_amount"].ToString(),
                            affecting_in = dt["affect_in"].ToString(),
                            othercomponent_type = dt["othercomponent_type"].ToString(),
                            salarycomponent_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                            salarycomponent_amount_employer = dt["salarycomponent_amount_employer"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            component_type = dt["salarygradetype"].ToString(),
                            component_percentage = dt["salarycomponent_percentage"].ToString(),
                            component_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                        });
                        values.Editdeductional_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading edit Salary Grade!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaEditotherssummary(string employee2salarygradetemplate_gid, MdlPayMstEditEmp2Grade values)
        {
            try
            {
                msSQL = " select a.salarygradetemplatedtl_gid, a.salarygradetemplate_gid, a.componentgroup_name, a.componentgroup_gid, a.salarygradetype,a.salarycomponent_name, " +
                        " a.salarycomponent_percentage, a.salarycomponent_amount, a.affect_in, a.othercomponent_type,a.salarycomponent_percentage_employer, " +
                        " a.salarycomponent_amount_employer, a.salarycomponent_gid from pay_mst_tsalarygradetemplatedtl a " +
                        " left join pay_trn_temployee2salarygradetemplate b on a.salarygradetemplate_gid = b.salarygradetemplate_gid " +
                        " where employee2salarygradetemplate_gid ='" + employee2salarygradetemplate_gid + "' " +
                        " and salarygradetype='others' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editotherslist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Editotherslist
                        {
                            salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarygradetype = dt["salarygradetype"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            salarycomponent_amount = dt["salarycomponent_amount"].ToString(),
                            affecting_in = dt["affect_in"].ToString(),
                            othercomponent_type = dt["othercomponent_type"].ToString(),
                            salarycomponent_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                            salarycomponent_amount_employer = dt["salarycomponent_amount_employer"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            component_type = dt["salarygradetype"].ToString(),
                            component_percentage = dt["salarycomponent_percentage"].ToString(),
                            component_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                        });
                        values.Editotherslist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading edit Salary Grade!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaUpdateemployee2grade(string user_gid, employee2editgradelist values)
        {
            try
            {

                if (values.BasicSalary == null || values.BasicSalary == "")
                {
                    values.BasicSalary = "0";
                }
                msSQL = " Update pay_trn_temployee2salarygradetemplate set" +
                        " employee2salarygradetemplate_gid = '" + values.employee2salarygrade_gid + "'," +
                        " salarygradetemplate_gid='" + values.salarygradetemplate_gid + "'," +
                        " salary_mode='" + values.salary_mode + "'," +
                        " user_gid='" + user_gid + "'," +
                        " created_by='" + user_gid + "'," +
                        " created_date='" + DateTime.Now.ToString("yyyyy-MM-dd") + "'," +
                        " basic_Salary='" + values.BasicSalary + "'," +
                        " gross_salary='" + values.gross_salary + "'," +
                        " ctc='" + values.ctc + "'," +
                        " net_salary='" + values.net_salary + "'" +
                        " where employee2salarygradetemplate_gid = '" + values.employee2salarygrade_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = " delete from pay_trn_temployee2salarygradetemplatedtl " +
                        " where employee2salarygradetemplate_gid = '" + values.employee2salarygrade_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (values.Addsummarybind_list != null)
                {
                    foreach (var data1 in values.Addsummarybind_list)
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
                                        " salarycomponent_gid" +
                                        ")values(" +
                                        " '" + msGetsalrygradedtlGID + "'," +
                                        " '" + values.employee2salarygrade_gid + "'," +
                                        " '" + values.salarygradetemplate_gid + "'," +
                                        " 'Addition'," +
                                        " '" + data1.componentgroup_gid + "'," +
                                        " '" + data1.componentgroup_name + "'," +
                                        " '" + data1.salarycomponent_name + "'," +
                                        " '" + user_gid + "'," +
                                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                        " '" + data1.salarycomponent_percentage + "'," +
                                        " '" + data1.salarycomponent_amount + "'," +
                                        " '" + data1.salarycomponent_percentage_employer + "'," +
                                        " '" + data1.salarycomponent_amount_employer + "'," +
                                        " '" + lssalarycomponent_gid + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }
                if (values.DedSummarybind_list != null)
                {
                    foreach (var data2 in values.DedSummarybind_list)
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
                                    " salarycomponent_gid" +
                                    ")values(" +
                                    " '" + msGetsalrygradedtlGID + "'," +
                                    " '" + values.employee2salarygrade_gid + "'," +
                                    " '" + values.salarygradetemplate_gid + "'," +
                                    " 'Deduction'," +
                                    " '" + data2.componentgroup_gid + "'," +
                                    " '" + data2.componentgroup_name + "'," +
                                    " '" + lssalarycomponent_name + "'," +
                                    " '" + user_gid + "'," +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                    " '" + data2.salarycomponent_percentage + "'," +
                                    " '" + data2.salarycomponent_amount + "'," +
                                    " '" + data2.salarycomponent_percentage_employer + "'," +
                                    " '" + data2.salarycomponent_amount_employer + "'," +
                                    " '" + lssalarycomponent_gid + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                }
                if (values.OthersSummarybind_list != null)
                {
                    foreach (var data3 in values.OthersSummarybind_list)
                    {
                        string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSDM");
                        if (msGetsalrygradedtlGID == "E")
                        {
                            values.status = false;
                            values.message = "Error While generating Gid";
                            return;

                        }
                        msSQL = "select salarycomponent_gid from pay_mst_tsalarycomponent where  component_name = '" + data3.salarycomponent_name + "'";
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
                                        " salarycomponent_gid " +
                                        ")values(" +
                                        " '" + msGetsalrygradedtlGID + "'," +
                                        " '" + values.employee2salarygrade_gid + "'," +
                                        " '" + values.salarygradetemplate_gid + "'," +
                                        " 'Others'," +
                                        " '" + data3.componentgroup_gid + "'," +
                                        " '" + data3.componentgroup_name + "'," +
                                        " '" + data3.salarycomponent_name + "'," +
                                        " '" + user_gid + "'," +
                                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                        " '" + data3.salarycomponent_percentage + "'," +
                                        " '" + data3.salarycomponent_amount + "'," +
                                        " '" + data3.salarycomponent_percentage_employer + "'," +
                                        " '" + data3.salarycomponent_amount_employer + "'," +
                                        " '" + lssalarycomponent_gid + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Employee assigned to grade successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while employee assigned to grade";
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