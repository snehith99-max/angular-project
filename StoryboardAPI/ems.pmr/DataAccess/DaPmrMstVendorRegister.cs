using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.pmr.Models;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text.RegularExpressions;

using System.Data.OleDb;

using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using static OfficeOpenXml.ExcelErrorValue;
using Newtonsoft.Json;
using RestSharp;

namespace ems.pmr.DataAccess
{
    public class DaPmrMstVendorRegister
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        HttpPostedFile httpPostedFile;
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        string msEmployeeGID, lsemployee_gid, lsentity_code, lscountry, lsCountryGID, lsdesignation_code, lsCode, msGetGid, msGetGid1, msGetGid2, msGetPrivilege_gid,
            final_path, msGetModule2employee_gid, maGetGID, lsvendor_code, msUserGid, maGetGID1;
        int mnResult, mnResult1, mnResult2, mnResult3, mnResult4, mnResult5, importcount;
        private object msGetGID;
        private string mrGetGID;
        private string exclproducttypecode;
        private string mcGetGID;

        string base_url, lsclient_id, api_key = string.Empty;

        string tin_number, pan_number, excise_details, ifsc_code, region_gid, lsvendor_companyname, address_gid, tax_gid,
            taxsegment_gid, currencyexchange_gid, country_gid, fax, msGETtmpGID,
            bank_details, cst_number, servicetax_numbers, lsregion_gid, vendor_code = string.Empty, mintsoft_countryid, mintsoft_currencyid, country_name;

