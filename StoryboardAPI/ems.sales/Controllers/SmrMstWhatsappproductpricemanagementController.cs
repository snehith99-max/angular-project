using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.sales.Controllers
{
    [RoutePrefix("api/SmrMstWhatsappproductpricemanagement")]
    [Authorize]
    public class SmrMstWhatsappproductpricemanagementController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaWhatsappproductpricemanagement objwappricemanagement = new DaWhatsappproductpricemanagement();

        //Get Branch summary
        [ActionName("Getwabranchsummary")]
        [HttpGet]
        public HttpResponseMessage Getwabranchsummary()
        {
            Mdlwhatsappproductpricemanagement values = new Mdlwhatsappproductpricemanagement();
            objwappricemanagement.DaGetwabranchsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //Assign Product to branch
        [ActionName("GetWaunassignedproductsummary")]
        [HttpGet]
        public HttpResponseMessage GetWaunassignedproductsummary(string branch_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            Mdlwhatsappproductpricemanagement values = new Mdlwhatsappproductpricemanagement();
            objwappricemanagement.DaGetWaunassignedproductsummary(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        // Assign Product
        [ActionName("postwaassignedlist")]
        [HttpPost]
        public HttpResponseMessage postassignedlist(waassign_list values)
        {
            string token = Request.Headers.GetValues("authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objwappricemanagement.Dapostwaassignedlist(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //Assigned Product summary
        [ActionName("GetwaassignedproductSummary")]
        [HttpGet]
        public HttpResponseMessage GetwaassignedproductSummary(string branch_gid)
        {
            Mdlwhatsappproductpricemanagement values = new Mdlwhatsappproductpricemanagement();
            objwappricemanagement.DaGetwaassignedproductSummary(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("updatewaproductprice")]
        [HttpPost]
        public HttpResponseMessage updatewaproductprice(waassign_list values)
        {
            string token = Request.Headers.GetValues("authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objwappricemanagement.Daupdatewaproductprice(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetbranchwhatsappProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetbranchwhatsappProductSummary(string branch_gid)
        {
            Mdlstockdetails values = new Mdlstockdetails();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objwappricemanagement.DaGetbranchwhatsappProductSummary(branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("BranchAddproducttowhatsapp")]
        [HttpGet]
        public HttpResponseMessage BranchAddproducttowhatsapp(string product_gid,string branch_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            branchproduct_list objresult = new branchproduct_list();
            objwappricemanagement.DaBranchAddproducttowhatsapp(getsessionvalues.user_gid, product_gid,branch_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Branchupdatewhatsappstockstatus")]
        [HttpPost]
        public HttpResponseMessage Branchupdatewhatsappstockstatus(branchproduct_list values)
        {
       
            objwappricemanagement.DaBranchupdatewhatsappstockstatus(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("BranchRemoveproductfromwt")]
        [HttpGet]
        public HttpResponseMessage BranchRemoveproductfromwt(string whatsapp_id,string branch_gid)
        {
            branchproduct_list objresult = new branchproduct_list();
            objwappricemanagement.DaBranchRemoveproductfromwt(whatsapp_id, branch_gid,objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("Getwabproduct")]
        [HttpGet]
        public HttpResponseMessage Getwabproduct()
        {
            Mdlwhatsappproductpricemanagement values = new Mdlwhatsappproductpricemanagement();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objwappricemanagement.DaGetwabproduct(getsessionvalues.branch_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("wtbranchupdatestockstatus")]
        [HttpPost]
        public HttpResponseMessage wtbranchupdatestockstatus(branchproduct_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objwappricemanagement.Dawtbranchupdatestockstatus(values, getsessionvalues.branch_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("updatemobileconfig")]
        [HttpPost]
        public HttpResponseMessage updatemobileconfig(mobileconfig_list values)
        {
            objwappricemanagement.Daupdatemobileconfig(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }

}
