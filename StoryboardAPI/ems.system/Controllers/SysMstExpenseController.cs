using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ems.system.DataAccess;
using ems.system.Models;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.system.Controllers
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
            MdlSysMstExpense values = new MdlSysMstExpense();
            objDaSysMstExpense.DaGetExpenseCategorySummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostExpense")]
        [HttpPost]
        public HttpResponseMessage PostExpense(expensecategory_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objDaSysMstExpense.DaPostExpense(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteExpense")]
        [HttpGet]
        public HttpResponseMessage DeleteExpense(string expense_gid)
        {
            expensecategory_list objresult = new expensecategory_list();
            objDaSysMstExpense.DaDeleteExpense(expense_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
    }
}