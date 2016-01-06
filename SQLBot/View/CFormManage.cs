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
    public partial class CFormManage : MaterialPanel, IFormManage
    {
        public CFormManage()
        {
            InitializeComponent();
            base.FormClosing += OnFormClosing;
            base.FormClosed += OnFormClosed;
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

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            if (ViewClosed != null)
                ViewClosed(this, e);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (ViewClosing != null)
                ViewClosing(this, e);
        }

        public string FilePath
        {
            get
            {
                return fPath.Text;
            }
            set
            {
                if (fPath.Text == value)
                    return;
                fPath.Text = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("FilePath"));
            }
        }

        public string Message
        {
            set
            {
                labelMessage.Text = value;
            }
        }

        public event EventHandler AddToDatabase;

        public Form FormControl
        {
            get { return this; }
        }

        public event EventHandler ViewClosing;
        public event EventHandler ViewClosed;
        public event PropertyChangedEventHandler PropertyChanged;

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.DefaultExt = "xml";
            fDialog.CheckFileExists = true;
            fDialog.FileName = FilePath;
            fDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            fDialog.FilterIndex = 1;
            fDialog.Multiselect = false;
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = fDialog.FileName;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (AddToDatabase != null)
                AddToDatabase(this, e);
        }
    }
}
