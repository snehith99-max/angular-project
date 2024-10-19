using ems.pmr.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Runtime.Remoting;
using System.Drawing;
using System.Globalization;

namespace ems.pmr.DataAccess
{
    public class DaPmrRptPurchaseOrder
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        DataTable dt_datatable1;
        string msGetGid, currency_code;
        int mnResult, mnResult1;
        // GetOrderForLastSixMonths
        public void DaGetPurchaseOrderReportForLastSixMonth(MdlPmrRptPurchaseOrder values)
        {
            try
            {
                msSQL = "select DATE_FORMAT(purchaseorder_date, '%b-%Y')  as purchaseorder_date,substring(date_format(a.purchaseorder_date,'%M'),1,3)as month,a.purchaseorder_gid,year(a.purchaseorder_date) as year,  " +
                    " format(round(sum(a.total_amount),2),2) as amount,count(a.purchaseorder_gid)as ordercount      from pmr_trn_tpurchaseorder a   " +
                    " where a.purchaseorder_date > date_add(now(), interval-6 month) and a.purchaseorder_date<=date(now())    " +
                    " and a.purchaseorder_flag not in('PO Amended','PO Canceled','Po Rejected') group by date_format(a.purchaseorder_date,'%M') order by a.purchaseorder_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetPurchaseOrderForLastSixMonths_List = new List<GetPurchaseOrderForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetPurchaseOrderForLastSixMonths_List.Add(new GetPurchaseOrderForLastSixMonths_List
                        {
                            purchaseorder_date = (dt["purchaseorder_date"].ToString()),
                            month = (dt["month"].ToString()),
                            year = (dt["year"].ToString()),
                            amount = (dt["amount"].ToString()),
                            ordercount = (dt["ordercount"].ToString()),
                        });
                        values.GetPurchaseOrderForLastSixMonths_List = GetPurchaseOrderForLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Order Report For Last Six Months !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        // GetOrderDetailSummary
        public void DaGetPurchaseOrderDetailSummary(string employee_gid, string month, string year, string from_date, string to_date, MdlPmrRptPurchaseOrder values)
        {
            try
            {
                if (from_date == null || from_date == "" || from_date == "undefined" || to_date == null || to_date == "" || to_date == "undefined")
                {

                    msSQL = " select a.purchaseorder_gid,date_format(a.purchaseorder_date,'%d-%m-%Y')as purchaseorder_date,e.vendor_companyname,CASE WHEN a.poref_no IS NULL OR a.poref_no = '' THEN a.purchaseorder_gid ELSE a.poref_no END AS porefno, " +
                        " concat(e.contactperson_name,'/',e.contact_telephonenumber,'/',e.email_id)as contact_details  ,a.purchase_type,format(a.total_amount, 2) as grandtotal," +
                        " concat(b.user_firstname,' ',b.user_lastname)as purchaseperson_name,a.purchaseorder_status  from pmr_trn_tpurchaseorder a " +
                        " left join adm_mst_tuser b on b.user_gid=a.created_by left join acp_mst_tvendor e on e.vendor_gid=a.vendor_gid " +
                        " where substring(date_format(a.purchaseorder_date,'%M'),1,3)='" + month + "' and year(a.purchaseorder_date)='" + year + "'  " +
                        " and a.purchaseorder_flag not in('PO Amended','PO Canceled','PO Rejected')  order by a.purchaseorder_date desc ";

                }
                else
                {
                    string uiDateStr = from_date;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string fromdate = uiDate.ToString("yyyy-MM-dd");

                    string uiDateStr1 = to_date;
                    DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string todate = uiDate1.ToString("yyyy-MM-dd");

                    msSQL = " select a.purchaseorder_gid,date_format(a.purchaseorder_date,'%d-%m-%Y')as purchaseorder_date,e.vendor_companyname,CASE WHEN a.poref_no IS NULL OR a.poref_no = '' THEN a.purchaseorder_gid ELSE a.poref_no END AS porefno, " +
                        " concat(e.contactperson_name,'/',e.contact_telephonenumber,'/',e.email_id)as contact_details  ,a.purchase_type,format(a.total_amount, 2) as grandtotal," +
                        " concat(b.user_firstname,' ',b.user_lastname)as purchaseperson_name,a.purchaseorder_status  from pmr_trn_tpurchaseorder a " +
                        " left join adm_mst_tuser b on b.user_gid=a.created_by left join acp_mst_tvendor e on e.vendor_gid=a.vendor_gid " +
                        " where substring(date_format(a.purchaseorder_date,'%M'),1,3)='" + month + "' and year(a.purchaseorder_date)='" + year + "'  " +
                        " and a.purchaseorder_date between '" + fromdate + "' and '" + todate + "'" +
                        " and a.purchaseorder_flag not in('PO Amended','PO Canceled','PO Rejected')  order by a.purchaseorder_date desc ";
                 
             
                }

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetOrderDetailSummary = new List<GetOrderDetailSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetOrderDetailSummary.Add(new GetOrderDetailSummary
                        {
                            purchaseorder_date = (dt["purchaseorder_date"].ToString()),
                            porefno = (dt["porefno"].ToString()),
                            vendor_companyname = (dt["vendor_companyname"].ToString()),
                            contact_details = (dt["contact_details"].ToString()),
                            purchaseorder_status = (dt["purchaseorder_status"].ToString()),
                            purchaseperson_name = (dt["purchaseperson_name"].ToString()),
                            grandtotal_l = (dt["grandtotal"].ToString()),
                            purchaseorder_gid = (dt["purchaseorder_gid"].ToString()),
                        });
                        values.GetOrderDetailSummary = GetOrderDetailSummary;
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


        public void DaGetIndividualreport(MdlPmrRptPurchaseOrder values, string purchaseorder_gid)
        {
            try
            {
                msSQL = " select a.purchaseorder_gid,b.invoice_reference,a.purchaseorder_date,a.poref_no,e.vendor_companyname," +
                    " concat(e.contactperson_name,'/',a.vendor_address,'/',e.contact_telephonenumber,'/',e.email_id)as vendor_details," +
                    "  b.invoice_date,format((a.total_amount),2) as grand_total, format((b.initialinvoice_amount),2) as advance_amount," +
                    " a.purchaseorder_status,a.purchase_type, format((b.total_amount_L),2) as invoice_amount, format((((a.total_amount)-(b.total_amount_L))),2) as pending_invoice_amount," +
                    " c.branch_name from pmr_trn_tpurchaseorder a left join acp_trn_tinvoice b on  a.purchaseorder_gid=b.purchaseorder_gid left join hrm_mst_tbranch c on  a.branch_gid=c.branch_gid" +
                    " left join acp_mst_tvendor e on  e.vendor_gid=a.vendor_gid" +
                    " where a.purchaseorder_gid='"+ purchaseorder_gid + "' order by a.purchaseorder_date desc ;";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var individualreport_list = new List<individualreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        individualreport_list.Add(new individualreport_list
                        {
                            invoice_refno = (dt["invoice_reference"].ToString()),
                            purchaseorder_date = (dt["purchaseorder_date"].ToString()),
                            poref_no = (dt["poref_no"].ToString()),
                            vendor_companyname = (dt["vendor_companyname"].ToString()),
                            vendor_details = (dt["vendor_details"].ToString()),
                            branch_name = (dt["branch_name"].ToString()),
                            purchaseorder_status = (dt["purchaseorder_status"].ToString()),
                            purchase_type = (dt["purchase_type"].ToString()),
                            invoice_date = (dt["invoice_date"].ToString()),
                            grand_total = (dt["grand_total"].ToString()),
                            advance_amount = (dt["advance_amount"].ToString()),
                            invoice_amount = (dt["invoice_amount"].ToString()),
                            pending_invoice_amount = (dt["pending_invoice_amount"].ToString()),
                        });
                        values.individualreport_list = individualreport_list;
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

        public void DaGetPurchaseOrderSummary(string employee_gid, string purchaseorder_gid, MdlPmrRptPurchaseOrder values)
        {
            try
            {

                msSQL = "  select substring(date_format(a.purchaseorder_date,'%M'),1,3)as month,year(a.purchaseorder_date) as year," +
                        " round(sum(a.total_amount), 2) as amount,format(sum(a.total_amount), 2) as orderamount,count(a.purchaseorder_gid) as ordercount from pmr_trn_tpurchaseorder a " +
                        " where a.purchaseorder_date > date_add(now(), interval - 6 month) and a.purchaseorder_date <= date(now()) and a.purchaseorder_status not in('PO Amended', 'PO Canceled', 'PO Rejected')" +
                        " group by date_format(a.purchaseorder_date, '%M') order by a.purchaseorder_date desc";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetPurchaseOrderForLastSixMonths_List = new List<GetPurchaseOrderForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetPurchaseOrderForLastSixMonths_List.Add(new GetPurchaseOrderForLastSixMonths_List
                        {
                            month = (dt["month"].ToString()),
                            year = (dt["year"].ToString()),
                            amount = (dt["amount"].ToString()),
                            orderamount = (dt["orderamount"].ToString()),
                            ordercount = (dt["ordercount"].ToString()),
                        });
                        values.GetPurchaseOrderForLastSixMonths_List = GetPurchaseOrderForLastSixMonths_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetMonthwiseOrderReport(string employee_gid, MdlPmrRptPurchaseOrder values)
        {
            try
            {


                msSQL = " select distinct date_format(purchaseorder_date,'%M/%Y') as month_wise,date_format(purchaseorder_date,'%Y') as year from pmr_trn_tpurchaseorder  group by purchaseorder_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var MonthwiseOrderReport_List = new List<GetMonthwiseOrderReport_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        msSQL = " select count(purchaseorder_gid) as so_total,format(ifnull(sum(total_amount*exchange_rate),0),2) as total  from " +
                                " pmr_trn_tpurchaseorder where date_format(purchaseorder_date, '%M/%Y') = '" + dt[0].ToString() + "' and purchaseorder_status " +
                                " not in ('PO Pending', 'PO Amended', 'PO Canceled') ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        msSQL = "  select format(sum(total_amount*exchange_rate),2) as total_invoice from acp_trn_tinvoice  where date_format(invoice_date,'%M/%Y')= '" + dt[0].ToString() + "' ";
                        values.total_invoice = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "    select format(sum(payment_total * exchange_rate),2) as total_payment from acp_trn_tpayment  where date_format(payment_date,'%M/%Y')= '" + dt[0].ToString() + "' ";
                        values.total_payment = objdbconn.GetExecuteScalar(msSQL);
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt1 in dt_datatable.Rows)
                            {

                                MonthwiseOrderReport_List.Add(new GetMonthwiseOrderReport_List
                                {
                                    total_invoice = values.total_invoice,
                                    total_payment = values.total_payment,
                                    month_wise = dt["month_wise"].ToString(),
                                    so_total = (dt1["so_total"].ToString()),
                                    total = (dt1["total"].ToString()),
                                    year = dt["year"].ToString(),

                                });
                            }
                        }


                        values.GetMonthwiseOrderReport_List = MonthwiseOrderReport_List;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Month Wise Order Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetCustomerData(string employee_gid, string month_wise, MdlPmrRptPurchaseOrder values)
        {
            try
            {


                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {
                    objOdbcDataReader.Read();
                    currency_code = objOdbcDataReader["currency_code"].ToString();
                    objOdbcDataReader.Close();
                }

                msSQL = " select distinct a.purchaseorder_gid, a.poref_no,   DATE_FORMAT(a.purchaseorder_date, '%m/%d/%Y') AS purchaseorder_date,c.user_firstname, e.vendor_companyname,a.created_by, " +
                         " a.purchaseorder_status,a.currency_code,  case when a.total_amount ='0.00' then format(a.total_amount,2) else format(a.total_amount,2) end as Grandtotal, " +
                         " case when a.currency_code = '" + currency_code + "' then e.vendor_companyname  when a.currency_code is null then e.vendor_companyname " +
                         "  when a.currency_code is not null and a.currency_code <> '" + currency_code + "'  then concat(e.vendor_companyname,' / ',h.country) end as vendor_companyname, " +
                         " case when a.vendor_emailid is null then concat(e.vendor_companyname,'/',e.contact_telephonenumber,'/',e.email_id) " +
                         " when a.vendor_emailid is not null then concat(e.contactperson_name,' / ',e.contact_telephonenumber,' / ',e.email_id) end as contact,  " +
                         " date_format(a.purchaseorder_date,'%Y') as year  from pmr_trn_tpurchaseorder a  left join acp_mst_tvendor e on a.vendor_gid=e.vendor_gid " +
                         " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code  left join adm_mst_tuser c on c.user_gid= a.created_by  " +
                         " where date_format(a.purchaseorder_date,'%M/%Y')='" + month_wise + "' " +
                         "  group by purchaseorder_gid  order by date(a.purchaseorder_date) desc,a.purchaseorder_date asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var customerdata_list = new List<customerdata_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        customerdata_list.Add(new customerdata_list
                        {
                            purchaseorder_gid = dt["purchaseorder_gid"].ToString(),
                            purchaseorder_date = dt["purchaseorder_date"].ToString(),
                            poref_no = dt["poref_no"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contact = dt["contact"].ToString(),
                            purchaseorder_status = dt["purchaseorder_status"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),

                        });

                    }
                }
                values.customerdata_list = customerdata_list;

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Month Wise Order Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetOrderForLastSixMonthsSearch(MdlPmrRptPurchaseOrder values, string from_date, string to_date)
        {
            try
            {
                if (from_date == null && to_date == null)
                {
                    msSQL = " select DATE_FORMAT(purchaseorder_date, '%b-%Y')  as purchaseorder_date,substring(date_format(a.purchaseorder_date,'%M'),1,3)as month,a.purchaseorder_gid,year(a.purchaseorder_date) as year, " +
                            " format(round(sum(a.total_amount),2),2)as amount,count(a.purchaseorder_gid)as ordercount,format(sum(a.total_amount),2)as orderamount   from pmr_trn_tpurchaseorder a    " +
                            " where a.purchaseorder_date > date_add(now(), interval-6 month) and a.purchaseorder_date<=date(now())    " +
                            " and a.purchaseorder_flag not in('PO Amended','PO Canceled','PO Rejected') group by date_format(a.purchaseorder_date,'%M') order by a.purchaseorder_date desc ";
                }
                else
                {
                    msSQL =  " select DATE_FORMAT(purchaseorder_date, '%b-%Y')  as purchaseorder_date,substring(date_format(a.purchaseorder_date,'%M'),1,3)as month,a.purchaseorder_gid,year(a.purchaseorder_date) as year, " +
                            " format(round(sum(a.total_amount),2),2)as amount,count(a.purchaseorder_gid)as ordercount,format(sum(a.total_amount),2)as orderamount   from pmr_trn_tpurchaseorder a    " +
                            " where a.purchaseorder_date between '" + from_date + "' and '" + to_date + "' " +
                            " and a.purchaseorder_flag not in('PO Amended','PO Canceled','PO Rejected') group by date_format(a.purchaseorder_date,'%M') order by a.purchaseorder_date desc ";
                }
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetPurchaseOrderForLastSixMonths_List = new List<GetPurchaseOrderForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetPurchaseOrderForLastSixMonths_List.Add(new GetPurchaseOrderForLastSixMonths_List
                        {
                            purchaseorder_date = (dt["purchaseorder_date"].ToString()),
                            month = (dt["month"].ToString()),
                            year = (dt["year"].ToString()),
                            amount = (dt["amount"].ToString()),
                            orderamount = (dt["orderamount"].ToString()),
                            ordercount = (dt["ordercount"].ToString()),
                        });
                        values.GetPurchaseOrderForLastSixMonths_List = GetPurchaseOrderForLastSixMonths_List;
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
