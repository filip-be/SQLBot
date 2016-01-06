using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cindalnet.SQLBot.View
{
    public partial class MaterialPanel : MaterialForm
    {
        public MaterialPanel()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.AutoScroll = true;
            this.Visible = true;
        }

        public System.Windows.Forms.DialogResult ShowDialog(MaterialForm parentForm, bool DrawActionBar)
        {
            Control TopParentObject = parentForm;
            while(TopParentObject != null && !(TopParentObject is IWin32Window) && TopParentObject.Parent != null)
            {
                TopParentObject = TopParentObject.Parent;
            };

            this.Visible = false;
            this.TopLevel = true;
            this.DrawBorderLine = true;
            this.DrawStatusBar = true;
            this.STATUS_BAR_HEIGHT = STATUS_BAR_HEIGHT;

            if (DrawActionBar)
            {
                this.DrawActionBar = true;
                this.ACTION_BAR_HEIGHT = ACTION_BAR_HEIGHT;
                this.TitleLocation = LocationType.ActionBar;
            }
            else
            {
                this.TitleLocation = LocationType.StatusBar;
            }
            this.Sizable = true;
            this.DrawBorderLine = true;
            
            this.Moveable = true;
            
            this.ControlBox = true;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            
            this.TitleImage = null;
            
            this.Width -= 1;
            this.Width += 1;

            if (this.Controls.Count == 1 && this.Controls[0] is System.Windows.Forms.TableLayoutPanel)
            {
                this.Controls[0].Dock = System.Windows.Forms.DockStyle.None;
                System.Drawing.Rectangle bounds = this.Controls[0].Bounds;
                bounds.Y += (DrawActionBar ? this.ACTION_BAR_HEIGHT : 0) + this.STATUS_BAR_HEIGHT;
                bounds.X += this.DefaultMargin.Left;
                bounds.Height -= (DrawActionBar ? this.ACTION_BAR_HEIGHT : 0) + this.STATUS_BAR_HEIGHT + this.DefaultMargin.Top + this.DefaultMargin.Bottom;
                bounds.Width -= (this.DefaultMargin.Left + this.DefaultMargin.Right + 2);
                this.Controls[0].Bounds = bounds;
                this.Controls[0].Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            }

            if (TopParentObject != null && TopParentObject is IWin32Window)
                return ShowDialog(TopParentObject);
            else
                return ShowDialog();
        }
        
    }
}
