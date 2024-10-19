using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.system.Controllers
{
    [Authorize]
    [RoutePrefix("api/Company")]

    public class CompanyController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaCompany ObjdaCompany = new DaCompany();



        [ActionName("GetCompany")]
        [HttpGet]
        public HttpResponseMessage getcompany()
        {
            mdlcompany values = new mdlcompany();
            ObjdaCompany.dagetcompany(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetCountrydropdown")]
        [HttpGet]
        public HttpResponseMessage GetCountrydropdown()
        {
            mdlcompany values = new mdlcompany();
            ObjdaCompany.DaGetCountrydropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetCurrencydropdown")]
        [HttpGet]
        public HttpResponseMessage GetCurrencydropdown()
        
        
        {
            mdlcompany values = new mdlcompany();
            ObjdaCompany.DaGetCurrencydropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("PostCompany")]
        [HttpPost]
        public HttpResponseMessage PostCompany(companylists values)
        {
           
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            ObjdaCompany.DaPostCompany(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("UpdatedCompanylogo")]
        [HttpPost]
        public HttpResponseMessage UpdatedCompanylogo()
        {
            HttpRequest httpRequest;
            //Postassetlocationcreation values = new Postassetlocationcreation();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            httpRequest = HttpContext.Current.Request;
            getsessionvalues = Objgetgid.gettokenvalues(token);
            result objResult = new result();

            ObjdaCompany.DaUpdatedCompanylogo(httpRequest, objResult, getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }
    }
} 