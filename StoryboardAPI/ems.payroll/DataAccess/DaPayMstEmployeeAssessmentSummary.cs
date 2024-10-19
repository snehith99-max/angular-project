using System;
using System.Collections.Generic;
using System.Web;
using ems.payroll.Models;
using ems.utilities.Functions;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using File = System.IO.File;
using System.Data;
using System.Globalization;
using System.Data.Odbc;
using System.Net.Http.Headers;

namespace ems.payroll.DataAccess
{
    public class DaPayMstEmployeeAssessmentSummary
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string msGetGid, pdf_name, lspath2, lspath1, mail_path, final_path, lsgrosssalary;
        double lsgross_salary, lsgross_salary_new;
        public void DaEmployeeAssessmentSummary(MdlPayMstEmployeeAssessmentSummary values)
        {
            try
            {
                msSQL = " select assessment_gid, cast(concat(assessmentyear_startdate,' ','to',' ', assessmentyear_enddate) as char) as duration, " +
                        " cast(concat(financialyear_startdate,' ','to',' ', financialyear_enddate) as char) as fin_duration, status " +
                        " from pay_mst_tassessmentyear " +
                        " order by assessment_gid asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employeeassessmentsummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employeeassessmentsummary_list
                        {
                            assessment_gid = dt["assessment_gid"].ToString(),
                            duration = dt["duration"].ToString(),
                            fin_duration = dt["fin_duration"].ToString(),
                            status = dt["status"].ToString(),
                        });
                        values.employeeassessmentsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading assessment details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public MdlEmployeePersonalData DaEmployeePersonalData(string employee_gid)
        {
            try
            {
                MdlEmployeePersonalData objemployeepersonaldatalist = new MdlEmployeePersonalData();

                msSQL = " select permanentaddress_gid from hrm_trn_temployeedtl " +
                        " where employee_gid='" + employee_gid + "' ";

                string lspermanentaddress_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select b.user_firstname, b.user_lastname, Date_Format(a.employee_dob,'%Y-%m-%d') as employee_dob, a.employee_emailid, a.employee_gender, " +
                        " a.pan_no, a.uan_no, a.bloodgroup, a.employee_mobileno, x.address1, x.address2, x.city, x.state, x.postal_code, z.country_name " +
                        " from hrm_mst_temployee a inner join adm_mst_tuser b on a.user_gid = b.user_gid " +
                        " left join adm_mst_taddress x on a.employee_gid = x.parent_gid " +
                        " left join hrm_trn_temployeedtl y on x.address_gid = y.permanentaddress_gid and y.permanentaddress_gid='" + lspermanentaddress_gid + "' " +
                        " left join adm_mst_tcountry z on x.country_gid = z.country_gid " +
                        " where a.employee_gid='" + employee_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();

                    objemployeepersonaldatalist.user_firstname = objMySqlDataReader["user_firstname"].ToString();
                    objemployeepersonaldatalist.user_lastname = objMySqlDataReader["user_lastname"].ToString();
                    objemployeepersonaldatalist.employee_dob = objMySqlDataReader["employee_dob"].ToString();
                    objemployeepersonaldatalist.employee_emailid = objMySqlDataReader["employee_emailid"].ToString();
                    objemployeepersonaldatalist.employee_gender = objMySqlDataReader["employee_gender"].ToString();
                    objemployeepersonaldatalist.pan_no = objMySqlDataReader["pan_no"].ToString();
                    objemployeepersonaldatalist.uan_no = objMySqlDataReader["uan_no"].ToString();
                    objemployeepersonaldatalist.bloodgroup = objMySqlDataReader["bloodgroup"].ToString();
                    objemployeepersonaldatalist.employee_mobileno = objMySqlDataReader["employee_mobileno"].ToString();
                    objemployeepersonaldatalist.address1 = objMySqlDataReader["address1"].ToString();
                    objemployeepersonaldatalist.address2 = objMySqlDataReader["address2"].ToString();
                    objemployeepersonaldatalist.city = objMySqlDataReader["city"].ToString();
                    objemployeepersonaldatalist.state = objMySqlDataReader["state"].ToString();
                    objemployeepersonaldatalist.postal_code = objMySqlDataReader["postal_code"].ToString();
                    objemployeepersonaldatalist.country_name = objMySqlDataReader["country_name"].ToString();

                    objMySqlDataReader.Close();
                }
                return objemployeepersonaldatalist;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                ex.ToString();
                return null;
            }
        }
        public void DaEmployeeOldTaxSlabSummary(MdlPayMstEmployeeAssessmentSummary values)
        {
            try
            {
                msSQL = " select tax_regime_gid, concat(tax_slabs_fromold, ' to ', tax_slabs_toold) as tax_slab, individuals, resident_senior_citizens, resident_super_senior_citizens " +
                        " from acp_mst_tincometax_regime where tax_name = 'Old Regime' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employeeoldtaxslabsummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employeeoldtaxslabsummary_list
                        {
                            tax_regime_gid = dt["tax_regime_gid"].ToString(),
                            tax_slab = dt["tax_slab"].ToString(),
                            individuals = dt["individuals"].ToString(),
                            resident_senior_citizens = dt["resident_senior_citizens"].ToString(),
                            resident_super_senior_citizens = dt["resident_super_senior_citizens"].ToString(),
                        });
                        values.employeeoldtaxslabsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading income tax details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaEmployeeNewTaxSlabSummary(MdlPayMstEmployeeAssessmentSummary values)
        {
            try
            {
                msSQL = " select tax_regime_gid, concat(tax_slabs_fromnew, ' to ', tax_slabs_tonew) as tax_slabnew, remarks_new, " +
                        " case when remarks_new <> '' then concat(income_tax_rates, ' ', '( ', remarks_new, ' )') else income_tax_rates end as income_tax_rates1 " +
                        " from acp_mst_tincometax_regime where tax_name = 'New Regime' order by created_date ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employeenewtaxslabsummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employeenewtaxslabsummary_list
                        {
                            tax_regime_gid = dt["tax_regime_gid"].ToString(),
                            tax_slabnew = dt["tax_slabnew"].ToString(),
                            income_tax_rates1 = dt["income_tax_rates1"].ToString(),
                        });
                        values.employeenewtaxslabsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading income tax details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaFinYearDropdown(MdlPayMstEmployeeAssessmentSummary values)
        {
            try
            {
                msSQL = " SELECT finyear_gid, CONCAT(YEAR(fyear_start), ' - ', YEAR(IFNULL(fyear_end, NOW()))) AS finyear_range FROM adm_mst_tyearendactivities ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<getfinyeardropdownlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new getfinyeardropdownlist
                        {
                            finyear_gid = dt["finyear_gid"].ToString(),
                            finyear_range = dt["finyear_range"].ToString(),

                        });
                        values.getfinyeardropdownlist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Finyear!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostEmployeeIncomeTaxDocument(HttpRequest httpRequest, string user_gid, result objResult)
        {
            // attachment get function

            HttpFileCollection httpFileCollection;
            MemoryStream ms_stream = new MemoryStream();
            HttpPostedFile httpPostedFile;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;            
            string document_gid = string.Empty;            
            string lspath;
            string msGetGid;

            msSQL = "Select company_code  from adm_mst_tcompany ";
            string lscompany_code = objdbconn.GetExecuteScalar(msSQL);

            //split function
            string finyear_range = httpRequest.Form[1];
            string document_type = httpRequest.Form[2];
            string document_title = httpRequest.Form[3];
            string remarks = httpRequest.Form[4];
            string fName = httpRequest.Form[5];

            MemoryStream ms = new MemoryStream();
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;

                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                        httpPostedFile = httpFileCollection[i];
                        string FileExtension = httpPostedFile.FileName;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);
                        bool status1;

                        status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "Payroll/Assessment/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                        ms.Close();
                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "Payroll/Assessment/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                        try
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("TDMP");
                            {
                                msSQL = " insert into pay_trn_ttaxdocument " +
                                        " (taxdocument_gid, " +
                                        " finyear_gid ," +
                                        " documenttype_gid, " +
                                        " document_title," +
                                        " remarks, " +
                                        " document_upload," +
                                        " created_by, " +
                                        " created_date) " +
                                        " values( " +
                                        "'" + msGetGid + "'," +
                                        "'" + finyear_range + "'," +
                                        "'" + document_type + "'," +
                                        "'" + document_title + "'," +
                                        "'" + remarks + "'," +
                                        "'" + final_path + msdocument_gid + FileExtension + "'," +
                                        "'" + user_gid + "'," +
                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            }

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                objResult.status = true;
                                objResult.message = "Income Tax document details submitted successfully";
                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error occured while submitting income tax document details !!";
                            }
                        }
                        catch (Exception ex)
                        {
                            objResult.message = "Exception occured while loading Finyear!";
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        public void DaEmployeeIncomeTaxSummary(MdlPayMstEmployeeAssessmentSummary values)
        {
            try
            {
                msSQL = " select a.taxdocument_gid,a.document_upload, CONCAT(YEAR(b.fyear_start), ' - ', YEAR(IFNULL(b.fyear_end, NOW()))) AS finyear_range, date_format(a.created_date,'%Y-%m-%d') as created_date, a.documenttype_gid, a.document_title " +
                        " from pay_trn_ttaxdocument a left join adm_mst_tyearendactivities b on a.finyear_gid = b.finyear_gid " +
                        " ORDER BY taxdocument_gid DESC ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<employeeincometaxsummary_lists>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new employeeincometaxsummary_lists
                        {
                            taxdocument_gid = dt["taxdocument_gid"].ToString(),
                            finyear_range = dt["finyear_range"].ToString(),
                            created_date = Convert.ToDateTime(dt["created_date"].ToString()),
                            documenttype_gid = dt["documenttype_gid"].ToString(),
                            document_title = dt["document_title"].ToString(),
                            document_upload = dt["document_upload"].ToString(),

                        });
                        values.employeeincometaxsummary_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Finyear!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaPostEmployeeIncomeData(MdlEmployeeIncomedata values, string employee_gid)
        {
            try
            {
                msSQL = " delete from pay_trn_ttdsgrosssalary where employee_gid = '" + employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from pay_trn_ttdsallowencetotheextent where employee_gid = '" + employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " delete from pay_trn_ttdsotherincomeemployee where employee_gid = '" + employee_gid + "' and assessment_gid = '" + values.assessment_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " select gross_salary from pay_trn_temployee2salarygradetemplate where employee_gid = '" + employee_gid + "' ";
                lsgrosssalary = objdbconn.GetExecuteScalar(msSQL);

                lsgross_salary = double.Parse(lsgrosssalary);
                lsgross_salary_new = (lsgross_salary) * 12;

                string msGetGid = objcmnfunctions.GetMasterGID("PAGS");

                msSQL = " insert into pay_trn_ttdsgrosssalary ( " +
                        " tdsgrosssalary_gid, " +
                        " assessment_gid, " +
                        " employee_gid, " +
                        " grosssalary_amount, " +
                        " perquisites_amount, " +
                        " profitinlieu_amount, " +
                        " grosstotal_qualifiying_amount, " +
                        " created_by," +
                        " created_date " +
                        " ) values ( " +
                        " '" + msGetGid + "', " +
                        " '" + values.assessment_gid + "', " +
                        " '" + employee_gid + "', " +
                        " '" + lsgross_salary_new + "', " +
                        " '" + values.perquisites_amount + "', " +
                        " '" + values.profitof_salary + "', " +
                        " '" + values.totalamount + "', " +
                        " '" + employee_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    string msgetGid = objcmnfunctions.GetMasterGID("PAAE");

                    msSQL = " insert into pay_trn_ttdsallowencetotheextent ( " +
                            " tdsallowencetotheextent_gid, " +
                            " assessment_gid, " +
                            " employee_gid, " +
                            " lessallowence_name1, " +
                            " lessallowence_name2, " +
                            " lessallowence_name3, " +
                            " transport_totamount, " +
                            " lessallowence_amount2, " +
                            " lessallowence_amount3, " +
                            " transport_qualifiying_amount, " +
                            " balance_qualifiying_amount, " +
                            " entertainment_amount, " +
                            " taxonemployment_amount, " +
                            " aggreegate_qualifiying_amount, " +
                            " incomechargableunder_headsal_deductible_amount, " +
                            " created_by," +
                            " created_date " +
                            " ) values ( " +
                            " '" + msgetGid + "', " +
                            " '" + values.assessment_gid + "', " +
                            " '" + employee_gid + "', " +
                            " '" + values.component1 + "', " +
                            " '" + values.component2 + "', " +
                            " '" + values.component3 + "', " +
                            " '" + values.pfamount1 + "', " +
                            " '" + values.pfamount2 + "', " +
                            " '" + values.pfamount3 + "', " +
                            " '" + values.lessallowancetotal + "', " +
                            " '" + values.balanceamount + "', " +
                            " '" + values.entertainment_allowance + "', " +
                            " '" + values.taxon_emp + "', " +
                            " '" + values.aggreagateofab + "', " +
                            " '" + values.incomecharge_headsalaries + "', " +
                            " '" + employee_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult == 1)
                {
                    string msgetGid = objcmnfunctions.GetMasterGID("PAOI");

                    msSQL = " insert into pay_trn_ttdsotherincomeemployee ( " +
                            " tdsotherincomeemployee_gid, " +
                            " assessment_gid, " +
                            " employee_gid, " +
                            " otherincome1_name, " +
                            " otherincome2_name, " +
                            " otherincome3_name, " +
                            " otherincomeemployee_totamount1, " +
                            " otherincomeemployee_totamount2, " +
                            " otherincomeemployee_totamount3, " +
                            " otherincomeemployee_qualifiying_amount3, " +
                            " overallgross_deductible_amount, " +
                            " created_by," +
                            " created_date " +
                            " ) values ( " +
                            " '" + msgetGid + "', " +
                            " '" + values.assessment_gid + "', " +
                            " '" + employee_gid + "', " +
                            " '" + values.employee_income1 + "', " +
                            " '" + values.employee_income2 + "', " +
                            " '" + values.employee_income3 + "', " +
                            " '" + values.employeeincome_rs1 + "', " +
                            " '" + values.employeeincome_rs2 + "', " +
                            " '" + values.employeeincome_rs3 + "', " +
                            " '" + values.employeeincome_total + "', " +
                            " '" + values.grosstotal_income + "', " +
                            " '" + employee_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Income Details are Inserted Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred while inserting income details !!";
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public MdlEmployeeIncomedata DaEmployeeIncomedata(string employee_gid, string assessment_gid)
        {
            try
            {
                MdlEmployeeIncomedata objemployeeincomedatalist = new MdlEmployeeIncomedata();

                msSQL = " select format(a.grosssalary_amount, 2) as grosssalary_amount, format(a.perquisites_amount, 2) as perquisites_amount, " +
                        " format(a.profitinlieu_amount, 2) as profitinlieu_amount, format(a.grosstotal_qualifiying_amount, 2) as grosstotal_qualifiying_amount, " +
                        " b.lessallowence_name1, b.lessallowence_name2, b.lessallowence_name3, format(b.lessallowence_amount1, 2) as lessallowence_amount1, " +
                        " format(b.lessallowence_amount2, 2) as lessallowence_amount2, format(b.lessallowence_amount3, 2) as lessallowence_amount3, " +
                        " format(b.transport_qualifiying_amount, 2) as transport_qualifiying_amount, format(b.balance_qualifiying_amount, 2) as balance_qualifiying_amount, " +
                        " format(b.entertainment_amount, 2) as entertainment_amount, format(b.taxonemployment_amount, 2) as taxonemployment_amount, " +
                        " format(b.aggreegate_qualifiying_amount, 2) as aggreegate_qualifiying_amount, format(b.incomechargableunder_headsal_deductible_amount, 2) as incomechargableunder_headsal_deductible_amount, " +
                        " c.otherincome1_name, c.otherincome2_name, c.otherincome3_name, format(c.otherincomeemployee_totamount1, 2) as otherincomeemployee_totamount1, " +
                        " format(c.otherincomeemployee_totamount2, 2) as otherincomeemployee_totamount2, format(c.otherincomeemployee_totamount3, 2) as otherincomeemployee_totamount3, " +
                        " format(c.otherincomeemployee_qualifiying_amount3, 2) as otherincomeemployee_qualifiying_amount3, format(c.overallgross_deductible_amount, 2) as overallgross_deductible_amount " +
                        " from pay_trn_ttdsgrosssalary a " +
                        " inner join pay_trn_ttdsallowencetotheextent b on a.employee_gid = b.employee_gid " +
                        " inner join pay_trn_ttdsotherincomeemployee c on b.employee_gid = c.employee_gid " +
                        " where a.assessment_gid='" + assessment_gid + "' and a.employee_gid='" + employee_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                msSQL = " select gross_salary from pay_trn_temployee2salarygradetemplate where employee_gid = '" + employee_gid + "' ";
                lsgrosssalary = objdbconn.GetExecuteScalar(msSQL);

                lsgross_salary = double.Parse(lsgrosssalary);
                lsgross_salary_new = Math.Round((lsgross_salary) * 12, 2);
                objemployeeincomedatalist.grosssalary_amount = lsgross_salary_new;

                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();

                    objemployeeincomedatalist.grosssalary_amount = lsgross_salary_new;
                    objemployeeincomedatalist.perquisites_amount = objMySqlDataReader["perquisites_amount"].ToString();
                    objemployeeincomedatalist.profitof_salary = objMySqlDataReader["profitinlieu_amount"].ToString();
                    objemployeeincomedatalist.totalamount = objMySqlDataReader["grosstotal_qualifiying_amount"].ToString();
                    objemployeeincomedatalist.component1 = objMySqlDataReader["lessallowence_name1"].ToString();
                    objemployeeincomedatalist.component2 = objMySqlDataReader["lessallowence_name2"].ToString();
                    objemployeeincomedatalist.component3 = objMySqlDataReader["lessallowence_name3"].ToString();
                    objemployeeincomedatalist.pfamount1 = objMySqlDataReader["lessallowence_amount1"].ToString();
                    objemployeeincomedatalist.pfamount2 = objMySqlDataReader["lessallowence_amount2"].ToString();
                    objemployeeincomedatalist.pfamount3 = objMySqlDataReader["lessallowence_amount3"].ToString();
                    objemployeeincomedatalist.lessallowancetotal = objMySqlDataReader["transport_qualifiying_amount"].ToString();
                    objemployeeincomedatalist.balanceamount = objMySqlDataReader["balance_qualifiying_amount"].ToString();
                    objemployeeincomedatalist.entertainment_allowance = objMySqlDataReader["entertainment_amount"].ToString();
                    objemployeeincomedatalist.taxon_emp = objMySqlDataReader["taxonemployment_amount"].ToString();
                    objemployeeincomedatalist.aggreagateofab = objMySqlDataReader["aggreegate_qualifiying_amount"].ToString();
                    objemployeeincomedatalist.incomecharge_headsalaries = objMySqlDataReader["incomechargableunder_headsal_deductible_amount"].ToString();
                    objemployeeincomedatalist.employee_income1 = objMySqlDataReader["otherincome1_name"].ToString();
                    objemployeeincomedatalist.employee_income2 = objMySqlDataReader["otherincome2_name"].ToString();
                    objemployeeincomedatalist.employee_income3 = objMySqlDataReader["otherincome3_name"].ToString();
                    objemployeeincomedatalist.employeeincome_rs1 = objMySqlDataReader["otherincomeemployee_totamount1"].ToString();
                    objemployeeincomedatalist.employeeincome_rs2 = objMySqlDataReader["otherincomeemployee_totamount2"].ToString();
                    objemployeeincomedatalist.employeeincome_rs3 = objMySqlDataReader["otherincomeemployee_totamount3"].ToString();
                    objemployeeincomedatalist.employeeincome_total = objMySqlDataReader["otherincomeemployee_qualifiying_amount3"].ToString();
                    objemployeeincomedatalist.grosstotal_income = objMySqlDataReader["overallgross_deductible_amount"].ToString();

                    objMySqlDataReader.Close();
                }
                return objemployeeincomedatalist;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return null;
            }
        }        
        public void DaPostEmployeeTDSData(MdlEmployeeTDSData values, string tds_gid, string employee_gid)
        {
            try
            {
                msSQL = " update pay_trn_ttds set " +
                        " status = 'Approval Pending'," +
                        " updated_by = '" + employee_gid + "'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                        " where tds_gid = '" + tds_gid + "' and " +
                        " employee_gid = '" + employee_gid + "' ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "TDS Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while updating TDS info";
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public MdlEmployeeTDSData DaEmployeeTDSData(string employee_gid, string assessment_gid)
        {
            try
            {
                MdlEmployeeTDSData objemployeetdsdatalist = new MdlEmployeeTDSData();

                msSQL = " select d.tds_gid, a.tdsdeductions_gid, a.assessment_gid, a.employee_gid, a.section80c_name1, format(a.section80c_grossamount1, 2) as section80c_grossamount1, " +
                        " a.section80c_name2, format(a.section80c_grossamount2, 2) as section80c_grossamount2, a.section80c_name3, format(a.section80c_grossamount3, 2) as section80c_grossamount3, " +
                        " a.section80c_name4, format(a.section80c_grossamount4, 2) as section80c_grossamount4, a.section80c_name5, format(a.section80c_grossamount5, 2) as section80c_grossamount5, " +
                        " a.section80c_name6, format(a.section80c_grossamount6, 2) as section80c_grossamount6, a.section80c_name7, format(a.section80c_grossamount7, 2) as section80c_grossamount7, " +
                        " format(a.section80c_amount7, 2) as section80c_amount7, format(a.section80c_deductible_amount7, 2) as section80c_deductible_amount7, format(a.section80ccc_grossamount, 2) as section80ccc_grossamount, " +
                        " format(a.section80ccc_deductible_amount, 2) as section80ccc_deductible_amount, format(a.section80ccd_grossamount, 2) as section80ccd_grossamount, format(a.section80ccd_deductible_amount, 2) as section80ccd_deductible_amount, " +
                        " format(a.aggregate_gross, 2) as aggregate_gross, format(a.aggregate_deduct, 2) as aggregate_deduct, format(a.section80ccd1b_grossamount, 2) as section80ccd1b_grossamount, format(a.section80ccd1b_deductamount, 2) as section80ccd1b_deductamount," +
                        " b.section1_name, format(b.section1_grossamount, 2) as section1_grossamount, format(b.section1_qualifiying_amount, 2) as section1_qualifiying_amount, format(b.section1_deductible_amount, 2) as section1_deductible_amount, " +
                        " b.section2_name, format(b.section2_grossamount, 2) as section2_grossamount, format(b.section2_qualifiying_amount, 2) as section2_qualifiying_amount, format(b.section2_deductible_amount, 2) as section2_deductible_amount, " +
                        " b.section3_name, format(b.section3_grossamount, 2) as section3_grossamount, format(b.section3_qualifiying_amount, 2) as section3_qualifiying_amount, format(b.section3_deductible_amount, 2) as section3_deductible_amount, " +
                        " b.section4_name, format(b.section4_grossamount, 2) as section4_grossamount, format(b.section4_qualifiying_amount, 2) as section4_qualifiying_amount, format(b.section4_deductible_amount, 2) as section4_deductible_amount, " +
                        " b.section5_name, format(b.section5_grossamount, 2) as section5_grossamount, format(b.section5_qualifiying_amount, 2) as section5_qualifiying_amount, format(b.section5_deductible_amount, 2) as section5_deductible_amount, " +
                        " format(c.aggregatedeductable_totalamount, 2) as aggregatedeductable_totalamount, format(d.total_income, 2) as total_income, format(d.tax_total_income, 2) as tax_total_income, " +
                        " format(d.educationcess_amount, 2) as educationcess_amount, format(d.tax_payable12plus13, 2) as tax_payable12plus13, format(d.less_relief89, 2) as less_relief89, " +
                        " format(d.tax_payable14minus15, 2) as tax_payable14minus15, format(d.less_tds, 2) as less_tds, format(d.balance_tax, 2) as balance_tax " +
                        " from pay_trn_ttdsdeductions a " +
                        " inner join pay_trn_ttdsothersections b on a.employee_gid = b.employee_gid " +
                        " inner join pay_trn_ttdsaggregateofdeductable c on b.employee_gid = c.employee_gid " +
                        " inner join pay_trn_ttds d on c.employee_gid = d.employee_gid " +
                        " where a.assessment_gid='" + assessment_gid + "' and a.employee_gid='" + employee_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();
                    objemployeetdsdatalist.tds_gid = objMySqlDataReader["tds_gid"].ToString();
                    objemployeetdsdatalist.section80C_i_name = objMySqlDataReader["section80c_name1"].ToString();
                    objemployeetdsdatalist.section80C_i_value = objMySqlDataReader["section80c_grossamount1"].ToString();
                    objemployeetdsdatalist.section80C_ii_name = objMySqlDataReader["section80c_name2"].ToString();
                    objemployeetdsdatalist.section80C_ii_value = objMySqlDataReader["section80c_grossamount2"].ToString();
                    objemployeetdsdatalist.section80C_iii_name = objMySqlDataReader["section80c_name3"].ToString();
                    objemployeetdsdatalist.section80C_iii_value = objMySqlDataReader["section80c_grossamount3"].ToString();
                    objemployeetdsdatalist.section80C_iv_name = objMySqlDataReader["section80c_name4"].ToString();
                    objemployeetdsdatalist.section80C_iv_value = objMySqlDataReader["section80c_grossamount4"].ToString();
                    objemployeetdsdatalist.section80C_v_name = objMySqlDataReader["section80c_name5"].ToString();
                    objemployeetdsdatalist.section80C_v_value = objMySqlDataReader["section80c_grossamount5"].ToString();
                    objemployeetdsdatalist.section80C_vi_name = objMySqlDataReader["section80c_name6"].ToString();
                    objemployeetdsdatalist.section80C_vi_value = objMySqlDataReader["section80c_grossamount6"].ToString();
                    objemployeetdsdatalist.section80C_vii_name = objMySqlDataReader["section80c_name7"].ToString();
                    objemployeetdsdatalist.section80C_vii_value = objMySqlDataReader["section80c_amount7"].ToString();
                    objemployeetdsdatalist.section80C_vii_gross_total = objMySqlDataReader["section80c_grossamount7"].ToString();
                    objemployeetdsdatalist.section80C_vii_deductable_total = objMySqlDataReader["section80c_deductible_amount7"].ToString();
                    objemployeetdsdatalist.section80CCC_gross_total = objMySqlDataReader["section80ccc_grossamount"].ToString();
                    objemployeetdsdatalist.section80CCC_deductable_total = objMySqlDataReader["section80ccc_deductible_amount"].ToString();
                    objemployeetdsdatalist.section80CCD_gross_total = objMySqlDataReader["section80ccd_grossamount"].ToString();
                    objemployeetdsdatalist.section80CCD_deductable_total = objMySqlDataReader["section80ccd_deductible_amount"].ToString();
                    objemployeetdsdatalist.aggregate3sec_gross_total = objMySqlDataReader["aggregate_gross"].ToString();
                    objemployeetdsdatalist.aggregate3sec_deductable_total = objMySqlDataReader["aggregate_deduct"].ToString();
                    objemployeetdsdatalist.section80CCD1B_gross_total = objMySqlDataReader["section80ccd1b_grossamount"].ToString();
                    objemployeetdsdatalist.section80CCD1B_deductable_total = objMySqlDataReader["section80ccd1b_deductamount"].ToString();
                    objemployeetdsdatalist.other_section1_value = objMySqlDataReader["section1_name"].ToString();
                    objemployeetdsdatalist.other_section1_gross_amount = objMySqlDataReader["section1_grossamount"].ToString();
                    objemployeetdsdatalist.other_section1_qualifying_amount = objMySqlDataReader["section1_qualifiying_amount"].ToString();
                    objemployeetdsdatalist.other_section1_deductable = objMySqlDataReader["section1_deductible_amount"].ToString();
                    objemployeetdsdatalist.other_section2_value = objMySqlDataReader["section2_name"].ToString();
                    objemployeetdsdatalist.other_section2_gross_amount = objMySqlDataReader["section2_grossamount"].ToString();
                    objemployeetdsdatalist.other_section2_qualifying_amount = objMySqlDataReader["section2_qualifiying_amount"].ToString();
                    objemployeetdsdatalist.other_section2_deductable = objMySqlDataReader["section2_deductible_amount"].ToString();
                    objemployeetdsdatalist.other_section3_value = objMySqlDataReader["section3_name"].ToString();
                    objemployeetdsdatalist.other_section3_gross_amount = objMySqlDataReader["section3_grossamount"].ToString();
                    objemployeetdsdatalist.other_section3_qualifying_amount = objMySqlDataReader["section3_qualifiying_amount"].ToString();
                    objemployeetdsdatalist.other_section3_deductable = objMySqlDataReader["section3_deductible_amount"].ToString();
                    objemployeetdsdatalist.other_section4_value = objMySqlDataReader["section4_name"].ToString();
                    objemployeetdsdatalist.other_section4_gross_amount = objMySqlDataReader["section4_grossamount"].ToString();
                    objemployeetdsdatalist.other_section4_qualifying_amount = objMySqlDataReader["section4_qualifiying_amount"].ToString();
                    objemployeetdsdatalist.other_section4_deductable = objMySqlDataReader["section4_deductible_amount"].ToString();
                    objemployeetdsdatalist.other_section5_value = objMySqlDataReader["section5_name"].ToString();
                    objemployeetdsdatalist.other_section5_gross_amount = objMySqlDataReader["section5_grossamount"].ToString();
                    objemployeetdsdatalist.other_section5_qualifying_amount = objMySqlDataReader["section5_qualifiying_amount"].ToString();
                    objemployeetdsdatalist.other_section5_deductable = objMySqlDataReader["section5_deductible_amount"].ToString();
                    objemployeetdsdatalist.aggregate4Asec_deductible_total = objMySqlDataReader["aggregatedeductable_totalamount"].ToString();
                    objemployeetdsdatalist.total_taxable_income = objMySqlDataReader["total_income"].ToString();
                    objemployeetdsdatalist.tax_on_total_income = objMySqlDataReader["tax_total_income"].ToString();
                    objemployeetdsdatalist.educationcess = objMySqlDataReader["educationcess_amount"].ToString();
                    objemployeetdsdatalist.tax_payable = objMySqlDataReader["tax_payable12plus13"].ToString();
                    objemployeetdsdatalist.less_relief = objMySqlDataReader["less_relief89"].ToString();
                    objemployeetdsdatalist.net_tax_payable = objMySqlDataReader["tax_payable14minus15"].ToString();
                    objemployeetdsdatalist.less_tax_deducted_at_source = objMySqlDataReader["less_tds"].ToString();
                    objemployeetdsdatalist.balance_tax_pay_refund = objMySqlDataReader["balance_tax"].ToString();

                    objMySqlDataReader.Close();
                }
                return objemployeetdsdatalist;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Payroll/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                ex.ToString();
                return null;
            }
        }
    }
}