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

using PAL_POS   = System.UInt32;

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
using static PalMap.Pal_Map;
using static PalRes.Pal_Res;

namespace SwordStudio.NET
{
    public partial class SWORD : Form
    {
        Form_SceneSelect    SceneSelect;
        BOOL                fIsInitialLoad      = FALSE;
        INT                 _iFirstMapTitleNum  = -1;
        INT                 _iThisMapTitleNum   = -1;

        Surface             sfWorldViewport;
        PAL_POS             dwMaxMapPos;
        PAL_POS             posWorldViewport    = PAL_XY(0, 0);
        Rect                rectWorldViewport   = new Rect();

        BOOL                fIsMovingMap        = FALSE;
        Point               MouseBeginPos;
        Point               MouseEndPos;

        PAL_EDIT_MODE       pemEditMode         = PAL_EDIT_MODE.None;
        PAL_DISPLAY_MODE    pdmDisplayMode      = PAL_DISPLAY_MODE.None | PAL_DISPLAY_MODE.LowTile | PAL_DISPLAY_MODE.HighTile;

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

            SceneSelect                     = new Form_SceneSelect(this);

            WorldViewport_PictureBox.Image  = new Bitmap(WorldViewport_PictureBox.Width, WorldViewport_PictureBox.Height);
            sfWorldViewport                 = new Surface(WorldViewport_PictureBox.Width, WorldViewport_PictureBox.Height);

            dwMaxMapPos                     = PAL_XY((WORD)(Pal_Map.wMapHeight - sfWorldViewport.w), (WORD)(Pal_Map.wMapHeight - sfWorldViewport.h));

            rectWorldViewport.x             = PAL_X(posWorldViewport);
            rectWorldViewport.y             = PAL_Y(posWorldViewport);
            rectWorldViewport.w             = WorldViewport_PictureBox.Width;
            rectWorldViewport.h             = WorldViewport_PictureBox.Height;
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
                        if (WorldViewport_PictureBox.Image.Width != WorldViewport_PictureBox.Width || WorldViewport_PictureBox.Image.Height != WorldViewport_PictureBox.Height)
                        {
                            WorldViewport_PictureBox.Image = new Bitmap(WorldViewport_PictureBox.Width, WorldViewport_PictureBox.Height);

                            rectWorldViewport.w = WorldViewport_PictureBox.Width;
                            rectWorldViewport.h = WorldViewport_PictureBox.Height;
                        }

                        if (!fIsInitialLoad) break;

                        PAL_DrawMapToSurface(sfWorldViewport, rectWorldViewport, WorldViewport_PictureBox, posWorldViewport);

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

                    //
                    // Initialize toolbar button status
                    //
                    Select_ToolsBar_Word_Button.PerformClick();
                    LowLayer_ToolsBar_Word_Button.PerformClick();
                    NoPassBlock_ToolsBar_Word_Button.PerformClick();
                    EventBlock_ToolsBar_Word_Button.PerformClick();
                }

                Message_Status_Word_ToolStripStatusLabel.Text   = $"{Pal_Map.iMapNum}=>[0x{Pal_Global.iThisScene:X4}] {Pal_Global.iThisScene:D5} ";
                Message_Status_Word_ToolStripStatusLabel.Text  += Pal_Cfg_GetCfgNodeItem(lpszSceneDesc, $"0x{(Pal_Global.iThisScene + 1):X4}").lpszTitle;
                ScrollBar_BitmapList_Word_VScrollBar.Value      = 0;
                ScrollBar_BitmapList_Word_VScrollBar.Maximum    = PAL_SpriteGetNumFrames(Pal_Map.TileSprite) - 1;
                ScrollTD_ScrollBoxT_ActualBoxR_VScrollBar.Value = 0;
                ScrollLR_ScrollBoxD_ActualBoxR_HScrollBar.Value = 0;

                //
                // Load map titles selection area
                //
                _iFirstMapTitleNum  = 0;
                _iThisMapTitleNum   = _iFirstMapTitleNum;
                LoadMapTileList();

