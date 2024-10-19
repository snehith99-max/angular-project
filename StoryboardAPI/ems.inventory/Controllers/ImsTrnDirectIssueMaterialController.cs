using ems.inventory.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.inventory.Models;

namespace ems.inventory.Controllers
{
    [Authorize]
    [RoutePrefix("api/ImsTrnDirectIssueMaterial")]

    public class ImsTrnDirectIssueMaterialController : ApiController
    {

        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaImsTrnDirectIssueMaterial objDirectissue= new DaImsTrnDirectIssueMaterial();

        [ActionName("Getimslocation")]
        [HttpGet]
        public HttpResponseMessage Getimslocation()
        {
            MdlImsTrnDirectIssueMaterial values = new MdlImsTrnDirectIssueMaterial();
            objDirectissue.DaGetimslocation(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getimscostcenter")]
        [HttpGet]
        public HttpResponseMessage Getimscostcenter()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnDirectIssueMaterial values = new MdlImsTrnDirectIssueMaterial();
            objDirectissue.DaGetimscostcenter(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getimsdisummary")]
        [HttpGet]
        public HttpResponseMessage Getimsdisummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnDirectIssueMaterial values = new MdlImsTrnDirectIssueMaterial();
            objDirectissue.DaGetimsdisummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getimsavailable")]
        [HttpGet]
        public HttpResponseMessage Getimsavailable(string costcenter_gid)
        {
            MdlImsTrnDirectIssueMaterial values = new MdlImsTrnDirectIssueMaterial();
            objDirectissue.DaGetimsavailable(costcenter_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getimsproducttype")]
        [HttpGet]
        public HttpResponseMessage Getimsproducttype()
        {
            MdlImsTrnDirectIssueMaterial values = new MdlImsTrnDirectIssueMaterial();
            objDirectissue.DaGetimsproducttype(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
      
        [ActionName("GetImsProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetImsProductSummary(string producttype_gid, string product_name)
        {
            MdlImsTrnDirectIssueMaterial values = new MdlImsTrnDirectIssueMaterial();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDirectissue.DaGetImsProductSummary(getsessionvalues.branch_gid,producttype_gid, product_name, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetImsProduct")]
        [HttpGet]
        public HttpResponseMessage GetImsProduct(string product_gid)
        {
            MdlImsTrnDirectIssueMaterial values = new MdlImsTrnDirectIssueMaterial();
            objDirectissue.DaGetImsProduct( product_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostOnIssuematerial")]
        [HttpPost]
        public HttpResponseMessage PostOnIssuematerial(imsproductissue_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDirectissue.DaPostOnIssuematerial(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("PostIssuematerial")]
        [HttpPost]
        public HttpResponseMessage PostIssuematerial(POissuemetrial values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDirectissue.DaPostIssuematerial(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GettmpProductSummary")]
        [HttpGet]
        public HttpResponseMessage GettmpProductSummary()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            MdlImsTrnDirectIssueMaterial values = new MdlImsTrnDirectIssueMaterial();
            objDirectissue.DaGettmpProductSummary(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeletetmpProductSummary")]
        [HttpGet]
        public HttpResponseMessage DeletetmpProductSummary(string tmpmaterialrequisition_gid)
        {
            MdlImsTrnDirectIssueMaterial values = new MdlImsTrnDirectIssueMaterial();
            objDirectissue.DaDeletetmpProductSummary(tmpmaterialrequisition_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("MaterialIssue")]
        [HttpPost]
        public HttpResponseMessage MaterialIssue(issuematerial_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDirectissue.DaMaterialIssue(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}