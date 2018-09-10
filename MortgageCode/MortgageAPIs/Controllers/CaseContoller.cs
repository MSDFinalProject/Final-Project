﻿using System;
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
    public class CaseContoller : ApiController
    {
        [HttpGet]
        public string Get() 
        {
            return LoginCheck.CaseGet();
        }

        [HttpPost]
        public IHttpActionResult Post(string p)
        {
            try
            {
                LoginCheck.CasePost(p);
                return Ok("Cases Added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}