                //
                // Load World Map
                //
                posWorldViewport    = Pal_Map.dwMinMapPos;
                rectWorldViewport.x = PAL_X(posWorldViewport);
                rectWorldViewport.y = PAL_Y(posWorldViewport);
                PAL_DrawMapToSurface(sfWorldViewport, rectWorldViewport, WorldViewport_PictureBox, posWorldViewport);
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

        private void Select_ToolsBar_Word_Button_Click(object sender, EventArgs e)
        {
            pemEditMode = PAL_EDIT_MODE.Select;
        }

        private void Edit_ToolsBar_Word_Button_Click(object sender, EventArgs e)
        {
            pemEditMode = PAL_EDIT_MODE.Edit;
        }

        private void Delete_ToolsBar_Word_Button_Click(object sender, EventArgs e)
        {
            pemEditMode = PAL_EDIT_MODE.Delete;
        }

        private void NoPassBlock_ToolsBar_Word_Button_Click(object sender, EventArgs e)
        {
            UtilButton  ubButton        = sender as UtilButton;

            if (ubButton != null)
            {
                if ((pdmDisplayMode & PAL_DISPLAY_MODE.NoPassTile) != 0)
                    pdmDisplayMode ^= PAL_DISPLAY_MODE.NoPassTile;
                else
                    pdmDisplayMode |= PAL_DISPLAY_MODE.NoPassTile;

                PAL_DrawMapToSurface(sfWorldViewport, rectWorldViewport, WorldViewport_PictureBox, posWorldViewport, pdmDisplayMode);
            }
        }

        private void EventBlock_ToolsBar_Word_Button_Click(object sender, EventArgs e)
        {
            UtilButton  ubButton        = sender as UtilButton;
            Bitmap      bitmapNormal    = global::SwordStudio.NET.Properties.Resources.ToolBar_EventBlock;
            Bitmap      bitmapDisplay   = global::SwordStudio.NET.Properties.Resources.ToolBar_EventBlock_Display;

            if (ubButton != null)
            {
                ubButton.Image  = ubButton.GetIsClink() ? bitmapNormal : bitmapDisplay;

                if ((pdmDisplayMode & PAL_DISPLAY_MODE.EventSprite) != 0)
                    pdmDisplayMode ^= PAL_DISPLAY_MODE.EventSprite;
                else
                    pdmDisplayMode |= PAL_DISPLAY_MODE.EventSprite;

                PAL_DrawMapToSurface(sfWorldViewport, rectWorldViewport, WorldViewport_PictureBox, posWorldViewport, pdmDisplayMode);
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

        private void WorldViewport_PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if ((pemEditMode & PAL_EDIT_MODE.Select) != 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    fIsMovingMap = TRUE;

                    MouseBeginPos = Cursor.Position;
                }
            }
            else if ((pemEditMode & PAL_EDIT_MODE.Edit) != 0)
            {
                Point   mousePosition;
                WORD x, y, h;

                mousePosition = WorldViewport_PictureBox.PointToClient(Cursor.Position);

                x = (WORD)(sfWorldViewport.w * ((double)mousePosition.X / WorldViewport_PictureBox.Width));
                y = (WORD)(sfWorldViewport.h * ((double)mousePosition.Y / WorldViewport_PictureBox.Height));

                PAL_POS_TO_XYH(PAL_XY(x, y), out x, out y, out h);
                posClick = PAL_XYH_TO_POS(x, y, h);

                PAL_RLEBlitToSurface(bitmapNoPass, sfWorldViewport, posClick);

                //
                // Display the currently selected scene image
                //
                Video_DrawEnlargeBitmap(sfMapViewport, WorldViewport_PictureBox.Image, -1);
                WorldViewport_PictureBox.Refresh();
            }
        }

