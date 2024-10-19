using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Data;
using System.Data.Odbc;
using Newtonsoft.Json;
using RestSharp;
using System.Web.UI;
using System.Web.UI.WebControls;
using ems.crm.Controllers;
using ems.crm.Models;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Web.Http.Results;





namespace ems.crm.DataAccess
{
    public class DaCampaignService
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, lscustomertype_gid, lsmodule_gid, lsshopify_flag, lsupdated_date, lscreated_date, lsupdated_by,
            lscreated_by, lscustomer_type, final_path, final_path1, max_menulevel;
        string domain = string.Empty;

        public void DaGetWhatsappSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select s_no,workspace_id,channel_id,access_token,channelgroup_id,mobile_number, whatsapp_status," +
                        " channel_name,created_by,created_date from crm_smm_whatsapp_service limit 1";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<campaignservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new campaignservice_list
                        {
                            s_no = dt["s_no"].ToString(),
                            workspace_id = dt["workspace_id"].ToString(),
                            channel_id = dt["channel_id"].ToString(),
                            access_token = dt["access_token"].ToString(),
                            channelgroup_id = dt["channelgroup_id"].ToString(),
                            mobile_number = dt["mobile_number"].ToString(),
                            channel_name = dt["channel_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            whatsapp_status = dt["whatsapp_status"].ToString(),
                        });
                        values.campaignservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting whatsapp summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateWhatsappService(string user_gid, campaignservice_list values)

