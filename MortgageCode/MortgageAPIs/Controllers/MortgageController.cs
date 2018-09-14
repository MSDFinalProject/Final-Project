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
    public class MortgageController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            try
            {
                return LoginCheck.MortgageGet();
            }
            catch (Exception ex)
            {
                return ex.Message+" "+ex.StackTrace;
            }


        }


        [HttpPost]
        public IHttpActionResult Post([FromBody]string query)
        {
            try
            {
                LoginCheck.MortgagePost(query);
                return Ok("Mortgage Added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message+" "+ex.StackTrace);
            }

        }

    }
}