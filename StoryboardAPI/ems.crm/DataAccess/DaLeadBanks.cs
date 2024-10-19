using ems.crm.Models;
using ems.utilities.Functions;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Threading;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Web.Http.Results;
using static ems.crm.Models.leadbank_list;


using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace ems.crm.DataAccess
{
    public class DaLeadBank
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msdocument_gid, lscompany_code;
        string msSQL, msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string lssource_name, lsleadbank_name, lscategoryindustry_name, lscountry_name, lsleadbank_gid, lscustomertype_gid,
            lsregion_name, lsbankcontact, msGetGid, msGetGid1, msGetGid2, msGetGid3, msGetGid4, final_path,
            msGetGid5, msGetGid6, msGetGid7, msGetGid8, msGetGid9, msGetGid10, msGetGid11;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5,
            mnResult6, mnResult7, mnResult8, mnResult9, mnResult10, mnResult11,
            mnResult12, mnResult13, mnResult14, mnResult15, mnResult16, mnResult17, mnResult18, mnResult19;
        char lsstatus, lsaddtocustomer;
        public void DaGetLeadbankSummary(MdlLeadBank values)
        {
            try
            {
                //msSQL = "SELECT a.leadbank_gid,b.leadbankcontact_gid,a.leadbank_pin, a.leadbank_id,a.approval_flag,m.country_name,b.mobile,a.leadbank_state,b.email,b.address1,b.address2,a.leadbank_city, a.leadbank_name,b.leadbankcontact_name,ifnull(a.customer_gid,' ') as customer_gid," +
                //     "a.remarks,CONCAT(IFNULL(b.leadbankcontact_name, ''),' / ',IFNULL(b.country_code1, ''), IFNULL('-', ''),IFNULL(b.mobile, ''),' / ',IFNULL(b.email, '')) AS contact_details,a.lead_status, " +
                //     "concat(d.region_name,' / ',a.leadbank_city,' / ',a.leadbank_state) as region_name, " +
                //     "concat(b.address1,b.address2) As address_details,a.customer_type,concat_ws('-',c.user_firstname,c.user_lastname) as created_by," +
                //     "date_format(a.created_date,'%d-%m-%Y') as created_date," +
                //     "CONCAT(CASE WHEN e.source_name IS NULL THEN '' ELSE e.source_name END,' / ',CASE WHEN l.categoryindustry_name IS NULL THEN '' ELSE l.categoryindustry_name END) AS source_name," +
                //     "( g.assign_to )as assigned ,  " +
                //     "concat(j.user_firstname,' ',j.user_lastname) as assign_to from crm_trn_tleadbank a  " +
                //     "left join crm_trn_tleadbankcontact b on a.leadbank_gid=b.leadbank_gid  " +
                //     "left join crm_mst_tregion d on a.leadbank_region=d.region_gid  " +
                //     "left join crm_mst_tsource e on a.source_gid=e.source_gid  " +
                //     "left join crm_mst_tcategoryindustry l on a.categoryindustry_gid = l.categoryindustry_gid  " +
                //     "left join hrm_mst_temployee k on a.created_by=k.employee_gid  " +
                //     "left join adm_mst_tuser c on  c.user_gid = k.user_gid  " +
                //     "left join crm_trn_tlead2campaign g on a.leadbank_gid = g.leadbank_gid " +
                //     "left join hrm_mst_temployee h on  g.assign_to = h.employee_gid  " +
                //     "left join adm_mst_tuser j on h.user_gid = j.user_gid " +
                //     "left join adm_mst_tcountry m on a.leadbank_country = m.country_gid " +
                //     "where a.customertype_gid = 'BCRT240331002'" +
                //     "group by a.leadbank_gid Order by date(a.created_date) desc,a.created_date asc,a.leadbank_gid desc";

                msSQL = "call crm_trn_spleadbankretailer";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadbank_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadbank_list
                        {
                            leadbank_id = dt["leadbank_id"].ToString(),
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            email = dt["email"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            leadbank_city = dt["leadbank_city"].ToString(),
                            leadbank_state = dt["leadbank_state"].ToString(),
                            leadbank_pin = dt["leadbank_pin"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            assign_to = dt["assign_to"].ToString(),
                            remarks = dt["remarks"].ToString(),


                        });
                        values.leadbank_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank summry!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetLeadbankSummary1(MdlLeadBank values)
        {
            try
            {

                var getModuleList = new List<leadbank_list1>();
                msSQL = "call crm_trn_spleadbankcorporate";

                DataTable dtLeadEntries = objdbconn.GetDataTable(msSQL);

                if (dtLeadEntries.Rows.Count > 0)
                {
                    var LeadDict = new Dictionary<string, leadbank_list1>();

                    foreach (DataRow row in dtLeadEntries.Rows)
                    {
                        string leadbank_gid = row["leadbank_gid"].ToString();

                        if (!LeadDict.ContainsKey(leadbank_gid))
                        {
                            LeadDict[leadbank_gid] = new leadbank_list1
                            {
                                leadbank_id = row["leadbank_id"].ToString(),
                                leadbank_gid = row["leadbank_gid"].ToString(),
                                mobile = row["mobile"].ToString(),
                                email = row["email"].ToString(),
                                leadbankcontact_name = row["leadbankcontact_name"].ToString(),
                                address1 = row["address1"].ToString(),
                                address2 = row["address2"].ToString(),
                                leadbank_city = row["leadbank_city"].ToString(),
                                leadbank_state = row["leadbank_state"].ToString(),
                                leadbank_pin = row["leadbank_pin"].ToString(),
                                country_name = row["country_name"].ToString(),
                                leadbank_name = row["leadbank_name"].ToString(),
                                contact_details = row["contact_details"].ToString(),
                                customer_type = row["customer_type"].ToString(),
                                region_name = row["region_name"].ToString(),
                                source_name = row["source_name"].ToString(),
                                created_by = row["created_by"].ToString(),
                                created_date = row["created_date"].ToString(),
                                lead_status = row["lead_status"].ToString(),
                                assign_to = row["assign_to"].ToString(),
                                remarks = row["remarks"].ToString(),
                            };
                        }
                    }

                    getModuleList = LeadDict.Values.ToList();
                    values.leadbank_list1 = getModuleList;

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank summary1!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetLeadbankSummary2(MdlLeadBank values)
        {
            try
            {

                //msSQL = "call crm_trn_spleadbanksummary";

                //dt_datatable = objdbconn.GetDataTable(msSQL);
                //var getModuleList = new List<leadbank_list2>();
                //if (dt_datatable.Rows.Count != 0)
                //{
                //    foreach (DataRow dt in dt_datatable.Rows)
                //    {
                //        getModuleList.Add(new leadbank_list2
                //        {
                //            leadbank_id = dt["leadbank_id"].ToString(),
                //            leadbank_gid = dt["leadbank_gid"].ToString(),
                //            mobile = dt["mobile"].ToString(),

                //            email = dt["email"].ToString(),
                //            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                //            address1 = dt["address1"].ToString(),
                //            address2 = dt["address2"].ToString(),
                //            leadbank_city = dt["leadbank_city"].ToString(),
                //            leadbank_state = dt["leadbank_state"].ToString(),
                //            leadbank_pin = dt["leadbank_pin"].ToString(),
                //            country_name = dt["country_name"].ToString(),
                //            leadbank_name = dt["leadbank_name"].ToString(),
                //            contact_details = dt["contact_details"].ToString(),
                //            customer_type = dt["customer_type"].ToString(),
                //            region_name = dt["region_name"].ToString(),
                //            source_name = dt["source_name"].ToString(),
                //            created_by = dt["created_by"].ToString(),
                //            created_date = dt["created_date"].ToString(),
                //            lead_status = dt["lead_status"].ToString(),
                //            assign_to = dt["assign_to"].ToString(),
                //            remarks = dt["remarks"].ToString(),
                //            lead_type = dt["lead_type"].ToString(),

                //        });
                //        values.leadbank_list2 = getModuleList;
                //    }
                //}
                //dt_datatable.Dispose();


                //


                var getModuleList = new List<leadbank_list2>();
                msSQL = "call crm_trn_spleadbanksummary";

                DataTable dtLeadEntries = objdbconn.GetDataTable(msSQL);

                if (dtLeadEntries.Rows.Count > 0)
                {
                    var LeadDict = new Dictionary<string, leadbank_list2>();

                    foreach (DataRow row in dtLeadEntries.Rows)
                    {
                        string leadbank_gid = row["leadbank_gid"].ToString();

                        if (!LeadDict.ContainsKey(leadbank_gid))
                        {
                            LeadDict[leadbank_gid] = new leadbank_list2
                            {
                                leadbank_id = row["leadbank_id"].ToString(),
                                leadbank_gid = row["leadbank_gid"].ToString(),
                                mobile = row["mobile"].ToString(),
                                email = row["email"].ToString(),
                                leadbankcontact_name = row["leadbankcontact_name"].ToString(),
                                address1 = row["address1"].ToString(),
                                address2 = row["address2"].ToString(),
                                leadbank_city = row["leadbank_city"].ToString(),
                                leadbank_state = row["leadbank_state"].ToString(),
                                leadbank_pin = row["leadbank_pin"].ToString(),
                                country_name = row["country_name"].ToString(),
                                leadbank_name = row["leadbank_name"].ToString(),
                                contact_details = row["contact_details"].ToString(),
                                customer_type = row["customer_type"].ToString(),
                                region_name = row["region_name"].ToString(),
                                source_name = row["source_name"].ToString(),
                                created_by = row["created_by"].ToString(),
                                created_date = row["created_date"].ToString(),
                                lead_status = row["lead_status"].ToString(),
                                assign_to = row["assign_to"].ToString(),
                                remarks = row["remarks"].ToString(),
                            };
                        }
                    }

                    getModuleList = LeadDict.Values.ToList();
                    values.leadbank_list2 = getModuleList;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank summary2!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetsourcedropdown(MdlLeadBank values)
        {
            try
            {
                msSQL = "Select source_gid,source_name as source_name from crm_mst_tsource where status='Y' Order by source_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<source_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new source_list
                        {
                            source_gid = dt["source_gid"].ToString(),
                            source_name = dt["source_name"].ToString(),
                        });
                        values.source_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting source!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetindustrydropdown(MdlLeadBank values)
        {
            try
            {
                msSQL = "select categoryindustry_gid,categoryindustry_name  from crm_mst_tcategoryindustry";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<industryname_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new industryname_list
                        {
                            categoryindustry_gid = dt["categoryindustry_gid"].ToString(),
                            categoryindustry_name = dt["categoryindustry_name"].ToString(),

                        });
                        values.industryname_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting industry!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaGetregiondropdown(MdlLeadBank values)
        {
            try
            {
                msSQL = "SELECT region_gid, concat (region_name,' / ',city) as region_name" +
                    " FROM crm_mst_tregion  where status='Y' Order by region_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<regionname_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new regionname_list
                        {
                            region_gid = dt["region_gid"].ToString(),
                            region_name = dt["region_name"].ToString(),

                        });
                        values.regionname_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting region !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetcountrynamedropdown(MdlLeadBank values)
        {
            try
            {
                msSQL = "Select country_gid,country_name from adm_mst_tcountry";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<country_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new country_list1
                        {
                            country_gid = dt["country_gid"].ToString(),
                            country_name = dt["country_name"].ToString(),
                        });
                        values.country_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting country name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostleadbank(string employee_gid, leadbank_list values)
        {
            try
            {

                msSQL = " Select source_name  from crm_mst_tsource where source_gid = '" + values.source_name + "'";
                string lssource_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select leadbank_name  from crm_trn_tleadbank where  leadbank_name = '" + values.leadbank_name.Replace("'", "\\\'") + "'";
                string lsleadbank_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select country_name  from adm_mst_tcountry where country_gid = '" + values.country_name + "'";
                string lscountry_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select  region_name from crm_mst_tregion where  region_gid = '" + values.region_name + "'";
                string lsregion_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select customer_type from crm_mst_tcustomertype Where customertype_gid='" + values.customer_type + "'";
                string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select email from crm_trn_tleadbankcontact Where email='" + values.email + "' and main_contact ='Y'";
                string lsemail = objdbconn.GetExecuteScalar(msSQL);

                if (lsleadbank_name == null || lsleadbank_name == "")
                {

                    if (values.status != "Y")
                    {
                        lsstatus = 'Y';
                    }

                    if (values.addtocustomer != false)
                    {
                        lsaddtocustomer = 'Y';
                    }

                    msGetGid = objcmnfunctions.GetMasterGID("BMCC");
                    msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

                    msSQL = " INSERT INTO crm_trn_tleadbank(" +
                            " leadbank_gid," +
                            " source_gid," +
                            " leadbank_id," +
                            " leadbank_name," +
                            " status," +
                            " company_website," +
                            " approval_flag, " +
                            " lead_status," +
                            " leadbank_code," +
                            " leadbank_state," +
                            " leadbank_address1," +
                            " leadbank_address2," +
                            " leadbank_region," +
                            " leadbank_country," +
                            " leadbank_city," +
                            " leadbank_pin," +
                            " customer_type," +
                            " customertype_gid," +
                            " created_by," +
                            " remarks," +
                            " categoryindustry_gid," +
                            " referred_by," +
                            " main_branch," +
                            " lead_type," +
                            " created_date)" +
                            " values(" +
                            " '" + msGetGid1 + "'," +
                            " '" + values.source_name + "'," +
                            " '" + msGetGid + "'," +
                            " '" + values.leadbank_name.Replace("'", "\\\'") + "'," +
                            " ' Y'," +
                            " '" + values.company_website + "'," +
                            " 'Approved'," +
                            " 'Not Assigned'," +
                            " 'H.Q'," +
                              " '" + (String.IsNullOrEmpty(values.leadbank_state) ? values.leadbank_state : values.leadbank_state.Replace("'", "\\\'")) + "'," +
                            " '" + (String.IsNullOrEmpty(values.leadbank_address1) ? values.leadbank_address1 : values.leadbank_address1.Replace("'", "\\\'")) + "'," +
                            " '" + (String.IsNullOrEmpty(values.leadbank_address2) ? values.leadbank_address2 : values.leadbank_address2.Replace("'", "\\\'")) + "'," +
                            " '" + values.region_name + "'," +
                            " '" + values.country_name + "'," +
                            " '" + (String.IsNullOrEmpty(values.leadbank_city) ? values.leadbank_city : values.leadbank_city.Replace("'", "\\\'")) + "'," +
                            " '" + values.leadbank_pin + "'," +
                            " '" + values.customer_type + "'," +
                            " '" + values.customer_type + "'," +
                            " '" + employee_gid + "'," +
                            " '" + (String.IsNullOrEmpty(values.remarks) ? values.remarks : values.remarks.Replace("'", "\\\'")) + "'," +
                            " '" + values.categoryindustry_name + "'," +
                            " '" + values.referred_by + "'," +
                            " 'Y'," +
                            " '" + values.lead_type + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                    if (msGetGid2 == "E")
                    {
                        values.Status = false;
                        values.message = "Create sequence code BLCC for Lead Bank";
                    }
                    msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                        " (leadbankcontact_gid," +
                        " leadbank_gid," +
                        " leadbankcontact_name," +
                        " email, mobile," +
                        " wedding_day," +
                        " birthday," +
                        " interests," +
                        " phone1," +
                        " country_code1," +
                        " area_code1," +
                        " phone2," +
                        " country_code2," +
                        " area_code2," +
                        " fax," +
                        " fax_country_code," +
                        " fax_area_code," +
                        " designation," +
                        " created_date," +
                        " created_by," +
                        " leadbankbranch_name, " +
                        " address1, " +
                        " address2, " +
                        " city, " +
                        " state, " +
                        " pincode, " +
                        " country_gid, " +
                        " region_name, " +
                        " main_contact)" +
                        " values( " +
                        " '" + msGetGid2 + "'," +
                        " '" + msGetGid1 + "'," +
                        " '" + (String.IsNullOrEmpty(values.leadbankcontact_name) ? values.leadbankcontact_name : values.leadbankcontact_name.Replace("'", "\\\'")) + "',";



                    if (values.email == "" || values.email == null)
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += " '" + values.email + "',";
                    }

                    if (values.phone.e164Number == "" || values.phone.e164Number == null)
                    {
                        msSQL += "'',";
                    }
                    else
                    {
                        msSQL += " '" + values.phone.e164Number + "',";
                    }

                    msSQL +=
                     " '" + values.wedding_day + "'," +
                     " '" + values.birth_day + "'," +
                     " '" + values.interests + "'," +
                     " '" + values.phone1 + "'," +
                     " '" + values.country_code1 + "'," +
                     " '" + values.area_code1 + "'," +
                     " '" + values.phone2 + "'," +
                     " '" + values.country_code2 + "'," +
                     " '" + values.area_code2 + "'," +
                     " '" + values.fax + "'," +
                     " '" + values.fax_country_code + "'," +
                     " '" + values.fax_area_code + "'," +
                     " '" + (String.IsNullOrEmpty(values.designation) ? values.designation : values.designation.Replace("'", "\\\'")) + "'," +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " '" + employee_gid + "'," +
                        " 'H.Q'," +
                        " '" + (String.IsNullOrEmpty(values.leadbank_address1) ? values.leadbank_address1 : values.leadbank_address1.Replace("'", "\\\'")) + "'," +
                        " '" + (String.IsNullOrEmpty(values.leadbank_address2) ? values.leadbank_address2 : values.leadbank_address2.Replace("'", "\\\'")) + "'," +
                        " '" + (String.IsNullOrEmpty(values.leadbank_city) ? values.leadbank_city : values.leadbank_city.Replace("'", "\\\'")) + "'," +
                        " '" + (String.IsNullOrEmpty(values.leadbank_state) ? values.leadbank_state : values.leadbank_state.Replace("'", "\\\'")) + "'," +
                        " '" + values.leadbank_pin + "'," +
                        " '" + values.country_name + "'," +
                        " '" + (String.IsNullOrEmpty(lsregion_name) ? lsregion_name : lsregion_name.Replace("'", "\\\'")) + "'," +
                        " 'Y'" + ")";
                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult1 == 1)
                    {
                        msSQL = "select to_mail from crm_smm_mailmanagement where to_mail='" + values.email + "';";
                        string to_mail = objdbconn.GetExecuteScalar(msSQL);
                        if (to_mail != null)
                        {
                            msSQL = "update crm_smm_mailmanagement set leadbank_gid='" + msGetGid1 + "'  where to_mail='" + to_mail + "';";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                        msSQL = "select mobile from crm_trn_tleadbankcontact where leadbank_gid = '" + msGetGid1 + "' and leadbankcontact_gid='" + msGetGid2 + "' and main_contact ='Y'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            string mobile = objOdbcDataReader["mobile"].ToString();

                            msSQL = "select leadbank_name, SUBSTRING_INDEX(leadbank_name, ' ', 1) AS firstName," +
                                "CASE WHEN LOCATE(' ', leadbank_name) > 0 THEN SUBSTRING_INDEX(leadbank_name, ' ', -1)ELSE ''END AS lastName" +
                                " from crm_trn_tleadbank where leadbank_gid = '" + msGetGid1 + "'";

                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.HasRows)
                            {
                                string leadbank_name = objOdbcDataReader["leadbank_name"].ToString();
                                string firstName = objOdbcDataReader["firstName"].ToString();
                                string lastName = objOdbcDataReader["lastName"].ToString();

                                if (mobile != null && mobile != "")
                                {

                                    Rootobject objRootobject = new Rootobject();
                                    string contactjson = "{\"displayName\":\"" + values.leadbank_name + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + mobile + "\"}],\"firstName\":\"" + firstName + "\",\"lastName\":\"" + lastName + "\"}";
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                    var request = new RestRequest(ConfigurationManager.AppSettings["messagebirdcontact"].ToString(), Method.POST);
                                    request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                                    request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                                    IRestResponse response = client.Execute(request);
                                    var responseoutput = response.Content;
                                    objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);
                                    if (response.StatusCode == HttpStatusCode.Created)
                                    {
                                        msSQL = "insert into crm_smm_whatsapp(leadbank_gid,leadbankcontact_gid,id,wkey,wvalue,displayName,firstName,lastName,created_date,created_by)values(" +
                                                " '" + msGetGid2 + "'," +
                                                " '" + msGetGid1 + "'," +
                                                "'" + objRootobject.id + "'," +
                                                "'phonenumber'," +
                                                "'" + mobile + "'," +
                                                " '" + (String.IsNullOrEmpty(leadbank_name) ? leadbank_name : leadbank_name.Replace("'", "\\\'")) + "'," +
                                                " '" + (String.IsNullOrEmpty(firstName) ? firstName : firstName.Replace("'", "\\\'")) + "'," +
                                                " '" + (String.IsNullOrEmpty(lastName) ? lastName : lastName.Replace("'", "\\\'")) + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                "'" + employee_gid + "')";

                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult == 1)
                                        {
                                            msSQL = "update crm_trn_tleadbank set wh_flag = 'Y', wh_id = '" + objRootobject.id + "' where leadbank_gid = '" + msGetGid1 + "'";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                        }

                                    }
                                }
                                objOdbcDataReader.Close();

                            }
                        }



                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.Status = true;
                            values.message = "Lead Added Successfully";
                        }
                        else
                        {
                            values.Status = false;
                            values.message = "Error Occurred While Inserting Records";
                        }
                    }

                }
                else
                {
                    values.Status = false;
                    values.message = "Lead Already Exist";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding lead bank!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaRetailerPostleadbank(string employee_gid, leadbank_list values)
        {
            try
            {
                msSQL = " Select source_name  from crm_mst_tsource where source_gid = '" + values.source_name + "'";
                string lssource_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select leadbank_name  from crm_trn_tleadbank where  leadbank_gid = '" + values.leadbankcontact_name + "' and main_contact ='Y'";
                string lsleadbank_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select country_name  from adm_mst_tcountry where country_gid = '" + values.country_name + "'";
                string lscountry_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select  region_name from crm_mst_tregion where  region_gid = '" + values.region_name + "'";
                string lsregion_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select customer_type from crm_mst_tcustomertype Where customertype_gid='BCRT240331002'";
                string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);


                if (values.status != "Y")
                {
                    lsstatus = 'Y';
                }

                if (values.addtocustomer != false)
                {
                    lsaddtocustomer = 'Y';
                }

                msGetGid = objcmnfunctions.GetMasterGID("BMCC");
                msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

                msSQL = " INSERT INTO crm_trn_tleadbank(" +
                        " leadbank_gid," +
                        " source_gid," +
                        " leadbank_id," +
                        " leadbank_name," +
                        " status," +
                        " company_website," +
                        " approval_flag, " +
                        " lead_status," +
                        " leadbank_code," +
                        " leadbank_state," +
                        " leadbank_address1," +
                        " leadbank_address2," +
                        " leadbank_region," +
                        " leadbank_country," +
                        " leadbank_city," +
                        " leadbank_pin," +
                        " customer_type," +
                        " customertype_gid," +
                        " created_by," +
                        " remarks," +
                        " categoryindustry_gid," +
                        " referred_by," +
                        " main_branch," +
                        " created_date)" +
                        " values(" +
                        " '" + msGetGid1 + "'," +
                        " '" + values.source_name + "'," +
                        " '" + msGetGid + "'," +
                        " '" + values.leadbankcontact_name + "'," +
                        " 'Y'," +
                        " '" + values.company_website + "'," +
                        " 'Approved'," +
                        " 'Not Assigned'," +
                        " 'H.Q'," +
                        " '" + values.leadbank_state + "'," +
                        " '" + values.leadbank_address1 + "'," +
                        " '" + values.leadbank_address2 + "'," +
                        " '" + values.region_name + "'," +
                        " '" + values.country_name + "'," +
                        " '" + values.leadbank_city + "'," +
                        " '" + values.leadbank_pin + "'," +
                        " '" + lscustomer_type + "'," +
                        " 'BCRT240331002'," +
                        " '" + employee_gid + "'," +
                        " '" + values.remarks.Replace("'", "\\\'") + "'," +
                        " '" + values.categoryindustry_name + "'," +
                        " '" + values.referred_by + "'," +
                        " 'Y'," +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                if (msGetGid2 == "E")
                {
                    values.Status = false;
                    values.message = "Create sequence code BLCC for Lead Bank";
                }
                msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                     " (leadbankcontact_gid," +
                     " leadbank_gid," +
                     " leadbankcontact_name," +
                     " email, mobile," +
                     " phone1," +
                     " country_code1," +
                     " area_code1," +
                     " phone2," +
                     " country_code2," +
                     " area_code2," +
                     " fax," +
                     " fax_country_code," +
                     " fax_area_code," +
                     " designation," +
                     " created_date," +
                     " created_by," +
                     " leadbankbranch_name, " +
                     " address1, " +
                     " address2, " +
                     " city, " +
                     " state, " +
                     " pincode, " +
                     " country_gid, " +
                     " region_name, " +
                     " main_contact)" +
                     " values( " +
                     " '" + msGetGid2 + "'," +
                     " '" + msGetGid1 + "',";

                if (values.leadbankcontact_name == "" || values.leadbankcontact_name == null)
                {
                    msSQL += "'',";
                }
                else
                {
                    msSQL += " '" + values.leadbankcontact_name + "',";
                }

                if (values.email == "" || values.email == null)
                {
                    msSQL += "'',";
                }
                else
                {
                    msSQL += " '" + values.email + "',";
                }

                if (values.email == "" || values.email == null)
                {
                    msSQL += "'',";
                }
                else
                {
                    msSQL += " '" + values.phone.e164Number + "',";
                }

                msSQL += " '" + values.phone1 + "'," +
                 " '" + values.country_code1 + "'," +
                 " '" + values.area_code1 + "'," +
                 " '" + values.phone2 + "'," +
                 " '" + values.country_code2 + "'," +
                 " '" + values.area_code2 + "'," +
                 " '" + values.fax + "'," +
                 " '" + values.fax_country_code + "'," +
                 " '" + values.fax_area_code + "',";
                if (values.email == "" || values.email == null)
                {
                    msSQL += "'',";
                }
                else
                {
                    msSQL += " '" + values.designation + "',";
                }

                msSQL += " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    " '" + employee_gid + "'," +
                    " 'H.Q'," +
                    " '" + values.leadbank_address1 + "'," +
                    " '" + values.leadbank_address2 + "'," +
                    " '" + values.leadbank_city + "'," +
                    " '" + values.leadbank_state + "'," +
                    " '" + values.leadbank_pin + "'," +
                    " '" + values.country_gid + "'," +
                    " '" + lsregion_name + "'," +
                    " 'Y'" + ")";
                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult1 == 1)
                {
                    msSQL = "select to_mail from crm_smm_mailmanagement where to_mail='" + values.email + "';";
                    string to_mail = objdbconn.GetExecuteScalar(msSQL);
                    if (to_mail != null)
                    {
                        msSQL = "update crm_smm_mailmanagement set leadbank_gid='" + msGetGid1 + "'  where to_mail='" + to_mail + "';";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    msSQL = "select mobile from crm_trn_tleadbankcontact where leadbank_gid = '" + msGetGid1 + "' and leadbankcontact_gid='" + msGetGid2 + "'and main_contact ='Y'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        string mobile = objOdbcDataReader["mobile"].ToString();

                        msSQL = "select leadbank_name, SUBSTRING_INDEX(leadbank_name, ' ', 1) AS firstName," +
                            "CASE WHEN LOCATE(' ', leadbank_name) > 0 THEN SUBSTRING_INDEX(leadbank_name, ' ', -1)ELSE ''END AS lastName" +
                            " from crm_trn_tleadbank where leadbank_gid = '" + msGetGid1 + "'";

                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            string leadbank_name = objOdbcDataReader["leadbank_name"].ToString();
                            string firstName = objOdbcDataReader["firstName"].ToString();
                            string lastName = objOdbcDataReader["lastName"].ToString();
                            if (mobile != null && mobile != "")
                            {

                                Rootobject objRootobject = new Rootobject();
                                string contactjson = "{\"displayName\":\"" + values.leadbankcontact_name + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + mobile + "\"}],\"firstName\":\"" + firstName + "\",\"lastName\":\"" + lastName + "\"}";
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                var request = new RestRequest(ConfigurationManager.AppSettings["messagebirdcontact"].ToString(), Method.POST);
                                request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                var responseoutput = response.Content;
                                objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);
                                if (response.StatusCode == HttpStatusCode.Created)
                                {
                                    msSQL = "insert into crm_smm_whatsapp(leadbank_gid,leadbankcontact_gid,id,wkey,wvalue,displayName,firstName,lastName,created_date,created_by)values(" +
                                            " '" + msGetGid2 + "'," +
                                            " '" + msGetGid1 + "'," +
                                            "'" + objRootobject.id + "'," +
                                            "'phonenumber'," +
                                            "'" + mobile + "'," +
                                            "'" + leadbank_name + "'," +
                                            "'" + firstName + "'," +
                                            "'" + lastName + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "'" + employee_gid + "')";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        msSQL = "update crm_trn_tleadbank set wh_flag = 'Y', wh_id = '" + objRootobject.id + "' where leadbank_gid = '" + msGetGid1 + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    }

                                }
                            }

                        }
                    }



                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.Status = true;
                        values.message = "Lead Added Successfully";
                    }
                    else
                    {
                        values.Status = false;
                        values.message = "Error Occurred While Inserting Records";
                    }
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding retail lead bank!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }


        public void DaUpdateleadbank(string user_gid, leadbank_list values)
        {
            try
            {
                msSQL = " Select source_gid  from crm_mst_tsource where source_name = '" + values.source_name + "'";
                string lssource_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select leadbank_gid  from crm_trn_tleadbank where  leadbank_name = '" + values.leadbank_name.Replace("'", "\\\'") + "'";
                string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select  categoryindustry_name from crm_mst_tcategoryindustry where categoryindustry_gid = '" + values.categoryindustry_name + "'";
                string lscategoryindustry_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select country_gid  from adm_mst_tcountry where country_name = '" + values.country_name + "'";
                string lscountry_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select  region_name from crm_mst_tregion where  region_gid = '" + values.region_name + "'";
                string lsregion_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select  leadbankcontact_gid from crm_trn_tleadbankcontact where  leadbankcontact_name = '" + (String.IsNullOrEmpty(values.leadbankcontact_name) ? values.leadbankcontact_name : values.leadbankcontact_name.Replace("'", "\\\'")) + "'," + "' and main_contact ='Y'";
                string lsbankcontact = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select customer_type from crm_mst_tcustomertype Where customertype_gid='" + values.customer_type + "'";
                string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " Update crm_trn_tleadbank set" +
                " source_gid = '" + values.source_name + "'," +
                " categoryindustry_gid = '" + values.categoryindustry_name + "'," +
                " leadbank_name = '" + values.leadbank_name.Replace("'", "\\\'") + "'," +
                " status = '" + 'Y' + "'," +
                " company_website= '" + values.company_website + "'," +
                " leadbank_address1 =   '" + (String.IsNullOrEmpty(values.leadbank_address1) ? values.leadbank_address1 : values.leadbank_address1.Replace("'", "\\\'")) + "'," +
                " leadbank_address2 = '" + (String.IsNullOrEmpty(values.leadbank_address2) ? values.leadbank_address2 : values.leadbank_address2.Replace("'", "\\\'")) + "'," +
                " leadbank_city = '" + (String.IsNullOrEmpty(values.leadbank_city) ? values.leadbank_city : values.leadbank_city.Replace("'", "\\\'")) + "'," +
                " leadbank_region = '" + values.region_name + "'," +
                " leadbank_state = '" + (String.IsNullOrEmpty(values.leadbank_state) ? values.leadbank_state : values.leadbank_state.Replace("'", "\\\'")) + "'," +
                " leadbank_country = '" + values.country_name + "'," +
                " leadbank_pin = '" + values.leadbank_pin + "'," +
                " customer_type = '" + values.customer_type + "'," +
                " customertype_gid = '" + values.customer_type + "'," +
                " lead_type = '" + values.lead_type + "'," +
                " updated_by = '" + user_gid + "'," +
                " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                " approval_flag = 'Approved'," +
                " referred_by= '" + values.referred_by + "'," +
                " remarks = '" + (String.IsNullOrEmpty(values.remarks) ? values.remarks : values.remarks.Replace("'", "\\\'")) + "'" +
                " where leadbank_gid = '" + values.leadbank_gid + "'";


                mnResult10 = objdbconn.ExecuteNonQuerySQL(msSQL);



                msSQL = " Update crm_trn_tleadbankcontact set" +
                    " leadbankcontact_name = '" + (String.IsNullOrEmpty(values.leadbankcontact_name) ? values.leadbankcontact_name : values.leadbankcontact_name.Replace("'", "\\\'")) + "'," +
                    " mobile = '" + values.phone.e164Number + "'," +
                    " email = '" + values.email + "'," +
                    " designation = '" + (String.IsNullOrEmpty(values.designation) ? values.designation : values.designation.Replace("'", "\\\'")) + "'," +
                    " phone1 = '" + values.phone1 + "'," +
                    " phone2 ='" + values.phone2 + "'," +
                    " fax = '" + values.fax + "'," +
                    " wedding_day = '" + values.wedding_day + "'," +
                    " birthday = '" + values.birth_day + "'," +
                    " interests = '" + values.interests + "'," +
                    " address1 = '" + (String.IsNullOrEmpty(values.leadbank_address1) ? values.leadbank_address1 : values.leadbank_address1.Replace("'", "\\\'")) + "'," +
                    " address2 = '" + (String.IsNullOrEmpty(values.leadbank_address2) ? values.leadbank_address2 : values.leadbank_address2.Replace("'", "\\\'")) + "'," +
                    " city = '" + (String.IsNullOrEmpty(values.leadbank_city) ? values.leadbank_city : values.leadbank_city.Replace("'", "\\\'")) + "'," +
                    " state = '" + (String.IsNullOrEmpty(values.leadbank_state) ? values.leadbank_state : values.leadbank_state.Replace("'", "\\\'")) + "'," +
                    " country_gid = '" + lscountry_gid + "'," +
                    " pincode = '" + values.pincode + "'," +
                    " region_name ='" + (String.IsNullOrEmpty(lsregion_name) ? lsregion_name : lsregion_name.Replace("'", "\\\'")) + "'" +
                    " where leadbank_gid ='" + values.leadbank_gid + "'" +
                    " and leadbankcontact_gid = '" + values.leadbankcontact_gid + "' and main_contact ='Y'";

                mnResult11 = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult11 != 0)
                {
                    msSQL = "select mobile from crm_trn_tleadbankcontact where leadbank_gid = '" + msGetGid1 + "' and leadbankcontact_gid='" + msGetGid2 + "'and main_contact ='Y'";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        string mobile = objOdbcDataReader["mobile"].ToString();

                        msSQL = "select leadbank_name, SUBSTRING_INDEX(leadbank_name, ' ', 1) AS firstName," +
                            "CASE WHEN LOCATE(' ', leadbank_name) > 0 THEN SUBSTRING_INDEX(leadbank_name, ' ', -1)ELSE ''END AS lastName" +
                            " from crm_trn_tleadbank where leadbank_gid = '" + msGetGid1 + "'";

                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            string leadbank_name = objOdbcDataReader["leadbank_name"].ToString();
                            string firstName = objOdbcDataReader["firstName"].ToString();
                            string lastName = objOdbcDataReader["lastName"].ToString();
                            if (mobile != null && mobile != "")
                            {

                                Rootobject objRootobject = new Rootobject();
                                string contactjson = "{\"displayName\":\"" + values.leadbank_name + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + mobile + "\"}],\"firstName\":\"" + firstName + "\",\"lastName\":\"" + lastName + "\"}";
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                var request = new RestRequest(ConfigurationManager.AppSettings["messagebirdcontact"].ToString(), Method.POST);
                                request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                                request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                var responseoutput = response.Content;
                                objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);
                                if (response.StatusCode == HttpStatusCode.Created)
                                {
                                    msSQL = "insert into crm_smm_whatsapp(leadbank_gid,leadbankcontact_gid,id,wkey,wvalue,displayName,firstName,lastName,created_date,created_by)values(" +
                                            " '" + msGetGid2 + "'," +
                                            " '" + msGetGid1 + "'," +
                                            "'" + objRootobject.id + "'," +
                                            "'phonenumber'," +
                                            "'" + mobile + "'," +
                                            "'" + leadbank_name.Replace("'", "\\\'") + "'," +
                                            "'" + (String.IsNullOrEmpty(firstName) ? firstName : firstName.Replace("'", "\\\'")) + "'," +
                                            "'" + (String.IsNullOrEmpty(lastName) ? lastName : lastName.Replace("'", "\\\'")) + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "'" + user_gid + "')";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    if (mnResult == 1)
                                    {
                                        msSQL = "update crm_trn_tleadbank set wh_flag = 'Y', wh_id = '" + objRootobject.id + "' where leadbank_gid = '" + msGetGid1 + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    }

                                }
                            }

                        }
                    }
                    else
                    {
                        values.Status = true;
                        values.message = "Lead Bank Updated Successfully";
                    }

                }
                else
                {
                    values.Status = false;
                    values.message = "Error While Updating Lead Bank";
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating lead bank!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaUpdateretailer(string user_gid, leadbank_list values)
        {
            try
            {
                msSQL = " Select source_gid  from crm_mst_tsource where source_name = '" + values.source_name + "'";
                string lssource_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select leadbank_gid  from crm_trn_tleadbank where  leadbank_name = '" + values.leadbank_name + "'";
                string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select  categoryindustry_name from crm_mst_tcategoryindustry where categoryindustry_gid = '" + values.categoryindustry_name + "'";
                string lscategoryindustry_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select country_gid  from adm_mst_tcountry where country_name = '" + values.country_name + "'";
                string lscountry_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select  region_name from crm_mst_tregion where  region_gid = '" + values.region_name + "'";
                string lsregion_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select  leadbankcontact_gid from crm_trn_tleadbankcontact where  leadbankcontact_name = '" + values.leadbankcontact_name + "' and main_contact ='Y'";
                string lsbankcontact = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " Select  leadbankcontact_name crm_trn_tleadbankcontact  leadbankcontact_gid = '" + values.leadbankcontact_gid + "' and main_contact ='Y'";
                string lsbankcontact_name = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " select customer_type from crm_mst_tcustomertype Where customertype_gid='" + values.customer_type + "'";
                string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);
                if (lsbankcontact_name != values.leadbankcontact_name)
                {
                    msSQL = " Update crm_trn_tleadbank set" +
                  " leadbank_name = '" + values.leadbankcontact_name + "'," +
                  " where leadbank_gid = '" + values.leadbank_gid + "'";
                }

                msSQL = " Update crm_trn_tleadbank set" +
                    " source_gid = '" + values.source_name + "'," +
                    " categoryindustry_gid = '" + values.categoryindustry_name + "'," +
                    " status = '" + 'Y' + "'," +
                    " leadbank_address1 = '" + values.leadbank_address1 + "'," +
                    " leadbank_address2 = '" + values.leadbank_address2 + "'," +
                    " leadbank_city = '" + values.leadbank_city + "'," +
                    " leadbank_region = '" + values.region_name + "'," +
                    " leadbank_state = '" + values.leadbank_state + "'," +
                    " leadbank_country = '" + values.country_name + "'," +
                    " leadbank_pin = '" + values.leadbank_pin + "'," +
                    " customer_type = '" + lscustomer_type + "'," +
                    " customertype_gid = '" + values.customer_type + "'," +
                    " updated_by = '" + user_gid + "'," +
                    " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    " approval_flag = 'Approved'," +
                    " referred_by= '" + values.referred_by + "'," +
                    " remarks = '" + values.remarks + "'" +
                    " where leadbank_gid = '" + values.leadbank_gid + "'";



                mnResult10 = objdbconn.ExecuteNonQuerySQL(msSQL);



                msSQL = " Update crm_trn_tleadbankcontact set" +
                    " leadbankcontact_name = '" + values.leadbankcontact_name + "'," +
                    " mobile = '" + values.mobile + "'," +
                    " email = '" + values.email + "'," +
                    " designation = '" + values.designation + "'," +
                    " region_name = '" + lsregion_name + "'" +
                    " where leadbank_gid ='" + values.leadbank_gid + "'" +
                    " and leadbankcontact_gid = '" + values.leadbankcontact_gid + "' and main_contact ='Y'";

                mnResult11 = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult11 != 0)
                {
                    values.Status = true;
                    values.message = "Lead Bank Updated Successfully";
                }
                else
                {
                    values.Status = false;
                    values.message = "Error While Updating Lead Bank";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating retail!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetleadbankeditSummary(string leadbank_gid, MdlLeadBank values)
        {
            try
            {
                msSQL = "select a.leadbank_gid,a.leadbank_id,b.wedding_day,b.birthday,b.interests,b.leadbankcontact_gid,a.status,a.referred_by, a.remarks, a.leadbank_name, b.leadbankcontact_name,a.source_gid,b.country_gid, a.leadbank_country, a.categoryindustry_gid, " +
                         " b.designation, b.mobile, b.email, a.company_website,b.fax ,b.fax_area_code , b.fax_country_code, f.region_gid, " +
                         " b.country_code1, b.area_code1, b.phone1, b.country_code2, b.area_code2, b.phone2, a.leadbank_address1,  " +
                         " a.leadbank_address2, a.leadbank_city, a.leadbank_state, a.leadbank_pin, a.approval_flag, d.country_name,a.lead_type , " +
                         " c.source_name, f.region_name, e.categoryindustry_name,customertype_gid,customer_type from crm_trn_tleadbank a " +
                         " left join crm_trn_tleadbankcontact b on a.leadbank_gid = b.leadbank_gid " +
                         " left join crm_mst_tsource c on c.source_gid = a.source_gid " +
                         "left join adm_mst_tcountry d on d.country_gid = b.country_gid " +
                         "left join crm_mst_tregion f on b.region_name = f.region_name " +
                         "left join crm_mst_tcategoryindustry e on e.categoryindustry_gid = a.categoryindustry_gid " +
                         "where a.leadbank_gid ='" + leadbank_gid + "'and b.main_contact ='Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadbankedit_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        {
                            getModuleList.Add(new leadbankedit_list
                            {
                                leadbank_gid = dt["leadbank_gid"].ToString(),
                                leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                                referred_by = dt["referred_by"].ToString(),
                                remarks = dt["remarks"].ToString(),
                                source_gid = dt["source_gid"].ToString(),
                                country_gid = dt["leadbank_country"].ToString(),
                                region_gid = dt["region_gid"].ToString(),
                                categoryindustry_gid = dt["categoryindustry_gid"].ToString(),
                                leadbank_name = dt["leadbank_name"].ToString(),
                                leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                                designation = dt["designation"].ToString(),
                                mobile = dt["mobile"].ToString(),
                                email = dt["email"].ToString(),
                                company_website = dt["company_website"].ToString(),
                                fax = dt["fax"].ToString(),
                                fax_area_code = dt["fax_area_code"].ToString(),
                                fax_country_code = dt["fax_country_code"].ToString(),
                                country_code1 = dt["country_code1"].ToString(),
                                area_code1 = dt["area_code1"].ToString(),
                                phone1 = dt["phone1"].ToString(),
                                country_code2 = dt["country_code2"].ToString(),
                                area_code2 = dt["area_code2"].ToString(),
                                phone2 = dt["phone2"].ToString(),
                                leadbank_address1 = dt["leadbank_address1"].ToString(),
                                leadbank_address2 = dt["leadbank_address2"].ToString(),
                                leadbank_city = dt["leadbank_city"].ToString(),
                                leadbank_state = dt["leadbank_state"].ToString(),
                                leadbank_pin = dt["leadbank_pin"].ToString(),
                                customer_type = dt["customer_type"].ToString(),
                                approval_flag = dt["approval_flag"].ToString(),
                                country_name = dt["country_name"].ToString(),
                                source_name = dt["source_name"].ToString(),
                                region_name = dt["region_name"].ToString(),
                                categoryindustry_name = dt["categoryindustry_name"].ToString(),
                                customertype_gid = dt["customertype_gid"].ToString(),
                                wedding_day = dt["wedding_day"].ToString(),
                                birth_day = dt["birthday"].ToString(),
                                interests = dt["interests"].ToString(),
                                lead_type = dt["lead_type"].ToString(),
                            });
                            values.leadbankedit_list = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank edit summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetleadbankcontactaddSummary(string leadbank_gid, MdlLeadBank values)
        {
            try
            {
                msSQL = " select leadbankbranch_name,leadbank_gid,leadbankcontact_gid,leadbankcontact_name,designation,mobile,email,fax,fax_country_code," +
                        " fax_area_code,phone1,country_code1,area_code1,phone2,country_code2,area_code2 " +
                        " from crm_trn_tleadbankcontact " +
                        " where leadbank_gid ='" + leadbank_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadbank_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadbank_list
                        {

                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            designation = dt["designation"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            email = dt["email"].ToString(),
                            fax_area_code = dt["fax_area_code"].ToString(),
                            fax_country_code = dt["fax_country_code"].ToString(),
                            fax = dt["fax"].ToString(),
                            country_code1 = dt["country_code1"].ToString(),
                            area_code1 = dt["area_code1"].ToString(),
                            country_code2 = dt["country_code2"].ToString(),
                            area_code2 = dt["area_code2"].ToString(),
                            phone2 = dt["phone2"].ToString(),

                        });
                        values.leadbank_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding lead bank !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetleadbankcontacteditsSummary(string leadbankcontact_gid, MdlLeadBank values)
        {
            try
            {
                msSQL = " select leadbankbranch_name,leadbank_gid,leadbankcontact_gid,leadbankcontact_name,designation,mobile,email,fax,fax_country_code," +
                       " fax_area_code,phone1,country_code1,area_code1,phone2,country_code2,area_code2 " +
                       " from crm_trn_tleadbankcontact " +
                       " where leadbankcontact_gid ='" + leadbankcontact_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadbank_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadbank_list
                        {

                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            designation = dt["designation"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            email = dt["email"].ToString(),
                            fax_country_code = dt["fax_country_code"].ToString(),
                            fax_area_code = dt["fax_area_code"].ToString(),
                            fax = dt["fax"].ToString(),
                            phone1 = dt["phone1"].ToString(),
                            phone2 = dt["phone2"].ToString(),
                            country_code1 = dt["country_code1"].ToString(),
                            country_code2 = dt["country_code2"].ToString(),
                            area_code1 = dt["area_code1"].ToString(),
                            area_code2 = dt["area_code2"].ToString(),

                        });
                        values.leadbank_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank edit summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetleadbankviewSummary(string leadbank_gid, MdlLeadBank values)
        {
            try
            {
                msSQL = " select a.referred_by,ref.leadbank_name as referred_by_name,a.remarks,b.wedding_day,b.birthday,b.interests,a.leadbank_name, b.leadbankcontact_name, " +
                         " b.designation, b.mobile, b.email, a.company_website, b.fax_area_code , b.fax_country_code,  " +
                         " b.country_code1, b.area_code1, b.phone1,b.fax, b.country_code2, b.area_code2, b.phone2, a.leadbank_address1,  " +
                         " a.leadbank_address2, a.leadbank_city, a.leadbank_state, a.leadbank_pin, a.leadbank_country,d.country_name, " +
                         " c.source_name, concat(f.region_name,'/',f.city) as region_name, e.categoryindustry_name ,a.leadbank_code,b.leadbankbranch_name,a.customer_type,g.leadtype_name as lead_type from crm_trn_tleadbank a " +
                         " left join crm_trn_tleadbankcontact b on a.leadbank_gid = b.leadbank_gid " +
                         " left join crm_mst_tsource c on c.source_gid = a.source_gid " +
                         " left join adm_mst_tcountry d on d.country_gid = a.leadbank_country   " +
                         " left join crm_mst_tcategoryindustry e on e.categoryindustry_gid = a.categoryindustry_gid " +
                         " left join crm_mst_tregion f on f.region_gid = a.leadbank_region " +
                         " left join crm_mst_tleadtype g on g.leadtype_gid = a.lead_type " +
                         " left join crm_trn_tleadbank ref on a.referred_by = ref.leadbank_gid " +
                         " where a.leadbank_gid ='" + leadbank_gid + "' and  b.main_contact='Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadbank_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        {
                            getModuleList.Add(new leadbank_list
                            {
                                referred_by = dt["referred_by_name"].ToString(),
                                remarks = dt["remarks"].ToString(),
                                leadbank_name = dt["leadbank_name"].ToString(),
                                leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                                designation = dt["designation"].ToString(),
                                mobile = dt["mobile"].ToString(),
                                email = dt["email"].ToString(),
                                company_website = dt["company_website"].ToString(),
                                fax_area_code = dt["fax_area_code"].ToString(),
                                fax_country_code = dt["fax_country_code"].ToString(),
                                fax = dt["fax"].ToString(),
                                country_code1 = dt["country_code1"].ToString(),
                                area_code1 = dt["area_code1"].ToString(),
                                phone1 = dt["phone1"].ToString(),
                                country_code2 = dt["country_code2"].ToString(),
                                area_code2 = dt["area_code2"].ToString(),
                                phone2 = dt["phone2"].ToString(),
                                leadbank_address1 = dt["leadbank_address1"].ToString(),
                                leadbank_address2 = dt["leadbank_address2"].ToString(),
                                leadbank_city = dt["leadbank_city"].ToString(),
                                leadbank_state = dt["leadbank_state"].ToString(),
                                leadbank_pin = dt["leadbank_pin"].ToString(),
                                customer_type = dt["customer_type"].ToString(),
                                country_name = dt["country_name"].ToString(),
                                source_name = dt["source_name"].ToString(),
                                region_name = dt["region_name"].ToString(),
                                categoryindustry_name = dt["categoryindustry_name"].ToString(),
                                leadbank_code = dt["leadbank_code"].ToString(),
                                leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                                weddingday = dt["wedding_day"].ToString(),
                                birthday = dt["birthday"].ToString(),
                                interests = dt["interests"].ToString(),
                                lead_type = dt["lead_type"].ToString(),
                            });
                            values.leadbank_list = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank view summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetleadbankviewSummary1(string leadbank_gid, MdlLeadBank values)
        {
            try
            {
                msSQL = " select a.referred_by, a.remarks, a.leadbank_name, b.leadbankcontact_name, " +
                         " b.designation, b.mobile, b.email, a.company_website, b.fax_area_code , b.fax_country_code,  " +
                         " b.country_code1, b.area_code1, b.phone1,b.fax, b.country_code2, b.area_code2, b.phone2, a.leadbank_address1,  " +
                         " a.leadbank_address2, a.leadbank_city, a.leadbank_state, a.leadbank_pin, a.leadbank_country,d.country_name, " +
                         " c.source_name, concat(b.region_name,'/',f.city) as region_name, e.categoryindustry_name ,a.leadbank_code,b.leadbankbranch_name, customer_type from crm_trn_tleadbank a " +
                         " left join crm_trn_tleadbankcontact b on a.leadbank_gid = b.leadbank_gid " +
                         " left join crm_mst_tsource c on c.source_gid = a.source_gid " +
                         " left join adm_mst_tcountry d on d.country_gid = a.leadbank_country   " +
                         " left join crm_mst_tcategoryindustry e on e.categoryindustry_gid = a.categoryindustry_gid " +
                          " left join crm_mst_tregion f on f.region_gid = a.leadbank_region " +
                         " where a.leadbank_gid ='" + leadbank_gid + "' and  b.main_contact='Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadbank_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        {
                            getModuleList.Add(new leadbank_list1
                            {
                                referred_by = dt["referred_by"].ToString(),
                                remarks = dt["remarks"].ToString(),
                                leadbank_name = dt["leadbank_name"].ToString(),
                                leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                                designation = dt["designation"].ToString(),

                                mobile = dt["mobile"].ToString(),
                                email = dt["email"].ToString(),
                                company_website = dt["company_website"].ToString(),
                                fax_area_code = dt["fax_area_code"].ToString(),
                                fax_country_code = dt["fax_country_code"].ToString(),
                                fax = dt["fax"].ToString(),

                                country_code1 = dt["country_code1"].ToString(),
                                area_code1 = dt["area_code1"].ToString(),
                                phone1 = dt["phone1"].ToString(),
                                country_code2 = dt["country_code2"].ToString(),
                                area_code2 = dt["area_code2"].ToString(),

                                phone2 = dt["phone2"].ToString(),
                                leadbank_address1 = dt["leadbank_address1"].ToString(),
                                leadbank_address2 = dt["leadbank_address2"].ToString(),
                                leadbank_city = dt["leadbank_city"].ToString(),
                                leadbank_state = dt["leadbank_state"].ToString(),

                                leadbank_pin = dt["leadbank_pin"].ToString(),
                                customer_type = dt["customer_type"].ToString(),
                                country_name = dt["country_name"].ToString(),
                                source_name = dt["source_name"].ToString(),
                                region_name = dt["region_name"].ToString(),

                                categoryindustry_name = dt["categoryindustry_name"].ToString(),
                                leadbank_code = dt["leadbank_code"].ToString(),
                                leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                            });
                            values.leadbank_list1 = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank view summary1!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetleadbankviewSummary2(string leadbank_gid, MdlLeadBank values)
        {
            try
            {
                msSQL = " select a.referred_by, a.remarks, a.leadbank_name, b.leadbankcontact_name, " +
                         " b.designation, b.mobile, b.email, a.company_website, b.fax_area_code , b.fax_country_code,  " +
                         " b.country_code1, b.area_code1, b.phone1,b.fax, b.country_code2, b.area_code2, b.phone2, a.leadbank_address1,  " +
                         " a.leadbank_address2, a.leadbank_city, a.leadbank_state, a.leadbank_pin, a.leadbank_country,d.country_name, " +
                         " c.source_name, concat(b.region_name,'/',f.city) as region_name, e.categoryindustry_name ,a.leadbank_code,b.leadbankbranch_name ,customer_type from crm_trn_tleadbank a " +
                         " left join crm_trn_tleadbankcontact b on a.leadbank_gid = b.leadbank_gid " +
                         " left join crm_mst_tsource c on c.source_gid = a.source_gid " +
                         " left join adm_mst_tcountry d on d.country_gid = a.leadbank_country   " +
                         " left join crm_mst_tcategoryindustry e on e.categoryindustry_gid = a.categoryindustry_gid " +
                          " left join crm_mst_tregion f on f.region_gid = a.leadbank_region " +
                         " where a.leadbank_gid ='" + leadbank_gid + "' and  b.main_contact='Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadbank_list2>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        {
                            getModuleList.Add(new leadbank_list2
                            {
                                referred_by = dt["referred_by"].ToString(),
                                remarks = dt["remarks"].ToString(),
                                leadbank_name = dt["leadbank_name"].ToString(),
                                leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                                designation = dt["designation"].ToString(),

                                mobile = dt["mobile"].ToString(),
                                email = dt["email"].ToString(),
                                company_website = dt["company_website"].ToString(),
                                fax_area_code = dt["fax_area_code"].ToString(),
                                fax_country_code = dt["fax_country_code"].ToString(),
                                fax = dt["fax"].ToString(),

                                country_code1 = dt["country_code1"].ToString(),
                                area_code1 = dt["area_code1"].ToString(),
                                phone1 = dt["phone1"].ToString(),
                                country_code2 = dt["country_code2"].ToString(),
                                area_code2 = dt["area_code2"].ToString(),

                                phone2 = dt["phone2"].ToString(),
                                leadbank_address1 = dt["leadbank_address1"].ToString(),
                                leadbank_address2 = dt["leadbank_address2"].ToString(),
                                leadbank_city = dt["leadbank_city"].ToString(),
                                leadbank_state = dt["leadbank_state"].ToString(),

                                leadbank_pin = dt["leadbank_pin"].ToString(),
                                customer_type = dt["customer_type"].ToString(),
                                country_name = dt["country_name"].ToString(),
                                source_name = dt["source_name"].ToString(),
                                region_name = dt["region_name"].ToString(),

                                categoryindustry_name = dt["categoryindustry_name"].ToString(),
                                leadbank_code = dt["leadbank_code"].ToString(),
                                leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                            });
                            values.leadbank_list2 = getModuleList;
                        }
                    }
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank view summary2!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetLeadReportExport(MdlLeadBank values)
        {
            try
            {
                msSQL = " SELECT distinct a.leadbank_gid,a.leadbank_name,a.remarks,b.leadbankcontact_name,b.mobile," +
                    "b.email,d.region_name,a.leadbank_address1,a.leadbank_address2,a.leadbank_city,a.leadbank_state," +
                    "a.leadbank_pin,f.country_name,date_format(a.created_date,'%d-%m-%Y') as created_date,e.source_name " +
                    " from crm_trn_tleadbank a " +
                    " left join crm_trn_tleadbankcontact b on a.leadbank_gid=b.leadbank_gid " +
                    " left join crm_mst_tregion d on a.leadbank_region=d.region_gid " +
                    " left join crm_mst_tsource e on a.source_gid=e.source_gid " +
                    " left join adm_mst_tuser c on  c.user_gid = a.created_by " +
                    " left join adm_mst_tcountry f on a.leadbank_country=f.country_gid" +
                    " where a.main_branch='Y' and a.main_contact='Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                string lscompany_code = string.Empty;
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Lead Report");
                try
                {
                    msSQL = " select company_code from adm_mst_tcompany";

                    lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                    string lspath = ConfigurationManager.AppSettings["exportexcelfile"] + "/leadbank/export" + "/" + lscompany_code + "/" + "Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                    //values.lspath = ConfigurationManager.AppSettings["file_path"] + "/erp_documents" + "/" + lscompany_code + "/" + "SDC/TestReport/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                    {
                        if ((!System.IO.Directory.Exists(lspath)))
                            System.IO.Directory.CreateDirectory(lspath);
                    }

                    string lsname2 = "Lead_Report" + DateTime.Now.ToString("(dd-MM-yyyy HH-mm-ss)") + ".xlsx";
                    string lspath1 = ConfigurationManager.AppSettings["exportexcelfile"] + "/leadbank/export" + "/" + lscompany_code + "/" + "Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + lsname2;

                    workSheet.Cells[1, 1].LoadFromDataTable(dt_datatable, true);
                    FileInfo file = new FileInfo(lspath1);
                    using (var range = workSheet.Cells[1, 1, 1, 8])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        range.Style.Font.Color.SetColor(Color.White);
                    }
                    excel.SaveAs(file);

                    var getModuleList = new List<leadexport_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {

                        getModuleList.Add(new leadexport_list
                        {
                            lsname2 = lsname2,
                            lspath1 = lspath1,
                        });
                        values.leadexport_list = getModuleList;

                    }
                    dt_datatable.Dispose();
                    values.status = true;
                    values.message = "Success";
                }
                catch (Exception ex)
                {
                    values.status = false;
                    values.message = "Failure";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while exporting lead report!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaLeadReportImport(HttpRequest httpRequest, string user_gid, result objResult, leadbank_list values)
        {
            try
            {
                msSQL = "select employee_gid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
                string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                HttpFileCollection httpFileCollection;
                string lspath, lsfilePath;

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                // Create Directory
                lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"] + lscompany_code + "/" + " Import_Excel/Sales_Module/" + "Product_Master/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                if (!Directory.Exists(lsfilePath))
                    Directory.CreateDirectory(lsfilePath);

                httpFileCollection = httpRequest.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    httpPostedFile = httpFileCollection[i];
                }
                string FileExtension = httpPostedFile.FileName;

                if (FileExtension.IndexOf("Lead Template", StringComparison.OrdinalIgnoreCase) >= 0)
                {



                    msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                    string lsfile_gid = msdocument_gid;
                    FileExtension = Path.GetExtension(FileExtension).ToLower();
                    lsfile_gid = lsfile_gid + FileExtension;
                    FileInfo fileinfo = new FileInfo(lsfilePath);
                    Stream ls_readStream;
                    ls_readStream = httpPostedFile.InputStream;
                    MemoryStream ms = new MemoryStream();
                    ls_readStream.CopyTo(ms);

                    //path creation        
                    lspath = lsfilePath + "/";
                    FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
                    ms.WriteTo(file);

                    try
                    {
                        string status;
                        status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                        file.Close();
                        ms.Close();
                    }
                    catch (Exception ex)
                    {
                        objResult.status = false;
                        objResult.message = ex.ToString();
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                        return;
                    }

                    try
                    {
                        DataTable dataTable = new DataTable();
                        int totalSheet = 1;
                        string connectionString = string.Empty;
                        string fileExtension = Path.GetExtension(lspath);

                        lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";

                        string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");

                        try
                        {
                            connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + correctedPath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0';";
                        }
                        catch (Exception ex)
                        {
                            objResult.status = false;
                            objResult.message = ex.Message;
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                            return;
                        }
                        using (OleDbConnection connection = new OleDbConnection(connectionString))
                        {
                            connection.Open();
                            DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            if (schemaTable != null)
                            {
                                var tempDataTable = (from dataRow in schemaTable.AsEnumerable()
                                                     where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                                     select dataRow).CopyToDataTable();

                                schemaTable = tempDataTable;
                                totalSheet = schemaTable.Rows.Count;

                                using (OleDbCommand command = new OleDbCommand())
                                {
                                    command.Connection = connection;
                                    command.CommandText = "select * from [Sheet1$]";

                                    using (OleDbDataReader reader = command.ExecuteReader())
                                    {
                                        dataTable.Load(reader);
                                    }
                                    foreach (DataRow row in dataTable.Rows)
                                    {
                                        string leadbank_name = row["Company Name"].ToString();
                                        string customertype = row["Customer Type"].ToString();
                                        string remarks = row["Remarks"].ToString();
                                        string leadbankcontact_name = row["RM Name"].ToString();
                                        string country_code = row["Country Code"].ToString();
                                        string mobile = row["Mobile Number"].ToString();
                                        string email = row["Email Address"].ToString();
                                        string region_name = row["Region"].ToString();
                                        string leadbank_address1 = row["Address Line 1"].ToString();
                                        string leadbank_address2 = row["Address Line 2"].ToString();
                                        string leadbank_city = row["City"].ToString();
                                        string leadbank_state = row["State"].ToString();
                                        string leadbank_pin = row["Postal Code"].ToString();
                                        string country_name = row["Country"].ToString();
                                        //string created_date = row["Created Date"].ToString();
                                        string source_name = row["Source"].ToString();
                                        string fi = row["FI"].ToString();
                                        string Constitution = row["Constitution"].ToString();
                                        string Website = row["Website"].ToString();
                                        string regiongid = objcmnfunctions.GetMasterGID("BRNM");
                                        string sourcegid = objcmnfunctions.GetMasterGID("BSEM");

                                        string mobile_number = country_code + mobile;

                                        msSQL = " select leadbank_gid from crm_trn_tleadbank where leadbank_name='" + leadbank_name.Replace("'", "\\'") + "';";
                                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                        if (objOdbcDataReader.HasRows)
                                        {
                                            string lsleadbank_gid = objOdbcDataReader["leadbank_gid"].ToString();

                                        }


                                        if (objOdbcDataReader.HasRows == false)
                                        {
                                            objOdbcDataReader.Close();
                                            if (leadbank_name != "")
                                            {


                                                msGetGid = objcmnfunctions.GetMasterGID("BMCC");
                                                msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

                                                msSQL = " INSERT INTO crm_trn_tleadbank" +
                                                " (leadbank_gid, " +
                                                 " leadbank_name," +
                                                 " customer_type," +
                                                 " FI," +
                                                 " remarks," +
                                                 " leadbank_address1," +
                                                 " leadbank_address2," +
                                                 " leadbank_city," +
                                                 " leadbank_state," +
                                                 " leadbank_pin," +
                                                 " constitution_gid," +
                                                 " constitution," +
                                                 " company_website," +
                                                 " leadbank_country," +
                                                 " created_by," +
                                                 " status," +
                                                 " approval_flag," +
                                                 " leadbank_code," +
                                                 " main_branch," +
                                                 " main_contact," +
                                                 " lead_status," +
                                                 " source_gid," +
                                                 " leadbank_region," +
                                                 " created_date)" +
                                                 " values( " +
                                                " '" + msGetGid1 + "'," +
                                                 " '" + leadbank_name.Replace("'", "\\'") + "'," +
                                                 " '" + customertype + "'," +
                                                 " '" + fi + "'," +
                                                 " '" + (String.IsNullOrEmpty(remarks) ? remarks : remarks.Replace("'", "\\'")) + "'," +
                                                 " '" + (String.IsNullOrEmpty(leadbank_address1) ? leadbank_address1 : leadbank_address1.Replace("'", "\\'")) + "'," +
                                                 " '" + (String.IsNullOrEmpty(leadbank_address2) ? leadbank_address2 : leadbank_address2.Replace("'", "\\'")) + "'," +
                                                 " '" + (String.IsNullOrEmpty(leadbank_city) ? leadbank_city : leadbank_city.Replace("'", "\\'")) + "'," +
                                                 " '" + (String.IsNullOrEmpty(leadbank_state) ? leadbank_state : leadbank_state.Replace("'", "\\'")) + "'," +
                                                 " '" + leadbank_pin + "'," +
                                                 " (select constitution_gid from crm_mst_tconstitution where constitution_name = '" + (string.IsNullOrEmpty(Constitution) ? Constitution : Constitution.Replace("'", "\\'")) + "')," +
                                                 " '" + (String.IsNullOrEmpty(Constitution) ? Constitution : Constitution.Replace("'", "\\'")) + "'," +
                                                 " '" + Website + "'," +
                                                 " (select country_gid from adm_mst_tcountry where country_name = '" + (String.IsNullOrEmpty(country_name) ? country_name : country_name.Replace("'", "\\'")) + "')," +
                                                 " '" + lsemployee_gid + "'," +
                                                 " 'Y'," +
                                                " '" + msGetGid + "'," +
                                                 " 'Approved'," +
                                                 " 'H.Q'," +
                                                 " 'Y'," +
                                                 " 'Y',";
                                                if (source_name != "" && source_name != null)
                                                {
                                                    msSQL1 = " select source_gid from crm_mst_tsource where source_name = '" + (String.IsNullOrEmpty(source_name) ? source_name : source_name.Replace("'", "\\'")) + "'";
                                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL1);

                                                    if (objOdbcDataReader.HasRows == false)
                                                    {
                                                        objOdbcDataReader.Close();
                                                        msSQL1 = " Select sequence_curval from adm_mst_tsequence where sequence_code ='BSEM' order by finyear desc limit 0,1 ";
                                                        string lsCode = objdbconn.GetExecuteScalar(msSQL1);
                                                        string lssource_code = "SCM" + "000" + lsCode;
                                                        msSQL1 = " insert into crm_mst_tsource(" +
                                                                " source_gid," +
                                                                " source_code," +
                                                                " source_name," +
                                                                " created_by, " +
                                                                " created_date)" +
                                                                " values(" +
                                                                " '" + sourcegid + "'," +
                                                                " '" + lssource_code + "'," +
                                                                "'" + (String.IsNullOrEmpty(source_name) ? source_name : source_name.Replace("'", "\\'")) + "'," +
                                                                "'" + user_gid + "'," +
                                                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
                                                        msSQL += " '" + sourcegid + "', ";
                                                    }
                                                    else
                                                    {
                                                        msSQL += " (select source_gid from crm_mst_tsource where source_name = '" + (String.IsNullOrEmpty(source_name) ? source_name : source_name.Replace("'", "\\'")) + "'),";
                                                    }
                                                }
                                                else
                                                {
                                                    msSQL += " (select source_gid from crm_mst_tsource where source_name = '" + (String.IsNullOrEmpty(source_name) ? source_name : source_name.Replace("'", "\\'")) + "'),";
                                                }
                                                if (region_name != "")
                                                {
                                                    msSQL1 = "select region_gid from crm_mst_tregion where region_name = '" + (String.IsNullOrEmpty(region_name) ? region_name : region_name.Replace("'", "\\'")) + "' and city= '" + (String.IsNullOrEmpty(leadbank_city) ? leadbank_city : leadbank_city.Replace("'", "\\'")) + "'";
                                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL1);
                                                    if (objOdbcDataReader.HasRows == false)
                                                    {
                                                        objOdbcDataReader.Close();
                                                        msSQL1 = " Select sequence_curval from adm_mst_tsequence where sequence_code ='BRNM' order by finyear desc limit 0,1 ";
                                                        string lsCode = objdbconn.GetExecuteScalar(msSQL1);
                                                        string lsregion_code = "BRN" + "000" + lsCode;
                                                        msSQL1 = " insert into crm_mst_tregion(" +
                                                                " region_gid," +
                                                                " region_code," +
                                                                " region_name," +
                                                                " city," +
                                                                " created_by," +
                                                                " created_date)" +
                                                                " values(" +
                                                                " '" + regiongid + "'," +
                                                                " '" + lsregion_code + "'," +
                                                                "'" + (String.IsNullOrEmpty(region_name) ? region_name : region_name.Replace("'", "\\'")) + "'," +
                                                                " '" + (String.IsNullOrEmpty(leadbank_city) ? leadbank_city : leadbank_city.Replace("'", "\\'")) + "'," +
                                                                "'" + user_gid + "'," +
                                                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL1);
                                                        msSQL += " '" + regiongid + "', ";
                                                    }
                                                    else
                                                    {
                                                        if (leadbank_city == "")
                                                        {
                                                            msSQL += " (select region_gid from crm_mst_tregion where region_name = '" + (String.IsNullOrEmpty(region_name) ? region_name : region_name.Replace("'", "\\'")) + "'),";
                                                        }
                                                        else
                                                        {
                                                            msSQL += " (select region_gid from crm_mst_tregion where region_name = '" + (String.IsNullOrEmpty(region_name) ? region_name : region_name.Replace("'", "\\'")) + "' and city = '" + (String.IsNullOrEmpty(leadbank_city) ? leadbank_city : leadbank_city.Replace("'", "\\'")) + "'),";

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    msSQL += " '', ";
                                                }
                                                msSQL +=
                                                 " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                                mnResult12 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                if (mnResult12 == 1)
                                                {
                                                    msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");

                                                    msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                                                        " (leadbankcontact_gid," +
                                                       " leadbankcontact_name," +
                                                                                " leadbank_gid," +
                                                                                   " mobile," +
                                                                                   " email," +
                                                                                  " region_name," +
                                                                                   " created_by," +
                                                                                   " address1," +
                                                                                   " address2," +
                                                                                   " main_contact," +
                                                                                   " pincode," +
                                                                                   " country_gid," +
                                                                                   "state," +
                                                                                   " city," +
                                                                                   " created_date)" +
                                                                                   " values( " +
                                                                                   " '" + msGetGid2 + "'," +
                                                                                   " '" + (String.IsNullOrEmpty(leadbankcontact_name) ? leadbankcontact_name : leadbankcontact_name.Replace("'", "\\'")) + "'," +
                                                                                   " '" + msGetGid1 + "'," +
                                                                                   " '" + mobile_number + "'," +
                                                                                   " '" + email + "',";
                                                    if (leadbank_city == "")
                                                    {
                                                        msSQL += " '" + (String.IsNullOrEmpty(region_name) ? region_name : region_name.Replace("'", "\\'")) + "', ";
                                                    }
                                                    else
                                                    {
                                                        msSQL += " '" + (String.IsNullOrEmpty(region_name) ? region_name : region_name.Replace("'", "\\'")) + "', ";

                                                    }
                                                    msSQL += " '" + user_gid + "'," +
                                                    " '" + (String.IsNullOrEmpty(leadbank_address1) ? leadbank_address1 : leadbank_address1.Replace("'", "\\'")) + "'," +
                                                    " '" + (String.IsNullOrEmpty(leadbank_address2) ? leadbank_address2 : leadbank_address2.Replace("'", "\\'")) + "'," +
                                                    "'Y'," +
                                                    "'" + leadbank_pin + "'," +
                                                    "'" + (String.IsNullOrEmpty(country_name) ? country_name : country_name.Replace("'", "\\'")) + "'," +
                                                    "'" + (String.IsNullOrEmpty(leadbank_state) ? leadbank_state : leadbank_state.Replace("'", "\\'")) + "'," +
                                                    "'" + (String.IsNullOrEmpty(leadbank_city) ? leadbank_city : leadbank_city.Replace("'", "\\'")) + "'," +
                                                   " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                                                    mnResult12 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                    objOdbcDataReader.Close();
                                                }
                                                if (mobile != "" && mobile != null)
                                                {
                                                    msSQL1 = "select  SUBSTRING_INDEX(leadbank_name, ' ', 1) AS firstName," +
                                                            "CASE WHEN LOCATE(' ', leadbank_name) > 0 THEN SUBSTRING_INDEX(leadbank_name, ' ', -1)ELSE ''END AS lastName" +
                                                            " from crm_trn_tleadbank where leadbank_gid = '" + msGetGid1 + "'";
                                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL1);
                                                    if (objOdbcDataReader.HasRows == true)
                                                    {

                                                        string firstName = objOdbcDataReader["firstName"].ToString();
                                                        string lastName = objOdbcDataReader["lastName"].ToString();
                                                        objOdbcDataReader.Close();

                                                        Rootobject objRootobject = new Rootobject();
                                                        string contactjson = "{\"displayName\":\"" + leadbank_name + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + mobile_number + "\"}],\"firstName\":\"" + firstName + "\",\"lastName\":\"" + lastName + "\"}";
                                                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                                                        var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
                                                        var request = new RestRequest(ConfigurationManager.AppSettings["messagebirdcontact"].ToString(), Method.POST);
                                                        request.AddHeader("authorization", ConfigurationManager.AppSettings["messagebirdaccesskey"].ToString());
                                                        request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                                                        IRestResponse response = client.Execute(request);
                                                        var responseoutput = response.Content;
                                                        objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);
                                                        if (response.StatusCode == HttpStatusCode.Created)
                                                        {
                                                            msSQL = "insert into crm_smm_whatsapp(leadbank_gid,leadbankcontact_gid,id,wkey,wvalue,displayName,firstName,lastName,created_date,created_by)values(" +
                                                                    " '" + msGetGid1 + "'," +
                                                                    " '" + msGetGid2 + "'," +
                                                                    "'" + objRootobject.id + "'," +
                                                                    "'phonenumber'," +
                                                                    "'" + mobile_number + "'," +
                                                                    "'" + (String.IsNullOrEmpty(leadbank_name) ? leadbank_name : leadbank_name.Replace("'", "\\'")) + "'," +
                                                                    "'" + (String.IsNullOrEmpty(firstName) ? firstName : firstName.Replace("'", "\\'")) + "'," +
                                                                    "'" + (String.IsNullOrEmpty(lastName) ? lastName : lastName.Replace("'", "\\'")) + "'," +
                                                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                    "'" + user_gid + "')";
                                                            mnResult12 = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                        }
                                                    }
                                                }


                                            }
                                            else
                                            {
                                                objResult.status = false;
                                                objResult.message = "Company Name Is Mandatory";
                                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + values.message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                                            }
                                            objOdbcDataReader.Close();

                                        }

                                        if (mnResult12 != 0)
                                        {
                                            objResult.status = true;
                                            objResult.message = "Lead Imported Successfully";
                                        }
                                        else
                                        {
                                            objResult.status = false;
                                            objResult.message = "Error While Adding Lead";
                                        }

                                        objOdbcDataReader.Close();
                                    }

                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        objResult.status = false;
                        objResult.message = ex.Message;
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                        return;
                    }
                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Invalid file format. Please upload a valid Excel file";
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + objResult.message + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                }
            }
            catch (Exception ex)
            {
                objResult.status = false;
                objResult.message = ex.Message;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                return;
            }

        }

        //public void DaLeadReportImport(HttpRequest httpRequest, string user_gid, result objResult, leadbank_list values)
        //{
        //    string lscompany_code;
        //    string excelRange, endRange;
        //    int rowCount, columnCount;


        //    try
        //    {
        //        int insertCount = 0;
        //        HttpFileCollection httpFileCollection;
        //        DataTable dt = null;
        //        string lspath, lsfilePath;

        //        msSQL = " select company_code from adm_mst_tcompany";
        //        lscompany_code = objdbconn.GetExecuteScalar(msSQL);

        //        // Create Directory
        //        lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"];

        //        if (!Directory.Exists(lsfilePath))
        //            Directory.CreateDirectory(lsfilePath);

        //        httpFileCollection = httpRequest.Files;
        //        for (int i = 0; i < httpFileCollection.Count; i++)
        //        {
        //            httpPostedFile = httpFileCollection[i];
        //        }
        //        string FileExtension = httpPostedFile.FileName;
        //        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
        //        string lsfile_gid = msdocument_gid;
        //        FileExtension = Path.GetExtension(FileExtension).ToLower();
        //        lsfile_gid = lsfile_gid + FileExtension;
        //        FileInfo fileinfo = new FileInfo(lsfilePath);
        //        Stream ls_readStream;
        //        ls_readStream = httpPostedFile.InputStream;
        //        MemoryStream ms = new MemoryStream();
        //        ls_readStream.CopyTo(ms);

        //        //path creation        
        //        lspath = lsfilePath + "/";
        //        FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
        //        ms.WriteTo(file);
        //        try
        //        {


        //            string status;
        //            status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
        //            file.Close();
        //            ms.Close();
        //            objcmnfunctions.uploadFile(lspath, lsfile_gid);
        //        }
        //        catch (Exception ex)
        //        {
        //            objResult.status = false;
        //            objResult.message = ex.ToString();
        //            return;
        //        }

        //        try
        //        {


        //            if (httpRequest.Files.Count > 0)
        //            {
        //                string lsfirstdocument_filepath = string.Empty;
        //                httpFileCollection = httpRequest.Files;
        //                for (int i = 0; i < httpFileCollection.Count; i++)
        //                {
        //                    //string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
        //                    //httpPostedFile = httpFileCollection[i];
        //                    //string FileExtension = httpPostedFile.FileName;
        //                    //string lsfile_gid = msdocument_gid + FileExtension;
        //                    //string lsfile_gid = msdocument_gid;
        //                    string lscompany_document_flag = string.Empty;
        //                    //FileExtension = Path.GetExtension(FileExtension).ToLower();
        //                    //lsfile_gid = lsfile_gid + FileExtension;
        //                    //Stream ls_readStream;
        //                    //ls_readStream = httpPostedFile.InputStream;
        //                    //ls_readStream.CopyTo(ms);

        //                    bool status1;


        //                    status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Product/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
        //                    ms.Close();

        //                    final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Product/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
        //                    //msSQL = "update pmr_mst_tproduct set " +
        //                    //                               " product_image='" + final_path + msdocument_gid + FileExtension + "'" +
        //                    //                                " where product_gid='" + product_gid + "'";

        //                    //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            objResult.status = false;
        //            objResult.message = ex.ToString();
        //            return;
        //        }

        //        //Excel To DataTable
        //        try
        //        {
        //            DataTable dataTable = new DataTable();
        //            int totalSheet = 1;
        //            string connectionString = string.Empty;
        //            string fileExtension = Path.GetExtension(lspath);
        //            lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";
        //            string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");
        //            string sheetName;
        //            try
        //            {
        //                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + correctedPath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0';";
        //            }
        //            catch (Exception ex)
        //            {

        //            }

        //            using (OleDbConnection connection = new OleDbConnection(connectionString))
        //            {
        //                connection.Open();
        //                DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        //                if (schemaTable != null)
        //                {
        //                    var tempDataTable = (from dataRow in schemaTable.AsEnumerable()
        //                                         where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
        //                                         select dataRow).CopyToDataTable();

        //                    schemaTable = tempDataTable;
        //                    totalSheet = schemaTable.Rows.Count;
        //                    using (OleDbCommand command = new OleDbCommand())
        //                    {
        //                        command.Connection = connection;
        //                        command.CommandText = "select * from [Sheet1$]";

        //                        using (OleDbDataReader reader = command.ExecuteReader())
        //                        {
        //                            dataTable.Load(reader);
        //                        }
        //                        foreach (DataRow row in dataTable.Rows)
        //                        {


        //                            //string leadbank_id = row["leadbank_id"].ToString();
        //                            string leadbank_name = row["Leadbank Name"].ToString();
        //                            string remarks = row["Remarks"].ToString();
        //                            string leadbankcontact_name = row["Leadbankcontact Name"].ToString();
        //                            string mobile = row["Mobile"].ToString();
        //                            string email = row["Email"].ToString();
        //                            string region_name = row["Region Name"].ToString();
        //                            string leadbank_address1 = row["Lead Address 1"].ToString();
        //                            string leadbank_address2 = row["Lead Address 2"].ToString();
        //                            string leadbank_city = row["Lead City"].ToString();
        //                            string leadbank_state = row["Lead State"].ToString();
        //                            string leadbank_pin = row["Lead Pin"].ToString();
        //                            string country_name = row["Country Name"].ToString();
        //                            string created_date = row["Created Date"].ToString();
        //                            string source_name = row["Source Name"].ToString();
        //                            string customer_type = row["Customer Type"].ToString();

        //                            msSQL = "select country_gid from adm_mst_tcountry where country_name = '" + country_name + "'";
        //                            string lscountry_gid = objdbconn.GetExecuteScalar(msSQL);
        //                            if (lscountry_gid == "")
        //                            {
        //                                values.message = "";
        //                            }
        //                            else
        //                            {

        //                                lscountry_gid = lscountry_gid;
        //                            }

        //                            msSQL = "select source_gid from crm_mst_tsource where source_name = '" + source_name + "'";
        //                            string lssource_gid = objdbconn.GetExecuteScalar(msSQL);

        //                            if (lssource_gid == "")
        //                            {
        //                                values.message = "";
        //                            }
        //                            else
        //                            {
        //                                lssource_gid = lssource_gid;
        //                            }
        //                            {

        //                                //insertion in table
        //                                msGetGid = objcmnfunctions.GetMasterGID("BMCC");
        //                                msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

        //                                msSQL = " INSERT INTO crm_trn_tleadbank" +
        //                            " (leadbank_gid, " +
        //                            " leadbank_name," +
        //                            " customer_type," +
        //                            " remarks," +
        //                            " leadbank_address1," +
        //                            " leadbank_address2," +
        //                            " leadbank_city," +
        //                            " leadbank_state," +
        //                            " leadbank_pin," +
        //                            " leadbank_country," +
        //                            " source_gid," +
        //                            " created_by," +
        //                            " created_date)" +
        //                            " values( " +
        //                            " '" + msGetGid1 + "'," +
        //                            " '" + leadbank_name + "'," +
        //                            " '" + customer_type + "'," +
        //                            " '" + remarks + "'," +
        //                            " '" + leadbank_address1 + "'," +
        //                            " '" + leadbank_address2 + "'," +
        //                            " '" + leadbank_city + "'," +
        //                            " '" + leadbank_state + "'," +
        //                            " '" + leadbank_pin + "'," +
        //                            " '" + lscountry_gid + "'," +
        //                            " '" + lssource_gid + "'," +
        //                            " '" + user_gid + "'," +
        //                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

        //                                mnResult12 = objdbconn.ExecuteNonQuerySQL(msSQL);

        //                                if (mnResult12 == 1)
        //                                {
        //                                    msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");

        //                                    msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
        //                               " (leadbankcontact_gid," +
        //                               " leadbankcontact_name," +
        //                               " leadbank_gid," +
        //                               " mobile," +
        //                               " email," +
        //                               " region_name," +
        //                               " created_by," +
        //                               " created_date)" +
        //                               " values( " +
        //                               " '" + msGetGid2 + "'," +
        //                               " '" + leadbankcontact_name + "'," +
        //                               " '" + msGetGid1 + "'," +
        //                               " '" + mobile + "'," +
        //                               " '" + email + "'," +
        //                               " '" + region_name + "'," +
        //                               " '" + user_gid + "'," +
        //                               " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

        //                                    mnResult12 = objdbconn.ExecuteNonQuerySQL(msSQL);

        //                                }


        //                                if (mnResult12 != 0)
        //                                {
        //                                    objResult.status = true;
        //                                    values.message = "Lead Imported Successfully";
        //                                }
        //                                else
        //                                {
        //                                    objResult.status = false;
        //                                    values.message = "Error While Adding Lead";
        //                                }
        //                            }




        //                        }
        //                    }
        //                }
        //            }
        //        }


        //        catch (Exception ex)
        //        {
        //            objResult.status = false;
        //            objResult.message = ex.ToString();
        //            return;
        //        }
        //        //  Nullable<DateTime> ldcodecreation_date;


        //    }
        //    catch (Exception ex)
        //    {
        //        objResult.status = false;
        //        objResult.message = ex.ToString();
        //    }

        //}
        // 28-03
        //public void DaLeadReportImport(HttpRequest httpRequest, string user_gid, result objResult, leadbank_list values)
        //{
        //    try
        //    {

        //        string lscompany_code;
        //        string excelRange, endRange;
        //        int rowCount, columnCount;


        //        try
        //        {
        //            int insertCount = 0;
        //            HttpFileCollection httpFileCollection;
        //            DataTable dt = null;
        //            string lspath, lsfilePath;

        //            msSQL = " select company_code from adm_mst_tcompany";
        //            lscompany_code = objdbconn.GetExecuteScalar(msSQL);

        //            // Create Directory
        //            lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"];

        //            if (!Directory.Exists(lsfilePath))
        //                Directory.CreateDirectory(lsfilePath);

        //            httpFileCollection = httpRequest.Files;
        //            for (int i = 0; i < httpFileCollection.Count; i++)
        //            {
        //                httpPostedFile = httpFileCollection[i];
        //            }
        //            string FileExtension = httpPostedFile.FileName;
        //            string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
        //            string lsfile_gid = msdocument_gid;
        //            FileExtension = Path.GetExtension(FileExtension).ToLower();
        //            lsfile_gid = lsfile_gid + FileExtension;
        //            FileInfo fileinfo = new FileInfo(lsfilePath);
        //            Stream ls_readStream;
        //            ls_readStream = httpPostedFile.InputStream;
        //            MemoryStream ms = new MemoryStream();
        //            ls_readStream.CopyTo(ms);

        //            //path creation        
        //            lspath = lsfilePath + "/";
        //            FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
        //            ms.WriteTo(file);
        //            bool status1;


        //            status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "Lead Import/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);


        //            // Connect to the storage account
        //            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"].ToString());
        //            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        //            CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["blob_containername"].ToString());

        //            // Get a reference to the blob
        //            CloudBlockBlob blockBlob = container.GetBlockBlobReference(lscompany_code + "/" + "Lead Import/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension);
        //            string path_url = lscompany_code + "/" + "Lead Import/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension;


        //            // Download the blob's contents and read Excel file
        //            using (MemoryStream memoryStream = new MemoryStream())
        //            {
        //                // await blockBlob.DownloadToStreamAsync(memoryStream);

        //                blockBlob.DownloadToStream(memoryStream);
        //                memoryStream.Seek(0, SeekOrigin.Begin);
        //                memoryStream.Position = 0;
        //                // Load Excel package from the memory stream
        //                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //                using (ExcelPackage package = new ExcelPackage(memoryStream))
        //                {
        //                    ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"]; // worksheet name
        //                                                                                      // Remove the first row
        //                    worksheet.DeleteRow(1);

        //                    // Convert Excel data to array list format
        //                    List<List<string>> excelData = new List<List<string>>();

        //                    for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
        //                    {
        //                        List<string> rowData = new List<string>();
        //                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //                        {
        //                            var cellValue = worksheet.Cells[row, col].Value?.ToString();
        //                            rowData.Add(cellValue);
        //                        }

        //                        string leadbank_id = rowData[0];
        //                        string leadbank_name = rowData[1];
        //                        string remarks = rowData[2];
        //                        string leadbankcontact_name = rowData[3];
        //                        string mobile = rowData[4];
        //                        string email = rowData[5];
        //                        string region_name = rowData[6];
        //                        string leadbank_address1 = rowData[7];
        //                        string leadbank_address2 = rowData[8];
        //                        string leadbank_city = rowData[9];
        //                        string leadbank_state = rowData[10];
        //                        string leadbank_pin = rowData[11];
        //                        string country_name = rowData[12];
        //                        string source_name = rowData[14];
        //                        string customer_type = rowData[15];

        //                        msSQL = "select leadbank_gid from crm_trn_tleadbank where leadbank_name='" + leadbank_name + "'";
        //                        string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);
        //                        if (lsleadbank_gid == "" && customer_type != "")
        //                        {

        //                            msSQL = "select employee_gid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
        //                            string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

        //                            msSQL = "select country_gid from adm_mst_tcountry where country_name = '" + country_name + "'";
        //                            string lscountry_gid = objdbconn.GetExecuteScalar(msSQL);
        //                            if (lscountry_gid == "")
        //                            {
        //                                values.message = "";
        //                            }
        //                            else
        //                            {

        //                                lscountry_gid = lscountry_gid;
        //                            }

        //                            msSQL = "select source_gid from crm_mst_tsource where source_name = '" + source_name + "'";
        //                            string lssource_gid = objdbconn.GetExecuteScalar(msSQL);

        //                            if (lssource_gid == "")
        //                            {
        //                                values.message = "";
        //                            }
        //                            else
        //                            {
        //                                lssource_gid = lssource_gid;
        //                            }

        //                            msSQL = " Select  region_gid from crm_mst_tregion where  region_name = '" + region_name + "'";
        //                            string lsregion_name = objdbconn.GetExecuteScalar(msSQL);
        //                            if (lsregion_name == "")
        //                            {
        //                                values.message = "";
        //                            }
        //                            else
        //                            {
        //                                lsregion_name = lsregion_name;
        //                            }

        //                            {

        //                                //insertion in table
        //                                msGetGid = objcmnfunctions.GetMasterGID("BMCC");
        //                                msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

        //                                msSQL = " INSERT INTO crm_trn_tleadbank" +
        //                            " (leadbank_gid, " +
        //                            " leadbank_name," +
        //                            " customer_type," +
        //                            " remarks," +
        //                            " leadbank_address1," +
        //                            " leadbank_address2," +
        //                            " leadbank_city," +
        //                            " leadbank_state," +
        //                            " leadbank_pin," +
        //                            " leadbank_country," +
        //                            " source_gid," +
        //                            " created_by," +
        //                            " status," +
        //                            " leadbank_id," +
        //                            " approval_flag," +
        //                            " leadbank_code," +
        //                            " main_branch," +
        //                            " main_contact," +
        //                            " lead_status," +
        //                            " leadbank_region," +
        //                             " cuslead_id," +
        //                            " created_date)" +
        //                            " values( " +
        //                            " '" + msGetGid1 + "'," +
        //                            " '" + leadbank_name + "'," +
        //                            " '" + customer_type + "'," +
        //                            " '" + remarks + "'," +
        //                            " '" + leadbank_address1 + "'," +
        //                            " '" + leadbank_address2 + "'," +
        //                            " '" + leadbank_city + "'," +
        //                            " '" + leadbank_state + "'," +
        //                            " '" + leadbank_pin + "'," +
        //                            " '" + lscountry_gid + "'," +
        //                            " '" + lssource_gid + "'," +
        //                            " '" + lsemployee_gid + "'," +
        //                            " 'Y'," +
        //                            " '" + msGetGid + "'," +
        //                            " 'Approved'," +
        //                            " 'H.Q'," +
        //                            " 'Y'," +
        //                            " 'Y'," +
        //                            " 'Not Assigned'," +
        //                            " '" + lsregion_name + "'," +
        //                            " '" + leadbank_id + "'," +
        //                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

        //                                mnResult12 = objdbconn.ExecuteNonQuerySQL(msSQL);

        //                                if (mnResult12 == 1)
        //                                {
        //                                    msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");

        //                                    msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
        //                               " (leadbankcontact_gid," +
        //                               " leadbankcontact_name," +
        //                               " leadbank_gid," +
        //                               " mobile," +
        //                               " email," +
        //                               " region_name," +
        //                               " created_by," +
        //                               " address1," +
        //                               " address2," +
        //                               " created_date)" +
        //                               " values( " +
        //                               " '" + msGetGid2 + "'," +
        //                               " '" + leadbankcontact_name + "'," +
        //                               " '" + msGetGid1 + "'," +
        //                               " '" + mobile + "'," +
        //                               " '" + email + "'," +
        //                               " '" + region_name + "'," +
        //                               " '" + user_gid + "'," +
        //                               " '" + leadbank_address2 + "'," +
        //                               " '" + user_gid + "'," +
        //                               " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

        //                                    mnResult12 = objdbconn.ExecuteNonQuerySQL(msSQL);

        //                                }


        //                                if (mnResult12 != 0)
        //                                {

        //                                    whatsappconfiguration getwhatsappcredentials = whatsappcredentials();
        //                                    Rootobject objRootobject = new Rootobject();
        //                                    string contactjson = "{\"displayName\":\"" + leadbank_name + "\",\"identifiers\":[{\"key\":\"phonenumber\",\"value\":\"" + mobile + "\"}],\"firstName\":\"" + leadbank_name + "\",\"lastName\":\"" + leadbank_name + "\"}";
        //                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        //                                    var client = new RestClient(ConfigurationManager.AppSettings["messagebirdbaseurl"].ToString());
        //                                    var request = new RestRequest("/workspaces/" + getwhatsappcredentials.workspace_id + "/contacts", Method.POST);
        //                                    request.AddHeader("authorization", "" + getwhatsappcredentials.access_token + "");
        //                                    request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
        //                                    IRestResponse response = client.Execute(request);
        //                                    var responseoutput = response.Content;
        //                                    objRootobject = JsonConvert.DeserializeObject<Rootobject>(responseoutput);
        //                                    if (response.StatusCode == HttpStatusCode.Created)
        //                                    {
        //                                        msSQL = "insert into crm_smm_whatsapp(id,wvalue,displayName,created_date,created_by)values(" +
        //                                        "'" + objRootobject.id + "'," +
        //                                        "'" + mobile + "'," +
        //                                        "'" + leadbank_name + "'," +
        //                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
        //                                        "'" + user_gid + "')";

        //                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                                        if (mnResult == 1)
        //                                        {
        //                                            msSQL = "update crm_smm_whatsapp set leadbank_gid='" + msGetGid1 + "' where id='" + objRootobject.id + "'";
        //                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        //                                            msSQL = "update crm_trn_tleadbank set wh_id='" + objRootobject.id + "' where leadbank_gid='" + msGetGid1 + "'";
        //                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                                        }

        //                                        objResult.status = true;
        //                                        values.message = "Lead Imported Successfully";
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    objResult.status = false;
        //                                    values.message = "Error While Adding Lead";
        //                                }
        //                            }

        //                        }
        //                        else
        //                        {
        //                            objResult.status = false;
        //                            values.message = "Added Lead Already Exist";
        //                        }

        //                    }
        //                    ms.Close();
        //                }
        //            }
        //        }


        //        //List<List<string>> excelData = new List<List<string>>();

        //        //// Loop through the rows and columns to read the data
        //        //for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
        //        //{
        //        //    List<string> rowData = new List<string>();
        //        //    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //        //    {
        //        //        var cellValue = worksheet.Cells[row, col].Value?.ToString();
        //        //        rowData.Add(cellValue);
        //        //    }
        //        //    excelData.Add(rowData);
        //        //}




        //        catch (Exception ex)
        //        {
        //            objResult.status = false;
        //            objResult.message = ex.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while adding lead template";
        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

        //    }



        //}

        //public void DaLeadReportImport(HttpRequest httpRequest, string user_gid, result objResult, leadbank_list values)
        //{
        //    try
        //    {
        //        try
        //        {
        //            msSQL = "select employee_gid from hrm_mst_temployee where user_gid = '" + user_gid + "'";
        //            string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

        //            HttpFileCollection httpFileCollection = httpRequest.Files;
        //            httpPostedFile = httpFileCollection[0];

        //            Stream ls_readStream = httpPostedFile.InputStream;
        //            MemoryStream ms = new MemoryStream();
        //            ls_readStream.CopyTo(ms);

        //            //Load Excel package from the memory stream
        //            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //            using (ExcelPackage package = new ExcelPackage(ms))
        //            {
        //                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"]; // worksheet name
        //                                                                                  // Remove the first row
        //                worksheet.DeleteRow(1);

        //                for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
        //                {
        //                    msSQL = "select leadbank_gid from crm_trn_tleadbank where leadbank_name='" + worksheet.Cells[row, 2].Value?.ToString() + "'";
        //                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //                    if (objOdbcDataReader != null && !objOdbcDataReader.HasRows)
        //                    {
        //                        objOdbcDataReader.Close();


        //                        msGetGid = objcmnfunctions.GetMasterGID("BMCC");
        //                        msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

        //                        msSQL = " INSERT INTO crm_trn_tleadbank" +
        //                                " (leadbank_gid, " +
        //                                " leadbank_name," +
        //                                " customer_type," +
        //                                " customertype_gid," +
        //                                " remarks," +
        //                                " leadbank_address1," +
        //                                " leadbank_address2," +
        //                                " leadbank_city," +
        //                                " leadbank_state," +
        //                                " leadbank_pin," +
        //                                " leadbank_country," +
        //                                " source_gid," +
        //                                " created_by," +
        //                                " status," +
        //                                " leadbank_id," +
        //                                " approval_flag," +
        //                                " leadbank_code," +
        //                                " main_branch," +
        //                                " main_contact," +
        //                                " lead_status," +
        //                                " leadbank_region," +
        //                                 " cuslead_id," +
        //                                " created_date)" +
        //                                " values( " +
        //                                " '" + msGetGid1 + "'," +
        //                                " '" + worksheet.Cells[row, 2].Value?.ToString() + "'," +
        //                                " '" + worksheet.Cells[row, 16].Value?.ToString() + "'," +
        //                                " (select customertype_gid from crm_mst_tcustomertype where customer_type='" + worksheet.Cells[row, 16].Value?.ToString() + "')," +
        //                                " '" + worksheet.Cells[row, 3].Value?.ToString() + "'," +
        //                                " '" + worksheet.Cells[row, 8].Value?.ToString() + "'," +
        //                                " '" + worksheet.Cells[row, 9].Value?.ToString() + "'," +
        //                                " '" + worksheet.Cells[row, 10].Value?.ToString() + "'," +
        //                                " '" + worksheet.Cells[row, 11].Value?.ToString() + "'," +
        //                                " '" + worksheet.Cells[row, 12].Value?.ToString() + "'," +
        //                                " (select country_gid from adm_mst_tcountry where country_name = '" + worksheet.Cells[row, 13].Value?.ToString() + "')," +
        //                                " (select source_gid from crm_mst_tsource where source_name = '" + worksheet.Cells[row, 15].Value?.ToString() + "')," +
        //                                " '" + lsemployee_gid + "'," +
        //                                " 'Y'," +
        //                                " '" + msGetGid + "'," +
        //                                " 'Approved'," +
        //                                " 'H.Q'," +
        //                                " 'Y'," +
        //                                " 'Y'," +
        //                                " 'Not Assigned'," +
        //                                " (select region_gid from crm_mst_tregion where region_name = '" + worksheet.Cells[row, 7].Value?.ToString() + "')," +
        //                                " '" + worksheet.Cells[row, 1].Value?.ToString() + "'," +
        //                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
        //                        mnResult12 = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                        if (mnResult12 == 1)
        //                        {
        //                            msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");

        //                            msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
        //                                   " (leadbankcontact_gid," +
        //                                   " leadbankcontact_name," +
        //                                   " leadbank_gid," +
        //                                   " mobile," +
        //                                   " email," +
        //                                   " region_name," +
        //                                   " created_by," +
        //                                   " address1," +
        //                                   " address2," +
        //                                   " created_date)" +
        //                                   " values( " +
        //                                   " '" + msGetGid2 + "'," +
        //                                   " '" + worksheet.Cells[row, 4].Value?.ToString() + "'," +
        //                                   " '" + msGetGid1 + "'," +
        //                                   " '" + worksheet.Cells[row, 5].Value?.ToString() + "'," +
        //                                   " '" + worksheet.Cells[row, 6].Value?.ToString() + "'," +
        //                                   " (select region_gid from crm_mst_tregion where region_name = '" + worksheet.Cells[row, 7].Value?.ToString() + "')," +
        //                                   " '" + user_gid + "'," +
        //                                   " '" + worksheet.Cells[row, 8].Value?.ToString() + "'," +
        //                                   " '" + worksheet.Cells[row, 9].Value?.ToString() + "'," +
        //                                   " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
        //                            mnResult12 = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                            if (mnResult12 == 0)
        //                            {
        //                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        objcmnfunctions.LogForAudit("LeadBank Name " + worksheet.Cells[row, 2].Value?.ToString() + "Already Exists", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //                    }
        //                }
        //                ms.Close();
        //                objResult.status = true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            values.message = ex.Message;
        //            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = ex.Message;
        //        return;
        //    }
        //}




        public void DadeleteLeadbankSummary(string leadbank_gid, leadbank_list values)
        {
            try
            {
                msSQL = "Select leadbank_gid from crm_trn_tleadbank where leadbank_gid ='" + leadbank_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    msSQL = "SELECT lead2campaign_gid FROM crm_trn_ttelelead2campaign WHERE leadbank_gid='" + leadbank_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable.Rows.Count != 0)
                    {
                        values.Status = false;
                        values.message = "This Lead has been Assigned";
                    }
                    else
                    {
                        msSQL = "SELECT appointment_gid FROM crm_trn_tappointment WHERE leadbank_gid= '" + leadbank_gid + "' AND Leadstage_gid!='0'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        if (dt_datatable.Rows.Count != 0)
                        {
                            values.Status = false;
                            values.message = "This Lead has been Assigned";
                        }
                        else
                        {
                            msSQL = "DELETE FROM crm_trn_tleadbank WHERE leadbank_gid='" + leadbank_gid + "'";
                            mnResult18 = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = "DELETE FROM crm_trn_tleadbankcontact WHERE leadbank_gid = '" + leadbank_gid + "'";
                            mnResult19 = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult18 != 0 && mnResult19 != 0)
                            {
                                values.Status = true;
                                values.message = "Lead deleted Successfully";
                            }
                            else
                            {
                                values.Status = false;
                                values.message = "Error While Deleting Lead";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                values.message = ex.Message;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DadeleteLeadbankcontact(string leadbankcontact_gid, leadbank_list values)
        {
            try
            {
                msSQL = "Select leadbank_gid from crm_trn_tleadbankcontact where leadbankcontact_gid ='" + leadbankcontact_gid + "' limit 1";
                string leadbank_gid = objdbconn.GetExecuteScalar(msSQL);
                if (leadbank_gid != "" || leadbank_gid != null)
                {
                    msSQL = "SELECT lead2campaign_gid FROM crm_trn_ttelelead2campaign WHERE leadbank_gid='" + leadbank_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    if (dt_datatable.Rows.Count != 0)
                    {
                        values.Status = false;
                        values.message = "This Lead has been Assigned";
                    }
                    else
                    {
                        msSQL = "SELECT appointment_gid FROM crm_trn_tappointment WHERE leadbank_gid= '" + leadbank_gid + "' AND Leadstage_gid!='0'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        if (dt_datatable.Rows.Count != 0)
                        {
                            values.Status = false;
                            values.message = "This Lead has been Assigned";
                        }
                        else
                        {
                            msSQL = "DELETE FROM crm_trn_tleadbankcontact WHERE leadbankcontact_gid ='" + leadbankcontact_gid + "'";
                            mnResult18 = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult18 != 0)
                            {
                                values.Status = true;
                                values.message = "Lead Bank Contact deleted Successfully";
                            }
                            else
                            {
                                values.Status = false;
                                values.message = "Error While Deleting Lead Bank Contact";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                values.message = ex.Message;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostleadbankcontactadd(string user_gid, leadbank_list values)
        {
            try
            {
                msGetGid9 = objcmnfunctions.GetMasterGID("BLCC");
                msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                " (leadbankcontact_gid," +
                " leadbank_gid," +
                " leadbankbranch_name," +
                " leadbankcontact_name," +
                " email," +
                " mobile," +
                " designation," +
                " phone1," +
                " phone2," +
                " created_by," +
                " created_date)" +
                " values( " +
                " '" + msGetGid9 + "'," +
                " '" + values.leadbank_gid + "'," +
                "  'H.Q '," +
                " '" + values.leadbankcontact_name.Replace("'", "\\\'") + "'," +
                " '" + values.email + "',";
                if (values.phone.e164Number == "" || values.phone.e164Number == null)
                {
                    msSQL += "'',";
                }
                else
                {
                    msSQL += " '" + values.phone.e164Number + "',";
                }
                msSQL +=
                  " '" + (String.IsNullOrEmpty(values.designation) ? values.designation : values.designation.Replace("'", "\\\'")) + "'," +
                  " '" + values.phone1 + "'," +
                  " '" + values.phone2 + "'," +
                  " '" + user_gid + "'," +
                  " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                mnResult12 = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult12 != 0)
                {

                    values.Status = true;
                    values.message = "Lead Bank Contact Added Successfully";

                }
                else
                {
                    values.Status = false;
                    values.message = "Error While Adding Lead Bank Contact";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding lead bank contact!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }


        public void DaGetbranchdropdown(string leadbank_gid, MdlLeadBank values)
        {
            try
            {
                msSQL = " select leadbankcontact_gid,leadbankbranch_name from crm_trn_tleadbankcontact" +
                        " where leadbank_gid ='" + leadbank_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadbankcontact_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadbankcontact_list
                        {
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            leadbankbranch_name = dt["leadbankbranch_name"].ToString(),

                        });
                        values.leadbankcontact_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting branch drop down!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateleadbankContactedit(string user_gid, leadbank_list values)
        {
            try
            {

                msSQL = " Update crm_trn_tleadbankcontact set" +
                        " leadbankcontact_name = '" + values.leadbankcontact_nameedit.Replace("'", "\\\'") + "'," +
                        " designation = '" + (String.IsNullOrEmpty(values.designation_edit) ? values.designation_edit : values.designation_edit.Replace("'", "\\\'")) + "'," +
                        " mobile = '" + values.phone_edit.e164Number + "'," +
                        " email =  '" + values.email_edit + "'," +
                        " phone1 = '" + values.phone1_edit + "'," +
                        " phone2 =  '" + values.phone2_edit + "'" +
                        " where leadbankcontact_gid = '" + values.leadbankcontact_gid + "'";

                mnResult14 = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult14 != 0)
                {

                    values.Status = true;
                    values.message = "Lead Bank Contact Updated Successfully";

                }
                else
                {
                    values.Status = false;
                    values.message = "Error While Updating Contact";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank edit contact!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        // Lead Bank Count
        public void DaGetLeadBankCount(string employee_gid, string user_gid, MdlLeadBank values)
        {
            try
            {
                msSQL = " select (select count(leadbank_gid) from crm_trn_tleadbank where customertype_gid='BCRT240331000') as corporate_count, " +
                        " (select count(leadbank_gid) from crm_trn_tleadbank where customertype_gid='BCRT240331002') as retailer_counts," +
                        " (select count(leadbank_gid) from crm_trn_tleadbank where customertype_gid='BCRT240331001') as distributor_count, " +
                        " (select count(leadbank_gid) from crm_trn_tleadbank where leadbank_gid is not null and leadbank_gid!='') as total_count, " +
                        " (select display_name from crm_mst_tcustomertype where customertype_gid='BCRT240331000')corporate_label," +
                        " (select display_name from crm_mst_tcustomertype where customertype_gid='BCRT240331001')distributor_label," +
                        " (select display_name from crm_mst_tcustomertype where customertype_gid='BCRT240331002')retailer_label ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getLeadBankCountList = new List<LeadBankCount_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getLeadBankCountList.Add(new LeadBankCount_List
                        {
                            distributor_count = (dt["distributor_count"].ToString()),
                            retailer_counts = (dt["retailer_counts"].ToString()),
                            corporate_count = (dt["corporate_count"].ToString()),
                            total_count = (dt["total_count"].ToString()),
                            corporate_label = (dt["corporate_label"].ToString()),
                            distributor_label = (dt["distributor_label"].ToString()),
                            retailer_label = (dt["retailer_label"].ToString()),
                        });
                        values.LeadBankCount_List = getLeadBankCountList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        //Customer Type Dropdown
        public void DaGetCustomerTypeSummary(MdlLeadBank values)
        {
            msSQL = "select customertype_gid,display_name from crm_mst_tcustomertype where status_flag = 'Y' ORDER BY customertype_gid ASC";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<customertype_list1>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new customertype_list1
                    {
                        customertype_gid1 = dt["customertype_gid"].ToString(),
                        customer_type1 = dt["display_name"].ToString(),
                    });
                    values.customertype_list1 = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }
        public whatsappconfiguration whatsappcredentials()
        {
            whatsappconfiguration getwhatsappcredentials = new whatsappconfiguration();
            try
            {


                msSQL = " select workspace_id,channel_id,access_token,channelgroup_id from crm_smm_whatsapp_service";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {

                    getwhatsappcredentials.workspace_id = objOdbcDataReader["workspace_id"].ToString();
                    getwhatsappcredentials.channel_id = objOdbcDataReader["channel_id"].ToString();
                    getwhatsappcredentials.access_token = objOdbcDataReader["access_token"].ToString();
                    getwhatsappcredentials.channelgroup_id = objOdbcDataReader["channelgroup_id"].ToString();


                }
                else
                {

                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return getwhatsappcredentials;
        }
        public void DaGetLeadsCount(MdlLeadBank values)
        {
            msSQL = "select count(b.display_name) as lead_count,b.display_name from crm_trn_tleadbank a " +
                    " left join crm_mst_tcustomertype b on a.customertype_gid=b.customertype_gid " +
                    " where a.customertype_gid is not null and b.customer_type is not null group by b.display_name order by lead_count DESC";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<LeadBankCount_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new LeadBankCount_List
                    {
                        lead_count = dt["lead_count"].ToString(),
                        display_name = dt["display_name"].ToString(),
                    });
                    values.LeadBankCount_List = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }


        public void DaGetleadtypedropdown(MdlLeadBank values)
        {
            try
            {
                msSQL = "select leadtype_name,leadtype_gid from crm_mst_tleadtype where status_flag='Y';";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadtype_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadtype_list
                        {
                            leadtype_name = dt["leadtype_name"].ToString(),
                            leadtype_gid = dt["leadtype_gid"].ToString(),

                        });
                        values.leadtype_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting region !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

    }
}