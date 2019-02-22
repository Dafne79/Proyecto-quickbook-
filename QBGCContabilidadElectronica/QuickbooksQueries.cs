using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Interop.QBXMLRP2;
using System.Diagnostics;
namespace QB_App
{
    public class QuickbooksQueries : QuickbooksProcessor
    {
        // Indexed by RFC
        public Dictionary<string, Vendor> Vendors;
        public Dictionary<string, Client> Clients;

        // Indexed by ListId and Parent
        public Dictionary<string, Account> Accounts;
        public Dictionary<string, Bill> Bills;
        public Dictionary<string, Item> Items;
        public Dictionary<string, UnitMeasure> Uoms;
        public Dictionary<string, Currency> Currencies;
        public Dictionary<string, Tax> Taxes;
        //
        public Dictionary<string, string> TrialListRefs;
        public Dictionary<string, string> AccountTrialsRefs;
        public enum ReportType { TRIALS, BALANCE, PROFIT_LOSS }
        public Company company;


        public string LoadTransactions2()
        {

            //Taxes = new Dictionary<string, Tax>();
            string result = "";
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("TransactionQueryRQ");
            
            /***************************************************/

            XmlElement dateElment = inputXMLDoc.CreateElement("TransactionModificatedDateRangeFilter");
            qbXMLMsgsRq.AppendChild(dateElment);

            XmlElement fromDate = inputXMLDoc.CreateElement("FromModifiedDate");
            fromDate.InnerText = "2019-01-01";
            dateElment.AppendChild(fromDate);

            XmlElement toDate = inputXMLDoc.CreateElement("ToModifiedDate");
            toDate.InnerText = "2019-02-20";
            dateElment.AppendChild(toDate);
            QueryRq.AppendChild(dateElment);
            qbXMLMsgsRq.AppendChild(QueryRq);
            /**************************************************/
            string qbResponse;
            QueryQB(inputXMLDoc.OuterXml, out qbResponse);
            QuicbooksResponse qbResultQuery = new QuicbooksResponse(qbResponse, "SalesTaxCodeQueryRs");
            if (qbResultQuery.GetNumberOfResulst() > 0)
            {
                XmlNodeList responseNodeList = qbResultQuery.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode responseNode in responseNodeList)
                {
                    Tax tax = new Tax();
                    if (responseNode["ListID"] != null)
                        tax.listId = responseNode["ListID"].InnerText;
                    if (responseNode["Name"] != null)
                        tax.name = responseNode["Name"].InnerText;

                    if (!String.IsNullOrEmpty(tax.listId))
                    {
                        Taxes[tax.name] = tax;
                    }
                }
            }
            return result;

        }


        public string LoadTaxes()
        {

            Taxes = new Dictionary<string, Tax>();
            string result = "";
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("SalesTaxCodeQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);

            string qbResponse;
            QueryQB(inputXMLDoc.OuterXml, out qbResponse);
            QuicbooksResponse qbResultQuery = new QuicbooksResponse(qbResponse, "SalesTaxCodeQueryRs");
            if (qbResultQuery.GetNumberOfResulst() > 0)
            {
                XmlNodeList responseNodeList = qbResultQuery.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode responseNode in responseNodeList)
                {
                    Tax tax = new Tax();
                    if (responseNode["ListID"] != null)
                        tax.listId = responseNode["ListID"].InnerText;
                    if (responseNode["Name"] != null)
                        tax.name = responseNode["Name"].InnerText;

                    if (!String.IsNullOrEmpty(tax.listId))
                    {
                        Taxes[tax.name] = tax;
                    }
                }
            }
            return result;

        }

        public string LoadCurrencies() {

            Currencies = new Dictionary<string, Currency>();
            string result = "";
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("CurrencyQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);

            string qbResponse;
            QueryQB(inputXMLDoc.OuterXml, out qbResponse);
            QuicbooksResponse qbResultQuery = new QuicbooksResponse(qbResponse, "CurrencyQueryRs");
            if (qbResultQuery.GetNumberOfResulst() > 0)
            {
                XmlNodeList responseNodeList = qbResultQuery.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode responseNode in responseNodeList)
                {
                    Currency currency = new Currency();
                    if (responseNode["ListID"] != null)
                        currency.listId = responseNode["ListID"].InnerText;
                    if (responseNode["Name"] != null)
                        currency.name = responseNode["Name"].InnerText;

                    if (!String.IsNullOrEmpty(currency.listId))
                    {
                        Currencies[currency.name] = currency;
                    }
                }
            }
            return result;

        }


