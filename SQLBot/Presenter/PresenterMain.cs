﻿using Cindalnet.SQLBot.View;
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

            View.ProcessMessage += View_ProcessMessage;
        }



        unsafe void View_ProcessMessage(object sender, EventArgs e)
        {
            Console.WriteLine(View.Query);
            View.Response = View.Query;

            View.Response = Model.QueryParser.ParseQuery(View.Query);

            View.Query = "";
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
    }
}
