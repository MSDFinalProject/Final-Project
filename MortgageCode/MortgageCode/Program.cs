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

namespace MortgageCode
{
    class Program
    {
        static void Main(string[] args)
        {
            CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
            IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient != null ? (IOrganizationService)client.OrganizationWebProxyClient : (IOrganizationService)client.OrganizationServiceProxy;



            string xml = $@"<fetch distinct='false' mapping='logical' output-format='xml - platform' version='1.0'>
                              <entity name = 'project_mortgagepayment' >
                                   <attribute name = 'project_mortgagepaymentid' />
                                   <attribute name = 'project_name' />
                                   <attribute name = 'project_status' />
                                   <attribute name = 'statuscode' />
                                   <order descending = 'false' attribute = 'project_name' />
                                   <filter type = 'and' >
                                    <condition attribute = 'statecode' value = '0' operator= 'eq' />
                                   </filter >
                                   <link-entity name = 'project_mortgage' alias = 'ac' link-type = 'inner' to = 'project_paymentsid' from = 'project_mortgageid' >
                                    <filter type = 'and'>
                                        <condition attribute = 'project_mortgagenumbera' value = '001011201809' operator= 'eq' />
                                     </filter>
                                    </link-entity >
                                   </entity >
                                 </fetch > ";
            EntityCollection collections = service.RetrieveMultiple(new FetchExpression(xml));
            Entity pay = collections.Entities.FirstOrDefault();
            //OptionSetValue state = new OptionSetValue();
            //OptionSetValue status = new OptionSetValue();
            //state.Value = 1; // inactive
            //status.Value = -1; // default status for the state
            pay.Attributes["project_status"] = true;
            pay.Attributes["statuscode"] = new OptionSetValue(2);
            pay.Attributes["statecode"] = new OptionSetValue(1);
            service.Update(pay);


            #region MortgageNumber
            //UpdateAttributeRequest widgetSerialNumberAttributeRequest = new UpdateAttributeRequest
            //{
            //    EntityName = "project_mortgage",
            //    Attribute = new StringAttributeMetadata
            //    {
            //        //Define the format of the attribute
            //        AutoNumberFormat = "{SEQNUM:6}{DATETIMEUTC:yyyyMM}",
            //        LogicalName = "project_mortgagenumbera",
            //        SchemaName = "project_mortgagenumberA",
            //        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
            //        MaxLength = 12, // The MaxLength defined for the string attribute must be greater than the length of the AutoNumberFormat value, that is, it should be able to fit in the generated value.
            //        DisplayName = new Label("Mortgage Number", 1033),
            //        Description = new Label("Mortgage Number of the Account.", 1033)
            //    }
            //};
            //service.Execute(widgetSerialNumberAttributeRequest);
            #endregion
            #region testworkflow
            //try
            //{
            //    //Create the tracing service
            //    WebRequest request = WebRequest.Create("http://mortgageproject.azurewebsites.net/api/risk");
            //    WebResponse response = request.GetResponse();
            //    StreamReader reader = new StreamReader(response.GetResponseStream());
            //    string responseString = reader.ReadToEnd();
            //    reader.Close();
            //    response.Close();
            //    decimal Risk=Convert.ToDecimal(responseString);

            //    Entity Mortgage = service.Retrieve("project_mortgage", new Guid("6584A212-5BB2-E811-A969-000D3A1CABCE"), new ColumnSet
            //   (
            //       "project_mortgageamount",
            //       "project_monthlypayment",
            //       "project_mortgageterm",
            //       "project_region",
            //       "project_contactid"
            //   ));

            //    //
            //    Money Total = (Money)Mortgage.Attributes["project_mortgageamount"];
            //    int term = (int)Mortgage.Attributes["project_mortgageterm"];
            //    decimal Base;
            //    decimal Margin;
            //    int credit = Convert.ToInt32(Risk);
            //    decimal Tax;

            //    #region GetApr
            //    // find the base apr
            //    QueryByAttribute query = new QueryByAttribute();
            //    query.EntityName = "project_configurations";
            //    query.ColumnSet = new ColumnSet(new string[] { "project_value" });
            //    query.AddAttributeValue("project_key", "APR");
            //    EntityCollection test = service.RetrieveMultiple(query);
            //    Entity apr = test.Entities.FirstOrDefault();
            //    Base = Convert.ToDecimal(apr.Attributes["project_value"]) / 100;
            //    #endregion
            //    #region getMargin
            //    // find the margin 
            //    QueryByAttribute query2 = new QueryByAttribute();
            //    query2.EntityName = "project_configurations";
            //    query2.ColumnSet = new ColumnSet(new string[] { "project_value" });
            //    query2.AddAttributeValue("project_key", "Margin");
            //    test = service.RetrieveMultiple(query2);
            //    Entity margin = test.Entities.FirstOrDefault();
            //    Margin = Convert.ToDecimal(margin.Attributes["project_value"]) / 100;
            //    #endregion

            //    EntityReference c = (EntityReference)Mortgage.Attributes["project_contactid"];
            //    Entity contact = service.Retrieve("contact", c.Id, new ColumnSet("address1_stateorprovince"));

            //    //get tax baes on state
            //   // Mortgage.Attributes["project_region"]
            //    if (Mortgage.GetAttribute‌​‌​Value<bool>("project_region") == true)
            //    {
            //        query = new QueryByAttribute();
            //        query.EntityName = "project_configurations";
            //        query.ColumnSet = new ColumnSet(new string[] { "project_value" });
            //        query.AddAttributeValue("project_key", "Canada");
            //        test = service.RetrieveMultiple(query);
            //        apr = test.Entities.FirstOrDefault();
            //        Tax = Convert.ToDecimal(margin.Attributes["project_value"]) / 100;

            //    }
            //    else
            //    {
            //        if (contact.Attributes["address1_stateorprovince"] != null)
            //        {
            //            query = new QueryByAttribute();
            //            query.EntityName = "project_taxe";
            //            query.ColumnSet = new ColumnSet(new string[] { "project_tax" });
            //            query.AddAttributeValue("project_state", contact.Attributes["address1_stateorprovince"]);
            //            test = service.RetrieveMultiple(query);
            //            Entity tax = test.Entities.FirstOrDefault();
            //            Tax = Convert.ToDecimal(tax.Attributes["project_tax"]) / 100;
            //        }
            //        else
            //            throw new Exception("No state provided");
            //    }

            //    decimal Interest = 12 * (term / 12);
            //    decimal APR = (Base + Margin) + ((decimal)Math.Log(credit,10)) + Tax;
            //    decimal Rate = APR / 12;
            //    double pay = Convert.ToDouble((Total.Value * Rate)) / ((1 - (Math.Pow((double)(1 + Rate), (double)(-Interest)))));
            //    Mortgage.Attributes["project_monthlypayment"] = Math.Round(pay)/12;






            //    Console.WriteLine("total:" + Total.Value);
            //    Console.WriteLine("Base:" + Base);
            //    Console.WriteLine("margin:" + Margin);
            //    Console.WriteLine("risk:" + credit);
            //    Console.WriteLine(" log Score:" + Math.Log(credit));
            //    Console.WriteLine("tax:" + Tax);
            //    Console.WriteLine("Final APR:" + APR);
            //    Console.WriteLine("rate:" + Rate);


            //    Console.WriteLine("interest:" + (-Interest));
            //    Console.WriteLine("power: "+ (Math.Pow((double)(1 + Rate), (double)(-Interest))));
            //    Console.WriteLine("half: " + ((1 - (Math.Pow((double)(1 + Rate), (double)(-Interest))))));
            //    Console.WriteLine(Math.Round(pay)/12);
            //    Console.Read();
            //}

            //catch (Exception ex)
            //{

            //    throw new Exception("Errors!" + ex.StackTrace +ex.Message,ex);
            //}
            #endregion
            //string user = "JoeRusso,Password1234";
            //bool usernameComplete = false;
            //string p = "";
            //string u="";

            //for (int i = 0; i < user.Length; i++)
            //{
            //    if (user[i] == ',')
            //    {
            //        usernameComplete = true;
            //        continue;
            //    }
            //    else
            //    {
            //        if (usernameComplete == false)
            //        {
            //            u += user[i];
            //        }
            //        else
            //        {
            //            p += user[i];
            //        }
            //    }
            //}
            //Console.WriteLine(u);
            //Console.WriteLine(p);
            //Console.Read();

            //string query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>  <entity name = 'contact' >
            //                <attribute name='fullname' />
            //             <attribute name = 'parentcustomerid' />
            //                <attribute name='telephone1' />
            //                <attribute name = 'emailaddress1' />
            //                <attribute name='contactid' />
            //                <order attribute = 'fullname' descending='false' />
            //                <filter type = 'and' >
            //                  <filter type='and'>
            //                    <condition attribute = 'project_usename' operator='eq' value='{u}' />
            //                    <condition attribute = 'project_password' operator='eq' value='{p}' />
            //                  </filter>
            //                </filter>
            //              </entity>
            //            </fetch>";
            //EntityCollection collection = service.RetrieveMultiple(new FetchExpression(query));
            //foreach (Entity a in collection.Entities)
            //{
            //    Console.WriteLine(collection.Entities.Count);
            //    Console.WriteLine(a.Attributes["fullname"].ToString());
            //    Console.WriteLine(a.Attributes["contactid"].ToString());
            //}
            //Console.Read();
        }
    }
}
