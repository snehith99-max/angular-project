using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.storage.Models;



namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/FileManagement")]

    public class FileManagementController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaFileManagement objdafilemanager = new DaFileManagement();
        Fnazurestorage objFnazurestorage = new Fnazurestorage();

        [ActionName("DocumentUploadSummary")]
        [HttpGet]
        public HttpResponseMessage DocumentUploadSummary()
        {
            MdlFileManagement values = new MdlFileManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objdafilemanager.DaDocumentUploadSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DocumentUploadSummaryList")]
        [HttpGet]
        public HttpResponseMessage DocumentUploadSummaryList()
        {
            MdlFileManagement values = new MdlFileManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objdafilemanager.DaDocumentUploadSummaryList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("CreateFolder")]
        [HttpPost]
        public HttpResponseMessage CreateFolder(MdlFileManagement values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objdafilemanager.DaPostCreateFolder(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DocumentUpdate")]
        [HttpPost]
        public HttpResponseMessage DocumentUpdate(documentuploadlist_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objdafilemanager.DaDocumentUpdate(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("FileUpdate")]
        [HttpPost]
        public HttpResponseMessage FileUpdate(documentuploadlist_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objdafilemanager.DaFileUpdate(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("FolderDtls")]
        [HttpGet]
        public HttpResponseMessage DaGetFolderDtls(string parent_directorygid)
        {


            MdlFileManagement value = new MdlFileManagement();
            objdafilemanager.DaGetFolderDtls(parent_directorygid, value);
            return Request.CreateResponse(HttpStatusCode.OK, value);
        }

        [ActionName("FolderDelete")]
        [HttpGet]
        public HttpResponseMessage PostFolderDelete(string docupload_gid)
        {
            documentuploadlist_list values = new documentuploadlist_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objdafilemanager.DaPostFolderDelete(docupload_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("FileDelete")]
        [HttpGet]
        public HttpResponseMessage PostFileDelete(string docupload_gid)
        {
            documentuploadlist_list values = new documentuploadlist_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objdafilemanager.DaPostFileDelete(docupload_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("DocumentUpload")]
        [HttpPost]
        public HttpResponseMessage DocumentUpload()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objdafilemanager.DaDocumentUpload(httpRequest, getsessionvalues.user_gid, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("DownloadDocumentazurefile")]
        [HttpPost]
        public HttpResponseMessage DownloadDocumentazurefile(MdlTelecallingDownload values)
        {
            var ls_response = new Dictionary<string, object>();
            //values.file_path = objFnazurestorage.DecryptData(values.file_path);
            ls_response = objdafilemanager.FnDownloadDocumentAzureContainer(values.file_path, values.file_name);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }
    }

}