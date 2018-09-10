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

            
        }//GetU

        public static void ContactPost(string Contact)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
            IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? (IOrganizationService)client.OrganizationServiceProxy;

            if (Contact != null)
            {
                ArrayList data = new ArrayList();
                string contents = "";
                for (int i = 0; i < Contact.Length; i++)
                {
                    if (Contact[i] == ',')
                    {
                        data.Add(contents);
                        continue;
                    }
                    else
                    {
                        contents += Contact[i];
                    }
                }
                Entity contactRecord = new Entity("contact");
                contactRecord.Attributes.Add("emailaddress1", data.IndexOf(0));
                contactRecord.Attributes.Add("project_password", data.IndexOf(1));
                //Both First and Last name
                contactRecord.Attributes.Add("fullname", data.IndexOf(2));
                //Both Address 1 and 2, city, state and zip
                contactRecord.Attributes.Add("address1_composite", data.IndexOf(3));
                contactRecord.Attributes.Add("project_socialsecuritynumber", data.IndexOf(4));
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
                                <attribute name='createdon' />
                                <attribute name='project_mortgageamount' />
                                <attribute name='project_contactid' />
                                <attribute name='project_mortgageid' />
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
                EntityReference M_ContactId = (EntityReference)mortgage.Attributes["project_contactid"];
                string m_c_name = M_ContactId.Name;
                if (contactname.Equals(m_c_name))
                {
                   
                    mortgages += mortgage.Attributes["project_mortgagenumbera"].ToString() + ",";
                    mortgages += mortgage.Attributes["project_name"].ToString() + ",";
                    mortgages += mortgage.Attributes["project_mortgageterm"].ToString() + ",";
                    M = (Money)mortgage.Attributes["project_mortgageamount"];
                    mortgages += M.Value + ",";
                    M = (Money)mortgage.Attributes["project_mortgagepayment"];
                    mortgages += M.Value + ";";
                }
            }
            return mortgages;
        }//GetM

        public static void MortgagePost(string data)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
            IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? (IOrganizationService)client.OrganizationServiceProxy;

            if (data != null)
            {
                ArrayList mortgage = new ArrayList();
                string contents = "";
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == ',')
                    {
                        mortgage.Add(contents);
                        continue;
                    }
                    else
                    {
                        contents += data[i];
                    }
                }

                Entity mortgageRecord = new Entity("project_mortgage");
                mortgageRecord.Attributes.Add("project_mortgageamount", mortgage.IndexOf(0));    //Currency
                mortgageRecord.Attributes.Add("project_mortgageterm", mortgage.IndexOf(1));      //Whole Number
                mortgageRecord.Attributes.Add("project_region", mortgage.IndexOf(2));            //Two Option
                mortgageRecord.Attributes.Add("project_contactid",new  EntityReference("contact",new Guid(id)));
                service.Create(mortgageRecord);
            }//end if

        }//PostM

        public static string CaseGet()
        {
            Guid contactid = new Guid(id);
            string contactname = name;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient client = new CrmServiceClient("Url=https://finalproject.crm.dynamics.com; Username=Jrusso@finalproject.onmicrosoft.com; Password=Hpesoj93; authtype=Office365");
            IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? (IOrganizationService)client.OrganizationServiceProxy;

            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='project_mortgage'>
                                <attribute name='project_name' />
                                <attribute name='createdon' />
                                <attribute name='project_mortgageamount' />
                                <attribute name='project_contactid' />
                                <attribute name='project_mortgageid' />
                                <order attribute='project_contactid' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";
            EntityCollection collections = service.RetrieveMultiple(new FetchExpression(xml));

            string cases = "";
            foreach (Entity incident in collections.Entities)
            {
                EntityReference M_ContactId = (EntityReference)incident.Attributes["project_contactid"];
                string m_c_name = M_ContactId.Name;
                if (contactname.Equals(m_c_name))
                {
                    cases += incident.Attributes["ticketnumber"].ToString() + ",";
                    cases += incident.Attributes["title"].ToString() + ",";
                    cases += incident.Attributes["project_priority"].ToString() + ",";
                    cases += incident.Attributes["project_problemdescription"].ToString() + ",";
                    cases += incident.Attributes["statecode"].ToString() + ";";
                }
            }

            return cases;
        }//GetC

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
                    caseRecord.Attributes.Add("project_priority", data.IndexOf(2));//optionset
                    caseRecord.Attributes.Add("project_problemdescription", data.IndexOf(3));
                    //add high priority reason if therre 
                    caseRecord.Attributes.Add("primarycontactid", contactid);
                }

                //else 
                caseRecord.Attributes.Add("title", data.IndexOf(0));
                caseRecord.Attributes.Add("project_priority", data.IndexOf(1));//optionset
                caseRecord.Attributes.Add("project_problemdescription", data.IndexOf(2));
                //add high priority reason if therre 3
                caseRecord.Attributes.Add("primarycontactid", contactid);
                
                service.Create(caseRecord);
                
            }//end if

        }//PostC


    }

}
