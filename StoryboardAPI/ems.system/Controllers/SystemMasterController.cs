﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ems.system.Models;
using ems.system.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;

namespace ems.system.Controllers
{
    [RoutePrefix("api/SystemMaster")]
    [Authorize]
    public class SystemMasterController : ApiController
    {
        DaSystemMaster objDaSystemMaster = new DaSystemMaster();
        session_values Objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();

        // First Level Menu List
        [ActionName("GetFirstLevelMenu")]
        [HttpGet]
        public HttpResponseMessage GetFirstLevelMenu()
        {
            menu objmaster = new menu();
            objDaSystemMaster.DaGetFirstLevelMenu(objmaster);
            return Request.CreateResponse(HttpStatusCode.OK, objmaster);
        }
        // Second Level Menu List Based on First Level
        [ActionName("GetSecondLevelMenu")]
        [HttpGet]
        public HttpResponseMessage GetSecondLevelMenu(string module_gid_parent)
        {
            menu objmaster = new menu();
            objDaSystemMaster.DaGetSecondLevelMenu(objmaster, module_gid_parent);
            return Request.CreateResponse(HttpStatusCode.OK, objmaster);
        }
        // Third Level Menu List Based on Second Level
        [ActionName("GetThirdLevelMenu")]
        [HttpGet]
        public HttpResponseMessage GetThirdLevelMenu(string module_gid_parent)
        {
            menu objmaster = new menu();
            objDaSystemMaster.DaGetThirdLevelMenu(objmaster, module_gid_parent);
            return Request.CreateResponse(HttpStatusCode.OK, objmaster);
        }
        // Fourth Level Menu List Based on Second Level
        [ActionName("GetFourthLevelMenu")]
        [HttpGet]
        public HttpResponseMessage GetFourthLevelMenu(string module_gid_parent)
        {
            menu objmaster = new menu();
            objDaSystemMaster.DaGetFourthLevelMenu(objmaster, module_gid_parent);
            return Request.CreateResponse(HttpStatusCode.OK, objmaster);
        }
        // Menu Add
        [ActionName("PostMenudAdd")]
        [HttpPost]
        public HttpResponseMessage PostMenudAdd(menu values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSystemMaster.DaPostMenudAdd(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        // Menu Mapping Summary

        [ActionName("GetMenuMappingSummary")]
        [HttpGet]
        public HttpResponseMessage GetMenuMappingSummary()
        {
            menu objmaster = new menu();
            objDaSystemMaster.DaGetMenuMappingSummary(objmaster);
            return Request.CreateResponse(HttpStatusCode.OK, objmaster);
        }


        [ActionName("GetMenuMappingEdit")]
        [HttpGet]
        public HttpResponseMessage GetMenuMappingEdit(string menu_gid)
        {
            menu objmaster = new menu();
            objDaSystemMaster.DaGetMenuMappingEdit(menu_gid, objmaster);
            return Request.CreateResponse(HttpStatusCode.OK, objmaster);
        }

        [ActionName("GetMenuMappingInactivate")]
        [HttpPost]
        public HttpResponseMessage GetMenuMappingInactivate(menu values)
        {
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = Objgetgid.gettokenvalues(token);
            objDaSystemMaster.DaGetMenuMappingInactivate(values, getsessionvalues.employee_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("GetMenuMappingInactivateview")]
        [HttpGet]
        public HttpResponseMessage GetMenuMappingInactivateview(string menu_gid)
        {
            menu values = new menu();
            objDaSystemMaster.DaGetMenuMappingInactivateview(menu_gid, values);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("GetEmployeelist")]
        [HttpGet]
        public HttpResponseMessage GetEmployeelist()
        {
            mdlemployee objmaster = new mdlemployee();
            objDaSystemMaster.DaGetEmployeelist(objmaster);
            return Request.CreateResponse(HttpStatusCode.OK, objmaster);
        }


    }
}