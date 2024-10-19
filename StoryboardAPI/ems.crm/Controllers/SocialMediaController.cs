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

namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/SocialMedia")]
    public class SocialMediaController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSocailMedia objDaleadbank = new DaSocailMedia();
        // code By snehith
        [ActionName("GetShopifyStoreName")]
        [HttpGet]
        public HttpResponseMessage GetShopifyStoreName()
        {
            shopifystorename values = new shopifystorename();
            values = objDaleadbank.DaGetShopifyStoreName();
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetShopifyProductCount")]
        [HttpGet]
        public HttpResponseMessage GetShopifyProductCount()
        {
            shopifyproductcount values = new shopifyproductcount();
            values = objDaleadbank.DaGetShopifyProductCount();
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetShopifyCustomerCount")]
        [HttpGet]
        public HttpResponseMessage GetShopifyCustomerCount()
        {
            shopifycustomercount values = new shopifycustomercount();
            values = objDaleadbank.DaGetShopifyCustomerCount();
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetShopifyOrderCount")]
        [HttpGet]
        public HttpResponseMessage GetShopifyOrderCount()
        {
            shopifyordercount values = new shopifyordercount();
            values = objDaleadbank.DaGetShopifyOrderCount();
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetContactCount")]
        [HttpGet]
        public HttpResponseMessage GetContactCount()
        {
            MdlSocialMedia values = new MdlSocialMedia();
            objDaleadbank.DaGetContactCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMessageCount")]
        [HttpGet]
        public HttpResponseMessage GetMessageCount()
        {
            MdlSocialMedia values = new MdlSocialMedia();
            objDaleadbank.DaGetMessageCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMessageOutgoingCount")]
        [HttpGet]
        public HttpResponseMessage GetMessageOutgoingCount()
        {
            MdlSocialMedia values = new MdlSocialMedia();
            objDaleadbank.DaGetMessageOutgoingCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMessageIncomingCount")]
        [HttpGet]
        public HttpResponseMessage GetMessageIncomingCount()
        {
            MdlSocialMedia values = new MdlSocialMedia();
            objDaleadbank.DaGetMessageIncomingCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEmailStatusCount")]
        [HttpGet]
        public HttpResponseMessage GetEmailStatusCount()
        {
            MdlSocialMedia values = new MdlSocialMedia();
            objDaleadbank.DaGetEmailStatusCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetSentCount")]
        [HttpGet]
        public HttpResponseMessage GetSentCount()
        {
            MdlSocialMedia values = new MdlSocialMedia();
            objDaleadbank.DaGetSentCount(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetShopifyProductCounts")]
        [HttpGet]
        public HttpResponseMessage GetShopifyProductCounts()
        {
            MdlSocialMedia values = new MdlSocialMedia();
            objDaleadbank.DaGetShopifyProductCounts(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}