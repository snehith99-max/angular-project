using ems.inventory.DataAccess;
using ems.inventory.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.inventory.Controllers
{
    [RoutePrefix("api/ImsTrnOpenDcSummary")]
    [Authorize]
    public class ImsTrnOpenDcSummaryController :ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnOpenDcSummary objDaInventory = new DaImsTrnOpenDcSummary();

        [ActionName("GetImsTrnOpenDeliveryOrderSummary")]
        [HttpGet]
        public HttpResponseMessage GetImsTrnOpenDeliveryOrderSummary()
        {

            MdlImsTrnOpenDCSummary values = new MdlImsTrnOpenDCSummary();
            objDaInventory.DaGetImsTrnOpenDeliveryOrderSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetImsTrnOpenDcAddSummary")]
        [HttpGet]
        public HttpResponseMessage GetImsTrnOpenDcAddSummary()
        {

            MdlImsTrnOpenDCSummary values = new MdlImsTrnOpenDCSummary();
            objDaInventory.DaGetImsTrnOpenDcAddSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOpenDcUpdate")]
        [HttpGet]
        public HttpResponseMessage GetOpenDcUpdate(string salesorder_gid)
        {
            MdlImsTrnOpenDCSummary objresult = new MdlImsTrnOpenDCSummary();
            objDaInventory.DaGetOpenDcUpdate(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("DaGetOnChangeTerms")]
        [HttpGet]
        public HttpResponseMessage DaGetOnChangeTerms(string template_gid)
        {
            MdlImsTrnOpenDCSummary objresult = new MdlImsTrnOpenDCSummary();
            objDaInventory.DaGetOnChangeTerms(template_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetOpenDcUpdateProd")]
        [HttpGet]
        public HttpResponseMessage GetOpenDcUpdateProd(string salesorder_gid)
        {
            MdlImsTrnOpenDCSummary objresult = new MdlImsTrnOpenDCSummary();
            objDaInventory.DaGetOpenDcUpdateProd(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("PostOpenDcSubmit")]
        [HttpPost]
        public HttpResponseMessage PostOpenDcSubmit(opendc_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostOpenDcSubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        //download Report files
        [ActionName("GetOpenDCRpt")]
        [HttpGet]
        public HttpResponseMessage GetOpenDCRpt(string directorder_gid)
        {
            MdlImsTrnOpenDCSummary values = new MdlImsTrnOpenDCSummary();
            var ls_response = new Dictionary<string, object>();
            ls_response = objDaInventory.DaGetOpenDCRpt(directorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }

        //open Dc for manoj_bhavan
        [ActionName("GetOpenDCpdf")]
        [HttpGet]
        public HttpResponseMessage GetOpenDCpdf(string directorder_gid)
        {
            MdlImsTrnOpenDCSummary values = new MdlImsTrnOpenDCSummary();
            var ls_response = new Dictionary<string, object>();
            ls_response = objDaInventory.DaGetOpenDCpdf(directorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }



        [ActionName("GetImsTrnOpenDCSummary")]
        [HttpGet]
        public HttpResponseMessage GetImsTrnOpenDCSummary()
        {

            MdlImsTrnOpenDCSummary values = new MdlImsTrnOpenDCSummary();
            objDaInventory.DaGetImsTrnOpenDCSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetImsTrnDCBranch")]
        [HttpGet]
        public HttpResponseMessage GetImsTrnDCBranch()
        {

            MdlImsTrnOpenDCSummary values = new MdlImsTrnOpenDCSummary();
            objDaInventory.DaGetImsTrnDCBranch(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("PostDcproduct")]
        [HttpPost]
        public HttpResponseMessage PostDcproduct(imsproductissue_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostDcproduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GettmpdcProduct")]
        [HttpGet]
        public HttpResponseMessage GettmpdcProduct()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnOpenDCSummary values = new MdlImsTrnOpenDCSummary();
            objDaInventory.DaGettmpdcProduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("openDCSubmit")]
        [HttpPost]
        public HttpResponseMessage openDCSubmit(opendcnew_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaopenDCSubmit(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //View DC
        [ActionName("GetViewDCSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewDCSummary(string directorder_gid)
        {
            MdlImsTrnOpenDCSummary objresult = new MdlImsTrnOpenDCSummary();
            objDaInventory.DaGetViewDCSummary(directorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetdcProduct")]
        [HttpGet]
        public HttpResponseMessage GetdcProduct(string directorder_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnOpenDCSummary values = new MdlImsTrnOpenDCSummary();
            objDaInventory.DaGetdcProduct(getsessionvalues.user_gid, directorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOpendcViewProduct")]
        [HttpGet]
        public HttpResponseMessage GetOpendcViewProduct(string salesorder_gid)
        {
            MdlImsTrnOpenDCSummary objresult = new MdlImsTrnOpenDCSummary();
            objDaInventory.DaGetOpendcViewProduct(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }




    }
}