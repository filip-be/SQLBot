using Cindalnet.SQLBot.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cindalnet.SQLBot.Presenter
{
    public class PresenterWariant : PresenterForm
    {
        public event EventHandler ViewClosed;
        public event EventHandler DisplayPrzystanek;
        private Query.Linie Linia { get; set; }
        public Query.WariantTrasy Wariant
        {
            get
            {
                return View.Selected as Query.WariantTrasy;
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

        public PresenterWariant(Query.Linie linia)
        {
            this.Linia = linia;

            MaterialForm = new CFormListView();
            View = (IFormListView)MaterialForm;
            FormControl = View.FormControl;

            View.PropertyChanged += view_PropertyChanged;
            View.ViewClosed += view_Closed;
            View.SetHeaders(new List<string>() { "Nazwa", "Start", "Koniec" });
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
            if (DisplayPrzystanek != null)
                DisplayPrzystanek(this, e);
        }

        private void View_RefreshClicked(object sender, EventArgs e)
        {
            workerRefreshGrid.RunWorkerAsync();
        }

        private void GetLines(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                Query.TramwajeDataContext dc = new Query.TramwajeDataContext();
                IList<ListViewItem> listItems = new List<ListViewItem>();
                foreach (var wariant in dc.WariantTrasies.Where(wariant => wariant.Linie == Linia))
                {
                    ListViewItem item = new ListViewItem(wariant.Nazwa);
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, wariant.Start));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, wariant.Koniec));
                    item.Tag = wariant;
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
