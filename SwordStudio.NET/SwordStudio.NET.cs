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
using SDWORD    = System.Int64;
using DWORD     = System.UInt64;
using LPSTR     = System.String;

using UitlCtrls;
using PalCfg;
using PalGlobal;

using static PalMain.Pal_Main;
using static PalCfg.Pal_Cfg;
using static PalGlobal.Pal_Global;
using static PalGlobal.PAL_File;
using static PalCommon.Pal_Common;

namespace SwordStudio.NET
{
    public partial class SWORD : Form
    {
        public SWORD()
        {
            InitializeComponent();
        }

        private void SWORD_Load(object sender, EventArgs e)
        {
            main(null);
        }

        private void Open_ToolsBar_Word_Button_Click(object sender, EventArgs e)
        {
            PAL_File        pfFile_Map, pfFile_SSS;
            INT             iEventIndex, iSceneIndex, iSizeOfEvent, iSizeOfScene, iSceneCount;

            //
            // Get the map file node
            //
            pfFile_Map      = Pal_File_GetFile(lpszGameMap);

            //
            // Get file indexes for events and scenes
            //
            //iEventIndex = Pal_Cfg_GetCfgNodeItemIndex(lpszMainData, lpszEvent);
            iSceneIndex     = Pal_Cfg_GetCfgNodeItemIndex(lpszMainData, lpszScene);

            //
            // Get main file data
            //
            pfFile_SSS      = Pal_File_GetFile(lpszMainData);

            //
            // Get the size of scenes data
            //
            //iEventIndex = PAL_MKFGetChunkSize(iEventIndex, ref pfFile_SSS.bufFile);
            iSceneIndex     = PAL_MKFGetChunkSize(iSceneIndex, ref pfFile_SSS.bufFile);

            //
            // Get the size of each group of events and scenes
            //
            //iSizeOfEvent    = Pal_Cfg_GetChunkSize(lpszEvent);
            iSizeOfScene    = Pal_Cfg_GetChunkSize(lpszScene);

            //
            // Get the number of scenes
            //
            iSceneCount     = iSceneIndex / iSizeOfScene;
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
