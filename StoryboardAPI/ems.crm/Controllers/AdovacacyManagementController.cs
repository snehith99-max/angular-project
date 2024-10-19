using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.crm.DataAccess;
using ems.crm.Models;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
namespace ems.crm.Controllers
{
    [Authorize]
    [RoutePrefix("api/AdovacacyManagement")]
    public class AdovacacyManagementController:ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaAdovacacyManagement ObjAdovacacyManagement = new DaAdovacacyManagement();


        [ActionName("GetLeaddropdownforadvocacy")]
        [HttpGet]
        public HttpResponseMessage GetLeaddropdown()
        {
            MdlAdovacacyManagement values = new MdlAdovacacyManagement();
            ObjAdovacacyManagement.DaGetLeaddropdownforadvocacy(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCustomerdropdown")]
        [HttpGet]
        public HttpResponseMessage GetCustomerdropdown()
        {
            MdlAdovacacyManagement values = new MdlAdovacacyManagement();
            ObjAdovacacyManagement.DaGetCustomerdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostAdovacacy")]
        [HttpPost]
        public HttpResponseMessage PostAdovacacy(postadovacacy_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            ObjAdovacacyManagement.DaPostAdovacacy(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetAdovacacysummary")]
        [HttpGet]
        public HttpResponseMessage GetAdovacacysummary()
        {
            MdlAdovacacyManagement values = new MdlAdovacacyManagement();
            ObjAdovacacyManagement.DaGetAdovacacysummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        
        [ActionName("GetAdvocacyDetails")]
        [HttpGet]
        public HttpResponseMessage GetAdvocacyDetails( string leadbank_gid)
        {
            MdlAdovacacyManagement values = new MdlAdovacacyManagement();
            ObjAdovacacyManagement.DaGetAdvocacyDetails(values, leadbank_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }        
        [ActionName("DeleteAdvocacy")]
        [HttpGet]
        public HttpResponseMessage DeleteAdvocacy( string reference_leadbankgid,string adovacacy_leadbankgid)
        {
            MdlAdovacacyManagement values = new MdlAdovacacyManagement();
            ObjAdovacacyManagement.DaDeleteAdvocacy(values, reference_leadbankgid, adovacacy_leadbankgid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }










    }

}