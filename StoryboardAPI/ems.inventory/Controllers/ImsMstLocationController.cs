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

    [RoutePrefix("ImsMstLocation")]
    [Authorize]
    public class ImsMstLocationController:ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsMstLocation objImsLocation = new DaImsMstLocation();

        [ActionName("Imslocationsummary")]
        [HttpGet]

        public HttpResponseMessage Imslocationsummary()
        {
            MdlImsMstLocation values = new MdlImsMstLocation();
            objImsLocation.DaImslocationsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("Imslocationbranch")]
        [HttpGet]

        public HttpResponseMessage Imslocationbranch()
        {
            MdlImsMstLocation values = new MdlImsMstLocation();
            objImsLocation.DaImslocationbranch(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

        [ActionName("PostLocation")]
        [HttpPost]
        public HttpResponseMessage PostLocation(locationadd_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objImsLocation.DaPostLocation(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Editlocation")]
        [HttpPost]
        public HttpResponseMessage Editlocation(locationadd_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objImsLocation.DaEditlocation(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Deletelocationsummary")]
        [HttpGet]
        public HttpResponseMessage Deletelocationsummary(string location_gid)
        {
            locationadd_list objresult = new locationadd_list();
            objImsLocation.DaDeletelocationsummary(location_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }



    }
}