﻿using ems.pmr.DataAccess;
using ems.pmr.Models;
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

namespace ems.pmr.Controllers
{
    [RoutePrefix("api/PmrTrnGrnchecker")]
    [Authorize]
    public class PmrTrnGrncheckerController : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPmrTrnGrnchecker objpurchase = new DaPmrTrnGrnchecker();

        [ActionName("GetPmrTrnGrnchecker")]
        [HttpGet]
        public HttpResponseMessage GetPmrTrnGrnchecker()
        {
            MdlPmrTrnGrnchecker values = new MdlPmrTrnGrnchecker();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objpurchase.DaGetPmrTrnGrnchecker( getsessionvalues.user_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

    }
}