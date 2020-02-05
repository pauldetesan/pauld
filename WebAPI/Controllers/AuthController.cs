using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace WebAPI.Controllers
{
    [RoutePrefix("Auth")]
    [Authorize]
    public class AuthController : ApiController
    {
        [Route("Test")]
        [HttpGet]
        public HttpResponseMessage CheckAuth()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello");
        }
        public JsonResult<int> Login(string Username, string Password)
        {
            return Json(1);
        }
    }
}
