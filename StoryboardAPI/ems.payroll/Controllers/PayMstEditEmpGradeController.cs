using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ems.payroll.DataAccess;
using ems.payroll.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Web.Http.Results;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ems.payroll.Controllers
{
    [RoutePrefix("api/PayMstEditEmpGrade")]
    [Authorize]
    public class PayMstEditEmpGradeController : ApiController
    {
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaPayMstEditEmp2Grade objDaDaPayMstEditEmp2Grade = new DaPayMstEditEmp2Grade();



        [ActionName("editgrade2employeedetails")]
        [HttpGet]
        public HttpResponseMessage editgrade2employeedetails(string employee2salarygradetemplate_gid)
        {
            Emp2Gradeedit_list values = new Emp2Gradeedit_list();
            values = objDaDaPayMstEditEmp2Grade.Daeditgrade2employeedetails(employee2salarygradetemplate_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Getsalarygradetemplatedropdown")]
        [HttpGet]

        public HttpResponseMessage Getgradetemplatedropdown()
        {
            MdlPayMstGradeConfirm values = new MdlPayMstGradeConfirm();
            objDaDaPayMstEditEmp2Grade.DaGetsalarygradetemplatedropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("Editadditionsummary")]
        [HttpGet]
        public HttpResponseMessage Editadditionsummary(string employee2salarygradetemplate_gid)
        {
            MdlPayMstEditEmp2Grade values = new MdlPayMstEditEmp2Grade();
            objDaDaPayMstEditEmp2Grade.DaEditadditionsummary(employee2salarygradetemplate_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Editdeductionsummary")]
        [HttpGet]
        public HttpResponseMessage Editdeductionsummary(string employee2salarygradetemplate_gid)
        {
            MdlPayMstEditEmp2Grade values = new MdlPayMstEditEmp2Grade();
            objDaDaPayMstEditEmp2Grade.DaEditdeductionsummary(employee2salarygradetemplate_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Editotherssummary")]
        [HttpGet]
        public HttpResponseMessage Editotherssummary(string employee2salarygradetemplate_gid)
        {
            MdlPayMstEditEmp2Grade values = new MdlPayMstEditEmp2Grade();
            objDaDaPayMstEditEmp2Grade.DaEditotherssummary(employee2salarygradetemplate_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Updateemployee2grade")]
        [HttpPost]
        public HttpResponseMessage Updateemployee2grade(employee2editgradelist values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaDaPayMstEditEmp2Grade.DaUpdateemployee2grade(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }







    }
}
 
