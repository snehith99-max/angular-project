using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;
using ems.crm.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Configuration;
using System.Text.RegularExpressions;

namespace ems.crm.DataAccess
{
    public class Daclicktocall
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid, msGetGid2, lssource_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, total_count;

        public void DaCallSummary(Mdlclicktocall values,string user_gid)
        {
            try
            {
                msSQL = "SELECT (SELECT COUNT(call_status) FROM crm_smm_tclicktocall WHERE call_status = 'CALLER' AND MONTH(start_time) = MONTH(CURDATE())) AS answered," +
                        "    (SELECT COUNT(call_status) FROM crm_smm_tclicktocall WHERE call_status = 'AGENT MISSED' AND MONTH(start_time) = MONTH(CURDATE())) AS agent_missed," +
                        "    (SELECT COUNT(call_status) FROM crm_smm_tclicktocall WHERE MONTH(start_time) = MONTH(CURDATE())) AS total_count," +
                         "    (SELECT COUNT(call_status) FROM crm_smm_tclicktocall WHERE call_status = 'CUSTOMER MISSED' AND MONTH(start_time) = MONTH(CURDATE())) AS customer_missed; ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getLeadBankCountList = new List<agent_barchartreport>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        int total_count = Convert.ToInt32(dt["total_count"]); // Assigning total_count value
                        if (total_count != 0)
                        {
                            getLeadBankCountList.Add(new agent_barchartreport
                            {
                                agent_missed = dt["agent_missed"].ToString(),
                                answered = dt["answered"].ToString(),
                                customer_missed = dt["customer_missed"].ToString(),
                                total_count = total_count.ToString(),
                                status = true
                            });
                            values.agent_barchartreport = getLeadBankCountList;
                        }
                    }
                    dt_datatable.Dispose();
                }

                msSQL = "Select a.individual_gid,a.uniqueid,a.user_name,a.station,a.phone_number,a.didnumber,a.status,a.direction,a.duration,a.start_time,a.answertime,a.endtime,a.recording_path,a.call_status,b.agent_name as agent,a.notes,a.lead_flag from crm_smm_tclicktocall a" +
                    " left join crm_smm_tclicktocallagents b on b.agent_mailid=a.agent where user_gid ='"+ user_gid + "'  order by a.start_time desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<calllog_report>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string callStatus = dt["call_status"].ToString();
                        string status = callStatus.Equals("CALLER", StringComparison.OrdinalIgnoreCase) ? "ANSWERED" : callStatus;
                        string phoneNumber = dt["phone_number"].ToString();
                        string userName = dt["user_name"].ToString();
                        if (string.IsNullOrEmpty(userName))
                        {
                            userName = "Visitor_";
                            if (phoneNumber.Length >= 4)
                            {
                                userName += phoneNumber.Substring(phoneNumber.Length - 4);
                            }
                            else
                            {
                                userName += phoneNumber;
                            }
                        }

