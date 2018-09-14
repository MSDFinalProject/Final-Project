using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using System.Web.Http.Results;
using WebOperationsm;

namespace MortgageAPIs
{
    [EnableCors("*", "*", "*")]
    public class LoginController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(string query)
        {
            try
            {
                string L = LoginCheck.Exist(query);
                if (L == "true")
                    return Ok("Login Successful");
                else
                    throw new Exception("Invalid Login");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]string query)
        {
            try {
                LoginCheck.ContactPost(query);
                return Ok("Contact Added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message+":::"+ex.StackTrace);
            }
        }


    }
}
