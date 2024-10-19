using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ems.hrm.Controllers
{
    [RoutePrefix("api/HrmRptEmployeeFormA")]
    [Authorize]
    public class HrmRptEmployeeFormAController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmRptEmployeeFormA objDaHrmRptEmployeeFormA = new DaHrmRptEmployeeFormA();

        [ActionName("GetEmployeeFormA")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeFormA()
        {
            MdlHrmRptEmployeeFormA values = new MdlHrmRptEmployeeFormA();
            objDaHrmRptEmployeeFormA.DaGetEmployeeFormA(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        // Image Upload
        [ActionName("GetEmployeeImage")]
        [HttpPost]

        public HttpResponseMessage GetEmployeeImage()
        {
            HttpRequest httpRequest;
            httpRequest = HttpContext.Current.Request;
            MdlHrmRptEmployeeFormA objResult = new MdlHrmRptEmployeeFormA();
            objDaHrmRptEmployeeFormA.DaGetEmployeeImage(httpRequest, objResult);
            return Request.CreateResponse(HttpStatusCode.OK, objResult);
        }

        [ActionName("ExportFormAExcel")]
        [HttpGet]
        public HttpResponseMessage ExportFormAExcel()
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            var ls_response = new Dictionary<string, object>();
            MdlHrmRptEmployeeFormA values = new MdlHrmRptEmployeeFormA();
            try { ls_response = objDaHrmRptEmployeeFormA.DaExportFormAExcel(values); }
            catch (Exception ex)
            {

                ls_response = new Dictionary<string, object>
                    {
                        {"status",false },
                        {"message",ex.Message}
                    };
            }

            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
       
        }

    }
}