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
    [RoutePrefix("api/PmrTrnGrn")]
    [Authorize]
    public class PmrTrnGrnController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrTrnGrn objpurchase = new DaPmrTrnGrn();

        [ActionName("GetGrninwardSummary")]
        [HttpGet]
        public HttpResponseMessage GetGrninwardSummary()
        {
            MdlPmrTrnGrn values = new MdlPmrTrnGrn();
            objpurchase.DaGrninwardSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);


        }

        [ActionName("Getaddgrnsummary")]
        [HttpGet]
        public HttpResponseMessage Getaddgrnsummary(string purchaseorder_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            MdlPmrTrnGrn values = new MdlPmrTrnGrn();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaGetaddgrnsummary(getsessionvalues.user_gid, purchaseorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Getsummaryaddgrn")]
        [HttpGet]
        public HttpResponseMessage Getsummaryaddgrn(string purchaseorder_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            MdlPmrTrnGrn values = new MdlPmrTrnGrn();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaGetsummaryaddgrnsummary(getsessionvalues.user_gid, purchaseorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("PostGrnSubmit")]
        [HttpPost]
        public HttpResponseMessage PostGrnSubmit(addgrn_lists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPostGrnSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetGRNPDF")]
        [HttpGet]
        public HttpResponseMessage GetGRNPDF(string grn_gid)
        {
            MdlPmrTrnGrn values = new MdlPmrTrnGrn();
            var response = new Dictionary<string, object>();
            response = objpurchase.DaGetGRNPDF(grn_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        // submit with file upload
        [ActionName("PostGrnSubmitUpload")]
        [HttpPost]
        public HttpResponseMessage PostGrnSubmitUpload()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objpurchase.DaPostGrnSubmitUpload(httpRequest, objResult, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("GetGRNViewProduct")]
        [HttpGet]
        public HttpResponseMessage GetGRNViewProduct(string grn_gid)
        {
            MdlPmrTrnGrn values = new MdlPmrTrnGrn();
            objpurchase.DaGetGRNViewProduct(grn_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

    }
}