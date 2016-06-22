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
    public partial class CFormConnectToServer : MaterialPanel, IFormConnectToServer
    {
        public CFormConnectToServer()
        {
            InitializeComponent();
        }

        public string Server
        {
            get
            {
                return textServer.Text;
            }
            set
            {
                if (textServer.Text == value)
                    return;
                textServer.Text = value;
            }
        }

        public string User
        {
            get
            {
                return textUser.Text;
            }
            set
            {
                if (textUser.Text == value)
                    return;
                textUser.Text = value;
            }
        }

        public string Database
        {
            get
            {
                return textDatabase.Text;
            }
            set
            {
                if (textDatabase.Text == value)
                    return;
                textDatabase.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return textPassword.Text;
            }
            set
            {
                if (textPassword.Text == value)
                    return;
                textPassword.Text = value;
            }
        }

        public string Error
        {
            set
            {
                labelError.Text = value;
                labelError.ForeColor = Color.Red;
                labelError.Visible = true;
            }
        }

        public Form FormControl
        {
            get { return this; }
        }

        public event EventHandler Cancel;
        public event EventHandler OK;
        public event EventHandler ViewClosing
        {
            add { }
            remove { }
        }
        public event EventHandler ViewClosed
        {
            add { }
            remove { }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (OK != null)
                OK(this, e);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (Cancel != null)
                Cancel(this, e);
        }

        private void textServer_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Server"));
        }

        private void textDatabase_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Database"));
        }

        private void textUser_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
        }
    }
}
