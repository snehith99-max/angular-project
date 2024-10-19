using ems.payroll.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;

namespace ems.payroll.DataAccess
{
    public class DaPayMstGradeConfirm
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;

        OdbcDataReader objMySqlDataReader, objMySqlDataReader2;
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


        public void Daadditionsummarybind(string salarygradetemplategid, string gross_salary, MdlPayMstGradeConfirm values)
        {
            double additioncomponentamount = 0.00;
            double additioncmpamt = 0.00;
            double basicsalary = 0.00;
            double employeerbasicsalary = 0.00;
            string lscomponentname1 = "";
            double lscomponent_percentage = 0.00;
            double lscomponent_amount = 0.00;
            double lsemployer_percentage = 0.00;
            double lsemployer_amount = 0.00;
            string lscomponent_flag = "";
            string lsemployercomponent_flag1 = "";
            string lsaffect_in = "";
            string lscomponentgroup_name = "";
            string lssalarycomponent_name = "";
            string lsemployercomponent_flag = "";
            Double lsgross_salary = Convert.ToDouble(gross_salary);
            Double lslocal = 0.00;
            Double lslocal_employer = 0.00;

            string lscomponentname1_cust = "";
            double lscomponent_percentage_cust = 0;
            double lscomponent_amount_cust = 0;
            double lsemployer_percentage_coust = 0;
            string lscomponent_flag_cust = "";
            string lsemployercomponent_flag1_cust = "";
            double lsemployer_amount_cust = 0;

            string lscomponentname1_splf = "";
            double lscomponent_percentage_splf = 0;
            double lscomponent_amount_splf = 0;
            double lsemployer_percentage_splf = 0;
            string lscomponent_flag_splf = "";
            string lsemployercomponent_flag1_splf = "";
            double lsemployer_amount_custsplf = 0;

            try
            {
                msSQL = " select salarycomponent_gid, componentgroup_gid ,componentgroup_name,salarycomponent_name,salarycomponent_percentage,affect_in,salarycomponent_amount, " +
                        " salarycomponent_percentage_employer, salarycomponent_amount_employer from pay_mst_tsalarygradetemplatedtl " +
                        " where salarygradetemplate_gid='" + salarygradetemplategid + "' and salarygradetype='Addition' and componentgroup_gid not in(select componentgroup_gid from pay_tmp_tsalarygradetemplatedtl where salarygradetemplate_gid='" + salarygradetemplategid + "')";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList12 = new List<Addsummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        string msGetGID = objcmnfunctions.GetMasterGID("PTMP");

                        msSQL = " insert into pay_tmp_tsalarygradetemplatedtl (" +
                        " salarygradetmpdtl_gid," +
                        " salarygradetemplate_gid," +
                        " componentgroup_name," +
                        " componentgroup_gid," +
                        " salarygradetype," +
                        " salarycomponent_gid, " +
                        " salarycomponent_name," +
                        " created_date," +
                        " salarycomponent_percentage, " +
                        " salarycomponent_amount," +
                        " affect_in, " +
                        " salarycomponent_percentage_employer, " +
                        " salarycomponent_amount_employer " +
                        ")values(" +
                       " '" + msGetGID + "'," +
                       " '" + salarygradetemplategid + "'," +
                       " '" + dt["componentgroup_name"].ToString() + "'," +
                       " '" + dt["componentgroup_gid"] + "'," +
                       " 'Addition'," +
                       " '" + dt["salarycomponent_gid"] + "'," +
                       " '" + dt["salarycomponent_name"] + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       " '" + dt["salarycomponent_percentage"] + "'," +
                       " '" + dt["salarycomponent_amount"] + "'," +
                       " '" + dt["affect_in"] + "'," +
                       " '" + dt["salarycomponent_percentage_employer"] + "'," +
                       " '" + dt["salarycomponent_amount_employer"] + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    }
                }


