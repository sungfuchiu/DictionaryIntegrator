namespace DictionaryIntegrator
{
    partial class SettingForm
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
            lblAPIKey = new Label();
            tbxAPIKey = new TextBox();
            lblDatabaseID = new Label();
            tbxDatabaseID = new TextBox();
            btnConfirm = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblAPIKey
            // 
            lblAPIKey.AutoSize = true;
            lblAPIKey.Location = new Point(12, 20);
            lblAPIKey.Name = "lblAPIKey";
            lblAPIKey.Size = new Size(50, 15);
            lblAPIKey.TabIndex = 0;
            lblAPIKey.Text = "API Key:";
            // 
            // tbxAPIKey
            // 
            tbxAPIKey.Location = new Point(68, 17);
            tbxAPIKey.Name = "tbxAPIKey";
            tbxAPIKey.Size = new Size(262, 23);
            tbxAPIKey.TabIndex = 1;
            // 
            // lblDatabaseID
            // 
            lblDatabaseID.AutoSize = true;
            lblDatabaseID.Location = new Point(12, 61);
            lblDatabaseID.Name = "lblDatabaseID";
            lblDatabaseID.Size = new Size(72, 15);
            lblDatabaseID.TabIndex = 2;
            lblDatabaseID.Text = "Database ID:";
            // 
            // tbxDatabaseID
            // 
            tbxDatabaseID.Location = new Point(90, 58);
            tbxDatabaseID.Name = "tbxDatabaseID";
            tbxDatabaseID.Size = new Size(240, 23);
            tbxDatabaseID.TabIndex = 3;
            // 
            // btnConfirm
            // 
            btnConfirm.Location = new Point(209, 106);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(75, 23);
            btnConfirm.TabIndex = 4;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(52, 106);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // SettingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(342, 150);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Controls.Add(tbxDatabaseID);
            Controls.Add(lblDatabaseID);
            Controls.Add(tbxAPIKey);
            Controls.Add(lblAPIKey);
            Name = "SettingForm";
            Text = "SettingForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblAPIKey;
        private TextBox tbxAPIKey;
        private Label lblDatabaseID;
        private TextBox tbxDatabaseID;
        private Button btnConfirm;
        private Button btnCancel;
    }
}