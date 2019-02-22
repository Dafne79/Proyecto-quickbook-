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
    public partial class ParseResult : Form
    {
        QuickbooksQueries quickbooksQueries;

        public ParseResult()
        {
            InitializeComponent();
        }

        public void SetQuickbooksQueries(QuickbooksQueries quickbooksQueries)
        {
            this.quickbooksQueries = quickbooksQueries;
        }

        public void EnableLoadToQBButton(bool enable)
        {
            LoadToQuickbooksButton.Enabled = enable;
        }

        public void UpdateContent(bool showStatus)
        {
            parseTreeView.Nodes.Clear();
            TreeNode rootNode = new TreeNode("Principal");
            rootNode.ImageIndex = 2;
            parseTreeView.Nodes.Add(rootNode);
            rootNode.SelectedImageIndex = rootNode.ImageIndex;

            parseTreeView.ExpandAll();
        }

        private void ParseResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
