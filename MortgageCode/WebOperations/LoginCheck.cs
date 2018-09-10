using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Client;
using System.Net;
using System.IO;
using System.Threading;

namespace WebOperationsm
{
    public class LoginCheck
    {
        public bool Exist(string user) {
             CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
             IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? (IOrganizationService)client.OrganizationServiceProxy;

             string query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>  <entity name = 'contact' >
                            < attribute name='fullname' />
	                        <attribute name = 'parentcustomerid' />
                            < attribute name='telephone1' />
                            <attribute name = 'emailaddress1' />
                            < attribute name='contactid' />
                            <order attribute = 'fullname' descending='false' />
                            <filter type = 'and' >
                              < filter type='and'>
                                <condition attribute = 'project_usename' operator='eq' value='' />
                                <condition attribute = 'project_password' operator='eq' value='' />
                              </filter>
                            </filter>
                          </entity>
                        </fetch>";
            EntityCollection collection = service.RetrieveMultiple(new FetchExpression(query));
            if(collection.TotalRecordCount == 1 )
            {
                return true;
            }
            else
            {
                return false;
            }
            

        }

    }
}
