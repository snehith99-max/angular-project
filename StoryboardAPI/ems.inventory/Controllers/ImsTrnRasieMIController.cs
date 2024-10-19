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
using static ems.inventory.Models.MdlImsTrnRaiseMI;

namespace ems.inventory.Controllers
{
    [Authorize]
    [RoutePrefix("api/ImsTrnRasieMI")]

    public class ImsTrnRasieMIController: ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnRaiseMI objRaiseMI = new DaImsTrnRaiseMI();


        [ActionName("GetMIsummary")]
        [HttpGet]
        public HttpResponseMessage GetMIsummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnRaiseMI values = new MdlImsTrnRaiseMI();
            objRaiseMI.DaGetMIsummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetMIproducttype")]
        [HttpGet]
        public HttpResponseMessage GetMIproducttype()
        {
            MdlImsTrnRaiseMI values = new MdlImsTrnRaiseMI();
            objRaiseMI.DaGetMIproducttype(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetMIProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetMIProductSummary(string producttype_gid, string product_name)
        {
            MdlImsTrnRaiseMI values = new MdlImsTrnRaiseMI();
            objRaiseMI.DaGetMIProductSummary(producttype_gid, product_name, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostOnmiproduct")]
        [HttpPost]
        public HttpResponseMessage PostOnmiproduct(imsproductsingle_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objRaiseMI.DaPostOnmiproduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("tmpProductSummary")]
        [HttpGet]
        public HttpResponseMessage tmpProductSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnRaiseMI values = new MdlImsTrnRaiseMI();
            objRaiseMI.DatmpProductSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("MaterialIndent")]
        [HttpPost]
        public HttpResponseMessage MaterialIndent(issuematerial_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objRaiseMI.DaMaterialIndent(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostMIProduct")]
        [HttpPost]
        public HttpResponseMessage PostMIProduct(MIproductbulk values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objRaiseMI.DaPostMIProduct(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeletetmpProductSummary")]
        [HttpGet]
        public HttpResponseMessage DeletetmpProductSummary(string tmpmaterialrequisition_gid)
        {
            MdlImsTrnRaiseMI values = new MdlImsTrnRaiseMI();
            objRaiseMI.DaDeletetmpProductSummary(tmpmaterialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


    }
}