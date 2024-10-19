using ems.law.Models;
using ems.utilities.Functions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Security;
using System.Web;
using System.Xml.Linq;
using System.Web.Http.Filters;
using static System.Net.WebRequestMethods;
using System.Diagnostics.Eventing.Reader;


namespace ems.law.DataAccess
{
    public class DaCaseManagement
    {
       
        string msSQL =  string.Empty;
        DataTable dt_caseTable;
        dbconn conn = new dbconn();
        cmnfunctions cmnfunctions = new cmnfunctions();
        Fnazurestorage objcmnstorage = new Fnazurestorage();
        string institute_name, client_name, bravery_name, casetype_name, case_no, case_date,
            FileExtension, doc_name, filepath_gid, lscompany_code, lspath2, lspath1, return_path
            , msGetUPGID, msCEGetGid, institute_gid, casetype_gid, get_localFilePath, case_gid,
            doc_name1, combine_path;
        string lspath, msdocument_gid, file_path = string.Empty;
        int mnResult;
        int i;
             
        public void DaGetCaseManagementSummary(MdlCaseManagement values)
        {
            try
            {
                msSQL = " select case_gid, casetype_gid,remarks as case_remarks, casetype_name, institute_gid,institute_name, date_format(case_date,'%d-%m-%y') as case_date, case_no, " +
                    "client_name, bravery_name, date_format(created_date,'%d-%m-%y') as created_date, created_by from lgl_trn_tcaseinformation";
                dt_caseTable = conn.GetDataTable(msSQL);
                var GetCaseMgmtSummary = new List<GetCaseManagementSummart_list>();
                if (dt_caseTable.Rows.Count > 0)
                {
                    foreach (DataRow CTS in dt_caseTable.Rows)
                    {
                        GetCaseMgmtSummary.Add(new GetCaseManagementSummart_list
                        {
                            case_gid = CTS["case_gid"].ToString(),
                            casetype_gid = CTS["casetype_gid"].ToString(),
                            casetype_name = CTS["casetype_name"].ToString(),
                            institute_gid = CTS["institute_gid"].ToString(),
                            institute_name = CTS["institute_name"].ToString(),
                            case_date = CTS["case_date"].ToString(),
                            case_no = CTS["case_no"].ToString(),
                            client_name = CTS["client_name"].ToString(),
                            case_remarks = CTS["case_remarks"].ToString(),
                            created_date = CTS["created_date"].ToString(),
                            created_by = CTS["created_by"].ToString(),
                        });
                        values.GetCaseManagementSummart_list = GetCaseMgmtSummary;
                    }
                }
            }
            catch(Exception ex) 
            {
                values.message = "Exception occured while getting whatsapp summary!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }
        }

 // ============================================ add evnt start =============================================//
        public void DaGetCasetype(MdlCaseManagement values)
        {
            try 
            {
                msSQL = " select casetype_code, casetype_gid, casetype_name from lgl_mst_tcasetype";
                dt_caseTable = conn.GetDataTable(msSQL);
                var GetCaseType = new List<Getcasetype_list>();
                if (dt_caseTable.Rows.Count > 0)
                {
                    foreach (DataRow CT in dt_caseTable.Rows)
                    {
                        GetCaseType.Add(new Getcasetype_list
                        {
                            casetype_code = CT["casetype_code"].ToString(),
                            casetype_gid = CT["casetype_gid"].ToString(),
                            casetype_name = CT["casetype_name"].ToString(),
                        });
                        values.Getcasetype_list = GetCaseType;
                    }
                }
            }
            catch(Exception ex) {
                values.message = "Exception occured while getting whatsapp summary!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + 
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }

        }

        public void DaGetCaseInstitute(MdlCaseManagement values)
        {
            try 
            {
                msSQL = " select institute_code, institute_gid, institute_name from law_mst_tinstitute";
                dt_caseTable = conn.GetDataTable(msSQL);
                var GetCaseInstitute = new List<Getcaseinstitute_list>();
                if (dt_caseTable.Rows.Count > 0)
                {
                    foreach (DataRow CI in dt_caseTable.Rows)
                    {
                        GetCaseInstitute.Add(new Getcaseinstitute_list
                        {
                            institute_code = CI["institute_code"].ToString(),
                            institute_gid = CI["institute_gid"].ToString(),
                            institute_name = CI["institute_name"].ToString(),
                        });
                        values.Getcaseinstitute_list = GetCaseInstitute;
                    }
                }
            }
            catch(Exception ex) {
                values.message = "Exception occured while getting whatsapp summary!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + 
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }

        }

        public void PostCaseInformation(string user_gid, GetCaseManagementSummart_list values)
        {
            try 
            {                             
                msSQL = " select institute_name from law_mst_tinstitute where institute_gid='" + values.institute_name + "'";
                institute_gid = conn.GetExecuteScalar(msSQL);

                msSQL = " select casetype_name from lgl_mst_tcasetype where casetype_gid ='" + values.casetype_name + "'";
                casetype_gid = conn.GetExecuteScalar(msSQL);
               
                string caseDate = values.case_date;

                try 
                {
                    DateTime case_date1 = DateTime.ParseExact(caseDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    case_date = case_date1.ToString("yyyy-MM-dd");
                }
                catch (Exception error)
                {
                    values.status = false;
                    values.message = error.Message.ToString();
                    return;
                }
                try 
                {
                    msCEGetGid = cmnfunctions.GetMasterGID("CEIF");
                }
                catch(Exception ex)
                {
                    values.status = false;
                    values.message = ex.Message.ToString();
                    return;
                }
                
                try
                {
                    msSQL = " insert into lgl_trn_tcaseinformation (" +
                    " case_gid," +
                    " casetype_gid," +
                    " casetype_name," +
                    " institute_gid," +
                    " institute_name," +
                    " case_date," +
                    " case_no," +
                    " client_name," +
                    " remarks," +
                    " created_date," +
                    " created_by" +
                    " ) values ( " +
                    "'" + msCEGetGid + "'," +
                    "'" + values.casetype_name + "'," +
                    "'" + casetype_gid + "'," +
                    "'" + values.institute_name + "'," +
                    "'" + institute_gid + "'," +
                    "'" + case_date + "'," +
                    "'" + values.case_no + "'," +
                    "'" + values.client_name + "'," +
                    "'" + values.case_remarks + "'," +
                    "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + user_gid + "')";
                    try
                    {
                        mnResult = conn.ExecuteNonQuerySQL(msSQL);
                        if (mnResult != 0)
                        {
                            values.status = true;
                            values.message = "Case Details added successfully !!";
                        }
                    }
                    catch (Exception ex)
                    {
                        values.status = false;
                        values.message = ex.Message.ToString();
                        return;
                    }
                                    
                }
                catch (Exception error)
                {
                    values.status = false;
                    values.message = "Failed to add Case Details due to an error: " +  error.Message.ToString();
                    return;
                }               
            }
             catch (Exception ex)
            {
                values.message = "Exception occured while getting case insert into!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }
        }

        // ============================================ add evnt end =============================================//

        // ===================================== upload event start =============================================//
        public void PostCaseDoc(HttpRequest httpRequest, result objResult, string user_gid)
        {
            try
            {

                HttpFileCollection httpFileCollection;
                MemoryStream ms_stream = new MemoryStream();
                string lscompany_code = string.Empty;
                HttpPostedFile httpPostedFile;
                string lspath;
                string msGetGid;
                string case_gid = httpRequest.Form["case_gid"];
                string doc_name = httpRequest.Form["doc_name"];
                string casestage_name = httpRequest.Form["casestage_name"];
                string doc_provider = httpRequest.Form["doc_provider"];
                string uploaded_by = httpRequest.Form["uploaded_by"];
                string remarks = httpRequest.Form["remarks"];
                msSQL = " select company_code from adm_mst_tcompany where company_gid='1'";
                lscompany_code = conn.GetExecuteScalar(msSQL);
                MemoryStream ms = new MemoryStream();
                lspath = ConfigurationManager.AppSettings["upload_file"] + "erpdocument" + "/" + lscompany_code + "/" + "/Case/Case_management/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
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
                            string msdocument_gid = cmnfunctions.GetMasterGID("UPLF");
                            httpPostedFile = httpFileCollection[i];
                            string FileExtension = httpPostedFile.FileName;
                            string lsfile_gid = msdocument_gid;
                            string lscompany_document_flag = string.Empty;
                            FileExtension = Path.GetExtension(FileExtension).ToLower();
                            lsfile_gid = lsfile_gid + FileExtension;
                            Stream ls_readStream;
                            ls_readStream = httpPostedFile.InputStream;
                            ls_readStream.CopyTo(ms);

                            lspath1 = ConfigurationManager.AppSettings["upload_file"] + "erpdocument" + "/" + lscompany_code + "/" + "Case/Case_management/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                            string status;
                            status = cmnfunctions.uploadFile(lspath + msdocument_gid, FileExtension);
                            ms.Close();
                            lspath = "assets/media/images/erpdocument" + "/" + lscompany_code + "/" + "Case/Case_management/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension;
                            string final_path = lspath1 + msdocument_gid + FileExtension;
                            string doc_filepath= msdocument_gid + FileExtension;
                            msGetUPGID = cmnfunctions.GetMasterGID("DOUP");
                                msSQL = " insert into lgl_doc_tcasedocments (" +
                                         " doc_gid , " +
                                         " case_gid , " +
                                         " doc_name , " +
                                         " doc_extension , " +
                                         " doc_path , " +
                                         " doc_attpath , " +
                                         " casestage_name , " +
                                         " doc_provider , " +
                                         " uploaded_by , " +
                                         " remarks , " +
                                         " created_date , " +
                                         " created_by " +
                                         " ) values (" +
                                         "'" + msGetUPGID + "'," +
                                         "'" + case_gid + "'," +
                                         "'" + doc_name + "'," +
                                         "'" + FileExtension + "'," +
                                         "'" + final_path + "'," +
                                         "'" + doc_filepath + "'," +
                                         "'" + casestage_name + "'," +
                                         "'" + doc_provider + "'," +
                                         "'" + uploaded_by + "'," +
                                         "'" + remarks + "'," +
                                         "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                         "'" + user_gid + "')";
                                mnResult = conn.ExecuteNonQuerySQL(msSQL);
                            if (mnResult != 0)
                            {
                                objResult.status = true;
                                objResult.message = "Case Documents uploaded successfully !!";
                            }
                            else
                            {
                                objResult.status = false;
                                objResult.message = "Error while uploading Case Documents !!";
                            }

                        }

                    }

                }
                catch (Exception ex)
                {
                    objResult.message = ex.Message.ToString();
                }
            }
            catch (Exception ex)
            {
                objResult.message = "Exception occured while Case Documents!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" +
                $"DataAccess: {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" +
                objResult.message.ToString() + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");

            }

        }

