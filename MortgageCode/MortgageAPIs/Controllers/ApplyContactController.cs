using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Crm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;

namespace MortgageControllers.Controllers
{
    public class ApplyContactController : ApiController
    {
        static CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
        IOrganizationService service = (IOrganizationService)
            client.OrganizationWebProxyClient != null ? (IOrganizationService)client.OrganizationWebProxyClient : (IOrganizationService)client.OrganizationServiceProxy;


        [HttpPost]
        public IHttpActionResult Post(string p)
        {
            if (p != null)
            {
                ArrayList data = new ArrayList();
                string contents = "";
                for (int i = 0; i < p.Length; i++)
                {
                    if (p[i] == ',')
                    {
                        data.Add(contents);
                        continue;
                    }
                    else
                    {
                        contents += p[i];
                    }
                }

                Entity contactRecord = new Entity("contact");
                contactRecord.Attributes.Add("emailaddress1", data.IndexOf(0));
                contactRecord.Attributes.Add("project_password", data.IndexOf(1));
                //Both First and Last name
                contactRecord.Attributes.Add("fullname", data.IndexOf(2));
                //Both Address 1 and 2, city, state and zip
                contactRecord.Attributes.Add("address1_composite", data.IndexOf(3));
                contactRecord.Attributes.Add("project_socialsecuritynumber", data.IndexOf(4));

                service.Create(contactRecord);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
