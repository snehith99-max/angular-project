using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ems.hrm.Controllers
{
    [Authorize]
    [RoutePrefix("api/ExitManagement")]
    public class ExitManagementController : ApiController
    {
        session_values objgetGID = new session_values();
        logintoken getsession_values = new logintoken();
        DaExitManagement objresult = new DaExitManagement();

        [ActionName("GetExitmanagementSummary")]
        [HttpGet]
        public HttpResponseMessage GetExitmanagementSummary()
        {
            MdlExitmanagement values = new MdlExitmanagement();
            objresult.DaGetExitmanagementSummary(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }

        //--------------------------------------------------------360------------------------------------------------------//

        [ActionName("GetEmployeeDetails")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeDetails(string exitemployee_gid)
        {
            MdlExitmanagement values = new MdlExitmanagement();
            objresult.DaGetEmployeeDetails(exitemployee_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }

        [ActionName("GetLeaveDetails")]
        [HttpGet]
        public HttpResponseMessage GetLeaveDetails()
        {
            MdlExitmanagement values = new MdlExitmanagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = objgetGID.gettokenvalues(token);
            objresult.DaGetLeaveDetails(getsession_values.employee_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
//----------------------------------popup--------------------------------------------------//
        [ActionName("GetInitiateApproval")]
        [HttpGet]
        public HttpResponseMessage GetInitiateApproval(string exitemployee_gid)
        {
            MdlExitmanagement values = new MdlExitmanagement();
            objresult.DaGetInitiateApproval(exitemployee_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }

        [ActionName("PostInitiateApproval")]
        [HttpPost]
        public HttpResponseMessage PostInitiateApproval(PostParma_list values)
        {            
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = objgetGID.gettokenvalues(token);
            result lsresult = new result();
            objresult.PostInitiateApproval(values.exitemployee_gid, values.manager_name, getsession_values.employee_gid, lsresult);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, lsresult);
        }

        [ActionName("GetInitiateApprovalSummary")]
        [HttpGet]
        public HttpResponseMessage GetInitiateApprovalSummary(string exitemployee_gid)
        {
            MdlExitmanagement values = new MdlExitmanagement();
            objresult.DaGetInitiateApprovalSummary(exitemployee_gid,values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }

        [ActionName("GetSalaryDetailsSummary")]
        [HttpGet]
        public HttpResponseMessage GetSalaryDetailsSummary(string exitemployee_gid)
        {
            MdlExitmanagement GetSalaryDetailsAll_list = new MdlExitmanagement();
            GetSalaryDetailsAll_list= objresult.DaGetSalaryDetailsSummary(exitemployee_gid, GetSalaryDetailsAll_list);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, GetSalaryDetailsAll_list);
        }

        [ActionName("GetAssetCustodianSummary")]
        [HttpGet]
        public HttpResponseMessage GetAssetCustodianSummary(string employee_gid)
        {
            MdlExitmanagement values = new MdlExitmanagement();
            objresult.DaGetAssetCustodianSummary(employee_gid,values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("Post360Submit")]
        [HttpPost]
        public HttpResponseMessage Post360Submit(PostParma_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = objgetGID.gettokenvalues(token);
            result lsresult = new result();
            objresult.Post360Submit(values.exitemployee_gid, values.editor_content, getsession_values.employee_gid, lsresult);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, lsresult);
        }
    }
}