       // public void PostCaseDoc(HttpRequest httpRequest, string user_gid, result objresultcase)
       //{
       //     try
       //     {
       //         msSQL = " select company_code from adm_mst_tcompany where company_gid='1'";
       //         lscompany_code = conn.GetExecuteScalar(msSQL);
                
       //         case_gid = httpRequest.Form["case_gid"];
               
       //         lspath = ConfigurationManager.AppSettings["upload_file"] + "erp_documents" + "/" + lscompany_code + "/" + "Case_management/Case/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
       //         {
       //             if ((!System.IO.Directory.Exists(lspath)))
       //                 System.IO.Directory.CreateDirectory(lspath);
       //         }                
       //         List<attachements> attachemet = new List<attachements>();

       //         try
       //         {
       //             if (httpRequest.Files.Count > 0)
       //             {
       //                 for (int i = 0; i < httpRequest.Files.Count; i++)
       //                 {
       //                     HttpPostedFile httpPostedFile = httpRequest.Files[i];
       //                     FileExtension = Path.GetExtension(httpPostedFile.FileName).ToLower();
       //                     doc_name = Path.GetFileNameWithoutExtension(httpPostedFile.FileName);
       //                     msdocument_gid = cmnfunctions.GetMasterGID("UPLF");
       //                     doc_name1 = doc_name + "-" + cmnfunctions.ExtractLast4Digits(doc_name + msdocument_gid);

