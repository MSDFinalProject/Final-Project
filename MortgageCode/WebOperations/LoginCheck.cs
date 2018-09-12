using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Client;
using System.Net;
using System.IO;
using System.Threading;
using System.Collections;

namespace WebOperationsm
{
    static public class LoginCheck
    {
        public static string name;
        public static string id;
        public static string mId;

        static public string Exist(string user)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
             IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? (IOrganizationService)client.OrganizationServiceProxy;

            bool usernameComplete = false; 
            string p = "";
            string u = ""; 

            for (int i = 0; i < user.Length; i++)
            {
                if (user[i] ==',')
                {
                    usernameComplete = true;
                    continue; 
                }
                else
                {
                    if (usernameComplete == false)
                    {
                        u += user[i]; 
                    }
                    else
                    {
                        p += user[i]; 
                    }
                }
            } 

            string query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>  <entity name = 'contact' >
                            <attribute name='fullname' />
	                        <attribute name = 'parentcustomerid' />
                            <attribute name='telephone1' />
                            <attribute name = 'emailaddress1' />
                            <attribute name='contactid' />
                            <order attribute = 'fullname' descending='false' />
                            <filter type = 'and' >
                              <filter type='and'>
                                <condition attribute = 'project_usename' operator='eq' value='{u}' />
                                <condition attribute = 'project_password' operator='eq' value='{p}' />
                              </filter>
                            </filter>
                          </entity>
                        </fetch>";
            EntityCollection collection = service.RetrieveMultiple(new FetchExpression(query));

            if(collection.Entities.Count == 1 )
            {
                Entity c=collection.Entities.FirstOrDefault();
                name=c.Attributes["fullname"].ToString();
                id=c.Attributes["contactid"].ToString();
                return "true";
            }
            else
            {
                return "false";
            }

            
        }//GetU//Working

        public static void ContactPost(string data)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
            IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? (IOrganizationService)client.OrganizationServiceProxy;

