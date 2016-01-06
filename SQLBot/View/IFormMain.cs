using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cindalnet.SQLBot.View
{
    public interface IFormMain : IForm
    {
        void AddMaterialPanelTab(MaterialForm form, bool focus = false);
        void AddMaterialPanelTab(MaterialPanel panel, bool focus = false);

        bool RemoveMaterialPanelTab(string panelText);
        bool RemoveMaterialPanelTab(MaterialForm form);
        bool RemoveMaterialPanelTab(MaterialPanel panel);
    }
}
