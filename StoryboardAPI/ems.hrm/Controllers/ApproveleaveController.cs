﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.utilities.Models;
using ems.utilities.Functions;
using ems.hrm.Models;
using ems.hrm.DataAccess;

namespace StoryboardAPI.Controllers.ems.hrm

{

    [RoutePrefix("api/approveLeave")]
    [Authorize]

    public class approveLeaveController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaApproveLeave objDaApproveLeave = new DaApproveLeave();
        
        [ActionName("getapproval_count")]
        [HttpGet]

        public HttpResponseMessage getapproval_count()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            approvalcountdetails objleavecountdetails = new approvalcountdetails();
            objDaApproveLeave.DaGetApprovalCount(getsessionvalues.employee_gid, objleavecountdetails);
            return Request.CreateResponse(HttpStatusCode.OK, objleavecountdetails);
        }

        //......................* 1. LEAVE APPROVAL Details *.................................//

        // Get Leave Details For Approval...//

        [ActionName("GetapproveLeavedetails")]
        [HttpGet]
        public HttpResponseMessage GetapproveLeavedetails(string leave_gid)
        {
            GetapproveLeavedetails values = new GetapproveLeavedetails();
            objDaApproveLeave.DaGetApproveLeaveDetails(values, leave_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Approval Pending Summary....//

        [ActionName("getleaveapprovependingdetails")]
        [HttpGet]

        public HttpResponseMessage getleaveapprovependingdetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavedetails values = new getleavedetails();
            objDaApproveLeave.DaGetLeaveApprovePendingDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Approved Summary....//

        [ActionName("getleaveapprovedetails")]
        [HttpGet]

        public HttpResponseMessage getleaveapprovedetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavedetails values = new getleavedetails();
            objDaApproveLeave.DaGetLeaveApproveDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Rejected Summary.....//

        [ActionName("getleaverejectdetails")]
        [HttpGet]

        public HttpResponseMessage getleaverejectdetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavedetails values = new getleavedetails();
            objDaApproveLeave.DaGetLeaveRejectDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Approve Leave Click......//

        [ActionName("approveleaveclick")]
        [HttpPost]

        public HttpResponseMessage postapproveleave(approveleavedetails values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaApproveLeave.DaPostApproveLeave(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Reject Leave Click......//

        [ActionName("rejectleaveclick")]
        [HttpPost]

        public HttpResponseMessage postrejectleave(applyleavedetails values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaApproveLeave.DaPostRejectLeave(values, getsessionvalues.employee_gid, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("getleavedocument")]
        //[HttpGet]

        //public HttpResponseMessage getleavedocument(string leave_gid)
        //{
        //    uploaddocument values = new uploaddocument();
        //    objDaApproveLeave.DaGetLeaveDocument(leave_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        //..............................* 2. LOGIN APPROVAL Details *..............................//

        [ActionName("getloginsummarydetails")]
        [HttpGet]

        public HttpResponseMessage getloginsummarydetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getlogindetails values = new getlogindetails();
            objDaApproveLeave.DaGetLoginApproval(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //.....* Login approved summary............//

        [ActionName("getloginleaveapprovedetails")]
        [HttpGet]

        public HttpResponseMessage getloginleaveapprovedetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavesummarylogindetails values = new getleavesummarylogindetails();
            objDaApproveLeave.getloginleaveapprovedetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getloginleaverejectdetails")]
        [HttpGet]

        public HttpResponseMessage getloginleaverejectdetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavesummarylogindetails values = new getleavesummarylogindetails();
            objDaApproveLeave.DaGetLoginLeaveRejectDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("approvelogin")]
        [HttpPost]

        public HttpResponseMessage postapprovelogin(approvelogin values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaApproveLeave.DaPostApproveLogin(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("rejectlogin")]
        [HttpPost]

        public HttpResponseMessage postrejectlogin(approvelogin values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaApproveLeave.DaPostRejectLogin(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //..............................* 3. LOGOUT APPROVAL  Details *.............................//

        [ActionName("getlogoutsummarydetails")]
        [HttpGet]

        public HttpResponseMessage getlogoutsummarydetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getlogoutdetails values = new getlogoutdetails();
            objDaApproveLeave.DaGetLogoutApproval(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //.....* Logoutapproved summary...........//
        [ActionName("getlogoutleaveapprovedetails")]
        [HttpGet]

        public HttpResponseMessage getlogoutleaveapprovedetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavesummarylogoutdetails values = new getleavesummarylogoutdetails();
            objDaApproveLeave.DaGetLogoutLeaveApproveDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getlogoutleaverejectdetails")]
        [HttpGet]

        public HttpResponseMessage getlogoutleaverejectdetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavesummarylogoutdetails values = new getleavesummarylogoutdetails();
            objDaApproveLeave.DaGetLogoutLeaveRejectDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("approvelogout")]
        [HttpPost]

        public HttpResponseMessage PostApproveLogout(approvelogout values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaApproveLeave.DaPostApproveLogout(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("rejectlogout")]
        [HttpPost]

        public HttpResponseMessage PostRejectLogout(approvelogout values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaApproveLeave.DaPostRejectLogout(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //..............................* 4. OD APPROVAL Details *..................................//

        [ActionName("getODsummarydetails")]
        [HttpGet]

        public HttpResponseMessage GetODSummaryDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getODdetails values = new getODdetails();
            objDaApproveLeave.DaGetODSummaryDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getodleaveapprovedetails")]
        [HttpGet]

        public HttpResponseMessage GetODLeaveApproveDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavesummaryoddetails values = new getleavesummaryoddetails();
            objDaApproveLeave.DaGetODLeaveApproveDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getodleaverejectdetails")]
        [HttpGet]

        public HttpResponseMessage GetODLeaveRejectDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavesummaryoddetails values = new getleavesummaryoddetails();
            objDaApproveLeave.GetODLeaveRejectDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("approveOD")]
        [HttpPost]

        public HttpResponseMessage PostApproveOD(approveOD values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            var status = objDaApproveLeave.DaPostApproveOD(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("rejectOD")]
        [HttpPost]

        public HttpResponseMessage PostRejectOD(approveOD values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaApproveLeave.DaPostRejectOD(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //..............................* 5. PERMISSION APPROVAL  Details *..........................//

        [ActionName("getPermissionsummarydetails")]
        [HttpGet]

        public HttpResponseMessage GetPermissionSummaryDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getpermissiondetails values = new getpermissiondetails();
            objDaApproveLeave.GetPermissionSummaryDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getpermissionleaveapprovedetails")]
        [HttpGet]

        public HttpResponseMessage GetPermissionLeaveapproveDetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavesummarypermissiondetails values = new getleavesummarypermissiondetails();
            objDaApproveLeave.DaGetPermissionLeaveApproveDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getpermissionleaverejectdetails")]
        [HttpGet]

        public HttpResponseMessage getpermissionleaverejectdetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavesummarypermissiondetails values = new getleavesummarypermissiondetails();
            objDaApproveLeave.GetPermissionLeaveRejectDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("approvepermission")]
        [HttpPost]

        public HttpResponseMessage ApprovePermission(string permissiondtl_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            approvepermission values = new approvepermission();
            objDaApproveLeave.DapostApprovePermission(permissiondtl_gid, values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("rejectpermission")]
        [HttpPost]

        public HttpResponseMessage RejectPermission(string permissiondtl_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            rejectpermission values = new rejectpermission();
            objDaApproveLeave.DapostRejectPermission(permissiondtl_gid, values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //..............................* 6. COMPOFF APPROVAL Details *..............................//

        [ActionName("getCompoffsummarydetails")]
        [HttpGet]

        public HttpResponseMessage getCompoffsummarydetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getcompoffdetails values = new getcompoffdetails();
            objDaApproveLeave.DaGetCompoffSummaryDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getcompoffleaveapprovedetails")]
        [HttpGet]

        public HttpResponseMessage getcompoffleaveapprovedetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavesummarycompoffdetails values = new getleavesummarycompoffdetails();
            var status = objDaApproveLeave.DaGetCompoffLeaveApproveDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getcompoffleaverejectdetails")]
        [HttpGet]

        public HttpResponseMessage getcompoffleaverejectdetails()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            getleavesummarycompoffdetails values = new getleavesummarycompoffdetails();
            objDaApproveLeave.DaGetCompoffLeaveRejectDetails(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("approvecompoff")]
        [HttpPost]

        public HttpResponseMessage PostApproveCompoff(approvecompoff values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaApproveLeave.DapostCompoffApprove(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("rejectcompoff")]
        [HttpPost]

        public HttpResponseMessage RejectCompoff(string compensatoryoff_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            rejectcompoff values = new rejectcompoff();
            objDaApproveLeave.DapostRejectCompoff(compensatoryoff_gid, values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}