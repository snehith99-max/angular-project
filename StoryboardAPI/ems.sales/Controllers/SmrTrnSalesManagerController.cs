using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.sales.Controllers
{
    [Authorize]
    [RoutePrefix("api/SmrTrnSalesManager")]
    public class SmrTrnSalesManagerController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnSalesManager objDasales = new DaSmrTrnSalesManager();

        //total summary
        [ActionName("GetSalesManagerTotal")]
        [HttpGet]
        public HttpResponseMessage GetSalesManagerTotal()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.DaGetSalesManagerTotal(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        //complete summary
        [ActionName("GetSalesManagerComplete")]
        [HttpGet]
        public HttpResponseMessage GetSalesManagerComplete()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.DaGetSalesManagerComplete(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        //prospect summary
        [ActionName("GetSalesManagerProspect")]
        [HttpGet]
        public HttpResponseMessage GetSalesManagerProspect()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.DaGetSalesManagerProspect(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        //potential summary
        [ActionName("GetSalesManagerPotential")]
        [HttpGet]
        public HttpResponseMessage GetSalesManagerPotential()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.DaGetSalesManagerPotential(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }


        //Drop summary
        [ActionName("GetSalesManagerdrop")]
        [HttpGet]
        public HttpResponseMessage GetSalesManagerdrop()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.DaGetSalesManagerDrop(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }


        //Count 
        [ActionName("GetSmrTrnManagerCount")]
        [HttpGet]
        public HttpResponseMessage GetSmrTrnManagerCount()
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.DaGetSmrTrnManagerCount(getsessionvalues.employee_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Charts for customer,quotation,enquiry and order

        [ActionName("customercount")]
        [HttpGet]
        public HttpResponseMessage customercount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.Dacustomercount(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("quotationchartcount")]
        [HttpGet]
        public HttpResponseMessage quotationchartcount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.Daquotationchartcount(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("enquirychartcount")]
        [HttpGet]
        public HttpResponseMessage enquirychartcount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.Daenquirychartcount(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("saleschartcount")]
        [HttpGet]
        public HttpResponseMessage saleschartcount()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.Dasaleschartcount(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("teamactivitySummary")]
        [HttpGet]
        public HttpResponseMessage teamactivitysummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.DateamactivitySummary(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSalesTeamSummary")]
        [HttpGet]
        public HttpResponseMessage GetSalesTeamSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.DaGetSalesTeamSummary(getsessionvalues.employee_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        // overall Count for the Chart
        [ActionName("Getsaleschart")]
        [HttpGet]
        public HttpResponseMessage Getsaleschart()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnSalesManager values = new MdlSmrTrnSalesManager();
            objDasales.DaGetsaleschart(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}