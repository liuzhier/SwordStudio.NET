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
using PalVideo;
using PalMap;

using static PalMain.Pal_Main;
using static PalConfig.Pal_Config;
using static PalCfg.Pal_Cfg;
using static PalGlobal.Pal_Global;
using static PalGlobal.Pal_File;
using static PalCommon.Pal_Common;
using static PalVideo.Pal_Video;

namespace SwordStudio.NET
{
    public partial class SWORD : Form
    {
        Form_SceneSelect    SceneSelect;
        BOOL                fIsInitialLoad      = FALSE;
        INT                 _iFirstMapTitleNum   = -1;
        INT                 _iThisMapTitleNum    = -1;

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
            LoadAboutLabelBack();

            main(null);

            SceneSelect = new Form_SceneSelect(this);
        }

        private void SWORD_Resize(object sender, EventArgs e)
        {
            switch (Main_TabControl.SelectedIndex)
            {
                case 0:
                    {
                        if (ReadMe_About_Label.Width <= 0 || ReadMe_About_Label.Height <= 0) break;

                        LoadAboutLabelBack();
                    }
                    break;

                case 1:
                    {
                        if (!fIsInitialLoad) break;

                        LoadMapTileList(_iThisMapTitleNum);
                    }
                    break;

                default:
                    {

                    }
                    break;
            }
        }

        private void LoadAboutLabelBack()
        {
            ReadMe_About_Label.Image = global::SwordStudio.NET.Properties.Resources.AboutBack;
            ReadMe_About_Label.Image = Video_ChangeImageSize(ReadMe_About_Label.Image, ReadMe_About_Label.Width, ReadMe_About_Label.Height);
        }

        private void LoadMapTileList(INT iFirstMapTitleNum = -1)
        {
            INT         i, iThisMapTitleNum;
            PictureBox  pictureBox;

            //
            // Check if this is the first time loading the map
            //
            iThisMapTitleNum = (iFirstMapTitleNum != -1) ? iFirstMapTitleNum : 0;

            //
            // Load map tile list
            //
            for (i = 0; i < BitmapList_World_TableLayoutPanel.RowCount; i++, iThisMapTitleNum++)
            {
                BitmapList_World_TableLayoutPanel.GetControlFromPosition(0, i).Text = $"0x{iThisMapTitleNum:X4}\n{iThisMapTitleNum:D5}";

                BitmapList_World_TableLayoutPanel.GetControlFromPosition(0, i).BackColor = (iThisMapTitleNum == _iThisMapTitleNum) ? Color.LightPink : Color.MistyRose;

                pictureBox = (PictureBox)BitmapList_World_TableLayoutPanel.GetControlFromPosition(1, i);

                PAL_RLEBlitToSurface(PAL_SpriteGetFrame(Pal_Map.TileSprite, iThisMapTitleNum), Pal_Map.TileSurface.CleanSpirit(), PAL_XY(0, 0));
                Video_DrawEnlargeBitmap(Pal_Map.TileSurface, Pal_Map.TileImage, 1);
                pictureBox.Image = Video_ChangeImageSize(Pal_Map.TileImage, pictureBox.Width, pictureBox.Height);

                pictureBox.Refresh();
            }

            if (iFirstMapTitleNum == -1)
            {
                //
                // First time loading map
                //
                pictureBox                          = (PictureBox)BitmapList_World_TableLayoutPanel.GetControlFromPosition(1, 0);
                BitmapID_ThisBitmap_Label.Text      = $"[0x{0:X4}]\n{0:D5}";
                Picture_ThisBitmap_PictureBox.Image = Video_ChangeImageSize(pictureBox.Image, Picture_ThisBitmap_PictureBox.Width, Picture_ThisBitmap_PictureBox.Height);
                BitmapList_World_TableLayoutPanel.GetControlFromPosition(0, 0).BackColor = Color.LightPink;
            }
            else
            {
                //
                // When changing window size
                //
                Picture_ThisBitmap_PictureBox.Image = Video_ChangeImageSize(Picture_ThisBitmap_PictureBox.Image, Picture_ThisBitmap_PictureBox.Width, Picture_ThisBitmap_PictureBox.Height);
                BitmapList_World_TableLayoutPanel.GetControlFromPosition(0, 0).BackColor = Color.LightPink;
            }
        }

