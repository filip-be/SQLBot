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
    public partial class CFormChat : MaterialPanel, IFormChat
    {
        public CFormChat()
        {
            InitializeComponent();
            base.FormClosing += OnFormClosing;
            base.FormClosed += OnFormClosed;
        }

        public string Query
        {
            get
            {
                return textQuestion.Text;
            }
            set
            {
                if (textQuestion.Text == value)
                    return;
                textQuestion.Text = value;
                textQuestion.SelectAll();
            }
        }

        public string Response
        {
            set
            {
                listAnswers.Items.Add(new ListViewItem(value));
                listAnswers.Items[listAnswers.Items.Count - 1].EnsureVisible();
            }
        }

        public event EventHandler ProcessMessage;

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

        public event EventHandler ViewClosing;
        public event EventHandler ViewClosed;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { }
            remove { }
        }

        private void buttonAsk_Click(object sender, EventArgs e)
        {
            if (ProcessMessage != null)
                ProcessMessage(this, e);
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
    }
}
