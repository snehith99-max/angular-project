using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;
using ems.crm.Models;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;
using System.Web.Http.Results;

namespace ems.crm.DataAccess{
    public class DaIndiaMart
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;

        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msGetGid, lssource_gid, msGetGid1, msGetGid2, msGetGid4;
        int mnResult;

        public void DaGetindiamartsummary(MdlIndiaMart values)
        {
            try
           {
                msSQL = "select UNIQUE_QUERY_ID,SENDER_NAME,SENDER_MOBILE,SENDER_ADDRESS,QUERY_PRODUCT_NAME,QUERY_TIME,SENDER_CITY,SENDER_STATE,READ_FLAG from crm_mst_tindiamartleads order by QUERY_TIME desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<indiamartsummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new indiamartsummary_list
                        {
                            sender_name = dt["SENDER_NAME"].ToString(),
                            unique_query_id = dt["UNIQUE_QUERY_ID"].ToString(),
                            sender_mobile = dt["SENDER_MOBILE"].ToString(),
                            sender_address = dt["SENDER_ADDRESS"].ToString(),
                            query_product_name = dt["QUERY_PRODUCT_NAME"].ToString(),
                            query_time = DateTime.Parse(dt["QUERY_TIME"].ToString()).ToString("dd MMM yyyy"),
                            sender_city = dt["SENDER_CITY"].ToString(),
                            sender_state = dt["SENDER_STATE"].ToString(),
                            read_flag = dt["READ_FLAG"].ToString(),
                        });
                    }
                    values.indiamartsummary_list = getModuleList;
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Getting Campaign !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetsyncdetails(MdlIndiaMart values)
        {
            try
            {
                msSQL = " select start_time as contactsync_till,last_sync_at,DATE_ADD(last_sync_at, INTERVAL 7 MINUTE) AS nextsync_at,(SELECT COUNT(DISTINCT UNIQUE_QUERY_ID) FROM crm_mst_tindiamartleads) AS unique_query_count from crm_smm_tindiamartservice";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.contactsync_till = objOdbcDataReader["contactsync_till"].ToString();
                    values.last_sync_at = objOdbcDataReader["last_sync_at"].ToString();
                    values.nextsync_at = objOdbcDataReader["nextsync_at"].ToString();
                    values.unique_query_count = objOdbcDataReader["unique_query_count"].ToString();


                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Getting Campaign !!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Whatsapp/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }

        public void DaGetindiamartviewSummary(string unique_query_id, indiamartview_list values)
        {
            try
            {
                msSQL = "select QUERY_TYPE,LEADBANK_GID,SENDER_NAME,SENDER_EMAIL,SENDER_COMPANY,SENDER_CITY,SENDER_STATE,SENDER_STATE,SENDER_ADDRESS," +
                        "SENDER_PINCODE,SENDER_COUNTRY_ISO,SENDER_MOBILE_ALT,SENDER_MOBILE,QUERY_MESSAGE,QUERY_MCAT_NAME,CALL_DURATION,QUERY_PRODUCT_NAME,RECEIVERMOBILE,READ_FLAG  from crm_mst_tindiamartleads where UNIQUE_QUERY_ID='" + unique_query_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.query_type = objOdbcDataReader["QUERY_TYPE"].ToString();
                    values.sender_name = objOdbcDataReader["SENDER_NAME"].ToString();
                    values.sender_email = objOdbcDataReader["SENDER_EMAIL"].ToString();
                    values.sender_mobile = objOdbcDataReader["SENDER_MOBILE"].ToString();
                    values.sender_company = objOdbcDataReader["SENDER_COMPANY"].ToString();
                    values.sender_city = objOdbcDataReader["SENDER_CITY"].ToString();
                    values.sender_state = objOdbcDataReader["SENDER_STATE"].ToString();
                    values.sender_pincode = objOdbcDataReader["SENDER_PINCODE"].ToString();
                    values.sender_country_iso = objOdbcDataReader["SENDER_COUNTRY_ISO"].ToString();
                    values.sender_mobile_alt = objOdbcDataReader["SENDER_MOBILE_ALT"].ToString();
                    values.query_message = objOdbcDataReader["QUERY_MESSAGE"].ToString();
                    values.query_mcat_name = objOdbcDataReader["QUERY_MCAT_NAME"].ToString();
                    values.call_duration = objOdbcDataReader["CALL_DURATION"].ToString();
                    values.query_product_name = objOdbcDataReader["QUERY_PRODUCT_NAME"].ToString();
                    values.sender_address = objOdbcDataReader["SENDER_ADDRESS"].ToString();
                    values.receiver_mobile = objOdbcDataReader["RECEIVERMOBILE"].ToString();
                    values.leadbank_gid = objOdbcDataReader["LEADBANK_GID"].ToString();
                    values.read_flag = objOdbcDataReader["READ_FLAG"].ToString();

                    if(values.read_flag == "N")
                    {
                        msSQL = "update crm_mst_tindiamartleads set READ_FLAG = 'Y' where UNIQUE_QUERY_ID = '" + unique_query_id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if(mnResult == 1)
                        {
                            values.read_flag = "Y";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting lead bank view summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            finally
            {
                if(objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }

        public void DaSyncDetails(MdlSyncDetails values)
        {
            try
            {
                msSQL = "select indiamart_status,start_time from crm_smm_tindiamartservice";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader != null)
                {
                    if(objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Read();
                        values.indiamart_status = objOdbcDataReader["indiamart_status"].ToString();
                        if(objOdbcDataReader["start_time"].ToString() != null || objOdbcDataReader["start_time"].ToString() != "")
                            values.synced_till = DateTime.Parse(objOdbcDataReader["start_time"].ToString()).ToString("dd MMM yyyy");
                        values.status = true;
                        values.message = "Details fetched successfully";
                    }
                    else
                    {
                        values.message = "Kindly add API key to proceed!";
                    }
                }
                else
                {
                    values.message = "Error occured while fetching api key details";
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit(ex.ToString(), "IndiaMart/Log_" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }

        public MdlIndiamartResponse DaLoadLeadsFromIndiaMart()
        {
            MdlIndiamartResponse objMdlIndiamartResponse = new MdlIndiamartResponse();
            try
            {
                string api_key, start_time, end_time, last_sync;
                msSQL = "select api_key,last_sync_at,start_time,end_time from crm_smm_tindiamartservice";
                objOdbcDataReader  = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    objOdbcDataReader.Read();
                    api_key = objOdbcDataReader["api_key"].ToString();
                    last_sync = objOdbcDataReader["last_sync_at"].ToString();
                    start_time = objOdbcDataReader["start_time"].ToString();
                    end_time = objOdbcDataReader["end_time"].ToString();

                    if (!string.IsNullOrEmpty(api_key))
                    {
                        if (string.IsNullOrEmpty(start_time) || string.IsNullOrEmpty(end_time))
                        {
                            msSQL = "update crm_smm_tindiamartservice set " +
                                    "start_time = '" + DateTime.Now.AddDays(-364).ToString("yyyy-MM-dd 00:00:00") + "'," +
                                    "end_time = '" + DateTime.Now.AddDays(-357).ToString("yyyy-MM-dd 00:00:00") + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                objMdlIndiamartResponse.message = "Error while updating start and end date!!";
                                return objMdlIndiamartResponse;
                            }
                            DaLoadLeadsFromIndiaMart();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(last_sync))
                            {
                                objMdlIndiamartResponse = indiamartAPI(api_key, DateTime.Parse(start_time).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Parse(end_time).ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                            else
                            {
                                DateTime last_sync_date = DateTime.Parse(last_sync);
                                DateTime threshold = last_sync_date.AddMinutes(6);

                                if (DateTime.Now >= threshold)
                                {
                                    objMdlIndiamartResponse = indiamartAPI(api_key, DateTime.Parse(start_time).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Parse(end_time).ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                else
                                {
                                    objMdlIndiamartResponse.message = "STOP";
                                }
                            }
                        }
                    }
                }
                else
                {
                    objMdlIndiamartResponse.message = "No key found!!";
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit(ex.ToString(), "IndiaMart/Log_" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if(objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
            return objMdlIndiamartResponse;
        }

        public MdlIndiamartResponse indiamartAPI(string api_key, string start_time, string end_time)
        {
            MdlIndiamartResponse objresult = new MdlIndiamartResponse();
            try
            {
                msSQL = "update crm_smm_tindiamartservice set last_sync_at = '" + DateTime.Now.AddMilliseconds(-3000).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (DateTime.Parse(start_time) > DateTime.Now)
                {
                    updatedate("S", end_time);
                    msSQL = "select start_time,end_time from crm_smm_tindiamartservice";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader.HasRows)
                    {
                        objOdbcDataReader.Read();
                        start_time = DateTime.Parse(objOdbcDataReader["start_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        end_time = DateTime.Parse(objOdbcDataReader["end_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                msSQL = "select history_flag from crm_smm_tindiamartservice";
                string history_flag = objdbconn.GetExecuteScalar(msSQL);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                MdlIndiaMartLeads objMdlIndiaMartLeads = new MdlIndiaMartLeads();
                var client = new RestClient(ConfigurationManager.AppSettings["indiamartBaseURL"].ToString());
                var request = new RestRequest("/wservce/crm/crmListing/v2/?glusr_crm_key=" + api_key + "&start_time=" + start_time + "&end_time=" + end_time, Method.GET);
                IRestResponse response = client.Execute(request);
                objMdlIndiaMartLeads = JsonConvert.DeserializeObject<MdlIndiaMartLeads>(response.Content);
                if (objMdlIndiaMartLeads.CODE == 200)
                {
                    foreach (var item in objMdlIndiaMartLeads.RESPONSE)
                    {
                        msSQL = "select UNIQUE_QUERY_ID from crm_mst_tindiamartleads where UNIQUE_QUERY_ID = '" + item.UNIQUE_QUERY_ID + "'";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (!objOdbcDataReader.HasRows)
                        {
                            msSQL = "insert into crm_mst_tindiamartleads(" +
                                    "UNIQUE_QUERY_ID," +
                                    "QUERY_TYPE," +
                                    "QUERY_TIME," +
                                    "SENDER_NAME," +
                                    "SENDER_MOBILE," +
                                    "SENDER_EMAIL," +
                                    "SENDER_COMPANY," +
                                    "SENDER_ADDRESS," +
                                    "SENDER_CITY," +
                                    "SENDER_STATE," +
                                    "SENDER_PINCODE," +
                                    "SENDER_COUNTRY_ISO," +
                                    "SENDER_MOBILE_ALT," +
                                    "QUERY_PRODUCT_NAME," +
                                    "QUERY_MESSAGE," +
                                    "QUERY_MCAT_NAME," +
                                    "CALL_DURATION," +
                                    "RECEIVERMOBILE)values(" +
                                    "'" + item.UNIQUE_QUERY_ID + "'," +
                                    "'" + item.QUERY_TYPE + "'," +
                                    "'" + item.QUERY_TIME + "'," +
                                    "'" + item.SENDER_NAME.Replace("\'", "\\\'") + "'," +
                                    "'" + item.SENDER_MOBILE + "'," +
                                    "'" + item.SENDER_EMAIL + "'," +
                                    "'" + item.SENDER_COMPANY.Replace("\'", "\\\'") + "'," +
                                    "'" + item.SENDER_ADDRESS.Replace("\'", "\\\'") + "'," +
                                    "'" + item.SENDER_CITY + "'," +
                                    "'" + item.SENDER_STATE + "'," +
                                    "'" + item.SENDER_PINCODE + "'," +
                                    "'" + item.SENDER_COUNTRY_ISO + "'," +
                                    "'" + item.SENDER_MOBILE_ALT + "'," +
                                    "'" + item.QUERY_PRODUCT_NAME.Replace("\'", "\\\'") + "'," +
                                    "'" + item.QUERY_MESSAGE.Replace("\'", "\\\'") + "'," +
                                    "'" + item.QUERY_MCAT_NAME.Replace("\'", "\\\'") + "'," +
                                    "'" + item.CALL_DURATION + "'," +
                                    "'" + item.RECEIVER_MOBILE + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                objcmnfunctions.LogForAudit("Update failed: " + msSQL, "IndiaMart/Log_" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            }
                        }
                        objOdbcDataReader.Close();
                    }
                    updatedate(history_flag, end_time);
                    objresult.status = true;
                    objresult.code = objMdlIndiaMartLeads.CODE;
                    objresult.message = "Success";
                }
                else if (objMdlIndiaMartLeads.CODE == 204)
                {
                    if (objMdlIndiaMartLeads.MESSAGE == "There are no leads in the given time duration. Please try for a different duration.")
                    {
                        updatedate(history_flag, end_time);
                        objresult.status = true;
                        objresult.code = objMdlIndiaMartLeads.CODE;
                        objresult.message = objMdlIndiaMartLeads.MESSAGE;

                    }
                    else
                    {
                        objresult.status = true;
                        string[] arr = objMdlIndiaMartLeads.MESSAGE.Split(' ');
                        objMdlIndiaMartLeads.MESSAGE = arr[2] + " " + arr[3];
                        objMdlIndiaMartLeads.MESSAGE = DateTime.Parse(objMdlIndiaMartLeads.MESSAGE).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
                        objresult.message = "Fetch details from Indiamart will start after " + objMdlIndiaMartLeads.MESSAGE;
                        msSQL = "update crm_smm_tindiamartservice set last_sync_at = '" + objMdlIndiaMartLeads.MESSAGE + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 0)
                        {
                            objcmnfunctions.LogForAudit("Update failed: " + msSQL, "IndiaMart/Log_" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                        }
                        objresult.code = objMdlIndiaMartLeads.CODE;
                        objresult.auth_code = 1;     
                    }
                }
                else if (objMdlIndiaMartLeads.CODE == 401)
                {
                    objresult.code = objMdlIndiaMartLeads.CODE;
                    objresult.message = objMdlIndiaMartLeads.MESSAGE;
                }
                else if (objMdlIndiaMartLeads.CODE == 429 && objMdlIndiaMartLeads.APP_AUTH_FAILURE_CODE == 429)
                {
                    msSQL = "update crm_smm_tindiamartservice set last_sync_at = '" + DateTime.Now.AddMinutes(15).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        objcmnfunctions.LogForAudit("Update failed: " + msSQL, "IndiaMart/Log_" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                    objresult.code = objMdlIndiaMartLeads.CODE;
                    objresult.auth_code = objMdlIndiaMartLeads.APP_AUTH_FAILURE_CODE;
                    objresult.message = "Please try again after 15 minutes!";
                }
                else if (objMdlIndiaMartLeads.CODE == 429 && objMdlIndiaMartLeads.APP_AUTH_FAILURE_CODE == 0)
                {
                    msSQL = "update crm_smm_tindiamartservice set last_sync_at = '" + DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        objcmnfunctions.LogForAudit("Update failed: " + msSQL, "IndiaMart/Log_" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                    }
                    objresult.code = objMdlIndiaMartLeads.CODE;
                    objresult.message = "Please try again after 5 minutes!";
                }
                else if (objMdlIndiaMartLeads.CODE == 400)
                { 
                    objresult.code = objMdlIndiaMartLeads.CODE;
                    objresult.message = "Start date or end date incorrect in the request";
                }
                else
                {
                    objresult.code = objMdlIndiaMartLeads.CODE;
                    objresult.message = objMdlIndiaMartLeads.MESSAGE;
                    objcmnfunctions.LogForAudit(objMdlIndiaMartLeads.MESSAGE, "IndiaMart/Log_" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                }
            }
            catch (Exception e)
            {
                objresult.message = "Exception occured!!";
                objcmnfunctions.LogForAudit(e.ToString(), "IndiaMart/Log_" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
            return objresult;
        }

        public void updatedate(string history_flag, string end_time)
        {
            if (history_flag == "Y")
            {
                msSQL = "update crm_smm_tindiamartservice set " +
                        "start_time = '" + end_time + "'," +
                        "end_time = '" + DateTime.Parse(end_time).AddDays(7).ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            else if (history_flag == "S")
            {
                msSQL = "update crm_smm_tindiamartservice set " +
                        "start_time = '" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "'," +
                        "end_time = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "history_flag = 'N'";
            }
            else
            {
                msSQL = "update crm_smm_tindiamartservice set " +
                        "start_time = '" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00") + "'," +
                        "end_time = '" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00") + "'";
            }
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult == 0)
            {
                objcmnfunctions.LogForAudit("Update failed: " + msSQL, "IndiaMart/Log_" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaPostAddtoLeadBank(mdlAddasleadtolead values, string user_gid)
        {
            try
            {
                msSQL = " select QUERY_TYPE,SENDER_NAME,SENDER_EMAIL,SENDER_COMPANY,SENDER_CITY,SENDER_STATE,SENDER_STATE,SENDER_ADDRESS," +
                   "SENDER_PINCODE,SENDER_COUNTRY_ISO,SENDER_MOBILE_ALT,SENDER_MOBILE,QUERY_MESSAGE,QUERY_MCAT_NAME,CALL_DURATION,QUERY_PRODUCT_NAME,RECEIVERMOBILE  from crm_mst_tindiamartleads where UNIQUE_QUERY_ID='" + values.unique_query_id + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.query_type = objOdbcDataReader["QUERY_TYPE"].ToString();
                    values.sender_name = objOdbcDataReader["SENDER_NAME"].ToString();
                    values.sender_email = objOdbcDataReader["SENDER_EMAIL"].ToString();
                    values.sender_mobile = objOdbcDataReader["SENDER_MOBILE"].ToString();
                    values.sender_company = objOdbcDataReader["SENDER_COMPANY"].ToString();
                    values.sender_city = objOdbcDataReader["SENDER_CITY"].ToString();
                    values.sender_state = objOdbcDataReader["SENDER_STATE"].ToString();
                    values.sender_pincode = objOdbcDataReader["SENDER_PINCODE"].ToString();
                    values.sender_country_iso = objOdbcDataReader["SENDER_COUNTRY_ISO"].ToString();
                    values.sender_mobile_alt = objOdbcDataReader["SENDER_MOBILE_ALT"].ToString();
                    values.query_message = objOdbcDataReader["QUERY_MESSAGE"].ToString();
                    values.query_mcat_name = objOdbcDataReader["QUERY_MCAT_NAME"].ToString();
                    values.call_duration = objOdbcDataReader["CALL_DURATION"].ToString();
                    values.query_product_name = objOdbcDataReader["QUERY_PRODUCT_NAME"].ToString();
                    values.sender_address = objOdbcDataReader["SENDER_ADDRESS"].ToString();
                    values.receiver_mobile = objOdbcDataReader["RECEIVERMOBILE"].ToString();

                    msSQL = "select source_gid from crm_mst_tsource where source_name = 'Indiamart'";
                    string source_gid = objdbconn.GetExecuteScalar(msSQL);

                    if (string.IsNullOrEmpty(source_gid))
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("BSEM");
                        msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='BSEM' order by finyear desc limit 0,1 ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);
                        string lssource_code = "SCM" + "000" + lsCode;

                        msSQL = " insert into crm_mst_tsource(" +
                                " source_gid," +
                                " source_code," +
                                " source_name," +
                                " created_by, " +
                                " created_date)" +
                                " values(" +
                                " '" + msGetGid + "'," +
                                " '" + lssource_code + "'," +
                                "'Indiamart'," +
                                "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            lssource_gid = msGetGid;
                        }
                    }
                    else
                    {
                        lssource_gid = source_gid;
                    }

                    msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + user_gid + "'";
                    string lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select customer_type from crm_mst_tcustomertype Where customertype_gid='" + values.customer_type + "'";
                    string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select leadbank_gid from crm_trn_tleadbank Where leadbank_name ='" + values.sender_company + "'";
                    string leadbank_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select leadbank_gid from crm_trn_tleadbank Where leadbank_name ='" + values.sender_name + "'";
                    string leadbank_gid1 = objdbconn.GetExecuteScalar(msSQL);

                    if(leadbank_gid == null || leadbank_gid == "" && leadbank_gid1 == null || leadbank_gid1 == "") 
                    { 

                    msGetGid = objcmnfunctions.GetMasterGID("BMCC");
                    msGetGid1 = objcmnfunctions.GetMasterGID("BLBP");

                    msSQL = " INSERT INTO crm_trn_tleadbank(" +
                            " leadbank_gid," +
                            " source_gid," +
                            " leadbank_id," +
                            " leadbank_name," +
                            " status," +
                            " approval_flag, " +
                            " lead_status," +
                            " leadbank_code," +
                            " customer_type," +
                            " customertype_gid," +
                            " created_by," +
                            " main_branch," +
                            " created_date)" +
                            " values(" +
                            " '" + msGetGid1 + "'," +
                            " '" + lssource_gid + "'," +
                            " '" + msGetGid + "',";
                    if (values.sender_company == null || values.sender_company == "")
                    {
                        msSQL += "'" + values.sender_name + "',"; ;
                    }
                    else
                    {
                        msSQL += "'" + values.sender_company + "',";
                    }

                    msSQL += " 'y'," +
                              " 'Approved'," +
                              " 'Not Assigned'," +
                              " 'H.Q'," +
                              " '" + lscustomer_type + "'," +
                              " '" + values.customer_type + "'," +
                                " '" + lsemployee_gid + "'," +
                              " 'Y'," +
                              " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");

                    msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                        " (leadbankcontact_gid," +
                        " leadbank_gid," +
                        " leadbankcontact_name," +
                        " mobile," +
                        " email," +
                        " created_date," +
                        " created_by," +
                         " address1," +
                            " city," +
                            " state," +
                            " pincode," +
                        " leadbankbranch_name, " +
                        " main_contact)" +
                        " values( " +
                        " '" + msGetGid2 + "'," +
                        " '" + msGetGid1 + "'," +
                        " '" + values.sender_name + "'," +
                        " '" + values.sender_mobile.Replace("-", "") + "'," +
                         " '" + values.sender_email + "'," +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                        " '" + lsemployee_gid + "'," +
                         " '" + values.sender_address + "'," +
                            " '" + values.sender_city + "'," +
                            " '" + values.sender_state + "'," +
                            " '" + values.sender_pincode + "'," +
                        " 'H.Q'," +
                        " 'Y'" + ")";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    msSQL = "update crm_trn_tleadbank set UNIQUE_QUERY_ID ='" + values.unique_query_id + "' where leadbank_gid ='" + msGetGid1 + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "update crm_mst_tindiamartleads set leadbank_gid ='" + msGetGid1 + "' where UNIQUE_QUERY_ID ='" + values.unique_query_id + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msGetGid4 = objcmnfunctions.GetMasterGID("APMT");

                        msSQL = " insert into crm_trn_tappointment (" +
                                    " appointment_gid," +
                                    " lead_title, " +
                                    " leadbank_gid, " +
                                    " business_vertical, " +
                                    " appointment_date, " +
                                    " created_by," +
                                    "created_date" +
                                     ") values (" +
                                    "'" + msGetGid4 + "', " +
                                    "'" + values.lead_title + "'," +
                                    "'" + msGetGid1 + "'," +
                                    "'" + values.bussiness_verticle + "'," +
                                    "'" + values.appointment_timing + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult == 1)
                        {
                            values.status = true;
                            values.message = "Added to Opportunity Successfully !!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While adding Opportunity !!";
                        }
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While adding Opportunity !!";
                    }

                }
                    else
                    {
                        values.status = false;
                        values.message = "Error While adding Opportunity !!";
                    }
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Contact!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaMarkAsUnread(string id, result values)
        {
            msSQL = "update crm_mst_tindiamartleads set READ_FLAG = 'N' where UNIQUE_QUERY_ID = '" + id + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if(mnResult == 1)
            {
                values.status = true;
                values.message = "Marked as unread";
            }
            else
            {
                values.message = "Error occured while update.";
            }
        }
    }
}   