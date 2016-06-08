using Cindalnet.SQLBot.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cindalnet.SQLBot.Model;
using System.Windows.Forms;

namespace Cindalnet.SQLBot.Presenter
{
    public class PresenterQueryResult : PresenterForm, IPresenter
    {
        public event EventHandler ViewClosed;

        public string FormText
        {
            set
            {
                View.FormText = value;
            }
        }

        private IFormListView View;

        public PresenterQueryResult()
        {
            Initialize();
        }

        public PresenterQueryResult(SQLQueryResult QueryResult)
        {
            Initialize();

            SetHeaders(QueryResult);
            SetData(QueryResult);
        }

        private void Initialize()
        {
            MaterialForm = new CFormListView();
            View = (IFormListView)MaterialForm;
            FormControl = View.FormControl;
            FormText = "SQLQuery";

            View.PropertyChanged += view_PropertyChanged;
            View.ViewClosed += view_Closed;
        }

        private void SetHeaders(SQLQueryResult QueryResult)
        {
            IList<string> Headers = new List<string>();
            foreach(var header in QueryResult.FieldInfo)
            {
                Headers.Add(header.Name);
            }

            View.SetHeaders(Headers);
        }

        private void SetData(SQLQueryResult QueryResult)
        {
            IList<ListViewItem> listItems = new List<ListViewItem>();
            if (QueryResult.FieldInfo.Count > 0)
            {
                foreach (var row in QueryResult.Row)
                {
                    ListViewItem item = new ListViewItem(row[0].Text);
                    for (int colNum = 1; colNum < QueryResult.FieldInfo.Count; colNum++)
                    {
                        item.SubItems.Add(new ListViewItem.ListViewSubItem(item, row[colNum].Text));
                    }
                    
                    listItems.Add(item);
                }
            }
            View.SetData(listItems.ToArray());
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
