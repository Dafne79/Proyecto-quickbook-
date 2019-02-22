using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using QB_App;

namespace QB_App.BillProcessing
{
    class VendorAdder
    {

        public VendorAdder()
        {
        }

        public static string ValidateAccounts(ref Dictionary<string, Account> Accounts)
        {
            List<string> missingAccounts = new List<string>();

            string[] requiredAccounts = { "11901", "11903", "21610", "216", "50301", "CLASIFICACION PENDIENTE" };

            foreach (string requiredAccount in requiredAccounts)
            {
                var accountCount = Accounts.Count(element => element.Value.satAccount.NumCta != null && element.Value.satAccount.NumCta == requiredAccount);
                if (accountCount == 0)
                {
                    missingAccounts.Add(requiredAccount);
                }
            }

            return String.Join(", ", missingAccounts);
        }

        public string InsertVendor(ref QuickbooksQueries quickbooksQueries, ref Vendor vendor, int recursive_int = 1)
        {
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement InvoicesAddRq = inputXMLDoc.CreateElement("VendorAddRq");
            qbXMLMsgsRq.AppendChild(InvoicesAddRq);
            XmlElement invoiceXML = inputXMLDoc.CreateElement("VendorAdd");

            InvoicesAddRq.AppendChild(invoiceXML);

            XmlElement companyName = inputXMLDoc.CreateElement("Name");
            companyName.InnerText = vendor.nombre.Substring(0, Math.Min(40, vendor.nombre.Length));
            invoiceXML.AppendChild(companyName);

            XmlElement vendorAddress = inputXMLDoc.CreateElement("VendorAddress");

            if (!String.IsNullOrWhiteSpace(vendor.calle + vendor.noInterior + vendor.noExterior))
            {
                XmlElement addr1 = inputXMLDoc.CreateElement("Addr1");
                string addr1Full = ((vendor.calle.Trim() + " " + vendor.noInterior).Trim() + " " + vendor.noExterior).Trim();
                addr1.InnerText = addr1Full.Substring(0, Math.Min(40, addr1Full.Length));
                vendorAddress.AppendChild(addr1);
            }

            if (!String.IsNullOrWhiteSpace(vendor.colonia))
            {
                XmlElement addr2 = inputXMLDoc.CreateElement("Addr2");
                string addr2Full = vendor.colonia.Trim();
                addr2.InnerText = addr2Full.Substring(0, Math.Min(40, addr2Full.Length));
                vendorAddress.AppendChild(addr2);
            }

            if (!String.IsNullOrWhiteSpace(vendor.municipio))
            {
                XmlElement city = inputXMLDoc.CreateElement("City");
                city.InnerText = vendor.municipio.Substring(0, Math.Min(30, vendor.municipio.Length)); ;
                vendorAddress.AppendChild(city);
            }

            if (!String.IsNullOrWhiteSpace(vendor.estado))
            {
                XmlElement state = inputXMLDoc.CreateElement("State");
                state.InnerText = vendor.estado.Substring(0, Math.Min(30, vendor.estado.Length)); ;
                vendorAddress.AppendChild(state);
            }

            if (!String.IsNullOrWhiteSpace(vendor.codigoPostal))
            {
                XmlElement postalCode = inputXMLDoc.CreateElement("PostalCode");
                postalCode.InnerText = vendor.codigoPostal.Substring(0, Math.Min(10, vendor.codigoPostal.Length));
                vendorAddress.AppendChild(postalCode);
            }

            invoiceXML.AppendChild(vendorAddress);

            XmlElement accountNumber = inputXMLDoc.CreateElement("AccountNumber");
            accountNumber.InnerText = vendor.rfc;
            invoiceXML.AppendChild(accountNumber);

            /*aqui puede ser*/
            

            if (vendor.rfc.Contains("XEXX010101000"))
            {
               XmlElement currencyRef = inputXMLDoc.CreateElement("CurrencyRef");
               XmlElement currencyRefId = inputXMLDoc.CreateElement("ListID");
               currencyRefId.InnerText = quickbooksQueries.Currencies["US Dollar"].listId;
               /*currencyRefId.InnerText = quickbooksQueries.Currencies["Mexican Peso"].listId;*/
               currencyRef.AppendChild(currencyRefId);
               invoiceXML.AppendChild(currencyRef);
            }



            string updateResponse;
            VendorAdder vendorAdder = new VendorAdder();
            if (quickbooksQueries.QueryQB(inputXMLDoc.OuterXml, out updateResponse))
            {
                QuicbooksResponse qbResultUpdateInvoice = new QuicbooksResponse(updateResponse, "VendorAddRs");
                if (qbResultUpdateInvoice.GetNumberOfResulst() > 0)
                {
                    if (qbResultUpdateInvoice.success)
                    {
                        // Grab the ListID for the new vendor
                        XmlNode VendorNode = qbResultUpdateInvoice.GetXmlNodeList().Item(0).ChildNodes.Item(0);
                        if (VendorNode["ListID"] != null)
                        {
                            vendor.listId = VendorNode["ListID"].InnerText;
                            return null;
                        }
                        else
                        {
                            return "Vendor no tiene ListID: " + vendor.nombre;
                        }
                    }
                    else
                    {
                       
                        if (recursive_int < 7)
                        {
                            ++recursive_int;
                            vendor.nombre = vendor.nombre + " - " + recursive_int.ToString();
                            string vendorAdderResult = vendorAdder.InsertVendor(ref quickbooksQueries, ref vendor, recursive_int);
                            //string msg = "No se puede agregar proveedor " + ;
                            //string msg; msg = "Agregado " + vendor.nombre;

                            return vendorAdderResult;
                        }
                        else
                        {
                            return "No se puede agregar Proveedor " + vendor.nombre;
                        }

                    }
                }
                else
                {
                    return "Quickbooks regresó una respuesta vacía al agregar " + vendor.nombre;
                }
            }
            else
            {
                return "Se produjo un error al agregar insertar " + vendor.nombre + ": " + quickbooksQueries.lastError;
            }
        }
    }
}
