using ems.law.DataAccess;
using ems.law.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
namespace ems.law.Controllers
{
    [Authorize]
    [RoutePrefix("api/LawTrnArbitration")]
    public class LawTrnArbitrationController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaLawTrnArbitration objarbit = new DaLawTrnArbitration();

        [ActionName("GetArbitrationsummary")]
        [HttpGet]
        public HttpResponseMessage GetArbitrationsummary()
        {
            MdlLawTrnArbitration values = new MdlLawTrnArbitration();
            objarbit.DaGetArbitrationsummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

    }
}