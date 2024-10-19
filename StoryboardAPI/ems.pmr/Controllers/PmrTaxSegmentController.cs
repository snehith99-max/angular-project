using ems.pmr.DataAccess;
using ems.pmr.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using static OfficeOpenXml.ExcelErrorValue;

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrTaxSegment")]
    public class PmrTaxSegmentController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrTaxSegment objTaxSegment = new DaPmrTaxSegment();

        [ActionName("GetTaxSegmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetTaxSegmentSummary()
        {
            MdlPmrTaxSegment values = new MdlPmrTaxSegment();
            objTaxSegment.DaGetTaxSegmentSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostPmrTaxSegment")]
        [HttpPost]
        public HttpResponseMessage PostPmrTaxSegment(PmrTaxSegmentSummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objTaxSegment.PostPmrTaxSegment(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdatedTaxSegmentSummary")]
        [HttpPost]
        public HttpResponseMessage UpdatedTaxSegmentSummary(PmrTaxSegmentSummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objTaxSegment.DaUpdatedTaxSegmentSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetWithStateSegmentSummary")]
        [HttpGet]

        public HttpResponseMessage GetWithStateSegmentSummary()
        {
            MdlPmrTaxSegment values = new MdlPmrTaxSegment();
            objTaxSegment.DaGetTotalWithinState(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetTaxSegmentDropDown")]
        [HttpGet]
        public HttpResponseMessage GetTaxSegmentDropDown(string taxsegment_gid)
        {
            MdlPmrTaxSegment values = new MdlPmrTaxSegment();
            objTaxSegment.DaGetTaxSegmentDropDown(taxsegment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetInterstateSegmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetInterstateSegmentSummary()
        {
            MdlPmrTaxSegment values = new MdlPmrTaxSegment();
            objTaxSegment.DaGetInterstateSegmentSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOthersSegmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetOthersSegmentSummary()
        {
            MdlPmrTaxSegment values = new MdlPmrTaxSegment();
            objTaxSegment.DaGetOthersSegmentSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOverseasSegmentSummary")]
        [HttpGet]
        public HttpResponseMessage GetOverseasSegmentSummary()
        {
            MdlPmrTaxSegment values = new MdlPmrTaxSegment();
            objTaxSegment.DaGetOverseasSegmentSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTotalVendorSegment")]
        [HttpGet]
        public HttpResponseMessage GetTotalVendorSegment()
        {
            MdlPmrTaxSegment values = new MdlPmrTaxSegment();
            objTaxSegment.DaGetTotalVendorSegment(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetUnassignSummary")]
        [HttpGet]
        public HttpResponseMessage GetUnassignSummary()
        {
            MdlPmrTaxSegment values = new MdlPmrTaxSegment();
            objTaxSegment.DaGetUnassignSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostTaxsegmentMoveOn")]
        [HttpPost]

        public HttpResponseMessage PostTaxsegmentMoveOn(PmrPostTaxsegment values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objTaxSegment.DaPmrPostTaxsegmentMoveOn(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetVendorrCount")]
        [HttpGet]
        public HttpResponseMessage GetVendorrCount()
        {
            MdlPmrTaxSegment values = new MdlPmrTaxSegment();
            objTaxSegment.DaGetVendorCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteTaxSegmentSummary")]
        [HttpGet]
        public HttpResponseMessage DeleteTaxSegmentSummary(string taxsegment_gid)
        {
            MdlPmrTaxSegment objresult = new MdlPmrTaxSegment();
            objTaxSegment.DaDeleteTaxSegmentSummary(taxsegment_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetTaxSegmentAssignVendorSummary")]
        [HttpGet]
        public HttpResponseMessage GetTaxSegmentAssignVendorSummary()
        {
            MdlPmrTaxSegment values = new MdlPmrTaxSegment();
            objTaxSegment.DaGetTaxSegmentAssignVendorSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetTaxSegmentUnAssignVendorSummary")]
        [HttpGet]
        public HttpResponseMessage GetTaxSegmentUnAssignVendorSummary(string taxsegment_gid)
        {
            MdlPmrTaxSegment values = new MdlPmrTaxSegment();
            objTaxSegment.DaGetTaxSegmentunAssignVendorSummary(taxsegment_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostPmrTaxSegment2Vendor")]
        [HttpPost]
        public HttpResponseMessage PostPmrTaxSegment2Vendor(PostPmrTaxSegment2Vendor_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objTaxSegment.DaPostPmrTaxSegment2Vendor(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostPmrTaxSegmentunassign2Vendor")]
        [HttpPost]
        public HttpResponseMessage PostPmrTaxSegmentunassign2Vendor(PostPmrTaxSegment2unassignVendor_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objTaxSegment.DaPostPmrTaxSegment2unassignVendor(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}