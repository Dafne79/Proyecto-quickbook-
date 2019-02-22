using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using QB_App.BillProcessing;
using System.Diagnostics;

namespace QB_App
{
    public partial class MainForm : Form
    {
        QuickbooksQueries quickbooksQueries;
        ReportsResult reportsResult;
        bool quickbooksReady;
        bool enableBillImport;
        string mainRfc;
        public MainForm()
        {
            InitializeComponent();
            quickbooksReady = false;
            enableBillImport = false;
            startDateTimePicker.Value = new DateTime(2015, 6, 1);
        }

        private void startQuickbooks()
        {
            if (!quickbooksReady)
            {
                Cursor.Current = Cursors.WaitCursor;
                quickbooksQueries = new QuickbooksQueries();
                try
                {
                    string qbFilename = "";
                    if (!String.IsNullOrWhiteSpace(Properties.Settings.Default["QBLastFilePath"].ToString()))
                        qbFilename = Properties.Settings.Default["QBLastFilePath"].ToString();

                    if (quickbooksQueries.OpenConnection("QBGC Contabilidad Electronica", qbFilename))
                    {
                        quickbooksReady = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    reportsResult.Hide();
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void startProcessing()
        {
            startQuickbooks();

            if (quickbooksReady)
            {

                  
                Cursor.Current = Cursors.WaitCursor;
                // Creates the list with accounts
                quickbooksQueries.LoadTransactions2();
                string resultLoad = quickbooksQueries.LoadAccounts();

                // Load vendors
                resultLoad += quickbooksQueries.LoadVendors();
                // Load customers
                resultLoad += quickbooksQueries.LoadCustomers();
                // Load Items
                resultLoad += quickbooksQueries.LoadItems();
                // Load ITem FIELDS
                quickbooksQueries.LoadItemCustomFields();
                // Load Units
                resultLoad += quickbooksQueries.LoadUoms();
                // Creates the reports
                resultLoad += quickbooksQueries.LoadCompany();
                // Creates the reports
                resultLoad += quickbooksQueries.LoadCurrencies();
                // Creates the reports
                resultLoad += quickbooksQueries.LoadTaxes();

                mainRfc = quickbooksQueries.LoadCompany();
                resultLoad += quickbooksQueries.LoadBalance(startDateTimePicker.Value, endDateTimePicker.Value);
                resultLoad += quickbooksQueries.LoadProfitAndLoss(startDateTimePicker.Value, endDateTimePicker.Value);
                resultLoad += quickbooksQueries.LoadTransactions(startDateTimePicker.Value, endDateTimePicker.Value);

                /*if (!String.IsNullOrWhiteSpace(resultLoad))
                {
                    MessageBox.Show(resultLoad);
                }
                else*/
                {
                    reportsResult.Accounts = quickbooksQueries.Accounts;
                    reportsResult.Vendors = quickbooksQueries.Vendors;
                    reportsResult.startDate = startDateTimePicker.Value;
                    reportsResult.endDate = endDateTimePicker.Value;
                    reportsResult.company = quickbooksQueries.company;
                    reportsResult.Clients = quickbooksQueries.Clients;
                    reportsResult.Items = quickbooksQueries.Items;
                    reportsResult.Uoms = quickbooksQueries.Uoms;
                    reportsResult.Currencies = quickbooksQueries.Currencies;
                    reportsResult.Taxes = quickbooksQueries.Taxes;
                    reportsResult.DisplayAccounts();
                    reportsResult.DisplayTransactions();
                    reportsResult.DisplayVendors();
                    reportsResult.DisplayClients();
                    reportsResult.DisplayItems();
                    reportsResult.DisplayUoms();
                    reportsResult.DisplayCurrencies();
                    reportsResult.DisplayTaxes();
                    // Show the report
                    reportsResult.Show();
                    reportsResult.Location = new Point(this.Location.X + this.Size.Width, this.Location.Y);
                    tryEnableBillImport();
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void tryEnableBillImport()
        {
            string results = VendorAdder.ValidateAccounts(ref quickbooksQueries.Accounts);
            if (String.IsNullOrWhiteSpace(results))
            {

                initialPanel.Visible = false;
            }
            else
            {
                initialPanel.BackColor = Color.LightCoral;
                importAccountsMissingLabel.Text = "Faltan cuentas: " + results;
                importAccountsMissingLabel.Visible = true;
                importHelpLabel.Visible = false;
            }
        }

        private void startBillImport(string[] files)
        {
            List<BillParser> billParsers = new List<BillParser>();
            string[] headers = { "RFC", "Nombre", "Total", "Descuentos", "Retenciones IVA", "Retenciones ISR", "Traslados IVA", "Traslados IEPS", "Status Proveedor", "Status Factura", "Tipo", "UUID", "Numero" };
            List<List<string>> values = new List<List<string>>();
            int fileIndex = 0;
            foreach (var filename in files)
            {
                ++fileIndex;
                BillParser billParser = new BillParser();
                billParser.Load(filename);
                if (billParser.Parse(mainRfc))
                {
                    Vendor vendor = billParser.GetVendor();
                    Client client = billParser.GetClient();

                    string not_invoice_error_msg = "";
                    if (mainRfc != vendor.rfc && mainRfc != client.rfc)
                    {
                    not_invoice_error_msg = "El RFC del emisor: " + vendor.rfc + " y el RFC del receptor: " + client.rfc + " no corresponden con el RFC de QB: " + mainRfc;
                    }
                    string vendor_or_client_rfc = "Error!";//Load this in a single var
                    string vendor_or_client_name = "Error!";
                    string invoice_or_bill = "Error!";
                    billParsers.Add(billParser);
                    Bill bill = billParser.GetBill();
                    List<string> gridRow = new List<string>();


                    if (bill.tipoComprobante == "n")
                    {
                        invoice_or_bill = "NOMINA!";
                        vendor_or_client_rfc = "NOMINA!";
                        vendor_or_client_name = "NOMINA!";
                        not_invoice_error_msg = "NOMINA!";
                    }
                    else
                    {
                        if (vendor.rfc == mainRfc)
                        {
                            invoice_or_bill = "Invoice";
                            vendor_or_client_rfc = client.rfc;
                            vendor_or_client_name = client.nombre;
                        }
                        else if (client.rfc == mainRfc)
                        {
                            invoice_or_bill = "Bill";
                            vendor_or_client_rfc = vendor.rfc;
                            vendor_or_client_name = vendor.nombre;
                        }
                    }

                    gridRow.Add(vendor_or_client_rfc);
                    gridRow.Add(vendor_or_client_name);
                    gridRow.Add(bill.total.ToString("C2"));
                    gridRow.Add(bill.descuento.ToString("C2"));
                    gridRow.Add(bill.retencionesIVA.ToString("C2"));
                    gridRow.Add(bill.retencionesISR.ToString("C2"));
                    gridRow.Add(bill.trasladosIVA.ToString("C2"));
                    gridRow.Add(bill.trasladosIEPS.ToString("C2"));
                    gridRow.Add("");
                    gridRow.Add(not_invoice_error_msg);
                    gridRow.Add(invoice_or_bill);
                    gridRow.Add(bill.uuid);
                    gridRow.Add(fileIndex.ToString());
                    values.Add(gridRow);


                }
            }
            int[] widths = {100, 240, 70, 60, 60, 60, 60, 60, 100, 100};
            ImportResults importResults = new ImportResults();
            importResults.StartPosition = FormStartPosition.Manual;
            importResults.Location = new Point(this.Location.X + this.Size.Width, this.Location.Y + 20);
            importResults.SetGrid(headers, values);
            importResults.SetWidths(widths);
            importResults.SetOption1Checkbox("Sólo importar CATALOGOS", false);
            importResults.SetOption2Checkbox("Modo SIMULACIÓN", false);
            var results = importResults.ShowDialog();

            if (results == DialogResult.OK)
            {

                pBar pbar = new pBar();
                pbar.StartPosition = FormStartPosition.Manual;
                pbar.Location = new Point(this.Location.X + this.Size.Width, this.Location.Y + 20);
                //pbar.ShowDialog();
                pbar.Show();
                ProgressBar pbar1 = pbar.progressBar1;

                pbar1.Minimum = 0;
                pbar1.Maximum = billParsers.Count();

                int billIndex = -1;
                foreach (BillParser billParser in billParsers)
                {
                    ++billIndex;
                    pbar1.Value = billIndex;
                    //Importar Clientes/Provedores
                    Vendor vendor = billParser.GetVendor();
                    Client client = billParser.GetClient();
                    List<BillLineItem> lineItems = billParser.GetLineItems();
                    Bill bill = billParser.GetBill();
                    bool is_invoice = (mainRfc == vendor.rfc);
                    bool is_bill    = (mainRfc == client.rfc);
                    bool is_payroll = bill.tipoComprobante == "n";
                    if ((is_invoice || is_bill) && !is_payroll)
                    {
                        if (is_invoice)
                        {
                            // Crear Client, si no existe
                            /*aqui puede ser */
                            /*Genera una cadena agregando el nombre y dlls*/
                            /*if (client.rfc == "XEXX010101000")*/

                            if (bill.moneda != "mxn")
                            {
                                client.nombre = client.nombre + "-DLLS";
                                var splited = client.nombre.ToUpper().Split('-')[0];
                                client.rfc = client.rfc + "-DLLS-" + splited;
                            } else {

                                client.rfc = client.rfc + "-" + client.nombre;

                            }





                            if (!quickbooksQueries.Clients.ContainsKey(client.rfc))
                            {
                                ClientAdder customerAdder = new ClientAdder();
                                if (!importResults.GetOption2Checkbox())
                                {
                                    string customerAdderResult = customerAdder.InsertClient(ref quickbooksQueries, ref client,1,bill.moneda);
                                    if (String.IsNullOrWhiteSpace(customerAdderResult))
                                    {
                                        Console.Write(values.ElementAt(billIndex));
                                        values.ElementAt(billIndex)[8] = "Creado"; // Add validation ElemenaAt(billIndex) not null
                                        quickbooksQueries.Clients[client.rfc] = client;
                                    }
                                    else
                                    {
                                        values.ElementAt(billIndex)[8] = customerAdderResult;
                                        continue;
                                    }
                                }
                                else
                                {
                                    values.ElementAt(billIndex)[8] = "Crear";
                                }
                            }
                            else
                            {
                                Debug.Write(values.ElementAt(billIndex));
                                values.ElementAt(billIndex)[8] = "Existente";
                            }
                        }
                        else if (is_bill)
                        {
                            /*aqui puede ser */
                            if (bill.moneda != "mxn")
                            {
                               vendor.nombre = vendor.nombre + "-DLLS";
                                var splited = vendor.nombre.ToUpper().Split('-')[0];
                                vendor.rfc = vendor.rfc + "-DLLS-" + splited;
                            } else
                            {

                                client.rfc = client.rfc + "-" + client.nombre;

                            }

                            // Crear Vendor, si no existe
                            if (!quickbooksQueries.Vendors.ContainsKey(vendor.rfc))
                            {
                                VendorAdder vendorAdder = new VendorAdder();
                                if (!importResults.GetOption2Checkbox())
                                {
                                    string vendorAdderResult = vendorAdder.InsertVendor(ref quickbooksQueries, ref vendor);
                                    if (String.IsNullOrWhiteSpace(vendorAdderResult))
                                    {
                                        Console.Write(values.ElementAt(billIndex));
                                        values.ElementAt(billIndex)[8] = "Creado"; // Add validation ElemenaAt(billIndex) not null
                                        quickbooksQueries.Vendors[vendor.rfc] = vendor;
                                    }
                                    else
                                    {
                                        values.ElementAt(billIndex)[8] = vendorAdderResult;
                                        continue;
                                    }
                                }
                                else
                                {
                                    values.ElementAt(billIndex)[8] = "Crear";
                                }
                            }
                            else
                            {
                                Debug.Write(values.ElementAt(billIndex));
                                values.ElementAt(billIndex)[8] = "Existente";
                            }
                        }

                        //Loop Items
                        List<bool> created_items = new List<bool>();
                        foreach (BillLineItem item in lineItems)
                        {
                            // Crear Item, si no existe
                            BillLineItem new_item = item;

                            string item_msg = "Factura " + bill.uuid + " " + item.claveprodserv + " => " + item.unidad + " " + item.descripcion;
                            string unit_msg = "Factura " + bill.uuid + " " + item.claveunidad + " => " + item.unidad + " " + item.descripcion;

                            //foreach (LineItemContribution item_contribution in item.contributions)
                            //{
                            //    string perc_string = (item_contribution.percentage * 100).ToString();
                            //    string claveProd = item.descripcion + "-" + item_contribution.name + "-" + perc_string;
                            //}

                            if (!quickbooksQueries.Uoms.ContainsKey(item.claveunidad))
                            {
                                System.Diagnostics.Trace.WriteLine("UNIT NO existe " + unit_msg);
                                UomAdder uomAdder = new UomAdder();
                                if (!importResults.GetOption2Checkbox())
                                {
                                    string itemAdderResult = uomAdder.InsertUom(ref quickbooksQueries, ref new_item);
                                    if (String.IsNullOrWhiteSpace(itemAdderResult))
                                    {
                                        System.Diagnostics.Trace.WriteLine("UNIT creado exitosamente");
                                    }
                                    else
                                    {
                                        System.Diagnostics.Trace.WriteLine("UNIT no puedo ser creado :/");
                                    }

                                }
                            } else
                            {
                                System.Diagnostics.Trace.WriteLine("UNIT existe " + unit_msg);
                            }

                            if (!quickbooksQueries.Items.ContainsKey(item.claveprodserv))
                            {
                                //string new_msg =
                                System.Diagnostics.Trace.WriteLine("Item NO existe " + item_msg);
                                ItemAdder itemAdder = new ItemAdder();
                                if (!importResults.GetOption2Checkbox())
                                {
                                    string itemAdderResult = itemAdder.InsertItem(ref quickbooksQueries, ref new_item);
                                    if (String.IsNullOrWhiteSpace(itemAdderResult))
                                    {
                                        //Insert claveprodserv in customField
                                        DataExtAdder dataExtAdder = new DataExtAdder();
                                        string itemListID = quickbooksQueries.Items[item.claveprodserv].listId;
                                        string dataExtAdderResult = dataExtAdder.insertData(ref quickbooksQueries, itemListID, item.claveprodserv);

                                        if (String.IsNullOrWhiteSpace(dataExtAdderResult))
                                        {
                                            string str_dataAdderms = "some msg";
                                        }

                                    }
                                    else
                                    {
                                        created_items.Add(false);
                                        //values.ElementAt(billIndex)[8] = vendorAdderResult;
                                        //continue;
                                    }
                                }
                                else
                                {
                                    //values.ElementAt(billIndex)[8] = "Crear";
                                }
                            }
                            else
                            {
                                System.Diagnostics.Trace.WriteLine("Item EXISTE " + item_msg);
                            }


                        }

                        if (!importResults.GetOption1Checkbox() && !importResults.GetOption2Checkbox() && !created_items.Contains(false))
                        {
                            if (is_invoice) {



                            }


                            if (is_invoice)
                            {
                                InvoiceAdder billAdder = new InvoiceAdder(ref quickbooksQueries.Accounts, quickbooksQueries.Clients[client.rfc]);
                                string billAdderResult = billAdder.InsertInvoice(ref quickbooksQueries, bill);
                                if (String.IsNullOrWhiteSpace(billAdderResult))
                                {
                                    values.ElementAt(billIndex)[9] = "Creada";
                                }
                                else
                                {
                                    values.ElementAt(billIndex)[9] = billAdderResult;
                                }

                            }
                            else if (is_bill)
                            {
                                // Crear el Bill
                                BillAdder billAdder = new BillAdder(ref quickbooksQueries.Accounts, quickbooksQueries.Vendors[vendor.rfc]);
                                string billAdderResult = billAdder.InsertBill(ref quickbooksQueries, bill);
                                if (String.IsNullOrWhiteSpace(billAdderResult))
                                {
                                    values.ElementAt(billIndex)[9] = "Creada";
                                }
                                else
                                {
                                    values.ElementAt(billIndex)[9] = billAdderResult;
                                }
                            }
                        }
                        else
                        {
                            values.ElementAt(billIndex)[9] = "Ignorada";
                        }
                    }
                }
                pbar.Close();
                // Resultados de la importacion
                importResults = new ImportResults();
                importResults.StartPosition = FormStartPosition.Manual;
                importResults.Location = new Point(this.Location.X + this.Size.Width, this.Location.Y + 20);
                importResults.SetAcknowledge();
                importResults.SetGrid(headers, values);
                importResults.SetWidths(widths);
                results = importResults.ShowDialog();

                // Recargar vendors al final
                startProcessing();
            }
        }

        private void dropPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void dropPanel_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            startBillImport(files);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(Properties.Settings.Default["QBLastFilePath"].ToString()))
            {
                generateXMLFiles.Enabled = true;
                labelFileToUse.Text = Properties.Settings.Default["QBLastFilePath"].ToString();
            }
            reportsResult = new ReportsResult();
            reportsResult.Hide();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (quickbooksQueries != null)
                quickbooksQueries.CloseConnection();
        }

        private void chooseQBFileButton_Click_1(object sender, EventArgs e)
        {
            DialogResult result = chooseQBFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Properties.Settings.Default["QBLastFilePath"] = chooseQBFileDialog.FileName;
                Properties.Settings.Default.Save();
                labelFileToUse.Text = chooseQBFileDialog.FileName;
                generateXMLFiles.Enabled = true;
            }
        }

        private void chooseTXTLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult result = chooseTXTFiles.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (chooseTXTFiles.FileNames.Length > 0)
                {
                    startBillImport(chooseTXTFiles.FileNames);
                }
            }
        }

        private void generateXMLFiles_Click(object sender, EventArgs e)
        {
            startProcessing();
        }

        private void startDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime endDate = startDateTimePicker.Value;
            endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));
            endDateTimePicker.Value = endDate;
        }

