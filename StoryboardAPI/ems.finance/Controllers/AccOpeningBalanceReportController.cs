using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.finance.DataAccess;
using ems.finance.Models;

namespace ems.finance.Controllers
{
    [Authorize]
    [RoutePrefix("api/AccOpeningBalanceReport")]
    public class AccOpeningBalanceReportController : ApiController
    {

        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaAccOpeningBalanceReport objdacreditcard = new DaAccOpeningBalanceReport();

        [ActionName("getFolders")]
        [HttpGet]
        public HttpResponseMessage getFolders(string entity_name, string finyear)
        { 
            MdlAccOpeningBalanceReport objresult = new MdlAccOpeningBalanceReport();
            objdacreditcard.DaOpeningBalanceReportFolderList(entity_name, finyear, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }


    }

}
