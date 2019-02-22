using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QB_App.BillProcessing
{
    public class BillAdder
    {


        public enum BillLineItemType { Concepto, Descuentos, RetencionesIVA, RetencionesISR, TrasladosIVA, TrasladosIEPS, ClasificacionPendiente};
        Dictionary<BillLineItemType, string> specialAccounts;
        Dictionary<string, Item> Items;
        Vendor vendor;

        public BillAdder(ref Dictionary<string, Account> Accounts, Vendor vendor)
        {
            this.vendor = vendor;
            //this.Items = Items;
            specialAccounts = new Dictionary<BillLineItemType, string>();
            foreach (KeyValuePair<string, Account> account in Accounts)
            {
                switch (account.Value.satAccount.NumCta)
                {
                    case "50301": specialAccounts[BillLineItemType.Descuentos] = account.Value.listId; break;
                    case "21610": specialAccounts[BillLineItemType.RetencionesIVA] = account.Value.listId; break;
                    case "216": specialAccounts[BillLineItemType.RetencionesISR] = account.Value.listId; break;
                    case "11901": specialAccounts[BillLineItemType.TrasladosIVA] = account.Value.listId; break;
                    case "11903": specialAccounts[BillLineItemType.TrasladosIEPS] = account.Value.listId; break;
                    case "CLASIFICACION PENDIENTE": specialAccounts[BillLineItemType.ClasificacionPendiente] = account.Value.listId; break;
                }
            }
        }

        public XmlElement CreateBillLine(ref XmlDocument inputXMLDoc, BillLineItemType billLineItemType, double amount, ref QuickbooksQueries quickbooksQueries, string claveprodserv, string descripcion, double quantity)
        {
            XmlElement expenseLine = inputXMLDoc.CreateElement("ItemLineAdd");
            /*
            // Agregar cuenta si el vendor tiene
            if (billLineItemType == BillLineItemType.Concepto)
            {
                XmlElement accountRef = inputXMLDoc.CreateElement("AccountRef");
                XmlElement accountList = inputXMLDoc.CreateElement("ListID");
                if (!String.IsNullOrEmpty(vendor.accountListId))
                {
                    accountList.InnerText = vendor.accountListId;
                }
                else
                {
                    accountList.InnerText = specialAccounts[BillLineItemType.ClasificacionPendiente];
                }
                accountRef.AppendChild(accountList);
                expenseLine.AppendChild(accountRef);
            }
            else if (billLineItemType != BillLineItemType.Concepto)
            {
                XmlElement accountRef = inputXMLDoc.CreateElement("AccountRef");
                XmlElement accountList = inputXMLDoc.CreateElement("ListID");
                accountList.InnerText = specialAccounts[billLineItemType];
                accountRef.AppendChild(accountList);
                expenseLine.AppendChild(accountRef);
            }
            */

            Item bLineItem = quickbooksQueries.Items[claveprodserv];
            string itemlistId = bLineItem.listId;
            
            System.Diagnostics.Trace.WriteLine(itemlistId);
            XmlElement itemNode = inputXMLDoc.CreateElement("ItemRef");
            //itemNode.InnerText = Convert.ToDecimal(amount).ToString("F");
            expenseLine.AppendChild(itemNode);

            XmlElement itemListIdNode = inputXMLDoc.CreateElement("ListID");
            itemListIdNode.InnerText = itemlistId;// GET ITEM ID
            itemNode.AppendChild(itemListIdNode);

            // Otros datos del billLineItem
            XmlElement memoItem = inputXMLDoc.CreateElement("Desc");
            memoItem.InnerText = descripcion.Substring(0, Math.Min(4000, descripcion.Length));
            expenseLine.AppendChild(memoItem);

            // Otros datos del billLineItem
            XmlElement quantItem = inputXMLDoc.CreateElement("Quantity");
            quantItem.InnerText = quantity.ToString();
            expenseLine.AppendChild(quantItem);

            XmlElement amountNode = inputXMLDoc.CreateElement("Amount");
            amountNode.InnerText = Convert.ToDecimal(amount).ToString("F");
            expenseLine.AppendChild(amountNode);



            return expenseLine;
        }

        public string InsertBill(ref QuickbooksQueries quickbooksQueries, Bill bill)
        {
            XmlDocument inputXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement InvoicesAddRq = inputXMLDoc.CreateElement("BillAddRq");
            qbXMLMsgsRq.AppendChild(InvoicesAddRq);
            XmlElement invoiceXML = inputXMLDoc.CreateElement("BillAdd");

            InvoicesAddRq.AppendChild(invoiceXML);

            XmlElement vendorRef = inputXMLDoc.CreateElement("VendorRef");
            XmlElement vendorListID = inputXMLDoc.CreateElement("ListID");
            vendorListID.InnerText = vendor.listId;
            vendorRef.AppendChild(vendorListID);
            invoiceXML.AppendChild(vendorRef);

            XmlElement txnDate = inputXMLDoc.CreateElement("TxnDate");
            string billDate = bill.fecha.ToString("yyyy-MM-dd");
            txnDate.InnerText = billDate;
            invoiceXML.AppendChild(txnDate);


            bool billExists = quickbooksQueries.QueryExistingBill(billDate, bill.uuid);

            if (billExists)
            {
                return "La factura ya existe";
            }
            else
            {

                XmlElement refNumber = inputXMLDoc.CreateElement("RefNumber");
                string folioAndSeries = bill.serie.Trim() + bill.folio.Trim();
                refNumber.InnerText = folioAndSeries.Substring(0, Math.Min(19, folioAndSeries.Length));
                invoiceXML.AppendChild(refNumber);

                XmlElement memo = inputXMLDoc.CreateElement("Memo");
                memo.InnerText = bill.uuid;
                invoiceXML.AppendChild(memo);

                if (bill.moneda != "mxn")
                {
                    XmlElement exchangeRate = inputXMLDoc.CreateElement("ExchangeRate");
                    exchangeRate.InnerText = bill.tipoCambio.ToString();
                    invoiceXML.AppendChild(exchangeRate);
                }
                
                foreach (BillLineItem billLineItem in bill.conceptos)
                {
                    invoiceXML.AppendChild(CreateBillLine(ref inputXMLDoc, BillLineItemType.Concepto, billLineItem.importe, ref quickbooksQueries, billLineItem.claveprodserv, billLineItem.descripcion, billLineItem.cantidad));
                }



                // Agregar cada tipo de impuesto
                /*
                string claveprodserv = "0";
                if (bill.descuento != 0)
                {
                    invoiceXML.AppendChild(CreateBillLine(ref inputXMLDoc, BillLineItemType.Descuentos, -bill.descuento, ref quickbooksQueries, claveprodserv, "Descuento"));
                }
                */
                string claveprodserv;
                if (bill.retencionesIVA != 0)
                {
                    claveprodserv = "IMPUESTOS002BR";
                    invoiceXML.AppendChild(CreateBillLine(ref inputXMLDoc, BillLineItemType.RetencionesIVA, -bill.retencionesIVA, ref quickbooksQueries, claveprodserv, "Retenciones IVA", 1));
                }
                if (bill.retencionesISR != 0)
                {
                    claveprodserv = "IMPUESTOS001BR";
                    invoiceXML.AppendChild(CreateBillLine(ref inputXMLDoc, BillLineItemType.RetencionesISR, -bill.retencionesISR, ref quickbooksQueries, claveprodserv, "Retenciones ISR", 1));
                }
                if (bill.trasladosIVA != 0)
                {
                    claveprodserv = "IMPUESTOS002IT";
                    invoiceXML.AppendChild(CreateBillLine(ref inputXMLDoc, BillLineItemType.TrasladosIVA, bill.trasladosIVA, ref quickbooksQueries, claveprodserv, "Traslados IVA", 1));
                }
                if (bill.trasladosIEPS != 0)
                {
                    claveprodserv = "IMPUESTOS003BT";
                    invoiceXML.AppendChild(CreateBillLine(ref inputXMLDoc, BillLineItemType.TrasladosIEPS, bill.trasladosIEPS, ref quickbooksQueries, claveprodserv, "Traslados IEPS", 1));
                }
                
                string updateResponse;
                if (quickbooksQueries.QueryQB(inputXMLDoc.OuterXml, out updateResponse))
                {
                    QuicbooksResponse qbResultUpdateInvoice = new QuicbooksResponse(updateResponse, "BillAddRs");
                    if (qbResultUpdateInvoice.GetNumberOfResulst() > 0)
                    {
                        if (qbResultUpdateInvoice.success)
                        {
                            return null;
                        }
                        else
                        {
                            return "No se puedo agregar bill " + bill.uuid + " proveedor " + vendor.nombre;
                        }
                    }
                    else
                    {
                        return "Quickbooks regresó una respuesta vacía al agregar bill " + bill.uuid + " proveedor " + vendor.nombre;
                    }
                }
                else
                {
                    return "Se produjo un error al agregar insertar bill " + bill.uuid + " proveedor " + vendor.nombre + ": " + quickbooksQueries.lastError;
                }
            }
        }
    }
}
