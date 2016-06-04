using Cindalnet.SQLBot.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cindalnet.SQLBot.Presenter
{
    public class PresenterPrzyjazd : PresenterForm
    {
        public event EventHandler ViewClosed;
        private Query.Przystanek Przystanek { get; set; }
        public string FormText
        {
            set
            {
                View.FormText = value;
            }
        }
        private IFormListView View;
        private OverlayBackgroundWorker workerRefreshGrid;

        public PresenterPrzyjazd(Query.Przystanek przystanek)
        {
            this.Przystanek = przystanek;

            MaterialForm = new CFormListView();
            View = (IFormListView)MaterialForm;
            FormControl = View.FormControl;

            View.PropertyChanged += view_PropertyChanged;
            View.ViewClosed += view_Closed;
            View.SetHeaders(new List<string>() { "Dzień", "Godzina", "Minuta" });
            View.RefreshClicked += View_RefreshClicked;

            workerRefreshGrid = new OverlayBackgroundWorker();
            workerRefreshGrid.DisplayControl = View.FormControl;
            workerRefreshGrid.DoWork += GetLines;
            workerRefreshGrid.RunWorkerCompleted += DisplayLines;
            workerRefreshGrid.RunWorkerAsync();
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
                foreach (var wariant in dc.Przyjazds.Where(przyjazd => przyjazd.Przystanek == Przystanek))
                {
                    ListViewItem item = new ListViewItem(wariant.Dzień);
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, wariant.Godzina.ToString()));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, wariant.Minuta.ToString()));
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
