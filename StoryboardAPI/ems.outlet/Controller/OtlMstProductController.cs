using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.utilities.Functions;
using ems.utilities.Models;
using ems.outlet.Dataaccess;
using ems.outlet.Models;
using System.Linq;

namespace ems.outlet.Controller
{
    [Authorize]
    [RoutePrefix("OtlMstProduct")]
    public class OtlMstProductController : ApiController 
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaOtlMstProduct objproduct = new DaOtlMstProduct();


        // Summary
        [ActionName("GetProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            MdlOtlMstProduct values = new MdlOtlMstProduct();
            objproduct.DaGetProductSummary(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Image Upload
        [ActionName("GetProductImage")]
        [HttpPost]

        public HttpResponseMessage GetProductImage()
        {
            HttpRequest httpRequest;
            //Postassetlocationcreation values = new Postassetlocationcreation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            httpRequest = HttpContext.Current.Request;
            Products_list objResult = new Products_list();
            objproduct.DaGetProductImage(getsessionvalues.user_gid, httpRequest, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        // Post Product
        [ActionName("PostProduct")]
        [HttpPost]
        public HttpResponseMessage PostProduct(Products_list values)
        {
            HttpRequest httpRequest = HttpContext.Current.Request;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objproduct.DaPostProduct(getsessionvalues.user_gid, httpRequest, values );
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Edit
        [ActionName("GetEditProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetEditProductSummary(string product_gid)
        {
            MdlOtlMstProduct objresult = new MdlOtlMstProduct();
            objproduct.DaGetEditProductSummary(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        // Update
        [ActionName("ProductUpdate")]
        [HttpPost]
        public HttpResponseMessage ProductUpdate(Products_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objproduct.DaProductUpdate(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // View
        [ActionName("GetViewProductSummary")]
        [HttpGet]
        public HttpResponseMessage GeteditProductSummary(string product_gid)
        {
            MdlOtlMstProduct objresult = new MdlOtlMstProduct();
            objproduct.DaGetViewProductSummary(product_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


        // Drop-Downs
        [ActionName("GetProductGroup")]
        [HttpGet]
        public HttpResponseMessage GetProductGroup()
        {
            MdlOtlMstProduct values = new MdlOtlMstProduct();
            objproduct.DaGetProductGroup(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProducttype")]
        [HttpGet]
        public HttpResponseMessage GetProducttype()
        {
            MdlOtlMstProduct values = new MdlOtlMstProduct();
            objproduct.DaGetProducttype(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductUnitclass")]
        [HttpGet]
        public HttpResponseMessage GetProductUnitclass()
        {
            MdlOtlMstProduct values = new MdlOtlMstProduct();
            objproduct.DaGetProductUnitclass(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("gettaxdropdown")]
        [HttpGet]
        public HttpResponseMessage gettaxdropdown()
        {
            MdlOtlMstProduct values = new MdlOtlMstProduct();
            objproduct.Dagettaxdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductUnit")]
        [HttpGet]
        public HttpResponseMessage GetProductUnit()
        {
            MdlOtlMstProduct values = new MdlOtlMstProduct();
            objproduct.DaGetProductUnit(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetOnChangeProductUnitClass")]
        [HttpGet]
        public HttpResponseMessage GetOnChangeProductUnitClass(string productuomclass_gid)
        {
            MdlOtlMstProduct values = new MdlOtlMstProduct();
            objproduct.DaGetOnChangeProductUnitClass(productuomclass_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}