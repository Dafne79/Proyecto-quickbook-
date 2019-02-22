namespace QB_App
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.chooseQBFileButton = new System.Windows.Forms.Button();
            this.labelFileToUse = new System.Windows.Forms.Label();
            this.chooseTXTFiles = new System.Windows.Forms.OpenFileDialog();
            this.chooseQBFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.generateXMLFiles = new System.Windows.Forms.Button();
            this.bdLabel = new System.Windows.Forms.Label();
            this.endDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.startDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.toLabel = new System.Windows.Forms.Label();
            this.importAccounts = new System.Windows.Forms.Button();
            this.chooseAccountCSV = new System.Windows.Forms.OpenFileDialog();
            this.initialPanel = new System.Windows.Forms.Panel();
            this.importAccountsMissingLabel = new System.Windows.Forms.Label();
            this.importHelpLabel = new System.Windows.Forms.Label();
            this.dropPanel = new System.Windows.Forms.Panel();
            this.importBillsLabel = new System.Windows.Forms.Label();
            this.chooseTXTLinkLabel = new System.Windows.Forms.LinkLabel();
            this.initialPanel.SuspendLayout();
            this.dropPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // chooseQBFileButton
            // 
            this.chooseQBFileButton.Location = new System.Drawing.Point(356, 9);
            this.chooseQBFileButton.Name = "chooseQBFileButton";
            this.chooseQBFileButton.Size = new System.Drawing.Size(47, 32);
            this.chooseQBFileButton.TabIndex = 0;
            this.chooseQBFileButton.Text = "...";
            this.chooseQBFileButton.UseVisualStyleBackColor = true;
            this.chooseQBFileButton.Click += new System.EventHandler(this.chooseQBFileButton_Click_1);
            // 
            // labelFileToUse
            // 
            this.labelFileToUse.AutoEllipsis = true;
            this.labelFileToUse.Location = new System.Drawing.Point(129, 9);
            this.labelFileToUse.Name = "labelFileToUse";
            this.labelFileToUse.Size = new System.Drawing.Size(221, 29);
            this.labelFileToUse.TabIndex = 2;
            this.labelFileToUse.Text = "Usa el boton a la derecha";
            this.labelFileToUse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chooseTXTFiles
            // 
            this.chooseTXTFiles.Filter = "Archivo XML|*.xml";
            this.chooseTXTFiles.Multiselect = true;
            // 
            // chooseQBFileDialog
            // 
            this.chooseQBFileDialog.Filter = "Archivos de Quickbooks|*.qbw";
            this.chooseQBFileDialog.ValidateNames = false;
            // 
            // generateXMLFiles
            // 
            this.generateXMLFiles.Enabled = false;
            this.generateXMLFiles.Location = new System.Drawing.Point(203, 73);
            this.generateXMLFiles.Name = "generateXMLFiles";
            this.generateXMLFiles.Size = new System.Drawing.Size(203, 28);
            this.generateXMLFiles.TabIndex = 5;
            this.generateXMLFiles.Text = "Descargar de QB";
            this.generateXMLFiles.UseVisualStyleBackColor = false;
            this.generateXMLFiles.Click += new System.EventHandler(this.generateXMLFiles_Click);
            // 
            // bdLabel
            // 
            this.bdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bdLabel.Location = new System.Drawing.Point(12, 9);
            this.bdLabel.Name = "bdLabel";
            this.bdLabel.Size = new System.Drawing.Size(101, 29);
            this.bdLabel.TabIndex = 6;
            this.bdLabel.Text = "Base de datos:";
            this.bdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // endDateTimePicker
            // 
            this.endDateTimePicker.CustomFormat = "dd/MMM/yyyy";
            this.endDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDateTimePicker.Location = new System.Drawing.Point(283, 47);
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.Size = new System.Drawing.Size(120, 20);
            this.endDateTimePicker.TabIndex = 7;
            // 
            // startDateLabel
            // 
            this.startDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startDateLabel.Location = new System.Drawing.Point(12, 45);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(101, 22);
            this.startDateLabel.TabIndex = 9;
            this.startDateLabel.Text = "Periodo:";
            this.startDateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.CustomFormat = "dd/MMM/yyyy";
            this.startDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDateTimePicker.Location = new System.Drawing.Point(138, 47);
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new System.Drawing.Size(120, 20);
            this.startDateTimePicker.TabIndex = 11;
            this.startDateTimePicker.ValueChanged += new System.EventHandler(this.startDateTimePicker_ValueChanged);
            // 
            // toLabel
            // 
            this.toLabel.AutoSize = true;
            this.toLabel.Location = new System.Drawing.Point(262, 50);
            this.toLabel.Name = "toLabel";
            this.toLabel.Size = new System.Drawing.Size(15, 13);
            this.toLabel.TabIndex = 12;
            this.toLabel.Text = "al";
            // 
            // importAccounts
            // 
            this.importAccounts.Location = new System.Drawing.Point(15, 73);
            this.importAccounts.Name = "importAccounts";
            this.importAccounts.Size = new System.Drawing.Size(98, 28);
            this.importAccounts.TabIndex = 13;
            this.importAccounts.Text = "Importar Cuentas";
            this.importAccounts.UseVisualStyleBackColor = true;
            this.importAccounts.Click += new System.EventHandler(this.importAccounts_Click);
            // 
            // chooseAccountCSV
            // 
            this.chooseAccountCSV.Filter = "Archivo CSV|*.csv";
            // 
            // initialPanel
            // 
            this.initialPanel.BackColor = System.Drawing.Color.White;
            this.initialPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("initialPanel.BackgroundImage")));
            this.initialPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.initialPanel.Controls.Add(this.importAccountsMissingLabel);
            this.initialPanel.Controls.Add(this.importHelpLabel);
            this.initialPanel.Location = new System.Drawing.Point(15, 107);
            this.initialPanel.Name = "initialPanel";
            this.initialPanel.Size = new System.Drawing.Size(391, 248);
            this.initialPanel.TabIndex = 6;
            // 
            // importAccountsMissingLabel
            // 
            this.importAccountsMissingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importAccountsMissingLabel.BackColor = System.Drawing.Color.Transparent;
            this.importAccountsMissingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importAccountsMissingLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.importAccountsMissingLabel.Location = new System.Drawing.Point(58, 139);
            this.importAccountsMissingLabel.Name = "importAccountsMissingLabel";
            this.importAccountsMissingLabel.Size = new System.Drawing.Size(288, 74);
            this.importAccountsMissingLabel.TabIndex = 7;
            this.importAccountsMissingLabel.Text = "Missing";
            this.importAccountsMissingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.importAccountsMissingLabel.Visible = false;
            // 
            // importHelpLabel
            // 
            this.importHelpLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importHelpLabel.AutoSize = true;
            this.importHelpLabel.BackColor = System.Drawing.Color.Transparent;
            this.importHelpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importHelpLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.importHelpLabel.Location = new System.Drawing.Point(55, 181);
            this.importHelpLabel.Name = "importHelpLabel";
            this.importHelpLabel.Size = new System.Drawing.Size(291, 17);
            this.importHelpLabel.TabIndex = 6;
            this.importHelpLabel.Text = "Descargar de QB antes de arrastrar facturas";
            this.importHelpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dropPanel
            // 
            this.dropPanel.AllowDrop = true;
            this.dropPanel.BackColor = System.Drawing.Color.White;
            this.dropPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("dropPanel.BackgroundImage")));
            this.dropPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dropPanel.Controls.Add(this.importBillsLabel);
            this.dropPanel.Controls.Add(this.chooseTXTLinkLabel);
            this.dropPanel.Location = new System.Drawing.Point(15, 107);
            this.dropPanel.Name = "dropPanel";
            this.dropPanel.Size = new System.Drawing.Size(391, 248);
            this.dropPanel.TabIndex = 4;
            this.dropPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropPanel_DragDrop);
            this.dropPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.dropPanel_DragEnter);
            // 
            // importBillsLabel
            // 
            this.importBillsLabel.AutoSize = true;
            this.importBillsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importBillsLabel.Location = new System.Drawing.Point(154, 32);
            this.importBillsLabel.Name = "importBillsLabel";
            this.importBillsLabel.Size = new System.Drawing.Size(89, 17);
            this.importBillsLabel.TabIndex = 5;
            this.importBillsLabel.Text = "Importar Bills";
            // 
            // chooseTXTLinkLabel
            // 
            this.chooseTXTLinkLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chooseTXTLinkLabel.AutoSize = true;
            this.chooseTXTLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseTXTLinkLabel.Location = new System.Drawing.Point(111, 139);
            this.chooseTXTLinkLabel.Name = "chooseTXTLinkLabel";
            this.chooseTXTLinkLabel.Size = new System.Drawing.Size(175, 45);
            this.chooseTXTLinkLabel.TabIndex = 4;
            this.chooseTXTLinkLabel.TabStop = true;
            this.chooseTXTLinkLabel.Text = "Arrastra aquí los archivos XML \r\na importar o da clic aquí \r\npara seleccionarlos";
            this.chooseTXTLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chooseTXTLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.chooseTXTLinkLabel_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 370);
            this.Controls.Add(this.initialPanel);
            this.Controls.Add(this.importAccounts);
            this.Controls.Add(this.toLabel);
            this.Controls.Add(this.endDateTimePicker);
            this.Controls.Add(this.chooseQBFileButton);
            this.Controls.Add(this.labelFileToUse);
            this.Controls.Add(this.bdLabel);
            this.Controls.Add(this.startDateLabel);
            this.Controls.Add(this.generateXMLFiles);
            this.Controls.Add(this.dropPanel);
            this.Controls.Add(this.startDateTimePicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "QBGC Contabilidad Electrónica";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.initialPanel.ResumeLayout(false);
            this.initialPanel.PerformLayout();
            this.dropPanel.ResumeLayout(false);
            this.dropPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button chooseQBFileButton;
        private System.Windows.Forms.Label labelFileToUse;
        private System.Windows.Forms.Panel dropPanel;
        private System.Windows.Forms.LinkLabel chooseTXTLinkLabel;
        private System.Windows.Forms.OpenFileDialog chooseTXTFiles;
        private System.Windows.Forms.OpenFileDialog chooseQBFileDialog;
        private System.Windows.Forms.Button generateXMLFiles;
        private System.Windows.Forms.Label bdLabel;
        private System.Windows.Forms.DateTimePicker endDateTimePicker;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.DateTimePicker startDateTimePicker;
        private System.Windows.Forms.Label toLabel;
        private System.Windows.Forms.Button importAccounts;
        private System.Windows.Forms.OpenFileDialog chooseAccountCSV;
        private System.Windows.Forms.Label importBillsLabel;
        private System.Windows.Forms.Label importHelpLabel;
        private System.Windows.Forms.Panel initialPanel;
        private System.Windows.Forms.Label importAccountsMissingLabel;

    }
}

