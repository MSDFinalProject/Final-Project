using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Client;
using System.Net;
using System.IO;
using System.Threading;

namespace WebOperations
{
    public class Class1
    {

        static CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
        IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? (IOrganizationService)client.OrganizationServiceProxy;
    }
}