       //                     combine_path = Path.Combine(lspath, doc_name1 + FileExtension);

       //                     if (!Directory.Exists(Path.GetDirectoryName(combine_path)))
       //                     {
       //                         Directory.CreateDirectory(Path.GetDirectoryName(combine_path));
       //                     }

       //                     using (Stream fileStream = httpPostedFile.InputStream)
       //                     {
       //                         using (FileStream fs = System.IO.File.Create(combine_path))
       //                         {
       //                             fileStream.CopyTo(fs);
       //                         }
       //                     }

       //                     attachemet.Add(new attachements
       //                     {
       //                         doc = doc_name,
       //                         lspath1 = "erp_documents/" + lscompany_code + "/Case/Case_management/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + msdocument_gid + FileExtension,
       //                         file_extension = FileExtension,
       //                         lspath2 = "erp_documents/" + lscompany_code + "/Case/Case_management/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" +  doc_name1
       //                     }) ;
       //                 }
                                    
       //                 for (i = 0; i < attachemet.Count; i++)
       //                 {
       //                     msGetUPGID = cmnfunctions.GetMasterGID("DOUP");

       //                     msSQL = " insert into lgl_doc_tcasedocments (" +
       //                  " doc_gid , " +
       //                  " case_gid , " +
       //                  " doc_name , " +
       //                  " doc_extension , " +
       //                  " doc_path , " +
       //                  " doc_attpath , " +
       //                  " doc_filepath , " +
       //                  " created_date , " +
       //                  " created_by " +
       //                  " ) values (" +
       //                  "'" + msGetUPGID + "'," +
       //                  "'" + case_gid + "'," +
       //                  "'" + attachemet[i].doc + "'," +
       //                  "'" + attachemet[i].file_extension + "'," +
       //                  "'" + lspath + "'," +
       //                  "'" + attachemet[i].lspath1 + "'," +
       //                  "'" + attachemet[i].lspath2 + "'," +
       //                  "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
       //                  "'" + user_gid + "')";
       //                     mnResult = conn.ExecuteNonQuerySQL(msSQL);
       //                 }
       //                 if (mnResult != 0)
       //                 {
       //                     objresultcase.status = true;
       //                     objresultcase.message = "Case Documents uploaded successfully !!";
       //                 }
       //                 else
       //                 {
       //                     objresultcase.status = false;
       //                     objresultcase.message = "Error while uploading Case Documents !!";
       //                 }
       //             }
       //         }
       //         catch (Exception e)
       //         {
       //             objresultcase.message = e.Message;
       //         }
       //     }
       //     catch (Exception ex)
       //     {
       //         objresultcase.message = "Exception occured while getting case documents upload !";
       //         cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
       //         "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
       //         " * **********" + ex.Message.ToString() + "***********" + objresultcase.message.ToString() + "*****Query****"
       //         + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
       //         + ".txt");
       //     }
          
