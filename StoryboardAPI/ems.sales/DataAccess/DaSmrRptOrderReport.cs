using ems.sales.Models;
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

namespace ems.sales.DataAccess
{
    public class DaSmrRptOrderReport
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

        public void Dasalespersondropdown(MdlSmrRptOrderReport values)
        {
            msSQL = "select user_gid , concat_ws(' ',user_firstname,user_lastname) as user_name from adm_mst_tuser";
           dt_datatable = objdbconn.GetDataTable(msSQL);
            var salesperson_list = new List<salesperson_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    salesperson_list.Add(new salesperson_list
                    {
                       
                        user_gid = (dt["user_gid"].ToString()),
                        user_name = (dt["user_name"].ToString()),
                    });
                    values.salesperson_list = salesperson_list;
                }

            }

        }
        public void DaGetOrderForLastSixMonths(MdlSmrRptOrderReport values)
        {
            try
            {
                msSQL = " select DATE_FORMAT(salesorder_date, '%b-%Y')  as salesorder_date,substring(date_format(a.salesorder_date,'%M'),1,3)as month,a.salesorder_gid,year(a.salesorder_date) as year, " +
                 " format(round(sum(a.grandtotal),2),2)as amount,round(sum(a.grandtotal),2) as amount1,count(a.salesorder_gid)as ordercount ,  date_format(salesorder_date,'%M/%Y') as month_wise " + 
                 " from smr_trn_tsalesorder a   " +
                 " where a.salesorder_date > date_add(now(), interval-6 month) and a.salesorder_date<=date(now())   " +
                 " and a.salesorder_status not in('SO Amended','Cancelled','Rejected') group by date_format(a.salesorder_date,'%M') order by a.salesorder_date desc  ";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetOrderForLastSixMonths_List = new List<GetOrderForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetOrderForLastSixMonths_List.Add(new GetOrderForLastSixMonths_List
                        {
                            salesorder_date = (dt["salesorder_date"].ToString()),
                            month = (dt["month"].ToString()),
                            month_wise = (dt["month_wise"].ToString()),
                            year = (dt["year"].ToString()),
                            amount = (dt["amount"].ToString()),
                            amount1 = (dt["amount1"].ToString()),
                            ordercount = (dt["ordercount"].ToString()),
                        });
                        values.GetOrderForLastSixMonths_List = GetOrderForLastSixMonths_List;
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

        public void DaGetOrderForLastSixMonthsSearch(MdlSmrRptOrderReport values, string from_date, string to_date ,string sales_person)
        {
            try
            {
                if ( from_date != "undefined" && to_date != "undefined" && sales_person != "undefined")
                {
                    msSQL = " select DATE_FORMAT(salesorder_date, '%b-%Y')  as salesorder_date,substring(date_format(a.salesorder_date,'%M'),1,3)as month,a.salesorder_gid,year(a.salesorder_date) as year, " +
                             " format(round(sum(a.grandtotal),2),2)as amount, round(sum(a.grandtotal),2) as amount1 ,format(sum(a.grandtotal),2)as orderamount,count(a.salesorder_gid)as ordercount,date_format(salesorder_date,'%M/%Y') as month_wise " +
                             " from smr_trn_tsalesorder a   " +
                             " where a.salesorder_date between '" + from_date + "' and '" + to_date + "' " +
                             " and a.salesperson_gid = '" + sales_person + "' and a.salesorder_status not in('SO Amended','Cancelled','Rejected') group by date_format(a.salesorder_date,'%M') order by a.salesorder_date desc  ";

                }
                else if ( sales_person != "undefined" &&  from_date == "undefined" && to_date == "undefined")
                {
                    msSQL = " select DATE_FORMAT(salesorder_date, '%b-%Y')  as salesorder_date,substring(date_format(a.salesorder_date,'%M'),1,3)as month,a.salesorder_gid,year(a.salesorder_date) as year, " +
                        " format(round(sum(a.grandtotal),2),2)as amount, round(sum(a.grandtotal),2) as amount1 ,count(a.salesorder_gid)as ordercount,format(sum(a.grandtotal),2)as orderamount ,date_format(salesorder_date,'%M/%Y') as month_wise " +
                        " from smr_trn_tsalesorder a   " +
                        " where a.salesorder_date > date_add(now(), interval-6 month) and a.salesorder_date<=date(now())   " +
                        " and a.salesperson_gid = '" + sales_person + "' and a.salesorder_status not in('SO Amended','Cancelled','Rejected') group by date_format(a.salesorder_date,'%M') order by a.salesorder_date desc  ";

                }
                else  
                {
                    

                        msSQL = " select DATE_FORMAT(salesorder_date, '%b-%Y')  as salesorder_date,substring(date_format(a.salesorder_date,'%M'),1,3)as month,a.salesorder_gid,year(a.salesorder_date) as year, " +
                                " format(round(sum(a.grandtotal),2),2)as amount,round(sum(a.grandtotal),2) as amount1 ,format(sum(a.grandtotal),2)as orderamount,count(a.salesorder_gid)as ordercount,date_format(salesorder_date,'%M/%Y') as month_wise " +
                                " from smr_trn_tsalesorder a   " +
                                " where a.salesorder_date between '" + from_date + "' and '" + to_date + "' " +
                                " and a.salesorder_status not in('SO Amended','Cancelled','Rejected') group by date_format(a.salesorder_date,'%M') order by a.salesorder_date desc  ";
                    
                }
                 
               
                //else
                //{
                //    msSQL = " select DATE_FORMAT(salesorder_date, '%b-%Y')  as salesorder_date,substring(date_format(a.salesorder_date,'%M'),1,3)as month,a.salesorder_gid,year(a.salesorder_date) as year, " +
                //        " format(round(sum(a.grandtotal),2),2)as amount, round(sum(a.grandtotal),2) as amount1 ,count(a.salesorder_gid)as ordercount,format(sum(a.grandtotal),2)as orderamount ,date_format(salesorder_date,'%M/%Y') as month_wise " +
                //        " from smr_trn_tsalesorder a   " +
                //        " where a.salesorder_date > date_add(now(), interval-6 month) and a.salesorder_date<=date(now())   " +
                //        " and a.salesorder_status not in('SO Amended','Cancelled','Rejected') group by date_format(a.salesorder_date,'%M') order by a.salesorder_date desc  ";

                //}

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetOrderForLastSixMonths_List = new List<GetOrderForLastSixMonths_List>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetOrderForLastSixMonths_List.Add(new GetOrderForLastSixMonths_List
                        {
                            salesorder_date = (dt["salesorder_date"].ToString()),
                            month = (dt["month"].ToString()),
                            month_wise = (dt["month_wise"].ToString()),
                            year = (dt["year"].ToString()),
                            amount = (dt["amount"].ToString()),
                            amount1 = (dt["amount1"].ToString()),
                            orderamount = (dt["orderamount"].ToString()),
                            ordercount = (dt["ordercount"].ToString()),
                        });
                        values.GetOrderForLastSixMonths_List = GetOrderForLastSixMonths_List;
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

        // GetOrderDetailSummary
        public void DaGetOrderDetailSummary(string employee_gid, string month_wise, string sales_person, MdlSmrRptOrderReport values)
        {
            try
            {
                if (sales_person != "undefined") 
                {
                    msSQL = " select a.salesorder_gid,date_format(a.salesorder_date,'%d-%m-%Y')as salesorder_date,Concat(z.customer_id, '/',z.customer_name) as customer_name, " +
                      " concat_ws( '/',y.customercontact_name,y.mobile,y.email)as contact_details " +
                      " ,a.so_type,format(round(a.grandtotal, 2),2) as grandtotal,concat(b.user_firstname,' ',b.user_lastname)as salesperson_name,a.salesorder_status, a.so_referenceno1 " +
                      " from smr_trn_tsalesorder a " +
                      " left join adm_mst_tuser b on b.user_gid=a.salesperson_gid " +
                       " left join crm_mst_tcustomer z on a.customer_gid = z.customer_gid " +
                        " left join crm_mst_tcustomercontact y on y.customer_gid = z.customer_gid " +
                      " where date_format(a.salesorder_date,'%M/%Y')='" + month_wise + "' " +
                      " and a.salesperson_gid = '"+sales_person+"' and a.salesorder_status not in('SO Amended','Cancelled','Rejected') group by a.salesorder_gid  order by a.salesorder_date desc";


                }
                else
                {



                    msSQL = " select a.salesorder_gid,date_format(a.salesorder_date,'%d-%m-%Y')as salesorder_date,Concat(z.customer_id, '/',z.customer_name) as customer_name, " +
                        " concat_ws( '/',y.customercontact_name,y.mobile,y.email)as contact_details " +
                        " ,a.so_type,format(round(a.grandtotal, 2),2) as grandtotal,concat(b.user_firstname,' ',b.user_lastname)as salesperson_name,a.salesorder_status, a.so_referenceno1 " +
                        " from smr_trn_tsalesorder a " +
                        " left join adm_mst_tuser b on b.user_gid=a.salesperson_gid " +
                         " left join crm_mst_tcustomer z on a.customer_gid = z.customer_gid " +
                          " left join crm_mst_tcustomercontact y on y.customer_gid = z.customer_gid " +
                        " where date_format(a.salesorder_date,'%M/%Y')='" + month_wise + "' " +
                        " and a.salesorder_status not in('SO Amended','Cancelled','Rejected') group by a.salesorder_gid  order by a.salesorder_date desc";
                }
               

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetOrderDetailSummary = new List<GetOrderDetailSummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetOrderDetailSummary.Add(new GetOrderDetailSummary
                        {
                            salesorder_date = (dt["salesorder_date"].ToString()),
                            customer_name = (dt["customer_name"].ToString()),
                            so_referenceno1 = (dt["so_referenceno1"].ToString()),
                            contact_details = (dt["contact_details"].ToString()),
                            salesorder_status = (dt["salesorder_status"].ToString()),
                            salesperson_name = (dt["salesperson_name"].ToString()),
                            grandtotal_l = (dt["grandtotal"].ToString()),
                            salesorder_gid = (dt["salesorder_gid"].ToString()),
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
        //individualreport details
        public void DaGetIndividualreport(MdlSmrRptOrderReport values, string salesorder_gid)
        {
            try
            {
                msSQL = "select a.salesorder_gid,b.invoice_refno,a.salesorder_date,a.so_referenceno1," +
                    "a.customer_name,concat(a.customer_contact_person,'/',a.customer_address,'/',a.customer_mobile,'/',a.customer_email)as customer_details," +
                    "b.invoice_date,format((a.grandtotal_l),2) as grand_total, format((b.advance_amount),2) as advance_amount,a.salesorder_status,a.so_type," +
                    " format((b.total_amount_L),2) as invoice_amount, format((((a.grandtotal_l)-(b.total_amount_L))),2) as pending_invoice_amount,c.branch_name " +
                    " from smr_trn_tsalesorder a left join rbl_trn_tinvoice b on  a.salesorder_gid=b.invoice_reference left join hrm_mst_tbranch c on  a.branch_gid=c.branch_gid where salesorder_gid='" + salesorder_gid + "' order by a.salesorder_date desc ;";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var individualreport_list = new List<individualreport_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        individualreport_list.Add(new individualreport_list
                        {
                            invoice_refno = (dt["invoice_refno"].ToString()),
                            salesorder_date = (dt["salesorder_date"].ToString()),
                            so_referenceno1 = (dt["so_referenceno1"].ToString()),
                            customer_name = (dt["customer_name"].ToString()),
                            customer_details = (dt["customer_details"].ToString()),
                            branch_name = (dt["branch_name"].ToString()),
                            salesorder_status = (dt["salesorder_status"].ToString()),
                            so_type = (dt["so_type"].ToString()),
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
        // GetOrderSummary
        public void DaGetOrderSummary(string employee_gid, string salesorder_gid, MdlSmrRptOrderReport values)
        {
            try
            {
                
                msSQL = " select substring(date_format(a.salesorder_date,'%M'),1,3)as month,year(a.salesorder_date) as year, " +
                " round(sum(a.grandtotal),2)as amount,format(sum(a.grandtotal),2)as orderamount,count(a.salesorder_gid)as ordercount " +
                " from smr_trn_tsalesorder a " +
                " where a.salesorder_date > date_add(now(),interval-6 month) and a.salesorder_date<=date(now()) " +
                " and a.salesorder_status not in('SO Amended','Cancelled','Rejected') group by date_format(a.salesorder_date,'%M') order by a.salesorder_date desc";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var GetOrderForLastSixMonths_List = new List<GetOrderForLastSixMonths_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetOrderForLastSixMonths_List.Add(new GetOrderForLastSixMonths_List
                    {
                        month = (dt["month"].ToString()),
                        year = (dt["year"].ToString()),
                        amount = (dt["amount"].ToString()),
                        orderamount = (dt["orderamount"].ToString()),
                        ordercount = (dt["ordercount"].ToString()),
                    });
                    values.GetOrderForLastSixMonths_List = GetOrderForLastSixMonths_List;
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

       
        // GetMonthwiseOrderReport
        public void DaGetMonthwiseOrderReport(string employee_gid, MdlSmrRptOrderReport values)
        {
            try
            {
               

                msSQL = " select distinct date_format(salesorder_date,'%M/%Y') as month_wise,date_format(salesorder_date,'%Y') as year from smr_trn_tsalesorder " +
                    " group by salesorder_date desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var MonthwiseOrderReport_List = new List<GetMonthwiseOrderReport_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = "select count(salesorder_gid) as so_total,format(ifnull(sum(Grandtotal*exchange_rate),0),2) as total " +
                        " from smr_trn_tsalesorder where date_format(salesorder_date,'%M/%Y')='" + dt[0].ToString() + "' and salesorder_status " +
                        " not in ('Approve Pending','SO Amended','Cancelled');";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    msSQL = " select format(sum(total_amount*exchange_rate),2) as total_invoice from rbl_trn_tinvoice" +
                       " where date_format(invoice_date,'%M/%Y')='" + dt[0].ToString() + "' ";
                    values.total_invoice = objdbconn.GetExecuteScalar(msSQL);
                    
                    msSQL = "  select format(sum(total_amount * exchange_rate),2) as total_payment from rbl_trn_tpayment" +
                    " where date_format(payment_date,'%M/%Y')='" + dt[0].ToString() + "' ";
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

                                }) ;
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

        public void DaGetCustomerData(string employee_gid, string month, string year, string from_date, string to_date, MdlSmrRptOrderReport values)
        {
            try
            {
                

                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);

                if (objOdbcDataReader.HasRows == true)
                {
                    
                     currency_code = objOdbcDataReader["currency_code"].ToString();
                    
                }
                objOdbcDataReader.Close();
                if (from_date == null || from_date == "" || from_date == "undefined" || to_date == null || to_date == "" || to_date == "undefined")
                {
                    msSQL = "select distinct a.salesorder_gid, a.so_referenceno1, " +
                        " DATE_FORMAT(a.salesorder_date, '%m/%d/%Y') AS salesorder_date,c.user_firstname, a.customer_contact_person,a.created_by," +
                        " a.salesorder_status,a.currency_code," +
                        " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                        " case when a.currency_code = '" + currency_code + "' then a.customer_name" +
                        " when a.currency_code is null then a.customer_name" +
                        " when a.currency_code is not null and a.currency_code <> '" + currency_code + "'" +
                        " then concat(a.customer_name,' / ',h.country) end as customer_name," +
                        " case when a.customer_email is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email)" +
                        " when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact, " +
                        " date_format(a.salesorder_date,'%Y') as year" +
                        " from smr_trn_tsalesorder a" +
                        " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid" +
                        " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid" +
                        " left join hrm_mst_temployee b on b.employee_gid=a.created_by" +
                        " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code" +
                        " left join adm_mst_tuser c on b.user_gid= c.user_gid  where so_type='Sales'" +
                        " and substring(date_format(a.salesorder_date,'%M'),1,3)='" + month + "' and year(a.salesorder_date)='" + year + "'  " +
                        " order by date(a.salesorder_date) desc,a.salesorder_date asc ";
                }
                else
                {
                    msSQL = "select distinct a.salesorder_gid, a.so_referenceno1, " +
                       " DATE_FORMAT(a.salesorder_date, '%m/%d/%Y') AS salesorder_date,c.user_firstname, a.customer_contact_person,a.created_by," +
                       " a.salesorder_status,a.currency_code," +
                       " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                       " case when a.currency_code = '" + currency_code + "' then a.customer_name" +
                       " when a.currency_code is null then a.customer_name" +
                       " when a.currency_code is not null and a.currency_code <> '" + currency_code + "'" +
                       " then concat(a.customer_name,' / ',h.country) end as customer_name," +
                       " case when a.customer_email is null then concat(e.customercontact_name,'/',e.mobile,'/',e.email)" +
                       " when a.customer_email is not null then concat(a.customer_contact_person,' / ',a.customer_mobile,' / ',a.customer_email) end as contact, " +
                       " date_format(a.salesorder_date,'%Y') as year" +
                       " from smr_trn_tsalesorder a" +
                       " left join crm_mst_tcustomer d on a.customer_gid=d.customer_gid" +
                       " left join crm_mst_tcustomercontact e on d.customer_gid=e.customer_gid" +
                       " left join hrm_mst_temployee b on b.employee_gid=a.created_by" +
                       " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code" +
                       " left join adm_mst_tuser c on b.user_gid= c.user_gid  where so_type='Sales'" +
                       " and substring(date_format(a.salesorder_date,'%M'),1,3)='" + month + "' and year(a.salesorder_date)='" + year + "'  " +
                       " and a.salesorder_date between '" + from_date + "' and '" + to_date + "'" +
                       " order by date(a.salesorder_date) desc,a.salesorder_date asc ";
                }
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var customerdata_list = new List<customerdata_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        customerdata_list.Add(new customerdata_list
                        {
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            salesorder_status = dt["salesorder_status"].ToString(),
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

        // GetOrderWiseOrderReport
        public void DaGetOrderWiseOrderReport(string employee_gid,  MdlSmrRptOrderReport values)
        {
            try
            {
               
                msSQL = " select distinct date_format(salesorder_date,'%d/%M/%Y') as month_wise,salesorder_gid from smr_trn_tsalesorder " +
                    " group by salesorder_date desc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var MonthwiseOrderReport_List = new List<GetOrderwiseOrderReport_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    msSQL = "select count(salesorder_gid) as so_total,format(ifnull(sum(Grandtotal*exchange_rate),0),2) as total " +
                        " from smr_trn_tsalesorder where date_format(salesorder_date,'%d/%M/%Y')='" + dt[0].ToString() + "' and salesorder_status " +
                        " not in ('Approve Pending','SO Amended','Cancelled');";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    msSQL = " select format(sum(total_amount*exchange_rate),2) as total_invoice from rbl_trn_tinvoice" +
                       " where date_format(invoice_date,'%d/%M/%Y')='" + dt[0].ToString() + "' ";
                    values.total_invoice = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "  select format(sum(total_amount * exchange_rate),2) as total_payment from rbl_trn_tpayment" +
                    " where date_format(payment_date,'%d/%M/%Y')='" + dt[0].ToString() + "' ";
                    values.total_payment = objdbconn.GetExecuteScalar(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt1 in dt_datatable.Rows)
                        {

                            MonthwiseOrderReport_List.Add(new GetOrderwiseOrderReport_List
                            {
                               
                                total_invoice = values.total_invoice,
                                total_payment = values.total_payment,
                                month_wise = dt[0].ToString(),
                                so_total = (dt1["so_total"].ToString()),
                                total = (dt1["total"].ToString()),

                            });
                        }
                    }


                    values.GetOrderwiseOrderReport_List = MonthwiseOrderReport_List;
                }

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Order Wise Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }



        // Sales Report Chart

        public void DaGetOrdersCharts(string employee_gid, MdlSmrRptOrderReport values)
        {
            try
            {

                msSQL = " SELECT DATE_FORMAT(salesorder_date, '%b-%Y') AS salesorder_date," +
                    " date_format(salesorder_date,'%M/%Y') as month_wise," +
                    " count(salesorder_gid) as so_total," +
                    " SUBSTRING(DATE_FORMAT(salesorder_date, '%M'), 1, 3) AS month," +
                    " salesorder_gid, YEAR(salesorder_date) AS year,ROUND(SUM(grandtotal), 2) AS amount " +
                    " FROM smr_trn_tsalesorder  WHERE salesorder_status NOT IN ('SO Amended', 'Cancelled', 'Rejected')" +
                    " AND salesorder_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)GROUP BY YEAR(salesorder_date)," +
                    " MONTH(salesorder_date)  ORDER BY YEAR(salesorder_date) DESC, MONTH(salesorder_date) DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetOrders = new List<GetOrders>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetOrders.Add(new GetOrders
                        {
                            salesorder_date = (dt["salesorder_date"].ToString()),
                            month = (dt["month"].ToString()),
                            year = (dt["year"].ToString()),
                            amount = (dt["amount"].ToString()),
                            month_wise = (dt["month_wise"].ToString()),
                            so_total = (dt["so_total"].ToString()),
                        });
                        values.GetOrders = GetOrders;
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



        public void DaGetOrdersChartsummary(MdlSmrRptOrderReport values)
        {
            try
            {

                msSQL = " SELECT salesorder_gid,DATE_FORMAT(salesorder_date, '%b-%Y') AS salesorder_date," +
                    " date_format(salesorder_date,'%M/%Y') as month_wise," +
                    " count(salesorder_gid) as so_total," +
                    " SUBSTRING(DATE_FORMAT(salesorder_date, '%M'), 1, 3) AS month," +
                    " salesorder_gid, YEAR(salesorder_date) AS year,format(ROUND(SUM(grandtotal), 2),2) AS amount " +
                    " FROM smr_trn_tsalesorder  WHERE salesorder_status NOT IN ('SO Amended', 'Cancelled', 'Rejected')" +
                    " and salesorder_date >= CURDATE() - INTERVAL 6 MONTH" +
                    " GROUP BY YEAR(salesorder_date)," +
                    " MONTH(salesorder_date)  ORDER BY YEAR(salesorder_date) DESC, MONTH(salesorder_date) DESC";

                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetOrders = new List<GetOrdersummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetOrders.Add(new GetOrdersummary
                        {
                            salesorder_date = (dt["salesorder_date"].ToString()),
                            month = (dt["month"].ToString()),
                            year = (dt["year"].ToString()),
                            amount = (dt["amount"].ToString()),
                            month_wise = (dt["month_wise"].ToString()),
                            so_total = (dt["so_total"].ToString()),
                            salesorder_gid = dt["salesorder_gid"].ToString()
                        });
                        values.GetOrdersummary = GetOrders;
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

        public void DaGetProductdetail(string salesorder_gid, MdlSmrRptOrderReport values)
        {
            try
            {

                msSQL = "select a.qty_quoted,b.product_name,c.productuom_name from smr_trn_tsalesorderdtl a " +
                        " left join pmr_mst_tproduct b on a.product_gid = b.product_gid " +
                        " left join pmr_mst_tproductuom c on a.uom_gid = c.productuom_gid " +
                        " where a.salesorder_gid='" + salesorder_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Product_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Product_list
                        {
                            product_name = dt["product_name"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                        });
                        values.Product_list = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting RaiseQuotedetail !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        public void DaGetDeliverydetail(string salesorder_gid, MdlSmrRptOrderReport values)
        {
            try
            {

                msSQL = " select a.directorder_refno,a.directorder_date,a.delivered_date,d.user_firstname as delivered_by, b.product_qtydelivered from smr_trn_tdeliveryorder a" +
                        " left join smr_trn_tdeliveryorderdtl b on b.directorder_gid=a.directorder_gid " +
                        " left join hrm_mst_temployee c on c.employee_gid=a.delivered_by" +
                        " left join adm_mst_tuser d on d.user_gid=c.user_gid" +
                        " where a.salesorder_gid='" + salesorder_gid + "'";             
                var getModuleList = new List<Delivery_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Delivery_list
                        {
                            directorder_refno = dt["directorder_refno"].ToString(),
                            directorder_date = dt["directorder_date"].ToString(),
                            delivered_date = dt["delivered_date"].ToString(),
                            delivered_by = dt["delivered_by"].ToString(),
                            product_qtydelivered = dt["product_qtydelivered"].ToString(),
                        });
                        values.Delivery_list = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Deliverydetail !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        public void DaGetOrderSearch(MdlSmrRptOrderReport values, string from_date, string to_date)
        {
            try
            {

                string uiDateStr = from_date;
                DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string start_date = uiDate.ToString("yyyy-MM-dd");

                string uiDateStr1 = to_date;
                DateTime uiDate1 = DateTime.ParseExact(uiDateStr1, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string end_date = uiDate1.ToString("yyyy-MM-dd");

                if (from_date == null && to_date == null)
                {
                    msSQL = " SELECT DATE_FORMAT(salesorder_date, '%b-%Y') AS salesorder_date," +
                   " date_format(salesorder_date,'%M/%Y') as month_wise," +
                   " count(salesorder_gid) as so_total," +
                   " SUBSTRING(DATE_FORMAT(salesorder_date, '%M'), 1, 3) AS month," +
                   " salesorder_gid, YEAR(salesorder_date) AS year,ROUND(SUM(grandtotal), 2) AS amount " +
                   " FROM smr_trn_tsalesorder  WHERE salesorder_status NOT IN ('SO Amended', 'Cancelled', 'Rejected')" +
                   " AND salesorder_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH)GROUP BY YEAR(salesorder_date)," +
                   " MONTH(salesorder_date)  ORDER BY YEAR(salesorder_date) DESC, MONTH(salesorder_date) DESC";
                }
                else
                {
                    msSQL = " SELECT DATE_FORMAT(salesorder_date, '%b-%Y') AS salesorder_date," +
                    " date_format(salesorder_date,'%M/%Y') as month_wise," +
                    " count(salesorder_gid) as so_total," +
                    " SUBSTRING(DATE_FORMAT(salesorder_date, '%M'), 1, 3) AS month," +
                    " salesorder_gid, YEAR(salesorder_date) AS year,ROUND(SUM(grandtotal), 2) AS amount " +
                    " FROM smr_trn_tsalesorder  WHERE salesorder_status NOT IN ('SO Amended', 'Cancelled', 'Rejected')" +
                    " AND  salesorder_date between '" + start_date + "' and '" + end_date + "' " +
                    " GROUP BY YEAR(salesorder_date)," +
                    " MONTH(salesorder_date)  ORDER BY YEAR(salesorder_date) DESC, MONTH(salesorder_date) DESC";

                }
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var GetOrders = new List<GetOrders>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        GetOrders.Add(new GetOrders
                        {
                            
                            salesorder_date = (dt["salesorder_date"].ToString()),
                            month = (dt["month"].ToString()),
                            year = (dt["year"].ToString()),
                            amount = (dt["amount"].ToString()),
                            month_wise = (dt["month_wise"].ToString()),
                            so_total = (dt["so_total"].ToString()),
                        });
                        values.GetOrders = GetOrders;
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