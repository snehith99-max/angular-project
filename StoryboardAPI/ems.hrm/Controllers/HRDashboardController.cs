using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.hrm.Controllers
{
    [RoutePrefix("api/HRDashboard")]
    [Authorize]
    public class HRDashboardController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHRDashboard objDaHRDashboard = new DaHRDashboard();

        [ActionName("GetTotalActiveEmployeeCount")]
        [HttpGet]
        public HttpResponseMessage GetTotalActiveEmployeeCount()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetTotalActiveEmployeeCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeePresentList")]
        [HttpGet]
        public HttpResponseMessage GetEmployeePresentList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetEmployeePresentList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeAbsentList")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeAbsentList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetEmployeeAbsentList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeLeaveList")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeLeaveList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetEmployeeLeaveList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTotalActiveEmployeeList")]
        [HttpGet]
        public HttpResponseMessage GetTotalActiveEmployeeList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetTotalActiveEmployeeList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTodayBirthdayCount")]
        [HttpGet]
        public HttpResponseMessage GetTodayBirthdayCount()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetTodayBirthdayCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTodayBirthdayList")]
        [HttpGet]
        public HttpResponseMessage GetTodayBirthdayList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetTodayBirthdayList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetUpcomingBirthdayCount")]
        [HttpGet]
        public HttpResponseMessage GetUpcomingBirthdayCount()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetUpcomingBirthdayCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetUpcomingBirthdayList")]
        [HttpGet]
        public HttpResponseMessage GetUpcomingBirthdayList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetUpcomingBirthdayList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetWorkAnniversaryCount")]
        [HttpGet]
        public HttpResponseMessage GetWorkAnniversaryCount()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetWorkAnniversaryCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetWorkAnniversaryList")]
        [HttpGet]
        public HttpResponseMessage GetWorkAnniversaryList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetWorkAnniversaryList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnProbationCount")]
        [HttpGet]
        public HttpResponseMessage GetOnProbationCount()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetOnProbationCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnProbationList")]
        [HttpGet]
        public HttpResponseMessage GetOnProbationList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetOnProbationList(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmpCountbyLocation")]
        [HttpGet]
        public HttpResponseMessage GetEmpCountbyLocation()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetEmpCountbyLocation(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }        

        [ActionName("GetTotalActiveEmployees")]
        [HttpGet]
        public HttpResponseMessage GetTotalActiveEmployees()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetTotalActiveEmployees(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetToDoListCount")]
        [HttpGet]
        public HttpResponseMessage GetToDoListCount(string leave_gid)
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetToDoListCount(values,leave_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("GetToDoList")]
        //[HttpGet]
        //public HttpResponseMessage GetToDoList()
        //{
        //    objDaHRDashboard.DaGetToDoList(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("GetToDoList")]
        [HttpGet]
        public HttpResponseMessage GetToDoList()
        {        
            MdlHRDashboard values = new MdlHRDashboard();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaGetToDoList(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLeaveHistoryDetails")] 
        [HttpGet]
        public HttpResponseMessage GetLeaveHistoryDetails()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetLeaveHistoryDetails(values); 
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLoginHistoryDetails")] 
        [HttpGet]
        public HttpResponseMessage GetLoginHistoryDetails()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetLoginHistoryDetails(values);  
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLogoutHistoryDetails")]
        [HttpGet]
        public HttpResponseMessage GetLogoutHistoryDetails()   
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetLogoutHistoryDetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOdHistoryDetails")]
        [HttpGet]
        public HttpResponseMessage GetOdHistoryDetails()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetOdHistoryDetails(values); 
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCompoffHistoryDetails")]
        [HttpGet]
        public HttpResponseMessage GetCompoffHistoryDetails() 
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetCompoffHistoryDetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetPermissionHistoryDetails")]
        [HttpGet]
        public HttpResponseMessage GetPermissionHistoryDetails()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaGetPermissionHistoryDetails(values); 
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("GetToDoLoginList")]
        //[HttpGet]
        //public HttpResponseMessage GetToDoLoginList()
        //{
        //    MdlHRDashboard values = new MdlHRDashboard();
        //    objDaHRDashboard.DaGetToDoLoginList(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}
        [ActionName("GetToDoLoginList")]
        [HttpGet]
        public HttpResponseMessage GetToDoLoginList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaGetToDoLoginList(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("GetToDoLogoutList")]
        //[HttpGet]  
        //public HttpResponseMessage GetToDoLogoutList()
        //{
        //    MdlHRDashboard values = new MdlHRDashboard();
        //    objDaHRDashboard.DaGetToDoLogoutList(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}


        [ActionName("GetToDoLogoutList")]
        [HttpGet]
        public HttpResponseMessage GetToDoLogoutList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaGetToDoLogoutList(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //[ActionName("GetToDoODList")]
        //[HttpGet]
        //public HttpResponseMessage GetToDoODList()
        //{
        //    MdlHRDashboard values = new MdlHRDashboard();
        //    objDaHRDashboard.DaGetToDoODList(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("GetToDoODList")]
        [HttpGet]
        public HttpResponseMessage GetToDoODList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaGetToDoODList(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //[ActionName("GetToDoPermissionList")]
        //[HttpGet]
        //public HttpResponseMessage GetToDoPermissionList() 
        //{ 
        //    MdlHRDashboard values = new MdlHRDashboard();
        //    objDaHRDashboard.DaGetToDoPermissionList(values);  
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("GetToDoPermissionList")]
        [HttpGet]
        public HttpResponseMessage GetToDoPermissionList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaGetToDoPermissionList(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("GetToDoCompoffList")]
        //[HttpGet]
        //public HttpResponseMessage GetToDoCompoffList()  
        //{
        //    MdlHRDashboard values = new MdlHRDashboard();
        //    objDaHRDashboard.DaGetToDoCompoffList(values);  
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("GetToDoCompoffList")]
        [HttpGet]
        public HttpResponseMessage GetToDoCompoffList()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaGetToDoCompoffList(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetempStatistics")]
        [HttpGet]
        public HttpResponseMessage GetempStatistics()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaempStatistics(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetempActivecount")]
        [HttpGet]
        public HttpResponseMessage GetempActivecount()
        {
            MdlHRDashboard values = new MdlHRDashboard();
            objDaHRDashboard.DaempActivecount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("approveleavesubmit")]
        [HttpPost]
        public HttpResponseMessage postapproveleavesubmit(approveleave values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.Daapproveleavesubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Approveevent")]
        [HttpPost]
        public HttpResponseMessage postApproveevent(approvesubmit values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaApproveevent(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Rejectleaveevent")]
        [HttpPost]
        public HttpResponseMessage postRejectleaveevent(approvesubmit values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaRejectleaveevent(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("LoginApprove")]
        [HttpPost]
        public HttpResponseMessage LoginApprove(loginapprove values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaLoginApprove(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Loginreject")]
        [HttpPost]
        public HttpResponseMessage Loginreject(loginapprove values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaLoginreject(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("LogoutApprove")]
        [HttpPost]
        public HttpResponseMessage LogoutApprove(logoutapprove values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaLogoutApprove(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Logoutreject")]
        [HttpPost]
        public HttpResponseMessage Logoutreject(logoutapprove values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaLogoutreject(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("CompoffApprove")]
        [HttpPost]
        public HttpResponseMessage CompoffApprove(compoffapprove values) 
        { 
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaCompoffApprove(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Compoffreject")]
        [HttpPost]
        public HttpResponseMessage Compoffreject(compoffapprove values) 
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token); 
            objDaHRDashboard.DaCompoffreject(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ApproveOD")]
        [HttpPost]
        public HttpResponseMessage ApproveOD(approveOD_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaApproveOD(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("RejectOD")]
        [HttpPost]
        public HttpResponseMessage RejectOD(approveOD_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaRejectOD(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Permissiontapprove")]
        [HttpPost]
        public HttpResponseMessage Permissiontapprove(permissionapprove_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHRDashboard.DaPermissiontapprove(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("PermissionReject")]
        [HttpPost]
        public HttpResponseMessage PermissionReject(permissionapprove_list values)
        {           
            objDaHRDashboard.DaPermissionReject(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}