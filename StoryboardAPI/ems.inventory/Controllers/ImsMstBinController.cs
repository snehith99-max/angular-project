using ems.inventory.DataAccess;
using ems.inventory.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.inventory.Controllers
{
    [RoutePrefix("ImsMstBin")]
    [Authorize]
    public class ImsMstBinController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsMstBin objImsbin = new DaImsMstBin();

        [ActionName("Imsbinsummary")]
        [HttpGet]
        public HttpResponseMessage Imsbinsummary()
        {
            MdlImsMstBin values = new MdlImsMstBin();
            objImsbin.DaImsbinsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("PostBin")]
        [HttpPost]
        public HttpResponseMessage PostBin(imsbin_addlist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objImsbin.DaPostBin(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Imsbinaddsummary")]
        [HttpGet]
        public HttpResponseMessage Imsbinaddsummary(string location_gid)
        {
            MdlImsMstBin values = new MdlImsMstBin();
            objImsbin.DaImsbinaddsummary(location_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Imsbinadd")]
        [HttpGet]
        public HttpResponseMessage Imsbinadd(string location_gid)
        {
            MdlImsMstBin values = new MdlImsMstBin();
            objImsbin.DaImsbinadd(location_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Deletebin")]
        [HttpGet]
        public HttpResponseMessage Deletebin(string bin_gid)
        {
            MdlImsMstBin values = new MdlImsMstBin();
            objImsbin.DaDeletebin(bin_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


    }
}