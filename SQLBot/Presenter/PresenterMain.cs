using Cindalnet.SQLBot.View;
using MaterialSkin;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Presenter
{
    public class PresenterMain : PresenterForm, IPresenter
    {
        protected IFormMain View;

        public PresenterMain()
        {
            MaterialForm = new CFormMain();
            View = (CFormMain)MaterialForm;
            FormControl = View.FormControl;
            // Initialize MaterialSkinManager
            MaterialSkinManager = MaterialSkinManager.Instance;
            MaterialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            View.PropertyChanged += view_PropertyChanged;
            View.ViewClosed += view_Closed;

            try
            {
                PresenterChat pChat = new PresenterChat();
                pChat.MaterialSkinManager = MaterialSkinManager;
                pChat.MaterialForm.Parent = View.FormControl;
                pChat.ShowQueryResult += pChat_ShowQueryResult;
                View.AddMaterialPanelTab(pChat.MaterialForm, false);
                pChat.ViewClosed += OnTabClosed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void pChat_ShowQueryResult(object sender, EventArgs e)
        {
            try
            {
                PresenterQueryResult pQueryResults;
                if (e is ObjectEventArgs && e != null && (e as ObjectEventArgs).Obj is Model.SQLQueryResult)
                {
                    pQueryResults = new PresenterQueryResult((e as ObjectEventArgs).Obj as Model.SQLQueryResult);
                }
                else
                {
                    pQueryResults = new PresenterQueryResult();
                }
                pQueryResults.MaterialSkinManager = MaterialSkinManager;
                pQueryResults.MaterialForm.Parent = View.FormControl;
                View.AddMaterialPanelTab(pQueryResults.MaterialForm, false);
                pQueryResults.ViewClosed += OnTabClosed;
            }
            catch(Exception)
            {

            }
        }

        public void view_Closed(object sender, EventArgs e)
        {
            //
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

        private void OnTabClosed(object sender, EventArgs e)
        {
            if (sender is PresenterForm)
            {
                View.RemoveMaterialPanelTab(((PresenterForm)sender).MaterialForm);
            }
        }
    }
}
