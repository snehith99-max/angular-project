using ems.sales.DataAccess;
using ems.utilities.Functions;
using ems.utilities.Models;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using ems.sales.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ems.sales.Controllers
{
    [Authorize]
    [RoutePrefix("api/Mintsoft")]
    public class MintsoftController : ApiController
    {
        session_values objgetgid = new session_values();
        logintoken getsessionvalues = new logintoken();
        DaMintsoft objdaproduct = new DaMintsoft();

        [ActionName("CreateOrder")]
        [HttpPost]
        public HttpResponseMessage CreateOrder(MdlSalesOrder values)
        {
            result objresult = new result();
            objresult = objdaproduct.DaCreateOrder(values.salesorder_gid, values.CourierServiceId);
            return Request.CreateResponse(HttpStatusCode.OK, objresult);
        }
        [ActionName("MintsoftCourierDetails")]
        [HttpGet]
        public async Task<HttpResponseMessage> MintsoftCourierDetails()
        {
            get values = new get();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            values = await objdaproduct.DaMintsoftCourierDetails(getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
        [ActionName("MintsoftAsnstatusgoodssupplier")]
        [HttpGet]
        public async Task<HttpResponseMessage> MintsoftAsnstatusgoodssupplier()
        {
            get values = new get();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            values = await objdaproduct.DaMintsoftAsnstatusgoodssupplier(getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }

        [ActionName("Mintsoftgetsalesorders")]
        [HttpGet]
        public async Task<HttpResponseMessage> Mintsoftgetsalesorders()
        {
            get values = new get();
            string token = Request.Headers.GetValues("Authorization").FirstOrDefault();
            getsessionvalues = objgetgid.gettokenvalues(token);
            values = await objdaproduct.DaMintsoftgetsalesorders(getsessionvalues.user_gid);
            return Request.CreateResponse(HttpStatusCode.OK, values);
        }
    }
}
