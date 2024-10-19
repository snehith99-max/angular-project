using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.hrm.Controllers
{
    [Authorize]
    [RoutePrefix("api/HrmTrnProfileManagement")]

    public class HrmTrnProfileManagementController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmTrnProfileManagement objDaHrmTrnProfileManagement = new DaHrmTrnProfileManagement();

        [ActionName("UpdatePersonalDetails")]
        [HttpPost]
        public HttpResponseMessage UpdatePersonalDetails(personaldetails values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaUpdatePersonalDetails(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditEmployee")]
        [HttpGet]
        public HttpResponseMessage GetEditEmployee()
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaGetEditEmployee(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        [ActionName("UpdatePassword")]
        [HttpPost]
        public HttpResponseMessage UpdatePassword(passwordupdate values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaUpdatePassword(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("changepasswordcheck")]
        //[HttpPost]
        //public HttpResponseMessage changepasswordcheck(passwordchange values)
        //{
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = Objgetgid.gettokenvalues(token);
        //    objDaHrmTrnProfileManagement.Dachangepasswordcheck(getsessionvalues.user_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}



        [ActionName("WorkExperienceSubmit")]
        [HttpPost]
        public HttpResponseMessage WorkExperienceSubmit(workexperience values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaWorkExperience(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("NominationSubmit")]
        [HttpPost]
        public HttpResponseMessage NominationSubmit(nomination values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaNomination(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("StatutorySubmit")]
        [HttpPost]
        public HttpResponseMessage StatutorySubmit(statutory values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaStatutorySubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("EmergencySubmit")]
        [HttpPost]
        public HttpResponseMessage EmergencySubmit(emergencycontact values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaEmergencyContact(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("DependentSubmit")]
        [HttpPost]
        public HttpResponseMessage DependentSubmit(dependent values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaDependent(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("EducationSubmit")]
        [HttpPost]
        public HttpResponseMessage EducationSubmit(education values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaEducation(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetWorkExperienceSummary")]
        [HttpGet]
        public HttpResponseMessage GetWorkExperienceSummary()
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaGetWorkExperienceSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetNominationSummary")]
        [HttpGet]
        public HttpResponseMessage GetNominationSummary()
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaGetNominationSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetStatutorySummary")]
        [HttpGet]
        public HttpResponseMessage GetStatutorySummary()
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaGetStatutorySummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmergencyContactSummary")]
        [HttpGet]
        public HttpResponseMessage GetEmergencyContactSummary()
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaGetEmergencyContactSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDependentSummary")]
        [HttpGet]
        public HttpResponseMessage GetDependentSummary()
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaGetDependentSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEducationSummary")]
        [HttpGet]
        public HttpResponseMessage GetEducationSummary()
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaGetEducationSummary(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
       
        //[ActionName("GetCompanyPolicies")]
        //[HttpGet]
        //public HttpResponseMessage GetCompanyPolicies()
        //{
        //    MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
        //    objDaHrmTrnProfileManagement.DaGetCompanyPolicies(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}
        [ActionName("employeeList")]
        [HttpGet]
        public HttpResponseMessage employeeList()
        {
            MdlHrmTrnProfileManagement objresult = new MdlHrmTrnProfileManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaHrmTrnProfileManagement.DaGetEmployeeList(getsessionvalues.user_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        //[ActionName("GetBloodGroup")]
        //[HttpGet]
        //public HttpResponseMessage GetBloodGroup()
        //{
        //    MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
        //    objDaHrmTrnProfileManagement.DaGetBloodGroup(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("Getrelationshipwithemployee")]
        [HttpGet]
        public HttpResponseMessage Getrelationshipwithemployee()
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            objDaHrmTrnProfileManagement.DaGetrelationshipwithemployee(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteDependent")]
        [HttpGet]
        public HttpResponseMessage DeleteDependent(string params_gid)
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            objDaHrmTrnProfileManagement.DaDeleteDependent(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteEducation")]
        [HttpGet]
        public HttpResponseMessage DeleteEducation(string params_gid)
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            objDaHrmTrnProfileManagement.DaDeleteEducation(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteEmergency")]
        [HttpGet]
        public HttpResponseMessage DeleteEmergency(string params_gid)
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            objDaHrmTrnProfileManagement.DaDeleteEmergency(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteStatutory")]
        [HttpGet]
        public HttpResponseMessage DeleteStatutory(string params_gid)
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            objDaHrmTrnProfileManagement.DaDeleteStatutory(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteNomination")]
        [HttpGet]
        public HttpResponseMessage DeleteNomination(string params_gid)
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            objDaHrmTrnProfileManagement.DaDeleteNomination(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteWorkExperience")]
        [HttpGet]
        public HttpResponseMessage DeleteWorkExperience(string params_gid)
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            objDaHrmTrnProfileManagement.DaDeleteWorkExperience(params_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCompanyPolicysummary")]
        [HttpGet]
        public HttpResponseMessage GetCompanyPolicysummary()
        {
            MdlHrmTrnProfileManagement values = new MdlHrmTrnProfileManagement();
            objDaHrmTrnProfileManagement.DaGetCompanyPolicysummary( values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}