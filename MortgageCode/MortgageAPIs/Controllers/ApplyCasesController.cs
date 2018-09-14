using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MortgageControllers.Controllers
{
    public class ApplyCasesController : ApiController
    {
        static CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
        IOrganizationService service = (IOrganizationService)
            client.OrganizationWebProxyClient != null ? (IOrganizationService)client.OrganizationWebProxyClient : (IOrganizationService)client.OrganizationServiceProxy;


        [HttpGet]
        public string Get()
        //Need to get CONTACT entities first, and then get 'this' CONTACT's INCIDENT entities. 
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='project_mortgage'>
                                <attribute name='project_name' />
                                <attribute name='createdon' />
                                <attribute name='project_mortgageamount' />
                                <attribute name='project_contactid' />
                                <attribute name='project_mortgageid' />
                                <order attribute='project_contactid' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";

            EntityCollection collections = service.RetrieveMultiple(new FetchExpression(xml));
            
            string cases = "";

            foreach (Entity incident in collections.Entities)
            {
                cases += incident.Attributes["ticketnumber"].ToString() + ",";
                cases += incident.Attributes["title"].ToString() + ",";
                cases += incident.Attributes["project_priority"].ToString() + ",";
                cases += incident.Attributes["project_problemdescription"].ToString() + ",";
                cases += incident.Attributes["statecode"].ToString() + ";";
            }
            return cases;

        }


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

                Entity caseRecord = new Entity("incident");
                caseRecord.Attributes.Add("title", data.IndexOf(0));
                caseRecord.Attributes.Add("project_priority", data.IndexOf(1));
                caseRecord.Attributes.Add("project_problemdescription", data.IndexOf(2));
                //Fname and Lname are being ignored

                service.Create(caseRecord);
                return Ok("Case added");
            }
            else
            {
                return BadRequest("Sorry and error occured  please contact support");
            }
        }
    }
}
