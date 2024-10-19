﻿using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Runtime.Remoting;
using System.Drawing;


namespace ems.sales.DataAccess
{
    public class DaSmrRptEnquiryReport
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        DataTable dt_datatable1;
        string msGetGid;
        int mnResult, mnResult1;


        // GetEnquiryForLastSixMonths
        public void DaGetEnquiryForLastSixMonths(string employee_gid, MdlSmrRptEnquiryReport values)
        
        {
            try
            {
                
                msSQL = "  select a.enquiry_gid,substring(date_format(a.enquiry_date,'%M'),1,3)as month,DATE_FORMAT(a.enquiry_date, '%b-%Y')  as enquiry_date,year(a.enquiry_date) as year, " +
                " count(a.enquiry_gid) as enquirycount " +
                " from smr_trn_tsalesenquiry a " +
                " where a.enquiry_date > date_add(now(), interval -6 month) and a.enquiry_date <= date(now())  ";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var GetEnquiryForLastSixMonths_List = new List<GetEnquiryForLastSixMonths_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetEnquiryForLastSixMonths_List.Add(new GetEnquiryForLastSixMonths_List
                    {
                        salesorder_date = (dt["enquiry_date"].ToString()),
                        month = (dt["month"].ToString()),
                        year = (dt["year"].ToString()),                       
                        ordercount = (dt["enquirycount"].ToString()),
                    });
                    values.GetEnquiryForLastSixMonths_List = GetEnquiryForLastSixMonths_List;
                }

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Enquiry Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }
        public void DaGetEnquiryForLastSixMonthsSearch(MdlSmrRptEnquiryReport values, string from_date, string to_date)
        {
            try
            {
                
                if (from_date == null && to_date == null)
            {
                msSQL = "  select a.enquiry_gid,substring(date_format(a.enquiry_date,'%M'),1,3)as month,DATE_FORMAT(a.enquiry_date, '%b-%Y')  as enquiry_date,year(a.enquiry_date) as year, " +
                " count(a.enquiry_gid) as enquirycount " +
                " from smr_trn_tsalesenquiry a " +
                " where a.enquiry_date > date_add(now(), interval -6 month) and a.enquiry_date <= date(now())  ";
            }
            else
            {
                msSQL = "  select a.enquiry_gid,substring(date_format(a.enquiry_date,'%M'),1,3)as month,DATE_FORMAT(a.enquiry_date, '%b-%Y')  as enquiry_date,year(a.enquiry_date) as year, " +
                " count(a.enquiry_gid) as enquirycount " +
                " from smr_trn_tsalesenquiry a " +
                " where a.enquiry_date > date_add(now(), interval -6 month) and a.enquiry_date between DATE_FORMAT(STR_TO_DATE('" + from_date + "', '%d-%m-%Y'), '%Y-%m-%d') and DATE_FORMAT(STR_TO_DATE('" + to_date + "', '%d-%m-%Y'), '%Y-%m-%d')  ";

            }
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var GetEnquiryForLastSixMonths_List = new List<GetEnquiryForLastSixMonths_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetEnquiryForLastSixMonths_List.Add(new GetEnquiryForLastSixMonths_List
                    {
                        salesorder_date = (dt["enquiry_date"].ToString()),
                        month = (dt["month"].ToString()),
                        year = (dt["year"].ToString()),
                        ordercount = (dt["enquirycount"].ToString()),
                    });
                    values.GetEnquiryForLastSixMonths_List = GetEnquiryForLastSixMonths_List;
                }

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Enquiry Report For Last Six Months!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        // GetEnquirySummary
        public void DaGetEnquirySummary(string employee_gid, string salesorder_gid, MdlSmrRptEnquiryReport values)
        {
            try
            {
               
                msSQL = " select a.enquiry_gid,substring(date_format(a.enquiry_date, '%M'), 1, 3) as month,year(a.enquiry_date) as year, " +
                " count(a.enquiry_gid) as enquirycount " +
                " from smr_trn_tsalesenquiry a " +
                " where  a.enquiry_date <= date(now()) ";

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var GetEnquiryForLastSixMonths_List = new List<GetEnquiryForLastSixMonths_List>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetEnquiryForLastSixMonths_List.Add(new GetEnquiryForLastSixMonths_List
                    {
                        month = (dt["month"].ToString()),
                        year = (dt["year"].ToString()),                      
                        ordercount = (dt["enquirycount"].ToString()),
                    });
                    values.GetEnquiryForLastSixMonths_List = GetEnquiryForLastSixMonths_List;
                }

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Enquiry Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
            
        }

        // GetEnquiryDetailSummary
        public void DaGetEnquiryDetailSummary(string employee_gid, string month ,string year, MdlSmrRptEnquiryReport values)
        {
            try
            {
               

                msSQL = " Select distinct concat(a.enquiry_gid,' / ',a.enquiry_type) as enquiry_refno,a.potorder_value,a.enquiry_gid,a.enquiry_date,concat(b.user_firstname,' ',b.user_lastname) as salesperson_name,a.customer_name," +
                    " a.customer_gid,a.lead_status,a.enquiry_gid,s.source_name,a.customer_name, " +
                    " a.enquiry_status,a.enquiry_type, " +
                    " concat(b.user_firstname,'-',b.user_lastname) as campaign, " +
                    " case when a.contact_person is null then concat(n.leadbankcontact_name,' / ',n.mobile,' / ',n.email) " +
                    " when a.contact_person is not null then concat(a.contact_person,' / ',a.contact_number,' / ',a.contact_email) end as contact_details, " +
                    " a.enquiry_type from smr_trn_tsalesenquiry a " +
                    " left join crm_trn_tleadbank m on m.leadbank_gid=a.customer_gid " +
                    " left join crm_trn_tleadbankcontact n on n.leadbank_gid=m.leadbank_gid " +
                    " left join hrm_mst_temployee k on k.employee_gid=a.created_by " +
                    " left join adm_mst_tuser b on b.user_gid= k.user_gid" +
                    " left join crm_mst_tsource s on s.source_gid=m.source_gid" +
                    " where substring(date_format(a.enquiry_date,'%M'),1,3)='" + month + "' and year(a.enquiry_date)='" + year + "' ";
                   

            dt_datatable = objdbconn.GetDataTable(msSQL);

            var GetEnquiryDetailSummary = new List<GetEnquiryDetailSummary>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetEnquiryDetailSummary.Add(new GetEnquiryDetailSummary
                    {
                        salesorder_date = (dt["enquiry_date"].ToString()),
                        enquiry_refno = (dt["enquiry_refno"].ToString()),
                        customer_name = (dt["customer_name"].ToString()),
                        contact_details = (dt["contact_details"].ToString()),
                        potential_value = (dt["potorder_value"].ToString()),
                        salesorder_status = (dt["enquiry_status"].ToString()),
                        salesperson_name = (dt["salesperson_name"].ToString()),
                        //grandtotal_l = (dt["grandtotal_l"].ToString()),
                    });
                    values.GetEnquiryDetailSummary = GetEnquiryDetailSummary;
                }

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Enquiry Detail Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

        // GetMonthwiseOrderReport
        public void DaGetMonthwiseOrderReport(string employee_gid, MdlSmrRptEnquiryReport values)
        {
            try
            {
                

                msSQL = " select distinct date_format(salesorder_date,'%M/%Y') as month_wise from smr_trn_tsalesorder " +
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
                                
                            });
                        }
                    }


                    values.GetMonthwiseOrderReport_List = MonthwiseOrderReport_List;
                }

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Month Wise Enquiry Report !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
           
        }

     
    }
}