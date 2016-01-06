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
    public partial class CFormListView : MaterialPanel, IFormListView
    {
        public CFormListView()
        {
            InitializeComponent();
        }

        public string FormText
        {
            set
            {
                this.Name = value;
                this.Text = value;
            }
        }

        public object Selected
        {
            get 
            {
                if (listView.SelectedItems.Count > 0)
                    return listView.SelectedItems[0].Tag;
                else
                    return null;
            }
        }

        public void SetHeaders(IList<string> Headers)
        {
            listView.Columns.Clear();
            foreach(var cName in Headers)
            {
                ColumnHeader cHeader = new ColumnHeader(cName);
                cHeader.Name = cName;
                cHeader.Text = cName;
                cHeader.Width = listView.Width / Headers.Count;
                listView.Columns.Add(cHeader);
            }
        }

        public void SetData(ListViewItem []List)
        {
            listView.Items.Clear();
            listView.Items.AddRange(List);
        }

        public event EventHandler ObjectClicked;
        public event EventHandler RefreshClicked;

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

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            if (ObjectClicked != null)
                ObjectClicked(this, e);
        }

        private void CFormListView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ViewClosing != null)
                ViewClosing(this, e);
        }

        private void CFormListView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ViewClosed != null)
                ViewClosed(this, e);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            if (RefreshClicked != null)
                RefreshClicked(this, e);
        }
    }
}
