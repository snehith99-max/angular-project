using ems.hrm.DataAccess;
using ems.hrm.Models;
using ems.utilities.Functions;
using ems.utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ems.hrm.Controllers
{
    [Authorize]
    [RoutePrefix("api/HrmForm22")]
    public class HrmForm22Controller : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmForm22 objdaform22 = new DaHrmForm22();

        [ActionName("CompanySummary")]
        [HttpGet]
        public HttpResponseMessage CompanySummary()
        {
            MdlHrmForm22 values = new MdlHrmForm22();
            objdaform22.DaCompanySummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }


        [ActionName("GetYearReturnSummary")]
        [HttpGet]
        public HttpResponseMessage GetYearReturnSummary()
        {
            MdlHrmForm22 values = new MdlHrmForm22();
            objdaform22.DaGetYearReturnSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Getformdropdown")]
        [HttpGet]

        public HttpResponseMessage Getformdropdown()
        {
            MdlHrmForm22 values = new MdlHrmForm22();
            objdaform22.DaGetformdropdown(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostForm")]
        [HttpPost]
        public HttpResponseMessage PostForm(form_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objdaform22.DaPostForm(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("GetHalfyearlysubrule3Summary")]
        //[HttpGet]
        //public HttpResponseMessage GetHalfyearlysubrule3Summary(string form_gid)
        //{
        //    MdlHrmForm22 values = new MdlHrmForm22();
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = objgetgid.gettokenvalues(token);
        //    objdaform22.DaGetHalfyearlysubrule3Summary(values, form_gid);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);

        //}

        [ActionName("Form2SubRule3Submit")]
        [HttpPost]
        public HttpResponseMessage Form2SubRule3Submit(form2subrule3 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaform22.DaForm2SubRule3Submit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Form2SubRule4Submit")]
        [HttpPost]
        public HttpResponseMessage Form2SubRule4Submit(form2subrule4 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaform22.DaForm2SubRule4Submit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Form21SubRule1Submit")]
        [HttpPost]
        public HttpResponseMessage Form21SubRule1Submit(form21subrule1 values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaform22.DaForm21SubRule1Submit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditForm22SubRule3")]
        [HttpGet]
        public HttpResponseMessage GetEditForm22SubRule3(string form_gid, string processed_year)
        {
            MdlHrmForm22 values = new MdlHrmForm22();
            objdaform22.DaGetEditForm22SubRule3(form_gid, processed_year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditForm22SubRule4")]
        [HttpGet]
        public HttpResponseMessage GetEditForm22SubRule4(string form_gid, string processed_year)
        {
            MdlHrmForm22 values = new MdlHrmForm22();
            objdaform22.DaGetEditForm22SubRule4(form_gid, processed_year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditForm22SubRule1")]
        [HttpGet]
        public HttpResponseMessage GetEditForm22SubRule1(string form_gid, string processed_year)
        {
            MdlHrmForm22 values = new MdlHrmForm22();
            objdaform22.DaGetEditForm22SubRule1(form_gid, processed_year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteForm22")]
        [HttpGet]
        public HttpResponseMessage DeleteDepartment(string params_gid)
        {
            form_list objresult = new form_list();
            objdaform22.DaDeleteForm22(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetEditCompany")]
        [HttpGet]
        public HttpResponseMessage GetEditCompany(string form_name, string processed_year)
        {
            MdlHrmForm22 values = new MdlHrmForm22();
            objdaform22.DaGetEditCompany(form_name, processed_year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("UpdateCompanyDetails")]
        [HttpPost]
        public HttpResponseMessage UpdateCompanyDetails(companydetails values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaform22.DaUpdateCompanyDetails(getsessionvalues.user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetProductSummary")]
        [HttpGet]
        public HttpResponseMessage GetProductSummary(string form_name, string processed_year)
        {
            MdlHrmForm22 values = new MdlHrmForm22();
            objdaform22.DaGetProductSummary(form_name, processed_year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("PostProduct")]
        [HttpPost]
        public HttpResponseMessage PostProduct(product_list values, string user_gid)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            objdaform22.DaPostProduct(user_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("DeleteProduct")]
        [HttpGet]
        public HttpResponseMessage DeleteProduct(string params_gid)
        {
            product_list objresult = new product_list();
            objdaform22.DaDeleteProduct(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("LeaveWagesubmit")]
        [HttpPost]
        public HttpResponseMessage LeaveWagesubmit(leavewage_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaform22.DaLeaveWagesubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("EmployeeManagementsubmit")]
        [HttpPost]
        public HttpResponseMessage EmployeeManagementsubmit(employeement_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaform22.DaEmployeeManagementsubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Concessionssubmit")]
        [HttpPost]
        public HttpResponseMessage Concessionssubmit(concession_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objdaform22.DaConcessionssubmit(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditConcession")]
        [HttpGet]
        public HttpResponseMessage GetEditConcession(string form_gid, string processed_year)
        {
            MdlHrmForm22 values = new MdlHrmForm22();
            objdaform22.DaGetEditConcession(form_gid, processed_year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditLeaveWage")]
        [HttpGet]
        public HttpResponseMessage GetEditLeaveWage(string form_gid, string processed_year)
        {
            MdlHrmForm22 values = new MdlHrmForm22();
            objdaform22.DaGetEditLeaveWage(form_gid, processed_year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEditEmployeeManagement")]
        [HttpGet]
        public HttpResponseMessage GetEditEmployeeManagement(string form_gid, string processed_year)
        {
            MdlHrmForm22 values = new MdlHrmForm22();
            objdaform22.DaGetEditEmployeeManagement(form_gid, processed_year, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}