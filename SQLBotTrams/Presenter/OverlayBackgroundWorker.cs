using Cindalnet.SQLBot.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cindalnet.SQLBot.Presenter
{
    public class OverlayBackgroundWorker : BackgroundWorker
    {
        public Control DisplayControl { get; set; }
        public string LoadingLabel { get; set; }
        protected CFormLoading Overlay;

        public OverlayBackgroundWorker()
            : base()
        {
            LoadingLabel = "Ładowanie";
        }

        //
        // Summary:
        //     Starts execution of a background operation.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     System.ComponentModel.BackgroundWorker.IsBusy is true.
        public new void RunWorkerAsync()
        {
            ShadeControl();
            base.RunWorkerAsync();
        }
        //
        // Summary:
        //     Starts execution of a background operation.
        //
        // Parameters:
        //   argument:
        //     A parameter for use by the background operation to be executed in the System.ComponentModel.BackgroundWorker.DoWork
        //     event handler.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     System.ComponentModel.BackgroundWorker.IsBusy is true.
        public new void RunWorkerAsync(object argument)
        {
            ShadeControl();
            base.RunWorkerAsync(argument);
        }

        public void ShadeControl()
        {
            if (DisplayControl != null)
            {
                // "Gray" control
                Overlay = new CFormLoading(DisplayControl);
                Overlay.LoadingLabel = LoadingLabel;
                Overlay.Show();
            }
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            if (DisplayControl != null && Overlay != null)
            {
                // "Ungray" control
                Overlay.Hide();
                //Overlay.Parent.Controls.Remove(Overlay);
                Overlay.Dispose();
                Overlay = null;
            }
            base.OnRunWorkerCompleted(e);
        }

        protected override void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (WorkerReportsProgress && Overlay != null)
            {
                Overlay.ProgressChanged(this, e);
            }
            base.OnProgressChanged(e);
        }
    }
}
