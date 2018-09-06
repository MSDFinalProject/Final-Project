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
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MotgagePlugins
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
                Money MonthlyPayment=new Money();
                try
                {
                    //WebRequest request = WebRequest.Create("http://localhost:64288/api/risk");
                    //WebResponse response = request.GetResponse();
                    //StreamReader reader = new StreamReader(response.GetResponseStream());
                    //string responseString = reader.ReadToEnd();
                    //reader.Close();
                    //response.Close();
                    //decimal RiskScore = Convert.ToDecimal(responseString);


                    Money Total = (Money)Mortgage.Attributes["project_monthlyamount"];
                    int term= (int)Mortgage.Attributes["project_mortgageterm"];
                    decimal Base;
                    decimal Margin;
                    int Risk=(int)Mortgage.Attributes["project_credit"];
                    decimal Tax;
                    
                    // find the base apr
                    QueryByAttribute query = new QueryByAttribute();
                    query.EntityName = "project_configurations";
                    query.ColumnSet = new ColumnSet(new string[] { "project_value" });
                    query.AddAttributeValue("project_key", "APR");
                    EntityCollection test = service.RetrieveMultiple(query);
                    Entity apr = test.Entities.FirstOrDefault();
                    Base = Convert.ToDecimal(apr.Attributes["project_value"])/100;

                    // find the margin 
                    QueryByAttribute query2 = new QueryByAttribute();
                    query2.EntityName = "project_configurations";
                    query2.ColumnSet = new ColumnSet(new string[] { "project_value" });
                    query2.AddAttributeValue("project_key", "Margin");
                    test = service.RetrieveMultiple(query2);
                    Entity margin = test.Entities.FirstOrDefault();
                    Margin = Convert.ToDecimal(margin.Attributes["project_value"]) / 100;


                  Entity contact = service.Retrieve("contact", (Guid)Mortgage.Attributes["project_contactid"], new ColumnSet("name","address1_stateorprovidence"));
                    //get tax baes on state
                    if (Mortgage.Attributes["project_region"].ToString()=="canada")
                    {
                        query = new QueryByAttribute();
                        query.EntityName = "project_configurations";
                        query.ColumnSet = new ColumnSet(new string[] { "project_value" });
                        query.AddAttributeValue("project_key", "Canada");
                        test = service.RetrieveMultiple(query);
                        apr= test.Entities.FirstOrDefault();
                        Tax = Convert.ToDecimal(margin.Attributes["project_value"]) / 100;
                        
                    }
                    else
                    {
                        query = new QueryByAttribute();
                        query.EntityName = "project_taxe";
                        query.ColumnSet = new ColumnSet(new string[] { "project_tax" });
                        query.AddAttributeValue("project_state", contact.Attributes["address1_cityorprovidence"]);
                        test = service.RetrieveMultiple(query);
                        Entity tax = test.Entities.FirstOrDefault();
                        Tax = Convert.ToDecimal(tax.Attributes["project_Tax"]) / 100;
                    }

                    decimal Interest = 12 * (term / 12);
                    decimal APR = (Base+Margin) + (decimal)Math.Log(Risk) + Tax;
                    decimal Rate = APR / 12;
                    decimal pay = (Total.Value * Rate) / (decimal)(1 - (Math.Pow((double)(1 + Rate), (double)(-Interest))));
                    MonthlyPayment.Value=pay;
                    Console.WriteLine(pay);
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
