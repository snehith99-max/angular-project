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
    [RoutePrefix("api/PurchaseLedger")]
    [Authorize]
    public class PurchaseLedgerController:ApiController
    {
            session_values Objgetgid = new session_values();
            logintoken getsessionvalues = new logintoken();
            DaPurchaseLedger objpurchase = new DaPurchaseLedger();

            [ActionName("GetPurchaselegder")]
            [HttpGet]
            public HttpResponseMessage GetPurchaselegder()
            {
                MdlPurchaseLedger values = new MdlPurchaseLedger();
                objpurchase.DaGetPurchaselegder(values);
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
        [ActionName("GetPurchaselegderDatefin")]
        [HttpGet]
        public HttpResponseMessage GetPurchaselegderDatefin(string from_date, string to_date)
        {
            MdlPurchaseLedger values = new MdlPurchaseLedger();
            objpurchase.DaGetPurchaselegderDatefin(from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaselegderView")]
        [HttpGet]
        public HttpResponseMessage GetPurchaselegderView(string invoice_gid)
        {
            MdlPurchaseLedger values = new MdlPurchaseLedger();
            objpurchase.DaGetPurchaselegderView(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaselegderProductView")]
        [HttpGet]
        public HttpResponseMessage GetPurchaselegderProductView(string invoice_gid)
        {
            MdlPurchaseLedger values = new MdlPurchaseLedger();
            objpurchase.DaGetPurchaselegderProductView(invoice_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}