       // }

// ======================================upload event end  ==================================================//

//========================================= view summary =================================================//        
        public void DaGetViewSummary(string case_gid, MdlCaseManagement values)
        {
            try
            {
                msSQL = " select case_gid, casetype_gid, casetype_name, institute_gid,institute_name, date_format(case_date,'%d-%m-%y') as case_date, case_no, " +
                    "client_name, remarks as case_remarks, date_format(created_date,'%d-%m-%y') as created_date, created_by from lgl_trn_tcaseinformation" +
                    " where case_gid = '" + case_gid  + "'";
                dt_caseTable = conn.GetDataTable(msSQL);
                var GetViewCaseMgmtSummary = new List<GetViewSummaryCase_list>();
                if (dt_caseTable.Rows.Count > 0)
                {
                    foreach (DataRow CTS in dt_caseTable.Rows)
                    {
                        GetViewCaseMgmtSummary.Add(new GetViewSummaryCase_list
                        {
                            case_gid = CTS["case_gid"].ToString(),
                            casetype_gid = CTS["casetype_gid"].ToString(),
                            casetype_name = CTS["casetype_name"].ToString(),
                            institute_gid = CTS["institute_gid"].ToString(),
                            institute_name = CTS["institute_name"].ToString(),
                            case_date = CTS["case_date"].ToString(),
                            case_no = CTS["case_no"].ToString(),
                            client_name = CTS["client_name"].ToString(),
                            case_remarks = CTS["case_remarks"].ToString(),
                            created_date = CTS["created_date"].ToString(),
                            created_by = CTS["created_by"].ToString(),
                        });
                        values.GetViewSummaryCase_list = GetViewCaseMgmtSummary;
                    }
                }
            }
            catch (Exception ex) 
            {
                values.message = "Exception occured while getting case view!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }
        }
//==================================== view summary end ===============================================//

// ====================================document get ==========================================//
        public  void DaGetDocumentupload(string case_gid, MdlCaseManagement values)
        {           

            try
            {              
                msSQL = " select doc_gid,doc_extension, case_gid, doc_path,doc_name, doc_filepath ,doc_attpath,casestage_name,doc_provider,uploaded_by,remarks,date_format(created_date, '%d-%b-%Y') as created_date from lgl_doc_tcasedocments " +
                        " where case_gid='"+ case_gid + "'"; 
                dt_caseTable = conn.GetDataTable(msSQL);
                var GetDoc_list = new List<GetViewDocument_list>();
                if (dt_caseTable.Rows.Count > 0)
                {
                    foreach (DataRow Doc in dt_caseTable.Rows)
                    {
                        GetDoc_list.Add(new GetViewDocument_list
                        {
                            doc_extension = Doc["doc_extension"].ToString(),
                            doc_gid = Doc["doc_gid"].ToString(),
                            case_gid = Doc["case_gid"].ToString(),
                            doc_name = Doc["doc_name"].ToString(),
                            doc_filepath = Doc["doc_filepath"].ToString(),
                            doc_attpath = Doc["doc_attpath"].ToString(),
                            doc_path = Doc["doc_path"].ToString(),
                            casestage_name = Doc["casestage_name"].ToString(),
                            doc_provider = Doc["doc_provider"].ToString(),
                            uploaded_by = Doc["uploaded_by"].ToString(),
                            remarks = Doc["remarks"].ToString(),
                            created_date = Doc["created_date"].ToString(),
                            //doc_path = objcmnstorage.EncryptData((Doc["doc_path"].ToString())),
                        });
                        values.GetViewDocument_list = GetDoc_list;
                    }
                }              
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting whatsapp summary!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }
        }

        public void DaGetDocument(string case_gid, MdlCaseManagement values)
        {
            try
            {
                msSQL = " select doc_gid,doc_extension, case_gid, doc_path,doc_name, doc_filepath ,doc_attpath,casestage_name,doc_provider,uploaded_by,remarks,date_format(created_date, '%d-%b-%Y') as created_date from lgl_doc_tcasedocments " +
                        " where case_gid='" + case_gid + "'";
                dt_caseTable = conn.GetDataTable(msSQL);
                var groupedDocuments = dt_caseTable.AsEnumerable()
                                                   .GroupBy(row => row.Field<string>("casestage_name"))
                                                   .ToDictionary(g => g.Key, g => g.Select(row => new GetViewDoc_list
                                                   {
                                                       doc_extension = row.Field<string>("doc_extension"),
                                                       doc_gid = row.Field<string>("doc_gid"),
                                                       case_gid = row.Field<string>("case_gid"),
                                                       doc_name = row.Field<string>("doc_name"),
                                                       doc_filepath = row.Field<string>("doc_filepath"),
                                                       doc_attpath = row.Field<string>("doc_attpath"),
                                                       doc_path = row.Field<string>("doc_path"),
                                                       casestage_name = row.Field<string>("casestage_name"),
                                                       doc_provider = row.Field<string>("doc_provider"),
                                                       uploaded_by = row.Field<string>("uploaded_by"),
                                                       remarks = row.Field<string>("remarks"),
                                                       created_date = row.Field<string>("created_date"),
                                                   }).ToList());

                values.GetViewDoc_list = groupedDocuments;
            }
            catch (Exception ex)
            {
                values.message = "Exception occurred while getting whatsapp summary!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }
        }



        // ============================================document get end =========================================//


        // ==========================================institute case management summary -=========================//

        public void DaGetInstituteCase(string institute_gid,MdlCaseManagement values)
        {
            try
            {
                msSQL = " select case_gid, casetype_gid,remarks as case_remarks, casetype_name, institute_gid,institute_name," +
                    " date_format(case_date,'%d-%m-%y') as case_date, case_no,client_name, " +
                    " date_format(created_date,'%d-%m-%y') as created_date, created_by from lgl_trn_tcaseinformation where institute_gid='"+ institute_gid + "' ";
                    ;
                dt_caseTable = conn.GetDataTable(msSQL);
                var GetCaseMgmtSummary = new List<GetCaseManagementSummart_list>();
                if (dt_caseTable.Rows.Count > 0)
                {
                    foreach (DataRow CTS in dt_caseTable.Rows)
                    {
                        GetCaseMgmtSummary.Add(new GetCaseManagementSummart_list
                        {
                            case_gid = CTS["case_gid"].ToString(),
                            casetype_gid = CTS["casetype_gid"].ToString(),
                            casetype_name = CTS["casetype_name"].ToString(),
                            institute_gid = CTS["institute_gid"].ToString(),
                            institute_name = CTS["institute_name"].ToString(),
                            case_date = CTS["case_date"].ToString(),
                            case_no = CTS["case_no"].ToString(),
                            client_name = CTS["client_name"].ToString(),
                            case_remarks = CTS["case_remarks"].ToString(),
                            created_date = CTS["created_date"].ToString(),
                            created_by = CTS["created_by"].ToString(),
                        });
                        values.GetCaseManagementSummart_list = GetCaseMgmtSummary;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting case management summary!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }
        }
        public void DaDeletedocument(string doc_gid, MdlCaseManagement values)
        {
            try
            {
                msSQL = " delete from lgl_doc_tcasedocments where doc_gid='" + doc_gid + "'";
                mnResult = conn.ExecuteNonQuerySQL(msSQL);
                if (mnResult != 0)
                {
                    values.status = true;
                    values.message = "Case Document Deleted Successfully";
                }
                else
                {
                    values.status = false;
                    values.message = "Error While Delete Case Document";
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while delete Case Document !";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:" +
              $" {System.Reflection.MethodBase.GetCurrentMethod().Name}" + "***********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****" +
              msSQL + "*******Apiref********", "ErrorLog/Sales/" + "Log" + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt");
            }
        }

        public void DaGetcasestage(MdlCaseManagement values)
        {
            try
            {
                msSQL = "select casestage_gid,casestage_name from lgl_mst_tcasestage  order by casestage_code asc";
                dt_caseTable = conn.GetDataTable(msSQL);
                var GetCaseInstitute = new List<getcasestage_list>();
                if (dt_caseTable.Rows.Count > 0)
                {
                    foreach (DataRow CI in dt_caseTable.Rows)
                    {
                        GetCaseInstitute.Add(new getcasestage_list
                        {
                            casestage_gid = CI["casestage_gid"].ToString(),
                            casestage_name = CI["casestage_name"].ToString(),
                        });
                        values.getcasestage_list = GetCaseInstitute;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting stage!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }

        }

        public void DaGetdocprovider(MdlCaseManagement values)
        {
            try
            {
                msSQL = "select provider_gid, doc_provider from lgl_mst_tcaseprovider";
                dt_caseTable = conn.GetDataTable(msSQL);
                var GetCaseInstitute = new List<docprovider_list>();
                if (dt_caseTable.Rows.Count > 0)
                {
                    foreach (DataRow CI in dt_caseTable.Rows)
                    {
                        GetCaseInstitute.Add(new docprovider_list
                        {
                            provider_gid = CI["provider_gid"].ToString(),
                            doc_provider = CI["doc_provider"].ToString(),
                        });
                        values.docprovider_list = GetCaseInstitute;
                    }
                }
            }
            catch (Exception ex)
            {
                values.message = "Exception occured while getting provider!";
                cmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") +
                "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " +
                " * **********" + ex.Message.ToString() + "***********" + values.message.ToString() + "*****Query****"
                + msSQL + "*******Apiref********", "ErrorLog/Law/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH")
                + ".txt");
            }

        }






    }
}