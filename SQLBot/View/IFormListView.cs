using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.View
{
    public interface IFormListView : IForm
    {
        object Selected { get; }
        void SetHeaders(IList<string> Headers);
        void SetData(System.Windows.Forms.ListViewItem []List);
        string FormText { set; }

        event EventHandler ObjectClicked;
        event EventHandler RefreshClicked;
    }
}
