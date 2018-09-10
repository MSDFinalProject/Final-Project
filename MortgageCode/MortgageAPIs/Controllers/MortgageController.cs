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

namespace MortgageAPIs.Controllers
{
    [EnableCors("*", "*", "*")]
    public class MortgageController: ApiController
    {
        [HttpGet]
        public string Get()
        {
            return LoginCheck.MortgageGet();
        }


        [HttpPost]
        public IHttpActionResult Post(string p)
        {
            try
            {
                LoginCheck.MortgagePost(p);
                return Ok("Mortgage Added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}