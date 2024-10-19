
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using ems.pmr.Models;

//using Google.Protobuf.WellKnownTypes;
using System.Xml.Linq;


namespace ems.pmr.DataAccess
{
    public class DaPmrRptAgeingReport
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


        public void DaGet30purchaseinvoicereport(MdlPmrRptAgeingReport values)
        {
            msSQL = " select invoice_gid,invoice_refno,replace(invoice_status,'IV','Invoice') as invoice_status,branch_gid,branch_name,date_format(invoice_date,'%d-%m-%Y') as invoice_date,invoice_type," +
                    " invoice_flag,advance_amount ,payment_amount,costcenter_name,vendor_refnodate,overall_status,initialinvoice_amount,pending_days, due_date,vendor_gid,invoice_amount," +
                    " invoice_date,payment_flag,companydetails,invoice_from,outstanding_amount from (select distinct a.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag," +
                    " format(a.advance_amount,2) as advance_amount,ifnull(format(((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate)),2),'0') as payment_amount," +
                    " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', format(a.initialinvoice_amount,2) as initialinvoice_amount," +
                    " case when a.payment_date < Now() then (select DATEDIFF(Now(),a.payment_date) as date) end as pending_days, date_format(a.payment_date,'%d-%m-%Y') as due_date,a.vendor_gid," +
                    " ifnull(format(a.invoice_amount*a.exchange_rate,2),'0') as invoice_amount, a.payment_flag,c.branch_name,concat(b.vendor_companyname,'/',b.contactperson_name,'/',b.contact_telephonenumber) as companydetails," +
                    " a.invoice_from,f.costcenter_name,k.vendor_refnodate, a.invoice_date,c.branch_gid," +
                    " ifnull(format(((a.invoice_amount*a.exchange_rate)-((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate))),2),'0') as outstanding_amount,a.invoice_type from acp_trn_tinvoice a" +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid left join hrm_mst_tbranch c on a.branch_gid = c.branch_gid " +
                    " left join acp_trn_tpo2invoice d on a.invoice_gid=d.invoice_gid " +
                    " left join pmr_trn_tpurchaseorder e on d.purchaseorder_gid=e.purchaseorder_gid " +
                    " left join pmr_mst_tcostcenter f on e.costcenter_gid=f.costcenter_gid " +
                    " inner join  acp_trn_tinvoicedtl k on a.invoice_gid=k.invoice_gid " +
                    " where a.invoice_date > date_add(now(), interval-30 day) and a.invoice_date<=now() and a.invoice_status <> 'IV Canceled')x where outstanding_amount <> '0.00' order by date(invoice_date) desc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<thirtydays_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new thirtydays_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        branch = dt["branch_name"].ToString(),
                        company_details = dt["companydetails"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        statuses = dt["overall_status"].ToString(),
                    });
                    values.thirtydays_list = getModuleList;

                }
            }
            dt_datatable.Dispose();
        }


        public void DaGet30to60invoicereport(MdlPmrRptAgeingReport values)
        {
            msSQL = " select invoice_gid,invoice_refno,replace(invoice_status,'IV','Invoice') as invoice_status,branch_gid,branch_name,date_format(invoice_date,'%d-%m-%Y') as invoice_date,invoice_type," +
                    " invoice_flag,advance_amount ,payment_amount,costcenter_name,vendor_refnodate,overall_status,initialinvoice_amount,pending_days, due_date,vendor_gid,invoice_amount," +
                    " invoice_date,payment_flag,companydetails,invoice_from,outstanding_amount from (select distinct a.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag," +
                    " format(a.advance_amount,2) as advance_amount,ifnull(format(((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate)),2),'0') as payment_amount," +
                    " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', format(a.initialinvoice_amount,2) as initialinvoice_amount," +
                    " case when a.payment_date < Now() then (select DATEDIFF(Now(),a.payment_date) as date) end as pending_days, date_format(a.payment_date,'%d-%m-%Y') as due_date,a.vendor_gid," +
                    " ifnull(format(a.invoice_amount*a.exchange_rate,2),'0') as invoice_amount, a.payment_flag,c.branch_name,concat(b.vendor_companyname,'/',b.contactperson_name,'/',b.contact_telephonenumber) as companydetails," +
                    " a.invoice_from,f.costcenter_name,k.vendor_refnodate, a.invoice_date,c.branch_gid," +
                    " ifnull(format(((a.invoice_amount*a.exchange_rate)-((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate))),2),'0') as outstanding_amount,a.invoice_type from acp_trn_tinvoice a" +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid left join hrm_mst_tbranch c on a.branch_gid = c.branch_gid " +
                    " left join acp_trn_tpo2invoice d on a.invoice_gid=d.invoice_gid " +
                    " left join pmr_trn_tpurchaseorder e on d.purchaseorder_gid=e.purchaseorder_gid " +
                    " left join pmr_mst_tcostcenter f on e.costcenter_gid=f.costcenter_gid " +
                    " inner join  acp_trn_tinvoicedtl k on a.invoice_gid=k.invoice_gid " +
                    "  where datediff(current_date,date(invoice_date)) BETWEEN 31 AND 60 and a.invoice_status <> 'IV Canceled')x where outstanding_amount <> '0.00' order by date(invoice_date) desc ";
           
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<thirtytosixty_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new thirtytosixty_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        branch = dt["branch_name"].ToString(),
                        company_details = dt["companydetails"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        statuses = dt["overall_status"].ToString(),
                    });
                    values.thirtytosixty_list = getModuleList;

                }
            }
            dt_datatable.Dispose();

        }


        public void DaGet60to90invoicereport(MdlPmrRptAgeingReport values)
        {
            msSQL = " select invoice_gid,invoice_refno,replace(invoice_status,'IV','Invoice') as invoice_status,branch_gid,branch_name,date_format(invoice_date,'%d-%m-%Y') as invoice_date,invoice_type," +
                    " invoice_flag,advance_amount ,payment_amount,costcenter_name,vendor_refnodate,overall_status,initialinvoice_amount,pending_days, due_date,vendor_gid,invoice_amount," +
                    " invoice_date,payment_flag,companydetails,invoice_from,outstanding_amount from (select distinct a.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag," +
                    " format(a.advance_amount,2) as advance_amount,ifnull(format(((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate)),2),'0') as payment_amount," +
                    " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', format(a.initialinvoice_amount,2) as initialinvoice_amount," +
                    " case when a.payment_date < Now() then (select DATEDIFF(Now(),a.payment_date) as date) end as pending_days, date_format(a.payment_date,'%d-%m-%Y') as due_date,a.vendor_gid," +
                    " ifnull(format(a.invoice_amount*a.exchange_rate,2),'0') as invoice_amount, a.payment_flag,c.branch_name,concat(b.vendor_companyname,'/',b.contactperson_name,'/',b.contact_telephonenumber) as companydetails," +
                    " a.invoice_from,f.costcenter_name,k.vendor_refnodate, a.invoice_date,c.branch_gid," +
                    " ifnull(format(((a.invoice_amount*a.exchange_rate)-((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate))),2),'0') as outstanding_amount,a.invoice_type from acp_trn_tinvoice a" +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid left join hrm_mst_tbranch c on a.branch_gid = c.branch_gid " +
                    " left join acp_trn_tpo2invoice d on a.invoice_gid=d.invoice_gid " +
                    " left join pmr_trn_tpurchaseorder e on d.purchaseorder_gid=e.purchaseorder_gid " +
                    " left join pmr_mst_tcostcenter f on e.costcenter_gid=f.costcenter_gid " +
                    " inner join  acp_trn_tinvoicedtl k on a.invoice_gid=k.invoice_gid " +
                    "  where datediff(current_date,date(invoice_date)) BETWEEN 61 AND 90 and a.invoice_status <> 'IV Canceled')x where outstanding_amount <> '0.00' order by date(invoice_date) desc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<sixtytoninty_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new sixtytoninty_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        branch = dt["branch_name"].ToString(),
                        company_details = dt["companydetails"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        statuses = dt["overall_status"].ToString(),
                    });
                    values.sixtytoninty_list = getModuleList;

                }
            }
            dt_datatable.Dispose();

        }


        public void DaGet90to120invoicereport(MdlPmrRptAgeingReport values)
        {
            msSQL = " select invoice_gid,invoice_refno,replace(invoice_status,'IV','Invoice') as invoice_status,branch_gid,branch_name,date_format(invoice_date,'%d-%m-%Y') as invoice_date,invoice_type," +
                    " invoice_flag,advance_amount ,payment_amount,costcenter_name,vendor_refnodate,overall_status,initialinvoice_amount,pending_days, due_date,vendor_gid,invoice_amount," +
                    " invoice_date,payment_flag,companydetails,invoice_from,outstanding_amount from (select distinct a.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag," +
                    " format(a.advance_amount,2) as advance_amount,ifnull(format(((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate)),2),'0') as payment_amount," +
                    " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', format(a.initialinvoice_amount,2) as initialinvoice_amount," +
                    " case when a.payment_date < Now() then (select DATEDIFF(Now(),a.payment_date) as date) end as pending_days, date_format(a.payment_date,'%d-%m-%Y') as due_date,a.vendor_gid," +
                    " ifnull(format(a.invoice_amount*a.exchange_rate,2),'0') as invoice_amount, a.payment_flag,c.branch_name,concat(b.vendor_companyname,'/',b.contactperson_name,'/',b.contact_telephonenumber) as companydetails," +
                    " a.invoice_from,f.costcenter_name,k.vendor_refnodate, a.invoice_date,c.branch_gid," +
                    " ifnull(format(((a.invoice_amount*a.exchange_rate)-((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate))),2),'0') as outstanding_amount,a.invoice_type from acp_trn_tinvoice a" +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid left join hrm_mst_tbranch c on a.branch_gid = c.branch_gid " +
                    " left join acp_trn_tpo2invoice d on a.invoice_gid=d.invoice_gid " +
                    " left join pmr_trn_tpurchaseorder e on d.purchaseorder_gid=e.purchaseorder_gid " +
                    " left join pmr_mst_tcostcenter f on e.costcenter_gid=f.costcenter_gid " +
                    " inner join  acp_trn_tinvoicedtl k on a.invoice_gid=k.invoice_gid " +
                    "  where datediff(current_date,date(invoice_date)) BETWEEN 91 AND 120 and a.invoice_status <> 'IV Canceled')x where outstanding_amount <> '0.00' order by date(invoice_date) desc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<nintytoonetwenty_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new nintytoonetwenty_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        branch = dt["branch_name"].ToString(),
                        company_details = dt["companydetails"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        statuses = dt["overall_status"].ToString(),
                    });
                    values.nintytoonetwenty_list = getModuleList;

                }
            }
            dt_datatable.Dispose();

        }


        public void DaGet120to180invoicereport(MdlPmrRptAgeingReport values)
        {
            msSQL = " select invoice_gid,invoice_refno,replace(invoice_status,'IV','Invoice') as invoice_status,branch_gid,branch_name,date_format(invoice_date,'%d-%m-%Y') as invoice_date,invoice_type," +
                    " invoice_flag,advance_amount ,payment_amount,costcenter_name,vendor_refnodate,overall_status,initialinvoice_amount,pending_days, due_date,vendor_gid,invoice_amount," +
                    " invoice_date,payment_flag,companydetails,invoice_from,outstanding_amount from (select distinct a.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag," +
                    " format(a.advance_amount,2) as advance_amount,ifnull(format(((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate)),2),'0') as payment_amount," +
                    " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', format(a.initialinvoice_amount,2) as initialinvoice_amount," +
                    " case when a.payment_date < Now() then (select DATEDIFF(Now(),a.payment_date) as date) end as pending_days, date_format(a.payment_date,'%d-%m-%Y') as due_date,a.vendor_gid," +
                    " ifnull(format(a.invoice_amount*a.exchange_rate,2),'0') as invoice_amount, a.payment_flag,c.branch_name,concat(b.vendor_companyname,'/',b.contactperson_name,'/',b.contact_telephonenumber) as companydetails," +
                    " a.invoice_from,f.costcenter_name,k.vendor_refnodate, a.invoice_date,c.branch_gid," +
                    " ifnull(format(((a.invoice_amount*a.exchange_rate)-((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate))),2),'0') as outstanding_amount,a.invoice_type from acp_trn_tinvoice a" +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid left join hrm_mst_tbranch c on a.branch_gid = c.branch_gid " +
                    " left join acp_trn_tpo2invoice d on a.invoice_gid=d.invoice_gid " +
                    " left join pmr_trn_tpurchaseorder e on d.purchaseorder_gid=e.purchaseorder_gid " +
                    " left join pmr_mst_tcostcenter f on e.costcenter_gid=f.costcenter_gid " +
                    " inner join  acp_trn_tinvoicedtl k on a.invoice_gid=k.invoice_gid " +
                    "  where datediff(current_date,date(invoice_date)) > 121  and a.invoice_status <> 'IV Canceled')x where outstanding_amount <> '0.00' order by date(invoice_date) desc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<onetwentytooneeighty_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new onetwentytooneeighty_list
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        branch = dt["branch_name"].ToString(),
                        company_details = dt["companydetails"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        statuses = dt["overall_status"].ToString(),
                    });
                    values.onetwentytooneeighty_list = getModuleList;

                }
            }
            dt_datatable.Dispose();

        }


        public void DaGetallinvoicereport(MdlPmrRptAgeingReport values)
        {
            msSQL = " select invoice_gid,invoice_refno,replace(invoice_status,'IV','Invoice') as invoice_status,branch_gid,branch_name,date_format(invoice_date,'%d-%m-%Y') as invoice_date,invoice_type," +
                    " invoice_flag,advance_amount ,payment_amount,costcenter_name,vendor_refnodate,overall_status,initialinvoice_amount,pending_days, due_date,vendor_gid,invoice_amount," +
                    " invoice_date,payment_flag,companydetails,invoice_from,outstanding_amount from (select distinct a.invoice_gid, a.invoice_refno, a.invoice_status, a.invoice_flag," +
                    " format(a.advance_amount,2) as advance_amount,ifnull(format(((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate)),2),'0') as payment_amount," +
                    " CASE when a.payment_flag <> 'PY Pending' then a.payment_flag else a.invoice_flag end as 'overall_status', format(a.initialinvoice_amount,2) as initialinvoice_amount," +
                    " case when a.payment_date < Now() then (select DATEDIFF(Now(),a.payment_date) as date) end as pending_days, date_format(a.payment_date,'%d-%m-%Y') as due_date,a.vendor_gid," +
                    " ifnull(format(a.invoice_amount*a.exchange_rate,2),'0') as invoice_amount, a.payment_flag,c.branch_name,concat(b.vendor_companyname,'/',b.contactperson_name,'/',b.contact_telephonenumber) as companydetails," +
                    " a.invoice_from,f.costcenter_name,k.vendor_refnodate, a.invoice_date,c.branch_gid," +
                    " ifnull(format(((a.invoice_amount*a.exchange_rate)-((a.payment_amount*a.exchange_rate)+(a.advance_amount*a.exchange_rate)+(a.debit_note*a.exchange_rate))),2),'0') as outstanding_amount,a.invoice_type from acp_trn_tinvoice a" +
                    " left join acp_mst_tvendor b on a.vendor_gid = b.vendor_gid left join hrm_mst_tbranch c on a.branch_gid = c.branch_gid " +
                    " left join acp_trn_tpo2invoice d on a.invoice_gid=d.invoice_gid " +
                    " left join pmr_trn_tpurchaseorder e on d.purchaseorder_gid=e.purchaseorder_gid " +
                    " left join pmr_mst_tcostcenter f on e.costcenter_gid=f.costcenter_gid " +
                    " inner join  acp_trn_tinvoicedtl k on a.invoice_gid=k.invoice_gid " +
                    " where a.invoice_status <> 'IV Canceled')x where outstanding_amount <> '0.00' order by date(invoice_date) desc  ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<All_lists>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new All_lists
                    {
                        invoice_date = dt["invoice_date"].ToString(),
                        invoice_refno = dt["invoice_refno"].ToString(),
                        branch = dt["branch_name"].ToString(),
                        company_details = dt["companydetails"].ToString(),
                        type = dt["invoice_type"].ToString(),
                        invoice_amount = dt["invoice_amount"].ToString(),
                        paid_amount = dt["payment_amount"].ToString(),
                        outstanding_amount = dt["outstanding_amount"].ToString(),
                        due_date = dt["due_date"].ToString(),
                        statuses = dt["overall_status"].ToString(),
                    });
                    values.All_lists = getModuleList;

                }
            }
            dt_datatable.Dispose();

        }

    }
}