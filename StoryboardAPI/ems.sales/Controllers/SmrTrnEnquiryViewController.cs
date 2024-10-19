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
    [RoutePrefix("api/SmrTrnEnquiryView")]
    [Authorize]
    public class SmrTrnEnquiryViewController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnEnquiryView objsales = new DaSmrTrnEnquiryView();


        // VIEW SUMMARY

        [ActionName("GetEnquiryView")]
        [HttpGet]
        public HttpResponseMessage GetEnquiryView(string enquiry_gid)
        {
            MdlSmrTrnEnquiryView objresult = new MdlSmrTrnEnquiryView();
            objsales.DaGetEnquiryView(enquiry_gid, getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // VIEW ENQUIRY PRODUCT SUMMARY

        [ActionName("GetEnquiryProductView")]
        [HttpGet]
        public HttpResponseMessage GetEnquiryProductView(string enquiry_gid)
        {
            MdlSmrTrnEnquiryView objresult = new MdlSmrTrnEnquiryView();
            objsales.DaGetEnquiryProductView(enquiry_gid, getsessionvalues.employee_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}