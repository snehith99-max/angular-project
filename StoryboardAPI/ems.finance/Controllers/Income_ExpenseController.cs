using ems.finance.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.IO;
using ems.finance.Models;


namespace ems.finance.Controllers
{
    [RoutePrefix("api/Income_Expense")]
    [Authorize]

    public class Income_ExpenseController : ApiController
    {
        session_values Objget = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaIncome_Expense objincome = new DaIncome_Expense();

        [ActionName("GetIncomesummary")]
        [HttpGet]
        public HttpResponseMessage GetIncomesummary()
        {
            if (Objget != null)
            {
                MdlIncome_Expanse values = new MdlIncome_Expanse();
                objincome.DaGetIncomeSummary(values);
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"100");
            }
        } 
        [ActionName("GetExpenseSummary")]
        [HttpGet]
        public HttpResponseMessage GetExpenseSummary()
        {
            if (Objget != null)
            {
                MdlIncome_Expanse values = new MdlIncome_Expanse();
                objincome.DaGetExpenseSummary(values);
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"100");
            }
        } 
        [ActionName("IncomeExpenseGraph")]
        [HttpGet]
        public HttpResponseMessage IncomeExpenseGraph()
        {
            if (Objget != null)
            {
                MdlIncome_Expanse values = new MdlIncome_Expanse();
                objincome.DaIncomeExpenseGraph(values);
                return Request.CreateResponse(HttpStatusCode.OK, values);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"100");
            }
        }
    }
}