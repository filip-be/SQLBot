using Cindalnet.SQLBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Cindalnet.SQLBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Properties.Settings.Default.BazaRelacyjnaCustomConnectionString 
            //    = Properties.Settings.Default.BazaRelacyjnaConnectionString;
            Properties.Settings.Default.BazaRelacyjnaCustomConnectionString
                = Properties.Settings.Default.BazaRelacyjnaConnectionStringLocal;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Presenter.PresenterMain().FormControl);
        }
    }
}
