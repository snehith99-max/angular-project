using ems.inventory.DataAccess;
using ems.inventory.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace ems.inventory.Controllers
{

    [RoutePrefix("api/ImsRptStockreport")]
    [Authorize]


    public class ImsRptStockreportController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsRptStockreport objDaInventory = new DaImsRptStockreport();

        [ActionName("GetImsRptStockreport")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStockreport(string branch_gid)
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetImsRptStockreport(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBranch")]
        [HttpGet]
        public HttpResponseMessage GetBranch()
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetBranch(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetImsRptStockstatement")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStockstatement()
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetImsRptStockstatement(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }   
        [ActionName("GetStockStatementProduct")]
        [HttpGet]
        public HttpResponseMessage GetStockStatementProduct(string product_gid,string branch_gid)
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetStockStatementProduct(product_gid, branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetStockStatementPurchase")]
        [HttpGet]
        public HttpResponseMessage GetStockStatementPurchase(string product_gid)
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetStockStatementPurchase(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchasevendor")]
        [HttpGet]
        public HttpResponseMessage GetPurchasevendor(string vendor_gid)
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetPurchasevendor(vendor_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPurchaseorder_history")]
        [HttpGet]

        public HttpResponseMessage GetPurchaseorder_history(string vendor_gid,string product_gid)
        {
            MdlImsRptStockreport values=new MdlImsRptStockreport();
            objDaInventory.DaGetPurchaseorder_history(vendor_gid, product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetStockStatementSales")]
        [HttpGet]
        public HttpResponseMessage GetStockStatementSales(string product_gid)
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetStockStatementSales(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetSalescustomer")]
        [HttpGet]
        public HttpResponseMessage GetSalescustomer(string customer_gid)
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetSalescustomer(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSalesorder_history")]
        [HttpGet]

        public HttpResponseMessage GetSalesorder_history(string customer_gid, string product_gid)
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetSalesorder_history(customer_gid, product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetStockStatementSummary")]
        [HttpGet]
        public HttpResponseMessage GetStockStatementSummary(string product_gid)
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetStockStatementSummary(product_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetStockStatementPurchaseSummary")]
        [HttpGet]
        public HttpResponseMessage GetStockStatementPurchaseSummary(string product_gid)
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetStockStatementPurchaseSummary(product_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetImsRptStockmovement")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStockmovement()
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetImsRptStockmovement(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetstockStatus")]
        [HttpGet]
        public HttpResponseMessage GetstockStatus()
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetstockStatus(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getlastsixmonthstock")]
        [HttpGet]
        public HttpResponseMessage Getlastsixmonthstock()
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetlastsixmonthstock(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetImsRptMovementreportsearch")]
        [HttpGet]
        public HttpResponseMessage GetImsRptMovementreportsearch(string branch_gid)
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetImsRptMovementreportsearch(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetStockmovementview")]
        [HttpGet]
        public HttpResponseMessage GetStockmovementview(string product_gid, string branch_gid)
        {
            MdlImsRptStockreport values = new MdlImsRptStockreport();
            objDaInventory.DaGetStockmovementview(product_gid, branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}