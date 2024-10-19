using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using ems.system.Models;
using ems.utilities.Functions;
using System.Web.UI.WebControls;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.IO;
using System.Configuration;

namespace ems.system.DataAccess
{
    public class DaCompany
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        string msSQL = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable, objtbl;
        int mnresult,mnresult1;

        public void dagetcompany(mdlcompany values)
        {
            try
            {
                msSQL = "Select company_gid,company_code,company_name,company_address,salary_startdate,company_phone,company_website," +
                    " company_mail,contact_person,manufacturer_licence,auth_code,fax, " +
                    " pop_server,pop_port,pop_username,pop_password,currency_code,company_logo_path," +
                    " welcome_logo, authorised_sign,sequence_reset,country_name, file_name,country_gid from adm_mst_tcompany";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<companylists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new companylists
                        {
                            company_gid = dt["company_gid"].ToString(),
                            company_code = dt["company_code"].ToString(),
                            company_name = dt["company_name"].ToString(),
                            company_address = dt["company_address"].ToString(),
                            salary_startdate = dt["salary_startdate"].ToString(),
                            company_phone = dt["company_phone"].ToString(),
                            company_website = dt["company_website"].ToString(),
                            company_mail = dt["company_mail"].ToString(),
                            contact_person = dt["contact_person"].ToString(),
                            manufacturer_licence = dt["manufacturer_licence"].ToString(),
                            auth_code = dt["auth_code"].ToString(),
                            fax = dt["fax"].ToString(),
                            pop_server = dt["pop_server"].ToString(),
                            pop_port = dt["pop_port"].ToString(),
                            pop_username = dt["pop_username"].ToString(),
                            pop_password = dt["pop_password"].ToString(),
                            currency_code = dt["currency_code"].ToString(),
                            company_logo_path = dt["company_logo_path"].ToString(),
                            welcome_logo = dt["welcome_logo"].ToString(),
                            authorised_sign = dt["authorised_sign"].ToString(),
                            sequence_reset = dt["sequence_reset"].ToString(),
                            country_name = dt["country_name"].ToString(),
                            country_gid = dt["country_gid"].ToString(),
                            file_name = dt["file_name"].ToString(),
                        });
                        values.companylists = getModuleList;
                    }
                }
                dt_datatable.Dispose();

            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }



        public void DaGetCountrydropdown(mdlcompany values)
        {
            try
            {

                msSQL = "Select country_name,country_gid from adm_mst_tcountry";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<countrylists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new countrylists
                        {
                            country_name = dt["country_name"].ToString(),
                            country_gid = dt["country_gid"].ToString(),
                        });
                        values.countrylists = getModuleList;

                    }

                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }






        }

        public void DaGetCurrencydropdown(mdlcompany values)
        {
            try
            {
                msSQL = "select currencyexchange_gid,currency_code" +
                       " from crm_trn_tcurrencyexchange";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getModuleList = new List<currencylists>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getModuleList.Add(new currencylists
                        {
                            currencyexchange_gid = dt["currencyexchange_gid"].ToString(),
                            currency_code = dt["currency_code"].ToString()
                        });
                        values.currencylists = getModuleList;
                    }
                }
                dt_datatable.Dispose();
            }
            catch (Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }

        public void DaPostCompany(companylists values)
        {
            try
            {

                string company_name = values.company_name.Replace("'", "\\'");
                string company_address = values.company_address.Replace("'", "\\'");


                msSQL = "update adm_mst_tcompany set " +
                           "company_name='" + company_name + "'," +
                           "company_address='" + company_address + "'," +
                           "file_name='" + values.file_name + "'," +
                           "company_code='" + values.company_code + "'," +
                           "company_phone='" + values.company_phone + "'," +
                           "company_website = '" + values.company_website + "'," +
                           "contact_person = '" + values.contact_person.Replace("'", "\\'") + "'," +
                           "company_mail = '" + values.company_mail + "'," +
                           "country_name = '" + values.country_name + "'," +
                           "currency_code='" + values.currency_code + "'" +
                           " where company_gid='1' ";
                mnresult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if(mnresult != 0)
                {
                    values.status = true;
                    values.message = "Company Details Updated Successfully";

                }
                else
                {
                    values.status = false;
                    values.message = "Company Details Updated Unsuccessfully";
                }

            }
            catch(Exception ex)
            {
                values.status = false;

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() +
                 "***********" + ex.Message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/SYS/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }
        }
        public void DaUpdatedCompanylogo(HttpRequest httpRequest, result objResult, string user_gid)
        {
            HttpFileCollection httpFileCollection;
            string lsfilepath = string.Empty;
            string lsdocument_gid = string.Empty;
            MemoryStream ms_stream = new MemoryStream();
            string document_gid = string.Empty;
            string lscompany_code = string.Empty;
            string lsfile_name = string.Empty;
            HttpPostedFile httpPostedFile;
            string lspath; 
            string msGetGid;


            msSQL = " SELECT  a.company_code  FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            string company_code = httpRequest.Form[0];
            string company_name = httpRequest.Form[1];
            string company_website = httpRequest.Form[2];
            string contact_person = httpRequest.Form[3];
            string company_phone = httpRequest.Form[4];
            string company_mail = httpRequest.Form[5];
            string company_address = httpRequest.Form[6];
            string country_name = httpRequest.Form[7];
            string currency_code = httpRequest.Form[8];
            //string country = httpRequest.Form[7];
            //string currency = httpRequest.Form[8];

            MemoryStream ms = new MemoryStream();
            lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "System" + "/" + "Companydetails" + "/";

            {
                if ((!System.IO.Directory.Exists(lspath)))
                    System.IO.Directory.CreateDirectory(lspath);
            }
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
                        string FileExtension_name = httpPostedFile.FileName;
                        //string lsfile_gid = msdocument_gid + FileExtension;
                        string lsfile_gid = msdocument_gid;
                        string lscompany_document_flag = string.Empty;
                        string FileExtension = Path.GetExtension(FileExtension_name).ToLower();
                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        ls_readStream.CopyTo(ms);

                        //lspath = ConfigurationManager.AppSettings["imgfile_path"] + "/erpdocument" + "/" + "/" + lscompany_code + "/" + "Company/" + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month;
                        //string status;
                        //status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        ////string local_path = "E:/Angular15/AngularUI/src";
                        //ms.Close();
                        ////lspath = "assets/media/images/erpdocument" + "/" + "/" + lscompany_code + "/" + "Company/" + "/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

                        //string final_path = lspath + msdocument_gid + FileExtension;

                        lspath = ConfigurationManager.AppSettings["Doc_upload_file"] + "/erp_documents" + "/" + lscompany_code + "/" + "System" + "/" + "Companydetails" + "/";
                        string status;
                        status = objcmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                        ms.Close();
                        string lspath1 = "erp_documents" + "/" + lscompany_code + "/" + "System" + "/" + "Companydetails" + "/";
                        string final_path = lspath1 + msdocument_gid + FileExtension;

                        msSQL = "update adm_mst_tcompany set " +
                            "company_code='" + company_code + "'," +
                            "file_name='" + FileExtension_name + "'," +
                            "company_name='" + company_name.Replace("'", "\\'") + "'," +
                            "company_address='" + company_address.Replace("'", "\\'") + "'," +
                            "company_website = '" + company_website + "'," +
                            "company_phone = '" + company_phone + "'," +
                            "company_mail = '" + company_mail + "'," +
                            "contact_person = '" + contact_person.Replace("'", "\\'") + "'," +
                            "authorised_sign='" + final_path + "'," +
                            "country_name = '" + country_name + "'," +
                            "currency_code='" + currency_code + "'," +
                            "updated_by = '" + user_gid + "'," +
                            "updated_date = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where company_gid='1' ";
                        mnresult1 = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }
                }

                if (mnresult1 != 0)
                {
                    objResult.status = true;
                    objResult.message = "Company Updated Successfully !!";
                }
                else
                {
                    objResult.status = false;
                    objResult.message = "Error While Updating Company !!";
                }
            }

            catch (Exception ex)
            {
                objResult.message = ex.ToString();
            }
            //return true;

        }


    }
}