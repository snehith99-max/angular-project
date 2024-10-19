using ems.sales.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using OfficeOpenXml.Style;
using System.Drawing;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Text;


namespace ems.sales.DataAccess
{
    public class DaSmrTrnCustomerSummary
    {
        HttpPostedFile httpPostedFile;

        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        finance_cmnfunction objfinance = new finance_cmnfunction();
        string msSQL, msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string exclcustomer_code, lsregiongid, lscountrygid, lscurrencyexchangegid, msGetGid4, msGetGid5, msGetGID11;
        string lspricesegment_gid, lscustomer_name,lspricesegment_name, lscustomercode, msGetGid, lscustomertypegid, msGetGid1, msGetGid2, msGetGid3;
        int mnResult, mnResult1, mnResult2, mnResult3;
        public void DaGetSmrTrnCustomerSummary(MdlSmrTrnCustomerSummary values)
        {
            try {

                //msSQL = " Select distinct UCASE(a.customer_id) as customer_id,y.user_firstname, a.customer_gid,z.pricesegment_name,a.customer_name, concat(c.customercontact_name,' / ',c.mobile,' / ',c.email) as contact_details, " +
                //"  d.region_name, e.customer_type,a.customer_state, " +
                //"  CONCAT((SELECT CONCAT(DATEDIFF(CURDATE(), MAX(salesorder_date)), ' days') FROM smr_trn_tsalesorder WHERE customer_gid = a.customer_gid)) AS last_order_date, " +

                //" (SELECT DATE_FORMAT(Min(salesorder_date), '%d %b %Y') FROM smr_trn_tsalesorder WHERE customer_gid = a.customer_gid) AS first_order_date," +

                //" (SELECT CONCAT(DATEDIFF(CURDATE(), MIN(created_date)), ' days') FROM crm_mst_tcustomer WHERE customer_gid = a.customer_gid) as customer_since," +
                //" a.status as status" + 
                //" from crm_mst_tcustomer a" +                
                //" left join crm_mst_tcustomercontact c on a.customer_gid=c.customer_gid " +
                //  " left join adm_mst_tuser y on a.salesperson_gid = y.user_gid " +
                //" left join crm_mst_tregion d on a.customer_region=d.region_gid " +
                //"left join smr_trn_tpricesegment z on a.pricesegment_gid = z.pricesegment_gid " +
                //" left join crm_mst_tcustomertype e on a.customer_type=e.customertype_gid " +
                //" where c.customerbranch_name='H.Q'" +
                //" order by a.created_date DESC ";

                var smrcustomer_list = new List<smrcustomer_list>();
                msSQL = "call crm_mst_spcustomersummary";

            dt_datatable = objdbconn.GetDataTable(msSQL);
           
            if (dt_datatable.Rows.Count > 0)
            {
             var smrcustomer_listdict = new Dictionary<string, smrcustomer_list>();
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        string customer_gid = dt["customer_gid"].ToString();
                      if (!smrcustomer_listdict.ContainsKey(customer_gid))
                      {
                            smrcustomer_listdict[customer_gid] = new smrcustomer_list
                            {
                                customer_gid = customer_gid,
                                customer_id = dt["customer_id"].ToString(),
                                customer_name = dt["customer_name"].ToString(),
                                contact_details = dt["contact_details"].ToString(),
                                region_name = dt["region_name"].ToString(),
                                sales_person = dt["user_firstname"].ToString(),
                                pricesegment_name = dt["pricesegment_name"].ToString(),
                                customer_state = dt["customer_state"].ToString(),
                                customer_since = dt["customer_since"].ToString(),
                                statuses = dt["status"].ToString(),
                            };
                      }
                    };
                    smrcustomer_list = smrcustomer_listdict.Values.ToList();
                    values.smrcustomer_list = smrcustomer_list;
                
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Customer Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }
        public void DaGetSmrTrnCustomertotalCount(string employee_gid, string user_gid, MdlSmrTrnCustomerSummary values)
        {
            try
            {
                msSQL = " select (select count(customer_gid) from crm_mst_tcustomer a left join crm_mst_tcustomertype b on a.customer_type=b.customertype_gid where b.customertype_gid='BCRT240331001' and b.display_name='Distributor') as distributor_count, " +
                    " (select count(customer_gid) from crm_mst_tcustomer a left join crm_mst_tcustomertype b on a.customer_type=b.customertype_gid where b.customertype_gid='BCRT240331002' and b.display_name='Retailer') as retailer_counts," +
                    " (select count(customer_gid) from crm_mst_tcustomer a left join crm_mst_tcustomertype b on a.customer_type=b.customertype_gid where b.customertype_gid='BCRT240331000' and b.display_name='Corporate') as corporate_count, " +
                    " (select count(x.customer_gid) from crm_mst_tcustomer x left join crm_mst_tcustomercontact z on x.customer_gid = z.customer_gid where z.main_contact = 'Y') as total_count ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var customertotalcount_list = new List<customertotalcount_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        customertotalcount_list.Add(new customertotalcount_list
                        {
                            dealer_count = (dt["distributor_count"].ToString()),
                            retailer_counts = (dt["retailer_counts"].ToString()),
                            corporate_count = (dt["corporate_count"].ToString()),
                            total_count = (dt["total_count"].ToString()),
                        });
                        values.customertotalcount_list = customertotalcount_list;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer Count !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
        }

        public void DaGetcountry(MdlSmrTrnCustomerSummary values)
        {
            try {
               
                msSQL = " Select country_name,country_gid  " +
                    " from adm_mst_tcountry ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getcountry>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getcountry
                    {
                        country_name = dt["country_name"].ToString(),
                        country_gid = dt["country_gid"].ToString(),
                    });
                    values.Getcountry = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Country Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        public void DaGetpricesegment(MdlSmrTrnCustomerSummary values)
        {
            try
            {

                msSQL = " Select pricesegment_name,pricesegment_gid  " +
                    " from smr_trn_tpricesegment ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<pricesegment_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new pricesegment_list
                        {
                            pricesegment_name = dt["pricesegment_name"].ToString(),
                            pricesegment_gid = dt["pricesegment_gid"].ToString(),
                        });
                        values.pricesegment_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Country Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

        public void GetOnChangeCountry(string country_gid, MdlSmrTrnCustomerSummary values)
        {
            try {
                
                msSQL = " select a.country_gid,a.country_name,b.currency_code ,b.exchange_rate from adm_mst_tcountry a  " +
                    " left join crm_trn_tcurrencyexchange b on a.country_gid = b.country_gid " +
                    " where a.country_gid='" + country_gid + "'";
           

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetOnchangecuontry>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetOnchangecuontry
                    {

                        country_name = dt["country_name"].ToString(),
                        currency_code = dt["currency_code"].ToString(),
                    });
                    values.GetOnchangecuontry = getModuleList;
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
        public void daGetsalespersondtl( MdlSmrTrnCustomerSummary values)
        {
            try
            {

                msSQL = "select user_gid, user_firstname from adm_mst_tuser";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getsalespersondtl_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getsalespersondtl_list
                        {

                            salesperson_gid = dt["user_gid"].ToString(),
                            salesperson_name = dt["user_firstname"].ToString(),
                        });
                        values.Getsalespersondtl_list = getModuleList;
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
        public void dagetcurrencydtl( MdlSmrTrnCustomerSummary values)
        {
            try
            {

                msSQL = "select currencyexchange_gid,currency_code from crm_trn_tcurrencyexchange";


                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<getcurrencydtl_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new getcurrencydtl_list
                        {
                            currency_gid = dt["currencyexchange_gid"].ToString(),
                            currency_name = dt["currency_code"].ToString(),
                        });
                        values.getcurrencydtl_list = getModuleList;
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




        public void DaGetregion(MdlSmrTrnCustomerSummary values)
        {
            try {
              
                msSQL = " SELECT region_gid, region_code, concat_ws('/',city,region_name) as region_name  FROM crm_mst_tregion Order by region_name asc";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<Getregion>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new Getregion
                    {
                        region_name = dt["region_name"].ToString(),
                        region_gid = dt["region_gid"].ToString(),
                    });
                    values.Getregion = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Region !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
       

        public void DaPostCustomer(string user_gid, postcustomer_list values)

        {
            try {

                msSQL = " select * from crm_mst_tcustomer where customer_name= '" + values.customer_name.Replace("'", "\\\'") + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {                   
                    values.message = "Customer Name Already Exist";
                    return;
                }
                else
                {

                    msGetGid = objcmnfunctions.GetMasterGID("CC");
            msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='CC'";
            string lsCode = objdbconn.GetExecuteScalar(msSQL);

            string lscustomer_code = "CC-" + "00" + lsCode;
            string lscustomercode = "H.Q";
            string lscustomer_branch = "H.Q";


                //msSQL = " Select customer_type from crm_mst_tcustomertype where customertype_gid='" + values.customer_type + "'";
                //string lscustomer_type = objdbconn.GetExecuteScalar(msSQL);

                    {
                        msGetGid = objcmnfunctions.GetMasterGID("BCRM");
                        msGetGid1 = objcmnfunctions.GetMasterGID("BCCM");
                        msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                        msGetGid3 = objcmnfunctions.GetMasterGID("BLCC");
                        msSQL = " insert into crm_mst_tcustomer (" +
                           " customer_gid," +
                           " customer_id, " +
                           " customer_code, " +
                           " customer_name, " +
                           " company_website, " +
                           " customer_address, " +
                           " customer_address2," +
                           " customer_city," +
                           " customer_country," +
                           " customer_region," +
                           " customer_state," +
                           " gst_number ," +
                           " customer_pin ," +
                           " customer_type ," +
                           " status ," +
                           " taxsegment_gid ," +
                           " pricesegment_gid ," +
                           " salesperson_gid ," +
                           " credit_days ," +
                           " currency_gid ," +
                           " created_by," +
                           "created_date" +
                           ") values (" +
                           "'" + msGetGid + "', " +
                           "'" + lscustomer_code + "'," +
                           "'H.Q'," +
                           "'" + values.customer_name.Replace("'", "\\\'") + "'," +
                           "'" + values.company_website + "'," +
                           "'" + values.address1.Replace("'", "\\\'") + "',";
                            if (values.address2 != null)
                            {

                               msSQL+= "'" + values.address2.Replace("'", "\\\'") + "',";
                            }
                            else
                            {
                                msSQL += "'" + values.address2 + "',";
                            }
                         msSQL+=  "'" + (String.IsNullOrEmpty(values.customer_city) ? values.customer_city : values.customer_city.Replace("'","\\\'")) + "'," +

                           "'" + values.countryname + "'," +
                           "'" + values.region_name + "'," +
                           "'" + (String.IsNullOrEmpty(values.customer_state) ? values.customer_state :  values.customer_state.Replace("'","\\\'")) + "'," +
                           "'" + values.tax_no + "'," +
                           "'" + values.postal_code + "'," +
                           "'" + values.customer_type + "'," +
                           "'Active'," +
                           "'" + values.taxsegment_name + "'," +
                           "'" + values.pricesegment_name + "'," +
                           "'" + values.sales_person + "',";
                            if (values.credit_days == null || values.credit_days == "")
                            {
                                  msSQL += "'" + 0 + "',";
                            }
                            else
                            {
                           
                                  msSQL += "'" + values.credit_days + "',";
                            }
                            msSQL += "'" + values.currency + "'," +
                            "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        

                        if (mnResult != 0)
                        {
                            //objfinance.finance_vendor_debitor("Sales", exclcustomer_code, values.customer_name, msGetGid, user_gid);
                            //string trace_comment = " Added a customer on " + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //objcmnfunctions.Tracelog(msGetGid, user_gid, trace_comment, "added_customer");

                            if (values.pricesegment_name != null || values.pricesegment_name != "undefined" || values.pricesegment_name != "")
                            {

                                msGetGID11 = objcmnfunctions.GetMasterGID("VPDC");
                                msSQL = "insert into smr_trn_tpricesegment2customer (" +
                                    "pricesegment2customer_gid," +
                                    "pricesegment_gid," +
                                    "customer_gid," +
                                    "customer_name," +
                                    "created_by," +
                                    "created_date " +
                                    ") values (" +
                                    "'" + msGetGID11 + "'," +
                                    "'" + values.pricesegment_name + "'," +
                                    "'" + msGetGid + "'," +
                                    "'" + values.customer_name.Replace("'", "\\\'") + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            }

                            msSQL = " insert into crm_mst_tcustomercontact  (" +
                            " customercontact_gid," +
                            " customer_gid," +
                            " customercontact_name, " +
                            " customerbranch_name, " +
                            " email, " +
                              " billing_email, " +
                            " mobile, " +
                             " main_contact, " +
                            " designation," +
                            " address1," +
                            " address2," +
                            " state," +
                            " city," +
                            " country_gid," +
                            " region," +
                            " fax, " +
                            " zip_code, " +

                            " fax_country_code," +
                            " gst_number, " +
                             " created_by," +
                           "created_date" +

                            ") values (" +
                            "'" + msGetGid1 + "', " +
                            "'" + msGetGid + "', " +
                            "'" + values.customercontact_name.Replace("'", "\\\'") + "'," +
                            "'" + lscustomer_branch + "'," +
                            "'" + values.email + "'," +
                              "'" + values.billemail + "',";
                            if (values.mobiles != null)
                            {

                                msSQL += "'" + values.mobiles.e164Number + "',";
                            }
                            else
                            {
                                msSQL += "'',";
                            }
                            msSQL += "'Y'," +
                            "'" + values.designation + "'," +
                            "'" + values.address1.Replace("'", "\\\'") + "',";

                           if (values.address2 != null)
                            {

                                msSQL += "'" + values.address2.Replace("'", "\\\'") + "',";
                            }
                            else
                            {
                                msSQL += "'" + values.address2 + "',";
                            }
                            msSQL+="'" + (String.IsNullOrEmpty(values.customer_state) ? values.customer_state : values.customer_state.Replace("'", "\\\'")) + "'," +
                            "'" + (String.IsNullOrEmpty(values.customer_city) ? values.customer_city : values.customer_city.Replace("'", "\\\'")) + "'," +
                             "'" + values.countryname + "'," +
                            "'" + values.region_name + "'," +
                             "'" + values.fax + "'," +
                             "'" + values.postal_code + "'," +
                       
                             "'" + values.country_code + "'," +
                           "'" + values.tax_no + "'," +
                           "'" + user_gid + "'," +
                             "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                        if (mnResult != 0)
                        {

                            msSQL = " INSERT INTO crm_trn_tleadbank " +
                                   " (leadbank_gid, " +
                                   " customer_gid, " +
                                   " leadbank_name," +
                                   " leadbank_address1, " +
                                   " leadbank_address2, " +
                                   " leadbank_city, " +
                                   " leadbank_code, " +
                                   " leadbank_state, " +
                                   " leadbank_pin, " +
                                   " leadbank_country, " +
                                   " leadbank_region, " +
                                   " approval_flag, " +
                                   " leadbank_id, " +
                                   " status, " +
                                   " main_branch," +
                                   " main_contact," +
                                   " customer_type," +
                                   " customertype_gid," +
                                   " created_by, " +
                                   " created_date)" +
                                   " values ( " +
                                   "'" + msGetGid2 + "'," +
                                   "'" + msGetGid + "'," +
                                   "'" + values.customer_name.Replace("'", "\\\'") + "'," +
                                   "'" + values.address1.Replace("'", "\\\'") + "',";
                                   if (values.address2 != null)
                                   {

                                msSQL += "'" + values.address2.Replace("'", "\\\'") + "',";
                                   }
                            else
                            {
                                msSQL += "'" + values.address2 + "',";
                            }
                           msSQL += "'" + (String.IsNullOrEmpty(values.customer_city) ? values.customer_city : values.customer_city.Replace("'", "\\\'")) + "'," +
                                   "'" + lscustomercode + "'," +
                                   "'" + (String.IsNullOrEmpty(values.customer_state) ? values.customer_state : values.customer_state.Replace("'", "\\\'")) + "'," +
                                   "'" + values.postal_code + "'," +
                                   "'" + values.countryname + "'," +
                                   "'" + values.region_name + "'," +
                                   "'Approved'," +
                                   "'" + lscustomer_code + "'," +
                                   "'Y'," +
                                   "'Y'," +
                                   "'Y'," +
                                    "'Corporate'," +
                                    "'Corporate'," +
                                   "'" + user_gid + "'," +
                                   "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                        if (mnResult != 0)
                        {
                            msSQL = " INSERT into crm_trn_tleadbankcontact (" +
                                " leadbankcontact_gid, " +
                                " leadbank_gid," +
                                " leadbankbranch_name, " +
                                " leadbankcontact_name," +
                                " email," +
                                " mobile," +
                                " designation," +
                                " did_number," +
                                " created_date," +
                                " created_by," +
                                " address1," +
                                " address2, " +
                                " state, " +
                                " country_gid, " +
                                " city, " +
                                " pincode, " +
                                " region_name, " +
                                " main_contact," +
                                " phone1," +

                                " country_code1," +
                                " fax," +

                                " fax_country_code)" +
                                " values (" +
                                " '" + msGetGid3 + "'," +
                                " '" + msGetGid2 + "'," +
                                "'" + lscustomercode + "'," +
                                "'" + values.customercontact_name.Replace("'", "\\\'") + "'," +
                                "'" + values.email + "'," +
                                "'" + values.mobile + "'," +
                                "'" + values.designation + "'," +
                                "'0'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                "'" + user_gid + "'," +
                                "'" + values.address1.Replace("'", "\\\'") + "',";
                                if (values.address2 != null)
                                {

                                msSQL += "'" + values.address2.Replace("'", "\\\'") + "',";
                                }
                            else
                            {
                                msSQL += "'" + values.address2 + "',";
                            }
                          msSQL+=  "'" + (String.IsNullOrEmpty(values.customer_state) ? values.customer_state : values.customer_state.Replace("'", "\\\'")) + "'," +
                                "'" + values.countryname + "'," +
                                "'" + (String.IsNullOrEmpty(values.customer_city) ? values.customer_city :  values.customer_city.Replace("'","\\\'")) + "'," +
                                "'" + values.postal_code + "'," +
                                "'" + values.region_name + "'," +
                                "'Y'," +
                                "'" + values.phone1 + "'," +
                              
                                "'" + values.country_code + "'," +
                                "'" + values.fax + "'," +
                              
                                "'" + values.fax_country_code + "')";

                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        }

                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Customer Added Successfully";
                            return;
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Customer";
                            return;
                        }
                    }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Submitting Customer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaGetSmrTrnDistributorSummary(MdlSmrTrnCustomerSummary values)
        {
            try {
              
                msSQL = " Select distinct UCASE(a.customer_id) as customer_id,a.status as status,a.customer_gid, a.customer_name, a.customer_state, concat(c.customercontact_name,' / ',c.mobile,' / ',c.email) as contact_details, " +
                " d.region_name from crm_mst_tcustomer a" +
                " left join crm_mst_tregion d on a.customer_region =d.region_gid " +
                " left join crm_mst_tcustomercontact c on a.customer_gid=c.customer_gid " +
                " left join crm_mst_tcustomertype e on a.customer_type=e.customertype_gid " +
                " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid  where  c.customerbranch_name='H.Q' and  e.customertype_gid = 'BCRT240331001' and e.display_name='Distributor'" +
                " group  by a.customer_gid asc order by a.created_by DESC";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<smrcustomer_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new smrcustomer_list
                    {
                        customer_gid = dt["customer_gid"].ToString(),
                        customer_id = dt["customer_id"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        customer_state = dt["customer_state"].ToString(),
                        statuses = dt["status"].ToString(),
                    });
                    values.smrcustomer_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Distributor !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }
        public void DaGetSmrTrnRetailerSummary(MdlSmrTrnCustomerSummary values)
        {
            try {
               
                msSQL = " Select distinct UCASE(a.customer_id) as customer_id,a.status as status,a.customer_gid, a.customer_name, a.customer_state, concat(c.customercontact_name,' / ',c.mobile,' / ',c.email) as contact_details, " +                
                " d.region_name from crm_mst_tcustomer a" +
                " left join crm_mst_tregion d on a.customer_region =d.region_gid " +
                " left join crm_mst_tcustomercontact c on a.customer_gid=c.customer_gid " +
                " left join crm_mst_tcustomertype e on a.customer_type=e.customertype_gid " +
                " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid  where  c.customerbranch_name='H.Q' and e.customertype_gid = 'BCRT240331002' and e.display_name='Retailer' " +
                 " group  by a.customer_gid asc order by a.created_by DESC"; 

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<smrcustomer_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new smrcustomer_list
                    {
                        customer_gid = dt["customer_gid"].ToString(),
                        customer_id = dt["customer_id"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        customer_state = dt["customer_state"].ToString(),
                        statuses = dt["status"].ToString(),
                    });
                    values.smrcustomer_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Retailer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }
        public void DaGetSmrTrnCorporateSummary(MdlSmrTrnCustomerSummary values)
        {
            try {
               
                msSQL = " Select distinct UCASE(a.customer_id) as customer_id,a.status as status,a.customer_gid, a.customer_name, a.customer_state, concat(c.customercontact_name,' / ',c.mobile,' / ',c.email) as contact_details, " +
                " d.region_name from crm_mst_tcustomer a" +
                " left join crm_mst_tregion d on a.customer_region =d.region_gid " +
                " left join crm_mst_tcustomercontact c on a.customer_gid=c.customer_gid " +
                " left join crm_mst_tcustomertype e on a.customer_type=e.customertype_gid " +
                " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid  where c.customerbranch_name='H.Q' and e.customertype_gid = 'BCRT240331000' and e.display_name='Corporate' " +
                 " order  by a.created_by desc"; 

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<smrcustomer_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new smrcustomer_list
                    {
                        customer_gid = dt["customer_gid"].ToString(),
                        customer_id = dt["customer_id"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        contact_details = dt["contact_details"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        customer_state = dt["customer_state"].ToString(),
                        statuses = dt["status"].ToString(),
                    });
                    values.smrcustomer_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured whileLoading Corporate!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaGetSmrTrnCustomerCount(string employee_gid, string user_gid, MdlSmrTrnCustomerSummary values)
        {
            try {

                msSQL = "select count(b.display_name) as customercount,b.display_name,count(a.customer_gid) as total_count from crm_mst_tcustomer a "+
                    "left join crm_mst_tcustomertype b on  a.customer_type = b.customertype_gid "+
                      " where a.customer_type is not null and b.customer_type is not null group by b.display_name order by customercount DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
            var customercount_list = new List<customercount_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    customercount_list.Add(new customercount_list
                    {
                       
                        display_name = (dt["display_name"].ToString()),
                        customercount = (dt["customercount"].ToString()),
                        total_count= (dt["total_count"].ToString()),
                    });
                    values.customercount_list = customercount_list;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer Count !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaGetViewcustomerSummary(string customer_gid, MdlSmrTrnCustomerSummary values)
        
        {
            try {
               

                msSQL = " SELECT a.customer_gid,f.customer_type,z.pricesegment_name,h.user_firstname,a.credit_days, a.customer_name, a.company_website, UCASE(a.customer_id) as customer_id,a.gst_number, " +
                " a.customer_code,b.currency_code, a.customer_address,c.phone,c.area_code,c.country_code,c.fax,c.fax_area_code, " +
                " a.customer_city, a.customer_state,  e.country_name as customer_country, a.customer_pin,c.fax_country_code, " +
                " d.region_name, a.main_branch, a.customer_address2,c.customercontact_gid, c.customer_gid, c.customerbranch_name ," +
                " c.customercontact_name, c.email, " +
                " c.mobile, c.designation, c.main_contact,d.region_name,a.customer_country,g.taxsegment_gid,g.taxsegment_name FROM crm_mst_tcustomer a " +
                 " left join crm_trn_tcurrencyexchange b on a.currency_gid=b.currencyexchange_gid " +
                " left join crm_mst_tcustomercontact c on a.customer_gid = c.customer_gid " +
                " left join crm_mst_tregion d on a.customer_region = d.region_gid " +
                " left join adm_mst_tcountry e on  e.country_gid  = a.customer_country " +
                  " left join adm_mst_tuser h on  h.user_gid  = a.salesperson_gid " +
                " left join crm_mst_tcustomertype f on  a.customer_type  = f.customertype_gid " +               
                " left join acp_mst_ttaxsegment g on g.taxsegment_gid=a.taxsegment_gid " +
                "left join smr_trn_tpricesegment z on z.pricesegment_gid = a.pricesegment_gid" +
                " Where a.customer_gid = '" + customer_gid + "'" +
                "group  by a.customer_gid asc ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<postcustomer_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new postcustomer_list
                    {


                        customer_id = dt["customer_id"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        company_website = dt["company_website"].ToString(),
                        customercontact_name = dt["customercontact_name"].ToString(),
                        designation = dt["designation"].ToString(),
                        mobile = dt["mobile"].ToString(),
                        email = dt["email"].ToString(),
                        sales_person = dt["user_firstname"].ToString(),
                        currency = dt["currency_code"].ToString(),
                        credit_days = dt["credit_days"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        pricesegment_name = dt["pricesegment_name"].ToString(),
                        customer_address = dt["customer_address"].ToString(),
                        customer_address2 = dt["customer_address2"].ToString(),
                        customer_city = dt["customer_city"].ToString(),
                        customer_state = dt["customer_state"].ToString(),
                        countryname = dt["customer_country"].ToString(),
                        customer_pin = dt["customer_pin"].ToString(),
                        region_name = dt["region_name"].ToString(),
                        customer_code = dt["customer_code"].ToString(),
                        phone = dt["phone"].ToString(),
                       
                        country_code = dt["country_code"].ToString(),
                        fax = dt["fax"].ToString(),
                      
                        fax_country_code = dt["fax_country_code"].ToString(),
                        gst_number = dt["gst_number"].ToString(),
                        customerbranch_name = dt["customerbranch_name"].ToString(),
                        taxsegment_name = dt["taxsegment_name"].ToString(),

                    });
                    values.postcustomer_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Cutomer View !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaGetcustomerInactive(string Customer_gid, MdlSmrTrnCustomerSummary values)
        {
            try {
              
                msSQL = " update crm_mst_tcustomer set" +
                        " status='Inactive'" +
                        " where customer_gid = '" + Customer_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Customer Inactivated Successfully";
                    return;
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Customer Inactivated";
                        return;
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  Updating Customer Inactivated !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaGetcustomerActive(string Customer_gid, MdlSmrTrnCustomerSummary values)
        {
            try {
               
                msSQL = " update crm_mst_tcustomer set" +
                        " status='Active'" +
                        " where customer_gid = '" + Customer_gid + "'";
            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = "Customer Activated Successfully";
                    return;
            }
            else
            {
                {
                    values.status = false;
                    values.message = "Error While Customer Activated ";
                        return;
                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Customer Activated !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }


       
        //custmerpricesegment summary
        public void DaGetProductAssignSummary(string customer_gid, MdlSmrTrnCustomerSummary values)
        {
            try {
               
                msSQL = "select * from smr_trn_tpricesegment2customer where customer_gid='" + customer_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count == 0)
            {
                msSQL = " SELECT d.producttype_name,concat(b.productgroup_code,   ' | '   ,b.productgroup_name) as productgroup_name, a.product_gid, a.cost_price, a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name, " +
                    " CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date, " +
                    " (select customer_gid from crm_mst_tcustomer where customer_gid='" + customer_gid + "') as customer_gid ," +
                    " (select customer_name from crm_mst_tcustomer where customer_gid='" + customer_gid + "') as customer_name ," +
                    " c.productuomclass_code,e.productuom_code,c.productuomclass_name,(case when a.stockable ='Y' then 'Yes' else 'No ' end) as stockable," +
                    " e.productuom_name,e.productuom_gid,d.producttype_name as product_type,(case when a.status ='1' then 'Active' else 'Inactive' end) as Status," +
                    " (case when a.serial_flag ='Y' then 'Yes' else 'No' end)as serial_flag,a.product_price,a.mrp_price," +
                    " (case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time,'  ', 'days') end)as lead_time  from pmr_mst_tproduct a " +
                    " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid " +
                    " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid " +
                    " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid " +
                    " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid " +
                    " left join smr_trn_tpricelistdtl i on i.product_gid = a.product_gid " +
                    " left join adm_mst_tuser f on f.user_gid=a.created_by group by a.product_name order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getproductlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getproductlist
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            selling_price = dt["mrp_price"].ToString(),
                            product_price = dt["mrp_price"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                        });
                        values.product_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            else {
                msSQL = " SELECT d.producttype_name,concat(b.productgroup_code,   ' | '   ,b.productgroup_name) as productgroup_name, a.product_gid, a.cost_price, a.product_code, CONCAT_WS('|',a.product_name,a.size, a.width, a.length) as product_name,"+
                        " CONCAT(f.user_firstname,' ',f.user_lastname) as created_by,date_format(a.created_date,'%d-%m-%Y')  as created_date,j.stock_gid, g.pricesegment2product_gid," +
                        " (select customer_gid from crm_mst_tcustomer where customer_gid='" + customer_gid + "') as customer_gid , " +
                        " (select customer_name from crm_mst_tcustomer where customer_gid='" + customer_gid + "') as customer_name , " +
                        " c.productuomclass_code,e.productuom_code,c.productuomclass_name,(case when a.stockable ='Y' then 'Yes' else 'No ' end) as stockable, " +
                        " e.productuom_name,e.productuom_gid,d.producttype_name as product_type,(case when a.status ='1' then 'Active' else 'Inactive' end) as Status," +
                        " (case when a.serial_flag ='Y' then 'Yes' else 'No' end)as serial_flag,g.product_price,a.mrp_price, " +
                        " (case when a.avg_lead_time is null then '0 days' else concat(a.avg_lead_time,'  ', 'days') end)as lead_time  from pmr_mst_tproduct a  " +
                        " left join pmr_mst_tproductgroup b on a.productgroup_gid = b.productgroup_gid  " +
                        " left join pmr_mst_tproductuomclass c on a.productuomclass_gid = c.productuomclass_gid  " +
                        " left join pmr_mst_tproducttype d on a.producttype_gid = d.producttype_gid  " +
                        " left join pmr_mst_tproductuom e on a.productuom_gid = e.productuom_gid  " +
                        " left join smr_trn_tpricelistdtl i on i.product_gid = a.product_gid  " +
                        " left join adm_mst_tuser f on f.user_gid=a.created_by " +
                        " left join smr_trn_tpricesegment2product g on g.product_gid=a.product_gid " +
                        " left join smr_trn_tpricesegment2customer h on h.pricesegment_gid=g.pricesegment_gid  " +
                        " left join ims_trn_tstock j on j.product_gid=g.product_gid " +
                        " where h.customer_gid='" + customer_gid + "'" +
                        " group by a.product_name order by a.created_date desc";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getproductlist>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getproductlist
                        {
                            customer_gid = dt["customer_gid"].ToString(),
                            customer_name = dt["customer_name"].ToString(),
                            product_gid = dt["product_gid"].ToString(),
                            product_name = dt["product_name"].ToString(),
                            product_code = dt["product_code"].ToString(),
                            productgroup_name = dt["productgroup_name"].ToString(),
                            cost_price = dt["cost_price"].ToString(),
                            selling_price = dt["product_price"].ToString(),
                            product_price = dt["mrp_price"].ToString(),
                            productuomclass_name = dt["productuomclass_name"].ToString(),
                            productuom_name = dt["productuom_name"].ToString(),
                            productuom_gid = dt["productuom_gid"].ToString(),
                            stock_gid = dt["stock_gid"].ToString(),
                            pricesegment2product_gid = dt["pricesegment2product_gid"].ToString(),
                        });
                        values.product_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Assigning Product !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        // Customer edit summary
        public void DaGetEditCustomer(string customer_gid, MdlSmrTrnCustomerSummary values)
        {
            try {
               
                msSQL = " SELECT a.customer_gid,a.customer_name,c.billing_email,a.credit_days,i.pricesegment_name,b.currency_code,b.currencyexchange_gid,i.pricesegment_gid,j.user_firstname,j.user_gid, a.company_website,f.customer_type,a.gst_number, a.customer_id ,c.phone,c.area_code,c.country_code,c.fax,c.fax_area_code," +
                    " a.customer_code,a.customer_address,c.city,a.customer_country,c.state,c.fax_country_code,c.country_gid , " +
                    " c.zip_code,a.customer_region,c.customercontact_gid,d.region_name,c.customercontact_name,b.currency_code,c.email,c.mobile,c.designation,a.customer_address2,g.taxsegment_name,a.taxsegment_gid " +
                    " from crm_mst_tcustomer a" +
                    " left join crm_mst_tcustomercontact c on a.customer_gid=c.customer_gid " +
                    " left join crm_mst_tregion d on a.customer_region = d.region_gid " +
                    " left join adm_mst_tcountry e on a.customer_country = e.country_gid " +
                    " left join crm_mst_tcustomertype f on a.customer_type = f.customertype_gid " +
                     " left join smr_trn_tpricesegment i on a.pricesegment_gid = i.pricesegment_gid " +
                       " left join adm_mst_tuser j on a.salesperson_gid = j.user_gid " +
                    " left join crm_trn_tcurrencyexchange b on a.currency_gid=b.currencyexchange_gid " +
                     " left join acp_mst_ttaxsegment g on g.taxsegment_gid=a.taxsegment_gid " +
                    " where a.customer_gid ='" + customer_gid + "' and  c.main_contact = 'Y'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetCustomerlist>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetCustomerlist
                    {
                        customer_gid = dt["customer_gid"].ToString(),
                        customer_id = dt["customer_id"].ToString(),
                        customer_name = dt["customer_name"].ToString(),
                        customercontact_name = dt["customercontact_name"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                        mobile_number= dt["mobile"].ToString(),
                        address1 = dt["customer_address"].ToString(),
                        address2 = dt["customer_address2"].ToString(),
                        email = dt["email"].ToString(),
                        city = dt["city"].ToString(),
                        postal_code = dt["zip_code"].ToString(),
                        country_name = dt["customer_country"].ToString(),
                        currencyname = dt["currency_code"].ToString(),
                        region_name = dt["customer_region"].ToString(),
                        gst_number = dt["gst_number"].ToString(),
                        company_website = dt["company_website"].ToString(),
                        country_code = dt["fax_country_code"].ToString(),
                        area_code = dt["fax_area_code"].ToString(),
                        fax_number = dt["fax"].ToString(),
                        designation = dt["designation"].ToString(),
                        customer_state = dt["state"].ToString(),
                        billing_email = dt["billing_email"].ToString(),
                        pricesegment_name = dt["pricesegment_name"].ToString(),
                        pricesegment_gid = dt["pricesegment_gid"].ToString(),
                        sales_person = dt["user_firstname"].ToString(),
                        salesperson_gid = dt["user_gid"].ToString(),
                        currency = dt["currency_code"].ToString(),
                        currency_gid = dt["currencyexchange_gid"].ToString(),
                        credit_days = dt["credit_days"].ToString(),
                        taxsegment_name = dt["taxsegment_name"].ToString(),
                        taxsegment_gid = dt["taxsegment_gid"].ToString(),

                    });
                    values.GetCustomerList = getModuleList;
                }

            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting  Customer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
          
        }

        public void DaGetcustomername(string customer_gid, MdlSmrTrnCustomerSummary values)
        {
            try {
              

                msSQL = "select a.customer_name,b.customer_type from crm_mst_tcustomer a" +
                       " left join crm_mst_tcustomertype b on a.customer_type = b.customertype_gid" +
                       "  where a.customer_gid='" + customer_gid + "'  "; dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<GetCustomerlist>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new GetCustomerlist
                    {
                        customer_name = dt["customer_name"].ToString(),
                        customer_type = dt["customer_type"].ToString(),
                    });
                    values.GetCustomerList = getModuleList;
                }

            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer Type !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           


        }
        public void DaUpdateCustomerEdit(string employee_gid,GetCustomerlist values)
        {
            try
            {
                msSQL = " select a.customer_name from crm_mst_tcustomer a left join crm_mst_tcustomercontact " +
                         " b on a.customer_gid = b.customer_gid   " +
        " where a.customer_name = '" + values.customer_name.Replace("'", "\\\'") + "'" +
        " and a.customer_gid !='" + values.customer_gid + "' and b.email = '"+values.email+"'";
                string lsCustomerName = objdbconn.GetExecuteScalar(msSQL);
                if (lsCustomerName != "" && lsCustomerName != null)
                {
                    values.status = false;
                    values.message = "Customer Name Already Exist";
                    return;
                }
                else
                {

                    msSQL = " Select region_name from crm_mst_tregion where region_gid ='" + values.region_name + "'";
                    string lsregionname = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " Select user_gid from adm_mst_tuser where user_firstname ='" + values.sales_person + "'";
                    string lssalesperson = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " Select pricesegment_gid from smr_trn_tpricesegment where pricesegment_name ='" + values.pricesegment_name + "'";
                    string lspricesegment = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = " Select currencyexchange_gid from crm_trn_tcurrencyexchange where currency_code ='" + values.currency + "'";
                    string lscurrency = objdbconn.GetExecuteScalar(msSQL);


                    msSQL = "Update crm_mst_tcustomer set" +
                        " customer_name = '" + values.customer_name.Replace("'", "\\\'") + "'," +
                       " customer_pin = '" + values.postal_code + "'," +
                       " customer_city = '" + (String.IsNullOrEmpty(values.city) ? values.city : values.city.Replace("'", "\\\'")) + "'," +

                        " company_website = '" + values.company_website + "'," +
                        " customer_id = '" + values.customer_id + "'," +
                        " customer_address = '" + values.address1.Replace("'", "\\\'") + "',";
                    if (values.address2 != null)
                    {


                       msSQL+= " customer_address2 = '" + values.address2.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += " customer_address2 = '" + values.address2 + "',";
                    }
                   
                     msSQL+=   " customer_country = '" + values.country_name + "'," +
                        " customer_region = '" + values.region_name + "'," +
                        " customer_state = '" + (String.IsNullOrEmpty(values.customer_state) ? values.customer_state : values.customer_state.Replace("'", "\\\'")) + "'," +
                        " taxsegment_gid = '" + values.taxsegment_name + "'," +
                         " salesperson_gid = '" + lssalesperson + "'," +
                          " currency_gid = '" + lscurrency + "'," +
                           " pricesegment_gid = '" + lspricesegment + "',";
                    if(values.credit_days == null || values.credit_days == "")
                    {
                        msSQL += "'" + 0 + "'";

                    }
                    else
                    {
                        msSQL += " credit_days = '" + values.credit_days + "',";
                    }
                          
                      msSQL+=  " gst_number = '" + values.tax_no + "'" +
                        " where customer_gid = '" + values.customer_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult == 1)
                    {


                        msSQL = "Update crm_mst_tcustomercontact set" +
                               " customercontact_name = '" + values.customercontact_name.Replace("'", "\\\'") + "'," +
                               " address1='" + values.address1.Replace("'", "\\\'") + "',";
                              
                                if (values.address2 != null)
                                {


                                     msSQL += " address2 = '" + values.address2.Replace("'", "\\\'") + "',";
                                }
                               else
                               {
                            msSQL += " address2 = '" + values.address2 + "',";
                               }
                       msSQL+= " state='" + (String.IsNullOrEmpty(values.customer_state) ? values.customer_state : values.customer_state.Replace("'","\\\'"))  + "'," +
                               " city='" + (String.IsNullOrEmpty(values.city) ? values.city : values.city.Replace("'","\\\'")) + "'," +
                               " region='" + (String.IsNullOrEmpty(lsregionname) ? lsregionname : lsregionname.Replace("'","\\\'")) + "'," +
                               " zip_code='" + values.postal_code + "',";

                               if (values.mobiles != null)
                               {

                            msSQL += " mobile = '" + values.mobiles.e164Number + "',";
                                }
                               else
                               {
                            msSQL += " mobile = '',";
                        }
                        
                             msSQL+=  " email = '" + values.email + "'," +
                                " billing_email = '" + values.billemail + "'," +
                               " designation = '" + values.designation + "'," +
                               " fax_area_code = '" + values.area_code + "'," +
                               " fax_country_code = '" + values.country_code + "'," +
                               " fax = '" + values.fax_number + "'," +
                               " gst_number='" + values.tax_no + "'" +
                               " where customer_gid = '" + values.customer_gid + "' and main_contact = 'Y'";
                        mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult1 == 1)
                    {
                        msSQL = "update crm_trn_tleadbank set" +
                                " leadbank_name = '" + values.customer_name.Replace("'", "\\\'") + "'," +
                               
                                " customer_type = '" + values.customer_type + "'" +
                                " where customer_gid = '" + values.customer_gid + "'";
                        mnResult2 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }

                    msSQL = "select leadbank_gid from crm_trn_tleadbank " +
                        " where customer_gid = '" + values.customer_gid + "'";
                    string lsleadbankname = objdbconn.GetExecuteScalar(msSQL);


                    if (mnResult2 == 1)
                    {
                        msSQL = " update crm_trn_tleadbank set leadbank_country='" + values.countryname + "', " +
                                " leadbank_pin='" + values.postal_code + "', " +
                                " leadbank_address1='" + values.address1.Replace("'", "\\\'") + "', ";
                        if (values.address2 != null)
                        {


                            msSQL += " leadbank_address2 = '" + values.address2.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += " leadbank_address2 = '" + values.address2 + "',";
                        }
                       msSQL+= " leadbank_city='" + (String.IsNullOrEmpty(values.city) ? values.city : values.city.Replace("'", "\\\'")) + "', " +
                                " leadbank_state='" + (String.IsNullOrEmpty(values.customer_state) ? values.customer_state : values.customer_state.Replace("'", "\\\'")) + "', " +
                                " leadbank_region='" + values.region_name + "', " +
                                " company_website='" + values.company_website + "' " +
                                " where leadbank_gid='" + lsleadbankname + "' ";
                        mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult3 == 1)
                    {

                        msSQL = " update crm_trn_tleadbankcontact set" +
                                " leadbankcontact_name = '" + values.customercontact_name.Replace("'", "\\\'") + "'," +
                                " address1='" + values.address1.Replace("'", "\\\'") + "',";
                                

                                 if (values.address2 != null)
                        {


                            msSQL += " address2 = '" + values.address2.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += " address2 = '" + values.address2 + "',";
                        }
                        msSQL += " state='" + (String.IsNullOrEmpty(values.customer_state) ? values.customer_state : values.customer_state.Replace("'", "\\\'")) + "', " +
                                 " city='" + (String.IsNullOrEmpty(values.city) ? values.city : values.city.Replace("'", "\\\'")) + "', " +
                                 " pincode='" + values.postal_code + "', " +
                                 " region_name='" + values.region_name + "', " +
                                 " email = '" + values.email + "',";
                        if(values.mobiles != null)
                        {
                            msSQL += " mobile = '" + values.mobiles.e164Number + "',";
                        }
                        else
                        {
                          msSQL+=  " mobile = '',";
                        }
                             msSQL +=
                                " designation = '" + values.designation + "'," +
                                " area_code1 = '" + values.area_code + "'," +
                                " country_code1 = '" + values.country_code + "'," +
                                " fax = '" + values.fax_number + "'," +
                                " fax_area_code = '" + values.area_code + "'," +
                                " fax_country_code = '" + values.country_code + "'" +
                                " where leadbank_gid='" + lsleadbankname + "'";
                        mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult == 1 && mnResult1 == 1 && mnResult2 == 1 && mnResult3 == 1)
                    {

                        values.status = true;
                        values.message = " Customer Details Updated Successfully";
                        return;
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error while updating";
                        return;
                    }
                }
                
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updating Customer  !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            

        }
        //customer Segment Add and edit
        public void DaCustomerprice(string user_gid, Getproductlist values)
        {
            try {
               
                msSQL = "select * from smr_trn_tpricesegment2customer where customer_gid ='" + values.customer_gid + "'";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count == 0)
            {
                msSQL = "  select customer_name from crm_mst_tcustomer where customer_gid='" + values.customer_gid + "'";
                       lscustomer_name = objdbconn.GetExecuteScalar(msSQL);

                msSQL = "select * from smr_trn_tpricesegment where pricesegment_name ='" + lscustomer_name + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count == 0)
                {
                    string msGetGID = objcmnfunctions.GetMasterGID("SPRS");
                    msSQL = " insert into smr_trn_tpricesegment ( " +
                            " pricesegment_gid," +
                            " pricesegment_code, " +
                            " pricesegment_name " +
                            " ) values( " +
                            " '" + msGetGID + "', " +
                            " '" + values.customer_gid + "'," +
                            " '" + lscustomer_name + "' )";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                }
                msSQL = "select pricesegment_gid,pricesegment_name from smr_trn_tpricesegment where pricesegment_name ='" + lscustomer_name + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        lspricesegment_gid = dt["pricesegment_gid"].ToString();
                        lspricesegment_name = dt["pricesegment_name"].ToString();
                    }
                }
                string msGetGID1 = objcmnfunctions.GetMasterGID("VPDC");
                msSQL = " insert into smr_trn_tpricesegment2customer(" +
                        " pricesegment2customer_gid, " +
                        " pricesegment_gid, " +
                        " pricesegment_name," +
                        " customer_gid, " +
                        " customer_name, " +
                        " created_by, " +
                        " created_date" +
                        " )values( " +
                        "'" + msGetGID1 + "', " +
                        "'" + lspricesegment_gid + "'," +
                        "'" + lscustomer_name + "'," +
                        "'" + values.customer_gid + "', " +
                        "'" + lscustomer_name + "', " +
                        "'" + user_gid + "', " +
                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                foreach (var data in values.salesproduct_list)
                {
                    string lsproductgroup_code = data.productgroup_name.Split('|').First().Trim();
                    string lsproductgroup_name = data.productgroup_name.Split('|').Last().Trim();
                    string mspricesegment2productgetgid = objcmnfunctions.GetMasterGID("SRCT");
                    msSQL = " insert into smr_trn_tpricesegment2product( " +
                            " pricesegment2product_gid, " +
                            " product_code, " +
                            " product_name," +
                            " product_gid," +
                            " productuom_gid," +
                            " productuom_name," +
                            " productgroup_code," +
                            " productgroup , " +
                            " pricesegment_gid , " +
                            " pricesegment_name , " +
                            " product_price, " +
                            " cost_price, " +
                            " customerproduct_code, " +
                            " created_by," +
                            " created_date)" +
                            " values(" +
                            " '" + mspricesegment2productgetgid + "', " +
                            " '" + data.product_code + "'," +
                            " '" + data.product_name + "', " +
                            " '" + data.product_gid + "', " +
                            " '" + data.productuom_gid + "'," +
                            " '" + data.productuom_name + "'," +
                            " '" + lsproductgroup_code + "'," +
                            " '" + lsproductgroup_name + "'," +
                            " '" + lspricesegment_gid + "'," +
                            " '" + lspricesegment_name + "'," +
                            " '" + data.selling_price + "'," +
                            " '" + data.cost_price + "'," +
                            " '" + lsproductgroup_code + "', " +
                            " '" + user_gid + "'," +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    if (mnResult != 0)
                    {
                        string msSTOCKGetGID = objcmnfunctions.GetMasterGID("ISKP");
                        msSQL = " insert into ims_trn_tstock " +
                                " (stock_gid, " +
                                " branch_gid, " +
                                " product_gid, " +
                                " uom_gid, " +
                                " stock_qty, " +
                                " grn_qty, " +
                                " unit_price, " +
                                " remarks, " +
                                " stocktype_gid, " +
                                " reference_gid, " +
                                " stock_flag, " +
                                " created_by, " +
                                " created_date)" +
                                " values( " +
                                " '" + msSTOCKGetGID + "'," +
                                " '" + data.branch_gid + "'," +
                                " '" + data.product_gid + "', " +
                                " '" + data.productuom_gid + "'," +
                                " '0'," +
                                " '0'," +
                                " '" + data.selling_price + "'," +
                                " 'From Sales'," +
                                " 'SY0905270003'," +
                                " '" + mspricesegment2productgetgid + "'," +
                                " 'Y'," +
                                " '" + user_gid + "'," +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        msSQL = " INSERT INTO smr_trn_tstockpricehistory( " +
                                " product_gid, " +
                                " pricesegment_gid, " +
                                " customerproduct_code," +
                                " old_price, " +
                                " stock_gid, " +
                                " updated_price, " +
                                " updated_by ," +
                                " updated_date " +
                                " ) VALUES ( " +
                                " '" + data.product_gid + "', " +
                                " '" + lspricesegment_gid + "', " +
                                " '" + lsproductgroup_code + "'," +
                                " '" + data.product_price + "', " +
                                " '" + msSTOCKGetGID + "', " +
                                " '" + data.selling_price + "', " +
                                " '" + user_gid + "', " +
                                " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ) ";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = " Price Segment Added Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occured While Adding Price Segment";
                    }

                }
            }
            else
            {             
                    foreach (var data in values.salesproduct_list)
                    {
                        msSQL = "  select customer_name from crm_mst_tcustomer where customer_gid='" + values.customer_gid + "'";
                        lscustomer_name = objdbconn.GetExecuteScalar(msSQL);
                        string lsproductgroup_code = data.productgroup_name.Split('|').First().Trim();
                        msSQL = "select pricesegment_gid from smr_trn_tpricesegment where pricesegment_name ='" + lscustomer_name + "'";
                        lspricesegment_gid = objdbconn.GetExecuteScalar(msSQL);
                        msSQL = " update smr_trn_tpricesegment2product set" +
                                " product_price = '" + data.selling_price + "'," +
                                " cost_price='" + data.cost_price + "'" +
                                " where pricesegment_gid='" + lspricesegment_gid + "'" +
                                " and pricesegment2product_gid='" + data.pricesegment2product_gid +"'";
                        mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (mnResult3 != 0)
                        {
                            msSQL = " INSERT INTO smr_trn_tstockpricehistory( " +
                                    " product_gid, " +
                                    " pricesegment_gid, " +
                                    " customerproduct_code," +
                                    " old_price, " +
                                    " stock_gid, " +
                                    " updated_price, " +
                                    " updated_by ," +
                                    " updated_date " +
                                    " ) VALUES ( " +
                                    " '" + data.product_gid + "', " +
                                    " '" + lspricesegment_gid + "', " +
                                    " '" + lsproductgroup_code + "'," +
                                    " '" + data.product_price + "', " +
                                    " '" + data.stock_gid + "', " +
                                    " '" + data.selling_price + "', " +
                                    " '" + user_gid + "', " +
                                    " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ) ";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                values.status = true;
                                values.message = " Pricesegment Updated Successfully";
                            }
                            else
                            {
                                values.status = false;
                                values.message = "Error Occured While Updated Pricesegment";
                            }

                        }
                    }
               

            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured whileUpdated Pricesegment !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }

        public void DaCustomerpriceupdate(string user_gid, Getproductlist values)
        {
            try {
               
                foreach (var data in values.salesproduct_list)
            {
                msSQL = "  select customer_name from crm_mst_tcustomer where customer_gid='" + values.customer_gid + "'";
                lscustomer_name = objdbconn.GetExecuteScalar(msSQL);
                string lsproductgroup_code = data.productgroup_name.Split('|').First().Trim();
                msSQL = "select pricesegment_gid from smr_trn_tpricesegment where pricesegment_name ='" + lscustomer_name + "'";
                lspricesegment_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " update smr_trn_tpricesegment2product set" +
                        " product_price = '" + data.selling_price + "'," +
                        " cost_price='" + data.cost_price + "'" +
                        " where pricesegment_gid='" + lspricesegment_gid + "'" +
                        " and pricesegment2product_gid='" + data.pricesegment2product_gid + "'";
                mnResult3 = objdbconn.ExecuteNonQuerySQL(msSQL);

                if (mnResult3 != 0)
                {
                    msSQL = " INSERT INTO smr_trn_tstockpricehistory( " +
                            " product_gid, " +
                            " pricesegment_gid, " +
                            " customerproduct_code," +
                            " old_price, " +
                            " stock_gid, " +
                            " updated_price, " +
                            " updated_by ," +
                            " updated_date " +
                            " ) VALUES ( " +
                            " '" + data.product_gid + "', " +
                            " '" + lspricesegment_gid + "', " +
                            " '" + lsproductgroup_code + "'," +
                            " '" + data.product_price + "', " +
                            " '" + data.stock_gid + "', " +
                            " '" + data.selling_price + "', " +
                            " '" + user_gid + "', " +
                            " '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ) ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = " Pricesegment Updated Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occured While Updated Pricesegment";
                    }

                }
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Updated Pricesegment !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           

        }


        public void DaCustomerImport(HttpRequest httpRequest, string user_gid, result objResult, postcustomer_list values)
        {
            try {
              
                string lscompany_code;
          

            try
            {

                HttpFileCollection httpFileCollection;
                string lspath, lsfilePath;

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                // Create Directory
                lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"] + "erp_documents/" + lscompany_code + "/Sales_module/Import_Excel/Customer_Details/"
                        + DateTime.Now.Year +"/" + DateTime.Now.Month + "/";

                if (!Directory.Exists(lsfilePath))
                    Directory.CreateDirectory(lsfilePath);

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
                FileInfo fileinfo = new FileInfo(lsfilePath);
                Stream ls_readStream;
                ls_readStream = httpPostedFile.InputStream;
                MemoryStream ms = new MemoryStream();
                ls_readStream.CopyTo(ms);

                //path creation        
                lspath = lsfilePath + "/";
                FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                try
                {
                    //ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    //using (ExcelPackage xlPackage = new ExcelPackage(ms))
                    //{
                    //    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Customer"];
                    //    rowCount = worksheet.Dimension.End.Row;
                    //    columnCount = worksheet.Dimension.End.Column;
                    //    endRange = worksheet.Dimension.End.Address;
                    //}
                    string status;
                    status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                    file.Close();
                    ms.Close();

                    objcmnfunctions.uploadFile(lspath, lsfile_gid);
                }
                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = ex.Message.ToString();
                    return;
                }

                //Excel To DataTable
                try
                {
                    DataTable dataTable = new DataTable();
                    int totalSheet = 1;
                    string connectionString = string.Empty;
                    string fileExtension = Path.GetExtension(lspath);
                    lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";

                    string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");
                    try
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + correctedPath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0';";
                    }
                    catch (Exception ex)
                    {
                            values.status = false;
                            values.message = ex.Message;
                            return;
                        }

                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        if (schemaTable != null)
                        {
                            var tempDataTable = (from dataRow in schemaTable.AsEnumerable()
                                                 where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                                 select dataRow).CopyToDataTable();

                            schemaTable = tempDataTable;
                            totalSheet = schemaTable.Rows.Count;
                            using (OleDbCommand command = new OleDbCommand())
                            {
                                    command.Connection = connection;
                                    command.CommandText = "select * from [Sheet1$]";

                                    using (OleDbDataReader reader = command.ExecuteReader())
                                {
                                    dataTable.Load(reader);
                                }

                                foreach (DataRow row in dataTable.Rows)
                                {

                                    string excustomercode = row["Customer Code"].ToString();
                                    string excustomername = row["Customer Name"].ToString();
                                    string excustomertype = row["Customer Type"].ToString();
                                    string exccontactpersonname = row["Contact Person"].ToString();
                                    string excustomermobile = row["Moblie Number"].ToString();
                                    string exaddress1 = row["Address 1"].ToString();
                                    string exaddress2 = row["Address 2"].ToString();
                                    string excity = row["City"].ToString();
                                    string expostalcode = row["Postal Code"].ToString();
                                    string excountry = row["Country"].ToString();
                                    string excurrency = row["Currency "].ToString();
                                    string exemail = row["Email"].ToString();
                                    string exstate = row["State"].ToString();
                                    string exgstnumber = row["GST Number"].ToString();
                                    string exfaxnumber = row["Fax Number"].ToString();
                                    string exdesignation = row["Desgination"].ToString();
                                    string excompanywebsite = row["Company Website"].ToString();
                                    string exregion = row["Region"].ToString();
                                    string taxsegment = row["Tax Segment"].ToString();
                                        string lsstatus = "Active";

                                        if (excustomername != null && excustomername != "" && exaddress1 != null && exaddress1 != "" && excountry != null && excountry != ""
                                            && excustomermobile != null && excustomermobile != "" && exemail != null && exemail != "" && exregion != null && exregion != ""
                                            && exccontactpersonname != null && exccontactpersonname != "" && taxsegment != null && taxsegment != "")
                                        {
                                            if (excustomercode == "")
                                            {
                                                msGetGid = objcmnfunctions.GetMasterGID("CC");
                                                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='CC' order by finyear asc limit 0,1 ";
                                                string exCode = objdbconn.GetExecuteScalar(msSQL);
                                                exclcustomer_code = "CC-" + "00" + exCode;

                                            }
                                            else
                                            {
                                                exclcustomer_code = excustomercode;

                                            }

                                            string exclcustomer_branch = "H.Q";


                                            msSQL = "select currencyexchange_gid from crm_trn_tcurrencyexchange where currency_code = '" + excurrency + "'";
                                            string lscurrencyexchange_gid = objdbconn.GetExecuteScalar(msSQL);
                                            if (lscurrencyexchange_gid == "")
                                            {
                                                lscurrencyexchangegid = "";

                                            }
                                            else
                                            {
                                                lscurrencyexchangegid = lscurrencyexchange_gid;
                                            }
                                            msSQL = "select country_gid from adm_mst_tcountry where country_name = '" + excountry + "'";
                                            string lscountry_gid = objdbconn.GetExecuteScalar(msSQL);
                                            if (lscountry_gid == "")
                                            {
                                                lscountrygid = "";
                                            }
                                            else
                                            {

                                                lscountrygid = lscountry_gid;
                                            }
                                            msSQL1 = "select region_gid from crm_mst_tregion where region_name = '" + exregion + "'";
                                            string lsregion_gid = objdbconn.GetExecuteScalar(msSQL1);
                                            if (lsregion_gid == "")
                                            {
                                                values.message = "";
                                            }
                                            else
                                            {
                                                lsregiongid = lsregion_gid;
                                            }
                                            msSQL = "select customertype_gid from crm_mst_tcustomertype where customer_type = '" + excustomertype + "'";
                                            string lscustomertype_gid = objdbconn.GetExecuteScalar(msSQL);
                                            if (lscustomertype_gid == "")
                                            {
                                                values.message = "";
                                            }
                                            else
                                            {

                                                lscustomertypegid = lscustomertype_gid;
                                            }
                                            msSQL = "select taxsegment_gid from acp_mst_ttaxsegment where taxsegment_name = '" + taxsegment + "'";
                                            string taxsegmentgid = objdbconn.GetExecuteScalar(msSQL);
                                           
                                            {

                                                msGetGid = objcmnfunctions.GetMasterGID("BCRM");
                                                msGetGid1 = objcmnfunctions.GetMasterGID("BCCM");
                                                msGetGid2 = objcmnfunctions.GetMasterGID("BLBP");
                                                msGetGid3 = objcmnfunctions.GetMasterGID("BLCC");
                                                

                                                msSQL = " insert into crm_mst_tcustomer (" +
                                                   " customer_gid," +
                                                   " customer_id, " +
                                                   " customer_name, " +
                                                   " company_website, " +
                                                   " status, " +
                                                   " customer_address, " +
                                                   " customer_address2," +
                                                   " customer_city," +
                                                   " currency_gid," +
                                                   " customer_country," +
                                                   " customer_region," +
                                                   " customer_state," +
                                                   " gst_number ," +
                                                   " customer_pin ," +
                                                   " customer_type ," +
                                                   " taxsegment_gid ," +
                                                  " created_by," +
                                                   "created_date" +
                                                    ") values (" +
                                                   "'" + msGetGid + "', " +
                                                   "'" + exclcustomer_code + "'," +
                                                   "'" + excustomername + "'," +
                                                   "'" + excompanywebsite + "'," +
                                                   "'" + lsstatus + "'," +
                                                   "'" + exaddress1 + "'," +
                                                   "'" + exaddress2 + "'," +
                                                   "'" + excity + "'," +
                                                   "'" + lscurrencyexchangegid + "'," +
                                                   "'" + lscountrygid + "'," +
                                                   "'" + lsregiongid + "'," +
                                                   "'" + exstate + "'," +
                                                   "'" + exgstnumber + "'," +
                                                   "'" + expostalcode + "'," +
                                                    "'" + lscustomertypegid + "'," +
                                                    "'" + taxsegmentgid + "'," +
                                                    "'" + user_gid + "'," +
                                                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                                if (mnResult != 0)
                                                {
                                                    //objfinance.finance_vendor_debitor("Sales", exclcustomer_code, values.customer_name, msGetGid, user_gid);
                                                    //string trace_comment = " Added a customer on " + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                    //objcmnfunctions.Tracelog(msGetGid, user_gid, trace_comment, "added_customer");

                                                    msSQL = " insert into crm_mst_tcustomercontact  (" +
                                                    " customercontact_gid," +
                                                    " customer_gid," +
                                                    " customercontact_name, " +
                                                    " customerbranch_name, " +
                                                    " email, " +
                                                    " mobile, " +
                                                     " main_contact, " +
                                                    " designation," +
                                                    " address1," +
                                                    " address2," +
                                                    " state," +
                                                    " city," +
                                                    " country_gid," +
                                                    " region," +
                                                    " fax, " +
                                                    " zip_code, " +
                                                  
                                                    " fax_country_code," +
                                                    " gst_number, " +
                                                     " created_by," +
                                                   "created_date" +

                                                    ") values (" +
                                                    "'" + msGetGid1 + "', " +
                                                    "'" + msGetGid + "', " +
                                                    "'" + exccontactpersonname + "'," +
                                                    "'" + exclcustomer_branch + "'," +
                                                    "'" + exemail + "'," +
                                                    "'" + excustomermobile + "'," +
                                                    "'Y'," +
                                                    "'" + exdesignation + "'," +
                                                    "'" + exaddress1 + "'," +
                                                    "'" + exaddress2 + "'," +
                                                    "'" + exstate + "'," +
                                                    "'" + excity + "'," +
                                                     "'" + lscountrygid + "'," +
                                                    "'" + lsregiongid + "'," +
                                                     "'" + exfaxnumber + "'," +
                                                     "'" + expostalcode + "'," +
                                                   
                                                     "'" + values.fax_country_code + "'," +
                                                   "'" + values.gst_number + "'," +
                                                   "'" + user_gid + "'," +
                                                     "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                }

                                                if (mnResult != 0)
                                                {

                                                    msSQL = " INSERT INTO crm_trn_tleadbank " +
                                                           " (leadbank_gid, " +
                                                           " customer_gid, " +
                                                           " leadbank_name," +
                                                           " leadbank_address1, " +
                                                           " leadbank_address2, " +
                                                           " leadbank_city, " +
                                                           " leadbank_code, " +
                                                           " leadbank_state, " +
                                                           " leadbank_pin, " +
                                                           " leadbank_country, " +
                                                           " leadbank_region, " +
                                                           " approval_flag, " +
                                                           " leadbank_id, " +
                                                           " status, " +
                                                           " main_branch," +
                                                           " main_contact," +
                                                           " customer_type," +
                                                           " customertype_gid," +
                                                           " created_by, " +
                                                           " created_date)" +
                                                           " values ( " +
                                                           "'" + msGetGid2 + "'," +
                                                           "'" + msGetGid + "'," +
                                                           "'" + excustomername + "'," +
                                                           "'" + exaddress1 + "'," +
                                                           "'" + exaddress2 + "'," +
                                                           "'" + excity + "'," +
                                                           "'" + lscustomercode + "'," +
                                                           "'" + exstate + "'," +
                                                           "'" + expostalcode + "'," +
                                                           "'" + lscountrygid + "'," +
                                                           "'" + lsregiongid + "'," +
                                                           "'Approved'," +
                                                           "'" + exclcustomer_code + "'," +
                                                           "'Y'," +
                                                           "'Y'," +
                                                           "'Y'," +
                                                            "'" + excustomertype + "'," +
                                                            "'" + lscustomertypegid + "'," +
                                                           "'" + user_gid + "'," +
                                                           "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";

                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                }

                                                if (mnResult != 0)
                                                {
                                                    msSQL = " INSERT into crm_trn_tleadbankcontact (" +
                                                        " leadbankcontact_gid, " +
                                                        " leadbank_gid," +
                                                        " leadbankbranch_name, " +
                                                        " leadbankcontact_name," +
                                                        " email," +
                                                        " mobile," +
                                                        " designation," +
                                                        " did_number," +
                                                        " created_date," +
                                                        " created_by," +
                                                        " address1," +
                                                        " address2, " +
                                                        " state, " +
                                                        " country_gid, " +
                                                        " city, " +
                                                        " pincode, " +
                                                        " region_name, " +
                                                        " main_contact," +
                                                        " phone1," +
                                                      
                                                        " country_code1," +
                                                        " fax," +
                                                     
                                                        " fax_country_code)" +
                                                        " values (" +
                                                        " '" + msGetGid3 + "'," +
                                                        " '" + msGetGid2 + "'," +
                                                        "'" + lscustomercode + "'," +
                                                        "'" + exccontactpersonname + "'," +
                                                        "'" + exemail + "'," +
                                                        "'" + excustomermobile + "'," +
                                                        "'" + exdesignation + "'," +
                                                        "'0'," +
                                                        "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                                                        "'" + user_gid + "'," +
                                                        "'" + exaddress1 + "'," +
                                                        "'" + exaddress2 + "'," +
                                                        "'" + exstate + "'," +
                                                        "'" + lscountrygid + "'," +
                                                        "'" + excity + "'," +
                                                        "'" + expostalcode + "'," +
                                                        "'" + lsregiongid + "'," +
                                                        "'Y'," +
                                                        "'" + values.phone1 + "'," +
                                                    
                                                        "'" + values.country_code + "'," +
                                                        "'" + values.fax + "'," +
                                                        
                                                        "'" + values.fax_country_code + "')";

                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                }
                                                if(mnResult != 0)
                                                {
                                                    msGetGid4 = objcmnfunctions.GetMasterGID("TS2C");

                                                    msSQL = " INSERT into acp_mst_ttaxsegment2customer (" +
                                                       " taxsegment2customer_gid, " +
                                                       " taxsegment_gid," +
                                                       " taxsegment_name, " +
                                                       " customer_gid," +
                                                       " created_by," +
                                                       " created_date)" +                                                       
                                                       " values (" +
                                                       " '" + msGetGid4 + "'," +
                                                       " '" + taxsegmentgid + "'," +
                                                       "'" + taxsegment + "'," +
                                                       "'" + msGetGid + "'," +
                                                       "'" + user_gid + "'," +
                                                       "'" + DateTime.Now.ToString("yyyy-MM-dd") +"')";
                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            values.status = false;
                                            values.message = " Kindly Provide All Mandatory Fields!";

                                        }



                                }
                                    if (mnResult != 0)
                                    {
                                        values.status = true;
                                        values.message = "Customer Added Successfully";

                                    }
                                    else
                                    {
                                        values.status = false;
                                        values.message = "Error While Adding Customer";

                                    }
                            }
                        }
                    }
                }


                catch (Exception ex)
                {
                    objResult.status = false;
                    objResult.message = ex.Message.ToString();
                    return;
                }
            }
            catch (Exception ex)
            {
                objResult.status = false;
                objResult.message = ex.Message.ToString();
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Customer!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }           

        }

        //For Customer Branch Add
        public void DaPostCustomerbranch(customerbranch_list values)
        {
            try {

                msSQL = "select customerbranch_name from crm_mst_tcustomercontact where customer_gid = '"+ values.customer_gid +"'";
                string customerbranch = objdbconn.GetExecuteScalar(msSQL);

                if (customerbranch != values.customerbranch_name)
                {
                
                    msGetGid1 = objcmnfunctions.GetMasterGID("BCCM");
                    msSQL = " insert into crm_mst_tcustomercontact( " +
                            " customercontact_gid, " +
                            " customer_gid, " +
                            " customerbranch_name, " +
                            " address1, " +
                            " address2, " +
                            " city, " +
                            " state, " +
                            " region, " +
                          
                            " zip_code, " +
                            " country_gid, " +
                          
                            " email " +
                            ") VALUES (" +

                               "'" + msGetGid1 + "'," +
                             "'" + values.customer_gid + "'," +
                             "'" + values.customerbranch_name + "'," +
                             "'" + values.address1.Replace("'", "\\\'") + "',";
                    if(values.address2 != null)
                    {
                        msSQL += "'" + values.address2.Replace("'", "\\\'") + "',";
                    }
                    else
                    {
                        msSQL += "'" + values.address2 + "',";
                    }
                            
                            msSQL += "'" + values.customer_city + "'," +
                             "'" + values.customer_state + "'," +
                             "'" + values.region_name + "'," +
                          
                             "'" + values.customer_pin + "'," +
                             "'" + values.country_name + "'," +
                           
                             "'" + values.email + "')";

                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = " Customer Branch Updated Successfully";
                        return;
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error Occured While Updated Customer Branch";
                        return;
                    }
                }

                else
                {
                    values.status = false;
                    values.message = "Customer Branch Already Exists";
                    return;
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while  While Updated Customer Branch !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }
        public void DaGetSmrTrnCustomerBranch(string customer_gid, MdlSmrTrnCustomerSummary values)
        {
            try {
               
                msSQL = " SELECT distinct a.customercontact_gid as customer_gid, a.customerbranch_name,a.address1, " +
                  " a.city as customer_city, a.state as customer_state, a.zip_code as customer_pin,b.country_name,a.customercontact_name,a.mobile,a.designation " +
                  " from crm_mst_tcustomercontact a left join adm_mst_tcountry b on a.country_gid = b.country_gid " +
                  " where customer_gid = '" + customer_gid + "' ";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<smrcustomerbranch_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new smrcustomerbranch_list
                    {
                        customer_gid = dt["customer_gid"].ToString(),
                        customerbranch_name = dt["customerbranch_name"].ToString(),
                        customer_city = dt["customer_city"].ToString(),
                        customer_state = dt["customer_state"].ToString(),
                        customer_address = dt["address1"].ToString(),
                        customer_pin = dt["customer_pin"].ToString(),
                        country_name = dt["country_name"].ToString(),
                        customercontact_name = dt["customercontact_name"].ToString(),
                        designation = dt["designation"].ToString(),
                        mobile = dt["mobile"].ToString(),
                    });

                    values.smrcustomerbranch_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured whileLoading Customer Branch !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }

        public void DaGetCustomerReportExport(MdlSmrTrnCustomerSummary values)
        {
            try {
                
                msSQL = " Select distinct UCASE(a.customer_id) as CustomerId,a.customer_type as CustomerType,a.customer_name as CustomerName,a.customer_state as State,concat(c.customercontact_name,' / ',c.mobile,' / ',c.email) as ContactDetails, " +
                    " case when d.region_name is null then concat(a.customer_city,' / ',a.customer_state)" +
                    " when d.region_name is not null " +
                    " then Concat(d.region_name,' / ',a.customer_city,' / ',a.customer_state) end as RegionName , a.customer_type as CustomerType, " +
                    " (SELECT DATE_FORMAT(MAX(salesorder_date), '%d %b %Y') FROM smr_trn_tsalesorder WHERE customer_gid = a.customer_gid) AS LastOrderDate," +
                    " (SELECT DATE_FORMAT(Min(salesorder_date), '%d %b %Y') FROM smr_trn_tsalesorder WHERE customer_gid = a.customer_gid) AS FirstOrderDate, " +
                    " CONCAT((SELECT DATE_FORMAT(MIN(salesorder_date), '%d %b %Y') FROM smr_trn_tsalesorder WHERE customer_gid = a.customer_gid),'  (', " +
                    " (SELECT CONCAT(DATEDIFF(CURDATE(), MIN(salesorder_date)), ' days', ')') FROM smr_trn_tsalesorder WHERE customer_gid = a.customer_gid)) as CustomerSince " +
                    " from crm_mst_tcustomer a" +
                    " left join crm_mst_tregion d on a.customer_region =d.region_gid " +
                    " left join crm_mst_tcustomercontact c on a.customer_gid=c.customer_gid " +
                    " order by a.customer_id asc ";
            dt_datatable = objdbconn.GetDataTable(msSQL);

            string lscompany_code = string.Empty;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Customer Report");
            try
            {
                msSQL = " select company_code from adm_mst_tcompany";

                lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                string lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents/customer/export" + "/" + lscompany_code + "/" + "Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                
                    if ((!System.IO.Directory.Exists(lspath)))
                        System.IO.Directory.CreateDirectory(lspath);
                
                    string lsname2 = "Customer_Report" + DateTime.Now.ToString("(dd-MM-yyyy HH-mm-ss)") + ".xlsx";
                    string ls_path2 = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents/customer/export" + "/" + lscompany_code + "/" + "Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + lsname2;

                    workSheet.Cells[1, 1].LoadFromDataTable(dt_datatable, true);
                    FileInfo file = new FileInfo(ls_path2);
                    using (var range = workSheet.Cells[1, 1, 1, 10])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        range.Style.Font.Color.SetColor(Color.White);
                    }
                    excel.SaveAs(file);

                    var getModuleList = new List<customerexport_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {
                        getModuleList.Add(new customerexport_list
                        {
                            lsname2 = lsname2,
                            lspath1 = ls_path2,
                        });
                        values.customerexport_list = getModuleList;
                    }
                    dt_datatable.Dispose();
                    values.status = true;
                    values.message = "Success";
                }
            catch (Exception ex)
            {
                values.status = false;
                values.message = "Failure";
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Export Customer !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }



        public void DaGetbranch( string customer_gid,MdlSmrTrnCustomerSummary values)
        {
            try {
              
                msSQL = " Select customercontact_gid,customer_gid,customerbranch_name " +
                " from crm_mst_tcustomercontact" +
            " where customer_gid = '" + customer_gid + "' ";

            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<branch_list>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new branch_list
                    {
                        customerbranch_name = dt["customerbranch_name"].ToString(),
                        customer_gid = dt["customer_gid"].ToString(),
                        customercontact_gid = dt["customercontact_gid"].ToString(),
                    });
                    values.branch_list = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer Branch Name !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           
        }


        public void DaPostCustomercontact(string user_gid,customercontact_list values)
        {
            try {
               
                msGetGid1 = objcmnfunctions.GetMasterGID("BCCM");
                msSQL = " INSERT INTO crm_mst_tcustomercontact" +
                    " (customercontact_gid," +
                    " customer_gid," +
                    " customerbranch_name," +
                    " customercontact_name," +
                    " email," +
                    " mobile," +
                    " designation," +
                    " address1," +
                    " address2," +
                    " city," +
                    " state," +
                    " country_gid," +
                    " zip_code," +
                    " created_by," +
                    " created_date " +
                     ") VALUES (" +

                           "'" + msGetGid1 + "'," +
                         "'" + values.customer_gid + "'," +
                         "'" + values.customerbranch_name + "'," +
                         "'" + values.customercontact_name + "'," +
                         "'" + values.email + "'," +
                         "'" + values.mobiles.e164Number + "'," +
                         "'" + values.designation + "'," +
                         "'" + values.address1.Replace("'", "\\\'") + "',";
                if(values.address2 != null)
                {
                    msSQL += "'" + values.address2.Replace("'", "\\\'") + "',";
                }
                else
                {
                    msSQL += "'" + values.address2 + "',";
                }
                    
                   msSQL +=  "'" + values.city + "'," +
                     "'" + values.state + "'," +
                     "'" + values.country_name + "'," +
                     "'" + values.zip_code + "'," +
                      "'" + user_gid + "'," +
                      "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";


            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
            if (mnResult != 0)
            {
                values.status = true;
                values.message = " Customer Contact Added Successfully";
            }
            else
            {
                values.status = false;
                values.message = "Error Occured While Adding Customer Contact";
            }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Adding Customer Contact !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
           

        }
        public void DaGetCustomerTypeSummary(MdlSmrTrnCustomerSummary values)
        {
            msSQL = "select customertype_gid,customer_type, display_name from crm_mst_tcustomertype  ";
            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getmodulelist = new List<customertype_list1>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getmodulelist.Add(new customertype_list1
                    {
                        customertype_gid1 = dt["customertype_gid"].ToString(),
                        customer_type1 = dt["customer_type"].ToString(),
                        display_name = dt["display_name"].ToString(),
                    });
                    values.customertype_list1 = getmodulelist;
                }
            }
            dt_datatable.Dispose();
        }
        public void DaGetSmrTrnCustomerContact(string customer_gid, MdlSmrTrnCustomerSummary values)
        {
            try {
              
                msSQL = "  select a.customercontact_gid,a.customer_gid,a.customerbranch_name," +
                " a.customercontact_name, a.email, a.mobile, a.designation, a.did_number, " +
                " a.address1,a.city,a.state,a.country_gid," +
                "c.country_name,a.zip_code,b.customer_code " +
                " from crm_mst_tcustomercontact a" +
                " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                " left join adm_mst_tcountry c on a.country_gid = c.country_gid" +
                " where a.customer_gid = '" + customer_gid + "'" +
                " union " +
                " select a.customercontact_gid,a.customer_gid,a.customerbranch_name," +
                " a.customercontact_name, a.email, a.mobile, a.designation, " +
                " a.did_number,concat(a.address1,'|',a.address2)as address1,a.city,a.state," +
                " a.country_gid,c.country_name,a.zip_code, b.customer_code " +
                " from crm_mst_tcustomercontact a" +
                " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                " left join adm_mst_tcountry c on a.country_gid = c.country_gid" +
                " where b.customergroup_gid = '" + customer_gid + "'" +
                " group by customercontact_gid order by customercontact_name asc";


            dt_datatable = objdbconn.GetDataTable(msSQL);
            var getModuleList = new List<customercontact_list1>();
            if (dt_datatable.Rows.Count != 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    getModuleList.Add(new customercontact_list1
                    {
                        customer_gid = dt["customer_gid"].ToString(),
                        customerbranch_name = dt["customerbranch_name"].ToString(),
                        city = dt["city"].ToString(),
                        state = dt["state"].ToString(),
                        customercontact_name = dt["customercontact_name"].ToString(),
                        designation = dt["designation"].ToString(),
                        zip_code = dt["zip_code"].ToString(),
                        country_name = dt["country_name"].ToString(),
                        mobiles = dt["mobile"].ToString(),
                        address1 = dt["address1"].ToString(),
                        address2 = dt["address1"].ToString(),
                    });

                    values.customercontact_list1 = getModuleList;
                }
            }
            dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Getting Customer Contact!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }
            
        }


        public void DaGetLastFiveOrderSummary(string customer_gid,MdlSmrTrnCustomerSummary values)
        {
            try
            {

                msSQL = " select a.salesorder_gid,date_format(a.salesorder_date, '%d-%b-%Y') as salesorder_date," +
                        " CONCAT(g.user_code, ' / ', g.user_firstname, ' ', g.user_lastname) AS salesperson_name," +
                       " b.customer_gid, a.so_referenceno1, a.so_type,format(a.Grandtotal, 2) as Grandtotal from smr_trn_tsalesorder a " +
                       " LEFT JOIN adm_mst_tuser g ON g.user_gid = a.salesperson_gid " +
                        " left join crm_mst_tcustomer b on a.customer_gid = b.customer_gid " +
                        " where a.customer_gid = '"+ customer_gid +"' order by a.salesorder_gid desc Limit 5 ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<fiveorder_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new fiveorder_list
                        {
                            salesorder_gid = dt["salesorder_gid"].ToString(),
                            salesorder_date = dt["salesorder_date"].ToString(),
                            customer_gid = dt["customer_gid"].ToString(),
                            Grandtotal = dt["Grandtotal"].ToString(),
                            so_type = dt["so_type"].ToString(),
                            so_referenceno1 = dt["so_referenceno1"].ToString(),
                            salesperson = dt["salesperson_name"].ToString(),
                        });

                        values.fiveorder_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while Loading Customer Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

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
                }
                objOdbcDataReader.Close();
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name}" + " * **********" + ex.ToString() + "***********" + "Error While Fetching Mail configuration Summary " + "*****Query****" + msSQL + "*******Apiref********", "SocialMedia/ErrorLog/Mail/" + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
            return getgmailcredentials;
        }
        public void Dagetdefaulttemplate(MdlSmrTrnCustomerSummary values)
        {
            try
            {

                msSQL = "select default_template from smr_smm_gmail_service";
                values.default_template = objdbconn.GetExecuteScalar(msSQL);

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Mail Details !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                values.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

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

        public void daeportalmail(string employee_gid , string user_gid , eportallist values)
        {
            msSQL = " select eportal_emailid from crm_mst_tcustomer where eportal_emailid='" + values.eportalemail_id + "'";
            string lsmail_id = objdbconn.GetExecuteScalar(msSQL);
            if (lsmail_id == null || lsmail_id == "")
            {

                msSQL = "Update crm_mst_tcustomer set " +
                " eportal_emailid = '" + values.eportalemail_id + "'," +
                "eportal_password = '" + objcmnfunctions.ConvertToAscii(values.confirmpassword) + "'," +
                "length = '" + values.confirmpassword.Length + "'" +

                "where customer_gid = '" + values.customer_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = "select company_code from adm_mst_tcompany";
                string companyname = objdbconn.GetExecuteScalar(msSQL);

                if (companyname == "BOBA")
                {
                    msSQL = " select company_code from adm_mst_tcompany";
                    string lscompany_code = objdbconn.GetExecuteScalar(msSQL);

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

                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = options.Execute(request);
                    string errornetsuiteJSON = response.Content;
                    refreshtokenlist objMdlGmailCampaignResponse = new refreshtokenlist();
                    objMdlGmailCampaignResponse = JsonConvert.DeserializeObject<refreshtokenlist>(errornetsuiteJSON);

                    string customer_gid = values.customer_gid;
                    string from_mail = getgmailcredentials.gmail_address;
                    string to_mail = values.eportalemail_id;
                    string sub = values.subject;
                    string bodiess = values.body;

                    var options1 = new RestClient("https://gmail.googleapis.com");
                    var request1 = new RestRequest("/gmail/v1/users/me/messages/send", Method.POST);
                    request1.AddHeader("Authorization", "Bearer  " + objMdlGmailCampaignResponse.access_token + "");
                    request1.AddHeader("Content-Type", "application/json");
                    var bodies = @"{" + "\n" +
                   @"    ""raw"":""" + values.fullcontent + "\"" + "\n" +
                   @"    " + "\n" +
                   @"}";

                    request1.AddParameter("application/json", bodies, ParameterType.RequestBody);
                    IRestResponse response1 = options1.Execute(request1);
                    string errornetsuiteJSON1 = response1.Content;
                    responselist objMdlGmailCampaignResponse1 = new responselist();
                    objMdlGmailCampaignResponse1 = JsonConvert.DeserializeObject<responselist>(errornetsuiteJSON);

                    if (response1.StatusCode == HttpStatusCode.OK)
                    {

                        msSQL = "update crm_mst_tcustomer set " +
                       " eportal_status = 'Y'" +
                       "where customer_gid = '" + values.customer_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                        msGetGid1 = objcmnfunctions.GetMasterGID("UPLF");
                        string encodedBody = EncodeToBase64(bodies);
                        string encodedsub = EncodeToBase64(sub);
                        msSQL = "INSERT INTO smr_trn_gmailattachment (" +
                                  "gmail_gid, " +
                                "from_mailaddress, " +
                                "to_mailaddress, " +
                                "mail_subject, " +
                                "mail_body, " +
                                "transmission_id, " +
                              "file_gid, " +
                                 " created_by, " +
                                "created_date) " +
                                "VALUES (" +
                                 "'" + msGetGid1 + "', " +
                                "'" + from_mail + "', " +
                                "'" + to_mail + "', " +
                                "'" + encodedsub + "', " +
                                "'" + encodedBody + "', " +
                                "'" + objMdlGmailCampaignResponse1.id + "', " +
                                  "'" + customer_gid + "', " +
                                  "'" + user_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        if (response1.StatusCode == HttpStatusCode.OK)
                        {
                            values.status = true;
                            values.message = "mail sent successfully";
                            return;
                        }
                        else
                        {
                            values.status = false;
                            values.message = "error occured while sending Mail";
                            return;
                        }

                    }

                }
                else
                {
                    string lsapprovedby;
                    string message;
                    string employee_mailid = null;
                    string employeename = null;
                    string applied_by = null;
                    string supportmail = null;
                    string pwd = null;
                    string reason = null;
                    string days = null;
                    string fromhours = null;
                    string tohours = null;
                    string emailpassword = null;
                    string trace_comment;
                    string permission_date = null;
                    string todate = null;
                    string fromdate = null;
                    string lsleavetypename = null;
                    string password = "";
                    string toemailid = "";
                    string customername = "";
                    bool MailFlag;

                    msSQL = "select customer_name,eportal_password,eportal_emailid from crm_mst_tcustomer where customer_gid = '" + values.customer_gid + "'";
                    dt_datatable = objdbconn.GetDataTable(msSQL);
                    if (dt_datatable.Rows.Count != 0)
                    {
                        foreach (DataRow dt in dt_datatable.Rows)
                        {
                            customername = dt["customer_name"].ToString();
                            toemailid = dt["eportal_emailid"].ToString();
                            password = dt["eportal_password"].ToString();
                        }
                    }
                    password = values.confirmpassword;





                    MailFlag = objcmnfunctions.mail(toemailid, values.subject, values.body);
                    if (MailFlag == true)
                    {
                        msSQL = "update crm_mst_tcustomer set " +
                            " eportal_status = 'Y'" +
                            "where customer_gid = '" + values.customer_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        values.status = true;
                        values.message = "mail sent successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "error occured while sending Mail";
                    }

                }
            }
            else
            {
                values.status = false;
                values.message = "This email address is already in use by another customer";
            }
        }
    }
}