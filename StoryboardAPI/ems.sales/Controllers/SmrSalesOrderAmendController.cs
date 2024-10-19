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
    [RoutePrefix("api/SmrSalesOrderAmend")]
    [Authorize]
    public class SmrSalesOrderAmendController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrSalesOrderAmend objsales = new DaSmrSalesOrderAmend();

        // DATA FETCHING FROM SALES ORDER SUMMARY FOR AMEND SUMMARY

        [ActionName("GetAmendSalesOrderDtl")]
        [HttpGet]
        public HttpResponseMessage GetAmendSalesOrderDtl(string salesorder_gid)
        {
            MdlSmrSalesOrderAmend values = new MdlSmrSalesOrderAmend();
            objsales.DaGetAmendSalesOrderDtl(salesorder_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // DATA FETCHING FROM SALES ORDER SUMMARY FOR AMEND TEMPORARY SUMMARY

        [ActionName("GetProductEditSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductEditSummary(string tmpsalesorderdtl_gid)
        {
            MdlSmrSalesOrderAmend objresult = new MdlSmrSalesOrderAmend();
            objsales.DaGetProductEditSummary(tmpsalesorderdtl_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        // PRODUCT ADD FOR SALES ORDER AMEND

        [ActionName("PostSOAmendProduct")]
        [HttpPost]
        public HttpResponseMessage PostSOAmendProduct(amendtemplist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostSOAmendProduct(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // PRODUCT SUMMARY FOR SALES ORDER AMEND

        [ActionName("GetSOProductAmendSummary")]
        [HttpGet]
        public HttpResponseMessage GetSOProductAmendSummary(string salesorder_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlSmrSalesOrderAmend objresult = new MdlSmrSalesOrderAmend();
            objsales.DaGetSOProductAmendSummary(getsessionvalues.employee_gid, salesorder_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // OVERALL SUBMIT FOR AMEND SALES ORDER

        [ActionName("AmendSalesOrder")]
        [HttpPost]
        public HttpResponseMessage AmendSalesOrder(PostAmendSO_List values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaAmendSalesOrder(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // TEMP SUMMARY UPDATE EVENT
        [ActionName("updateSalesOrderedit")]
        [HttpPost]
        public HttpResponseMessage updateSalesOrderedit(Salesorders_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaupdateSalesOrderedit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}