        public string BuildUomsQuery()
        {
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("UnitOfMeasureSetQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);
            /*
            if (!String.IsNullOrWhiteSpace(listId))
            {
                XmlElement listIdField = inputXMLDoc.CreateElement("ListID");
                listIdField.InnerText = listId;
                QueryRq.AppendChild(listIdField);
            }

            XmlElement maxReturnedField = inputXMLDoc.CreateElement("MaxReturned");
            maxReturnedField.InnerText = "100";
            QueryRq.AppendChild(maxReturnedField);

            XmlElement ownerIdField = inputXMLDoc.CreateElement("OwnerID");
            ownerIdField.InnerText = "0";
            QueryRq.AppendChild(ownerIdField);
            */
            return inputXMLDoc.OuterXml;
        }


        public string LoadUoms()
        {
            Uoms = new Dictionary<string, UnitMeasure>();
            string result = "";

            string qbResponse;
            QueryQB(BuildUomsQuery(), out qbResponse);
            QuicbooksResponse qbResultQuery = new QuicbooksResponse(qbResponse, "UnitOfMeasureSetQueryRs");
            if (qbResultQuery.GetNumberOfResulst() > 0)
            {
                XmlNodeList responseNodeList = qbResultQuery.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode responseNode in responseNodeList)
                {
                    UnitMeasure uom = new UnitMeasure();
                    if (responseNode["ListID"] != null)
                        uom.listId = responseNode["ListID"].InnerText;
                    if (responseNode["Name"] != null)
                        uom.name = responseNode["Name"].InnerText;

                    if (responseNode["BaseUnit"] != null)
                    {
                        foreach (XmlNode DataRowNode in responseNode["BaseUnit"])
                        {

                            if (DataRowNode.Name == "Name")
                                uom.baseUnitName = DataRowNode.InnerText;
                            if (DataRowNode.Name == "Abbreviation")
                                uom.abbreviation = DataRowNode.InnerText;
                        }

                    }


                    if (!String.IsNullOrEmpty(uom.abbreviation))
                    {
                        Uoms[uom.abbreviation] = uom;
                    }
                }
            }
            return result;
        }


        public string BuildItemsQuery(string listId = "")
        {
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("ItemQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);

            if (!String.IsNullOrWhiteSpace(listId))
            {
                XmlElement listIdField = inputXMLDoc.CreateElement("ListID");
                listIdField.InnerText = listId;
                QueryRq.AppendChild(listIdField);
            }

            //XmlElement maxReturnedField = inputXMLDoc.CreateElement("MaxReturned");
            //maxReturnedField.InnerText = "100";
            //QueryRq.AppendChild(maxReturnedField);

            XmlElement ownerIdField = inputXMLDoc.CreateElement("OwnerID");
            ownerIdField.InnerText = "0";
            QueryRq.AppendChild(ownerIdField);

            return inputXMLDoc.OuterXml;
        }


        public string LoadItems()
        {
            Items = new Dictionary<string, Item>();
            string result = "";

            string qbResponse;
            QueryQB(BuildItemsQuery(), out qbResponse);
            QuicbooksResponse qbResultQuery = new QuicbooksResponse(qbResponse, "ItemQueryRs");
            if (qbResultQuery.GetNumberOfResulst() > 0)
            {
                XmlNodeList responseNodeList = qbResultQuery.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode responseNode in responseNodeList)
                {
                    Item item = new Item();
                    if (responseNode["ListID"] != null)
                        item.listId = responseNode["ListID"].InnerText;
                    if (responseNode["Name"] != null)
                        item.name = responseNode["Name"].InnerText;
                    if (responseNode["FullName"] != null)
                        item.fullName = responseNode["FullName"].InnerText;

                    //if (responseNode["ManufacturerPartNumber"] != null)
                    //    item.productKey = responseNode["ManufacturerPartNumber"].InnerText;


                    //m.Diagnostics.Trace.WriteLine(responseNode.Name);
                    //System.Diagnostics.Trace.WriteLine(responseNode.Name);
                    if (responseNode["DataExtRet"] != null)
                    {
                        bool getVal = false;
                        foreach (XmlNode DataRowNode in responseNode["DataExtRet"])
                        {
                            //System.Diagnostics.Trace.WriteLine(DataRowNode.Name == "DataExtName");
                            //System.Diagnostics.Trace.WriteLine(DataRowNode.InnerText);
                            if (DataRowNode.Name == "DataExtName" && DataRowNode.InnerText == "CLAVEPROD")
                                getVal = true;

                            if (getVal && DataRowNode.Name == "DataExtValue")
                                item.productKey = DataRowNode.InnerText;
                        }
                    }


                    if (responseNode["UnitOfMeasureSetRef"] != null)
                    {
                        foreach (XmlNode DataRowNode in responseNode["UnitOfMeasureSetRef"])
                        {
                            if (DataRowNode["ListID"] != null)
                                item.uomListId = DataRowNode["ListID"].InnerText;
                            if (DataRowNode["FullName"] != null)
                                item.uomFullName = DataRowNode["FullName"].InnerText;
                        }
                    }

                    if (!String.IsNullOrEmpty(item.productKey))
                    {
                        Items[item.productKey] = item;
                    }
                }
            }
            return result;
        }


