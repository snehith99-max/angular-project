using ems.law.DataAccess;
using ems.law.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.UI.WebControls;

namespace ems.law.Controllers
{

    [Authorize]
    [RoutePrefix("api/CaseManagement")]
    public class CaseManagementController : ApiController
    {
        session_values objgetGID = new session_values();
        logintoken getsession_values = new logintoken();
        DaCaseManagement objresult = new DaCaseManagement();

// ------------------------------------------- summary end --------------------------------------------------- //

        [ActionName("GetCaseManagementSummary")]
        [HttpGet]
        public HttpResponseMessage GetCaseManagementSummary()
        {
            MdlCaseManagement values = new MdlCaseManagement();
            objresult.DaGetCaseManagementSummary(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
// ------------------------------------------- summary end --------------------------------------------------- //

// ------------------------------------------- add start --------------------------------------------------- //
        [ActionName("GetCasetype")]
        [HttpGet]
        public HttpResponseMessage GetCasetype()
        {
            MdlCaseManagement values = new MdlCaseManagement();
            objresult.DaGetCasetype(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        
        [ActionName("GetCaseInstitute")]
        [HttpGet]
        public HttpResponseMessage GetCaseInstitute()
        {
            MdlCaseManagement values = new MdlCaseManagement();
            objresult.DaGetCaseInstitute(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }

        [ActionName("PostCaseInformation")]
        [HttpPost]
        public HttpResponseMessage PostCaseInformation(GetCaseManagementSummart_list values) 
        {            
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = objgetGID.gettokenvalues(token);                        
            objresult.PostCaseInformation(getsession_values.user_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }

   
        [ActionName("PostCaseDoc")]
        [HttpPost]
        public HttpResponseMessage PostCaseDoc()
        {
            HttpRequest httpRequest;
            //Postassetlocationcreation values = new Postassetlocationcreation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = objgetGID.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objresult.PostCaseDoc(httpRequest, objResult, getsession_values.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        // ------------------------------------------- add end --------------------------------------------------- //

        // ------------------------------------------- view start --------------------------------------------------- //

        [ActionName("GetViewSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewSummary(string case_gid)
        {
            MdlCaseManagement values = new MdlCaseManagement();
            objresult.DaGetViewSummary(case_gid,values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK,values);
        }

        [ActionName("GetDocument")]
        [HttpGet]
        public HttpResponseMessage GetDocument(string case_gid)
        {
            MdlCaseManagement values = new MdlCaseManagement();
            objresult.DaGetDocument(case_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK,values);
           
        }
        [ActionName("GetDocumentupload")]
        [HttpGet]
        public HttpResponseMessage GetDocumentupload(string case_gid)
        {
            MdlCaseManagement values = new MdlCaseManagement();
            objresult.DaGetDocumentupload(case_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);

        }
        // ------------------------------------------- view end --------------------------------------------------- //


        // -------------------------------------------- institute case management summary -----------------------------------//

        [ActionName("GetInstituteCase")]
        [HttpGet]

        public HttpResponseMessage GetInstituteCase(string institute_gid)
        {
            MdlCaseManagement values = new MdlCaseManagement();
            objresult.DaGetInstituteCase(institute_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK,values);
        }

        [ActionName("Deletedocument")]
        [HttpGet]

        public HttpResponseMessage Deletedocument(string doc_gid)
        {
            MdlCaseManagement values = new MdlCaseManagement();
            objresult.DaDeletedocument(doc_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }

        [ActionName("Getcasestage")]
        [HttpGet]
        public HttpResponseMessage Getcasestage()
        {
            MdlCaseManagement values = new MdlCaseManagement();
            objresult.DaGetcasestage(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }


        [ActionName("Getdocprovider")]
        [HttpGet]
        public HttpResponseMessage Getdocprovider()
        {
            MdlCaseManagement values = new MdlCaseManagement();
            objresult.DaGetdocprovider(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }




    }
}