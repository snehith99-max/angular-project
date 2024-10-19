using ems.subscription.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http;
using ems.utilities.Functions;
using System.Data.Odbc;
using System.Data;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using Razorpay.Api;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Stripe;
using Newtonsoft.Json;
using RestSharp;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using System.Data.Common;
using ems.utilities.Models;
using ems.subscription.DataAccess;

namespace ems.subscription.Controllers
{
    [Authorize]
    [RoutePrefix("api/Dynamicdb")]
    public class DynamicdbController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaDynamicdb ObjDaDynamicdb = new DaDynamicdb();
        string domain = string.Empty;



        [ActionName("GetModuledropdown")]
        [HttpGet]
        public HttpResponseMessage Getdatabasenamedropdown()
        {
            MdlDynamicdb values = new MdlDynamicdb();
            ObjDaDynamicdb.DaGetModuledropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        //[ActionName("InternalDynamicDBcreationInSQLFiles")]
        //[HttpPost]
        //public HttpResponseMessage InternalDynamicDBcreationInSQLFiles(Mdlscriptmanagement values)
        //{
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = Objgetgid.gettokenvalues(token);
        //    domain = Request.RequestUri.Host.ToLower();
        //    ObjDaDynamicdb.DaInternalDynamicDBcreationInSQLFiles(domain,getsessionvalues.employee_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}
        [ActionName("GetdynamicdbSummary")]
        [HttpGet]
        public HttpResponseMessage GetdynamicdbSummary()
        {
            MdlDynamicdb values = new MdlDynamicdb();
            ObjDaDynamicdb.DaGetdynamicdbSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("InternalDynamicDBcreationInSQLFiles")]
        [HttpPost]
        public HttpResponseMessage InternalDynamicDBcreationInSQLFiles(Mdlscriptmanagement values)
        {
            //HttpRequest httpRequest;
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            //httpRequest = HttpContext.Current.Request;
            domain = Request.RequestUri.Host.ToLower();
            ObjDaDynamicdb.DaInternalDynamicDBcreationInSQLFiles(domain, values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDynamicDBScriptViewSummary")]
        [HttpGet]
        public HttpResponseMessage GetDynamicDBScriptViewSummary(string dynamicdbscriptmanagement_gid)
        {
            Mdlscriptmanagement values = new Mdlscriptmanagement();
            ObjDaDynamicdb.DaGetDynamicDBScriptViewSummary(values, dynamicdbscriptmanagement_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteDatabase")]
        [HttpPost]
        public HttpResponseMessage DeleteDatabase(Mdlscriptmanagement values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            ObjDaDynamicdb.DaDeleteDatabase(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK,values);
        }
    }
}