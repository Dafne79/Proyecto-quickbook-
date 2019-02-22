using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QB_App
{
    public partial class ImportResults : Form
    {
        public ImportResults()
        {
            InitializeComponent();
        }

        public void SetAcknowledge()
        {
            this.Text = "Resultado de Importacion";
            confirmAccountImportLabel.Text = "Este fue el resultado de importar las facturas:";
            cancelImport.Visible = false;
            confirmImport.Text = "Cerrar";
        }

        public void SetOption1Checkbox(string label, bool check)
        {
            option1Checkbox.Text = label;
            option1Checkbox.Visible = true;
            option1Checkbox.Checked = check;
        }

        public void SetOption2Checkbox(string label, bool check)
        {
            option2Checkbox.Text = label;
            option2Checkbox.Visible = true;
            option2Checkbox.Checked = check;
        }

        public bool GetOption1Checkbox()
        {
            return option1Checkbox.Checked;
        }

        public bool GetOption2Checkbox()
        {
            return option2Checkbox.Checked;
        }

        public void SetGrid(string[] headers, List<List<string>> values)
        {
            foreach (var header in headers)
                importGridView.Columns.Add(header, header);
            
            foreach (var row in values)
                importGridView.Rows.Add(row.ToArray());
        }

        public void SetWidths(int[] widths)
        {
            importGridView.ScrollBars = ScrollBars.Both;
            for (int i = 0; i < widths.Length; ++i)
                importGridView.Columns[i].Width = widths[i];
        }

        private void cancelImport_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void confirmImport_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
