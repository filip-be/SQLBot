using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialListView : ListView, IMaterialControl
	{
		[Browsable(false)]
		public int Depth { get; set; }
		[Browsable(false)]
		public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
		[Browsable(false)]
		public MouseState MouseState { get; set; }
		[Browsable(false)]
		public Point MouseLocation { get; set; }
        [Browsable(false)]
        Control editControl { get; set; }
        [Browsable(true)]
        public bool FitToWidth { get; set; }

		public MaterialListView()
		{
			GridLines = false;
			FullRowSelect = true;
			HeaderStyle = ColumnHeaderStyle.Nonclickable;
			View = View.Details;
			OwnerDraw = true;
			ResizeRedraw = true;
			BorderStyle = BorderStyle.None;
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            FitToWidth = false;

			//Fix for hovers, by default it doesn't redraw
			//TODO: should only redraw when the hovered line changed, this to reduce unnecessary redraws
			MouseLocation = new Point(-1, -1);
			MouseState = MouseState.OUT;
			MouseEnter += delegate
			{
				MouseState = MouseState.HOVER;
			}; 
			MouseLeave += delegate
			{
				MouseState = MouseState.OUT; 
				MouseLocation = new Point(-1, -1);
				Invalidate();
			};
			MouseDown += delegate { MouseState = MouseState.DOWN; };
			MouseUp += delegate{ MouseState = MouseState.HOVER; };
			MouseMove += delegate(object sender, MouseEventArgs args)
			{
				MouseLocation = args.Location;
				//Invalidate();
			};
		}

		protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
            base.OnDrawColumnHeader(e);

			e.Graphics.FillRectangle(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), new Rectangle(e.Bounds.X, e.Bounds.Y, Width, e.Bounds.Height));
            e.Graphics.DrawRectangle(new Pen(SkinManager.GetFlatButtonHoverBackgroundBrush()), new Rectangle(e.Bounds.X, e.Bounds.Y, Width, e.Bounds.Height));
			e.Graphics.DrawString(e.Header.Text, 
				SkinManager.ROBOTO_MEDIUM_10, 
				SkinManager.GetSecondaryTextBrush(),
				new Rectangle(e.Bounds.X , e.Bounds.Y , e.Bounds.Width  , e.Bounds.Height ), 
				getStringFormat());
		}

		private const int ITEM_PADDING = 12;
		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
            //Debug.WriteLine("OnDrawItem");
			//We draw the current line of items (= item with subitems) on a temp bitmap, then draw the bitmap at once. This is to reduce flickering.
			var b = new Bitmap(e.Item.Bounds.Width, e.Item.Bounds.Height);
			var g = Graphics.FromImage(b);
            
			//always draw default background
			g.FillRectangle(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
			
			if (e.State.HasFlag(ListViewItemStates.Selected))
			{
				//selected background
				g.FillRectangle(SkinManager.GetFlatButtonPressedBackgroundBrush(), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
			}
			else if (e.Bounds.Contains(MouseLocation) && MouseState == MouseState.HOVER)
			{
				//hover background
				g.FillRectangle(SkinManager.GetFlatButtonHoverBackgroundBrush(), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
			}

            
			//Draw seperator
            if(GridLines)
			    g.DrawLine(new Pen(SkinManager.GetDividersColor()), e.Bounds.Left, 0, e.Bounds.Right, 0);
			
			foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
			{
				//Draw text
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				g.DrawString(subItem.Text, SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetPrimaryTextBrush(),
								 new Rectangle(subItem.Bounds.Location.X , 0, subItem.Bounds.Width, subItem.Bounds.Height),
								 getStringFormat());
			}

			e.Graphics.DrawImage((Image) b.Clone(), e.Item.Bounds.Location);
			g.Dispose();
			b.Dispose();
		}

        protected ListViewItem clickedItem;
        protected int clickedCoordX;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            clickedItem = this.GetItemAt(e.X, e.Y);
            clickedCoordX = e.X;
        }

        protected override void OnDoubleClick(EventArgs e)
        //protected override void OnClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            //base.OnClick(e);

            int nStart = clickedCoordX;
            int spos = 0;
            if (this.Columns.Count < 1)
                return;
            int epos = this.Columns[0].Width;
            int subItemSelected = -1;
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (nStart > spos && nStart < epos)
                {
                    subItemSelected = i;
                    break;
                }
                spos = epos;
                epos += this.Columns[i].Width;
            }
            if (subItemSelected > -1 && clickedItem.SubItems.Count > subItemSelected && clickedItem.SubItems[subItemSelected] is MaterialListViewSubItem)
            {
                MaterialListViewSubItem clickedSubItem = (MaterialListViewSubItem)clickedItem.SubItems[subItemSelected];
                clickedSubItem.Location = new Point(spos, clickedSubItem.Bounds.Y + 5);

                ((MaterialListViewSubItem)clickedItem.SubItems[subItemSelected]).Clicked(this, e);
            }
        }

		private StringFormat getStringFormat()
		{
			return new StringFormat
			{
				FormatFlags = StringFormatFlags.LineLimit,
				Trimming = StringTrimming.EllipsisCharacter,
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Center
			};
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			//This is a hax for the needed padding.
			//Another way would be intercepting all ListViewItems and changing the sizes, but really, that will be a lot of work
			//This will do for now.
			Font = new Font(SkinManager.ROBOTO_MEDIUM_12.FontFamily, Font == null ? 24 : Font.Size);
		}

        protected override void WndProc(ref Message message)
        {
            const int WM_PAINT = 0xf;

            // if the control is in details view mode and columns
            // have been added, then intercept the WM_PAINT message
            // and reset the last column width to fill the list view
            switch (message.Msg)
            {
                case WM_PAINT:
                    if (FitToWidth && this.View == View.Details && this.Columns.Count > 0)
                        this.Columns[this.Columns.Count - 1].Width = -2;
                    break;
            }

            // pass messages on to the base control for processing
            base.WndProc(ref message);
        }
	}
}
