namespace Cindalnet.SQLBot.View
{
    partial class CFormLoading
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
            this.LoadingGif = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelLoading = new MaterialSkin.Controls.MaterialLabel();
            this.labelProgress = new MaterialSkin.Controls.MaterialLabel();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingGif)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoadingGif
            // 
            this.LoadingGif.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LoadingGif.Image = global::Cindalnet.SQLBot.Properties.Resources.Loading;
            this.LoadingGif.Location = new System.Drawing.Point(179, 98);
            this.LoadingGif.Margin = new System.Windows.Forms.Padding(0);
            this.LoadingGif.Name = "LoadingGif";
            this.LoadingGif.Size = new System.Drawing.Size(25, 25);
            this.LoadingGif.TabIndex = 0;
            this.LoadingGif.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.LoadingGif, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelLoading, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelProgress, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 271);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // labelLoading
            // 
            this.labelLoading.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelLoading.AutoSize = true;
            this.labelLoading.Depth = 0;
            this.labelLoading.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelLoading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelLoading.Location = new System.Drawing.Point(145, 129);
            this.labelLoading.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(93, 19);
            this.labelLoading.TabIndex = 2;
            this.labelLoading.Text = "Ładowanie...";
            // 
            // labelProgress
            // 
            this.labelProgress.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelProgress.AutoSize = true;
            this.labelProgress.Depth = 0;
            this.labelProgress.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelProgress.Location = new System.Drawing.Point(192, 154);
            this.labelProgress.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(0, 19);
            this.labelProgress.TabIndex = 3;
            // 
            // CFormLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 271);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CFormLoading";
            this.Opacity = 0.8D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CFormLoading";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.LoadingGif)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox LoadingGif;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MaterialSkin.Controls.MaterialLabel labelLoading;
        private MaterialSkin.Controls.MaterialLabel labelProgress;
    }
}