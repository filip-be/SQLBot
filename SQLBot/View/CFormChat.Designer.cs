namespace Cindalnet.SQLBot.View
{
    partial class CFormChat
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
            this.panelMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelQuestion = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAsk = new MaterialSkin.Controls.MaterialRaisedButton();
            this.textQuestion = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.listAnswers = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelMain.SuspendLayout();
            this.panelQuestion.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelMain.ColumnCount = 1;
            this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panelMain.Controls.Add(this.panelQuestion, 0, 1);
            this.panelMain.Controls.Add(this.listAnswers, 0, 0);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.RowCount = 2;
            this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.panelMain.Size = new System.Drawing.Size(517, 321);
            this.panelMain.TabIndex = 1;
            // 
            // panelQuestion
            // 
            this.panelQuestion.ColumnCount = 2;
            this.panelQuestion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelQuestion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.panelQuestion.Controls.Add(this.buttonAsk, 1, 0);
            this.panelQuestion.Controls.Add(this.textQuestion, 0, 0);
            this.panelQuestion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelQuestion.Location = new System.Drawing.Point(0, 291);
            this.panelQuestion.Margin = new System.Windows.Forms.Padding(0);
            this.panelQuestion.Name = "panelQuestion";
            this.panelQuestion.RowCount = 1;
            this.panelQuestion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelQuestion.Size = new System.Drawing.Size(517, 30);
            this.panelQuestion.TabIndex = 0;
            // 
            // buttonAsk
            // 
            this.buttonAsk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAsk.Depth = 0;
            this.buttonAsk.Location = new System.Drawing.Point(420, 3);
            this.buttonAsk.MouseState = MaterialSkin.MouseState.HOVER;
            this.buttonAsk.Name = "buttonAsk";
            this.buttonAsk.Primary = true;
            this.buttonAsk.Size = new System.Drawing.Size(94, 23);
            this.buttonAsk.TabIndex = 0;
            this.buttonAsk.Text = "Zapytaj";
            this.buttonAsk.UseVisualStyleBackColor = true;
            this.buttonAsk.Click += new System.EventHandler(this.buttonAsk_Click);
            // 
            // textQuestion
            // 
            this.textQuestion.AcceptsReturn = false;
            this.textQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textQuestion.Depth = 0;
            this.textQuestion.Hint = "Czego chcesz się dowiedzieć?";
            this.textQuestion.Location = new System.Drawing.Point(3, 3);
            this.textQuestion.MaxLength = 32767;
            this.textQuestion.MouseState = MaterialSkin.MouseState.HOVER;
            this.textQuestion.Name = "textQuestion";
            this.textQuestion.PasswordChar = '\0';
            this.textQuestion.SelectedText = "";
            this.textQuestion.SelectionLength = 0;
            this.textQuestion.SelectionStart = 0;
            this.textQuestion.Size = new System.Drawing.Size(411, 23);
            this.textQuestion.TabIndex = 1;
            this.textQuestion.TabStop = false;
            this.textQuestion.UseSystemPasswordChar = false;
            this.textQuestion.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textQuestion_KeyDown);
            // 
            // listAnswers
            // 
            this.listAnswers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listAnswers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listAnswers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listAnswers.Depth = 0;
            this.listAnswers.FitToWidth = true;
            this.listAnswers.Font = new System.Drawing.Font("Roboto", 10.15F);
            this.listAnswers.FullRowSelect = true;
            this.listAnswers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listAnswers.Location = new System.Drawing.Point(3, 3);
            this.listAnswers.MouseLocation = new System.Drawing.Point(-1, -1);
            this.listAnswers.MouseState = MaterialSkin.MouseState.OUT;
            this.listAnswers.Name = "listAnswers";
            this.listAnswers.OwnerDraw = true;
            this.listAnswers.Size = new System.Drawing.Size(511, 285);
            this.listAnswers.TabIndex = 1;
            this.listAnswers.UseCompatibleStateImageBehavior = false;
            this.listAnswers.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ColumnHeader";
            this.columnHeader1.Width = 105;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 406;
            // 
            // CFormChat
            // 
            this.AcceptButton = this.buttonAsk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(517, 321);
            this.Controls.Add(this.panelMain);
            this.Name = "CFormChat";
            this.Text = "Zapytanie";
            this.TitleLocation = MaterialSkin.Controls.MaterialForm.LocationType.None;
            this.panelMain.ResumeLayout(false);
            this.panelQuestion.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel panelMain;
        private System.Windows.Forms.TableLayoutPanel panelQuestion;
        private MaterialSkin.Controls.MaterialRaisedButton buttonAsk;
        private MaterialSkin.Controls.MaterialSingleLineTextField textQuestion;
        private MaterialSkin.Controls.MaterialListView listAnswers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}