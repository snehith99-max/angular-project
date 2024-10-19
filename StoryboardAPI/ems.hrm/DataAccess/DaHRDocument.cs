﻿using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Web;
using System.Net;
using System.Drawing;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using RestSharp;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using MySql.Data.MySqlClient;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.hrm.DataAccess
{
    public class DaHRDocument
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        Fnazurestorage objcmnstorage = new Fnazurestorage();
        DataTable dt_datatable;
        OdbcDataReader objMySqlDataReader;
        string msSQL, msGetGid, lsmaster_value, lscompany_document_flag, lsname, lsemployee_emailid;
        int mnResult;
        string msGetGidRs, msGetGidRsSP, msGetAPICode;
        string lsfilePath, EsignPath, certificatepath, lsid, lsfile_name, lscreated_at, lsupdated_at, lsexpire_on, lsstatus, lsdigioresponse;
        string lsemployee_gid, lshrdocsigned_path, lshrdocsigned_path1, lsagreement_status, lsdigiouploadesignresponseformdataspdtls_gid;
        int mnResultRs, mnResultRsSP, SigningPartiesCount = 0, mnResultRsSPSecondary;
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string lspath;
        HttpPostedFile httpPostedFile;
        public void DaGetSysHRDocument(MdlHRDocument objhrdocument)
        {
            try
            {
                
                msSQL = " SELECT a.hrdocument_gid,a.hrdocument_name, date_format(a.created_date,'%d-%m-%Y %h:%i %p') as created_date, " +
                        " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by,api_code," +
                        " case when a.status='N' then 'Inactive' else 'Active' end as status" +
                        " FROM sys_mst_thrdocument a" +
                        " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid order by a.hrdocument_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var gethrdocument_list = new List<hrdocument_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        gethrdocument_list.Add(new hrdocument_list
                        {
                            hrdocument_gid = (dr_datarow["hrdocument_gid"].ToString()),
                            hrdocument_name = (dr_datarow["hrdocument_name"].ToString()),
                            created_by = (dr_datarow["created_by"].ToString()),
                            created_date = (dr_datarow["created_date"].ToString()),
                            status = (dr_datarow["status"].ToString()),
                            api_code = (dr_datarow["api_code"].ToString()),
                        });
                    }
                    objhrdocument.hrdocument_list = gethrdocument_list;
                }
                dt_datatable.Dispose();
                objhrdocument.status = true;
            }
            catch (Exception ex)
            {
                objhrdocument.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetSysHRDocumentDropDown(MdlHRDocument objhrdocument)
        {
            try
            {
                
                msSQL = " SELECT a.hrdocument_gid,a.hrdocument_name " +
                        " FROM sys_mst_thrdocument a" +
                        " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                        " left join adm_mst_tuser c on c.user_gid = b.user_gid Where a.status='Y' order by a.hrdocument_gid desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var gethrdocument_list = new List<hrdocument_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        gethrdocument_list.Add(new hrdocument_list
                        {
                            hrdocument_gid = (dr_datarow["hrdocument_gid"].ToString()),
                            hrdocument_name = (dr_datarow["hrdocument_name"].ToString()),

                        });
                    }
                    objhrdocument.hrdocument_list = gethrdocument_list;
                }
                dt_datatable.Dispose();
                objhrdocument.status = true;
            }
            catch (Exception ex)
            {
                objhrdocument.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaCreateSysHRDocument(hrdocument values, string employee_gid)
        {
            try
            {
                
                msSQL = "select hrdocument_name from sys_mst_thrdocument where hrdocument_name = '" + values.hrdocument_name.Replace("'", @"\'") + "'";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    values.status = false;
                    values.message = "HR Document name Already Exist";
                }
                else
                {
                    msGetAPICode = objcmnfunctions.GetApiMasterGID("HRAC");
                    msGetGid = objcmnfunctions.GetMasterGID("HRDG");
                    msSQL = " insert into sys_mst_thrdocument(" +
                            " hrdocument_gid ," +
                            " api_code," +
                            " hrdocument_name ," +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            "'" + msGetGid + "'," +
                            "'" + msGetAPICode + "',";

                    msSQL += "'" + values.hrdocument_name.Replace("'", @"\'") + "'," +
                            "'" + employee_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "HR Document Added Successfully";
                    }
                    else
                    {
                        values.message = "Error occured while adding";
                        values.status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        // Edit
        public void DaEditSysHRDocument(string hrdocument_gid, hrdocument values)
        {
            try
            {
                
                msSQL = " SELECT hrdocument_gid,hrdocument_name, status as status FROM sys_mst_thrdocument " +
                        " where hrdocument_gid='" + hrdocument_gid + "' ";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    values.hrdocument_gid = objMySqlDataReader["hrdocument_gid"].ToString();
                    values.hrdocument_name = objMySqlDataReader["hrdocument_name"].ToString();
                    values.Status = objMySqlDataReader["Status"].ToString();
                }
                objMySqlDataReader.Close();
                values.status = true;

            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaUpdateSysHRDocument(string employee_gid, hrdocument values)
        {
            try
            {
                
                msSQL = "select updated_by, date_format(updated_date,'%Y-%m-%d %h:%i:%s') as updated_date,hrdocument_name from sys_mst_thrdocument where hrdocument_gid ='" + values.hrdocument_gid + "' ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows == true)
                {
                    string lsUpdatedBy = objMySqlDataReader["updated_by"].ToString();
                    string lsUpdatedDate = objMySqlDataReader["updated_date"].ToString();

                    if (!(String.IsNullOrEmpty(lsUpdatedBy)) && !(String.IsNullOrEmpty(lsUpdatedDate)))
                    {
                        msGetGid = objcmnfunctions.GetMasterGID("HDLG");
                        msSQL = " insert into sys_mst_thrdocumentlog(" +
                                  " hrdocumentlog_gid  ," +
                                  " hrdocument_gid," +
                                  " hrdocument_name, " +
                                  " updated_by, " +
                                  " updated_date) " +
                                  " values(" +
                                  "'" + msGetGid + "'," +
                                  "'" + values.hrdocument_gid + "'," +
                                  "'" + objMySqlDataReader["hrdocument_name"].ToString().Replace("'", @"\'") + "'," +
                                  "'" + employee_gid + "'," +
                                  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }
                objMySqlDataReader.Close();
                msSQL = " update sys_mst_thrdocument set ";

                msSQL += " hrdocument_name='" + values.hrdocument_name.Replace("'", @"\'") + "'," +
                     " updated_by='" + employee_gid + "'," +
                     " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                     " where hrdocument_gid='" + values.hrdocument_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "HR Document Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured while updating";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        //Status
        public void DaInactiveSysHRDocument(hrdocument values, string employee_gid)
        {
            try
            {
                
                msSQL = " update sys_mst_thrdocument set status='" + values.rbo_status + "'," +
                    " remarks='" + values.remarks.Replace("'", @"\'") + "'" +
                    " where hrdocument_gid='" + values.hrdocument_gid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    msGetGid = objcmnfunctions.GetMasterGID("HRIA");

                    msSQL = " insert into sys_mst_thrdocumentinactivelog (" +
                          " hrdocumentinactivelog_gid, " +
                          " hrdocument_gid," +
                          " hrdocument_name," +
                          " status," +
                          " remarks," +
                          " updated_by," +
                          " updated_date) " +
                          " values (" +
                          " '" + msGetGid + "'," +
                          " '" + values.hrdocument_gid + "'," +
                          " '" + values.hrdocument_name.Replace("'", @"\'") + "'," +
                          " '" + values.rbo_status + "'," +
                          " '" + values.remarks.Replace("'", @"\'") + "'," +
                          " '" + employee_gid + "'," +
                          " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (values.rbo_status == 'N')
                    {
                        values.status = true;
                        values.message = "HR Document Inactivated Successfully";
                    }
                    else
                    {
                        values.status = true;
                        values.message = "HR Document Activated Successfully";
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Error occurred";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaInactiveSysHRDocumentHistory(SysHRDocumentInactiveHistory objhrdocumenthistory, string hrdocument_gid)
        {
            try
            {
                
                msSQL = " select a.remarks,date_format(a.updated_date,'%Y-%m-%d %h:%i:%s') as updated_date, " +
                " concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as updated_by," +
                " case when a.status='N' then 'Inactive' else 'Active' end as status" +
                " from sys_mst_thrdocumentinactivelog a " +
                " left join hrm_mst_temployee b on a.updated_by = b.employee_gid" +
                " left join adm_mst_tuser c on b.user_gid = c.user_gid " +
                " where a.hrdocument_gid='" + hrdocument_gid + "' order by a.hrdocumentinactivelog_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var gethrdocumentinactivehistory_list = new List<hrdocumentinactivehistory_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        gethrdocumentinactivehistory_list.Add(new hrdocumentinactivehistory_list
                        {
                            status = (dr_datarow["status"].ToString()),
                            remarks = (dr_datarow["remarks"].ToString()),
                            updated_by = (dr_datarow["updated_by"].ToString()),
                            updated_date = (dr_datarow["updated_date"].ToString())
                        });
                    }
                    objhrdocumenthistory.hrdocumentinactivehistory_list = gethrdocumentinactivehistory_list;
                }
                dt_datatable.Dispose();
                objhrdocumenthistory.status = true;
            }
            catch (Exception ex)
            {
                objhrdocumenthistory.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        // Delete
        public void DaDeleteSysHRDocument(string hrdocument_gid, string employee_gid, result values)
        {
            try
            {
                
                msSQL = " delete from sys_mst_thrdocument where hrdocument_gid ='" + hrdocument_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "HR Document Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error occured..!";
                }
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        //E Signing - Uploading Document to Digio
        public void DaUploadDocumenttoDigio(MdlFileDetailsEsign values)
        {
            try
            {
                
                int lastIndex = values.file_name.LastIndexOf('.');
                var filename = values.file_name.Substring(0, lastIndex);
                var format = values.file_name.Substring(lastIndex + 1);

                if (format == "pdf")
                {
                    try
                    {
                        msSQL = "select company_document_flag from adm_mst_tcompany";
                        lscompany_document_flag = objdbconn.GetExecuteScalar(msSQL);
                        MemoryStream ms = new MemoryStream();

                        //Downloading File Stream From Azure
                        if (values.migration_flag == "Y")
                        {
                            ms = objFnazurestorage.OtherDownloadStream("samhrdocument", values.file_path, lscompany_document_flag, "HRMigration");
                        }
                        else
                            ms = objFnazurestorage.DownloadStream("erpdocument", values.file_path, lscompany_document_flag);

                        lsfilePath = HttpContext.Current.Server.MapPath("../../../erpdocument/Esign/" + DateTime.Now.Year + "/" + DateTime.Now.Month);

                        if ((!System.IO.Directory.Exists(lsfilePath)))
                            System.IO.Directory.CreateDirectory(lsfilePath);

                        EsignPath = filename + "." + format;

                        certificatepath = Path.Combine(lsfilePath, EsignPath);

                        FileStream file = new FileStream(certificatepath, FileMode.Create, FileAccess.Write);
                        ms.WriteTo(file);
                        file.Close();
                        ms.Close();

                    }
                    catch (Exception ex)
                    {
                        values.message = "Error Occured While Proceeding for E sign";
                        values.status = false;
                        return;
                    }

                    MdlUploadDocumenttoDigioRequest ObjMdlUploadDocumenttoDigioRequest = new MdlUploadDocumenttoDigioRequest();

                    msSQL = " SELECT expire_in_days,comment,display_on_page,send_sign_link,notify_signers FROM sys_mst_tdigiouploadesignrequestformdata " +
                            " where digiouploadesignrequestformdata_gid='" + ConfigurationManager.AppSettings["esignrequesttemplateid"].ToString() + "'";

                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows)
                    {
                        ObjMdlUploadDocumenttoDigioRequest.expire_in_days = objMySqlDataReader["expire_in_days"].ToString();
                        ObjMdlUploadDocumenttoDigioRequest.comment = objMySqlDataReader["comment"].ToString();
                        ObjMdlUploadDocumenttoDigioRequest.display_on_page = objMySqlDataReader["display_on_page"].ToString();
                        ObjMdlUploadDocumenttoDigioRequest.send_sign_link = objMySqlDataReader["send_sign_link"].ToString();
                        ObjMdlUploadDocumenttoDigioRequest.notify_signers = objMySqlDataReader["notify_signers"].ToString();
                    }
                    objMySqlDataReader.Close();

                    msSQL = "SELECT employee_gid from sys_mst_temployeehrdocument where hrdoc_id='" + values.hrdoc_id + "'";
                    lsemployee_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " select concat(a.user_firstname,' ',a.user_lastname) as name,b.employee_emailid from  adm_mst_tuser a left join hrm_mst_temployee b on a.user_gid=b.user_gid " +
                            " where b.employee_gid='" + lsemployee_gid + "'";

                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows)
                    {
                        lsname = objMySqlDataReader["name"].ToString();
                        lsemployee_emailid = objMySqlDataReader["employee_emailid"].ToString();
                    }
                    objMySqlDataReader.Close();

                    if (String.IsNullOrEmpty(lsemployee_emailid))
                    {
                        values.message = "Employee email details is not available";
                        values.status = false;
                        return;
                    }
                    if (String.IsNullOrEmpty(lsname))
                    {
                        values.message = "Employee name is not available";
                        values.status = false;
                        return;
                    }

                    msSQL = " SELECT identifier,name,reason FROM sys_mst_tdigiouploadesignrequestformdatasignersdtl " +
                            " where digiouploadesignrequestformdata_gid='" + ConfigurationManager.AppSettings["esignrequesttemplateid"].ToString() + "'";

                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    ObjMdlUploadDocumenttoDigioRequest.signers = new UploadDocumenttoDigioRequestsigners[dt_datatable.Rows.Count];

                    for (int i = 0; i < ObjMdlUploadDocumenttoDigioRequest.signers.Length; i++)
                    {
                        ObjMdlUploadDocumenttoDigioRequest.signers[i] = new UploadDocumenttoDigioRequestsigners();
                    }
                    if (dt_datatable.Rows.Count != 0)
                    {
                        int signersdtl = 0;
                        foreach (DataRow dr_datarow in dt_datatable.Rows)
                        {
                            ObjMdlUploadDocumenttoDigioRequest.signers[signersdtl].identifier = lsemployee_emailid;
                            ObjMdlUploadDocumenttoDigioRequest.signers[signersdtl].name = lsname;
                            ObjMdlUploadDocumenttoDigioRequest.signers[signersdtl].reason = dr_datarow["reason"].ToString();

                            signersdtl++;
                        }
                    }
                    dt_datatable.Dispose();

                    string lsuploaddocumenttodigio_json = Newtonsoft.Json.JsonConvert.SerializeObject(ObjMdlUploadDocumenttoDigioRequest);

                    MdlUploadDocumenttoDigioResponse ObjMdlUploadDocumenttoDigioResponse = new MdlUploadDocumenttoDigioResponse();

                    var client = new RestClient(ConfigurationManager.AppSettings["esign_uploaddocumenttodigiourl"].ToString());
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                    var request = new RestRequest(Method.POST);
                    request.AlwaysMultipartFormData = true;
                    request.AddHeader("Content-Type", "multipart/form-data");
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["esign_basicauthusername"].ToString() + ":" + ConfigurationManager.AppSettings["esign_basicauthuserpassword"].ToString());
                    string val = System.Convert.ToBase64String(plainTextBytes);
                    request.AddHeader("Authorization", "Basic " + val);
                    request.AddParameter("request", lsuploaddocumenttodigio_json);
                    request.AddFile("file", certificatepath, "application/pdf");

                    IRestResponse response = client.Execute(request);

                    lsdigioresponse = response.Content;

                    ObjMdlUploadDocumenttoDigioResponse = JsonConvert.DeserializeObject<MdlUploadDocumenttoDigioResponse>(response.Content);

                    msGetGidRs = objcmnfunctions.GetMasterGID("UEFR");
                    msSQL = " insert into sys_mst_tdigiouploadesignresponseformdata(" +
                       " digiouploadesignresponseformdata_gid," +
                       " hrdoc_id," +
                       " id," +
                       " is_agreement," +
                       " agreement_type," +
                       " agreement_status," +
                       " file_name," +
                       " created_at," +
                       " self_signed," +
                       " self_sign_type," +
                       " no_of_pages," +
                       " name," +
                       " requested_on," +
                       " expire_on," +
                       " identifier," +
                       " requester_type," +
                       " channel," +
                       " response_time," +
                       " response_json)" +
                       " values(" +
                       "'" + msGetGidRs + "'," +
                       "'" + values.hrdoc_id + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.id + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.is_agreement + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.agreement_type + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.agreement_status + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.file_name + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.created_at + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.self_signed + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.self_sign_type + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.no_of_pages + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.sign_request_details.name + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.sign_request_details.requested_on + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.sign_request_details.expire_on + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.sign_request_details.identifier + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.sign_request_details.requester_type + "'," +
                       "'" + ObjMdlUploadDocumenttoDigioResponse.channel + "'," +
                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                       "'" + response.Content + "')";

                    mnResultRs = objdbconn.ExecuteNonQuerySQL(msSQL);

                    foreach (UploadDocumenttoDigioResponsesigning_parties signing_parties in ObjMdlUploadDocumenttoDigioResponse.signing_parties)
                    {
                        msGetGidRsSP = objcmnfunctions.GetMasterGID("ESFD");

                        msSQL = " insert into sys_mst_tdigiouploadesignresponseformdataspdtls(" +
                            " digiouploadesignresponseformdataspdtls_gid," +
                           " digiouploadesignresponseformdata_gid," +
                           " hrdoc_id," +
                           " name," +
                           " status," +
                           " type," +
                           " signature_type," +
                           " identifier," +
                           " reason," +
                           " expire_on)" +
                           " values(" +
                           "'" + msGetGidRsSP + "'," +
                           "'" + msGetGidRs + "'," +
                           "'" + values.hrdoc_id + "'," +
                           "'" + signing_parties.name + "'," +
                           "'" + signing_parties.status + "'," +
                           "'" + signing_parties.type + "'," +
                           "'" + signing_parties.signature_type + "'," +
                           "'" + signing_parties.identifier + "'," +
                           "'" + signing_parties.reason + "'," +
                           "'" + signing_parties.expire_on + "')";

                        mnResultRsSP = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResultRsSP == 1)
                        {
                            SigningPartiesCount++;
                        }
                    }
                    if (SigningPartiesCount == ObjMdlUploadDocumenttoDigioResponse.signing_parties.Length)
                    {
                        mnResultRsSPSecondary = 1;
                    }
                    if (mnResultRs != 0 && mnResultRsSPSecondary != 0)
                    {
                        values.message = "Document has been sent for E Signing";
                        values.status = true;
                        msSQL = " update sys_mst_temployeehrdocument set " +
                                " documentsentforsign_flag='Y'," +
                                " esignexpiry_flag ='N'" +
                                " where hrdoc_id='" + values.hrdoc_id + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    }
                    else
                    {
                        values.message = "Error occurred while Proceeding for E sign";
                        values.status = false;
                    }

                }
                else
                {
                    values.message = "Invalid Document Format";
                    values.status = false;
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        //E Signing - Get Document Details
        public void DaGetDocumentDetails(MdlFileDetailsEsign values, string user_gid)
        {
            try
            {
                
                msSQL = " select id,file_name,created_at from sys_mst_tdigiouploadesignresponseformdata where hrdoc_id ='" + values.hrdoc_id + "' order by response_time desc limit 1";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    lsid = objMySqlDataReader["id"].ToString();
                    lsfile_name = objMySqlDataReader["file_name"].ToString();
                    lscreated_at = objMySqlDataReader["created_at"].ToString();
                    objMySqlDataReader.Close();
                }
                else
                {
                    values.message = "E signing has not been done yet";
                    values.status = false;
                    return;
                }

                MdlUploadDocumenttoDigioResponse ObjMdlUploadDocumenttoDigioResponse = new MdlUploadDocumenttoDigioResponse();

                var client = new RestClient(ConfigurationManager.AppSettings["esign_getdocumentdetailsurl"].ToString() + lsid);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["esign_basicauthusername"].ToString() + ":" + ConfigurationManager.AppSettings["esign_basicauthuserpassword"].ToString());
                string val = System.Convert.ToBase64String(plainTextBytes);
                request.AddHeader("Authorization", "Basic " + val);

                var body = @"";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                lsdigioresponse = response.Content;

                ObjMdlUploadDocumenttoDigioResponse = JsonConvert.DeserializeObject<MdlUploadDocumenttoDigioResponse>(response.Content);

                lsstatus = ObjMdlUploadDocumenttoDigioResponse.signing_parties[0].status;

                msSQL = " update sys_mst_tdigiouploadesignresponseformdata set " +
                        " updated_at='" + ObjMdlUploadDocumenttoDigioResponse.updated_at + "'" +
                        " where id='" + lsid + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                lsupdated_at = ObjMdlUploadDocumenttoDigioResponse.updated_at;
                lsagreement_status = ObjMdlUploadDocumenttoDigioResponse.agreement_status;

                if (lsstatus == "signed")
                {
                    msSQL = " update sys_mst_tdigiouploadesignresponseformdata set " +
                            " documentsigned_flag='Y'," +
                            " agreement_status='" + lsagreement_status + "'" +
                            " where id='" + lsid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " update sys_mst_temployeehrdocument set " +
                            " documentsigned_flag='Y'" +
                            " where hrdoc_id='" + values.hrdoc_id + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " select digiouploadesignresponseformdataspdtls_gid from sys_mst_tdigiouploadesignresponseformdataspdtls a left join sys_mst_tdigiouploadesignresponseformdata b on a.digiouploadesignresponseformdata_gid=b.digiouploadesignresponseformdata_gid" +
                            " where b.id='" + lsid + "'";
                    lsdigiouploadesignresponseformdataspdtls_gid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " update sys_mst_tdigiouploadesignresponseformdataspdtls set " +
                            " status='" + lsstatus + "'" +
                            " where digiouploadesignresponseformdataspdtls_gid='" + lsdigiouploadesignresponseformdataspdtls_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.message = "E signing has been Completed for the Document at " + lsupdated_at + " ";
                    values.status = true;

                }
                else if (lsstatus == "expired")
                {
                    msSQL = " update sys_mst_tdigiouploadesignresponseformdata set " +
                            " agreement_status='" + lsagreement_status + "'" +
                            " where id='" + lsid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " select digiouploadesignresponseformdataspdtls_gid from sys_mst_tdigiouploadesignresponseformdataspdtls a left join sys_mst_tdigiouploadesignresponseformdata b on a.digiouploadesignresponseformdata_gid=b.digiouploadesignresponseformdata_gid" +
                            " where b.id='" + lsid + "'";
                    lsdigiouploadesignresponseformdataspdtls_gid = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = " update sys_mst_tdigiouploadesignresponseformdataspdtls set " +
                            " status='" + lsstatus + "'" +
                            " where digiouploadesignresponseformdataspdtls_gid='" + lsdigiouploadesignresponseformdataspdtls_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    values.message = "Document has been Expired";
                    values.status = false;

                }
                else
                {
                    values.message = "E signing has not been done yet " +
                                     "current status : " + lsstatus + "";
                    values.status = false;
                }

            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        //E Signing - Download Document from Digio
        public dynamic DaDownloadDocfromDigio(MdlFileDetailsEsign values)
        {
            string format = "pdf";

            var ls_response = new Dictionary<string, object>();

            string lscompany_code = string.Empty;

            String path = lspath;
            try
            {
                
                msSQL = " select id,file_name from sys_mst_tdigiouploadesignresponseformdata where hrdoc_id ='" + values.hrdoc_id + "' order by response_time desc limit 1";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows)
                {
                    lsid = objMySqlDataReader["id"].ToString();
                    lsfile_name = objMySqlDataReader["file_name"].ToString();
                }
                objMySqlDataReader.Close();

                int lastIndex = lsfile_name.LastIndexOf('.');
                var filename = lsfile_name.Substring(0, lastIndex);

                string DownloadDocfromDigioURL = ConfigurationManager.AppSettings["esign_downloaddocfromdigiourl"].ToString();

                var uriBuilder = new UriBuilder(DownloadDocfromDigioURL);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["document_id"] = lsid;
                uriBuilder.Query = query.ToString();
                DownloadDocfromDigioURL = uriBuilder.ToString();

                var client = new RestClient(DownloadDocfromDigioURL);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["esign_basicauthusername"].ToString() + ":" + ConfigurationManager.AppSettings["esign_basicauthuserpassword"].ToString());
                string val = System.Convert.ToBase64String(plainTextBytes);
                request.AddHeader("Authorization", "Basic " + val);

                var body = @"";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                lsdigioresponse = response.Content;

                byte[] buffer = client.DownloadData(request);

                MemoryStream ms = new MemoryStream(buffer);

                string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");

                msSQL = " select hrdocsigned_path,signeddoc_updatedtime from  sys_mst_temployeehrdocument " +
                         " where hrdoc_id='" + values.hrdoc_id + "'";

                lshrdocsigned_path1 = objdbconn.GetExecuteScalar(msSQL);

                if (String.IsNullOrEmpty(lshrdocsigned_path1))
                {

                    msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
                    lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                    path = ConfigurationManager.AppSettings["file_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "System/HRDocumentEsigned/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                    if ((!System.IO.Directory.Exists(path)))
                        System.IO.Directory.CreateDirectory(path);

                    bool status;
                    status = objcmnstorage.UploadStream("erpdocument", lscompany_code + "/" + "System/HRDocumentEsigned/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + "." + format, ms);
                    ms.Close();
                    lspath = "erpdocument" + "/" + lscompany_code + "/" + "System/HRDocumentEsigned/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                    msSQL = " update sys_mst_temployeehrdocument  set " +
                            " hrdocsigned_path ='" + lspath + msdocument_gid + "." + format + "'," +
                            " signeddoc_updatedtime ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                            " where hrdoc_id='" + values.hrdoc_id + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }

                msSQL = "select company_document_flag from adm_mst_tcompany";
                lscompany_document_flag = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select hrdocsigned_path from sys_mst_temployeehrdocument where hrdoc_id='" + values.hrdoc_id + "'";
                lshrdocsigned_path = objdbconn.GetExecuteScalar(msSQL);

                MemoryStream ms_fromazure = new MemoryStream();

                //Downloading File Stream From Azure
                ms_fromazure = objFnazurestorage.DownloadStream("erpdocument", lshrdocsigned_path, lscompany_document_flag);

                ls_response = objFnazurestorage.ConvertDocumentToByteArray(ms_fromazure, filename, format);

                ms.Close();

            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            return ls_response;
        }

        //E Signing - Update Expiry Date of the document
        public void DaUpdateExpiryDate(hrdoc_list values, string employee_gid)
        {
            try
            {
                
                msSQL = " select hrdoc_id,hrdocument_gid " +
                    " from sys_mst_temployeehrdocument a " +
                    " left join hrm_mst_temployee b on a.created_by = b.employee_gid " +
                    " where a.employee_gid = '" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                List<string> HRDocumentSummary_List = new List<string>();

                HRDocumentSummary_List = dt_datatable.AsEnumerable().Select(p => p.Field<string>("hrdoc_id")).ToList();

                foreach (var lshrdoc_id in HRDocumentSummary_List)
                {
                    msSQL = " select expire_on,created_at " +
                            " from sys_mst_tdigiouploadesignresponseformdata  " +
                            " where hrdoc_id = '" + lshrdoc_id + "' order by response_time desc limit 1 ";
                    objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                    if (objMySqlDataReader.HasRows)
                    {
                        lsexpire_on = objMySqlDataReader["expire_on"].ToString();
                        lscreated_at = objMySqlDataReader["created_at"].ToString();
                    }
                    objMySqlDataReader.Close();

                    if (!(String.IsNullOrEmpty(lsexpire_on)))
                    {
                        if ((DateTime.Now) > (DateTime.Parse(lsexpire_on)))
                        {
                            msSQL = " update sys_mst_temployeehrdocument  set " +
                                    " esignexpiry_flag ='Y'" +
                                    " where hrdoc_id='" + lshrdoc_id + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        //Exception log
        public void logforAuditEsignAPI(string strVal)
        {
            string loglspath = ConfigurationManager.AppSettings["file_path"] + "/erpdocument" + "/" + "ErrorLogEsignAPI/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
            if ((!System.IO.Directory.Exists(loglspath)))
                System.IO.Directory.CreateDirectory(loglspath);

            loglspath = loglspath + "log.txt";
            System.IO.StreamWriter sw = new System.IO.StreamWriter(loglspath, true);
            sw.WriteLine(strVal);
            sw.Close();
        }

        //E Sign Unsigned Summary
        public bool DaGetESignUnsignedSummary(string employee_gid, hrdoc_list objemployeedoc_list)
        {
            try
            {
                
                msSQL = "select a.employee_gid as documentemployee_gid,concat(c.user_firstname, ' ', c.user_lastname, '/', c.user_code) as documentemployee_name,a.hrdoc_id,a.hrdocument_gid,a.hrdocument_name,a.hrdoc_name, a.hrdoc_path,a.documentsentforsign_flag,a.esignexpiry_flag,a.documentsigned_flag, " +
                "date_format(d.created_at, '%d-%m-%Y %h:%i %p') as document_proceededforesign, " +
                "date_format(d.expire_on, '%d-%m-%Y %h:%i %p') as expire_on  " +
                "from sys_mst_temployeehrdocument a " +
                "left join hrm_mst_temployee b on a.employee_gid = b.employee_gid  " +
                "left join adm_mst_tuser c on c.user_gid = b.user_gid  " +
                "left join sys_mst_tdigiouploadesignresponseformdata d on a.hrdoc_id = d.hrdoc_id  " +
                "where a.documentsigned_flag = 'N' and documentsentforsign_flag = 'Y' and expire_on > now() and " +
                "digiouploadesignresponseformdata_gid = (SELECT Max(digiouploadesignresponseformdata_gid) " +
                "FROM   sys_mst_tdigiouploadesignresponseformdata ds WHERE  ds.hrdoc_id = a.hrdoc_id) ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_hrdoc_list = new List<hrdoc>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_hrdoc_list.Add(new hrdoc_list
                        {
                            documentemployee_gid = dr_datarow["documentemployee_gid"].ToString(),
                            documentemployee_name = dr_datarow["documentemployee_name"].ToString(),
                            hrdoc_id = dr_datarow["hrdoc_id"].ToString(),
                            hrdocument_gid = dr_datarow["hrdocument_gid"].ToString(),
                            hrdocument_name = dr_datarow["hrdocument_name"].ToString(),
                            hrdoc_name = dr_datarow["hrdoc_name"].ToString(),
                            hrdoc_path = objcmnstorage.EncryptData((dr_datarow["hrdoc_path"].ToString())),
                            documentsentforsign_flag = dr_datarow["documentsentforsign_flag"].ToString(),
                            esignexpiry_flag = dr_datarow["esignexpiry_flag"].ToString(),
                            documentsigned_flag = dr_datarow["documentsigned_flag"].ToString(),
                            document_proceededforesign = dr_datarow["document_proceededforesign"].ToString(),
                            expire_on = dr_datarow["expire_on"].ToString()
                        });
                    }
                    objemployeedoc_list.hrdoc = get_hrdoc_list;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                objemployeedoc_list.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return false;
            }
        }

        //E Sign Signed Summary
        public bool DaGetESignSignedSummary(string employee_gid, hrdoc_list objemployeedoc_list)
        {
            try
            {
                
                msSQL = "select a.employee_gid as documentemployee_gid,concat(c.user_firstname, ' ', c.user_lastname, '/', c.user_code) as documentemployee_name,a.hrdoc_id,a.hrdocument_gid,a.hrdocument_name,a.hrdoc_name, a.hrdoc_path,a.documentsentforsign_flag,a.esignexpiry_flag,a.documentsigned_flag, " +
                        "date_format(d.created_at, '%d-%m-%Y %h:%i %p') as document_proceededforesign, " +
                        "date_format(d.updated_at, '%d-%m-%Y %h:%i %p') as documentsigned_date, " +
                        "date_format(d.expire_on, '%d-%m-%Y %h:%i %p') as expire_on  " +
                        "from sys_mst_temployeehrdocument a " +
                        "left join hrm_mst_temployee b on a.employee_gid = b.employee_gid  " +
                        "left join adm_mst_tuser c on c.user_gid = b.user_gid  " +
                        "left join sys_mst_tdigiouploadesignresponseformdata d on a.hrdoc_id = d.hrdoc_id  " +
                        "where a.documentsigned_flag = 'Y' and documentsentforsign_flag = 'Y' and " +
                        "digiouploadesignresponseformdata_gid = (SELECT Max(digiouploadesignresponseformdata_gid) " +
                        "FROM   sys_mst_tdigiouploadesignresponseformdata ds WHERE  ds.hrdoc_id = a.hrdoc_id) ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_hrdoc_list = new List<hrdoc>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_hrdoc_list.Add(new hrdoc_list
                        {
                            documentemployee_gid = dr_datarow["documentemployee_gid"].ToString(),
                            documentemployee_name = dr_datarow["documentemployee_name"].ToString(),
                            hrdoc_id = dr_datarow["hrdoc_id"].ToString(),
                            hrdocument_gid = dr_datarow["hrdocument_gid"].ToString(),
                            hrdocument_name = dr_datarow["hrdocument_name"].ToString(),
                            hrdoc_name = dr_datarow["hrdoc_name"].ToString(),
                            hrdoc_path = objcmnstorage.EncryptData((dr_datarow["hrdoc_path"].ToString())),
                            documentsentforsign_flag = dr_datarow["documentsentforsign_flag"].ToString(),
                            esignexpiry_flag = dr_datarow["esignexpiry_flag"].ToString(),
                            documentsigned_flag = dr_datarow["documentsigned_flag"].ToString(),
                            document_proceededforesign = dr_datarow["document_proceededforesign"].ToString(),
                            documentsigned_date = dr_datarow["documentsigned_date"].ToString()
                        });
                    }
                    objemployeedoc_list.hrdoc = get_hrdoc_list;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                objemployeedoc_list.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return false;
            }
        }
        //E Sign Expired Summary
        public bool DaGetESignExpiredSummary(string employee_gid, hrdoc_list objemployeedoc_list)
        {
            try
            {
                
                msSQL = "select a.employee_gid as documentemployee_gid,concat(c.user_firstname, ' ', c.user_lastname, '/', c.user_code) as documentemployee_name,a.hrdoc_id,a.hrdocument_gid,a.hrdocument_name,a.hrdoc_name, a.hrdoc_path,a.documentsentforsign_flag,a.esignexpiry_flag,a.documentsigned_flag, " +
                        "date_format(d.created_at, '%d-%m-%Y %h:%i %p') as document_proceededforesign, " +
                        "date_format(d.updated_at, '%d-%m-%Y %h:%i %p') as documentsigned_date, " +
                        "date_format(d.expire_on, '%d-%m-%Y %h:%i %p') as expire_on  " +
                        "from sys_mst_temployeehrdocument a " +
                        "left join hrm_mst_temployee b on a.employee_gid = b.employee_gid  " +
                        "left join adm_mst_tuser c on c.user_gid = b.user_gid  " +
                        "left join sys_mst_tdigiouploadesignresponseformdata d on a.hrdoc_id = d.hrdoc_id  " +
                        "where a.documentsigned_flag = 'N' and documentsentforsign_flag = 'Y' and " +
                        "expire_on <= now() and " +
                        "digiouploadesignresponseformdata_gid = (SELECT Max(digiouploadesignresponseformdata_gid) " +
                        "FROM   sys_mst_tdigiouploadesignresponseformdata ds WHERE  ds.hrdoc_id = a.hrdoc_id) ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var get_hrdoc_list = new List<hrdoc>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dr_datarow in dt_datatable.Rows)
                    {
                        get_hrdoc_list.Add(new hrdoc_list
                        {
                            documentemployee_gid = dr_datarow["documentemployee_gid"].ToString(),
                            documentemployee_name = dr_datarow["documentemployee_name"].ToString(),
                            hrdoc_id = dr_datarow["hrdoc_id"].ToString(),
                            hrdocument_gid = dr_datarow["hrdocument_gid"].ToString(),
                            hrdocument_name = dr_datarow["hrdocument_name"].ToString(),
                            hrdoc_name = dr_datarow["hrdoc_name"].ToString(),
                            hrdoc_path = objcmnstorage.EncryptData((dr_datarow["hrdoc_path"].ToString())),
                            documentsentforsign_flag = dr_datarow["documentsentforsign_flag"].ToString(),
                            esignexpiry_flag = dr_datarow["esignexpiry_flag"].ToString(),
                            documentsigned_flag = dr_datarow["documentsigned_flag"].ToString(),
                            document_proceededforesign = dr_datarow["document_proceededforesign"].ToString(),
                            documentsigned_date = dr_datarow["documentsigned_date"].ToString(),
                            expire_on = dr_datarow["expire_on"].ToString()
                        });
                    }
                    objemployeedoc_list.hrdoc = get_hrdoc_list;
                }
                dt_datatable.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                objemployeedoc_list.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return false;
            }
        }
        //E Sign Report Summary Count
        public void DaGetESignReportSummaryCount(hrdoc_list values)
        {
            try
            {
                
                msSQL = "select count(a.hrdoc_id) as unsigneddocument_count " +
                    "from sys_mst_temployeehrdocument a " +
                    "left join sys_mst_tdigiouploadesignresponseformdata d on a.hrdoc_id = d.hrdoc_id  " +
                    "where a.documentsigned_flag = 'N' and documentsentforsign_flag = 'Y' and expire_on > now() and " +
                    "digiouploadesignresponseformdata_gid = (SELECT Max(digiouploadesignresponseformdata_gid) " +
                    "FROM   sys_mst_tdigiouploadesignresponseformdata ds WHERE  ds.hrdoc_id = a.hrdoc_id) ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    values.pendingesign_count = objMySqlDataReader["unsigneddocument_count"].ToString();

                }
                objMySqlDataReader.Close();

                msSQL = "select count(a.hrdoc_id) as signeddocument_count " +
                        "from sys_mst_temployeehrdocument a " +
                        "left join sys_mst_tdigiouploadesignresponseformdata d on a.hrdoc_id = d.hrdoc_id  " +
                        "where a.documentsigned_flag = 'Y' and documentsentforsign_flag = 'Y' and " +
                        "digiouploadesignresponseformdata_gid = (SELECT Max(digiouploadesignresponseformdata_gid) " +
                        "FROM   sys_mst_tdigiouploadesignresponseformdata ds WHERE  ds.hrdoc_id = a.hrdoc_id) ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    values.completedesign_count = objMySqlDataReader["signeddocument_count"].ToString();

                }
                objMySqlDataReader.Close();

                msSQL = "   select count(a.hrdoc_id) as expireddocument_count " +
                      "from sys_mst_temployeehrdocument a " +
                      "left join sys_mst_tdigiouploadesignresponseformdata d on a.hrdoc_id = d.hrdoc_id  " +
                      "where a.documentsigned_flag = 'N' and documentsentforsign_flag = 'Y' and  expire_on <= now() and  " +
                      "digiouploadesignresponseformdata_gid = (SELECT Max(digiouploadesignresponseformdata_gid) " +
                      "FROM   sys_mst_tdigiouploadesignresponseformdata ds WHERE  ds.hrdoc_id = a.hrdoc_id)     ";
                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    values.expiredesign_count = objMySqlDataReader["expireddocument_count"].ToString();

                }
                objMySqlDataReader.Close();
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        //E Sign Report HR Document Excel Export
        public void DaGetESignReportHRDocExcelExport(hrdoc values)
        {
            try
            {
                
                msSQL = "select concat(c.user_firstname, ' ', c.user_lastname, '/', c.user_code) as 'Employee Name',a.hrdocument_name as 'Document Type',a.hrdoc_name as 'Document Name', " +
                    "IF(a.documentsigned_flag = 'Y', 'Signed', 'Un Signed') as 'Sign Status', " +
                    "date_format(d.created_at, '%d-%m-%Y %h:%i %p') as 'E Sign Request Date', " +
                    "IF(a.documentsigned_flag = 'Y',  date_format(d.updated_at, '%d-%m-%Y %h:%i %p'), 'NA') as 'Document Signed Date', " +
                    "date_format(d.expire_on, '%d-%m-%Y %h:%i %p') as 'Expiry Date'  " +
                    "from sys_mst_temployeehrdocument a " +
                    "left join hrm_mst_temployee b on a.employee_gid = b.employee_gid  " +
                    "left join adm_mst_tuser c on c.user_gid = b.user_gid  " +
                    "left join sys_mst_tdigiouploadesignresponseformdata d on a.hrdoc_id = d.hrdoc_id  " +
                    "where documentsentforsign_flag = 'Y' and " +
                    "digiouploadesignresponseformdata_gid = (SELECT Max(digiouploadesignresponseformdata_gid) " +
                    "FROM   sys_mst_tdigiouploadesignresponseformdata ds WHERE  ds.hrdoc_id = a.hrdoc_id) ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                string lscompany_code = string.Empty;
                MemoryStream ms = new MemoryStream();
                ExcelPackage excel = new ExcelPackage(ms);

                var workSheet = excel.Workbook.Worksheets.Add("ESign HRDoc Report");
                try
                {
                    msSQL = " select company_code from adm_mst_tcompany";
                    lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                    values.lsname = "ESign HRDoc Report.xlsx";
                    var path = ConfigurationManager.AppSettings["file_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "System/ESignHRDocReport/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                    values.lspath = ConfigurationManager.AppSettings["file_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "System/ESignHRDocReport/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + values.lsname;

                    values.lscloudpath = lscompany_code + "/" + "System/ESignHRDocReport/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + values.lsname;
                    bool exists = System.IO.Directory.Exists(path);
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    workSheet.Cells[1, 1].LoadFromDataTable(dt_datatable, true);
                    FileInfo file = new FileInfo(values.lspath);
                    using (var range = workSheet.Cells[1, 1, 1, 7])  //Address "A1:A7"
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        range.Style.Font.Color.SetColor(Color.White);
                    }
                    //excel.SaveAs(file);
                    excel.SaveAs(ms);
                    bool status;
                    status = objcmnstorage.UploadStream("erpdocument", lscompany_code + "/" + "System/ESignHRDocReport/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + values.lsname, ms);
                    ms.Close();

                }
                catch (Exception ex)
                {
                    values.status = false;
                    values.message = "Failure";
                    return;
                }
                values.lscloudpath = objcmnstorage.EncryptData(values.lscloudpath);
                values.lspath = objcmnstorage.EncryptData(values.lspath);
                values.status = true;
                values.message = "Success";
            }
            catch (Exception ex)
            {
                values.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaImportHRdocumentData(HttpRequest httpRequest, string employee_gid, result objResult)
        {
            DataTable dt = null;
            string lscompany_code = "";
            DataTable Generaltable = new DataTable();
            List<HrDocumentImportdtl> HrDocumentdtl = new List<HrDocumentImportdtl>();

            try
            {
                //int insertCount = 0;
                HttpFileCollection httpFileCollection;

                string lspath, lsfilePath;

                
                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                // Create Directory
                lsfilePath = HttpContext.Current.Server.MapPath("../../../erpdocument" + "/" + lscompany_code + "/System/HRMigration/" + DateTime.Now.Year + "/" + DateTime.Now.Month);

                if ((!System.IO.Directory.Exists(lsfilePath)))
                    System.IO.Directory.CreateDirectory(lsfilePath);

                httpFileCollection = httpRequest.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    httpPostedFile = httpFileCollection[i];
                }
                string FileExtension = httpPostedFile.FileName;

                string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                string lsfile_gid = msdocument_gid;
                FileExtension = Path.GetExtension(FileExtension).ToLower();
                lsfile_gid = lsfile_gid + FileExtension;

                Stream ls_readStream;
                ls_readStream = httpPostedFile.InputStream;
                MemoryStream ms = new MemoryStream();
                ls_readStream.CopyTo(ms);

                //path creation        
                lspath = lsfilePath + "/";
                FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                var obj = new List<MdlExcelSheetInfo_list>();
                //MdlExcelSheetInfo obj = new MdlExcelSheetInfo();
                try
                {
                    using (ExcelPackage xlPackage = new ExcelPackage(ms))
                    {
                        int[] arr = new int[xlPackage.Workbook.Worksheets.Count];
                        int totalsheet = xlPackage.Workbook.Worksheets.Count;
                        for (int i = 0; i < totalsheet; i++)
                        {
                            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[i + 1];
                            obj.Add(new MdlExcelSheetInfo_list
                            {
                                sheetName = "" + worksheet.Name + "$",
                                rowCount = worksheet.Dimension.End.Row,
                                columnCount = worksheet.Dimension.End.Column,
                                endRange = worksheet.Dimension.End.Address,
                            });
                        }
                    }
                    file.Close();
                    ms.Close();

                    objcmnfunctions.uploadFile(lspath, lsfile_gid);

                    try
                    {
                        lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";

                        string lsConnectionString = string.Empty;
                        string fileExtension = Path.GetExtension(lsfilePath);
                        if (fileExtension == ".xls")
                        {
                            lsConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + lsfilePath + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                        }
                        else if (fileExtension == ".xlsx")
                        {
                            lsConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + lsfilePath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0';";
                        }
                        //int totalSheet = 1;

                        string excelRange;

                        using (OleDbConnection objConn = new OleDbConnection(lsConnectionString))
                        {
                            objConn.Open();
                            OleDbCommand cmd = new OleDbCommand();
                            OleDbDataAdapter oleda = new OleDbDataAdapter();
                            DataSet ds = new DataSet();
                            DataTable dt1 = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = string.Empty;
                            if (dt1 != null)
                            {
                                var tempDataTable = (from dataRow in dt1.AsEnumerable()
                                                     where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                                     select dataRow).CopyToDataTable();
                                dt1 = tempDataTable;
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {

                                    //totalSheet = dt1.Rows.Count;
                                    sheetName = dt1.Rows[i]["TABLE_NAME"].ToString();
                                    var getrange = obj.Where(x => x.sheetName == sheetName).FirstOrDefault();
                                    excelRange = "A1:" + getrange.endRange + getrange.rowCount.ToString();
                                    sheetName = sheetName.Replace("'", "").Trim() + excelRange;
                                    cmd.Connection = objConn;
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                                    oleda = new OleDbDataAdapter(cmd);
                                    string DataTableName = dt1.Rows[i]["TABLE_NAME"].ToString().Replace("'", "").Trim();
                                    DataTableName = DataTableName.Replace("$", "");
                                    DataTableName = DataTableName.Replace(" ", "");
                                    oleda.Fill(ds, DataTableName);
                                }
                            }

                            Generaltable = ds.Tables["HRDocument"];
                            objConn.Close();

                            if (Generaltable != null)
                            {
                                Generaltable = Generaltable.Rows.Cast<DataRow>().Where(r => string.Join("", r.ItemArray).Trim() != string.Empty).CopyToDataTable();
                                HrDocumentdtl = cmnfunctions.ConvertDataTable<HrDocumentImportdtl>(Generaltable);
                                Generaltable.Dispose();
                                Generaltable = null;
                            }
                            if (HrDocumentdtl.Count != 0)
                            {
                                LogForAudit("---------HR Document Mapping Details - Started !--------------");
                                msSQL = " SELECT a.hrdocument_gid,a.hrdocument_name " +
                                        " FROM sys_mst_thrdocument a" +
                                        " left join hrm_mst_temployee b on a.created_by = b.employee_gid" +
                                        " left join adm_mst_tuser c on c.user_gid = b.user_gid Where a.status='Y' " +
                                        " order by a.hrdocument_gid desc ";
                                dt_datatable = objdbconn.GetDataTable(msSQL);
                                List<hrdocument_list> Msthrdocument_list = new List<hrdocument_list>();
                                Msthrdocument_list = cmnfunctions.ConvertDataTable<hrdocument_list>(dt_datatable);
                                dt_datatable.Dispose();

                                msSQL = " SELECT a.employee_gid,c.user_code " +
                                    " FROM hrm_mst_temployee a" +
                                    " left join adm_mst_tuser c on c.user_gid = a.user_gid Where c.user_status='Y'";
                                dt_datatable = objdbconn.GetDataTable(msSQL);
                                List<Mdlemployee_list> Mdlemployee_list = new List<Mdlemployee_list>();
                                Mdlemployee_list = cmnfunctions.ConvertDataTable<Mdlemployee_list>(dt_datatable);
                                dt_datatable.Dispose();

                                foreach (var values in HrDocumentdtl)
                                {
                                    try
                                    {
                                        var getDocumentid = Msthrdocument_list.Where(x => x.hrdocument_name.ToLower().Trim() == values.document_id.ToLower().Trim()).FirstOrDefault();
                                        var employeedtl = Mdlemployee_list.Where(x => x.user_code == values.employee_code).FirstOrDefault();
                                        if (employeedtl != null)
                                        {
                                            msGetGid = objcmnfunctions.GetMasterGID("HRDU");
                                            msSQL = " insert into sys_mst_temployeehrdocument(" +
                                                        " hrdoc_id," +
                                                        " employee_gid ," +
                                                        " hrdocument_gid," +
                                                        " hrdocument_name," +
                                                        " hrdoc_name ," +
                                                        " hrdoc_path," +
                                                        " migration_flag, " +
                                                        " created_by," +
                                                        " created_date" +
                                                        " )values(" +
                                                        "'" + msGetGid + "'," +
                                                        "'" + employeedtl.employee_gid + "'," +
                                                        "'" + getDocumentid.hrdocument_gid + "'," +
                                                        "'" + getDocumentid.hrdocument_name.Replace("'", @"\'") + "'," +
                                                        "'" + values.file_name + "'," +
                                                        "'" + values.file_path + "'," +
                                                        "'Y'," +
                                                        "'" + employee_gid + "'," +
                                                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        }
                                        else
                                        {
                                            LogForAudit("User Mapping Error - Code Not Found '" + values.employee_code + "'");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LogForAudit(values.employee_code + "-" + ex.ToString());
                                    }

                                }
                                LogForAudit("---------HR Document Mapping Details - Completed !--------------");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogForAudit(ex.ToString());
                        objResult.status = false;
                        objResult.message = ex.ToString();

                        return;
                    }
                }
                catch (Exception ex)
                {

                    LogForAudit(ex.ToString());
                    objResult.status = false;
                    objResult.message = ex.ToString();
                    return;
                }

                objResult.status = true;
                objResult.message = "Excel uploaded successfully";

            }

            catch (Exception ex)
            {
                objResult.status = false;
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void LogForAudit(string strVal)
        {
            try
            {
                
                string lspath = ConfigurationManager.AppSettings["file_path"].ToString() + "/erpdocument/HRMigrationLog/" + DateTime.Now.Year + @"\" + DateTime.Now.Month;
                if ((!System.IO.Directory.Exists(lspath)))
                    System.IO.Directory.CreateDirectory(lspath);

                lspath = lspath + @"\" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt";
                System.IO.StreamWriter sw = new System.IO.StreamWriter(lspath, true);
                sw.WriteLine(strVal);
                sw.Close();
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                "***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/HR/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
    }
}
    
