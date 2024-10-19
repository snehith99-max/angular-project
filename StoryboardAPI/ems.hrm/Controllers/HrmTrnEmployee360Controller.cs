using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.hrm.Controllers
{
    [Authorize]
    [RoutePrefix("api/HrmTrnEmployee360")]
    public class HrmTrnEmployee360Controller : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsession_values = new logintoken();
        DaHrmTrnEmployee360 ObjdaEmployee360 = new DaHrmTrnEmployee360();


        [ActionName("Getemployeedatabinding")]
        [HttpGet]
        public HttpResponseMessage Getemployeedatabinding(string employee_gid)
        {
            MdlHrmTrnEmployee360 values = new MdlHrmTrnEmployee360();
            ObjdaEmployee360.DaGetemployeedatabinding(values, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getemployeeinformation")]
        [HttpGet]
        public HttpResponseMessage Getemployeeinformation(string employee_gid)
        {
            MdlHrmTrnEmployee360 values = new MdlHrmTrnEmployee360();
            ObjdaEmployee360.DaGetemployeeinformation(values, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getemployeegeneraldetails")]
        [HttpGet]
        public HttpResponseMessage Getemployeegeneraldetails(string employee_gid)
        {
            MdlHrmTrnEmployee360 values = new MdlHrmTrnEmployee360();
            ObjdaEmployee360.DaGetemployeegeneraldetails(values, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getemployeeaccount")]
        [HttpGet]
        public HttpResponseMessage Getemployeeaccount(string employee_gid)
        {
            MdlHrmTrnEmployee360 values = new MdlHrmTrnEmployee360();
            ObjdaEmployee360.Dagetemployeeaccount(values, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("getemployeeaddress")]
        [HttpGet]
        public HttpResponseMessage getemployeeaddress(string employee_gid)
        {
            MdlHrmTrnEmployee360 values = new MdlHrmTrnEmployee360();
            ObjdaEmployee360.getemployeeaddress(values, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getpaymentdetails")]
        [HttpGet]
        public HttpResponseMessage Getpaymentdetails(string employee_gid)
        {
            MdlHrmTrnEmployee360 values = new MdlHrmTrnEmployee360();
            ObjdaEmployee360.DagetGetpaymentdetails(values, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getloandetails")]
        [HttpGet]
        public HttpResponseMessage Getloandetails(string employee_gid)
        {
            MdlHrmTrnEmployee360 values = new MdlHrmTrnEmployee360();
            ObjdaEmployee360.DagetGetloandetails(values, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getattendancetails")]
        [HttpGet]
        public HttpResponseMessage Getattendancetails(string employee_gid)
        {
            MdlHrmTrnEmployee360 values = new MdlHrmTrnEmployee360();
            ObjdaEmployee360.DagetGetattendancetails(values, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getststutorydetails")]
        [HttpGet]
        public HttpResponseMessage Getststutorydetails(string employee_gid)
        {
            MdlHrmTrnEmployee360 values = new MdlHrmTrnEmployee360();
            ObjdaEmployee360.DagetGetststutorydetails(values, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getdocumentdetailsdetails")]
        [HttpGet]
        public HttpResponseMessage Getdocumentdetailsdetails(string employee_gid)
        {
            MdlHrmTrnEmployee360 values = new MdlHrmTrnEmployee360();
            ObjdaEmployee360.DagetGetdocumentdetails(values, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getworkexperiencedetails")]
        [HttpGet]
        public HttpResponseMessage Getworkexperiencedetails(string employee_gid)
        {
            MdlHrmTrnEmployee360 values = new MdlHrmTrnEmployee360();
            ObjdaEmployee360.DagetGetworkexperiencedetails(values, employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}