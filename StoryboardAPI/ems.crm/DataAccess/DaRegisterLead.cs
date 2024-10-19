﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.crm.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
//using System.Web;
//using OfficeOpenXml;
using System.Configuration;
using System.IO;
//using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using static ems.crm.Models.MdlRegisterLead;
using System.Net.NetworkInformation;
using ems.system.Models;
using Newtonsoft.Json;
using RestSharp;



namespace ems.crm.DataAccess
{
    public class DaRegisterLead
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader  objOdbcDataReader ;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, source_name, msconGetGID, leadbankcontact_gid, lsdesignation_code, lssource_name, 
            lscategoryindustry_name, lsleadbank_name, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, lsleadbankbranch_name;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;

        public void DaGetRegisterLeadSummary(MdlRegisterLead values, string employee_gid)
        {
            try
            {
                 
               // msSQL = "SELECT a.leadbank_gid,b.leadbankcontact_gid, a.leadbank_id,a.approval_flag,  a.leadbank_name,b.leadbankcontact_name,ifnull(a.customer_gid,' ') as customer_gid," +
               //"a.remarks,concat(b.leadbankcontact_name,' / ',b.country_code1,'-',b.mobile,' / ',b.email)as contact_details,a.lead_status, " +
               //"concat(d.region_name,' / ',a.leadbank_city,' / ',a.leadbank_state) as region_name, " +
               //"concat(b.address1,b.address2) As address_details,a.customer_type,concat_ws('-',c.user_firstname,c.user_lastname) as created_by," +
               //"date_format(a.created_date,'%d-%m-%Y') as created_date,concat(case when e.source_name is null then '' else e.source_name end,' / '," +
               //"case when l.categoryindustry_name is null then '' else l.categoryindustry_name end  ) as source_name, ( g.assign_to )as assigned ,  " +
               //"concat(j.user_firstname,' ',j.user_lastname) as assign_to from crm_trn_tleadbank a  " +
               //"left join crm_trn_tleadbankcontact b on a.leadbank_gid=b.leadbank_gid  " +
               //"left join crm_mst_tregion d on a.leadbank_region=d.region_gid  " +
               //"left join crm_mst_tsource e on a.source_gid=e.source_gid  " +
               //"left join crm_mst_tcategoryindustry l on a.categoryindustry_gid = l.categoryindustry_gid  " +
               //"left join hrm_mst_temployee k on a.created_by=k.employee_gid  " +
               //"left join adm_mst_tuser c on  c.user_gid = k.user_gid  " +
               //"left join crm_trn_tlead2campaign g on a.leadbank_gid = g.leadbank_gid " +
               //"left join hrm_mst_temployee h on  g.assign_to = h.employee_gid  " +
               //"left join adm_mst_tuser j on h.user_gid = j.user_gid " +
               //"where a.main_branch ='Y' and a.customertype_gid = 'BCRT240331002'and a.created_by='" + employee_gid + "'" +
               //"group by a.leadbank_gid Order by date(a.created_date) desc,a.created_date asc,a.leadbank_gid desc";

                msSQL = "call crm_trn_spregisterleadretailer ('" + employee_gid + "')";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRegisterLeadSummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRegisterLeadSummary_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            // remarks = dt["remarks"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            assign_to = dt["assign_to"].ToString(),

                        });
                        values.GetRegisterLeadSummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Bank Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            
          
        }
        public void DaGetRegisterLeadSummary1(MdlRegisterLead values, string employee_gid)
        {
            try
            {

                //msSQL = "SELECT a.leadbank_gid,b.leadbankcontact_gid, a.leadbank_id,a.approval_flag,  a.leadbank_name,b.leadbankcontact_name,ifnull(a.customer_gid,' ') as customer_gid," +
                //"a.remarks,concat(b.leadbankcontact_name,' / ',b.country_code1,'-',b.mobile,' / ',b.email)as contact_details,a.lead_status, " +
                //"concat(d.region_name,' / ',a.leadbank_city,' / ',a.leadbank_state) as region_name, " +
                //"concat(b.address1,b.address2) As address_details,a.customer_type,concat_ws('-',c.user_firstname,c.user_lastname) as created_by," +
                //"date_format(a.created_date,'%d-%m-%Y') as created_date,concat(case when e.source_name is null then '' else e.source_name end,' / '," +
                //"case when l.categoryindustry_name is null then '' else l.categoryindustry_name end  ) as source_name, ( g.assign_to )as assigned ,  " +
                //"concat(j.user_firstname,' ',j.user_lastname) as assign_to from crm_trn_tleadbank a  " +
                //"left join crm_trn_tleadbankcontact b on a.leadbank_gid=b.leadbank_gid  " +
                //"left join crm_mst_tregion d on a.leadbank_region=d.region_gid  " +
                //"left join crm_mst_tsource e on a.source_gid=e.source_gid  " +
                //"left join crm_mst_tcategoryindustry l on a.categoryindustry_gid = l.categoryindustry_gid  " +
                //"left join hrm_mst_temployee k on a.created_by=k.employee_gid  " +
                //"left join adm_mst_tuser c on  c.user_gid = k.user_gid  " +
                //"left join crm_trn_tlead2campaign g on a.leadbank_gid = g.leadbank_gid " +
                //"left join hrm_mst_temployee h on  g.assign_to = h.employee_gid  " +
                //"left join adm_mst_tuser j on h.user_gid = j.user_gid " +
                //"where a.main_branch ='Y' and a.customertype_gid = 'BCRT240331000'and a.created_by='" + employee_gid + "'" +
                //"group by a.leadbank_gid Order by date(a.created_date) desc,a.created_date asc,a.leadbank_gid desc";

                msSQL = "call crm_trn_spregisterleadsummary ('" + employee_gid + "')";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRegisterLeadSummary_list1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRegisterLeadSummary_list1
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            // remarks = dt["remarks"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            assign_to = dt["assign_to"].ToString(),

                        });
                        values.GetRegisterLeadSummary_list1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Bank Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            
           
        }
        public void DaGetRegisterLeadSummary2(MdlRegisterLead values, string employee_gid)
        {
            try
            {

                //msSQL = "SELECT a.leadbank_gid,b.leadbankcontact_gid, a.leadbank_id,a.approval_flag,  a.leadbank_name,b.leadbankcontact_name,ifnull(a.customer_gid,' ') as customer_gid," +
                //"a.remarks,concat(b.leadbankcontact_name,' / ',b.country_code1,'-',b.mobile,' / ',b.email)as contact_details,a.lead_status, " +
                //"concat(d.region_name,' / ',a.leadbank_city,' / ',a.leadbank_state) as region_name, " +
                //"concat(b.address1,b.address2) As address_details,a.customer_type,concat_ws('-',c.user_firstname,c.user_lastname) as created_by," +
                //"date_format(a.created_date,'%d-%m-%Y') as created_date,concat(case when e.source_name is null then '' else e.source_name end,' / '," +
                //"case when l.categoryindustry_name is null then '' else l.categoryindustry_name end  ) as source_name, ( g.assign_to )as assigned ,  " +
                //"concat(j.user_firstname,' ',j.user_lastname) as assign_to from crm_trn_tleadbank a  " +
                //"left join crm_trn_tleadbankcontact b on a.leadbank_gid=b.leadbank_gid  " +
                //"left join crm_mst_tregion d on a.leadbank_region=d.region_gid  " +
                //"left join crm_mst_tsource e on a.source_gid=e.source_gid  " +
                //"left join crm_mst_tcategoryindustry l on a.categoryindustry_gid = l.categoryindustry_gid  " +
                //"left join hrm_mst_temployee k on a.created_by=k.employee_gid  " +
                //"left join adm_mst_tuser c on  c.user_gid = k.user_gid  " +
                //"left join crm_trn_tlead2campaign g on a.leadbank_gid = g.leadbank_gid " +
                //"left join hrm_mst_temployee h on  g.assign_to = h.employee_gid  " +
                //"left join adm_mst_tuser j on h.user_gid = j.user_gid " +
                //"where a.main_branch ='Y' and a.customertype_gid = 'BCRT240331001'and a.created_by='" + employee_gid + "'" +
                //"group by a.leadbank_gid Order by date(a.created_date) desc,a.created_date asc,a.leadbank_gid desc";

                msSQL = "call crm_trn_spregisterleaddistributor ('" + employee_gid + "')";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetRegisterLeadSummary_list2>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetRegisterLeadSummary_list2
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                            contact_details = dt["contact_details"].ToString(),
                            customer_type = dt["customer_type"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            source_name = dt["source_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            remarks = dt["remarks"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            // remarks = dt["remarks"].ToString(),
                            lead_status = dt["lead_status"].ToString(),
                            assign_to = dt["assign_to"].ToString(),

                        });
                        values.GetRegisterLeadSummary_list2 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Lead Bank Summary";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
           

        }

        public void DaPostregisterlead(string employee_gid, Registerlead_list values)
        {
            try
            {
                 
                msSQL = " Select source_name from crm_mst_tsource where source_gid = '" + values.source_name + "'";
                string lssource_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " Select leadbank_name  from crm_trn_tleadbank where  leadbank_gid = '" + values.leadbank_gid + "'";
                string lsleadbank_name = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " Select region_name  from crm_mst_tregion where  region_gid = '" + values.region_name + "'";
                string lsregion_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " Select  country_name from adm_mst_tcountry where  country_gid = '" + values.country_name + "'";
                string lscountry_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select customer_type from crm_mst_tcustomertype Where customertype_gid='" + values.customer_type + "'";
                string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);

                msGetGid = objcmnfunctions.GetMasterGID("BMCC");
                if (msGetGid == "E")
                {
                    values.status = false;
                    values.message = "Create sequence code BMCC for lead bank";
                }

                msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");
                if (msGetGid1 == "E")
                {
                    values.status = false;
                    values.message = "Create sequence code BLBP for Lead Bank";
                }
                msSQL = " INSERT INTO crm_trn_tleadbank(" +
                        " leadbank_gid," +
                         " source_gid," +
                         " leadbank_id," +
                         " leadbank_name," +
                          " status," +
                          " company_website," +
                          " main_branch," +
                          " approval_flag, " +
                          " lead_status," +
                          " leadbank_code," +
                          " leadbank_state," +
                          " leadbank_address1," +
                          " leadbank_address2," +
                          " leadbank_city," +
                          " leadbank_region," +
                          " leadbank_country," +
                          " leadbank_pin," +
                          " customer_type," +
                          " customertype_gid," +
                          " created_by," +
                          " remarks," +
                          " categoryindustry_gid," +
                          " assign_to," +
                          " referred_by," +
                          " created_date)" +
                          " values(" +
                          " '" + msGetGid1 + "'," +
                          " '" + values.source_name + "'," +
                          " '" + msGetGid + "'," +
                          " '" + values.leadbank_name + "'," +
                          " 'Y'," +
                          " '" + values.company_website + "'," +
                          " 'Y'," +
                          " 'Approved'," +
                          " 'Not Assigned'," +
                          " 'H.Q'," +
                          " '" + values.leadbank_state + "'," +
                          " '" + values.leadbank_address1 + "'," +
                          " '" + values.leadbank_address2 + "'," +
                          " '" + values.leadbank_city + "'," +
                          " '" + values.region_name + "'," +
                          " '" + lscountry_name + "'," +
                          " '" + values.leadbank_pin + "'," +
                          " '" + lscustomer_type + "'," +
                          " '" + values.customer_type + "'," +
                          " '" + employee_gid + "'," +
                          " '" + values.remarks.Replace("'", "\\\'") + "'," +
                          " '" + lscategoryindustry_name + "'," +
                          " '" + employee_gid + "'," +
                          " '" + values.referred_by + "'," +
                          " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                msconGetGID = objcmnfunctions.GetMasterGID("BLCC");

                if (msconGetGID == "E")
                {
                    values.status = false;
                    values.message = "Create sequence code BLCC for Lead Contact ";
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
                    " '" + msconGetGID + "'," +
                    " '" + msGetGid1 + "',";
                if ((values.leadbankcontact_name == "" || values.leadbankcontact_name == null) && values.customer_type == "Retailer")
                {
                    msSQL += " '" + values.leadbank_name + "',";
                }
                else if (values.leadbankcontact_name == "" || values.leadbankcontact_name == null)
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
                    " 'y'" + ")";
                mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult == 1)
                {
                    msSQL = "select mobile from crm_trn_tleadbankcontact where leadbank_gid = '" + msGetGid1 + "' and leadbankcontact_gid='" + msconGetGID + "' and main_contact ='Y'";
                    objOdbcDataReader  = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader .HasRows)
                    {
                        string mobile = objOdbcDataReader ["mobile"].ToString();

                        msSQL = "select leadbank_name, SUBSTRING_INDEX(leadbank_name, ' ', 1) AS firstName," +
                            "CASE WHEN LOCATE(' ', leadbank_name) > 0 THEN SUBSTRING_INDEX(leadbank_name, ' ', -1)ELSE ''END AS lastName" +
                            " from crm_trn_tleadbank where leadbank_gid = '" + msGetGid1 + "'";

                        objOdbcDataReader  = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader .HasRows)
                        {
                            string leadbank_name = objOdbcDataReader ["leadbank_name"].ToString();
                            string firstName = objOdbcDataReader ["firstName"].ToString();
                            string lastName = objOdbcDataReader ["lastName"].ToString();
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
                                            " '" + msGetGid1 + "'," +
                                            " '" + msconGetGID + "'," +
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

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Lead Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Inserting Records";
                    }

                }
                objOdbcDataReader .Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Lead";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
           
      
        }
        public void DaAddbranchlead(string user_gid, leadaddbranch_list values)
        {
            try
            {
                 
                msSQL = " Select region_name  from crm_mst_tregion where  region_gid = '" + values.region_name + "'";
                string lsregion_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " Select  country_name from adm_mst_tcountry where  country_gid = '" + values.country + "'";
                string lscountry_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select leadbankbranch_name from crm_trn_tleadbankcontact where leadbank_gid='" + values.leadbank_gid + "' and leadbankbranch_name='" + values.leadbankbranch_name + "' and c.main_contact ='Y'";
                string lsleadbankbranch_name = objdbconn.GetExecuteScalar(msSQL);


                if (lsleadbankbranch_name != values.leadbankbranch_name)
                {
                    msconGetGID = objcmnfunctions.GetMasterGID("BLCC");

                    if (msconGetGID == "E")
                    {
                        values.status = false;
                        values.message = "Create sequence code BLCC for Lead Branch ";
                    }


                    msSQL = " INSERT INTO crm_trn_tleadbankcontact(" +
                            " leadbankcontact_gid," +
                            " leadbank_gid," +
                            " leadbankbranch_name," +
                            " leadbankcontact_name," +
                            " email," +
                            " mobile," +
                            " designation, " +
                            " created_by," +
                            " address1," +
                            " address2," +
                            " country," +
                            " city," +
                            " region_name," +
                            " state," +
                            " pincode," +
                            " created_date)" +
                            " values(" +
                            " '" + msconGetGID + "'," +
                            " '" + values.leadbank_gid + "'," +
                            " '" + values.leadbankbranch_name + "'," +
                            " '" + values.leadbankcontact_name + "'," +
                            " '" + values.email + "'," +
                            " '" + values.mobile + "'," +
                            " '" + values.designation + "'," +
                            " '" + user_gid + "'," +
                            " '" + values.address1 + "'," +
                            " '" + values.address2 + "'," +
                            " '" + values.country + "'," +
                            " '" + values.city + "'," +
                            " '" + lsregion_name + "'," +
                            " '" + values.state + "'," +
                            " '" + values.pincode + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {

                        values.status = true;
                        values.message = "Lead Bank branch added Successfully";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While adding branch";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Lead Bank Branch name is already exist";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            
       


        }

        public void DaGetbranchdropdown(string leadbank_gid, MdlRegisterLead values)
        {
            try
            {
                 
                msSQL = " select leadbankbranch_name,leadbank_gid from crm_trn_tleadbankcontact" +
                " where leadbank_gid ='" + leadbank_gid + "' and c.main_contact ='Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<branch_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new branch_list
                        {
                            leadbankbranch_name = dt["leadbankbranch_name"].ToString(),

                        });
                        values.branch_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            
        
        }

        public void DaGetleadbankbrancheditSummary(string leadbankcontact_gid, MdlRegisterLead values)
        {
            try
            {
                 
                msSQL = "select leadbank_gid,leadbankcontact_gid,leadbankbranch_name,leadbankcontact_gid,leadbankcontact_name, designation, mobile,email,address1, address2,city,state,region_name,country,pincode  from  crm_trn_tleadbankcontact  " +
           " where leadbankcontact_gid ='" + leadbankcontact_gid + "' and main_contact ='Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadaddbranch_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadaddbranch_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            designation = dt["designation"].ToString(),
                            email = dt["email"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            country = dt["country"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            pincode = dt["pincode"].ToString(),
                        });
                        values.leadaddbranch_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while editing lead bank branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
           
   
        }

        public void DaGetleadbranchaddSummary(string leadbank_gid, MdlRegisterLead values)
        {
            try
            {
                 
                msSQL = " SELECT distinct a.leadbankcontact_gid,a.leadbank_gid,a.leadbankbranch_name,a.leadbankcontact_name,a.mobile,a.designation,concat (a.address1,a.address2) As Address," +
     " a.city, a.state, a.pincode, a.country,a.region_name,a.email,a.country_code1,a.country_gid " +
     " from crm_trn_tleadbankcontact a" +
     " where a.leadbank_gid ='" + leadbank_gid + "' and a.main_contact ='Y'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadaddbranch_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadaddbranch_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankcontact_gid = dt["leadbankcontact_gid"].ToString(),
                            leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                            leadbankcontact_name = dt["leadbankcontact_name"].ToString(),
                            designation = dt["designation"].ToString(),
                            mobile = dt["mobile"].ToString(),
                            Address = dt["Address"].ToString(),
                            city = dt["city"].ToString(),
                            state = dt["state"].ToString(),
                            country = dt["country"].ToString(),
                            pincode = dt["pincode"].ToString(),
                        });
                        values.leadaddbranch_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding lead bank summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            

        }



        public void DaGetregisterleadbranchSummary(string leadbank_gid, MdlRegisterLead values)
        {
            try
            {
                 
                msSQL = " select leadbank_gid, a.leadbankbranch_name,concat (a.address1,a.address2) As Address,b.leadbank_city,a.state, " +
                   " a.pincode,b.leadbank_country from crm_trn_tleadbankcontact a " +
                   " left join crm_trn_tleadbank  b on a.leadbank_gid=b.leadbank_gid where a.main_contact ='Y'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadbranch_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadbranch_list
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbankbranch_name = dt["leadbankbranch_name"].ToString(),
                            Address = dt["Address"].ToString(),
                            leadbank_city = dt["leadbank_city"].ToString(),
                            state = dt["state"].ToString(),
                            pincode = dt["pincode"].ToString(),
                            leadbank_country = dt["leadbank_country"].ToString(),
                        });
                        values.leadbranch_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            
           
        }

      


        public void DaUpdatedregisterlead(string employee_gid, Registerlead_list values)
        {
            try
            {
                 
                msSQL = " Select source_name from crm_mst_tsource where source_gid = '" + values.source_name + "'";
                string lssource_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " Select  leadbank_name from crm_trn_tleadbank where leadbank_gid = '" + values.leadbank_name + "'";
                string lsleadbank_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " Select  categoryindustry_name  from crm_mst_tcategoryindustry where categoryindustry_gid = '" + values.categoryindustry_name + "'";
                string lscategoryindustry_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " Select region_name  from crm_mst_tregion where  region_gid = '" + values.region_name + "'";
                string lsregion_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " Select  country_name from adm_mst_tcountry where  country_gid = '" + values.country_name + "'";
                string lscountry_name = objdbconn.GetExecuteScalar(msSQL);


                msSQL = " update  crm_trn_tleadbank  set " +
              " leadbank_gid = '" + msGetGid1 + "'," +
              " source_gid = '" + lssource_name + "'," +
              " leadbank_id = '" + msGetGid + "'," +
              " leadbank_name = '" + lsleadbank_name + "'," +
              " company_website = '" + values.company_website + "'," +
              " leadbank_state = '" + values.leadbank_state + "'," +
              " leadbank_address1 = '" + values.leadbank_address1 + "'," +
              " leadbank_address2 = '" + values.leadbank_address2 + "'," +
              " leadbank_city = '" + values.leadbank_city + "'," +
              " leadbank_region = '" + lsregion_name + "'," +
              " leadbank_country = '" + lscountry_name + "'," +
              " leadbank_pin = '" + values.leadbank_pin + "'," +
              " remarks = '" + values.remarks.Replace("'", "\\\'") + "'," +
              " categoryindustry_gid = '" + lscategoryindustry_name + "'," +
              " referred_by = '" + values.referred_by + "'," +
              " updated_by = '" + employee_gid + "'," +
              " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where leadbank_gid='" + values.leadbank_gid + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error Occured While Inserting Records";
                }

                msSQL = " update  crm_trn_tleadbankcontact  set " +
             " leadbankcontact_gid = '" + msconGetGID + "'," +
             " leadbank_gid = '" + msGetGid1 + "'," +
             " leadbankcontact_name = '" + values.leadbankcontact_name + "'," +
             " email = '" + values.email + "'," +
             " mobile = '" + values.mobile + "'," +
             " designation = '" + values.designation + "'," +
             " phone1 = '" + values.phone1 + "'," +
             " country_code1 = '" + values.country_code1 + "'," +
             " area_code1 = '" + values.area_code1 + "'," +
             " phone2 = '" + values.phone2 + "'," +
             " country_code2 = '" + values.country_code2 + "'," +
             " area_code2 = '" + values.area_code2 + "'," +
             " fax_country_code = '" + values.fax_country_code + "'," +
             " fax_area_code = '" + values.fax_area_code + "'," +
             " fax = '" + values.fax + "'," +
             " address1 = '" + values.leadbank_address1 + "'," +
             " address2 = '" + values.leadbank_address2 + "'," +
             " country = '" + lscountry_name + "'," +
             " region_name = '" + lsregion_name + "'," +
             " state = '" + values.state + "'," +
             " pincode = '" + values.leadbank_pin + "'," +
             " updated_by = '" + employee_gid + "'," +
             " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where leadbank_gid='" + values.leadbank_gid + "'  ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult == 0)
                {
                    values.status = false;
                    values.message = "Error Occured While Inserting Records";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating lead bank!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
           
     


        }


        public void DadeleteregisterleadSummary(string leadbank_gid, GetRegisterLeadSummary_list values)
        {
            try
            {
                 
                msSQL = " Select leadbank_gid from crm_trn_tleadbank " +
                        " where leadbank_gid ='" + leadbank_gid + "' and customer_gid is null";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    msSQL = " select lead2campaign_gid from crm_trn_ttelelead2campaign where leadbank_gid='" + leadbank_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count == 0)
                    {
                        msSQL = " select lead2campaign_gid from crm_trn_tlead2campaign where leadbank_gid= '" + leadbank_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        if (dt_datatable.Rows.Count == 0)
                        {
                            msSQL = " Delete from crm_trn_tleadbank " +
                                    " where leadbank_gid='" + leadbank_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                msSQL = " Delete from crm_trn_tleadbankcontact " +
                                        " where leadbank_gid = '" + leadbank_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            }
                        }
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Lead Bank deleted Successfully";

                        }
                        else
                        {
                            values.status = false;
                            values.message = "Lead already assigned cant delete";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleteing lead bank summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            

        }


        public void DaGetcountrynamedropdown(MdlRegisterLead values)
        {
            try
            {
                 
                msSQL = " select  country_gid, country_code, country_name " +
                 " from adm_mst_tcountry ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getcountrynamedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcountrynamedropdown
                        {
                            country_gid = dt["country_gid"].ToString(),
                            country_name = dt["country_name"].ToString(),
                        });
                        values.Getcountrynamedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting country!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            
         
        }

        public void DaGetregiondropdown1(MdlRegisterLead values)
        {
            try
            {
                 
                msSQL = " select region_gid,concat (region_name,' / ',city) as region_name" +
                   " from crm_mst_tregion Order by region_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getregiondropdown1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getregiondropdown1
                        {
                            region_gid = dt["region_gid"].ToString(),
                            region_name = dt["region_name"].ToString(),
                        });
                        values.Getregiondropdown1 = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting region!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            
           
        }

        public void DaGetindustrydropdown(MdlRegisterLead values)
        {
            try
            {
                 
                msSQL = " select categoryindustry_gid,categoryindustry_code,categoryindustry_name " +
              "  from crm_mst_tcategoryindustry ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getindustrydropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getindustrydropdown
                        {
                            categoryindustry_gid = dt["categoryindustry_gid"].ToString(),
                            categoryindustry_name = dt["categoryindustry_name"].ToString(),
                        });
                        values.Getindustrydropdown = getModuleList;
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

        public void DaGetSourcetypedropdown(MdlRegisterLead values)
        {
            try
            {
                 
                msSQL = " select source_gid,source_name as source_name " +
               "  from crm_mst_tsource ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetSourcetypedropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetSourcetypedropdown
                        {
                            source_gid = dt["source_gid"].ToString(),
                            source_name = dt["source_name"].ToString(),
                        });
                        values.GetSourcetypedropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting source type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
           
           
        }

        public void DaGetcompanylistdropdown(MdlRegisterLead values)
        {
            try
            {
                 
                msSQL = " select leadbank_gid,leadbank_name " +
            "  from crm_trn_tleadbank ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getcompanylistdropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcompanylistdropdown
                        {
                            leadbank_gid = dt["leadbank_gid"].ToString(),
                            leadbank_name = dt["leadbank_name"].ToString(),
                        });
                        values.Getcompanylistdropdown = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting company drop down!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            
        
        }

        public void DaUpdateleadbranchedit(string user_gid, leadaddbranch_list values)
        {

            //msSQL = " select leadbankbranch_name from crm_trn_tleadbankcontact where leadbank_gid='" + values.leadbank_gid + "' and leadbankbranch_name='" + values.leadbankbranch_name + "'";
            //string lsleadbankbranch_name = objdbconn.GetExecuteScalar(msSQL);


            //if (lsleadbankbranch_name != values.leadbankbranch_name)
            //{
            try
            {
                 

                msSQL = " Update crm_trn_tleadbankcontact set" +
                         " leadbankbranch_name = '" + values.leadbankbranch_name + "'," +
                         " leadbankcontact_name = '" + values.leadbankcontact_name + "'," +
                         " designation = '" + values.designation + "'," +
                         " mobile = '" + values.mobile + "'," +
                         " email =  '" + values.email + "'," +
                         " address1 =  '" + values.address1 + "'," +
                         " address2 =  '" + values.address2 + "'," +
                         " city =  '" + values.city + "'," +
                         " state = '" + values.state + "'," +
                         " country = '" + values.country + "'," +
                         " region_name = '" + values.region_name + "'," +
                         " pincode = '" + values.pincode + "'" +
                         " where leadbankcontact_gid = '" + values.leadbankcontact_gid + "'" +
                         " and leadbank_gid = '" + values.leadbank_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {

                    values.status = true;
                    values.message = "Lead Bank branch Updated Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating branch";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while uptating lead branch!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
           

            //}
            //else
            //{
            //    values.status = false;
            //    values.message = "Leadbank branch name is already exist";
            //}

        }
        // Register Lead Count
        public void DaGetRegisterLeadCount(string employee_gid, string user_gid, MdlRegisterLead values)
        {
            try
            {
                 
                msSQL = " select (select count(leadbank_gid) from crm_trn_tleadbank where customertype_gid='BCRT240331002' and " +
           " created_by='" + employee_gid + "') as corporate_count, " +
           " (select count(leadbank_gid) from crm_trn_tleadbank where customertype_gid='BCRT240331001' and " +
           " created_by='" + employee_gid + "') as distributor_count," +
           " (select count(leadbank_gid) from crm_trn_tleadbank where customertype_gid='BCRT240331000' and " +
           " created_by='" + employee_gid + "') as retailer_counts, " +
           " (select count(leadbank_gid) from crm_trn_tleadbank where leadbank_gid is not null and leadbank_gid!='' and " +
           " created_by='" + employee_gid + "') as total_count ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getRegisterLeadCountList = new List<RegisterLeadCount_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getRegisterLeadCountList.Add(new RegisterLeadCount_List
                        {
                            distributor_count = (dt["distributor_count"].ToString()),
                            retailer_counts = (dt["retailer_counts"].ToString()),
                            corporate_count = (dt["corporate_count"].ToString()),
                            total_count = (dt["total_count"].ToString()),
                        });
                        values.RegisterLeadCount_List = getRegisterLeadCountList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting register lead count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
           
   
        }
        public void DaGetCustomerTypeCount(string employee_gid, MdlRegisterLead values)
        {
            try
            {

                msSQL = "select count(b.display_name) as lead_count,b.display_name " +
                        " from crm_trn_tleadbank a " +
                        " left join crm_mst_tcustomertype b on a.customertype_gid=b.customertype_gid " +
                        " where a.customertype_gid is not null and a.created_by='" + employee_gid + "' group by b.display_name order by lead_count DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getRegisterLeadCountList = new List<RegisterLeadCount_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getRegisterLeadCountList.Add(new RegisterLeadCount_List
                        {
                            lead_count = (dt["lead_count"].ToString()),
                            display_name = (dt["display_name"].ToString()),
                        });
                        values.RegisterLeadCount_List = getRegisterLeadCountList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Customer Lead lead count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }


        }

    }
}