                msSQL = " select a.affecting_in,a.component_flag,a.componentgroup_gid,a.componentgroup_name,a.component_percentage,a.component_amount,a.component_flag_employer,a.component_name," +
                " a.component_percentage_employer,a.component_amount_employer,b.group_belongsto from pay_mst_tsalarycomponent a" +
                " left join pay_mst_tcomponentgroupmaster b on a.componentgroup_gid= b.componentgroup_gid " +
                " left join pay_mst_tsalarygradetemplatedtl c on c.salarycomponent_gid= a.salarycomponent_gid " +
                " where a.component_name like '%basic%' and b.group_belongsto='Gross' and c.salarygradetemplate_gid = '" + salarygradetemplategid + "'";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    lscomponentname1 = objMySqlDataReader["component_name"].ToString();
                    lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                    lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                    lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                    lsemployercomponent_flag1 = objMySqlDataReader["component_flag_employer"].ToString();
                    lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                    lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                }

                if (lscomponent_flag == "Y")
                {
                    lsbasicsalary = (lsgross_salary) * (lscomponent_percentage / 100);
                }
                else
                {
                    lsbasicsalary = lscomponent_amount;
                }

                if (lsemployercomponent_flag1 == "Y")
                {
                    lsbasic_salary_employeer = (lsgross_salary) * (lsemployer_percentage / 100);
                }
                else
                {
                    lsbasic_salary_employeer = lsemployer_amount;
                }



                msSQL = " select salarygradetmpdtl_gid,salarycomponent_gid, componentgroup_gid ,componentgroup_name,salarycomponent_gid from pay_tmp_tsalarygradetemplatedtl " +
                        " where salarygradetemplate_gid='" + salarygradetemplategid + "' and salarygradetype='Addition'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList1 = new List<Addsummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                        " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows == true)
                        {
                            lsaffect_in = objMySqlDataReader["affecting_in"].ToString();
                            lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                            lscomponentgroup_name = objMySqlDataReader["componentgroup_name"].ToString();
                            lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                            lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                            lssalarycomponent_name = objMySqlDataReader["component_name"].ToString();
                            lsemployercomponent_flag = objMySqlDataReader["component_flag_employer"].ToString();
                            lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                            lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                        }


                        if (lsaffect_in == "Basic")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsbasicsalary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsbasicsalary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (lsaffect_in == "Gross")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (lsaffect_in == "Customize")
                        {
                            string lscustomize_component = "";
                            double lscustomize_employee = 0;
                            double lscustomize_employer = 0;
                            string[] lscustomizelist = null;
                            msSQL = "select customize_component from pay_mst_tsalarycomponent " +
                                   " where salarycomponent_gid = '" + dt["salarycomponent_gid"].ToString() + "'";
                            objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader2.HasRows == true)
                            {
                                lscustomize_component = objMySqlDataReader2["customize_component"].ToString();
                                lscustomizelist = lscustomize_component.Split(',');
                            }
                            for (int j = 0; j < lscustomizelist.Length; j++)
                            {
                                string lsaffect_in_cust = "", lssalarycomponent_name_cust = "", lsemployercomponent_flag_cust = "";
                                double lsemployer_percentage_cust = 0;
                                msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                                         " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + lscustomizelist[j].ToString() + "'";

                                objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                                if (objMySqlDataReader2.HasRows == true)
                                {
                                    lsaffect_in_cust = objMySqlDataReader2["affecting_in"].ToString();
                                    lscomponent_flag_cust = objMySqlDataReader2["component_flag"].ToString();
                                    //lscomponentgroup_name = objMySqlDataReader2["componentgroup_name"].ToString();
                                    lscomponent_percentage_cust = Convert.ToDouble(objMySqlDataReader2["component_percentage"].ToString());
                                    lscomponent_amount_cust = Convert.ToDouble(objMySqlDataReader2["component_amount"].ToString());
                                    lssalarycomponent_name_cust = objMySqlDataReader2["component_name"].ToString();
                                    lsemployercomponent_flag_cust = objMySqlDataReader2["component_flag_employer"].ToString();
                                    lsemployer_percentage_cust = Convert.ToDouble(objMySqlDataReader2["component_percentage_employer"].ToString());
                                    lsemployer_amount_cust = Convert.ToDouble(objMySqlDataReader2["component_amount_employer"].ToString());
                                }


                                if (lsaffect_in_cust == "Basic")
                                {
                                    if (lscomponent_flag_cust == "Y")
                                    {
                                        lslocal = (lsbasicsalary) * (lscomponent_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_cust;
                                    }

                                    if (lsemployercomponent_flag_cust == "Y")
                                    {
                                        lslocal_employer = (lsbasicsalary) * (lsemployer_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_cust;
                                    }

                                }
                                if (lsaffect_in_cust == "Gross")
                                {
                                    if (lscomponent_flag_cust == "Y")
                                    {
                                        lslocal = (lsgross_salary) * (lscomponent_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_cust;
                                    }

                                    if (lsemployercomponent_flag_cust == "Y")
                                    {
                                        lslocal_employer = (lsgross_salary) * (lsemployer_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_cust;
                                    }

                                }
                                lscustomize_employee += lslocal;
                                lscustomize_employer += lsemployer_amount;

                            }

                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lscustomize_employee) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lscustomize_employer) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (lsaffect_in == "Formula")
                        {
                            string lssource_variale = "", lsformula_operator = "", lsformula_variable = "";
                            string[] lsvariableslist = null;
                            double lsformula_employee = 0;
                            double lsformula_employer = 0;
                            msSQL = "select source_variale,formula_operator,formula_variable from pay_mst_tsalarycomponent" +
                                    " where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";
                            objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader2.HasRows == true)
                            {
                                lssource_variale = objMySqlDataReader2["source_variale"].ToString();
                                lsformula_operator = objMySqlDataReader2["formula_operator"].ToString();
                                lsformula_variable = objMySqlDataReader2["formula_variable"].ToString();
                            }
                            lsvariableslist = lsformula_variable.Split(',');
                            for (int l = 0; l < lsvariableslist.Length; l++)
                            {
                                string lsaffect_in_splf = "", lssalarycomponent_name_splf = "", lsemployercomponent_flag_splf = "";
                                double lsemployer_amount_splf = 0;
                                msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                                         " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + lsvariableslist[l].ToString() + "'";

                                objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                                if (objMySqlDataReader2.HasRows == true)
                                {
                                    lsaffect_in_splf = objMySqlDataReader2["affecting_in"].ToString();
                                    lscomponent_flag_splf = objMySqlDataReader2["component_flag"].ToString();
                                    //lscomponentgroup_name = objMySqlDataReader2["componentgroup_name"].ToString();
                                    lscomponent_percentage_splf = Convert.ToDouble(objMySqlDataReader2["component_percentage"].ToString());
                                    lscomponent_amount_splf = Convert.ToDouble(objMySqlDataReader2["component_amount"].ToString());
                                    lssalarycomponent_name_splf = objMySqlDataReader2["component_name"].ToString();
                                    lsemployercomponent_flag_splf = objMySqlDataReader2["component_flag_employer"].ToString();
                                    lsemployer_percentage_splf = Convert.ToDouble(objMySqlDataReader2["component_percentage_employer"].ToString());
                                    lsemployer_amount_splf = Convert.ToDouble(objMySqlDataReader2["component_amount_employer"].ToString());
                                }


                                if (lsaffect_in_splf == "Basic")
                                {
                                    if (lscomponent_flag_splf == "Y")
                                    {
                                        lslocal = (lsbasicsalary) * (lscomponent_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_splf;
                                    }

                                    if (lsemployercomponent_flag_splf == "Y")
                                    {
                                        lslocal_employer = (lsbasicsalary) * (lsemployer_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_splf;
                                    }

                                }
                                if (lsaffect_in_splf == "Gross")
                                {
                                    if (lscomponent_flag_splf == "Y")
                                    {
                                        lslocal = (lsgross_salary) * (lscomponent_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_splf;
                                    }

                                    if (lsemployercomponent_flag_splf == "Y")
                                    {
                                        lslocal_employer = (lsgross_salary) * (lsemployer_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_splf;
                                    }

                                }
                                lsformula_employee +=  lslocal;
                                lsformula_employer +=  lslocal_employer;

                            }

                            if (lssource_variale == "Gross")
                            {
                                if (lsformula_operator == "add")
                                {
                                    lsformula_employee = lsgross_salary + lsformula_employee;
                                }
                                else
                                {
                                    lsformula_employee = lsgross_salary - lsformula_employee;
                                }
                            }
                            else if (lssource_variale == "Basic")
                            {
                                if (lsformula_operator == "add")
                                {
                                    lsformula_employee = lsbasicsalary + lsformula_employee;
                                }
                                else
                                {
                                    lsformula_employee = lsbasicsalary - lsformula_employee;
                                }
                            }
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsformula_employee) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lsformula_employee;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsformula_employer) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsformula_employer;
                            }
                        }



                        getModuleList1.Add(new Addsummarybind_list
                        {
                            //salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = salarygradetemplategid,
                            salarygradetmpdtl_gid = dt["salarygradetmpdtl_gid"].ToString(),
                            componentgroup_name = lscomponentgroup_name,
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = lssalarycomponent_name,
                            salarycomponent_percentage = lscomponent_percentage,
                            salarycomponent_amount = Math.Round(lslocal),
                            salarycomponent_percentage_employer = lsemployer_percentage,
                            salarycomponent_amount_employer = Math.Round(lslocal_employer),
                            basicsalay = lsbasicsalary



                        });
                        values.Addsummarybind_list = getModuleList1;
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


        public void DaDeductionsummarybind(string salarygradetemplategid, string gross_salary, MdlPayMstGradeConfirm values)
        {

            double employerdeductioncmpamt = 0.00;
            double lstotaladditionemployee = 0.00;
            double lstotaladditionemployer = 0.00;
            double deductioncmpamt = 0.00;
            double additioncomponentamount = 0.00;
            double additioncmpamt = 0.00;
            double basicsalary = 0.00;
            double netsalary = 0.00;
            double ctc = 0.00;
            double employeerbasicsalary = 0.00;
            string lscomponentname1 = "";
            double lscomponent_percentage = 0.00;
            double lscomponent_amount = 0.00;
            double lsemployer_percentage = 0.00;
            double lsemployer_amount = 0.00;
            string lscomponent_flag = "";
            string lsemployercomponent_flag1 = "";
            string lsaffect_in = "";
            string lscomponentgroup_name = "";
            string lssalarycomponent_name = "";
            string lsothercomponttype = "";
            string lsemployercomponent_flag = "";
            Double lsgross_salary = Convert.ToDouble(gross_salary);
            Double lslocal = 0.00;
            Double lslocal_employer = 0.00;

            string lscomponentname1_cust = "";
            double lscomponent_percentage_cust = 0;
            double lscomponent_amount_cust = 0;
            double lsemployer_percentage_coust = 0;
            string lscomponent_flag_cust = "";
            string lsemployercomponent_flag1_cust = "";
            double lsemployer_amount_cust = 0;
            double lscomponent_percentage_splf = 0;
            double lscomponent_amount_splf = 0;
            double lsemployer_percentage_splf = 0;
            string lscomponent_flag_splf = "";


            try
            {
                msSQL = "delete from pay_tmp_tsalarygradetemplatedtl  where salarygradetemplate_gid='" + salarygradetemplategid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msSQL = " select salarycomponent_gid, componentgroup_gid ,componentgroup_name,salarycomponent_name,salarycomponent_percentage,affect_in,salarycomponent_amount, " +
                       " salarycomponent_percentage_employer, salarycomponent_amount_employer from pay_mst_tsalarygradetemplatedtl " +
                       " where salarygradetemplate_gid='" + salarygradetemplategid + "' and " +
                       " salarygradetype='Addition' and componentgroup_gid not in  " +
                       " (select componentgroup_gid from pay_tmp_tsalarygradetemplatedtl where " +
                       " salarygradetemplate_gid='" + salarygradetemplategid + "') order by salarycomponent_name " ;

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList12 = new List<Addsummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {

                        string msGetGID = objcmnfunctions.GetMasterGID("PTMP");

                        msSQL = " insert into pay_tmp_tsalarygradetemplatedtl (" +
                        " salarygradetmpdtl_gid," +
                        " salarygradetemplate_gid," +
                        " componentgroup_name," +
                        " componentgroup_gid," +
                        " salarygradetype," +
                        " salarycomponent_gid, " +
                        " salarycomponent_name," +
                        " created_date," +
                        " salarycomponent_percentage, " +
                        " salarycomponent_amount," +
                        " affect_in, " +
                        " salarycomponent_percentage_employer, " +
                        " salarycomponent_amount_employer " +
                        ")values(" +
                       " '" + msGetGID + "'," +
                       " '" + salarygradetemplategid + "'," +
                       " '" + dt["componentgroup_name"].ToString() + "'," +
                       " '" + dt["componentgroup_gid"] + "'," +
                       " 'Addition'," +
                       " '" + dt["salarycomponent_gid"] + "'," +
                       " '" + dt["salarycomponent_name"] + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       " '" + dt["salarycomponent_percentage"] + "'," +
                       " '" + dt["salarycomponent_amount"] + "'," +
                       " '" + dt["affect_in"] + "'," +
                       " '" + dt["salarycomponent_percentage_employer"] + "'," +
                       " '" + dt["salarycomponent_amount_employer"] + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    }
                }


                msSQL = " select a.affecting_in,a.component_flag,a.componentgroup_gid,a.componentgroup_name,a.component_percentage,a.component_amount,a.component_flag_employer,a.component_name," +
                " a.component_percentage_employer,a.component_amount_employer,b.group_belongsto from pay_mst_tsalarycomponent a" +
                " left join pay_mst_tcomponentgroupmaster b on a.componentgroup_gid= b.componentgroup_gid " +
                " left join pay_mst_tsalarygradetemplatedtl c on c.salarycomponent_gid= a.salarycomponent_gid " +
                " where a.component_name like '%basic%' and b.group_belongsto='Gross' and c.salarygradetemplate_gid = '" + salarygradetemplategid + "'";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    lscomponentname1 = objMySqlDataReader["component_name"].ToString();
                    lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                    lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                    lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                    lsemployercomponent_flag1 = objMySqlDataReader["component_flag_employer"].ToString();
                    lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                    lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                }

                if (lscomponent_flag == "Y")
                {
                    lsbasicsalary = (lsgross_salary) * (lscomponent_percentage / 100);
                }
                else
                {
                    lsbasicsalary = lscomponent_amount;
                }

                if (lsemployercomponent_flag1 == "Y")
                {
                    lsbasic_salary_employeer = (lsgross_salary) * (lsemployer_percentage / 100);
                }
                else
                {
                    lsbasic_salary_employeer = lsemployer_amount;
                }



                msSQL = " select salarygradetmpdtl_gid,salarycomponent_gid, componentgroup_gid ,componentgroup_name,salarycomponent_gid from pay_tmp_tsalarygradetemplatedtl " +
                        " where salarygradetemplate_gid='" + salarygradetemplategid + "' and salarygradetype='Addition' order by salarycomponent_name ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList1 = new List<Addsummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                        " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows == true)
                        {
                            lsaffect_in = objMySqlDataReader["affecting_in"].ToString();
                            lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                            lscomponentgroup_name = objMySqlDataReader["componentgroup_name"].ToString();
                            lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                            lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                            lssalarycomponent_name = objMySqlDataReader["component_name"].ToString();
                            lsemployercomponent_flag = objMySqlDataReader["component_flag_employer"].ToString();
                            lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                            lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                        }


                        if (lsaffect_in == "Basic")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsbasicsalary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsbasicsalary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (lsaffect_in == "Gross")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (lsaffect_in == "Customize")
                        {
                            string lscustomize_component = "";
                            double lscustomize_employee = 0;
                            double lscustomize_employer = 0;
                            string[] lscustomizelist = null;
                            msSQL = "select customize_component from pay_mst_tsalarycomponent " +
                                   " where salarycomponent_gid = '" + dt["salarycomponent_gid"].ToString() + "'";
                            objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader2.HasRows == true)
                            {
                                lscustomize_component = objMySqlDataReader2["customize_component"].ToString();
                                lscustomizelist = lscustomize_component.Split(',');
                            }
                            for (int j = 0; j < lscustomizelist.Length; j++)
                            {
                                string lsaffect_in_cust = "", lssalarycomponent_name_cust = "", lsemployercomponent_flag_cust = "";
                                double lsemployer_percentage_cust = 0;
                                msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                                         " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + lscustomizelist[j].ToString() + "'";

                                objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                                if (objMySqlDataReader2.HasRows == true)
                                {
                                    lsaffect_in_cust = objMySqlDataReader2["affecting_in"].ToString();
                                    lscomponent_flag_cust = objMySqlDataReader2["component_flag"].ToString();
                                    //lscomponentgroup_name = objMySqlDataReader2["componentgroup_name"].ToString();
                                    lscomponent_percentage_cust = Convert.ToDouble(objMySqlDataReader2["component_percentage"].ToString());
                                    lscomponent_amount_cust = Convert.ToDouble(objMySqlDataReader2["component_amount"].ToString());
                                    lssalarycomponent_name_cust = objMySqlDataReader2["component_name"].ToString();
                                    lsemployercomponent_flag_cust = objMySqlDataReader2["component_flag_employer"].ToString();
                                    lsemployer_percentage_cust = Convert.ToDouble(objMySqlDataReader2["component_percentage_employer"].ToString());
                                    lsemployer_amount_cust = Convert.ToDouble(objMySqlDataReader2["component_amount_employer"].ToString());
                                }


                                if (lsaffect_in_cust == "Basic")
                                {
                                    if (lscomponent_flag_cust == "Y")
                                    {
                                        lslocal = (lsbasicsalary) * (lscomponent_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_cust;
                                    }

                                    if (lsemployercomponent_flag_cust == "Y")
                                    {
                                        lslocal_employer = (lsbasicsalary) * (lsemployer_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_cust;
                                    }

                                }
                                if (lsaffect_in_cust == "Gross")
                                {
                                    if (lscomponent_flag_cust == "Y")
                                    {
                                        lslocal = (lsgross_salary) * (lscomponent_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_cust;
                                    }

                                    if (lsemployercomponent_flag_cust == "Y")
                                    {
                                        lslocal_employer = (lsgross_salary) * (lsemployer_percentage_cust / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_cust;
                                    }

                                }
                                lscustomize_employee += lslocal;
                                lscustomize_employer += lsemployer_amount;

                            }

                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lscustomize_employee) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lscustomize_employer) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (lsaffect_in == "Formula")
                        {
                            string lssource_variale = "", lsformula_operator = "", lsformula_variable = "";
                            string[] lsvariableslist = null;
                            double lsformula_employee = 0;
                            double lsformula_employer = 0;
                            msSQL = "select source_variale,formula_operator,formula_variable from pay_mst_tsalarycomponent" +
                                    " where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";
                            objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader2.HasRows == true)
                            {
                                lssource_variale = objMySqlDataReader2["source_variale"].ToString();
                                lsformula_operator = objMySqlDataReader2["formula_operator"].ToString();
                                lsformula_variable = objMySqlDataReader2["formula_variable"].ToString();
                            }
                            lsvariableslist = lsformula_variable.Split(',');
                            for (int l = 0; l < lsvariableslist.Length; l++)
                            {
                                string lsaffect_in_splf = "", lssalarycomponent_name_splf = "", lsemployercomponent_flag_splf = "";
                                double lsemployer_amount_splf = 0;
                                msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                                         " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + lsvariableslist[l].ToString() + "'";

                                objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                                if (objMySqlDataReader2.HasRows == true)
                                {
                                    lsaffect_in_splf = objMySqlDataReader2["affecting_in"].ToString();
                                    lscomponent_flag_splf = objMySqlDataReader2["component_flag"].ToString();
                                    //lscomponentgroup_name = objMySqlDataReader2["componentgroup_name"].ToString();
                                    lscomponent_percentage_splf = Convert.ToDouble(objMySqlDataReader2["component_percentage"].ToString());
                                    lscomponent_amount_splf = Convert.ToDouble(objMySqlDataReader2["component_amount"].ToString());
                                    lssalarycomponent_name_splf = objMySqlDataReader2["component_name"].ToString();
                                    lsemployercomponent_flag_splf = objMySqlDataReader2["component_flag_employer"].ToString();
                                    lsemployer_percentage_splf = Convert.ToDouble(objMySqlDataReader2["component_percentage_employer"].ToString());
                                    lsemployer_amount_splf = Convert.ToDouble(objMySqlDataReader2["component_amount_employer"].ToString());
                                }


                                if (lsaffect_in_splf == "Basic")
                                {
                                    if (lscomponent_flag_splf == "Y")
                                    {
                                        lslocal = (lsbasicsalary) * (lscomponent_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_splf;
                                    }

                                    if (lsemployercomponent_flag_splf == "Y")
                                    {
                                        lslocal_employer = (lsbasicsalary) * (lsemployer_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_splf;
                                    }

                                }
                                if (lsaffect_in_splf == "Gross")
                                {
                                    if (lscomponent_flag_splf == "Y")
                                    {
                                        lslocal = (lsgross_salary) * (lscomponent_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_splf;
                                    }

                                    if (lsemployercomponent_flag_splf == "Y")
                                    {
                                        lslocal_employer = (lsgross_salary) * (lsemployer_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_splf;
                                    }

                                }
                                if (lsaffect_in_splf == "Customize")
                                {
                                    string lscustomize_component = "";
                                    double lscustomize_employee = 0;
                                    double lscustomize_employer = 0;
                                    string[] lscustomizelist = null;
                                    msSQL = "select customize_component from pay_mst_tsalarycomponent " +
                                           " where salarycomponent_gid = '" + lsvariableslist[l].ToString() + "'";
                                    objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);
                                    if (objMySqlDataReader2.HasRows == true)
                                    {
                                        lscustomize_component = objMySqlDataReader2["customize_component"].ToString();
                                        lscustomizelist = lscustomize_component.Split(',');
                                    }
                                    for (int j = 0; j < lscustomizelist.Length; j++)
                                    {
                                        string lsaffect_in_cust = "", lssalarycomponent_name_cust = "", lsemployercomponent_flag_cust = "";
                                        double lsemployer_percentage_cust = 0;
                                        msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                                                 " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + lscustomizelist[j].ToString() + "'";

                                        objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                                        if (objMySqlDataReader2.HasRows == true)
                                        {
                                            lsaffect_in_cust = objMySqlDataReader2["affecting_in"].ToString();
                                            lscomponent_flag_cust = objMySqlDataReader2["component_flag"].ToString();
                                            //lscomponentgroup_name = objMySqlDataReader2["componentgroup_name"].ToString();
                                            lscomponent_percentage_cust = Convert.ToDouble(objMySqlDataReader2["component_percentage"].ToString());
                                            lscomponent_amount_cust = Convert.ToDouble(objMySqlDataReader2["component_amount"].ToString());
                                            lssalarycomponent_name_cust = objMySqlDataReader2["component_name"].ToString();
                                            lsemployercomponent_flag_cust = objMySqlDataReader2["component_flag_employer"].ToString();
                                            lsemployer_percentage_cust = Convert.ToDouble(objMySqlDataReader2["component_percentage_employer"].ToString());
                                            lsemployer_amount_cust = Convert.ToDouble(objMySqlDataReader2["component_amount_employer"].ToString());
                                        }


                                        if (lsaffect_in_cust == "Basic")
                                        {
                                            if (lscomponent_flag_cust == "Y")
                                            {
                                                lslocal = (lsbasicsalary) * (lscomponent_percentage_cust / 100);
                                            }
                                            else
                                            {
                                                lslocal = lscomponent_amount_cust;
                                            }

                                            if (lsemployercomponent_flag_cust == "Y")
                                            {
                                                lslocal_employer = (lsbasicsalary) * (lsemployer_percentage_cust / 100);
                                            }
                                            else
                                            {
                                                lslocal_employer = lsemployer_amount_cust;
                                            }

                                        }
                                        if (lsaffect_in_cust == "Gross")
                                        {
                                            if (lscomponent_flag_cust == "Y")
                                            {
                                                lslocal = (lsgross_salary) * (lscomponent_percentage_cust / 100);
                                            }
                                            else
                                            {
                                                lslocal = lscomponent_amount_cust;
                                            }

                                            if (lsemployercomponent_flag_cust == "Y")
                                            {
                                                lslocal_employer = (lsgross_salary) * (lsemployer_percentage_cust / 100);
                                            }
                                            else
                                            {
                                                lslocal_employer = lsemployer_amount_cust;
                                            }

                                        }
                                        lscustomize_employee += lslocal;
                                        lscustomize_employer += lsemployer_amount;

                                    }

                                    if (lscomponent_flag_splf == "Y")
                                    {
                                        lslocal = (lscustomize_employee) * (lscomponent_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_splf;
                                    }

                                    if (lsemployercomponent_flag_splf == "Y")
                                    {
                                        lslocal_employer = (lscustomize_employer) * (lsemployer_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_splf;
                                    }

                                }
                                lsformula_employee += lslocal;
                                lsformula_employer += lslocal_employer;

                            }

                            if (lssource_variale == "Gross")
                            {
                                if (lsformula_operator == "add")
                                {
                                    lsformula_employee = lsgross_salary + lsformula_employee;
                                }
                                else
                                {
                                    lsformula_employee = lsgross_salary - lsformula_employee;
                                }
                            }
                            else if (lssource_variale == "Basic")
                            {
                                if (lsformula_operator == "add")
                                {
                                    lsformula_employee = lsbasicsalary + lsformula_employee;
                                }
                                else
                                {
                                    lsformula_employee = lsbasicsalary - lsformula_employee;
                                }
                            }
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsformula_employee) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lsformula_employee;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsformula_employer) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsformula_employer;
                            }
                        }

                        lstotaladditionemployee += lslocal;
                        lstotaladditionemployer += lslocal_employer;
                        msSQL = "UPDATE pay_tmp_tsalarygradetemplatedtl set salarycomponent_amount='" + lslocal + "'," +
                                " salarycomponent_amount_employer = '"+ lslocal_employer + "'" +
                                " where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'" +
                                " and salarygradetemplate_gid='" + salarygradetemplategid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        getModuleList1.Add(new Addsummarybind_list
                        {
                            //salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = salarygradetemplategid,
                            salarygradetmpdtl_gid = dt["salarygradetmpdtl_gid"].ToString(),
                            componentgroup_name = lscomponentgroup_name,
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = lssalarycomponent_name,
                            salarycomponent_percentage = lscomponent_percentage,
                            salarycomponent_amount = Math.Round(lslocal),
                            salarycomponent_percentage_employer = lsemployer_percentage,
                            salarycomponent_amount_employer = Math.Round(lslocal_employer),
                            basicsalay = lsbasicsalary



                        });
                        values.Addsummarybind_list = getModuleList1;
                    }
                }

                msSQL = " select salarycomponent_gid, componentgroup_gid ,componentgroup_name,salarycomponent_name,salarycomponent_percentage,affect_in,salarycomponent_amount, " +
                        " salarycomponent_percentage_employer, salarycomponent_amount_employer from pay_mst_tsalarygradetemplatedtl " +
                        " where salarygradetemplate_gid='" + salarygradetemplategid + "' and salarygradetype='Deduction'and componentgroup_gid not in(select componentgroup_gid from pay_tmp_tsalarygradetemplatedtl where salarygradetemplate_gid='" + salarygradetemplategid + "')";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<DedSummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string msGetGID = objcmnfunctions.GetMasterGID("PTMP");

                        msSQL = " insert into pay_tmp_tsalarygradetemplatedtl (" +
                        " salarygradetmpdtl_gid," +
                        " salarygradetemplate_gid," +
                        " componentgroup_name," +
                        " componentgroup_gid," +
                        " salarygradetype," +
                        " salarycomponent_gid, " +
                        " salarycomponent_name," +
                        " created_date," +
                        " salarycomponent_percentage, " +
                        " salarycomponent_amount," +
                        " affect_in, " +
                        " salarycomponent_percentage_employer, " +
                        " salarycomponent_amount_employer " +
                        ")values(" +
                       " '" + msGetGID + "'," +
                       " '" + salarygradetemplategid + "'," +
                       " '" + dt["componentgroup_name"].ToString() + "'," +
                       " '" + dt["componentgroup_gid"] + "'," +
                       " 'Deduction'," +
                       " '" + dt["salarycomponent_gid"] + "'," +
                       " '" + dt["salarycomponent_name"] + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       " '" + dt["salarycomponent_percentage"] + "'," +
                       " '" + dt["salarycomponent_amount"] + "'," +
                       " '" + dt["affect_in"] + "'," +
                       " '" + dt["salarycomponent_percentage_employer"] + "'," +
                       " '" + dt["salarycomponent_amount_employer"] + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    }
                }

                msSQL = " select salarygradetmpdtl_gid,salarycomponent_gid,componentgroup_gid,salarycomponent_name,salarycomponent_gid,componentgroup_name from pay_tmp_tsalarygradetemplatedtl " +
                        " where salarygradetemplate_gid='" + salarygradetemplategid + "' and salarygradetype='Deduction'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList2 = new List<DedSummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                                " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows == true)
                        {
                            lsaffect_in = objMySqlDataReader["affecting_in"].ToString();
                            lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                            lscomponentgroup_name = objMySqlDataReader["componentgroup_name"].ToString();
                            lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                            lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                            lssalarycomponent_name = objMySqlDataReader["component_name"].ToString();
                            lsemployercomponent_flag = objMySqlDataReader["component_flag_employer"].ToString();
                            lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                            lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());

                        }

                        //check here decduction


                        if (lsaffect_in == "Basic")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsbasicsalary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag == "Y")
                            {
                                lslocal_employer = (lsbasicsalary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }

                        if (lsaffect_in == "Gross")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag == "Y")
                            {
                                lslocal_employer = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }

                        if (lssalarycomponent_name.Contains("ESI"))
                        {


                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag == "Y")
                            {
                                lslocal_employer = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                            if (lsgross_salary >= 21000)
                            {
                                lslocal = 0;
                                lslocal_employer = 0;

                            }
                        }

                        if (lssalarycomponent_name.Contains("PF") || lssalarycomponent_name.Contains("EPF"))
                        {

                            if (lscomponent_flag == "Y")

                            {
                                if (lsbasicsalary >= 15000)
                                {
                                    lslocal = 1800;
                                }
                                else
                                {
                                    lslocal = (lsbasicsalary) * (lscomponent_percentage / 100);
                                }
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }


                            if (lsemployercomponent_flag == "Y")
                            {
                                if (lsbasicsalary >= 15000)
                                {
                                    lslocal_employer = 1800;
                                }
                                else
                                {
                                    lslocal_employer = (lsbasicsalary) * (lscomponent_percentage / 100);
                                }
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if(lsaffect_in == "Customize")
                        {
                            string lscustomize_component = "";
                            string lsamount = "";
                            double lscustomize_employee = 0;
                            double lscustomize_employer = 0;
                            string[] lscustomizelist = null;
                            msSQL = "select customize_component from pay_mst_tsalarycomponent " +
                                   " where salarycomponent_gid = '" + dt["salarycomponent_gid"].ToString() + "'";
                            objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader2.HasRows == true)
                            {
                                lscustomize_component = objMySqlDataReader2["customize_component"].ToString();
                                lscustomizelist = lscustomize_component.Split(',');

                                msSQL = "select sum(ifnull(salarycomponent_amount,0)) from pay_tmp_tsalarygradetemplatedtl " +
                                    " where salarycomponent_gid in('" + lscustomize_component.Replace(",", "','") + "') and  salarygradetemplate_gid='" + salarygradetemplategid + "'";
                                lsamount = objdbconn.GetExecuteScalar(msSQL);
                                lscustomize_employee = double.Parse(lsamount);
                                msSQL = "select sum(ifnull(salarycomponent_amount_employer,0)) from pay_tmp_tsalarygradetemplatedtl " +
                                    " where salarycomponent_gid in('" + lscustomize_component.Replace(",", "','") + "') and salarygradetemplate_gid='" + salarygradetemplategid + "'";
                                var lsamount1 = objdbconn.GetExecuteScalar(msSQL);
                                lscustomize_employer = double.Parse(lsamount1);

                                if (lscomponent_flag == "Y")
                                {
                                    lslocal = (lscustomize_employee) * (lscomponent_percentage / 100);
                                }
                                else
                                {
                                    lslocal = lscomponent_amount;
                                }

                                if (lsemployercomponent_flag == "Y")
                                {
                                    lslocal_employer = (lscustomize_employer) * (lsemployer_percentage / 100);
                                }
                                else
                                {
                                    lslocal_employer = lsemployer_amount;
                                }


                                if (lssalarycomponent_name.Contains("PF") || lssalarycomponent_name.Contains("EPF"))
                                {

                                    if (lscomponent_flag == "Y")

                                    {
                                        if (lscustomize_employee >= 15000)
                                        {
                                            lslocal = 1800;
                                        }
                                        else
                                        {
                                            lslocal = (lscustomize_employee) * (lscomponent_percentage / 100);
                                        }
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount;
                                    }


                                    if (lsemployercomponent_flag == "Y")
                                    {
                                        if (lscustomize_employer >= 15000)
                                        {
                                            lslocal_employer = 1800;
                                        }
                                        else
                                        {
                                            lslocal_employer = (lscustomize_employee) * (lscomponent_percentage / 100);
                                        }
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount;
                                    }

                                }

                            }
                        }

                        if (lsaffect_in == "Formula")
                        {
                            string lssource_variale = "", lsformula_operator = "", lsformula_variable = "";
                            string[] lsvariableslist = null;
                            double lsformula_employee = 0;
                            double lsformula_employer = 0;
                            msSQL = "select source_variale,formula_operator,formula_variable from pay_mst_tsalarycomponent" +
                                    " where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";
                            objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);
                            if (objMySqlDataReader2.HasRows == true)
                            {
                                lssource_variale = objMySqlDataReader2["source_variale"].ToString();
                                lsformula_operator = objMySqlDataReader2["formula_operator"].ToString();
                                lsformula_variable = objMySqlDataReader2["formula_variable"].ToString();
                            }
                            lsvariableslist = lsformula_variable.Split(',');
                            for (int l = 0; l < lsvariableslist.Length; l++)
                            {
                                string lsaffect_in_splf = "", lssalarycomponent_name_splf = "", lsemployercomponent_flag_splf = "";
                                double lsemployer_amount_splf = 0;
                                msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                                         " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + lsvariableslist[l].ToString() + "'";

                                objMySqlDataReader2 = objdbconn.GetDataReader(msSQL);

                                if (objMySqlDataReader2.HasRows == true)
                                {
                                    lsaffect_in_splf = objMySqlDataReader2["affecting_in"].ToString();
                                    lscomponent_flag_splf = objMySqlDataReader2["component_flag"].ToString();
                                    //lscomponentgroup_name = objMySqlDataReader2["componentgroup_name"].ToString();
                                    lscomponent_percentage_splf = Convert.ToDouble(objMySqlDataReader2["component_percentage"].ToString());
                                    lscomponent_amount_splf = Convert.ToDouble(objMySqlDataReader2["component_amount"].ToString());
                                    lssalarycomponent_name_splf = objMySqlDataReader2["component_name"].ToString();
                                    lsemployercomponent_flag_splf = objMySqlDataReader2["component_flag_employer"].ToString();
                                    lsemployer_percentage_splf = Convert.ToDouble(objMySqlDataReader2["component_percentage_employer"].ToString());
                                    lsemployer_amount_splf = Convert.ToDouble(objMySqlDataReader2["component_amount_employer"].ToString());
                                }


                                if (lsaffect_in_splf == "Basic")
                                {
                                    if (lscomponent_flag_splf == "Y")
                                    {
                                        lslocal = (lsbasicsalary) * (lscomponent_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_splf;
                                    }

                                    if (lsemployercomponent_flag_splf == "Y")
                                    {
                                        lslocal_employer = (lsbasicsalary) * (lsemployer_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_splf;
                                    }

                                }
                                if (lsaffect_in_splf == "Gross")
                                {
                                    if (lscomponent_flag_splf == "Y")
                                    {
                                        lslocal = (lsgross_salary) * (lscomponent_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal = lscomponent_amount_splf;
                                    }

                                    if (lsemployercomponent_flag_splf == "Y")
                                    {
                                        lslocal_employer = (lsgross_salary) * (lsemployer_percentage_splf / 100);
                                    }
                                    else
                                    {
                                        lslocal_employer = lsemployer_amount_splf;
                                    }

                                }
                                lsformula_employee += lslocal;
                                lsformula_employer +=  lslocal_employer;

                            }

                            if (lssource_variale == "Gross")
                            {
                                if (lsformula_operator == "add")
                                {
                                    lsformula_employee = lsgross_salary + lsformula_employee;
                                }
                                else
                                {
                                    lsformula_employee = lsgross_salary - lsformula_employee;
                                }
                            }
                            else if (lssource_variale == "Basic")
                            {
                                if (lsformula_operator == "add")
                                {
                                    lsformula_employee = lsbasicsalary + lsformula_employee;
                                }
                                else
                                {
                                    lsformula_employee = lsbasicsalary - lsformula_employee;
                                }
                            }
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsformula_employee) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lsformula_employee;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsformula_employer) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsformula_employer;
                            }
                        }

                        deductioncmpamt += lslocal;
                        employerdeductioncmpamt += lslocal_employer;

                        values.netsalary = Math.Round(lstotaladditionemployee - (deductioncmpamt));
                        values.ctc = Math.Round(lsgross_salary + employerdeductioncmpamt);

                        getModuleList2.Add(new DedSummarybind_list
                        {
                            //salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = salarygradetemplategid,
                            salarygradetmpdtl_gid = dt["salarygradetmpdtl_gid"].ToString(),
                            componentgroup_name = lscomponentgroup_name,
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = lssalarycomponent_name,
                            salarycomponent_percentage = lscomponent_percentage,
                            salarycomponent_amount = Math.Round(lslocal),
                            salarycomponent_percentage_employer = lsemployer_percentage,
                            salarycomponent_amount_employer = Math.Round(lslocal_employer),
                            net_salary = Math.Round(netsalary),
                            ctc = Math.Round(ctc),


                        });
                        values.DedSummarybind_list = getModuleList2;

                    }

                }
                dt_datatable.Dispose();
                msSQL = " select salarycomponent_gid,othercomponent_type, componentgroup_gid ,componentgroup_name,salarycomponent_name,salarycomponent_percentage,affect_in,salarycomponent_amount, " +
                     " salarycomponent_percentage_employer, salarycomponent_amount_employer from pay_mst_tsalarygradetemplatedtl " +
                     " where salarygradetemplate_gid='" + salarygradetemplategid + "' and salarygradetype='others'and componentgroup_gid not in(select componentgroup_gid from pay_tmp_tsalarygradetemplatedtl where salarygradetemplate_gid='" + salarygradetemplategid + "')";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList23 = new List<OthersSummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string msGetGID = objcmnfunctions.GetMasterGID("PTMP");

                        msSQL = " insert into pay_tmp_tsalarygradetemplatedtl (" +
                        " salarygradetmpdtl_gid," +
                        " salarygradetemplate_gid," +
                        " componentgroup_name," +
                        " componentgroup_gid," +
                        " salarygradetype," +
                        " salarycomponent_gid, " +
                        " othercomponent_type, " +
                        " salarycomponent_name," +
                        " created_date," +
                        " salarycomponent_percentage, " +
                        " salarycomponent_amount," +
                        " affect_in, " +
                        " salarycomponent_percentage_employer, " +
                        " salarycomponent_amount_employer " +
                        ")values(" +
                       " '" + msGetGID + "'," +
                       " '" + salarygradetemplategid + "'," +
                       " '" + dt["componentgroup_name"].ToString() + "'," +
                       " '" + dt["componentgroup_gid"] + "'," +
                       " 'Others'," +
                       " '" + dt["salarycomponent_gid"] + "'," +
                       " '" + dt["othercomponent_type"] + "'," +
                       " '" + dt["salarycomponent_name"] + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       " '" + dt["salarycomponent_percentage"] + "'," +
                       " '" + dt["salarycomponent_amount"] + "'," +
                       " '" + dt["affect_in"] + "'," +
                       " '" + dt["salarycomponent_percentage_employer"] + "'," +
                       " '" + dt["salarycomponent_amount_employer"] + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    }
                }
                msSQL = " select a.affecting_in,a.component_flag,a.componentgroup_gid,a.componentgroup_name,a.component_percentage,a.component_amount,a.component_flag_employer,a.component_name," +
                " a.component_percentage_employer,a.component_amount_employer,b.group_belongsto from pay_mst_tsalarycomponent a" +
                " left join pay_mst_tcomponentgroupmaster b on a.componentgroup_gid= b.componentgroup_gid " +
                " left join pay_mst_tsalarygradetemplatedtl c on c.salarycomponent_gid= a.salarycomponent_gid " +
                " where a.component_name like '%basic%' and b.group_belongsto='Gross' and c.salarygradetemplate_gid = '" + salarygradetemplategid + "'";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    lscomponentname1 = objMySqlDataReader["component_name"].ToString();
                    lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                    lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                    lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                    lsemployercomponent_flag1 = objMySqlDataReader["component_flag_employer"].ToString();
                    lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                    lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                }


                if (lscomponent_flag == "Y")
                {
                    lsbasicsalary = (lsgross_salary) * (lscomponent_percentage / 100);
                }
                else
                {
                    lsbasicsalary = lscomponent_amount;
                }

                if (lsemployercomponent_flag1 == "Y")
                {
                    lsbasic_salary_employeer = (lsgross_salary) * (lsemployer_percentage / 100);
                }
                else
                {
                    lsbasic_salary_employeer = lsemployer_amount;
                }



                msSQL = " select salarycomponent_gid,salarygradetmpdtl_gid,componentgroup_gid ,componentgroup_name,salarycomponent_gid,othercomponent_type from pay_tmp_tsalarygradetemplatedtl " +
                        " where salarygradetemplate_gid='" + salarygradetemplategid + "' and salarygradetype='Others'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList3 = new List<OthersSummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,othercomponent_type,component_flag_employer,component_name," +
                        " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows == true)
                        {
                            lsaffect_in = objMySqlDataReader["affecting_in"].ToString();
                            lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                            lscomponentgroup_name = objMySqlDataReader["componentgroup_name"].ToString();
                            lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                            lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                            lssalarycomponent_name = objMySqlDataReader["component_name"].ToString();
                            lsothercomponttype = objMySqlDataReader["othercomponent_type"].ToString();
                            lsemployercomponent_flag = objMySqlDataReader["component_flag_employer"].ToString();
                            lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                            lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                        }


                        if (lssalarycomponent_name == "Basic salary")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal = lsemployer_amount;
                            }

                        }

                        if (lsaffect_in == "Basic")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsbasicsalary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsbasicsalary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (lsaffect_in == "Gross")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        //values.netsalary += (othercomponentadd + othercomponentemployeradd) - (othercomponentded + othercomponentemployerded);
                        //values.ctc += (othercomponentadd + othercomponentemployeradd) + (othercomponentded + othercomponentemployerded);
                        if (lsothercomponttype == "Addition")
                        {
                            values.overallnetsalary = values.netsalary + lslocal ;

                        }
                        else if (lsothercomponttype == "Deduction")
                        {
                            values.overallnetsalary = values.netsalary - (lslocal);
                        }
                        else
                        {
                            values.overallnetsalary = values.netsalary;
                        }


                        getModuleList3.Add(new OthersSummarybind_list
                        {
                            //salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = salarygradetemplategid,
                            salarygradetmpdtl_gid = dt["salarygradetmpdtl_gid"].ToString(),
                            componentgroup_name = lscomponentgroup_name,
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = lssalarycomponent_name,
                            salarycomponent_percentage = lscomponent_percentage,
                            salarycomponent_amount = Math.Round(lslocal, 2),
                            salarycomponent_percentage_employer = lsemployer_percentage,
                            salarycomponent_amount_employer = Math.Round(lslocal_employer, 2),
                            othercomponent_type = dt["othercomponent_type"].ToString(),
                            basicsalay = lsbasicsalary



                        });

                        values.OthersSummarybind_list = getModuleList3;

                    }

                }
                if (values.OthersSummarybind_list == null)
                {
                    values.overallnetsalary = values.netsalary;

                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Component name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaOtherssummarybind(string salarygradetemplategid, string gross_salary, MdlPayMstGradeConfirm values)
        {
            double additioncomponentamount = 0.00;
            double additioncmpamt = 0.00;
            double basicsalary = 0.00;
            double employeerbasicsalary = 0.00;
            double othercomponentadd = 0.00;
            double othercomponentemployeradd = 0.00;
            double othercomponentded = 0.00;
            double othercomponentemployerded = 0.00;
            string lscomponentname1 = "";
            double lscomponent_percentage = 0.00;
            double lscomponent_amount = 0.00;
            double lsemployer_percentage = 0.00;
            double lsemployer_amount = 0.00;
            string lscomponent_flag = "";
            string lsemployercomponent_flag1 = "";
            string lsaffect_in = "";
            string lscomponentgroup_name = "";
            string lssalarycomponent_name = "";
            string lsemployercomponent_flag = "";
            Double lsgross_salary = Convert.ToDouble(gross_salary);
            Double lslocal = 0.00;
            Double lslocal_employer = 0.00;



            try
            {
                msSQL = " select salarycomponent_gid, componentgroup_gid ,componentgroup_name,salarycomponent_name,salarycomponent_percentage,affect_in,salarycomponent_amount, " +
                       " salarycomponent_percentage_employer, salarycomponent_amount_employer from pay_mst_tsalarygradetemplatedtl " +
                       " where salarygradetemplate_gid='" + salarygradetemplategid + "' and salarygradetype='others'and componentgroup_gid not in(select componentgroup_gid from pay_tmp_tsalarygradetemplatedtl where salarygradetemplate_gid='" + salarygradetemplategid + "')";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<OthersSummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string msGetGID = objcmnfunctions.GetMasterGID("PTMP");

                        msSQL = " insert into pay_tmp_tsalarygradetemplatedtl (" +
                        " salarygradetmpdtl_gid," +
                        " salarygradetemplate_gid," +
                        " componentgroup_name," +
                        " componentgroup_gid," +
                        " salarygradetype," +
                        " salarycomponent_gid, " +
                        " salarycomponent_name," +
                        " created_date," +
                        " salarycomponent_percentage, " +
                        " salarycomponent_amount," +
                        " affect_in, " +
                        " salarycomponent_percentage_employer, " +
                        " salarycomponent_amount_employer " +
                        ")values(" +
                       " '" + msGetGID + "'," +
                       " '" + salarygradetemplategid + "'," +
                       " '" + dt["componentgroup_name"].ToString() + "'," +
                       " '" + dt["componentgroup_gid"] + "'," +
                       " 'Others'," +
                       " '" + dt["salarycomponent_gid"] + "'," +
                       " '" + dt["salarycomponent_name"] + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                       " '" + dt["salarycomponent_percentage"] + "'," +
                       " '" + dt["salarycomponent_amount"] + "'," +
                       " '" + dt["affect_in"] + "'," +
                       " '" + dt["salarycomponent_percentage_employer"] + "'," +
                       " '" + dt["salarycomponent_amount_employer"] + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    }
                }
                msSQL = " select a.affecting_in,a.component_flag,a.componentgroup_gid,a.componentgroup_name,a.component_percentage,a.component_amount,a.component_flag_employer,a.component_name," +
                " a.component_percentage_employer,a.component_amount_employer,b.group_belongsto from pay_mst_tsalarycomponent a" +
                " left join pay_mst_tcomponentgroupmaster b on a.componentgroup_gid= b.componentgroup_gid " +
                " left join pay_mst_tsalarygradetemplatedtl c on c.salarycomponent_gid= a.salarycomponent_gid " +
                " where a.component_name like '%basic%' and b.group_belongsto='Gross' and c.salarygradetemplate_gid = '" + salarygradetemplategid + "'";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    lscomponentname1 = objMySqlDataReader["component_name"].ToString();
                    lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                    lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                    lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                    lsemployercomponent_flag1 = objMySqlDataReader["component_flag_employer"].ToString();
                    lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                    lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                }


                if (lscomponent_flag == "Y")
                {
                    lsbasicsalary = (lsgross_salary) * (lscomponent_percentage / 100);
                }
                else
                {
                    lsbasicsalary = lscomponent_amount;
                }

                if (lsemployercomponent_flag1 == "Y")
                {
                    lsbasic_salary_employeer = (lsgross_salary) * (lsemployer_percentage / 100);
                }
                else
                {
                    lsbasic_salary_employeer = lsemployer_amount;
                }



                msSQL = " select salarycomponent_gid,salarygradetmpdtl_gid,componentgroup_gid ,componentgroup_name,salarycomponent_gid,othercomponent_type from pay_tmp_tsalarygradetemplatedtl " +
                        " where salarygradetemplate_gid='" + salarygradetemplategid + "' and salarygradetype='Others'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList3 = new List<OthersSummarybind_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select affecting_in,component_flag,componentgroup_name,component_percentage,component_amount,component_flag_employer,component_name," +
                        " component_percentage_employer,component_amount_employer from pay_mst_tsalarycomponent where salarycomponent_gid='" + dt["salarycomponent_gid"].ToString() + "'";

                        objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                        if (objMySqlDataReader.HasRows == true)
                        {
                            lsaffect_in = objMySqlDataReader["affecting_in"].ToString();
                            lscomponent_flag = objMySqlDataReader["component_flag"].ToString();
                            lscomponentgroup_name = objMySqlDataReader["componentgroup_name"].ToString();
                            lscomponent_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage"].ToString());
                            lscomponent_amount = Convert.ToDouble(objMySqlDataReader["component_amount"].ToString());
                            lssalarycomponent_name = objMySqlDataReader["component_name"].ToString();
                            lsemployercomponent_flag = objMySqlDataReader["component_flag_employer"].ToString();
                            lsemployer_percentage = Convert.ToDouble(objMySqlDataReader["component_percentage_employer"].ToString());
                            lsemployer_amount = Convert.ToDouble(objMySqlDataReader["component_amount_employer"].ToString());
                        }


                        if (lssalarycomponent_name == "Basic salary")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal = lsemployer_amount;
                            }

                        }

                        if (lsaffect_in == "Basic")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsbasicsalary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsbasicsalary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (lsaffect_in == "Gross")
                        {
                            if (lscomponent_flag == "Y")
                            {
                                lslocal = (lsgross_salary) * (lscomponent_percentage / 100);
                            }
                            else
                            {
                                lslocal = lscomponent_amount;
                            }

                            if (lsemployercomponent_flag1 == "Y")
                            {
                                lslocal_employer = (lsgross_salary) * (lsemployer_percentage / 100);
                            }
                            else
                            {
                                lslocal_employer = lsemployer_amount;
                            }

                        }
                        if (dt["othercomponent_type"].ToString() == "Addition")
                        {
                            othercomponentadd += lslocal;
                            othercomponentemployeradd += lslocal_employer;

                        }


                        else if (dt["othercomponent_type"].ToString() == "Deduction")
                        {

                            othercomponentded += lslocal;
                            othercomponentemployerded += lslocal_employer;
                        }



                        //values.netsalary += (othercomponentadd + othercomponentemployeradd) - (othercomponentded + othercomponentemployerded);
                        //values.ctc += (othercomponentadd + othercomponentemployeradd) + (othercomponentded + othercomponentemployerded);

                        //values.overallnetsalary += lslocal+values.netsalary;





                        getModuleList3.Add(new OthersSummarybind_list
                        {
                            //salarygradetemplatedtl_gid = dt["salarygradetemplatedtl_gid"].ToString(),
                            salarygradetemplate_gid = salarygradetemplategid,
                            salarygradetmpdtl_gid = dt["salarygradetmpdtl_gid"].ToString(),
                            componentgroup_name = lscomponentgroup_name,
                            componentgroup_gid = dt["componentgroup_gid"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            salarycomponent_name = lssalarycomponent_name,
                            salarycomponent_percentage = lscomponent_percentage,
                            salarycomponent_amount = Math.Round(lslocal, 2),
                            salarycomponent_percentage_employer = lsemployer_percentage,
                            salarycomponent_amount_employer = Math.Round(lslocal_employer, 2),
                            othercomponent_type = dt["othercomponent_type"].ToString(),
                            basicsalay = lsbasicsalary

                        });

                        values.OthersSummarybind_list = getModuleList3;

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


        public void DagetDeleteComponent(string salarygradetmpdtl_gid, string salarygradetemplate_gid, MdlPayMstGradeConfirm values)
        {
            try
            {

                msSQL = "  delete from pay_tmp_tsalarygradetemplatedtl where salarygradetmpdtl_gid='" + salarygradetmpdtl_gid + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from pay_mst_tsalarygradetemplatedtl where salarygradetemplate_gid= '" + salarygradetemplate_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Component Deleted Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Component";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting component!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetcomponentlistdropdown(MdlPayMstGradeConfirm values)
        {
            try
            {

                msSQL = " select salarycomponent_gid,component_name  " +
                        " from pay_mst_tsalarycomponent ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<componentlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new componentlist
                        {
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString(),
                            component_name = dt["component_name"].ToString(),
                        });
                        values.componentlist = getModuleList;
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

        public void DaUpdateempgrade(string user_gid, Updateempgradelist values)
        {

            msSQL = "select salarycomponent_gid from pay_mst_tsalarycomponent where component_name = '" + values.editcomponent_name + "'";
            string lssalaryccomponent_gid = objdbconn.GetExecuteScalar(msSQL);

            msSQL = " update  pay_tmp_tsalarygradetemplatedtl set " +
                    " componentgroup_name = '" + values.editcomponentgroup_name + "'," +
                    " salarycomponent_name = '" + values.editcomponent_name + "'," +
                    " salarycomponent_gid = '" + lssalaryccomponent_gid + "'," +
                    " updated_by = '" + user_gid + "'," +
                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    "' where salarygradetmpdtl_gid='" + values.salarygradetmpdtl_gid + "'  ";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult == 1)
            {
                values.status = true;
                values.message = "Employee Grade Updated Successfully !!";
            }
            else
            {
                values.status = false;
                values.message = "Error While Updating Employee Grade !!";
            }
        }


        public void DaGetcomponenttypedropdown(MdlPayMstGradeConfirm values)
        {
            try
            {

                msSQL = " select distinct component_type from pay_mst_tsalarycomponent ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<componenttypelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new componenttypelist
                        {
                            component_type = dt["component_type"].ToString()
                        });
                        values.componenttypelist = getModuleList;
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
        public void DaGetOnChangeComponentgroup(MdlPayMstGradeConfirm values, string component_type)
        {
            try
            {

                msSQL = " select distinct componentgroup_name ,componentgroup_gid from pay_mst_tsalarycomponent " +
                          " where component_type='" + component_type + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<componentgrouplist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new componentgrouplist
                        {
                            componentgroup_name = dt["componentgroup_name"].ToString(),
                            componentgroup_gid = dt["componentgroup_gid"].ToString()
                        });
                        values.componentgrouplist = getModuleList;
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

        public void DaGetOnChangeComponent(MdlPayMstGradeConfirm values, string componentgroup_name)
        {
            try
            {

                msSQL = " select distinct component_name ,salarycomponent_gid from pay_mst_tsalarycomponent " +
                        " where componentgroup_name='" + componentgroup_name + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<componentnamelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new componentnamelist
                        {
                            component_name = dt["component_name"].ToString(),
                            salarycomponent_gid = dt["salarycomponent_gid"].ToString()
                        });
                        values.componentnamelist = getModuleList;
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


        public void DaGetOnChangeComponentamount(MdlPayMstGradeConfirm values, string component_name)
        {
            try
            {

                msSQL = " select distinct component_percentage,component_percentage_employer,component_amount,component_amount_employer from pay_mst_tsalarycomponent " +
                        " where component_name='" + component_name + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<componentamountlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new componentamountlist
                        {
                            component_percentage = dt["component_percentage"].ToString(),
                            component_percentage_employer = dt["component_percentage_employer"].ToString(),
                            component_amount = dt["component_amount"].ToString(),
                            component_amount_employer = dt["component_amount_employer"].ToString(),
                        });
                        values.componentamountlist = getModuleList;
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

        public void DaPostemployee2grade(string user_gid, employee2gradelist values)
        {

            try
            {
                foreach (var dt in values.employee_lists)
                {
                    if (values.BasicSalary == null || values.BasicSalary == "")
                    {
                        values.BasicSalary = "0";
                    }
                    string msgetsalarygradeGID = objcmnfunctions.GetMasterGID("PSAM");
                    msSQL = " insert into pay_trn_temployee2salarygradetemplate(" +
                            " employee2salarygradetemplate_gid ," +
                            " salarygradetemplate_gid," +
                            " employee_gid ," +
                            " salary_mode ," +
                            " user_gid ," +
                            " created_by," +
                            " created_date," +
                            " basic_Salary," +
                            " gross_salary," +
                            " ctc, " +
                            " net_salary" +
                            " )values(" +
                            " '" + msgetsalarygradeGID + "'," +
                            " '" + values.template_name + "'," +
                            " '" + dt.employee_gid + "'," +
                            " '" + values.salary_mode + "'," +
                            " '" + user_gid + "'," +
                            " '" + user_gid + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    "COALESCE('" + values.BasicSalary + "', 0)," +
                    "COALESCE('" + values.gross_salary + "', 0)," +
                    "COALESCE('" + values.ctc + "', 0)," +
                    "COALESCE('" + values.net_salary + "', 0))";

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
                                            " '" + msgetsalarygradeGID + "'," +
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
                                        " '" + msgetsalarygradeGID + "'," +
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
                                            " '" + msgetsalarygradeGID + "'," +
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
