﻿using ems.pmr.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.pmr.Models;

namespace ems.pmr.Controllers
{
    [Authorize]
    [RoutePrefix("api/PmrMstProduct")]
    public class PmrMstProductController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrMstProduct objdaproduct = new DaPmrMstProduct();
        // Module Summary
        [ActionName("GetProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSummary()
        {
            MdlPmrMstProduct values = new MdlPmrMstProduct();
            objdaproduct.DaGetProductSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostProduct")]
        [HttpPost]
        public HttpResponseMessage PostProduct(product_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaproduct.DaPostProduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProducttype")]
        [HttpGet]
        public HttpResponseMessage GetProducttype()
        {
            MdlPmrMstProduct values = new MdlPmrMstProduct();
            objdaproduct.DaGetProducttype(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductGroup")]
        [HttpGet]
        public HttpResponseMessage GetProductGroup()
        {
            MdlPmrMstProduct values = new MdlPmrMstProduct();
            objdaproduct.DaGetProductGroup(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductUnitclass")]
        [HttpGet]
        public HttpResponseMessage GetProductUnitclass()
        {
            MdlPmrMstProduct values = new MdlPmrMstProduct();
            objdaproduct.DaGetProductUnitclass(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //[ActionName("GetProductUnit")]
        //[HttpGet]
        //public HttpResponseMessage GetProductUnit(string productuomclass_gid)
        //{
        //    MdlPmrMstProduct values = new MdlPmrMstProduct();
        //    objdaproduct.DaGetProductUnit(productuomclass_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}
        [ActionName("GetOnChangeProductUnitClass")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductUnitClass(string productuomclass_gid)
        {
            MdlPmrMstProduct values = new MdlPmrMstProduct();
            objdaproduct.DaGetOnChangeProductUnitClass(productuomclass_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDeleteProductdetails")]
        [HttpGet]
        public HttpResponseMessage GetDeleteProductdetails(string product_gid)
        {
            product_list objresult = new product_list();
            objdaproduct.DaGetDeleteProductdetails(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetViewProductSummary")]
        [HttpGet]
        public HttpResponseMessage GeteditProductSummary(string product_gid)
        {
            MdlPmrMstProduct objresult = new MdlPmrMstProduct();
            objdaproduct.DaGetViewProductSummary(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetEditProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditProductSummary(string product_gid)
        {
            MdlPmrMstProduct objresult = new MdlPmrMstProduct();
            objdaproduct.DaGetEditProductSummary(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("PmrMstProductUpdate")]
        [HttpPost]
        public HttpResponseMessage PmrMstProductUpdate(product_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaproduct.DaPmrMstProductUpdate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetImportLog")]
        [HttpGet]
        public HttpResponseMessage GetImportLog()
        {
            MdlPmrMstProduct values = new MdlPmrMstProduct();
            objdaproduct.DaGetImportLog(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteLog")]
        [HttpGet]
        public HttpResponseMessage DeleteLog(string log_id)
        {
            product_list values = new product_list();
            objdaproduct.DaDeleteLog(log_id, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //Product Import Excel

        [ActionName("ProductImportExcel")]
        [HttpPost]
        public HttpResponseMessage productImportExcel()
        {
            HttpRequest httpRequest;
            product_list values = new product_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            httpRequest = HttpContext.Current.Request;
            getsessionvalues = objgetgid.gettokenvalues(token);
            result objresult = new result();
            objdaproduct.DaproductImportExcel(httpRequest, getsessionvalues.user_gid, objresult, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Export Excel

        [ActionName("GetProductReportExport")]
        [HttpGet]
        public HttpResponseMessage GetProductReportExport()
        {
            MdlPmrMstProduct values = new MdlPmrMstProduct();
            objdaproduct.DaGetProductReportExport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("gettaxdropdown")]
        [HttpGet]
        public HttpResponseMessage gettaxdropdown()
        {
            MdlPmrMstProduct values = new MdlPmrMstProduct();
            objdaproduct.Dagettaxdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductUnit")]
        [HttpGet]
        public HttpResponseMessage GetProductUnit()
        {
            MdlPmrMstProduct values = new MdlPmrMstProduct();
            objdaproduct.DaGetProductUnit(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetcustomerActive")]
        [HttpGet]
        public HttpResponseMessage GetcustomerActive(string params_gid)
        {
            MdlPmrMstProduct objresult = new MdlPmrMstProduct();
            objdaproduct.DaGetcustomerActive(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetcustomerInactive")]
        [HttpGet]
        public HttpResponseMessage GetcustomerInactive(string params_gid)
        {
            MdlPmrMstProduct objresult = new MdlPmrMstProduct();
            objdaproduct.DaGetcustomerInactive(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}