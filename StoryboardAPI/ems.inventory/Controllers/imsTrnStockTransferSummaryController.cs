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
    [RoutePrefix("api/ImsTrnStockTransferSummary")]
    [Authorize]
    public class ImsTrnStockTransferSummaryController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnStockTransferSummary objDaInventory = new DaImsTrnStockTransferSummary();

        [ActionName("GetBranchWiseSummary")]
        [HttpGet]
        public HttpResponseMessage GetBranchWiseSummary()
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetBranchWiseSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLocationWiseSummary")]
        [HttpGet]
        public HttpResponseMessage GetLocationWiseSummary()
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetLocationWiseSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetBranchWiseView")]
        [HttpGet]
        public HttpResponseMessage GetBranchWiseView(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetBranchWiseView(stocktransfer_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLocationView")]
        [HttpGet]
        public HttpResponseMessage GetLocationView(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetLocationView(stocktransfer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetLocationProductView")]
        [HttpGet]
        public HttpResponseMessage GetLocationProductView(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetLocationProductView(stocktransfer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBranchWiseaddSummary")]
        [HttpGet]
        public HttpResponseMessage GetBranchWiseaddSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetBranchWiseaddSummary(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBranchWiseTransfer")]
        [HttpGet]
        public HttpResponseMessage GetBranchWiseTransfer(string stock_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetBranchWiseTransfer(stock_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Poststocktransfer")]
        [HttpPost]
        public HttpResponseMessage Poststocktransfer(Poststocktransfer values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPoststocktransfer(getsessionvalues.branch_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //location

        [ActionName("GetLocation")]
        [HttpGet]
        public HttpResponseMessage GetLocation()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetLocation(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetLocationTo")]
        [HttpGet]
        public HttpResponseMessage GetLocationTo()
        {

            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetLocationTo(getsessionvalues.branch_gid,  values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductCode1")]
        [HttpGet]
        public HttpResponseMessage GetProductCode1(string location_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetProductCode1(getsessionvalues.branch_gid, location_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductgroup")]
        [HttpGet]
        public HttpResponseMessage GetProductgroup(string location_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetProductgroup(getsessionvalues.branch_gid, location_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProduct1")]
        [HttpGet]
        public HttpResponseMessage GetProduct1(string location_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetProduct1(getsessionvalues.branch_gid,location_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetPopSummary")]
        [HttpGet]
        public HttpResponseMessage GetPopSummary(string location_gid,string product_gid,string productuom_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetPopSummary(getsessionvalues.branch_gid, location_gid, productuom_gid, product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        
        [ActionName("PopSubmit")]
        [HttpPost]
        public HttpResponseMessage PopSubmit(GetPopsummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPopSubmit(getsessionvalues.employee_gid ,getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        
        [ActionName("GetStockQuantity")]
        [HttpGet]
        public HttpResponseMessage GetStockQuantity(string stock_gid,string product_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetStockQuantity(stock_gid,product_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //location transfer post submit
        [ActionName("PostLocationstocktransfer")]
        [HttpPost]
        public HttpResponseMessage PostLocationstocktransfer(Postlocationstocktransfer values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostLocationstocktransfer(getsessionvalues.branch_gid, getsessionvalues.user_gid, getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostOnAddpr")]
        [HttpPost]
        public HttpResponseMessage PostOnAddpr(productlist1 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaPostOnAddpr(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSummary()
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaProductSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDeletePrProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeletePrProductSummary(string tmpstocktransfer_gid)
        {
            productsummary_list1 objresult = new productsummary_list1();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaGetDeletePrProductSummary(getsessionvalues.user_gid, tmpstocktransfer_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetImsRptStocktransferreport")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStocktransferreport()
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetImsRptStocktransferreport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetImsRptStocktransferapproval")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStocktransferapproval()
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetImsRptStocktransferapproval(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetImsRptStocktransferapprovalview")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStocktransferapprovalview(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetImsRptStocktransferapprovalview(stocktransfer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetStocktransferProductView")]
        [HttpGet]
        public HttpResponseMessage GetStocktransferProductView(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetStocktransferProductView(stocktransfer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //approval


        [ActionName("StockTransferApproval")]
        [HttpGet]
        public HttpResponseMessage StockTransferApproval(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaStockTransferApproval(stocktransfer_gid,getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("StockTransferReject")]
        [HttpGet]
        public HttpResponseMessage StockTransferReject(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaStockTransferReject(stocktransfer_gid,getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetImsRptStocktransferacknowlege")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStocktransferacknowlege()
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaGetImsRptStocktransferacknowlege(getsessionvalues.branch_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetImsRptStocktransferacknowlegelocation")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStocktransferacknowlegelocation()
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaGetImsRptStocktransferacknowlegelocation(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetStocktransferAckdate")]
        [HttpGet]
        public HttpResponseMessage GetStocktransferAckdate(string ref_no, string from_date, string to_date)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetStocktransferAckdate(ref_no,from_date, to_date, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Acknowledgement

        [ActionName("GetImsRptStocktransferackview")]
        [HttpGet]
        public HttpResponseMessage GetImsRptStocktransferackview(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetImsRptStocktransferackview(stocktransfer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetStocktransferackProductView")]
        [HttpGet]
        public HttpResponseMessage GetStocktransferackProductView(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetStocktransferackProductView(stocktransfer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("StockTransferAckApproval")]
        [HttpGet]
        public HttpResponseMessage StockTransferAckApproval(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaStockTransferAckApproval(stocktransfer_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("StockTransferAckReject")]
        [HttpGet]
        public HttpResponseMessage StockTransferAckReject(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaInventory.DaStockTransferAckReject(stocktransfer_gid, getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDetialViewProduct")]
        [HttpGet]
        public HttpResponseMessage GetDetialViewProduct(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetDetialViewProduct(stocktransfer_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDetialProduct")]
        [HttpGet]
        public HttpResponseMessage GetDetialProduct(string stocktransfer_gid)
        {
            MdlImsTrnStockTransferSummary values = new MdlImsTrnStockTransferSummary();
            objDaInventory.DaGetDetialProduct(stocktransfer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}