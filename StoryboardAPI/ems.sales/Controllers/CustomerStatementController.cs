using ems.sales.DataAccess;
using ems.sales.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ems.sales.Controllers
{
    [RoutePrefix("CustomerStatement")]
    [Authorize]
    public class CustomerStatementController : ApiController
    {
        logintoken getsessionvalues = new logintoken();
        session_values ObjGetValues = new session_values();
        DaCustomerStatement objCustomerStatement = new DaCustomerStatement();

        [ActionName("GetCustomerStatementPDF")]
        [HttpGet]
        public HttpResponseMessage GetCustomerStatement(string customer_gid)
        {
            MdlCustomerStatement values = new MdlCustomerStatement();
            var ls_response = new Dictionary<string, object>();
            ls_response = objCustomerStatement.DaGetCustomerStatementPDF(customer_gid, values);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, ls_response);
        }
    }
}