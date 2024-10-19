using ems.hrm.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.hrm.Models;

namespace ems.hrm.Controllers
{
    [RoutePrefix("api/HrmTrnForm25")]
    [Authorize]
    public class HrmTrnForm25Controller:ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmTrnForm25 objHrmTrnForm25 = new DaHrmTrnForm25();

        [ActionName("GetMusterSummary")]
        [HttpGet]
        public HttpResponseMessage GetMusterSummary()
        {
            MdlHrmTrnForm25 values = new MdlHrmTrnForm25();
            objHrmTrnForm25.DaGetMusterSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetDetailSummary")]
        [HttpGet]
        public HttpResponseMessage GetDetailSummary()
        {
            MdlHrmTrnForm25 values = new MdlHrmTrnForm25();
            objHrmTrnForm25.DaGetDetailSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetForm25Rpt")]
        [HttpGet]
        public HttpResponseMessage GetForm25Rpt(string month, string year, string branch_gid,string department_gid)

     
        {
            MdlHrmTrnForm25 values = new MdlHrmTrnForm25();
            var ls_response = new Dictionary<string, object>();
            ls_response = objHrmTrnForm25.DaGetForm25Rpt(month, year, branch_gid,department_gid, values);

            return Request.CreateResponse(HttpStatusCode.OK, ls_response);
        }

    }
}