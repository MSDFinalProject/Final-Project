using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace MortgageWorkflow
{
    public class RiskScore : CodeActivity
    {
        //[Input("project_monthlypayment")]
        //public InArgument<Money> MonthPayment { get; set; }

        [Output("project_credit")]
        public OutArgument<string> risk { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            //Read input from Argument

            try
            {
                WebRequest request = WebRequest.Create("http://localhost:64288/api/risk");
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                response.Close();
                decimal RiskScore = Convert.ToDecimal(responseString);
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in Payment Creation Workflow.", ex);
            }

            catch (Exception ex)
            {
                tracingService.Trace("Payment Creation: {0}", ex.ToString());
                throw;
            }
            //output
            //FormattedDescription.Set(executionContext, d);
        }
    }
}
