using ems.outlet.Dataaccess;
using ems.outlet.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ems.outlet.Controller
{
    [RoutePrefix("api/OtlWhatsAppOrder")]
    [Authorize]
    public class OtlWhatsAppOrderController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaOtlWhatsAppOrder objwhatsapp = new DaOtlWhatsAppOrder();

        [ActionName("Getwhatsappordersummary")]
        [HttpGet]
        public HttpResponseMessage Getwhatsappordersummary()
        {
            MdlOtlWhatsAppOrder values = new MdlOtlWhatsAppOrder();
            objwhatsapp.DaGetwhatsappordersummary(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetViewwhatsapporderSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewsalesorderSummary(string kot_gid)
        {
            whatsappordersummary_list values = new whatsappordersummary_list();
            objwhatsapp.DaGetViewwhatsapporderSummary(kot_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("onapprovewhatapporder")]
        [HttpGet]
        public HttpResponseMessage onapprovewhatapporder(string kot_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlOtlWhatsAppOrder values = new MdlOtlWhatsAppOrder();
            objwhatsapp.Daonapprovewhatapporder(kot_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Updatewtsorderpayment")]
        [HttpGet]
        public HttpResponseMessage Updatewtsorderpayment(string kot_gid)
        {
            result objresult = new result();
            objwhatsapp.DaUpdatewtsorderpayment(kot_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("completeorder")]
        [HttpGet]
        public HttpResponseMessage completeorder(string kot_gid)
        {
            result objresult = new result();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objwhatsapp.Dacompleteorder(kot_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}