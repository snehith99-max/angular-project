using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.finance.DataAccess;
using ems.finance.Models;

namespace ems.finance.Controllers
{
    [Authorize]
    [RoutePrefix("api/JournalEntry")]
    public class JournalEntryController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaJournalEntry objdacreditcard = new DaJournalEntry();

        [ActionName("GetJournalEntrySummary")]
        [HttpGet]
        public HttpResponseMessage GetJournalEntrySummary()
        {
            MdlJournalEntry values = new MdlJournalEntry();
            objdacreditcard.DaGetJournalEntrySummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetJournalEntrySummarys")]
        [HttpGet]
        public HttpResponseMessage GetJournalEntrySummarys()
        {
            MdlJournalEntry values = new MdlJournalEntry();
            objdacreditcard.DaGetJournalEntrySummarys(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetJournalEntryTransaction")]
        [HttpGet]
        public HttpResponseMessage GetJournalEntryTransaction(string journal_gid)
        {
            MdlJournalEntry objresult = new MdlJournalEntry();
            objdacreditcard.DaGetJournalEntryTransaction(journal_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetAccountGroupDetails")]
        [HttpGet]
        public HttpResponseMessage GetAccountGroupDetails()
        {
            MdlJournalEntry values = new MdlJournalEntry();
            objdacreditcard.DaGetAccountGroupDetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetAccountNameDetails")]
        [HttpGet]
        public HttpResponseMessage GetAccountNameDetails()
        {
            MdlJournalEntry values = new MdlJournalEntry();
            objdacreditcard.DaGetAccountNameDetails(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetAccountNameBaseonGroup")]
        [HttpGet]
        public HttpResponseMessage GetAccountNameBaseonGroup(string accountGroup)
        {
            MdlJournalEntry objresult = new MdlJournalEntry();
            objdacreditcard.DaGetAccountNameBaseonGroup(accountGroup, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("PostJournalEntry")]
        [HttpPost]
        public HttpResponseMessage PostJournalEntry(postjournal_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            results objResult = new results();
            objdacreditcard.DaPostJournalEntry(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostJournalEntryWithDoc")]
        [HttpPost]
        public HttpResponseMessage PostJournalEntryWithDoc()
        {
            HttpRequest httpRequest;
            //Postassetlocationcreation values = new Postassetlocationcreation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            results values = new results();
            objdacreditcard.DaPostJournalEntryWithDoc(httpRequest, values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteJounralEntry")]
        [HttpGet]
        public HttpResponseMessage DeleteJounralEntry(string journal_gid)
        {
            results values = new results();
            objdacreditcard.DaDeleteJounralEntry(journal_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetJournalEntrySummaryEdit")]
        [HttpGet]
        public HttpResponseMessage GetJournalEntrySummaryEdit(string journal_gid)
        {
            MdlJournalEntry objresult = new MdlJournalEntry();
            objdacreditcard.DaGetJournalEntrySummaryEdit(journal_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetJournalEntrySummaryEditDtl")]
        [HttpGet]
        public HttpResponseMessage GetJournalEntrySummaryEditDtl(string journal_gid)
        {
            MdlJournalEntry objresult = new MdlJournalEntry();
            objdacreditcard.DaGetJournalEntrySummaryEditDtl(journal_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("UpdateJournalEntry")]
        [HttpPost]
        public HttpResponseMessage UpdateJournalEntry(updatejournal_list values)
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            results objResult = new results();
            objdacreditcard.DaUpdateJournalEntry(values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateJournalEntryWithDoc")]
        [HttpPost]
        public HttpResponseMessage UpdateJournalEntryWithDoc()
        {
            HttpRequest httpRequest;
            //Postassetlocationcreation values = new Postassetlocationcreation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            results values = new results();
            objdacreditcard.DaUpdateJournalEntryWithDoc(httpRequest, values, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteJounralDtlEntry")]
        [HttpGet]
        public HttpResponseMessage DeleteJounralDtlEntry(string journaldtl_gid)
        {
            results values = new results();
            objdacreditcard.DaDeleteJounralDtlEntry(journaldtl_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}