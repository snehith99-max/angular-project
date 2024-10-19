﻿using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Configuration;
using System.IO;


namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrMstProductGroup")]
    [Authorize]
    public class SmrMstProductGroupController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrMstProductGroup objsales = new DaSmrMstProductGroup();

        //summary
        [ActionName("GetSalesProductGroupSummary")]
        [HttpGet]
        public HttpResponseMessage GetSalesProductGroupSummary()
        {
            MdlSmrMstProductGroup values = new MdlSmrMstProductGroup();
            objsales.DaGetSalesProductGroupSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        //Add event

        [ActionName("PostSalesProductGroup")]
        [HttpPost]
        public HttpResponseMessage PostSalesProductGroup(salesproductgroup_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaPostSalesProductGroup(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Edit Event

        [ActionName("GetUpdatedSalesProductgroup")]
        [HttpPost]
        public HttpResponseMessage GetUpdatedSalesProductgroup(salesproductgroup_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetUpdatedSalesProductgroup(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // delete event

        [ActionName("GetDeleteSalesProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetDeleteSalesProductSummary(string productgroup_gid)
        {
            salesproductgroup_list objresult = new salesproductgroup_list();
            objsales.DaGetDeleteSalesProductSummary(productgroup_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        // Add Tax
        [ActionName("GetUpdatedSalesTax")]
        [HttpPost]
        public HttpResponseMessage GetUpdatedSalesTax(salesproductgroup_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objsales.DaGetUpdatedSalesTax(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }



        // Tax dropdown 1



        [ActionName("GetTaxDtl")]
        [HttpGet]
        public HttpResponseMessage GetTaxDtl()
        {
            MdlSmrMstProductGroup values = new MdlSmrMstProductGroup();
            objsales.DaGetTaxDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax dropdown 2



        [ActionName("GetTax2Dtl")]
        [HttpGet]
        public HttpResponseMessage GetTax2Dtl()
        {
            MdlSmrMstProductGroup values = new MdlSmrMstProductGroup();
            objsales.DaGetTax2Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Tax dropdown 3



        [ActionName("GetTax3Dtl")]
        [HttpGet]
        public HttpResponseMessage GetTax3Dtl()
        {
            MdlSmrMstProductGroup values = new MdlSmrMstProductGroup();
            objsales.DaGetTax3Dtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
