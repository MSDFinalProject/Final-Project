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

namespace MortgageCode
{
    class Program
    {
        static void Main(string[] args)
        {
            CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
            IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient != null ? (IOrganizationService)client.OrganizationWebProxyClient : (IOrganizationService)client.OrganizationServiceProxy;


            #region MortgageNumber
            //CreateAttributeRequest widgetSerialNumberAttributeRequest = new CreateAttributeRequest
            //{
            //    EntityName = "project_mortgage",
            //    Attribute = new StringAttributeMetadata
            //    {
            //        //Define the format of the attribute
            //        AutoNumberFormat = "{ DATETIMEUTC:yyyymm}-{ SEQNUM: 6}",
            //        LogicalName = "project_mortgagenumberA",
            //        SchemaName = "project_mortgagenumberA",
            //        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
            //        MaxLength = 12, // The MaxLength defined for the string attribute must be greater than the length of the AutoNumberFormat value, that is, it should be able to fit in the generated value.
            //        DisplayName = new Label("Mortgage Number", 1033),
            //        Description = new Label("Mortgage Number of the Account.", 1033)
            //    }
            //};
            //service.Execute(widgetSerialNumberAttributeRequest);
            #endregion

        }
    }
}
