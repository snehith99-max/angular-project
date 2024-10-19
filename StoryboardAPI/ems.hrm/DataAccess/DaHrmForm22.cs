using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Globalization;

namespace ems.hrm.DataAccess
{
    public class DaHrmForm22
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string lsattendance_startdate, lsattendance_enddate, lsEmployee_gid, lsemployee_name, lsleavegrade_gid, msgetassign2employee_gid;

        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lsempoyeegid, msgetshift, lsleavegrade_code, lsleavegrade_name, lsleavetype_gid, lsleavetype_name, lstotal_leavecount, lsavailable_leavecount, lsleave_limit, lsform_gid;

        public void DaCompanySummary(MdlHrmForm22 values)
        {
            try
            {
                msSQL = " select company_gid,company_name,company_address,contact_person,contactperson_address,occupier_address,occupier_name," +
                        " natureof_industry, " + " companyregistrtion_number from adm_mst_tcompany ";
                dt_datatable = objdbconn.GetDataTable(msSQL);


                var getModuleList = new List<company_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new company_list
                        {
                            company_gid = dt["company_gid"].ToString(),
                            company_name = dt["company_name"].ToString(),
                            company_address = dt["company_address"].ToString(),
                            contact_person = dt["contact_person"].ToString(),
                            contactperson_address = dt["contactperson_address"].ToString(),
                            occupier_address = dt["occupier_address"].ToString(),
                            occupier_name = dt["occupier_name"].ToString(),
                            natureof_industry = dt["natureof_industry"].ToString(),
                            companyregistrtion_number = dt["companyregistrtion_number"].ToString(),

                        });
                        values.company_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while loading Company Master!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }


        }

        public void DaGetYearReturnSummary(MdlHrmForm22 values)
        {
            try
            {
                msSQL = " select a.form_gid,b.form_name,processed_year from hrm_mst_tyearlyreturns a " +
                        " inner join hrm_mst_tstatutoryform b on a.form_name=b.sanctuaryform_gid order by form_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<yearreturn_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new yearreturn_list
                        {
                            form_gid = dt["form_gid"].ToString(),
                            form_name = dt["form_name"].ToString(),
                            processed_year = dt["processed_year"].ToString(),


                        });
                        values.yearreturn_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Year Return Master!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }

        public void DaGetformdropdown(MdlHrmForm22 values)
        {
            try
            {

                msSQL = " select sanctuaryform_gid,form_name from hrm_mst_tstatutoryform ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getformdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getformdropdown
                        {
                            sanctuaryform_gid = dt["sanctuaryform_gid"].ToString(),
                            form_name = dt["form_name"].ToString(),
                        });
                        values.Getformdropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Form Name!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostForm(string user_gid, form_list values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("PYTP");
                if (msGetGid == "E")
                {
                    objdbconn.CloseConn();
                    values.status = false;
                    values.message = "Can not generate your unique id";
                }
                msSQL = " insert into hrm_mst_tyearlyreturns (" +
                        " form_gid, " +
                        " form_name, " +
                        " processed_year, " +
                        " created_by, " +
                        " created_date ) " +
                        " values ( " +
                        " '" + msGetGid + "', " +
                        " '" + values.form_name + "', " +
                        " '" + values.processed_year + "',";

                msSQL += "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Year Return Added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Year Return";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Form!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        //public void DaGetHalfyearlysubrule3Summary(MdlHrmForm22 values, string form_gid)
        //{
        //    try
        //    {
        //        msSQL = " select a.numberof_workmen,a.numberofworkmen_form1,a.totalworkdays_year,a.nonpermanent_count,a.permanent_count,a.permanentcount_firstjuly,a.reasons,a.remarks, " + " " +
        //                " a.company_address,a.typeof_industry,b.form_gid,b.processed_year from hrm_trn_thalfyearlysubrule3 a inner join hrm_mst_tyearlyreturns b on a.form_gid=b.form_gid " + " where a.form_gid= '" + form_gid + "'  ";
        //        dt_datatable = objdbconn.GetDataTable(msSQL);

        //        var getModuleList = new List<halfyearlysubrule3_list>();
        //        if (dt_datatable.Rows.Count != 0)
        //        {
        //            foreach (DataRow dt in dt_datatable.Rows)
        //            {
        //                getModuleList.Add(new halfyearlysubrule3_list
        //                {
        //                    numberof_workmen = dt["numberof_workmen"].ToString(),
        //                    numberofworkmen_form1 = dt["numberofworkmen_form1"].ToString(),
        //                    totalworkdays_year = dt["totalworkdays_year"].ToString(),
        //                    nonpermanent_count = dt["nonpermanent_count"].ToString(),
        //                    permanent_count = dt["permanent_count"].ToString(),
        //                    permanentcount_firstjuly = dt["permanentcount_firstjuly"].ToString(),
        //                    reasons = dt["reasons"].ToString(),
        //                    remarks = dt["remarks"].ToString(),
        //                    company_address = dt["company_address"].ToString(),
        //                    typeof_industry = dt["typeof_industry"].ToString(),
        //                    form_gid = dt["form_gid"].ToString(),
        //                    processed_year = dt["processed_year"].ToString(),

        //                });
        //                values.halfyearlysubrule3_list = getModuleList;
        //            }
        //        }
        //        dt_datatable.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while loading Half Yearly Summary!";
        //        values.status = false;
        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
        //        "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

        //    }
        //}

        public void DaForm2SubRule3Submit(string employee_gid, form2subrule3 values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("HHYR");
                msSQL = " insert into hrm_trn_thalfyearlysubrule3 ( " +
                    " halfyearlysubrule3_gid, " +
                    " numberof_workmen, " +
                    " numberofworkmen_form1, " +
                    " totalworkdays_year, " +
                    " nonpermanent_count, " +
                    " permanent_count, " +
                    " permanentcount_firstjuly, " +
                    " reasons, " +
                    " remarks, " +
                    " processed_year, " +
                    " form_gid, " +
                    " company_address, " +
                    " typeof_industry, " +
                    " created_by, " +
                    " created_date) " +
                    " values ( " +
                    "'" + msGetGid + "'," +
                    "'" + values.numberof_workmen + "'," +
                    "'" + values.numberofworkmen_form1 + "'," +
                    "'" + values.totalworkdays_year + "'," +
                    "'" + values.nonpermanent_count + "'," +
                    "'" + values.permanent_count + "'," +
                    "'" + values.permanentcount_firstjuly + "'," +
                    "'" + values.reason_delay + "'," +
                    "'" + values.remarks + "'," +
                    "'" + values.processed_year + "'," +
                    "'" + values.form_gid + "'," +
                    "'" + values.company_address + "', " +
                    "'" + values.typeof_industry + "', " +
                    "'" + employee_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Details are added successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while adding details";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaForm2SubRule4Submit(string employee_gid, form2subrule4 values)
        {
            try
            {
                msGetGid = objcmnfunctions.GetMasterGID("HYRF");
                msSQL = " insert into hrm_trn_thalfyearlysubrule4 ( " +
                       " halfyearlysubrule4_gid, " +
                       " postal_address, " +
                       " managingpartner_address, " +
                       " serial_number, " +
                       " nameaddressofemployee_suspension, " +
                       " wagespaid_monthlyemployees, " +
                       " departmentanddesignation_last, " +
                       " natureof_offence, " +
                       " suspension_date, " +
                       " commencementenquiry_date, " +
                       " completionenquiry_date, " +
                       " revocationsuspension_date, " +
                       " subsistenceallowence_rate, " +
                       " subsistenceallowence_paid, " +
                       " dateofissue_finalorder, " +
                       " employees_punishment, " +
                       " remarks, " +
                       " form_gid, " +
                       " created_by, " +
                       " created_date) " +
                       "  values ( " +
                       "'" + msGetGid + "'," +
                       "'" + values.postal_address + "'," +
                       "'" + values.nameaddress_employers + "'," +
                       "'" + values.serial_number + "'," +
                       "'" + values.nameaddressofemployee_suspension + "'," +
                       "'" + values.wagespaid_monthlyemployees + "'," +
                       "'" + values.departmentanddesignation_last + "'," +
                       "'" + values.natureof_offence + "'," +
                       "'" + values.suspension_date + "'," +
                       "'" + values.commencementenquiry_date + "'," +
                       "'" + values.completionenquiry_date + "'," +
                       "'" + values.revocationsuspension_date + "', " +
                       "'" + values.subsistenceallowence_rate + "', " +
                       "'" + values.subsistenceallowence_paid + "', " +
                       "'" + values.dateofissue_finalorder + "', " +
                       "'" + values.employees_punishment + "'," +
                       "'" + values.remarks_data + "'," +
                        "'" + values.form_gid + "'," +
                       "'" + employee_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Details are added successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while adding details";
                }
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaForm21SubRule1Submit(string employee_gid, form21subrule1 values)
        {
            try
            {


                string uiDateStr = values.dispatch_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string mysqlDOD = uiDate.ToString("yyyy-MM-dd");

                msGetGid = objcmnfunctions.GetMasterGID("HYFR");
                msSQL = " insert into hrm_trn_thalfyearlyform21 ( " +
                        " halfyearlyform21_gid, " +
                        " averageofworkersemployeed_daily, " +
                        " adultsmale_count, " +
                        " adultsfemale_count, " +
                        " adolescentsmale_count, " +
                        " adolescentsfemale_count, " +
                        " childrenmale_count, " +
                        " childrenfemale_count, " +
                        " dispatch_date, " +
                        " registration_number, " +
                        " occupier_address, " +
                        " managingpartner_address, " +
                        " managingpartner_address1, " +
                        " natureof_industry, " +
                        " numberofdaysworked_halfyear, " +
                        " form_gid, " +
                        " created_by, " +
                        " created_date) " +
                        "  values ( " +
                        "'" + msGetGid + "'," +
                        "'" + values.averageofworkersemployeed_daily + "'," +
                        "'" + values.adultsmale_count + "'," +
                        "'" + values.adultsfemale_count + "'," +
                        "'" + values.adolescentsmale_count + "'," +
                        "'" + values.adolescentsfemale_count + "'," +
                        "'" + values.childrenmale_count + "'," +
                        "'" + values.childrenfemale_count + "'," +
                        "'" + mysqlDOD + "'," +
                        "'" + values.registration_number + "', " +
                        "'" + values.occupier_address + "', " +
                        "'" + values.managingpartner_address + "', " +
                        "'" + values.managingpartner_address1 + "', " +
                        "'" + values.natureof_industry + "'," +
                        "'" + values.numberofdaysworked_halfyear + "'," +
                        "'" + values.form_gid + "'," +
                       "'" + employee_gid + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Details are added successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while adding details";
                }

            }

            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetEditForm22SubRule3(string form_gid, string processed_year, MdlHrmForm22 values)
        {
            try
            {
                msSQL = " select a.numberof_workmen,a.numberofworkmen_form1,a.totalworkdays_year,a.nonpermanent_count,a.permanent_count,a.permanentcount_firstjuly,a.reasons,a.remarks, " + " " +
                        " a.company_address,a.typeof_industry,b.form_gid,b.processed_year " +
                        " from hrm_trn_thalfyearlysubrule3 a inner join hrm_mst_tyearlyreturns b on a.form_gid=b.form_gid " + " where a.form_gid= '" + form_gid + "'  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetEditForm22SubRule3>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEditForm22SubRule3
                        {
                            numberof_workmen = dt["numberof_workmen"].ToString(),
                            numberofworkmen_form1 = dt["numberofworkmen_form1"].ToString(),
                            totalworkdays_year = dt["totalworkdays_year"].ToString(),
                            nonpermanent_count = dt["nonpermanent_count"].ToString(),
                            permanent_count = dt["permanent_count"].ToString(),
                            permanentcount_firstjuly = dt["permanentcount_firstjuly"].ToString(),
                            reasons = dt["reasons"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            company_address = dt["company_address"].ToString(),
                            typeof_industry = dt["typeof_industry"].ToString(),
                            form_gid = dt["form_gid"].ToString(),
                            processed_year = dt["processed_year"].ToString(),

                        });
                        values.GetEditForm22SubRule3 = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Half Yearly Summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }

        public void DaGetEditForm22SubRule4(string form_gid, string processed_year, MdlHrmForm22 values)
        {
            try
            {
                msSQL = " select a.serial_number,a.nameaddressofemployee_suspension,a.wagespaid_monthlyemployees,a.departmentanddesignation_last,a.natureof_offence,a.suspension_date, " +
                        " a.commencementenquiry_date, a.completionenquiry_date,a.revocationsuspension_date,a.subsistenceallowence_rate,a.subsistenceallowence_paid, " +
                        " a.dateofissue_finalorder,a.employees_punishment,a.remarks,a.postal_address,a.managingpartner_address,b.processed_year,b.form_gid from hrm_trn_thalfyearlysubrule4 a " +
                        " inner join hrm_mst_tyearlyreturns b on a.form_gid=b.form_gid " +
                        " where a.form_gid= '" + form_gid + "'  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetEditForm22SubRule4>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEditForm22SubRule4
                        {
                            serial_number = dt["serial_number"].ToString(),
                            nameaddressofemployee_suspension = dt["nameaddressofemployee_suspension"].ToString(),
                            wagespaid_monthlyemployees = dt["wagespaid_monthlyemployees"].ToString(),
                            departmentanddesignation_last = dt["departmentanddesignation_last"].ToString(),
                            natureof_offence = dt["natureof_offence"].ToString(),
                            suspension_date = dt["suspension_date"].ToString(),
                            commencementenquiry_date = dt["commencementenquiry_date"].ToString(),
                            completionenquiry_date = dt["completionenquiry_date"].ToString(),
                            revocationsuspension_date = dt["revocationsuspension_date"].ToString(),
                            subsistenceallowence_rate = dt["subsistenceallowence_rate"].ToString(),
                            subsistenceallowence_paid = dt["subsistenceallowence_paid"].ToString(),
                            dateofissue_finalorder = dt["dateofissue_finalorder"].ToString(),
                            employees_punishment = dt["employees_punishment"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            postal_address = dt["postal_address"].ToString(),
                            managingpartner_address = dt["managingpartner_address"].ToString(),


                        });
                        values.GetEditForm22SubRule4 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Half Yearly Summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }

        public void DaGetEditForm22SubRule1(string form_gid, string processed_year, MdlHrmForm22 values)
        {
            try
            {
                msSQL = " select a.averageofworkersemployeed_daily,a.numberofdaysworked_halfyear,a.adultsmale_count,a.adultsfemale_count,a.adolescentsmale_count,a.adolescentsfemale_count,a.childrenmale_count, " +
                        " a.childrenfemale_count,date_format(a.dispatch_date, '%d-%m-%Y') as dispatch_date,a.natureof_industry, " +
                        " a.registration_number,a.occupier_address,a.managingpartner_address,a.managingpartner_address1, b.processed_year,b.form_gid " +
                        " from hrm_trn_thalfyearlyform21 a inner join hrm_mst_tyearlyreturns b on a.form_gid=b.form_gid " +
                        " where a.form_gid= '" + form_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetEditForm22SubRule1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetEditForm22SubRule1
                        {
                            averageofworkersemployeed_daily = dt["averageofworkersemployeed_daily"].ToString(),
                            numberofdaysworked_halfyear = dt["numberofdaysworked_halfyear"].ToString(),
                            adultsmale_count = dt["adultsmale_count"].ToString(),
                            adultsfemale_count = dt["adultsfemale_count"].ToString(),
                            adolescentsmale_count = dt["adolescentsmale_count"].ToString(),
                            adolescentsfemale_count = dt["adolescentsfemale_count"].ToString(),
                            childrenmale_count = dt["childrenmale_count"].ToString(),
                            childrenfemale_count = dt["childrenfemale_count"].ToString(),
                            dispatch_date = dt["dispatch_date"].ToString(),
                            registration_number = dt["registration_number"].ToString(),
                            occupier_address = dt["occupier_address"].ToString(),
                            managingpartner_address = dt["managingpartner_address"].ToString(),
                            managingpartner_address1 = dt["managingpartner_address1"].ToString(),
                            natureof_industry = dt["natureof_industry"].ToString(),




                        });
                        values.GetEditForm22SubRule1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Half Yearly Summary!";
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }

        }

        public void DaDeleteForm22(string form_gid, form_list values)
        {
            msSQL = "  delete from hrm_mst_tyearlyreturns  where form_gid='" + form_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Form Deleted Successfully";
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Deleting Form";
                }
            }


        }

        public void DaUpdateCompanyDetails(string user_gid, companydetails values)
        {
            msGetGid = objcmnfunctions.GetMasterGID("SCMM");
            if (msGetGid == "E")
            {
                objdbconn.CloseConn();
                values.status = false;
                values.message = "Create sequence code SCMM for Task Table";
            }

            msSQL = " update adm_mst_tcompany set " +
                    " companyregistrtion_number='" + values.companyregistrtion_number + "', " +
                    " company_name='" + values.company_name + "'," +
                    " occupier_name='" + values.occupier_name + "', " +
                    " occupier_address='" + values.occupier_address + "', " +
                    " company_address='" + values.company_address + "', " +
                    " company_phone='" + values.company_phone + "', " +
                    " natureof_industry='" + values.natureof_industry + "', " +
                    " updated_by = '" + user_gid + "'," +
                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where company_gid='1' ";

            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Company Details Updated Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Updating Company Details";
            }

        }

        public void DaGetEditCompany(string form_name, string processed_year, MdlHrmForm22 values)
        {
            msSQL = " select companyregistrtion_number,contact_person,contactperson_address,company_name,occupier_name,occupier_address, " +
                    " company_address,company_phone,natureof_industry from adm_mst_tcompany where company_gid='1' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetEditCompany>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetEditCompany
                    {
                        company_name = dt["company_name"].ToString(),
                        contact_person = dt["contact_person"].ToString(),
                        occupier_name = dt["occupier_name"].ToString(),
                        contactperson_address = dt["contactperson_address"].ToString(),
                        occupier_address = dt["occupier_address"].ToString(),
                        companyregistrtion_number = dt["companyregistrtion_number"].ToString(),
                        company_address = dt["company_address"].ToString(),
                        company_phone = dt["company_phone"].ToString(),
                        natureof_industry = dt["natureof_industry"].ToString(),

                    });
                    values.GetEditCompany = getModuleList;
                }
            }
            dt_datatable.Dispose();


        }

        public void DaGetProductSummary(string form_name, string processed_year, MdlHrmForm22 values)
        {
            msSQL = " select productdetails_gid, product_name,capacity,quantity,product_value from hrm_trn_tproductdetails order by productdetails_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetProduct>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetProduct
                    {
                        productdetails_gid = dt["productdetails_gid"].ToString(),
                        product_name = dt["product_name"].ToString(),
                        capacity = dt["capacity"].ToString(),
                        quantity = dt["quantity"].ToString(),
                        product_value = dt["product_value"].ToString(),
                    });
                    values.GetProduct = getModuleList;


                }
            }
            dt_datatable.Dispose();
        }

        public void DaPostProduct(string user_gid, product_list values)
        {
            msGetGid = objcmnfunctions.GetMasterGID("HRPD");
            if (msGetGid == "E")
            {
                values.status = false;
                values.message = "Create sequence code HRPD for Task";
            }



            msSQL = " insert into hrm_trn_tproductdetails( " +
               " productdetails_gid, " +
               " product_name, " +
               " capacity, " +
               " quantity, " +
               " product_value, " +
               " form_gid, " +
               " processed_year, " +
               " created_by, " +
               " created_date) " +
               " values( " +
               " '" + msGetGid + "', " +
               " '" + values.product_name + "', " +
               " '" + values.capacity + "', " +
               " '" + values.quantity + "', " +
               " '" + values.product_value + "', " +
               " '" + values.form_gid + "', " +
               " '" + values.processed_year + "', " +
               "'" + user_gid + "'," +
               "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Product Details Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Adding Product Details";
            }

        }

        public void DaDeleteProduct(string productdetails_gid, product_list values)
        {
            msSQL = "  delete from hrm_trn_tproductdetails where productdetails_gid='" + productdetails_gid + "'  ";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Product Deleted Successfully";
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Deleting Product";
                }
            }


        }

        public void DaLeaveWagesubmit(string user_gid, leavewage_list values)
        {
            msGetGid = objcmnfunctions.GetMasterGID("HELW");

            if (msGetGid == "E")
            {
                values.status = false;
                values.message = "Create sequence code HELW for Task Table";
            }
            msSQL = " insert into hrm_trn_tearnleavewage( " +
                    " earnleavewage_gid, " +
                    " total_employeemen, " +
                    " employee_eligiblemen, " +
                    " no_ofemployeeavailedmen, " +
                    " no_ofemployeedischargemen, " +
                    " employee_lieuearnmen, " +
                    " total_employeewomen, " +
                    " employee_eligiblewomen, " +
                    " no_ofemployeeavailedwomen, " +
                    " no_employeedischargewomen, " +
                    " employee_lieuearnwomen, " +
                    " total_employeeado, " +
                    " employee_eligibleado, " +
                    " no_ofemployeeavailedado, " +
                    " no_employeedischargeado, " +
                    " employee_lieuearnado, " +
                    " form_gid, " +
                    " processed_year, " +
                    " created_by, " +
                    " created_date) " +
                    " values( " +
            " '" + msGetGid + "', " +
            " '" + values.mentotalnoofemp + "', " +
            " '" + values.menearnedleave + "', " +
            " '" + values.mengrantedleave + "', " +
            " '" + values.mendischarged + "', " +
            " '" + values.mennoofempwages + "', " +
             " '" + values.womentotalnoofemp + "', " +
             " '" + values.womenearnedleave + "', " +
             " '" + values.womengrantedleave + "', " +
            " '" + values.womendischarged + "', " +
            " '" + values.woemennoofempwages + "', " +
            " '" + values.adolescentstotalnoofemp + "', " +
            " '" + values.adloscentsearnedleave + "', " +
            " '" + values.adloscentsgrantedleave + "', " +
            " '" + values.adloescentsdischarged + "', " +
            " '" + values.adolescentsnoofempwages + "', " +
            " '" + values.form_gid + "', " +
             " '" + values.processed_year + "', " +
             "'" + user_gid + "'," +
             "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Leave Wage Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Adding Leave Wage";
            }






        }

        public void DaEmployeeManagementsubmit(string user_gid, employeement_list values)
        {
            //msGetGid = objcmnfunctions.GetMasterGID("HRET");

            //if (msGetGid == "E")
            //{
            //    values.status = false;
            //    values.message = "Create sequence code HBOP for Task Table";
            //}

           // msSQL = " insert into hrm_trn_temployeement( " +
           //     " employmentwork_gid, " +
           //     " startdate_mencount, " +
           //     " startdate_womencount, " +
           //     " startdate_adomcount, " +
           //     " startdate_adowcount, " +
           //     " startdate_totalcount, " +
               
           //     " enddate_mencount, " +
           //     " enddate_womencount, " +
           //     " enddate_adomcount, " +
           //     " enddate_adowcount, " +
           //     " enddate_totalcount, " +
               
           //     " no_offactoryworkedmen, " +
           //     " no_offactoryworkedwomen, " +
           //     " no_offactoryworkedadom, " +
           //     " no_offactoryworkedadow, " +
           //     " no_offactoryworkedtotal, " +
               
           //     " mandays_men, " +
           //     " mandays_women, " +
           //     " mandays_adom, " +
           //     " mandays_adow, " +
           //     " mandays_total, " +
               
           //     " normal_men, " +
           //     " normal_women, " +
           //     " normal_adom, " +
           //     " normal_adow, " +
           //     " normal_total, " +
                
           //     " ot_men, " +
           //     " ot_women, " +
           //     " ot_adom, " +
           //     " ot_adow, " +
           //     " ot_total, " +
               
           //     " total_hrsmen, " +
           //     " total_hrswomen, " +
           //     " total_hrsadom, " +
           //     " total_hrsadow, " +
           //     " total_hrs, " +
               
           //     " avgweek_permen, " +
           //     " avgweek_perwomen, " +
           //     " avgweek_peradom, " +
           //     " avgweek_peradow, " +
           //     " avgweek_total, " +
               
           //     " total_amttomen, " +
           //     " total_amttowomen, " +
           //     " total_amttoadom, " +
           //     " total_amttoadow, " +
           //     " total_amt, " +
                
           //     " employed_year, " +
           //     " avg_mens, " +
           //     " avg_womens, " +
           //     " avg_adomens, " +
           //     " avg_adowomens, " +
           //     " avg_total, " +
                  
           //     " form_gid, " +
           //     " processed_year, " +
           //     " created_by, " +
           //     " created_date) " +
           //        " values( " +
           //     " '" + msGetGid + "', " +
           //         " '" + values.menrollstartdate + "', " +
           //         " '" + values.womenrollstartdate + "', " +
           //         " '" + values.adloscentmenrollstartdate + "', " +
           //         " '" + values.adloscentwomenrollstartdate + "', " +
           //   " '" + Values.txtstartyear.Text + Val(txtstartyearwomen.Text) + Val(txtstartyearado.Text) + Val(txtstartyearadow.Text) + "', " +

           //   " '" + values.menrollenddate + "', " +
           // " '" + values.womenrollenddate + "', " +
           //  " '" + values.adloscentmenrollenddate + "', " +
           //    " '" + values.adloscentwomenrollenddate + "', " +
           // " '" + Val(txtendyear.Text) + Val(txtendyearwoemn.Text) + Val(txtendyearado.Text) + Val(txtendyearadow.Text) + "', " +

           // " '" + values.menfactoryworked + "', " +
           // " '" + values.womenfactoryworked + "', " +
           //  " '" + values.adloscentmenfactoryworked + "', " +
           //    " '" + values.adloscentwomenfactoryworked + "', " +
           //   " '" + Val(txtfactoryworkeddays.Text) + Val(txtfactoryworkeddayswomen.Text) + Val(txtfactoryworkeddaysado.Text) + Val(txtfactoryworkeddaysadow.Text) + "', " +

           //   " '" + txtmandays.Text + "', " +
           // " '" + txtmandayswomen.Text + "', " +
           //  " '" + txtmandaysado.Text + "', " +
           //    " '" + txtmandaysadow.Text + "', " +
           //  " '" + Val(txtmandays.Text) + Val(txtmandayswomen.Text) + Val(txtmandaysado.Text) + Val(txtmandaysadow.Text) + "', " +

           //  " '" + values.menworkedyearnormal + "', " +
           // " '" + values.womenworkedyearnormal + "', " +
           //  " '" + values.adloscentmenworkedyearnormal + "', " +
           //    " '" + values.adloscentwomenworkedyearnormal + "', " +
           //  " '" + Val(txtnormal.Text) + Val(txtnormalwomen.Text) + Val(txtnormalado.Text) + Val(txtnormaladow.Text) + "', " +

           //  " '" + values.menworkedyearot + "', " +
           // " '" + values.womenworkedyearot + "', " +
           //  " '" + values.adloscentmenworkedyearot + "', " +
           //    " '" + values.adloscentwomenworkedyearot + "', " +
           //  " '" + Val(txtOt.Text) + Val(txtOtwomen.Text) + Val(txtotado.Text) + Val(txtotadow.Text) + "', " +

           //  " '" + values.menworkedyeartotal + "', " +
           // " '" + values.womenworkedyeartotal + "', " +
           //  " '" + values.adloscentmenworkedyeartotal + "', " +
           //    " '" + values.adloscentwomenworkedyeartotal + "', " +
           //   " '" + Val(txttotal.Text) + Val(txttotalwomen.Text) + Val(txttotalado.Text) + Val(txttotaladow.Text) + "', " +

           //   " '" + values.menworkperweek + "', " +
           // " '" + values.womenworkperweek + "', " +
           //  " '" + values.adloscentmenworkperweek + "', " +
           //    " '" + values.adloscentwomenworkperweek + "', " +
           //    " '" + Val(txtaveragehoursweek.Text) + Val(txtaveragehoursweekwomen.Text) + Val(txtaveragehoursweekado.Text) + Val(txtaveragehoursweekadow.Text) + "', " +

           //    " '" + values.mentotalamount + "', " +
           // " '" + values.womentotalamount + "', " +
           //  " '" + values.adloscentwomentotalamount + "', " +
           //    " '" + txtsalaryadownew.Text + "', " +
           //  " '" + Val(txtsalarynew.Text) + Val(txtsalarywomennew.Text) + Val(txtsalaryadonew.Text) + Val(txtsalaryadownew.Text) + "', " +

           //  " '" + txtempyear.Text + "', " +
           //   " '" + txtmenscount.Text + "', " +
           // " '" + txtwomenscount.Text + "', " +
           //  " '" + txtadomencount.Text + "', " +
           //    " '" + txtadowomenscount.Text + "', " +
           //  " '" + Val(txtmenscount.Text) + Val(txtwomenscount.Text) + Val(txtadomencount.Text) + Val(txtadomencount.Text) + "', " +

           //  " '" + values.form_gid + "', " +
           //" '" + values.processed_year + "', " +
           //     "'" + user_gid + "'," +
           //  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
           // mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


        }

        public void DaConcessionssubmit(string user_gid, concession_list values)
        {
            msGetGid = objcmnfunctions.GetMasterGID("HBOP");

            if (msGetGid == "E")
            {
                values.status = false;
                values.message = "Create sequence code HBOP for Task Table";
            }
            string uiDateStr1 = values.bonus_date;
            DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string mysqlbonus_date = uiDate1.ToString("yyyy-MM-dd");

            string uiDateStr2 = values.exgratia_date;
            DateTime uiDate2 = DateTime.ParseExact(uiDateStr2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string mysqlexgratia_date = uiDate2.ToString("yyyy-MM-dd");

            string uiDateStr3 = values.incentive_date;
            DateTime uiDate3 = DateTime.ParseExact(uiDateStr3, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string mysqlincentive_date = uiDate3.ToString("yyyy-MM-dd");

            msSQL = " insert into hrm_trn_tbonuspaid ( " +
                    " bonus_gid, " +
                    " numberofemployeeseligible_bonus, " +
                    " bonus_percentage, " +
                    " amountof_bonus, " +
                    " amountof_exgratia, " +
                    " amountof_incentive, " +
                    " paymentof_bonus, " +
                    " paymentof_exgratia, " +
                    " paymentof_incentive, " +
                    " form_gid, " +
                    " processed_year, " +
                    " created_by, " +
                    " created_date) " +
                    " values( " +
                    " '" + msGetGid + "', " +
                    " '" + values.empbonus_number + "', " +
                    " '" + values.bonus_declared + "', " +
                    " '" + values.bonus_amount + "', " +
                    " '" + values.exgratia_amount + "', " +
                    " '" + values.incentive_amount + "', " +
                    " '" + mysqlbonus_date + "', " +
                    " '" + mysqlexgratia_date + "', " +
                    " '" + mysqlincentive_date + "', " +
                    " '" + values.form_gid + "', " +
                    " '" + values.processed_year + "', " +
                    "'" + user_gid + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Concession Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error While Adding Concession";
            }

        }

        public void DaGetEditConcession(string form_gid, string processed_year, MdlHrmForm22 values)
        {
            msSQL = " select bonus_gid,numberofemployeeseligible_bonus,bonus_percentage,amountof_bonus,amountof_exgratia,amountof_incentive, " +
                    " date_format(paymentof_bonus, '%d-%m-%Y') as paymentof_bonus,date_format(paymentof_exgratia, '%d-%m-%Y') as paymentof_exgratia,date_format(paymentof_incentive, '%d-%m-%Y') as paymentof_incentive from hrm_trn_tbonuspaid order by bonus_gid desc";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetEditConcession>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetEditConcession
                    {
                        bonus_gid = dt["bonus_gid"].ToString(),
                        numberofemployeeseligible_bonus = dt["numberofemployeeseligible_bonus"].ToString(),
                        bonus_percentage = dt["bonus_percentage"].ToString(),
                        amountof_bonus = dt["amountof_bonus"].ToString(),
                        amountof_exgratia = dt["amountof_exgratia"].ToString(),
                        amountof_incentive = dt["amountof_incentive"].ToString(),
                        paymentof_bonus = dt["paymentof_bonus"].ToString(),
                        paymentof_exgratia = dt["paymentof_exgratia"].ToString(),
                        paymentof_incentive = dt["paymentof_incentive"].ToString(),

                    });
                    values.GetEditConcession = getModuleList;
                }
            }
            dt_datatable.Dispose();


        }

        public void DaGetEditLeaveWage(string form_gid, string processed_year, MdlHrmForm22 values)
        {
            msSQL = " select earnleavewage_gid,total_employeemen,employee_eligiblemen,no_ofemployeeavailedmen,no_ofemployeedischargemen,employee_lieuearnmen, " +
                    " total_employeewomen,employee_eligiblewomen,no_ofemployeeavailedwomen,no_employeedischargewomen,employee_lieuearnwomen," +
                    " total_employeeado,employee_eligibleado,no_ofemployeeavailedado,no_employeedischargeado,employee_lieuearnado from hrm_trn_tearnleavewage order by earnleavewage_gid desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var getModuleList = new List<GetEditLeaveWage>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetEditLeaveWage
                    {
                        earnleavewage_gid = dt["earnleavewage_gid"].ToString(),
                        total_employeemen = dt["total_employeemen"].ToString(),
                        employee_eligiblemen = dt["employee_eligiblemen"].ToString(),
                        no_ofemployeeavailedmen = dt["no_ofemployeeavailedmen"].ToString(),
                        no_ofemployeedischargemen = dt["no_ofemployeedischargemen"].ToString(),
                        employee_lieuearnmen = dt["employee_lieuearnmen"].ToString(),
                        total_employeewomen = dt["total_employeewomen"].ToString(),
                        employee_eligiblewomen = dt["employee_eligiblewomen"].ToString(),
                        no_ofemployeeavailedwomen = dt["no_ofemployeeavailedwomen"].ToString(),
                        no_employeedischargewomen = dt["no_employeedischargewomen"].ToString(),
                        employee_lieuearnwomen = dt["employee_lieuearnwomen"].ToString(),
                        total_employeeado = dt["total_employeeado"].ToString(),
                        employee_eligibleado = dt["employee_eligibleado"].ToString(),
                        no_ofemployeeavailedado = dt["no_ofemployeeavailedado"].ToString(),
                        no_employeedischargeado = dt["no_employeedischargeado"].ToString(),
                        employee_lieuearnado = dt["employee_lieuearnado"].ToString(),

                    });
                    values.GetEditLeaveWage = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }

        public void DaGetEditEmployeeManagement(string form_gid, string processed_year, MdlHrmForm22 values)
        { 
        }

        }
}


