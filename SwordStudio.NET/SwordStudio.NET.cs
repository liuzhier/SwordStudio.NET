using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UitlCtrls;

namespace SwordStudio.NET
{
    public partial class SWORD : Form
    {
        public SWORD()
        {
            InitializeComponent();
        }

        private void EventBlock_ToolsBar_Word_Button_Click(object sender, EventArgs e)
        {
            UtilButton  ubButton        = sender as UtilButton;
            Bitmap      bitmapNormal    = global::SwordStudio.NET.Properties.Resources.ToolBar_EventBlock;
            Bitmap      bitmapDisplay   = global::SwordStudio.NET.Properties.Resources.ToolBar_EventBlock_Display;

            if (ubButton != null)
            {
                ubButton.Image = ubButton.fIsClink ? bitmapNormal : bitmapDisplay;
            }
        }
    }
}
