using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using QB_App;


namespace QB_App
{
    public class ItemAdder
    {

        public enum BillLineItemType { Concepto, Descuentos, RetencionesIVA, RetencionesISR, TrasladosIVA, TrasladosIEPS, ClasificacionPendiente, TaxedAccount, NonTaxedAccount, Inventario };
        Dictionary<BillLineItemType, string> specialAccounts;

        public ItemAdder()
        {
        }

        public string InsertItem(ref QuickbooksQueries quickbooksQueries, ref BillLineItem item, int recursive_int = 1)
        {
            //specialAccounts = 


            specialAccounts = new Dictionary<BillLineItemType, string>();
            foreach (KeyValuePair<string, Account> account in quickbooksQueries.Accounts)
            {
                switch (account.Value.satAccount.NumCta)
                {
                    case "50301": specialAccounts[BillLineItemType.Descuentos] = account.Value.listId; break;
                    case "21610": specialAccounts[BillLineItemType.RetencionesIVA] = account.Value.listId; break;
                    case "216": specialAccounts[BillLineItemType.RetencionesISR] = account.Value.listId; break;
                    case "11901": specialAccounts[BillLineItemType.TrasladosIVA] = account.Value.listId; break;
                    case "11903": specialAccounts[BillLineItemType.TrasladosIEPS] = account.Value.listId; break;
                    case "40101": specialAccounts[BillLineItemType.TaxedAccount] = account.Value.listId; break;
                    case "40104": specialAccounts[BillLineItemType.NonTaxedAccount] = account.Value.listId; break;
                    case "11501": specialAccounts[BillLineItemType.Inventario] = account.Value.listId; break;
                    case "CLASIFICACION PENDIENTE": specialAccounts[BillLineItemType.ClasificacionPendiente] = account.Value.listId; break;
                }
            }

            
            //Creates item

            XmlDocument inputItemXMLDoc;
            XmlElement qbXMLMsgsRq;
            QuickbooksUtils.BuildXMLQueryBase(out inputItemXMLDoc, out qbXMLMsgsRq);

            // Mark as query type
            XmlElement ItemsAddRq = inputItemXMLDoc.CreateElement("ItemNonInventoryAddRq");
            qbXMLMsgsRq.AppendChild(ItemsAddRq);

            XmlElement itemXML = inputItemXMLDoc.CreateElement("ItemNonInventoryAdd");
            ItemsAddRq.AppendChild(itemXML);

            XmlElement itemNameXML = inputItemXMLDoc.CreateElement("Name");
            itemNameXML.InnerText = item.descripcion.Substring(0, Math.Min(28, item.descripcion.Length));
            itemXML.AppendChild(itemNameXML);


            XmlElement itemProductKeyXML = inputItemXMLDoc.CreateElement("ManufacturerPartNumber");
            itemProductKeyXML.InnerText = item.claveprodserv;
            itemXML.AppendChild(itemProductKeyXML);

            if (quickbooksQueries.Uoms.ContainsKey(item.claveunidad))
            {
                XmlElement uomXML = inputItemXMLDoc.CreateElement("UnitOfMeasureSetRef");
                itemXML.AppendChild(uomXML);

                XmlElement uomNameXML = inputItemXMLDoc.CreateElement("ListID");
                uomNameXML.InnerText = quickbooksQueries.Uoms[item.claveunidad].listId;
                uomXML.AppendChild(uomNameXML);
            }



            //Fields For non item inventory TESTING WORKING
            /*
            XmlElement salesOrPurchaseXML = inputItemXMLDoc.CreateElement("SalesOrPurchase");
            itemXML.AppendChild(salesOrPurchaseXML);


            XmlElement salesDescXML = inputItemXMLDoc.CreateElement("Desc");
            salesDescXML.InnerText = item.descripcion;
            salesOrPurchaseXML.AppendChild(salesDescXML);


            XmlElement salesPriceXML = inputItemXMLDoc.CreateElement("Price");
            salesPriceXML.InnerText = item.importe.ToString();
            salesOrPurchaseXML.AppendChild(salesPriceXML);

            XmlElement incomeAccountXML = inputItemXMLDoc.CreateElement("AccountRef");
            salesOrPurchaseXML.AppendChild(incomeAccountXML);

            XmlElement incomeAccountListIdXML = inputItemXMLDoc.CreateElement("ListID");
            incomeAccountListIdXML.InnerText = specialAccounts[BillLineItemType.ClasificacionPendiente];
            incomeAccountXML.AppendChild(incomeAccountListIdXML);

            */

            XmlElement salesTaxCodeXML = inputItemXMLDoc.CreateElement("SalesTaxCodeRef");
            itemXML.AppendChild(salesTaxCodeXML);

            XmlElement salesTaxIdXML = inputItemXMLDoc.CreateElement("ListID");

            if (item.createsTax)
            {
                salesTaxIdXML.InnerText = quickbooksQueries.Taxes["Tax"].listId;
            }
            else
            {
                salesTaxIdXML.InnerText = quickbooksQueries.Taxes["Non"].listId;
            }
            
            salesTaxCodeXML.AppendChild(salesTaxIdXML);


            //Fields For non item inventory TESTING
            XmlElement salesAndPurchaseXML = inputItemXMLDoc.CreateElement("SalesAndPurchase");
            itemXML.AppendChild(salesAndPurchaseXML);


            XmlElement salesDescXML = inputItemXMLDoc.CreateElement("SalesDesc");
            salesDescXML.InnerText = item.descripcion;
            salesAndPurchaseXML.AppendChild(salesDescXML);

            XmlElement salesPriceXML = inputItemXMLDoc.CreateElement("SalesPrice");
            salesPriceXML.InnerText = "0";//item.importe.ToString();
            salesAndPurchaseXML.AppendChild(salesPriceXML);


            XmlElement incomeAccountXML = inputItemXMLDoc.CreateElement("IncomeAccountRef");
            salesAndPurchaseXML.AppendChild(incomeAccountXML);

            XmlElement incomeAccountListIdXML = inputItemXMLDoc.CreateElement("ListID");

            if (item.createsTax)
            {
                incomeAccountListIdXML.InnerText = specialAccounts[BillLineItemType.TaxedAccount];
            }
            else
            {
                incomeAccountListIdXML.InnerText = specialAccounts[BillLineItemType.NonTaxedAccount];
            }
            
            incomeAccountXML.AppendChild(incomeAccountListIdXML);


            XmlElement purchaseDescXML = inputItemXMLDoc.CreateElement("PurchaseDesc");
            purchaseDescXML.InnerText = item.descripcion;
            salesAndPurchaseXML.AppendChild(purchaseDescXML);

            XmlElement purchaseCostXML = inputItemXMLDoc.CreateElement("PurchaseCost");
            purchaseCostXML.InnerText = "0"; //item.importe.ToString();
            salesAndPurchaseXML.AppendChild(purchaseCostXML);
        
            XmlElement expenseAccountXML = inputItemXMLDoc.CreateElement("ExpenseAccountRef");
            salesAndPurchaseXML.AppendChild(expenseAccountXML);

            XmlElement expenseAccountListIdXML = inputItemXMLDoc.CreateElement("ListID");
            if (item.is_invoice)
            {
                expenseAccountListIdXML.InnerText = specialAccounts[BillLineItemType.Inventario];
            }
            else
            {
                expenseAccountListIdXML.InnerText = specialAccounts[BillLineItemType.ClasificacionPendiente];
            }
            
            expenseAccountXML.AppendChild(expenseAccountListIdXML);


            //XmlElement prodKey = inputItemXMLDoc.CreateElement("IncludeRetElement");
            //prodKey.InnerText = "CLAVEPROD";
            //itemXML.AppendChild(prodKey);

            /*
            XmlElement vendorAccountXML = inputItemXMLDoc.CreateElement("PrefVendorRef");
            salesAndPurchaseXML.AppendChild(vendorAccountXML);

            XmlElement vendorAccountListIdXML = inputItemXMLDoc.CreateElement("ListID");
            vendorAccountListIdXML.InnerText = specialAccounts[BillLineItemType.ClasificacionPendiente];
            vendorAccountXML.AppendChild(vendorAccountListIdXML);
            */
            //string updateResponse;

            //Fields for item inventory
            /*
            
            XmlElement incomeAccountXML = inputItemXMLDoc.CreateElement("IncomeAccountRef");
            itemXML.AppendChild(incomeAccountXML);

            XmlElement incomeAccountListIdXML = inputItemXMLDoc.CreateElement("ListID");
            incomeAccountListIdXML.InnerText = specialAccounts[BillLineItemType.ClasificacionPendiente];
            incomeAccountXML.AppendChild(incomeAccountListIdXML);

            XmlElement cogsAccountXML = inputItemXMLDoc.CreateElement("COGSAccountRef");
            itemXML.AppendChild(cogsAccountXML);

            XmlElement cogsAccountListIdXML = inputItemXMLDoc.CreateElement("ListID");
            cogsAccountListIdXML.InnerText = specialAccounts[BillLineItemType.ClasificacionPendiente];
            cogsAccountXML.AppendChild(cogsAccountListIdXML);




            XmlElement assetAccountXML = inputItemXMLDoc.CreateElement("AssetAccountRef");
            itemXML.AppendChild(assetAccountXML);

            XmlElement assetAccountListIdXML = inputItemXMLDoc.CreateElement("ListID");
            assetAccountListIdXML.InnerText = specialAccounts[BillLineItemType.ClasificacionPendiente];
            assetAccountXML.AppendChild(assetAccountListIdXML);
            */
            string msg = "some_msg";
            string qbResponse;
            ItemAdder itemAdder = new ItemAdder();
            if (quickbooksQueries.QueryQB(inputItemXMLDoc.OuterXml, out qbResponse))
            {

                QuicbooksResponse qbResultQuery = new QuicbooksResponse(qbResponse, "ItemNonInventoryAddRs");
                if (qbResultQuery.GetNumberOfResulst() > 0)
                {
                    if (qbResultQuery.success)
                    {
                        //qbXMLMsgsRsNodeList
                        //itemlistId = "";
                        msg = "Item created";
                        XmlNodeList responseNodeList = qbResultQuery.GetXmlNodeList().Item(0).ChildNodes;
                        foreach (XmlNode responseNode in responseNodeList)
                        {
                            Item new_item = new Item();

                            if (responseNode["ListID"] != null)
                                new_item.listId = responseNode["ListID"].InnerText;
                            if (responseNode["Name"] != null)
                                new_item.name = responseNode["Name"].InnerText;
                            if (responseNode["FullName"] != null)
                                new_item.fullName = responseNode["FullName"].InnerText;

                            if (responseNode["ManufacturerPartNumber"] != null)
                                new_item.productKey = responseNode["ManufacturerPartNumber"].InnerText;

                            if (responseNode["UnitOfMeasureSetRef"] != null)
                            {
                                foreach (XmlNode DataRowNode in responseNode["UnitOfMeasureSetRef"])
                                {
                                    if (DataRowNode["ListID"] != null)
                                        new_item.uomListId = DataRowNode["ListID"].InnerText;
                                    if (DataRowNode["FullName"] != null)
                                        new_item.uomFullName = DataRowNode["FullName"].InnerText;
                                }
                            }

                            if (!String.IsNullOrEmpty(new_item.listId))
                            {
                                //itemlistId = new_item.listId;
                                //Items[new_item.productKey] = new_item;
                                quickbooksQueries.Items[new_item.productKey] = new_item;
                                return null;
                            }
                            else
                            {
                                return "Item no tiene List id " + new_item.name;
                            }
 
                        }
                    }
                    else
                    {

                        if (recursive_int < 7)
                        {
                            ++recursive_int;
                            item.descripcion = item.descripcion.Substring(0, Math.Min(26, item.descripcion.Length)) + "-" + recursive_int.ToString();
                            //item.descripcion = item.descripcion + " - " + recursive_int.ToString();
                            string itemAdderResult = itemAdder.InsertItem(ref quickbooksQueries, ref item, recursive_int);
                            return itemAdderResult;
                        }
                        else
                        {
                            return "No se puede agregar item " + item.descripcion;
                        }
                        
                    }
                }
                else
                {
                    // It could be a same item name..// QB doesnt allow 2 items with same name
                    msg = "Quickbooks regresó una respuesta vacía al agregar item ";
                }
            }
            else
            {
                msg = "Se produjo un error al agregar insertar item ";
            }

            string some_string = msg;// To debug
            return some_string;
        }
        

    }
}
