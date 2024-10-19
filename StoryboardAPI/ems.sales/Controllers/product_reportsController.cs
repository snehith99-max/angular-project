using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.sales.DataAccess;
using ems.sales.Models;

namespace ems.sales.Controllers
{
    [RoutePrefix("/api/product_reports")]
    [Authorize]
    public class product_reportsController:ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        Daproductreports objproductreport = new Daproductreports();

        [ActionName("productsellingforlastsixmonths")]
        [HttpGet]
        public HttpResponseMessage productsellingforlastsixmonths()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            Mdlproductreports values = new Mdlproductreports();
            objproductreport.dagetproductsellingforsixmonths(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("productdropdown")]
        [HttpGet]
        public HttpResponseMessage productdropdown()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            Mdlproductreports values = new Mdlproductreports();
            objproductreport.daproductdropdown(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("getproductsellingforsixmonthssearch")]
        [HttpGet]
        public HttpResponseMessage getproductsellingforsixmonthssearch(string product)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            Mdlproductreports values = new Mdlproductreports();
            objproductreport.dagetproductsellingforsixmonthssearch( product ,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductGroupwiseChart")]
        [HttpGet]
        public HttpResponseMessage GetProductGroupwiseChart()
        {
            Mdlproductreports values = new Mdlproductreports();
            objproductreport.DaProductGroupwiseChart(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductGroupwiseChartSearch")]
        [HttpGet]
        public HttpResponseMessage GetProductGroupwiseChartSearch(string from_date, string to_date)
        {
            Mdlproductreports values = new Mdlproductreports();
            objproductreport.DaProductGroupwiseChartSearch(values, from_date, to_date);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductConsumptionReport")]
        [HttpGet]
        public HttpResponseMessage GetProductConsumptionReport()
        {
            Mdlproductreports values = new Mdlproductreports();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objproductreport.DaGetProductConsumptionReport(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductReportgrid")]
        [HttpGet]
        public HttpResponseMessage GetProductReportgrid(string product_gid)
        {
            Mdlproductreports values = new Mdlproductreports();
            objproductreport.DaProductReportgrid(product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
   
}