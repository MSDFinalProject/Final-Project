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
    public class ApplyMortgageController : ApiController
    {
        static CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
        IOrganizationService service = (IOrganizationService)
            client.OrganizationWebProxyClient != null ? (IOrganizationService)client.OrganizationWebProxyClient : (IOrganizationService)client.OrganizationServiceProxy;
        

        [HttpGet]
        public string Get()
        //Need to get CONTACT entities first, and then get 'this' CONTACT's MORTGAGE entities. 
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
            
            string mortgages = "";

            foreach (Entity mortgage in collections.Entities)
            {
                mortgages += mortgage.Attributes["project_mortgagenumbera"].ToString() + ",";
                mortgages += mortgage.Attributes["project_name"].ToString() + ",";
                mortgages += mortgage.Attributes["project_mortgageterm"].ToString() + ",";
                mortgages += mortgage.Attributes["project_mortgageamount"].ToString() + ",";
                mortgages += mortgage.Attributes["project_monthlypayment"].ToString() + ";";
            }
            return mortgages;

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

                Entity contactRecord = new Entity("project_mortgage");
                contactRecord.Attributes.Add("project_mortgageamount", data.IndexOf(0));    //Currency
                contactRecord.Attributes.Add("project_mortgageterm", data.IndexOf(1));      //Whole Number
                contactRecord.Attributes.Add("project_region", data.IndexOf(2));            //Two Option
                //Missing "state", but could be coupled with Region

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
