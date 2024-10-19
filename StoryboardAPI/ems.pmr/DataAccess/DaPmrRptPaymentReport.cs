using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.pmr.Models;
using System.Globalization;

namespace ems.pmr.DataAccess
{
    public class DaPmrRptPaymentReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        DataTable dt_datatable1;
        string msGetGid, currency_code;
        int mnResult, mnResult1;

        public void DaGetPaymentReportforSixMonths(MdlPmrRptPaymentReport values)
        {
            try
            {
                msSQL = "  select DATE_FORMAT(a.payment_date, '%b-%Y')  as payment_date," +
                    " substring(date_format(a.payment_date,'%M'),1,3)as month,a.payment_gid," +
                    " year(a.payment_date) as year,   round(sum(a.payment_total),2)as amount," +
                    " count(a.payment_gid)as ordercount from acp_trn_tpayment a" +
                    " left join acp_trn_tinvoice2payment b on a.payment_gid=b.payment_gid" +
                    " left join acp_trn_tinvoice c on b.invoice_gid=c.invoice_gid" +
                    " where a.payment_date > date_add(now(), interval-5 month) and" +
                    " a.payment_date<=date(now()) and c.invoice_flag" +
                    " not in('Invoice Cancelled','Rejected') and" +
                    " c.invoice_status not in('Invoice Cancelled','Rejected')" +
                    " group by date_format(a.payment_date,'%M') order by a.payment_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var GetPaymentOrderForLastSixMonths_List = new List<GetPaymentOrderForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetPaymentOrderForLastSixMonths_List.Add(new GetPaymentOrderForLastSixMonths_List
                        {
                            payment_date = (dt["payment_date"].ToString()),
                            month_na = (dt["month"].ToString()),
                            year = (dt["year"].ToString()),
                            payment_amount = (dt["amount"].ToString()),
                            payment_count = (dt["ordercount"].ToString()),
                            payment_gid = (dt["payment_gid"].ToString()),
                        });
                        values.GetPaymentOrderForLastSixMonths_List = GetPaymentOrderForLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        public void DaGetPaymentReportDetailSummary(string employee_gid, string month, string year, string from_date, string to_date, MdlPmrRptPaymentReport values)
        {
            try
            {
                if (from_date == null || from_date == "" || from_date == "undefined" || to_date == null || to_date == "" || to_date == "undefined")
                {

                    msSQL = " select round(sum((a.payment_total) * a.exchange_rate),2) as payment_amount,m.vendor_companyname,m.contactperson_name," +
                        "year(a.payment_date)as year,  concat(a.payment_mode,'/',m.vendor_companyname,'/',m.contactperson_name)as payment_mode," +
                        " a.tds_amount,  a.payment_date,substring(date_format(a.payment_date,'%M'),1,3)as month," +
                        " a.payment_status, a.payment_gid,count(a.payment_gid)as payment_count from acp_trn_tpayment a" +
                        " left join acp_trn_tinvoice2payment b on a.payment_gid=b.payment_gid" +
                        " left join hrm_mst_temployee c on c.employee_gid= a.created_by" +
                        " left join adm_mst_tuser d on d.user_gid=c.user_gid" +
                        " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code" +
                        " left join acp_mst_tvendor m on a.vendor_gid=m.vendor_gid" +
                        " left join acp_trn_tinvoice n on b.invoice_gid=n.invoice_gid" +
                        " where substring(date_format(a.payment_date,'%M'),1,3)='" + month + "' and year(a.payment_date)='" + year + "'" +
                        " and n.invoice_status not in('Invoice Cancelled','Rejected') and n.invoice_flag not in('Invoice Cancelled','Rejected')" +
                        " group by a.payment_gid order by a.payment_date desc";

                }
                else
                {
                    //string uiDateStr = from_date;
                    //DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    //string fromdate = uiDate.ToString("yyyy-MM-dd");

                    //string uiDateStr1 = to_date;
                    //DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    //string todate = uiDate1.ToString("yyyy-MM-dd");

                    msSQL = " select round(sum((a.payment_total) * a.exchange_rate),2) as payment_amount,m.vendor_companyname,m.contactperson_name," +
                        " year(a.payment_date)as year,  concat(a.payment_mode,'/',m.vendor_companyname,'/',m.contactperson_name)as payment_mode," +
                        " a.tds_amount,  a.payment_date,substring(date_format(a.payment_date,'%M'),1,3)as month," +
                        " a.payment_status, a.payment_gid,count(a.payment_gid)as payment_count from acp_trn_tpayment a" +
                        " left join acp_trn_tinvoice2payment b on a.payment_gid=b.payment_gid" +
                        " left join hrm_mst_temployee c on c.employee_gid= a.created_by" +
                        " left join adm_mst_tuser d on d.user_gid=c.user_gid" +
                        " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code" +
                        " left join acp_mst_tvendor m on a.vendor_gid=m.vendor_gid" +
                        " left join acp_trn_tinvoice n on b.invoice_gid=n.invoice_gid" +
                        " where substring(date_format(a.payment_date,'%M'),1,3)='" + month + "' and year(a.payment_date)='" + year + "'" +
                        " and n.invoice_status not in('Invoice Cancelled','Rejected') and n.invoice_flag not in('Invoice Cancelled','Rejected')" +
                        " and a.payment_date >= DATE_FORMAT(CURDATE(), '%Y-%m-01') - INTERVAL 6 MONTH and a.payment_date >='" + from_date + "' and a.payment_date<='" + to_date + "'" +
                        " group by a.payment_gid order by a.payment_date desc";
                }

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetPaymentDetailSummary = new List<GetPaymentDetailSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetPaymentDetailSummary.Add(new GetPaymentDetailSummary
                        {
                            payment_date = (dt["payment_date"].ToString()),
                            vendor_companyname = (dt["vendor_companyname"].ToString()),
                            payment_status = (dt["payment_status"].ToString()),
                            contactperson_name = (dt["contactperson_name"].ToString()),
                            payment_amount = (dt["payment_amount"].ToString()),
                            payment_gid = (dt["payment_gid"].ToString()),
                            payment_mode = (dt["payment_mode"].ToString()),
                            tds_amount = (dt["tds_amount"].ToString()),
                        });
                        values.GetPaymentDetailSummary = GetPaymentDetailSummary;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Order Detail Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetPaymentReportForLastSixMonthsSearch(MdlPmrRptPaymentReport values, string from_date, string to_date)
        {
            try
            {
                if (from_date == null && to_date == null)
                {
                    msSQL = "  select DATE_FORMAT(a.payment_date, '%b-%Y')  as payment_date," +
                    " substring(date_format(a.payment_date,'%M'),1,3)as month,a.payment_gid," +
                    " year(a.payment_date) as year,   round(sum(a.payment_total),2)as amount," +
                    " count(a.payment_gid)as ordercount from acp_trn_tpayment a" +
                    " left join acp_trn_tinvoice2payment b on a.payment_gid=b.payment_gid" +
                    " left join acp_trn_tinvoice c on b.invoice_gid=c.invoice_gid" +
                    " where a.payment_date > date_add(now(), interval-5 month) and" +
                    " a.payment_date<=date(now()) and c.invoice_flag" +
                    " not in('Invoice Cancelled','Rejected') and" +
                    " c.invoice_status not in('Invoice Cancelled','Rejected')" +
                    " group by date_format(a.payment_date,'%M') order by a.payment_date desc";
                }
                
                else
                {
                    msSQL ="  select DATE_FORMAT(a.payment_date, '%b-%Y')  as payment_date," +
                    " substring(date_format(a.payment_date,'%M'),1,3)as month,a.payment_gid," +
                    " year(a.payment_date) as year,   round(sum(a.payment_total),2)as amount," +
                    " count(a.payment_gid)as ordercount from acp_trn_tpayment a" +
                    " left join acp_trn_tinvoice2payment b on a.payment_gid=b.payment_gid" +
                    " left join acp_trn_tinvoice c on b.invoice_gid=c.invoice_gid" +
                    " where a.payment_date between '" + from_date + "' and '" + to_date + "' " +
                    " and c.invoice_flag not in('Invoice Cancelled','Rejected') and" +
                    " c.invoice_status not in('Invoice Cancelled','Rejected')" +
                    " group by date_format(a.payment_date,'%M') order by a.payment_date desc";
                }
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetPaymentForLastSixMonths_List = new List<GetPaymentForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetPaymentForLastSixMonths_List.Add(new GetPaymentForLastSixMonths_List
                        {
                            payment_date = (dt["payment_date"].ToString()),
                            month_na = (dt["month"].ToString()),
                            year = (dt["year"].ToString()),
                            payment_amount = (dt["amount"].ToString()),
                            payment_count = (dt["ordercount"].ToString()),
                            payment_gid = (dt["payment_gid"].ToString()),
                        });
                        values.GetPaymentForLastSixMonths_List = GetPaymentForLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Specific Date Data !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }


    }
}