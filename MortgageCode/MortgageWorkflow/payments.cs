using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace MortgageWorkflow
{
    public class payments: CodeActivity
    {
        [Input("project_mortgageamount")]
        public InArgument<string> Amount { get; set; }
        [Input("project_mortgagepayments")]
        public InArgument<string> payments { get; set; }

        [Output("")]
        public OutArgument<string> payment { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            //Read input from Argument

            Entity payment = new Entity("");
            service.Create(payment);

            //output
            //FormattedDescription.Set(executionContext, d);
        }



    }
}
