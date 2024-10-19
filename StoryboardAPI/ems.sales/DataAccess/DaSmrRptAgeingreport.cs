using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.sales.Models;

//using Google.Protobuf.WellKnownTypes;
using System.Xml.Linq;


namespace ems.sales.DataAccess
{
    public class DaSmrRptAgeingreport
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL, msSQL1, msSQL2 = string.Empty;
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lsdesignation_code, lsorder_type,
            lsdiscountpercentage, lsdiscountamount, lsprice, lstype1, lsproduct_price, mssalesorderGID,
            mssalesorderGID1, mscusconGetGID, lscustomer_name, msGetCustomergid, lscustomer_gid, msGetGid2,
            msGetGid3, lsCode, msPOGetGID, msGetGID, msGetGid, msGetGid1, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5;


        public void DaGet30invoicereport(MdlSmrRptAgeingreport values)
        {
           

            msSQL = " Select distinct a.invoice_gid,a.invoice_refno,a.invoice_status,a.invoice_flag,DATE_FORMAT(a.invoice_date, '%d-%m-%Y') as invoice_date,a.invoice_type,c.branch_name, " +
                " FORMAT(a.advance_amount,2) as advance_amount,invoice_from, " +
                " format((payment_amount * exchange_rate),2) as payment_amount,a.payment_date, " +
                " CASE WHEN a.payment_flag <>'PY Pending' then a.payment_flag else invoice_flag END as 'overall_status',  " +
                " CASE WHEN a.payment_date < NOW() then (select DATEDIFF(NOW(),payment_date) as pending_days) END as expiry_days, " +
                " DATE_FORMAT(a.payment_date,'%d-%m-%Y') as due_date, " +
                " format((a.invoice_amount*a.exchange_rate),2) as invoice_amount,invoice_date, " +
                " CONCAT(a.customer_name,'/',a.customer_contactperson,'/',a.customer_contactnumber) as companydetails,  " +
                 " CONCAT(z.customer_id,'/',z.customer_name) as customer_name,  " +
                " a.invoice_from,format(((a.invoice_amount*a.exchange_rate) - (a.payment_amount*a.exchange_rate)),2) as outstanding_amount " +
                " FROM rbl_trn_tinvoice a " +
                " LEFT JOIN rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                " LEFT JOIN hrm_mst_tbranch c on a.branch_gid=c.branch_gid " +
                 " LEFT JOIN crm_mst_tcustomer z on z.customer_gid=a.customer_gid " +
                " WHERE 1=1 and a.invoice_amount  <> a.payment_amount and a.invoice_status <> 'Invoice Cancelled' and a.invoice_flag <> 'Invoice Cancelled'";
            msSQL += " and a.invoice_date > date_add(now(), interval-30 day) and a.invoice_date<=now()";
            msSQL += " GROUP BY a.invoice_gid " +
           " ORDER by date(invoice_date) desc,invoice_date asc, invoice_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<thirtydays_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    values.invoice_amount30 += double.Parse(dt["invoice_amount"].ToString());
                    values.paid_amount30 += double.Parse(dt["payment_amount"].ToString());
                    values.outstanding_amount30 += double.Parse(dt["outstanding_amount"].ToString());

                    getModuleList.Add(new thirtydays_list
                    {
                        invoice_date  = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        branch = dt["branch_name"].ToString(),
                        company_details = dt["customer_name"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        statuses = dt["invoice_status"].ToString(),
                    });
                    values.thirtydays_list = getModuleList;

                }
            }
        }
        public void DaGet30to60invoicereport(MdlSmrRptAgeingreport values)
        {
            

            msSQL = " Select distinct a.invoice_gid,a.invoice_refno,a.invoice_status,a.invoice_flag,DATE_FORMAT(a.invoice_date, '%d-%m-%Y') as invoice_date,a.invoice_type,c.branch_name, " +
                " FORMAT(a.advance_amount,2) as advance_amount,invoice_from, " +
                " format((payment_amount * exchange_rate),2) as payment_amount,a.payment_date, " +
                " CASE WHEN a.payment_flag <>'PY Pending' then a.payment_flag else invoice_flag END as 'overall_status',  " +
                " CASE WHEN a.payment_date < NOW() then (select DATEDIFF(NOW(),payment_date) as pending_days) END as expiry_days, " +
                " DATE_FORMAT(a.payment_date,'%d-%m-%Y') as due_date, " +
                " format((a.invoice_amount*a.exchange_rate),2) as invoice_amount,invoice_date, " +
                " CONCAT(a.customer_name,'/',a.customer_contactperson,'/',a.customer_contactnumber) as companydetails,  " +
                " CONCAT(z.customer_id,'/',z.customer_name) as customer_name,  " +
                " a.invoice_from,format(((a.invoice_amount*a.exchange_rate) - (a.payment_amount*a.exchange_rate)),2) as outstanding_amount " +
                " FROM rbl_trn_tinvoice a " +
                " LEFT JOIN rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                " LEFT JOIN hrm_mst_tbranch c on a.branch_gid=c.branch_gid " +
                 " LEFT JOIN crm_mst_tcustomer z on z.customer_gid=a.customer_gid " +
                " WHERE 1=1 and a.invoice_amount  <> a.payment_amount and a.invoice_status <> 'Invoice Cancelled' and a.invoice_flag <> 'Invoice Cancelled'";
            msSQL += "and  datediff(current_date, date(invoice_date)) BETWEEN 31 AND 60";
            msSQL += " GROUP BY a.invoice_gid " +
           " ORDER by date(invoice_date) desc,invoice_date asc, invoice_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<thirtytosixty_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    values.invoice_amount60 += double.Parse(dt["invoice_amount"].ToString());
                    values.paid_amount60 += double.Parse(dt["payment_amount"].ToString());
                    values.outstanding_amount60 += double.Parse(dt["outstanding_amount"].ToString());

                    getModuleList.Add(new thirtytosixty_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        branch = dt["branch_name"].ToString(),
                        company_details = dt["customer_name"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        statuses = dt["invoice_status"].ToString(),
                    });
                    values.thirtytosixty_list = getModuleList;

                }
            }

        }
        public void DaGet60to90invoicereport(MdlSmrRptAgeingreport values)
        {

            

            msSQL = " Select distinct a.invoice_gid,a.invoice_refno,a.invoice_status,a.invoice_flag,DATE_FORMAT(a.invoice_date, '%d-%m-%Y') as invoice_date,a.invoice_type,c.branch_name, " +
                " FORMAT(a.advance_amount,2) as advance_amount,invoice_from, " +
                " format((payment_amount * exchange_rate),2) as payment_amount,a.payment_date, " +
                " CASE WHEN a.payment_flag <>'PY Pending' then a.payment_flag else invoice_flag END as 'overall_status',  " +
                " CASE WHEN a.payment_date < NOW() then (select DATEDIFF(NOW(),payment_date) as pending_days) END as expiry_days, " +
                " DATE_FORMAT(a.payment_date,'%d-%m-%Y') as due_date, " +
                " format((a.invoice_amount*a.exchange_rate),2) as invoice_amount,invoice_date, " +
                " CONCAT(a.customer_name,'/',a.customer_contactperson,'/',a.customer_contactnumber) as companydetails,  " +
                 " CONCAT(z.customer_id,'/',z.customer_name) as customer_name,  " +
                " a.invoice_from,format(((a.invoice_amount*a.exchange_rate) - (a.payment_amount*a.exchange_rate)),2) as outstanding_amount " +
                " FROM rbl_trn_tinvoice a " +
                " LEFT JOIN rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                " LEFT JOIN hrm_mst_tbranch c on a.branch_gid=c.branch_gid " +
                 " LEFT JOIN crm_mst_tcustomer z on z.customer_gid=a.customer_gid " +
                " WHERE 1=1 and a.invoice_amount  <> a.payment_amount and a.invoice_status <> 'Invoice Cancelled' and a.invoice_flag <> 'Invoice Cancelled'";
            msSQL += " and  datediff(current_date,date(invoice_date)) BETWEEN 61 AND 90";
            msSQL += " GROUP BY a.invoice_gid " +
           " ORDER by date(invoice_date) desc,invoice_date asc, invoice_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<sixtytoninty_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    values.invoice_amount90 += double.Parse(dt["invoice_amount"].ToString());
                    values.paid_amount90 += double.Parse(dt["payment_amount"].ToString());
                    values.outstanding_amount90 += double.Parse(dt["outstanding_amount"].ToString());

                    getModuleList.Add(new sixtytoninty_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        branch = dt["branch_name"].ToString(),
                        company_details = dt["customer_name"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        statuses = dt["invoice_status"].ToString(),
                    });
                    values.sixtytoninty_list = getModuleList;

                }
            }

        }
        public void DaGet90to120invoicereport(MdlSmrRptAgeingreport values)
        {
            

            msSQL = " Select distinct a.invoice_gid,a.invoice_refno,a.invoice_status,a.invoice_flag,DATE_FORMAT(a.invoice_date, '%d-%m-%Y') as invoice_date,a.invoice_type,c.branch_name, " +
                " FORMAT(a.advance_amount,2) as advance_amount,invoice_from, " +
                " format((payment_amount * exchange_rate),2) as payment_amount,a.payment_date, " +
                " CASE WHEN a.payment_flag <>'PY Pending' then a.payment_flag else invoice_flag END as 'overall_status',  " +
                " CASE WHEN a.payment_date < NOW() then (select DATEDIFF(NOW(),payment_date) as pending_days) END as expiry_days, " +
                " DATE_FORMAT(a.payment_date,'%d-%m-%Y') as due_date, " +
                " format((a.invoice_amount*a.exchange_rate),2) as invoice_amount,invoice_date, " +
                " CONCAT(a.customer_name,'/',a.customer_contactperson,'/',a.customer_contactnumber) as companydetails,  " +
                 " CONCAT(z.customer_id,'/',z.customer_name) as customer_name,  " +
                " a.invoice_from,format(((a.invoice_amount*a.exchange_rate) - (a.payment_amount*a.exchange_rate)),2) as outstanding_amount " +
                " FROM rbl_trn_tinvoice a " +
                " LEFT JOIN rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                " LEFT JOIN hrm_mst_tbranch c on a.branch_gid=c.branch_gid " +
                 " LEFT JOIN crm_mst_tcustomer z on z.customer_gid=a.customer_gid " +
                " WHERE 1=1 and a.invoice_amount  <> a.payment_amount and a.invoice_status <> 'Invoice Cancelled' and a.invoice_flag <> 'Invoice Cancelled'";
            msSQL += " and  datediff(current_date,date(invoice_date)) BETWEEN 91 AND 120";
            msSQL += " GROUP BY a.invoice_gid " +
           " ORDER by date(invoice_date) desc,invoice_date asc, invoice_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<nintytoonetwenty_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    values.invoice_amount120 += double.Parse(dt["invoice_amount"].ToString());
                    values.paid_amount120 += double.Parse(dt["payment_amount"].ToString());
                    values.outstanding_amount120 += double.Parse(dt["outstanding_amount"].ToString());

                    getModuleList.Add(new nintytoonetwenty_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        branch = dt["branch_name"].ToString(),
                        company_details = dt["customer_name"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        statuses = dt["invoice_status"].ToString(),
                    });
                    values.nintytoonetwenty_list = getModuleList;

                }
            }

        }
        public void DaGet120to180invoicereport(MdlSmrRptAgeingreport values)
        {
            

            msSQL = " Select distinct a.invoice_gid,a.invoice_refno,a.invoice_status,a.invoice_flag,DATE_FORMAT(a.invoice_date, '%d-%m-%Y') as invoice_date,a.invoice_type,c.branch_name, " +
                " FORMAT(a.advance_amount,2) as advance_amount,invoice_from, " +
                " format((payment_amount * exchange_rate),2) as payment_amount,a.payment_date, " +
                " CASE WHEN a.payment_flag <>'PY Pending' then a.payment_flag else invoice_flag END as 'overall_status',  " +
                " CASE WHEN a.payment_date < NOW() then (select DATEDIFF(NOW(),payment_date) as pending_days) END as expiry_days, " +
                " DATE_FORMAT(a.payment_date,'%d-%m-%Y') as due_date, " +
                " format((a.invoice_amount*a.exchange_rate),2) as invoice_amount,invoice_date, " +
                " CONCAT(a.customer_name,'/',a.customer_contactperson,'/',a.customer_contactnumber) as companydetails,  " +
                " CONCAT(z.customer_id,'/',z.customer_name) as customer_name,  " +
                " a.invoice_from,format(((a.invoice_amount*a.exchange_rate) - (a.payment_amount*a.exchange_rate)),2) as outstanding_amount " +
                " FROM rbl_trn_tinvoice a " +
                " LEFT JOIN rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                " LEFT JOIN hrm_mst_tbranch c on a.branch_gid=c.branch_gid " +
                 " LEFT JOIN crm_mst_tcustomer z on z.customer_gid=a.customer_gid " +
                " WHERE 1=1 and a.invoice_amount  <> a.payment_amount and a.invoice_status <> 'Invoice Cancelled' and a.invoice_flag <> 'Invoice Cancelled'";
            msSQL += " and  datediff(current_date,date(invoice_date)) BETWEEN 121 AND 180";
            msSQL += " GROUP BY a.invoice_gid " +
           " ORDER by date(invoice_date) desc,invoice_date asc, invoice_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<onetwentytooneeighty_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    values.invoice_amount180 += double.Parse(dt["invoice_amount"].ToString());
                    values.paid_amount180 += double.Parse(dt["payment_amount"].ToString());
                    values.outstanding_amount180 += double.Parse(dt["outstanding_amount"].ToString());

                    getModuleList.Add(new onetwentytooneeighty_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        branch = dt["branch_name"].ToString(),
                        company_details = dt["customer_name"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        statuses = dt["invoice_status"].ToString(),
                    });
                    values.onetwentytooneeighty_list = getModuleList;

                }
            }
        }
        public void DaGetallinvoicereport(MdlSmrRptAgeingreport values)
        {
            

            msSQL = " Select distinct a.invoice_gid,a.invoice_refno,a.invoice_status,a.invoice_flag,DATE_FORMAT(a.invoice_date, '%d-%m-%Y') as invoice_date,a.invoice_type,c.branch_name, " +
                " FORMAT(a.advance_amount,2) as advance_amount,invoice_from, " +
                " ifnull(cast(payment_amount * exchange_rate as decimal),0.00) as payment_amount,a.payment_date, " +
                " CASE WHEN a.payment_flag <>'PY Pending' then a.payment_flag else invoice_flag END as 'overall_status',  " +
                " CASE WHEN a.payment_date < NOW() then (select DATEDIFF(NOW(),payment_date) as pending_days) END as expiry_days, " +
                " DATE_FORMAT(a.payment_date,'%d-%m-%Y') as due_date, " +
                " ifnull(cast(a.invoice_amount*a.exchange_rate as decimal),0.00) as invoice_amount,invoice_date, " +
                " CONCAT(a.customer_name,'/',a.customer_contactperson,'/',a.customer_contactnumber) as companydetails,  " +
                 " CONCAT(z.customer_id,'/',z.customer_name) as customer_name,  " +
                " a.invoice_from,ifnull(cast((a.invoice_amount*a.exchange_rate) - (a.payment_amount*a.exchange_rate) as decimal),0.00) as outstanding_amount " +
                " FROM rbl_trn_tinvoice a " +
                " LEFT JOIN rbl_trn_tinvoicedtl b on a.invoice_gid = b.invoice_gid " +
                " LEFT JOIN hrm_mst_tbranch c on a.branch_gid=c.branch_gid " +
                 " LEFT JOIN crm_mst_tcustomer z on z.customer_gid=a.customer_gid " +
                " WHERE 1=1 and a.invoice_amount  <> a.payment_amount and a.invoice_status <> 'Invoice Cancelled' and a.invoice_flag <> 'Invoice Cancelled'";
            msSQL += " GROUP BY a.invoice_gid " +
           " ORDER by date(invoice_date) desc,invoice_date asc, invoice_gid desc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<All_lists>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    values.invoice_amountall += double.Parse(dt["invoice_amount"].ToString());
                    values.paid_amountall += double.Parse(dt["payment_amount"].ToString());
                    values.outstanding_amountall += double.Parse(dt["outstanding_amount"].ToString());

                    getModuleList.Add(new All_lists
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        branch = dt["branch_name"].ToString(),
                        company_details = dt["customer_name"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        statuses = dt["invoice_status"].ToString(),
                    });
                    values.All_lists = getModuleList;

                }
            }
        }

    }
}