using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading.Tasks;

namespace QB_App
{

    
    public struct LineItemContribution
    {
        public string name;
        public double percentage;
        public string type;
        public double amount;

    }
    

    public struct BillLineItem
    {
        public double cantidad;
        public string unidad;
        public string descripcion;
        public double valorUnitario;
        public double importe;
        public string claveprodserv;
        public string claveunidad;
        public bool createsTax;
        public bool is_invoice;
        // Taxes
        public List<LineItemContribution> contributions;
        public void SetDefaults()
        {
            cantidad = 0;
            unidad = "ND";
            descripcion = "Sin descripcion";
            valorUnitario = 0;
            importe = 0;
            contributions = new List<LineItemContribution>();
        }
    }
    public struct Bill
    {
        public double   total;
        public double   subtotal;
        public DateTime fecha;
        public double   descuento;
        public string   formaDePago;
        public string   metodoDePago;
        public string   uuid;
        public string   serie;
        public string   folio;
        public string   moneda;
        public double   tipoCambio;
        public string   tipoComprobante;
        public int      invoice_or_bill; // 1 Invoice, 2 Bill
        public bool     is_invoice;
        
        // Conceptos
        public List<BillLineItem> conceptos;

        // Impuestos
        public double retencionesIVA;
        public double retencionesISR;
        public double trasladosIVA;
        public double trasladosIEPS;
        
        public void SetDefaults()
        {
            total = 0;
            subtotal = 0;
            descuento = 0;
            fecha = new DateTime();
            serie = "";
            moneda = "";
            tipoComprobante = "";
            tipoCambio = 0;
            folio = "";
            uuid = "Sin UUID";
            formaDePago = "SIN ESPECIFICAR";
            metodoDePago = "SIN ESPECIFICAR";
            conceptos = new List<BillLineItem>();
            retencionesIVA = 0;
            retencionesISR = 0;
            trasladosIVA = 0;
            trasladosIEPS = 0;
            invoice_or_bill = 1;
            is_invoice = false;
        }
    }
    class BillParser
    {
        XmlDocument xmlDocument;
        Bill bill = new Bill();
        Vendor vendor = new Vendor();
        Client client = new Client();
        
