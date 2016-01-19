using Cindalnet.SQLBot.Database;
using Cindalnet.SQLBot.Model;
using Cindalnet.SQLBot.View;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Presenter
{
    public class PresenterChat : PresenterForm, IPresenter
    {
        public event EventHandler ViewClosed;
        private IFormChat View;
        private QueryParser QueryParser;
        private OverlayBackgroundWorker workerProcessMessage;

        public PresenterChat()
        {
            MaterialForm = new CFormChat();
            View = (IFormChat)MaterialForm;
            FormControl = View.FormControl;
            QueryParser = new Model.QueryParser();

            View.PropertyChanged += view_PropertyChanged;
            View.ViewClosed += view_Closed;
            View.ProcessMessage += View_ProcessMessage;

            workerProcessMessage = new OverlayBackgroundWorker();
            workerProcessMessage.DisplayControl = View.FormControl;
            workerProcessMessage.DoWork += ProcessQuery;
            workerProcessMessage.RunWorkerCompleted += DisplayQuery;
        }

        private void DisplayQuery(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            View.Response = View.Query;

            if(e.Result != null && e.Result is string)
                View.Response = (string)e.Result;

            View.Query = "";
        }

        private void ProcessQuery(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                e.Result = QueryParser.ParseQuery(View.Query);
            }catch(Exception ex)
            {
                e.Result = "Coś poszło nie tak: " + ex.Message;
            }
        }

        void View_ProcessMessage(object sender, EventArgs e)
        {
            workerProcessMessage.RunWorkerAsync();
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
            //
        }

        public void view_OK(object sender, EventArgs e)
        {
            //
        }

        public void view_Cancel(object sender, EventArgs e)
        {
            //
        }
    }
}
