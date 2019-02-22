using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using QB_App;

namespace QB_App.BillProcessing
{
    public class DataExtAdder
    {

        public DataExtAdder()
        {

        }


        public string insertData(ref QuickbooksQueries quickbooksQueries, string listId, string value)
        {
            string msg = "";
            
            XmlDocument inputItemXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputItemXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement ItemsAddRq = inputItemXMLDoc.CreateElement("DataExtAddRq");
            qbXMLMsgsRq.AppendChild(ItemsAddRq);

            XmlElement itemXML = inputItemXMLDoc.CreateElement("DataExtAdd");
            ItemsAddRq.AppendChild(itemXML);

            XmlElement itemOwnerXML = inputItemXMLDoc.CreateElement("OwnerID");
            itemOwnerXML.InnerText = "0";
            itemXML.AppendChild(itemOwnerXML);

            XmlElement itemNameXML = inputItemXMLDoc.CreateElement("DataExtName");
            itemNameXML.InnerText = "CLAVEPROD";
            itemXML.AppendChild(itemNameXML);

            XmlElement itemtypeXML = inputItemXMLDoc.CreateElement("ListDataExtType");
            itemtypeXML.InnerText = "Item";
            itemXML.AppendChild(itemtypeXML);

            XmlElement itemobjXML = inputItemXMLDoc.CreateElement("ListObjRef");
            itemXML.AppendChild(itemobjXML);

            XmlElement itemListIdXML = inputItemXMLDoc.CreateElement("ListID");
            itemListIdXML.InnerText = listId;
            itemobjXML.AppendChild(itemListIdXML);
            
            XmlElement itemValueXML = inputItemXMLDoc.CreateElement("DataExtValue");
            itemValueXML.InnerText = value;
            itemXML.AppendChild(itemValueXML);

            string qbResponse;

            if (quickbooksQueries.QueryQB(inputItemXMLDoc.OuterXml, out qbResponse))
            {

                QuicbooksResponse qbResultQuery = new QuicbooksResponse(qbResponse, "DataExtAddRs");
                if (qbResultQuery.GetNumberOfResulst() > 0)
                {
                    if (qbResultQuery.success)
                    {

                        msg = null;
                    }
                    else
                    {
                        msg = "No se pudo agregar el item ";
                    }
                }
                else
                {
                    msg = "Quickbooks regresó una respuesta vacía al agregar item ";
                }
            }
            else
            {
                msg = "Se produjo un error al agregar insertar item ";
            }

            

            return msg;
        }
    }
}
