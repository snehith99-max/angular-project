using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.outlet.Dataaccess;
using ems.outlet.Models;

namespace ems.outlet.Controller
{
    [RoutePrefix("otltrnMI")]
    [Authorize]

    public class otltrnMIController:ApiController
    {
     


        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaotltrnMI objIndentmaterial = new DaotltrnMI();

        [ActionName("MatrialIndentsummary")]
        [HttpGet]

        public HttpResponseMessage MatrialIndentsummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlotltrnMI values = new MdlotltrnMI();
            objIndentmaterial.DaMatrialIndentsummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }
        [ActionName("GetMaterialIndentView")]
        [HttpGet]

        public HttpResponseMessage GetMaterialIndentView(string materialrequisition_gid)
        {
            MdlotltrnMI values = new MdlotltrnMI();
            objIndentmaterial.DaGetMaterialIndentView(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }


        [ActionName("GetMaterialIndentViewProduct")]
        [HttpGet]

        public HttpResponseMessage GetMaterialIndentViewProduct(string materialrequisition_gid)
        {
            MdlotltrnMI values = new MdlotltrnMI();
            objIndentmaterial.DaGetMaterialIndentViewProduct(materialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);

        }

    }
}