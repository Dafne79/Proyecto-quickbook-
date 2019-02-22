using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace QB_App
{
    public partial class ReportsResult : Form
    {
        public Dictionary<string, Account> Accounts { get; set; }
        public Dictionary<string, Vendor> Vendors { get; set; }
        public Dictionary<string, Client> Clients { get; set; }
        public Dictionary<string, Item> Items { get; set; }
        public Dictionary<string, UnitMeasure> Uoms { get; set; }
        public Dictionary<string, Currency> Currencies { get; set; }
        public Dictionary<string, Tax> Taxes { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public Company company { get; set; }

        public ReportsResult()
        {
            InitializeComponent();
        }

        public void DisplayAccounts()
        {
            accountsGridView.Rows.Clear();
            foreach (KeyValuePair<string, Account> accountPair in Accounts)
            {
                CatalogoCtas cuenta = accountPair.Value.satAccount;
                string parentAccount;
                if (cuenta.SubCtaDe != null)
                    parentAccount = cuenta.SubCtaDe;
                else
                    parentAccount = "";
                accountsGridView.Rows.Add(cuenta.Desc, 
                    accountPair.Value.GetNature(),
                    accountPair.Value.type, 
                    cuenta.NumCta, 
                    parentAccount, 
                    cuenta.Nivel, 
                    cuenta.CodAgrup,
                    accountPair.Value.listId);
            }
        }

        public void DisplayTransactions()
        {
            trialsGridView.Rows.Clear();
            foreach (KeyValuePair<string, Account> accountPair in Accounts)
            {
                trialsGridView.Rows.Add(accountPair.Key,
                    String.Format("{0:C}", accountPair.Value.initialAmount),
                    String.Format("{0:C}", accountPair.Value.debit),
                    String.Format("{0:C}", accountPair.Value.credit),
                    String.Format("{0:C}", accountPair.Value.finalAmount));
            }
        }

        public void DisplayVendors()
        {
            vendorsGridView.Rows.Clear();
            foreach (KeyValuePair<string, Vendor> vendorPair in Vendors)
            {
                Vendor vendor = vendorPair.Value;
                vendorsGridView.Rows.Add(vendor.rfc, vendor.nombre);
            }
        }

        public void DisplayClients()
        {
            clientsGridView.Rows.Clear();
            foreach (KeyValuePair<string, Client> clientPair in Clients)
            {
                Client client = clientPair.Value;
                clientsGridView.Rows.Add(client.rfc, client.nombre);
            }
        }

        public void DisplayItems()
        {
            itemsGridView.Rows.Clear();
            int indexer = 0; 
            foreach (KeyValuePair<string, Item> itemPair in Items)
            {
                ++indexer;
                Item item = itemPair.Value;
                itemsGridView.Rows.Add(item.listId, item.fullName, item.name, item.productKey, item.uomFullName, item.uomListId, indexer);
            }
        }

        public void DisplayUoms()
        {
            uomsGridView.Rows.Clear();
            foreach (KeyValuePair<string, UnitMeasure> itemPair in Uoms)
            {
                UnitMeasure item = itemPair.Value;
                uomsGridView.Rows.Add(item.listId, item.name, item.baseUnitName, item.abbreviation);
            }
        }

        public void DisplayCurrencies()
        {
            dataGridView1.Rows.Clear();
            foreach (KeyValuePair<string, Currency> itemPair in Currencies)
            {
                Currency item = itemPair.Value;
                dataGridView1.Rows.Add(item.name, item.listId);
            }
        }

        public void DisplayTaxes()
        {
            dataGridView1.Rows.Clear();
            foreach (KeyValuePair<string, Tax> itemPair in Taxes)
            {
                Tax item = itemPair.Value;
                dataGridView2.Rows.Add(item.name, item.listId);
            }
        }

        private void ReportsResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void SaveAccountsXML(string filename)
        {
            Catalogo catalogo = new Catalogo();
            catalogo.Version = "1.1";
            catalogo.Mes = startDate.ToString("MM");
            catalogo.Anio = startDate.Year;
            catalogo.RFC = company.rfc;
            foreach (KeyValuePair<string, Account> accountPair in Accounts)
            {
                catalogo.Ctas.Add(accountPair.Value.satAccount);
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(catalogo.Serialize());
            XmlAttribute xsi_schema_location = xmlDocument.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xsi_schema_location.Value = @"www.sat.gob.mx/esquemas/ContabilidadE/1_1/CatalogoCuentas http://www.sat.gob.mx/esquemas/ContabilidadE/1_1/CatalogoCuentas/CatalogoCuentas_1_1.xsd";
            xmlDocument.LastChild.Attributes.SetNamedItem(xsi_schema_location);
            xmlDocument.Save(filename);
        }

        private void getXMLAccountsButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveAccountsDialog = new SaveFileDialog();
            saveAccountsDialog.FileName = company.rfc + startDate.ToString("yyyyMM") + "CT.xml";
            saveAccountsDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            saveAccountsDialog.RestoreDirectory = true;
            if (saveAccountsDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = saveAccountsDialog.FileName;
                SaveAccountsXML(filename);
            }
        }

        private void SaveBalanceXML(string filename)
        {
            Balanza balanza = new Balanza();
            balanza.Version = "1.1";
            balanza.RFC = company.rfc;
            balanza.Mes = startDate.ToString("MM");
            balanza.Anio = startDate.Year;
            balanza.TipoEnvio = "N";
            foreach (KeyValuePair<string, Account> accountPair in Accounts)
            {
                BalanzaCtas balanzaCuenta = new BalanzaCtas();
                Account account = accountPair.Value;
                balanzaCuenta.NumCta = account.satAccount.NumCta;
                balanzaCuenta.Debe = Decimal.Round(Convert.ToDecimal(account.debit), 2, MidpointRounding.AwayFromZero);
                balanzaCuenta.Haber = Decimal.Round(Convert.ToDecimal(account.credit), 2, MidpointRounding.AwayFromZero);
                balanzaCuenta.SaldoIni = Decimal.Round(Convert.ToDecimal(account.initialAmount), 2, MidpointRounding.AwayFromZero);
                balanzaCuenta.SaldoFin = Decimal.Round(Convert.ToDecimal(account.finalAmount), 2, MidpointRounding.AwayFromZero);
                balanza.Ctas.Add(balanzaCuenta);
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(balanza.Serialize());
            XmlAttribute xsi_schema_location = xmlDocument.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xsi_schema_location.Value = @"www.sat.gob.mx/esquemas/ContabilidadE/1_1/BalanzaComprobacion http://www.sat.gob.mx/esquemas/ContabilidadE/1_1/BalanzaComprobacion/BalanzaComprobacion_1_1.xsd";
            xmlDocument.LastChild.Attributes.SetNamedItem(xsi_schema_location);
            xmlDocument.Save(filename);
        }

        private void getXMLBalancesButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveAccountsDialog = new SaveFileDialog();
            saveAccountsDialog.FileName = company.rfc + startDate.ToString("yyyyMM") + "BN.xml";
            saveAccountsDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            saveAccountsDialog.RestoreDirectory = true;
            if (saveAccountsDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = saveAccountsDialog.FileName;
                SaveBalanceXML(filename);
            }
        }

        private void uomsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
