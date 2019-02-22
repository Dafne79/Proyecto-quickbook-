namespace QB_App
{
    partial class ImportResults
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportResults));
            this.confirmAccountImportLabel = new System.Windows.Forms.Label();
            this.importGridView = new System.Windows.Forms.DataGridView();
            this.confirmImport = new System.Windows.Forms.Button();
            this.cancelImport = new System.Windows.Forms.Button();
            this.option1Checkbox = new System.Windows.Forms.CheckBox();
            this.option2Checkbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.importGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // confirmAccountImportLabel
            // 
            this.confirmAccountImportLabel.AutoSize = true;
            this.confirmAccountImportLabel.Location = new System.Drawing.Point(12, 9);
            this.confirmAccountImportLabel.Name = "confirmAccountImportLabel";
            this.confirmAccountImportLabel.Size = new System.Drawing.Size(268, 13);
            this.confirmAccountImportLabel.TabIndex = 0;
            this.confirmAccountImportLabel.Text = "Si los datos abajo de ven bien, aprieta el botón importar";
            // 
            // importGridView
            // 
            this.importGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.importGridView.Location = new System.Drawing.Point(12, 40);
            this.importGridView.Name = "importGridView";
            this.importGridView.Size = new System.Drawing.Size(993, 424);
            this.importGridView.TabIndex = 1;
            // 
            // confirmImport
            // 
            this.confirmImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.confirmImport.Location = new System.Drawing.Point(836, 470);
            this.confirmImport.Name = "confirmImport";
            this.confirmImport.Size = new System.Drawing.Size(169, 30);
            this.confirmImport.TabIndex = 2;
            this.confirmImport.Text = "Confirmar";
            this.confirmImport.UseVisualStyleBackColor = true;
            this.confirmImport.Click += new System.EventHandler(this.confirmImport_Click);
            // 
            // cancelImport
            // 
            this.cancelImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelImport.Location = new System.Drawing.Point(12, 470);
            this.cancelImport.Name = "cancelImport";
            this.cancelImport.Size = new System.Drawing.Size(169, 30);
            this.cancelImport.TabIndex = 3;
            this.cancelImport.Text = "Cancelar";
            this.cancelImport.UseVisualStyleBackColor = true;
            this.cancelImport.Click += new System.EventHandler(this.cancelImport_Click);
            // 
            // option1Checkbox
            // 
            this.option1Checkbox.AutoSize = true;
            this.option1Checkbox.Location = new System.Drawing.Point(663, 478);
            this.option1Checkbox.Name = "option1Checkbox";
            this.option1Checkbox.Size = new System.Drawing.Size(69, 17);
            this.option1Checkbox.TabIndex = 4;
            this.option1Checkbox.Text = "Opcion 1";
            this.option1Checkbox.UseVisualStyleBackColor = true;
            this.option1Checkbox.Visible = false;
            // 
            // option2Checkbox
            // 
            this.option2Checkbox.AutoSize = true;
            this.option2Checkbox.Location = new System.Drawing.Point(437, 478);
            this.option2Checkbox.Name = "option2Checkbox";
            this.option2Checkbox.Size = new System.Drawing.Size(69, 17);
            this.option2Checkbox.TabIndex = 5;
            this.option2Checkbox.Text = "Opcion 2";
            this.option2Checkbox.UseVisualStyleBackColor = true;
            this.option2Checkbox.Visible = false;
            // 
            // ImportResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 512);
            this.Controls.Add(this.option2Checkbox);
            this.Controls.Add(this.option1Checkbox);
            this.Controls.Add(this.cancelImport);
            this.Controls.Add(this.confirmImport);
            this.Controls.Add(this.importGridView);
            this.Controls.Add(this.confirmAccountImportLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImportResults";
            this.Text = "Verificar Importación de Cuentas";
            ((System.ComponentModel.ISupportInitialize)(this.importGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label confirmAccountImportLabel;
        private System.Windows.Forms.DataGridView importGridView;
        private System.Windows.Forms.Button confirmImport;
        private System.Windows.Forms.Button cancelImport;
        private System.Windows.Forms.CheckBox option1Checkbox;
        private System.Windows.Forms.CheckBox option2Checkbox;
    }
}