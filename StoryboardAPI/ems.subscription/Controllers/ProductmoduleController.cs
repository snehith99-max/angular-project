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
    [RoutePrefix("api/Productmodule")]
    public class ProductmoduleController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaProductmodule ObjDaProductmodule = new DaProductmodule();
        string domain = string.Empty;
       
        [ActionName("PostProductmodule")]
        [HttpPost]
        public HttpResponseMessage PostProductmodule(productmodulelists values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            ObjDaProductmodule.DaPostProductmodule(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetproductmoduleSummary")]
        [HttpGet]
        public HttpResponseMessage GetproductmoduleSummary()
        {
            MdlProductmodule values = new MdlProductmodule();
            ObjDaProductmodule.DaGetproductmoduleSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("DeleteProductmodule")]
        [HttpGet]
        public HttpResponseMessage DeleteProductmodule(string productmodule_gid)
        {
            MdlProductmodule objresult = new MdlProductmodule();
            ObjDaProductmodule.DaDeleteProductmodule(productmodule_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}