        public string LoadItemCustomFields()
        {
            string customFieldName = "CLAVEPROD";
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("DataExtDefQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);


            XmlElement assignToObject = inputXMLDoc.CreateElement("AssignToObject");
            assignToObject.InnerText = "Item";
            QueryRq.AppendChild(assignToObject);

            bool hasKeyProdField = false;
            string result = "";
            string qbResponse;
            QueryQB(inputXMLDoc.OuterXml, out qbResponse);
            QuicbooksResponse qbResultQuery = new QuicbooksResponse(qbResponse, "DataExtDefQueryRs");
            if (qbResultQuery.GetNumberOfResulst() > 0)
            {
                XmlNodeList responseNodeList = qbResultQuery.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode responseNode in responseNodeList)
                {
                    //System.Diagnostics.Trace.WriteLine(responseNode.Name);
                    foreach (XmlNode responseNodeChild in responseNode)
                    {
                        //System.Diagnostics.Trace.WriteLine(responseNodeChild.Name);
                        //System.Diagnostics.Trace.WriteLine(responseNodeChild.InnerText);
                        if (responseNodeChild.Name == "DataExtName" && responseNodeChild.InnerText == customFieldName)
                        {
                            hasKeyProdField = true;
                        }
                    }

                }
            }

            //Creates field is doesnt exists
            if (!hasKeyProdField)
            {
                System.Diagnostics.Trace.WriteLine("Creando " + customFieldName);
                XmlDocument inputXMLDoc2;
                XmlElement qbXMLMsgsRq2;
                QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc2, out qbXMLMsgsRq2);

                // Mark as query type
                XmlElement addRq = inputXMLDoc2.CreateElement("DataExtDefAddRq");
                qbXMLMsgsRq2.AppendChild(addRq);

                XmlElement defAdd = inputXMLDoc2.CreateElement("DataExtDefAdd");
                addRq.AppendChild(defAdd);

                XmlElement ownerID = inputXMLDoc2.CreateElement("OwnerID");
                ownerID.InnerText = "0";
                defAdd.AppendChild(ownerID);

                XmlElement dataName = inputXMLDoc2.CreateElement("DataExtName");
                dataName.InnerText = customFieldName;
                defAdd.AppendChild(dataName);

                XmlElement dataType = inputXMLDoc2.CreateElement("DataExtType");
                dataType.InnerText = "STR255TYPE";
                defAdd.AppendChild(dataType);

                XmlElement assignToObj = inputXMLDoc2.CreateElement("AssignToObject");
                assignToObj.InnerText = "Item";
                defAdd.AppendChild(assignToObj);

                string qbResponse2;
                QueryQB(inputXMLDoc2.OuterXml, out qbResponse2);
                QuicbooksResponse qbResultQuery2 = new QuicbooksResponse(qbResponse2, "DataExtDefAddRs");
                if (qbResultQuery2.GetNumberOfResulst() > 0)
                {
                   if (qbResultQuery2.success)
                    {
                        System.Diagnostics.Trace.WriteLine("CReated custom field " + customFieldName);
                    }
                }


            }


