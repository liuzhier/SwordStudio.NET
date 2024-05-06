using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Xml.Linq;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

using PalGlobal;
using PalCfg;
using PalVideo;
using PalMap;

using static PalGlobal.Pal_Global;
using static PalGlobal.Pal_File;
using static PalCommon.Pal_Common;
using static PalUtil.Pal_Util;
using static PalConfig.Pal_Config;
using static PalCfg.Pal_Cfg;
using static PalMap.Pal_Map;
using static PalVideo.Pal_Video;

namespace SwordStudio.NET
{
    public partial class Form_SceneSelect : Form
    {
        private static      INT         nEvent, nScene, iSizeOfScene;
        private static      Pal_File    pfFile_Map = null, pfFile_SSS = null;
        private static      Surface     sfMapPreview        = null;
        private static      BYTE[]      Map_Tmp             = null;
        private static      BOOL        fIsLoadingCompleted = FALSE;
        private static      Rect        rect                = new Rect();
        private readonly    PAL_POS     dwMaxMapPos;

        private static      BOOL        fIsMovingMap        = FALSE;
        private static      Point       MouseBeginPos;
        private static      Point       MouseEndPos;

        public              INT         _iThisScene         = -1;
        public  static      INT         _iStartEvent        = -1;
        public  static      INT         _iEndEvent          = -1;
        public              BOOL        _fIsEnter           = FALSE;
        public              PAL_POS     _dwMapPos           = PAL_XY(0, 0);
        public              dynamic[,]  _AllSceneData;

        private Form_SceneSelect() { }

        public Form_SceneSelect(
            Form        Father_Form
        )
        {
            InitializeComponent();

            this.Owner                  = Father_Form;

            MapPreview_PictureBox.Image = new Bitmap(MapPreview_PictureBox.Width, MapPreview_PictureBox.Height);
            sfMapPreview                = new Surface(MapPreview_PictureBox.Width, MapPreview_PictureBox.Height);

            dwMaxMapPos                 = PAL_XY((WORD)(Pal_Map.wMapHeight - sfMapPreview.w), (WORD)(Pal_Map.wMapHeight - sfMapPreview.h));

            rect.x                      = PAL_X(_dwMapPos);
            rect.y                      = PAL_Y(_dwMapPos);
            rect.w                      = MapPreview_PictureBox.Width;
            rect.h                      = MapPreview_PictureBox.Height;
        }

        private void Form_SceneSelect_Resize(object sender, EventArgs e)
        {
            if (MapPreview_PictureBox == null || MapPreview_PictureBox.Image == null) return;
            if (MapPreview_PictureBox.Image.Width != MapPreview_PictureBox.Width || MapPreview_PictureBox.Image.Height != MapPreview_PictureBox.Height)
            {
                MapPreview_PictureBox.Image = new Bitmap(MapPreview_PictureBox.Width, MapPreview_PictureBox.Height);

                rect.w = MapPreview_PictureBox.Width;
                rect.h = MapPreview_PictureBox.Height;
            }

            if (_iThisScene != -1) PAL_DrawMapToSurface(sfMapPreview, rect, MapPreview_PictureBox, _dwMapPos);
        }

        private void MapNameList_SceneSelectBoxT_ListView_Resize(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView listView = sender as System.Windows.Forms.ListView;

            if (listView != null && listView.Columns.Count > 0) listView.Columns[0].Width = listView.Width;
        }

        private void MapPreview_PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                fIsMovingMap    = true;

