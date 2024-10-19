using ems.hrm.Models;
using ems.utilities.Functions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Web;
using System.Globalization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using MySqlX.XDevAPI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using static System.Drawing.ImageConverter;
using System.Web.UI.WebControls;
using Mysqlx;
using System.Web.Http.Controllers;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Web.Http;
using ems.hrm.DataAccess;
using ems.utilities.Models;
using System.Net.Http;
using System.Net;

namespace ems.hrm.Controllers
{
    [Authorize]
    [RoutePrefix("api/HrmTrnDePromote")]
    public class HrmTrnDePromoteController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaHrmTrnDePromote objDaHrmTrnDePromote = new DaHrmTrnDePromote();

        [ActionName("GetDePromoteSummary")]
        [HttpGet]
        public HttpResponseMessage GetDePromoteSummary()
        {
            MdlHrmTrnDePromote values = new MdlHrmTrnDePromote();
            objDaHrmTrnDePromote.DaGetDePromoteSummary(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeNameDtl")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeNameDtl()
        {
            MdlHrmTrnDePromote values = new MdlHrmTrnDePromote();
            objDaHrmTrnDePromote.DaGetEmployeeNameDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetBranchNameDtl")]
        [HttpGet]
        public HttpResponseMessage GetBranchNameDtl()
        {
            MdlHrmTrnDePromote values = new MdlHrmTrnDePromote();
            objDaHrmTrnDePromote.DaGetBranchNameDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDepartmentNameDtl")]
        [HttpGet]
        public HttpResponseMessage GetDepartmentNameDtl()
        {
            MdlHrmTrnDePromote values = new MdlHrmTrnDePromote();
            objDaHrmTrnDePromote.DaGetDepartmentNameDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetDesignationNameDtl")]
        [HttpGet]
        public HttpResponseMessage GetDesignationNameDtl()
        {
            MdlHrmTrnDePromote values = new MdlHrmTrnDePromote();
            objDaHrmTrnDePromote.DaGetDesignationNameDtl(values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetEmployeeDetail")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeDetail(string employee_gid)
        {
            MdlHrmTrnDePromote values = new MdlHrmTrnDePromote();
            objDaHrmTrnDePromote.DaGetEmployeeDetail(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("GetEmployeeDtl")]
        //[HttpGet]
        //public HttpResponseMessage GetEmployeeDtl()
        //{
        //    MdlHrmTrnDePromote values = new MdlHrmTrnDePromote();
        //    objDaHrmTrnDePromote.DaGetEmployeeDtl(values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("PostDePromotion")]
        [HttpPost]
        public HttpResponseMessage PostDePromotion(DePromotionsummary_list values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            objDaHrmTrnDePromote.DaPostDePromotion(getsessionvalues.employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        //[ActionName("getUpdatedDePromotion")]
        //[HttpPost]
        //public HttpResponseMessage getUpdatedDePromotion(DePromotion_list values)
        //{
        //    string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
        //    getsessionvalues = objgetgid.gettokenvalues(token);
        //    objDaHrmTrnDePromote.DagetUpdatedDePromotion(getsessionvalues.user_gid, values);
        //    return Request.CreateResponse(HttpStatusCode.OK, values);
        //}

        [ActionName("Deletedepromotion")]
        [HttpGet]
        public HttpResponseMessage Deletedepromotion(string params_gid)
        {
            DePromotionsummary_list objresult = new DePromotionsummary_list();
            objDaHrmTrnDePromote.DaDeletedepromotion(params_gid, objresult);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }

        [ActionName("GetDePromotionHistorySummary")]
        [HttpGet]
        public HttpResponseMessage GetDePromotionHistorySummary(string employee_gid)
        {
            MdlHrmTrnDePromote values = new MdlHrmTrnDePromote();
            objDaHrmTrnDePromote.DaGetDePromotionHistorySummary(employee_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}