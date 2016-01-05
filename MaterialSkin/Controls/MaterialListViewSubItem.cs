using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public class MaterialListViewSubItem : ListViewItem.ListViewSubItem, INotifyPropertyChanged
    {
        public enum SubItemEditType
        {
            TYPE_TEXT,
            TYPE_PASSWORD,
            TYPE_FLOAT,
            TYPE_DECIMAL,
            TYPE_DATETIME,
            TYPE_CHECKBOX
        }
        
        public SubItemEditType Type { get; set; }
        public Control EditControl { get; set; }
        public Point Location { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public new string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text == value)
                    return;
                base.Text = value;
                if (PropertyChanged != null)
                    PropertyChanged(base.Text, new PropertyChangedEventArgs(this.Name));
            }
        }

        // Summary:
        //     Initializes a new instance of the MaterialSkin.Controls.MaterialListViewSubItem
        //     class with default values.
        // Parameters:
        //   type:
        //     A MaterialSkin.Controls.SubItemEditType that represents the type of item
        //
        //   parent:
        //     A System.Windows.Forms.Control object that represents parent item for editable objects
        public MaterialListViewSubItem(SubItemEditType type, Control parent)
            : base()
        {
            Type = type;
            CreateEditControl(parent);
        }
        //
        // Summary:
        //     Initializes a new instance of the MaterialSkin.Controls.MaterialListViewSubItem
        //     class with the specified owner and text.
        //
        // Parameters:
        //   type:
        //     A MaterialSkin.Controls.SubItemEditType that represents the type of item
        //
        //   parent:
        //     A System.Windows.Forms.Control object that represents parent item for editable objects
        //
        //   owner:
        //     A System.Windows.Forms.ListViewItem that represents the item that owns the
        //     subitem.
        //
        //   text:
        //     The text to display for the subitem.
        public MaterialListViewSubItem(SubItemEditType type, Control parent, ListViewItem owner, string text) 
            : base(owner, text)
        {
            Type = type;
            CreateEditControl(parent);
        }
        //
        // Summary:
        //     Initializes a new instance of the MaterialSkin.Controls.MaterialListViewSubItem
        //     class with the specified owner, text, foreground color, background color,
        //     and font values.
        //
        // Parameters:
        //   type:
        //     A MaterialSkin.Controls.SubItemEditType that represents the type of item
        //
        //   owner:
        //     A System.Windows.Forms.ListViewItem that represents the item that owns the
        //     subitem.
        //
        //   text:
        //     The text to display for the subitem.
        //
        //   parent:
        //     A System.Windows.Forms.Control object that represents parent item for editable objects
        //
        //   foreColor:
        //     A System.Drawing.Color that represents the foreground color of the subitem.
        //
        //   backColor:
        //     A System.Drawing.Color that represents the background color of the subitem.
        //
        //   font:
        //     A System.Drawing.Font that represents the font to display the subitem's text
        //     in.
        public MaterialListViewSubItem(SubItemEditType type, Control parent, ListViewItem owner, string text, Color foreColor, Color backColor, Font font)
            : base(owner, text, foreColor, backColor, font)
        {
            Type = type;
            CreateEditControl(parent);
        }
    
        protected void CreateEditControl(Control parent)
        {
            switch(Type)
            {
                case SubItemEditType.TYPE_TEXT:
                case SubItemEditType.TYPE_PASSWORD:
                case SubItemEditType.TYPE_FLOAT:
                case SubItemEditType.TYPE_DECIMAL:
                    MaterialSingleLineTextField ctrlT = new MaterialSingleLineTextField();
                    //EditControl = new TextBox();
                    ctrlT.Parent = parent;
                    ctrlT.Size = new Size(0, 0);
                    ctrlT.Location = new System.Drawing.Point(0, 0);
                    ctrlT.Anchor = AnchorStyles.Left;
                    ctrlT.TextChanged += new EventHandler(this.TextChanged);
                    ctrlT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditOver);
                    ctrlT.LostFocus += new System.EventHandler(this.EditFocusOver);
                    ctrlT.Hide();
                    ctrlT.Text = "";
                    ctrlT.AcceptsReturn = true;
                    EditControl = ctrlT;
                    
                    break;
                case SubItemEditType.TYPE_DATETIME:
                    DateTimePicker ctrlD = new DateTimePicker();
                    ctrlD.Parent = parent;
                    ctrlD.Size = new Size(0, 0);
                    ctrlD.Location = new System.Drawing.Point(0, 0);
                    ctrlD.Anchor = AnchorStyles.Left;
                    ctrlD.TextChanged += new EventHandler(this.TextChanged);
                    ctrlD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditOver);
                    ctrlD.LostFocus += new System.EventHandler(this.EditFocusOver);
                    ctrlD.Hide();
                    ctrlD.Text = "";
                    ctrlD.Format = DateTimePickerFormat.Custom;
                    ctrlD.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                    EditControl = ctrlD;
                    break;
                case SubItemEditType.TYPE_CHECKBOX:
                    MaterialCheckBox ctrlC = new MaterialCheckBox();
                    ctrlC.Parent = parent;
                    ctrlC.Size = new Size(0, 0);
                    ctrlC.Location = new System.Drawing.Point(0, 0);
                    ctrlC.Anchor = AnchorStyles.Left;
                    ctrlC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditOver);
                    ctrlC.LostFocus += new System.EventHandler(this.EditFocusOver);
                    ctrlC.CheckStateChanged += new System.EventHandler(this.EditFocusOver);
                    ctrlC.Hide();
                    ctrlC.Text = "";
                    EditControl = ctrlC;
                    break;
                default:
                    break;
            }

            Clicked += OnClicked;
        }

        private void TextChanged(object sender, EventArgs e)
        {
            if (EditControl != null)
            {
                ////////////////////////////////////////////////////
                // INPUT MUST VALIDATED HERE
                //  WHETER IT IS FLOAT/DECIMAL!
                ////////////////////////////////////////////////////
                int selectionStart = -1;
                string newText = EditControl.Text;

                if(Type == SubItemEditType.TYPE_FLOAT || Type == SubItemEditType.TYPE_DECIMAL)
                {
                    newText = Regex.Replace(newText, @"\s", "");
                    selectionStart = ((MaterialSingleLineTextField)EditControl).SelectionStart;
                }

                bool isValid = true;
                switch (Type)
                {
                    case SubItemEditType.TYPE_DECIMAL:
                        if (!Regex.IsMatch(newText, @"^\d+\z"))
                        {
                            isValid = false;
                        }
                        break;
                    case SubItemEditType.TYPE_FLOAT:
                        if (!(Regex.IsMatch(newText, @"^\d+\z") 
                            || Regex.IsMatch(newText, @"^\d+\,\z")
                            || Regex.IsMatch(newText, @"^\d+\,\d+\z")))
                        {
                            isValid = false;
                        }
                        break;
                    case SubItemEditType.TYPE_DATETIME:
                        try
                        {
                            EditControl.Text = newText;
                        }catch(Exception)
                        {
                            isValid = false;
                        }
                        break;
                    default:
                        break;
                }

                if(isValid)
                {
                    if(!newText.EndsWith("."))
                        this.Text = newText;
                    EditControl.Text = newText;
                }else
                {
                    if (Type == SubItemEditType.TYPE_FLOAT || Type == SubItemEditType.TYPE_DECIMAL)
                    {
                        selectionStart -= newText.Length - this.Text.Length;
                        if (selectionStart > -1)
                        {
                            ((MaterialSingleLineTextField)EditControl).SelectionStart =
                                    selectionStart > Text.Length ? Text.Length : selectionStart;
                        }
                    }

                    EditControl.Text = this.Text;
                    System.Media.SystemSounds.Beep.Play();
                }
            }
        }

        protected void EditOver(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (EditControl != null)
            {
                if (e.KeyChar == 13)
                {
                    //Console.WriteLine("EditOver13");
                    this.Text = EditControl.Text;
                    EditControl.Hide();
                }
                else if (e.KeyChar == 27)
                {
                    //Console.WriteLine("EditOver27");
                    EditControl.Hide();
                }
            }
        }

        public void EditFocusOver(object sender, System.EventArgs e)
        {
            if (EditControl != null)
            {
                //Console.WriteLine("FocusOver");
                this.Text = EditControl.Text;
                EditControl.Hide();
            }
        }

        public EventHandler Clicked;

        protected void OnClicked(object sender, EventArgs e)
        {
            if (EditControl != null)
            {
                //EditControl.Show();
                EditControl.Visible = true;
                EditControl.Text = Text;
                EditControl.Size = Bounds.Size;
                EditControl.Location = Location;
                switch(Type)
                {
                    case SubItemEditType.TYPE_TEXT:
                    case SubItemEditType.TYPE_PASSWORD:
                    case SubItemEditType.TYPE_FLOAT:
                    case SubItemEditType.TYPE_DECIMAL:
                        ((MaterialSingleLineTextField)EditControl).SelectAll();
                        //((TextBox)EditControl).SelectAll();
                        //EditControl.Focus();
                        break;
                    case SubItemEditType.TYPE_DATETIME:
                        break;
                    default:
                        break;
                }
                
            }
        }
    }
}
