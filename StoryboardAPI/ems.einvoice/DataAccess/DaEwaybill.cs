﻿using System;
using System.Collections.Generic;
using System.Web;
using ems.einvoice.Models;
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
namespace ems.einvoice.DataAccess
{
    public class DaEwaybill
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        OdbcDataReader objMySqlDataReader;
        DataTable dt_datatable;
        int mnResult;
        public void Daewaybillsummary(MdlEwaybill values)
        {
            try
            {

                msSQL = " select ewaybill_gid, ewaybill_refno, invoice_refno, transporter_name, transport_mode, transport_doc_no, vehicle_no, vehicle_type, format(invoice_amount,2) as invoice_amount, created_date " +
                        " from rbl_trn_tewaybill order by ewaybill_gid desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ewaybillsummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ewaybillsummary_list
                        {
                            ewaybill_gid = dt["ewaybill_gid"].ToString(),
                            ewaybill_refno = dt["ewaybill_refno"].ToString(),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            transporter_name = dt["transporter_name"].ToString(),
                            transport_mode = dt["transport_mode"].ToString(),
                            transport_doc_no = dt["transport_doc_no"].ToString(),
                            vehicle_no = dt["vehicle_no"].ToString(),
                            vehicle_type = dt["vehicle_type"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            created_date = Convert.ToDateTime(dt["created_date"].ToString()),
                        });
                        values.ewaybillsummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while loading Eway bill details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        
        }
        public void Daewaybillinvoicesummary(MdlEwaybill values)
        {
            try
            {


                msSQL = " select distinct a.irn, a.created_date, a.invoice_gid, a.invoice_refno, a.customer_gid, a.irn, a.invoice_date, a.invoice_reference, " +
                    " a.mail_status, a.additionalcharges_amount, a.discount_amount, format(a.invoice_amount, 2) as invoice_amount, " +
                    " case when a.customer_contactnumber is null then concat(a.customer_contactperson,' / ',a.customer_contactnumber) else concat(a.customer_contactperson, " +
                    " if (a.customer_email = '',' ',concat(' / ', a.customer_email))) end as customer_contactperson, " +
                    " case when a.currency_code = 'INR' then a.customer_name when a.currency_code is null then a.customer_name " +
                    " when a.currency_code is not null and a.currency_code <> 'INR' then concat(a.customer_name) end as customer_name, " +
                    " a.currency_code, a.customer_contactnumber as mobile,a.invoice_from, " +
                    " case when irn is null then 'IRN Pending' when a.irncancel_date is not null then 'IRN Cancelled' " +
                    " when a.creditnote_status='Y' then 'Credit Note Raised' when a.irn is not null then 'IRN Generated' else 'Invoice Raised' end as invoice_status " +
                    " from rbl_trn_tinvoice a where invoice_amount >= '50000' and a.irn is not null and creditnote_status = 'N' " +
                    " group by a.invoice_refno order by a.created_date desc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<ewaybillinvoicesummary_list>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new ewaybillinvoicesummary_list
                        {
                            irn = dt["irn"].ToString(),
                            invoice_gid = dt["invoice_gid"].ToString(),
                            invoice_date = Convert.ToDateTime(dt["invoice_date"].ToString()),
                            invoice_refno = dt["invoice_refno"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_contactperson = dt["customer_contactperson"].ToString(),
                            invoice_reference = dt["invoice_reference"].ToString(),
                            invoice_from = dt["invoice_from"].ToString(),
                            mail_status = dt["mail_status"].ToString(),
                            invoice_amount = dt["invoice_amount"].ToString(),
                            invoice_status = dt["invoice_status"].ToString(),
                        });
                        values.ewaybillinvoicesummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while loading Eway bill invoice details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

           
        }
        public ewaybillinvoicedata_list DaewaybillInvoicedata(string invoice_gid)
        {
            try
            {

                ewaybillinvoicedata_list objewaybillinvoicedata_list = new ewaybillinvoicedata_list();
                {
                    msSQL = " select invoice_gid, irn, invoice_refno, invoice_amount from rbl_trn_tinvoice " +
                            " where invoice_gid='" + invoice_gid + "'";
                }

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);

                if (objMySqlDataReader.HasRows)
                {
                    objMySqlDataReader.Read();
                    objewaybillinvoicedata_list.invoice_gid = objMySqlDataReader["invoice_gid"].ToString();
                    objewaybillinvoicedata_list.irn = objMySqlDataReader["irn"].ToString();
                    objewaybillinvoicedata_list.invoice_refno = objMySqlDataReader["invoice_refno"].ToString();
                    objewaybillinvoicedata_list.invoice_amount = double.Parse(objMySqlDataReader["invoice_amount"].ToString());
                    objMySqlDataReader.Close();
                }
                return objewaybillinvoicedata_list;
            }

            catch (Exception ex)
            {
                ex.ToString();
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                return null;
            }

          
        }
        public void DaAddewaybill(string employee_gid, addewaybill_list values)
        {
            try
            {

                string msGetGid = objcmnfunctions.GetMasterGID("EWB");

                generateewaybill generateewaybill = new generateewaybill();

                generateewaybill.Irn = values.ewaybill_irn;
                generateewaybill.Distance = values.ewaybill_approximate_distance;
                generateewaybill.TransMode = values.ewaybill_transport_mode;
                generateewaybill.TransId = values.ewaybill_transporter_id;
                generateewaybill.TransName = values.ewaybill_transporter_name;
                generateewaybill.TransDocDt = (values.ewaybill_transporter_date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture); ;
                generateewaybill.TransDocNo = values.ewaybill_transporter_doc_no;
                generateewaybill.VehNo = values.ewaybill_vehicle_no;
                generateewaybill.VehType = values.ewaybill_vehicle_type;
                string ewaybilljson = JsonConvert.SerializeObject(generateewaybill);

                string lstoken;
                string lsaccess_token = "";
                string lsexpiry_date1 = "";

                msSQL = "select einvoice_token,einvoice_tokenexpiry from adm_mst_tcompany";

                objMySqlDataReader = objdbconn.GetDataReader(msSQL);
                if (objMySqlDataReader.HasRows == true)
                {
                    lstoken = objMySqlDataReader["einvoice_token"].ToString();
                    lsexpiry_date1 = objMySqlDataReader["einvoice_tokenexpiry"].ToString();

                    if (lstoken == "")
                    {
                        var auth = AuthCall();

                        msSQL = " update adm_mst_tcompany set einvoice_tokenexpiry=Now() + INTERVAL 28 DAY," +
                                " einvoice_token='" + auth.access_token + "'";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult == 1)
                        {
                            lsaccess_token = auth.access_token;
                        }
                    }
                    else
                    {
                        var lsexpiry_date = Convert.ToDateTime(lsexpiry_date1);

                        if (lsexpiry_date < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                        {
                            var auth = AuthCall();

                            msSQL = " update adm_mst_tcompany set einvoive_tokenexpiry=Now() + INTERVAL 28 DAY,einvoive_token='" + auth.access_token + "'";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            if (mnResult == 1)
                            {
                                lsaccess_token = auth.access_token;
                            }
                            else
                            {
                                lsaccess_token = lstoken;
                            }
                        }
                        else
                        {
                            lsaccess_token = lstoken;
                        }
                    }
                }

                var token = "bearer" + lsaccess_token;
                var bill = new ApiResponse();

                bill = GenerateEWayBill(token, ewaybilljson, values.ewaybill_invoice_gid);

                msSQL = " insert into rbl_trn_tewaybill ( " +
                        " ewaybill_gid," +
                        " ewaybill_refno," +
                        " invoice_gid," +
                        " irn," +
                        " invoice_refno," +
                        " invoice_amount," +
                        " transport_id," +
                        " transport_mode," +
                        " transport_doc_no, " +
                        " transport_doc_date, " +
                        " transporter_name, " +
                        " vehicle_no, " +
                        " vehicle_date, " +
                        " vehicle_type, " +
                        " distance, " +
                        " created_by, " +
                        " created_date " +
                        " ) values ( " +
                        "'" + msGetGid + "'," +
                        "'" + msGetGid + "'," +
                        "'" + values.ewaybill_invoice_gid + "'," +
                        "'" + values.ewaybill_irn + "'," +
                        "'" + values.ewaybill_invoice_ref_no + "'," +
                        "'" + values.ewaybill_invoice_amount + "'," +
                        "'" + values.ewaybill_transporter_id + "'," +
                        "'" + values.ewaybill_transport_mode + "'," +
                        "'" + values.ewaybill_transporter_doc_no + "'," +
                        "'" + values.ewaybill_transporter_date + "'," +
                        "'" + values.ewaybill_transporter_name + "'," +
                        "'" + values.ewaybill_vehicle_no + "'," +
                        "'" + values.ewaybill_vehicle_date + "'," +
                        "'" + values.ewaybill_vehicle_type + "'," +
                        "'" + values.ewaybill_approximate_distance + "'," +
                        "'" + employee_gid + "'," +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "E-way bill details added Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error while adding E-way bill details";
                }
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while loading Purchase Liability Report Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
"***********" + ex.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/rbl/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

          
        }        
        private AuthResponse AuthCall()
        {
            string uri = ConfigurationManager.AppSettings["einvoiceAutenticationURL"].ToString();

            Uri ourUri = new Uri(uri);
            WebRequest request = WebRequest.Create(uri);
            WebResponse response;
            request.Method = "POST";
            request.Headers.Add("gspappid", ConfigurationManager.AppSettings["gspappid"].ToString());
            request.Headers.Add("gspappsecret", ConfigurationManager.AppSettings["gspappsecret"].ToString());
            response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string streamText = reader.ReadToEnd();
            var json = JsonConvert.DeserializeObject<AuthResponse>(streamText);
            return json;
        }
        private ApiResponse GenerateEWayBill(string token, string ewaybilljson, string invoice_gid)
        {
            string msGetGID;
            string uri = ConfigurationManager.AppSettings["ewaybillGenerate"].ToString();
            Uri ourUri = new Uri(uri);
            string requestBody = ewaybilljson;
            msGetGID = objcmnfunctions.GetMasterGID("RQST");
            WebRequest request = WebRequest.Create(uri);
            WebResponse response;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("user_name", ConfigurationManager.AppSettings["einvoiceuser_name"].ToString());
            request.Headers.Add("password", ConfigurationManager.AppSettings["einvoicepwd"].ToString());
            request.Headers.Add("requestid", msGetGID);
            request.Headers.Add("gstin", ConfigurationManager.AppSettings["einvoicegstin"].ToString());
            request.Headers.Add("Authorization", token);

            byte[] requestBodyBytes = Encoding.UTF8.GetBytes(requestBody);
            request.ContentLength = requestBodyBytes.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(requestBodyBytes, 0, requestBodyBytes.Length);
            }

            response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string streamText = reader.ReadToEnd();

            msSQL = " insert into rblt_trn_teinvoice_responselog (" +
                    " request_gid, " +
                    " reponse_json, " +
                    " request_json, " +
                    " invoice_gid" +
                    " ) values ( " +
                    "'" + msGetGID + "'," +
                    "'" + streamText + "'," +
                    "'" + ewaybilljson + "'," +
                    "'" + invoice_gid + "')";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            ApiResponse irnresponse = new ApiResponse();
            try
            {
                var json = new ResponseData();

                json = JsonConvert.DeserializeObject<ResponseData>(streamText);
                if (json.success == true)
                {
                    msSQL = " update rblt_trn_teinvoice_responselog set " +
                            " success ='" + json.success + "', " +
                            " message ='" + json.message.ToString().Replace("'", "") + "', " +
                            " EwbDt ='" + Convert.ToDateTime(json.result.EwbDt).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                            " EwbValidTill ='" + Convert.ToDateTime(json.result.EwbValidTill).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                            " EwbNo ='" + json.result.Irn + "',  " +
                            " SignedInvoice ='" + json.result.SignedInvoice + "',  " +
                            " SignedQRCode ='" + json.result.SignedQRCode + "',  " +
                            " Status ='" + json.result.Status + "',  " +
                            " Remarks ='" + json.result.Remarks + "' " +
                            " where request_gid ='" + msGetGID + "' ";
                    
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult == 1)
                    {
                        msSQL = " update rbl_trn_tinvoice set " +
                                " EwbNo ='" + json.result.EwbNo + "' , " +
                                " EwbValidTill='" + Convert.ToDateTime(json.result.EwbValidTill).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                " EwbDt='" + Convert.ToDateTime(json.result.EwbDt).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                " where invoice_gid ='" + invoice_gid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        irnresponse.request_id = msGetGID;
                        irnresponse.qr_code = json.result.SignedQRCode;
                        irnresponse.success = true;
                    }
                    else
                        irnresponse.success = false;
                }
                else
                {
                    irnresponse.success = false;
                    msSQL = " update rblt_trn_teinvoice_responselog set " +
                            " success ='" + json.success + "', " +
                            " message ='" + json.message.ToString().Replace("'", "") + "' " +
                            " where request_gid ='" + msGetGID + "' ";
                    
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = " select message from rblt_trn_teinvoice_responselog where request_gid ='" + msGetGID + "' ";

                    irnresponse.errorMessage = objdbconn.GetExecuteScalar(msSQL);
                }
            }

            catch (Exception ex)
            {
                irnresponse.success = false;
                
                msSQL = " update rblt_trn_teinvoice_responselog set " +
                        " message ='Exception occured while generating EwayBill - " + ex.ToString().Replace("'", "") + "' " +
                        " where request_gid ='" + msGetGID + "' ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            }
            return irnresponse;
        }
    }
}