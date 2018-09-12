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
    public class PayController:ApiController
    {

        [HttpPost]
        public IHttpActionResult Post([FromBody]string p)
        {
            try
            {
                string x= LoginCheck.Paymentpay(p);
                return Ok("Payment Sucessful  "+x);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}