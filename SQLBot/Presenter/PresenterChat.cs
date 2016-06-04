using Cindalnet.SQLBot.Query;
using Cindalnet.SQLBot.Model;
using Cindalnet.SQLBot.View;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace Cindalnet.SQLBot.Presenter
{
    public class PresenterChat : PresenterForm, IPresenter
    {
        public event EventHandler ViewClosed;
        private IFormChat View;
        private QueryParser QueryParser;
        private OverlayBackgroundWorker workerProcessMessage;

        public event EventHandler ShowQueryResult;

        private List<string> responseHistory { get; set; }
        private int responseHistoryIndex { get; set; }

        public PresenterChat()
        {
            MaterialForm = new CFormChat();
            View = (IFormChat)MaterialForm;
            FormControl = View.FormControl;
            QueryParser = new Query.QueryParser();
            responseHistory = new List<string>();
            responseHistory.Add("");
            responseHistoryIndex = 0;

            View.PropertyChanged += view_PropertyChanged;
            View.ViewClosed += view_Closed;
            View.ProcessMessage += View_ProcessMessage;
            View.KeyUpPressed += View_KeyUpPressed;
            View.KeyDownPressed += View_KeyDownPressed;

            workerProcessMessage = new OverlayBackgroundWorker();
            workerProcessMessage.DisplayControl = View.FormControl;
            workerProcessMessage.DoWork += ProcessQuery;
            workerProcessMessage.RunWorkerCompleted += DisplayQuery;
        }

        void View_KeyDownPressed(object sender, EventArgs e)
        {
            if (responseHistoryIndex >= responseHistory.Count - 1)
            {
                SystemSounds.Beep.Play();
            }
            else
            {   
                View.Query = responseHistory[++responseHistoryIndex];
            }
        }

        void View_KeyUpPressed(object sender, EventArgs e)
        {
            if(responseHistoryIndex <= 0)
            {
                SystemSounds.Beep.Play();
            }
            else
            {
                View.Query = responseHistory[--responseHistoryIndex];
            }
        }

        private void DisplayQuery(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            View.Response = View.Query;

            if (e.Result != null && e.Result is string)
            {
                // Handle multiline response!
                foreach (var line in (e.Result as string).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
                {
                    View.Response = line;
                }
            }

            View.Query = "";

            if (QueryParser.QueryResult != null && ShowQueryResult != null)
                ShowQueryResult(this, new ObjectEventArgs(QueryParser.QueryResult));

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
            responseHistory.Insert(responseHistory.Count - 1, View.Query);
            responseHistoryIndex = responseHistory.Count - 1;
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