            return result;

        }


        public string BuildBillsQuery(string date)
        {
            string FromDate = date;
            string ToDate   = date;

            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("BillQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);

            XmlElement ModifiedDateRangeFilter = inputXMLDoc.CreateElement("ModifiedDateRangeFilter");

            XmlElement FromDateField = inputXMLDoc.CreateElement("FromModifiedDate");
            FromDateField.InnerText = FromDate;
            ModifiedDateRangeFilter.AppendChild(FromDateField);

            XmlElement ToDateField = inputXMLDoc.CreateElement("ToModifiedDate");
            ToDateField.InnerText = ToDate;
            ModifiedDateRangeFilter.AppendChild(ToDateField);

            return inputXMLDoc.OuterXml;
        }


        public string BuildInvoiceQuery(string date)
        {
            string FromDate = date;
            string ToDate = date;

            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("InvoiceQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);

            XmlElement ModifiedDateRangeFilter = inputXMLDoc.CreateElement("ModifiedDateRangeFilter");

            XmlElement FromDateField = inputXMLDoc.CreateElement("FromModifiedDate");
            FromDateField.InnerText = FromDate;
            ModifiedDateRangeFilter.AppendChild(FromDateField);

            XmlElement ToDateField = inputXMLDoc.CreateElement("ToModifiedDate");
            ToDateField.InnerText = ToDate;
            ModifiedDateRangeFilter.AppendChild(ToDateField);

            return inputXMLDoc.OuterXml;
        }

        public bool QueryExistingBill(string date, string SATuuid)
        {
            Bills = new Dictionary<string, Bill>();
            bool result = false;

            string responseBills;
            QueryQB(BuildBillsQuery(date), out responseBills);
            QuicbooksResponse qbResultBillsQuery = new QuicbooksResponse(responseBills, "BillQueryRs");

            if (qbResultBillsQuery.GetNumberOfResulst() > 0)
            {
                XmlNodeList BillsNodeList = qbResultBillsQuery.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode BillNode in BillsNodeList)
                {

                    if (BillNode["Memo"] != null && BillNode["Memo"].InnerText == SATuuid)
                    {
                        result = true;
                        string msg = "La factura con UUID " + SATuuid + " ya existe!";
                        break;
                    }

                }
            }

            return result;

        }


        public bool QueryExistingInvoice(string date, string SATuuid)
        {
            Bills = new Dictionary<string, Bill>();
            bool result = false;

            string responseBills;
            QueryQB(BuildInvoiceQuery(date), out responseBills);
            QuicbooksResponse qbResultBillsQuery = new QuicbooksResponse(responseBills, "InvoiceQueryRs");

            if (qbResultBillsQuery.GetNumberOfResulst() > 0)
            {
                XmlNodeList BillsNodeList = qbResultBillsQuery.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode BillNode in BillsNodeList)
                {

                    if (BillNode["Memo"] != null && BillNode["Memo"].InnerText == SATuuid)
                    {
                        result = true;
                        string msg = "La factura con UUID " + SATuuid + " ya existe!";
                        break;
                    }

                }
            }

            return result;

        }

        public string BuildCompanyQuery()
        {
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("CompanyQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);

            return inputXMLDoc.OuterXml;
        }

        public string LoadCompany()
        {
            company = new Company();
            string result = "";

            string responseCompany;
            QueryQB(BuildCompanyQuery(), out responseCompany);
            QuicbooksResponse qbResultQueryCompany = new QuicbooksResponse(responseCompany, "CompanyQueryRs");
            if (qbResultQueryCompany.GetNumberOfResulst() > 0)
            {
                XmlNodeList AccountsNodeList = qbResultQueryCompany.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode CompanyNode in AccountsNodeList)
                {
                    if (CompanyNode["Fax"] != null)
                        company.rfc = CompanyNode["Fax"].InnerText;
                    result = company.rfc;
                }
            }
            return result;
        }

        public string BuildAccountsQuery()
        {
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("AccountQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);

            // Query for all accounts
            XmlElement AllAccounts = inputXMLDoc.CreateElement("ActiveStatus");
            AllAccounts.InnerText = "All";
            QueryRq.AppendChild(AllAccounts);

            return inputXMLDoc.OuterXml;
        }

        public string LoadAccounts()
        {
            Accounts = new Dictionary<string, Account>();
            string result = "";

            string responseAccount;
            QueryQB(BuildAccountsQuery(), out responseAccount);
            QuicbooksResponse qbResultQueryAccount = new QuicbooksResponse(responseAccount, "AccountQueryRs");
            if (qbResultQueryAccount.GetNumberOfResulst() > 0)
            {
                XmlNodeList AccountsNodeList = qbResultQueryAccount.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode AccountNode in AccountsNodeList)
                {
                    string listId = "", parentListId = "", accountType = "";
                    CatalogoCtas cuenta = new CatalogoCtas();
                    if (AccountNode["ListID"] != null)
                        listId = AccountNode["ListID"].InnerText;
                    if (AccountNode["ParentRef"] != null && AccountNode["ParentRef"]["ListID"] != null)
                        parentListId = AccountNode["ParentRef"]["ListID"].InnerText;

                    if (AccountNode["Name"] != null)
                        cuenta.Desc = AccountNode["Name"].InnerText;
                    if (AccountNode["AccountType"] != null)
                        accountType = AccountNode["AccountType"].InnerText;
                    if (AccountNode["AccountNumber"] != null)
                        cuenta.NumCta = AccountNode["AccountNumber"].InnerText;
                    if (AccountNode["Sublevel"] != null)
                    {
                        int level = 0;
                        Int32.TryParse(AccountNode["Sublevel"].InnerText, out level);
                        cuenta.Nivel = level + 1;
                    }
                    else
                        cuenta.Nivel = 1;
                    if (AccountNode["Desc"] != null)
                    {
                        using (System.IO.StringReader reader = new System.IO.StringReader(AccountNode["Desc"].InnerText))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                Regex regExpressionCodAgrupador = new Regex(@"^CA:(.*)", RegexOptions.IgnoreCase);
                                Match groupCodeMatch = regExpressionCodAgrupador.Match(line);
                                if (groupCodeMatch.Success)
                                    cuenta.CodAgrup = groupCodeMatch.Groups[1].Value;
                            }
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(cuenta.NumCta))
                    {
                        if (!Accounts.ContainsKey(cuenta.Desc))
                        {
                            Account account = new Account();
                            account.satAccount = cuenta;
                            account.listId = listId;
                            account.parentListId = parentListId;
                            account.type = accountType;
                            cuenta.Natur = account.GetNature();
                            Accounts.Add(cuenta.NumCta + " · " + cuenta.Desc, account);
                        }
                        else
                            result += "Cuenta duplicada: " + cuenta.NumCta + " · " + cuenta.Desc + "\r\n";
                    }
                    else if (cuenta.Desc == "CLASIFICACION PENDIENTE")
                    {
                        // Cuenta especial para uso del importador de bills
                        Account account = new Account();
                        account.listId = listId;
                        account.parentListId = parentListId;
                        account.satAccount = new CatalogoCtas();
                        account.satAccount.NumCta = cuenta.Desc;
                        Accounts.Add(cuenta.Desc, account);
                    }
                }
            }
            // Map child accounts to parent
            foreach (KeyValuePair<string, Account> accountPair in Accounts)
            {
                if (!String.IsNullOrWhiteSpace(accountPair.Value.parentListId))
                {
                    // Look for the parent by name
                    foreach (KeyValuePair<string, Account> accountPairLookup in Accounts)
                    {
                        if (accountPairLookup.Value.listId == accountPair.Value.parentListId)
                            accountPair.Value.satAccount.SubCtaDe = Accounts[accountPairLookup.Value.satAccount.NumCta + " · " + accountPairLookup.Value.satAccount.Desc].satAccount.NumCta;
                    }
                }
            }
            return result;
        }

        public string BuildVendorsQuery()
        {
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("VendorQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);

            // Query for all accounts
            XmlElement AllAccounts = inputXMLDoc.CreateElement("ActiveStatus");
            AllAccounts.InnerText = "All";
            QueryRq.AppendChild(AllAccounts);

            // Grab only some fields
            XmlElement ListId = inputXMLDoc.CreateElement("IncludeRetElement");
            ListId.InnerText = "ListID";
            QueryRq.AppendChild(ListId);
            XmlElement NameField = inputXMLDoc.CreateElement("IncludeRetElement");
            NameField.InnerText = "Name";
            QueryRq.AppendChild(NameField);
            XmlElement RFCField = inputXMLDoc.CreateElement("IncludeRetElement");
            RFCField.InnerText = "AccountNumber";
            QueryRq.AppendChild(RFCField);
            XmlElement AccountsField = inputXMLDoc.CreateElement("IncludeRetElement");
            AccountsField.InnerText = "PrefillAccountRef";
            QueryRq.AppendChild(AccountsField);

            return inputXMLDoc.OuterXml;
        }

        public string LoadVendors()
        {
            Vendors = new Dictionary<string, Vendor>();
            string result = "";

            string responseAccount;
            QueryQB(BuildVendorsQuery(), out responseAccount);
            QuicbooksResponse qbResultQueryAccount = new QuicbooksResponse(responseAccount, "VendorQueryRs");
            if (qbResultQueryAccount.GetNumberOfResulst() > 0)
            {
                XmlNodeList VendorsNodeList = qbResultQueryAccount.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode VendorNode in VendorsNodeList)
                {
                    Vendor vendor = new Vendor();
                    vendor.SetDefaults();
                    if (VendorNode["ListID"] != null)
                        vendor.listId = VendorNode["ListID"].InnerText;
                    if (VendorNode["Name"] != null)
                        vendor.nombre = VendorNode["Name"].InnerText;
                    if (VendorNode["AccountNumber"] != null)
                        vendor.rfc = VendorNode["AccountNumber"].InnerText;
                    if (VendorNode["PrefillAccountRef"] != null && VendorNode["PrefillAccountRef"].ChildNodes.Count > 0)
                        vendor.accountListId = VendorNode["PrefillAccountRef"].ChildNodes.Item(0).InnerText;

                    if (!String.IsNullOrEmpty(vendor.rfc))
                    {
                        Vendors[vendor.rfc] = vendor;
                    }
                }
            }
            return result;
        }

        public string BuildCustomersQuery()
        {
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("CustomerQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);

            // Query for all accounts
            XmlElement AllAccounts = inputXMLDoc.CreateElement("ActiveStatus");
            AllAccounts.InnerText = "All";
            QueryRq.AppendChild(AllAccounts);

            // Grab only some fields
            XmlElement ListId = inputXMLDoc.CreateElement("IncludeRetElement");
            ListId.InnerText = "ListID";
            QueryRq.AppendChild(ListId);
            XmlElement NameField = inputXMLDoc.CreateElement("IncludeRetElement");
            NameField.InnerText = "Name";
            QueryRq.AppendChild(NameField);
            XmlElement RFCField = inputXMLDoc.CreateElement("IncludeRetElement");
            RFCField.InnerText = "AccountNumber";
            QueryRq.AppendChild(RFCField);
            XmlElement AccountsField = inputXMLDoc.CreateElement("IncludeRetElement");
            AccountsField.InnerText = "PrefillAccountRef";
            QueryRq.AppendChild(AccountsField);

            return inputXMLDoc.OuterXml;
        }

        public string LoadCustomers()
        {
            Clients = new Dictionary<string, Client>();
            string result = "";

            string responseAccount;
            QueryQB(BuildCustomersQuery(), out responseAccount);
            QuicbooksResponse qbResultQueryAccount = new QuicbooksResponse(responseAccount, "CustomerQueryRs");
            if (qbResultQueryAccount.GetNumberOfResulst() > 0)
            {
                XmlNodeList VendorsNodeList = qbResultQueryAccount.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode VendorNode in VendorsNodeList)
                {
                    Client client = new Client();
                    client.SetDefaults();
                    if (VendorNode["ListID"] != null)
                        client.listId = VendorNode["ListID"].InnerText;
                    if (VendorNode["Name"] != null)
                        client.nombre = VendorNode["Name"].InnerText;
                    if (VendorNode["AccountNumber"] != null)
                        client.rfc = VendorNode["AccountNumber"].InnerText;
                    if (VendorNode["PrefillAccountRef"] != null && VendorNode["PrefillAccountRef"].ChildNodes.Count > 0)
                        client.accountListId = VendorNode["PrefillAccountRef"].ChildNodes.Item(0).InnerText;

                    if (!String.IsNullOrEmpty(client.rfc))
                    {
                        Clients[client.rfc] = client;
                    }
                }
            }
            return result;
        }






        public string BuildReportsQuery(string reportType, DateTime startDate, DateTime endDate)
        {
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("GeneralSummaryReportQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);

            // Choose the report type
            XmlElement reportTypeElement = inputXMLDoc.CreateElement("GeneralSummaryReportType");
            reportTypeElement.InnerText = reportType;
            QueryRq.AppendChild(reportTypeElement);

            // Set dates
            XmlElement txnDateRangeFilter = inputXMLDoc.CreateElement("ReportPeriod");
            txnDateRangeFilter.AppendChild(inputXMLDoc.CreateElement("FromReportDate")).InnerText = startDate.ToString("yyyy-MM-dd");
            txnDateRangeFilter.AppendChild(inputXMLDoc.CreateElement("ToReportDate")).InnerText = endDate.ToString("yyyy-MM-dd");
            QueryRq.AppendChild(txnDateRangeFilter);

            return inputXMLDoc.OuterXml;
        }

        public string LoadBalance(DateTime startDate, DateTime endDate)
        {
            return LoadReport(ReportType.BALANCE, startDate);
        }

        public string LoadProfitAndLoss(DateTime startDate, DateTime endDate)
        {
            return LoadReport(ReportType.PROFIT_LOSS, startDate);
        }

        public string LoadReport(ReportType reportType, DateTime startDate)
        {
            string result = "";
            string responseReport = "";
            switch (reportType)
            {
                case ReportType.BALANCE:
                    // Eg: 1 al 30 de Junio, tomar el reporte al ultimo de mayo
                    DateTime queryDate = new DateTime(startDate.Year, startDate.Month, 1);
                    queryDate = queryDate.AddDays(-1);
                    QueryQB(BuildReportsQuery("BalanceSheetStandard", queryDate, queryDate), out responseReport);
                    break;
                case ReportType.PROFIT_LOSS:
                    // Eg: 1 al 30 de Junio, tomar el reporte del 1 al ultimo de mayo
                    DateTime queryStartDate = new DateTime(startDate.Year, 1, 1);
                    DateTime queryEndDate = new DateTime(startDate.Year, startDate.Month, 1);
                    queryEndDate = queryEndDate.AddDays(-1);
                    QueryQB(BuildReportsQuery("ProfitAndLossStandard", queryStartDate, queryEndDate), out responseReport);
                    break;
            }
            QuicbooksResponse qbResultQueryAccount = new QuicbooksResponse(responseReport, "GeneralSummaryReportQueryRs");
            if (qbResultQueryAccount.GetNumberOfResulst() > 0)
            {
                XmlNodeList AccountsNodeList = qbResultQueryAccount.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode AccountNode in AccountsNodeList)
                {
                    if (AccountNode["ReportData"] != null)
                    {
                        foreach (XmlNode DataRowNode in AccountNode["ReportData"])
                        {
                            //RowData
                            if (DataRowNode.Name == "DataRow" || DataRowNode.Name == "SubtotalRow")
                            {
                                string accountName = "";
                                double debit = 0, credit = 0;
                                foreach (XmlNode column in DataRowNode.ChildNodes)
                                {
                                    if (column.Attributes["colID"] != null)
                                    {
                                        if (column.Attributes["colID"].Value == "1" && column.Attributes["value"] != null)
                                            accountName = column.Attributes["value"].Value;
                                        else if (column.Attributes["colID"].Value == "2" && column.Attributes["value"] != null)
                                            Double.TryParse(column.Attributes["value"].Value, out debit);
                                        else if (column.Attributes["colID"].Value == "3" && column.Attributes["value"] != null)
                                            Double.TryParse(column.Attributes["value"].Value, out credit);
                                        accountName = Regex.Replace(accountName, "^Total ", ""); // Quitar prefijo Total
                                    }
                                }
                                if (Accounts.ContainsKey(accountName))
                                {
                                    switch (reportType)
                                    {
                                        case ReportType.BALANCE:
                                        case ReportType.PROFIT_LOSS:
                                            Accounts[accountName].initialAmount = debit;
                                            Accounts[accountName].finalAmount = debit;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public string BuildTransactionsQuery(DateTime startDate, DateTime endDate)
        {
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("GeneralDetailReportQueryRq");
            qbXMLMsgsRq.AppendChild(QueryRq);

            // Choose the report type
            XmlElement reportTypeElement = inputXMLDoc.CreateElement("GeneralDetailReportType");
            reportTypeElement.InnerText = "GeneralLedger";
            QueryRq.AppendChild(reportTypeElement);

            // Set dates
            XmlElement txnDateRangeFilter = inputXMLDoc.CreateElement("ReportPeriod");
            txnDateRangeFilter.AppendChild(inputXMLDoc.CreateElement("FromReportDate")).InnerText = startDate.ToString("yyyy-MM-dd");
            txnDateRangeFilter.AppendChild(inputXMLDoc.CreateElement("ToReportDate")).InnerText = endDate.ToString("yyyy-MM-dd");
            QueryRq.AppendChild(txnDateRangeFilter);

            return inputXMLDoc.OuterXml;
        }

        public string LoadTransactions(DateTime startDate, DateTime endDate)
        {
            string result = "";

            string responseAccount;
            QueryQB(BuildTransactionsQuery(startDate, endDate), out responseAccount);
            QuicbooksResponse qbResultQueryTransactions = new QuicbooksResponse(responseAccount, "GeneralDetailReportQueryRs");
            if (qbResultQueryTransactions.GetNumberOfResulst() > 0)
            {
                XmlNodeList TransactionsNodeList = qbResultQueryTransactions.GetXmlNodeList().Item(0).ChildNodes;
                foreach (XmlNode AccountNode in TransactionsNodeList)
                {
                    if (AccountNode["ReportData"] != null)
                    {
                        foreach (XmlNode DataRowNode in AccountNode["ReportData"])
                        {
                            //RowData
                            if (DataRowNode.Name == "SubtotalRow")
                            {
                                bool isAccountRow = false;
                                string accountName = "";
                                double debit = 0, credit = 0;
                                foreach (XmlNode column in DataRowNode.ChildNodes)
                                {
                                    if (column.Attributes["rowType"] != null &&
                                        column.Attributes["rowType"].Value == "account" &&
                                        column.Attributes["value"] != null)
                                    {
                                        accountName = column.Attributes["value"].Value;
                                        isAccountRow = true;
                                    }
                                    if (column.Attributes["colID"] != null)
                                    {
                                        if (column.Attributes["colID"].Value == "9" && column.Attributes["value"] != null)
                                            Double.TryParse(column.Attributes["value"].Value, out debit);
                                        if (column.Attributes["colID"].Value == "10" && column.Attributes["value"] != null)
                                            Double.TryParse(column.Attributes["value"].Value, out credit);
                                    }
                                }
                                if (isAccountRow && Accounts.ContainsKey(accountName))
                                {
                                    Accounts[accountName].debit = debit;
                                    Accounts[accountName].credit = credit;
                                    if (Accounts[accountName].GetNature() == "D")
                                        Accounts[accountName].finalAmount = Accounts[accountName].initialAmount + Accounts[accountName].debit - Accounts[accountName].credit;
                                    else
                                        Accounts[accountName].finalAmount = Accounts[accountName].initialAmount - Accounts[accountName].debit + Accounts[accountName].credit;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        struct AccountNode
        {
            public string name;
            public string parentAccountNumber;
            public string parentAccountName;
            public bool isActive;
            public string accountType;
            public string accountNumber;
            public string desc;
            public bool isCreated;
        };

        public string UploadAccounts(List<List<string>> accounts, out int totalAccountsImported)
        {
            string result = "";
            // Key will be account numbers
            Dictionary<string, AccountNode> newAccounts = new Dictionary<string,AccountNode>();
            // Add a virtual root node
            AccountNode rootNode = new AccountNode();
            rootNode.name = "ROOT";
            rootNode.isCreated = true;
            newAccounts[""] = rootNode;
            foreach (var account in accounts)
            {
                AccountNode newAccount = new AccountNode();
                string accountType = "";
                foreach (var word in account[0].Split(' '))
                {
                    string firstLetter = word.Substring(0, 1).ToUpper();
                    string afterFirstLetter = word.Substring(1).ToLower();
                    accountType += firstLetter + afterFirstLetter;
                }
                /*aqui puede ser*/
                newAccount.accountType = accountType;
                newAccount.accountNumber = account[1];
                newAccount.name = account[2];
                newAccount.parentAccountNumber = account[3];
                newAccount.desc = account[4];
                newAccount.isActive = account[5].ToUpper() == "NO";
                newAccount.isCreated = false;

                newAccounts[newAccount.accountNumber] = newAccount;
            }

            totalAccountsImported = 0;

            // Assign name of parents
            foreach (var accountPair in newAccounts.ToList())
            {
                AccountNode newAccount = accountPair.Value;
                if (newAccount.name != "ROOT")
                {
                    // Validate all parents are present
                    if (!newAccounts.ContainsKey(accountPair.Value.parentAccountNumber))
                    {
                        result = "La cuenta " + accountPair.Value.accountNumber + " - " + accountPair.Value.name
                            + " contiene un numero de cuenta padre que no existe: " + accountPair.Value.parentAccountNumber;
                        return result;
                    }
                    newAccount.parentAccountName = newAccounts[accountPair.Value.parentAccountNumber].name;
                    newAccounts[accountPair.Key] = newAccount;
                }
            }

            bool continueAccountUpload;
            do
            {
                continueAccountUpload = false;
                foreach (var accountPair in newAccounts.ToList())
                {
                    AccountNode accountNode = accountPair.Value;

                    if (accountNode.name == "ROOT") continue;

                    // if it is not created and: it has a white space parent OR its parent has already bein created
                    if (accountNode.isCreated == false
                        && (String.IsNullOrWhiteSpace(accountNode.parentAccountName) || newAccounts[accountNode.parentAccountNumber].isCreated))
                    {
                        string resultAccount = CreateAccount(ref accountNode);
                        if (String.IsNullOrWhiteSpace(resultAccount))
                        {
                            totalAccountsImported++;
                            accountNode.isCreated = true;
                            newAccounts[accountPair.Key] = accountNode;
                            continueAccountUpload = true;
                        }
                        else
                        {
                            result += resultAccount;
                            return resultAccount;
                        }
                    }
                }
            } while (continueAccountUpload);
            return result;
        }

        private string CreateAccount(ref AccountNode accountNode)
        {
            string result = "";

            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement QueryRq = inputXMLDoc.CreateElement("AccountAddRq");
            qbXMLMsgsRq.AppendChild(QueryRq);

            // Add all fields
            XmlElement AddAccount = inputXMLDoc.CreateElement("AccountAdd");

            // Add all fields
            XmlElement AccountName = inputXMLDoc.CreateElement("Name");
            AccountName.InnerText = accountNode.name;
            AddAccount.AppendChild(AccountName);

            XmlElement IsActive = inputXMLDoc.CreateElement("IsActive");
            IsActive.InnerText = accountNode.isActive ? "true" : "false";
            AddAccount.AppendChild(IsActive);

            if (!String.IsNullOrWhiteSpace(accountNode.parentAccountNumber))
            {
                XmlElement ParentRef = inputXMLDoc.CreateElement("ParentRef");
                XmlElement ParentFullName = inputXMLDoc.CreateElement("FullName");
                ParentFullName.InnerText = accountNode.parentAccountName;
                ParentRef.AppendChild(ParentFullName);
                AddAccount.AppendChild(ParentRef);
            }
            XmlElement AccountType = inputXMLDoc.CreateElement("AccountType");
            AccountType.InnerText = accountNode.accountType;
            AddAccount.AppendChild(AccountType);

            XmlElement AccountNumber = inputXMLDoc.CreateElement("AccountNumber");
            AccountNumber.InnerText = accountNode.accountNumber;
            AddAccount.AppendChild(AccountNumber);

            XmlElement Desc = inputXMLDoc.CreateElement("Desc");
            Desc.InnerText = accountNode.desc;
            AddAccount.AppendChild(Desc);

            QueryRq.AppendChild(AddAccount);

            string responseAccount;
            QueryQB(inputXMLDoc.OuterXml, out responseAccount);
            QuicbooksResponse qbResultQueryTransactions = new QuicbooksResponse(responseAccount, "AccountAddRs");
            if (qbResultQueryTransactions.GetNumberOfResulst() > 0)
            {
                if (qbResultQueryTransactions.success)
                {
                    return "";
                }
                else
                {
                    return "Error in Quickbooks cuenta " + accountNode.accountNumber + " - " + accountNode.name
                        + ": " + qbResultQueryTransactions.lastMessage;
                }
            }

            return result;
        }

    }
}
