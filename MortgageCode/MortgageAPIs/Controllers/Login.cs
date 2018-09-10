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

namespace MortgageAPIs
{
    [EnableCors("*", "*", "*")]
    public class Login : ApiController
    {
        //CreditCheck creditNum = new CreditCheck();
        [HttpGet]
        public IHttpActionResult Get(string login)
        {
            try
            {
              
                
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
