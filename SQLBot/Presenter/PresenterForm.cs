using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cindalnet.SQLBot.Presenter
{
    public class PresenterForm
    {
        public MaterialForm MaterialForm { get; set; }
        public Form FormControl { get; set; }

        protected MaterialSkin.MaterialSkinManager _MaterialSkinManager;
        public MaterialSkin.MaterialSkinManager MaterialSkinManager
        {
            get
            {
                return _MaterialSkinManager;
            }
            set
            {
                _MaterialSkinManager = value;
                if (MaterialForm != null)
                    _MaterialSkinManager.AddFormToManage(MaterialForm);
            }
        }

        public PresenterForm()
        {
        }
    }
}
