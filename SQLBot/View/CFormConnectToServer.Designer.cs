namespace Cindalnet.SQLBot.View
{
    partial class CFormConnectToServer
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
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.labelServer = new MaterialSkin.Controls.MaterialLabel();
            this.textServer = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.textPassword = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.labelPassword = new MaterialSkin.Controls.MaterialLabel();
            this.labelUser = new MaterialSkin.Controls.MaterialLabel();
            this.textUser = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.textDatabase = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.labelDatabase = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabelInsertData = new MaterialSkin.Controls.MaterialLabel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCancel = new MaterialSkin.Controls.MaterialFlatButton();
            this.buttonSave = new MaterialSkin.Controls.MaterialRaisedButton();
            this.labelError = new MaterialSkin.Controls.MaterialLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.materialLabelInsertData, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelError, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(668, 266);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.labelServer, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.textServer, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.textPassword, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.labelPassword, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.labelUser, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.textUser, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.textDatabase, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.labelDatabase, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(662, 155);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // labelServer
            // 
            this.labelServer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelServer.AutoSize = true;
            this.labelServer.Depth = 0;
            this.labelServer.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelServer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelServer.Location = new System.Drawing.Point(3, 5);
            this.labelServer.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(59, 19);
            this.labelServer.TabIndex = 0;
            this.labelServer.Text = "Serwer:";
            // 
            // textServer
            // 
            this.textServer.AcceptsReturn = false;
            this.textServer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textServer.Depth = 0;
            this.textServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.textServer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textServer.Hint = "";
            this.textServer.Location = new System.Drawing.Point(153, 3);
            this.textServer.MaxLength = 32767;
            this.textServer.MouseState = MaterialSkin.MouseState.HOVER;
            this.textServer.Name = "textServer";
            this.textServer.PasswordChar = '\0';
            this.textServer.SelectedText = "";
            this.textServer.SelectionLength = 0;
            this.textServer.SelectionStart = 0;
            this.textServer.Size = new System.Drawing.Size(506, 23);
            this.textServer.TabIndex = 1;
            this.textServer.TabStop = false;
            this.textServer.UseSystemPasswordChar = false;
            this.textServer.TextChanged += new System.EventHandler(this.textServer_TextChanged);
            // 
            // textPassword
            // 
            this.textPassword.AcceptsReturn = false;
            this.textPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPassword.Depth = 0;
            this.textPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.textPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textPassword.Hint = "";
            this.textPassword.Location = new System.Drawing.Point(153, 93);
            this.textPassword.MaxLength = 32767;
            this.textPassword.MouseState = MaterialSkin.MouseState.HOVER;
            this.textPassword.Name = "textPassword";
            this.textPassword.PasswordChar = '*';
            this.textPassword.SelectedText = "";
            this.textPassword.SelectionLength = 0;
            this.textPassword.SelectionStart = 0;
            this.textPassword.Size = new System.Drawing.Size(506, 23);
            this.textPassword.TabIndex = 7;
            this.textPassword.TabStop = false;
            this.textPassword.UseSystemPasswordChar = false;
            this.textPassword.TextChanged += new System.EventHandler(this.textPassword_TextChanged);
            // 
            // labelPassword
            // 
            this.labelPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelPassword.AutoSize = true;
            this.labelPassword.Depth = 0;
            this.labelPassword.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelPassword.Location = new System.Drawing.Point(3, 95);
            this.labelPassword.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(53, 19);
            this.labelPassword.TabIndex = 6;
            this.labelPassword.Text = "Hasło:";
            // 
            // labelUser
            // 
            this.labelUser.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelUser.AutoSize = true;
            this.labelUser.Depth = 0;
            this.labelUser.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelUser.Location = new System.Drawing.Point(3, 65);
            this.labelUser.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(90, 19);
            this.labelUser.TabIndex = 4;
            this.labelUser.Text = "Użytkownik:";
            // 
            // textUser
            // 
            this.textUser.AcceptsReturn = false;
            this.textUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textUser.Depth = 0;
            this.textUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.textUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textUser.Hint = "";
            this.textUser.Location = new System.Drawing.Point(153, 63);
            this.textUser.MaxLength = 32767;
            this.textUser.MouseState = MaterialSkin.MouseState.HOVER;
            this.textUser.Name = "textUser";
            this.textUser.PasswordChar = '\0';
            this.textUser.SelectedText = "";
            this.textUser.SelectionLength = 0;
            this.textUser.SelectionStart = 0;
            this.textUser.Size = new System.Drawing.Size(506, 23);
            this.textUser.TabIndex = 5;
            this.textUser.TabStop = false;
            this.textUser.UseSystemPasswordChar = false;
            this.textUser.TextChanged += new System.EventHandler(this.textUser_TextChanged);
            // 
            // textDatabase
            // 
            this.textDatabase.AcceptsReturn = false;
            this.textDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textDatabase.Depth = 0;
            this.textDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.textDatabase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textDatabase.Hint = "";
            this.textDatabase.Location = new System.Drawing.Point(153, 33);
            this.textDatabase.MaxLength = 32767;
            this.textDatabase.MouseState = MaterialSkin.MouseState.HOVER;
            this.textDatabase.Name = "textDatabase";
            this.textDatabase.PasswordChar = '\0';
            this.textDatabase.SelectedText = "";
            this.textDatabase.SelectionLength = 0;
            this.textDatabase.SelectionStart = 0;
            this.textDatabase.Size = new System.Drawing.Size(506, 23);
            this.textDatabase.TabIndex = 3;
            this.textDatabase.TabStop = false;
            this.textDatabase.UseSystemPasswordChar = false;
            this.textDatabase.TextChanged += new System.EventHandler(this.textDatabase_TextChanged);
            // 
            // labelDatabase
            // 
            this.labelDatabase.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelDatabase.AutoSize = true;
            this.labelDatabase.Depth = 0;
            this.labelDatabase.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelDatabase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelDatabase.Location = new System.Drawing.Point(3, 35);
            this.labelDatabase.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(96, 19);
            this.labelDatabase.TabIndex = 2;
            this.labelDatabase.Text = "Baza danych:";
            // 
            // materialLabelInsertData
            // 
            this.materialLabelInsertData.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.materialLabelInsertData.AutoSize = true;
            this.materialLabelInsertData.Depth = 0;
            this.materialLabelInsertData.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabelInsertData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabelInsertData.Location = new System.Drawing.Point(3, 3);
            this.materialLabelInsertData.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabelInsertData.Name = "materialLabelInsertData";
            this.materialLabelInsertData.Size = new System.Drawing.Size(175, 19);
            this.materialLabelInsertData.TabIndex = 0;
            this.materialLabelInsertData.Text = "Wprowadź dane serwera:";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.Controls.Add(this.buttonCancel, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonSave, 2, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 216);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(668, 50);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonCancel.AutoSize = true;
            this.buttonCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonCancel.Depth = 0;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Font = new System.Drawing.Font("Roboto", 10F);
            this.buttonCancel.Location = new System.Drawing.Point(486, 6);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonCancel.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Primary = false;
            this.buttonCancel.Size = new System.Drawing.Size(64, 36);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Anuluj";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Visible = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonSave.Depth = 0;
            this.buttonSave.Font = new System.Drawing.Font("Roboto", 10F);
            this.buttonSave.Location = new System.Drawing.Point(571, 3);
            this.buttonSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Primary = true;
            this.buttonSave.Size = new System.Drawing.Size(94, 42);
            this.buttonSave.TabIndex = 9;
            this.buttonSave.Text = "Połącz";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Depth = 0;
            this.labelError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelError.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelError.Location = new System.Drawing.Point(3, 186);
            this.labelError.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(662, 30);
            this.labelError.TabIndex = 4;
            // 
            // CFormConnectToServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 266);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CFormConnectToServer";
            this.Text = "Połączenie";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private MaterialSkin.Controls.MaterialLabel labelServer;
        private MaterialSkin.Controls.MaterialLabel labelUser;
        private MaterialSkin.Controls.MaterialSingleLineTextField textServer;
        private MaterialSkin.Controls.MaterialSingleLineTextField textUser;
        private MaterialSkin.Controls.MaterialSingleLineTextField textDatabase;
        private MaterialSkin.Controls.MaterialSingleLineTextField textPassword;
        private MaterialSkin.Controls.MaterialLabel labelDatabase;
        private MaterialSkin.Controls.MaterialLabel labelPassword;
        private MaterialSkin.Controls.MaterialLabel materialLabelInsertData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private MaterialSkin.Controls.MaterialFlatButton buttonCancel;
        private MaterialSkin.Controls.MaterialRaisedButton buttonSave;
        private MaterialSkin.Controls.MaterialLabel labelError;

    }
}