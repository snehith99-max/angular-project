using ems.subscription.DataAccess;
using ems.subscription.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;


namespace ems.subscription.Controllers
{
    [RoutePrefix("api/SubTrnSubscrition")]
    [Authorize]
    public class SubTrnSubscritionController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSubManagement objsub = new DaSubManagement();

        [ActionName("GetsubscriptionSummary")]
        [HttpGet]
        public HttpResponseMessage GetsubscriptionSummary()
        {
            MdlSubManagement values = new MdlSubManagement();
            objsub.DaGetsubscriptionSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetServerSummary")]
        [HttpGet]
        public HttpResponseMessage GetServerSummary()
        {
            MdlSubManagement values = new MdlSubManagement();
            objsub.DaGetServerSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
       
        [ActionName("UpdateServer")]
        [HttpPost]
        public HttpResponseMessage UpdateServer(MdlSubManagement values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsub.DaUpdateServer(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetConsumerSummary")]
        [HttpGet]
        public HttpResponseMessage GetConsumerSummary()
        {
            MdlSubManagement values = new MdlSubManagement();
            objsub.DaGetConsumerSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("UpdateConsumer")]
        [HttpPost]
        public HttpResponseMessage UpdateConsumer(MdlSubManagement values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsub.DaUpdateConsumer(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostServer")]
        [HttpPost]
        public HttpResponseMessage PostServer(MdlSubManagement values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsub.DaPostServer(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetServerView")]
        [HttpGet]
        public HttpResponseMessage GetServerView(string server_gid)
        {
            MdlSubManagement values = new MdlSubManagement();
            objsub.DaGetServerView(server_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("InactiveConsumer")]
        [HttpPost]
        public HttpResponseMessage InactiveConsumer(consumerinactive values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsub.DaInactiveConsumer(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("InactiveConsumerHistory")]
        [HttpGet]
        public HttpResponseMessage InactiveConsumerHistory(string consumer_gid)
        {
            consumerhistory values = new consumerhistory();
            objsub.DaInactiveConsumerHistory(values, consumer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetConsumerEdit")]
        [HttpGet]
        public HttpResponseMessage GetConsumerEdit(string consumer_gid)
        {
            MdlSubManagement values = new MdlSubManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsub.DaGetConsumerEdit(values, consumer_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PopCountry")]
        [HttpGet]
        public HttpResponseMessage GetPopCountry()
        {
            MdlSubManagement values = new MdlSubManagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsub.DaPopCountry(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("ScriptDocumentUpload")]
        [HttpPost]
        public HttpResponseMessage ScriptDocumentUpload()
        {
            HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            scriptuploaddocument documentname = new scriptuploaddocument();
            objsub.DaPostScriptDocumentUpload(httpRequest, documentname, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, documentname);
        }
        [ActionName("PostConsumer")]
        [HttpPost]
        public HttpResponseMessage PostConsumer(MdlSubManagement values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsub.DaPostConsumer(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}