        public void DaGetVendorRegisterSummary(MdlPmrMstVendorRegister values)
        {
            try
            {

                msSQL = " SELECT a.vendor_gid, a.taxsegment_gid, a.vendor_code, a.vendor_companyname, CONCAT(a.contactperson_name, '  / ', a.email_id, ' / ', a.contact_telephonenumber) AS Contact, " +
                " a.average_leadtime, a.contact_telephonenumber, a.email_id, a.billing_email, a.tax_number, e.taxsegment_name, b.region_name, c.address1, " +
                " c.address2, c.city, c.postal_code, d.country_name, a.credit_days, a.payment_terms, " +
                " CASE WHEN a.active_flag = 'Y' THEN 'Active' " +
                " ELSE 'In-Active' END AS active_flag,(select mintsoft_flag from adm_mst_tcompany limit 1) as mintsoft_flag,supplier_id,a.vendor_gid    FROM acp_mst_tvendor a " +
                " LEFT JOIN crm_mst_Tregion b ON a.region_gid = b.region_gid " +
                " LEFT JOIN adm_mst_taddress c ON a.address_gid = c.address_gid " +
                " LEFT JOIN adm_mst_tcountry d ON c.country_gid = d.country_gid " +
                " LEFT JOIN acp_mst_ttaxsegment e on e.taxsegment_gid = e.taxsegment_gid " +
                " group by vendor_gid ORDER BY a.vendor_gid DESC ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getvendor_lists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getvendor_lists
                        {

                            vendor_gid = dt["vendor_gid"].ToString(),
                            vendor_code = dt["vendor_code"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contactperson_name = dt["Contact"].ToString(),
                            region = dt["region_name"].ToString(),
                            average_leadtime = dt["average_leadtime"].ToString(),
                            active_flag = dt["active_flag"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            billing_email = dt["billing_email"].ToString(),
                            tax_number = dt["tax_number"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            city = dt["city"].ToString(),
                            mintsoft_flag = dt["mintsoft_flag"].ToString(),
                            supplier_id = dt["supplier_id"].ToString(),
                            ///vendor_gid = dt["vendor_gid"].ToString(),
                        });
                        values.Getvendor_lists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting vendor register summary!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetcountry(MdlPmrMstVendorRegister values)
        {
            try
            {

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
                values.message = "Exception occured while getting country";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetcurrency(MdlPmrMstVendorRegister values)
        {
            try
            {

                msSQL = " Select currency_code ,currencyexchange_gid  " +
                " from crm_trn_tcurrencyexchange ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Getcurency>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Getcurency
                        {
                            currency_code = dt["currency_code"].ToString(),
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                        });
                        values.Getcurency = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting currency!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGettax(MdlPmrMstVendorRegister values)
        {
            try
            {

                msSQL = " Select tax_gid ,tax_name " +
            " from acp_mst_ttax  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<Gettax>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new Gettax
                        {
                            tax_name = dt["tax_name"].ToString(),
                            tax_gid = dt["tax_gid"].ToString(),
                        });
                        values.Gettax = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetRegion(MdlPmrMstVendorRegister values)
        {
            try
            {

                msSQL = " Select region_gid ,concat (region_name,' / ',city) as region_name " +
            " from crm_mst_Tregion  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<VendorRegion_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new VendorRegion_list
                        {
                            region_name = dt["region_name"].ToString(),
                            region_gid = dt["region_gid"].ToString(),
                        });
                        values.VendorRegion_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting tax!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostVendorRegister(string user_gid, vendor_list values)

        {
            try
            {


                msSQL = " select * from acp_mst_tvendor where vendor_code= '" + values.vendor_code + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    //values.status = true;
                    values.message = "Vendor code Already Exist";
                }
                msSQL = " select * from acp_mst_tvendor where vendor_companyname= '" + values.vendor_companyname.Replace("'", "\\\'")
                + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    //values.status = true;
                    values.message = "Vendor Already Exist";
                }
                else
                {
                    msSQL = " select mintsoft_flag from adm_mst_tcompany limit 1";
                    string mintsoft_flag = objdbconn.GetExecuteScalar(msSQL);
                    // code by snehith to push  product in mintsoft
                    if (mintsoft_flag == "Y")
                    {

                        msSQL = " select * from smr_trn_tminsoftconfig;";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            
                            base_url = objOdbcDataReader["base_url"].ToString();
                            api_key = objOdbcDataReader["api_key"].ToString();
                            lsclient_id = objOdbcDataReader["client_id"].ToString();
                        }
                        objOdbcDataReader.Close();

                        msSQL = "SELECT mintsoft_countryid,country_name FROM adm_mst_tcountry " +
                            " where  country_gid= '" + values.countryname + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            mintsoft_countryid = objOdbcDataReader["mintsoft_countryid"].ToString();
                            country_name = objOdbcDataReader["country_name"].ToString();
                        }
                        objOdbcDataReader.Close();
                        msSQL = "SELECT mintsoft_currencyid FROM crm_trn_tcurrencyexchange " +
                              " where  currencyexchange_gid= '" + values.currencyname + "' ";
                        objOdbcDataReader = objdbconn.GetDataReader(msSQL);
                        if (objOdbcDataReader.HasRows == true)
                        {
                            mintsoft_currencyid = objOdbcDataReader["mintsoft_currencyid"].ToString();
                        }
                        objOdbcDataReader.Close();
                        if (!string.IsNullOrEmpty(mintsoft_countryid))
                        {
                            if (!string.IsNullOrEmpty(mintsoft_currencyid))
                            {
                                msGetGid1 = objcmnfunctions.GetMasterGID("PVRR");
                                msGetGid2 = objcmnfunctions.GetMasterGID("PVRM");
                                msGetGid = objcmnfunctions.GetMasterGID("SADM");
                                msSQL = " SELECT currency_code FROM crm_trn_tcurrencyexchange WHERE currencyexchange_gid='" + values.currencyname + "' ";
                                string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

                                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PVRR' order by finyear desc limit 0,1 ";
                                string lsCode = objdbconn.GetExecuteScalar(msSQL);

                                string lsvendor_code = "VC" + "00" + lsCode;

                                //msSQL = " insert into acp_mst_tvendorregister (" +
                                //    " vendorregister_gid, " +
                                //    " vendor_code, " +
                                //    " vendor_companyname, " +
                                //    " contactperson_name, " +
                                //    " contact_telephonenumber, " +
                                //    " email_id," +
                                //    " active_flag," +
                                //    " tin_number," +
                                //    " excise_details," +
                                //    " pan_number," +
                                //    " servicetax_number," +
                                //    " cst_number," +
                                //    " bank_details," +
                                //    " ifsc_code," +
                                //    " region_gid, " +
                                //    " average_leadtime, " +
                                //    " billing_email, " +
                                //    " credit_days, " +
                                //    " payment_terms, " +
                                //    " tax_number, " +
                                //    " rtgs_code, " +
                                //    " vendor_status, " +
                                //    " address_gid," +
                                //    " tax_gid," +
                                //     " taxsegment_gid," +
                                //    " currencyexchange_gid) " +
                                //    " values (" +
                                //    "'" + msGetGid1 + "', " +
                                //    "'" + lsvendor_code + "', " +
                                //    "'" + values.vendor_companyname.Replace("'", "\\\'") + "'," +
                                //    "'" + values.contactperson_name.Replace("'", "\\\'") + "'," +
                                //    "'" + values.mobile.e164Number + "'," +
                                //    "'" + values.email_address + "'," +
                                //    "'Y'," +
                                //    "'" + values.tin_number + "'," +
                                //    "'" + values.excise_details + "'," +
                                //    "'" + values.pan_number + "'," +
                                //    "'" + values.servicetax_number + "'," +
                                //    "'" + values.cst_number + "'," +
                                //    "'" + values.bank_details + "'," +
                                //    "'" + values.ifsc_code + "'," +
                                //    "'" + values.region + "'," +
                                //    "'" + values.averageleadtime + "'," +
                                //    "'" + values.billingemail_address + "'," +
                                //    "'" + values.creditdays + "'," +
                                //    "'" + values.paymentterms + "'," +
                                //    "'" + values.tax_number + "'," +
                                //    "'" + values.rtgs_code + "'," +
                                //    "'Vendor Approved'," +
                                //    "'" + msGetGid + "', " +
                                //    "'" + values.taxname + "'," +
                                //     "'" + values.taxsegment_name + "'," +
                                //    "'" + values.currencyname + "')";
                                //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                                //if (mnResult != 0)
                                //{
                                msSQL = " insert into acp_mst_tvendor (" +
                                 " vendor_gid, " +
                                 " vendor_code, " +
                                 " vendor_companyname, " +
                                 " contactperson_name, " +
                                 " contact_telephonenumber, " +
                                 " email_id," +
                                 " active_flag," +
                                 " tin_number," +
                                 " excise_details," +
                                 " pan_number," +
                                 " servicetax_number," +
                                 " cst_number," +
                                 " bank_details," +
                                 " ifsc_code," +
                                  " region_gid, " +
                                 " average_leadtime, " +
                                 " billing_email, " +
                                 " credit_days, " +
                                 " payment_terms, " +
                                 " tax_number, " +
                                 " rtgs_code, " +
                                 " autopo, " +
                                 " address_gid," +
                                 " vendorregister_gid," +
                                 " tax_gid," +
                                  " taxsegment_gid," +
                                 " currencyexchange_gid) " +
                                 " values (" +
                                 "'" + msGetGid1 + "', " +
                                 "'" + lsvendor_code + "', ";
                                     if (!string.IsNullOrEmpty(values.vendor_companyname) && values.vendor_companyname.Contains("'"))
                                    {
                                        msSQL += "'" + values.vendor_companyname.Replace("'", "\\\'") + "', ";
                                    }
                                    else
                                    {
                                        msSQL += "'" + values.vendor_companyname + "', ";
                                    }
                                
                           msSQL += "'" + values.contactperson_name.Replace("'", "\\\'") + "'," +
                                    "'" + values.mobile.e164Number + "'," +
                                    "'" + values.email_address + "'," +
                                    "'Y'," +
                                    "'" + values.tin_number + "'," +
                                    "'" + values.excise_details + "'," +
                                    "'" + values.pan_number + "'," +
                                    "'" + values.servicetax_number + "'," +
                                    "'" + values.cst_number + "'," +
                                    "'" + values.bank_details + "'," +
                                    "'" + values.ifsc_code + "'," +
                                      "'" + values.region + "'," +
                                    "'" + values.averageleadtime + "'," +
                                    "'" + values.billingemail_address + "'," +
                                    "'" + values.creditdays + "'," +
                                    "'" + values.paymentterms + "'," +
                                    "'" + values.tax_number + "'," +
                                    "'" + values.rtgs_code + "'," +
                                    "'N'," +
                                    "'" + msGetGid + "', " +
                                    "'" + msGetGid1 + "', " +
                                    "'" + values.taxname + "'," +
                                      "'" + values.taxsegment_name + "'," +
                                    "'" + values.currencyname + "')";
                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                //}

                                if (mnResult != 0)
                                {

                                    msSQL = " insert into adm_mst_taddress " +
                                    " (address_gid, " +
                                    " vendor_gid, " +
                                    " address1, " +
                                    " address2, " +
                                    " city, " +
                                    " state, " +
                                    " postal_code, " +
                                    " country_gid, " +
                                    " fax ) " +
                                    " values (" +
                                    "'" + msGetGid + "', " +
                                    "'" + msGetGid1 + "', ";
                                    //"'" + values.address.Trim().Replace("'", "") + "'," +
                                    if (!string.IsNullOrEmpty(values.address) && values.address.Contains("'"))
                                    {
                                        msSQL += "'" + values.address.Replace("'", "\\\'") + "',";
                                    }
                                    else
                                    {
                                        msSQL += "'" + values.address + "', ";
                                    }


                                    //"'" + values.address2.Trim().Replace("'", "") + "'," +
                                    if (!string.IsNullOrEmpty(values.address2) && values.address2.Contains("'"))
                                    {
                                        msSQL += "'" + values.address2.Replace("'", "\\\'") + "',";
                                    }
                                    else
                                    {
                                        msSQL += "'" + values.address2 + "', ";
                                    }

                                    msSQL += "'" + values.city + "'," +
                                    "'" + values.state_name + "'," +
                                    "'" + values.postal_code + "'," +
                                    "'" + values.countryname + "'," +
                                    "'" + values.fax_name + "')";

                                    mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);


                                }
                                if (mnResult != 0)
                                {
                                    var client = new RestClient(base_url);
                                    var request = new RestRequest("/api/Product/Suppliers", Method.PUT);
                                    request.AddHeader("Content-Type", "application/json");
                                    request.AddHeader("ms-apikey", api_key);
                                    var jsonPayload = new
                                    {
                                        Name = values.vendor_companyname.Replace("'", "\\\'"),
                                        ContactName = values.contactperson_name.Replace("'", "\\\'"),
                                        ContactNumber = values.mobile.e164Number,
                                        AddressLine1 = values.address.Replace("'", "\\\'"),
                                        AddressLine2 = values.address2.Replace("'", "\\\'"),
                                        Town = values.city,
                                        County = country_name,
                                        Postcode = values.postal_code,
                                        ContactEmail = values.email_address,
                                        Active = true,
                                        Code = lsvendor_code,
                                        CountryId = mintsoft_countryid,
                                        CurrencyId = mintsoft_currencyid,
                                        ClientId = lsclient_id
                                    };

                                    request.AddJsonBody(jsonPayload);
                                    IRestResponse response = client.Execute(request);
                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        var responseData = JsonConvert.DeserializeObject<MintsoftSupplierResponse>(response.Content);
                                        msSQL = "update acp_mst_tvendor set " +
                                        " supplier_id  = '" + responseData.ID + "'" +
                                        " where vendor_gid = '" + msGetGid1 + "'";
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        if (mnResult != 0)
                                        {
                                            values.status = true;
                                            values.message = "Vendor Added Successfully";
                                        }
                                        else
                                        {
                                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                           "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                                            values.status = false;
                                            values.message = "Error While Creating Vendor";
                                        }
                                    }
                                    else
                                    {
                                        string errorMessage = $"Failed to fetch products. Status code: {response.StatusCode}, Reason: {response.ErrorMessage}";
                                        objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + errorMessage, "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                        values.status = false;
                                        values.message = "Error While Creating Product";
                                    }
                                }
                                else
                                {
                                    values.status = false;
                                    values.message = "Error While Adding Vendor";
                                }
                            }
                            else
                            {
                                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + "Selected Currency Not Available in Mintosft", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                                values.status = false;
                                values.message = "Selected Currency Not Available in Mintosft";


                            }
                        }
                        else
                        {
                            objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "***********" + "Selected Country Not Available in Mintosft", "ErrorLog/Mintsoft/ " + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
                            values.status = false;
                            values.message = "Selected Country Not Available in Mintosft";
                        }
                    }
                    else
                    {
                        msGetGid1 = objcmnfunctions.GetMasterGID("PVRR");
                        msGetGid2 = objcmnfunctions.GetMasterGID("PVRM");
                        msGetGid = objcmnfunctions.GetMasterGID("SADM");
                        msSQL = " SELECT currency_code FROM crm_trn_tcurrencyexchange WHERE currencyexchange_gid='" + values.currencyname + "' ";
                        string lscurrency_code = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PVRR' order by finyear desc limit 0,1 ";
                        string lsCode = objdbconn.GetExecuteScalar(msSQL);

                        string lsvendor_code = "VC" + "00" + lsCode;

                        //msSQL = " insert into acp_mst_tvendorregister (" +
                        //    " vendorregister_gid, " +
                        //    " vendor_code, " +
                        //    " vendor_companyname, " +
                        //    " contactperson_name, " +
                        //    " contact_telephonenumber, " +
                        //    " email_id," +
                        //    " active_flag," +
                        //    " tin_number," +
                        //    " excise_details," +
                        //    " pan_number," +
                        //    " servicetax_number," +
                        //    " cst_number," +
                        //    " bank_details," +
                        //    " ifsc_code," +
                        //    " region_gid, " +
                        //    " average_leadtime, " +
                        //    " billing_email, " +
                        //    " credit_days, " +
                        //    " payment_terms, " +
                        //    " tax_number, " +
                        //    " rtgs_code, " +
                        //    " vendor_status, " +
                        //    " address_gid," +
                        //    " tax_gid," +
                        //     " taxsegment_gid," +
                        //    " currencyexchange_gid) " +
                        //    " values (" +
                        //    "'" + msGetGid1 + "', " +
                        //    "'" + lsvendor_code + "', " +
                        //    "'" + values.vendor_companyname.Replace("'", "\\\'") + "'," +
                        //    "'" + values.contactperson_name.Replace("'", "\\\'") + "'," +
                        //    "'" + values.mobile.e164Number + "'," +
                        //    "'" + values.email_address + "'," +
                        //    "'Y'," +
                        //    "'" + values.tin_number + "'," +
                        //    "'" + values.excise_details + "'," +
                        //    "'" + values.pan_number + "'," +
                        //    "'" + values.servicetax_number + "'," +
                        //    "'" + values.cst_number + "'," +
                        //    "'" + values.bank_details + "'," +
                        //    "'" + values.ifsc_code + "'," +
                        //    "'" + values.region + "'," +
                        //    "'" + values.averageleadtime + "'," +
                        //    "'" + values.billingemail_address + "'," +
                        //    "'" + values.creditdays + "'," +
                        //    "'" + values.paymentterms + "'," +
                        //    "'" + values.tax_number + "'," +
                        //    "'" + values.rtgs_code + "'," +
                        //    "'Vendor Approved'," +
                        //    "'" + msGetGid + "', " +
                        //    "'" + values.taxname + "'," +
                        //     "'" + values.taxsegment_name + "'," +
                        //    "'" + values.currencyname + "')";
                        //mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        //if (mnResult != 0)
                        //{
                        msSQL = " insert into acp_mst_tvendor (" +
                        " vendor_gid, " +
                        " vendor_code, " +
                        " vendor_companyname, " +
                        " contactperson_name, " +
                        " contact_telephonenumber, " +
                        " email_id," +
                        " active_flag," +
                        " tin_number," +
                        " excise_details," +
                        " pan_number," +
                        " servicetax_number," +
                        " cst_number," +
                        " bank_details," +
                        " ifsc_code," +
                         " region_gid, " +
                        " average_leadtime, " +
                        " billing_email, " +
                        " credit_days, " +
                        " payment_terms, " +
                        " tax_number, " +
                        " rtgs_code, " +
                        " autopo, " +
                        " address_gid," +
                        " vendorregister_gid," +
                        " tax_gid," +
                         " taxsegment_gid," +
                        " currencyexchange_gid) " +
                        " values (" +
                        "'" + msGetGid1 + "', " +
                        "'" + lsvendor_code + "', ";
                            if (!string.IsNullOrEmpty(values.vendor_companyname) && values.vendor_companyname.Contains("'"))
                        {
                            msSQL += "'" + values.vendor_companyname.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.vendor_companyname + "', ";
                        }
                        if (!string.IsNullOrEmpty(values.contactperson_name) && values.contactperson_name.Contains("'"))
                        {
                            msSQL += "'" + values.contactperson_name.Replace("'", "\\\'") + "',";
                        }
                        else
                        {
                            msSQL += "'" + values.contactperson_name + "', ";
                        }
                          msSQL +=  "'" + values.mobile.e164Number + "'," +
                            "'" + values.email_address + "'," +
                            "'Y'," +
                            "'" + values.tin_number + "'," +
                            "'" + values.excise_details + "'," +
                            "'" + values.pan_number + "'," +
                            "'" + values.servicetax_number + "'," +
                            "'" + values.cst_number + "'," +
                            "'" + values.bank_details + "'," +
                            "'" + values.ifsc_code + "'," +
                              "'" + values.region + "'," +
                            "'" + values.averageleadtime + "'," +
                            "'" + values.billingemail_address + "'," +
                            "'" + values.creditdays + "'," +
                            "'" + values.paymentterms + "'," +
                            "'" + values.tax_number + "'," +
                            "'" + values.rtgs_code + "'," +
                            "'N'," +
                            "'" + msGetGid + "', " +
                            "'" + msGetGid1 + "', " +
                            "'" + values.taxname + "'," +
                              "'" + values.taxsegment_name + "'," +
                            "'" + values.currencyname + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        //}

                        if (mnResult != 0)
                        {

                            msSQL = " insert into adm_mst_taddress " +
                            " (address_gid, " +
                            " vendor_gid, " +
                            " address1, " +
                            " address2, " +
                            " city, " +
                            " state, " +
                            " postal_code, " +
                            " country_gid, " +
                            " fax ) " +
                            " values (" +
                            "'" + msGetGid + "', "+
                            "'" + msGetGid1 + "', ";
                            //"'" + values.address.Trim().Replace("'", "") + "'," +
                            if (!string.IsNullOrEmpty(values.address) && values.address.Contains("'"))
                            {
                                msSQL += "'" + values.address.Replace("'", "\\\'") + "',";
                            }
                            else
                            {
                                msSQL += "'" + values.address + "', ";
                            }

                            //"'" + values.address2.Trim().Replace("'", "") + "'," +
                            if (!string.IsNullOrEmpty(values.address2) && values.address2.Contains("'"))
                            {
                                msSQL += "'" + values.address2.Replace("'", "\\\'") + "',";
                            }
                            else
                            {
                                msSQL += "'" + values.address2 + "', ";
                            }
                            msSQL += "'" + values.city + "'," +
                            "'" + values.state_name + "'," +
                            "'" + values.postal_code + "'," +
                            "'" + values.countryname + "'," +
                            "'" + values.fax_name + "')";

                            mnResult1 = objdbconn.ExecuteNonQuerySQL(msSQL);


                        }
                        if (mnResult != 0)
                        {

                            values.status = true;
                            values.message = "Vendor Added Successfully";
                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Adding Vendor";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding vendor register!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public class MintsoftSupplierResponse
        {
            public string ID { get; set; }
            public string Success { get; set; }
            public string Message { get; set; }
            public string WarningMessage { get; set; }
            public string AllocatedFromReplen { get; set; }

        }
        public void DaGetDocumentType(MdlPmrMstVendorRegister values)
        {
            try
            {

                msSQL = "select documenttype_gid,documenttype_name " +
     " from  acp_mst_tvendordocumenttype  ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetDocumentType>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetDocumentType
                        {
                            documenttype_gid = dt["documenttype_gid"].ToString(),
                            documenttype_name = dt["documenttype_name"].ToString(),
                        });
                        values.GetDocumentType = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting document type!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaUpdateVendorStatus(string user_gid, ActiveStatus_list values)
        {
            try
            {

  //              msSQL = " update acp_mst_tvendorregister set active_flag='" + values.active_flag + "',approved_remarks='" + values.product_desc.Replace("'", "\\\'")
  //+ "' where vendorregister_gid='" + values.vendorregister_gid + "'";

  //              mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                msSQL = " update acp_mst_tvendor set active_flag='" + values.active_flag + "' where vendor_gid='" + values.vendor_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Vendor Status Updated Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Vendor Status ";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating vendor status!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetVendorRegisterDetail(string vendor_gid, MdlPmrMstVendorRegister values)
        {
            try
            {

                msSQL = " SELECT a.vendor_gid,a.address_gid, a.currencyexchange_gid, a.vendor_code,d.currency_code," +
                        " a.vendor_companyname, a.contactperson_name," +
                        " a.contact_telephonenumber, a.email_id, b.address1,a.region_gid," +
                        " b.address2, b.city,a.tin_number,a.excise_details,a.pan_number," +
                        " a.servicetax_number,a.cst_number, a.bank_details, a.ifsc_code, a.rtgs_code," +
                        " a.blacklist_remarks, a.blacklist_flag, a.blacklist_date, a.blacklist_by, " +
                        " b.state,f.tax_name ,b.postal_code," +
                        " b.country_gid, b.fax," +
                        " c.country_name, g.taxsegment_gid,g.taxsegment_name," +
                        " a.average_leadtime,a.billing_email,a.credit_days,h.region_name,a.tax_number,a.payment_terms " +
                        " FROM acp_mst_tvendor a " +
                        " left join adm_mst_taddress b on  a.address_gid = b.address_gid " +
                        " left join adm_mst_tcountry c on  b.country_gid = c.country_gid " +
                        " left join crm_trn_tcurrencyexchange d on a.currencyexchange_gid = d.currencyexchange_gid " +
                        " left join acp_mst_ttax f on a.tax_gid = f.tax_gid " +
                        "  left join crm_mst_tregion h on a.region_gid=h.region_gid " +
                         " left join acp_mst_ttaxsegment g on g.taxsegment_gid=a.taxsegment_gid " +
                        " where a.vendor_gid = '" + vendor_gid + "'";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<editvendorregistersummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new editvendorregistersummary_list
                        {

                            vendorregister_gid = dt["vendor_gid"].ToString(),

                            vendor_code = dt["vendor_code"].ToString(),
                            tax_name = dt["tax_name"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            vendor_companyname = dt["vendor_companyname"].ToString(),
                            contactperson_name = dt["contactperson_name"].ToString(),
                            contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                            email_id = dt["email_id"].ToString(),
                            address1 = dt["address1"].ToString(),
                            address2 = dt["address2"].ToString(),
                            city = dt["city"].ToString(),
                            tin_number = dt["tin_number"].ToString(),
                            excise_details = dt["excise_details"].ToString(),
                            pan_number = dt["pan_number"].ToString(),
                            servicetax_number = dt["servicetax_number"].ToString(),
                            cst_number = dt["cst_number"].ToString(),
                            bank_details = dt["bank_details"].ToString(),
                            ifsc_code = dt["ifsc_code"].ToString(),
                            rtgs_code = dt["rtgs_code"].ToString(),
                            blacklist_remarks = dt["blacklist_remarks"].ToString(),
                            blacklist_flag = dt["blacklist_flag"].ToString(),
                            blacklist_date = dt["blacklist_date"].ToString(),
                            blacklist_by = dt["blacklist_by"].ToString(),
                            state = dt["state"].ToString(),
                            average_leadtime = dt["average_leadtime"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            postal_code = dt["postal_code"].ToString(),
                            fax = dt["fax"].ToString(),
                            billing_email = dt["billing_email"].ToString(),
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),
                            tax_number = dt["tax_number"].ToString(),
                            region_name = dt["region_name"].ToString(),
                            payment_terms = dt["payment_terms"].ToString(),
                            credit_days = dt["credit_days"].ToString(),
                            country_gid = dt["country_gid"].ToString(),
                            region_gid = dt["region_gid"].ToString(),
                            address_gid = dt["address_gid"].ToString(),

                        });
                        values.editvendorregistersummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting vendor register details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostVendorRegisterUpdate(string user_gid, vendor_listaddinfo values)


        {
            try
            {


                msSQL = " SELECT country_gid FROM  adm_mst_tcountry WHERE country_name='" + values.country_name + "' ";
                string country_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT currencyexchange_gid FROM crm_trn_tcurrencyexchange WHERE currency_code='" + values.currency_code + "' ";
                string currencyexchange_gid = objdbconn.GetExecuteScalar(msSQL);



                msSQL = " SELECT taxsegment_gid FROM acp_mst_ttaxsegment WHERE taxsegment_name='" + values.taxsegment_name + "' and reference_type='Vendor' ";
                string taxsegment_gid = objdbconn.GetExecuteScalar(msSQL);
                msSQL = " SELECT region_gid FROM crm_mst_Tregion WHERE region_gid='" + values.region + "' ";
                string lsregion_gid = objdbconn.GetExecuteScalar(msSQL);

                          
                msSQL = "UPDATE acp_mst_tvendor SET ";

                if (!string.IsNullOrEmpty(values.vendor_companyname) && values.vendor_companyname.Contains("'"))
                {
                    msSQL += "vendor_companyname = '" + values.vendor_companyname.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "vendor_companyname = '" + values.vendor_companyname + "', ";
                }

                if (!string.IsNullOrEmpty(values.contactperson_name) && values.contactperson_name.Contains("'"))
                {
                    msSQL += "contactperson_name = '" + values.contactperson_name.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "contactperson_name = '" + values.contactperson_name + "', ";
                }

                msSQL += "contact_telephonenumber = '" + values.contact_telephonenumber.e164Number + "'," +
                         "email_id = '" + values.email_id + "'," +
                         "tin_number = '" + values.tin_number + "'," +
                         "excise_details = '" + values.excise_details + "'," +
                         "pan_number = '" + values.pan_number + "'," +
                         "servicetax_number = '" + values.servicetax_number + "'," +
                         "tax_number = '" + values.tax_number + "'," +
                         "cst_number = '" + values.cst_number + "'," +
                         "bank_details = '" + values.bank_details + "'," +
                         "ifsc_code = '" + values.ifsc_code + "'," +
                         "rtgs_code = '" + values.rtgs_code + "'," +
                         "region_gid = '" + lsregion_gid + "'," +
                         "average_leadtime = '" + values.averageleadtime + "',";

                if (!string.IsNullOrEmpty(values.paymentterms) && values.paymentterms.Contains("'"))
                {
                    msSQL += "payment_terms = '" + values.paymentterms.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "payment_terms = '" + values.paymentterms + "', ";
                }

                msSQL += "credit_days = '" + values.creditdays + "'," +
                         "billing_email = '" + values.billingemail_address + "'," +
                         "taxsegment_gid = '" + taxsegment_gid + "'," +
                         "currencyexchange_gid = '" + currencyexchange_gid + "'" +
                         " WHERE vendor_gid = '" + values.vendor_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

               
                msSQL = "UPDATE adm_mst_taddress SET ";

                if (!string.IsNullOrEmpty(values.address) && values.address.Contains("'"))
                {
                    msSQL += "address1 = '" + values.address.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "address1 = '" + values.address + "', ";
                }

                if (!string.IsNullOrEmpty(values.address2) && values.address2.Contains("'"))
                {
                    msSQL += "address2 = '" + values.address2.Replace("'", "\\\'") + "', ";
                }
                else
                {
                    msSQL += "address2 = '" + values.address2 + "', ";
                }

                msSQL += "city = '" + values.city + "'," +
                         "state = '" + values.state + "'," +
                         "postal_code = '" + values.postal_code + "'," +
                         "country_gid = '" + country_gid + "'," +
                         "fax = '" + values.fax + "'" +
                         " WHERE address_gid = '" + values.address_gid + "'";

                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult != 0)

                {

                    values.status = true;
                    values.message = "Vendor Updated Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Error While Updating Vendor ";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while updating vendor register!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaVendorRegisterSummaryDelete(string vendor_gid, vendor_listaddinfo values)
        {
            try
            {

                msSQL = "select vendor_gid from pmr_trn_tpurchaseorder where vendor_gid ='" + vendor_gid + "' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    values.message = "You Cannot Delete Because Purchase Order has been Raised for this Vendor!";
                    return;
                }
                else
                {


                    msSQL = " SELECT vendor_code FROM  acp_mst_tvendor WHERE vendor_gid='" + vendor_gid + "' ";
                    string vendor_code = objdbconn.GetExecuteScalar(msSQL);

                    msSQL = "  Delete from acp_mst_tvendor where vendor_code='" + vendor_code + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    msSQL = "  Delete from acp_mst_tvendor where vendor_gid='" + vendor_gid + "'  ";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        values.status = true;
                        values.message = "Vendor Deleted Successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While Deleting Vendor";
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while deleting vendor!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostVendorRegisterAdditionalInformation(string user_gid, vendor_listaddinfo values)

        {
            try
            {

                if (values.bank_details == null && values.cst_number == null && values.excise_details == null
                        && values.ifsc_code == null && values.pan_number == null &&
                        values.rtgs_code == null && values.servicetax_number == null && values.tin_number == null)
                { values.message = "Please Enter The Values"; }
                else
                {
                    msSQL = " update acp_mst_tvendor set" +
                            " bank_details='" + values.bank_details + "', " +
                            " cst_number='" + values.cst_number + "'," +
                            " excise_details='" + values.excise_details + "'," +
                            " ifsc_code='" + values.ifsc_code + "'," +
                            " pan_number='" + values.pan_number + "'," +
                            " rtgs_code='" + values.rtgs_code + "'," +
                            " servicetax_number='" + values.servicetax_number + "'," +
                            " tin_number='" + values.tin_number + "'" +
                            " WHERE vendorregister_gid = '" + values.vendor_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    if (mnResult != 0)
                    {
                        msSQL = " SELECT vendor_code FROM  acp_mst_tvendorregister WHERE vendor_gid='" + values.vendor_gid + "' ";
                        string vendor_code = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = "  Select vendor_gid from acp_mst_tvendor where vendor_code='" + vendor_code + "'  ";
                        string vendor_gid = objdbconn.GetExecuteScalar(msSQL);

                        msSQL = " update acp_mst_tvendor set" +
                            " bank_details='" + values.bank_details + "', " +
                            " cst_number='" + values.cst_number + "'," +
                            " excise_details='" + values.excise_details + "'," +
                            " ifsc_code='" + values.ifsc_code + "'," +
                            " pan_number='" + values.pan_number + "'," +
                            " rtgs_code='" + values.rtgs_code + "'," +
                            " servicetax_number='" + values.servicetax_number + "'," +
                            " tin_number='" + values.tin_number + "'" +
                            " WHERE vendor_gid = '" + vendor_gid + "'";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Vendor Additional Information Updated Successfully";

                        }
                        else
                        {
                            values.status = false;
                            values.message = "Error While Updating Vendor ";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while adding additional information in vendor!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaVendorImportExcel(HttpRequest httpRequest, string user_gid, result objResult, product_list values)
        {
            try
            {
                string lscompany_code;
                string excelRange, endRange;
                int rowCount, columnCount;


                int insertCount = 0;
                HttpFileCollection httpFileCollection;
                DataTable dt = null;
                string lspath, lsfilePath;

                msSQL = " select company_code from adm_mst_tcompany";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                // Create Directory
                lsfilePath = ConfigurationManager.AppSettings["importexcelfile1"];

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
                string filename = Path.GetFileNameWithoutExtension(FileExtension).ToLower();
                lsfile_gid = lsfile_gid + FileExtension;
                FileInfo fileinfo = new FileInfo(lsfilePath);
                Stream ls_readStream;
                ls_readStream = httpPostedFile.InputStream;
                MemoryStream ms = new MemoryStream();
                ls_readStream.CopyTo(ms);

                msSQL = "insert into adm_log_tuplaod( " +
                        " upload_gid ," +
                        " file_name , " +
                        " file_extension , " +
                        " upload_date , " +
                        " upload_by  " +
                        " ) values ( " +
                        " '" + msdocument_gid + "'," +
                        " '" + filename + "'," +
                        " '" + FileExtension + "'," +
                        " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        " '" + user_gid + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                //path creation        
                lspath = lsfilePath + "/";
                FileStream file = new FileStream(lspath + lsfile_gid, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                string status;
                status = objcmnfunctions.uploadFile(lsfilePath, FileExtension);
                file.Close();
                ms.Close();

                try
                {
                    DataTable dataTable = new DataTable();
                    int totalSheet = 1;
                    string connectionString = string.Empty;
                    string fileExtension = Path.GetExtension(lspath);

                    lsfilePath = @"" + lsfilePath.Replace("/", "\\") + "\\" + lsfile_gid + "";

                    string correctedPath = Regex.Replace(lsfilePath, @"\\+", @"\");

                    //dt_datatable = objcmnfunctions.ExcelToDataTable(correctedPath, );

                    try
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + correctedPath + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0';";
                    }
                    catch (Exception ex)
                    {

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

                                importcount = 0;

                                char[] charsToReplace = { '*', ' ', '/', '@', '$', '#', '!', '^', '%', '(', ')', '\'' };

                                // Get the header names from the CSV file
                                List<string> headers = dataTable.Columns.Cast<DataColumn>().Select(column =>
                                    string.Join("", column.ColumnName.Split(charsToReplace, StringSplitOptions.RemoveEmptyEntries))
                                        .ToLower()).ToList();
                                if (dataTable.Rows.Count == 0)
                                {
                                    values.message = "No data found ";
                                    values.status = false;
                                    return;
                                }
                                int vendorcode = headers.IndexOf("vendorcode");
                                int vendorcompanyname = headers.IndexOf("vendorcompanyname");
                                int contactpersonname = headers.IndexOf("contactpersonname");
                                int contacttelephonenumber = headers.IndexOf("contacttelephonenumber");
                                int emailid = headers.IndexOf("emailid");

                                int faxno = headers.IndexOf("fax");
                                int address_1 = headers.IndexOf("addressline1");
                                int address_2 = headers.IndexOf("addressline2");
                                int city1 = headers.IndexOf("city");
                                int state1 = headers.IndexOf("state");
                                int postalcode = headers.IndexOf("pincode");
                                int countryname = headers.IndexOf("country");
                                int tax1 = headers.IndexOf("tax");
                                foreach (DataRow dt_product in dataTable.Rows)
                                {                                   

                                    vendor_code = dt_product["Vendor Code"].ToString().Trim();
                                    string vendor_companyname = dt_product["Vendor Company Name"]?.ToString().Trim();
                                    string contactperson_name = dt_product["Contact Person Name"]?.ToString().Trim();
                                    string contact_telephonenumber = dt_product["Contact  Number"]?.ToString().Trim();
                                    string email_id = dt_product["Contact Email"]?.ToString().Trim();
                                    string billing_mail = dt_product["Billing Email"]?.ToString().Trim();
                                    string tax_no = dt_product["Tax Number"]?.ToString().Trim();
                                    string tax_segment = dt_product["Tax Segment"]?.ToString().Trim();
                                    string avrge_lead_time = dt_product["Average Lead Time"]?.ToString().Trim();
                                    string address1 = dt_product["Address I"]?.ToString().Trim();
                                    string address2 = dt_product["Address II"]?.ToString().Trim();
                                    string city = dt_product["City"]?.ToString().Trim();
                                    string postal_code = dt_product["Post Code"]?.ToString().Trim();
                                    string country_name = dt_product["Country"]?.ToString().Trim();
                                    string region = dt_product["Region"]?.ToString().Trim();
                                    string currency = dt_product["Currency"]?.ToString().Trim();
                                    string payment_term = dt_product["Payment Terms"]?.ToString().Trim();
                                    string credit_days = dt_product["Credit Days"]?.ToString().Trim();

                                    msGetGid1 = objcmnfunctions.GetMasterGID("PVRR");
                                    msGetGid2 = objcmnfunctions.GetMasterGID("PVRM");
                                    msGetGid = objcmnfunctions.GetMasterGID("SADM");

                                    if (string.IsNullOrEmpty(vendor_companyname)
                                    || string.IsNullOrEmpty(contactperson_name) || string.IsNullOrEmpty(contact_telephonenumber)
                                    || string.IsNullOrEmpty(email_id) || string.IsNullOrEmpty(tax_segment)
                                    || string.IsNullOrEmpty(address1) || string.IsNullOrEmpty(country_name)
                                    || string.IsNullOrEmpty(currency))
                                    {
                                        msGETtmpGID = objcmnfunctions.GetMasterGID("VENT");
                                        msSQL = "insert into adm_tmp_tvendorregister (" +
                                            " tmpvendor_gid, " +
                                            " upload_gid, " +
                                            " vendor_code, " +
                                            " vendor_companyname, " +
                                            " contactperson_name, " +
                                            " contact_telephonenumber, " +
                                            " email_id, " +
                                            " billing_mail, " +
                                            " tax_no, " +
                                            " tax_segment, " +
                                            " avrge_lead_time, " +
                                            " address1, " +
                                            " address2, " +
                                            " city, " +
                                            " postal_code, " +
                                            " country_name, " +
                                            " region, " +
                                            " currency, " +
                                            " credit_days, " +
                                            " remarks, " +
                                            " payment_term, " +
                                            " created_date, " +
                                            " created_by " +
                                            " ) values (" +
                                            "'" + msGETtmpGID + "'," +
                                            "'" + msdocument_gid + "'," +
                                            "'" + vendor_code + "'," +
                                            "'" + vendor_companyname + "'," +
                                            "'" + contactperson_name + "'," +
                                            "'" + contact_telephonenumber + "'," +
                                            "'" + email_id + "'," +
                                            "'" + billing_mail + "'," +
                                            "'" + tax_no + "'," +
                                            "'" + tax_segment + "'," +
                                            "'" + avrge_lead_time + "'," +
                                            "'" + address1 + "'," +
                                            "'" + address2 + "'," +
                                            "'" + city + "'," +
                                            "'" + postal_code + "'," +
                                            "'" + country_name + "'," +
                                            "'" + region + "'," +
                                            "'" + currency + "'," +
                                            "'" + credit_days + "'," +
                                            "'Kindly fill the missed data'," +
                                            "'" + payment_term + "'," +
                                            "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            "'" + user_gid + "')"
                                            ;
                                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                    }
                                    else
                                    {
                                        msSQL = "select vendor_companyname from acp_mst_tvendor where vendor_companyname='" + vendor_companyname + "'";
                                        lsvendor_companyname = objdbconn.GetExecuteScalar(msSQL);
                                        if (lsvendor_companyname != "")
                                        {
                                            msGETtmpGID = objcmnfunctions.GetMasterGID("VENT");
                                            msSQL = "insert into adm_tmp_tvendorregister (" +
                                                " tmpvendor_gid, " +
                                                " upload_gid, " +
                                                " vendor_code, " +
                                                " vendor_companyname, " +
                                                " contactperson_name, " +
                                                " contact_telephonenumber, " +
                                                " email_id, " +
                                                " billing_mail, " +
                                                " tax_no, " +
                                                " tax_segment, " +
                                                " avrge_lead_time, " +
                                                " address1, " +
                                                " address2, " +
                                                " city, " +
                                                " postal_code, " +
                                                " country_name, " +
                                                " region, " +
                                                " currency, " +
                                                " credit_days, " +
                                                " remarks, " +
                                                " payment_term, " +
                                                " created_date, " +
                                                " created_by " +
                                                " ) values (" +
                                                "'" + msGETtmpGID + "'," +
                                                "'" + msdocument_gid + "'," +
                                                "'" + vendor_code + "'," +
                                                "'" + vendor_companyname + "'," +
                                                "'" + contactperson_name + "'," +
                                                "'" + contact_telephonenumber + "'," +
                                                "'" + email_id + "'," +
                                                "'" + billing_mail + "'," +
                                                "'" + tax_no + "'," +
                                                "'" + tax_segment + "'," +
                                                "'" + avrge_lead_time + "'," +
                                                "'" + address1 + "'," +
                                                "'" + address2 + "'," +
                                                "'" + city + "'," +
                                                "'" + postal_code + "'," +
                                                "'" + country_name + "'," +
                                                "'" + region + "'," +
                                                "'" + currency + "'," +
                                                "'" + credit_days + "'," +
                                                "'This `"+ vendor_companyname + "` already exist, so kindly check previous record'," +
                                                "'" + payment_term + "'," +
                                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                "'" + user_gid + "')"
                                                ;
                                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                        }
                                        else
                                        {
                                            if (vendor_code == "")
                                            {
                                                msSQL = " Select sequence_curval from adm_mst_tsequence where sequence_code ='PVRR' order by finyear desc limit 0,1 ";
                                                string lsCode = objdbconn.GetExecuteScalar(msSQL);

                                                vendor_code = "VC" + "00" + lsCode;
                                            }
                                           
                                            msSQL = "select region_gid from crm_mst_Tregion where region_name='"+ region +"'";
                                            region_gid = objdbconn.GetExecuteScalar(msSQL);
                                            
                                            msSQL = "select taxsegment_gid from acp_mst_ttaxsegment where taxsegment_name='" + tax_segment +"'";
                                            taxsegment_gid = objdbconn.GetExecuteScalar(msSQL);
                                            
                                            msSQL = "select currencyexchange_gid from crm_trn_tcurrencyexchange where currency_code='" + currency +"'";
                                            currencyexchange_gid = objdbconn.GetExecuteScalar(msSQL);
                                            
                                            msSQL = "select country_gid from  adm_mst_tcountry where country_name='" + country_name +"'";
                                            country_gid = objdbconn.GetExecuteScalar(msSQL);

                                           // msSQL = " insert into acp_mst_tvendorregister (" +
                                           //" vendorregister_gid, " +
                                           //" vendor_code, " +
                                           //" vendor_companyname, " +
                                           //" contactperson_name, " +
                                           //" contact_telephonenumber, " +
                                           //" email_id," +
                                           //" active_flag," +
                                           //" tin_number," +
                                           //" excise_details," +
                                           //" pan_number," +
                                           //" servicetax_number," +
                                           //" cst_number," +
                                           //" bank_details," +
                                           //" ifsc_code," +
                                           //" region_gid, " +
                                           //" average_leadtime, " +
                                           //" billing_email, " +
                                           //" credit_days, " +
                                           //" payment_terms, " +
                                           //" tax_number, " +
                                           //" rtgs_code, " +
                                           //" vendor_status, " +
                                           //" address_gid," +
                                           //" tax_gid," +
                                           //" taxsegment_gid," +
                                           //" currencyexchange_gid) " +
                                           //" values (" +
                                           //"'" + msGetGid1 + "'," +
                                           //"'" + vendor_code + "'," +
                                           //"'" + vendor_companyname + "'," +
                                           //"'" + contactperson_name + "'," +
                                           //"'" + contact_telephonenumber + "'," +
                                           //"'" + email_id + "'," +
                                           //"'Y'," +
                                           //"'" + tin_number + "'," +
                                           //"'" + excise_details + "'," +
                                           //"'" + pan_number + "'," +
                                           //"'" + servicetax_numbers + "'," +
                                           //"'" + cst_number + "'," +
                                           //"'" + bank_details + "'," +
                                           //"'" + ifsc_code + "'," +
                                           //"'" + region_gid + "'," +
                                           //"'" + avrge_lead_time + "'," +
                                           //"'" + billing_mail + "'," +
                                           //"'" + credit_days + "'," +
                                           //"'" + payment_term + "'," +
                                           //"'" + tax_no + "'," +
                                           //"'" + tax_no + "'," +
                                           //"'Vendor Approved'," +
                                           //"'" + msGetGid + "'," +
                                           //"'" + tax_gid + "'," +
                                           //"'" + taxsegment_gid + "'," +
                                           //"'" + currencyexchange_gid + "')";
                                           // mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                           // if (mnResult == 1)
                                           // {
                                                msSQL = " insert into acp_mst_tvendor (" +
                                                        " vendor_gid, " +
                                                    " vendor_code, " +
                                                    " vendor_companyname, " +
                                                    " contactperson_name, " +
                                                    " contact_telephonenumber, " +
                                                    " email_id," +
                                                    " active_flag," +
                                                    " tin_number," +
                                                    " excise_details," +
                                                    " pan_number," +
                                                    " servicetax_number," +
                                                    " cst_number," +
                                                    " bank_details," +
                                                    " ifsc_code," +
                                                    " region_gid, " +
                                                    " average_leadtime, " +
                                                    " billing_email, " +
                                                    " credit_days, " +
                                                    " payment_terms, " +
                                                    " tax_number, " +
                                                    " rtgs_code, " +
                                                    " autopo, " +
                                                    " address_gid," +
                                                    " vendorregister_gid," +
                                                    " tax_gid," +
                                                    " taxsegment_gid," +
                                                    " currencyexchange_gid) " +
                                                    " values (" +
                                                    "'" + msGetGid2 + "'," +
                                                    "'" + vendor_code + "'," +
                                                    "'" + vendor_companyname + "'," +
                                                    "'" + contactperson_name + "'," +
                                                    "'" + contact_telephonenumber + "'," +
                                                    "'" + email_id + "'," +
                                                    "'Y'," +
                                                    "'" + tin_number + "'," +
                                                    "'" + excise_details + "'," +
                                                    "'" + pan_number + "'," +
                                                    "'" + servicetax_numbers + "'," +
                                                    "'" + cst_number + "'," +
                                                    "'" + bank_details + "'," +
                                                    "'" + ifsc_code + "'," +
                                                    "'" + region_gid + "'," +
                                                    "'" + avrge_lead_time + "'," +
                                                    "'" + billing_mail + "'," +
                                                    "'" + credit_days + "'," +
                                                    "'" + payment_term + "'," +
                                                    "'" + tax_no + "'," +
                                                    "''," +
                                                    "'N'," +
                                                    "'" + msGetGid + "'," +
                                                    "'" + msGetGid1 + "'," +
                                                    "'" + tax_gid + "'," +
                                                    "'" + taxsegment_gid + "'," +
                                                    "'" + currencyexchange_gid + "')";
                                                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                if (mnResult == 1)
                                                {
                                                    msSQL = " insert into adm_mst_taddress " +
                                                           " (address_gid, " +
                                                           " address1, " +
                                                           " address2, " +
                                                           " city, " +
                                                           " state, " +
                                                           " postal_code, " +
                                                           " country_gid, " +
                                                           " fax ) " +
                                                           " values (" +
                                                           "'" + msGetGid + "'," +
                                                           "'" + address1 + "'," +
                                                           "'" + address2 + "'," +
                                                           "'" + city + "'," +
                                                           "'" + state1 + "'," +
                                                           "'" + postal_code + "'," +
                                                           "'" + country_gid + "'," +
                                                           "'" + fax + "')";
                                                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                                                }
                                            }
                                        }
                                    }
                                }

                                msSQL = "select * from adm_tmp_tvendorregister";
                                dt = objdbconn.GetDataTable(msSQL);
                                if (dt.Rows.Count > 0)
                                {
                                    values.status = false;
                                    values.message = "Some vendors have not been added, so please check the log.";
                                }
                                else
                                {
                                    values.status = true;
                                    values.message = "Excel Uploaded Successfully";
                                }
                            }
                        }
                    //}
                }

                catch (Exception ex)
                {
                    objResult.message = ex.Message.ToString();
                }                
            }

            catch (Exception ex)
            {
                values.message = "Exception occured while adding vendor template";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
                ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
                msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
                DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }
        public void DaGetVendorReportExport(MdlPmrMstVendorRegister values)
        {
            try
            {

                msSQL = " SELECT vendor_code as VendorCode, " +
                        " vendor_companyname as VendorCompany, contactperson_name as ContactPerson, " +
                        " contact_telephonenumber as ContactNumber,vendor_status as Status " +
                        " from acp_mst_tvendor " +
                        " Order by  vendor_gid desc  ";
                dt_datatable = objdbconn.GetDataTable(msSQL);

                string lscompany_code = string.Empty;
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("vendor Report");
                try
                {
                    msSQL = " select company_code from adm_mst_tcompany";

                    lscompany_code = objdbconn.GetExecuteScalar(msSQL);
                    string lspath = ConfigurationManager.AppSettings["exportexcelfile"] + "/vendor/export" + "/" + lscompany_code + "/" + "Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                    //values.lspath = ConfigurationManager.AppSettings["file_path"] + "/erp_documents" + "/" + lscompany_code + "/" + "SDC/TestReport/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                    {
                        if ((!System.IO.Directory.Exists(lspath)))
                            System.IO.Directory.CreateDirectory(lspath);
                    }

                    string lsname2 = "vendor_Report" + DateTime.Now.ToString("(dd-MM-yyyy HH-mm-ss)") + ".xlsx";
                    string lspath1 = ConfigurationManager.AppSettings["exportexcelfile"] + "/vendor/export" + "/" + lscompany_code + "/" + "Export/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + lsname2;

                    workSheet.Cells[1, 1].LoadFromDataTable(dt_datatable, true);
                    FileInfo file = new FileInfo(lspath1);
                    using (var range = workSheet.Cells[1, 1, 1, 8])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        range.Style.Font.Color.SetColor(Color.White);
                    }
                    excel.SaveAs(file);

                    var getModuleList = new List<vendorexport_list>();
                    if (dt_datatable.Rows.Count != 0)
                    {

                        getModuleList.Add(new vendorexport_list
                        {
                            lsname2 = lsname2,
                            lspath1 = lspath1,
                        });
                        values.vendorexport_list = getModuleList;

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
                values.message = "Exception occured while exporting vendor!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaPostdocumentImage(HttpRequest httpRequest, result objResult, string user_gid)
        {
            try
            {


                HttpFileCollection httpFileCollection;
                string lsfilepath = string.Empty;
                string lsdocument_gid = string.Empty;
                MemoryStream ms_stream = new MemoryStream();
                string documenttype_gid = string.Empty;
                string lscompany_code = string.Empty;
                HttpPostedFile httpPostedFile;

                string lspath;
                string msGetGid;

                msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
                lscompany_code = objdbconn.GetExecuteScalar(msSQL);

                string vendorregister_gid = httpRequest.Form[1];
                string documenttype_name = httpRequest.Form[0];

                msSQL = "select documenttype_name  from  acp_mst_tvendordocumenttype  where documenttype_gid ='" + documenttype_name + "'";
                documenttype_gid = objdbconn.GetExecuteScalar(msSQL);

                MemoryStream ms = new MemoryStream();

                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        string lsfirstdocument_filepath = string.Empty;
                        httpFileCollection = httpRequest.Files;
                        for (int i = 0; i < httpFileCollection.Count; i++)
                        {
                            string msdocument_gid = objcmnfunctions.GetMasterGID("UPLF");
                            httpPostedFile = httpFileCollection[i];
                            string FileExtension = httpPostedFile.FileName;
                            //string lsfile_gid = msdocument_gid + FileExtension;
                            string lsfile_gid = msdocument_gid;
                            string lscompany_document_flag = string.Empty;
                            FileExtension = Path.GetExtension(FileExtension).ToLower();
                            lsfile_gid = lsfile_gid + FileExtension;
                            Stream ls_readStream;
                            ls_readStream = httpPostedFile.InputStream;
                            ls_readStream.CopyTo(ms);

                            bool status1;
                            //status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/Product/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                            //ms.Close();

                            //final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/Product/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                            //msSQL = "update pmr_mst_tproduct set " +
                            //" product_image='" + ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +'&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +'&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"] + "'" +
                            // " where product_gid='" + product_gid + "'";


                            status1 = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "PURCHASE/Product/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                            ms.Close();

                            final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "PURCHASE/Product/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                            string httpsUrl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                       '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                       '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];


                            maGetGID1 = objcmnfunctions.GetMasterGID("SVDM");
                            msSQL = " insert into acp_mst_tvendorregisterdocument (" +
                                " document_gid, " +
                                " vendorregister_gid, " +
                                " document_type, " +
                                " document_name, " +
                                " created_date, " +
                                " file_path, " +
                                " created_by ) " +
                                " values (" +
                                "'" + maGetGID1 + "', " +
                                "'" + vendorregister_gid + "'," +
                                "'" + documenttype_name + "'," +
                                 "'" + documenttype_gid + "'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                "'" + httpsUrl + "'," +
                                "'" + user_gid + "')";
                            mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                        }
                    }


                    if (mnResult != 0)
                    {
                        objResult.status = true;
                        objResult.message = "Document Added Successfully !!";
                    }
                    else
                    {
                        objResult.status = false;
                        objResult.message = "Error While Adding Document !!";
                    }

                }

                catch (Exception ex)
                {
                    objResult.message = ex.Message.ToString();
                }
                //return true;
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while adding document image!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
            ex.Message.ToString() + "***********" + objResult.message.ToString() + "*****Query****" +
            msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
            DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }


        }
        public void DaGetDocumentdtl(string vendorregister_gid, MdlPmrMstVendorRegister values)
        {
            try
            {

                msSQL = " select a.document_gid,a.document_type,a.document_name,date_format(a.created_date,'%d-%m-%Y')as created_date,a.file_path," +
                        " a.created_by,b.user_firstname from" +
                        " acp_mst_tvendorregisterdocument a" +
                        " left join adm_mst_tuser b on b.user_gid=a.created_by" +
                        " where vendorregister_gid='" + vendorregister_gid + "' " +
                        " group by a.document_gid order by DATE(a.created_date) asc, created_date desc  ";

                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<GetDocument_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new GetDocument_list
                        {


                            created_date = dt["created_date"].ToString(),
                            document_gid = dt["document_gid"].ToString(),
                            document_type = dt["document_type"].ToString(),
                            document_name = dt["document_name"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            user_firstname = dt["user_firstname"].ToString(),
                            file_path = dt["file_path"].ToString(),


                        });
                        values.GetDocument_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting document details!";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
               $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" +
           ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
           msSQL + "*******Apiref********", "ErrorLog/Purchase/" + "Log" +
           DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetTaxSegmentSummary(MdlPmrMstVendorRegister values)
        {
            try
            {

                msSQL = "select taxsegment_gid,taxsegment_name from acp_mst_ttaxsegment where reference_type='Vendor' and active_flag= 'Y' ";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<TaxSegmentSummary_list>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new TaxSegmentSummary_list
                        {
                            taxsegment_gid = dt["taxsegment_gid"].ToString(),
                            taxsegment_name = dt["taxsegment_name"].ToString(),


                        });
                        values.TaxSegmentSummary_list = getModuleList;
                    }
                }
                dt_datatable.Dispose();


            }
            catch (Exception ex)
            {
                values.message = "Exception occured while loading Tax Summary !";
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }

        }
        public void DaGetImportExcelLog(MdlPmrMstVendorRegister values)
        {
            msSQL = "select count(b.tmpvendor_gid) as missed_count, date_format(a.upload_date,'%d-%m-%Y') as upload_date, " +
                " concat(c.user_firstname ,' ', c.user_lastname) as user_name, a.upload_by, a.upload_gid from adm_log_tuplaod a " +
                " left join adm_tmp_tvendorregister b on a.upload_gid=b.upload_gid" +
                " left join adm_mst_tuser c on a.upload_by=c.user_gid where b.tmpvendor_gid is not null" +
                " group by a.upload_gid";
            var GetExcelLog = new List<GetExcelLog_list>();
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetExcelLog.Add(new GetExcelLog_list
                    {
                        missed_count = dt["missed_count"].ToString(),
                        upload_date = dt["upload_date"].ToString(),
                        user_name = dt["user_name"].ToString(),
                        upload_by = dt["upload_by"].ToString(),
                        upload_gid = dt["upload_gid"].ToString(),
                    });
                  values.GetExcelLog_list = GetExcelLog;
                }
            }
        }
        public void DaGetExcelLogDetails(string upload_gid, MdlPmrMstVendorRegister values)
        {
            msSQL = " select * from adm_tmp_tvendorregister where upload_gid='" + upload_gid + "'";
            var GetExcelLogDetails = new List<GetExcelLogDetails_list>();
            dt_datatable = objdbconn.GetDataTable(msSQL);
            if (dt_datatable.Rows.Count > 0)
            {
                foreach (DataRow dt in dt_datatable.Rows)
                {
                    GetExcelLogDetails.Add(new GetExcelLogDetails_list
                    {
                        tmpvendor_gid = dt["tmpvendor_gid"].ToString(),
                        upload_gid = dt["upload_gid"].ToString(),
                        vendor_code = dt["vendor_code"].ToString(),
                        vendor_companyname = dt["vendor_companyname"].ToString(),
                        contactperson_name = dt["contactperson_name"].ToString(),
                        contact_telephonenumber = dt["contact_telephonenumber"].ToString(),
                        email_id = dt["email_id"].ToString(),
                        billing_mail = dt["billing_mail"].ToString(),
                        tax_no = dt["tax_no"].ToString(),
                        tax_segment = dt["tax_segment"].ToString(),
                        avrge_lead_time = dt["avrge_lead_time"].ToString(),
                        address1 = dt["address1"].ToString(),
                        address2 = dt["address2"].ToString(),
                        city = dt["city"].ToString(),
                        postal_code = dt["postal_code"].ToString(),
                        country_name = dt["country_name"].ToString(),
                        region = dt["region"].ToString(),
                        payment_term = dt["payment_term"].ToString(),
                        credit_days = dt["credit_days"].ToString(),
                        currency = dt["currency"].ToString(),
                        remarks = dt["remarks"].ToString(),
                    });
                    values.GetExcelLogDetails_list = GetExcelLogDetails;
                }
            }
        }
    }
}