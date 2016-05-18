using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cindalnet.SQLBot.View
{
    public partial class CFormLoading : Form, IDisposable
    {
        public ProgressChangedEventHandler ProgressChanged;
        public string LoadingLabel
        {
            get
            {
                return labelLoading.Text;
            }
            set
            {
                labelLoading.Text = value;
            }
        }

        public CFormLoading()
        {
            InitializeComponent();
            Initialize(null);
        }

        public CFormLoading(Control HoveredControl)
        {
            InitializeComponent();
            Initialize(HoveredControl);
        }

        private Control TopParentControl { get; set; }
        private Control HoveredControl { get; set; }
        
        private void Initialize(Control hoveredControl)
        {
            this.TopLevel = false;
            
            if (hoveredControl != null)
            {
                this.HoveredControl = hoveredControl;
                this.TopParentControl = HoveredControl.Parent;

                this.Visible = HoveredControl.Visible;
                this.Location = HoveredControl.Location;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = HoveredControl.PointToScreen(Point.Empty);
                this.ClientSize = HoveredControl.ClientSize;
                BindHoveredControlEventHandlers();
                BindTopParentEventHandlers();
            }
            ProgressChanged += OnProgressChanged;
        }

        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelProgress.Text = e.ProgressPercentage + " %";
        }

        private void BindHoveredControlEventHandlers()
        {
            if (this.HoveredControl != null)
            {
                HoveredControl.VisibleChanged += OnVisibleChanged;
                HoveredControl.LocationChanged += OnLocationChanged;
                HoveredControl.SizeChanged += OnSizeChanged;
                HoveredControl.ClientSizeChanged += OnSizeChanged;
                HoveredControl.Move += OnMove;
                HoveredControl.Invalidated += OnInvalidate;
                HoveredControl.RegionChanged += OnRegionChanged;
                HoveredControl.GotFocus += OnVisibleChanged;
                HoveredControl.LostFocus += OnVisibleChanged;

                HoveredControl.ParentChanged += OnParentChanged;
            }
        }        

        private void DisposeHoveredControlEventHandlers()
        {
            if(this.HoveredControl != null)
            {
                HoveredControl.VisibleChanged -= OnVisibleChanged;
                HoveredControl.LocationChanged -= OnLocationChanged;
                HoveredControl.SizeChanged -= OnSizeChanged;
                HoveredControl.ClientSizeChanged -= OnSizeChanged;
                HoveredControl.Move -= OnMove;
                HoveredControl.Invalidated -= OnInvalidate;
                HoveredControl.RegionChanged -= OnRegionChanged;
                HoveredControl.ParentChanged -= OnParentChanged;
            }
        }

        private void BindTopParentEventHandlers()
        {
            Control control = this.TopParentControl;
            while (control != null)
            {
                control.LocationChanged += OnLocationChanged;
                control.SizeChanged += OnSizeChanged;
                control.ClientSizeChanged += OnSizeChanged;
                control.Move += OnMove;
                control.ParentChanged += OnParentChanged;
                
                if(control is TabControl)
                {   // In tabpanel
                    ((TabControl)control).SelectedIndexChanged += tabControl_SelectedIndexChanged;
                }
                if (control.Parent == null)
                {   // TopLevel Parent - propably MainFrame
                    this.Parent = control;
                }
                control = control.Parent;
            }
        }

        private void DisposeTopParentEventHandlers()
        {
            Control control = this.TopParentControl;
            while (control != null)
            {
                control.LocationChanged -= OnLocationChanged;
                control.SizeChanged -= OnSizeChanged;
                control.ClientSizeChanged -= OnSizeChanged;
                control.Move -= OnMove;
                control.ParentChanged -= OnParentChanged;
                if (control is TabControl)
                {   // In tabpanel
                    ((TabControl)control).SelectedIndexChanged -= tabControl_SelectedIndexChanged;
                }
                control = control.Parent;
            }
            UpdateVisibilityIncludingTabs();
        }

        private void DisposeEventHandlers()
        {
            DisposeTopParentEventHandlers();
            DisposeHoveredControlEventHandlers();
        }

        public new void Dispose()
        {
            DisposeEventHandlers();
            base.Dispose();
        }

        private void OnParentChanged(object sender, EventArgs e)
        {
            if (sender is Control && HoveredControl.Parent != null)
            {
                DisposeTopParentEventHandlers();
                this.TopParentControl = HoveredControl.Parent;
                BindTopParentEventHandlers();
            }
        }

        private void UpdateSizeAndLocation()
        {
            if(HoveredControl != null)
            {
                //this.Location = HoveredControl.PointToScreen(Point.Empty);
                if (Parent != null)
                {
                    this.Location = Parent.PointToClient(HoveredControl.PointToScreen(Point.Empty));
                }
                
                if (this.Location.X < 0 || this.Location.Y < 0)
                {
                    this.Location = new Point(0, 60);
                }

                this.ClientSize = HoveredControl.ClientSize;
                this.Focus();
                this.Activate();
                this.BringToFront();
            }
        }

        private void OnRegionChanged(object sender, EventArgs e)
        {
            UpdateSizeAndLocation();
        }

        private void OnInvalidate(object sender, InvalidateEventArgs e)
        {
            UpdateSizeAndLocation();
        }

        private void OnMove(object sender, EventArgs e)
        {
            UpdateSizeAndLocation();
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            UpdateSizeAndLocation();
        }

        private void OnLocationChanged(object sender, EventArgs e)
        {
            UpdateSizeAndLocation();
        }

        private void UpdateVisibilityIncludingTabs()
        {
            if(HoveredControl != null && !HoveredControl.IsDisposed)
            {
                try
                {
                    this.Visible = HoveredControl.Visible;
                }
                catch(Exception)
                { }
            }

            Control control = this.TopParentControl;
            while (control != null)
            {
                if (control is TabPage && control.Parent != null && control.Parent is TabControl)
                {   // In tabpanel
                    if (!(((TabControl)control.Parent).SelectedTab == control))
                    {
                        this.Visible = false;
                    }
                }
                control = control.Parent;
            }
        }
        
        private void OnVisibleChanged(object sender, EventArgs e)
        {
            if(sender is Control)
            {
                UpdateVisibilityIncludingTabs();
            }else
            {
                
            }

        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateVisibilityIncludingTabs();
            /*
            Control control = this.TopParentControl;
            while (control != null)
            {
                if (control is TabPage && control.Parent != null && control.Parent is TabControl && control.Parent == sender)
                {   // In tabpanel
                    this.Visible = ((TabControl)sender).SelectedTab == control;
                    break;
                }
                control = control.Parent;
            }
             * */
        }
    }
}