        private void Open_ToolsBar_Word_Button_Click(object sender, EventArgs e)
        {
            SceneSelect.ShowDialog();

            if (SceneSelect._fIsEnter)
            {
                //
                // Load scene edit
                //
                Pal_Global.iThisScene = SceneSelect._iThisScene;

                //
                // Enable all disabled controls
                //
                if (fIsInitialLoad = !fIsInitialLoad)
                {
                    Save_ToolsBar_Word_Button.Enabled                               = TRUE;
                    EditMode_ToolsBarGroup_World_FlowLayoutPanel.Enabled            = TRUE;
                    BlockDisplayMode_ToolsBarGroup_World_FlowLayoutPanel.Enabled    = TRUE;
                    LayerMode_ToolsBarGroup_World_FlowLayoutPanel.Enabled           = TRUE;
                    ActualBox_ContentBox_World_SplitContainer.Enabled               = TRUE;

                    foreach (Control control in BitmapList_World_TableLayoutPanel.Controls)
                    {
                        control.Click += new System.EventHandler(BitmapList_World_TableLayoutPanel_MouseClick);
                    }
                }

                Message_Status_Word_ToolStripStatusLabel.Text   = $"{Pal_Map.iMapNum}=>[0x{Pal_Global.iThisScene:X4}] {Pal_Global.iThisScene:D5} ";
                Message_Status_Word_ToolStripStatusLabel.Text  += Pal_Cfg_GetCfgNodeItem(lpszSceneDesc, $"0x{(Pal_Global.iThisScene + 1):X4}").lpszTitle;
                ScrollBar_BitmapList_Word_VScrollBar.Value      = 0;
                ScrollBar_BitmapList_Word_VScrollBar.Maximum    = PAL_SpriteGetNumFrames(Pal_Map.TileSprite) - 1;

                //
                // Load map titles selection area
                //
                _iFirstMapTitleNum  = 0;
                _iThisMapTitleNum   = _iFirstMapTitleNum;
                LoadMapTileList();
            }
        }

        private void BitmapList_World_TableLayoutPanel_MouseClick(object sender, EventArgs e)
        {
            Control             control = sender as Control;
            TableLayoutPanel    tableLayoutPanel;
            INT                 iRow;

            if (control != null)
            {
                tableLayoutPanel    = (TableLayoutPanel)control.Parent;
                iRow                = tableLayoutPanel.GetRow(control);

                if (iRow != -1)
                {
                    _iThisMapTitleNum = _iFirstMapTitleNum + iRow;

                    foreach (Control ThisBrother in tableLayoutPanel.Controls) ThisBrother.BackColor = Color.MistyRose;

                    tableLayoutPanel.GetControlFromPosition(0, iRow).BackColor = Color.LightPink;

                    BitmapID_ThisBitmap_Label.Text      = $"[0x{_iThisMapTitleNum:X4}]\n{_iThisMapTitleNum:D5}";
                    Picture_ThisBitmap_PictureBox.Image = Video_ChangeImageSize(((PictureBox)tableLayoutPanel.GetControlFromPosition(1, iRow)).Image,
                    Picture_ThisBitmap_PictureBox.Width, Picture_ThisBitmap_PictureBox.Height);
                }
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

        private void ScrollBar_BitmapList_Word_VScrollBar_ValueChanged(object sender, EventArgs e)
        {
            VScrollBar vScrollBar = sender as VScrollBar;
            
            if (vScrollBar != null)
            {
                _iFirstMapTitleNum = vScrollBar.Value;
                LoadMapTileList(_iFirstMapTitleNum);
            }
        }
    }
}
