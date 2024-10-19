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
    [RoutePrefix("api/Scriptmanagement")]
    public class ScriptmanagementController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        Dascriptmanagement ObjdaScriptmanagement = new Dascriptmanagement();

        [ActionName("Getdatabasenamedropdown")]
        [HttpGet]
        public HttpResponseMessage Getdatabasenamedropdown()
        {
            Mdlscriptmanagement values = new Mdlscriptmanagement();
            ObjdaScriptmanagement.DaGetdatabasenamedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getserverdropdown")]
        [HttpGet]
        public HttpResponseMessage Getserverdropdown()
        {
            Mdlscriptmanagement values = new Mdlscriptmanagement();
            ObjdaScriptmanagement.DaGetserverdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetScriptSummary")]
        [HttpGet]
        public HttpResponseMessage GetScriptSummary()
        {
            Mdlscriptmanagement values = new Mdlscriptmanagement();
            ObjdaScriptmanagement.DaGetScriptSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetScriptViewSummary")]
        [HttpGet]
        public HttpResponseMessage GetScriptViewSummary(string dbscriptmanagementdocument_gid)
        {
            Mdlscriptmanagement values = new Mdlscriptmanagement();
            ObjdaScriptmanagement.DaGetScriptViewSummary(values, dbscriptmanagementdocument_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetScriptexceptionViewSummary")]
        [HttpGet]
        public HttpResponseMessage GetScriptexceptionViewSummary(string dbscriptmanagementdocument_gid)
        {
            Mdlscriptmanagement values = new Mdlscriptmanagement();
            ObjdaScriptmanagement.DaGetScriptexceptionViewSummary(values, dbscriptmanagementdocument_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("Getserverdatabasenamedropdown")]
        [HttpGet]
        public HttpResponseMessage Getserverdatabasenamedropdown(string server_gid)
        {
            Mdlscriptmanagement values = new Mdlscriptmanagement();
            ObjdaScriptmanagement.DaGetserverdatabasenamedropdown(server_gid,values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}