                        getmodulelist.Add(new calllog_report
                        {
                            individual_gid = dt["individual_gid"].ToString(),
                            uniqueid = dt["uniqueid"].ToString(),
                            user_name = userName,
                            station = dt["station"].ToString(),
                            phone_number = dt["phone_number"].ToString(),
                            didnumber = dt["didnumber"].ToString(),
                            status = dt["status"].ToString(),
                            direction = dt["direction"].ToString(),
                            duration = dt["duration"].ToString(),   
                            start_time = dt["start_time"].ToString(),
                            answertime = dt["answertime"].ToString(),
                            endtime = dt["endtime"].ToString(),
                            recording_path = dt["recording_path"].ToString(),
                            call_status = status,
                            agent = dt["agent"].ToString(),
                            remarks = dt["notes"].ToString(),
                            lead_flag = dt["lead_flag"].ToString(),
                        });
                    }
                    values.calllog_report = getmodulelist;
                }

                if (dt_datatable != null)
                {
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Vieing Call Log Report";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Clicktocall/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaallCallSummary(Mdlclicktocall values)
        {
            try
            {
                msSQL = "select (select ifnull(count(call_status),0) from crm_smm_tclicktocall call_status where call_status='CALLER') as answered,(select ifnull(count(call_status),0)  " +
                        "from crm_smm_tclicktocall where call_status='AGENT MISSED') as agent_missed,(select ifnull(count(call_status),0)from crm_smm_tclicktocall ) as total_count,(select ifnull(count(call_status),0) from crm_smm_tclicktocall where call_status='CUSTOMER MISSED') as customer_missed ; ; ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    values.answered = objOdbcDataReader["answered"].ToString();
                    values.agent_missed = objOdbcDataReader["agent_missed"].ToString();
                    values.total_count = objOdbcDataReader["total_count"].ToString();
                    values.customer_missed = objOdbcDataReader["customer_missed"].ToString();

                }

                msSQL = "Select a.individual_gid,a.uniqueid,a.user_name,a.station,a.phone_number,a.didnumber,a.status,a.direction,a.duration,a.start_time,a.answertime,a.endtime,a.recording_path,a.call_status,b.agent_name as agent,a.notes,a.lead_flag from crm_smm_tclicktocall a" +
                    " left join crm_smm_tclicktocallagents b on b.agent_mailid=a.agent order by a.start_time desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<calllog_report>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string callStatus = dt["call_status"].ToString();
                        string status = callStatus.Equals("CALLER", StringComparison.OrdinalIgnoreCase) ? "ANSWERED" : callStatus;
                        string phoneNumber = dt["phone_number"].ToString();
                        string userName = dt["user_name"].ToString();
                        if (string.IsNullOrEmpty(userName))
                        {
                            userName = "Visitor_";
                            if (phoneNumber.Length >= 4)
                            {
                                userName += phoneNumber.Substring(phoneNumber.Length - 4);
                            }
                            else
                            {
                                userName += phoneNumber;
                            }
                        }


                        getmodulelist.Add(new calllog_report
                        {
                            individual_gid = dt["individual_gid"].ToString(),
                            uniqueid = dt["uniqueid"].ToString(),
                            user_name = userName,
                            station = dt["station"].ToString(),
                            phone_number = dt["phone_number"].ToString(),
                            didnumber = dt["didnumber"].ToString(),
                            status = dt["status"].ToString(),
                            direction = dt["direction"].ToString(),
                            duration = dt["duration"].ToString(),
                            start_time = dt["start_time"].ToString(),
                            answertime = dt["answertime"].ToString(),
                            endtime = dt["endtime"].ToString(),
                            recording_path = dt["recording_path"].ToString(),
                            call_status = status,
                            agent = dt["agent"].ToString(),
                            remarks = dt["notes"].ToString(),
                            lead_flag = dt["lead_flag"].ToString(),

                        });
                    }
                    values.calllog_report = getmodulelist;
                }

                if (dt_datatable != null)
                {
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Vieing Call Log Report";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Clicktocall/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public void Daaudioplay(string uniqueid, calllog_report values)

        {
            try
            {

                msSQL = "Select uniqueid,recording_path from crm_smm_tclicktocall where uniqueid= '" + uniqueid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if(objOdbcDataReader.HasRows == true)
                {
                   values.recording_path = objOdbcDataReader["recording_path"].ToString();
                   values.uniqueid = objOdbcDataReader["uniqueid"].ToString();

                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Fecthing Audio";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Clicktocall/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public result Dacustomercall(calling values, string user_gid)
        {
            result objresult = new result();
            try
                {
                clicktocallconfiguration getclicktocallcredentials = clicktocallcredentials();
                string msSQL = "select agent_gid,agent_name,agent_mailid,agent_number from crm_smm_tclicktocallagents where in_callstatus = 'N' and user_gid ='" + user_gid + "'";
                    OdbcDataReader objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader != null)
                    {
                        if (objOdbcDataReader.HasRows)
                        {
                        objOdbcDataReader.Read();
                        string lsmobile = values.phone_number;
                        if (!string.IsNullOrEmpty(lsmobile))
                        {
                            lsmobile = Regex.Replace(lsmobile, "[^0-9]", "");
                            if (lsmobile.Length > 10)
                            {
                                lsmobile = lsmobile.Substring(lsmobile.Length - 10);
                            }
                        }
                        string contactjson = "{\"station\":\"" + objOdbcDataReader["agent_number"].ToString() + "\",\"phone_number\":\"" + lsmobile+ "\",\"cli_number\":\"8633537772\",\"agent\" : {\"identity\":\"user\",\"value\":\"" + objOdbcDataReader["agent_mailid"].ToString() + "\"},\"custdata\":{}}";

                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient("" + getclicktocallcredentials.base_url+"");
                            var request = new RestRequest("/api/v2/call", Method.POST);
                            request.AddHeader("Authorization", "" + getclicktocallcredentials.access_token + "");
                            request.AddParameter("application/json", contactjson, ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.OK)
                        { 
                                click2callresponse objclick2callresponse = new click2callresponse();
                                objclick2callresponse = JsonConvert.DeserializeObject<click2callresponse>(response.Content);

                                msSQL = "update crm_smm_tclicktocallagents set uniqueid ='" + objclick2callresponse.uid + "', in_callstatus = 'Y' where agent_gid='" + objOdbcDataReader["agent_gid"].ToString() + "'";
                                int mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                objresult.message = "Success";
                                objresult.status = true;
                            }
                            else
                            {
                                click2callerrorresponse objclick2callresponse = new click2callerrorresponse();
                                objclick2callresponse = JsonConvert.DeserializeObject<click2callerrorresponse>(response.Content);
                                objresult.message = objclick2callresponse.error + " - " + objclick2callresponse.message;
                            }
                        }
                        else
                        {
                            objresult.message = "Cannot Place call since you are in call already!";
                        }
                    }
                    else
                    {
                        objresult.message = "Error Occured!";
                    }

                }
                catch (Exception ex)
                {
                    objresult.message = "Exception occured! Please try again after sometime!!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Clicktocall/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
            return objresult;
           
        }

        public void DaGetlogreport(Mdlclicktocall values,string phone_number)
        {
          
            try
            {

                msSQL = " select user_name,phone_number from  crm_smm_tclicktocall where phone_number= '"+ phone_number + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader != null && objOdbcDataReader.HasRows)
                {
                    values.user_name = objOdbcDataReader["user_name"].ToString();
                    values.phone_number = objOdbcDataReader["phone_number"].ToString();

                }

                msSQL = "Select a.station,a.status,a.direction,a.duration,a.start_time,a.call_status,b.agent_name as agent from crm_smm_tclicktocall a" +
                   " left join crm_smm_tclicktocallagents b on b.agent_mailid=a.agent  where phone_number= '" + phone_number + "'  order by a.start_time desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<calllog_report>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        DateTime startTime = (DateTime)dt["start_time"];
                        string startDateString = startTime.ToString("yyyy-MM-dd");
                        string callStatus = dt["call_status"].ToString();
                        string status = callStatus.Equals("CALLER", StringComparison.OrdinalIgnoreCase) ? "ANSWERED" :
                        callStatus.Equals("AGENT", StringComparison.OrdinalIgnoreCase) ? "ANSWERED" : callStatus; getmodulelist.Add(new calllog_report
                        {
                            station = dt["station"].ToString(),
                            status = dt["status"].ToString(),
                            direction = dt["direction"].ToString(),
                            duration = dt["duration"].ToString(),
                            start_time = startDateString,
                            call_status = status,
                            agent = dt["agent"].ToString(),
                        });
                    }
                    values.calllog_report = getmodulelist;
                }

                if (dt_datatable != null)
                {
                    dt_datatable.Dispose();
                }
            }
            catch (Exception ex)
            {
                values.message = "Error While Vieing Call Log Report";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Clicktocall/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }
        public result DaPostaddleads(addleadvalue values, string user_gid)
        {
            result objresult = new result();
            try
            {
                msSQL = " select mobile from crm_trn_tleadbankcontact WHERE mobile LIKE '%" + values.phone_number + "%' and main_contact ='Y' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);


                if (objOdbcDataReader.HasRows == true)
                {
                    objresult.status = false;
                    objresult.message = "Already Lead Added";
                }
                else
                {

                    msSQL = "select source_gid from crm_mst_tsource where source_name = 'Click to Call'";
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
                                "' Click to Call '," +
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

                    msSQL = " select customer_type from crm_mst_tcustomertype Where customertype_gid='" + values.customertype_edit + "'";
                    string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);

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
                            " '" + msGetGid + "'," +
                            " '" + values.user_name + "'," +
                            " 'y'," +
                            " 'Approved'," +
                            " 'Not Assigned'," +
                            " 'H.Q'," +
                            " '" + lscustomer_type + "'," +
                            " '" + values.customertype_edit + "'," +
                            " '" + lsemployee_gid + "'," +
                            " 'Y'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                    if (msGetGid2 == "E")
                    {
                        objresult.status = false;
                        objresult.message = "Create sequence code BLCC for Lead Bank";
                    }
                    else
                    {
                        msSQL = " INSERT INTO crm_trn_tleadbankcontact" +
                            " (leadbankcontact_gid," +
                            " leadbank_gid," +
                            " leadbankcontact_name," +
                            " mobile," +
                            " created_date," +
                            " created_by," +
                            " leadbankbranch_name, " +
                            " main_contact)" +
                            " values( " +
                            " '" + msGetGid2 + "'," +
                            " '" + msGetGid1 + "'," +
                            " '" + values.user_name + "'," +
                            " '" + values.phone_number + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                            " '" + lsemployee_gid + "'," +
                            " 'H.Q'," +
                            " 'y'" + ")";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        if (mnResult == 1)
                        {

                            msSQL = "update crm_smm_tclicktocall set leadbank_gid='" + msGetGid1 + "',leadbankcontact_gid='" + msGetGid2 + "',customer_type='" + lscustomer_type + "' where phone_number='" + values.phone_number + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 1)
                            {
                                objresult.status = true;
                                objresult.message = "Lead Added successfully!";
                            }
                            else
                            {
                                objresult.message = "Error occured while adding Lead!";
                            }
                        }
                        else
                        {
                            objresult.message = "Error occured while adding Lead!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objresult.message = "Error occured while posting contact At Click To Call!!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Clicktocall/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
            return objresult;
        }

        public result DaUpdatedRemarks(calling values)
        {
            result objresult = new result();
            try
            {
                msSQL = " update  crm_smm_tclicktocall  set " +
              " notes = '" + values.remarks + "' " +
              "  where individual_gid='" + values.individual_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    objresult.status = true;
                    objresult.message = "Updated Successfully";
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Error While Updating";
                }

                msSQL = " update  crm_smm_tclicktocall  set user_name = '" + values.user_name + "' where phone_number='" + values.phone_number + "'  ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    objresult.status = true;
                    objresult.message = "Updated Successfully";
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Error While Updating";
                }
            }
            
            catch (Exception ex)
            {
                objresult.message = "Error While Upadting";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + objresult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Clicktocall/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            return objresult;

        }
        public void DaGetagentSummary(Mdlclicktocall values)
        {
            try
            {
                msSQL = "SELECT b.agent_gid,b.agent_name,b.agent_number,b.agent_mailid,IFNULL(SUM(CASE WHEN a.call_status = 'CALLER' THEN 1 ELSE 0 END), 0) AS answered, " +
                    " IFNULL(SUM(CASE WHEN a.call_status = 'AGENT MISSED' THEN 1 ELSE 0 END), 0) AS agent_missed," +
                    " IFNULL(SUM(CASE WHEN a.call_status = 'CUSTOMER MISSED' THEN 1 ELSE 0 END), 0) AS customer_missed,COUNT(a.call_status) AS total_count  FROM crm_smm_tclicktocallagents b" +
                    " LEFT JOIN crm_smm_tclicktocall a ON b.agent_mailid = a.agent GROUP BY b.agent_name order by total_count desc  ; ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getmodulelist = new List<agent_report>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getmodulelist.Add(new agent_report
                        {
                            agent_name = dt["agent_name"].ToString(),
                            agent_gid = dt["agent_gid"].ToString(),
                            agent_number = dt["agent_number"].ToString(),
                            agent_mailid = dt["agent_mailid"].ToString(),
                            answered = dt["answered"].ToString(),
                            agent_missed = dt["agent_missed"].ToString(),
                            total_count = dt["total_count"].ToString(),
                            customer_missed = dt["customer_missed"].ToString(),
                        });
                    }
                }
                values.agent_report = getmodulelist;
            }
            catch (Exception ex)
            {
                values.message = "Error While Getting Agent Details";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Clicktocall/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
        }
        public void DaGetdaywisechart(Mdlclicktocall values)
        {
            try
            {
                string msSQL = "SELECT DATE_FORMAT(start_time, '%b-%d') AS daily_date,SUM(CASE WHEN direction = 'INBOUND' THEN 1 ELSE 0 END) AS inbound_users,SUM(CASE WHEN direction = 'OUTBOUND' THEN 1 ELSE 0 END) AS outbound_users " +
                               "FROM crm_smm_tclicktocall WHERE MONTH(start_time) = MONTH(CURDATE()) GROUP BY daily_date ORDER BY daily_date;";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdatemodulelist = new List<dateanalytics_report>();
                var getmodulelist = new List<inboundanalytics_report>();
                var getoutboundmodulelist = new List<outboundanalytics_report>();

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getdatemodulelist.Add(new dateanalytics_report
                    {
                        daily_date = dt["daily_date"].ToString(),
                    });

                    getmodulelist.Add(new inboundanalytics_report
                    {
                        daily_users = dt["inbound_users"].ToString(),
                    });

                    getoutboundmodulelist.Add(new outboundanalytics_report
                    {
                        daily_users = dt["outbound_users"].ToString(),
                    });
                }

                values.dateanalytics_report = getdatemodulelist;
                values.inboundanalytics_report = getmodulelist;
                values.outboundanalytics_report = getoutboundmodulelist;

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception While Getting Daywise Chart";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Clicktocall/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaGetweekwiseclicktocallchart(Mdlclicktocall values)
        {
           try
            {
                string msSQL = "SELECT DATE_FORMAT(start_time, '%b-%d') AS week_date,SUM(CASE WHEN direction = 'INBOUND' THEN 1 ELSE 0 END) AS inbound_users,SUM(CASE WHEN direction = 'OUTBOUND' THEN 1 ELSE 0 END) AS outbound_users " +
                               "  FROM crm_smm_tclicktocall WHERE start_time >= CURDATE() - INTERVAL 7 DAY GROUP BY week_date ORDER BY week_date;";

                DataTable dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdatemodulelist = new List<dateanalytics_report>();
                var getmodulelist = new List<inboundanalytics_report>();
                var getoutboundmodulelist = new List<outboundanalytics_report>();

                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getdatemodulelist.Add(new dateanalytics_report
                    {
                        week_date = dt["week_date"].ToString(),
                    });

                    getmodulelist.Add(new inboundanalytics_report
                    {
                        weekly_user = dt["inbound_users"].ToString(),
                    });

                    getoutboundmodulelist.Add(new outboundanalytics_report
                    {
                        weekly_user = dt["outbound_users"].ToString(),
                    });
                }

                values.dateanalytics_report = getdatemodulelist;
                values.inboundanalytics_report = getmodulelist;
                values.outboundanalytics_report = getoutboundmodulelist;

                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception While Getting Daywise Chart";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Clicktocall/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
            }
        public clicktocallconfiguration clicktocallcredentials()
        {

            clicktocallconfiguration geclicktocallcredentials = new clicktocallconfiguration();
            try
            {

                msSQL = " select base_url,access_token from crm_smm_tclicktocallservice";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader != null && objOdbcDataReader.HasRows)
                {
                    geclicktocallcredentials.base_url = objOdbcDataReader["base_url"].ToString();
                    geclicktocallcredentials.access_token = objOdbcDataReader["access_token"].ToString();

                }
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Clicktocall/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
            return geclicktocallcredentials;
        }

        public void Daupdatelead(Mdlclicktocall values)
        {
            try
            {

                msSQL = "select distinct phone_number from crm_smm_tclicktocall";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<leadupdate>();
                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new leadupdate
                        {
                            phone_number = dt["phone_number"].ToString()
                        });
                        values.leadupdate = getModuleList;
                    }
                }
                if (values.leadupdate != null)
                {
                    for (int i = 0; i < values.leadupdate.ToArray().Length; i++)
                    {

                        msSQL = "SELECT leadbank_gid,leadbankcontact_name FROM crm_trn_tleadbankcontact WHERE mobile LIKE '%" + values.leadupdate[i].phone_number + "%' and main_contact ='Y' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader != null && objOdbcDataReader.HasRows)
                        {
                            string lsleadbankcontact_name = objOdbcDataReader["leadbankcontact_name"].ToString();
                            msSQL = " update crm_smm_tclicktocall  set lead_flag = 'Y' ,user_name = '" + lsleadbankcontact_name + "' WHERE phone_number LIKE '%" + values.leadupdate[i].phone_number + "%' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                        }

                    }
            }
            catch (Exception ex)
            {
                values.message = "Error While Vieing Mail";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Livechat/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
            finally
            {
                if (objOdbcDataReader != null)
                    objOdbcDataReader.Close();
            }
        }

    }
}
