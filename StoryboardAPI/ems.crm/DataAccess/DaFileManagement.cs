using ems.crm.Models;
using ems.crm.DataAccess;
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
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using static OfficeOpenXml.ExcelErrorValue;
using System.Diagnostics;
using System.Web.Http.Results;
using System.Threading;
using System.Data.Common;
using System.Threading.Tasks;
using System.Text;
using System.Security.Policy;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;



namespace ems.crm.DataAccess
{
    public class DaFileManagement
    {
        dbconn objdbconn = new dbconn();
        cmnfunctions objcmnfunctions = new cmnfunctions();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();
        string msSQL = string.Empty;
        string msSQL1 = string.Empty;
        OdbcDataReader objOdbcDataReader;
        DataTable dt_datatable;
        int mnResult;
        string msGetGid, msGetGid1, final_path, lsmaster_value, parentDirectoryGid, lspath, msGetGid3, doc_gid, msGetGID, parentgid, lscompany_document_flag, lsblopname, httpsUrl;

        public bool DaDocumentUploadSummary(MdlFileManagement values)
        {
            try
            {
                msSQL = " SELECT a.docupload_gid,a.docupload_name,a.file_path,a.fileupload_name,a.docupload_type,a.docuploadparent_gid," +
                        " DATE_FORMAT(a.created_date, '%d-%m-%Y') as created_date,concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by,a.azure_path " +
                        " FROM crm_trn_tdocumentupload a left join adm_mst_tuser c on a.created_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid " +
                        " WHERE a.docuploadparent_gid IS not NULL and a.docuploadparent_gid like '%$%'  ORDER  BY a.docupload_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<documentuploadlist_list>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new documentuploadlist_list
                        {                  
                            docupload_gid = dt["docupload_gid"].ToString(),
                            docuploadparent_gid = dt["docuploadparent_gid"].ToString(),
                            docupload_name = dt["docupload_name"].ToString(),
                            fileupload_name = dt["fileupload_name"].ToString(),
                            file_path = dt["file_path"].ToString(),
                            docupload_type = dt["docupload_type"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            azure_path = dt["azure_path"].ToString(),
                        });
                    }
                    values.DocumentUploadlist_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
         ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                ex.StackTrace.ToString();
                values.status = false;
                return false;

            }
        }
        public bool DaDocumentUploadSummaryList(MdlFileManagement values)
        {
            try
            {
                msSQL = " SELECT a.docupload_gid,a.docupload_name,a.file_path,a.fileupload_name,a.docupload_type,a.docuploadparent_gid," +
                        " DATE_FORMAT(a.created_date, '%d-%m-%Y') as created_date,concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by,a.azure_path " +
                        " FROM crm_trn_tdocumentupload a left join adm_mst_tuser c on a.created_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid " +
                        " WHERE a.docupload_type='File'  ORDER  BY a.docupload_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var getdocumentlist = new List<documentuploadlist_list>();

                if (dt_datatable != null && dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow dt in dt_datatable.Rows)
                    {
                        getdocumentlist.Add(new documentuploadlist_list
                        {
                            docupload_gid = dt["docupload_gid"].ToString(),
                            docuploadparent_gid = dt["docuploadparent_gid"].ToString(),
                            docupload_name = dt["docupload_name"].ToString(),
                            fileupload_name = dt["fileupload_name"].ToString(),
                            file_path = dt["file_path"].ToString(),
                            docupload_type = dt["docupload_type"].ToString(),
                            created_date = dt["created_date"].ToString(),
                            created_by = dt["created_by"].ToString(),
                            azure_path = dt["azure_path"].ToString(),
                        });
                    }
                    values.DocumentUploadlist_list = getdocumentlist;
                    values.status = true;
                    dt_datatable.Dispose();
                    return true;
                }
                else
                {
                    values.status = false;
                    dt_datatable.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
         ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                ex.StackTrace.ToString();
                values.status = false;
                return false;

            }
        }
        public bool DaPostCreateFolder(string user_gid, MdlFileManagement values)
        {
            try
            {

                msGetGID = objcmnfunctions.GetMasterGID("UPLF");

                msSQL = " INSERT INTO crm_trn_tdocumentupload(" +
                " docupload_gid," +
                " docuploadparent_gid," +
                " docupload_name," +
                " docupload_type," +
                " created_date," +
                " created_by)" +
                " VALUES(" +
                "'" + msGetGID + "'," +
                "'" + values.parent_gid + "'," +
                "'" + values.folder_name.Replace("'", "''") + "'," +
                "'" + values.docupload_type + "'," +
                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                "'" + user_gid + "')";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "Folder Created Successfully";
                    return true;
                }
                else
                {
                    values.status = false;
                    values.message = "Error Occured";
                    return false;
                }


            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
             ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                ex.StackTrace.ToString();
                values.status = false;
                return false;

            }
        }

        public bool DaDocumentUpdate(documentuploadlist_list value, string user_gid)
        {
            try
            {
                msSQL = " select docuploadparent_gid from crm_trn_tdocumentupload  where docupload_gid = '" + value.docupload_gid + "'";
                var parent_directorygid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select docupload_name from crm_trn_tdocumentupload " +
              " where LOWER (docupload_name) = '" + value.docupload_name.Replace("'", "''").ToLower() + "'" +
              " and docupload_gid !='" + value.docupload_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    value.message = "Folder Name Already Exists";
                    value.status = false;
                    return false;
                }
                dt_datatable.Dispose();

                msSQL = "Update crm_trn_tdocumentupload set " +
                      " docupload_name='" + value.docupload_name.Replace("'", "''") + "'," +
                      " updated_by='" + user_gid + "'," +
                      " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                      " WHERE docupload_gid='" + value.docupload_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult == 1)
                {
                    value.message = "Folder Name Updated Successfully";
                    value.status = true;
                    return true;
                }
                else
                {
                    value.message = "Error while Updating Folder";
                    value.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
               ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                ex.StackTrace.ToString();
                value.status = false;
                return false;
            }

        }

        public bool DaFileUpdate(documentuploadlist_list value, string user_gid)
        {
            try
            {
                msSQL = " select docuploadparent_gid from crm_trn_tdocumentupload  where docupload_gid = '" + value.docupload_gid + "'";
                var parent_directorygid = objdbconn.GetExecuteScalar(msSQL);

                msSQL = " select fileupload_name from crm_trn_tdocumentupload " +
              " where LOWER (fileupload_name) = '" + value.fileupload_name.Replace("'", "''").ToLower() + "'" +
              " and docupload_gid !='" + value.docupload_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count != 0)
                {
                    value.message = "File Name Already Exists";
                    value.status = false;
                    return false;
                }
                dt_datatable.Dispose();
                msSQL = "Update crm_trn_tdocumentupload set " +
                      " fileupload_name='" + value.fileupload_name.Replace("'", "''") + "'," +
                      " updated_by='" + user_gid + "'," +
                      " updated_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                      " WHERE docupload_gid='" + value.docupload_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult == 1)
                {
                    value.message = "File Name Updated Successfully";
                    value.status = true;
                    return true;
                }
                else
                {
                    value.message = "Error while Updating File";
                    value.status = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                  ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                ex.StackTrace.ToString();
                value.status = false;
                return false;
            }

        }


        public bool DaGetFolderDtls(string parent_directorygid, MdlFileManagement value)
        {
            try
            {

                msSQL = " SELECT a.docuploadparent_gid,a.docupload_name,a.file_path,a.fileupload_name,a.docupload_type,a.docupload_gid," +
                            " DATE_FORMAT(a.created_date, '%d-%m-%Y') as created_date,concat(c.user_firstname,' ',c.user_lastname,' / ',c.user_code) as created_by,a.azure_path" +
                            " FROM crm_trn_tdocumentupload a left join adm_mst_tuser c on a.created_by = c.user_gid  left join hrm_mst_temployee b on b.employee_gid = c.user_gid " +
                            " where a.docuploadparent_gid='" + parent_directorygid + "'" +
                            " ORDER  BY a.docuploadparent_gid DESC";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                var FolderDtl = new List<FolderList>();
                if (dt_datatable.Rows.Count != 0)
                {
                    foreach (DataRow row in dt_datatable.Rows)
                    {
                        FolderDtl.Add(new FolderList
                        {
                            docuploadparent_gid = row["docuploadparent_gid"].ToString(),
                            docupload_gid = row["docupload_gid"].ToString(),
                            docupload_name = row["docupload_name"].ToString(),
                            fileupload_name = row["fileupload_name"].ToString(),
                            file_path = row["file_path"].ToString(),
                            created_date = row["created_date"].ToString(),
                            docupload_type = row["docupload_type"].ToString(),
                            created_by = row["created_by"].ToString(),
                            azure_path= row["azure_path"].ToString(),
                        });
                    }
                    value.Folder_list = FolderDtl;

                    value.status = true;
                    value.message = "Success";
                    return true;

                }

                else
                {
                    value.status = false;
                    value.message = "No Records Found";
                    return false;
                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                  ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                ex.StackTrace.ToString();
                value.status = false;
                return false;
            }

        }

        public bool DaPostFolderDelete(string docupload_gid, documentuploadlist_list values)
        {
            try
            {
                // Define the query with a CTE to check for files in the folder hierarchy
                msSQL = "select  docupload_gid from crm_trn_tdocumentupload where docuploadparent_gid='" + docupload_gid + "'";
                dt_datatable = objdbconn.GetDataTable(msSQL);
                if (dt_datatable.Rows.Count > 0)
                {
                    values.status = false;
                    values.message = "Folder Contain File or Subfolder, You Can't Delete";
                    return false;
                }
                else
                {
                    // Proceed to delete the folder
                    msSQL = "  delete from crm_trn_tdocumentupload where docupload_gid='" + docupload_gid + "'";
                    mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);

                    // Check if the deletion was successful
                    if (mnResult == 1)
                    {
                        values.status = true;
                        values.message = "Folder deleted successfully";
                    }
                    else
                    {
                        values.status = false;
                        values.message = "Error While delete the Folder";
                    }
                }

                return values.status;



            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                 ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                ex.StackTrace.ToString();
                values.status = false;
                return false;
            }
        }

        public bool DaPostFileDelete(string docupload_gid, documentuploadlist_list values)
        {
            try
            {


                msSQL = " delete from crm_trn_tdocumentupload where docupload_gid='" + docupload_gid + "'";
                mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);


                if (mnResult == 1)
                {
                    values.status = true;
                    values.message = "File Deleted Successfully";
                    return true;
                }
                else
                {
                    values.status = false;
                    values.message = "Error While delete the File";
                    return false;
                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                  ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                ex.StackTrace.ToString();
                values.status = false;
                return false;
            }
        }


        public void DaDocumentUpload(HttpRequest httpRequest, string user_gid, result objResult)
        {

            HttpFileCollection httpFileCollection;

            MemoryStream ms_stream = new MemoryStream();

            string docuploadparent_gid = httpRequest.Form[1];
            string lscompany_code = string.Empty;
            if (docuploadparent_gid == "")
            {
                parentgid = "$";
            }
            else
            {
                parentgid = httpRequest.Form[1];
            }
            String path = lspath;
            HttpPostedFile httpPostedFile;

            msSQL = " SELECT a.company_code FROM adm_mst_tcompany a ";
            lscompany_code = objdbconn.GetExecuteScalar(msSQL);
            path = ConfigurationManager.AppSettings["docufile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "CRM/FileManagement/" + DateTime.Now.Year + "/" + DateTime.Now.Month;

            if ((!System.IO.Directory.Exists(path)))
                System.IO.Directory.CreateDirectory(path);

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
                        string FileExtension = httpPostedFile.FileName.Replace("'", "''");
                        string lsfile_gid = msdocument_gid;
                        string project_flag = httpRequest.Form["project_flag"].ToString();
                        FileExtension = Path.GetExtension(FileExtension).ToLower();

                        lsfile_gid = lsfile_gid + FileExtension;
                        Stream ls_readStream;
                        ls_readStream = httpPostedFile.InputStream;
                        MemoryStream ms = new MemoryStream();
                        ls_readStream.CopyTo(ms);

                        byte[] bytes = ms.ToArray();
                        if ((objFnazurestorage.CheckIsValidfilename(FileExtension, project_flag) == false) || (objFnazurestorage.CheckIsExecutable(bytes) == true))
                        {
                            objResult.status = false;
                            objResult.message = "File format is not supported";
                            return;
                        }

                        bool status;
                        string type = "File";
                        status = objcmnfunctions.UploadStream(ConfigurationManager.AppSettings["blob_containername"], lscompany_code + "/" + "CRM/FileManagement/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + msdocument_gid + FileExtension, FileExtension, ms);
                        ms.Close();
                        final_path = ConfigurationManager.AppSettings["blob_containername"] + "/" + lscompany_code + "/" + "CRM/FileManagement/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";

                        httpsUrl = ConfigurationManager.AppSettings["blob_imagepath1"] + final_path + msdocument_gid + FileExtension + ConfigurationManager.AppSettings["blob_imagepath2"] +
                                                '&' + ConfigurationManager.AppSettings["blob_imagepath3"] + '&' + ConfigurationManager.AppSettings["blob_imagepath4"] + '&' + ConfigurationManager.AppSettings["blob_imagepath5"] +
                                                '&' + ConfigurationManager.AppSettings["blob_imagepath6"] + '&' + ConfigurationManager.AppSettings["blob_imagepath7"] + '&' + ConfigurationManager.AppSettings["blob_imagepath8"];
                        lspath = ConfigurationManager.AppSettings["docufile_path"] + "/erpdocument" + "/" + lscompany_code + "/" + "CRM/FileManagement/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                        objcmnfunctions.uploadFile(lspath, lsfile_gid);
                        lspath = "erpdocument" + "/" + lscompany_code + "/" + "CRM/FileManagement/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";



                        msSQL = " insert into crm_trn_tdocumentupload(" +
                                    " docupload_gid," +
                                    " docuploadparent_gid ," +
                                    " fileupload_name," +
                                    " docupload_type ," +
                                    " file_path," +
                                    " azure_path," +
                                    " created_by," +
                                    " created_date" +
                                    " )values(" +
                                    "'" + msdocument_gid + "'," +
                                    "'" + parentgid + "'," +
                                    "'" + httpPostedFile.FileName.Replace("'", "''") + "'," +
                                    "'" + type + "'," +
                                    "'" + lspath + msdocument_gid + FileExtension + "'," +
                                    "'" + httpsUrl + "'," +
                                    "'" + user_gid + "'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        mnResult = objdbconn.ExecuteNonQuerySQL(msSQL);
                    }


                    if (mnResult != 0)
                    {
                        objResult.status = true;
                        objResult.message = "Document Uploaded Successfully";

                    }
                    else
                    {
                        objResult.status = false;
                        objResult.message = "Error Occured While Uploading the document";

                    }

                }

            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                 ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

                objResult.status = false;

            }

        }


        public dynamic FnDownloadDocumentAzureContainer(string file_path, string file_name)
        {

            var ls_response = new Dictionary<string, object>();
            try
            {
                // string[] strlist = file_name.Split('.');

                int lastIndex = file_name.LastIndexOf('.');
                var filename = file_name.Substring(0, lastIndex);
                var format = file_name.Substring(lastIndex + 1);
                msSQL = "select company_document_flag from adm_mst_tcompany";
                lscompany_document_flag = objdbconn.GetExecuteScalar(msSQL);
                MemoryStream ms = new MemoryStream();
                ms = DownloadStreamAzureContainer("erpdocument", file_path, lscompany_document_flag);
                //ls_response = ConvertDocumentToByteArray(ms, strlist[0], strlist[1]);
                ls_response = ConvertDocumentToByteArrayContainer(ms, filename, format);
            }
            catch (Exception ex)
            {

                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");

            }
            return ls_response;

          
        }
        public MemoryStream DownloadStreamAzureContainer(string container_name, string blob_filename, string downlod_type)
        {
            if (downlod_type == "L")
            {
                MemoryStream ms = new MemoryStream();
                lsblopname = ConfigurationManager.AppSettings["docufile_path"] + "/" + blob_filename;
                using (FileStream file = new FileStream(lsblopname, FileMode.Open, System.IO.FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);
                }
                return ms;
            }

            else
            {
                try
                {
                    blob_filename = blob_filename.Replace("erpdocuments/", "");
                    MemoryStream memoryStream = new MemoryStream();
                    // Retrieve storage account from connection string.
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"].ToString());
                    // Create the blob client.
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    // Retrieve reference to a previously created container.
                    CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["blob_containername"].ToString());
                    // Retrieve reference to a blob named "photo1.jpg".
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob_filename);

                    var ls_downfile = new MemoryStream();

                    blockBlob.DownloadToStream(ls_downfile);
                    return ls_downfile;
                }
                catch (Exception ex)
                {
                    objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
                    var ls_downfile1 = new MemoryStream();
                    return ls_downfile1;
                }
            }
        }

        public Dictionary<string, object> ConvertDocumentToByteArrayContainer(MemoryStream msstream, string filename, string format)
        {
            var ls_response = new Dictionary<string, object>();
            string downloadfile = string.Empty;
            try
            {
                string message = "";
                string msg_type = "";
                if (msstream.Length > 0)
                {
                    msstream.Position = 0;
                    downloadfile = Convert.ToBase64String(msstream.ToArray());
                    //byte[] encodeddata= Encoding.UTF8.GetBytes(downloadfile);
                    //ls_response.Add("encodedfile", encodeddata);
                    ls_response.Add("file", downloadfile);
                    ls_response.Add("format", format);
                    ls_response.Add("name", filename + "." + format);
                    ls_response.Add("status", true);
                }
                else
                {

                    ls_response.Add("message", message);
                    ls_response.Add("status", false);
                }


            }
            catch (Exception ex)
            {
                objcmnfunctions.LogForAudit("*******Date*****" + DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss") + "***********" + $"DataAccess:{System.Reflection.MethodBase.GetCurrentMethod().Name} " + " * **********" +
                                ex.Message.ToString() + "***********" + "*****Query****" + msSQL + "*******Apiref********", "ErrorLog/Marketing/ " + "Log" + DateTime.Now.ToString("yyyy - MM - dd HH") + ".txt");
            }

            return ls_response;
        }
    }
}