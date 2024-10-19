//using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using ems.outlet.Dataaccess;
using ems.outlet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.outlet.Controller
{
    [Authorize]
    [RoutePrefix("api/SysMstExpense")]

    public class SysMstExpenseController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaSysMstExpense objDaSysMstExpense = new DaSysMstExpense();

        [ActionName("GetExpenseCategorySummary")]
        [HttpGet]
        public HttpResponseMessage GetExpenseCategorySummary()
        {
            ems.outlet.Models.MdlSysMstExpense values = new ems.outlet.Models.MdlSysMstExpense();
            objDaSysMstExpense.DaGetExpenseCategorySummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostExpense")]
        [HttpPost]
        public HttpResponseMessage PostExpense(expensecategory_listdata values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objDaSysMstExpense.DaPostExpense(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteExpense")]
        [HttpGet]
        public HttpResponseMessage DeleteExpense(string expense_gid)
        {
            expensecategory_listdata objresult = new expensecategory_listdata();
            objDaSysMstExpense.DaDeleteExpense(expense_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}