                MouseBeginPos   = Cursor.Position;
            }
        }

        private void MapPreview_PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) fIsMovingMap = false;
        }

        private void MapPreview_PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!fIsMovingMap) return;
            
            MouseEndPos     = Point.Subtract(Cursor.Position, new Size(MouseBeginPos));
            MouseEndPos.X  /= 2;
            MouseEndPos.Y  /= 2;
            MouseEndPos     = Point.Subtract(new Point(PAL_X(_dwMapPos), PAL_Y(_dwMapPos)), new Size(MouseEndPos));

            MouseEndPos.X   = Math.Min(PAL_X(dwMaxMapPos), MouseEndPos.X);
            MouseEndPos.X   = Math.Max(PAL_X(Pal_Map.dwMinMapPos), MouseEndPos.X);

            MouseEndPos.Y   = Math.Min(PAL_Y(dwMaxMapPos), MouseEndPos.Y);
            MouseEndPos.Y   = Math.Max(PAL_Y(Pal_Map.dwMinMapPos), MouseEndPos.Y);

            _dwMapPos       = PAL_XY((WORD)MouseEndPos.X, (WORD)MouseEndPos.Y);

            ScrollTD_ScrollBoxT_MainBoxL_VScrollBar.Value = (INT)(ScrollTD_ScrollBoxT_MainBoxL_VScrollBar.Maximum * ((double)PAL_Y(_dwMapPos) / PAL_Y(dwMaxMapPos)));
            ScrollLR_ScrollBoxD_MainBoxL_HScrollBar.Value = (INT)(ScrollLR_ScrollBoxD_MainBoxL_HScrollBar.Maximum * ((double)PAL_X(_dwMapPos) / PAL_X(dwMaxMapPos)));

            rect.x          = PAL_X(_dwMapPos);
            rect.y          = PAL_Y(_dwMapPos);
            PAL_DrawMapToSurface(sfMapPreview, rect, MapPreview_PictureBox, _dwMapPos);
        }

        private void ScrollTD_ScrollBoxT_MainBoxL_VScrollBar_ValueChanged(object sender, EventArgs e)
        {
            double percentage;

            if (fIsMovingMap) return;

            percentage  = (double)ScrollTD_ScrollBoxT_MainBoxL_VScrollBar.Value / ScrollTD_ScrollBoxT_MainBoxL_VScrollBar.Maximum;

            _dwMapPos   = PAL_XY(PAL_X(_dwMapPos), (WORD)((Pal_Map.wMapHeight - sfMapPreview.h) * percentage));

            rect.y      = PAL_Y(_dwMapPos);
            PAL_DrawMapToSurface(sfMapPreview, rect, MapPreview_PictureBox, _dwMapPos);
        }

        private void ScrollLR_ScrollBoxD_MainBoxL_HScrollBar_ValueChanged(object sender, EventArgs e)
        {
            double percentage;

            if (fIsMovingMap) return;

            percentage  = (double)ScrollLR_ScrollBoxD_MainBoxL_HScrollBar.Value / ScrollLR_ScrollBoxD_MainBoxL_HScrollBar.Maximum;

            _dwMapPos   = PAL_XY((WORD)((Pal_Map.wMapWidth - sfMapPreview.w) * percentage), PAL_Y(_dwMapPos));

            rect.x      = PAL_X(_dwMapPos);
            PAL_DrawMapToSurface(sfMapPreview, rect, MapPreview_PictureBox, _dwMapPos);
        }

        private void SubmitSelect_SceneSelectBoxD_UtilButton_Click(object sender, EventArgs e)
        {
            _fIsEnter = TRUE;
            this.Hide();
        }

        private void MapNameList_SceneSelectBoxT_ListView_ItemActivate(object sender, EventArgs e)
        {
            INT     i, j, k, iThisScene, iSceneIndex, iOffset = 0;
            BYTE[]  Data_Buf = null;
            LPSTR   lpszMapName;

            System.Windows.Forms.ListView _ListView = sender as System.Windows.Forms.ListView;
            ListViewItem                  ThisItem  = _ListView.FocusedItem;

            if (ThisItem != null)
            {
                foreach (ListViewItem brother in _ListView.Items) brother.BackColor = _ListView.BackColor;

                ThisItem.BackColor  = Color.DeepPink;

                //
                // Get the number of the currently selected scene
                //
                iThisScene          = _ListView.Items.IndexOf(ThisItem);
                Pal_Map.iSceneNum   = iThisScene;

                if (_iThisScene == -1 || _iThisScene != iThisScene)
                {
                    _iThisScene     = iThisScene;

                    //
                    // Clean up scene Wave
                    //
                    Pal_Global.wScreenWave      = 0;
                    Pal_Global.sWaveProgression = 0;

                    //
                    // Get the size of scenes data
                    //
                    iSceneIndex = Pal_Cfg_GetCfgNodeItemIndex(lpszScene, lpszEventObjectIndex);

                    //
                    // Get MapID, the number of events for the currently selected scene
                    //
                    Pal_Map.iMapNum = _AllSceneData[_iThisScene,        Pal_Cfg_GetCfgNodeItemIndex(lpszScene, lpszMapID)];
                    _iStartEvent    = _AllSceneData[_iThisScene,        iSceneIndex];
                    _iEndEvent      = _AllSceneData[_iThisScene + 1,    iSceneIndex];

                    //
                    // Display the information of the currently selected scene
                    //
                    lpszMapName = $"当前: [0x{_iThisScene:X4}] {_iThisScene:D5} 事件数: {_iEndEvent - _iStartEvent}\n{Pal_Cfg_GetCfgNodeItem(lpszSceneDesc, $"0x{_iThisScene + 1:X4}").lpszTitle}";
                    ThisSceneName_MapNameListT_SceneSelectBoxT_MainBoxR_SplitContainer.Text = lpszMapName;

                    //
                    // Initialize map tiles
                    //
                    {
                        Pal_Map_Tile   pmtTitle;

                        INT iTileThird      = Pal_Map.Tiles.GetLength(0);
                        INT iTileSsecond    = Pal_Map.Tiles.GetLength(1);
                        INT iTileFirst      = Pal_Map.Tiles.GetLength(2);

                        //
                        // Get map tiles data
                        //
                        Map_Tmp = Pal_File_GetFile(lpszGameMap).bufFile;
                        PAL_MKFDecompressChunk(ref Data_Buf, Pal_Map.iMapNum, Map_Tmp);

                        for (i = 0; i < iTileThird; i++)
                        {
                            for (j = 0; j < iTileSsecond; j++)
                            {
                                for (k = 0; k < iTileFirst; k++)
                                {
                                    pmtTitle                = new Pal_Map_Tile();

                                    pmtTitle.fIsNoPassBlock = (Data_Buf[iOffset + 1] & 0x20) != 0;
                                    pmtTitle.LowTile_Num    = (SHORT)(Data_Buf[iOffset++] | (((Data_Buf[iOffset] & 0x10) >> 4) << 8));
                                    pmtTitle.LowTile_Layer  = (BYTE)(Data_Buf[iOffset++] & 0xF);
                                    pmtTitle.HighTile_Num   = (SHORT)((Data_Buf[iOffset++] | (((Data_Buf[iOffset] & 0x10) >> 4) << 8)) - 1);
                                    pmtTitle.HighTile_Layer = (BYTE)(Data_Buf[iOffset++] & 0xF);

                                    Pal_Map.Tiles[i, j, k] = pmtTitle;
                                }
                            }
                        }

                        Data_Buf = null;
                    }

                    //
                    // Initialize map bitmaps
                    //
                    {
                        //
                        // Get map tiles data
                        //
                        Map_Tmp = Pal_File_GetFile(lpszGameMapTile).bufFile;
                        PAL_MKFReadChunk(ref Pal_Map.TileSprite, Pal_Map.iMapNum, Map_Tmp);
                    }

                    _dwMapPos   = Pal_Map.dwMinMapPos;
                    rect.x      = PAL_X(_dwMapPos);
                    rect.y      = PAL_Y(_dwMapPos);
                    PAL_DrawMapToSurface(sfMapPreview, rect, MapPreview_PictureBox, _dwMapPos);

                    ScrollTD_ScrollBoxT_MainBoxL_VScrollBar.Value   = 0;
                    ScrollLR_ScrollBoxD_MainBoxL_HScrollBar.Value   = 0;

                    //
                    // Allow clicking on the OK button
                    //
                    if (_iThisScene != -1)
                    {
                        SubmitSelect_SceneSelectBoxD_UtilButton.Enabled = TRUE;
                        MapPreview_PictureBox.Enabled                   = TRUE;
                        ScrollTD_ScrollBoxT_MainBoxL_VScrollBar.Enabled = TRUE;
                        ScrollLR_ScrollBoxD_MainBoxL_HScrollBar.Enabled = TRUE;
                    }
                }
                else
                {
                    //
                    // Simulate scene wave
                    //
                    if (Pal_Global.wScreenWave > 0)
                    {
                        Pal_Global.wScreenWave      = 0;
                        Pal_Global.sWaveProgression = 0;
                    }
                    else
                    {
                        Pal_Global.wScreenWave      = 6;
                        Pal_Global.sWaveProgression = 0;
                    }
                }
            }
        }

        private void Form_SceneSelect_Load(object sender, EventArgs e)
        {
            INT     i;
            LPSTR   lpszMapName;

            //
            // Initialize confirmation selection button
            //
            _fIsEnter       = FALSE;

            if (fIsLoadingCompleted) return;

            //
            // Get the map file node
            //
            pfFile_Map      = Pal_File_GetFile(lpszGameMap);

            //
            // Get file indexes for scenes
            //
            i               = Pal_Cfg_GetCfgNodeItemIndex(lpszMainData, lpszScene);

            //
            // Get all scene data
            //
            _AllSceneData   = Pal_Global.poMainData[i].Data;

            //
            // Get the number of scenes
            //
            nScene          = _AllSceneData.GetLength(0);

            //
            // Set the table header of the map list
            //
            MapNameList_SceneSelectBoxT_ListView.Columns.Add("地图编号", MapNameList_SceneSelectBoxT_ListView.Width, HorizontalAlignment.Center);

            //
            // Add all map names to the map list
            //
            for (i = 1; i < nScene; i++)
            {
                lpszMapName = Pal_Cfg_GetCfgNodeItem(lpszSceneDesc, $"0x{i:X4}").lpszTitle;
                MapNameList_SceneSelectBoxT_ListView.Items.Add(new ListViewItem($"[0x{i:X4}] {i:D5}: {lpszMapName}"));
            }

            //
            // Enable loading completion flag
            //
            fIsLoadingCompleted = TRUE;
        }
    }
}
