using ems.pmr.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using static System.Net.Mime.MediaTypeNames;
using System.Web.Http.Results;
using static ems.pmr.Models.addgrn_lists;
using System.Web.UI.WebControls;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using System.Data.SqlClient;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.pmr.DataAccess
{
    public class DaPmrDashboard
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string lsexchange_to_currency, lsexchange_from_currency, lsapi_url, lsapi_key, lsapi_host, msGetGid, lscreated_date, lscurrency_code;
        int mnResult;
        string currencyCode, lscountry, lsdefault_currency, lscountry_gid;

        public void DaGetPurchaseLiabilityReportChart(MdlPmrDashboard values)
        {
            try
            {

                msSQL = "SELECT Months, SUM(purchase_count) AS purchase_count, SUM(invoice_count) AS invoice_count, " +
                    "SUM(payment_count) AS payment_count FROM (SELECT DATE_FORMAT(created_date, '%b') AS Months," +
                    " COUNT(purchaseorder_gid) AS purchase_count, 0 AS invoice_count, 0 AS payment_count,created_date AS ordermonth" +
                    " FROM pmr_trn_tpurchaseorder WHERE created_date >= CURDATE() - INTERVAL 5 MONTH GROUP BY DATE_FORMAT(created_date, '%b')UNION ALL " +
                    "SELECT DATE_FORMAT(created_date, '%b') AS Months, 0 AS purchase_count, COUNT(invoice_gid) AS invoice_count, 0 AS payment_count," +
                    "created_date AS ordermonth  FROM acp_trn_tinvoice WHERE created_date >= CURDATE() - INTERVAL 5 MONTH" +
                    " GROUP BY DATE_FORMAT(created_date, '%b') UNION ALL SELECT DATE_FORMAT(created_date, '%b') AS Months," +
                    " 0 AS purchase_count, 0 AS invoice_count, COUNT(payment_gid) AS payment_count,created_date AS ordermonth  " +
                    "FROM acp_trn_tpayment WHERE created_date >= CURDATE() - INTERVAL 5 MONTH GROUP BY DATE_FORMAT(created_date, '%b')) AS combined" +
                    " GROUP BY Months  order by ordermonth;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getpurchasechart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getpurchasechart_list
                        {

                            Months = dt["Months"].ToString(),
                            purchase_count = dt["purchase_count"].ToString(),
                            invoice_count = dt["invoice_count"].ToString(),
                            payment_count = dt["payment_count"].ToString(),

                        });
                        values.Getpurchasechart_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading the Purchase Liability Report Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        // GetPurchaseCount
        public void DaGetPurchaseCount(MdlPmrDashboard values)
        {
            try
            {

                msSQL = "select( select count(purchaseorder_gid) from pmr_trn_tpurchaseorder) as pototalcount," +
                    "(select count(vendor_gid) from acp_mst_tvendor) as total_vendor,(select sum(invoice_amount) from acp_trn_tinvoice" +
                    " where  MONTH(created_date) = MONTH(CURDATE())AND  YEAR(created_date) = YEAR(CURDATE())) as month_invoiceamount,(select count(invoice_gid) from acp_trn_tinvoice where  MONTH(created_date) = MONTH(CURDATE())AND  YEAR(created_date) = YEAR(CURDATE())) as month_invoicecount," +
                    "(select DATE_FORMAT(CURDATE(), '%M')) as mtd_invoice,(select sum(invoice_amount) from acp_trn_tinvoice" +
                    " where YEAR(created_date) = YEAR(CURDATE())) as ytd_invoiceamount,(select DATE_FORMAT(CURDATE(), '%M')) as mtd_invoice," +
                    "(select count(invoice_gid) from acp_trn_tinvoice where YEAR(created_date) = YEAR(CURDATE())) as ytd_invoicecount," +
                    "(select b.symbol from adm_mst_tcompany a left join crm_trn_tcurrencyexchange b on a.currency_flag=b.currency_code) as currency_symbol," +
                    "(select DATE_FORMAT(CURDATE(), '%Y')) as ytd_invoice;";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getpmrdashboard_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getpmrdashboard_list
                        {

                            pototalcount = dt["pototalcount"].ToString(),
                            total_vendor = dt["total_vendor"].ToString(),
                            month_invoiceamount = dt["month_invoiceamount"].ToString(),
                            mtd_invoice = dt["mtd_invoice"].ToString(),
                            month_invoicecount = dt["month_invoicecount"].ToString(),
                            ytd_invoiceamount = dt["ytd_invoiceamount"].ToString(),
                            ytd_invoice = dt["ytd_invoice"].ToString(),
                            ytd_invoicecount = dt["ytd_invoicecount"].ToString(),
                            currency_symbol = dt["currency_symbol"].ToString(),

                        });
                        values.Getpmrdashboard_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting the puchase count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        //GetInvoiceCount

        public void DaGetInvoiceCount(MdlPmrDashboard values)
        {
            try
            {

                msSQL = "select(select count(invoice_gid) from acp_trn_tinvoice where invoice_status = 'IV Approved'or invoice_status = 'Invoice Approved') as approved_invoice, " +
                    "(select count(invoice_gid)as cancel_invoice  from acp_trn_tinvoice where invoice_status='IV Canceled') as cancel_invoice ," +
                    " (select count(invoice_gid) as pending_invoice  from acp_trn_tinvoice where invoice_status = 'IV Work In Progress') as pending_invoice, " +
                    " (select count(invoice_gid) as completed_invoice  from acp_trn_tinvoice where invoice_status = 'IV Completed') as completed_invoice ;";
               
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getinvoicechart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getinvoicechart_list
                        {

                            approved_invoice = dt["approved_invoice"].ToString(),
                            cancel_invoice = dt["cancel_invoice"].ToString(),
                            pending_invoice = dt["pending_invoice"].ToString(),
                            completed_invoice = dt["completed_invoice"].ToString(),

                        });
                        values.Getinvoicechart_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting the invoice count!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        //GetPaymentCount

        public void DaGetPaymentCount(MdlPmrDashboard values)
        {
            try
            {

                msSQL = "select(select count(payment_status) as cancelled_payment  from acp_trn_tpayment where payment_status = 'Payment Canceled'" +
                    " or payment_status = 'PY Canceled') as cancelled_payment ,  (select count(payment_status) as pending_payment  " +
                    "from acp_trn_tpayment where payment_status = 'PY Approved') as approved_payment,(select count(payment_status) as completed_payment " +
                    " from acp_trn_tpayment where payment_status = 'Payment Done') as completed_payment  ; ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getpaymentchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getpaymentchart_list
                        {

                            cancelled_payment = dt["cancelled_payment"].ToString(),
                            approved_payment = dt["approved_payment"].ToString(),
                            completed_payment = dt["completed_payment"].ToString(),

                        });
                        values.Getpaymentchart_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting the PaymentCount!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
         public void DaGetPurchasetCount(MdlPmrDashboard values)
        {
            try
            {

                msSQL = "select date_format(created_date, '%b') as months,sum(case when purchaseorder_status='PO Approved' then 1 else 0 end) as po_approved," +
                    "sum(case when purchaseorder_status='PO Completed' then 1 else 0 end) as po_completed," +
                    "sum(case when purchaseorder_status='PO Pending' then 1 else 0 end) as po_pending," +
                    "sum(case when purchaseorder_status='PO Canceled' then 1 else 0 end) as po_cancelled from pmr_trn_tpurchaseorder" +
                    " where created_date >= curdate()- interval 5 month group by months order by created_date ;";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getpurchaseorderchart_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getpurchaseorderchart_list
                        {

                            po_approved = dt["po_approved"].ToString(),
                            po_completed = dt["po_completed"].ToString(),
                            po_pending = dt["po_pending"].ToString(),
                            po_cancelled = dt["po_cancelled"].ToString(),

                        });
                        values.Getpurchaseorderchart_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting the PaymentCount!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }

        public void DaGetExchangeRateAPISummary(MdlPmrDashboard values)
        {
            try
            {

                msSQL = "select currencyexchangeapihistory_gid, updated_date, currency_code, " +
                    " currency_symbol, exchange_rate, country, default_currency, country_gid, updated_by, created_by, DATE_FORMAT(created_date, '%d-%m-%Y') AS created_date " +
                    "  from crm_trn_tcurrencyexchangeapihistory order by created_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetExchangeRateAPI_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetExchangeRateAPI_List
                        {

                            currencyexchangeapihistory_gid = dt["currencyexchangeapihistory_gid"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            updated_date = dt["updated_date"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),


                        });
                        values.GetExchangeRateAPI_List = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading the Purchase Liability Report Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaSmrSalesExchangeRateUpdate(string user_gid, GetExchangeRateAPICredential_List values)
        {
            try
            {

                msSQL = " SELECT api_url,api_key,api_host FROM crm_smm_exchangerate_service where s_no='1' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    msSQL = " update  crm_smm_exchangerate_service set " +
                 " api_url = '" + values.api_url + "'," +
                 " api_key = '" + values.api_key + "'," +
                 " api_host = '" + values.api_host + "'," +
                 " updated_by = '" + user_gid + "'," +
                 " updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where s_no='1'  ";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Currency Exchange Rate Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Updating  Exchange Rate";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updatuin Currency !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
               $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
               msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetExchangeRateAPICredential(MdlPmrDashboard values)
        {
            try
            {

                msSQL = " SELECT api_url,api_key,api_host FROM crm_smm_exchangerate_service limit 1 ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetExchangeRateAPICredential_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetExchangeRateAPICredential_List
                        {

                            api_url = dt["api_url"].ToString(),
                            api_key = dt["api_key"].ToString(),
                            api_host = dt["api_host"].ToString(),


                        });
                        values.GetExchangeRateAPICredential_List = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading the Purchase Liability Report Chart!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public MdlPmrDashboard DaGetExchangeRateAsync(string user_gid)
        {
            MdlPmrDashboard objresult = new MdlPmrDashboard();
            ExchangeRate_List objexchangerate = new ExchangeRate_List();
            Rates rates = new Rates();

            // lsexchange_to_currency = exchange_to_currency;
            lsexchange_to_currency = "USD";
            msSQL = " select currency_code from crm_trn_tcurrencyexchange where default_currency = 'Y' limit 1";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                lsexchange_from_currency = objOdbcDataReader["currency_code"].ToString();
            }
            msSQL = " SELECT api_url,api_key,api_host FROM crm_smm_exchangerate_service limit 1 ";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                lsapi_url = objOdbcDataReader["api_url"].ToString();
                lsapi_key = objOdbcDataReader["api_key"].ToString();
                lsapi_host = objOdbcDataReader["api_host"].ToString();

            }
            objOdbcDataReader.Close();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            msSQL = " select created_date from crm_trn_tcurrencyexchangeapihistory where created_date like '%" + DateTime.Now.ToString("yyyy-MM-dd") + "%' ";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows == true)
            {
                lscreated_date = objOdbcDataReader["created_date"].ToString();
                return objresult;
            }

            else
            {
                var client = new RestClient("https://" + lsapi_url + "" + lsexchange_to_currency + "");
                var request = new RestRequest(Method.GET);
                request.AddHeader("X-RapidAPI-Key", "" + lsapi_key + "");
                request.AddHeader("X-RapidAPI-Host", "" + lsapi_host + "");
                IRestResponse response = client.Execute(request);
                string response_content = response.Content;

                //var client = new RestClient("https://exchangerate-api.p.rapidapi.com/rapid/latest/USD");
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("X-RapidAPI-Key", "14da834b12msh710e57f3bd88c1fp19cc93jsnba653ed40922");
                //request.AddHeader("X-RapidAPI-Host", "exchangerate-api.p.rapidapi.com");
                //IRestResponse response = client.Execute(request);
                //string response_content = response.Content;  

                ExchangeRate_List objMdlExchangeRateMessageResponse = JsonConvert.DeserializeObject<ExchangeRate_List>(response_content);
                float usdRate = objMdlExchangeRateMessageResponse.rates.USD;
                float aedRate = objMdlExchangeRateMessageResponse.rates.AED;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    foreach (var property in typeof(Rates).GetProperties())
                    {
                        string currencyCode = property.Name;
                        string exchangeRateString = property.GetValue(objMdlExchangeRateMessageResponse.rates).ToString();

                        if (float.TryParse(exchangeRateString, out float exchangeRate))
                        {
                            msGetGid = objcmnfunctions.GetMasterGID("CURH");
                            msSQL = " insert into crm_trn_tcurrencyexchangeapihistory(" +
                                " currencyexchangeapihistory_gid," +
                                " currency_code," +
                                " exchange_rate," +
                                " created_by, " +
                                " created_date)" +
                                " values(" +
                                " '" + msGetGid + "'," +
                                " '" + currencyCode + "'," +
                                " '" + exchangeRateString + "',";
                            msSQL += "'" + user_gid + "'," +
                                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                        // %%%% Exchange rate integration query with currency master - DONT DELETE %%%%

                        //msSQL = "SELECT currency_code,country,country_gid,default_currency FROM crm_trn_tcurrencyexchange WHERE currency_code = '" + currencyCode + "' ";
                        //objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        //if (objOdbcDataReader.HasRows == true)
                        //{

                        //    lscurrency_code = objOdbcDataReader["currency_code"].ToString();
                        //    lscountry = objOdbcDataReader["country"].ToString();
                        //    lsdefault_currency = objOdbcDataReader["default_currency"].ToString();
                        //    lscountry_gid = objOdbcDataReader["country_gid"].ToString();

                        //    dt_datatable = objdbconn.GetDataTable(msSQL);

                        //    if (dt_datatable.Rows.Count != 0)
                        //    {
                        //        foreach (DataRow dt in dt_datatable.Rows)
                        //        {
                        //            string msGetGid = objcmnfunctions.GetMasterGID("CUR");
                        //            msSQL = " insert into crm_trn_tcurrencyexchange(" +
                        //                    " currencyexchange_gid," +
                        //                    " currency_code," +
                        //                    " exchange_rate," +
                        //                    " country," +
                        //                    " default_currency," +
                        //                    " country_gid," +
                        //                    " created_by, " +
                        //                    " created_date)" +
                        //                    " values(" +
                        //                    " '" + msGetGid + "'," +
                        //                    " '" + lscurrency_code + "'," +
                        //                    " '" + exchangeRateString + "'," +
                        //                    " '" + lscountry + "'," +
                        //                    " '" + lsdefault_currency + "'," +
                        //                    " '" + lscountry_gid + "'," +
                        //                    "'" + user_gid + "'," +
                        //                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        //            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        //        }
                        //    }
                        //}
                    }

                }
                if (mnResult != 0)
                {
                    objresult.status = true;
                    objresult.message = "Currency Exchange History Saved Successfully";
                }
                else
                {
                    objresult.status = false;
                    objresult.message = "Error While Adding Currency";
                }
            }
            return objresult;

        }
    }
}