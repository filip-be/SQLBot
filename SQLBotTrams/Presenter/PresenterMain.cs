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
                PresenterManage pManage = new PresenterManage();
                pManage.MaterialSkinManager = MaterialSkinManager;
                pManage.MaterialForm.Parent = View.FormControl;
                View.AddMaterialPanelTab(pManage.MaterialForm, true);
                pManage.ViewClosed += OnTabClosed;
            }catch(Exception)
            { }

            try
            {
                PresenterChat pChat = new PresenterChat();
                pChat.MaterialSkinManager = MaterialSkinManager;
                pChat.MaterialForm.Parent = View.FormControl;
                View.AddMaterialPanelTab(pChat.MaterialForm, false);
                pChat.ViewClosed += OnTabClosed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                PresenterLinie pLinie = new PresenterLinie();
                pLinie.FormText = "Linie";
                pLinie.MaterialSkinManager = MaterialSkinManager;
                pLinie.MaterialForm.Parent = View.FormControl;
                View.AddMaterialPanelTab(pLinie.MaterialForm, false);
                pLinie.ViewClosed += OnTabClosed;
                pLinie.DisplayWariant += pLinie_DisplayWariant;
            }
            catch (Exception)
            { }
        }

        void pLinie_DisplayWariant(object sender, EventArgs e)
        {
            try
            {
                Query.Linie linia = ((PresenterLinie)sender).Linia;
                PresenterWariant pWariant = new PresenterWariant(linia);
                pWariant.FormText = "Linia " + linia.Nazwa.Trim();
                pWariant.MaterialSkinManager = MaterialSkinManager;
                pWariant.MaterialForm.Parent = View.FormControl;
                View.AddMaterialPanelTab(pWariant.MaterialForm, false);
                pWariant.ViewClosed += OnTabClosed;
                pWariant.DisplayPrzystanek += pWariant_DisplayPrzystanek;
            }
            catch (Exception)
            { }
        }

        void pWariant_DisplayPrzystanek(object sender, EventArgs e)
        {
            try
            {
                Query.WariantTrasy wariant = ((PresenterWariant)sender).Wariant;
                PresenterPrzystanek pPrzystanek = new PresenterPrzystanek(wariant);
                pPrzystanek.FormText = "Linia " + wariant.Linie.Nazwa.Trim() + " - " + wariant.Nazwa.Trim();
                pPrzystanek.MaterialSkinManager = MaterialSkinManager;
                pPrzystanek.MaterialForm.Parent = View.FormControl;
                View.AddMaterialPanelTab(pPrzystanek.MaterialForm, false);
                pPrzystanek.ViewClosed += OnTabClosed;
                pPrzystanek.DisplayPrzyjazd += pWariant_DisplayPrzyjazd;
            }
            catch (Exception)
            { }
        }

        private void pWariant_DisplayPrzyjazd(object sender, EventArgs e)
        {
            try
            {
                Query.Przystanek przystanek = ((PresenterPrzystanek)sender).Przystanek;
                PresenterPrzyjazd pPrzyjazd = new PresenterPrzyjazd(przystanek);
                pPrzyjazd.FormText = "Linia " + przystanek.WariantTrasy.Linie.Nazwa.Trim() + " - " + przystanek.WariantTrasy.Nazwa.Trim() + " - " + przystanek.Nazwa.Trim();
                pPrzyjazd.MaterialSkinManager = MaterialSkinManager;
                pPrzyjazd.MaterialForm.Parent = View.FormControl;
                View.AddMaterialPanelTab(pPrzyjazd.MaterialForm, false);
                pPrzyjazd.ViewClosed += OnTabClosed;
            }
            catch (Exception)
            { }
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
