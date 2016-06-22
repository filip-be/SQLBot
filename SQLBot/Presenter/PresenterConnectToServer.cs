using Cindalnet.SQLBot.View;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Presenter
{
    public class PresenterConnectToServer : PresenterForm, IPresenter
    {
        protected IFormConnectToServer View;
        protected SqlConnectionStringBuilder ConnectionStringBuilder;

        public EventHandler ViewClosed;
        public EventHandler ConnectedSuccessfully;
        public EventHandler ServerNotRunning;

        public PresenterConnectToServer(bool ConnectAutomatically)
        {
            
            MaterialForm = new CFormConnectToServer();
            View = (CFormConnectToServer)MaterialForm;
            FormControl = View.FormControl;

            View.PropertyChanged += view_PropertyChanged;
            View.ViewClosed += view_Closed;
            View.Cancel += view_Cancel;
            View.OK += view_OK;

            ConnectionStringBuilder = new SqlConnectionStringBuilder(Properties.Settings.Default.BazaRelacyjnaConnectionString);
            //Data Source=FILIPBE\CINDALNET;Initial Catalog=BazaRelacyjna;User ID=sa;Password=sql
            View.Server = (string)ConnectionStringBuilder["Data Source"];
            View.Database = (string)ConnectionStringBuilder["Initial Catalog"];
            View.User = (string)ConnectionStringBuilder["User ID"];
            View.Password = (string)ConnectionStringBuilder["Password"];

            if (ConnectAutomatically)
                TestConnection();
        }

        private bool TestConnectionString()
        {
            using(SqlConnection conn = new SqlConnection(ConnectionStringBuilder.ConnectionString))
            {
                try
                {
                    conn.Open();
                    return conn.State == System.Data.ConnectionState.Open;
                }
                catch
                {
                    return false;
                }
            }
        }

        private void TestConnection()
        {
            OverlayBackgroundWorker worker = new OverlayBackgroundWorker();
            worker.DoWork += TestConnectionString;
            worker.RunWorkerCompleted += TestConnectionComplete;
            worker.DisplayControl = View.FormControl;
            worker.RunWorkerAsync();
        }

        private void TestConnectionString(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            e.Result = TestConnectionString();
        }

        private void TestConnectionComplete(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Result is bool && (bool)e.Result == true)
            {
                if (ConnectedSuccessfully != null)
                    ConnectedSuccessfully(this, new EventArgs());
            }
            else
            {
                View.Error = "Nie udało się nawiązać połączenia z serwerem. Sprawdź poprawność danych i spróbuj ponownie.";
            }
        }

        public void view_OK(object sender, EventArgs e)
        {
            TestConnection();
        }

        public void view_Cancel(object sender, EventArgs e)
        {
            //
        }

        public void view_Closed(object sender, EventArgs e)
        {
            if (ViewClosed != null)
                ViewClosed(this, e);
        }

        public void model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //
        }

        public void view_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Server":
                    ConnectionStringBuilder["Data Source"] = View.Server;
                    break;
                case "Database":
                    ConnectionStringBuilder["Initial Catalog"] = View.Database;
                    break;
                case "Username":
                    ConnectionStringBuilder["User ID"] = View.User;
                    break;
                case "Password":
                    ConnectionStringBuilder["Password"] = View.Password;
                    break;
                default:
                    break;
            }
        }
    }
}
