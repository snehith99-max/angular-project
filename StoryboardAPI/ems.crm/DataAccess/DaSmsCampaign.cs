using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;
using ems.crm.Models;
using RestSharp;
using System.Net;
namespace ems.crm.DataAccess
{
    public class DaSmsCampaign
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;

        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string lssource_gid;
        string lssource_name, param1, lsleadbank_name, lsid, lscategoryindustry_name, lscountry_name, lscampagin_title, lscampaign_flag, lsleadbank_gid, lscountry_gid, mscusconGetGID, lscountrygid, mscustomerGetGID, msGETcustomercode,
            lsregion_name, lsbankcontact, msGetGid, msGetGid1, msGetGid2, msGetGid3, msGetGid4,
            msGetGid5, msGetGid6, msGetGid7, msGetGid8, msGetGid9, msGetGid10, msGetGid11, lscurrencyexchange_gid;
        int mnResult;
       
        private string phone_number;
        private int count;


        public void DaGetSmsCampaign(MdlSmsCampaign values)
        {
            msSQL = " select a.template_id,a.campagin_title,a.campagin_message,COALESCE(COUNT(c.template_id), 0) AS send_count,date_format(a.created_date,'%d-%m-%Y')as created_date,concat(b.user_firstname,(' '), b.user_lastname) as created_by " +
                    "  from crm_smm_tsmscampaign a left join adm_mst_tuser b on b.user_gid =a.created_by left join crm_smm_smscampaigndtl c on c.template_id=a.template_id  group by a.template_id  order by template_id desc; ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<smscampaign_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new smscampaign_list
                    {
                        template_id = dt["template_id"].ToString(),
                        campagin_title = dt["campagin_title"].ToString(),
                        campagin_message = dt["campagin_message"].ToString(),
                        created_date = dt["created_date"].ToString(),
                        created_by = dt["created_by"].ToString(),
                        send_count = dt["send_count"].ToString(),


                    });
                    values.smscampaign_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetSmsCampaignCount(MdlSmsCampaign values)
        {
            msSQL = "  select (select ifnull(count(template_id),0)  from crm_smm_tsmscampaign )as campaign_count,(select ifnull(count(template_id),0) from crm_smm_smscampaigndtl) as totalcampaign_send,(select ifnull(count(template_id),0) " +
                " from crm_smm_smscampaigndtl where month(created_date) = month(current_date())) as current_month, (SELECT DATE_FORMAT(CURDATE(), '%M')) AS crt_mth,(select DATE_FORMAT(CURDATE(), '%Y')) as crt_yr, " +
                "(select ifnull(count(template_id),0) from crm_smm_smscampaigndtl where year(created_date) = year(current_date())) as current_year;";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                 values.campaign_count = objOdbcDataReader["campaign_count"].ToString();
                 values.totalcampaign_send = objOdbcDataReader["totalcampaign_send"].ToString();
                 values.current_month = objOdbcDataReader["current_month"].ToString();
                 values.current_year = objOdbcDataReader["current_year"].ToString();
                 values.crt_mth = objOdbcDataReader["crt_mth"].ToString();
                 values.crt_yr = objOdbcDataReader["crt_yr"].ToString();
            }
        }
        public void DaPostSmsCampaign(string user_gid, smspostcampaign_list values)

        {
            msSQL = " select campagin_title from crm_smm_tsmscampaign where campagin_title = '" + values.campaign_title + "' limit 1";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

            if (objOdbcDataReader.HasRows == true)
            {
                values.status = false;
                values.message = "Campaign Title Already Exist !!";
            }

            else
            {

                msSQL = " insert into crm_smm_tsmscampaign(" +
                        " campagin_title," +
                        " campagin_message," +
                        " created_by," +
                        " created_date)" +
                        " values(" +
                        "'" + values.campaign_title + "'," +
                        " '" + values.campaign_message + "',";
                msSQL += "'" + user_gid + "'," +
                         "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Campaign Added Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Adding Campaign!!";
                }

            }

        }
        public void DaUpdateSmsCampaign(string user_gid, smspostcampaign_list values)
        {
            msSQL = " select template_id,campagin_title  from crm_smm_tsmscampaign where campagin_title = '" + values.campaign_titleedit + "' limit 1";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                lsid = objOdbcDataReader["template_id"].ToString();
                lscampagin_title = objOdbcDataReader["campagin_title"].ToString();
            }
            if (lsid == values.template_id)
            {
                msSQL = " update  crm_smm_tsmscampaign set " +

                " campagin_title = '" + values.campaign_titleedit + "'," +

                " campagin_message = '" + values.campaign_messageedit + "'," +

                " updated_by = '" + user_gid + "'," +

                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where itemplate_idd='" + values.template_id + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Campaign Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Campaign !!";
                }
            }
            else if (lsid == null || lsid == "")
            {
                msSQL = " update  crm_smm_tsmscampaign set " +

                " campagin_title = '" + values.campaign_titleedit + "'," +

                " campagin_message = '" + values.campaign_messageedit + "'," +

                " updated_by = '" + user_gid + "'," +

                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where template_id='" + values.template_id + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Campaign Updated Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Campaign !!";
                }
            }
            else
            {
                values.status = false;
                values.message = "Campaign with the same name already exists !!";
            }

        }
        public void DaDeleteSmsCampaign(string template_id, smspostcampaign_list values)
        {
            msSQL = "select template_id from crm_smm_smscampaigndtl where template_id='" + template_id + "';";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);

            if (objOdbcDataReader.HasRows)
            {
                values.status = false;
                values.message = "Campaign already used hence can't be deleted!!";
            }
            else
            {
                msSQL = "  delete from crm_smm_tsmscampaign where template_id = '" + template_id + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Campaign Deleted Successfully !!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Campaign !!";
                }

            }

        }
        public void DaSmsLeadCustomerDetails(MdlSmsCampaign values)

        {
            msSQL = "select b.address1,b.address2, b.city,b.state,a.customer_type,b.leadbankcontact_name,a.leadbank_gid,b.email,b.mobile," +
                "b.created_date from crm_trn_tleadbank a left join crm_trn_tleadbankcontact b on a.leadbank_gid=b.leadbank_gid where a.main_contact ='Y' group by b.mobile ;";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<smsleadcustomerdetails_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new smsleadcustomerdetails_list
                    {
                        leadbank_gid = dt["leadbank_gid"].ToString(),
                        names = dt["leadbankcontact_name"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        default_phone = dt["mobile"].ToString(),
                        created_date = dt["created_date"].ToString(),
                        email = dt["email"].ToString(),
                        address1 = dt["address1"].ToString(),
                        address2 = dt["address2"].ToString(),
                        city = dt["city"].ToString(),
                        state = dt["state"].ToString(),



                    });
                    values.smsleadcustomerdetails_list = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        } 
        public result Dasmstemplatesendsummarylist(smstemplatesendsummary_list values, string user_gid)
        {
            result result = new result();
            try
            {
                msSQL = "select campagin_message,campagin_title from crm_smm_tsmscampaign where template_id = '" + values.template_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    string campagin_message = objOdbcDataReader["campagin_message"].ToString();
                    string campagin_title = objOdbcDataReader["campagin_title"].ToString();
                 
                     daSendBulkMessage(values.template_id,campagin_message.Replace("\n"," ").Replace("\r"," "),campagin_title,values.customerdetailslist,user_gid);
           }
                result.status = true;
                result.message = "Messages sent successfully!";
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                result.message = "Error while Sending Mail";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + result.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

            return result;
        }
        public void daSendBulkMessage(string template_id, string campagin_message,string campagin_title, List<mdlBulksmssend> customerdetailslist,string user_gid)
        {
            smsconfiguration getsmscredentials = smscredentials();
            Result objsendmessage = new Result();
                try
                {
               foreach (var item in customerdetailslist)
                {
                    phone_number += item.default_phone;
                    count++;
                    if (count < customerdetailslist.Count)
                    {
                        phone_number += ",";
                    }
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    var fullUrl = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage"
                                + "&send_to=" + phone_number
                                + "&msg=" + campagin_message
                                + "&msg_type=TEXT"
                                + "&userid= "+ getsmscredentials.sms_user_id + " "
                                + "&auth_scheme=plain"
                                + "&password= "+ getsmscredentials.sms_password+ ""
                                + "&v=1.1"
                                + "&format=text";
                    var client = new RestClient(fullUrl);
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string[] lines = response.Content.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        string[] values = line.Split('|');
                        string status = values[0].Trim();
                        string phone_number = values[1].Trim();
                        string send_id = values[2].Trim();
                        msSQL = "insert into crm_smm_smscampaigndtl(" +
                                       "phone_number," +
                                       "template_id," +
                                       "status," +
                                       "send_id," +
                                       "created_by," +
                                       "created_date)" +
                                       "values(" +
                                       "'" + phone_number.Substring(2) + "'," +
                                       "'" + template_id + "'," +
                                       "'" + status + "'," +
                                      "'" + send_id + "'," +
                                       "'" + user_gid + "'," +
                                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 1)
                        {
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/SMS/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                    }
                }
                else
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error Occured while sending message to " + response.Content + " Template Id : " + template_id + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/SMS/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/SMS/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }
                    
                }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Exception Occured while sending message Template Id: " + template_id + " *******" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/SMS/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "****************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/SMS/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetsmscampaignlog(MdlSmsCampaign values, string template_id)
        {
            try
            {


                msSQL = "select a.smscampaigndtl_gid,a.phone_number,a.template_id,a.status,a.created_date,c.campagin_title,concat(b.user_firstname,(' '), b.user_lastname) as created_by" +
                    " ,(select d.leadbankcontact_name from crm_trn_tleadbankcontact d where d.mobile like concat('%',substring(a.phone_number,3),'%') and main_contact ='Y' group by d.leadbankcontact_gid limit 1) as contact_name from crm_smm_smscampaigndtl a left join crm_smm_tsmscampaign c on c.template_id =a.template_id left join adm_mst_tuser b on b.user_gid =a.created_by where a.template_id='" + template_id + "' order by a.smscampaigndtl_gid desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<smscampaignlog>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new smscampaignlog
                        {
                            campagin_title = dt["campagin_title"].ToString(),
                            contact_name = dt["contact_name"].ToString(),
                            smscampaigndtl_gid = dt["smscampaigndtl_gid"].ToString(),
                            phone_number = dt["phone_number"].ToString(),
                            template_id = dt["template_id"].ToString(),
                            status = dt["status"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString()

                        });
                        values.smscampaignlog = getModuleList;
                    }

                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Getting Campaign !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetindividuallog(MdlSmsCampaign values, string phone_number)
        {
            try
            {
                msSQL = " select a.template_id ,b.campagin_message,concat(c.user_firstname,(' '), c.user_lastname) as created_by,a.created_date,a.phone_number  from crm_smm_smscampaigndtl a left join crm_smm_tsmscampaign b on b.template_id=a.template_id" +
                    " left join adm_mst_tuser c on c.user_gid=a.created_by  where a.phone_number LIKE '%" + phone_number + "%' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<individualsmslog>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new individualsmslog
                        {
                            template_id = dt["template_id"].ToString(),
                            campagin_message = dt["campagin_message"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            phone_number = dt["phone_number"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.individualsmslog = getModuleList;
                    }

                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Loading Individual Record !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void Dasmstemplatepreview(MdlSmsCampaign values, string template_id)
        {
            try
            {
                string msSQL = "select campagin_title,campagin_message from crm_smm_tsmscampaign where template_id='" + template_id + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<template_previewlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new template_previewlist
                        {
                            campagin_title = dt["campagin_title"].ToString(),
                            campagin_message = dt["campagin_message"].ToString()
                        });
                        values.template_previewlist = getModuleList;
                    }

                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error while Fetching Template Preview";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public smsconfiguration smscredentials()
        {
            smsconfiguration getsmscredentials = new smsconfiguration();

            msSQL = " select sms_user_id,sms_password from crm_smm_tsmsservice";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows == true)
            {

                getsmscredentials.sms_user_id = objOdbcDataReader["sms_user_id"].ToString();
                getsmscredentials.sms_password = objOdbcDataReader["sms_password"].ToString();


            }
            else
            {

            }

            return getsmscredentials;
        }
    }
}