            if (data != null)
            {
                string[] contact = new string[20];
                string contents = "";
                int x = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == ',')
                    {

                        contact[x] = contents;
                        contents = "";
                        x++;
                        continue;
                    }
                    else
                    {
                        contents += data[i];
                    }
                }
                Entity contactRecord = new Entity("contact");
                string u = data[0].ToString();
                contactRecord.Attributes.Add("emailaddress1",u);
                contactRecord.Attributes.Add("project_usename", u);
                contactRecord.Attributes.Add("project_password", data[1].ToString());
                //Both First and Last name
                contactRecord.Attributes.Add("fullname", data[2].ToString());
                //Both Address 1 and 2, city, state and zip
                contactRecord.Attributes.Add("address1_composite", data[3].ToString());
                contactRecord.Attributes.Add("project_socialsecuritynumber", data[4].ToString());
                service.Create(contactRecord);
            }
        }//PostU

        public static string MortgageGet()
        {
            Guid contactid = new Guid(id);
            string contactname = name;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
            IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? (IOrganizationService)client.OrganizationServiceProxy;

            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='project_mortgage'>
                                <attribute name='project_name' />
                                <attribute name='project_mortgageterm' />
                                <attribute name='project_mortgageamount' />
                                <attribute name='project_contactid' />
                                <attribute name='project_mortgageid' />
                                <attribute name='project_mortgagenumbera' />
                                <attribute name='project_monthlypayment' />
                                <order attribute='project_contactid' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";

            EntityCollection collections = service.RetrieveMultiple(new FetchExpression(xml));

            string mortgages = "";
            Money M;
            foreach (Entity mortgage in collections.Entities)
            {
                Console.WriteLine(name);
                
                EntityReference M_ContactId = (EntityReference)mortgage.Attributes["project_contactid"];
                string m_c_name = M_ContactId.Name;
                if (contactname.Equals(m_c_name))
                {
                    mortgages += mortgage.Attributes["project_mortgagenumbera"].ToString() + ",";
                    mortgages += mortgage.Attributes["project_name"].ToString() + ",";
                    mortgages += mortgage.Attributes["project_mortgageterm"].ToString() + ",";
                    M = (Money)mortgage.Attributes["project_mortgageamount"];
                    mortgages += M.Value + ",";
                    M = (Money)mortgage.Attributes["project_monthlypayment"];
                    mortgages += M.Value + ",;";
                }
            }
            return mortgages;
        }//GetM//Working

        public static void MortgagePost(string data)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
            IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? (IOrganizationService)client.OrganizationServiceProxy;

            if (data != null)
            {
                string[] mortgage = new string[20];
                string contents = "";
                int x = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == ',')
                    {
                        
                        mortgage[x]=contents;
                        contents = "";
                        x++;
                        continue;
                    }
                    else
                    {
                        contents += data[i];
                    }
                }

                Entity mortgageRecord = new Entity("project_mortgage");
                Money M = new Money(Convert.ToDecimal(mortgage[0].ToString()));
                mortgageRecord.Attributes.Add("project_mortgageamount", M);    //Currency
                mortgageRecord.Attributes.Add("project_mortgageterm", Convert.ToInt32(mortgage[1].ToString()));      //Whole Number
                if (mortgage[2].Equals("US"))
                {
                   mortgageRecord.Attributes.Add("project_region",false);
                }
                else
                {
                    mortgageRecord.Attributes.Add("project_region", true);
                }
                           //Two Option
                mortgageRecord.Attributes.Add("project_contactid",new  EntityReference("contact",new Guid(id)));
                Random n = new Random();
                mortgageRecord.Attributes.Add("project_name",name+"'S Mortgage "+ n.Next(0,99));
                try
                {
                    service.Create(mortgageRecord);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message+"-"+e.StackTrace+"]");
                }
            }//end if

        }//PostM

        public static string CaseGet()
        {
            Guid contactid = new Guid(id);
            string contactname = name;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
            IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? (IOrganizationService)client.OrganizationServiceProxy;

            string xml = @"<fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>
	                            <entity name='incident'>
	                            <attribute name='incidentid'/>
	                            <attribute name='ticketnumber'/>
	                            <attribute name='prioritycode'/>
	                            <attribute name='title'/>
	                            <attribute name='customerid'/>
	                            <attribute name='ownerid'/>
	                            <attribute name='statuscode'/>
	                            <attribute name='statecode'/>
	                            <attribute name='project_problemdescription'/>
	                            <attribute name='project_priority'/>
	                            <attribute name='project_highpriorityreason'/>
	                            <attribute name='description'/>
	                            <order descending='false' attribute='title'/>
	                            <filter type='and'>
		                            <condition attribute='statecode' value='0' operator='eq'/>
	                            </filter>
	                            </entity>
                            </fetch>";
            EntityCollection collections = service.RetrieveMultiple(new FetchExpression(xml));

            string cases = "";
            foreach (Entity incident in collections.Entities)
            {
                EntityReference M_ContactId = (EntityReference)incident.Attributes["customerid"];
                string m_c_name = M_ContactId.Name;
                if (contactname.Equals(m_c_name))
                {
                    cases += incident.Attributes["ticketnumber"].ToString() + ",";
                    cases += incident.Attributes["title"].ToString() + ",";
                    OptionSetValue op = new OptionSetValue();
                    op = (OptionSetValue)incident.Attributes["prioritycode"];
                    var opValue = op.Value;
                    if(opValue==1)
                        cases += "High" + ",";//OPTION SET
                    else if(opValue == 2)
                        cases += "Normal" + ",";//OPTION SET
                    else
                        cases += "Low" + ",";//OPTION SET
                    cases += incident.Attributes["description"].ToString() + ",";
                    op = (OptionSetValue)incident.Attributes["statuscode"];
                    opValue = op.Value;
                    if (opValue == 1)
                        cases += "Active" + ",";//OPTION SET
                    else
                        cases += "Inactive" + ",";//OPTION SET
                    cases += op + ";";
                    //cases += incident.Attributes["statecode"].ToString() + ";";
                }
            }

            return cases;
        }//GetC//Working

        public static void CasePost(string cases)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
            IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? (IOrganizationService)client.OrganizationServiceProxy;

            Guid contactid = new Guid(id);

            if (cases != null)
            {
                ArrayList data = new ArrayList();
                string contents = "";
                for (int i = 0; i < cases.Length; i++)
                {
                    if (cases[i] == ',')
                    {
                        data.Add(contents);
                        continue;
                    }
                    else
                    {
                        contents += cases[i];
                    }
                }
                Entity caseRecord = new Entity("incident");
                //if  1  no high priority
                if (data.IndexOf(0) == 1)
                {
                    caseRecord.Attributes.Add("title", data.IndexOf(1));
                    caseRecord.Attributes.Add("prioritycode", data.IndexOf(2));//optionset
                    caseRecord.Attributes.Add("description", data.IndexOf(3));
                    caseRecord.Attributes.Add("customerid", new EntityReference("contact", new Guid(id)));
                }
                else
                {
                    caseRecord.Attributes.Add("title", data.IndexOf(0));
                    caseRecord.Attributes.Add("prioritycode", new OptionSetValue(data.IndexOf(1)));//optionset
                    caseRecord.Attributes.Add("project_problemdescription", data.IndexOf(2));
                    caseRecord.Attributes.Add("description", data.IndexOf(3));
                    caseRecord.Attributes.Add("customerid", new EntityReference("contact", new Guid(id)));
                }
                service.Create(caseRecord);
                
            }//end if

        }//PostC

        public static string Paymentpay(string number)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
            IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? (IOrganizationService)client.OrganizationServiceProxy;

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
                                        <condition attribute = 'project_mortgagenumbera' value = '{number}' operator= 'eq' />
                                     </filter>
                                    </link-entity >
                                   </entity >
                                 </fetch > ";
            EntityCollection collections = service.RetrieveMultiple(new FetchExpression(xml));
            Entity pay=collections.Entities.FirstOrDefault();
            pay.Attributes["project_status"] = true;
            pay.Attributes["statuscode"] = new OptionSetValue(2);
            pay.Attributes["statecode"] = new OptionSetValue(1);
            service.Update(pay);
            return pay.Attributes["project_name"].ToString()+" ";
            //Entity payment =new Entity("project_mortgagepayment",)
        }


    }

}
