using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cindalnet.SQLBot.View
{
    public partial class CFormMain : MaterialForm, IFormMain
    {
        public CFormMain()
        {
            InitializeComponent();
        }

        public Form FormControl
        {
            get { return this; }
        }

        public event EventHandler Cancel
        {
            add { }
            remove { }
        }

        public event EventHandler OK
        {
            add { }
            remove { }
        }

        public event EventHandler ViewClosing;
        public event EventHandler ViewClosed;
        public event PropertyChangedEventHandler PropertyChanged
        { 
            add { }
            remove { }
        }

        private void CFormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ViewClosing != null)
                ViewClosing(this, e);
        }

        private void CFormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ViewClosed != null)
                ViewClosed(this, e);
        }

        public void AddMaterialPanelTab(MaterialPanel panel, bool focus = false)
        {
            TabPage newTab = new TabPage(panel.Text);
            panel.Dock = DockStyle.Fill;
            panel.Parent = newTab;
            newTab.Controls.Add(panel);
            tabControl.TabPages.Add(newTab);
            if (focus)
                tabControl.SelectedTab = newTab;
        }

        public void AddMaterialPanelTab(MaterialForm form, bool focus = false)
        {
            TabPage newTab = new TabPage(form.Text);
            form.Dock = DockStyle.Fill;
            form.Parent = newTab;
            newTab.Controls.Add(form);
            tabControl.TabPages.Add(newTab);
            if (focus)
                tabControl.SelectedTab = newTab;
        }

        public bool RemoveMaterialPanelTab(string panelText)
        {
            int tabIndex = 0;
            
            foreach (TabPage tab in this.tabControl.TabPages)
            {
                if (tab.Text == panelText)
                {
                    int tabSelectedIndex = this.tabControl.SelectedIndex;

                    this.tabControl.TabPages.Remove(tab);

                    if (tabSelectedIndex >= tabIndex && tabIndex > 0)
                    {
                        this.tabControl.SelectTab(tabIndex - 1);
                    }
                    else
                    {
                        this.tabControl.SelectTab(tabSelectedIndex);
                    }
                    return true;
                }
                tabIndex++;
            }
            return false;
        }

        public bool RemoveMaterialPanelTab(MaterialForm form)
        {
            return RemoveMaterialPanelTab(form.Text);
        }

        public bool RemoveMaterialPanelTab(MaterialPanel panel)
        {
            return RemoveMaterialPanelTab(panel.Text);
        }
    }
}