        {
            try
            {
                if (values.whatsapp_status == "Y")
                {
                    if (values.whatsapp_id == null || values.whatsapp_id == "")
                    {
                        msSQL = " insert into crm_smm_whatsapp_service(" +
                        " workspace_id," +
                        " channel_id," +
                        " access_token," +
                        " mobile_number," +
                        " channel_name," +
                        " channelgroup_id," +
                        " created_by," +
                        " whatsapp_status," +
                        " created_date)" +
                        " values(" +
                        "'" + values.workspace_id + "'," +
                        "'" + values.channel_id + "'," +
                        "'AccessKey " + values.whatsapp_accesstoken + "'," +
                        "'" + values.mobile_number + "'," +
                        "'" + values.channel_name + "'," +
                        "'" + values.channelgroup_id + "'," +
                        "'" + user_gid + "'," +
                        "'Y'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        if (mnResult != 0)
                        {
                            msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Whatsapp'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Whatsapp'";
                                lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                                lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                                if (lsshopify_flag == "N")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Whatsapp Credentials Updated Successfully !!";
                                    }
                                }
                                else if (lsshopify_flag == "")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    values.status = true;
                                    values.message = "Shopify Credentials Updated Successfully!!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Whatsapp Credentials!!";
                                }

                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Whatsapp Credentials !!";
                            }
                        }
                    }
                    else
                    {
                        msSQL = " update  crm_smm_whatsapp_service set " +

                                " workspace_id = '" + values.workspace_id + "'," +
                                " channel_id = '" + values.channel_id + "'," +
                                " access_token = '" + values.whatsapp_accesstoken + "'," +
                                " mobile_number = '" + values.mobile_number + "'," +
                                " channel_name = '" + values.channel_name + "'," +
                                " channelgroup_id =  '" + values.channelgroup_id + "'," +
                                " updated_by = '" + user_gid + "'," +
                                "whatsapp_status='Y'," +
                                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.whatsapp_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Whatsapp'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Whatsapp'";
                                lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);

                                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                                lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                                if (lsshopify_flag == "N")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Whatsapp Credentials Updated Successfully !!";
                                    }
                                }
                                else if (lsshopify_flag == "")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    values.status = true;
                                    values.message = "Whatsapp Credentials Updated Successfully!!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Whatsapp Credentials!!";
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Whatsapp Credentials !!";
                            }
                        }
                    }
                }
                else
                {
                    msSQL = " update crm_smm_whatsapp_service set whatsapp_status = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'N' where module_name = 'Whatsapp'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Disabled Successfully!!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating whatsapp service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetShopifySummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select s_no,access_token,shopify_store_name,store_month_year,created_by,created_date,shopify_status" +
                        " from crm_smm_shopify_service limit 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<shopifycampaignservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new shopifycampaignservice_list
                        {
                            s_no = dt["s_no"].ToString(),
                            shopify_access_token = dt["access_token"].ToString(),
                            shopify_store_name = dt["shopify_store_name"].ToString(),
                            store_month_year = dt["store_month_year"].ToString(),
                            shopify_created_by = dt["created_by"].ToString(),
                            shopify_created_date = dt["created_date"].ToString(),
                            shopify_status = dt["shopify_status"].ToString(),
                        });
                        values.shopifycampaignservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting shopify summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetMailSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select mail_toggle,access_token,base_url,created_by,s_no,created_date,receiving_domain,email_status," +
                        " sending_domain,email_username from crm_smm_mail_service limit 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<mailcampaignservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new mailcampaignservice_list
                        {
                            mail_access_token = dt["access_token"].ToString(),
                            mail_base_url = dt["base_url"].ToString(),
                            mail_created_by = dt["created_by"].ToString(),
                            s_no = dt["s_no"].ToString(),
                            mail_created_date = dt["created_date"].ToString(),
                            receiving_domain = dt["receiving_domain"].ToString(),
                            sending_domain = dt["sending_domain"].ToString(),
                            email_username = dt["email_username"].ToString(),
                            email_status = dt["email_status"].ToString(),
                            mail_toggle = dt["mail_toggle"].ToString(),

                        });
                        values.mailcampaignservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting mail summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetGMailSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select s_no,client_id,client_secret,refresh_token,gmail_address,gmail_status from crm_smm_gmail_service limit 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<gmailcampaignservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailcampaignservice_list
                        {
                            client_id = dt["client_id"].ToString(),
                            client_secret = dt["client_secret"].ToString(),
                            refresh_token = dt["refresh_token"].ToString(),
                            gmail_address = dt["gmail_address"].ToString(),
                            gmail_status = dt["gmail_status"].ToString(),
                            s_no = dt["s_no"].ToString(),



                        });
                        values.gmailcampaignservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting mail summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateShopifyService(string user_gid, shopifyservcie_list values)
        {
            try
            {
                if (values.shopify_status == "Y")
                {
                    if (values.shopify_id == null || values.shopify_id == "")
                    {


                        msSQL = " insert into crm_smm_shopify_service(" +
                                " access_token," +
                                " shopify_store_name," +
                                " store_month_year," +
                                " created_by," +
                                "shopify_status," +
                                " created_date)" +
                                " values(" +
                                "'" + values.shopify_accesstoken + "'," +
                                 " '" + values.shopify_store_name + "'," +
                                " '" + values.store_month_year + "',";
                        msSQL += "'" + user_gid + "'," +
                            "'Y'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Shopify'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Shopify'";
                                lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                                lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                                if (lsshopify_flag == "N")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Shopify Credentials Updated Successfully !!";
                                    }
                                }
                                else if (lsshopify_flag == "")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    values.status = true;
                                    values.message = "Shopify Credentials Updated Successfully!!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Shopify Credentials!!";
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Shopify Credentials!!";
                            }

                        }
                    }
                    else
                    {
                        msSQL = " update  crm_smm_shopify_service set " +

                        " access_token = '" + values.shopify_accesstoken + "'," +

                        " shopify_store_name = '" + values.shopify_store_name + "'," +
                        " store_month_year = '" + values.store_month_year + "'," +

                        " updated_by = '" + user_gid + "'," +
                        "shopify_status='Y'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.shopify_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Shopify'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Shopify'";
                                lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                                lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                                if (lsshopify_flag == "N")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Shopify Credentials Updated Successfully !!";
                                    }
                                }
                                else if (lsshopify_flag == "")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    values.status = true;
                                    values.message = "Shopify Credentials Updated Successfully!!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Shopify Credentials !!";
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Shopify Credentials !!";
                            }
                        }
                    }
                }

                else
                {
                    msSQL = "update crm_smm_shopify_service set shopify_status = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "update adm_mst_tmoduleangular set shopify_flag = 'N' where module_name = 'Shopify'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    //}
                    //else
                    //{
                    values.status = true;
                    values.message = "Disabled Successfully!!";
                }




            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating shopify service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateEmailService(string user_gid, emailservice_list values)
        {
            try
            {
                msSQL = "select access_token,base_url from crm_smm_mail_service";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == false)
                {

                    msSQL = " insert into crm_smm_mail_service(" +
                            " access_token," +
                            " base_url," +
                            " receiving_domain," +
                            " sending_domain," +
                            " email_username," +
                            " created_by," +
                            " email_status," +
                            " created_date)" +
                            " values(" +
                            "'" + values.mail_access_token + "'," +
                            " '" + values.mail_base_url + "'," +
                            " '" + values.receiving_domain + "'," +
                            " '" + values.sending_domain + "'," +
                            " '" + values.email_username + "',";
                    msSQL += "'" + user_gid + "'," +
                         "'E'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmMailcampaignsummary' where module_name = 'Email'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " UPDATE crm_smm_tmailconfig SET switch_flag = CASE WHEN mail_service = '" + values.mail_service + "' THEN 'Y' ELSE 'N' END ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {

                            values.status = true;
                            values.message = "Email Credentials Updated Successfully !!";

                        }


                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Email Credentials !!";
                    }
                }
                else
                {
                    msSQL = " update  crm_smm_mail_service set " +

                    " access_token = '" + values.mail_access_token + "'," +
                    " base_url = '" + values.mail_base_url + "'," +
                    " updated_by = '" + user_gid + "'," +
                    " receiving_domain= '" + values.receiving_domain + "'," +
                    " sending_domain= '" + values.sending_domain + "'," +
                    " email_status= 'E'," +
                    " email_username= '" + values.email_username + "'," +

                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmMailcampaignsummary' where module_name = 'Email'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " UPDATE crm_smm_tmailconfig SET switch_flag = CASE WHEN mail_service = '" + values.mail_service + "' THEN 'Y' ELSE 'N' END ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {

                            values.status = true;
                            values.message = "Email Credentials Updated Successfully!!";

                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Email Credentials !!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Email Credentials !!";
                    }
                }



            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating email service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaUpdategmailService(string user_gid, gmailservice_list values)
        {
            try
            {
                if (values.gmail_status == "G")
                {
                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmGmailcampaignsummary' where module_name = 'Email'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmMailscompose' where module_name = 'Compose Mail'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmGmailFolderSummary' where module_name = 'Mail Folders'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmGmailDirectInboxSummary' where module_name = 'Inbox'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmMailsent' where module_name = 'Sent Items'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmGmailTrashSummary' where module_name = 'Trash'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update crm_smm_mail_service set email_status = ''";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " update crm_smm_outlook_service set outlook_status = ''";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = "update crm_smm_gmail_service set gmail_status = 'G'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    msSQL = " UPDATE crm_smm_tmailconfig SET switch_flag = CASE WHEN mail_service = '" + values.mail_service + "' THEN 'Y' ELSE 'N' END ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL); 
                    if (mnResult != 0)
                    {

                        values.status = true;
                        values.message = "Email Credentials Updated Successfully !!";

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Email Credentials !!";
                    }
                }
                else
                {
                    msSQL = "update crm_smm_gmail_service set gmail_status = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "update crm_smm_tmailconfig set switch_flag = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "update adm_mst_tmoduleangular set shopify_flag = 'N',lw_flag = 'N' where module_name = 'Email' or module_name = 'Compose Mail' or module_name = 'Inbox' or module_name = 'Sent Items' or module_name = 'Mail Folders' or module_name = 'Trash' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Disabled Successfully!!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating email service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetFacebookServiceSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select access_token,page_id,s_no,facebook_status,created_by,created_date from crm_smm_tfacebookservice ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<facebookcampaignservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new facebookcampaignservice_list
                        {
                            facebook_access_token = dt["access_token"].ToString(),
                            facebook_page_id = dt["page_id"].ToString(),
                            facebook_id = dt["s_no"].ToString(),
                            facebook_status = dt["facebook_status"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                        });
                        values.facebookcampaignservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting face book service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostFacebookkeys(string user_gid, facebookcampaignservice_list values)
        {
            try
            {
                msSQL = " select page_id from crm_smm_tfacebookservice where page_id = '" + values.facebook_page_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Page Id Already Exist !!";
                }
                else
                {
                    msSQL = " insert into crm_smm_tfacebookservice(" +
                                " page_id," +
                                " facebook_status," +
                                " access_token," +
                                " created_by," +
                                " created_date)" +
                                " values(" +
                                "'" + values.facebook_page_id + "'," +
                                 "'Y'," +
                                "'" + values.facebook_access_token + "',";
                    msSQL += "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Facebook Keys Added Successfully";
                    }
                    else
                    {

                        values.status = false;
                        values.message = "Error While Adding Facebook Keys";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating facebook service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void Daupdatefacebookkeys(string user_gid, facebookcampaignservice_list values)
        {
            try
            {
                if (values.facebook_status == "Y")
                {
                    msSQL = "update crm_smm_tfacebookservice  set facebook_status='Y' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Facebook'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Facebook'";
                            lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                            lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (lsshopify_flag == "N")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = "Facebook Credentials Updated Successfully !!";
                                }
                            }
                            else if (lsshopify_flag == "")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                values.status = true;
                                values.message = "Updated Successfully!!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Facebook Credentials!!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Facebook Credentials !!";
                        }


                    }
                }
                else
                {
                    msSQL = " update crm_smm_tfacebookservice set facebook_status = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'N' where module_name = 'Facebook'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Disabled Successfully!!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating facebook service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void Dadeleteaccesstoken(string page_id, facebookcampaignservice_list values)
        {

            try
            {
                msSQL = "  delete from crm_smm_tfacebookservice where page_id='" + page_id + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Key Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Key";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Access Token!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void Dadeletemailkey(string s_no, gmailservice_lists values)
        {

            try
            {
                msSQL = " select gmail_address from crm_smm_gmail_service where s_no ='" + s_no + "' limit 1";
                string gmail_address = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select integrated_gmail from hrm_mst_temployee where integrated_gmail ='" + gmail_address + "' limit 1";
                string integrated_gmail = objdbconn.GetExecuteScalar(msSQL);
                if (integrated_gmail == null || integrated_gmail == "")
                {
                    msSQL = "  delete from crm_smm_gmail_service where s_no='" + s_no + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Keys Deleted Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Keys";
                    }
                }
                else
                {
                    values.message = "Gmail Mapped to Employee Keys can't Delete !";
                    values.status = false;
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Key!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetLinkedinServiceSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select access_token,s_no,linkedin_status from crm_smm_tlinkedinservice limit 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<linkedincampaignservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new linkedincampaignservice_list
                        {
                            linkedin_access_token = dt["access_token"].ToString(),
                            linkedin_id = dt["s_no"].ToString(),
                            linkedin_status = dt["linkedin_status"].ToString(),
                        });
                        values.linkedincampaignservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting linkedin service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateLinkedinService(string user_gid, linkedincampaignservice_list values)
        {
            try
            {
                if (values.linkedin_status == "Y")
                {
                    if (values.linkedin_id == null || values.linkedin_id == "")
                    {


                        msSQL = " insert into crm_smm_tlinkedinservice(" +
                           " access_token," +
                           " created_by," +
                           " created_date)" +
                            " linkedin_status," +
                           " values(" +
                           "'" + values.linkedin_access_token + "',";
                        msSQL += "'" + user_gid + "'," +
                               "'Y'" +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Linkedin'";
                            lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);

                            msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                            lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (lsshopify_flag == "N")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = "Linkedin Credentials Updated Successfully !!";
                                }
                            }
                            else if (lsshopify_flag == "")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                values.status = true;
                                values.message = "Shopify Credentials Updated Successfully!!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Linkedin Credentials!!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Linedin Credentials !!";
                        }

                    }
                    else
                    {
                        msSQL = " update  crm_smm_tlinkedinservice set " +

                                " access_token = '" + values.linkedin_access_token + "'," +
                                " updated_by = '" + user_gid + "'," +
                                " linkedin_status = 'Y'," +
                                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.linkedin_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Linkedin'";
                            lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                            lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (lsshopify_flag == "N")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = "Linkedin Credentials Updated Successfully !!";
                                }
                            }
                            else if (lsshopify_flag == "")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                values.status = true;
                                values.message = "Shopify Credentials Updated Successfully!!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Linkedin Credentials!!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Linkedin Credentials !!";
                        }
                    }
                }

                else
                {
                    msSQL = " update crm_smm_tlinkedinservice set linkedin_status = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'N' where module_name = 'Linkedin'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Disabled Successfully!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updatsing linked services!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetTelegramServiceSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select bot_id,chat_id,s_no,telegram_status from crm_smm_ttelegramservice limit 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<telegramcampaignservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new telegramcampaignservice_list
                        {
                            bot_id = dt["bot_id"].ToString(),
                            chat_id = dt["chat_id"].ToString(),
                            telegram_id = dt["s_no"].ToString(),
                            telegram_status = dt["telegram_status"].ToString(),
                        });
                        values.telegramcampaignservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting telegram service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateTelegramService(string user_gid, telegramcampaignservice_list values)
        {
            try
            {
                if (values.telegram_status == "Y")
                {
                    if (values.telegram_id == null || values.telegram_id == "")
                    {


                        msSQL = " insert into crm_smm_ttelegramservice(" +
                                " bot_id," +
                                " chat_id," +
                                " created_by," +
                                " telegram_status," +
                                " created_date)" +
                                " values(" +
                                "'" + values.bot_id + "'," +
                                   "'" + values.chat_id + "',";
                        msSQL += "'" + user_gid + "'," +
                                    "'Y'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Telegram'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Telegram'";
                                lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                                lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                                if (lsshopify_flag == "N")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Telegram Credentials Updated Successfully !!";
                                    }
                                }
                                else if (lsshopify_flag == "")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    values.status = true;
                                    values.message = "Shopify Credentials Updated Successfully!!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Telegram Credentials!!";
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Telegram Credentials !!";
                            }

                        }
                    }
                    else
                    {
                        msSQL = " update  crm_smm_ttelegramservice set " +
                        " bot_id = '" + values.bot_id + "'," +
                        " chat_id = '" + values.chat_id + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " telegram_status='Y'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.telegram_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Telegram'";
                            lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                            lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (lsshopify_flag == "N")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = "Telegram Credentials Updated Successfully !!";
                                }
                            }
                            else if (lsshopify_flag == "")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                values.status = true;
                                values.message = "Shopify Credentials Updated Successfully!!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Telegram Credentials!!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Telegram Credentials !!";
                        }
                    }
                }

                else
                {
                    msSQL = " update crm_smm_ttelegramservice set telegram_status = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'N' where module_name = 'Telegram'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Disabled Successfully!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating telegram service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetCustomerTypeSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = "select customertype_gid,display_name from crm_mst_tcustomertype ORDER BY customertype_gid ASC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<customertype_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new customertype_list
                        {
                            customertype_gid = dt["customertype_gid"].ToString(),
                            customer_type = dt["display_name"].ToString(),
                        });
                        values.customertype_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting customer type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaUpdateCustomerType(string user_gid, customertype_list values)
        {
            try
            {
                if (values.corporate_gid != null)
                {
                    msSQL = "select display_name from crm_mst_tcustomertype where customertype_gid = 'BCRT240331000'";
                    string lscorporate_type = objdbconn.GetExecuteScalar(msSQL);

                    if (lscorporate_type != values.corporate_name)
                    {
                        msSQL = " select customertype_gid,display_name,created_by,updated_by,created_date,updated_date" +
                          " from crm_mst_tcustomertype where " +
                          " customertype_gid='BCRT240331000'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lscustomertype_gid = objOdbcDataReader["customertype_gid"].ToString();
                            lscustomer_type = objOdbcDataReader["display_name"].ToString();
                            lscreated_by = objOdbcDataReader["created_by"].ToString();
                            lsupdated_by = objOdbcDataReader["updated_by"].ToString();
                            lscreated_date = objOdbcDataReader["created_date"].ToString();
                            lsupdated_date = objOdbcDataReader["updated_date"].ToString();
                        }
                        msGetGid = objcmnfunctions.GetMasterGID("BCTL");
                        msSQL = " insert into crm_trn_tcustomertypelog(" +
                                " customertypelog_gid," +
                                " pre_customertype_gid," +
                                " pre_customertype," +
                                " curr_customertype," +
                                " updated_by, " +
                                " updated_date)" +
                                " values(" +
                                " '" + msGetGid + "'," +
                                " '" + lscustomertype_gid + "'," +
                                "'" + lscustomer_type + "'," +
                                "'" + values.corporate_name + "'," +
                                "'" + lscreated_by + "'," +
                                "STR_TO_DATE('" + lscreated_date + "', '%m/%d/%Y %h:%i:%s %p'))";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            msSQL = " update  crm_mst_tcustomertype set " +
                                    " display_name = '" + values.corporate_name + "'," +
                                    " updated_by = '" + user_gid + "'," +
                                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                    " where customertype_gid='" + values.corporate_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                msSQL = "update crm_trn_tleadbank set customer_type ='" + values.corporate_name + "' where customertype_gid='" + lscustomertype_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Customer Type Updated Successfully !!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Customer Type !!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Customer Type !!";
                        }
                    }


                }
                if (values.retailer_gid != null)
                {
                    msSQL = "select display_name from crm_mst_tcustomertype where customertype_gid = 'BCRT240331001'";
                    string lscorporate_type = objdbconn.GetExecuteScalar(msSQL);

                    if (lscorporate_type != values.retailer_name)
                    {
                        msSQL = " select customertype_gid,display_name,created_by,updated_by,created_date,updated_date" +
                          " from crm_mst_tcustomertype where " +
                          " customertype_gid='BCRT240331001'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lscustomertype_gid = objOdbcDataReader["customertype_gid"].ToString();
                            lscustomer_type = objOdbcDataReader["display_name"].ToString();
                            lscreated_by = objOdbcDataReader["created_by"].ToString();
                            lsupdated_by = objOdbcDataReader["updated_by"].ToString();
                            lscreated_date = objOdbcDataReader["created_date"].ToString();
                            lsupdated_date = objOdbcDataReader["updated_date"].ToString();
                        }
                        msGetGid = objcmnfunctions.GetMasterGID("BCTL");
                        msSQL = " insert into crm_trn_tcustomertypelog(" +
                                " customertypelog_gid," +
                                " pre_customertype_gid," +
                                " pre_customertype," +
                                " curr_customertype," +
                                " updated_by, " +
                                " updated_date)" +
                                " values(" +
                                " '" + msGetGid + "'," +
                                " '" + lscustomertype_gid + "'," +
                                "'" + lscustomer_type + "'," +
                                "'" + values.retailer_name + "'," +
                                "'" + lscreated_by + "'," +
                                "STR_TO_DATE('" + lscreated_date + "', '%m/%d/%Y %h:%i:%s %p'))";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            msSQL = " update  crm_mst_tcustomertype set " +
                                    " display_name = '" + values.retailer_name + "'," +
                                    " updated_by = '" + user_gid + "'," +
                                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                    " where customertype_gid='" + values.retailer_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                msSQL = "update crm_trn_tleadbank set customer_type ='" + values.retailer_name + "' where customertype_gid='" + lscustomertype_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Customer Type Updated Successfully !!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Customer Type !!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Customer Type !!";
                        }
                    }

                }
                if (values.distributor_gid != null)
                {
                    msSQL = "select display_name from crm_mst_tcustomertype where customertype_gid = 'BCRT240331002'";
                    string lscorporate_type = objdbconn.GetExecuteScalar(msSQL);

                    if (lscorporate_type != values.distributor_name)
                    {
                        msSQL = " select customertype_gid,display_name,created_by,updated_by,created_date,updated_date" +
                          " from crm_mst_tcustomertype where " +
                          " customertype_gid='BCRT240331002'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows)
                        {
                            lscustomertype_gid = objOdbcDataReader["customertype_gid"].ToString();
                            lscustomer_type = objOdbcDataReader["display_name"].ToString();
                            lscreated_by = objOdbcDataReader["created_by"].ToString();
                            lsupdated_by = objOdbcDataReader["updated_by"].ToString();
                            lscreated_date = objOdbcDataReader["created_date"].ToString();
                            lsupdated_date = objOdbcDataReader["updated_date"].ToString();
                        }
                        msGetGid = objcmnfunctions.GetMasterGID("BCTL");
                        msSQL = " insert into crm_trn_tcustomertypelog(" +
                                " customertypelog_gid," +
                                " pre_customertype_gid," +
                                " pre_customertype," +
                                " curr_customertype," +
                                " updated_by, " +
                                " updated_date)" +
                                " values(" +
                                " '" + msGetGid + "'," +
                                " '" + lscustomertype_gid + "'," +
                                "'" + lscustomer_type + "'," +
                                "'" + values.distributor_name + "'," +
                                "'" + lscreated_by + "'," +
                                "STR_TO_DATE('" + lscreated_date + "', '%m/%d/%Y %h:%i:%s %p'))";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            msSQL = " update  crm_mst_tcustomertype set " +
                                    " display_name = '" + values.distributor_name + "'," +
                                    " updated_by = '" + user_gid + "'," +
                                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                    " where customertype_gid='" + values.distributor_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                msSQL = "update crm_trn_tleadbank set customer_type ='" + values.distributor_name + "' where customertype_gid='" + lscustomertype_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                            if (mnResult == 1)
                            {
                                values.status = true;
                                values.message = "Customer Type Updated Successfully !!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Customer Type !!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Customer Type !!";
                        }
                    }

                }

                //msSQL = " select customer_type from crm_mst_tcustomertype where customer_type = '" + values.customer_type + "'";
                //objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                //if (objOdbcDataReader.HasRows == false)
                //{ }

                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Customer Type Details";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetLivechatServiceSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select s_no,id,access_token,livechat_status from crm_smm_tinlinechatservice limit 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<livechatservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new livechatservice_list
                        {
                            livechat_agentid = dt["id"].ToString(),
                            livechat_access_token = dt["access_token"].ToString(),
                            livechat_id = dt["s_no"].ToString(),
                            livechat_status = dt["livechat_status"].ToString(),
                        });
                        values.livechatservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Livechat service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateLivechatService(string user_gid, livechatservice_list values)
        {
            try
            {
                if (values.livechat_status == "Y")
                {
                    if (values.livechat_id == null || values.livechat_id == "")
                    {
                        msSQL = " insert into crm_smm_tinlinechatservice(" +
                                " id," +
                                " access_token," +
                                " created_by," +
                                " livechat_status," +
                                " created_date)" +
                                " values(" +
                                "'" + values.livechat_agentid + "'," +
                                   "'" + values.livechat_access_token + "',";
                        msSQL += "'" + user_gid + "'," +
                                    "'Y'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Live Chat'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Live Chat'";
                                lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                                lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                                if (lsshopify_flag == "N")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Live Chat Credentials Updated Successfully !!";
                                    }
                                }
                                else if (lsshopify_flag == "")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    values.status = true;
                                    values.message = "Shopify Credentials Updated Successfully!!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Live Chat Credentials!!";
                                }

                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Live Chat Credentials !!";
                            }
                        }
                    }
                    else
                    {
                        msSQL = " update  crm_smm_tinlinechatservice set " +
                         " id = '" + values.livechat_agentid + "'," +
                         " access_token = '" + values.livechat_access_token + "'," +
                         " updated_by = '" + user_gid + "'," +
                         " livechat_status='Y'," +
                         " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.livechat_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Live Chat'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Live Chat'";
                                lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);

                                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                                lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                                if (lsshopify_flag == "N")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Live Chat Credentials Updated Successfully !!";
                                    }
                                }
                                else if (lsshopify_flag == "")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    values.status = true;
                                    values.message = "Live Chat Credentials Updated Successfully!!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Live Chatv Credentials!!";
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Whatsapp Credentials !!";
                            }
                        }
                    }
                }
                else
                {
                    msSQL = " update crm_smm_tinlinechatservice set livechat_status = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'N' where module_name = 'Live Chat'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Disabled Successfully!!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Live Chat service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetCompanySummary(MdlCampaignService values)
        {
            try
            {
                msSQL = "SELECT company_gid, company_code, company_name, company_address, company_address1, salary_startdate, company_phone, company_website, " +
                        "company_mail, contact_person, manufacturer_licence, auth_code, fax, " +
                        "pop_server, pop_port, pop_username, pop_password, currency_code, company_logo_path, " +
                        "welcome_logo, company_logo, authorised_sign, sequence_reset, country_name, country_gid, company_state, " +
                        "contact_person_mail, contact_person_phone FROM adm_mst_tcompany";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<Company_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        //domain = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "");
                        //string relativeLogoPath = dt["welcome_logo"].ToString();

                        //domain = ConfigurationManager.AppSettings["domain"].ToString();

                        //string combinedPath = domain + "/" + relativeLogoPath;

                        getmodulelist.Add(new Company_list
                        {
                            company_code = dt["company_code"].ToString(),
                            company_name = dt["company_name"].ToString(),
                            company_phone = dt["company_phone"].ToString(),
                            company_mail = dt["company_mail"].ToString(),
                            contact_person = dt["contact_person"].ToString(),
                            company_address = dt["company_address"].ToString(),
                            contact_person_mail = dt["contact_person_mail"].ToString(),
                            contact_person_phone = dt["contact_person_phone"].ToString(),
                            company_state = dt["company_state"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            sequence_reset = dt["sequence_reset"].ToString(),
                            company_gid = dt["company_gid"].ToString(),
                            company_logo = dt["company_logo"].ToString(),
                            welcome_logo = dt["welcome_logo"].ToString(),
                            company_address1 = dt["company_address1"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            //currency = dt["currency"].ToString(),
                        });
                        values.Company_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Company Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetcurrency(MdlCampaignService values)
        {
            try
            {
                msSQL = "select default_currency,currency_code,concat(currency_code,'/',exchange_rate)as currency,symbol " +
                        " from crm_trn_tcurrencyexchange where default_currency='Y'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<currency_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new currency_list
                        {
                            currency = dt["currency"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            symbol = dt["symbol"].ToString(),
                        });
                        values.currency_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Cumpany Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetCountry(MdlCampaignService values)
        {
            try
            {
                msSQL = " Select country_gid,country_name From adm_mst_tcountry order by country_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<country_list2>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new country_list2
                        {
                            country_gid = dt["country_gid"].ToString(),
                            country_name = dt["country_name"].ToString(),

                        });
                        values.country_list2 = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Cumpany Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostCompanyDetails(HttpRequest httpRequest, result objResult)
        {
            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            string lscompany_code = string.Empty;
            HttpPostedFile httpPostedFile;

            string lspath;
            string msGetGid;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            //string product_gid = httpRequest.Form[0];
            string company_code = httpRequest.Form[0];
            string company_name = httpRequest.Form[1];
            string company_phone = httpRequest.Form[2];
            string company_mail = httpRequest.Form[3];
            string contact_person = httpRequest.Form[4];
            string company_address = httpRequest.Form[5];
            string company_address1 = httpRequest.Form[6];
            string contact_person_mail = httpRequest.Form[7];
            string contact_person_phone = httpRequest.Form[8];
            string company_state = httpRequest.Form[9];
            string country_name = httpRequest.Form[10];
            string company_gid = httpRequest.Form[12];
            httpFileCollection = httpRequest.Files;


            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;

                    //if (httpPostedFile.FileName != "")
                    //{
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        // Create a new MemoryStream for each file in the collection
                        using (MemoryStream ms = new MemoryStream())
                        {
                            httpPostedFile = httpFileCollection[i];
                            string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                            string FileExtension = httpPostedFile.FileName;
                            string lsfile_gid = msdocument_gid;
                            string lscompany_document_flag = string.Empty;
                            FileExtension = Path.GetExtension(FileExtension).ToLower();
                            lsfile_gid = lsfile_gid + FileExtension;

                            // Copy the file stream to the memory stream
                            Stream ls_readStream = httpPostedFile.InputStream;
                            ls_readStream.CopyTo(ms);
                            ms.Position = 0; // Reset the position to the start of the stream

                            bool status1 = objcmnfunctions.UploadStream(
                                ConfigurationManager.AppSettings["blob_containername"],
                                lscompany_code + "/" + "CRM/companylogo/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension,
                                FileExtension,
                                ms
                            );

                            // Construct the final path
                            final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/companylogo/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                            // Update the SQL statement
                            msSQL = "update adm_mst_tcompany set " +
                                    httpRequest.Files.AllKeys[i] + " = '" +
                                    ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension +
                                    ConfigurationManager.AppSettings["blob_imagepath2"] + '&' +
                                    ConfigurationManager.AppSettings["blob_imagepath3"] + '&' +
                                    ConfigurationManager.AppSettings["blob_imagepath4"] + '&' +
                                    ConfigurationManager.AppSettings["blob_imagepath5"] + '&' +
                                    ConfigurationManager.AppSettings["blob_imagepath6"] + '&' +
                                    ConfigurationManager.AppSettings["blob_imagepath7"] + '&' +
                                    ConfigurationManager.AppSettings["blob_imagepath8"] + "'";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }

                    //}

                    //    httpPostedFile = httpRequest.Files[1];
                    //if (httpPostedFile.FileName != "")
                    //{
                    //    MemoryStream ms = new MemoryStream();
                    //    for (int i = 0; i < httpFileCollection.Count; i++)
                    //    {
                    //        string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                    //        //httpPostedFile = httpFileCollection[i];
                    //        string FileExtension = httpPostedFile.FileName;
                    //        //string lsfile_gid = msdocument_gid + FileExtension;
                    //        string lsfile_gid = msdocument_gid;
                    //        string lscompany_document_flag = string.Empty;
                    //        FileExtension = Path.GetExtension(FileExtension).ToLower();
                    //        lsfile_gid = lsfile_gid + FileExtension;
                    //        Stream ls_readStream;
                    //        ls_readStream = httpPostedFile.InputStream;
                    //        ls_readStream.CopyTo(ms);

                    //        bool status1;


                    //        status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/welcomlogo/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);


                    //        final_path1 = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/welcomlogo/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";



                    //    msSQL = " update  adm_mst_tcompany set " +
                    //            " welcome_logo = '"+ ConfigurationManager.AppSettings["blob_imagepath1"] + final_path1 + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                    //                                   '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                    //                                   '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"] + "'" +
                    //            " where company_gid='" + company_gid + "'";
                    //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    //    }
                    //    ms.Close();
                    //}
                    msSQL = " Select country_gid from adm_mst_tcountry where country_name ='" + country_name + "'";
                    string lscountry_gid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " update  adm_mst_tcompany set " +
                                " company_code = '" + company_code.Replace("'", "\\'") + "'," +
                                " company_phone = '" + company_phone.Replace("'", "\\'") + "'," +
                                " company_mail = '" + company_mail.Replace("'", "\\'") + "'," +
                                " contact_person = '" + contact_person.Replace("'", "\\'") + "'," +
                                " company_address = '" + company_address.Replace("'", "\\'") + "'," +
                                 " company_address1 = '" + company_address1.Replace("'", "\\'") + "'," +
                                " contact_person_mail = '" + contact_person_mail.Replace("'", "\\'") + "'," +
                                " contact_person_phone = '" + contact_person_phone.Replace("'", "\\'") + "'," +
                                " company_state = '" + company_state.Replace("'", "\\'") + "'," +
                                " country_gid = '" + lscountry_gid + "'," +
                                " country_name = '" + country_name.Replace("'", "\\'") + "'" +
                                " where company_gid='" + company_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        objResult.status = true;
                        objResult.message = "Company Details Updated Successfully !!";
                    }
                    else
                    {
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + "***********" + objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        objResult.status = false;
                        objResult.message = "Error While Updating Company Details !!";
                    }
                }

            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Uploading Company Details Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostCompanyDetailsForm(Company_list values, string user_gid)
        {
            try
            {
                msSQL = " Select country_gid from adm_mst_tcountry where country_name ='" + values.country_name + "'";
                string lscountry_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " update  adm_mst_tcompany set " +
                               " company_code = '" + values.company_code.Replace("'", "\\'") + "'," +
                               " company_phone = '" + values.company_phone.Replace("'", "\\'") + "'," +
                               " company_mail = '" + values.company_mail.Replace("'", "\\'") + "'," +
                               " contact_person = '" + values.contact_person.Replace("'", "\\'") + "'," +
                               " company_address = '" + values.company_address.Replace("'", "\\'") + "'," +
                                " company_address1 = '" + values.company_address1.Replace("'", "\\'") + "'," +
                               " contact_person_mail = '" + values.contact_person_mail.Replace("'", "\\'") + "'," +
                               " contact_person_phone = '" + values.contact_person_phone.Replace("'", "\\'") + "'," +
                               " company_state = '" + values.company_state.Replace("'", "\\'") + "'," +
                               " country_gid = '" + lscountry_gid + "'," +
                               " country_name = '" + values.country_name.Replace("'", "\\'") + "'" +
                               " where company_gid='" + values.company_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Company Details Updated Successfully !!";
                }
                else
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                    values.status = false;
                    values.message = "Error While Updating Company Details !!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error While Updating Company Details  !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "*************Query****" + "Error occured while Adding Journal Entry!! " + " *******" + msSQL + "*******Apiref********", "Marketing/ErrorLog/Company/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetModuleNameSummary(string user_gid, MdlCampaignService values)
        {
            try
            {
                msSQL = " select distinct b.module_gid,b.module_name from adm_mst_tprivilegeangular a " +
                        " left join adm_mst_tmoduleangular b on a.module_gid=b.module_gid " +
                       " where a.user_gid='" + user_gid + "' and b.menu_level='1' order by b.module_name asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<module_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)

                    {
                        getmodulelist.Add(new module_list
                        {
                            module_gid = dt["module_gid"].ToString(),
                            module_name = dt["module_name"].ToString(),

                        });
                        values.module_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Cumpany Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }


        public void DaGetScreenNameSummary(string user_gid, MdlCampaignService values, string module_gid)
        {
            try
            {
                msSQL = "select max_menulevel from adm_mst_tmoduleangular where module_gid='" + module_gid + "'";
                max_menulevel = objdbconn.GetExecuteScalar(msSQL);
                if (max_menulevel == "2")
                {
                    msSQL = " select distinct a.module_gid,b.module_name from adm_mst_tprivilegeangular a " +
                       " left join adm_mst_tmoduleangular b on a.module_gid=b.module_gid " +
                       " where a.user_gid='" + user_gid + "' and a.module_gid like '" + module_gid + "%'" +
                        " and b.menu_level in ('2') order by b.module_name asc";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getmodulelist = new List<submodule_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getmodulelist.Add(new submodule_list
                            {
                                Module_gid = dt["module_gid"].ToString(),
                                Module_name = dt["module_name"].ToString(),

                            });
                            values.submodule_list = getmodulelist;
                        }
                    }
                    dt_datatable.Dispose();
                }
                else
                {
                    msSQL = " select distinct a.module_gid,b.module_name from adm_mst_tprivilegeangular a " +
                        " left join adm_mst_tmoduleangular b on a.module_gid=b.module_gid " +
                        " where a.user_gid='" + user_gid + "' and a.module_gid like '%" + module_gid + "%' " +
                        "and b.menu_level in ('3') order by b.module_name asc";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getmodulelist = new List<submodule_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getmodulelist.Add(new submodule_list
                            {
                                Module_gid = dt["module_gid"].ToString(),
                                Module_name = dt["module_name"].ToString(),

                            });
                            values.submodule_list = getmodulelist;
                        }
                    }
                    dt_datatable.Dispose();
                }


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Cumpany Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void Daupdatemodulename(string employee_gid, updatemodule_list values)
        {

            msSQL = " update hrm_mst_temployee set " +
                    " default_screen = '" + values.Module_name + "', " +
                    " default_module='" + values.module_name + "',home_page = CASE WHEN default_screen = 'SMRHMP' THEN 'Y' ELSE 'N' END  where employee_gid='" + employee_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Updated Welcome Page !!";
            }
            else
            {
                values.status = false;
                values.message = "Error While Updating Welcome Pade!!";
            }

        }
        public void DaGetModuleSummery(string user_gid, MdlCampaignService values)
        {
            try
            {
                msSQL = " select b.module_name from hrm_mst_temployee a " +
                        " left join adm_mst_tmoduleangular b on a.default_screen = b.module_gid or a.default_module = b.module_gid " +
                        " where user_gid = '" + user_gid + "' order by menu_level asc  ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<modulesummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new modulesummary_list
                        {

                            module_names = dt["module_name"].ToString(),

                        });
                        values.modulesummary_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Cumpany Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateCicktocallService(string user_gid, clicktocall_list values)
        {
            try
            {
                if (values.clicktocall_status == "Y")
                {
                    if (values.clicktocall_id == null || values.clicktocall_id == "")
                    {
                        msSQL = " insert into crm_smm_tclicktocallservice(" +
                                " base_url," +
                                " access_token," +
                                " created_by," +
                                " clicktocall_status," +
                                " created_date)" +
                                " values(" +
                                "'" + values.clicktocall_baseurl + "'," +
                                   "'" + values.clicktocall_access_token + "',";
                        msSQL += "'" + user_gid + "'," +
                                    "'Y'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Click To Call'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Click To Call'";
                                lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                                lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                                if (lsshopify_flag == "N")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Click To Call Credentials Updated Successfully !!";
                                    }
                                }
                                else if (lsshopify_flag == "")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    values.status = true;
                                    values.message = "Shopify Credentials Updated Successfully!!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Click To Call Credentials!!";
                                }

                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Click To Call Credentials !!";
                            }
                        }
                    }
                    else
                    {
                        msSQL = " update  crm_smm_tclicktocallservice set " +
                         " base_url = '" + values.clicktocall_baseurl + "'," +
                         " access_token = '" + values.clicktocall_access_token + "'," +
                         " updated_by = '" + user_gid + "'," +
                         " clicktocall_status='Y'," +
                         " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.clicktocall_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Click To Call'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Click To Call'";
                                lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);

                                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                                lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                                if (lsshopify_flag == "N")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Click To Call Credentials Updated Successfully !!";
                                    }
                                }
                                else if (lsshopify_flag == "")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    values.status = true;
                                    values.message = "Click To Call Credentials Updated Successfully!!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Click To Call Credentials!!";
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Click To Call Credentials !!";
                            }
                        }
                    }
                }
                else
                {
                    msSQL = " update crm_smm_tclicktocallservice set clicktocall_status = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'N' where module_name = 'Click To Call'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Disabled Successfully!!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Click To Call service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetClicktocallSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select s_no,base_url,access_token,clicktocall_status from crm_smm_tclicktocallservice limit 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<clicktocall_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new clicktocall_list
                        {
                            clicktocall_baseurl = dt["base_url"].ToString(),
                            clicktocall_access_token = dt["access_token"].ToString(),
                            clicktocall_id = dt["s_no"].ToString(),
                            clicktocall_status = dt["clicktocall_status"].ToString(),
                        });
                        values.clicktocall_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Click To Call service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetGoogleanalyticsserviceSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select s_no, user_url, page_url, googleanalytics_status from crm_smm_tgoogleanalyticsservice limit 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<googleanalyticsservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new googleanalyticsservice_list
                        {
                            user_url = dt["user_url"].ToString(),
                            page_url = dt["page_url"].ToString(),
                            googleanalytics_id = dt["s_no"].ToString(),
                            googleanalytics_status = dt["googleanalytics_status"].ToString(),
                        });
                        values.googleanalyticsservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Click To Call service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void Daupdategoogleanalyticsservice(string user_gid, googleanalyticsservice_list values)
        {
            try
            {
                if (values.googleanalytics_status == "Y")
                {
                    if (values.googleanalytics_id == null || values.googleanalytics_id == "")
                    {
                        msSQL = " insert into crm_smm_tgoogleanalyticsservice(" +
                                " user_url," +
                                " page_url," +
                                " created_by," +
                                " googleanalytics_status," +
                                " created_date)" +
                                " values(" +
                                "'" + values.user_url + "'," +
                                   "'" + values.page_url + "',";
                        msSQL += "'" + user_gid + "'," +
                                    "'Y'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Website'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult != 0)
                            {
                                msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Website'";
                                lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                                lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                                if (lsshopify_flag == "N")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Google Analytics Credentials Updated Successfully !!";
                                    }
                                }
                                else if (lsshopify_flag == "")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    values.status = true;
                                    values.message = "Google Analytics Credentials Updated Successfully!!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Google Analytics Credentials!!";
                                }

                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Google Analytics Credentials !!";
                            }
                        }
                    }
                    else
                    {
                        msSQL = " update  crm_smm_tgoogleanalyticsservice set " +
                         " user_url = '" + values.user_url + "'," +
                         " page_url = '" + values.page_url + "'," +
                         " updated_by = '" + user_gid + "'," +
                         " googleanalytics_status='Y'," +
                         " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.googleanalytics_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult != 0)
                        {
                            msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Website'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Website'";
                                lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);

                                msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                                lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                                if (lsshopify_flag == "N")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Click To Call Credentials Updated Successfully !!";
                                    }
                                }
                                else if (lsshopify_flag == "")
                                {
                                    msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                    values.status = true;
                                    values.message = "Google Analytics Credentials Updated Successfully!!";
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Updating Google Analytics Credentials!!";
                                }
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Google Analytics Credentials!!";
                            }
                        }
                    }
                }
                else
                {
                    msSQL = " update crm_smm_tgoogleanalyticsservice set googleanalytics_status = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'N' where module_name = 'Website'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Disabled Successfully!!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Click To Call service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetSmsServiceSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select sms_user_id,sms_password,s_no,sms_status from crm_smm_tsmsservice limit 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<smscampaignservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new smscampaignservice_list
                        {
                            sms_user_id = dt["sms_user_id"].ToString(),
                            sms_password = dt["sms_password"].ToString(),
                            sms_id = dt["s_no"].ToString(),
                            sms_status = dt["sms_status"].ToString(),
                        });
                        values.smscampaignservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting face book service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateSmsService(string user_gid, smscampaignservice_list values)
        {
            try
            {
                if (values.sms_status == "Y")
                {
                    if (values.sms_id == null || values.sms_id == "")
                    {


                        msSQL = " insert into crm_smm_tsmsservice(" +
                                " sms_user_id," +
                                " sms_password," +
                                " created_by," +
                                " sms_status," +
                                " created_date)" +
                                " values(" +
                                "'" + values.sms_user_id + "'," +
                                "'" + values.sms_password + "',";
                        msSQL += "'" + user_gid + "'," +
                                 "'Y'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='SMS'";
                            lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                            lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (lsshopify_flag == "N")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = "SMS Credentials Updated Successfully !!";
                                }
                            }
                            else if (lsshopify_flag == "")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                values.status = true;
                                values.message = "Shopify Credentials Updated Successfully!!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating SMS Credentials!!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating SMS Credentials !!";
                        }

                    }
                    else
                    {
                        msSQL = " update crm_smm_tsmsservice set " +

                                " sms_user_id = '" + values.sms_user_id + "'," +
                                " sms_password = '" + values.sms_password + "'," +
                                " updated_by = '" + user_gid + "'," +
                                " sms_status = 'Y'," +
                                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.sms_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='SMS'";
                            lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                            lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (lsshopify_flag == "N")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = "SMS Credentials Updated Successfully !!";
                                }
                            }
                            else if (lsshopify_flag == "")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                values.status = true;
                                values.message = "Credentials Updated Successfully!!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating SMS Credentials!!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating SMS Credentials !!";
                        }
                    }
                }

                else
                {
                    msSQL = " update crm_smm_tsmsservice set sms_status = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'N' where module_name = 'SMS'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Disabled Successfully!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating SMS service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }

        public void DaGetIndiaMARTServiceSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select api_key,s_no,indiamart_status from crm_smm_tindiamartservice limit 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<indiamartcampaignservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new indiamartcampaignservice_list
                        {
                            api_key = dt["api_key"].ToString(),
                            indiamart_id = dt["s_no"].ToString(),
                            indiamart_status = dt["indiamart_status"].ToString(),
                        });
                        values.indiamartcampaignservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting face book service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateIndiaMARTService(string user_gid, indiamartcampaignservice_list values)
        {
            try
            {
                if (values.indiamart_status == "Y")
                {
                    if (values.indiamart_id == null || values.indiamart_id == "")
                    {

                        msSQL = " insert into crm_smm_tindiamartservice(" +
                                " api_key," +
                                " created_by," +
                                " indiamart_status," +
                                " created_date)" +
                                " values(" +
                                "'" + values.api_key + "',";
                        msSQL += "'" + user_gid + "'," +
                                 "'Y'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='IndiaMART'";
                            lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                            lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (lsshopify_flag == "N")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = "IndiaMART Credentials Updated Successfully !!";
                                }
                            }
                            else if (lsshopify_flag == "")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                values.status = true;
                                values.message = "Shopify Credentials Updated Successfully!!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating IndiaMART Credentials!!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating IndiaMART Credentials !!";
                        }

                    }
                    else
                    {
                        msSQL = " update crm_smm_tindiamartservice set " +

                                " api_key = '" + values.api_key + "'," +
                                " updated_by = '" + user_gid + "'," +
                                " indiamart_status = 'Y'," +
                                " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.indiamart_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='IndiaMART'";
                            lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                            lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (lsshopify_flag == "N")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = "IndiaMART Credentials Updated Successfully !!";
                                }
                            }
                            else if (lsshopify_flag == "")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                values.status = true;
                                values.message = "Credentials Updated Successfully!!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating IndiaMART Credentials!!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating IndiaMART Credentials !!";
                        }
                    }
                }

                else
                {
                    msSQL = " update crm_smm_tindiamartservice set indiamart_status = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'N' where module_name = 'IndiaMART'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Disabled Successfully!!";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating IndiaMART service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetCalendarSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select token,s_no,active_flag from crm_smm_tcalendlyservice limit 1 ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<calendarservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new calendarservice_list
                        {
                            api_key = dt["token"].ToString(),
                            calendar_id = dt["s_no"].ToString(),
                            active_flag = dt["active_flag"].ToString(),
                        });
                        values.calendarservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Calendar service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateCalendarService(string user_gid, calendarservice_list values)
        {
            try
            {
                if (values.active_flag == "Y")
                {
                    if (values.calendar_id == null || values.calendar_id == "")
                    {


                        msSQL = " insert into crm_smm_tcalendlyservice(" +
                                " token," +
                                " created_by," +
                                " active_flag," +
                                " created_date)" +
                                " values(" +
                                "'" + values.api_key + "',";
                        msSQL += "'" + user_gid + "'," +
                                    "'Y'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Added Successfully!!";
                        }
                    }
                    else
                    {
                        msSQL = " update  crm_smm_tcalendlyservice set " +
                        " token = '" + values.api_key + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " active_flag='Y'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='" + values.calendar_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Updated Successfully!!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Calendar Credentials !!";
                        }
                    }
                }

                else
                {
                    msSQL = " update crm_smm_tcalendlyservice set active_flag = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Disabled Successfully!!";
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Calendar service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetEinvoiceSummary(MdlCampaignService values)
        {
            try
            {

                msSQL = "select  Id, einvoiceAutenticationURL, " +
                    " einvoiceIRNGenerate, gspappid, gspappsecret," +
                    " einvoiceuser_name, einvoicepwd, einvoicegstin, einvoice_Auth," +
                    " generateQRURL, cancleIRN, updated_by, created_date, updated_date," +
                    " created_by, einvoice_flag from rbl_trn_teinvoiceconfig;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<einvoice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new einvoice_list
                        {
                            id = dt["Id"].ToString(),
                            einvoiceAutenticationURL = dt["einvoiceAutenticationURL"].ToString(),
                            einvoiceIRNGenerate = dt["einvoiceIRNGenerate"].ToString(),
                            gspappid = dt["gspappid"].ToString(),
                            gspappsecret = dt["gspappsecret"].ToString(),
                            einvoiceuser_name = dt["einvoiceuser_name"].ToString(),
                            einvoicepwd = dt["einvoicepwd"].ToString(),
                            einvoicegstin = dt["einvoicegstin"].ToString(),
                            einvoice_Auth = dt["einvoice_Auth"].ToString(),
                            generateQRURL = dt["generateQRURL"].ToString(),
                            cancleIRN = dt["cancleIRN"].ToString(),
                            einvoice_flag = dt["einvoice_flag"].ToString()
                        });
                        values.einvoice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting face book service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaUpdateEinvoice(string user_gid, einvoice_list values)
        {
            try
            {
                if (values.einvoice_flag == "Y")
                {
                    if (values.einvoice_id == null || values.einvoice_id == "")
                    {


                        msSQL = " insert into rbl_trn_teinvoiceconfig(" +
                                " einvoiceAutenticationURL," +
                                " einvoiceIRNGenerate," +
                                " gspappid," +
                                " gspappsecret," +
                                " einvoiceuser_name," +
                                " einvoicepwd," +
                                " einvoicegstin," +
                                " einvoice_Auth," +
                                " generateQRURL," +
                                " cancleIRN," +
                                " created_by," +
                                " einvoice_flag," +
                                " created_date)" +
                                " values(" +
                                "'" + values.einvoiceAutenticationURL + "'," +
                                "'" + values.einvoiceIRNGenerate + "'," +
                                "'" + values.gspappid + "'," +
                                "'" + values.gspappsecret + "'," +
                                "'" + values.einvoiceuser_name + "'," +
                                "'" + values.einvoicepwd + "'," +
                                "'" + values.einvoicegstin + "'," +
                                "'" + values.einvoice_Auth + "'," +
                                "'" + values.generateQRURL + "'," +
                                "'" + values.cancleIRN + "',";
                        msSQL += "'" + user_gid + "'," +
                                    "'Y'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Added Successfully!!";
                        }
                    }
                    else
                    {
                        msSQL = " update  rbl_trn_teinvoiceconfig set " +
                        " einvoiceAutenticationURL = '" + values.einvoiceAutenticationURL + "'," +
                        " einvoiceIRNGenerate = '" + values.einvoiceIRNGenerate + "'," +
                        " gspappid = '" + values.gspappid + "'," +
                        " gspappsecret = '" + values.gspappsecret + "'," +
                        " einvoiceuser_name = '" + values.einvoiceuser_name + "'," +
                        " einvoicepwd = '" + values.einvoicepwd + "'," +
                        " einvoicegstin = '" + values.einvoicegstin + "'," +
                        " einvoice_Auth = '" + values.einvoice_Auth + "'," +
                        " generateQRURL = '" + values.generateQRURL + "'," +
                        " cancleIRN = '" + values.cancleIRN + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " einvoice_flag='Y'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Id='" + values.einvoice_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Updated Successfully!!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Einvoice Credentials !!";
                        }
                    }
                }

                else
                {
                    msSQL = " update rbl_trn_teinvoiceconfig set einvoice_flag = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Disabled Successfully!!";
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Einvoice service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetMintSoftSummary(MdlCampaignService values)
        {
            try
            {

                msSQL = " select Id,base_url,api_key,mintsoft_flag from smr_trn_tminsoftconfig";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<mintsoft_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new mintsoft_list
                        {
                            api_key = dt["api_key"].ToString(),
                            base_url = dt["base_url"].ToString(),
                            id = dt["Id"].ToString(),
                            mintsoft_flag = dt["mintsoft_flag"].ToString()
                        });
                        values.mintsoft_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting face book service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaUpdateMintSoft(string user_gid, mintsoft_list values)
        {
            try
            {
                if (values.mintsoft_flag == "Y")
                {
                    if (values.mintsoft_id == null || values.mintsoft_id == "")
                    {


                        msSQL = " insert into smr_trn_tminsoftconfig(" +
                                " base_url," +
                                " api_key," +
                                " created_by," +
                                " mintsoft_flag," +
                                " created_date)" +
                                " values(" +
                                "'" + values.base_url + "'," +
                                "'" + values.api_key + "',";
                        msSQL += "'" + user_gid + "'," +
                                    "'Y'," +
                                 "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Added Successfully!!";
                        }
                    }
                    else
                    {
                        msSQL = " update  smr_trn_tminsoftconfig set " +
                        " base_url = '" + values.base_url + "'," +
                        " api_key = '" + values.api_key + "'," +
                        " updated_by = '" + user_gid + "'," +
                        " mintsoft_flag='Y'," +
                        " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Id='" + values.mintsoft_id + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Updated Successfully!!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Mintsoft Credentials !!";
                        }
                    }
                }

                else
                {
                    msSQL = " update smr_trn_tminsoftconfig set mintsoft_flag = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Disabled Successfully!!";
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating MintSoft service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetInstaServiceSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select access_token,account_id,s_no,instagram_status from crm_smm_tinstagramservice ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<instagramcampaignservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new instagramcampaignservice_list
                        {
                            instagram_access_token = dt["access_token"].ToString(),
                            instagram_account_id = dt["account_id"].ToString(),
                            instagram_id = dt["s_no"].ToString(),
                            instagram_status = dt["instagram_status"].ToString(),
                        });
                        values.instagramcampaignservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Instagram service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostInstakeys(string user_gid, instagramcampaignservice_list values)
        {
            try
            {
                msSQL = " select account_id from crm_smm_tinstagramservice where account_id = '" + values.instagram_account_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Account Id Already Exist !!";
                }
                else
                {
                    msSQL = " insert into crm_smm_tinstagramservice(" +
                                " account_id," +
                                " instagram_status," +
                                " access_token," +
                                " created_by," +
                                " created_date)" +
                                " values(" +
                                "'" + values.instagram_account_id + "'," +
                                 "'Y'," +
                                "'" + values.instagram_access_token + "',";
                    msSQL += "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Instagram Keys Added Successfully";
                    }
                    else
                    {

                        values.status = false;
                        values.message = "Error While Adding Instagram Keys";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Instagram service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void Daupdateinstakeys(string user_gid, instagramcampaignservice_list values)
        {
            try
            {
                if (values.instagram_status == "Y")
                {
                    msSQL = "update crm_smm_tinstagramservice  set instagram_status='Y' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y' where module_name = 'Instagram'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            msSQL = "select module_gid_parent from adm_mst_tmoduleangular  where module_name ='Instagram'";
                            lsmodule_gid = objdbconn.GetExecuteScalar(msSQL);
                            msSQL = "select shopify_flag from adm_mst_tmoduleangular  where module_gid = '" + lsmodule_gid + " '";
                            lsshopify_flag = objdbconn.GetExecuteScalar(msSQL);
                            if (lsshopify_flag == "N")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='Y' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                if (mnResult != 0)
                                {
                                    values.status = true;
                                    values.message = "Instagram Credentials Updated Successfully !!";
                                }
                            }
                            else if (lsshopify_flag == "")
                            {
                                msSQL = "update adm_mst_tmoduleangular set shopify_flag='' where module_gid ='" + lsmodule_gid + " '";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                values.status = true;
                                values.message = "Updated Successfully!!";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error While Updating Instagram Credentials!!";
                            }
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Instagram Credentials !!";
                        }


                    }
                }
                else
                {
                    msSQL = " update crm_smm_tinstagramservice set instagram_status = 'N'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'N' where module_name = 'Instagram'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.status = true;
                    values.message = "Disabled Successfully!!";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Instagram service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void Dadeleteinstaaccesstoken(string instagram_account_id, instagramcampaignservice_list values)
        {

            try
            {
                msSQL = "  delete from crm_smm_tinstagramservice where account_id='" + instagram_account_id + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Key Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting Key";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Access Token!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdateoutlookService(string user_gid, outlookservice_list values)
        {
            try
            {

                msSQL = "select s_no,client_id,client_secret,tenant_id from crm_smm_outlook_service;";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == false)
                {


                    msSQL = " insert into crm_smm_outlook_service(" +
                            " client_id," +
                            " client_secret," +
                            " tenant_id," +
                            " outlook_status," +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            "'" + values.outlook_client_id + "'," +
                            " '" + values.outlook_client_secret + "'," +
                            " '" + values.tenant_id + "'," +
                            "'O',";
                    msSQL += "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmOutlookcampaignsummary' where module_name = 'Email'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmOutlookmailcompose' where module_name = 'Compose Mail'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmOutlookFolderSummary' where module_name = 'Mail Folders'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmOutlookInboxSummary' where module_name = 'Inbox'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmOutlooksentitems' where module_name = 'Sent Items'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmOutlookTrashSummary' where module_name = 'Trash'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update crm_smm_mail_service set email_status = ''";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update crm_smm_gmail_service set gmail_status = ''";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " UPDATE crm_smm_tmailconfig SET switch_flag = CASE WHEN mail_service = '" + values.mail_service + "' THEN 'Y' ELSE 'N' END ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Outlook Credentials Updated Successfully !!";

                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Outlook Credentials!!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Outlook Credentials !!";
                    }


                }
                else
                {
                    msSQL = " update  crm_smm_outlook_service set " +

                    " client_id = '" + values.outlook_client_id + "'," +
                    " client_secret = '" + values.outlook_client_secret + "'," +
                    " updated_by = '" + user_gid + "'," +
                    " tenant_id= '" + values.tenant_id + "'," +
                    " outlook_status= 'O'," +
                    " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmOutlookcampaignsummary' where module_name = 'Email'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmOutlookmailcompose' where module_name = 'Compose Mail'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmOutlookFolderSummary' where module_name = 'Mail Folders'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmOutlookInboxSummary' where module_name = 'Inbox'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmOutlooksentitems' where module_name = 'Sent Items'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update adm_mst_tmoduleangular set shopify_flag = 'Y',lw_flag = 'Y',sref='crm/CrmSmmOutlookTrashSummary' where module_name = 'Trash'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update crm_smm_mail_service set email_status = ''";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " update crm_smm_gmail_service set gmail_status = ''";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " UPDATE crm_smm_tmailconfig SET switch_flag = CASE WHEN mail_service = '" + values.mail_service + "' THEN 'Y' ELSE 'N' END ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Outlook Credentials Updated Successfully !!";

                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Outlook Credentials!!";
                        }

                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating Outlook Credentials!!";
                    }
                }



            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Outlook service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetOutlookSummary(MdlCampaignService values)
        {
            try
            {
                msSQL = " select client_id,client_secret,tenant_id,outlook_status from crm_smm_outlook_service ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<outlookcampaignservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new outlookcampaignservice_list
                        {
                            client_id = dt["client_id"].ToString(),
                            client_secret = dt["client_secret"].ToString(),
                            tenant_id = dt["tenant_id"].ToString(),
                            outlook_status = dt["outlook_status"].ToString()

                        });
                        values.outlookcampaignservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Outlook summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetMailmanagementSummary(MdlCampaignService values)
        {
            try
            {

                msSQL = " select s_no,client_id,client_secret,refresh_token,gmail_address,gmail_status from crm_smm_gmail_service";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<gmailcampaignservice_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new gmailcampaignservice_list
                        {
                            client_id = dt["client_id"].ToString(),
                            client_secret = dt["client_secret"].ToString(),
                            refresh_token = dt["refresh_token"].ToString(),
                            gmail_address = dt["gmail_address"].ToString(),
                            gmail_status = dt["gmail_status"].ToString(),
                            s_no = dt["s_no"].ToString(),

                        });
                        values.gmailcampaignservice_list = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting face book service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaPostMailmanagement(string user_gid, facebookcampaignservice_list values)
        {
            try
            {
                msSQL = " select page_id from crm_smm_tfacebookservice where page_id = '" + values.facebook_page_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Page Id Already Exist !!";
                }
                else
                {
                    msSQL = " insert into crm_smm_tfacebookservice(" +
                                " page_id," +
                                " facebook_status," +
                                " access_token," +
                                " created_by," +
                                " created_date)" +
                                " values(" +
                                "'" + values.facebook_page_id + "'," +
                                 "'Y'," +
                                "'" + values.facebook_access_token + "',";
                    msSQL += "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Facebook Keys Added Successfully";
                    }
                    else
                    {

                        values.status = false;
                        values.message = "Error While Adding Facebook Keys";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating facebook service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaPostmailmanagementkeys(string user_gid, gmailservice_lists values)
        {
            try
            {
                msSQL = " select client_id from crm_smm_gmail_service where client_id = '" + values.gclient_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "Client Id Already Exist !!";
                }
                else
                {
                    msSQL = " insert into crm_smm_gmail_service(" +
                               " client_id," +
                               " client_secret," +
                               " refresh_token," +
                               " gmail_address," +
                               " created_by," +
                               " gmail_status," +
                               " created_date)" +
                               " values(" +
                               "'" + values.gclient_id + "'," +
                               " '" + values.gclient_secret + "'," +
                               " '" + values.grefresh_token + "'," +
                               " '" + values.ggmail_address + "',";
                    msSQL += "'" + user_gid + "'," +
                         "'G'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Mail Management Keys Added Successfully";
                    }
                    else
                    {

                        values.status = false;
                        values.message = "Error While Adding Mail Management Keys";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Mail Management service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetEmployeeMailsTag(MdlCampaignService values, string emailaddress)
        {
            try
            {
                msSQL = "select concat(b.user_firstname,b.user_lastname,' / ',b.user_code) as name,b.user_gid from  hrm_mst_temployee a" +
                    " left join adm_mst_tuser b on a.user_gid=b.user_gid where a.integrated_gmail is null or integrated_gmail != '" + emailaddress + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<taglist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new taglist
                        {
                            name = dt["name"].ToString(),
                            user_gid = dt["user_gid"].ToString(),
                        });
                        values.taglist = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Mailmanagement summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetEmployeeMailsUnTag(MdlCampaignService values, string emailaddress)
        {
            try
            {
                msSQL = "select concat(b.user_firstname,b.user_lastname,' / ',b.user_code) as name,b.user_gid from  hrm_mst_temployee a" +
                    " left join adm_mst_tuser b on a.user_gid=b.user_gid where integrated_gmail = '" + emailaddress + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<untaglist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new untaglist
                        {
                            name = dt["name"].ToString(),
                            user_gid = dt["user_gid"].ToString(),
                        });
                        values.untaglist = getmodulelist;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Mailmanagement summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaTagemptoGmail(tagemployee values)
        {
            try
            {

                for (int i = 0; i < values.taglist.ToArray().Length; i++)
                {

                    msSQL = "update hrm_mst_temployee set integrated_gmail='" + values.emailaddress + "' where user_gid='" + values.taglist[i].user_gid + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 1)
                    {
                        objcmnfunctions.LogForAudit(
                            "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                            "***********" + msSQL +
                            "*******Apiref********", "SocialMedia/ErrorLog/service/" + "Log" +
                            DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Employee Tag to Gmail.";
                    }
                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Inbox Mail Tag to Employee Successfully !!";


                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred While Employee Tag to Gmail.";
                    objcmnfunctions.LogForAudit(
                       "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                       "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                       "***********" + msSQL +
                       "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                          DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error Occurred While Employee Tag to Gmail.";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaUnTagempGmail(untagemployee values)
        {
            try
            {


                for (int i = 0; i < values.untaglist.ToArray().Length; i++)
                {

                    msSQL = "update hrm_mst_temployee set integrated_gmail= '' where user_gid='" + values.untaglist[i].user_gid + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 1)
                    {
                        objcmnfunctions.LogForAudit(
                            "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                            "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                            "***********" + msSQL +
                            "*******Apiref********", "SocialMedia/ErrorLog/service/" + "Log" +
                            DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occurred While Employee Tag to Gmail.";
                    }
                }
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Inbox Mail Tag to Employee Successfully !!";


                }
                else
                {
                    values.status = false;
                    values.message = "Error Occurred While Employee Tag to Gmail.";
                    objcmnfunctions.LogForAudit(
                       "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                       "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                       "***********" + msSQL +
                       "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" +
                          DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Error Occurred While Employee Tag to Gmail.";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }

        public void Daupdatepaymentgatewayservice(string user_gid, paymentgatewayservice_list values)
        {
            try
            {
                if (values.payment_gateway == "RAZORPAY")
                {
                    msSQL = "update adm_mst_tcompany set key1='" + values.key1 + "',key2 ='" + values.key2 + "',payment_gateway='" + values.payment_gateway + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Razorpay Keys Added Successfully!!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error while updating Razorpay Keys!!";
                    }


                }
                else
                {
                    msSQL = "update adm_mst_tcompany set key1='" + values.key3 + "',key2='',payment_gateway='" + values.payment_gateway + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Stripe Keys Added Successfully!!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error while updating Stripe Keys!!";
                    }
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating Paymentgateway service!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

        }
        public void DaGetpaymentgatewaySummary(paymentgatewayservice_list values)
        {
            try
            {
                msSQL = " SELECT CASE WHEN payment_gateway = 'RAZORPAY' THEN key1 ELSE NULL END AS key1,key2,CASE WHEN payment_gateway = 'STRIPE' THEN key1 ELSE NULL END AS key3,payment_gateway FROM adm_Mst_tcompany LIMIT 1";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.key1 = objOdbcDataReader["key1"].ToString();
                    values.key2= objOdbcDataReader["key2"].ToString();
                    values.key3 = objOdbcDataReader["key3"].ToString();
                    values.payment_gateway = objOdbcDataReader["payment_gateway"].ToString();
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Paymentgateway service summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/ " + "DaGetpaymentgatewaySummary " + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void DaGetuserhomepage(string employee_gid, Mdlhomepage values)
        {
            try
            {
                msSQL = "select home_page from hrm_mst_temployee where employee_gid='" + employee_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.home_page = objOdbcDataReader["home_page"].ToString();
                   
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Company Summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/ " + "DaGetuserhomepage " + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void Daenablekotscreen(string selectedOption, result values)
        {

            try
            {
                msSQL = "update adm_Mst_tcompany set enable_kot='" + selectedOption + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Updated Successfully!!";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while updating ";
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Access Token!";
                //objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaGetkotscreensum(Mdlenablekot values)
        {
            try
            {
                msSQL = "select enable_kot from adm_mst_tcompany";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.enable_kot = objOdbcDataReader["enable_kot"].ToString();

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting Company Summary!";
                //objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH:mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "/WhatsAppOrders/ErrorLog/Summary/ " + "DaGetuserhomepage " + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
    }
}
