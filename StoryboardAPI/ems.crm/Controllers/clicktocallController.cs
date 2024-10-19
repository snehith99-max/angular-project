using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Linq;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/clicktocall")]
    public class clicktocallController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        Daclicktocall objDaclicktocall = new Daclicktocall();

        [ActionName("callSummary")]
        [HttpGet]
        public HttpResponseMessage callSummary()
        {
            Mdlclicktocall values = new Mdlclicktocall();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaclicktocall.DaCallSummary(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("allcallSummary")]
        [HttpGet]
        public HttpResponseMessage allcallSummary()
        {
            Mdlclicktocall values = new Mdlclicktocall();
            objDaclicktocall.DaallCallSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getaudioplay")]
        [HttpGet]
        public HttpResponseMessage Getaudioplay(string uniqueid)
        {
            calllog_report values = new calllog_report();
            objDaclicktocall.Daaudioplay(uniqueid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("customercall")]
        [HttpPost]
        public HttpResponseMessage customercall(calling values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();
            objResult = objDaclicktocall.Dacustomercall(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        [ActionName("Getlogreport")]
        [HttpGet]
        public HttpResponseMessage Getlogreport(string phone_number)
        {

            Mdlclicktocall values = new Mdlclicktocall();
              objDaclicktocall.DaGetlogreport(values, phone_number);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Postaddleads")]
        [HttpPost]
        public HttpResponseMessage Postaddleads(addleadvalue values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();    
            objResult = objDaclicktocall.DaPostaddleads(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);

        }
        [ActionName("UpdatedRemarks")]
        [HttpPost]
        public HttpResponseMessage UpdatedRemarks(calling values)
        {
              result objResult = new result();
              objResult = objDaclicktocall.DaUpdatedRemarks(values);
              return Request.CreateResponse(HttpStatusCode.OK, objResult);
           
        }
        [ActionName("GetagentSummary")]
        [HttpGet]
        public HttpResponseMessage GetagentSummary()
        {
            Mdlclicktocall values = new Mdlclicktocall();
            objDaclicktocall.DaGetagentSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getdaywisechartforclicktocall")]
        [HttpGet]
        public HttpResponseMessage Getdaywisechartforclicktocall()
        {
            Mdlclicktocall values = new Mdlclicktocall();
            objDaclicktocall.DaGetdaywisechart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getweekwiseclicktocallchart")]
        [HttpGet]
        public HttpResponseMessage Getweekwiseclicktocallchart()
        {
            Mdlclicktocall values = new Mdlclicktocall();
            objDaclicktocall.DaGetweekwiseclicktocallchart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("updatelead")]
        [HttpGet]
        public HttpResponseMessage Getlistofthreads()
        {
            Mdlclicktocall values = new Mdlclicktocall();
            objDaclicktocall.Daupdatelead(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);


        }
    }
}
