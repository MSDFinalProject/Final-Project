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
        [RequiredArgument]
        [Input("Mortgage")]
        [ReferenceTarget("project_mortgage")]
        public InArgument<EntityReference> MortgageReference { get; set; }

        [Input("project_mortgageamount")]
        public InArgument<string> Amount { get; set; }
        [Input("project_monthlypayment")]
        public InArgument<string> Payments { get; set; }



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

            Entity payment = new Entity("project_mortgagepayment");
            payment.Attributes.Add("project_name",DateTime.Now.AddMonths(0)+" Payment");
            payment.Attributes.Add("project_duedate", DateTime.Now.AddMonths(0));
            payment.Attributes.Add("project_payment",Payments);
            payment.Attributes.Add("project_paymentsid",MortgageReference);
            service.Create(payment);
            

            //output
            //FormattedDescription.Set(executionContext, d);
        }



    }
}