        public bool Load(string xmlFileName)
        {
            try
            {
                xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(File.ReadAllText(xmlFileName));
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Parse(string mainRfc = "")
        {
            try
            {
                bill.SetDefaults();
                vendor.SetDefaults();
                client.SetDefaults();

                foreach (XmlNode xmlNode in xmlDocument.ChildNodes)
                {
                    if (xmlNode.Name.ToLower().Contains("comprobante"))
                    {
                        // Detalle de Factura
                        foreach (XmlAttribute attribute in xmlNode.Attributes)
                        {
                            if (attribute.Name.ToLower() == "total")
                            {
                                bill.total = Double.Parse(attribute.Value);
                            }
                            else if (attribute.Name.ToLower() == "subtotal")
                            {
                                bill.subtotal = Double.Parse(attribute.Value);
                            }
                            else if (attribute.Name.ToLower() == "descuento")
                            {
                                bill.descuento = Double.Parse(attribute.Value);
                            }
                            else if (attribute.Name.ToLower() == "fecha")
                            {
                                bill.fecha = DateTime.Parse(attribute.Value);
                            }

                            else if (attribute.Name.ToLower() == "tipodecomprobante")
                            {
                                bill.tipoComprobante = attribute.Value.ToLower();
                            }

                            else if (attribute.Name.ToLower() == "serie")
                            {
                                bill.serie = attribute.Value;
                            }
                            else if (attribute.Name.ToLower() == "folio")
                            {
                                bill.folio = attribute.Value;
                            }
                            else if (attribute.Name.ToLower() == "formadepago")
                            {
                                bill.formaDePago = attribute.Value;
                            }
                            else if (attribute.Name.ToLower() == "metododepago")
                            {
                                bill.metodoDePago = attribute.Value;
                            }
                            else if (attribute.Name.ToLower() == "moneda")
                            {
                                bill.moneda = attribute.Value.ToLower();
                            }
                            else if (attribute.Name.ToLower() == "tipocambio")
                            {
                                bill.tipoCambio = Double.Parse(attribute.Value);
                            }
                            
                        }

                        // Datos del proveedor
                            foreach (XmlNode xmlNodeEmisor in xmlNode.ChildNodes)
                        {
                            if (xmlNodeEmisor.Name.ToLower().Contains("emisor"))
                            {
                                foreach (XmlAttribute attribute in xmlNodeEmisor.Attributes)
                                {
                                    if (attribute.Name.ToLower() == "rfc")
                                    {
                                        /*aqui se genera el rfc*/
                                        vendor.rfc = attribute.Value;
                                    }
                                    else if (attribute.Name.ToLower() == "nombre")
                                    {
                                        vendor.nombre = attribute.Value;
                                    }
                                }
                                // Direccion
                                foreach (XmlNode xmlNodeDireccion in xmlNodeEmisor.ChildNodes)
                                {
                                    if (xmlNodeDireccion.Name.ToLower().Contains("domicilio"))
                                    {
                                        foreach (XmlAttribute attribute in xmlNodeDireccion.Attributes)
                                        {
                                            if (attribute.Name.ToLower() == "calle")
                                            {
                                                vendor.calle = attribute.Value;
                                            }
                                            else if (attribute.Name.ToLower() == "noexterior")
                                            {
                                                vendor.noExterior = attribute.Value;
                                            }
                                            else if (attribute.Name.ToLower() == "nointerior")
                                            {
                                                vendor.noInterior = attribute.Value;
                                            }
                                            else if (attribute.Name.ToLower() == "colonia")
                                            {
                                                vendor.colonia = attribute.Value;
                                            }
                                            else if (attribute.Name.ToLower() == "municipio")
                                            {
                                                vendor.municipio = attribute.Value;
                                            }
                                            else if (attribute.Name.ToLower() == "estado")
                                            {
                                                vendor.estado = attribute.Value;
                                            }
                                            else if (attribute.Name.ToLower() == "pais")
                                            {
                                                vendor.pais = attribute.Value;
                                            }
                                            else if (attribute.Name.ToLower() == "codigopostal")
                                            {
                                                vendor.codigoPostal = attribute.Value;
                                            }
                                        }
                                    }
                                }
                            }
                            else if (xmlNodeEmisor.Name.ToLower().Contains("receptor"))
                            {
                                foreach (XmlAttribute attribute in xmlNodeEmisor.Attributes)
                                {
                                    /*aqui tambien se genera el rfc*/
                                    if (attribute.Name.ToLower() == "rfc")
                                    {
                                        client.rfc = attribute.Value;
                                    }
                                    else if (attribute.Name.ToLower() == "nombre")
                                    {
                                        client.nombre = attribute.Value;
                                    }
                                }
                            }



                            else if (xmlNodeEmisor.Name.ToLower().Contains("conceptos"))
                            {

                                if (vendor.rfc == mainRfc)
                                {
                                    bill.is_invoice = true;
                                }

                                // Concepto
                                foreach (XmlNode xmlNodeConcepto in xmlNodeEmisor.ChildNodes)
                                {
                                    if (xmlNodeConcepto.Name.ToLower().Contains("concepto"))
                                    {
                                        BillLineItem billLineItem = new BillLineItem();
                                        billLineItem.SetDefaults();
                                        billLineItem.is_invoice = bill.is_invoice;
                                        foreach (XmlAttribute attribute in xmlNodeConcepto.Attributes)
                                        {
                                            if (attribute.Name.ToLower() == "cantidad")
                                            {
                                                billLineItem.cantidad = Convert.ToDouble(attribute.Value);
                                            }
                                            else if (attribute.Name.ToLower() == "unidad")
                                            {
                                                billLineItem.unidad = attribute.Value;
                                            }
                                            else if (attribute.Name.ToLower() == "descripcion")
                                            {
                                                billLineItem.descripcion = attribute.Value;
                                            }
                                            else if (attribute.Name.ToLower() == "valorunitario")
                                            {
                                                billLineItem.valorUnitario = Convert.ToDouble(attribute.Value);
                                            }
                                            else if (attribute.Name.ToLower() == "importe")
                                            {
                                                billLineItem.importe = Convert.ToDouble(attribute.Value);
                                            }

                                            else if (attribute.Name.ToLower() == "claveprodserv")
                                            {
                                                billLineItem.claveprodserv = attribute.Value;
                                            }
                                            else if (attribute.Name.ToLower() == "claveunidad")
                                            {
                                                billLineItem.claveunidad = attribute.Value;
                                            }
                                        }


                                        foreach (XmlNode xmlNodeContribution in xmlNodeConcepto)
                                        {
                                            if (xmlNodeContribution.Name.ToLower().Contains("impuestos"))
                                            {
                                                foreach (XmlNode xmlNodeTaxes in xmlNodeContribution)
                                                {
                                                    
                                                    if (xmlNodeTaxes.Name.ToLower().Contains("traslados"))
                                                    {
                                                        
                                                        foreach (XmlNode xmlNodeTax in xmlNodeTaxes)
                                                        {
                                                             
                                                            LineItemContribution lineItemContribution = new LineItemContribution();
                                                            bool iva_present = false;
                                                            foreach (XmlAttribute xmlTaxAttribute in xmlNodeTax.Attributes)
                                                            {

                                                                //xmlNodeTax.Attributes.

                                                                if (xmlTaxAttribute.Name.ToLower() == "impuesto")
                                                                {
                                                                    lineItemContribution.name = xmlTaxAttribute.Value;
                                                                    
                                                                }

                                                                if (xmlTaxAttribute.Name.ToLower() == "tasaocuota")
                                                                {
                                                                    lineItemContribution.percentage = Double.Parse(xmlTaxAttribute.Value);

                                                                }

                                                                if (lineItemContribution.name == "002")
                                                                {
                                                                    iva_present = true;
                                                                    
                                                                }

                                                                
                                                            }

                                                            string taxSuffix = "";

                                                            if (iva_present)
                                                            {
                                                                if (String.IsNullOrEmpty(lineItemContribution.percentage.ToString())) //EXcento
                                                                {
                                                                    taxSuffix = "-IVA-Exc" ;
                                                                }
                                                                else //Some percentage
                                                                {
                                                                    if (lineItemContribution.percentage > 0)
                                                                    {
                                                                        billLineItem.createsTax = true;
                                                                    }
                                                                    taxSuffix = "-IVA-" + (lineItemContribution.percentage * 100).ToString();
                                                                }
                                                            }
                                                            else //Not provided
                                                            {
                                                                taxSuffix = "-IVA-Nul";
                                                            }

                                                            billLineItem.claveprodserv = billLineItem.claveprodserv + taxSuffix;
                                                            //billLineItem.contributions.Add(lineItemContribution);
                                                        }
                                                        
                                                    }
                                                    else if (xmlNodeTaxes.Name.ToLower().Contains("retenciones"))
                                                    {

                                                    }
                                                }

                                            }
                                        }

                                        bill.conceptos.Add(billLineItem);
                                    }
                                }
                                
                            }
                            else if (xmlNodeEmisor.Name.ToLower().Contains("impuestos"))
                            {
                                // Retenciones of Traslados
                                foreach (XmlNode xmlNodeRetencionesOTraslados in xmlNodeEmisor.ChildNodes)
                                {
                                    foreach (XmlNode xmlNodeImpuesto in xmlNodeRetencionesOTraslados.ChildNodes)
                                    {
                                        string taxType = "Unknown";
                                        double amount = 0.0;
                                        if (xmlNodeImpuesto.Name.ToLower().Contains("retencion") || xmlNodeImpuesto.Name.ToLower().Contains("traslado"))
                                        {
                                            if (xmlNodeImpuesto.Name.ToLower().Contains("retencion"))
                                            {
                                                taxType = "RETENCION_";
                                            }
                                            else
                                            {
                                                taxType = "TRASLADO_";
                                            }
                                            foreach (XmlAttribute attribute in xmlNodeImpuesto.Attributes)
                                            {
                                                if (attribute.Name.ToLower() == "impuesto")
                                                {

                                                    switch (attribute.Value)
                                                    {
                                                        case "001": //ISR
                                                            taxType += "ISR";
                                                            break;
                                                        case "002": //IVA
                                                            taxType += "IVA";
                                                            break;
                                                        case "003": //IEPS
                                                            taxType += "IEPS";
                                                            break;
                                                     }

                                                    //taxType += attribute.Value.ToUpper();
                                                }
                                                else if (attribute.Name.ToLower() == "importe")
                                                {
                                                    amount = Convert.ToDouble(attribute.Value);
                                                }
                                            }
                                        }
                                        switch (taxType)
                                        {
                                            case "RETENCION_IVA": bill.retencionesIVA = amount; break;
                                            case "RETENCION_ISR": bill.retencionesISR = amount; break;
                                            case "TRASLADO_IVA": bill.trasladosIVA = amount; break;
                                            case "TRASLADO_IEPS": bill.trasladosIEPS = amount; break;
                                        }
                                    }
                                }
                            }
                            else if (xmlNodeEmisor.Name.ToLower().Contains("complemento"))
                            {
                                // UUID
                                foreach (XmlNode xmlNodeComplemento in xmlNodeEmisor.ChildNodes)
                                {
                                    if (xmlNodeComplemento.Name.ToLower().Contains("timbrefiscal"))
                                    {
                                        foreach (XmlAttribute attribute in xmlNodeComplemento.Attributes)
                                        {
                                            if (attribute.Name.ToLower() == "uuid")
                                            {
                                                bill.uuid = attribute.Value.ToLower();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public Bill GetBill()
        {
            return bill;
        }

        public Vendor GetVendor()
        {
            return vendor;
        }

        public Client GetClient()
        {
            return client;
        }

        public List<BillLineItem> GetLineItems()
        {
            return bill.conceptos;
        }

    }
}
