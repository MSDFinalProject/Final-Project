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
    public class RiskController : ApiController
    {
        //CreditCheck creditNum = new CreditCheck();
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                Random randNum = new Random();
                int randRisk = randNum.Next(1, 101);
               // creditNum.riskScore = randRisk;
                return Ok(randRisk);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
