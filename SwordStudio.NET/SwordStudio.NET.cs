using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BOOL      = System.Boolean;
using CHAR      = System.Char;
using BYTE      = System.Byte;
using SHORT     = System.Int16;
using WORD      = System.UInt16;
using INT       = System.Int32;
using UINT      = System.UInt32;
using SDWORD    = System.Int32;
using DWORD     = System.UInt32;
using LPSTR     = System.String;

using UitlCtrls;
using PalCfg;
using PalGlobal;

using static PalMain.Pal_Main;
using static PalCfg.Pal_Cfg;
using static PalGlobal.Pal_Global;
using static PalGlobal.Pal_File;
using static PalCommon.Pal_Common;

namespace SwordStudio.NET
{
    public partial class SWORD : Form
    {
        Form_SceneSelect SceneSelect;

        public SWORD()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            SceneSelect.Dispose();
            this.Dispose();
        }

        private void SWORD_Load(object sender, EventArgs e)
        {
            main(null);

            SceneSelect = new Form_SceneSelect(this);
        }

        private void Open_ToolsBar_Word_Button_Click(object sender, EventArgs e)
        {
            SceneSelect.ShowDialog();

            if (SceneSelect._fIsEnter)
            {
                //
                // Load scene edit
                //
                Pal_Global.iiThisScene = SceneSelect._iThisScene;

            }
        }

        private void EventBlock_ToolsBar_Word_Button_Click(object sender, EventArgs e)
        {
            UtilButton  ubButton        = sender as UtilButton;
            Bitmap      bitmapNormal    = global::SwordStudio.NET.Properties.Resources.ToolBar_EventBlock;
            Bitmap      bitmapDisplay   = global::SwordStudio.NET.Properties.Resources.ToolBar_EventBlock_Display;

            if (ubButton != null)
            {
                ubButton.Image = ubButton.GetIsClink() ? bitmapNormal : bitmapDisplay;
            }
        }
    }
}
