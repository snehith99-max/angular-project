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

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrRptVendorLedgerreport")]
    [Authorize]
    public class PmrRptVendorledgerreportController: ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrRptVendorledgerreport objpurchase = new DaPmrRptVendorledgerreport();

        [ActionName("GetVendorledgerReportSummary")]
        [HttpGet]
        public HttpResponseMessage GetVendorledgerReportSummary()
        {
            MdlPmrRptVendorledgerreport values = new MdlPmrRptVendorledgerreport();
            objpurchase.DaGetVendorledgerReportSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("GetCreditorReportViewdate")]
        [HttpGet]
        public HttpResponseMessage GetCreditorReportViewdate(string vendor_gid,string from_date, string to_date)
        {
            MdlPmrRptVendorledgerreport values = new MdlPmrRptVendorledgerreport();
            objpurchase.DaGetCreditorReportViewdate(vendor_gid,from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetVendorReportView")]
        [HttpGet]
        public HttpResponseMessage GetVendorReportView(string vendor_gid)
        {
            MdlPmrRptVendorledgerreport values = new MdlPmrRptVendorledgerreport();
            objpurchase.DaGetVendorReportView(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetVendorPaymentReportView")]
        [HttpGet]
        public HttpResponseMessage GetVendorPaymentReportView(string vendor_gid)
        {
            MdlPmrRptVendorledgerreport values = new MdlPmrRptVendorledgerreport();
            objpurchase.DaGetVendorPaymentReportView(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


    }

}