        private void importAccounts_Click(object sender, EventArgs e)
        {
            List<List<string>> labels = new List<List<string>>();
            DialogResult result = chooseAccountCSV.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (File.Exists(chooseAccountCSV.FileName))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Csv.ICsvLine[] lines = Csv.CsvReader.ReadFromText(File.ReadAllText(chooseAccountCSV.FileName, Encoding.Default),
                            new Csv.CsvOptions {
                                Separator = ',',
                                HeaderMode = Csv.HeaderMode.HeaderPresent
                            }).ToArray();
                        if (lines.Length == 0)
                        {
                            MessageBox.Show("El archivo o esta vacío o no se pudo leer.");
                            return;
                        }

                        List<List<string>> data = new List<List<string>>();
                        foreach (var row in lines)
                        {
                            if (row.ColumnCount < 6) continue;
                            List<string> rowData = new List<string>();
                            for (int i = 0; i < row.ColumnCount; ++i)
                                rowData.Add(row[i]);
                            data.Add(rowData);
                        }

                        ImportResults importResults = new ImportResults();
                        importResults.SetGrid(lines[0].Headers, data);
                        importResults.Hide();
                        result = importResults.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            startQuickbooks();

                            if (quickbooksReady)
                            {
                                int totalAccountsImported;
                                string queryResult = quickbooksQueries.UploadAccounts(data, out totalAccountsImported);
                                if (!String.IsNullOrWhiteSpace(queryResult))
                                {
                                    MessageBox.Show(queryResult);
                                }
                                else
                                {
                                    MessageBox.Show("Importación de " + totalAccountsImported + " cuentas completa");
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                        return;
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
        }
    }
}
