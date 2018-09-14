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
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace MortgageWorkflow
{
    public class RiskScore : CodeActivity
    {
        //[Input("project_mortgageamount")]
        //public InArgument<decimal> Amount { get; set; }
        //[Input("project_mortgageterm")]
        //public InArgument<int> Term { get; set; }
        //[Input("project_contactid")]
        //public InArgument<Guid> Contact { get; set; }
        //[Input("project_region")]
        //public InArgument<string> Region { get; set; }
        [Input("address1_stateorprovince")]
        public InArgument<string> State { get; set; }
        //[Input("project_monthlypayment")]
        //public InArgument<Money> MonthPayment { get; set; }
        //[Input("project_monthlypayment")]
        //public InArgument<Money> MonthPayment { get; set; }
        [RequiredArgument]
        [Input("Mortgage")]
        [ReferenceTarget("project_mortgage")]
        public InArgument<EntityReference> MortgageReference { get; set; }


        [Output("project_credit")]
        public OutArgument<decimal> Risk{ get; set; }
        [Output("project_monthlypayment")]
        public OutArgument<decimal> fixedPay { get; set; }

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
                decimal r;
                //Create the tracing service
                WebRequest request = WebRequest.Create("http://mortgageproject.azurewebsites.net/api/risk");
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                response.Close();
                r = Convert.ToDecimal(responseString);
                Risk.Set(executionContext, Convert.ToDecimal(responseString));

                Entity Mortgage = service.Retrieve("project_mortgage", MortgageReference.Get(executionContext).Id, new ColumnSet
               (
                   "project_mortgageamount",
                   "project_monthlypayment",
                   "project_mortgageterm",
                   "project_region",
                   "project_contactid"
               ));

                //
                Money Total = (Money)Mortgage.Attributes["project_mortgageamount"];
                int term = (int)Mortgage.Attributes["project_mortgageterm"];
                decimal Base;
                decimal Margin;
                int credit = Convert.ToInt32(r);
                decimal Tax;

                #region GetApr
                // find the base apr
                QueryByAttribute query = new QueryByAttribute();
                query.EntityName = "project_configurations";
                query.ColumnSet = new ColumnSet(new string[] { "project_value" });
                query.AddAttributeValue("project_key", "APR");
                EntityCollection test = service.RetrieveMultiple(query);
                Entity apr = test.Entities.FirstOrDefault();
                Base = Convert.ToDecimal(apr.Attributes["project_value"]) / 100;
                #endregion
                #region getMargin
                // find the margin 
                QueryByAttribute query2 = new QueryByAttribute();
                query2.EntityName = "project_configurations";
                query2.ColumnSet = new ColumnSet(new string[] { "project_value" });
                query2.AddAttributeValue("project_key", "Margin");
                test = service.RetrieveMultiple(query2);
                Entity margin = test.Entities.FirstOrDefault();
                Margin = Convert.ToDecimal(margin.Attributes["project_value"]) / 100;
                #endregion

                EntityReference c = (EntityReference)Mortgage.Attributes["project_contactid"];
                Entity contact = service.Retrieve("contact", c.Id, new ColumnSet("address1_stateorprovince"));

                //get tax baes on state
                // Mortgage.Attributes["project_region"]
                if (Mortgage.GetAttribute‌​‌​Value<bool>("project_region") == true)
                {
                    query = new QueryByAttribute();
                    query.EntityName = "project_configurations";
                    query.ColumnSet = new ColumnSet(new string[] { "project_value" });
                    query.AddAttributeValue("project_key", "Canada");
                    test = service.RetrieveMultiple(query);
                    apr = test.Entities.FirstOrDefault();
                    Tax = Convert.ToDecimal(margin.Attributes["project_value"]) / 100;

                }
                else
                {
                    if (contact.Attributes["address1_stateorprovince"] != null)
                    {
                        query = new QueryByAttribute();
                        query.EntityName = "project_taxe";
                        query.ColumnSet = new ColumnSet(new string[] { "project_tax" });
                        query.AddAttributeValue("project_state", contact.Attributes["address1_stateorprovince"]);
                        test = service.RetrieveMultiple(query);
                        Entity tax = test.Entities.FirstOrDefault();
                        Tax = Convert.ToDecimal(tax.Attributes["project_tax"]) / 100;
                    }
                    else
                        throw new Exception("No state provided");
                }

                decimal Interest = 12 * (term / 12);
                decimal APR = (Base + Margin) + ((decimal)Math.Log(credit, 10)) + Tax;
                decimal Rate = APR / 12;
                double pay = Convert.ToDouble((Total.Value * Rate)) / ((1 - (Math.Pow((double)(1 + Rate), (double)(-Interest)))));
                Mortgage.Attributes["project_monthlypayment"] =Convert.ToDecimal( (Math.Round(pay) / 12));
                fixedPay.Set(executionContext,Convert.ToDecimal((Math.Round(pay) / 12))); 
                //Console.WriteLine("total:" + Total.Value);
                //Console.WriteLine("Base:" + Base);
                //Console.WriteLine("margin:" + Margin);
                //Console.WriteLine("risk:" + credit);
                //Console.WriteLine(" log Score:" + Math.Log(credit));
                //Console.WriteLine("tax:" + Tax);
                //Console.WriteLine("Final APR:" + APR);
                //Console.WriteLine("rate:" + Rate);
                //Console.WriteLine("interest:" + (-Interest));
                //Console.WriteLine("power: " + (Math.Pow((double)(1 + Rate), (double)(-Interest))));
                //Console.WriteLine("half: " + ((1 - (Math.Pow((double)(1 + Rate), (double)(-Interest))))));
                //Console.WriteLine(Math.Round(pay) / 12);
                //Console.Read();
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in RiskScore Workflow!." + ex.StackTrace + " "+ex.Message+" "+ex.Source , ex);
            }

            catch (Exception ex)
            {
                tracingService.Trace("RiskScore  + Creation: {1}"+ex.StackTrace+"--"+ ex.Message+"--"+ex.Source+"---", ex.ToString());
                throw;
            }
            
        }//edn execute
    }
}
