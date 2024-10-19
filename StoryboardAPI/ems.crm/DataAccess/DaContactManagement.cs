using ems.crm.Models;
using ems.utilities.Functions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using static ems.crm.Models.MdlContactManagement;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace ems.crm.DataAccess
{
    public class DaContactManagement
    {
        Fnazurestorage objcmnstorage = new Fnazurestorage();
        HttpPostedFile httpPostedFile;
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        OdbcDataReader objODBCDatareader;
        DataTable dt_levelthree, dt_datatable;
        private string msGetGid, msGetPromoterGID, msGetDirectorGID;
        string msSQL, lscontacttype, lspath;
        string lsmaster_value;
        string msGetGID, msGetcontactrefCode, msGetGstGID, msGetaddressGID;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4;
        Dictionary<string, object> objGetReaderScalar;
        List<Dictionary<string, object>> objGetReaderData;
        //List<string> gst_list;
        //start

        public bool DacontactSummary(contact_list objcontact)
        {
            try
            {
                msSQL = "SELECT a.leadbank_gid,a.remarks,a.lead_status,concat(j.user_firstname,' ',j.user_lastname) as assign_to,concat_ws('/', d.region_name,d.city)AS region_name,e.source_name, CONCAT(COALESCE(a.salutation, ''), ' ', COALESCE(a.first_name, ''), ' ', COALESCE(a.middle_name, ''), ' ', COALESCE(a.last_name, ''), ' ', COALESCE(a.leadbank_name, '')) AS contact_name," +
                        " CONCAT(COALESCE(a.pan_no, ''), ' ', COALESCE(a.corporate_pan_no, '')) AS pan_no, CONCAT(COALESCE(a.first_name, ''), ' ', COALESCE(a.middle_name, ''), ' ', COALESCE(a.last_name, ''), ' ', COALESCE(a.leadbank_name, '')) AS contact," +
                        " a.customer_type, DATE_FORMAT(a.created_date, '%d-%m-%Y') as created_date,concat_ws('-',f.user_firstname,f.user_lastname) as created_by, (SELECT COUNT(*) FROM crm_trn_tleadbank) AS total_count FROM crm_trn_tleadbank a" +
                        " LEFT JOIN adm_mst_tuser c ON c.user_gid = a.created_by" +
                        " left join crm_mst_tregion d on a.leadbank_region=d.region_gid  " +
                        " left join crm_mst_tsource e on a.source_gid=e.source_gid  " +
                        " left join crm_trn_tappointment g on a.leadbank_gid = g.leadbank_gid " +
                        " left join hrm_mst_temployee h on  g.assign_to = h.employee_gid " +
                        " left join adm_mst_tuser j on h.user_gid = j.user_gid " +
                        " left join hrm_mst_temployee k on a.created_by = k.employee_gid " +
                        " left join adm_mst_tuser f on f.user_gid = k.user_gid " +
                        " ORDER BY a.leadbank_gid DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_contact_list = new List<contactsummary>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_contact_list.Add(new contactsummary
                        {
                            contact_gid = dr_datarow["leadbank_gid"].ToString(),
                            contact_type = dr_datarow["customer_type"].ToString(),
                            contact_name = dr_datarow["contact_name"].ToString(),
                            pan_no = dr_datarow["pan_no"].ToString(),
                            created_by = dr_datarow["created_by"].ToString(),
                            created_date = dr_datarow["created_date"].ToString(),
                            source_name = dr_datarow["source_name"].ToString(),
                            region_name = dr_datarow["region_name"].ToString(),
                            lead_status = dr_datarow["lead_status"].ToString(),
                            remarks = dr_datarow["remarks"].ToString(),
                            assign_to = dr_datarow["assign_to"].ToString(),
                            total_count = dr_datarow["total_count"].ToString()
                        });
                    }
                    objcontact.contactsummary = get_contact_list;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "CMN");
                ex.StackTrace.ToString();
                objcontact.status = false;
                return false;
            }
        }
        public bool DaContactIndividualSummary(contact_list objcontact)
        {
            try
            {
                msSQL = "SELECT a.leadbank_gid,a.remarks,a.lead_status,concat(j.user_firstname,' ',j.user_lastname) as assign_to,concat_ws('/', d.region_name,d.city)AS region_name,e.source_name,COALESCE(CONCAT(a.salutation, ' ', a.first_name, ' ', a.middle_name, ' ', a.last_name), a.leadbank_name) AS contact_name, a.pan_no, a.customer_type," +
                        " DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date, concat_ws('-',f.user_firstname,f.user_lastname) as created_by, " +
                        "(SELECT COUNT(*) FROM crm_trn_tleadbank WHERE customer_type = 'Individual') AS Individual_count FROM crm_trn_tleadbank a " +
                        "LEFT JOIN adm_mst_tuser c ON c.user_gid = a.created_by" +
                         " left join crm_mst_tregion d on a.leadbank_region=d.region_gid  " +
                        " left join crm_mst_tsource e on a.source_gid=e.source_gid  " +
                        " left join crm_trn_tappointment g on a.leadbank_gid = g.leadbank_gid " +
                        " left join hrm_mst_temployee h on  g.assign_to = h.employee_gid " +
                        " left join adm_mst_tuser j on h.user_gid = j.user_gid " +
                        " left join hrm_mst_temployee k on a.created_by = k.employee_gid " +
                        " left join adm_mst_tuser f on f.user_gid = k.user_gid " +
                        " WHERE a.customer_type = 'Individual' ORDER BY a.leadbank_gid DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_contact_list = new List<contactIndividualsummary>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_contact_list.Add(new contactIndividualsummary
                        {
                            contact_gid = dr_datarow["leadbank_gid"].ToString(),
                            contact_name = dr_datarow["contact_name"].ToString(),
                            pan_no = dr_datarow["pan_no"].ToString(),
                            created_by = dr_datarow["created_by"].ToString(),
                            created_date = dr_datarow["created_date"].ToString(),
                            source_name = dr_datarow["source_name"].ToString(),
                            region_name = dr_datarow["region_name"].ToString(),
                            lead_status = dr_datarow["lead_status"].ToString(),
                            remarks = dr_datarow["remarks"].ToString(),
                            assign_to = dr_datarow["assign_to"].ToString(),
                            Individual_count = dr_datarow["Individual_count"].ToString()
                        });
                    }
                    objcontact.contactIndividualsummary = get_contact_list;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "CMN");
                ex.StackTrace.ToString();
                objcontact.status = false;
                return false;
            }
        }
        public bool DaContactCorporateSummary(contact_list objcontact)
        {
            try
            {
                msSQL = " SELECT  a.leadbank_gid,a.remarks,a.lead_status,concat(j.user_firstname,' ',j.user_lastname) as assign_to,concat_ws('/', d.region_name,d.city)AS region_name,e.source_name, a.leadbank_name, a.corporate_pan_no, a.customer_type, DATE_FORMAT(a.created_date, '%d-%m-%Y') AS created_date, " +
                        " concat_ws('-',f.user_firstname,f.user_lastname) as created_by, (SELECT COUNT(*) FROM crm_trn_tleadbank WHERE customer_type = 'Corporate') AS Corporate_count" +
                        " FROM crm_trn_tleadbank a" +
                        " LEFT JOIN adm_mst_tuser c ON c.user_gid = a.created_by" +
                        " left join crm_mst_tregion d on a.leadbank_region=d.region_gid  " +
                        " left join crm_mst_tsource e on a.source_gid=e.source_gid  " +
                        " left join crm_trn_tappointment g on a.leadbank_gid = g.leadbank_gid " +
                        " left join hrm_mst_temployee h on  g.assign_to = h.employee_gid " +
                        " left join adm_mst_tuser j on h.user_gid = j.user_gid " +
                        " left join hrm_mst_temployee k on a.created_by = k.employee_gid " +
                        " left join adm_mst_tuser f on f.user_gid = k.user_gid " +
                        " WHERE a.customer_type = 'Corporate' ORDER BY a.leadbank_gid DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_contact_list = new List<contactCorporatesummary>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_contact_list.Add(new contactCorporatesummary
                        {
                            contact_gid = dr_datarow["leadbank_gid"].ToString(),
                            lgltrade_name = dr_datarow["leadbank_name"].ToString(),
                            corporate_pan_no = dr_datarow["corporate_pan_no"].ToString(),
                            created_by = dr_datarow["created_by"].ToString(),
                            created_date = dr_datarow["created_date"].ToString(),
                            source_name = dr_datarow["source_name"].ToString(),
                            region_name = dr_datarow["region_name"].ToString(),
                            lead_status = dr_datarow["lead_status"].ToString(),
                            remarks = dr_datarow["remarks"].ToString(),
                            assign_to = dr_datarow["assign_to"].ToString(),
                            Corporate_count = dr_datarow["Corporate_count"].ToString()
                        });
                    }
                    objcontact.contactCorporatesummary = get_contact_list;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "CMN");
                ex.StackTrace.ToString();
                objcontact.status = false;
                return false;
            }
        }

        public bool DaContactAdd(contact values, string employee_gid)
        {
            try
            {
                if (values.contact_type == "Individual" && values.contact_type != null)
                {
                    msSQL = " select customer_type from crm_mst_tcustomertype Where customer_type='" + values.contact_type + "'";
                    string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);

                    string leadbank_name = (values.first_name ?? "") +
                                           (!string.IsNullOrEmpty(values.middle_name) ? " " + values.middle_name : "") +
                                           (!string.IsNullOrEmpty(values.last_name) ? " " + values.last_name : "");
                    msGetGID = objcmnfunctions.GetMasterGID("BLBP");
                    msSQL = "insert into crm_trn_tleadbank(" +
                          " leadbank_gid," +
                          " salutation," +
                          " referred_by," +
                          " salutation_gid," +
                          " leadbank_name," +
                          " first_name," +
                          " middle_name," +
                          " last_name," +
                          " physicalstatus_gid," +
                          " physicalstatus_name," +
                          " leadbank_address1," +
                          " leadbank_address2," +
                          " source_gid," +
                          " leadbank_region," +
                          " leadbank_city," +
                          " leadbank_state," +
                          " leadbank_pin," +
                          " leadbank_country," +
                          " latitude," +
                          " longitude," +
                          " tempaddress1," +
                          " tempaddress2," +
                          " tempcity," +
                          " tempstate," +
                          " temppostal_code," +
                          " tempcountry_name," +
                          " tempcountry_gid," +
                          " templatitude," +
                          " templongitude," +
                          " customer_type," +
                          " customertype_gid," +
                          " age," +
                          " individual_dob," +
                          " gender_name," +
                           " gender_gid," +
                          " aadhar_no," +
                          " pan_no," +
                          " maritalstatus_name," +
                          " maritalstatus_gid," +
                          " designation_gid," +
                          " designation_name," +
                          " father_firstname," +
                          " father_lastname," +
                          " fathercontact_no," +
                          " mother_firstname," +
                          " mother_lastname," +
                          " mothercontact_no," +
                          " spouse_firstname," +
                          " spouse_lastname," +
                          " spousecontact_no," +
                          " educationalqualification_name," +
                          " main_occupation," +
                          " annual_income," +
                          " monthly_income," +
                           " incometype_name," +
                           " incometype_gid," +
                          " created_by," +
                          " created_date) " +
                          " values(" +
                          "'" + msGetGID + "'," +
                          "'" + values.salutation + "'," +
                          "'" + values.referred_by + "'," +
                          "'" + values.salutation_gid + "'," +
                          "'" + leadbank_name.Replace("'", "\\\'") + "'," +
                          "'" + values.first_name.Replace("'", "\\\'") + "'," +
                          "'" + values.middle_name + "'," +
                          "'" + values.last_name + "'," +
                          "'" + values.physicalstatus_gid + "'," +
                          "'" + values.physicalstatus_name + "'," +
                          "'" + values.address1 + "'," +
                          "'" + values.address2 + "'," +
                          "'" + values.source_gid + "'," +
                          "'" + values.region_gid + "'," +
                          "'" + values.city + "'," +
                          "'" + values.state + "'," +
                          "'" + values.postal_code + "'," +
                          "'" + values.country_name + "'," +
                          "'" + values.latitude + "'," +
                          "'" + values.longitude + "'," +
                          "'" + values.tempaddress1 + "'," +
                          "'" + values.tempaddress2 + "'," +
                          "'" + values.tempcity + "'," +
                          "'" + values.tempstate + "'," +
                          "'" + values.temppostal_code + "'," +
                          "'" + values.tempcountry_name + "'," +
                          "'" + values.tempcountry_gid + "'," +
                          "'" + values.templatitude + "'," +
                          "'" + values.templongitude + "'," +
                          "'" + values.contact_type + "'," +
                          "'" + lscustomer_type + "'," +
                          "'" + values.age + "',";
                        if (values.dob_date == "" || values.dob_date == null || values.dob_date == "undefined")
                        {
                          msSQL += "null,";

                        }
                        else
                        {
                         msSQL += "'" + values.dob_date + "',";
                        }
                 msSQL += "'" + values.gender + "'," +
                          "'" + values.gender_gid + "'," +
                          "'" + values.aadhar_no + "'," +
                          "'" + values.pan_no + "'," +
                          "'" + values.marital_status + "'," +
                          "'" + values.marital_status_gid + "'," +
                          "'" + values.designation_gid + "'," +
                          "'" + values.designation_name + "'," +
                          "'" + values.father_first_name + "'," +
                          "'" + values.father_last_name + "'," +
                          "'" + values.father_contact_refno + "'," +
                          "'" + values.mother_first_name + "'," +
                          "'" + values.mother_last_name + "'," +
                          "'" + values.mother_contact_refno + "'," +
                          "'" + values.spouse_first_name + "'," +
                          "'" + values.spouse_last_name + "'," +
                          "'" + values.spouse_contact_refno + "'," +
                          "'" + values.education_qualification + "'," +
                          "'" + values.main_occupation + "'," +
                          "'" + values.annual_income + "'," +
                          "'" + values.monthly_income + "'," +
                          "'" + values.income_type + "'," +
                          "'" + values.income_type_gid + "'," +
                          "'" + employee_gid + "'," +
                           " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    for (int i = 0; i < values.mobile_list.Count; i++)
                    {
                        msGetGstGID = objcmnfunctions.GetMasterGID("CO2M");
                        msSQL = " insert into crm_mst_tcontact2mobileno(" +
                                  " contact2mobileno_gid," +
                                  " leadbank_gid," +
                                  " mobile_no," +
                                  " primary_status)" +
                                  " values( " +
                                  "'" + msGetGstGID + "'," +
                                  "'" + msGetGID + "'," +
                                  "'" + values.mobile_list[i].mobile_no + "'," +
                                  "'" + values.mobile_list[i].primary_status + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                        for (int j = 0; j < values.email_list.Count; j++)
                        {
                            msGetGstGID = objcmnfunctions.GetMasterGID("CO2E");
                            msSQL = " insert into crm_mst_tcontact2email(" +
                                      " contact2email_gid," +
                                      " leadbank_gid," +
                                      " email_address," +
                                      " primary_status)" +
                                      " values( " +
                                      "'" + msGetGstGID + "'," +
                                      "'" + msGetGID + "'," +
                                      "'" + values.email_list[j].email_address + "'," +
                                      "'" + values.email_list[j].primary_status + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                        if (mnResult != 0)
                        {
                            values.message = "Contact details Added Successfully";
                            values.status = true;
                            values.contact_gid = msGetGID;
                            return true;
                        }
                        else
                        {
                            values.message = "Error While Adding Contact details";
                            values.status = false;
                            return false;
                        }
                    

                }
                else
                {
                    msSQL = " select customer_type from crm_mst_tcustomertype Where customer_type='" + values.contact_type + "'";
                    string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);

                    msGetGID = objcmnfunctions.GetMasterGID("BLBP");

                    msSQL = "insert into crm_trn_tleadbank(" +
                          " leadbank_gid," +
                          " referred_by," +
                          " corporate_pan_no," +
                          " leadbank_name," +
                          " lei," +
                          " cin," +
                          " cin_date," +
                          " constitution," +
                          " source_gid," +
                          " leadbank_region," +
                          " constitution_gid," +
                          " businesss_vintage," +
                          " businessstart_date," +
                          " tan," +
                          " tan_state," +
                          " kin," +
                          " udhayam_registration," +
                          " category_aml," +
                          " amlcategory_gid," +
                          " category_business," +
                          " businesscategory_gid," +
                          " last_year_turnover," +
                          " customer_type," +
                          " customertype_gid," +
                          " created_by," +
                          " created_date) " +
                          " values(" +
                          "'" + msGetGID + "'," +
                          "'" + values.referred_by + "'," +
                          "'" + values.corporate_pan_no + "'," +
                          "'" + values.lgltrade_name.Replace("'", "\\\'") + "'," +
                          "'" + values.lei + "'," +
                          "'" + values.cin + "',";
                         if (values.cin_date == "" || values.cin_date == null || values.cin_date == "undefined")
                         {
                          msSQL += "null,";

                         }
                         else
                         {
                         msSQL += "'" + values.cin_date + "',";
                         }
                    msSQL += "'" + values.constitution + "'," +
                         "'" + values.source_gid + "'," +
                         "'" + values.region_gid + "'," +
                             "'" + values.constitution_gid + "'," +
                             "'" + values.businesss_vintage + "',";
                         if (values.businessstart_date == "" || values.businessstart_date == null || values.businessstart_date == "undefined")
                         {
                           msSQL += "null,";

                         }
                        else
                        {
                         msSQL += "'" + values.businessstart_date + "',";
                        }
                 msSQL += "'" + values.tan + "'," +
                          "'" + values.tan_state + "'," +
                          "'" + values.kin + "'," +
                          "'" + values.udhayam_registration + "'," +
                          "'" + values.category_aml + "'," +
                           "'" + values.amlcategory_gid + "'," +
                          "'" + values.category_business + "'," +
                          "'" + values.businesscategory_gid + "'," +
                          "'" + values.last_year_turnover + "'," +
                          "'" + values.contact_type + "'," +
                          "'" + lscustomer_type + "'," +
                          "'" + employee_gid + "'," +
                         " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                    for (int i = 0; i < values.gst_list.Count; i++)
                    {
                        msGetGstGID = objcmnfunctions.GetMasterGID("COGT");
                        msSQL = " insert into crm_mst_tcontact2gst(" +
                                  " contact2gst_gid," +
                                  " leadbank_gid," +
                                  " gst_location," +
                                  " gst_no," +
                                  " gst_state)" +
                                  " values( " +
                                  "'" + msGetGstGID + "'," +
                                  "'" + msGetGID + "'," +
                                  "'" + values.gst_list[i].gst_location + "'," +
                                  "'" + values.gst_list[i].gst_no + "'," +
                                  "'" + values.gst_list[i].gst_state + "')";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }


                    for (int i = 0; i < values.address_list.Count; i++)
                    {
                        msGetaddressGID = objcmnfunctions.GetMasterGID("COAD");
                        msSQL = " insert into crm_mst_tcontact2address(" +
                                  " contact2address_gid," +
                                  " leadbank_gid," +
                                  " addresstype_gid," +
                                  " address_type," +
                                  " address1," +
                                  " address2," +
                                  " city," +
                                  " state," +
                                  " postal_code," +
                                  " country_name," +
                                  " country_gid," +
                                  " latitude," +
                                  " longitude," +
                                  " email_address," +
                                  " mobile_no," +
                                  " primary_status)" +
                                  " values( " +
                                  "'" + msGetaddressGID + "'," +
                                  "'" + msGetGID + "'," +
                                  "'" + values.address_list[i].addresstype_gid + "'," +
                                  "'" + values.address_list[i].addresstype + "'," +
                                   "'" + values.address_list[i].address1 + "'," +
                                  "'" + values.address_list[i].address2 + "'," +
                                  "'" + values.address_list[i].city + "'," +
                                   "'" + values.address_list[i].state + "'," +
                                  "'" + values.address_list[i].postal_code + "'," +
                                  "'" + values.address_list[i].country_name + "'," +
                                  "'" + values.address_list[i].country_gid + "'," +
                                   "'" + values.address_list[i].latitude + "'," +
                                  "'" + values.address_list[i].longitude + "'," +
                                  "'" + values.address_list[i].email_address + "'," +
                                  "'" + values.address_list[i].mobile_no + "'," +
                                  "'" + values.address_list[i].primary_status + "')";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    for (int i = 0; i < values.promoter_list.Count; i++)
                    {
                        msGetPromoterGID = objcmnfunctions.GetMasterGID("COPR");

                        msSQL = "insert into crm_mst_tcontact2promotor(" +
                              "contact2promotor_gid," +
                              " leadbank_gid," +
                              " pan_no," +
                              " aadhar_no," +
                              " salutation," +
                              " salutation_gid," +
                              " first_name," +
                              " middle_name," +
                              " last_name," +
                              " designation," +
                              " email," +
                              " mobile," +
                              " created_by," +
                              " created_date) " +
                              " values(" +
                              "'" + msGetPromoterGID + "'," +
                              "'" + msGetGID + "'," +
                              "'" + values.promoter_list[i].pan_no + "'," +
                              "'" + values.promoter_list[i].aadhar_no + "'," +
                              "'" + values.promoter_list[i].salutation + "'," +
                              "'" + values.promoter_list[i].salutation_gid + "'," +
                              "'" + values.promoter_list[i].first_name + "'," +
                              "'" + values.promoter_list[i].middle_name + "'," +
                              "'" + values.promoter_list[i].last_name + "'," +
                              "'" + values.promoter_list[i].designation + "'," +
                              "'" + values.promoter_list[i].email + "'," +
                              "'" + values.promoter_list[i].mobile + "'," +
                              "'" + employee_gid + "'," +
                             " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    for (int i = 0; i < values.director_list.Count; i++)
                    {
                        msGetDirectorGID = objcmnfunctions.GetMasterGID("COGR");

                        msSQL = "insert into crm_mst_tcontact2director(" +
                              "contact2director_gid," +
                              " leadbank_gid," +
                              " pan_no," +
                              " aadhar_no," +
                              " salutation," +
                              " salutation_gid," +
                              " first_name," +
                              " middle_name," +
                              " last_name," +
                              " designation_name," +
                              " email_address," +
                              " mobile_no," +
                              " created_by," +
                              " created_date) " +
                              " values(" +
                              "'" + msGetDirectorGID + "'," +
                              "'" + msGetGID + "'," +
                              "'" + values.director_list[i].pan_no + "'," +
                              "'" + values.director_list[i].aadhar_no + "'," +
                              "'" + values.director_list[i].salutation + "'," +
                              "'" + values.director_list[i].salutation_gid + "'," +
                              "'" + values.director_list[i].first_name + "'," +
                              "'" + values.director_list[i].middle_name + "'," +
                              "'" + values.director_list[i].last_name + "'," +
                              "'" + values.director_list[i].designation + "'," +
                              "'" + values.director_list[i].email + "'," +
                              "'" + values.director_list[i].mobile + "'," +
                              "'" + employee_gid + "'," +
                             " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }

                if (mnResult1 != 0)
                {
                    values.status = true;
                    values.contact_gid = msGetGID;
                    values.message = "Contact details Added Successfully";
                    return true;
                }
                else
                {
                    values.message = "Error While Adding Contact details";
                    values.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "CMN");
                ex.StackTrace.ToString();
                values.status = false;
                return false;

            }
        }




        public void DaContactEditView(contact objcontact, string leadbank_gid)
        {

            try
            {

                msSQL = "select customer_type from crm_trn_tleadbank where leadbank_gid ='" + leadbank_gid + "'";
                lscontacttype = objdbconn.GetExecuteScalar(msSQL);


                if (lscontacttype == "Individual" && lscontacttype != null)
                {
                    msSQL = "SELECT " +
                            "COALESCE(CONCAT(a.salutation, ' ', a.first_name, ' ', a.middle_name, ' ', a.last_name), a.leadbank_name) AS contact_name, " +
                            "a.contact_ref_no, " +
                            "a.leadbank_gid, " +
                            "a.customer_type, " +
                            "a.pan_no, " +
                            "a.aadhar_no, " +
                            "a.individual_dob, " +
                            "a.age, " +
                            "a.gender_name, " +
                            "a.gender_gid, " +
                            "a.designation_name, " +
                            "a.designation_gid, " +
                            "a.maritalstatus_name, " +
                            "a.maritalstatus_gid, " +
                            "a.physicalstatus_name, " +
                            "a.physicalstatus_gid, " +
                            "a.leadbank_address1, " +
                            "a.leadbank_address2,a.leadbank_region,a.source_gid,ref.leadbank_name as referred_by,concat_ws('/', b.region_name,b.city)AS region_name,c.source_name, " +
                            "a.leadbank_city, " +
                            "a.leadbank_state, " +
                            "a.leadbank_pin, " +
                            "a.leadbank_country, " +
                            "a.leadbankcountry_gid, " +
                            "a.latitude, " +
                            "a.longitude, " +
                            "a.tempaddress1, " +
                            "a.tempaddress2, " +
                            "a.tempcity, " +
                            "a.tempstate, " +
                            "a.temppostal_code, " +
                            "a.tempcountry_name, " +
                            "a.tempcountry_gid, " +
                            "a.templatitude, " +
                            "a.templongitude, " +
                            "CONCAT(a.father_firstname, ' ', a.father_lastname) AS father_name, " +
                            "a.father_firstname, " +
                            "a.father_lastname, " +
                            "a.fathercontact_no, " +
                            "CONCAT(a.mother_firstname, ' ', a.mother_lastname) AS mother_name, " +
                            "a.mother_firstname, " +
                            "a.mother_lastname, " +
                            "a.mothercontact_no, " +
                            "CONCAT(a.spouse_firstname, ' ', a.spouse_lastname) AS spouse_name, " +
                            "a.spouse_firstname, " +
                            "a.spouse_lastname, " +
                            "a.spousecontact_no, " +
                            "a.educationalqualification_name, " +
                            "a.main_occupation, " +
                            "a.annual_income, " +
                            "a.monthly_income, " +
                            "a.incometype_name, " +
                            "a.incometype_gid, " +
                            "a.salutation, " +
                            "a.salutation_gid, " +
                            "a.first_name, " +
                            "a.middle_name, " +
                            "a.last_name " +
                            "FROM crm_trn_tleadbank a" +
                             " left join crm_mst_tregion b on b.region_gid =  a.leadbank_region" +
                            " left join crm_mst_tsource c on c.source_gid =  a.source_gid " +
                            " left join crm_trn_tleadbank ref on a.referred_by = ref.leadbank_gid " +
                            " where a.leadbank_gid='" + leadbank_gid + "'";
                    objODBCDatareader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDatareader.HasRows)
                    {
                        objcontact.contact_name = objODBCDatareader["contact_name"].ToString();
                        objcontact.contact_ref_no = objODBCDatareader["contact_ref_no"].ToString();
                        objcontact.contact_gid = objODBCDatareader["leadbank_gid"].ToString();
                        objcontact.contact_type = objODBCDatareader["customer_type"].ToString();
                        objcontact.pan_no = objODBCDatareader["pan_no"].ToString();
                        objcontact.aadhar_no = objODBCDatareader["aadhar_no"].ToString();
                        objcontact.individual_dob = objODBCDatareader["individual_dob"].ToString();
                        objcontact.age = objODBCDatareader["age"] != DBNull.Value ? Convert.ToInt32(objODBCDatareader["age"]) : 0;
                        objcontact.gender_name = objODBCDatareader["gender_name"].ToString();
                        objcontact.gender_gid = objODBCDatareader["gender_gid"].ToString();
                        objcontact.designation_name = objODBCDatareader["designation_name"].ToString();
                        objcontact.designation_gid = objODBCDatareader["designation_gid"].ToString();
                        objcontact.maritalstatus_name = objODBCDatareader["maritalstatus_name"].ToString();
                        objcontact.maritalstatus_gid = objODBCDatareader["maritalstatus_gid"].ToString();
                        objcontact.physicalstatus_name = objODBCDatareader["physicalstatus_name"].ToString();
                        objcontact.physicalstatus_gid = objODBCDatareader["physicalstatus_gid"].ToString();
                        objcontact.address1 = objODBCDatareader["leadbank_address1"].ToString();
                        objcontact.address2 = objODBCDatareader["leadbank_address2"].ToString();
                        objcontact.city = objODBCDatareader["leadbank_city"].ToString();
                        objcontact.state = objODBCDatareader["leadbank_state"].ToString();
                        objcontact.postal_code = objODBCDatareader["leadbank_pin"].ToString();
                        objcontact.country_name = objODBCDatareader["leadbank_country"].ToString();
                        objcontact.country_gid = objODBCDatareader["leadbankcountry_gid"].ToString();
                        objcontact.latitude = objODBCDatareader["latitude"].ToString();
                        objcontact.longitude = objODBCDatareader["longitude"].ToString();
                        objcontact.tempaddress1 = objODBCDatareader["tempaddress1"].ToString();
                        objcontact.tempaddress2 = objODBCDatareader["tempaddress2"].ToString();
                        objcontact.tempcity = objODBCDatareader["tempcity"].ToString();
                        objcontact.tempstate = objODBCDatareader["tempstate"].ToString();
                        objcontact.temppostal_code = objODBCDatareader["temppostal_code"].ToString();
                        objcontact.tempcountry_name = objODBCDatareader["tempcountry_name"].ToString();
                        objcontact.tempcountry_gid = objODBCDatareader["tempcountry_gid"].ToString();
                        objcontact.templatitude = objODBCDatareader["templatitude"].ToString();
                        objcontact.templongitude = objODBCDatareader["templongitude"].ToString();
                        objcontact.father_name = objODBCDatareader["father_name"].ToString();
                        objcontact.father_firstname = objODBCDatareader["father_firstname"].ToString();
                        objcontact.father_lastname = objODBCDatareader["father_lastname"].ToString();
                        objcontact.fathercontact_no = objODBCDatareader["fathercontact_no"].ToString();
                        objcontact.mother_name = objODBCDatareader["mother_name"].ToString();
                        objcontact.mother_firstname = objODBCDatareader["mother_firstname"].ToString();
                        objcontact.mother_lastname = objODBCDatareader["mother_lastname"].ToString();
                        objcontact.mothercontact_no = objODBCDatareader["mothercontact_no"].ToString();
                        objcontact.spouse_name = objODBCDatareader["spouse_name"].ToString();
                        objcontact.spouse_firstname = objODBCDatareader["spouse_firstname"].ToString();
                        objcontact.spouse_lastname = objODBCDatareader["spouse_lastname"].ToString();
                        objcontact.spousecontact_no = objODBCDatareader["spousecontact_no"].ToString();
                        objcontact.educationalqualification_name = objODBCDatareader["educationalqualification_name"].ToString();
                        objcontact.main_occupation = objODBCDatareader["main_occupation"].ToString();
                        objcontact.annual_income = objODBCDatareader["annual_income"].ToString();
                        objcontact.monthly_income = objODBCDatareader["monthly_income"].ToString();
                        objcontact.incometype_name = objODBCDatareader["incometype_name"].ToString();
                        objcontact.incometype_gid = objODBCDatareader["incometype_gid"].ToString();
                        objcontact.salutation = objODBCDatareader["salutation"].ToString();
                        objcontact.salutation_gid = objODBCDatareader["salutation_gid"].ToString();
                        objcontact.first_name = objODBCDatareader["first_name"].ToString();
                        objcontact.middle_name = objODBCDatareader["middle_name"].ToString();
                        objcontact.last_name = objODBCDatareader["last_name"].ToString();
                        objcontact.region_name = objODBCDatareader["region_name"].ToString();
                        objcontact.source_name = objODBCDatareader["source_name"].ToString();
                        objcontact.referred_by = objODBCDatareader["referred_by"].ToString();


                    }
                    msSQL = "select leadbank_gid,contact2mobileno_gid,mobile_no,primary_status from crm_mst_tcontact2mobileno" +
                            " where leadbank_gid='" + leadbank_gid + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var mobile_list = new List<mobile>();
                    if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dr_datarow in dt_datatable.Rows)
                        {
                            mobile_list.Add(new mobile
                            {
                                contact2mobileno_gid = dr_datarow["contact2mobileno_gid"].ToString(),
                                contact_gid = dr_datarow["leadbank_gid"].ToString(),
                                mobile_no = dr_datarow["mobile_no"].ToString(),
                                primary_status = dr_datarow["primary_status"].ToString(),
                            });
                        }
                    }

                    objcontact.mobile_list = mobile_list;
                    dt_datatable.Dispose();

                    msSQL = "select leadbank_gid,contact2email_gid,email_address,primary_status from crm_mst_tcontact2email" +
                            " where leadbank_gid='" + leadbank_gid + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var email_list = new List<email>();
                    if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dr_datarow in dt_datatable.Rows)
                        {
                            email_list.Add(new email
                            {
                                contact2email_gid = dr_datarow["contact2email_gid"].ToString(),
                                contact_gid = dr_datarow["leadbank_gid"].ToString(),
                                email_address = dr_datarow["email_address"].ToString(),
                                primary_status = dr_datarow["primary_status"].ToString(),
                            });
                        }
                    }

                    objcontact.email_list = email_list;
                    dt_datatable.Dispose();

                    msSQL = "select leadbank_gid,document_name,filename,document_path from crm_trn_tfiles" +
                            " where leadbank_gid ='" + leadbank_gid + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var DocumentList = new List<GeneralDocumentList>();
                    if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dr_datarow in dt_datatable.Rows)
                        {
                            DocumentList.Add(new GeneralDocumentList
                            {
                                contact_gid = dr_datarow["leadbank_gid"].ToString(),
                                document_name = dr_datarow["document_name"].ToString(),
                                //path = objcmnstorage.EncryptData(dr_datarow["filepath"].ToString()),
                                file_name = dr_datarow["filename"].ToString(),
                                path = dr_datarow["document_path"].ToString(),
                            });
                        }
                    }

                    objcontact.DocumentList = DocumentList;
                    dt_datatable.Dispose();

                }
                else
                {

                    msSQL = " select a.leadbank_name,a.corporate_pan_no,a.lei,a.cin,a.cin_date,a.constitution,a.leadbank_region,a.source_gid,ref.leadbank_name as referred_by,concat_ws('/', b.region_name,b.city)AS region_name,c.source_name, " +
                            " a.businessstart_date,a.businesss_vintage,a.tan,a.tan_state,a.kin,a.udhayam_registration, " +
                            " a.category_aml,a.category_business,a.last_year_turnover,a.contact_ref_no,a.customer_type,a.constitution_gid,a.amlcategory_gid,a.businesscategory_gid " +
                            " FROM crm_trn_tleadbank a" +
                            " left join crm_mst_tregion b on b.region_gid =  a.leadbank_region" +
                            " left join crm_mst_tsource c on c.source_gid =  a.source_gid" +
                            " left join crm_trn_tleadbank ref on a.referred_by = ref.leadbank_gid " +
                            " where a.leadbank_gid ='" + leadbank_gid + "'";

                    objODBCDatareader = objdbconn.GetDataReader(msSQL);
                    if (objODBCDatareader.HasRows)
                    {
                        objcontact.lgltrade_name = objODBCDatareader["leadbank_name"].ToString();
                        objcontact.referred_by = objODBCDatareader["referred_by"].ToString();
                        objcontact.leadbank_region = objODBCDatareader["leadbank_region"].ToString();
                        objcontact.source_gid = objODBCDatareader["source_gid"].ToString();
                        objcontact.corporate_pan_no = objODBCDatareader["corporate_pan_no"].ToString();
                        objcontact.lei = objODBCDatareader["lei"].ToString();
                        objcontact.cin = objODBCDatareader["cin"].ToString();
                        objcontact.cin_date = objODBCDatareader["cin_date"].ToString();
                        objcontact.constitution = objODBCDatareader["constitution"].ToString();
                        objcontact.constitution_gid = objODBCDatareader["constitution_gid"].ToString();
                        objcontact.businessstart_date = objODBCDatareader["businessstart_date"].ToString();
                        objcontact.businesss_vintage = objODBCDatareader["businesss_vintage"].ToString();
                        objcontact.tan = objODBCDatareader["tan"].ToString();
                        objcontact.tan_state = objODBCDatareader["tan_state"].ToString();
                        objcontact.kin = objODBCDatareader["kin"].ToString();
                        objcontact.udhayam_registration = objODBCDatareader["udhayam_registration"].ToString();
                        objcontact.category_aml = objODBCDatareader["category_aml"].ToString();
                        objcontact.amlcategory_gid = objODBCDatareader["amlcategory_gid"].ToString();
                        objcontact.category_business = objODBCDatareader["category_business"].ToString();
                        objcontact.businesscategory_gid = objODBCDatareader["businesscategory_gid"].ToString();
                        objcontact.last_year_turnover = objODBCDatareader["last_year_turnover"].ToString();
                        objcontact.contact_ref_no = objODBCDatareader["contact_ref_no"].ToString();
                        objcontact.contact_type = objODBCDatareader["customer_type"].ToString();
                        objcontact.region_name = objODBCDatareader["region_name"].ToString();
                        objcontact.source_name = objODBCDatareader["source_name"].ToString();

                    }


                    msSQL = "select leadbank_gid,contact2gst_gid,gst_location,gst_no,gst_state from crm_mst_tcontact2gst" +
                            " where leadbank_gid='" + leadbank_gid + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var gst_list = new List<gst>();
                    if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dr_datarow in dt_datatable.Rows)
                        {
                            gst_list.Add(new gst
                            {
                                contact2gst_gid = dr_datarow["contact2gst_gid"].ToString(),
                                contact_gid = dr_datarow["leadbank_gid"].ToString(),
                                gst_location = dr_datarow["gst_location"].ToString(),
                                gst_no = dr_datarow["gst_no"].ToString(),
                                gst_state = dr_datarow["gst_state"].ToString(),
                            });
                        }
                    }

                    objcontact.gst_list = gst_list;
                    dt_datatable.Dispose();



                    msSQL = "select contact2address_gid,address_type,address1,mobile_no,email_address, " +
                            "address2,city,state,postal_code,country_name,latitude,longitude,primary_status " +
                    "from crm_mst_tcontact2address" +
                            " where leadbank_gid='" + leadbank_gid + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var address_list = new List<address>();
                    if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dr_datarow in dt_datatable.Rows)
                        {
                            address_list.Add(new address
                            {
                                contact2address_gid = dr_datarow["contact2address_gid"].ToString(),
                                addresstype = dr_datarow["address_type"].ToString(),
                                address1 = dr_datarow["address1"].ToString(),
                                address2 = dr_datarow["address2"].ToString(),
                                city = dr_datarow["city"].ToString(),
                                state = dr_datarow["state"].ToString(),
                                postal_code = dr_datarow["postal_code"].ToString(),
                                country_name = dr_datarow["country_name"].ToString(),
                                latitude = dr_datarow["latitude"].ToString(),
                                longitude = dr_datarow["longitude"].ToString(),
                                mobile_no = dr_datarow["mobile_no"].ToString(),
                                email_address = dr_datarow["email_address"].ToString(),
                                primary_status = dr_datarow["primary_status"].ToString(),
                            });
                        }
                    }

                    objcontact.address_list = address_list;
                    dt_datatable.Dispose();

                    msSQL = "select salutation,first_name,middle_name,last_name," +
                            "pan_no,aadhar_no,designation,email,mobile from crm_mst_tcontact2promotor  " +
                            " where leadbank_gid='" + leadbank_gid + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var promoter_list = new List<promoter>();
                    if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dr_datarow in dt_datatable.Rows)
                        {
                            promoter_list.Add(new promoter
                            {
                                salutation = dr_datarow["salutation"].ToString(),
                                first_name = dr_datarow["first_name"].ToString(),
                                middle_name = dr_datarow["middle_name"].ToString(),
                                last_name = dr_datarow["last_name"].ToString(),
                                pan_no = dr_datarow["pan_no"].ToString(),
                                aadhar_no = dr_datarow["aadhar_no"].ToString(),
                                designation = dr_datarow["designation"].ToString(),
                                email = dr_datarow["email"].ToString(),
                                mobile = dr_datarow["mobile"].ToString(),
                            });
                        }
                    }

                    objcontact.promoter_list = promoter_list;
                    dt_datatable.Dispose();

                    msSQL = "select salutation,first_name,middle_name,last_name," +
                            "pan_no,aadhar_no,designation_name,email_address,mobile_no " +
                    "from crm_mst_tcontact2director" +
                            " where leadbank_gid='" + leadbank_gid + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var director_list = new List<director>();
                    if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dr_datarow in dt_datatable.Rows)
                        {
                            director_list.Add(new director
                            {
                                salutation = dr_datarow["salutation"].ToString(),
                                first_name = dr_datarow["first_name"].ToString(),
                                middle_name = dr_datarow["middle_name"].ToString(),
                                last_name = dr_datarow["last_name"].ToString(),
                                pan_no = dr_datarow["pan_no"].ToString(),
                                aadhar_no = dr_datarow["aadhar_no"].ToString(),
                                designation = dr_datarow["designation_name"].ToString(),
                                email = dr_datarow["email_address"].ToString(),
                                mobile = dr_datarow["mobile_no"].ToString()
                            });
                        }
                    }

                    objcontact.director_list = director_list;
                    dt_datatable.Dispose();
                }
                msSQL = "select leadbank_gid,document_name,filename,document_path from crm_trn_tfiles" +
                           " where leadbank_gid='" + leadbank_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var CorporateDocumentList = new List<GeneralDocumentList>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        CorporateDocumentList.Add(new GeneralDocumentList
                        {
                            contact_gid = dr_datarow["leadbank_gid"].ToString(),
                            document_name = dr_datarow["document_name"].ToString(),
                            FileName = dr_datarow["filename"].ToString(),
                            path = dr_datarow["document_path"].ToString(),
                            //path = objcmnstorage.EncryptData(dr_datarow["document_path"].ToString()),

                        });
                    }
                }

                objcontact.DocumentList = CorporateDocumentList;
                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "CMN");
                ex.StackTrace.ToString();
                objcontact.status = false;

            }
            objODBCDatareader.Close();
        }
    
        public void DaContactUpdate(string employee_gid, contact values)
        {
            try
            {
                if (values.contact_type == "Individual")
                {
                    msSQL = " update  crm_trn_tleadbank  set " +
                              " salutation ='" + values.salutation + "'," +
                              " salutation ='" + values.salutation + "'," +
                          " referred_by  = '" + values.referred_by + "'," +
                          " source_gid  = '" + values.source_gid + "'," +
                          " leadbank_region  = '" + values.region_gid + "'," +
                              " salutation_gid ='" + values.salutation_gid + "'," +
                              " first_name ='" + values.first_name.Replace("'", "\\\'") + "'," +
                              " middle_name ='" + values.middle_name + "'," +
                              " last_name ='" + values.last_name + "'," +
                              " physicalstatus_gid ='" + values.physicalstatus_gid + "'," +
                              " physicalstatus_name ='" + values.physicalstatus_name + "'," +
                              " leadbank_address1 ='" + values.address1 + "'," +
                              " leadbank_address2 ='" + values.address2 + "'," +
                              " leadbank_city ='" + values.city + "'," +
                              " contact_type ='" + values.contact_type + "'," +
                              " leadbank_state ='" + values.state + "',";
                              if (values.individual_dob == "" || values.individual_dob == null || values.individual_dob == "undefined")
                              {
                                msSQL += " individual_dob = null,";
                              }
                              else
                              {
                                msSQL += " individual_dob ='" + DateTime.Parse(values.individual_dob).ToString("yyyy-MM-dd") + "',";
                              }
                    msSQL += " leadbank_pin ='" + values.postal_code + "'," +
                              " leadbank_country ='" + values.country_name + "'," +
                              " leadbankcountry_gid ='" + values.country_gid + "'," +
                              " latitude ='" + values.latitude + "'," +
                              " longitude ='" + values.longitude + "'," +
                              " tempaddress1 ='" + values.tempaddress1 + "'," +
                              " tempaddress2 ='" + values.tempaddress2 + "'," +
                              " tempcity ='" + values.tempcity.Replace("'", "''") + "'," +
                              " tempstate ='" + values.tempstate + "'," +
                              " temppostal_code ='" + values.temppostal_code + "'," +
                              " tempcountry_name ='" + values.tempcountry_name + "'," +
                              " tempcountry_gid ='" + values.tempcountry_gid + "'," +
                              " templatitude ='" + values.templatitude + "'," +
                              " templongitude ='" + values.templongitude + "'," +
                              " age ='" + values.age + "'," +
                              " gender_name ='" + values.gender_name + "'," +
                              " gender_gid ='" + values.gender_gid + "'," +
                              " aadhar_no ='" + values.aadhar_no + "'," +
                              " pan_no ='" + values.pan_no + "'," +
                              " maritalstatus_name ='" + values.maritalstatus_name + "'," +
                              " maritalstatus_gid ='" + values.maritalstatus_gid + "'," +
                              " designation_gid ='" + values.designation_gid + "'," +
                              " designation_name ='" + values.designation_name + "'," +
                              " father_firstname ='" + values.father_firstname + "'," +
                              " father_lastname ='" + values.father_lastname + "'," +
                              " fathercontact_no ='" + values.fathercontact_no + "'," +
                              " mother_firstname ='" + values.mother_firstname + "'," +
                              " mother_lastname ='" + values.mother_lastname + "'," +
                              " mothercontact_no ='" + values.mothercontact_no + "'," +
                              " spouse_firstname ='" + values.spouse_firstname + "'," +
                              " spouse_lastname ='" + values.spouse_lastname + "'," +
                              " spousecontact_no ='" + values.spousecontact_no + "'," +
                              " educationalqualification_name ='" + values.educationalqualification_name + "'," +
                              " main_occupation ='" + values.main_occupation + "'," +
                              " annual_income ='" + values.annual_income + "'," +
                              " monthly_income ='" + values.monthly_income + "'," +
                              " incometype_gid ='" + values.incometype_gid + "'," +
                              " incometype_name ='" + values.incometype_name + "'," +
                              " updated_by  ='" + employee_gid + "'," +
                              " updated_date  =' " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                              " where leadbank_gid='" + values.contact_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "delete from crm_mst_tcontact2mobileno where leadbank_gid ='" + values.contact_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    for (int i = 0; i < values.mobile_list.Count; i++)
                    {
                        msGetGstGID = objcmnfunctions.GetMasterGID("CO2M");
                        msSQL = " insert into crm_mst_tcontact2mobileno(" +
                                  " contact2mobileno_gid," +
                                  " leadbank_gid," +
                                  " mobile_no," +
                                  " primary_status)" +
                                  " values( " +
                                  "'" + msGetGstGID + "'," +
                                  "'" + values.contact_gid + "'," +
                                  "'" + values.mobile_list[i].mobile_no + "'," +
                                  "'" + values.mobile_list[i].primary_status + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    msSQL = "delete from crm_mst_tcontact2email where leadbank_gid='" + values.contact_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    for (int j = 0; j < values.email_list.Count; j++)
                    {
                        msGetGstGID = objcmnfunctions.GetMasterGID("CO2E");
                        msSQL = " insert into crm_mst_tcontact2email(" +
                                  " contact2email_gid," +
                                  " leadbank_gid," +
                                  " email_address," +
                                  " primary_status)" +
                                  " values( " +
                                  "'" + msGetGstGID + "'," +
                                  "'" + values.contact_gid + "'," +
                                  "'" + values.email_list[j].email_address + "'," +
                                  "'" + values.email_list[j].primary_status + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                }
                else if (values.contact_type == "Corporate")
                {
                    msSQL = " update  crm_trn_tleadbank  set " +
                             " leadbank_gid = '" + values.contact_gid + "'," +
                             " leadbank_name = '" + values.lgltrade_name.Replace("'", "\\\'") + "'," +
                             " lei ='" + values.lei + "'," +
                             " cin ='" + values.cin + "'," +
                          " referred_by  = '" + values.referred_by + "'," +
                          " source_gid  = '" + values.source_gid + "'," +
                          " leadbank_region  = '" + values.region_gid + "'," +
                             " businesss_vintage ='" + values.businesss_vintage + "',";
                             if (values.cin_date == "" || values.cin_date == null || values.cin_date == "undefined")
                             {
                               msSQL += " cin_date = null,";
                             }
                             else
                             {
                               msSQL += " cin_date ='" + DateTime.Parse(values.cin_date).ToString("yyyy-MM-dd") + "',";
                             }
                    msSQL += " constitution ='" + values.constitution + "'," +
                             " constitution_gid ='" + values.constitution_gid + "',";
                             if (values.business_start_date == "" || values.business_start_date == null || values.business_start_date == "undefined")
                             {
                    msSQL += " businessstart_date = null,";
                             }
                             else
                             {
                    msSQL += " businessstart_date ='" + DateTime.Parse(values.business_start_date).ToString("yyyy-MM-dd") + "',";
                             }
                    msSQL += " tan ='" + values.tan + "'," +
                             " tan_state ='" + values.tan_state + "'," +
                             " kin ='" + values.kin + "'," +
                             " udhayam_registration ='" + values.udhayam_registration + "'," +
                             " category_aml ='" + values.category_aml + "'," +
                             " amlcategory_gid ='" + values.amlcategory_gid + "'," +
                             " category_business ='" + values.category_business + "'," +
                             " businesscategory_gid ='" + values.businesscategory_gid + "'," +
                             " corporate_pan_no='" + values.corporate_pan_no + "'," +
                             " last_year_turnover ='" + values.last_year_turnover + "'," +
                             " updated_by  ='" + employee_gid + "'," +
                             " updated_date  ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                             " where leadbank_gid='" + values.contact_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "delete from crm_mst_tcontact2gst where leadbank_gid='" + values.contact_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    for (int i = 0; i < values.gst_list.Count; i++)
                    {
                        msGetGstGID = objcmnfunctions.GetMasterGID("COGT");
                        msSQL = " insert into crm_mst_tcontact2gst(" +
                                  " contact2gst_gid," +
                                  " leadbank_gid," +
                                  " gst_location," +
                                  " gst_no," +
                                  " gst_state)" +
                                  " values( " +
                                  "'" + msGetGstGID + "'," +
                                  "'" + values.contact_gid + "'," +
                                  "'" + values.gst_list[i].gst_location + "'," +
                                  "'" + values.gst_list[i].gst_no + "'," +
                                  "'" + values.gst_list[i].gst_state + "')";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    msSQL = "delete from crm_mst_tcontact2address where leadbank_gid='" + values.contact_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    for (int i = 0; i < values.address_list.Count; i++)
                    {
                        msGetaddressGID = objcmnfunctions.GetMasterGID("COAD");
                        msSQL = " insert into crm_mst_tcontact2address(" +
                                  " contact2address_gid," +
                                  " leadbank_gid," +
                                  " addresstype_gid," +
                                  " address_type," +
                                  " address1," +
                                  " address2," +
                                  " city," +
                                  " state," +
                                  " postal_code," +
                                  " country_name," +
                                  " country_gid," +
                                  " latitude," +
                                  " longitude," +
                                  " email_address," +
                                  " mobile_no," +
                                  " primary_status)" +
                                  " values( " +
                                  "'" + msGetaddressGID + "'," +
                                  "'" + values.contact_gid + "'," +
                                  "'" + values.address_list[i].addresstype_gid + "'," +
                                  "'" + values.address_list[i].addresstype + "'," +
                                   "'" + values.address_list[i].address1 + "'," +
                                  "'" + values.address_list[i].address2 + "'," +
                                  "'" + values.address_list[i].city + "'," +
                                   "'" + values.address_list[i].state + "'," +
                                  "'" + values.address_list[i].postal_code + "'," +
                                  "'" + values.address_list[i].country_name + "'," +
                                  "'" + values.address_list[i].country_gid + "'," +
                                   "'" + values.address_list[i].latitude + "'," +
                                  "'" + values.address_list[i].longitude + "'," +
                                  "'" + values.address_list[i].email_address + "'," +
                                  "'" + values.address_list[i].mobile_no + "'," +
                                  "'" + values.address_list[i].primary_status + "')";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    msSQL = "delete from crm_mst_tcontact2promotor where leadbank_gid='" + values.contact_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    for (int i = 0; i < values.promoter_list.Count; i++)
                    {
                        msGetPromoterGID = objcmnfunctions.GetMasterGID("COPR");

                        msSQL = "insert into crm_mst_tcontact2promotor(" +
                              "contact2promotor_gid," +
                              " leadbank_gid," +
                              " pan_no," +
                              " aadhar_no," +
                              " salutation," +
                              " salutation_gid," +
                              " first_name," +
                              " middle_name," +
                              " last_name," +
                              " designation," +
                              " email," +
                              " mobile," +
                              " created_by," +
                              " created_date) " +
                              " values(" +
                              "'" + msGetPromoterGID + "'," +
                              "'" + values.contact_gid + "'," +
                              "'" + values.promoter_list[i].pan_no + "'," +
                              "'" + values.promoter_list[i].aadhar_no + "'," +
                              "'" + values.promoter_list[i].salutation + "'," +
                              "'" + values.promoter_list[i].salutation_gid + "'," +
                              "'" + values.promoter_list[i].first_name + "'," +
                              "'" + values.promoter_list[i].middle_name + "'," +
                              "'" + values.promoter_list[i].last_name + "'," +
                              "'" + values.promoter_list[i].designation + "'," +
                              "'" + values.promoter_list[i].email + "'," +
                              "'" + values.promoter_list[i].mobile + "'," +
                              "'" + employee_gid + "'," +
                             " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    msSQL = "delete from crm_mst_tcontact2director where leadbank_gid='" + values.contact_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    for (int i = 0; i < values.director_list.Count; i++)
                    {
                        msGetDirectorGID = objcmnfunctions.GetMasterGID("COGR");

                        msSQL = "insert into crm_mst_tcontact2director(" +
                              "contact2director_gid," +
                              " leadbank_gid," +
                              " pan_no," +
                              " aadhar_no," +
                              " salutation," +
                              " salutation_gid," +
                              " first_name," +
                              " middle_name," +
                              " last_name," +
                              " designation_name," +
                              " email_address," +
                              " mobile_no," +
                              " created_by," +
                              " created_date) " +
                              " values(" +
                              "'" + msGetDirectorGID + "'," +
                              "'" + values.contact_gid + "'," +
                              "'" + values.director_list[i].pan_no + "'," +
                              "'" + values.director_list[i].aadhar_no + "'," +
                              "'" + values.director_list[i].salutation + "'," +
                              "'" + values.director_list[i].salutation_gid + "'," +
                              "'" + values.director_list[i].first_name + "'," +
                              "'" + values.director_list[i].middle_name + "'," +
                              "'" + values.director_list[i].last_name + "'," +
                              "'" + values.director_list[i].designation + "'," +
                              "'" + values.director_list[i].email + "'," +
                              "'" + values.director_list[i].mobile + "'," +
                              "'" + employee_gid + "'," +
                             " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Contact Details Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Contact Details";
                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "CMN");
                ex.StackTrace.ToString();
                values.status = false;

            }
        }


        public void Dapostdocument(HttpRequest httpRequest, GeneralDocumentList objfilename, string user_gid)
        {
            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string contact_gid = string.Empty;
            string lscompany_code = string.Empty;
            string pdfFilName = string.Empty;
            string lsdocumenttype_gid = string.Empty;
            string srqdocument_name = httpRequest.Form["document_name"];
            string lscontact_gid = httpRequest.Form["contact_gid"];
            string employee = httpRequest.Form["employee_gid"];
            string GentralDOClist = httpRequest.Form["GentralDOClist"];
            List<mdlcontdoc> defDocumentList = JsonConvert.DeserializeObject<List<mdlcontdoc>>(GentralDOClist);
            String path = lspath;
            HttpPostedFile httpPostedFile;
            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            path = ConfigurationManager.AppSettings["file_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "CRM/ContactDoc/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
            if ((!System.IO.Directory.Exists(path)))
                System.IO.Directory.CreateDirectory(path);
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
                        var getdocumentdtl = defDocumentList.Where(a => a.AutoID_Key == httpFileCollection.AllKeys[i]).FirstOrDefault();
                        string lsfile_gid = msdocument_gid;
                        string project_flag = httpRequest.Form["project_flag"].ToString();
                        FileExtension = Path.GetExtension(FileExtension).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        MemoryStream ms = new MemoryStream();
                        ls_readStream.CopyTo(ms);
                        byte[] bytes = ms.ToArray();
                        if ((objcmnstorage.CheckIsValidfilename(FileExtension, project_flag) == false) || (objcmnstorage.CheckIsExecutable(bytes) == true))
                        {
                            objfilename.status = false;
                            objfilename.message = "File format is not supported";
                            return;
                        }
                        bool status;
                        status = objcmnstorage.UploadStream("erpdocument", lscompany_code + "/" + "crm/ContactDoc/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, ms);
                        ms.Close();
                        lspath = ConfigurationManager.AppSettings["file_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "crm/ContactDoc/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        objcmnfunctions.uploadFile(lspath, lsfile_gid);
                        lspath = "erpdocument" + "/" + lscompany_code + "/" + "crm/ContactDoc/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        msSQL = "  insert into crm_trn_tfiles( " +
                                    " file_gid , " +
                                    " leadbank_gid , " +
                                    " filename ," +
                                    " document_path," +
                                    " document_name," +
                                    " created_by," +
                                    " created_date" +
                                    " )values(" +
                                      "'" + msdocument_gid + "'," +
                                    "'" + lscontact_gid + "'," +
                                    "'" + httpPostedFile.FileName.Replace("'", "") + "'," +
                                    "'" + lspath + msdocument_gid + FileExtension + "'," +
                                     "'" + getdocumentdtl.document_name + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            objfilename.status = true;
                            objfilename.message = "Document Uploaded Successfully";
                        }
                        else
                        {
                            objfilename.status = false;
                            objfilename.message = "Error Occured While Uploading the document";

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "SRQ");
                ex.StackTrace.ToString();
                objfilename.status = false;

            }
        }
        public void DaDeleteContact(string contact_gid, string contact_type, result objResult, string employee_gid)
        {
            try
            {
                msSQL = "Select leadbank_gid from crm_trn_tleadbank " +
                      "where leadbank_gid ='" + contact_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    msSQL = " select lead2campaign_gid from crm_trn_ttelelead2campaign where leadbank_gid='" + contact_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count == 0)
                    {
                        msSQL = " select appointment_gid from crm_trn_tappointment where leadbank_gid= '" + contact_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count == 0)
                        {
                            msSQL = " Delete from crm_trn_tleadbank " +
                                    " where leadbank_gid='" + contact_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                msSQL = " Delete from crm_trn_tleadbankcontact " +
                                        " where leadbank_gid = '" + contact_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            }
                        }

                        if (mnResult != 0)
                        {
                            objResult.status = true;
                            objResult.message = "Lead deleted Successfully";

                        }
                        else
                        {
                            objResult.status = false;
                            objResult.message = "Lead already assigned cant delete";

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + ex.Message.ToString(), "CAD");
                ex.StackTrace.ToString();
                objResult.status = false;

            }
        }
        public void DaConstitutiondropdown(contact_list values)
        {
            msSQL = "select constitution_gid,constitution_name from crm_mst_tconstitution where status='Y'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Constitutiondropdown_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Constitutiondropdown_list
                    {
                        constitution_gid = dt["constitution_gid"].ToString(),
                        constitution_name = dt["constitution_name"].ToString(),
                    });
                    values.Constitutiondropdown_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
    }
}
