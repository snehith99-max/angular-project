using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Web;
using System;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using static System.Drawing.ImageConverter;
using System.Text;
using Newtonsoft.Json;
using RestSharp;
using CrystalDecisions.Shared.Json;

namespace ems.sales.DataAccess
{
    public class DaSmrTrnQuotation
    {

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string lspop_server, lspop_mail, lspop_password, lscompany, lscompany_code;
        string msINGetGID, msGet_att_Gid, msenquiryloggid;
        string lspath, lspath1, lspath2, mail_path, mail_filepath, pdf_name = "";
        OdbcDataReader objOdbcDataReader, objOdbcDataReader1;
        OdbcDataReader ds_tsalesorderadd;
        int lsamendcount;
        DataTable mail_datatable, dt_datatable;
        string company_logo_path,branchlogo;
        double grandtotal;
        Image branch_logo,company_logo;
        DataTable dt1 = new DataTable();
        DataTable DataTable3 = new DataTable();
        double exchange, costPrice, quantity, discountPercentage, subtotal, discountAmount, reCalTotalAmount, reCaltax_amount, reCaldiscountAmount, reCalTaxAmount, taxAmount;
        string msEmployeeGID, lsemployee_gid, lsenquiry_type, start_date, end_date, lsentity_code, lsquotationgid, lsproductgid1, lstaxname1, TempSOGID, SalesOrderGID, msGetSalesOrderGID, lstaxname, lstaxamount, lspercentage1, lscustomer_gid, lsproduct_price, msGetTempGID, lsquotation_type, lsdesignation_code, lstaxname2, lstaxname3, lsamount2, lsamount3, lspercentage2, lspercentage3, lscustomer_code, pricingsheet_refno, roundoff, mssalesorderGID, lsCode, msGetGid, lsrefno, msGetGid1, msgetGid2, msgetGid4, lstype1, lshierarchy_flag, msGetPrivilege_gid, msGetModule2employee_gid;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, lspop_port;
        string msGetCustomergid, msconGetGID;
        string lsproductgid, lsproductuom_gid, lsproduct_name, lsproductuom_name, mssalesorderGID1, msgetlead2campaign_gid, msGetGID, lsorder_type;
        decimal lsmrp_price;
        string lscustomer_name;
        string lscontact_person, lscustomercontact_gid, lscustomerbranch_name, lscustomercontact_names;
        string lstmpquotationgid;
        string lsproductgroup_gid;
        string lsproductgroup;
        string lsproductname_gid;
        string lsproductname;
        string lsuom_gid;
        string lsvendor_gid;
        string lsuom;
        string lsunitprice;
        string lsquantity;
        string lsdiscountpercentage;
        string lsdiscountamount;
        string lstax_name1;
        string lscustomerproduct_code;
        string lstax_name2;
        string lstax_name3;
        string lstaxamount_1;
        string lstaxamount_2;
        string lstaxamount_3;
        string lstotalamount;
        string lssono, lsprice;
        string lsdisplay_field, lslocalmarginpercentage, lslocalsellingprice, lsuom_name, lsreqdate_remarks, lsrequired_date;
        MailMessage message = new MailMessage();
        public void DaGetSmrTrnQuotation(MdlSmrTrnQuotation values)
        {
            try
            {
                
                msSQL = " select distinct d.leadbank_gid,a.quotation_gid, a.customer_gid,f.enquiry_gid,a.quotation_referencenumber," +
                        " ifnull(concat(f.enquiry_referencenumber ),'Direct Quotation') as quoteenquiry_referencenumber," +
                        " f.enquiry_referencenumber,s.source_name,DATE_FORMAT(a.quotation_date, '%d-%m-%Y') as quotation_date," +
                        " a.quotation_type,a.currency_code,a.quotation_referenceno1, " +
                        "  format(a.Grandtotal, 2) as Grandtotal,concat(g.user_firstname,' ',g.user_lastname) as salesperson," +
                        "  concat(c.user_firstname,' - ',c.user_lastname) as created_by,case when a.customer_gid like 'BCRM%' then" +
                        "  concat(z.customer_id,' / ' ,z.customer_name)  when a.customer_gid like 'BLBP%' THEN  concat(d.leadbank_id,' / ',d.leadbank_name )  end as customer_name," +
                        "  case when a.customer_gid like 'BCRM%' then concat(y.customercontact_name,' / ' ,y.mobile,' / ',y.email) when a.customer_gid like 'BLBP%' THEN  concat(e.leadbankcontact_name,' / ',e.mobile,' / ',e.email ) end as contact," +
                        " a.customer_contact_person, a.quotation_status,a.enquiry_gid, " +
                       " case when a.contact_mail is null then concat(e.leadbankcontact_name,'/',e.mobile,'/',e.email) " +
                       " when a.contact_mail is not null then concat(a.customer_contact_person,' / ',a.contact_no,' / ',a.contact_mail) end as contact, a.customer_address " +
                       " from smr_trn_treceivequotation a " +
                       " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                       " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                       " left join crm_trn_tleadbank d on d.leadbank_gid=a.customer_gid " +
                       " left join crm_mst_Tcustomer z on a.customer_gid=z.customer_gid " +
                       " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                       " left join crm_trn_tleadbankcontact e on e.leadbank_gid=d.leadbank_gid " +
                       " left join adm_mst_tuser g on a.salesperson_gid= g.user_gid   " +
                       " left join crm_mst_tsource s on s.source_gid=d.source_gid" +
                       "  LEFT join crm_mst_Tcustomercontact y on a.customer_gid = y.customer_gid" +
                       " left join acp_trn_tenquiry f on a.enquiry_gid=f.enquiry_gid " +
                       " where 1=1  and a.quotation_status not in('Cancelled','Quotation Amended') order by a.quotation_gid desc";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<quotation_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new quotation_list
                        {
                            quotation_gid = dt["quotation_gid"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            quotation_referenceno1 = dt["quotation_referenceno1"].ToString(),
                            enquiry_gid = dt["quoteenquiry_referencenumber"].ToString(),
                            enquiry_referencenumber = dt["enquiry_referencenumber"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact = dt["contact"].ToString(),
                            quotation_type = dt["quotation_type"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            quotation_status = dt["quotation_status"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            assign_to = dt["salesperson"].ToString()


                        });
                        values.quotation_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Quotation Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void daGetquotationsixmonthschart(MdlSmrTrnQuotation values)
        {

            msSQL = " select DATE_FORMAT(quotation_date, '%b-%Y')  as quotation_date,substring(date_format(a.quotation_date,'%M'),1,3)as month,a.quotation_gid,year(a.quotation_date) as year, " +
             " round(sum(a.Grandtotal),2) as amount1,  date_format(quotation_date,'%M/%Y') as month_wise " +
             " from smr_trn_treceivequotation a   " +
             " where a.quotation_date > date_add(now(), interval-6 month) and a.quotation_date<=date(now())   " +
             " and a.quotation_status not in('Cancelled','Quotation Amended') group by date_format(a.quotation_date,'%M') order by a.quotation_date desc  ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            var quotationlastsixmonths_list = new List<quotationlastsixmonths_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    quotationlastsixmonths_list.Add(new quotationlastsixmonths_list
                    {
                        quotation_date = (dt["quotation_date"].ToString()),
                        months = (dt["month"].ToString()),
                        quotationamount = (dt["amount1"].ToString()),
                       
                    });
                    values.quotationlastsixmonths_list = quotationlastsixmonths_list;
                }

            }

            msSQL = "select COUNT(CASE WHEN a.quotation_status = 'Quotation Amended' THEN 1 END) AS amended_count, " +
                    "  COUNT(CASE WHEN a.quotation_status = 'Approved' THEN 1 END) AS approved_count " +
                    "  FROM  " +
                      " smr_trn_treceivequotation a " +
                       " WHERE  a.quotation_date > DATE_ADD(NOW(), INTERVAL -6 MONTH)  AND a.quotation_date <= DATE(NOW())";
            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
            if (objOdbcDataReader.HasRows)
            {
                
                values.approvecount = objOdbcDataReader["approved_count"].ToString();
               values.amendedcount = objOdbcDataReader["amended_count"].ToString();
                objOdbcDataReader.Close();


            }

        }

        // Sales Person

        public void DaGetSalesDtl(MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = "select user_gid, user_firstname from adm_mst_tuser";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetSalesDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetSalesDropdown

                        {
                            user_gid = dt["user_gid"].ToString(),
                            user_name = dt["user_firstname"].ToString(),

                        });
                        values.GetSalesDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading User Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // Currency
        public void DaGetCurrencyCodeDtl(MdlSmrTrnQuotation values)
        {
            try
            {



                msSQL = "select currencyexchange_gid,currency_code from crm_trn_tcurrencyexchange order by currency_code asc ";

                //                msSQL = " SELECT e.currencyexchange_gid,e.currency_code,e.exchange_rate,e.country AS country_name,CONCAT(b.user_firstname, ' ', b.user_lastname) AS created_by, " +
                //" DATE_FORMAT(e.created_date, '%d-%m-%Y') AS created_date FROM crm_trn_tcurrencyexchange e JOIN (SELECT currency_code, MAX(created_date) AS max_created_date " +
                //" FROM crm_trn_tcurrencyexchange GROUP BY currency_code) m ON e.currency_code = m.currency_code AND e.created_date = m.max_created_date " +
                //" LEFT JOIN adm_mst_tuser b ON b.user_gid = e.created_by GROUP BY e.currency_code ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCurrencyCodeDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCurrencyCodeDropdown

                        {
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),

                        });
                        values.GetCurrencyCodeDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Currency Code !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        // Tax 1
        public void DaGetTaxOnceDtl(MdlSmrTrnQuotation values)
        {

            try
            {


                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxOnceDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxOnceDropdown

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTaxOnceDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }



        public void DaGetMailId(MdlSmrTrnQuotation values)
        {

            try
            {


                msSQL = " select pop_server,pop_port,pop_username,company_gid,pop_password,company_name,company_code from adm_mst_tcompany where company_gid='1' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetMailId>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetMailId

                        {
                            pop_server = dt["pop_server"].ToString(),
                            pop_port = dt["pop_port"].ToString(),
                            pop_username = dt["pop_username"].ToString(),
                            pop_password = dt["pop_password"].ToString(),
                            company_name = dt["company_name"].ToString(),
                            company_code = dt["company_code"].ToString(),
                            company_gid = dt["company_gid"].ToString()

                        });
                        values.GetMailid = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetSendMail_MailIdquot(MdlSmrTrnQuotation values, string quotation_gid)
        {

            try
            {


                msSQL = " select c.quotation_gid,c.quotation_referenceno1,DATE_FORMAT(c.quotation_date,'%d-%m-%Y') as quotation_date," +
                    " c.Grandtotal  " +
                    " ,eportal_emailid,group_concat(a.email SEPARATOR ',') as contact_emails," +
                    " group_concat(a.billing_email SEPARATOR ',' ) as billing_email," +
                    " b.customer_name,b.customer_gid from crm_mst_tcustomercontact a " +
                    " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                    " left join smr_trn_treceivequotation c on b.customer_gid = c.customer_gid where c.quotation_gid = '" + quotation_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetSendMail_MailIdquot>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetSendMail_MailIdquot

                        {
                            to_customer_email = dt["contact_emails"].ToString(),
                            cc_contact_emails = dt["billing_email"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            quotation_gid = dt["quotation_gid"].ToString(),
                            quotation_reference = dt["quotation_referenceno1"].ToString(),
                            due_date = dt["quotation_date"].ToString(),
                            total_amount = dt["Grandtotal"].ToString(),

                        });
                        values.GetSendMail_MailIdquot = getModuleList;
                    }
                }
                dt_datatable.Dispose();

                msSQL = "select a.company_name , b.symbol from adm_mst_tcompany a left join crm_trn_tcurrencyexchange b on b.country_gid = a.country_gid";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                   
                    values.subcompanyname = objOdbcDataReader["company_name"].ToString();
                    values.subsymbol = objOdbcDataReader["symbol"].ToString();

                    objOdbcDataReader.Close();
                    
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Mail Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // Tax 2
        public void DaGetTaxTwiceDtl(MdlSmrTrnQuotation values)
        {
            try
            {


                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxTwiceDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxTwiceDropdown

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name2 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTaxTwiceDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // Tax 3
        public void DaGetTaxThriceDtl(MdlSmrTrnQuotation values)
        {
            try
            {


                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxThriceDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxThriceDropdown

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name3 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTaxThriceDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }



        // Product

        public void DaGetProductNamesDtl(MdlSmrTrnQuotation values)
        {
            try
            {


                msSQL = "Select product_gid, product_name from pmr_mst_tproduct where status='1'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductNamesDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductNamesDropdown

                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),

                        });
                        values.GetProductNamesDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Product  Name  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // Tax 3
        public void DaGetTaxFourSDtl(MdlSmrTrnQuotation values)
        {
            try
            {



                msSQL = "  select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTaxFourSDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTaxFourSDropdown

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name4 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTaxFourSDtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Tax Percentage !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetProductTaxseg(string product_gid, string customercontact_gid, MdlSmrTrnQuotation values)

        {
            try
            {


                msSQL = " select f.taxsegment_gid,d.taxsegment_gid,e.taxsegment_name,d.tax_name,d.tax_gid, " +
                    " CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage,d.tax_amount, " +
                    " a.mrp_price,a.cost_price from acp_mst_ttaxsegment2product d " +
                    " left join acp_mst_ttaxsegment e on e.taxsegment_gid=d.taxsegment_gid " +
                    " left join crm_mst_tcustomer f on f.taxsegment_gid = e.taxsegment_gid " +
                    " left  join pmr_mst_tproduct a on a.product_gid=d.product_gid " +
                    " where a.product_gid='" + product_gid + "'   and f.customer_gid='" + customercontact_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getGetTaxSegmentList = new List<GetTaxSegmentList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getGetTaxSegmentList.Add(new GetTaxSegmentList
                        {
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),

                        });
                        values.GetTaxSegmentList = getGetTaxSegmentList;
                    }
                }
                dt_datatable.Dispose();

            }

            catch (Exception ex)
            {
                values.message = "Exception occured while  Adding Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }


        }

        // On change
        public void DaGetOnChangeProductsName(string product_gid, string customercontact_gid, MdlSmrTrnQuotation values)

        {
            try
            {

                if (customercontact_gid != null)
                {

                    msSQL = "  select a.product_price from smr_trn_tpricesegment2product a    left join smr_trn_tpricesegment2customer b on a.pricesegment_gid= b.pricesegment_gid " +
                        "  left join pmr_mst_tproduct c on a.product_gid=c.product_gid where b.customer_gid='" + customercontact_gid + "'   and a.product_gid='" + product_gid + "'";
                    lsproduct_price = objdbconn.GetExecuteScalar(msSQL);
                    if (lsproduct_price != "")
                    {

                        msSQL = " Select a.product_name, a.product_code,case when f.customer_gid is not null then(select a.product_price from smr_trn_tpricesegment2product a " +
                        " left join smr_trn_tpricesegment2customer b on a.pricesegment_gid= b.pricesegment_gid where b.customer_gid='" + customercontact_gid + "'" +
                        " and a.product_gid='" + product_gid + "') else (a.mrp_price)end as cost_price,  b.productuom_gid,b.productuom_name,c.productgroup_name," +
                        "c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid " +
                        "  left join pmr_mst_tproductgroup c on  a.productgroup_gid = c.productgroup_gid  left join smr_trn_tpricesegment2product e" +
                        " on a.product_gid=e.product_gid left join smr_trn_tpricesegment2customer f on e.pricesegment_gid=f.pricesegment_gid " +
                        " where a.product_gid='" + product_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        var getModuleList = new List<GetproductsCodes>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetproductsCodes
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    selling_price = lsproduct_price,

                                });
                                values.ProductsCode = getModuleList;
                            }
                        }
                        if (dt_datatable.Rows.Count > 0)
                        {
                            lsmrp_price = Convert.ToDecimal(lsproduct_price);
                        }
                        if (lsmrp_price > 0)
                        {
                            msSQL = " select f.taxsegment_gid,d.taxsegment_gid,e.taxsegment_name,d.tax_name,d.tax_gid, " +
                                " CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage,d.tax_amount, " +
                                " a.mrp_price,a.cost_price from acp_mst_ttaxsegment2product d " +
                                " left join acp_mst_ttaxsegment e on e.taxsegment_gid=d.taxsegment_gid " +
                                " left join crm_mst_tcustomer f on f.taxsegment_gid = e.taxsegment_gid " +
                                " left  join pmr_mst_tproduct a on a.product_gid=d.product_gid " +
                                " where a.product_gid='" + product_gid + "'   and f.customer_gid='" + customercontact_gid + "' ";
                            dt_datatable = objdbconn.GetDataTable(msSQL);

                            var getGetTaxSegmentList = new List<GetTaxSegmentList>();
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    getGetTaxSegmentList.Add(new GetTaxSegmentList
                                    {
                                        taxsegment_gid = dt["taxsegment_gid"].ToString(),
                                        taxsegment_name = dt["taxsegment_name"].ToString(),
                                        tax_name = dt["tax_name"].ToString(),
                                        tax_percentage = dt["tax_percentage"].ToString(),
                                        tax_gid = dt["tax_gid"].ToString(),
                                        mrp_price = dt["mrp_price"].ToString(),
                                        cost_price = dt["cost_price"].ToString(),
                                        tax_amount = dt["tax_amount"].ToString(),

                                    });
                                    values.GetTaxSegmentList = getGetTaxSegmentList;
                                }
                            }

                            dt_datatable.Dispose();
                        }
                    }
                    else
                    {
                        msSQL = " Select a.product_name, a.product_code,a.cost_price,a.mrp_price," +
                            " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                             " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                            " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                        " where a.product_gid='" + product_gid + "' ";
                        dt_datatable = objdbconn.GetDataTable(msSQL);

                        var getModuleList = new List<GetproductsCodes>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                getModuleList.Add(new GetproductsCodes
                                {
                                    product_name = dt["product_name"].ToString(),
                                    product_code = dt["product_code"].ToString(),
                                    productuom_name = dt["productuom_name"].ToString(),
                                    productgroup_name = dt["productgroup_name"].ToString(),
                                    productuom_gid = dt["productuom_gid"].ToString(),
                                    productgroup_gid = dt["productgroup_gid"].ToString(),
                                    selling_price = dt["mrp_price"].ToString(),

                                });
                                values.ProductsCode = getModuleList;
                            }
                        }

                        if (dt_datatable.Rows.Count > 0)
                        {
                            lsmrp_price = Convert.ToDecimal(dt_datatable.Rows[0]["mrp_price"]);
                        }
                        if (lsmrp_price > 0)
                        {
                            msSQL = " select f.taxsegment_gid,d.taxsegment_gid,e.taxsegment_name,d.tax_name,d.tax_gid, " +
                                " CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage,d.tax_amount, " +
                                " a.mrp_price,a.cost_price from acp_mst_ttaxsegment2product d " +
                                " left join acp_mst_ttaxsegment e on e.taxsegment_gid=d.taxsegment_gid " +
                                " left join crm_mst_tcustomer f on f.taxsegment_gid = e.taxsegment_gid " +
                                " left  join pmr_mst_tproduct a on a.product_gid=d.product_gid " +
                                " where a.product_gid='" + product_gid + "'   and f.customer_gid='" + customercontact_gid + "' ";
                            dt_datatable = objdbconn.GetDataTable(msSQL);

                            var getGetTaxSegmentList = new List<GetTaxSegmentList>();
                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt in dt_datatable.Rows)
                                {
                                    getGetTaxSegmentList.Add(new GetTaxSegmentList
                                    {
                                        taxsegment_gid = dt["taxsegment_gid"].ToString(),
                                        taxsegment_name = dt["taxsegment_name"].ToString(),
                                        tax_name = dt["tax_name"].ToString(),
                                        tax_percentage = dt["tax_percentage"].ToString(),
                                        tax_gid = dt["tax_gid"].ToString(),
                                        mrp_price = dt["mrp_price"].ToString(),
                                        cost_price = dt["cost_price"].ToString(),
                                        tax_amount = dt["tax_amount"].ToString(),

                                    });
                                    values.GetTaxSegmentList = getGetTaxSegmentList;
                                }
                            }
                            dt_datatable.Dispose();

                        }
                    }
                    dt_datatable.Dispose();
                }
                else
                {
                    values.status = false;
                    values.message = "Kindly Select Customer Before Adding Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Adding Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }


        }


        public void DaGetOnChangeProductsNames(string product_gid, MdlSmrTrnQuotation values)

        {
            try
            {
                msSQL = " Select a.product_name, a.product_code,a.cost_price,a.mrp_price," +
                    " b.productuom_gid,b.productuom_name,c.productgroup_name,c.productgroup_gid,a.productuom_gid  from pmr_mst_tproduct a  " +
                     " left join pmr_mst_tproductuom b on a.productuom_gid = b.productuom_gid  " +
                    " left join pmr_mst_tproductgroup c on a.productgroup_gid = c.productgroup_gid " +
                " where a.product_gid='" + product_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getModuleList = new List<GetproductsCodes>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetproductsCodes
                        {
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            selling_price = dt["mrp_price"].ToString(),

                        });
                        values.ProductsCode = getModuleList;
                    }
                }



            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Adding Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }


        }


        // branch

        public void DaGetBranchDt(MdlSmrTrnQuotation values)
        {
            try
            {


                msSQL = "select branch_gid,branch_name from hrm_mst_tbranch";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetBranchDropdowns>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetBranchDropdowns

                        {
                            branch_gid = dt["branch_gid"].ToString(),
                            branch_name = dt["branch_name"].ToString(),

                        });
                        values.GetBranchDt = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Branch !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        //Customer
        public void DaGetCustomerDtl(MdlSmrTrnQuotation values)
        {
            try
            {


                msSQL = "Select a.customer_gid, a.customer_name " +
                " from crm_mst_tcustomer a " +
                "where a.status='Active'";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCustomerDropdowns>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCustomerDropdowns

                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),

                        });
                        values.GetCustomerDt = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // Contact
        public void DaGetPersonDtl(MdlSmrTrnQuotation values)
        {
            try
            {


                msSQL = "select concat(c.department_name,' ','/',' ',a.user_firstname,' ',a.user_lastname) as user_name,a.user_gid from adm_mst_tuser a " +
                " left join hrm_mst_temployee b on a.user_gid=b.user_gid " +
                " left join hrm_mst_tdepartment c on b.department_gid=c.department_gid where a.user_status='Y' and " +
                " c.department_name in('Sales') order by a.user_code  asc ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetPersonDropdowns>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetPersonDropdowns

                        {
                            user_gid = dt["user_gid"].ToString(),
                            user_name = dt["user_name"].ToString(),

                        });
                        values.GetPersonDt = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting User Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetCurrencyDtl(MdlSmrTrnQuotation values)
        {

            try
            {


                msSQL = "  select currencyexchange_gid,currency_code,default_currency,exchange_rate from crm_trn_tcurrencyexchange order by currency_code asc";
                //                msSQL = " SELECT e.currencyexchange_gid,e.currency_code,e.exchange_rate,e.default_currency,e.country AS country_name,CONCAT(b.user_firstname, ' ', b.user_lastname) AS created_by, " +
                //" DATE_FORMAT(e.created_date, '%d-%m-%Y') AS created_date FROM crm_trn_tcurrencyexchange e JOIN (SELECT currency_code, MAX(created_date) AS max_created_date " +
                //" FROM crm_trn_tcurrencyexchange GROUP BY currency_code) m ON e.currency_code = m.currency_code AND e.created_date = m.max_created_date " +
                //" LEFT JOIN adm_mst_tuser b ON b.user_gid = e.created_by GROUP BY e.currency_code ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetCurrencyDropdowns>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetCurrencyDropdowns

                        {
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            default_currency = dt["default_currency"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),

                        });
                        values.GetCurrencyDt = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Currency!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        //Product
        public void DaGetProductDtl(MdlSmrTrnQuotation values)
        {
            try
            {


                msSQL = "Select product_gid, product_name from pmr_mst_tproduct";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetProductDropdowns>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetProductDropdowns

                        {
                            product_gid = dt["product_gid"].ToString(),
                            productname = dt["product_name"].ToString(),

                        });
                        values.GetProductDt = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // Tax 1
        public void DaGetTax1Dtl(MdlSmrTrnQuotation values)
        {

            try
            {

                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTax1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTax1

                        {
                            tax_gid = dt["tax_gid"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()
                        });
                        values.GetTax1Dtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // Tax 2
        public void DaGetTax2Dtl(MdlSmrTrnQuotation values)
        {
            try
            {


                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTax2>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTax2

                        {
                            tax_gid2 = dt["tax_gid"].ToString(),
                            tax_name2 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTax2Dtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // Tax 3
        public void DaGetTax3Dtl(MdlSmrTrnQuotation values)
        {

            try
            {


                msSQL = " select tax_name,tax_gid,percentage from acp_mst_ttax where active_flag='Y' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTax3>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTax3

                        {
                            tax_gid3 = dt["tax_gid"].ToString(),
                            tax_name3 = dt["tax_name"].ToString(),
                            percentage = dt["percentage"].ToString()

                        });
                        values.GetTax3Dtl = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Tax !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        //Terms And Condition Dropdown

        public void DaGetTermsandConditions(MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = "  select a.template_gid,a.template_name, a.template_content from adm_mst_ttemplate a " +
                        " left join adm_trn_ttemplate2module b on a.template_gid=b.template_gid where b.module_gid='SMR'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetTandCDropdown>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetTandCDropdown
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            termsandconditions = dt["template_content"].ToString()
                        });
                        values.GetTermsandConditions = getModuleList;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Template Name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetOnChangeTerms(string template_gid, MdlSmrTrnQuotation values)
        {
            try
            {

                if (template_gid != null)
                {
                    msSQL = " select template_gid, template_name, template_content from adm_mst_ttemplate where template_gid='" + template_gid + "' ";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    var getModuleList = new List<GetTermDropdown>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetTermDropdown
                            {
                                template_gid = dt["template_gid"].ToString(),
                                template_name = dt["template_name"].ToString(),
                                termsandconditions = dt["template_content"].ToString(),
                            });
                            values.terms_list = getModuleList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Template Content!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        // on change
        public void DaGetOnChangeCustomerDtls(string customercontact_gid, MdlSmrTrnQuotation values)
        {
            try
            {


                if (customercontact_gid != null)
                {
                    msSQL = " select a.customercontact_gid,concat(a.address1,' ',a.state) as address1,ifnull(a.address2,'') as address2,ifnull(a.city,'') as city, " +
                    " ifnull(a.state,'') as state,ifnull(a.country_gid,'') as country_gid,ifnull(a.zip_code,'') as zip_code, " +
                    " ifnull(a.mobile,'') as mobile,a.email,ifnull(b.country_name,'') as country_name,a.customerbranch_name,concat(a.customercontact_name) as " +
                    " customercontact_names, c.customer_gid,c.taxsegment_gid,c.customer_name,c.gst_number,d.taxsegment_name,e.pricesegment_name,f.region_name " +
                    " from crm_mst_tcustomercontact a " +
                    " left join crm_mst_tcustomer c on a.customer_gid=c.customer_gid " +
                    " left join adm_mst_tcountry b on a.country_gid=b.country_gid " +
                    " left join acp_mst_ttaxsegment d on d.taxsegment_gid=c.taxsegment_gid " +
                    " left join smr_trn_tpricesegment e on e.pricesegment_gid=c.pricesegment_gid " +
                    " left join crm_mst_tregion f on f.region_gid=c.customer_region " +
                    " where c.customer_gid='" + customercontact_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);

                    var getModuleList = new List<GetCustomerDetl>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            getModuleList.Add(new GetCustomerDetl
                            {
                                customercontact_names = dt["customercontact_names"].ToString(),
                                branch_name = dt["customerbranch_name"].ToString(),
                                country_name = dt["country_name"].ToString(),
                                email = dt["email"].ToString(),
                                mobile = dt["mobile"].ToString(),
                                zip_code = dt["zip_code"].ToString(),
                                country_gid = dt["country_gid"].ToString(),
                                state = dt["state"].ToString(),
                                city = dt["city"].ToString(),
                                address2 = dt["address2"].ToString(),
                                address1 = dt["address1"].ToString(),
                                customercontact_gid = dt["customercontact_gid"].ToString(),
                                customer_gid = dt["customer_gid"].ToString(),
                                taxsegment_gid = dt["taxsegment_gid"].ToString(),
                                customer_name = dt["customer_name"].ToString(),
                                taxsegment_name = dt["taxsegment_name"].ToString(),
                                pricesegment_name = dt["pricesegment_name"].ToString(),
                                region_name = dt["region_name"].ToString(),
                                gst_number = dt["gst_number"].ToString(),
                            });
                            values.GetCustomerdetls = getModuleList;
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Update Customer Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        // Temp Summary
        public void DaGetTempProductsSummary(string employee_gid, MdlSmrTrnQuotation values)
        {
            try
            {
                double grand_total = 0.00;
                double grandtotal = 0.00;

                msSQL = "SELECT a.tmpsalesorderdtl_gid,a.taxsegment_gid,a.salesorder_gid, a.tax_name,format(a.tax_amount,2) as tax_amount,a.tax_percentage,a.tax_name2,format(a.tax_amount2,2) as tax_amount2,a.tax_percentage2,a.tax_name3,format(a.tax_amount3,2) as tax_amount3,a.tax_percentage3,format(a.tax_amount,2) as tax_amount, a.salesorderdtl_gid, a.salesorder_gid, a.product_gid, a.productgroup_gid, " +
                   " v.productgroup_name, a.product_name, FORMAT(a.product_price, 2) AS product_price, b.product_code, a.qty_quoted, a.product_remarks, " +
                   " a.uom_gid, a.vendor_gid, a.slno, a.uom_name, FORMAT(a.price, 2) AS price, " +
                   " FORMAT(a.discount_percentage,2) AS discount_percentage, FORMAT(a.discount_amount,2) AS discount_amount, " +
                   " FORMAT(a.selling_price, '0.00') AS selling_price,a.product_remarks, " +
                   " CONCAT( CASE WHEN a.tax_name IS NULL THEN '' ELSE a.tax_name END, ' '," +
                   "CASE WHEN a.tax_percentage = '0' THEN '' ELSE concat('',a.tax_percentage,'%') END , " +
                   " CASE WHEN a.tax_amount = '0' THEN '' ELSE concat(':',a.tax_amount) END) AS tax, " +
                   " CONCAT(CASE WHEN a.tax_name2 IS NULL THEN '' ELSE a.tax_name2 END, ' ', " +
                   " CASE WHEN a.tax_percentage2 = '0' THEN '' ELSE concat('', a.tax_percentage2, '%') END, " +
                   " CASE WHEN a.tax_amount2 = '0' THEN '' ELSE concat(':', a.tax_amount2) END) AS tax2, " +
                   " CONCAT(  CASE WHEN a.tax_name3 IS NULL THEN '' ELSE a.tax_name3 END, ' ', " +
                   " CASE WHEN a.tax_percentage3 = '0' THEN '' ELSE concat('', a.tax_percentage3, ' %   ')" +
                   " END, CASE WHEN a.tax_amount3 = '0' THEN '  ' ELSE concat(' : ', a.tax_amount3) END) AS tax3" +
                   " , format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.tax_rate " +
                   " FROM smr_tmp_tsalesorderdtl a " +
                   " left join smr_trn_tsalesorder s on s.salesorder_gid=a.salesorder_gid " +
                   " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                   " left join pmr_mst_tproductgroup v on b.productgroup_gid=v.productgroup_gid " +
                   " LEFT JOIN acp_mst_tvendor e ON a.vendor_gid = e.vendor_gid " +
                   " WHERE a.employee_gid='" + employee_gid + "' and b.delete_flag='N' order by (a.slno+0) asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Gettemporarysummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["price"].ToString());
                        grandtotal += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new Gettemporarysummary
                        {
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            slno = dt["slno"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            discountpercentage = double.Parse(dt["discount_percentage"].ToString()),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            producttotalamount = dt["price"].ToString(),
                            totalamount = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            grand_total = dt["price"].ToString(),
                            grandtotal = dt["price"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            tax = dt["tax"].ToString(),
                            tax2 = dt["tax2"].ToString(),
                            tax3 = dt["tax3"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            taxamount = dt["taxamount"].ToString(),
                            tax_rate = dt["tax_rate"].ToString(),


                        });
                    }
                    values.Gettemporarysummary = getModuleList;
                }

                dt_datatable.Dispose();
                values.grand_total = grand_total;
                values.grandtotal = grandtotal;
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while getting product summary!";
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetDeleteQuotationProductSummary(string tmpsalesorderdtl_gid, summaryprod_list values)
        {
            try
            {


                msSQL = "select price from smr_tmp_tsalesorderdtl " +
                    " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)

                {
                   
                    lsprice = objOdbcDataReader["price"].ToString();
                    objOdbcDataReader.Close();


                }

                msSQL = " delete from smr_tmp_tsalesorderdtl " +
                        " where tmpsalesorderdtl_gid='" + tmpsalesorderdtl_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult != 0)

                {
                    values.status = true;
                    values.message = "Product Deleted Successfully!";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Deleting The Product!";


                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Deleting The Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        string lscustomergid;
        //new submit
        public void DaPostDirectQuotation(string user_gid, postQuatation values)
        {
            try
            {

                msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + user_gid + "'";
                string employee_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select * from smr_tmp_tsalesorderdtl " +
                     " where employee_gid='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {



                    

                    lsrefno = objcmnfunctions.getSequencecustomizerGID("QU", "RBL", values.branch_name);

                    if (msGetGid == "E")
                    {
                        values.status = false;
                        values.message = "Create Sequence Code VQDC for Raise Enquiry";
                        return;
                    }

                    msSQL = " Select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + values.currency_code + "'";
                    string lscurrencycode = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = " select leadbank_gid from crm_trn_tleadbank where customer_gid='" + values.customer_gid + "'";
                    string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);

                    string lsquotation_status = "Approved";
                    string lsgst_percentage = "0.00";
                    string uiDateStr = values.quotation_date;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string quotation_date = uiDate.ToString("yyyy-MM-dd");
                    msGetGid = objcmnfunctions.GetMasterGID("VQDC");

                    msSQL = " insert  into smr_trn_treceivequotation (" +
                             " quotation_gid ," +
                             " branch_gid ," +
                             " quotation_date," +
                             " customer_gid," +
                             " customer_name," +
                             " created_by," +
                             " quotation_remarks," +
                             " quotation_referenceno1, " +
                             " payment_days," +
                             " delivery_days, " +
                             " Grandtotal, " +
                             " termsandconditions, " +
                             " quotation_status," +
                             " contact_no, " +
                             " address1," +
                             " address2," +
                             " contact_mail, " +
                             " addon_charge, " +
                             " additional_discount, " +
                             " addon_charge_l, " +
                             " additional_discount_l, " +
                             " grandtotal_l, " +
                             " currency_code, " +
                             " exchange_rate, " +
                             " currency_gid, " +
                             " total_amount," +
                             " gst_percentage," +
                             " salesperson_gid," +
                             " roundoff, " +
                             " total_price, " +
                             " freight_charges," +
                             " buyback_charges," +
                             " packing_charges," +
                             " insurance_charges," +
                             " tax_amount4," +
                             " tax_name4," +
                             " created_date, " +
                             " pincode " +
                             ") values ( " +
                             " '" + msGetGid + "'," +
                             " '" + values.branch_name.Replace("'", "\\\'") + "'," +
                             " '" + quotation_date + "'," +
                             " '" + values.customer_gid + "'," +
                             " '" + values.customer_name.Replace("'", "\\\'") + "'," +
                             " '" + employee_gid + "',";
                    if(values.quotation_remarks != null)
                    {
                        msSQL += " '" + values.quotation_remarks.Replace("'","\\\'") + "',";
                    }
                    else
                    {
                      msSQL +=  " '" + values.quotation_remarks.Replace("'", "\\\'") + "',";
                    }

                         msSQL +=
                             " '" + lsrefno + "'," +
                             " '" + values.payment_days.Replace("'", "\\\'") + "'," +
                             " '" + values.delivery_days.Replace("'", "\\\'") + "'," +
                             "'" + values.grandtotal.Replace(",", "").Trim() + "', " +
                             " '" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "\\\'") + "'," +
                             " '" + lsquotation_status + "'," +
                             " '" + values.mobile + "'," +
                             " '" + values.address1.Replace("'", "\\\'") + "'," +
                             " '" + values.address2.Replace("'", "\\\'") + "'," +
                             " '" + values.email + "'," +
                             "' " + values.addoncharge + "'," +
                             "'" + values.additional_discount + "',";
                    if (values.addoncharge == "" || values.addoncharge == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.addoncharge.Replace(",", "").Trim() + "',";
                    }
                    if (values.additional_discount == "" || values.additional_discount == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.additional_discount.Replace(",", "").Trim() + "',";
                    }
                    msSQL += "'" + values.grandtotal.Replace(",", "").Trim() + "', " +
                             "'" + lscurrencycode + "'," +
                             "'" + values.exchange_rate + "'," +
                             "'" + values.currency_code + "',";
                    if (values.total_amount == "" || values.total_amount == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.total_amount.Replace(",", "").Trim() + "',";
                    }
                    msSQL += " '" + lsgst_percentage + "'," +
                             " '" + values.user_name + "',";
                    if (values.roundoff == "" || values.roundoff == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.roundoff + "',";
                    }
                    msSQL += "'" + values.producttotalamount + "',";
                    if (values.freightcharges == "" || values.freightcharges == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.freightcharges + "',";
                    }
                    if (values.buybackcharges == "" || values.buybackcharges == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.buybackcharges + "',";
                    }
                    if (values.packing_charges == "" || values.packing_charges == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.packing_charges + "',";
                    }
                    if (lstaxname == "" || lstaxname == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + lstaxname + "',";
                    }
                    if (values.tax_amount4 == "" || values.tax_amount4 == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_amount4 + "',";
                    }
                    if (values.tax_name4 == "" || values.tax_name4 == null)
                    {
                        msSQL += "'0.00',";
                    }
                    else
                    {
                        msSQL += "'" + values.tax_name4 + "',";
                    }
                    msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                              "'" + values.zip_code + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error Occured while inserting Quotation";
                        return;
                    }

                    else
                    {
                        msSQL = "SELECT a.tmpsalesorderdtl_gid,a.taxsegment_gid,a.salesorder_gid, a.tax_name,a.tax_amount," +
                            "a.tax_percentage,a.tax_name2,a.tax_amount2,a.tax_percentage2,a.tax_name3,a.tax_amount3," +
                            "a.tax_percentage3,a.tax_amount, a.salesorderdtl_gid, a.salesorder_gid, a.product_gid, a.productgroup_gid," +
                            "a.tax1_gid, a.tax2_gid,a.tax3_gid, " +
                      " v.productgroup_name, a.product_name, FORMAT(a.product_price, 2) AS product_price, b.product_code, a.qty_quoted, a.product_remarks, " +
                      " a.uom_gid, a.vendor_gid, a.slno, a.uom_name, FORMAT(a.price, 2) AS price, " +
                      " FORMAT(a.discount_percentage,2) AS discount_percentage, FORMAT(a.discount_amount,2) AS discount_amount, " +
                      " FORMAT(a.selling_price, '0.00') AS selling_price,a.product_remarks, " +
                      " CONCAT( CASE WHEN a.tax_name IS NULL THEN '' ELSE a.tax_name END, ' '," +
                      "CASE WHEN a.tax_percentage = '0' THEN '' ELSE concat('',a.tax_percentage,'%') END , " +
                      " CASE WHEN a.tax_amount = '0' THEN '' ELSE concat(':',a.tax_amount) END) AS tax, " +
                      " CONCAT(CASE WHEN a.tax_name2 IS NULL THEN '' ELSE a.tax_name2 END, ' ', " +
                      " CASE WHEN a.tax_percentage2 = '0' THEN '' ELSE concat('', a.tax_percentage2, '%') END, " +
                      " CASE WHEN a.tax_amount2 = '0' THEN '' ELSE concat(':', a.tax_amount2) END) AS tax2, " +
                      " CONCAT(  CASE WHEN a.tax_name3 IS NULL THEN '' ELSE a.tax_name3 END, ' ', " +
                      " CASE WHEN a.tax_percentage3 = '0' THEN '' ELSE concat('', a.tax_percentage3, ' %   ')" +
                      " END, CASE WHEN a.tax_amount3 = '0' THEN '  ' ELSE concat(' : ', a.tax_amount3) END) AS tax3" +
                      " , format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.tax_rate " +
                      " FROM smr_tmp_tsalesorderdtl a " +
                      " left join smr_trn_tsalesorder s on s.salesorder_gid=a.salesorder_gid " +
                      " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                      " left join pmr_mst_tproductgroup v on b.productgroup_gid=v.productgroup_gid " +
                      " LEFT JOIN acp_mst_tvendor e ON a.vendor_gid = e.vendor_gid " +
                      " where employee_gid='" + employee_gid + "'";
                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<Post_List>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            foreach (DataRow dt in dt_datatable.Rows)
                            {

                                msgetGid2 = objcmnfunctions.GetMasterGID("VQDT");
                                if (msgetGid2 == "E")
                                {
                                    values.status = true;
                                    values.message = "Create Sequence Code PPDC for Sales Enquiry Details";
                                    return;
                                }
                                else
                                {
                                    msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                        " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                         " WHERE product_gid='" + dt["product_gid"].ToString() + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                    
                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        lsorder_type = "Sales";
                                        objOdbcDataReader.Close();
                                    }
                                    else
                                    {
                                        lsorder_type = "Services";
                                    }

                                    string display_field = dt["product_remarks"].ToString();


                                    msSQL = "insert into smr_trn_treceivequotationdtl (" +
                                            " quotationdtl_gid ," +
                                            " quotation_gid," +
                                            " product_gid ," +
                                            " productgroup_gid," +
                                            " productgroup_name," +
                                            " product_name," +
                                            " product_code," +
                                            " display_field," +
                                            " product_price," +
                                            " qty_quoted," +
                                            " discount_percentage," +
                                            " discount_amount," +
                                            " uom_gid," +
                                            " uom_name," +
                                            " price," +
                                            " quote_type," +
                                            " tax_name," +
                                            " tax_name2," +
                                            " tax_name3," +
                                            " taxsegment_gid," +
                                            " product_remarks," +
                                            " tax_percentage," +
                                            " tax_percentage2," +
                                            " tax_percentage3," +
                                            " tax_amount," +
                                            " tax_amount2," +
                                            " tax_amount3," +
                                            " tax1_gid," +
                                            " tax2_gid," +
                                            " tax3_gid," +
                                            " slno " +
                                            ")values(" +
                                            " '" + msgetGid2 + "'," +
                                            " '" + msGetGid + "'," +
                                            " '" + dt["product_gid"].ToString() + "'," +
                                            " '" + dt["productgroup_gid"].ToString() + "'," +
                                            " '" + dt["productgroup_name"].ToString().Replace("'", "\\\'") + "'," +
                                            " '" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                                            " '" + dt["product_code"].ToString().Replace("'", "\\\'") + "',";
                                    if (display_field != null)
                                    {
                                       msSQL +=  " '" + display_field.Replace("'", "\\\'") + "',";
                                    }
                                    else
                                    {
                                        msSQL += " '" + display_field + "',";
                                    }

                                    msSQL += " '" + dt["product_price"].ToString().Replace(",", "").Trim() + "'," +
                                       " '" + dt["qty_quoted"].ToString() + "'," +
                                       " '" + dt["discount_percentage"].ToString() + "'," +
                                       " '" + dt["discount_amount"].ToString().Replace(",", "").Trim() + "'," +
                                       " '" + dt["uom_gid"].ToString() + "'," +
                                       " '" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                                       " '" + dt["price"].ToString().Replace(",", "").Trim() + "'," +
                                       " '" + lsorder_type + "'," +
                                       " '" + dt["tax_name"].ToString() + "'," +
                                       " '" + dt["tax_name2"].ToString() + "'," +
                                       " '" + dt["tax_name3"].ToString() + "'," +
                                       " '" + dt["taxsegment_gid"].ToString() + "',";
                                    if (display_field != null)
                                    {
                                        msSQL += " '" + display_field.Replace("'", "\\\'") + "',";
                                    }
                                    else
                                    {
                                        msSQL += " '" + display_field + "',";
                                    }

                                    msSQL += " '" + dt["tax_percentage"].ToString() + "'," +
                                            " '" + dt["tax_percentage2"].ToString() + "'," +
                                            " '" + dt["tax_percentage3"].ToString() + "'," +
                                            " '" + dt["tax_amount"].ToString() + "'," +
                                            " '" + dt["tax_amount2"].ToString() + "'," +
                                            " '" + dt["tax_amount3"].ToString() + "'," +
                                            " '" + dt["tax1_gid"].ToString() + "'," +
                                            " '" + dt["tax2_gid"].ToString() + "'," +
                                            " '" + dt["tax3_gid"].ToString() + "'," +
                                            "'" + dt_datatable.Rows.Count + "')";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                }
                            }
                        }
                        if (mnResult == 0)
                        {
                            values.status = false;
                            values.message = "Error occured while Inserting into Quotationdtl";
                            return;
                        }
                        else
                        {
                            msSQL = " delete from smr_tmp_tsalesorderdtl " +
                           " where employee_gid='" + employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = "select distinct quotation_type from smr_tmp_treceivequotationdtl where created_by='" + employee_gid + "' ";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            
                            if (objOdbcDataReader.HasRows == true)
                            {
                                lsquotation_type = "sales";
                                objOdbcDataReader.Close();

                            }
                            else
                            {
                                lsquotation_type = "Service";
                            }
                           
                        }

                        msSQL = " update smr_trn_treceivequotation set quotation_type='" + lsquotation_type + "' where quotation_gid='" + msGetGid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = " update smr_trn_treceivequotation Set " +
                    " leadbank_gid = '" + lsleadbank_gid + "'" +
                    " where quotation_gid='" + msGetGid + "' ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }


                    string lsstage = "4";
                    msgetlead2campaign_gid = objcmnfunctions.GetMasterGID("BLCC");
                    string lsso = "N";


                    msSQL = " Insert into crm_trn_tenquiry2campaign ( " +
                                   " lead2campaign_gid, " +
                                   " quotation_gid, " +
                                   " so_status, " +
                                   " created_by, " +
                                   " customer_gid, " +
                                   " leadstage_gid," +
                                   " created_date, " +
                                   " campaign_gid," +
                                   " assign_to ) " +
                                   " Values ( " +
                                   "'" + msgetlead2campaign_gid + "'," +
                                   "'" + msGetGid + "'," +
                                   "'" + lsso + "'," +
                                   "'" + employee_gid + "'," +
                                   "'" + values.customer_gid + "'," +
                                   "'" + lsstage + "'," +
                                   "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                   "'" + values.campaignGid + "'," +
                                   "'" + employee_gid + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                    msgetGid4 = objcmnfunctions.GetMasterGID("PODC");
                    {
                        msSQL = " insert into smr_trn_tapproval ( " +
                                " approval_gid, " +
                                " approved_by, " +
                                " approved_date, " +
                                " submodule_gid, " +
                                " qoapproval_gid " +
                                " ) values ( " +
                                "'" + msgetGid4 + "'," +
                                " '" + employee_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'SMRSMRQAP'," +
                                "'" + msGetGid + "') ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        msSQL = "select approval_flag from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + msGetGid + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == false)
                        {
                           
                            msSQL = " Update smr_trn_treceivequotation Set " +
                                   " quotation_status = 'Approved', " +
                                   " approved_by = '" + employee_gid + "', " +
                                   " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                   " where quotation_gid = '" + msGetGid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            objOdbcDataReader.Close();


                        }
                        else
                        {
                            msSQL = "select approved_by from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + msGetGid + "'";
                            objOdbcDataReader1 = objdbconn.GetDataReader(msSQL);
                        }
                        if (objOdbcDataReader1.RecordsAffected == 1)
                        {
                            
                            msSQL = " update smr_trn_tapproval set " +
                           " approval_flag = 'Y', " +
                           " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                           " where approved_by = '" + employee_gid + "'" +
                           " and qoapproval_gid = '" + msGetGid + "' and submodule_gid='SMRSMRQAP'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            msSQL = " Update smr_trn_treceivequotation Set " +
                                   " quotation_status = 'Approved', " +
                           " approved_by = '" + employee_gid + "', " +
                           " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                           " where quotation_gid = '" + msGetGid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                           
                        }
                        else if (objOdbcDataReader1.RecordsAffected > 1)
                        {
                            
                            msSQL = " update smr_trn_tapproval set " +
                                   " approval_flag = 'Y', " +
                                   " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                   " where approved_by = '" + employee_gid + "'" +
                                   " and qoapproval_gid = '" + msGetGid + "' and submodule_gid='SMRSMRQAP'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                           
                        }
                        objOdbcDataReader1.Close();
                    }


                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Quotation Raised Successfully!";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Raising Quotation!";
                        return;
                    }
                }
                else
                {
                    values.status = false;
                    values.message = "Select Atleast One Product to Raise Quotation";
                    return;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submiting  Quotation !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        //Overall submit for Direct Quotation

        //public void DaPostDirectQuotation(string employee_gid, string user_gid, Post_List values)
        //{
        //    try
        //    {

        //        string totalvalue = values.user_name;
        //        string[] separatedValues = totalvalue.Split('.');
        //        // Access individual components
        //        msSQL = " select * from smr_tmp_treceivequotationdtl " +
        //             " where created_by='" + employee_gid + "'";
        //        dt_datatable = objdbconn.GetDataTable(msSQL);
        //        if (dt_datatable.Rows.Count != 0)
        //        {


        //            msGetGid = objcmnfunctions.GetMasterGID("VQDC");
        //            lsrefno = objcmnfunctions.GetMasterGID("VQNP", "", user_gid);

        //            if (msGetGid == "E")
        //            {
        //                values.status = false;
        //                values.message = "Create Sequence Code VQDC for Raise Enquiry";
        //                return;
        //            }

        //            if (values.tax_amount == "")
        //            {
        //                msSQL += "'0.00',";
        //            }
        //            else
        //            {
        //                msSQL += "'" + values.tax_amount + "',";
        //            }

        //            msSQL = " Select currency_code from crm_trn_tcurrencyexchange where currencyexchange_gid ='" + values.currency_code + "'";
        //            string lscurrencycode = objdbconn.GetExecuteScalar(msSQL);

        //            msSQL = " select customercontact_gid from crm_mst_tcustomercontact where customer_gid=  '" + values.customer_gid + "'";
        //            string lscustomercontactgid = objdbconn.GetExecuteScalar(msSQL);

        //            msSQL = " select tax_name from acp_mst_ttax where tax_gid='" + values.tax_name4 + "'";
        //            string lstaxname = objdbconn.GetExecuteScalar(msSQL);

        //            msSQL = " select leadbank_gid from crm_trn_tleadbank where customer_gid='" + values.customer_gid + "'";
        //            string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);

        //            string lsquotation_status = "Approved";
        //            string uiDateStr = values.quotation_date;
        //            DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        //            string quotation_date = uiDate.ToString("yyyy-MM-dd");


        //            msSQL = " insert  into smr_trn_treceivequotation (" +
        //                     " quotation_gid ," +
        //                     " quotation_referencenumber ," +
        //                     " branch_gid ," +
        //                     " quotation_date," +
        //                     " customer_gid," +
        //                     " leadbank_gid," +
        //                     " customer_name," +
        //                     " customerbranch_gid," +
        //                     " customercontact_gid," +
        //                     " customer_contact_person," +
        //                     " created_by," +
        //                     " quotation_remarks," +
        //                     " quotation_referenceno1, " +
        //                     " payment_days, " +
        //                     " delivery_days, " +
        //                     " enquiry_receivedby, " +
        //                     " grandtotal_l, " +
        //                     " termsandconditions, " +
        //                     " quotation_status, " +
        //                     " contact_no, " +
        //                     " customer_address, " +
        //                     " contact_mail, " +
        //                     " addon_charge_l, " +
        //                     " additional_discount_l, " +
        //                     " addon_charge, " +
        //                     " additional_discount, " +
        //                     " Grandtotal, " +
        //                     " currency_code, " +
        //                     " exchange_rate, " +
        //                     " currency_gid, " +
        //                     " total_amount," +
        //                     " gst_percentage," +
        //                     " tax_gid," +
        //                     " salesperson_gid," +
        //                     " vessel_name, " +
        //                     " freight_terms, " +
        //                     " payment_terms," +
        //                     " tax_name," +
        //                     " roundoff, " +
        //                     " tax_amount, " +
        //                     " total_price, " +
        //                     " freight_charges," +
        //                     " buyback_charges," +
        //                     " packing_charges," +
        //                     " created_date ," +
        //                     " taxsegment_gid ," +
        //                     " insurance_charges " +
        //                     ") values ( " +
        //                     " '" + msGetGid + "'," +
        //                     " '" + lsrefno + "'," +
        //                     " '" + values.branch_name + "'," +
        //                     " '" + quotation_date + "'," +
        //                     " '" + values.customer_gid + "'," +
        //                     " '" + lsleadbank_gid + "'," +
        //                     " '" + values.customer_name + "'," +
        //                     " '" + lscustomerbranch_name + "'," +
        //                     " '" + lscustomercontactgid + "'," +
        //                     " '" + lscustomercontact_names + "'," +
        //                     " '" + employee_gid + "'," +
        //                     " '" + values.quotation_remarks + "'," +
        //                     " '" + values.quotation_referenceno1 + "'," +
        //                     " '" + values.payment_days + "'," +
        //                     " '" + values.delivery_days + "'," +
        //                     " '" + values.assign_to + "'," +
        //                     "'" + values.grandtotal.Replace(",", "").Trim() + "', " +
        //                     " '" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "") + "'," +
        //                     " '" + lsquotation_status + "'," +
        //                     " '" + values.mobile + "'," +
        //                     " '" + values.address1 + "'," +
        //                     " '" + values.email + "',";
        //            if (values.addoncharge == "" || values.addoncharge == null)
        //            {
        //                msSQL += "'0.00',";
        //            }
        //            else
        //            {
        //                msSQL += "'" + values.addoncharge.Replace(",", "").Trim() + "',";
        //            }
        //            if (values.additional_discount == "" || values.additional_discount == null)
        //            {
        //                msSQL += "'0.00',";
        //            }
        //            else
        //            {
        //                msSQL += "'" + values.additional_discount.Replace(",", "").Trim() + "',";
        //            }
        //            msSQL += "'" + addonCharges + "'," +
        //                     "'" + additionaldiscountAmount + "'," +
        //                     "'" + values.grandtotal.Replace(",", "").Trim() + "', " +
        //                     "'" + lscurrencycode + "'," +
        //                     "'" + values.exchange_rate + "'," +
        //                     "'" + values.currency_code + "',";
        //            if (values.total_amount == "" || values.total_amount == null)
        //            {
        //                msSQL += "'0.00',";
        //            }
        //            else
        //            {
        //                msSQL += "'" + values.total_amount.Replace(",", "").Trim() + "',";
        //            }
        //            msSQL += "'" + lsgst_percentage + "', ";
        //            if (values.tax_name4 == "" || values.tax_name4 == null)
        //            {
        //                msSQL += "'0.00',";
        //            }
        //            else
        //            {
        //                msSQL += "'" + values.tax_name4 + "',";
        //            }
        //            msSQL += "'" + USERGid + "'," +
        //             "'" + values.vessel_name + "'," +
        //             "'" + values.freight_terms.Trim().Replace("<br />", "<br>").Replace("'", "") + "'," +
        //             "'" + values.payment_terms.Trim().Replace("<br />", "<br>").Replace("'", "") + "',";
        //            if (lstaxname == "" || lstaxname == null)
        //            {
        //                msSQL += "'0.00',";
        //            }
        //            else
        //            {
        //                msSQL += "'" + lstaxname + "',";
        //            }
        //            if (values.roundoff == "" || values.roundoff == null)
        //            {
        //                msSQL += "'0.00',";
        //            }
        //            else
        //            {
        //                msSQL += "'" + values.roundoff + "',";
        //            }
        //            if (values.tax_amount4 == "" || values.tax_amount4 == null)
        //            {
        //                msSQL += "'0.00',";
        //            }
        //            else
        //            {
        //                msSQL += "'" + values.tax_amount4 + "',";
        //            }
        //            msSQL += "'" + values.producttotalamount + "',";

        //            if (values.freightcharges == "" || values.freightcharges == null)
        //            {
        //                msSQL += "'0.00',";
        //            }
        //            else
        //            {
        //                msSQL += "'" + values.freightcharges + "',";
        //            }
        //            if (values.buybackcharges == "" || values.buybackcharges == null)
        //            {
        //                msSQL += "'0.00',";
        //            }
        //            else
        //            {
        //                msSQL += "'" + values.buybackcharges + "',";
        //            }
        //            if (values.packing_charges == "" || values.packing_charges == null)
        //            {
        //                msSQL += "'0.00',";
        //            }
        //            else
        //            {
        //                msSQL += "'" + values.packing_charges + "',";
        //            }
        //            msSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
        //            msSQL += "'" + values.taxsegment_gid + "',";
        //            if (values.insurance_charges == "" || values.insurance_charges == null)
        //            {
        //                msSQL += "'0.00')";
        //            }
        //            else
        //            {
        //                msSQL += "'" + values.insurance_charges + "')";
        //            }
        //            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //            if (mnResult == 0)
        //            {
        //                values.status = false;
        //                values.message = "Error Occured while inserting Quotation";
        //                return;
        //            }

        //            else
        //            {
        //                msSQL = " select " +
        //                      " tmpquotationdtl_gid," +
        //                      " quotation_gid," +
        //                      " product_gid," +
        //                      " productgroup_gid," +
        //                      " productgroup_name," +
        //                      " product_name," +
        //                      " product_code," +
        //                      " product_price," +
        //                      " qty_quoted," +
        //                      " format(discount_percentage,2) as discount_percentage," +
        //                      " format(discount_amount,2) as discount_amount, " +
        //                      " uom_gid," +
        //                      " uom_name," +
        //                      " format(price,2) as price," +
        //                      " tax_name, " +
        //                      " tax_name2, " +
        //                      " tax_name3, " +
        //                      " quotation_type, " +
        //                      " slno, " +
        //                      " productrequireddate_remarks, " +
        //                      " tax_amount, " +
        //                       " taxsegment_gid, " +
        //                       " taxsegmenttax_gid, " +
        //                       " customerproduct_code, " +
        //                      " taxsegment_gid,taxsegmenttax_gid,taxseg_taxgid1, " +
        //                     " taxseg_taxgid2, taxseg_taxgid3, taxseg_taxname1, taxseg_taxname2, taxseg_taxname3, " +
        //                     " taxseg_taxpercent1, taxseg_taxpercent2, taxseg_taxpercent3, taxseg_taxamount1, taxseg_taxamount2, taxseg_taxamount3, " +
        //                     " taxseg_taxtotal " +
        //                       " from smr_tmp_treceivequotationdtl  where created_by='" + employee_gid + "'";
        //                dt_datatable = objdbconn.GetDataTable(msSQL);
        //                var getModuleList = new List<Post_List>();
        //                if (dt_datatable.Rows.Count != 0)
        //                {
        //                    foreach (DataRow dt in dt_datatable.Rows)
        //                    {
        //                        getModuleList.Add(new Post_List
        //                        {

        //                            tmpquotationdtl_gid = dt["tmpquotationdtl_gid"].ToString(),
        //                            quotation_gid = dt["quotation_gid"].ToString(),
        //                            product_gid = dt["product_gid"].ToString(),
        //                            productgroup_gid = dt["productgroup_gid"].ToString(),
        //                            customerproduct_code = dt["customerproduct_code"].ToString(),
        //                            product_name = dt["product_name"].ToString(),
        //                            product_price = dt["product_price"].ToString(),
        //                            quantity = dt["qty_quoted"].ToString(),
        //                            discountpercentage = dt["discount_amount"].ToString(),
        //                            discountamount = dt["discount_amount"].ToString(),
        //                            productuom_gid = dt["uom_gid"].ToString(),
        //                            productuom_name = dt["uom_gid"].ToString(),
        //                            tax_name = dt["tax_name"].ToString(),
        //                            slno = dt["slno"].ToString(),
        //                            tax_amount = dt["tax_amount"].ToString(),
        //                            price = dt["price"].ToString(),
        //                            quote_type = dt["quotation_type"].ToString(),
        //                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
        //                            taxsegmenttax_gid = dt["taxsegmenttax_gid"].ToString(),
        //                        });

        //                        msgetGid2 = objcmnfunctions.GetMasterGID("VQDC");
        //                        if (msgetGid2 == "E")
        //                        {
        //                            values.status = true;
        //                            values.message = "Create Sequence Code PPDC for Sales Enquiry Details";
        //                            return;
        //                        }
        //                        else
        //                        {




        //                            msSQL = "insert into smr_trn_treceivequotationdtl (" +
        //                                    " quotationdtl_gid ," +
        //                                    " quotation_gid," +
        //                                    " product_gid ," +
        //                                    " productgroup_gid," +
        //                                    " productgroup_name," +
        //                                    " product_name," +
        //                                    " product_code," +
        //                                    " product_price," +
        //                                    " qty_quoted," +
        //                                    " discount_percentage," +
        //                                    " discount_amount," +
        //                                    " uom_gid," +
        //                                    " uom_name," +
        //                                    " price," +
        //                                    " quote_type," +
        //                                    " tax_name," +
        //                                     " taxsegment_gid," +
        //                                      " taxsegmenttax_gid," +

        //                                       " taxseg_taxgid1," +
        //                                " taxseg_taxgid2," +
        //                                " taxseg_taxgid3," +
        //                                " taxseg_taxname1," +
        //                                " taxseg_taxname2," +
        //                                " taxseg_taxname3," +
        //                                " taxseg_taxpercent1," +
        //                                " taxseg_taxpercent2," +
        //                                " taxseg_taxpercent3," +
        //                                " taxseg_taxamount1," +
        //                                " taxseg_taxamount2," +
        //                                " taxseg_taxamount3," +
        //                                " taxseg_taxtotal," +

        //                                    " slno " +

        //                                    ")values(" +
        //                                    " '" + msgetGid2 + "'," +
        //                                    " '" + msGetGid + "'," +
        //                                    " '" + dt["product_gid"].ToString() + "'," +
        //                                    " '" + dt["productgroup_gid"].ToString() + "'," +
        //                                    " '" + dt["productgroup_name"].ToString() + "'," +
        //                                    " '" + dt["product_name"].ToString() + "'," +
        //                                    " '" + dt["product_code"].ToString() + "'," +
        //                                    " '" + dt["product_price"].ToString() + "'," +
        //                                    " '" + dt["qty_quoted"].ToString() + "'," +
        //                                    " '" + dt["discount_percentage"].ToString() + "'," +
        //                                    " '" + dt["discount_amount"].ToString().Replace(",", "").Trim() + "'," +
        //                                    " '" + dt["uom_gid"].ToString() + "'," +
        //                                    " '" + dt["uom_name"].ToString() + "'," +
        //                                    " '" + dt["price"].ToString().Replace(",", "").Trim() + "'," +
        //                                    " '" + dt["quotation_type"].ToString() + "'," +
        //                                    " '" + dt["tax_name"].ToString() + "'," +
        //                                    " '" + dt["taxsegment_gid"].ToString() + "'," +
        //                                    " '" + dt["taxsegmenttax_gid"].ToString() + "'," +

        //                                      "'" + dt["taxseg_taxgid1"].ToString() + "'," +
        //                                "'" + dt["taxseg_taxgid2"].ToString() + "'," +
        //                                 "'" + dt["taxseg_taxgid3"].ToString() + "'," +
        //                                  "'" + dt["taxseg_taxname1"].ToString() + "'," +
        //                                "'" + dt["taxseg_taxname2"].ToString() + "'," +
        //                                 "'" + dt["taxseg_taxname3"].ToString() + "'," +
        //                                "'" + dt["taxseg_taxpercent1"].ToString() + "'," +
        //                                 "'" + dt["taxseg_taxpercent2"].ToString() + "'," +
        //                                "'" + dt["taxseg_taxpercent3"].ToString() + "'," +
        //                                 "'" + dt["taxseg_taxamount1"].ToString() + "'," +
        //                                "'" + dt["taxseg_taxamount2"].ToString() + "'," +
        //                                "'" + dt["taxseg_taxamount3"].ToString() + "'," +
        //                                "'" + dt["taxseg_taxtotal"].ToString() + "'," +
        //                                " '" + dt_datatable.Rows.Count + "')";

        //                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        //                        }
        //                    }
        //                }
        //                if (mnResult == 0)
        //                {
        //                    values.status = false;
        //                    values.message = "Error occured while Inserting into Quotationdtl";
        //                    return;
        //                }
        //                else
        //                {

        //                    msSQL = "select distinct quotation_type from smr_tmp_treceivequotationdtl where created_by='" + employee_gid + "' ";
        //                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //                    if (objOdbcDataReader.HasRows == true)
        //                    {
        //                        lsquotation_type = "sales";


        //                    }

        //                    else
        //                    {
        //                        lsquotation_type = "Service";
        //                    }
        //                }

        //                msSQL = " update smr_trn_treceivequotation set quotation_type='" + lsquotation_type + "' where quotation_gid='" + msGetGid + "' ";
        //                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        //                msSQL = " update smr_trn_treceivequotation Set " +
        //            " leadbank_gid = '" + lsleadbank_gid + "'" +
        //            " where quotation_gid='" + msGetGid + "' ";
        //                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //            }


        //            string lsstage = "4";
        //            msgetlead2campaign_gid = objcmnfunctions.GetMasterGID("BLCC");
        //            string lsso = "N";

        //            msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + USERGid + "'";
        //            string employee = objdbconn.GetExecuteScalar(msSQL);

        //            msSQL = " Insert into crm_trn_tenquiry2campaign ( " +
        //                                              " lead2campaign_gid, " +
        //                                              " quotation_gid, " +
        //                                              " so_status, " +
        //                                              " created_by, " +
        //                                              " customer_gid, " +
        //                                              " leadstage_gid," +
        //                                              " created_date, " +
        //                                              " campaign_gid," +
        //                                              " assign_to ) " +
        //                                              " Values ( " +
        //                                              "'" + msgetlead2campaign_gid + "'," +
        //                                              "'" + msGetGid + "'," +
        //                                              "'" + lsso + "'," +
        //                                              "'" + employee_gid + "'," +
        //                                              "'" + values.customer_gid + "'," +
        //                                              "'" + lsstage + "'," +
        //                                              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
        //                                              "'" + campaignGid + "'," +
        //                                              "'" + employee + "')";
        //            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


        //            msgetGid4 = objcmnfunctions.GetMasterGID("PODC");
        //            {
        //                msSQL = " insert into smr_trn_tapproval ( " +
        //                        " approval_gid, " +
        //                        " approved_by, " +
        //                        " approved_date, " +
        //                        " submodule_gid, " +
        //                        " qoapproval_gid " +
        //                        " ) values ( " +
        //                        "'" + msgetGid4 + "'," +
        //                        " '" + employee_gid + "'," +
        //                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
        //                        "'SMRSMRQAP'," +
        //                        "'" + msGetGid + "') ";
        //                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

        //                msSQL = "select approval_flag from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + msGetGid + "' ";
        //                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //                if (objOdbcDataReader.HasRows == false)
        //                {
        //                    msSQL = " Update smr_trn_treceivequotation Set " +
        //                           " quotation_status = 'Approved', " +
        //                           " approved_by = '" + employee_gid + "', " +
        //                           " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
        //                           " where quotation_gid = '" + msGetGid + "'";
        //                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                }
        //                else
        //                {
        //                    msSQL = "select approved_by from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + msGetGid + "'";
        //                    objOdbcDataReader1 = objdbconn.GetDataReader(msSQL);
        //                }
        //                if (objOdbcDataReader1.RecordsAffected == 1)
        //                {
        //                    msSQL = " update smr_trn_tapproval set " +
        //                   " approval_flag = 'Y', " +
        //                   " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
        //                   " where approved_by = '" + employee_gid + "'" +
        //                   " and qoapproval_gid = '" + msGetGid + "' and submodule_gid='SMRSMRQAP'";
        //                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                    msSQL = " Update smr_trn_treceivequotation Set " +
        //                           " quotation_status = 'Approved', " +
        //                   " approved_by = '" + employee_gid + "', " +
        //                   " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
        //                   " where quotation_gid = '" + msGetGid + "'";
        //                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                }
        //                else if (objOdbcDataReader1.RecordsAffected > 1)
        //                {
        //                    msSQL = " update smr_trn_tapproval set " +
        //                           " approval_flag = 'Y', " +
        //                           " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
        //                           " where approved_by = '" + employee_gid + "'" +
        //                           " and qoapproval_gid = '" + msGetGid + "' and submodule_gid='SMRSMRQAP'";
        //                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //                }
        //            }


        //            if (mnResult == 0 || mnResult != 0)
        //            {
        //                msSQL = " delete from smr_tmp_treceivequotationdtl " +
        //                     " where created_by='" + employee_gid + "'";
        //                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //            }


        //            if (mnResult != 0)
        //            {
        //                values.status = true;
        //                values.message = "Quotation Raised Successfully!";
        //            }
        //            else
        //            {
        //                values.status = false;
        //                values.message = "Error While Raising Quotation!";
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            values.status = false;
        //            values.message = "Select Atleast One Product to Raise Quotation";
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while Submiting  Quotation !";
        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
        //        $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
        //        values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

        //    }

        //}
        public void DaGetViewQuotationSummary(string quotation_gid, MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = "  select a.pincode,m.campaign_gid,a.customer_gid,y.customer_gid,y.gst_number,y.customer_city,y.customer_state,l.tax_name,k.country_name,y.customer_pin,n.region_name,x.pricesegment_gid,z.taxsegment_gid,x.pricesegment_name,z.taxsegment_name, a.quotation_referenceno1,b.lead2campaign_gid,a.quotation_gid,a.address1,a.address2,a.currency_code,a.freight_terms," +
                        " a.payment_terms, a.customer_contact_person,a.salesperson_gid, a.termsandconditions," +
                        " date_format(a.quotation_date, '%d-%m-%Y') as quotation_date,a.quotation_remarks, " +
                        " concat(g.user_code, ' / ', g.user_firstname, ' ', g.user_lastname) AS salesperson_name, " +
                        " a.quotation_referenceno1,e.branch_name,a.exchange_rate,  a.contact_mail," +
                        " a.contact_no,format(a.Grandtotal_l, 2) as Grandtotal_l, a.delivery_days," +
                        " format(a.Grandtotal, 2) as Grandtotal,format(a.addon_charge, 2) as addon_charge," +
                        " format(a.additional_discount, 2) as additional_discount,  a.customer_name," +
                        " a.tax_gid,a.tax_name,format(a.total_amount, 2) as total_amount,format(a.total_price, 2) as total_price," +
                        " a.customer_address,a.payment_days,a.delivery_days,a.tax_name4,a.tax_amount4, " +
                        " concat(m.campaign_title, ' | ', j.user_code, ' | ', j.user_firstname, ' ', j.user_lastname) as user_firstname, " +
                        " format(a.freight_charges, 2) as freight_charges,format(a.buyback_charges, 2) as buyback_charges, format(a.roundoff, 2) as roundoff," +
                        " format(a.packing_charges, 2) as packing_charges,format(a.insurance_charges, 2) as insurance_charges from smr_trn_treceivequotation a " +
                        " left join crm_trn_tenquiry2campaign b on a.customer_gid = b.customer_gid " +
                        " left join smr_trn_tcampaign m on b.campaign_gid = m.campaign_gid " +
                        " left join hrm_mst_tbranch e on e.branch_gid = a.branch_gid " +
                        " left join hrm_mst_temployee h on b.assign_to = h.employee_gid " +
                        " left join adm_mst_tuser j on h.user_gid = j.user_gid " +
                        " left join crm_mst_Tcustomer y on a.customer_gid = y.customer_gid " +
                        " left join acp_mst_ttaxsegment z on z.taxsegment_gid = y.taxsegment_gid " +
                        " left join smr_trn_tpricesegment x on x.pricesegment_gid = y.pricesegment_gid " +
                        " left join crm_mst_tregion n on n.region_gid = y.customer_region " +
                        " LEFT JOIN adm_mst_tuser g ON g.user_gid = a.salesperson_gid " +
                       " left join adm_mst_tcountry k on y.customer_country= k.country_gid " +
                        " left join acp_mst_ttax l on a.tax_name4 = l.tax_gid " +
                        " where a.quotation_gid = '" + quotation_gid + "' group by a.quotation_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<postsalesquotation_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new postsalesquotation_list
                        {
                            quotation_gid = dt["quotation_referenceno1"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_person = dt["customer_contact_person"].ToString(),
                            contact_no = dt["contact_no"].ToString(),
                            contact_mail = dt["contact_mail"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            quotation_remarks = dt["quotation_remarks"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            freight_terms = dt["freight_terms"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            delivery_days = dt["delivery_days"].ToString(),
                            addon_charge = dt["addon_charge"].ToString(),
                            additional_discount = dt["additional_discount"].ToString(),
                            freight_charges = dt["freight_charges"].ToString(),
                            buyback_charges = dt["buyback_charges"].ToString(),
                            total_price = dt["total_amount"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            packing_charges = dt["packing_charges"].ToString(),
                            insurance_charges = dt["insurance_charges"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            total_amount = dt["total_price"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            pricesegment_name = dt["pricesegment_name"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            tax_amount4 = dt["tax_amount4"].ToString(),
                            tax_name4 = dt["tax_name4"].ToString(),
                            salesperson_name = dt["salesperson_name"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            customer_pin = dt["customer_pin"].ToString(),
                            customer_state = dt["customer_state"].ToString(),
                            customer_city = dt["customer_city"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            tax_nameno = dt["tax_name"].ToString(),
                            pincode = dt["pincode"].ToString(),

                        });
                        values.SO_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Quootation View !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        private DataTable GetTaxDetailsForProduct(string product_gid, string customer_gid)
        {
            // Query tax segment details based on product_gid
            msSQL = "SELECT f.taxsegment_gid, d.taxsegment_gid, e.taxsegment_name, d.tax_name, d.tax_gid, " +
                "CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, " +
                "d.tax_amount, a.mrp_price, a.cost_price, a.product_gid " +
                "FROM acp_mst_ttaxsegment2product d " +
                "LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                "LEFT JOIN crm_mst_tcustomer f ON f.taxsegment_gid = e.taxsegment_gid " +
                "LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid " +
                "WHERE a.product_gid = '" + product_gid + "' AND f.customer_gid = '" + customer_gid + "'";

            // Execute query to get tax segment details
            return objdbconn.GetDataTable(msSQL);
        }

        // Product Details for View

        public void DaGetViewquotationdetails(string quotation_gid, string customer_gid, MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = "select d.product_remarks,a.quotation_gid,d.productgroup_name,d.slno,d.product_name,d.uom_name,format(d.tax_amount, 2) as tax_amount,format(d.tax_amount2, 2) as tax_amount2,d.product_gid,d.product_code," +
                    " d.uom_name,d.qty_quoted,format(d.product_price, 2) as product_price,d.discount_percentage,format(d.discount_amount, 2) as discount_amount,d.tax_name,d.tax_name2,format(d.price, 2) as price ," +
                    " c.tax_prefix as tax_prefix1,e.tax_prefix as tax_prefix2 " +
                    " FROM smr_trn_treceivequotation a " +
                    " LEFT JOIN smr_trn_treceivequotationdtl d ON d.quotation_gid = a.quotation_gid" +
                    " left join acp_mst_ttax c on c.tax_gid = d.tax2_gid " +
                    " left join acp_mst_ttax e on e.tax_gid = d.tax1_gid " +
                    " WHERE a.quotation_gid = '" + quotation_gid + "' ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<postsalesquotationdetails_list>();
                var getGetTaxSegmentList = new List<GetTaxSegmentListView>();

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new postsalesquotationdetails_list
                        {
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            discount_amount = dt["discount_amount"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            discount_percentage = dt["discount_percentage"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_prefix1 = dt["tax_prefix1"].ToString(),
                            tax_prefix2 = dt["tax_prefix2"].ToString(),
                            slno = dt["slno"].ToString(),
                            price = dt["price"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),


                        });
                        if (!string.IsNullOrEmpty(customer_gid) && customer_gid != "undefined" && customer_gid != "0")
                        {
                            // Query tax segment details based on product_gid
                            string product_gid = dt["product_gid"].ToString();
                            DataTable taxSegmentDataTable = GetTaxDetailsForProduct(product_gid, customer_gid);

                            // Add tax segment details to the list
                            foreach (DataRow taxSegmentRow in taxSegmentDataTable.Rows)
                            {
                                getGetTaxSegmentList.Add(new GetTaxSegmentListView
                                {
                                    taxsegment_gid = taxSegmentRow["taxsegment_gid"].ToString(),
                                    product_gid = taxSegmentRow["product_gid"].ToString(),
                                    taxsegment_name = taxSegmentRow["taxsegment_name"].ToString(),
                                    tax_name = taxSegmentRow["tax_name"].ToString(),
                                    tax_percentage = taxSegmentRow["tax_percentage"].ToString(),
                                    tax_gid = taxSegmentRow["tax_gid"].ToString(),
                                    mrp_price = taxSegmentRow["mrp_price"].ToString(),
                                    cost_price = taxSegmentRow["cost_price"].ToString(),
                                    tax_amount = taxSegmentRow["tax_amount"].ToString(),
                                });
                            }
                        }
                        values.Sq_list = getModuleList;
                        values.GetTaxSegmentListView = getGetTaxSegmentList;

                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Sales Order Summary !";

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetOnchangeCurrency(string currencyexchange_gid, MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = " select currencyexchange_gid,currency_code,exchange_rate from crm_trn_tcurrencyexchange " +
                " where currencyexchange_gid='" + currencyexchange_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOnchangecurrency>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOnchangecurrency
                        {

                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                        });
                        values.GetOnchangecurrency = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured whileGetting Currency Code !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public void DaGetOnChangeCurrencyexhange(string currency_code, MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = " select currencyexchange_gid,currency_code,exchange_rate from crm_trn_tcurrencyexchange " +
                " where currency_code='" + currency_code + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetOnchangecurrency>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetOnchangecurrency
                        {

                            exchange_rate = dt["exchange_rate"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                        });
                        values.GetOnchangecurrency = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured whileGetting Currency Code !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetProductdetails(string quotation_gid, MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = "select product_gid,product_code,product_name,qty_quoted from smr_trn_treceivequotationdtl where quotation_gid='" + quotation_gid + "' group by quotationdtl_gid";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<productlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new productlist
                        {

                            product_code = dt["product_code"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                        });
                        values.product_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product Detailes !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DagetDeleteQuotation(string quotation_gid, string employee_gid, MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = " insert into smr_trn_tsalesdelete ( " +
                    " record_gid, " +
                    " deleted_by, " +
                    " deleted_date, " +
                    " record_reference " +
                   " ) values ( " +
              " '" + quotation_gid + "', " +
              " '" + employee_gid + "', " +
              "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
              " 'Quotation') ";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    msSQL = " update smr_trn_treceivequotation set delete_flag='Y' where quotation_gid='" + quotation_gid + "' ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Quotation Deleted Successfully";
                }
                else
                {
                    {
                        values.status = false;
                        values.message = "Error While Deleting Quotation";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Deleting Quotation!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public quotationhistorylist DaGetquotationhistorydata(string quotation_gid)
        {
            try
            {
                quotationhistorylist objquotationhistorylist = new quotationhistorylist();
                {

                    msSQL = " select a.quotation_gid,a.quotation_referencenumber,date_format(a.quotation_date,'%d-%m-%Y') as quotation_date,a.customer_name, " +
                            " a.quotation_remarks,f.enquiry_referencenumber from smr_trn_treceivequotation a " +
                            " left join acp_trn_tenquiry f on a.customer_gid=f.customer_gid " +
                            "  where a.quotation_gid='" + quotation_gid + "'";
                }

                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows)
                {
                    
                    objquotationhistorylist.quotation_gid = objOdbcDataReader["quotation_referencenumber"].ToString();
                    objquotationhistorylist.quotation_referenceno1 = objOdbcDataReader["enquiry_referencenumber"].ToString();
                    objquotationhistorylist.quotation_date = objOdbcDataReader["quotation_date"].ToString();
                    objquotationhistorylist.customer_name = objOdbcDataReader["customer_name"].ToString();
                    objquotationhistorylist.quotation_remarks = objOdbcDataReader["quotation_remarks"].ToString();

                    objOdbcDataReader.Close();
                }
                return objquotationhistorylist;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }



        public void DaGetquotationhistorysummarydata(MdlSmrTrnQuotation values, string quotation_gid)
        {
            try
            {

                msSQL = "select currency_code from crm_trn_tcurrencyexchange where default_currency='Y'";
                string currency = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select distinct a.quotation_gid,d.customer_gid, a.quotation_referencenumber,a.quotation_referenceno1,date_format(a.quotation_date,'%d-%m-%Y') as quotation_date,a.customer_name,c.user_firstname, " +
                        " case when a.grandtotal_l ='0.00' then format(a.Grandtotal,2) else format(a.grandtotal_l,2) end as Grandtotal," +
                        " case when a.currency_code = '" + currency + "' then a.customer_name " +
                        "  when a.currency_code is null then a.customer_name " +
                        "  when a.currency_code is not null and a.currency_code <> '" + currency +
                        "' then concat(a.customer_name,' / ',h.country) end as customer_name, " +
                        "  a.customer_contact_person, a.quotation_status,concat(e.customercontact_name,'/',e.mobile,'/',e.email) as contact,a.enquiry_gid " +
                        " from smr_trn_treceivequotation a " +
                        " left join hrm_mst_temployee b on b.employee_gid=a.created_by " +
                        " left join adm_mst_tuser c on b.user_gid= c.user_gid " +
                        " left join crm_mst_tcustomer d on d.customer_gid=a.customer_gid " +
                        " left join crm_trn_tcurrencyexchange h on a.currency_code = h.currency_code " +
                        " left join crm_mst_tcustomercontact e on e.customer_gid=d.customer_gid " +
                        " where 1=1 and a.quotation_status='Quotation Amended' and a.quotation_gid like '" + quotation_gid + "%" + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<quotationhistorysummarylist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new quotationhistorysummarylist
                        {
                            quotation_date = dt["quotation_date"].ToString(),
                            quotation_referenceno1 = dt["quotation_referencenumber"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            customer_contact_person = dt["customer_contact_person"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            quotation_status = dt["quotation_status"].ToString(),
                            quotation_gid = dt["quotation_gid"].ToString(),
                            customer_gid = dt["customer_gid"].ToString()


                        });
                        values.quotationhistorysummarylist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Quotation history!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetquotationproductdetails(string quotation_gid, MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = " select a.qty_quoted,a.product_name from smr_trn_treceivequotationdtl a " +
                    " left join pmr_mst_tproduct b on a.product_gid = b.product_gid where a.quotation_gid='" + quotation_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<quotationproduct_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new quotationproduct_list
                        {

                            product_name = dt["product_name"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),


                        });
                        values.quotationproduct_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        //mail function

        public void DaGetTemplatelist(MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = " select a.template_gid, c.template_name, c.template_content from adm_trn_ttemplate2module a " +
                 " left join adm_mst_tmodule b on a.module_gid = b.module_gid " +
                 " left join adm_mst_ttemplate c on a.template_gid = c.template_gid " +
                 " left join adm_mst_ttemplatetype d on c.templatetype_gid = d.templatetype_gid " +
                 " where a.module_gid = 'SMR' and c.templatetype_gid='2' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<templatelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new templatelist
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            template_content = dt["template_content"].ToString(),
                        });
                        values.templatelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Template Name!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            try
            {
                msSQL = " select pop_server,pop_port,pop_username,pop_password,company_name,company_code from adm_mst_tcompany where company_gid='1'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.pop_mail = dt_datatable.Rows[0]["pop_username"].ToString();
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Pop User Mail !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }
        public void DaGetTemplatet(string template_gid, MdlSmrTrnQuotation values)
        {
            try
            {



                msSQL = " select a.template_gid, c.template_name, c.template_content from adm_trn_ttemplate2module a " +
                 " left join adm_mst_tmodule b on a.module_gid = b.module_gid " +
                 " left join adm_mst_ttemplate c on a.template_gid = c.template_gid " +
                 " left join adm_mst_ttemplatetype d on c.templatetype_gid = d.templatetype_gid " +
                 " where a.module_gid = 'SMR' and c.template_gid='" + template_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<templatelist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new templatelist
                        {
                            template_gid = dt["template_gid"].ToString(),
                            template_name = dt["template_name"].ToString(),
                            template_content = dt["template_content"].ToString(),
                        });
                        values.templatelist = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Template Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        public graphtoken generateGraphAccesstoken()
        {
            graphtoken objtoken = new graphtoken();
            mdlgraph_list objMdlGraph = new mdlgraph_list();

            try
            {
                msSQL = "select client_id,client_secret,tenant_id from crm_smm_outlook_service";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader != null && objOdbcDataReader.HasRows)
                {
                    
                    objMdlGraph.tenantID = objOdbcDataReader["tenant_id"].ToString();
                    objMdlGraph.clientID = objOdbcDataReader["client_id"].ToString();
                    objMdlGraph.client_secret = objOdbcDataReader["client_secret"].ToString();
                   
                }

                if (!string.IsNullOrEmpty(objMdlGraph.tenantID) && !string.IsNullOrEmpty(objMdlGraph.clientID) && !string.IsNullOrEmpty(objMdlGraph.client_secret))
                {
                    msSQL = "select token,expiry_time from crm_smm_tgraphtoken";
                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                    if (objOdbcDataReader != null && objOdbcDataReader.HasRows)
                    {
                        
                        DateTime expiry = DateTime.Parse(objOdbcDataReader["expiry_time"].ToString());
                        if (DateTime.Compare(expiry, DateTime.Now) < 0)
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            var client = new RestClient(ConfigurationManager.AppSettings["GraphLoginURL"].ToString());
                            var request = new RestRequest("/" + objMdlGraph.tenantID + "/oauth2/v2.0/token", Method.POST);
                            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                            request.AddParameter("client_id", objMdlGraph.clientID);
                            request.AddParameter("scope", ConfigurationManager.AppSettings["GraphLoginScope"].ToString());
                            request.AddParameter("client_secret", objMdlGraph.client_secret);
                            request.AddParameter("grant_type", "client_credentials");
                            IRestResponse response = client.Execute(request);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                graphLoginSuccessResponse objgraphLoginSuccessResponse = new graphLoginSuccessResponse();
                                objgraphLoginSuccessResponse = JsonConvert.DeserializeObject<graphLoginSuccessResponse>(response.Content);
                                objtoken.access_token = objgraphLoginSuccessResponse.access_token;
                                objtoken.status = true;
                                msSQL = "update crm_smm_tgraphtoken set token = '" + objtoken.access_token +
                                        "',expiry_time = '" + DateTime.Now.AddSeconds(3595).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                                int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                if (mnResult == 0)
                                {
                                    objcmnfunctions.LogForAudit("Error occurred while insert: " + msSQL);
                                }
                            }
                        }
                        else
                        {
                            objtoken.access_token = objOdbcDataReader["token"].ToString();
                            objtoken.status = true;
                        }
                        objOdbcDataReader.Close();


                    }
                    else
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["GraphLoginURL"].ToString());
                        var request = new RestRequest("/" + objMdlGraph.tenantID + "/oauth2/v2.0/token", Method.POST);
                        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                        request.AddParameter("client_id", objMdlGraph.clientID);
                        request.AddParameter("scope", ConfigurationManager.AppSettings["GraphLoginScope"].ToString());
                        request.AddParameter("client_secret", objMdlGraph.client_secret);
                        request.AddParameter("grant_type", "client_credentials");
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            graphLoginSuccessResponse objgraphLoginSuccessResponse = new graphLoginSuccessResponse();
                            objgraphLoginSuccessResponse = JsonConvert.DeserializeObject<graphLoginSuccessResponse>(response.Content);
                            objtoken.access_token = objgraphLoginSuccessResponse.access_token;
                            objtoken.status = true;
                            msSQL = "insert into crm_smm_tgraphtoken(token,expiry_time)values(" +
                                    "'" + objtoken.access_token + "'," +
                                    "'" + DateTime.Now.AddSeconds(3595).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            int mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult == 0)
                            {
                                objcmnfunctions.LogForAudit("Error occurred while insert: " + msSQL);
                            }
                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("Error while generating access token: " + response.Content.ToString());
                        }
                    }
                }
                else
                {
                    objtoken.message = "Kindly add the app details for sending mails!";
                }

            }
            catch (Exception ex)
            {
                objtoken.message = ex.Message;
                objcmnfunctions.LogForAudit("Exception while generating access token: " + ex.ToString());
            }
            return objtoken;
        }
        public gmailconfiguration gmailcrendentials()
        {
            gmailconfiguration getgmailcredentials = new gmailconfiguration();
            try
            {
                msSQL = "select * from  smr_smm_gmail_service ;";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows == true)
                {

                    getgmailcredentials.client_id = objOdbcDataReader["client_id"].ToString();
                    getgmailcredentials.client_secret = objOdbcDataReader["client_secret"].ToString();
                    getgmailcredentials.refresh_token = objOdbcDataReader["refresh_token"].ToString();
                    getgmailcredentials.gmail_address = objOdbcDataReader["gmail_address"].ToString();
                    getgmailcredentials.default_template = objOdbcDataReader["default_template"].ToString();

                    objOdbcDataReader.Close();
                }
                
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
            return getgmailcredentials;
        }
        public void DaPostMail(HttpRequest httpRequest, string user_gid, result objResult)
        {
            string lscompany_code = "";
            msSQL = " select company_code from adm_mst_tcompany";
            string lscompany_codecheck = objdbconn.GetExecuteScalar(msSQL);

            if (lscompany_codecheck == "BOBA" || lscompany_codecheck == "MEDIA")
            {



                string final_path = string.Empty;
                string finalEmailBody = string.Empty;
                string msdocument_gid = string.Empty;

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                gmailconfiguration getgmailcredentials = gmailcrendentials();

                var options = new RestClient("https://accounts.google.com");
                var request = new RestRequest("/o/oauth2/token", Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Cookie", "__Host-GAPS=1:dSJc6jGXysjEnllWZJpCwl9LCgsLcw:IJtLApP2pdsRQh0Y; __Host-GAPS=1:dSJc6jGXysjEnllWZJpCwl9LCgsLcw:IJtLApP2pdsRQh0Y; NID=511=Kbcaybba2NN2meR6QaA1TfflTMD5X6JNa-vmRpwiHUMRZn4ru7MVloJIso4PHGVH40ORQYUKH1LJvUttyG79Vi5eCXIX4UgloYTQFHN0qYHR67sn7fC32LHA7Cfdbgz4G6g5FhOnXus0SVeSyNx4QPRGFQPakVRKwNlBIxK8FGc; __Host-GAPS=1:7IaKbeoNAD6DkBpPrg7Pl6ppMRE7yQ:a2G_qIglM2A-euWd");
                var body = @"{" + "\n" +
                @"    ""client_id"": " + "\"" + getgmailcredentials.client_id + "\"" + "," + "\n" +

                @"    ""client_secret"":  " + "\"" + getgmailcredentials.client_secret + "\"" + "," + "\n" +
                @"    ""grant_type"": ""refresh_token"",
                          " + "\n" +
                @"    ""refresh_token"": " + "\"" + getgmailcredentials.refresh_token + "" + "\"" + "\n" + @"}";

                request.AddParameter("application/json", body, RestSharp.ParameterType.RequestBody);
                IRestResponse response = options.Execute(request);
                string errornetsuiteJSON = response.Content;
                refreshtokenlist objMdlGmailCampaignResponse = new refreshtokenlist();
                objMdlGmailCampaignResponse = JsonConvert.DeserializeObject<refreshtokenlist>(errornetsuiteJSON);


                // attachment get function

                HttpFileCollection httpFileCollection;
                string basecode = httpRequest.Form["gmailfiles"];
                //List<sendmail_list> hrDocuments = JsonConvert.DeserializeObject<List<sendmail_list>>(basecode);

                //split function

                string employee_emailid = httpRequest.Form["employee_emailid"];
                string sub = httpRequest.Form["sub"];
                string to = httpRequest.Form["to"];
                string[] to_mails = to.Split(',');
                string bodies = httpRequest.Form["body"];
                string cc = httpRequest.Form["cc"];
                string[] cc_mails = cc.Split(',');
                string bcc = httpRequest.Form["bcc"];
                string[] bcc_mails = bcc.Split(',');
                string quotation_gid = httpRequest.Form["quotation_gid"];
                //string bcc = httpRequest.Form["bcc"];
                //string cc = httpRequest.Form["cc"];


                List<DbAttachmentPath> dbattachmentpath = new List<DbAttachmentPath>();
                List<MailAttachmentbase64> mailAttachmentbase64 = new List<MailAttachmentbase64>();
                string lsfilepath = string.Empty;
                string document_gid = string.Empty;
                string lspath, lspath1;
                string FileExtension = string.Empty;
                string file_name = string.Empty;
                string httpsUrl = string.Empty;

                HttpPostedFile httpPostedFile;

                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        MemoryStream ms = new MemoryStream();
                        //var getdocumentdtl = hrDocuments.Where(a => a.AutoID_Key == httpFileCollection.AllKeys[i]).FirstOrDefault();
                        msdocument_gid = objcmnfunctions.GetMasterGID("GILC");
                        httpPostedFile = httpFileCollection[i];
                        file_name = httpPostedFile.FileName;
                        string type = httpPostedFile.ContentType;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(file_name).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);
                        string base64String = string.Empty;
                        bool status1;


                        status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "Mail/Post/Purchase/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        httpsUrl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                               '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                               '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];

                        byte[] fileBytes = ms.ToArray();
                        base64String = Convert.ToBase64String(fileBytes);

                        ms.Close();

                        dbattachmentpath.Add(new DbAttachmentPath
                        { path = httpsUrl }
                        );
                        mailAttachmentbase64.Add(new MailAttachmentbase64
                        {
                            name = httpPostedFile.FileName,
                            type = type,
                            data = base64String

                        });
                    }
                    StringBuilder emailBody = new StringBuilder();
                    string boundary = "--boundary_example";
                    foreach (var attachment in mailAttachmentbase64)
                    {
                        emailBody.AppendLine("Content-Type: " + attachment.type + "; charset=UTF-8");
                        emailBody.AppendLine("Content-Transfer-Encoding: base64");
                        emailBody.AppendLine("Content-Disposition: attachment; filename=\"" + attachment.name + "\"");
                        emailBody.AppendLine();
                        emailBody.AppendLine(attachment.data);
                        emailBody.AppendLine(boundary);
                    }
                    finalEmailBody = emailBody.ToString();
                    string cc_emailString = String.Join(", ", cc_mails);
                    string to_emailString = String.Join(", ", to_mails);
                    string bcc_emailString = String.Join(", ", bcc_mails);
                    var options1 = new RestClient("https://www.googleapis.com");
                    var request1 = new RestRequest("/upload/gmail/v1/users/me/messages/send?uploadType=media", Method.POST);
                    request1.AddHeader("Authorization", "Bearer  " + objMdlGmailCampaignResponse.access_token + "");
                    request1.AddHeader("Content-Type", "message/rfc822");
                    request1.AddHeader("Cookie", "COMPASS=gmail-api-uploads-blobstore=CgAQ18HPrwYagAEACWuJVwxF5peESk5gbz5AE37T8tg_Yoh-YDKTZilInpK22DHDg7FuuU3LXoN11GAOnyqdLslKu6I5ePsGXCEsd3xSS2yUEWvqsZJtNX4R-ajkWB37okK3XRlg3MQM0P22BdEB5efrYEFEwlQWnrxQUPmucfMFffcwQAMnVi0yfzAB; COMPASS=gmail-api-uploads-blobstore=CgAQnrbWrwYagAEACWuJVwxF5peESk5gbz5AE37T8tg_Yoh-YDKTZilInpK22DHDg7FuuU3LXoN11GAOnyqdLslKu6I5ePsGXCEsd3xSS2yUEWvqsZJtNX4R-ajkWB37okK3XRlg3MQM0P22BdEB5efrYEFEwlQWnrxQUPmucfMFffcwQAMnVi0yfzAB; COMPASS=gmail-api-uploads-blobstore=CgAQ3M3WrwYagAEACWuJV70L2ArobrIhJ3QHHVMUMuhVzIt7hY_BOzuIcJ9f8aTM0lWNTsBGq8iRVZqbbVDXK1zOu9pSPMnm5hcrkX1dIku9gne04K4azeD3LO9TlrMLOaKbRMBzaLZZEsjzHG9ogDw5OoF3IB-_eL6aX22cxCxfAiuXIJU9MPFqsDAB");
                    var body1 = @"From:" + getgmailcredentials.gmail_address + "" + "\n" +
                    @"To:" + to_emailString + "" + "\n" +
                     @"Cc:" + cc_emailString + "" + "\n" +
                     @"Bcc:" + bcc_emailString + "" + "\n" +
                    @"Subject: " + sub + "" + "\n" +
                    @"MIME-Version: 1.0" + "\n" +
                    @"Content-Type: multipart/mixed; boundary=""boundary_example""" + "\n" +
                    @"" + "\n" +
                    @"--boundary_example" + "\n" +
                    @"Content-Type: text/html; charset=""UTF-8""" + "\n" +
                    @"MIME-Version: 1.0" + "\n" +
                    @"" + "\n" +
                    @"<html>" + "\n" +
                    @"  <body>" + "\n" +
                    @"    <p>" + bodies + "</p>" + "\n" +
                    @"    <p>" + getgmailcredentials.default_template + "</p>" + "\n" +
                    @"  </body>" + "\n" +
                    @"</html>" + "\n" +
                    @"" + "\n" +
                    @"--boundary_example" + "\n" +
                    finalEmailBody +
                    "\n";
                    request1.AddParameter("message/rfc822", body1, RestSharp.ParameterType.RequestBody);
                    IRestResponse response1 = options1.Execute(request1);
                    string errornetsuiteJSON1 = response1.Content;
                    responselist objMdlGmailCampaignResponse1 = new responselist();
                    objMdlGmailCampaignResponse1 = JsonConvert.DeserializeObject<responselist>(errornetsuiteJSON);
                    if (response1.StatusCode == HttpStatusCode.OK)
                    {
                        msSQL = "INSERT INTO crm_trn_gmail (" +
                              "gmail_gid, " +
                            "from_mailaddress, " +
                            "to_mailaddress, " +
                            "mail_subject, " +
                            "mail_body, " +
                            "transmission_id, " +
                            "leadbank_gid, " +
                             " created_by, " +
                            "created_date) " +
                            "VALUES (" +
                             "'" + msdocument_gid + "', " +
                            "'" + getgmailcredentials.gmail_address + "', " +
                            "'" + to + "', " +
                            "'" + sub.Replace("'", "\\\'") + "', " +
                            "'" + bodies.Replace("'", "\\\'") + "', " +
                            "'" + objMdlGmailCampaignResponse1.id + "', " +
                            "'" + quotation_gid + "', " +
                              "'" + user_gid + "'," +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        for (int i = 0; i < dbattachmentpath.Count; i++)
                        {
                            msGetGid1 = objcmnfunctions.GetMasterGID("UPLF");
                            msSQL = "INSERT INTO crm_trn_tfiles (" +
                             "file_gid, " +
                            "mailmanagement_gid, " +
                            "document_name, " +
                          "document_path, " +
                            "created_date) " +
                            "VALUES (" +
                             "'" + msGetGid1 + "', " +
                            "'" + msdocument_gid + "', " +
                            "'" + mailAttachmentbase64[i].name + "', " +
                             "'" + dbattachmentpath[i].path + "', " +
                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                        for (int i = 0; i < dbattachmentpath.Count; i++)
                        {
                            string encodedBody = EncodeToBase64(bodies);
                            string encodedsub = EncodeToBase64(sub);

                           
                            msSQL = "INSERT INTO smr_trn_gmailattachment (" +
                                  "gmail_gid, " +
                                "from_mailaddress, " +
                                "to_mailaddress, " +
                                "mail_subject, " +
                                "mail_body, " +
                                "transmission_id, " +
                                 "document_name, " +
                              "document_path, " +
                              "file_gid, " +
                                 " created_by, " +
                                "created_date) " +
                                "VALUES (" +
                                 "'" + msGetGid1 + "', " +
                                "'" + employee_emailid + "', " +
                                "'" + to + "', " +
                                "'" + encodedsub + "', " +
                                "'" + encodedBody + "', " +
                                "'" + objMdlGmailCampaignResponse1.id + "', " +
                                 "'" + mailAttachmentbase64[i].name + "', " +
                                 "'" + dbattachmentpath[i].path + "', " +
                                  "'" + quotation_gid + "', " +
                                  "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }
                    }
                }
            }
            else { 

            msSQL = " select company_code from adm_mst_tcompany";
             lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            // attachment get function

            HttpFileCollection httpFileCollection;
            string basecode = httpRequest.Form["gmailfiles"];

            graphtoken objtoken = new graphtoken();

            //split function

           
            List<DbAttachmentPath> dbattachmentpath = new List<DbAttachmentPath>();
            List<MailAttachmentbase64> mailAttachmentbase64 = new List<MailAttachmentbase64>();
            string lsfilepath = string.Empty;
            string document_gid = string.Empty;
            string lspath, lspath1;
            string FileExtension = string.Empty;
            string file_name = string.Empty;
            string httpsUrl = string.Empty;

            HttpPostedFile httpPostedFile;

                if (httpRequest.Files.Count > 0)
                {
                    string lsfirstdocument_filepath = string.Empty;
                    httpFileCollection = httpRequest.Files;
                    for (int i = 0; i < httpFileCollection.Count; i++)
                    {
                        MemoryStream ms = new MemoryStream();
                        //var getdocumentdtl = hrDocuments.Where(a => a.AutoID_Key == httpFileCollection.AllKeys[i]).FirstOrDefault();

                        string msGet_att_Gid = objcmnfunctions.GetMasterGID("BEAC");
                        httpPostedFile = httpFileCollection[i];
                        file_name = httpPostedFile.FileName;
                        string type = httpPostedFile.ContentType;
                        string lsfile_gid = msGet_att_Gid;
                        string lscompany_document_flag = string.Empty;
                        FileExtension = Path.GetExtension(file_name).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);
                        string base64String = string.Empty;
                        bool status1;
                        string final_path;


                        status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "Mail/Post/Purchase/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msGet_att_Gid + FileExtension, FileExtension, ms);
                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Mail/post/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        httpsUrl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msGet_att_Gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                               '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                               '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];

                        byte[] fileBytes = ms.ToArray();
                        base64String = Convert.ToBase64String(fileBytes);



                        dbattachmentpath.Add(new DbAttachmentPath
                        { path = httpsUrl }
                        );
                        mailAttachmentbase64.Add(new MailAttachmentbase64
                        {
                            name = httpPostedFile.FileName,
                            type = type,
                            data = base64String

                        });
                        ms.Close();
                        objtoken = generateGraphAccesstoken();
                    }

                    if (objtoken.status)
                    {
                        string employee_emailid = httpRequest.Form["employee_emailid"];
                        string sub = httpRequest.Form["sub"];

                        string bodies = httpRequest.Form["body"];


                        string invoice_gid = httpRequest.Form["quotation_gid"];
                        string cc_mail = httpRequest.Form["cc"];
                        string to_mail = httpRequest.Form["to"];
                        string bcc = httpRequest.Form["bcc"];

                        MdlGraphMailContent objMdlGraphMailContent = new MdlGraphMailContent();
                        objMdlGraphMailContent.message = new Message1();
                        objMdlGraphMailContent.saveToSentItems = true;
                        objMdlGraphMailContent.message.body = new Body2();
                        objMdlGraphMailContent.message.body.contentType = "HTML";
                        objMdlGraphMailContent.message.body.content = bodies;
                        objMdlGraphMailContent.message.subject = sub;
                        string[] to_mails = to_mail.Split(',');
                        string[] cc_mails = null;
                        string[] bcc_mails = null;

                        if (cc_mail != null && cc_mail != "")
                        {
                            cc_mails = cc_mail.Split(',');
                        }
                        if (bcc != null && bcc != "")
                        {
                            bcc_mails = bcc.Split(',');
                        }

                        //string[] files = lspath2.Split(',');

                        if (to_mails != null)
                        {
                            objMdlGraphMailContent.message.toRecipients = new Torecipient[to_mails.Length];
                            for (int i = 0; i < objMdlGraphMailContent.message.toRecipients.Length; i++)
                            {
                                objMdlGraphMailContent.message.toRecipients[i] = new Torecipient();
                                objMdlGraphMailContent.message.toRecipients[i].emailAddress = new Emailaddress();
                                objMdlGraphMailContent.message.toRecipients[i].emailAddress.address = to_mails[i];
                            }
                        }

                        if (cc_mails != null && cc_mails.Length != 0)
                        {
                            objMdlGraphMailContent.message.ccRecipients = new Torecipient[cc_mails.Length];
                            for (int i = 0; i < objMdlGraphMailContent.message.ccRecipients.Length; i++)
                            {
                                objMdlGraphMailContent.message.ccRecipients[i] = new Torecipient();
                                objMdlGraphMailContent.message.ccRecipients[i].emailAddress = new Emailaddress();
                                objMdlGraphMailContent.message.ccRecipients[i].emailAddress.address = cc_mails[i];
                            }
                        }
                        if (bcc_mails != null && bcc_mails.Length != 0)
                        {
                            objMdlGraphMailContent.message.bccRecipients = new Torecipient[bcc_mails.Length];
                            for (int i = 0; i < objMdlGraphMailContent.message.bccRecipients.Length; i++)
                            {
                                objMdlGraphMailContent.message.bccRecipients[i] = new Torecipient();
                                objMdlGraphMailContent.message.bccRecipients[i].emailAddress = new Emailaddress();
                                objMdlGraphMailContent.message.bccRecipients[i].emailAddress.address = bcc_mails[i];
                            }
                        }

                        if (httpFileCollection.Count != null && httpFileCollection.Count != 0)
                        {
                            objMdlGraphMailContent.message.attachments = new attachments[mailAttachmentbase64.Count];
                            for (int i = 0; i < mailAttachmentbase64.Count; i++)
                            {
                                objMdlGraphMailContent.message.attachments[i] = new attachments();
                                objMdlGraphMailContent.message.attachments[i].name = mailAttachmentbase64[i].name;
                                objMdlGraphMailContent.message.attachments[i].contentBytes = mailAttachmentbase64[i].data;
                                objMdlGraphMailContent.message.attachments[i].OdataType = "#microsoft.graph.fileAttachment";
                            }
                        }

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient(ConfigurationManager.AppSettings["GraphSendURL"].ToString());
                        var request = new RestRequest("/v1.0/users/" + employee_emailid + "/sendMail", Method.POST);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", objtoken.access_token);
                        string request_body = JsonConvert.SerializeObject(objMdlGraphMailContent);
                        request.AddParameter("application/json", request_body, RestSharp.ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        if (response.StatusCode == HttpStatusCode.Accepted)
                        {
                            string encodedBody = EncodeToBase64(bodies);
                            string encodedsub = EncodeToBase64(sub);
                            for (int i = 0; i < dbattachmentpath.Count; i++)
                            {
                                msGetGid1 = objcmnfunctions.GetMasterGID("UPLF");

                                msSQL = "INSERT INTO smr_trn_gmailattachment (" +
                                      "gmail_gid, " +
                                    "from_mailaddress, " +
                                    "to_mailaddress, " +
                                    "mail_subject, " +
                                    "mail_body, " +
                                     "document_name, " +
                                  "document_path, " +
                                  "file_gid, " +
                                     " created_by, " +
                                    "created_date) " +
                                    "VALUES (" +
                                     "'" + msGetGid1 + "', " +
                                    "'" + employee_emailid + "', " +
                                    "'" + to_mail + "', " +
                                    "'" + encodedsub + "', " +
                                    "'" + encodedBody + "', " +
                                     "'" + mailAttachmentbase64[i].name + "', " +
                                     "'" + dbattachmentpath[i].path + "', " +
                                      "'" + invoice_gid + "', " +
                                      "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + response.Content.ToString() + "**************Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                        }
                    }
                }

            }

            if (mnResult == 1)
            {
                objResult.status = true;
                objResult.message = "Mail Sent Successfully !!";
            }
            else
            {
                objResult.status = false;
                objResult.message = " Mail Not Sent !! ";
            }

        }
        private static string EncodeToBase64(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }


        // DIRECT QUOTATION PRODUCT SUMMARY

        public void DaGetDirectQuotationEditProductSummary(string tmpquotationdtl_gid, MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = " Select tmpquotationdtl_gid,quotation_gid,product_gid,productgroup_gid,tax1_gid," +
                    " productgroup_name,product_name,qty_quoted,discount_percentage,discount_amount,product_price," +
                    " tax_percentage,tax_amount,uom_gid,uom_name,price,tax_name,product_code,taxsegment_gid from smr_tmp_treceivequotationdtl" +
                    " where tmpquotationdtl_gid = '" + tmpquotationdtl_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<DirecteditQuotationList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new DirecteditQuotationList
                        {
                            tmpquotationdtl_gid = dt["tmpquotationdtl_gid"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            selling_price = dt["product_price"].ToString(),
                            discountpercentage = dt["discount_percentage"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            totalamount = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_gid = dt["tax1_gid"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),

                        });
                        values.directeditquotation_list = getModuleList;
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  DirectQuotationEditProductSummary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void DaGetRaiseQuotedetail(string product_gid, MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = " select a.quotation_gid,a.quotationdtl_gid,a.customerproduct_code,a.product_gid,b.currency_code,a.product_requireddate as product_requireddate," +
                    " d.product_name,date_format(b.quotation_date,'%d-%m-%Y') as quotation_date," +
                    " b.customer_gid,b.customer_name,a.qty_quoted,format(a.product_price,2) as product_price,c.leadbank_name " +
                    " from smr_trn_treceivequotationdtl a left join smr_trn_treceivequotation b on a.quotation_gid=b.quotation_gid " +
                    " left join pmr_mst_tproduct d on a.product_gid = d.product_gid " +
                    " left join crm_trn_tleadbank c on b.customer_gid=c.leadbank_gid " +
                    " where a.product_gid='" + product_gid + "' group by a.product_price " +
                    " order by b.quotation_date desc ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Directeddetailslist1>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Directeddetailslist1
                        {
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            qty_quoted = dt["qty_quoted"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            currency_code = dt["currency_code"].ToString(),

                        });
                        values.Directeddetailslist1 = getModuleList;
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


        //  PRODUCT UPDATE -- DIRECT QUOTATION PRODUCT SUMMARY

        public void DaPostUpdateDirectQuotationProduct(string employee_gid, DirecteditQuotationList values)
        {
            try
            {

                lspercentage1 = "0";
                if (values.product_gid != null)
                {
                    lsproductgid1 = values.product_gid;
                    msSQL = "Select product_name from pmr_mst_tproduct where product_gid='" + lsproductgid1 + "'";
                    values.product_name = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    msSQL = " Select product_gid from pmr_mst_tproduct where product_name = '" + values.product_name.Replace("'", "\\\'") + "'";
                    lsproductgid1 = objdbconn.GetExecuteScalar(msSQL);
                }
                if (values.tax_gid == null)
                {
                    msSQL = "Select percentage from acp_mst_ttax where tax_gid='" + values.tax_gid + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }
                else
                {
                    msSQL = "Select percentage from acp_mst_ttax where tax_gid='" + values.tax_gid + "'";
                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
                }

                msSQL = "Select productuom_gid from pmr_mst_tproductuom where productuom_name='" + values.productuom_name.Replace("'", "\\\'") + "'";
                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                        " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                        " WHERE product_gid='" + lsproductgid1 + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows == true)
                {
                    lsenquiry_type = "Sales";

                    objOdbcDataReader.Close();
                }

                else
                {
                    lsenquiry_type = "Service";
                }
                
                msSQL = " select * from smr_tmp_treceivequotationdtl where product_gid='" + lsproductgid1 + "' and uom_gid='" + lsproductuomgid + "'" +
                        " and product_price='" + values.selling_price + "'" +
                        "  and created_by='" + employee_gid + "' " +
                        " and discount_percentage='" + values.discountpercentage + "' ";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                   
                    msSQL = " update smr_tmp_treceivequotationdtl set qty_quoted='" + Convert.ToDouble(values.quantity) + "', " +
                            " price='" + Convert.ToDouble(values.totalamount) + "'," +
                             " tax_name= '" + values.tax_name + "'," +
                           " tax_amount='" + values.tax_amount + "'" +
                            " where tmpquotationdtl_gid='" + values.tmpquotationdtl_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    objOdbcDataReader.Close();
                }
                else
                {
                    msSQL = " update smr_tmp_treceivequotationdtl set " +
                           " product_gid = '" + lsproductgid1 + "'," +
                           " product_code='" + values.product_code.Replace("'", "\\\'") + "' ," +
                           " product_name= '" + values.product_name.Replace("'", "\\\'") + "'," +
                           " product_price='" + values.selling_price + "'," +
                           " qty_quoted='" + values.quantity + "'," +
                           " discount_percentage='" + values.discountpercentage + "'," +
                           " discount_amount='" + values.discountamount + "'," +
                           " uom_gid = '" + lsproductuomgid + "', " +
                           " uom_name='" + values.productuom_name.Replace("'", "\\\'") + "'," +
                           " price='" + values.totalamount + "'," +
                           " created_by='" + employee_gid + "'," +
                           " tax_name= '" + values.tax_name + "'," +
                           " tax_amount='" + values.tax_amount + "'," +
                           " tax_percentage='" + lspercentage1 + "'" +
                           " where tmpquotationdtl_gid='" + values.tmpquotationdtl_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = " Product Updated Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = " Error While Updating Product";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
               values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
        //download Report files
        public Dictionary<string, object> DaGetQuotationRpt(string quotation_gid, MdlSmrTrnQuotation values)
        {

            OdbcConnection myConnection = new OdbcConnection();
            myConnection.ConnectionString = objdbconn.GetConnectionString();
            OdbcCommand MyCommand = new OdbcCommand();
            MyCommand.Connection = myConnection;
            DataSet myDS = new DataSet();
            OdbcDataAdapter MyDA = new OdbcDataAdapter();
            Fnazurestorage objFnazurestorage = new Fnazurestorage();

            //currency number2words = fnconvertnumbertowords(quotation_gid, "SQ_REPORT");

            msSQL = "SELECT a.quotation_gid ,a.quotation_referenceno1 ," +
                    " DATE_FORMAT(a.quotation_date,'%d-%m-%Y')  as quotation_date," +
                    " Format(a.total_amount,2) as total_amount ,a.payment_days,a.delivery_days ,a.additional_discount ," +
                    " a.termsandconditions,format(a.Grandtotal,2) as Grandtotal ,a.customer_name ,"+
                    " CASE WHEN a.contact_no IS NULL THEN a.contact_mail" +
                     " ELSE CONCAT(a.contact_no, '/', a.contact_mail)" +
                    " END AS contact_no  ,a.currency_code ," +
                    " a.exchange_rate ,a.contact_mail ,Format(a.total_price,2) as total_price ,a.addon_charge ,a.roundoff,a.buyback_charges ," +
                    " a.packing_charges,a.insurance_charges,   "+
                    " c.customer_name  as customername,c.customer_address,c.customer_address2 ,c.customer_pin  ,a.address1 ,case when e.address2 is null then concat(c.customer_address,' ,',c.customer_address2,' ,',c.customer_state)" +
                    " else concat(e.address1,' ,',e.address2,' ,',e.state) end as address2,  " +
                    " d.branch_name,d.address1 as branchaddress,d.city as branchcity,d.state as branchstate,d.postal_code ,case when d.email is null then d.contact_number  else concat(d.contact_number,'/',d.email) end as branchcontactnumber, " +
                    " d.gst_no as branchgst,c.customer_city as DataColumn1,e.state as DataColumn2,e.gst_number as DataColumn3  " +
                    " from smr_trn_treceivequotation a" +
                    " left join hrm_mst_tbranch  d on d.branch_gid = a.branch_gid" +
                    " left join  crm_mst_tcustomer c on c.customer_gid = a.customer_gid "+
                    " left join  crm_mst_tcustomercontact e on e.customer_gid = a.customer_gid "+
                    " WHERE a.quotation_gid = '" + quotation_gid + "'";




            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable1");



            msSQL = "SELECT a.product_gid,case when b.hsn_number is null then a.product_code else b.hsn_number end as product_code ," +
                    " a.slno ,a.tax_percentage, a.productgroup_name,a.product_remarks ,e.productuom_gid,a.margin_percentage,a.selling_price, " +
                    " a.quotation_gid,a.product_name,FORMAT(a.product_price, 2)  AS product_price, " +
                    " a.qty_quoted, " +
                    " a.discount_percentage,a.discount_amount ,a.uom_name,case when a.display_field is null and a.product_remarks is null then a.product_name" +
                    " when a.display_field is null then concat(a.product_name,' - ',a.product_remarks)  when a.product_remarks is null then concat(a.product_name, ' - ',a.display_field) " +
                    " else a.product_name end  as display_field,a.tax_name,a.tax_amount, " +
                    " a.tax_name2,a.tax_amount2,a.tax_name3,a.tax_amount3, " +
                    " b.product_desc AS DataColumn8 " +
                    " from smr_trn_treceivequotationdtl a " +
                    " left join pmr_mst_tproduct b On b.product_gid=a.product_gid " +
                    " left join pmr_mst_tproductuom e on b.productuom_gid = e.productuom_gid"+
                    " left join pmr_mst_tproductgroup c On b.productgroup_gid=c.productgroup_gid " +
                    " WHERE a.quotation_gid='" + quotation_gid + "' " +
                    " order by a.quotationdtl_gid asc";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable2");

            msSQL = " select a.branch_logo_path,c.company_logo_path from hrm_mst_tbranch a" +
                   " left join smr_trn_treceivequotation b on a.branch_gid=b.branch_gid" +
                   " left join adm_mst_tcompany c on 1=1 " +
                   " where b.quotation_gid='" + quotation_gid + "'";
            dt1 = objdbconn.GetDataTable(msSQL);
            DataTable3.Columns.Add("branch_logo_path", typeof(byte[]));
            DataTable3.Columns.Add("company_logo_path", typeof(byte[]));
            if (dt1.Rows.Count != 0)
            {
                foreach (DataRow dr_datarow in dt1.Rows)
                {
                   company_logo_path = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["company_logo_path"].ToString().Replace("../../", ""));
                    branchlogo = HttpContext.Current.Server.MapPath("../../../" + dr_datarow["branch_logo_path"].ToString().Replace("../../", ""));


                    if (System.IO.File.Exists(company_logo_path) == true)
                    { 
                        //Convert  Image Path to Byte
                        
                       
                        company_logo = System.Drawing.Image.FromFile(company_logo_path);
                        branch_logo = System.Drawing.Image.FromFile(branchlogo);
                        byte[] branch_logo_bytes = (byte[])(new ImageConverter()).ConvertTo(branch_logo, typeof(byte[]));
                        byte[] companylogo_bytes = (byte[])(new ImageConverter()).ConvertTo(company_logo, typeof(byte[]));

                        DataRow newRow = DataTable3.NewRow();
                        newRow["branch_logo_path"] = branch_logo_bytes;
                        newRow["company_logo_path"] = companylogo_bytes;
                        DataTable3.Rows.Add(newRow);


                    }
                }
            }
            dt1.Dispose();
            DataTable3.TableName = "DataTable3";
            myDS.Tables.Add(DataTable3);
            msSQL = " select company_code from adm_mst_Tcompany";
            string lscompanycode = objdbconn.GetExecuteScalar(msSQL);
            if (lscompanycode == "BOBA")
            {
                msSQL = "select a.quotation_gid,CONCAT('£ ', CAST(SUM(CASE WHEN a.tax_name LIKE '%ZERO%' THEN (a.qty_quoted * a.product_price - a.discount_amount) ELSE 0 END) AS DECIMAL(10, 2))) AS DataColumn3," +
               " CONCAT('£ ', CAST(SUM(CASE WHEN a.tax_name LIKE '%ZERO%' THEN a.tax_amount ELSE 0 END) AS DECIMAL(10, 2))) AS DataColumn4," +
               " CONCAT('£ ', CAST(SUM(CASE WHEN a.tax_name LIKE '%VAT%' AND a.tax_name NOT LIKE '%ZERO VAT%' THEN (a.qty_quoted * a.product_price - a.discount_amount) ELSE 0 END) AS DECIMAL(10, 2))) AS DataColumn5," +
               " CONCAT('£ ', CAST(SUM(CASE WHEN a.tax_name LIKE '%VAT%' AND a.tax_name NOT LIKE '%ZERO VAT%' THEN a.tax_amount ELSE 0 END) AS DECIMAL(10, 2))) AS DataColumn6," +
               " FORMAT(SUM((a.qty_quoted * a.product_price) - a.discount_amount), 2) AS DataColumn7," +
               " format(sum((a.tax_amount + a.tax_amount2 + a.tax_amount3)),2)  AS DataColumn9" +
               " from smr_trn_treceivequotationdtl a" +
               " left join smr_trn_treceivequotation e on e.quotation_gid=a.quotation_gid " +
               " left join pmr_mst_tproduct b on b.product_gid=a.product_gid " +
               " left join pmr_mst_tproductgroup c on b.productgroup_gid=c.productgroup_gid " +
               " left join pmr_mst_tproductuom d on d.productuom_gid=a.uom_gid " +
               " WHERE a.quotation_gid='" + quotation_gid + "'";
            }
            else
            {
                msSQL = "SELECT a.quotation_gid,a.tax_name AS DataColumn3, sum(a.tax_amount) AS DataColumn4,a.tax_name2 AS DataColumn5," +
                        " sum(a.tax_amount2) AS DataColumn6,Format(SUM((a.tax_amount+a.tax_amount2)),2) AS DataColumn9, " +
                        " FORMAT(SUM((a.qty_quoted * a.product_price) - a.discount_amount), 2) AS DataColumn7 " +
                        " FROM smr_trn_treceivequotationdtl a  " +
                        " LEFT JOIN smr_trn_treceivequotation e ON e.quotation_gid = a.quotation_gid  " +
                        " LEFT JOIN pmr_mst_tproduct b ON b.product_gid = a.product_gid " +
                        " LEFT JOIN pmr_mst_tproductgroup c ON b.productgroup_gid = c.productgroup_gid " +
                        " LEFT JOIN pmr_mst_tproductuom d ON d.productuom_gid = a.uom_gid  " +
                        " WHERE a.quotation_gid='" + quotation_gid + "'";
 

            }
           

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable4");

            msSQL = "select company_gid AS DataColumn1, company_code AS DataColumn2, company_name AS DataColumn3, company_address AS DataColumn4, " +
                    " company_phone AS DataColumn5 ,contactperson_address as DataColumn6 ,occupier_name as DataCoumn7 from adm_mst_tcompany";

            MyCommand.CommandText = msSQL;
            MyCommand.CommandType = System.Data.CommandType.Text;
            MyDA.SelectCommand = MyCommand;
            myDS.EnforceConstraints = false;
            MyDA.Fill(myDS, "DataTable5");

            msSQL = "select salesquotation_rpt from crm_mst_tconfiguration";
            string lspdf = objdbconn.GetExecuteScalar(msSQL);

            ReportDocument oRpt = new ReportDocument();
            oRpt.Load(Path.Combine(ConfigurationManager.AppSettings["report_file_path_sales"].ToString(), lspdf));
            oRpt.SetDataSource(myDS);
            string path = Path.Combine(ConfigurationManager.AppSettings["report_path"].ToString(), "Quotation_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
           oRpt.ExportToDisk(ExportFormatType.PortableDocFormat, path);
            myConnection.Close();
            var ls_response = objFnazurestorage.reportStreamDownload(path);
            File.Delete(path);
             return ls_response;

           
               
            

        }
        public currency fnconvertnumbertowords(string gid, string type)
        {
            currency obj = new currency();
            string number = string.Empty;
            string words = string.Empty;
            string lscurrency_code = string.Empty;

            if (type == "PO_REPORT")
            {
                msSQL = "select total_amount from pmr_trn_tpurchaseorder where purchaseorder_gid='" + gid + "'";
                number = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select currency_code from pmr_trn_tpurchaseorder where purchaseorder_gid='" + gid + "'";
                lscurrency_code = objdbconn.GetExecuteScalar(msSQL);
            }
            else if (type == "SQ_REPORT")
            {
                msSQL = "select Grandtotal from smr_trn_treceivequotation where quotation_gid='" + gid + "'";
                number = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select currency_code from smr_trn_treceivequotation where quotation_gid='" + gid + "'";
                lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

            }
            string[] strarr = number.Split('.');
            string int_part = objcmnfunctions.NumberToWords(Int32.Parse(strarr[0]));


            string dec_part = "";
            if (strarr.Length > 1 && !string.IsNullOrEmpty(strarr[1]))
            {
                dec_part = objcmnfunctions.NumberToWords(Int32.Parse(strarr[1]));
            }

            if (!string.IsNullOrEmpty(dec_part))
            {
                if (lscurrency_code == "INR")
                {
                    words = int_part + " RUPEES AND " + dec_part + " PAISA ONLY";
                    words = words.ToUpper();
                    obj.symbol = "₹";

                }
                else if (lscurrency_code == "GBP")
                {
                    words = int_part + " POUNDS AND " + dec_part + " PENCE ONLY";
                    words = words.ToUpper();
                    obj.symbol = "£";
                }
                else if (lscurrency_code == "EUR")
                {
                    words = int_part + " EUROS AND " + dec_part + " CENTS ONLY";
                    words = words.ToUpper();
                    obj.symbol = "€";
                }

            }
            else
            {
                if (lscurrency_code == "INR")
                {
                    words = int_part + " RUPEES ONLY";
                    words = words.ToUpper();
                    obj.symbol = "₹";
                }

                else if (lscurrency_code == "GBP")
                {
                    words = int_part + " POUNDS ONLY";
                    words = words.ToUpper();
                    obj.symbol = "£";

                }
                else if (lscurrency_code == "EUR")
                {
                    words = int_part + " EUROS ONLY";
                    words = words.ToUpper();
                    obj.symbol = "€";

                }
            }

            obj.number2words = words;
            return obj;
        }

        public class currency
        {
            public string number2words { get; set; }
            public string symbol { get; set; }
        }


        public void DaGetProductsearchSummary(string producttype_gid, string product_name, string customer_gid, MdlSmrTrnQuotation values)
        {
            try
            {
                StringBuilder sqlQuery = new StringBuilder("SELECT a.product_name, a.product_code, a.product_gid, " +
                    " a.cost_price, b.productuom_gid, b.productuom_name, c.productgroup_name, c.productgroup_gid, " +
                    " a.productuom_gid, d.producttype_gid FROM pmr_mst_tproduct a " +
                    " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                    " LEFT JOIN pmr_mst_tproductgroup c ON a.productgroup_gid = c.productgroup_gid " +
                    " LEFT JOIN pmr_mst_tproducttype d ON d.producttype_gid = a.producttype_gid WHERE 1=1");

                if (!string.IsNullOrEmpty(producttype_gid) && producttype_gid != "undefined" && producttype_gid != "null")
                {
                    sqlQuery.Append(" AND a.producttype_gid = '").Append(producttype_gid).Append("'");
                }

                if (!string.IsNullOrEmpty(product_name) && product_name != "null")
                {
                    sqlQuery.Append(" AND a.product_name LIKE '%").Append(product_name).Append("%'");
                }

                dt_datatable = objdbconn.GetDataTable(sqlQuery.ToString());
                var getModuleList = new List<GetProductsearch>();
                var allTaxSegmentsList = new List<GetTaxSegmentList>(); // Create list to store all tax segments

                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        var product = new GetProductsearch
                        {
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            producttype_gid = dt["producttype_gid"].ToString(),
                            productgroup_gid = dt["productgroup_gid"].ToString(),
                            unitprice = dt["cost_price"].ToString(),
                            quantity = 0,
                            total_amount = 0,
                            discount_persentage = 0,
                            discount_amount = 0,
                        };
                        getModuleList.Add(product);

                        if (!string.IsNullOrEmpty(customer_gid) && customer_gid != "undefined")
                        {
                            string productGid = product.product_gid;
                            string productName = product.product_name;

                            StringBuilder taxSegmentQuery = new StringBuilder("SELECT f.taxsegment_gid, d.taxsegment_gid, " +
                                " e.taxsegment_name, d.tax_name, d.tax_gid, CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) " +
                                " THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage, d.tax_amount, a.mrp_price, " +
                                " a.cost_price FROM acp_mst_ttaxsegment2product d " +
                                " LEFT JOIN acp_mst_ttaxsegment e ON e.taxsegment_gid = d.taxsegment_gid " +
                                " LEFT JOIN crm_mst_tcustomer f ON f.taxsegment_gid = e.taxsegment_gid " +
                                " LEFT JOIN pmr_mst_tproduct a ON a.product_gid = d.product_gid WHERE a.product_gid = '");
                            taxSegmentQuery.Append(productGid).Append("' AND f.customer_gid = '").Append(customer_gid).Append("'");

                            dt_datatable = objdbconn.GetDataTable(taxSegmentQuery.ToString());

                            if (dt_datatable.Rows.Count != 0)
                            {
                                foreach (DataRow dt1 in dt_datatable.Rows)
                                {
                                    allTaxSegmentsList.Add(new GetTaxSegmentList
                                    {
                                        product_name = productName,
                                        product_gid = productGid,
                                        taxsegment_gid = dt1["taxsegment_gid"].ToString(),
                                        taxsegment_name = dt1["taxsegment_name"].ToString(),
                                        tax_name = dt1["tax_name"].ToString(),
                                        tax_percentage = dt1["tax_percentage"].ToString(),
                                        tax_gid = dt1["tax_gid"].ToString(),
                                        mrp_price = dt1["mrp_price"].ToString(),
                                        cost_price = dt1["cost_price"].ToString(),
                                        tax_amount = dt1["tax_amount"].ToString(),
                                    });
                                }
                            }
                        }
                    }
                    values.GetProductsearch = getModuleList; // Assign GetProductsearch to values.GetProductsearch
                }
                values.GetTaxSegmentList = allTaxSegmentsList; // Assign allTaxSegmentsList to values.GetTaxSegmentList
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while changing product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                  $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                  ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                  msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                  DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }

        public void DaPostOnAddproduct(string employee_gid, productsubmit__list values)

        {
            try
            {
                string lstax1, lstax2;
                msGetGid = objcmnfunctions.GetMasterGID("VQDT");
                msSQL = " SELECT a.productuom_gid, a.product_gid, a.product_name, b.productuom_name FROM pmr_mst_tproduct a " +
                     " LEFT JOIN pmr_mst_tproductuom b ON a.productuom_gid = b.productuom_gid " +
                     " WHERE product_gid = '" + values.product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                if (objOdbcDataReader.HasRows == true)
                {
                    
                    lsproductgid = objOdbcDataReader["product_gid"].ToString();
                    lsproductuom_gid = objOdbcDataReader["productuom_gid"].ToString();
                    lsproduct_name = objOdbcDataReader["product_name"].ToString();
                    lsproductuom_name = objOdbcDataReader["productuom_name"].ToString();

                    objOdbcDataReader.Close();


                }

             
                    msSQL = "select tax_prefix from acp_mst_ttax  where tax_gid='" + values.taxgid1 + "'";
                    lstax1 = objdbconn.GetExecuteScalar(msSQL);
                

               
               
                    msSQL = "select tax_prefix from acp_mst_ttax where tax_gid='" + values.taxgid2 + "'";
                    lstax2 = objdbconn.GetExecuteScalar(msSQL);
                
                if (values.productdiscount == null || values.productdiscount == "")
                {
                    lsdiscountpercentage = "0.00";
                }
                else
                {
                    lsdiscountpercentage = values.productdiscount;
                }

                if (values.discount_amount == null || values.discount_amount == "")
                {
                    lsdiscountamount = "0.00";
                }
                else
                {
                    lsdiscountamount = values.discount_amount;
                }

                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                         " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                          " WHERE product_gid='" + values.product_gid + "'";
                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
               
                if (objOdbcDataReader.HasRows == true)
                {
                    lsorder_type = "Sales";

                    objOdbcDataReader.Close();
                }
                else
                {
                    lsorder_type = "Services";
                }
                
               
                 if (values.unitprice == "undefined" || values.unitprice == null || values.unitprice == "")
                {
                    values.status = false;
                    values.message = "Price cannot be left empty";
                    return;
                }
                else 
                { 

                    msSQL = " insert into smr_tmp_tsalesorderdtl( " +
                              " tmpsalesorderdtl_gid," +
                              " salesorder_gid," +
                              " employee_gid," +
                              " enquiry_gid," +
                              " product_gid," +
                              " product_code," +
                              " product_name," +
                              " productgroup_gid," +
                              " product_price," +
                              " qty_quoted," +
                              " uom_gid," +
                              " uom_name," +
                              " price," +
                              " order_type," +
                              " tax_rate, " +
                              " taxsegment_gid, " +
                             " taxsegmenttax_gid, " +
                             " tax1_gid, " +
                             " tax2_gid, " +
                             " tax3_gid, " +
                             " tax_name, " +
                             " tax_name2, " +
                             " tax_name3, " +
                             " tax_percentage, " +
                             " tax_percentage2, " +
                             " tax_percentage3, " +
                             " tax_amount, " +
                             " tax_amount2, " +
                             " tax_amount3, " +
                              " discount_amount, " +
                              " product_remarks, " +
                              " discount_percentage" +
                              ")values(" +
                              "'" + msGetGid + "'," +
                              "'" + values.quotation_gid + "'," +
                              "'" + employee_gid + "'," +
                              "'" + values.enquiry_gid + "'," +
                              "'" + values.product_gid + "'," +
                              "'" + values.product_code.Replace("'", "\\\'") + "'," +
                              "'" + lsproduct_name.Replace("'", "\\\'") + "'," +
                              "'" + values.productgroup_name + "'," +
                              "'" + values.unitprice + "'," +
                              "'" + values.productquantity + "'," +
                              "'" + lsproductuom_gid + "'," +
                              "'" + lsproductuom_name.Replace("'", "\\\'") + "'," +
                              "'" + values.producttotal_amount + "'," +
                              " '" + lsorder_type + "', " +
                              " '" + values.tax_prefix + "', " +
                              " '" + values.taxsegment_gid + "', " +
                              " '" + values.taxsegment_gid + "', " +
                              " '" + values.taxgid1 + "', " +
                              " '" + values.taxgid2 + "', " +
                              " '" + values.taxgid3 + "', " +
                              " '" + lstax1 + "', " +
                              " '" + lstax2 + "', " +
                              " '" + values.taxname3 + "', ";
                if (values.taxprecentage1 == "" || values.taxprecentage1 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += " '" + values.taxprecentage1 + "', ";
                }
                if (values.taxprecentage2 == "" || values.taxprecentage2 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += " '" + values.taxprecentage2 + "', ";
                }
                if (values.taxprecentage3 == "" || values.taxprecentage3 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += " '" + values.taxprecentage3 + "', ";
                }
                if (values.taxamount1 == "" || values.taxamount1 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += " '" + values.taxamount1 + "', ";
                }
                if (values.taxamount2 == "" || values.taxamount2 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += " '" + values.taxamount2 + "', ";
                }
                if (values.taxamount3 == "" || values.taxamount3 == null)
                {
                    msSQL += "'0.00',";
                }
                else
                {
                    msSQL += " '" + values.taxamount3 + "', ";
                }

                    msSQL +=
                       "'" + values.discount_amount + "',";
                    if(values.product_remarks != null)
                    {
                       msSQL += "'" + values.product_remarks.Replace("'","\\\'") + "',";
                    }
                    else
                    {
                      msSQL +=  "'" + values.product_remarks + "',";
                    }
                  
                  msSQL += "'" + lsdiscountpercentage + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);



                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Product Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Adding Product";
                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                            $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + values.message.ToString() + "***********" +
                            values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

                    }
                }
               
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Product!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }



        //public void DaPostOnAddproduct(string user_gid, string employee_gid, summaryprod_list values)
        //{
        //    try
        //    {
        //        string taxsegment_gid = values.taxsegment_gid;
        //        string exchange_rate = values.exchange_rate;
        //        string currency_code = values.currency_code;
        //        foreach (var data in values.ProductList)
        //        {
        //            if (data.quantity == null || data.quantity == "0")
        //            {

        //            }
        //            else
        //            {

        //                string tax_gid1 = "";
        //                string tax_gid2 = "";
        //                string tax_gid3 = "";

        //                string tax_name1 = "";
        //                string tax_name2 = "";
        //                string tax_name3 = "";

        //                string tax_percent1 = "";
        //                string tax_percent2 = "";
        //                string tax_percent3 = "";

        //                string tax_amount1 = "";
        //                string tax_amount2 = "";
        //                string tax_amount3 = "";

        //                if (data.taxSegments != null && data.taxSegments.Count > 0)
        //                {
        //                    tax_gid1 = data.taxSegments[0]?.tax_gid ?? "--no tax--";
        //                    tax_name1 = data.taxSegments[0]?.tax_name ?? "--no tax--";
        //                    tax_percent1 = data.taxSegments[0]?.tax_percentage ?? "--no tax--";
        //                    tax_amount1 = data.taxSegments[0]?.taxAmount ?? "--no tax--";

        //                    if (data.taxSegments.Count > 1)
        //                    {
        //                        tax_gid2 = data.taxSegments[1]?.tax_gid ?? "--no tax--";
        //                        tax_name2 = data.taxSegments[1]?.tax_name ?? "--no tax--";
        //                        tax_percent2 = data.taxSegments[1]?.tax_percentage ?? "--no tax--";
        //                        tax_amount2 = data.taxSegments[1]?.taxAmount ?? "--no tax--";
        //                    }

        //                    if (data.taxSegments.Count > 2)
        //                    {
        //                        tax_gid3 = data.taxSegments[2]?.tax_gid ?? "--no tax--";
        //                        tax_name3 = data.taxSegments[2]?.tax_name ?? "--no tax--";
        //                        tax_percent3 = data.taxSegments[2]?.tax_percentage ?? "--no tax--";
        //                        tax_amount3 = data.taxSegments[2]?.taxAmount ?? "--no tax--";
        //                    }
        //                }

        //                msGetGid1 = objcmnfunctions.GetMasterGID("VQDT");


        //                msSQL = "select product_gid from pmr_mst_tproduct where product_name='" + data.product_name + "'";
        //                string lsproductgid = objdbconn.GetExecuteScalar(msSQL);

        //                msSQL = "select tax_gid from acp_mst_ttax where tax_name='" + data.tax_name + "'";
        //                string lstaxgid = objdbconn.GetExecuteScalar(msSQL);

        //                msSQL = "select productuom_gid from pmr_mst_tproductuom where productuom_name='" + data.productuom_name + "'";
        //                string lsproductuomgid = objdbconn.GetExecuteScalar(msSQL);


        //                if (data.discountpercentage == null || data.discountpercentage == "")
        //                {
        //                    lsdiscountpercentage = "0.00";
        //                }
        //                else
        //                {
        //                    lsdiscountpercentage = values.discountpercentage;
        //                }

        //                if (data.discountamount == null || data.discountamount == "")
        //                {
        //                    lsdiscountamount = "0.00";
        //                }
        //                else
        //                {
        //                    lsdiscountamount = data.discountamount;
        //                }


        //                if (data.tax_name == null || data.tax_name == "")
        //                {
        //                    lstaxname = "0.00";
        //                }
        //                else
        //                {
        //                    msSQL = "select tax_name from acp_mst_ttax where tax_gid='" + lstaxgid + "'";
        //                    lstaxname = objdbconn.GetExecuteScalar(msSQL);
        //                }

        //                if (data.tax_name == null || data.tax_name == "")
        //                {
        //                    lspercentage1 = "0.00";
        //                }
        //                else
        //                {
        //                    msSQL = "select percentage from acp_mst_ttax where tax_gid='" + lstaxgid + "'";
        //                    lspercentage1 = objdbconn.GetExecuteScalar(msSQL);
        //                }


        //                if (data.tax_amount == null || data.tax_amount == "")
        //                {
        //                    lstaxamount = "0.00";
        //                }
        //                else
        //                {
        //                    lstaxamount = data.tax_amount;
        //                }
        //                if ((lspercentage1 == "" || lspercentage1 == null) && (lstaxamount == null || lstaxamount == ""))
        //                {
        //                    lstaxamount = "0.00";
        //                    lspercentage1 = "0";
        //                }
        //                int i = 0;

        //                msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
        //                  " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
        //                  " WHERE product_gid='" + lsproductgid + "'";
        //                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
        //                if (objOdbcDataReader.HasRows == true)
        //                {
        //                    if (objOdbcDataReader["producttype_name"].ToString() != "Services")
        //                    {
        //                        lsquotation_type = "Sales";
        //                    }
        //                    else
        //                    {
        //                        lsquotation_type = "Services";
        //                    }

        //                }




        //                msSQL = " insert into smr_tmp_treceivequotationdtl( " +
        //                        " tmpquotationdtl_gid," +
        //                        " quotation_gid," +
        //                        " product_gid," +
        //                        " product_code," +
        //                        " product_name," +
        //                        " display_field," +
        //                        " product_price," +
        //                        " qty_quoted," +
        //                        " discount_percentage," +
        //                        " discount_amount," +
        //                        " uom_gid," +
        //                        " uom_name," +
        //                        " price," +
        //                        " created_by," +
        //                        " tax_name, " +
        //                        " tax1_gid, " +
        //                        " slno, " +
        //                        " quotation_type, " +
        //                        " foreign_currency, " +
        //                        " exchange_rate, " +
        //                        " taxsegment_gid, " +
        //                        " taxsegmenttax_gid, " +
        //                           " taxseg_taxgid1, " +
        //                          " taxseg_taxgid2, " +
        //                           " taxseg_taxgid3, " +
        //                          " taxseg_taxname1, " +
        //                           " taxseg_taxname2, " +
        //                            " taxseg_taxname3, " +
        //                                 " taxseg_taxpercent1, " +
        //                          " taxseg_taxpercent2, " +
        //                           " taxseg_taxpercent3, " +
        //                          " taxseg_taxamount1, " +
        //                           " taxseg_taxamount2, " +
        //                            " taxseg_taxamount3, " +
        //                            " taxseg_taxtotal, " +
        //                        " tax_percentage," +
        //                        " tax_amount " +
        //                        " ) values( " +
        //                        "'" + msGetGid1 + "'," +
        //                        "'" + values.quotation_gid + "'," +
        //                        "'" + lsproductgid + "'," +
        //                        "'" + data.product_code + "'," +
        //                        "'" + data.product_name + "', " +
        //                        "'" + data.display_field + "', " +
        //                        "'" + data.unitprice + "', " +
        //                        "'" + data.quantity + "', " +
        //                        "'" + data.discount_persentage + "', " +
        //                        "'" + data.discount_amount + "', " +
        //                        "'" + data.productuom_gid + "', " +
        //                        "'" + values.productuom_name + "', " +
        //                        "'" + data.total_amount + "', " +
        //                        "'" + employee_gid + "', " +
        //                        "'" + lstaxname + "', " +
        //                        "'" + lstaxgid + "', " +
        //                        "'" + i + 1 + "', " +
        //                        "'" + lsquotation_type + "'," +
        //                        "'" + currency_code + "'," +
        //                        "'" + exchange_rate + "'," +
        //                        "'" + values.taxsegment_gid + "'," +
        //                        "'" + values.taxsegmenttax_gid + "'," +
        //                          "'" + tax_gid1 + "'," +
        //                         "'" + tax_gid2 + "'," +
        //                           "'" + tax_gid3 + "'," +
        //                         "'" + tax_name1 + "'," +
        //                           "'" + tax_name2 + "'," +
        //                         "'" + tax_name3 + "'," +
        //                           "'" + tax_percent1 + "'," +
        //                         "'" + tax_percent2 + "'," +
        //                           "'" + tax_percent3 + "'," +
        //                         "'" + tax_amount1 + "'," +
        //                           "'" + tax_amount2 + "'," +
        //                         "'" + tax_amount3 + "'," +
        //                         "'" + data.totalTaxAmount + "'," +
        //                        "'" + lspercentage1 + "'," +
        //                        "'" + data.totalTaxAmount + "')";
        //                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
        //            }


        //        }
        //        if (mnResult != 0)
        //        {
        //            values.status = true;
        //            values.message = "Product Added Successfully";
        //        }
        //        else
        //        {
        //            values.status = false;
        //            values.message = "Error While Adding Product";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        values.message = "Exception occured while Adding Product!";
        //        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
        //        $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
        //        values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

        //    }
        //}


        public void DaGetProductTaxSegment(string product_gid, string vendor_gid, MdlSmrTrnQuotation values)

        {
            try
            {
                msSQL = " select f.taxsegment_gid,d.taxsegment_gid,e.taxsegment_name,d.tax_name,d.tax_gid, " +
                                " CASE WHEN d.tax_percentage = ROUND(d.tax_percentage) THEN ROUND(d.tax_percentage) ELSE d.tax_percentage END AS tax_percentage,d.tax_amount, " +
                                " a.mrp_price,a.cost_price from acp_mst_ttaxsegment2product d " +
                                " left join acp_mst_ttaxsegment e on e.taxsegment_gid=d.taxsegment_gid " +
                                " left join acp_mst_tvendor f on f.taxsegment_gid = e.taxsegment_gid " +
                                " left  join pmr_mst_tproduct a on a.product_gid=d.product_gid " +
                                " where a.product_gid='" + product_gid + "'   and f.vendor_gid='" + vendor_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                var getGetTaxSegmentList = new List<GetTaxSegmentList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getGetTaxSegmentList.Add(new GetTaxSegmentList
                        {
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                            mrp_price = dt["mrp_price"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),

                        });
                        values.GetTaxSegmentList = getGetTaxSegmentList;
                    }
                }

                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Getting Tax Segment details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }


        }

        public void DaGetEditQuotationSummary(string quotation_gid, MdlSmrTrnQuotation values)
        {
            try
            {

                msSQL = "  select a.pincode,m.campaign_gid,a.customer_gid,y.customer_gid,y.gst_number,y.customer_city,y.customer_state,l.tax_name,k.country_name,y.customer_pin,n.region_name,x.pricesegment_gid,z.taxsegment_gid,x.pricesegment_name,z.taxsegment_name, a.quotation_referenceno1,b.lead2campaign_gid,a.quotation_gid,a.address1,a.address2,a.currency_code,a.freight_terms," +
                        " a.payment_terms, a.customer_contact_person,a.salesperson_gid, a.termsandconditions," +
                        " date_format(a.quotation_date, '%d-%m-%Y') as quotation_date,a.quotation_remarks, " +
                        " concat(g.user_code, ' / ', g.user_firstname, ' ', g.user_lastname) AS salesperson_name, " +
                        " a.quotation_referenceno1,e.branch_name,a.exchange_rate,  a.contact_mail," +
                        " a.contact_no,format(a.Grandtotal_l, 2) as Grandtotal_l, a.delivery_days," +
                        " format(a.Grandtotal, 2) as Grandtotal,a.addon_charge as addon_charge," +
                        " a.additional_discount as additional_discount,  a.customer_name," +
                        " a.tax_gid,a.tax_name,format(a.total_amount, 2) as total_amount,format(a.total_price, 2) as total_price," +
                        " a.customer_address,a.payment_days,a.delivery_days,a.tax_name4,a.tax_amount4, " +
                        " concat(m.campaign_title, ' | ', j.user_code, ' | ', j.user_firstname, ' ', j.user_lastname) as user_firstname, " +
                        " a.freight_charges as freight_charges,format(a.buyback_charges, 2) as buyback_charges, a.roundoff as roundoff," +
                        " format(a.packing_charges, 2) as packing_charges,format(a.insurance_charges, 2) as insurance_charges from smr_trn_treceivequotation a " +
                        " left join crm_trn_tenquiry2campaign b on a.customer_gid = b.customer_gid " +
                        " left join smr_trn_tcampaign m on b.campaign_gid = m.campaign_gid " +
                        " left join hrm_mst_tbranch e on e.branch_gid = a.branch_gid " +
                        " left join hrm_mst_temployee h on b.assign_to = h.employee_gid " +
                        " left join adm_mst_tuser j on h.user_gid = j.user_gid " +
                        " left join crm_mst_Tcustomer y on a.customer_gid = y.customer_gid " +
                        " left join acp_mst_ttaxsegment z on z.taxsegment_gid = y.taxsegment_gid " +
                        " left join smr_trn_tpricesegment x on x.pricesegment_gid = y.pricesegment_gid " +
                        " left join crm_mst_tregion n on n.region_gid = y.customer_region " +
                        " LEFT JOIN adm_mst_tuser g ON g.user_gid = a.salesperson_gid " +
                       " left join adm_mst_tcountry k on y.customer_country= k.country_gid " +
                        " left join acp_mst_ttax l on a.tax_name4 = l.tax_gid " +
                        " where a.quotation_gid = '" + quotation_gid + "' group by a.quotation_gid";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<postsalesquotation_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new postsalesquotation_list
                        {
                            quotation_gid = dt["quotation_referenceno1"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            quotation_date = dt["quotation_date"].ToString(),
                            branch_name = dt["branch_name"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            contact_person = dt["customer_contact_person"].ToString(),
                            contact_no = dt["contact_no"].ToString(),
                            contact_mail = dt["contact_mail"].ToString(),
                            customer_address = dt["customer_address"].ToString(),
                            quotation_remarks = dt["quotation_remarks"].ToString(),
                            termsandconditions = dt["termsandconditions"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            exchange_rate = dt["exchange_rate"].ToString(),
                            freight_terms = dt["freight_terms"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            payment_days = dt["payment_days"].ToString(),
                            delivery_days = dt["delivery_days"].ToString(),
                            addon_charge = dt["addon_charge"].ToString(),
                            additional_discount = dt["additional_discount"].ToString(),
                            freight_charges = dt["freight_charges"].ToString(),
                            buyback_charges = dt["buyback_charges"].ToString(),
                            total_price = dt["total_amount"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            packing_charges = dt["packing_charges"].ToString(),
                            insurance_charges = dt["insurance_charges"].ToString(),
                            roundoff = dt["roundoff"].ToString(),
                            total_amount = dt["total_price"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            pricesegment_name = dt["pricesegment_name"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            tax_amount4 = dt["tax_amount4"].ToString(),
                            tax_name4 = dt["tax_name4"].ToString(),
                            salesperson_name = dt["salesperson_name"].ToString(),
                            gst_number = dt["gst_number"].ToString(),
                            customer_pin = dt["customer_pin"].ToString(),
                            customer_state = dt["customer_state"].ToString(),
                            customer_city = dt["customer_city"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            tax_nameno = dt["tax_name"].ToString(),
                            pincode = dt["pincode"].ToString()

                        });
                        values.SO_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Quootation View !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }


        public void DaGetEditTempProductsSummary(string employee_gid, MdlSmrTrnQuotation values)
        {
            try
            {
                double grand_total = 0.00;
                double grandtotal = 0.00;

                msSQL = "SELECT a.tmpsalesorderdtl_gid,a.taxsegment_gid,a.salesorder_gid, a.tax_name,a.tax_amount,a.tax_percentage,a.tax_name2,a.tax_amount2,a.tax_percentage2,a.tax_name3,a.tax_amount3,a.tax_percentage3,a.tax_amount, a.salesorderdtl_gid, a.salesorder_gid, a.product_gid, a.productgroup_gid, " +
                   " v.productgroup_name, a.product_name, FORMAT(a.product_price, 2) AS product_price, b.product_code, a.qty_quoted, a.product_remarks, " +
                   " a.uom_gid, a.vendor_gid, a.slno, a.uom_name, FORMAT(a.price, 2) AS price, " +
                   " FORMAT(a.discount_percentage,2) AS discount_percentage, FORMAT(a.discount_amount,2) AS discount_amount, " +
                   " FORMAT(a.selling_price, '0.00') AS selling_price,a.product_remarks, " +
                   " CONCAT( CASE WHEN a.tax_name IS NULL THEN '' ELSE a.tax_name END, ' '," +
                   "CASE WHEN a.tax_percentage = '0' THEN '' ELSE concat('',a.tax_percentage,'%') END , " +
                   " CASE WHEN a.tax_amount = '0' THEN '' ELSE concat(':',a.tax_amount) END) AS tax, " +
                   " CONCAT(CASE WHEN a.tax_name2 IS NULL THEN '' ELSE a.tax_name2 END, ' ', " +
                   " CASE WHEN a.tax_percentage2 = '0' THEN '' ELSE concat('', a.tax_percentage2, '%') END, " +
                   " CASE WHEN a.tax_amount2 = '0' THEN '' ELSE concat(':', a.tax_amount2) END) AS tax2, " +
                   " CONCAT(  CASE WHEN a.tax_name3 IS NULL THEN '' ELSE a.tax_name3 END, ' ', " +
                   " CASE WHEN a.tax_percentage3 = '0' THEN '' ELSE concat('', a.tax_percentage3, ' %   ')" +
                   " END, CASE WHEN a.tax_amount3 = '0' THEN '  ' ELSE concat(' : ', a.tax_amount3) END) AS tax3" +
                   " , format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.tax_rate " +
                   " FROM smr_tmp_tsalesorderdtl a " +
                   " left join smr_trn_tsalesorder s on s.salesorder_gid=a.salesorder_gid " +
                   " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                   " left join pmr_mst_tproductgroup v on b.productgroup_gid=v.productgroup_gid " +
                   " LEFT JOIN acp_mst_tvendor e ON a.vendor_gid = e.vendor_gid " +
                   " WHERE a.employee_gid='" + employee_gid + "' and b.delete_flag='N' order by (a.slno+0) asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Gettemporarysummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["price"].ToString());
                        grandtotal += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new Gettemporarysummary
                        {
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            slno = dt["slno"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            discountpercentage = double.Parse(dt["discount_percentage"].ToString()),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            producttotalamount = dt["price"].ToString(),
                            totalamount = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            grand_total = dt["price"].ToString(),
                            grandtotal = dt["price"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            tax = dt["tax"].ToString(),
                            tax2 = dt["tax2"].ToString(),
                            tax3 = dt["tax3"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            taxamount = dt["taxamount"].ToString(),
                            tax_rate = dt["tax_rate"].ToString(),


                        });
                    }
                    values.Gettemporarysummary = getModuleList;
                }

                dt_datatable.Dispose();
                values.grand_total = grand_total;
                values.grandtotal = grandtotal;
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while getting product summary!";
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetEditQuoteProdSummary(string quotation_gid, string employee_gid, MdlSmrTrnQuotation values)

        {

            try

            {
                msSQL = " delete from smr_tmp_tsalesorderdtl where " +
                      " employee_gid = '" + employee_gid + "' ";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    msSQL = "select d.quotationdtl_gid,d.product_gid,e.taxsegment_gid,i.productgroup_name,d.taxsegmenttax_gid, " +
                            " i.productgroup_gid,d.product_remarks,d.uom_gid,d.product_name,d.quotationdtl_gid," +
                            " d.product_code,d.uom_name,d.qty_quoted,d.margin_amount,d.margin_percentage,d.discount_percentage," +
                            " d.discount_amount,d.product_price ,d.tax_name,d.tax_amount,price,e.tax_prefix,  " +
                            " d.tax1_gid,d.tax2_gid,d.tax3_gid,d.tax_name2,d.tax_name3,d.tax_amount2,d.tax_amount3, " +
                            " d.tax_percentage,d.tax_percentage2,d.tax_percentage3,d.product_remarks FROM smr_trn_treceivequotation a " +
                            " LEFT JOIN smr_trn_treceivequotationdtl d ON d.quotation_gid = a.quotation_gid " +
                            " LEFT JOIN acp_mst_ttax e ON e.taxsegment_gid = d.taxsegment_gid " +
                            " LEFT JOIN pmr_mst_tproductgroup i ON i.productgroup_gid = d.productgroup_gid" +
                            " WHERE a.quotation_gid = '" + quotation_gid + "' group by  d.quotationdtl_gid order by d.quotationdtl_gid  desc;";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)

                    {

                        foreach (DataRow dt in dt_datatable.Rows)

                        {
                            //msGetGid = objcmnfunctions.GetMasterGID("VQDT");

                            string display_field = dt["product_remarks"].ToString();

                            msSQL = " insert into smr_tmp_tsalesorderdtl( " +
                                     " tmpsalesorderdtl_gid," +
                                     " salesorder_gid," +
                                     " employee_gid, " + 
                                     " product_gid," +
                                     " product_code," +
                                     " product_name," +
                                     " productgroup_gid," +
                                     " product_price," +
                                     " qty_quoted," +
                                     " uom_gid," +
                                     " uom_name," +
                                     " price," +
                                     " order_type," +
                                     " tax_rate, " +
                                     " taxsegment_gid, " +
                                    " taxsegmenttax_gid, " +
                                    " tax1_gid, " +
                                    " tax2_gid, " +
                                    " tax3_gid, " +
                                    " tax_name, " +
                                    " tax_name2, " +
                                    " tax_name3, " +
                                    " tax_percentage, " +
                                    " tax_percentage2, " +
                                    " tax_percentage3, " +
                                    " tax_amount, " +
                                    " tax_amount2, " +
                                    " tax_amount3, " +
                                     " discount_amount, " +
                                     " product_remarks, " +
                                     " discount_percentage" +
                                     ")values(" +
                                     "'" + dt["quotationdtl_gid"].ToString() + "'," +
                                     "'" + quotation_gid + "'," +
                                     "'" + employee_gid + "'," +
                                     "'" + dt["product_gid"].ToString() + "'," +
                                     "'" + dt["product_code"].ToString().Replace("'", "\\\'") + "'," +
                                     "'" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                                     "'" + dt["productgroup_gid"].ToString() + "'," +
                                     "'" + dt["product_price"].ToString() + "'," +
                                     "'" + dt["qty_quoted"].ToString() + "'," +
                                     "'" + dt["uom_gid"].ToString() + "'," +
                                     "'" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                                     "'" + dt["price"].ToString() + "'," +
                                     "'Sales'," +
                                     "'" + dt["tax_amount"].ToString() + "'," +
                                     "'" + dt["taxsegment_gid"].ToString() + "'," +
                                     "'" + dt["taxsegmenttax_gid"].ToString() + "'," +
                                     "'" + dt["tax1_gid"].ToString() + "'," +
                                     "'" + dt["tax2_gid"].ToString() + "'," +
                                     "'" + dt["tax3_gid"].ToString() + "'," +
                                     "'" + dt["tax_name"].ToString() + "'," +
                                     "'" + dt["tax_name2"].ToString() + "'," +
                                     "'" + dt["tax_name3"].ToString() + "',";
                            if (dt["tax_percentage"].ToString() == "" || dt["tax_percentage"].ToString() == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += " '" + dt["tax_percentage"].ToString() + "', ";
                            }
                            if (dt["tax_percentage2"].ToString() == "" || dt["tax_percentage2"].ToString() == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += " '" + dt["tax_percentage2"].ToString() + "', ";
                            }
                            if (dt["tax_percentage3"].ToString() == "" || dt["tax_percentage3"].ToString() == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += " '" + dt["tax_percentage3"].ToString() + "', ";
                            }
                            if (dt["tax_amount"].ToString() == "" || dt["tax_amount"].ToString() == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += " '" + dt["tax_amount"].ToString() + "', ";
                            }
                            if (dt["tax_amount2"].ToString() == "" || dt["tax_amount2"].ToString() == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += " '" + dt["tax_amount2"].ToString() + "', ";
                            }
                            if (dt["tax_amount3"].ToString() == "" || dt["tax_amount3"].ToString() == null)
                            {
                                msSQL += "'0.00',";
                            }
                            else
                            {
                                msSQL += " '" + dt["tax_amount3"].ToString() + "', ";
                            }

                            msSQL +=
                               "'" + dt["discount_amount"].ToString() + "',";
                                if (display_field != null)
                            {
                                msSQL += "'" + display_field.Replace("'", "\\\'") + "',";
                            }
                            else
                            {
                                msSQL += "'" + display_field + "',";
                            }
                           msSQL += "'" + dt["discount_percentage"].ToString() + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                values.message = "Exception occured while loading Sales Order Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaGetEditTempProdSummary(string quotation_gid, MdlSmrTrnQuotation values)
        {
            try
            {
                double grand_total = 0.00;
                double grandtotal = 0.00;

                msSQL = "SELECT a.tmpsalesorderdtl_gid,a.taxsegment_gid,a.salesorder_gid, a.tax_name,format(a.tax_amount,2) as tax_amount,a.tax_percentage,a.tax_name2,format(a.tax_amount2,2) as tax_amount2,a.tax_percentage2,a.tax_name3,format(a.tax_amount3,2) as tax_amount3,a.tax_percentage3,format(a.tax_amount,2) as tax_amount, a.salesorderdtl_gid, a.salesorder_gid, a.product_gid, a.productgroup_gid, " +
                   " v.productgroup_name, a.product_name, FORMAT(a.product_price, 2) AS product_price, b.product_code, a.qty_quoted, a.product_remarks, " +
                   " a.uom_gid, a.vendor_gid, a.slno, a.uom_name, FORMAT(a.price, 2) AS price, " +
                   " FORMAT(a.discount_percentage,2) AS discount_percentage, FORMAT(a.discount_amount,2) AS discount_amount, " +
                   " FORMAT(a.selling_price, '0.00') AS selling_price,a.product_remarks, " +
                   " CONCAT( CASE WHEN a.tax_name IS NULL THEN '' ELSE a.tax_name END, ' '," +
                   "CASE WHEN a.tax_percentage = '0' THEN '' ELSE concat('',a.tax_percentage,'%') END , " +
                   " CASE WHEN a.tax_amount = '0' THEN '' ELSE concat(':',a.tax_amount) END) AS tax, " +
                   " CONCAT(CASE WHEN a.tax_name2 IS NULL THEN '' ELSE a.tax_name2 END, ' ', " +
                   " CASE WHEN a.tax_percentage2 = '0' THEN '' ELSE concat('', a.tax_percentage2, '%') END, " +
                   " CASE WHEN a.tax_amount2 = '0' THEN '' ELSE concat(':', a.tax_amount2) END) AS tax2, " +
                   " CONCAT(  CASE WHEN a.tax_name3 IS NULL THEN '' ELSE a.tax_name3 END, ' ', " +
                   " CASE WHEN a.tax_percentage3 = '0' THEN '' ELSE concat('', a.tax_percentage3, ' %   ')" +
                   " END, CASE WHEN a.tax_amount3 = '0' THEN '  ' ELSE concat(' : ', a.tax_amount3) END) AS tax3" +
                   " , format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.tax_rate " +
                   " FROM smr_tmp_tsalesorderdtl a " +
                   " left join smr_trn_tsalesorder s on s.salesorder_gid=a.salesorder_gid " +
                   " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                   " left join pmr_mst_tproductgroup v on b.productgroup_gid=v.productgroup_gid " +
                   " LEFT JOIN acp_mst_tvendor e ON a.vendor_gid = e.vendor_gid " +
                   " WHERE a.salesorder_gid='" + quotation_gid + "' and b.delete_flag='N' order by (a.slno+0) asc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Gettemporarysummary>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        grand_total += double.Parse(dt["price"].ToString());
                        grandtotal += double.Parse(dt["price"].ToString());
                        getModuleList.Add(new Gettemporarysummary
                        {
                            tmpsalesorderdtl_gid = dt["tmpsalesorderdtl_gid"].ToString(),
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            slno = dt["slno"].ToString(),
                            discountamount = dt["discount_amount"].ToString(),
                            discountpercentage = double.Parse(dt["discount_percentage"].ToString()),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            product_price = dt["product_price"].ToString(),
                            quantity = dt["qty_quoted"].ToString(),
                            uom_gid = dt["uom_gid"].ToString(),
                            productuom_name = dt["uom_name"].ToString(),
                            producttotalamount = dt["price"].ToString(),
                            totalamount = dt["price"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            tax_name2 = dt["tax_name2"].ToString(),
                            tax_name3 = dt["tax_name3"].ToString(),
                            tax_amount = dt["tax_amount"].ToString(),
                            tax_amount2 = dt["tax_amount2"].ToString(),
                            tax_amount3 = dt["tax_amount3"].ToString(),
                            tax_percentage = dt["tax_percentage"].ToString(),
                            tax_percentage2 = dt["tax_percentage2"].ToString(),
                            tax_percentage3 = dt["tax_percentage3"].ToString(),
                            grand_total = dt["price"].ToString(),
                            grandtotal = dt["price"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            tax = dt["tax"].ToString(),
                            tax2 = dt["tax2"].ToString(),
                            tax3 = dt["tax3"].ToString(),
                            product_remarks = dt["product_remarks"].ToString(),
                            taxamount = dt["taxamount"].ToString(),
                            tax_rate = dt["tax_rate"].ToString(),


                        });
                    }
                    values.Gettemporarysummary = getModuleList;
                }

                dt_datatable.Dispose();
                values.grand_total = grand_total;
                values.grandtotal = grandtotal;
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while getting product summary!";
                objcmnfunctions.LogForAudit(
                    "*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" +
                    $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                    ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                    msSQL + "*******Apiref********", "ErrorLog/Purchase/Log" +
                    DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }


        public void DaPostUpdateQuotation(string user_gid, postQuatation values)
        {
            try
            {

                msSQL = " select employee_gid from hrm_mst_temployee where user_gid='" + user_gid + "'";
                string employee_gid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select * from smr_tmp_tsalesorderdtl " +
                     " where employee_gid='" + employee_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {



                    msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='VQNP' order by finyear desc limit 0,1 ";
                    string lsCode = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = "select user_gid from adm_mst_tuser where user_firstname = '"+values.user_name+"'";
                    string lsUsername = objdbconn.GetExecuteScalar(msSQL);
;
                    msSQL = " Select currencyexchange_gid from crm_trn_tcurrencyexchange where currency_code ='" + values.currency_code + "'";
                    string lscurrencygid = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "select customer_gid from crm_mst_tcustomer where customer_name ='"+values.customer_name.Replace("'", "\\\'") + "'";
                    string lscustomer_gid = objdbconn.GetExecuteScalar(msSQL);
                    msSQL = " select leadbank_gid from crm_trn_tleadbank where customer_gid='" + lscustomer_gid + "'";
                    string lsleadbank_gid = objdbconn.GetExecuteScalar(msSQL);

                    string lsquotation_status = "Approved";
                    string lsgst_percentage = "0.00";
                    string uiDateStr = values.quotation_date;
                    DateTime uiDate = DateTime.ParseExact(uiDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string quotation_date = uiDate.ToString("yyyy-MM-dd");


                    msSQL = "UPDATE smr_trn_treceivequotation SET " +
                             "branch_gid = '" + values.branch_name.Replace("'", "\\\'") + "', " +
                             "quotation_date = '" + quotation_date + "', " +
                             "customer_gid = '" + lscustomer_gid + "', " +
                             "customer_name = '" + values.customer_name.Replace("'", "\\\'") + "', " +
                             "created_by = '" + employee_gid + "', ";
                    if(values.quotation_remarks != null)
                    {
                        msSQL += "quotation_remarks = '" + values.quotation_remarks.Replace("'","\\\'") + "', ";
                    }
                    else
                    {
                        msSQL += "quotation_remarks = '" + values.quotation_remarks.Replace("'", "\\\'") + "', ";
                    }
                             
                           msSQL +=  "quotation_referenceno1 = '" + values.quotation_referenceno1 + "', " +
                             "payment_days = '" + values.payment_days.Replace("'", "\\\'") + "', " +
                             "delivery_days = '" + values.delivery_days.Replace("'", "\\\'") + "', " +
                             "Grandtotal = '" + values.grandtotal.Replace(",", "").Trim() + "', " +
                             "termsandconditions = '" + values.termsandconditions.Trim().Replace("<br />", "<br>").Replace("'", "\\\'") + "', " +
                             "quotation_status = '" + lsquotation_status + "', " +
                             "contact_no = '" + values.mobile + "', " +
                             "address1 = '" + values.address1.Replace("'", "\\\'") + "', " +
                             "address2 = '" + values.address2.Replace("'", "\\\'") + "', " +
                             "contact_mail = '" + values.email + "', " +
                             "addon_charge = '" + (values.addoncharge == "" || values.addoncharge == null ? "0.00" : values.addoncharge.Replace(",", "").Trim()) + "', " +
                             "additional_discount = '" + (values.additional_discount == "" || values.additional_discount == null ? "0.00" : values.additional_discount.Replace(",", "").Trim()) + "', " +
                             "addon_charge_l = '" + values.addoncharge.Replace(",", "").Trim() + "', " +
                             "additional_discount_l = '" + values.additional_discount.Replace(",", "").Trim() + "', " +
                             "grandtotal_l = '" + values.grandtotal.Replace(",", "").Trim() + "', " +
                             "currency_code = '" + values.currency_code + "', " +
                             "exchange_rate = '" + values.exchange_rate + "', " +
                             "currency_gid = '" + lscurrencygid + "', " +
                             "total_amount = '" + (values.total_amount == "" || values.total_amount == null ? "0.00" : values.total_amount.Replace(",", "").Trim()) + "', " +
                             "gst_percentage = '" + lsgst_percentage + "', " +
                             "salesperson_gid = '" + lsUsername + "', " +
                             "roundoff = '" + (values.roundoff == "" || values.roundoff == null ? "0.00" : values.roundoff) + "', " +
                             "total_price = '" + values.producttotalamount + "', " +
                             "freight_charges = '" + (values.freightcharges == "" || values.freightcharges == null ? "0.00" : values.freightcharges) + "', " +
                             "buyback_charges = '" + (values.buybackcharges == "" || values.buybackcharges == null ? "0.00" : values.buybackcharges) + "', " +
                             "packing_charges = '" + (values.packing_charges == "" || values.packing_charges == null ? "0.00" : values.packing_charges) + "', " +
                             "insurance_charges = '" + (lstaxname == "" || lstaxname == null ? "0.00" : lstaxname) + "', " +
                             "tax_amount4 = '" + (values.tax_amount4 == "" || values.tax_amount4 == null ? "0.00" : values.tax_amount4).Replace(",", "").Trim() + "', " +
                             "tax_name4 = '" + (values.tax_name4 == "" || values.tax_name4 == null ? "0.00" : values.tax_name4).Replace("'","\\\'") + "', " +
                             "created_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " +
                             "pincode = '" + values.zip_code.Replace("'", "\\\'") + "' " +
                             "WHERE quotation_gid = '" + values.quotation_gid + "'";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 0)
                    {
                        values.status = false;
                        values.message = "Error Occured while inserting Quotation";
                        return;
                    }

                    else
                    {
                        msSQL = "SELECT a.tmpsalesorderdtl_gid,a.taxsegment_gid,a.salesorder_gid, a.tax_name,a.tax_amount," +
                            "a.tax_percentage,a.tax_name2,a.tax_amount2,a.tax_percentage2,a.tax_name3,a.tax_amount3," +
                            "a.tax_percentage3,a.tax_amount, a.salesorderdtl_gid, a.salesorder_gid, a.product_gid, a.productgroup_gid," +
                            "a.tax1_gid, a.tax2_gid,a.tax3_gid, " +
                          " v.productgroup_name, a.product_name, FORMAT(a.product_price, 2) AS product_price, b.product_code, a.qty_quoted, a.product_remarks, " +
                          " a.uom_gid, a.vendor_gid, a.slno, a.uom_name, FORMAT(a.price, 2) AS price, " +
                          " FORMAT(a.discount_percentage,2) AS discount_percentage, FORMAT(a.discount_amount,2) AS discount_amount, " +
                          " FORMAT(a.selling_price, '0.00') AS selling_price,a.product_remarks, " +
                          " CONCAT( CASE WHEN a.tax_name IS NULL THEN '' ELSE a.tax_name END, ' '," +
                          "CASE WHEN a.tax_percentage = '0' THEN '' ELSE concat('',a.tax_percentage,'%') END , " +
                          " CASE WHEN a.tax_amount = '0' THEN '' ELSE concat(':',a.tax_amount) END) AS tax, " +
                          " CONCAT(CASE WHEN a.tax_name2 IS NULL THEN '' ELSE a.tax_name2 END, ' ', " +
                          " CASE WHEN a.tax_percentage2 = '0' THEN '' ELSE concat('', a.tax_percentage2, '%') END, " +
                          " CASE WHEN a.tax_amount2 = '0' THEN '' ELSE concat(':', a.tax_amount2) END) AS tax2, " +
                          " CONCAT(  CASE WHEN a.tax_name3 IS NULL THEN '' ELSE a.tax_name3 END, ' ', " +
                          " CASE WHEN a.tax_percentage3 = '0' THEN '' ELSE concat('', a.tax_percentage3, ' %   ')" +
                          " END, CASE WHEN a.tax_amount3 = '0' THEN '  ' ELSE concat(' : ', a.tax_amount3) END) AS tax3" +
                          " , format(a.tax_amount + a.tax_amount2 + a.tax_amount3, 2) as taxamount,a.tax_rate " +
                          " FROM smr_tmp_tsalesorderdtl a " +
                          " left join smr_trn_tsalesorder s on s.salesorder_gid=a.salesorder_gid " +
                          " LEFT JOIN pmr_mst_tproduct b ON a.product_gid = b.product_gid " +
                          " left join pmr_mst_tproductgroup v on b.productgroup_gid=v.productgroup_gid " +
                          " LEFT JOIN acp_mst_tvendor e ON a.vendor_gid = e.vendor_gid " +
                          " where a.salesorder_gid='" + values.quotation_gid + "'";

                        dt_datatable = objdbconn.GetDataTable(msSQL);
                        var getModuleList = new List<Post_List>();
                        if (dt_datatable.Rows.Count != 0)
                        {
                            msSQL = "delete from smr_trn_treceivequotationdtl where quotation_gid ='" + values.quotation_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if(mnResult == 1) { 

                            foreach (DataRow dt in dt_datatable.Rows)
                            {
                                
                                    msgetGid2 = objcmnfunctions.GetMasterGID("VQDT");

                                    msSQL = " SELECT a.producttype_name FROM pmr_mst_tproducttype a " +
                                            " INNER JOIN pmr_mst_tproduct b ON a.producttype_gid=b.producttype_gid  " +
                                            " WHERE product_gid='" + dt["product_gid"].ToString() + "'";
                                    objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                                  
                                    if (objOdbcDataReader.HasRows == true)
                                    {
                                        lsorder_type = "Sales";
                                        objOdbcDataReader.Close();
                                    }
                                    else
                                    {
                                        lsorder_type = "Services";
                                    }

                                    string display_field = dt["product_remarks"].ToString();


                                    msSQL = "insert into smr_trn_treceivequotationdtl (" +
                                            " quotationdtl_gid ," +
                                            " quotation_gid," +
                                            " product_gid ," +
                                            " productgroup_gid," +
                                            " productgroup_name," +
                                            " product_name," +
                                            " product_code," +
                                            " product_price," +
                                            " qty_quoted," +
                                            " discount_percentage," +
                                            " discount_amount," +
                                            " uom_gid," +
                                            " uom_name," +
                                            " price," +
                                            " quote_type," +
                                            " tax_name," +
                                            " tax_name2," +
                                            " tax_name3," +
                                            " taxsegment_gid," +
                                            " product_remarks," +
                                            " tax_percentage," +
                                            " tax_percentage2," +
                                            " tax_percentage3," +
                                            " tax_amount," +
                                            " tax_amount2," +
                                            " tax_amount3," +
                                            " tax1_gid," +
                                            " tax2_gid," +
                                            " tax3_gid," +
                                            " slno " +
                                            ")values(" +
                                            " '" + msgetGid2 + "'," +
                                            " '" + values.quotation_gid + "'," +
                                            " '" + dt["product_gid"].ToString() + "'," +
                                            " '" + dt["productgroup_gid"].ToString() + "'," +
                                            " '" + dt["productgroup_name"].ToString().Replace("'", "\\\'") + "'," +
                                            " '" + dt["product_name"].ToString().Replace("'", "\\\'") + "'," +
                                            " '" + dt["product_code"].ToString().Replace("'", "\\\'") + "'," +
                                            " '" + dt["product_price"].ToString().Replace(",", "").Trim() + "'," +
                                            " '" + dt["qty_quoted"].ToString() + "'," +
                                            " '" + dt["discount_percentage"].ToString() + "'," +
                                            " '" + dt["discount_amount"].ToString().Replace(",", "").Trim() + "'," +
                                            " '" + dt["uom_gid"].ToString() + "'," +
                                            " '" + dt["uom_name"].ToString().Replace("'", "\\\'") + "'," +
                                            " '" + dt["price"].ToString().Replace(",", "").Trim() + "'," +
                                            " '" + lsorder_type + "'," +
                                            " '" + dt["tax_name"].ToString() + "'," +
                                            " '" + dt["tax_name2"].ToString() + "'," +
                                            " '" + dt["tax_name3"].ToString() + "'," +
                                            " '" + dt["taxsegment_gid"].ToString() + "',";
                                    if(display_field != null)
                                    {
                                        msSQL += " '" + display_field.Replace("'","\\\'") + "',";
                                    }
                                    else
                                    {
                                       msSQL += " '" + display_field.Replace("'", "\\\'") + "',";
                                    }
                                         
                                        msSQL += " '" + dt["tax_percentage"].ToString() + "'," +
                                            " '" + dt["tax_percentage2"].ToString() + "'," +
                                            " '" + dt["tax_percentage3"].ToString() + "'," +
                                            " '" + dt["tax_amount"].ToString() + "'," +
                                            " '" + dt["tax_amount2"].ToString() + "'," +
                                            " '" + dt["tax_amount3"].ToString() + "'," +
                                            " '" + dt["tax1_gid"].ToString() + "'," +
                                            " '" + dt["tax2_gid"].ToString() + "'," +
                                            " '" + dt["tax3_gid"].ToString() + "'," +
                                            "'" + dt_datatable.Rows.Count + "')";

                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                }

                                //else
                                //{
                                //    msSQL =" UPDATE smr_trn_treceivequotationdtl SET" +
                                //            " product_gid = '" + dt["product_gid"].ToString() + "', " +
                                //            " productgroup_gid = '" + dt["productgroup_gid"].ToString() + "', " +
                                //            " productgroup_name = '" + dt["productgroup_name"].ToString() + "', " +
                                //            " product_name = '" + dt["product_name"].ToString() + "', " +
                                //            " product_code = '" + dt["product_code"].ToString() + "', " +
                                //            " product_price = '" + dt["product_price"].ToString().Replace(",", "").Trim() + "', " +
                                //            " qty_quoted = '" + dt["qty_quoted"].ToString() + "', " +
                                //            " discount_percentage = '" + dt["discount_percentage"].ToString() + "', " +
                                //            " discount_amount = '" + dt["discount_amount"].ToString().Replace(",", "").Trim() + "', " +
                                //            " uom_gid = '" + dt["uom_gid"].ToString() + "', " +
                                //            " uom_name = '" + dt["uom_name"].ToString() + "', " +
                                //            " price = '" + dt["price"].ToString().Replace(",", "").Trim() + "', " +
                                //            " quote_type = '" + lsorder_type + "', " +
                                //            " tax_name = '" + dt["tax_name"].ToString() + "', " +
                                //            " tax_name2 = '" + dt["tax_name2"].ToString() + "', " +
                                //            " tax_name3 = '" + dt["tax_name3"].ToString() + "', " +
                                //            " taxsegment_gid = '" + dt["taxsegment_gid"].ToString() + "', " +
                                //            " product_remarks = '" + dt["product_remarks"].ToString() + "', " +
                                //            " tax_percentage = '" + dt["tax_percentage"].ToString() + "', " +
                                //            " tax_percentage2 = '" + dt["tax_percentage2"].ToString() + "', " +
                                //            " tax_percentage3 = '" + dt["tax_percentage3"].ToString() + "', " +
                                //            " tax_amount = '" + dt["tax_amount"].ToString() + "', " +
                                //            " tax_amount2 = '" + dt["tax_amount2"].ToString() + "', " +
                                //            " tax_amount3 = '" + dt["tax_amount3"].ToString() + "', " +
                                //            " tax1_gid = '" + dt["tax1_gid"].ToString() + "', " +
                                //            " tax2_gid = '" + dt["tax2_gid"].ToString() + "', " +
                                //            " tax3_gid = '" + dt["tax3_gid"].ToString() + "' " +
                                //            " WHERE quotationdtl_gid = '" + dt["tmpsalesorderdtl_gid"].ToString() + "'";
                                //    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                //}
                            }
                            if (mnResult == 0)
                            {
                                values.status = false;
                                values.message = "Error occured while Inserting into Quotationdtl";
                                return;
                            }
                            else
                            {

                                msSQL = "select distinct quotation_type from smr_tmp_treceivequotationdtl where created_by='" + employee_gid + "' ";
                                objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                               
                                if (objOdbcDataReader.HasRows == true)
                                {
                                    lsquotation_type = "sales";
                                    objOdbcDataReader.Close();

                                }
                                else
                                {
                                    lsquotation_type = "Service";
                                }
                               
                            }

                            msSQL = " update smr_trn_treceivequotation set quotation_type='" + lsquotation_type + "' where quotation_gid='" + values.quotation_gid + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = " update smr_trn_treceivequotation Set " +
                        " leadbank_gid = '" + lsleadbank_gid + "'" +
                        " where quotation_gid='" + values.quotation_gid + "' ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }


                        string lsstage = "4";
                        msgetlead2campaign_gid = objcmnfunctions.GetMasterGID("BLCC");
                        string lsso = "N";


                        msSQL = " Insert into crm_trn_tenquiry2campaign ( " +
                                       " lead2campaign_gid, " +
                                       " quotation_gid, " +
                                       " so_status, " +
                                       " created_by, " +
                                       " customer_gid, " +
                                       " leadstage_gid," +
                                       " created_date, " +
                                       " campaign_gid," +
                                       " assign_to ) " +
                                       " Values ( " +
                                       "'" + msgetlead2campaign_gid + "'," +
                                       "'" + values.quotation_gid + "'," +
                                       "'" + lsso + "'," +
                                       "'" + employee_gid + "'," +
                                       "'" + values.customer_gid + "'," +
                                       "'" + lsstage + "'," +
                                       "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                       "'" + values.campaignGid + "'," +
                                       "'" + employee_gid + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        msgetGid4 = objcmnfunctions.GetMasterGID("PODC");
                        {
                            msSQL = " insert into smr_trn_tapproval ( " +
                                    " approval_gid, " +
                                    " approved_by, " +
                                    " approved_date, " +
                                    " submodule_gid, " +
                                    " qoapproval_gid " +
                                    " ) values ( " +
                                    "'" + msgetGid4 + "'," +
                                    " '" + employee_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                    "'SMRSMRQAP'," +
                                    "'" + values.quotation_gid + "') ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                            msSQL = "select approval_flag from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + values.quotation_gid + "' ";
                            objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                            if (objOdbcDataReader.HasRows == false)
                            {
                               
                                msSQL = " Update smr_trn_treceivequotation Set " +
                                       " quotation_status = 'Approved', " +
                                       " approved_by = '" + employee_gid + "', " +
                                       " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                       " where quotation_gid = '" + values.quotation_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                objOdbcDataReader.Close();


                            }
                            else
                            {
                                msSQL = "select approved_by from smr_trn_tapproval where submodule_gid='SMRSMRQAP' and qoapproval_gid='" + values.quotation_gid + "'";
                                objOdbcDataReader1 = objdbconn.GetDataReader(msSQL);
                            }
                            if (objOdbcDataReader1.RecordsAffected == 1)
                            {
                                
                                msSQL = " update smr_trn_tapproval set " +
                               " approval_flag = 'Y', " +
                               " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                               " where approved_by = '" + employee_gid + "'" +
                               " and qoapproval_gid = '" + values.quotation_gid + "' and submodule_gid='SMRSMRQAP'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                msSQL = " Update smr_trn_treceivequotation Set " +
                                       " quotation_status = 'Approved', " +
                               " approved_by = '" + employee_gid + "', " +
                               " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                               " where quotation_gid = '" + values.quotation_gid + "'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                
                            }
                            else if (objOdbcDataReader1.RecordsAffected > 1)
                            {
                               
                                msSQL = " update smr_trn_tapproval set " +
                                       " approval_flag = 'Y', " +
                                       " approved_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                                       " where approved_by = '" + employee_gid + "'" +
                                       " and qoapproval_gid = '" + values.quotation_gid + "' and submodule_gid='SMRSMRQAP'";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                
                            }
                        }


                        if (mnResult == 0 || mnResult != 0)
                        {
                            msSQL = " delete from smr_tmp_tsalesorderdtl " +
                                 " where employee_gid='" + employee_gid + "'";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }


                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Quotation Updated Successfully!";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Quotation!";
                            return;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submiting  Quotation !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }
    }
}



