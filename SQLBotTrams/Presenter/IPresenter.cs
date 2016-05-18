using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Presenter
{
    public interface IPresenter
    {
        /// <summary>
        /// Widok został zamknięty
        /// </summary>
        void view_Closed(object sender, EventArgs e);

        /// <summary>
        /// Pewna własność modelu uległa zmianie
        /// </summary>
        void model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e);

        /// <summary>
        /// Pewna własność widoku uległa zmianie
        /// </summary>
        void view_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e);

        /// <summary>
        /// Anuluj
        /// </summary>
        void view_Cancel(object sender, EventArgs e);

        /// <summary>
        /// OK
        /// </summary>
        void view_OK(object sender, EventArgs e);
    }
}
