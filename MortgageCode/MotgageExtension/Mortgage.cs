using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace MotgageExtension
{
    public class idk:IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            #region Setup
            // Extract the tracing service for use in debugging sandboxed plug-ins.
            // If you are not registering the plug-in in the sandbox, then you do
            // not have to add any tracing service related code.
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference which you will need for
            // web service calls.
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            #endregion

            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Entity Mortgage = (Entity)context.InputParameters["Target"];
               

                try
                {
                    //// Using Query by Attribute
                    //QueryByAttribute query = new QueryByAttribute();
                    //query.EntityName = "new_configuration";
                    //query.ColumnSet = new ColumnSet(new string[] { "new_value" });
                    //query.AddAttributeValue("new_tax", "Federal Tax");
                   



                  
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                }
            }//end if 
        }//end execute
    }
}
