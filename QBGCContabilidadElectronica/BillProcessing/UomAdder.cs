using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
namespace QB_App.BillProcessing
{
    public class UomAdder
    {
        public UomAdder()
        {

        }



        public string InsertUom(ref QuickbooksQueries quickbooksQueries, ref BillLineItem item)
        {
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement InvoicesAddRq = inputXMLDoc.CreateElement("UnitOfMeasureSetAddRq");
            qbXMLMsgsRq.AppendChild(InvoicesAddRq);
            XmlElement invoiceXML = inputXMLDoc.CreateElement("UnitOfMeasureSetAdd");

            InvoicesAddRq.AppendChild(invoiceXML);

            XmlElement name = inputXMLDoc.CreateElement("Name");
            name.InnerText = item.unidad.Substring(0, Math.Min(30, item.unidad.Length));
            invoiceXML.AppendChild(name);

            XmlElement unitType = inputXMLDoc.CreateElement("UnitOfMeasureType");
            unitType.InnerText = "Other";
            invoiceXML.AppendChild(unitType);
            
            XmlElement baseUnit = inputXMLDoc.CreateElement("BaseUnit");
            invoiceXML.AppendChild(baseUnit);

            XmlElement baseUnitName = inputXMLDoc.CreateElement("Name");
            baseUnitName.InnerText = item.unidad.Substring(0, Math.Min(30, item.unidad.Length));
            baseUnit.AppendChild(baseUnitName);

            XmlElement abbr = inputXMLDoc.CreateElement("Abbreviation");
            abbr.InnerText = item.claveunidad;
            baseUnit.AppendChild(abbr);

            string updateResponse;
            UomAdder uomAdder = new UomAdder();
            if (quickbooksQueries.QueryQB(inputXMLDoc.OuterXml, out updateResponse))
            {
                QuicbooksResponse qbResultUpdateInvoice = new QuicbooksResponse(updateResponse, "UnitOfMeasureSetAddRs");
                if (qbResultUpdateInvoice.GetNumberOfResulst() > 0)
                {
                    if (qbResultUpdateInvoice.success)
                    {
                        // Grab the ListID for the new vendor
                        //XmlNode ClientNode = qbResultUpdateInvoice.GetXmlNodeList().Item(0).ChildNodes.Item(0);
                        //msg = "Item created";
                        XmlNodeList responseNodeList = qbResultUpdateInvoice.GetXmlNodeList().Item(0).ChildNodes;
                        foreach (XmlNode responseNode in responseNodeList)
                        {
                            UnitMeasure newUom = new UnitMeasure();

                            //Here is some weird behavior
                            foreach (XmlNode childNode in responseNode.ChildNodes)
                            {
                                if (childNode.Name == "ListID")
                                    newUom.listId = childNode.InnerText;
                                if (childNode.Name == "Name")
                                    newUom.name = childNode.InnerText;                                
                            }

                            
                            if (responseNode["BaseUnit"] != null)
                            {
                                foreach (XmlNode DataRowNode in responseNode["BaseUnit"])
                                {
     
                                    if (DataRowNode.Name == "Name")
                                        newUom.baseUnitName = DataRowNode.InnerText;
                                    if (DataRowNode.Name == "Abbreviation")
                                        newUom.abbreviation = DataRowNode.InnerText;
                                }
                            }
                            
                            if (!String.IsNullOrEmpty(newUom.abbreviation))
                            {
                                //itemlistId = new_item.listId;
                                //Items[new_item.productKey] = new_item;
                                quickbooksQueries.Uoms[newUom.abbreviation] = newUom;
                                return null;
                            }
                            else
                            {
                                return "Item no tiene List id " + newUom.name;
                            }


                        }
                    }
                    else
                    {
                        //Random rnd = new Random();
                        //Makes recursive insertion tries 5 times // Misssing loop of 5 :/
                        //client.nombre = client.nombre + " - " + rnd.ToString();
                        //string clientAdderResult = clientAdder.InsertClient(ref quickbooksQueries, ref client);
                        //string msg = "No se puede agregar proveedor " + ;
                        //string msg; msg = "Agregado " + vendor.nombre;
                        return "No se puede agregar unit";
                    }
                }
                else
                {
                    return "Quickbooks regresó una respuesta vacía al agregar " + item.descripcion;
                }
            }
            else
            {
                return "Se produjo un error al agregar insertar " + item.descripcion + ": " + quickbooksQueries.lastError;
            }
            return "ERror";
        }

    }
}
