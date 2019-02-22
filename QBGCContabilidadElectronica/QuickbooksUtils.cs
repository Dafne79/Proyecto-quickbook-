using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Interop.QBXMLRP2;

namespace QB_App
{
    class QuicbooksResponse
    {
        public bool success { get; set; }
        XmlDocument outputXMLDoc;
        XmlNodeList qbXMLMsgsRsNodeList;
        public string lastMessage { get; set; }

        public QuicbooksResponse(string response, string queryNodeResult)
        {
            // Generate the target XML document
            outputXMLDoc = new XmlDocument();

            //en esta parte hay que revisar  los xml por donde pasen 
            outputXMLDoc.LoadXml(response);

            qbXMLMsgsRsNodeList = outputXMLDoc.GetElementsByTagName(queryNodeResult);
            // Obtain response status
            System.Text.StringBuilder popupMessage = new System.Text.StringBuilder();
            XmlAttributeCollection rsAttributes = qbXMLMsgsRsNodeList.Item(0).Attributes;
            //get the status Code, info and Severity
            string retStatusCode = rsAttributes.GetNamedItem("statusCode").Value;
            string retStatusSeverity = rsAttributes.GetNamedItem("statusSeverity").Value;
            string retStatusMessage = rsAttributes.GetNamedItem("statusMessage").Value;
            popupMessage.AppendFormat("statusCode = {0}, statusSeverity = {1}, statusMessage = {2}", retStatusCode, retStatusSeverity, retStatusMessage);
            lastMessage = retStatusMessage;
            success = retStatusCode == "0";
        }

        public int GetNumberOfResulst()
        {
            return qbXMLMsgsRsNodeList.Count;
        }

        public XmlNodeList GetXmlNodeList()
        {
            return qbXMLMsgsRsNodeList;
        }
    }

    public static class QuickbooksUtils
    {
        public static void BuildXMLQueryBase(out XmlDocument inputXMLDoc, out XmlElement qbXMLMsgsRq)
        {
            //Create the qbXML request
            inputXMLDoc = new XmlDocument();

            // Declarations
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"12.0\""));

            // Container
            XmlElement qbXML = inputXMLDoc.CreateElement("QBXML");
            inputXMLDoc.AppendChild(qbXML);

            // Message request type
            qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
        }

    }
}
