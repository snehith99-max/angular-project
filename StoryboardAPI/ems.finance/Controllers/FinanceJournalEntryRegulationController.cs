using ems.finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.finance.DataAccess;

namespace ems.finance.Controllers
{
    public class FinanceJournalEntryRegulation
    {

        [RoutePrefix("api/FinanceJournalEntryRegulation")]
        [Authorize]
        public class FinanceJournalEntryRegulationController : ApiController
        {
            session_values objgetgid = new session_values();
            logintoken getsessionvalues = new logintoken();
            DaFinanceJournalEntryRegulation objFinRegulation = new DaFinanceJournalEntryRegulation();
            //dateform objdateform =new dateform();
            [ActionName("RepostSalesJournals")]
            [HttpPost]
            public HttpResponseMessage PostSales(dateform objdateform)
            {
                result values = new result();
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                objFinRegulation.DaPostSlaes(objdateform);

                return Request.CreateResponse(HttpStatusCode.OK, objdateform);
            }
            [ActionName("RepostPurchaseJournals")]
            [HttpPost]
            public HttpResponseMessage PostPurchase(dateform value)
            {
                result values = new result();
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                objFinRegulation.DaPostPurchase(value);

                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            [ActionName("RepostReceiptJournals")]
            [HttpPost]
            public HttpResponseMessage PostReceipt(dateform value)
            {
                //results values = new results();
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                objFinRegulation.DaPostSalesReceipt(value);

                return Request.CreateResponse(HttpStatusCode.OK, value);
            }
            [ActionName("DaPostPBLPaymentJournals")]
            [HttpPost]
            public HttpResponseMessage PostPBLPayments(dateform value)
            {
                results values = new results();
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                objFinRegulation.DaPostPurchasePayment(value);

                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            [ActionName("DaPostEmployeeSalary")]
            [HttpPost]
            public HttpResponseMessage PostEmployeeSalary(dateform value)
            {
                results values = new results();
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                objFinRegulation.DaPostEmployeeSalary(value);

                return Request.CreateResponse(HttpStatusCode.OK, values);
            }

            [ActionName("PostDebtor")]
            [HttpPost]
            public HttpResponseMessage PostCreateDebtor(dateform value)
            {
                result values =new result();
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                objFinRegulation.DaPostDebtorLedger(values);

                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            [ActionName("PostCreditor")]
            [HttpPost]
            public HttpResponseMessage PostCreateCreditor(dateform value)
            {
                result values = new result();
                string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
                getsessionvalues = objgetgid.gettokenvalues(token);
                objFinRegulation.DaPostCreditorLedger(values);

                return Request.CreateResponse(HttpStatusCode.OK, values);
            }

        }

    }
}