        private void WorldViewport_PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) fIsMovingMap = false;
        }

        private void WorldViewport_PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition;
            WORD x, y, h;

            mousePosition = WorldViewport_PictureBox.PointToClient(Cursor.Position);

            x = (WORD)(sfWorldViewport.w * ((double)mousePosition.X / WorldViewport_PictureBox.Width));
            y = (WORD)(sfWorldViewport.h * ((double)mousePosition.Y / WorldViewport_PictureBox.Height));

            PAL_POS_TO_XYH(PAL_XY(x, y), out x, out y, out h);
            posClick = PAL_XYH_TO_POS(x, y, h);

            PAL_DrawMapToSurface(sfWorldViewport, rectWorldViewport, WorldViewport_PictureBox, posWorldViewport, pdmDisplayMode);

            if (!fIsMovingMap) return;

            MouseEndPos     = Point.Subtract(Cursor.Position, new Size(MouseBeginPos));
            MouseEndPos.X  /= 2;
            MouseEndPos.Y  /= 2;
            MouseEndPos     = Point.Subtract(new Point(PAL_X(posWorldViewport), PAL_Y(posWorldViewport)), new Size(MouseEndPos));

            MouseEndPos.X   = Math.Min(PAL_X(dwMaxMapPos), MouseEndPos.X);
            MouseEndPos.X   = Math.Max(PAL_X(Pal_Map.dwMinMapPos), MouseEndPos.X);

            MouseEndPos.Y   = Math.Min(PAL_Y(dwMaxMapPos), MouseEndPos.Y);
            MouseEndPos.Y   = Math.Max(PAL_Y(Pal_Map.dwMinMapPos), MouseEndPos.Y);

            posWorldViewport = PAL_XY((WORD)MouseEndPos.X, (WORD)MouseEndPos.Y);

            ScrollTD_ScrollBoxT_ActualBoxR_VScrollBar.Value = (INT)(ScrollTD_ScrollBoxT_ActualBoxR_VScrollBar.Maximum * ((double)PAL_Y(posWorldViewport) / PAL_Y(dwMaxMapPos)));
            ScrollLR_ScrollBoxD_ActualBoxR_HScrollBar.Value = (INT)(ScrollLR_ScrollBoxD_ActualBoxR_HScrollBar.Maximum * ((double)PAL_X(posWorldViewport) / PAL_X(dwMaxMapPos)));

            rectWorldViewport.x          = PAL_X(posWorldViewport);
            rectWorldViewport.y          = PAL_Y(posWorldViewport);
            PAL_DrawMapToSurface(sfWorldViewport, rectWorldViewport, WorldViewport_PictureBox, posWorldViewport, pdmDisplayMode);
        }

        private void ScrollTD_ScrollBoxT_ActualBoxR_VScrollBar_ValueChanged(object sender, EventArgs e)
        {
            double percentage;

            if (fIsMovingMap) return;

            percentage          = (double)ScrollTD_ScrollBoxT_ActualBoxR_VScrollBar.Value / ScrollTD_ScrollBoxT_ActualBoxR_VScrollBar.Maximum;

            posWorldViewport    = PAL_XY(PAL_X(posWorldViewport), (WORD)((Pal_Map.wMapHeight - sfWorldViewport.h) * percentage));

            rectWorldViewport.y = PAL_Y(posWorldViewport);
            PAL_DrawMapToSurface(sfWorldViewport, rectWorldViewport, WorldViewport_PictureBox, posWorldViewport, pdmDisplayMode);
        }

        private void ScrollLR_ScrollBoxD_ActualBoxR_HScrollBar_ValueChanged(object sender, EventArgs e)
        {
            double percentage;

            if (fIsMovingMap) return;

            percentage  = (double)ScrollLR_ScrollBoxD_ActualBoxR_HScrollBar.Value / ScrollLR_ScrollBoxD_ActualBoxR_HScrollBar.Maximum;

            posWorldViewport   = PAL_XY((WORD)((Pal_Map.wMapWidth - sfWorldViewport.w) * percentage), PAL_Y(posWorldViewport));

            rectWorldViewport.x      = PAL_X(posWorldViewport);
            PAL_DrawMapToSurface(sfWorldViewport, rectWorldViewport, WorldViewport_PictureBox, posWorldViewport, pdmDisplayMode);
        }

        private void Main_TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            SWORD_Resize(null, null);
        }
    }
}
