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
using System.Threading.Tasks;

namespace ems.sales.Controllers
{
    [Authorize]
    [RoutePrefix("api/SmrMstProduct")]
    public class SmrMstProductController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSmrMstProduct objsales = new DaSmrMstProduct();
        // Module Summary
        [ActionName("GetSalesProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetSalesProductSummary()
        {
            MdlSmrMstProduct values = new MdlSmrMstProduct();
            objsales.DaGetSalesProductSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Add event
        [ActionName("PostSalesProduct")]
        [HttpPost]
        public HttpResponseMessage PostSalesProduct(product_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objsales.DaPostSalesProduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }   
        //product type
        [ActionName("GetProducttype")]
        [HttpGet]
        public HttpResponseMessage GetProducttype()
        {
            MdlSmrMstProduct values = new MdlSmrMstProduct();
            objsales.DaGetProducttype(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("gettaxdropdown")]
        [HttpGet]
        public HttpResponseMessage gettaxdropdown()
        {
            MdlSmrMstProduct values = new MdlSmrMstProduct();
            objsales.Dagettaxdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //product group dropdown
        [ActionName("GetProductGroup")]
        [HttpGet]
        public HttpResponseMessage GetProductGroup()
        {
            MdlSmrMstProduct values = new MdlSmrMstProduct();
            objsales.DaGetProductGroup(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //unit class dropdown
        [ActionName("GetProductUnitclass")]
        [HttpGet]
        public HttpResponseMessage GetProductUnitclass()
        {
            MdlSmrMstProduct values = new MdlSmrMstProduct();
            objsales.DaGetProductUnitclass(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //product unit dropdown
        [ActionName("GetProductUnit")]
        [HttpGet]
        public HttpResponseMessage GetProductUnit()
        {
            MdlSmrMstProduct values = new MdlSmrMstProduct();
            objsales.DaGetProductUnit(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //On change
        [ActionName("GetOnChangeProductUnitClass")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductUnitClass(string productuomclass_gid)
        {
            MdlSmrMstProduct values = new MdlSmrMstProduct();
            objsales.DaGetOnChangeProductUnitClass(productuomclass_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //Edit event
        [ActionName("GetEditProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditProductSummary(string product_gid)
        {
            MdlSmrMstProduct objresult = new MdlSmrMstProduct();
            objsales.DaGetEditProductSummary(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("SmrMstProductUpdate")]
        [HttpPost]
        public HttpResponseMessage SmrMstProductUpdate(product_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objsales.DaSmrMstProductUpdate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //View
        [ActionName("GetViewProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetViewProductSummary(string product_gid)
        {
            MdlSmrMstProduct objresult = new MdlSmrMstProduct();
            objsales.DaGetViewProductSummary(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // Delete 
        [ActionName("GetDeleteSalesProductdetails")]
        [HttpGet]
        public HttpResponseMessage GetDeleteSalesProductdetails(string product_gid)
        {
            product_list objresult = new product_list();
            objsales.DaGetDeleteSalesProductdetails(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        //import excel
        //[ActionName("GetProductImportExcel")]
        //[HttpPost]
        //public HttpResponseMessage ProductImportExcel ()
        //{
        //    HttpRequest httpRequest;
        //    product_list values = new product_list();
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    httpRequest = HttpContext.Current.Request;
        //    getsessionvalues = objgetgid.gettokenvalues(token);
        //    result productresult = new result();
        //    objsales.DaGetProductImportExcel(httpRequest, getsessionvalues.user_gid, productresult, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, productresult);
        //}


        //Product Import Excel

        [ActionName("ProductImportExcel")]
        [HttpPost]
        public HttpResponseMessage ProductImportExcel()
        {
            HttpRequest httpRequest;
            product_list values = new product_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            httpRequest = HttpContext.Current.Request;
            getsessionvalues = objgetgid.gettokenvalues(token);
            result productresult = new result();
            objsales.DaProductImportExcel(httpRequest, getsessionvalues.user_gid, productresult, values);
            return Request.CreateResponse(HttpStatusCode.OK, productresult);
        }

        // Product Active - Inactive

        [ActionName("GetcustomerInactive")]
        [HttpGet]
        public HttpResponseMessage GetcustomerInactive(string params_gid)
        {
            MdlSmrTrnCustomerSummary objresult = new MdlSmrTrnCustomerSummary();
            objsales.DaGetcustomerInactive(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetcustomerActive")]
        [HttpGet]
        public HttpResponseMessage GetcustomerActive(string params_gid)
        {
            MdlSmrTrnCustomerSummary objresult = new MdlSmrTrnCustomerSummary();
            objsales.DaGetcustomerActive(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetProductReportExport")]
        [HttpGet]
        public HttpResponseMessage GetProductReportExport()
        {
            MdlSmrMstProduct values = new MdlSmrMstProduct();
            objsales.DaGetProductReportExport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //Product Upload Image
        [ActionName("ProductImage")]
        [HttpPost]

        public HttpResponseMessage ProductImage()
        {
            HttpRequest httpRequest;
            //Postassetlocationcreation values = new Postassetlocationcreation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objsales.DaProductImage(httpRequest, objResult, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }


        // Filter Product 

        [ActionName("GetFilterProduct")]
        [HttpGet]
        public HttpResponseMessage GetFilterProduct(string producttype_gid)
        {
            MdlSmrMstProduct objresult = new MdlSmrMstProduct();
            objsales.DaGetFilterProduct(producttype_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // Module Summary
        [ActionName("GetImportLog")]
        [HttpGet]
        public HttpResponseMessage GetImportLog()
        {
            MdlSmrMstProduct values = new MdlSmrMstProduct();
            objsales.DaGetImportLog(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteLog")]
        [HttpGet]
        public HttpResponseMessage DeleteLog(string log_id)
        {
            product_list values = new product_list();
            objsales.DaDeleteLog(log_id, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("MintsoftProductDetailsAsync")]
        [HttpGet]
        public async Task<HttpResponseMessage> MintsoftProductDetailsAsync()
        {
            get values = new get();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            values = await objsales.DaMintsoftProductDetailsAsync(getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }

    
}