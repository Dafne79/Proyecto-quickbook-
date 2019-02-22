using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using QB_App;

namespace QB_App.BillProcessing
{
    class ClientAdder
    {

        public ClientAdder()
        {
        }


        public string InsertClient(ref QuickbooksQueries quickbooksQueries, ref Client client, int recursive_int = 1,string moneda="mxn")
        {
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement InvoicesAddRq = inputXMLDoc.CreateElement("CustomerAddRq");
            qbXMLMsgsRq.AppendChild(InvoicesAddRq);
            XmlElement invoiceXML = inputXMLDoc.CreateElement("CustomerAdd");

            InvoicesAddRq.AppendChild(invoiceXML);

            XmlElement companyName = inputXMLDoc.CreateElement("Name");
            companyName.InnerText = client.nombre.Substring(0, Math.Min(40, client.nombre.Length));
            invoiceXML.AppendChild(companyName);
        
            XmlElement accountNumber = inputXMLDoc.CreateElement("AccountNumber");
            accountNumber.InnerText = client.rfc;
            invoiceXML.AppendChild(accountNumber);
           /*aqui puede ser*/
           /*Aqui tambien agrega*/

           /*if (client.rfc.Contains("XEXX010101000"))*/
           if(moneda != "mxn")
            {
               XmlElement currencyRef = inputXMLDoc.CreateElement("CurrencyRef");
               XmlElement currencyRefId = inputXMLDoc.CreateElement("ListID");
              /* currencyRefId.InnerText = quickbooksQueries.Currencies["Mexican Peso"].listId;*/
               currencyRefId.InnerText = quickbooksQueries.Currencies["US Dollar"].listId;
               currencyRef.AppendChild(currencyRefId);
               invoiceXML.AppendChild(currencyRef);
            }
            
            string updateResponse;
            ClientAdder clientAdder = new ClientAdder();
            if (quickbooksQueries.QueryQB(inputXMLDoc.OuterXml, out updateResponse))
            {
                QuicbooksResponse qbResultUpdateInvoice = new QuicbooksResponse(updateResponse, "CustomerAddRs");
                if (qbResultUpdateInvoice.GetNumberOfResulst() > 0)
                {
                    if (qbResultUpdateInvoice.success)
                    {
                        // Grab the ListID for the new vendor
                        XmlNode ClientNode = qbResultUpdateInvoice.GetXmlNodeList().Item(0).ChildNodes.Item(0);
                        if (ClientNode["ListID"] != null)
                        {
                            client.listId = ClientNode["ListID"].InnerText;
                            return null;
                        }
                        else
                        {
                            return "Customer no tiene ListID: " + client.nombre;
                        }
                    }
                    else
                    {
                        
                        if (recursive_int < 7)
                        {
                            ++recursive_int;
                            client.nombre = client.nombre + " - " + recursive_int.ToString();
                            string clientAdderResult = clientAdder.InsertClient(ref quickbooksQueries, ref client, recursive_int);
                            
                            return clientAdderResult;
                        }
                        else
                        {
                            return "No se puede agregar cliente " + client.nombre;
                        }

                    }
                }
                else
                {
                    return "Quickbooks regresó una respuesta vacía al agregar " + client.nombre;
                }
            }
            else
            {
                return "Se produjo un error al agregar insertar " + client.nombre + ": " + quickbooksQueries.lastError;
            }
        }

    }
}
