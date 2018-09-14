using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace MortgageWorkflow
{
    public class Payments: CodeActivity
    {
        //[RequiredArgument]
        //[Input("Mortgage")]
        //[ReferenceTarget("project_mortgage")]
        //public InArgument<EntityReference> MortgageReference { get; set; }
        
        [Input("project_mortgageterm")]
        public InArgument<int> Term { get; set; }
        [Input("project_monthlypayment")]
        public InArgument<decimal> MonthPayment { get; set; }


        //[Output("")]
        //public OutArgument<string> apayment { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            //Read input from Argument
            Entity apayment;
            try { 
            for (int m=0; m<Term.Get<int>(executionContext);m++)
            {
                apayment = new Entity("project_mortgagepayment");
                apayment.Attributes.Add("project_name", $"Payment Number{m+1} {DateTime.Now.AddMonths(m).ToString("MM/yyyy")} ");
                apayment.Attributes.Add("project_duedate", DateTime.Now.AddMonths(m));
                apayment.Attributes.Add("project_payment", MonthPayment.Get<decimal>(executionContext));
                apayment.Attributes.Add("project_paymentsid", new EntityReference(context.PrimaryEntityName,context.PrimaryEntityId));
                service.Create(apayment);
            }
          }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in Payment Creation Workflow." + ex.Message + "- " + ex.StackTrace, ex);
            }

            catch (Exception ex)
            {
                tracingService.Trace("Payment Creation: {0}"+ex.Message+"- "+ex.StackTrace, ex.ToString());
                throw;
            }
            //output
            //FormattedDescription.Set(executionContext, d);
        }



    }
}
