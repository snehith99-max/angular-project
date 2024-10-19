﻿using ems.pmr.DataAccess;
using ems.pmr.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrTrnGrnQcchecker")]
    [Authorize]
    public class PmrTrnGrnQccheckerController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrTrnGrnQcchecker objpurchase = new DaPmrTrnGrnQcchecker();

        [ActionName("GetPmrTrnGrnQcchecker")]
        [HttpGet]
        public HttpResponseMessage GetPmrTrnGrnQcchecker(string grn_gid)
        {
            MdlPmrTrnGrnQcchecker objresult = new MdlPmrTrnGrnQcchecker();
            objpurchase.DaGetPmrTrnGrnQcchecker(grn_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);

        }

        [ActionName("GetPmrTrnGrnQccheckerpo")]
        [HttpGet]
        public HttpResponseMessage GetPmrTrnGrnQccheckerpo(string grn_gid)
        {
            MdlPmrTrnGrnQcchecker objresult = new MdlPmrTrnGrnQcchecker();
            objpurchase.DaGetPmrTrnGrnQccheckerpo(grn_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);

        }

        [ActionName("PostPmrTrnGrnQcchecker")]
        [HttpPost]
        public HttpResponseMessage PostPmrTrnGrnQcchecker(PostGrnQcChecker_lists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaPostPmrTrnGrnQcchecker(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}