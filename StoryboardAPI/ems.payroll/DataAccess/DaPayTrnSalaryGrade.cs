using ems.payroll.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Web.Http;

//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace ems.payroll.DataAccess
{
    public class DaPayTrnSalaryGrade : ApiController
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        double lscomponent_amount, lscomponent_amountemployer;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsempoyeegid;

        // Module Summary
        public void DaSalarygradeSummary(MdlSalaryGrade values)
        {
            try
            {

                msSQL = " select salarygradetemplate_gid, salarygradetemplate_code, salarygradetemplate_name, created_by," +
                " created_date, format(basic_salary,2)as basic_salary , format(gross_salary,2)as gross_salary ," +
                " format(net_salary,2)As net_salary from pay_mst_tsalarygradetemplate" +
                " Order by salarygradetemplate_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<salarygradetemplate_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new salarygradetemplate_list
                        {
                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            salarygradetemplate_name = dt["salarygradetemplate_name"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            basic_salary = dt["basic_salary"].ToString(),
                            gross_salary = dt["gross_salary"].ToString(),
                            net_salary = dt["net_salary"].ToString(),

                        });
                        values.salarygrade_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Salary grade!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaAdditionComponentSummary( MdlSalaryGrade values, string salarytype, string basic_salary)
        {
            try
            {

                msSQL = " select a.salarycomponent_gid, a.component_code, a.componentgroup_name,a.componentgroup_gid," +
                " a.component_type, a.affecting_in,a.component_flag,a.affecting_in, " +
                " a.component_percentage ,a.component_amount,a.component_flag_employer, " +
                " case when a.component_amount_employer is null then 0.00 else a.component_amount_employer end as component_amount_employer, " +
                " case when a.component_percentage_employer is null then 0.00 else a.component_percentage_employer end as component_percentage_employer " +
                " from pay_mst_tsalarycomponent a " +
                " left join pay_mst_tcomponentgroupmaster b on b.componentgroup_gid = a.componentgroup_gid " +
                " where component_type='Addition' and b.group_belongsto='" + salarytype + "'  group by componentgroup_name";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<addition_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new addition_list
                        {
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            component_code = dt["component_code"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            component_type = dt["component_type"].ToString(),
                            affecting_in = dt["affecting_in"].ToString(),
                            component_flag = dt["component_flag"].ToString(),
                            component_percentage = dt["component_percentage"].ToString(),
                            component_amount = dt["component_amount"].ToString(),
                            component_flag_employer = dt["component_flag_employer"].ToString(),
                            component_amount_employer = dt["component_amount_employer"].ToString(),
                            employee_contribution = "0",
                            employer_contribution = "0",
                            component_percentage_employer = dt["component_percentage_employer"].ToString(),
                        });
                        values.addition_list = getModuleList;
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

        public void DaDeductionComponentSummary(MdlSalaryGrade values, string salarytype, string basic_salary)
        {
            try
            {

                msSQL = " select a.salarycomponent_gid, a.component_code, a.component_name, a.componentgroup_name,a.componentgroup_gid," +
                " a.component_type, a.affecting_in,a.component_flag,a.affecting_in, " +
                " a.component_percentage ,a.component_amount,a.component_flag_employer, " +
                " case when a.component_amount_employer is null then 0.00 else a.component_amount_employer end as component_amount_employer, " +
                " case when a.component_percentage_employer is null then 0.00 else a.component_percentage_employer end as component_percentage_employer " +
                " from pay_mst_tsalarycomponent a " +
                " left join pay_mst_tcomponentgroupmaster b on b.componentgroup_gid = a.componentgroup_gid " +
                " where component_type='Deduction' and b.group_belongsto='" + salarytype + "'  group by componentgroup_name";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<deduction_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new deduction_list
                        {
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            component_code = dt["component_code"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            component_type = dt["component_type"].ToString(),
                            affecting_in = dt["affecting_in"].ToString(),
                            component_flag = dt["component_flag"].ToString(),
                            component_percentage = dt["component_percentage"].ToString(),
                            component_amount = dt["component_amount"].ToString(),
                            component_flag_employer = dt["component_flag_employer"].ToString(),
                            demployee_contribution = "0",
                            demployer_contribution = "0",
                            component_amount_employer = dt["component_amount_employer"].ToString(),
                            component_percentage_employer = dt["component_percentage_employer"].ToString(),
                        });
                        values.deduction_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Deduction Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaOthersComponentSummary(MdlSalaryGrade values, string salarytype, string basic_salary)
        {
            try
            {

                msSQL = " select a.salarycomponent_gid, a.component_code, a.component_name, a.componentgroup_name,a.componentgroup_gid," +
                " a.component_type, a.affecting_in,a.component_flag,a.affecting_in, " +
                " a.component_percentage ,a.component_amount,a.component_flag_employer, " +
                " case when a.component_amount_employer is null then 0.00 else a.component_amount_employer end as component_amount_employer, " +
                " case when a.component_percentage_employer is null then 0.00 else a.component_percentage_employer end as component_percentage_employer " +
                " from pay_mst_tsalarycomponent a " +
                " left join pay_mst_tcomponentgroupmaster b on b.componentgroup_gid = a.componentgroup_gid " +
                " where component_type='Others' and b.group_belongsto='" + salarytype + "'  group by componentgroup_name";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<others_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new others_list
                        {
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            component_code = dt["component_code"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            component_type = dt["component_type"].ToString(),
                            affecting_in = dt["affecting_in"].ToString(),
                            component_flag = dt["component_flag"].ToString(),
                            component_percentage = dt["component_percentage"].ToString(),
                            component_amount = dt["component_amount"].ToString(),
                            component_flag_employer = dt["component_flag_employer"].ToString(),
                            component_amount_employer = dt["component_amount_employer"].ToString(),
                            component_percentage_employer = dt["component_percentage_employer"].ToString(),
                        });
                        values.others_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Deduction Component!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetcomponentname(string componentgroup_name, MdlSalaryGrade values)
        {
            try
            {

                msSQL = "select component_name,salarycomponent_gid from pay_mst_tsalarycomponent where componentgroup_name='" + componentgroup_name + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetComponentname>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetComponentname
                        {
                            salarycomponent_name = dt["component_name"].ToString(),
                            component_name = dt["component_name"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                        });
                        values.GetComponentname = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }



        public void DaGetcomponentamount(string salarycomponent_gid, MdlSalaryGrade values)
        {
            try
            {

                msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                    " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + salarycomponent_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getcomponentamount>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcomponentamount
                        {
                            affecting_in = dt["affecting_in"].ToString(),
                            component_flag = dt["component_flag"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            component_percentage = dt["component_percentage"].ToString(),
                            component_amount = dt["component_amount"].ToString(),
                            component_flag_employer = dt["component_flag_employer"].ToString(),
                            component_name = dt["component_name"].ToString(),
                            component_percentage_employer = dt["component_percentage_employer"].ToString(),
                            component_amount_employer = dt["component_amount_employer"].ToString(),
                        });
                        values.Getcomponentamount = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component Amount!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetpopup(string salarygradetemplate_gid, string salarygradetype, MdlSalaryGrade values)
        {
            try
            {

                msSQL = " select a.salarygradetemplate_gid,b.salarycomponent_name,b.salarycomponent_percentage,format(b.salarycomponent_amount,2) as salarycomponent_amount, " +
                " format(b.salarycomponent_amount_employer,2) as salarycomponent_amount_employer from pay_mst_tsalarygradetemplate a" +
                " left join pay_mst_tsalarygradetemplatedtl b on a.salarygradetemplate_gid=b.salarygradetemplate_gid" +
                " where a.salarygradetemplate_gid ='" + salarygradetemplate_gid + "' and b.salarygradetype='" + salarygradetype + "' and b.salarycomponent_name<>'--Select Component--' and b.primecomponent_flag <>'Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<componentdetails>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new componentdetails
                        {

                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            employee_contribution = dt["salarycomponent_amount"].ToString(),
                            employer_contribution = dt["salarycomponent_amount_employer"].ToString(),
                            component_name = dt["salarycomponent_name"].ToString(),
                        });
                        values.componentdetails = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component Details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaUpdateSalaryGrade(string user_gid, UpdateSalaryGradeData values)
        {
            try
            {

                msSQL = "select salarygradetemplatedtl_gid from pay_mst_tsalarygradetemplatedtl where salarygradetemplate_gid ='" + values.salarygradetemplate_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    msSQL = "delete from pay_mst_tsalarygradetemplatedtl where salarygradetemplate_gid='" + values.salarygradetemplate_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                foreach (var data in values.Editaditional)
                {
                    string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSGD");
                    msSQL = "select salarycomponent_gid from pay_mst_tsalarycomponent where  component_name = '" + data.salarycomponent_name + "'";
                    string lssalarycomponent_gid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " insert into pay_mst_tsalarygradetemplatedtl (" +
                        " salarygradetemplatedtl_gid," +
                        " salarygradetemplate_gid," +
                        " componentgroup_name," +
                        " componentgroup_gid," +
                        " salarygradetype," +
                        " salarycomponent_gid, " +
                        " salarycomponent_name," +
                        " created_by," +
                        " created_date," +
                        " salarycomponent_percentage, " +
                        " salarycomponent_amount," +
                        " affect_in, " +
                        " salarycomponent_percentage_employer, " +
                        " salarycomponent_amount_employer " +
                        ")values(" +
                       " '" + msGetsalrygradedtlGID + "'," +
                       " '" + values.salarygradetemplate_gid + "'," +
                       " '" + data.componentgroup_name + "'," +
                       " '" + data.componentgroup_gid + "'," +
                       " '" + data.component_type + "'," +
                       " '" + lssalarycomponent_gid + "'," +
                       " '" + data.salarycomponent_name + "'," +
                       " '" + user_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       " '" + data.component_percentage + "'," +
                       " '" + data.employee_contribution + "'," +
                       " '" + data.affecting_in + "'," +
                       " '" + data.component_percentage_employer + "'," +
                       " '" + data.employer_contribution + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }
                foreach (var data in values.Editdeduction)
                {
                    string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSGD");
                    msSQL = "select salarycomponent_gid from pay_mst_tsalarycomponent where  component_name  ='" + data.salarycomponent_name + "'";
                    string lssalarycomponent_gid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " insert into pay_mst_tsalarygradetemplatedtl (" +
                        " salarygradetemplatedtl_gid," +
                        " salarygradetemplate_gid," +
                        " componentgroup_name," +
                        " componentgroup_gid," +
                        " salarygradetype," +
                        " salarycomponent_gid, " +
                        " salarycomponent_name," +
                        " created_by," +
                        " created_date," +
                        " salarycomponent_percentage, " +
                        " salarycomponent_amount," +
                        " affect_in, " +
                        " salarycomponent_percentage_employer, " +
                        " salarycomponent_amount_employer " +
                        ")values(" +
                       " '" + msGetsalrygradedtlGID + "'," +
                       " '" + values.salarygradetemplate_gid + "'," +
                       " '" + data.componentgroup_name + "'," +
                       " '" + data.componentgroup_gid + "'," +
                       " '" + data.component_type + "'," +
                       " '" + lssalarycomponent_gid + "'," +
                       " '" + data.salarycomponent_name + "'," +
                       " '" + user_gid + "'," +
                       " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       " '" + data.component_percentage + "'," +
                       " '" + data.demployee_contribution + "'," +
                       " '" + data.affecting_in + "'," +
                       " '" + data.component_percentage_employer + "'," +
                       " '" + data.demployer_contribution + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                }

                msSQL = " update pay_mst_tsalarygradetemplate set" +
                        " salarygradetemplate_name='" + values.template_name + "', " +
                        " basic_salary='" + values.NewBasicSalary + "', " +
                        " gross_salary='" + values.gross_salary + "', " +
                        " net_salary='" + values.net_salary + "', " +
                        " updated_by='" + user_gid + "', " +
                        " ctc='" + values.ctc + "', " +
                        " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                        " where salarygradetemplate_gid='" + values.salarygradetemplate_gid + "' ";
                int mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult1 != 0)
                {
                    values.status = true;
                    values.message = "Salary Grade Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updateding Salary Grade";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while update Salary Grade!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaPostSalaryGrade(string user_gid, SalaryGradeData values)
        {
            try
            {

                string msgetsalarygradeGID = objcmnfunctions.GetMasterGID("PSGT");
                msSQL = " insert into pay_mst_tsalarygradetemplate(" +
                    " salarygradetemplate_gid," +
                    " salarygradetemplate_code," +
                    " salarygradetemplate_name," +
                    " created_by," +
                    " created_date," +
                    " basic_salary, " +
                    " gross_salary," +
                    " ctc, " +
                    " net_salary" +
                    " )values(" +
                    " '" + msgetsalarygradeGID + "'," +
                    " '" + msgetsalarygradeGID + "'," +
                    " '" + values.template_name + "'," +
                    " '" + user_gid + "'," +
                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    "COALESCE('" + values.NewBasicSalary + "', 0)," +
                    "COALESCE('" + values.gross_salary + "', 0)," +
                    "CASE WHEN '" + values.ctc + "' = '' THEN 0 ELSE '" + values.ctc + "' END," +
                    "CASE WHEN '" + values.net_salary + "' = '' THEN 0 ELSE '" + values.net_salary + "' END)";

                int mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                for (int i = 0; i < values.component_name.Count; i++)

                {
                    var lscomponent_name = values.component_name[i];
                    if (!string.IsNullOrEmpty(lscomponent_name))
                    {
                        if (lscomponent_name != "" && lscomponent_name != null)
                        {
                            string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSGD");

                            msSQL = " select componentgroup_name,componentgroup_gid,component_type,component_flag,component_name,component_percentage," +
                                    " component_amount,affecting_in,component_percentage_employer,component_amount_employer " +
                                    " from pay_mst_tsalarycomponent where salarycomponent_gid = '" + lscomponent_name + "'";

                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                            if (objMySqlDataReader.HasRows == true)
                            {

                                if (objMySqlDataReader["component_flag"].ToString() == "Y")
                                {
                                    string affectingIn = objMySqlDataReader["affecting_in"].ToString();

                                    double componentPercentage = double.Parse(objMySqlDataReader["component_percentage"].ToString());

                                    if (affectingIn == "Basic")
                                    {
                                        lscomponent_amount = (componentPercentage / 100) * values.NewBasicSalary;
                                        lscomponent_amountemployer = (componentPercentage / 100) * values.NewBasicSalary;
                                    }
                                    else if (affectingIn == "Gross")
                                    {
                                        lscomponent_amount = (componentPercentage / 100) * values.gross_salary;
                                        lscomponent_amountemployer = (componentPercentage / 100) * values.gross_salary;
                                    }
                                }

                                else
                                {
                                    lscomponent_amount = double.Parse(objMySqlDataReader["component_amount"].ToString());
                                    lscomponent_amountemployer = double.Parse(objMySqlDataReader["component_amount_employer"].ToString());
                                }
                            }


                            msSQL = " insert into pay_mst_tsalarygradetemplatedtl (" +
                                " salarygradetemplatedtl_gid," +
                                " salarygradetemplate_gid," +
                                " componentgroup_name," +
                                " componentgroup_gid," +
                                " salarygradetype," +
                                " salarycomponent_gid, " +
                                " salarycomponent_name," +
                                " created_by," +
                                " created_date," +
                                " salarycomponent_percentage, " +
                                " salarycomponent_amount," +
                                " affect_in, " +
                                " salarycomponent_percentage_employer, " +
                                " salarycomponent_amount_employer " +
                                ")values(" +
                               " '" + msGetsalrygradedtlGID + "'," +
                               " '" + msgetsalarygradeGID + "'," +
                               " '" + objMySqlDataReader["componentgroup_name"].ToString() + "'," +
                               " '" + objMySqlDataReader["componentgroup_gid"].ToString() + "'," +
                               " '" + objMySqlDataReader["component_type"].ToString() + "'," +
                               " '" + lscomponent_name + "'," +
                               " '" + objMySqlDataReader["component_name"].ToString() + "'," +
                               " '" + user_gid + "'," +
                               "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                               " '" + objMySqlDataReader["component_percentage"].ToString().Trim() + "'," +
                               " '" + lscomponent_amount + "'," +
                               " '" + objMySqlDataReader["affecting_in"].ToString() + "'," +
                               " '" + objMySqlDataReader["component_percentage_employer"].ToString().Trim() + "'," +
                               " '" + lscomponent_amountemployer + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }
            

                for(int j = 0;j < values.component_name1.Count;j++)
                {
                    var lsdecom = values.component_name1[j];
                    if (!string.IsNullOrEmpty(lsdecom))
                    {



                        if (lsdecom != "" && lsdecom != null)
                        {
                            string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSGD");

                            msSQL = " select componentgroup_name,componentgroup_gid,component_type,component_flag,component_name,component_percentage," +
                                   " component_amount,affecting_in,component_percentage_employer,component_amount_employer " +
                                   " from pay_mst_tsalarycomponent where salarycomponent_gid = '" + lsdecom + "'";

                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                            if (objMySqlDataReader.HasRows == true)
                            {

                                if (objMySqlDataReader["component_flag"].ToString() == "Y")
                                {
                                    string affectingIn = objMySqlDataReader["affecting_in"].ToString();

                                    double componentPercentage = double.Parse(objMySqlDataReader["component_percentage"].ToString());

                                    if (affectingIn == "Basic")
                                    {
                                        lscomponent_amount = (componentPercentage / 100) * values.NewBasicSalary;
                                        lscomponent_amountemployer = (componentPercentage / 100) * values.NewBasicSalary;
                                    }
                                    else if (affectingIn == "Gross")
                                    {
                                        lscomponent_amount = (componentPercentage / 100) * values.gross_salary;
                                        lscomponent_amountemployer = (componentPercentage / 100) * values.gross_salary;
                                    }
                                }

                                else
                                {
                                    lscomponent_amount = double.Parse(objMySqlDataReader["component_amount"].ToString());
                                    lscomponent_amountemployer = double.Parse(objMySqlDataReader["component_amount_employer"].ToString());
                                }
                            }

                            msSQL = " insert into pay_mst_tsalarygradetemplatedtl (" +
                                    " salarygradetemplatedtl_gid," +
                                    " salarygradetemplate_gid," +
                                    " componentgroup_name," +
                                    " componentgroup_gid," +
                                    " salarygradetype," +
                                    " salarycomponent_gid, " +
                                    " salarycomponent_name," +
                                    " created_by," +
                                    " created_date," +
                                    " salarycomponent_percentage, " +
                                    " salarycomponent_amount," +
                                    " affect_in, " +
                                    " salarycomponent_percentage_employer, " +
                                    " salarycomponent_amount_employer " +
                                    ")values(" +
                                   " '" + msGetsalrygradedtlGID + "'," +
                                   " '" + msgetsalarygradeGID + "'," +
                                   " '" + objMySqlDataReader["componentgroup_name"].ToString() + "'," +
                                   " '" + objMySqlDataReader["componentgroup_gid"].ToString() + "'," +
                                   " '" + objMySqlDataReader["component_type"].ToString() + "'," +
                                   " '" + lsdecom + "'," +
                                   " '" + objMySqlDataReader["component_name"].ToString() + "'," +
                                   " '" + user_gid + "'," +
                                   "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                   " '" + objMySqlDataReader["component_percentage"].ToString().Replace(" ","") + "'," +
                                   " '" + lscomponent_amount + "'," +
                                   " '" + objMySqlDataReader["affecting_in"].ToString() + "'," +
                                   " '" + objMySqlDataReader["component_percentage_employer"].ToString().Trim() + "'," +
                                   " '" + lscomponent_amountemployer + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                    }
                }


                for (int k = 0; k < values.component_name2.Count; k++)
                {
                    var lsotherscom = values.component_name2[k];
                    if (!string.IsNullOrEmpty(lsotherscom))
                    {

                        if (lsotherscom != "" && lsotherscom != null)
                        {
                            string msGetsalrygradedtlGID = objcmnfunctions.GetMasterGID("PSGD");

                            msSQL = " select componentgroup_name,componentgroup_gid,component_type,othercomponent_type,component_flag,component_name,component_percentage," +
                                    " component_amount,affecting_in,component_percentage_employer,component_amount_employer " +
                                    " from pay_mst_tsalarycomponent where salarycomponent_gid = '" + lsotherscom + "'";

                            objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                            if (objMySqlDataReader.HasRows == true)
                            {

                                if (objMySqlDataReader["component_flag"].ToString() == "Y")
                                {
                                    string affectingIn = objMySqlDataReader["affecting_in"].ToString();

                                    double componentPercentage = double.Parse(objMySqlDataReader["component_percentage"].ToString());

                                    if (affectingIn == "Basic")
                                    {
                                        lscomponent_amount = (componentPercentage / 100) * values.NewBasicSalary;
                                        lscomponent_amountemployer = (componentPercentage / 100) * values.NewBasicSalary;
                                    }
                                    else if (affectingIn == "Gross")
                                    {
                                        lscomponent_amount = (componentPercentage / 100) * values.gross_salary;
                                        lscomponent_amountemployer = (componentPercentage / 100) * values.gross_salary;
                                    }
                                }

                                else
                                {
                                    lscomponent_amount = double.Parse(objMySqlDataReader["component_amount"].ToString());
                                    lscomponent_amountemployer = double.Parse(objMySqlDataReader["component_amount_employer"].ToString());
                                }
                            }
                            string lsotherscomponenttype = objMySqlDataReader["othercomponent_type"].ToString();

                            msSQL = " insert into pay_mst_tsalarygradetemplatedtl (" +
                                    " salarygradetemplatedtl_gid," +
                                    " salarygradetemplate_gid," +
                                    " componentgroup_name," +
                                    " componentgroup_gid," +
                                    " salarygradetype," +
                                    " salarycomponent_gid, " +
                                    " othercomponent_type, " +
                                    " salarycomponent_name," +
                                    " created_by," +
                                    " created_date," +
                                    " salarycomponent_percentage, " +
                                    " salarycomponent_amount," +
                                    " affect_in, " +
                                    " salarycomponent_percentage_employer, " +
                                    " salarycomponent_amount_employer " +
                                    ")values(" +
                                   " '" + msGetsalrygradedtlGID + "'," +
                                   " '" + msgetsalarygradeGID + "'," +
                                   " '" + objMySqlDataReader["componentgroup_name"].ToString() + "'," +
                                   " '" + objMySqlDataReader["componentgroup_gid"].ToString() + "'," +
                                   " '" + objMySqlDataReader["component_type"].ToString() + "'," +
                                   " '" + lsotherscom + "'," +
                                   " '" + lsotherscomponenttype + "'," +
                                   " '" + objMySqlDataReader["component_name"].ToString() + "'," +
                                   " '" + user_gid + "'," +
                                   "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                   " '" + objMySqlDataReader["component_percentage"].ToString().Replace(" ", "") + "'," +
                                   " '" + lscomponent_amount + "'," +
                                   " '" + objMySqlDataReader["affecting_in"].ToString() + "'," +
                                   " '" + objMySqlDataReader["component_percentage_employer"].ToString().Trim() + "'," +
                                   " '" + lscomponent_amountemployer + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }
                
                if (mnResult1 != 0)
                {
                    values.status = true;
                    values.message = "Salary Grade Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Salary Grade";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Salary Grade!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetEditgrade(string salarygradetemplate_gid, MdlSalaryGrade values)
        {
            try
            {


                msSQL = " select salarygradetemplatedtl_gid, salarygradetemplate_gid, componentgroup_name, componentgroup_gid, salarygradetype,salarycomponent_name, " +
                            " salarycomponent_percentage, salarycomponent_amount, affect_in, othercomponent_type,salarycomponent_percentage_employer, " +
                            "  salarycomponent_amount_employer, salarycomponent_gid " +
                            " from pay_mst_tsalarygradetemplatedtl where salarygradetemplate_gid='" + salarygradetemplate_gid + "' " +
                            " and salarygradetype='Addition' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editaditional>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Editaditional
                        {
                            salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarygradetype = dt["salarygradetype"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            employee_contribution = dt["salarycomponent_amount"].ToString(),
                            affecting_in = dt["affect_in"].ToString(),
                            othercomponent_type = dt["othercomponent_type"].ToString(),
                            salarycomponent_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                            employer_contribution = dt["salarycomponent_amount_employer"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            component_type = dt["salarygradetype"].ToString(),
                            component_percentage = dt["salarycomponent_percentage"].ToString(),
                            component_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                        });
                        values.Editaditional = getModuleList;
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

        public void Editdeduction(string salarygradetemplate_gid, MdlSalaryGrade values)
        {
            try
            {

                msSQL = " select salarygradetemplatedtl_gid, salarygradetemplate_gid, componentgroup_name, componentgroup_gid, salarygradetype,salarycomponent_name, " +
                        " salarycomponent_percentage, salarycomponent_amount, affect_in, othercomponent_type,salarycomponent_percentage_employer, " +
                        "  salarycomponent_amount_employer, salarycomponent_gid " +
                        " from pay_mst_tsalarygradetemplatedtl where salarygradetemplate_gid='" + salarygradetemplate_gid + "' " +
                        " and salarygradetype='Deduction' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editdeduction>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Editdeduction
                        {
                            salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarygradetype = dt["salarygradetype"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            demployee_contribution = dt["salarycomponent_amount"].ToString(),
                            affecting_in = dt["affect_in"].ToString(),
                            othercomponent_type = dt["othercomponent_type"].ToString(),
                            salarycomponent_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                            demployer_contribution = dt["salarycomponent_amount_employer"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            component_type = dt["salarygradetype"].ToString(),
                            component_percentage = dt["salarycomponent_percentage"].ToString(),
                            component_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                        });
                        values.Editdeduction = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading edit Deduction!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void Editothers(string salarygradetemplate_gid, MdlSalaryGrade values)
        {
            try
            {

                msSQL = " select salarygradetemplatedtl_gid, salarygradetemplate_gid, componentgroup_name, componentgroup_gid, salarygradetype,salarycomponent_name, " +
                        " salarycomponent_percentage, salarycomponent_amount, affect_in, othercomponent_type,salarycomponent_percentage_employer, " +
                        "  salarycomponent_amount_employer, salarycomponent_gid " +
                        " from pay_mst_tsalarygradetemplatedtl where salarygradetemplate_gid='" + salarygradetemplate_gid + "' " +
                        " and salarygradetype='Others' ";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Editothers>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Editothers
                        {
                            salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = dt["salarygradetemplate_gid"].ToString(),
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarygradetype = dt["salarygradetype"].ToString(),
                            salarycomponent_name = dt["salarycomponent_name"].ToString(),
                            salarycomponent_percentage = dt["salarycomponent_percentage"].ToString(),
                            demployee_contribution = dt["salarycomponent_amount"].ToString(),
                            affecting_in = dt["affect_in"].ToString(),
                            othercomponent_type = dt["othercomponent_type"].ToString(),
                            salarycomponent_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                            demployer_contribution = dt["salarycomponent_amount_employer"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            component_type = dt["salarygradetype"].ToString(),
                            component_percentage = dt["salarycomponent_percentage"].ToString(),
                            component_percentage_employer = dt["salarycomponent_percentage_employer"].ToString(),
                        });
                        values.Editothers = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading edit Deduction!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaDeleteSalaryGrade(string salarygradetemplate_gid, deletegradelist values)
        {
            try
            {
                msSQL = "  delete from pay_mst_tsalarygradetemplate where salarygradetemplate_gid='" + salarygradetemplate_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "  delete from pay_mst_tsalarygradetemplatedtl where salarygradetemplate_gid='" + salarygradetemplate_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Salary Grade Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Salary Grade";
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