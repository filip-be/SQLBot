namespace Cindalnet.SQLBot.View
{
    partial class CFormManage
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelText = new MaterialSkin.Controls.MaterialLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.fPath = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.buttonBrowse = new MaterialSkin.Controls.MaterialFlatButton();
            this.buttonAdd = new MaterialSkin.Controls.MaterialRaisedButton();
            this.labelMessage = new MaterialSkin.Controls.MaterialLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelText, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonAdd, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelMessage, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(418, 264);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelText
            // 
            this.labelText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelText.AutoSize = true;
            this.labelText.Depth = 0;
            this.labelText.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelText.Location = new System.Drawing.Point(3, 3);
            this.labelText.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(412, 19);
            this.labelText.TabIndex = 0;
            this.labelText.Text = "Dodaj nową trasę";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.Controls.Add(this.fPath, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonBrowse, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(418, 25);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // fPath
            // 
            this.fPath.AcceptsReturn = false;
            this.fPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.fPath.Depth = 0;
            this.fPath.Hint = "Wprowadź ścieżkę pliku...";
            this.fPath.Location = new System.Drawing.Point(3, 3);
            this.fPath.MaxLength = 32767;
            this.fPath.MouseState = MaterialSkin.MouseState.HOVER;
            this.fPath.Name = "fPath";
            this.fPath.PasswordChar = '\0';
            this.fPath.SelectedText = "";
            this.fPath.SelectionLength = 0;
            this.fPath.SelectionStart = 0;
            this.fPath.Size = new System.Drawing.Size(312, 23);
            this.fPath.TabIndex = 0;
            this.fPath.TabStop = false;
            this.fPath.UseSystemPasswordChar = false;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.AutoSize = true;
            this.buttonBrowse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonBrowse.Depth = 0;
            this.buttonBrowse.Location = new System.Drawing.Point(322, 6);
            this.buttonBrowse.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonBrowse.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Primary = false;
            this.buttonBrowse.Size = new System.Drawing.Size(92, 13);
            this.buttonBrowse.TabIndex = 1;
            this.buttonBrowse.Text = "Przeglądaj";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonAdd.Depth = 0;
            this.buttonAdd.Location = new System.Drawing.Point(322, 53);
            this.buttonAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Primary = true;
            this.buttonAdd.Size = new System.Drawing.Size(93, 19);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "Dodaj";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMessage.AutoSize = true;
            this.labelMessage.Depth = 0;
            this.labelMessage.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelMessage.Location = new System.Drawing.Point(3, 78);
            this.labelMessage.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(412, 19);
            this.labelMessage.TabIndex = 3;
            // 
            // CFormManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(418, 264);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CFormManage";
            this.Text = "Zarządzaj";
            this.TitleLocation = MaterialSkin.Controls.MaterialForm.LocationType.None;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MaterialSkin.Controls.MaterialLabel labelText;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MaterialSkin.Controls.MaterialSingleLineTextField fPath;
        private MaterialSkin.Controls.MaterialFlatButton buttonBrowse;
        private MaterialSkin.Controls.MaterialRaisedButton buttonAdd;
        private MaterialSkin.Controls.MaterialLabel labelMessage;

    }
}