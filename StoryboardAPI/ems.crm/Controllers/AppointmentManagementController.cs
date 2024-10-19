using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/AppointmentManagement")]
    public class AppointmentManagementController:ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaAppointmentManagement objDaAppointmentManagement = new DaAppointmentManagement();

        [ActionName("GetLeaddropdown")]
        [HttpGet]
        public HttpResponseMessage GetLeaddropdown()
        {
            MdlAppointmentManagement values = new MdlAppointmentManagement();
            objDaAppointmentManagement.DaGetLeaddropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        } 
        [ActionName("Getbussinessverticledropdown")]
        [HttpGet]
        public HttpResponseMessage Getbussinessverticledropdown()
        {
            MdlAppointmentManagement values = new MdlAppointmentManagement();
            objDaAppointmentManagement.DaGetbussinessverticledropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("PostAppointment")]
        [HttpPost]
        public HttpResponseMessage PostAppointment(Postappointment_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaAppointmentManagement.DaPostAppointment(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetAppointmentsummary")]
        [HttpGet]
        public HttpResponseMessage GetAppointmentsummary()
        {
            MdlAppointmentManagement values = new MdlAppointmentManagement();
            objDaAppointmentManagement.DaGetAppointmentsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }  
        [ActionName("GetAppointmentTiles")]
        [HttpGet]
        public HttpResponseMessage GetAppointmentTiles()
        {
            MdlAppointmentManagement values = new MdlAppointmentManagement();
            objDaAppointmentManagement.DaGetAppointmentTiles(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }        
        [ActionName("GetTeamdropdown")]
        [HttpGet]
        public HttpResponseMessage GetTeamdropdown()
        {
            MdlAppointmentManagement values = new MdlAppointmentManagement();
            objDaAppointmentManagement.DaGetTeamdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostAssignedEmployee")]
        [HttpPost]
        public HttpResponseMessage PostAssignedEmployee(PostAssignedEmployee_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaAppointmentManagement.DaPostAssignedEmployee(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }    
        [ActionName("Posteditappointment")]
        [HttpPost]
        public HttpResponseMessage Posteditappointment(Posteditappointment_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaAppointmentManagement.DaPosteditappointment(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("shopifyenquiry")]
        [HttpGet]
        public HttpResponseMessage shopifyenquiry()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            objDaAppointmentManagement.Dashopifyenquiry(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("shopifyenquirysummary")]
        [HttpGet]
        public HttpResponseMessage shopifyenquirysummary()
        {
            MdlGmailCampaign values = new MdlGmailCampaign();
            objDaAppointmentManagement.Dashopifyenquirysummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}