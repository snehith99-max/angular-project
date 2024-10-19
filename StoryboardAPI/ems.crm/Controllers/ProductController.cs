﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.crm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.IO;
using System.Configuration;

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaProduct objdaproduct = new DaProduct();
        // Module Summary
        [ActionName("GetShopifyProduct")]
        [HttpGet]
        public HttpResponseMessage GetShopifyProduct()
        {
            getproduct values = new getproduct();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            values = objdaproduct.DaGetShopifyProduct(getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateShopifyProductPrice")]
        [HttpPost]
        public HttpResponseMessage UpdateShopifyProductPrice(productprice_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaproduct.DaUpdateShopifyProductPrice(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdateShopifyProductQuantity")]
        [HttpPost]
        public HttpResponseMessage UpdateShopifyProductQuantity(productquantity_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaproduct.DaUpdateShopifyProductQuantity(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetShopifyProductdetails")]
        [HttpGet]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetShopifyProductdetails()
        {
            getproduct values = new getproduct();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            values = await objdaproduct.DaGetShopifyProductdetails(getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetShopifyLocation")]
        [HttpGet]
        public HttpResponseMessage GetShopifyLocation()
        {
            getproduct values = new getproduct();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            values = objdaproduct.DaGetShopifyLocation(getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSummary()
        {
            MdlProduct values = new MdlProduct();
            objdaproduct.DaGetProductSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetShopifyProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetShopifyProductSummary()
        {
            MdlProduct values = new MdlProduct();
            objdaproduct.DaGetShopifyProductSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetShopifyProductInventorySummary")]
        [HttpGet]
        public HttpResponseMessage GetShopifyProductInventorySummary()
        {
            MdlProduct values = new MdlProduct();
            objdaproduct.DaGetShopifyProductInventorySummary(values);
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
        [ActionName("PostShopifyProduct")]
        [HttpPost]
        public HttpResponseMessage PostShopifyProduct(shopifyproduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaproduct.DaPostShopifyProduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getproducttypedropdown")]
        [HttpGet]
        public HttpResponseMessage Getproducttypedropdown()
        {
            MdlProduct values = new MdlProduct();
            objdaproduct.DaGetproducttypedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getproductgroupdropdown")]
        [HttpGet]
        public HttpResponseMessage Getproductgroupdropdown()
        {
            MdlProduct values = new MdlProduct();
            objdaproduct.DaGetproductgroupdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getproductunitclassdropdown")]
        [HttpGet]
        public HttpResponseMessage Getproductunitclassdropdown()
        {
            MdlProduct values = new MdlProduct();
            objdaproduct.DaGetproductunitclassdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getproductunitdropdown")]
        [HttpGet]
        public HttpResponseMessage Getproductunitdropdown()
        {
            MdlProduct values = new MdlProduct();
            objdaproduct.DaGetproductunitdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getcurrencydropdown")]
        [HttpGet]
        public HttpResponseMessage Getcurrencydropdown()
        {
            MdlProduct values = new MdlProduct();
            objdaproduct.DaGetcurrencydropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetOnChangeProductUnitClass")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductUnitClass(string productuomclass_gid)
        {
            MdlProduct values = new MdlProduct();
            objdaproduct.DaGetOnChangeProductUnitClass(productuomclass_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductImage")]
        [HttpPost]

        public HttpResponseMessage GetProductImage()
        {
            HttpRequest httpRequest;
            //Postassetlocationcreation values = new Postassetlocationcreation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objdaproduct.DaGetProductImage(httpRequest, objResult, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }


        [ActionName("ProductUpdate")]
        [HttpPost]
        public HttpResponseMessage ProductUpdate(product_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaproduct.DaProductUpdate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("ShopifyProductUpdate")]
        [HttpPost]
        public HttpResponseMessage ShopifyProductUpdate(shopifyproduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaproduct.DaShopifyProductUpdate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditProductSummary(string product_gid)
        {
            MdlProduct objresult = new MdlProduct();
            objdaproduct.DaGetEditProductSummary(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetEditShopifyProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditShopifyProductSummary(string shopifyproductid)
        {
            MdlProduct objresult = new MdlProduct();
            objdaproduct.DaGetEditShopifyProductSummary(shopifyproductid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Getdeleteproductdetails")]
        [HttpGet]
        public HttpResponseMessage Getdeleteproductdetails(string product_gid)
        {
            product_list objresult = new product_list();
            objdaproduct.DaGetdeleteproductdetails(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
       
        [ActionName("UpdateShopifyProductImage")]
        [HttpPost]
        public HttpResponseMessage UpdateShopifyProductImage(updateshopifyimage_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaproduct.DaUpdateShopifyProductImage(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("ProductUploadExcels")]
        [HttpPost]
        //public HttpResponseMessage ProductUploadExcels(mdlimportexcel values)
        //{
        //    HttpRequest httpRequest;
        //    product_list values = new product_list();
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = objgetgid.gettokenvalues(token);
        //    httpRequest = HttpContext.Current.Request;
        //    result objResult = new result();
        //    objdaproduct.DaProductUploadExcels(httpRequest, getsessionvalues.user_gid, objResult, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, objResult);



        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = Objgetgid.gettokenvalues(token);
        //    result objresult = new result();
        //    objresult = objDaWhatsapp.daPostImportExcel(values, getsessionvalues.user_gid);
        //    return Request.CreateResponse(HttpStatusCode.OK, objresult);
        //}
        public HttpResponseMessage ProductUploadExcels()
        {
            HttpRequest httpRequest;
            product_list values = new product_list();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            result objResult = new result();
            objdaproduct.DaProductUploadExcels(httpRequest, getsessionvalues.user_gid, objResult, values);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
        public HttpResponseMessage GetProductReportExport()
        {
            MdlProduct values = new MdlProduct();
            objdaproduct.DaGetProductReportExport(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("downloadImages")]
        [HttpGet]
        public HttpResponseMessage downloadImages(string product_gid)
        {
            MdlProduct objresult = new MdlProduct();
            objdaproduct.DadownloadImages(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetViewProductSummary")]
        [HttpGet]
        public HttpResponseMessage GeteditProductSummary(string product_gid)
        {
            MdlProduct objresult = new MdlProduct();
            objdaproduct.DaGetViewProductSummary(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Sendproductmaster")]
        [HttpPost]
        public HttpResponseMessage Sendproductmaster(shopifyproductmove_list values)
        {
            result objresult = new result();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objresult = objdaproduct.DaSendproductmaster(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);

        }
        [ActionName("GetcustomerInactive")]
        [HttpGet]
        public HttpResponseMessage GetcustomerInactive(string params_gid)
        {
            MdlProduct objresult = new MdlProduct();
            objdaproduct.DaGetcustomerInactive(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("GetcustomerActive")]
        [HttpGet]
        public HttpResponseMessage GetcustomerActive(string params_gid)
        {
            MdlProduct objresult = new MdlProduct();
            objdaproduct.DaGetcustomerActive(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Getsyncshopify")]
        [HttpGet]
        public HttpResponseMessage Getsyncshopify()
        {
            MdlProduct objresult = new MdlProduct();
            objdaproduct.DaGetsyncshopify(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Getsync_crm")]
        [HttpGet]
        public HttpResponseMessage Getsync_crm()
        {
            MdlProduct objresult = new MdlProduct();
            objdaproduct.DaGetsync_crm(objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Postsynccrm")]
        [HttpPost]
        public HttpResponseMessage Postsynccrm(shopifyproductmove_list values)
        {
            result objresult = new result();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objresult = objdaproduct.DaPostsynccrm(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);

        }
        [ActionName("Addproducttowhatsapp")]
        [HttpGet]
        public HttpResponseMessage Addproducttowhatsapp(string product_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            product_list objresult = new product_list();
            objdaproduct.DaAddproducttowhatsapp(getsessionvalues.user_gid, getsessionvalues.branch_gid, product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("updatewhatsappstockstatus")]
        [HttpPost]
        public HttpResponseMessage updatewhatsappstockstatus(product_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaproduct.Daupdatewhatsappstockstatus(getsessionvalues.branch_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetwhatsappProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetwhatsappProductSummary()
        {
            MdlProduct values = new MdlProduct();
            objdaproduct.DaGetwhatsappProductSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Removeproductfromwt")]
        [HttpGet]
        public HttpResponseMessage Removeproductfromwt(string whatsapp_id )
        {
            result objresult = new result();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaproduct.DaRemoveproductfromwt(whatsapp_id, getsessionvalues.branch_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

    }
}
