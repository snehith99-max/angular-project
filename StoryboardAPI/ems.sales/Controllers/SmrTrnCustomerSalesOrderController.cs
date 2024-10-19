using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrTrnCustomerSalesOrder")]
    [Authorize]
    public class SmrTrnCustomerSalesOrderController : ApiController
    {
        string msSQL = string.Empty;
        int mnResult;
        cmnfunctions cmnfunctions = new cmnfunctions();
        dbconn dbconn = new dbconn();

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrTrnCustomerSO objsales = new DaSmrTrnCustomerSO();

        // Summary 
        [ActionName("GetSmrTrnSalesordersummary")]
        [HttpGet]
        public HttpResponseMessage GetSmrTrnSalesordersummary(string customer_gid)
        {
            MdlSmrTrnCustomerSO values = new MdlSmrTrnCustomerSO();
            objsales.DaGetSmrTrnSalesordersummary(customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Cancel Order
        [ActionName("getCancelSalesOrder")]
        [HttpGet]
        public HttpResponseMessage getCancelSalesOrder(string params_gid)
        {
            MdlSmrTrnCustomerSO objresult = new MdlSmrTrnCustomerSO();
            objsales.DagetCancelSalesOrder(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // Get Product Group
        [ActionName("GetProductGroup")]
        [HttpGet]
        public HttpResponseMessage GetProductGroup()
        {
            MdlSmrTrnCustomerSO values = new MdlSmrTrnCustomerSO();
            objsales.DaGetProductGroup(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Get ProductSearch Summary
        [ActionName("GetProductsearchSummarySales")]
        [HttpGet]
        public HttpResponseMessage GetProductsearchSummarySales()
        {
            MdlSmrTrnCustomerSO values = new MdlSmrTrnCustomerSO();
            objsales.DaGetProductsearchSummarySales(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Products
        [ActionName("ProductSalesSummary")]
        [HttpGet]
        public HttpResponseMessage ProductSalesSummary(string customer_gid, string product_gid)
        {
            MdlSmrTrnCustomerSO values = new MdlSmrTrnCustomerSO();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaProductSalesSummary(customer_gid, values, product_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Product Submit
        [ActionName("PostOnAdds")]
        [HttpPost]
        public HttpResponseMessage PostOnAdds(Customersalesorders_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostOnAdds(values.customer_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetViewsalesorderSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewsalesorderSummary(string salesorder_gid)
        {
            MdlSmrTrnCustomerSO objresult = new MdlSmrTrnCustomerSO();
            objsales.DaGetViewsalesorderSummary(salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        //Product Summary  details
        [ActionName("GetViewsalesorderdetails")]
        [HttpGet]
        public HttpResponseMessage GetViewsalesorderdetails(string salesorder_gid, string customer_gid)
        {
            MdlSmrTrnCustomerSO objresult = new MdlSmrTrnCustomerSO();
            objsales.DaGetViewsalesorderdetails(salesorder_gid, customer_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        //Delete Product 
        [ActionName("GetDeleteDirectSOProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteDirectSOProductSummary(string tmpsalesorderdtl_gid)
        {
            Customersalesorders_list objresult = new Customersalesorders_list();
            objsales.GetDeleteDirectSOProductSummary(tmpsalesorderdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // OverAll Submit
        [ActionName("PostSalesOrder")]
        [HttpPost]
        public HttpResponseMessage PostSalesOrder(Customerpostsales_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostSalesOrder( values.customer_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("trucatetmpproductsummary")]
        [HttpGet]
        public HttpResponseMessage trucatetmpproductsummary(string customer_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.datrucatetmpproductsummary(getsessionvalues.employee_gid, customer_gid);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [ActionName("GetProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSummary()
        {
            MdlSmrTrnCustomerSO values = new MdlSmrTrnCustomerSO();
            objsales.DaGetProductsearchSummarySales(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("orderstatuscount")]
        [HttpGet]
        public HttpResponseMessage orderstatuscount(string customer_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrTrnCustomerSO objresult = new MdlSmrTrnCustomerSO();
            objsales.daorderstatuscount(customer_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("Loadpage")]
        [HttpGet]
        public HttpResponseMessage Loadpage (string customer_gid)
        {
            msSQL = " delete from smr_tmp_tsalesorderdtl where employee_gid='"+ customer_gid  + "'";
            mnResult = dbconn.ExecuteNonQuerySQL(msSQL);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [ActionName("GetProductWithTaxSummary")]
        [HttpGet]
        public HttpResponseMessage DaGetProductWithTaxSummary(string customer_gid, string product_gid)
        {
            MdlSmrTrnCustomerSO values = new MdlSmrTrnCustomerSO();
            objsales.DaGetProductWithTaxSummary(customer_gid, product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}