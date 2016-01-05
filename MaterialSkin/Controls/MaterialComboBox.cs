using MaterialSkin.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public class MaterialComboBox : ComboBox, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        private readonly AnimationManager animationManager;

        public MaterialComboBox()
        {
            BackColor = SkinManager.GetApplicationBackgroundColor();
            
            animationManager = new AnimationManager
            {
                Increment = 0.06,
                AnimationType = AnimationType.EaseInOut,
                InterruptAnimation = false
            };
            this.GotFocus += (sender, args) => animationManager.StartNewAnimation(AnimationDirection.In);
            this.LostFocus += (sender, args) => animationManager.StartNewAnimation(AnimationDirection.Out);
            this.Font = SkinManager.ROBOTO_MEDIUM_10;
            this.ForeColor = SkinManager.GetPrimaryTextColor();
        }
    }
}
