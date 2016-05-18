using Cindalnet.SQLBot.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cindalnet.SQLBot.Presenter
{
    public class PresenterLinie : PresenterForm, IPresenter
    {
        public event EventHandler ViewClosed;
        public event EventHandler DisplayWariant;
        public Database.Linie Linia
        {
            get
            {
                return View.Selected as Database.Linie;
            }
        }

        public string FormText
        {
            set
            {
                View.FormText = value;
            }
        }

        private IFormListView View;
        private OverlayBackgroundWorker workerRefreshGrid;

        public PresenterLinie()
        {
            MaterialForm = new CFormListView();
            View = (IFormListView)MaterialForm;
            FormControl = View.FormControl;

            View.PropertyChanged += view_PropertyChanged;
            View.ViewClosed += view_Closed;
            View.SetHeaders(new List<string>() { "Nazwa", "Typ" });
            View.RefreshClicked += View_RefreshClicked;
            View.ObjectClicked += View_ObjectClicked;

            workerRefreshGrid = new OverlayBackgroundWorker();
            workerRefreshGrid.DisplayControl = View.FormControl;
            workerRefreshGrid.DoWork += GetLines;
            workerRefreshGrid.RunWorkerCompleted += DisplayLines;
            workerRefreshGrid.RunWorkerAsync();
        }

        void View_ObjectClicked(object sender, EventArgs e)
        {
            if (DisplayWariant != null)
                DisplayWariant(this, e);
        }

        private void View_RefreshClicked(object sender, EventArgs e)
        {
            workerRefreshGrid.RunWorkerAsync();
        }

        private void GetLines(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                Database.TramwajeDataContext dc = new Database.TramwajeDataContext();
                IList<ListViewItem> listItems = new List<ListViewItem>();
                foreach (var linia in dc.Linies)
                {
                    ListViewItem item = new ListViewItem(linia.Nazwa);
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, linia.Typ));
                    item.Tag = linia;
                    listItems.Add(item);
                }
                e.Result = listItems.ToArray();
            }catch(Exception)
            {
                e.Result = null;
            }
        }

        private void DisplayLines(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if(e.Result != null && e.Result is ListViewItem[])
            {
                View.SetData((ListViewItem[]) e.Result);
            }
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

        public void view_Cancel(object sender, EventArgs e)
        {
            //
        }

        public void view_OK(object sender, EventArgs e)
        {
            //
        }
    }
}
