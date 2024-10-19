using ems.pmr.DataAccess;
using ems.pmr.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PblDebitNote")]
    [Authorize]
    public class PblDebitNoteController : ApiController
    {
        session_values objGetValue = new session_values();
        logintoken getsession_values = new logintoken();
        DaPblDebitNote objdebit = new DaPblDebitNote();

        [ActionName("GetDebitNoteSummary")]
        [HttpGet]
        public HttpResponseMessage GetDebitNoteSummary()
        {
            MdlPblDebitNote values = new MdlPblDebitNote();
            objdebit.DaGetDebitNoteSummary(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetRaiseDebitNote")]
        [HttpGet]
        public HttpResponseMessage GetRaiseDebitNote()
        {
            MdlPblDebitNote values = new MdlPblDebitNote();
            objdebit.DaGetRaiseDebitNote(values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetRaiseDebitNoteAdd")]
        [HttpGet]
        public HttpResponseMessage GetRaiseDebitNoteAdd(string invoice_gid)
        {
            MdlPblDebitNote values = new MdlPblDebitNote();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = objGetValue.gettokenvalues(token);
            objdebit.DaGetRaiseDebitNoteAdd(invoice_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetDebitProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDebitProductSummary(string invoice_gid)
        {
            MdlPblDebitNote values = new MdlPblDebitNote();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = objGetValue.gettokenvalues(token);
            objdebit.DaGetRaiseDebitProductSummary(invoice_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("PostDebitNote")]
        [HttpPost]
        public HttpResponseMessage PostDebitNote(Postdebit_list values)
        {            
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = objGetValue.gettokenvalues(token);
            objdebit.DaPostDebitNote(getsession_values.employee_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetStockReturnDebitSummary")]
        [HttpGet]
        public HttpResponseMessage GetStockReturnDebitSummary(string invoice_gid)
        {
            MdlPblDebitNote values = new MdlPblDebitNote();
            objdebit.DaGetStockReturnSummary(invoice_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("GetStockReturnDebitProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetStockReturnDebitProductSummary(string invoice_gid)
        {
            MdlPblDebitNote values = new MdlPblDebitNote();
            objdebit.DaGetStockRetrunProductSummary(invoice_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("PostStockReturnDebit")]
        [HttpPost]
        public HttpResponseMessage PostStockReturnDebit(PostReturnStockDebit_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsession_values = objGetValue.gettokenvalues(token);
            objdebit.DaPostStockReturnDebit(getsession_values.employee_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
        [ActionName("DebitPDF")]
        [HttpGet]
        public HttpResponseMessage DebitPDF(string debitnote_gid, string invoice_gid)
        {
            MdlPblDebitNote values = new MdlPblDebitNote();
            var ls_response = new Dictionary<string, object>();
            ls_response = objdebit.DaDebitPDF(debitnote_gid, invoice_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, ls_response);
        }
        [ActionName("DeleteDebitNote")]
        [HttpGet]
        public HttpResponseMessage DeleteDebitNote(string debitnote_gid, string payment_gid)
        {
            MdlPblDebitNote values = new MdlPblDebitNote();
            objdebit.DaDeleteDebitNote(debitnote_gid, payment_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, values);
        }
    }
}